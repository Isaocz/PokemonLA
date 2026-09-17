using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 第二阶段强技能2：天象时钟。
///
/// 1. 激光时针从12点方向开始顺时针旋转，并在旋转期间持续造成伤害。
/// 2. 时针每抵达一个刻度，弹幕以该刻度位置为圆心向外释放。
/// 3. 回到最终12点刻度时，限制圈立即开始消失。
/// 4. 地图中心随后连续释放多圈高密度减速弹幕，迫使玩家离开旧限制圈范围。
/// </summary>
public sealed class CelestialClock : MewBaseSkill
{
    public float FinalTrialSlotSeconds => Mathf.Ceil(skillStartup + rotationDuration +
        Mathf.Max(arenaFadeDuration, finalBurstWarningTime) + finalRingCount * finalRingInterval + skillEndup + 1f);
    public void ConfigureFinalTrial()
    {
        hourRingProjectileCount += 4;
        finalRingProjectileCount += 6;
    }
    [Header("星光弹幕")]
    [Tooltip("普通 StarLight，必须带 BarrageProjectile。")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float projectileRotationOffset;

    [Header("时钟旋转")]
    [Min(1f)]
    [SerializeField] private float rotationDuration = 7.2f;
    [Range(4, 24)]
    [SerializeField] private int hourCount = 12;
    [Range(4, 24)]
    [SerializeField] private int hourRingProjectileCount = 12;
    [Min(0f)]
    [SerializeField] private float hourRingStartSpeed = 5.4f;
    [SerializeField] private float hourRingAcceleration = 0.65f;
    [SerializeField] private float hourRingAngleShift = 7.5f;

    [Header("激光时针视觉")]
    [Tooltip("可选。Prefab 应以局部Y轴朝上；脚本会围绕Z轴旋转。留空时运行时生成 LineRenderer。")]
    [SerializeField] private GameObject laserHandPrefab;
    [SerializeField] private float laserPrefabRotationOffset;
    [Range(0.2f, 1.2f)]
    [SerializeField] private float handLengthRatio = 0.96f;
    [Min(0.01f)]
    [SerializeField] private float fallbackHandWidth = 0.32f;
    [SerializeField] private Color fallbackHandColor =
        new Color(0.95f, 0.45f, 1f, 0.96f);
    [SerializeField] private int fallbackSortingOrder = 95;

    [Header("激光时针伤害")]
    [Tooltip("独立于视觉宽度的实际命中宽度。")]
    [Min(0.05f)]
    [SerializeField] private float laserHitWidth = 0.68f;
    [Min(0f)]
    [SerializeField] private float laserCenterBlindRadius = 0.25f;
    [Min(1f)]
    [SerializeField] private float laserDamage = 42f;
    [Min(0.05f)]
    [SerializeField] private float laserDamageInterval = 0.32f;
    [Min(0f)]
    [SerializeField] private float laserKnockback = 1.8f;
    [SerializeField] private LayerMask laserHitLayers = ~0;

    [Header("时点视觉")]
    [Tooltip("可选。会在限制圈内侧生成时点标记。")]
    [SerializeField] private GameObject hourMarkerPrefab;
    [Tooltip("可选。时针到达时点时生成一次短暂特效。")]
    [SerializeField] private GameObject tickEffectPrefab;
    [Range(0.2f, 1f)]
    [SerializeField] private float markerRadiusRatio = 0.84f;
    [Min(0f)]
    [SerializeField] private float tickEffectLifetime = 0.7f;

    [Header("最终驱离弹幕")]
    [Tooltip("最终12点抵达后，限制圈淡出的时间。建议保持较短。")]
    [Min(0f)]
    [SerializeField] private float arenaFadeDuration = 0.32f;
    [Min(0f)]
    [SerializeField] private float finalBurstWarningTime = 0.12f;
    [Range(3, 10)]
    [SerializeField] private int finalRingCount = 7;
    [Range(12, 48)]
    [SerializeField] private int finalRingProjectileCount = 36;
    [Min(0f)]
    [SerializeField] private float finalRingInterval = 0.16f;
    [Min(0f)]
    [SerializeField] private float finalRingStartSpeed = 10.8f;
    [Min(0f)]
    [SerializeField] private float finalRingSpeedStep = 0.75f;
    [Tooltip("应为负值。后发弹幕速度略高，会逐渐压成高密度扩散波。")]
    [SerializeField] private float finalRingAcceleration = -0.9f;
    [Min(0.1f)]
    [SerializeField] private float finalRingMinimumSpeed = 2.5f;
    [SerializeField] private float finalRingAngleShift = 5f;

    private readonly List<GameObject> clockVisuals = new List<GameObject>();
    private readonly List<Collider2D> laserHits = new List<Collider2D>(128);
    private readonly Dictionary<int, float> nextLaserHitTimes =
        new Dictionary<int, float>();

    private GameObject handObject;
    private LineRenderer fallbackHand;
    private Material fallbackMaterial;
    private bool contextReady;

    private void Reset()
    {
        skillName = "天象时钟";
        skillStartup = 0.45f;
        skillEndup = 0.55f;
        existTime = 0f;
        repeat = 1;
        repeatTime = 0f;
    }

    protected override void OnContextInitialized(MewSkillContext context)
    {
        contextReady = empty != null && context.ArenaRadius > 0.01f;

        if (!contextReady)
        {
            Debug.LogError(
                name + "：天象时钟必须由 Mew 以带 ArenaCenter/ArenaRadius 的上下文初始化。",
                this);
        }
    }

    public override IEnumerator Startup()
    {
        if (!ValidateConfiguration())
        {
            yield break;
        }

        nextLaserHitTimes.Clear();
        CreateClockVisuals();
        UpdateHand(90f);

        // 前摇期间只显示时针作为预警，不造成伤害。
        if (skillStartup > 0f)
        {
            yield return new WaitForSeconds(skillStartup);
        }
    }

    public override IEnumerator CoreLogic()
    {
        if (!ValidateConfiguration())
        {
            yield break;
        }

        float elapsed = 0f;
        int nextHour = 1;

        // 初始12点是预警起点；真正的第12次刻度触发发生在旋转一圈回到12点时。
        while (elapsed < rotationDuration)
        {
            elapsed += Time.deltaTime;
            float progress = Mathf.Clamp01(elapsed / rotationDuration);
            float angle = 90f - progress * 360f;

            UpdateHand(angle);
            DamagePlayersOnHand(angle);
            if (empty is Mew endingBoss && endingBoss.IsEnding) yield break;

            while (nextHour <= hourCount &&
                   progress >= nextHour / (float)hourCount)
            {
                FireHour(nextHour);
                nextHour++;
            }

            yield return null;
        }

        UpdateHand(-270f);
        DamagePlayersOnHand(-270f);
        if (empty is Mew finishedBoss && finishedBoss.IsEnding) yield break;
        DestroyClockVisuals();

        // 最终12点：先解除外圈，再从中心释放驱离弹幕。
        ReleaseArenaBoundary();

        float finalBurstDelay = Mathf.Max(
            Mathf.Max(0f, arenaFadeDuration),
            Mathf.Max(0f, finalBurstWarningTime));
        if (finalBurstDelay > 0f)
        {
            yield return new WaitForSeconds(finalBurstDelay);
        }

        for (int ringIndex = 0; ringIndex < finalRingCount; ringIndex++)
        {
            float angleOffset = ringIndex * finalRingAngleShift;
            float speed = finalRingStartSpeed + ringIndex * finalRingSpeedStep;

            FireRing(
                ArenaCenter,
                finalRingProjectileCount,
                angleOffset,
                speed,
                finalRingAcceleration,
                true);

            if (ringIndex < finalRingCount - 1 && finalRingInterval > 0f)
            {
                yield return new WaitForSeconds(finalRingInterval);
            }
        }
    }

    protected override void OnSkillFinished(MewSkillFinishReason reason)
    {
        DestroyClockVisuals();
        nextLaserHitTimes.Clear();
    }

    private bool ValidateConfiguration()
    {
        if (!contextReady || empty == null)
        {
            return false;
        }

        if (projectilePrefab == null)
        {
            Debug.LogError(
                name + "：CelestialClock 没有设置 projectilePrefab。",
                this);
            return false;
        }

        rotationDuration = Mathf.Max(1f, rotationDuration);
        hourCount = Mathf.Max(4, hourCount);
        hourRingProjectileCount = Mathf.Max(4, hourRingProjectileCount);
        laserHitWidth = Mathf.Max(0.05f, laserHitWidth);
        laserDamageInterval = Mathf.Max(0.05f, laserDamageInterval);
        finalRingCount = Mathf.Max(3, finalRingCount);
        finalRingProjectileCount = Mathf.Max(12, finalRingProjectileCount);
        finalRingMinimumSpeed = Mathf.Max(0.1f, finalRingMinimumSpeed);
        return true;
    }

    private void FireHour(int hourIndex)
    {
        float clockAngle = 90f - hourIndex * (360f / hourCount);
        Vector2 tickPosition = GetTickPosition(clockAngle);

        SpawnTickEffect(clockAngle, tickPosition);

        // 每个刻度都以刻度本身为圆心释放弹幕，而不是从地图中心释放。
        FireRing(
            tickPosition,
            hourRingProjectileCount,
            hourIndex * hourRingAngleShift,
            hourRingStartSpeed,
            hourRingAcceleration,
            false);
    }

    private void FireRing(
        Vector2 origin,
        int projectileCount,
        float angleOffset,
        float speed,
        float acceleration,
        bool preventReverse)
    {
        projectileCount = Mathf.Max(1, projectileCount);
        float step = 360f / projectileCount;

        for (int i = 0; i < projectileCount; i++)
        {
            float angle = angleOffset + i * step;
            Vector2 direction =
                Quaternion.Euler(0f, 0f, angle) * Vector2.right;

            SpawnProjectile(
                origin,
                direction,
                speed,
                acceleration,
                preventReverse);
        }
    }

    private void SpawnProjectile(
        Vector3 origin,
        Vector2 direction,
        float speed,
        float acceleration,
        bool preventReverse)
    {
        float angle =
            Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg +
            projectileRotationOffset;

        GameObject projectileObject = MewStarPool.Spawn(
            projectilePrefab,
            origin,
            Quaternion.Euler(0f, 0f, angle), empty);

        BarrageProjectile projectile =
            projectileObject.GetComponent<BarrageProjectile>();

        if (projectile == null)
        {
            Debug.LogError(
                projectilePrefab.name +
                " 缺少 BarrageProjectile；天象时钟无法初始化星光。",
                projectileObject);
            Destroy(projectileObject);
            return;
        }

        projectile.empty = empty;
        projectile.SetBehavior(BarrageProjectile.projectileBehavior.Straight);
        projectile.SetDirection(direction);
        projectile.SetSpeed(speed, acceleration);

        if (preventReverse && acceleration < -0.001f)
        {
            float safeLifetime =
                (Mathf.Max(speed, finalRingMinimumSpeed) -
                 finalRingMinimumSpeed) /
                -acceleration;

            safeLifetime = Mathf.Max(0.55f, safeLifetime);
            projectile.ExistTime = Mathf.Min(
                Mathf.Max(0.55f, projectile.ExistTime),
                safeLifetime);
        }
    }

    private void DamagePlayersOnHand(float clockAngle)
    {
        if (empty == null || laserDamage <= 0f)
        {
            return;
        }

        float fullLength = ArenaRadius * handLengthRatio;
        float damageLength = Mathf.Max(
            0.05f,
            fullLength - Mathf.Max(0f, laserCenterBlindRadius));

        Vector2 direction =
            Quaternion.Euler(0f, 0f, clockAngle) * Vector2.right;
        Vector2 start = ArenaCenter +
            direction * Mathf.Max(0f, laserCenterBlindRadius);
        Vector2 midpoint = start + direction * (damageLength * 0.5f);

        ContactFilter2D filter = new ContactFilter2D { useLayerMask = true, layerMask = laserHitLayers, useTriggers = true };
        Physics2D.OverlapBox(
            midpoint,
            new Vector2(damageLength, laserHitWidth),
            clockAngle,
            filter, laserHits);

        for (int i = 0; i < laserHits.Count; i++)
        {
            Collider2D hit = laserHits[i];
            if (hit == null)
            {
                continue;
            }

            PlayerControler target = hit.GetComponent<PlayerControler>();
            if (target == null)
            {
                target = hit.GetComponentInParent<PlayerControler>();
            }

            if (target == null)
            {
                continue;
            }

            int id = target.GetInstanceID();
            float nextAllowedTime;
            if (nextLaserHitTimes.TryGetValue(id, out nextAllowedTime) &&
                Time.time < nextAllowedTime)
            {
                continue;
            }

            nextLaserHitTimes[id] = Time.time + laserDamageInterval;

            Pokemon.PokemonHpChange(
                empty.gameObject,
                target.gameObject,
                0,
                laserDamage,
                0,
                PokemonType.TypeEnum.Psychic);

            if (empty is Mew boss && boss.IsEnding) return;
            if (laserKnockback > 0f)
            {
                Vector2 knockDirection =
                    (Vector2)target.transform.position - ArenaCenter;
                if (knockDirection.sqrMagnitude < 0.0001f)
                {
                    knockDirection = direction;
                }

                target.KnockOutPoint = laserKnockback;
                target.KnockOutDirection = knockDirection.normalized;
            }
        }
    }

    private void ReleaseArenaBoundary()
    {
        if (Phase == MewSkillPhase.Phase3) return;
        Mew mew = empty as Mew;
        if (mew != null)
        {
            mew.ReleasePhaseTwoArenaBoundary(arenaFadeDuration);
            return;
        }

        if (ArenaTransform != null)
        {
            MewArenaBoundary boundary =
                ArenaTransform.GetComponent<MewArenaBoundary>();
            if (boundary != null)
            {
                boundary.Deactivate(arenaFadeDuration, true);
            }
        }
    }

    private void CreateClockVisuals()
    {
        DestroyClockVisuals();

        var dial = MewSigilVisual.Create(null, ArenaCenter, ArenaRadius * markerRadiusRatio, MewSigilVisual.Style.Clock, fallbackSortingOrder - 8);
        clockVisuals.Add(dial.gameObject);
        handObject = new GameObject("Celestial Clock Hand");
        handObject.transform.position = ArenaCenter;
        handObject.AddComponent<MewClockHand>().Build(ArenaRadius * handLengthRatio,
            laserHitWidth, fallbackHandColor, fallbackSortingOrder);
        clockVisuals.Add(handObject);
        if (hourMarkerPrefab == null)
        {
            return;
        }

        for (int i = 0; i < hourCount; i++)
        {
            float angle = 90f - i * (360f / hourCount);
            Vector3 position = GetTickPosition(angle);

            GameObject marker = Instantiate(
                hourMarkerPrefab,
                position,
                Quaternion.Euler(0f, 0f, angle - 90f));
            clockVisuals.Add(marker);
        }
    }

    private void UpdateHand(float clockAngle)
    {
        if (handObject == null)
        {
            return;
        }

        float handLength = ArenaRadius * handLengthRatio;
        Vector2 direction =
            Quaternion.Euler(0f, 0f, clockAngle) * Vector2.right;

        if (fallbackHand != null)
        {
            fallbackHand.SetPosition(0, ArenaCenter);
            fallbackHand.SetPosition(1, ArenaCenter + direction * handLength);
            return;
        }

        handObject.transform.position = ArenaCenter;
        handObject.transform.rotation = Quaternion.Euler(
            0f,
            0f,
            clockAngle - 90f);
    }

    private Vector2 GetTickPosition(float clockAngle)
    {
        float radius = ArenaRadius * markerRadiusRatio;
        Vector2 direction =
            Quaternion.Euler(0f, 0f, clockAngle) * Vector2.right;
        return ArenaCenter + direction * radius;
    }

    private void SpawnTickEffect(float clockAngle, Vector2 tickPosition)
    {
        if (tickEffectPrefab == null)
        {
            return;
        }

        GameObject effect = Instantiate(
            tickEffectPrefab,
            tickPosition,
            Quaternion.Euler(0f, 0f, clockAngle - 90f));

        if (tickEffectLifetime > 0f)
        {
            Destroy(effect, tickEffectLifetime);
        }
    }

    private void DestroyClockVisuals()
    {
        for (int i = clockVisuals.Count - 1; i >= 0; i--)
        {
            GameObject target = clockVisuals[i];
            if (target != null)
            {
                Destroy(target);
            }
        }

        clockVisuals.Clear();
        handObject = null;
        fallbackHand = null;

        if (fallbackMaterial != null)
        {
            Destroy(fallbackMaterial);
            fallbackMaterial = null;
        }
    }
}
