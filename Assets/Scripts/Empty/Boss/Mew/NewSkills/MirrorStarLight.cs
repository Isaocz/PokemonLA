using System.Collections;
using UnityEngine;

/// <summary>
/// 第二阶段强技能1：镜像星光。
///
/// 规则：
/// 1. Boss 与无伤害分身始终关于固定限制圈中心对称。
/// 2. 两者在限制圈外沿同向环绕。
/// 3. 持续向中心发射普通 StarLight；间歇释放朝中心展开的扇形散射。
/// 4. 弹幕只依赖 BarrageProjectile，不给 StarLight 增加新的移动脚本。
/// </summary>
public sealed class MirrorStarLight : MewBaseSkill
{
    public float FinalTrialSlotSeconds => Mathf.Ceil(skillStartup + orbitDuration + skillEndup + 2.6f);
    public GameObject ProjectileTemplate => projectilePrefab;
    public void ConfigureFinalTrial()
    {
        continuousFireInterval = Mathf.Max(0.06f, continuousFireInterval * 0.8f);
        burstInterval *= 0.85f;
        burstProjectileCount += 2;
    }
    [Header("视觉分身")]
    [Tooltip("推荐使用只包含梦幻视觉、Animator 的 Prefab，不要挂 Mew、Empty、Collider2D。留空时同步 SpriteParent/MewIdle_0 的梦幻动画。")]
    [SerializeField] private GameObject mirrorClonePrefab;
    [SerializeField] private Color cloneTint =
        new Color(0.78f, 0.70f, 1f, 0.82f);

    [Header("星光弹幕")]
    [Tooltip("普通 StarLight，必须带 BarrageProjectile。")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float projectileRotationOffset = 0f;
    [Min(0.01f)]
    [SerializeField] private float continuousFireInterval = 0.14f;
    [Min(0f)]
    [SerializeField] private float continuousShotSpeed = 8.5f;
    [SerializeField] private float continuousShotAcceleration = 0.35f;

    [Header("向心散射")]
    [Min(0.1f)]
    [SerializeField] private float burstInterval = 1.25f;
    [Min(2)]
    [SerializeField] private int burstProjectileCount = 7;
    [Range(1f, 120f)]
    [SerializeField] private float burstSpreadAngle = 54f;
    [Min(0f)]
    [SerializeField] private float burstShotSpeed = 6.4f;
    [SerializeField] private float burstShotAcceleration = 0.8f;

    [Header("环绕")]
    [Tooltip("Boss 与分身位于限制圈半径之外。")]
    [Min(0f)]
    [SerializeField] private float orbitRadiusOffset = 1.35f;
    [Min(0.1f)]
    [SerializeField] private float orbitDuration = 9.5f;
    [Min(0.1f)]
    [SerializeField] private float orbitRevolutions = 2.4f;
    [SerializeField] private bool clockwise = true;
    [Tooltip("结束后把 Boss 放回限制圈内部，避免下一技能开始前停留在场外。")]
    [Range(0.1f, 0.9f)]
    [SerializeField] private float casterReturnRadiusRatio = 0.52f;

    private SpriteRenderer mirrorSource, mirrorVisual;
    private GameObject mirrorClone;

    private void LateUpdate()
    {
        if (empty == null || mirrorClone == null || mirrorSource == null || mirrorVisual == null) return;
        mirrorVisual.enabled = mirrorSource.enabled && mirrorSource.gameObject.activeInHierarchy;
        mirrorVisual.sprite = mirrorSource.sprite;
        mirrorVisual.flipX = mirrorSource.flipX;
        mirrorVisual.flipY = mirrorSource.flipY;
        mirrorVisual.color = cloneTint;
        mirrorVisual.transform.localPosition = empty.transform.InverseTransformPoint(mirrorSource.transform.position);
        mirrorVisual.transform.localRotation = Quaternion.Inverse(empty.transform.rotation) * mirrorSource.transform.rotation;
        Vector3 scale = mirrorSource.transform.lossyScale;
        Vector3 rootScale = mirrorClone.transform.lossyScale;
        mirrorVisual.transform.localScale = new Vector3(scale.x / Mathf.Max(0.001f, rootScale.x), scale.y / Mathf.Max(0.001f, rootScale.y), 1f);
    }
    private float currentAngle;
    private float usedOrbitRadius;
    private Vector3 originalCasterPosition;
    private bool contextReady;

    protected override void OnContextInitialized(MewSkillContext context)
    {
        contextReady =
            empty != null &&
            context.ArenaRadius > 0.01f;

        if (!contextReady)
        {
            Debug.LogError(
                name +
                "：镜像星光必须由 Mew 以带 ArenaCenter/ArenaRadius 的上下文初始化。",
                this);
        }
    }

    public override IEnumerator Startup()
    {
        if (!ValidateConfiguration())
        {
            yield break;
        }

        originalCasterPosition = empty.transform.position;
        usedOrbitRadius = Mathf.Max(
            1f,
            ArenaRadius + Mathf.Max(0f, orbitRadiusOffset));

        Vector2 fromCenter =
            (Vector2)empty.transform.position - ArenaCenter;

        currentAngle = fromCenter.sqrMagnitude > 0.001f
            ? Mathf.Atan2(fromCenter.y, fromCenter.x) * Mathf.Rad2Deg
            : Random.Range(0f, 360f);

        Vector3 clonePosition =
            GetArenaPoint(currentAngle + 180f, usedOrbitRadius);

        if (empty is Mew enteringBoss)
            yield return enteringBoss.TeleportForSkill(GetArenaPoint(currentAngle, usedOrbitRadius));
        mirrorClone = CreateMirrorClone(clonePosition);
        var casterHalo = MewSigilVisual.Create(empty.transform, empty.transform.position, 1.65f, MewSigilVisual.Style.Mirror);
        RegisterTemporaryObject(casterHalo.gameObject);
        if (mirrorClone != null)
        {
            MewSigilVisual.Create(mirrorClone.transform, mirrorClone.transform.position, 1.65f, MewSigilVisual.Style.Mirror);
            RegisterTemporaryObject(mirrorClone);
        }

        PositionActors(currentAngle);

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
        float continuousTimer = 0f;
        float burstTimer = 0f;
        float directionSign = clockwise ? -1f : 1f;
        float degreesPerSecond =
            orbitRevolutions * 360f / Mathf.Max(0.1f, orbitDuration);

        while (elapsed < orbitDuration)
        {
            float dt = Time.deltaTime;
            elapsed += dt;
            continuousTimer += dt;
            burstTimer += dt;

            currentAngle += directionSign * degreesPerSecond * dt;
            PositionActors(currentAngle);

            while (continuousTimer >= continuousFireInterval)
            {
                continuousTimer -= continuousFireInterval;
                FireContinuousPair();
            }

            if (burstTimer >= burstInterval)
            {
                burstTimer -= burstInterval;
                FireBurstPair();
            }

            yield return null;
        }

        yield return ReturnCasterInsideArena();
        DestroyMirrorClone();
    }

    protected override void OnSkillFinished(MewSkillFinishReason reason)
    {
        CleanupTemporaryObjects();
        if (reason != MewSkillFinishReason.Completed && empty is Mew boss && !boss.IsEnding)
            boss.StartCoroutine(ReturnCasterInsideArena());
        DestroyMirrorClone();
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
                name + "：MirrorStarLight 没有设置 projectilePrefab。",
                this);
            return false;
        }

        continuousFireInterval =
            Mathf.Max(0.01f, continuousFireInterval);
        burstInterval = Mathf.Max(0.1f, burstInterval);
        burstProjectileCount = Mathf.Max(2, burstProjectileCount);
        orbitDuration = Mathf.Max(0.1f, orbitDuration);
        orbitRevolutions = Mathf.Max(0.1f, orbitRevolutions);
        return true;
    }

    private void PositionActors(float angle)
    {
        if (empty == null)
        {
            return;
        }

        Vector3 casterPosition =
            GetArenaPoint(angle, usedOrbitRadius);
        Vector3 clonePosition =
            GetArenaPoint(angle + 180f, usedOrbitRadius);

        empty.transform.position = new Vector3(
            casterPosition.x,
            casterPosition.y,
            empty.transform.position.z);

        Rigidbody2D casterBody =
            empty.GetComponent<Rigidbody2D>();
        if (casterBody != null)
        {
            casterBody.velocity = Vector2.zero;
            casterBody.angularVelocity = 0f;
        }

        if (mirrorClone != null)
        {
            mirrorClone.transform.position = new Vector3(
                clonePosition.x,
                clonePosition.y,
                mirrorClone.transform.position.z);
        }
    }

    private void FireContinuousPair()
    {
        Vector3 casterPosition = empty.transform.position;
        FireTowardCenter(
            casterPosition,
            continuousShotSpeed,
            continuousShotAcceleration);

        if (mirrorClone != null)
        {
            FireTowardCenter(
                mirrorClone.transform.position,
                continuousShotSpeed,
                continuousShotAcceleration);
        }
    }

    private void FireBurstPair()
    {
        FireFanTowardCenter(empty.transform.position);

        if (mirrorClone != null)
        {
            FireFanTowardCenter(mirrorClone.transform.position);
        }
    }

    private void FireFanTowardCenter(Vector3 origin)
    {
        Vector2 centerDirection =
            ArenaCenter - (Vector2)origin;

        if (centerDirection.sqrMagnitude < 0.0001f)
        {
            centerDirection = Vector2.right;
        }

        float centerAngle =
            Mathf.Atan2(centerDirection.y, centerDirection.x) *
            Mathf.Rad2Deg;

        float step = burstProjectileCount <= 1
            ? 0f
            : burstSpreadAngle / (burstProjectileCount - 1);

        float startAngle = centerAngle - burstSpreadAngle * 0.5f;

        for (int i = 0; i < burstProjectileCount; i++)
        {
            float angle = startAngle + step * i;
            Vector2 direction =
                Quaternion.Euler(0f, 0f, angle) * Vector2.right;

            SpawnProjectile(
                origin,
                direction,
                burstShotSpeed,
                burstShotAcceleration);
        }
    }

    private void FireTowardCenter(
        Vector3 origin,
        float speed,
        float acceleration)
    {
        Vector2 direction =
            ArenaCenter - (Vector2)origin;

        if (direction.sqrMagnitude < 0.0001f)
        {
            direction = Vector2.right;
        }

        SpawnProjectile(
            origin,
            direction.normalized,
            speed,
            acceleration);
    }

    private void SpawnProjectile(
        Vector3 origin,
        Vector2 direction,
        float speed,
        float acceleration)
    {
        float angle =
            Mathf.Atan2(direction.y, direction.x) *
            Mathf.Rad2Deg +
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
                " 缺少 BarrageProjectile；镜像星光不会给弹幕额外挂移动脚本。",
                projectileObject);
            Destroy(projectileObject);
            return;
        }

        projectile.empty = empty;
        projectile.SetBehavior(
            BarrageProjectile.projectileBehavior.Straight);
        projectile.SetDirection(direction);
        projectile.SetSpeed(speed, acceleration);
    }

    private GameObject CreateMirrorClone(Vector3 position)
    {
        GameObject clone;

        if (mirrorClonePrefab != null)
        {
            clone = Instantiate(
                mirrorClonePrefab,
                position,
                empty.transform.rotation);

            DisableCloneGameplayComponents(clone);
            TintClone(clone);
            clone.name = "Mew Mirror Clone";
            return clone;
        }

        clone = new GameObject("Mew Animated Psychic Mirror");
        clone.transform.position = position;
        clone.transform.rotation = empty.transform.rotation;
        clone.transform.localScale = empty.transform.lossyScale;

        // The first renderer under Mew is a shadow, not the animated body.
        Transform body = empty.transform.Find("SpriteParent/MewIdle_0");
        SpriteRenderer source = body != null ? body.GetComponent<SpriteRenderer>() : null;

        if (source == null)
        {
            Debug.LogWarning(
                name +
                "：没有 mirrorClonePrefab，也没有找到梦幻本体 SpriteParent/MewIdle_0。",
                this);
            return clone;
        }

        var visualObject = new GameObject("Animated Mew silhouette");
        visualObject.transform.SetParent(clone.transform, false);
        SpriteRenderer renderer = visualObject.AddComponent<SpriteRenderer>();
        mirrorSource = source;
        mirrorVisual = renderer;
        renderer.sprite = source.sprite;
        renderer.sharedMaterial = source.sharedMaterial;
        renderer.sortingLayerID = source.sortingLayerID;
        renderer.sortingOrder = source.sortingOrder - 1;
        renderer.flipX = source.flipX;
        renderer.flipY = source.flipY;
        renderer.color = MultiplyColor(source.color, cloneTint);
        return clone;
    }

    private static void DisableCloneGameplayComponents(
        GameObject clone)
    {
        Collider2D[] colliders =
            clone.GetComponentsInChildren<Collider2D>(true);
        for (int i = 0; i < colliders.Length; i++)
        {
            colliders[i].enabled = false;
        }

        Rigidbody2D[] bodies =
            clone.GetComponentsInChildren<Rigidbody2D>(true);
        for (int i = 0; i < bodies.Length; i++)
        {
            bodies[i].velocity = Vector2.zero;
            bodies[i].angularVelocity = 0f;
            bodies[i].simulated = false;
        }

        Mew[] mewComponents =
            clone.GetComponentsInChildren<Mew>(true);
        for (int i = 0; i < mewComponents.Length; i++)
        {
            mewComponents[i].enabled = false;
        }
    }

    private void TintClone(GameObject clone)
    {
        SpriteRenderer[] renderers =
            clone.GetComponentsInChildren<SpriteRenderer>(true);

        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].color =
                MultiplyColor(renderers[i].color, cloneTint);
        }
    }

    private static Color MultiplyColor(Color a, Color b)
    {
        return new Color(
            a.r * b.r,
            a.g * b.g,
            a.b * b.b,
            a.a * b.a);
    }

    private IEnumerator ReturnCasterInsideArena()
    {
        if (empty == null || ArenaRadius <= 0.01f)
        {
            yield break;
        }

        Vector2 direction =
            (Vector2)empty.transform.position - ArenaCenter;

        if (direction.sqrMagnitude < 0.0001f)
        {
            direction =
                (Vector2)originalCasterPosition - ArenaCenter;
        }

        if (direction.sqrMagnitude < 0.0001f)
        {
            direction = Vector2.up;
        }

        Vector2 safePosition =
            ArenaCenter +
            direction.normalized *
            (ArenaRadius * casterReturnRadiusRatio);

        if (empty is Mew boss && !boss.IsEnding)
            yield return boss.ReturnFromSkill(new Vector3(safePosition.x, safePosition.y, empty.transform.position.z));

        Rigidbody2D body =
            empty.GetComponent<Rigidbody2D>();
        if (body != null)
        {
            body.velocity = Vector2.zero;
            body.angularVelocity = 0f;
        }
    }

    private void DestroyMirrorClone()
    {
        if (mirrorClone == null)
        {
            return;
        }

        UnregisterTemporaryObject(mirrorClone);
        Destroy(mirrorClone);
        mirrorClone = null;
    }
}
