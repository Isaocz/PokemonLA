using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 第二阶段强技能3：拟态群星。
///
/// 1. 梦幻在外侧限制圈之外绕场移动，并把星光逐颗发射到组阵位置。
/// 2. 星光组成一大一小两个六角星，密度更高，并以相反方向缓慢旋转。
/// 3. 顶点发射前两个六角星平滑减速至完全停止。
/// 4. 大小六角星的12颗顶点星光同时向中心加速穿过。
/// 5. 顶点星光离开中心圆后才显示中央安全圈；其余星光随后同步经过切点并散开。
/// </summary>
public sealed class MimicStars : MewBaseSkill
{
    public float FinalTrialSlotSeconds => Mathf.Ceil(skillStartup + skillEndup + 1.9f +
        12f * (Mathf.Min(10, largeSegmentsPerEdge + 2) + Mathf.Min(10, smallSegmentsPerEdge + 2)) * formationFireInterval * 0.8f +
        formationSettleTimeout + oppositeRotationHoldDuration + rotationStopDuration + vertexExitTimeout +
        safeCircleRevealLeadTime + tangentTravelDuration + postScatterDuration);
    public void ConfigureFinalTrial()
    {
        largeSegmentsPerEdge = Mathf.Min(10, largeSegmentsPerEdge + 2);
        smallSegmentsPerEdge = Mathf.Min(10, smallSegmentsPerEdge + 2);
        formationFireInterval *= 0.8f;
    }
    private sealed class FormationPoint
    {
        public Vector2 localPoint;
        public bool isVertex;
        public int ringIndex;
    }

    private sealed class FormationStar
    {
        public MimicStarBullet bullet;
        public Vector2 localPoint;
        public bool isVertex;
        public int ringIndex;
    }

    [Header("星光弹幕")]
    [Tooltip("普通 StarLight，必须带 BarrageProjectile、Rigidbody2D、Collider2D。")]
    [SerializeField] private GameObject projectilePrefab;
    [Tooltip("可选。留空时顶点也使用普通 StarLight。")]
    [SerializeField] private GameObject vertexProjectilePrefab;
    [SerializeField] private float projectileRotationOffset;

    [Header("梦幻绕圈组阵")]
    [Tooltip("梦幻在外限制圈半径之外额外偏移的距离。")]
    [Min(0.2f)]
    [SerializeField] private float casterOrbitOutsideOffset = 1.8f;
    [SerializeField] private float casterOrbitStartAngle = 90f;
    [SerializeField] private float casterOrbitSpeed = -82f;
    [Min(0.005f)]
    [SerializeField] private float formationFireInterval = 0.026f;
    [Min(0.05f)]
    [SerializeField] private float formationTravelDuration = 0.62f;
    [Min(0.1f)]
    [SerializeField] private float formationSettleTimeout = 1.25f;
    [Min(0.02f)]
    [SerializeField] private float patternFollowSmoothTime = 0.16f;
    [Min(0f)]
    [SerializeField] private float casterReturnDuration = 0.45f;

    [Header("大六角星")]
    [Min(3f)]
    [SerializeField] private float largeOuterRadius = 11.2f;
    [Min(1f)]
    [SerializeField] private float largeInnerRadius = 6.45f;
    [Range(3, 10)]
    [Tooltip("默认7：大六角星约84颗弹幕。")]
    [SerializeField] private int largeSegmentsPerEdge = 7;
    [SerializeField] private float largeStartRotation = 90f;
    [SerializeField] private float largeRotationSpeed = 12f;
    [SerializeField] private bool largeClockwiseTangents = true;

    [Header("小六角星")]
    [Min(2f)]
    [SerializeField] private float smallOuterRadius = 7.2f;
    [Min(0.8f)]
    [SerializeField] private float smallInnerRadius = 4.15f;
    [Range(3, 10)]
    [Tooltip("默认5：小六角星约60颗弹幕。")]
    [SerializeField] private int smallSegmentsPerEdge = 5;
    [SerializeField] private float smallStartRotation = 60f;
    [SerializeField] private float smallRotationSpeed = -16f;
    [SerializeField] private bool smallClockwiseTangents = false;

    [Header("图案旋转节奏")]
    [Min(0f)]
    [SerializeField] private float oppositeRotationHoldDuration = 1.65f;
    [Min(0.05f)]
    [SerializeField] private float rotationStopDuration = 1.15f;
    [Min(0f)]
    [SerializeField] private float arenaPadding = 1.15f;

    [Header("顶点星光")]
    [Min(0.5f)]
    [SerializeField] private float centerCircleRadius = 3.05f;
    [Min(0f)]
    [SerializeField] private float vertexStartSpeed = 1.35f;
    [Min(0f)]
    [SerializeField] private float vertexAcceleration = 8.8f;
    [Min(0.01f)]
    [SerializeField] private float vertexExitPadding = 0.28f;
    [Min(0.5f)]
    [SerializeField] private float vertexLifetime = 8.5f;
    [Min(0.5f)]
    [Tooltip("顶点因碰撞等原因未完成穿越时的最大等待，防止技能卡死。")]
    [SerializeField] private float vertexExitTimeout = 4.8f;
    [SerializeField] private GameObject vertexLaunchEffectPrefab;
    [Min(0f)]
    [SerializeField] private float vertexLaunchEffectLifetime = 0.7f;

    [Header("安全圈与切线星光")]
    [Tooltip("中央安全圈只会在顶点星光完全离开后出现。")]
    [SerializeField] private GameObject centerCirclePrefab;
    [Min(0f)]
    [SerializeField] private float safeCircleRevealLeadTime = 0.35f;
    [Min(0.2f)]
    [Tooltip("所有非顶点星光都在该时间后同时经过各自切点。")]
    [SerializeField] private float tangentTravelDuration = 1.55f;
    [Min(0f)]
    [SerializeField] private float tangentScatterAcceleration = 3.1f;
    [Min(0.5f)]
    [SerializeField] private float tangentBulletLifetime = 9.5f;
    [Min(0f)]
    [SerializeField] private float postScatterDuration = 1.6f;

    [Header("安全圈备用视觉")]
    [Range(24, 128)]
    [SerializeField] private int fallbackCircleSegments = 72;
    [Min(0.01f)]
    [SerializeField] private float fallbackCircleWidth = 0.13f;
    [SerializeField] private Color fallbackCircleColor =
        new Color(0.72f, 0.48f, 1f, 0.88f);
    [SerializeField] private int fallbackSortingOrder = 96;

    private readonly List<FormationPoint> spawnPlan =
        new List<FormationPoint>();
    private readonly List<FormationStar> formedStars =
        new List<FormationStar>();
    private readonly List<MimicStarBullet> vertexBullets =
        new List<MimicStarBullet>();
    private readonly List<MimicStarBullet> tangentBullets =
        new List<MimicStarBullet>();

    private GameObject centerVisual;
    private Material fallbackCircleMaterial;
    private Vector2 patternCenter;
    private Vector2 patternFollowVelocity;
    private Vector3 casterOriginalPosition;
    private float usedLargeOuterRadius;
    private float usedLargeInnerRadius;
    private float usedSmallOuterRadius;
    private float usedSmallInnerRadius;
    private float usedCenterRadius;
    private float largeRotation;
    private float smallRotation;
    private float casterOrbitAngle;
    private bool contextReady;

    private void Reset()
    {
        skillName = "拟态群星";
        skillStartup = 0.4f;
        skillEndup = 0.4f;
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
                name + "：拟态群星必须由 Mew 以带 ArenaCenter/ArenaRadius 的上下文初始化。",
                this);
        }
    }

    public override IEnumerator Startup()
    {
        if (!ValidateConfiguration())
        {
            yield break;
        }

        ResolvePatternGeometry();
        BuildSpawnPlan();

        casterOriginalPosition = empty.transform.position;
        casterOrbitAngle = casterOrbitStartAngle;
        largeRotation = largeStartRotation;
        smallRotation = smallStartRotation;
        patternFollowVelocity = Vector2.zero;
        patternCenter = ResolveDesiredPatternCenter();
        if (empty is Mew enteringBoss)
        {
            Vector2 entryDirection = Quaternion.Euler(0f, 0f, casterOrbitAngle) * Vector2.right;
            Vector2 entry = ArenaCenter + entryDirection * (ArenaRadius + Mathf.Max(0.2f, casterOrbitOutsideOffset));
            yield return enteringBoss.TeleportForSkill(entry);
        }

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

        formedStars.Clear();
        vertexBullets.Clear();
        tangentBullets.Clear();
        DestroyCenterVisual();

        yield return OrbitAndBuildFormation();
        yield return HoldOppositeRotation();
        yield return StopFormationRotation();

        LaunchVertices();

        float waitTimer = 0f;
        while (!AllVerticesExited() && waitTimer < vertexExitTimeout)
        {
            waitTimer += Time.deltaTime;
            yield return null;
        }

        // 顶点星光离开后才把中央安全圈显示给玩家。
        CreateCenterVisual();
        if (safeCircleRevealLeadTime > 0f)
        {
            yield return new WaitForSeconds(safeCircleRevealLeadTime);
        }

        LaunchTangents();

        float observationTime = tangentTravelDuration + postScatterDuration;
        yield return ReturnCasterDuringObservation(observationTime);

        DestroyCenterVisual();
    }

    protected override void OnSkillFinished(MewSkillFinishReason reason)
    {
        DestroyCenterVisual();
        if (empty is Mew boss && boss.IsEnding) return;
        DestroyUnlaunchedBullets(vertexBullets);
        DestroyUnlaunchedBullets(tangentBullets);
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
                name + "：MimicStars 没有设置 projectilePrefab。",
                this);
            return false;
        }

        formationFireInterval = Mathf.Max(0.005f, formationFireInterval);
        formationTravelDuration = Mathf.Max(0.05f, formationTravelDuration);
        largeSegmentsPerEdge = Mathf.Max(3, largeSegmentsPerEdge);
        smallSegmentsPerEdge = Mathf.Max(3, smallSegmentsPerEdge);
        tangentTravelDuration = Mathf.Max(0.2f, tangentTravelDuration);
        vertexExitTimeout = Mathf.Max(0.5f, vertexExitTimeout);
        return true;
    }

    private void ResolvePatternGeometry()
    {
        float maximumOuter = Mathf.Max(
            4f,
            ArenaRadius - Mathf.Max(0f, arenaPadding));

        usedLargeOuterRadius = Mathf.Min(largeOuterRadius, maximumOuter);
        usedLargeInnerRadius = Mathf.Clamp(
            largeInnerRadius,
            1f,
            usedLargeOuterRadius * 0.78f);

        usedSmallOuterRadius = Mathf.Clamp(
            smallOuterRadius,
            2f,
            usedLargeOuterRadius * 0.82f);
        usedSmallInnerRadius = Mathf.Clamp(
            smallInnerRadius,
            0.8f,
            usedSmallOuterRadius * 0.78f);

        usedCenterRadius = Mathf.Clamp(
            centerCircleRadius,
            0.5f,
            usedSmallInnerRadius * 0.78f);
    }

    private void BuildSpawnPlan()
    {
        spawnPlan.Clear();

        List<FormationPoint> large = BuildHexagramPoints(
            usedLargeOuterRadius,
            usedLargeInnerRadius,
            largeSegmentsPerEdge,
            0);
        List<FormationPoint> small = BuildHexagramPoints(
            usedSmallOuterRadius,
            usedSmallInnerRadius,
            smallSegmentsPerEdge,
            1);

        // 大小星交错生成，使梦幻绕圈发射时两个图案同步成形。
        int max = Mathf.Max(large.Count, small.Count);
        for (int i = 0; i < max; i++)
        {
            if (i < large.Count)
            {
                spawnPlan.Add(large[i]);
            }
            if (i < small.Count)
            {
                spawnPlan.Add(small[i]);
            }
        }
    }

    private static List<FormationPoint> BuildHexagramPoints(
        float outerRadius,
        float innerRadius,
        int segmentsPerEdge,
        int ringIndex)
    {
        List<FormationPoint> result = new List<FormationPoint>();
        Vector2[] corners = new Vector2[12];

        for (int i = 0; i < corners.Length; i++)
        {
            float radius = i % 2 == 0 ? outerRadius : innerRadius;
            float angle = i * 30f;
            corners[i] =
                (Vector2)(Quaternion.Euler(0f, 0f, angle) * Vector2.right) *
                radius;
        }

        for (int edge = 0; edge < 12; edge++)
        {
            result.Add(new FormationPoint
            {
                localPoint = corners[edge],
                isVertex = edge % 2 == 0,
                ringIndex = ringIndex
            });

            Vector2 start = corners[edge];
            Vector2 end = corners[(edge + 1) % 12];
            for (int sample = 1; sample < segmentsPerEdge; sample++)
            {
                float t = sample / (float)segmentsPerEdge;
                result.Add(new FormationPoint
                {
                    localPoint = Vector2.Lerp(start, end, t),
                    isVertex = false,
                    ringIndex = ringIndex
                });
            }
        }

        return result;
    }

    private IEnumerator OrbitAndBuildFormation()
    {
        int spawnIndex = 0;
        float spawnTimer = formationFireInterval;

        while (spawnIndex < spawnPlan.Count)
        {
            float dt = Time.deltaTime;
            FollowPlayerWithPattern(dt);
            AdvanceRotations(dt, 1f);
            UpdateCasterOrbit(dt);
            UpdateFormationTargets();

            spawnTimer += dt;
            while (spawnTimer >= formationFireInterval &&
                   spawnIndex < spawnPlan.Count)
            {
                spawnTimer -= formationFireInterval;
                SpawnFormationStar(spawnPlan[spawnIndex]);
                spawnIndex++;
            }

            yield return null;
        }

        float settleTimer = 0f;
        while (!AllFormationStarsReady() && settleTimer < formationSettleTimeout)
        {
            float dt = Time.deltaTime;
            settleTimer += dt;
            FollowPlayerWithPattern(dt);
            AdvanceRotations(dt, 1f);
            UpdateCasterOrbit(dt);
            UpdateFormationTargets();
            yield return null;
        }
    }

    private IEnumerator HoldOppositeRotation()
    {
        float timer = 0f;
        while (timer < oppositeRotationHoldDuration)
        {
            float dt = Time.deltaTime;
            timer += dt;
            FollowPlayerWithPattern(dt);
            AdvanceRotations(dt, 1f);
            UpdateCasterOrbit(dt);
            UpdateFormationTargets();
            yield return null;
        }
    }

    private IEnumerator StopFormationRotation()
    {
        float timer = 0f;
        float duration = Mathf.Max(0.05f, rotationStopDuration);

        while (timer < duration)
        {
            float dt = Time.deltaTime;
            timer += dt;
            float t = Mathf.Clamp01(timer / duration);
            float smooth = t * t * (3f - 2f * t);
            float speedScale = 1f - smooth;

            FollowPlayerWithPattern(dt);
            AdvanceRotations(dt, speedScale);
            UpdateCasterOrbit(dt);
            UpdateFormationTargets();
            yield return null;
        }

        // 从这里开始锁定图案中心，顶点和切点轨迹不会再漂移。
        UpdateFormationTargets();
    }

    private void SpawnFormationStar(FormationPoint point)
    {
        GameObject usedPrefab = point.isVertex && vertexProjectilePrefab != null
            ? vertexProjectilePrefab
            : projectilePrefab;

        Vector2 target = GetWorldFormationPoint(point.localPoint, point.ringIndex);
        Vector2 spawnPosition = empty != null
            ? (Vector2)empty.transform.position
            : ArenaCenter;
        Vector2 visualDirection = target - spawnPosition;
        float angle = visualDirection.sqrMagnitude > 0.0001f
            ? Mathf.Atan2(visualDirection.y, visualDirection.x) * Mathf.Rad2Deg
            : 0f;

        GameObject bulletObject = MewStarPool.Spawn(
            usedPrefab,
            spawnPosition,
            Quaternion.Euler(0f, 0f, angle + projectileRotationOffset), empty);

        BarrageProjectile projectile =
            bulletObject.GetComponent<BarrageProjectile>();
        Rigidbody2D body = bulletObject.GetComponent<Rigidbody2D>();

        if (projectile == null || body == null)
        {
            Debug.LogError(
                usedPrefab.name +
                " 必须同时包含 BarrageProjectile 与 Rigidbody2D。",
                bulletObject);
            Destroy(bulletObject);
            return;
        }

        MimicStarBullet motion =
            bulletObject.GetComponent<MimicStarBullet>();
        if (motion == null)
        {
            motion = bulletObject.AddComponent<MimicStarBullet>();
        }

        motion.PrepareFormation(
            empty,
            target,
            formationTravelDuration,
            24f);

        SpriteRenderer starSprite = bulletObject.GetComponent<SpriteRenderer>();
        if (starSprite != null)
            starSprite.color = point.ringIndex == 0 ? new Color(1f, 0.64f, 0.92f) : new Color(0.56f, 0.94f, 1f);

        FormationStar star = new FormationStar
        {
            bullet = motion,
            localPoint = point.localPoint,
            isVertex = point.isVertex,
            ringIndex = point.ringIndex
        };
        formedStars.Add(star);

        if (point.isVertex)
        {
            vertexBullets.Add(motion);
        }
        else
        {
            tangentBullets.Add(motion);
        }
    }

    private void UpdateFormationTargets()
    {
        Quaternion large = Quaternion.Euler(0f, 0f, largeRotation);
        Quaternion small = Quaternion.Euler(0f, 0f, smallRotation);
        for (int i = 0; i < formedStars.Count; i++)
        {
            FormationStar star = formedStars[i];
            if (star == null || star.bullet == null || star.bullet.HasLaunched)
            {
                continue;
            }

            Vector2 target = patternCenter + (Vector2)((star.ringIndex == 0 ? large : small) * star.localPoint);
            Vector2 inward = patternCenter - target;
            float angle = inward.sqrMagnitude > 0.0001f
                ? Mathf.Atan2(inward.y, inward.x) * Mathf.Rad2Deg
                : 0f;

            star.bullet.SetFormationTarget(
                target,
                angle + projectileRotationOffset);
        }
    }

    private Vector2 GetWorldFormationPoint(Vector2 localPoint, int ringIndex)
    {
        float rotation = ringIndex == 0 ? largeRotation : smallRotation;
        Vector2 rotated = Quaternion.Euler(0f, 0f, rotation) * localPoint;
        return patternCenter + rotated;
    }

    private void FollowPlayerWithPattern(float dt)
    {
        Vector2 desired = ResolveDesiredPatternCenter();
        patternCenter = Vector2.SmoothDamp(
            patternCenter,
            desired,
            ref patternFollowVelocity,
            Mathf.Max(0.02f, patternFollowSmoothTime),
            Mathf.Infinity,
            Mathf.Max(0.0001f, dt));
    }

    private Vector2 ResolveDesiredPatternCenter()
    {
        Vector2 desired = ArenaCenter;
        Transform target = GetPlayerTransform();
        if (target != null)
        {
            desired = target.position;
        }

        float maximumOffset = Mathf.Max(
            0f,
            ArenaRadius - usedLargeOuterRadius - Mathf.Max(0f, arenaPadding));
        Vector2 offset = desired - ArenaCenter;
        if (offset.sqrMagnitude > maximumOffset * maximumOffset &&
            offset.sqrMagnitude > 0.0001f)
        {
            offset = offset.normalized * maximumOffset;
        }

        return ArenaCenter + offset;
    }

    private void AdvanceRotations(float dt, float speedScale)
    {
        largeRotation += largeRotationSpeed * speedScale * dt;
        smallRotation += smallRotationSpeed * speedScale * dt;
    }

    private void UpdateCasterOrbit(float dt)
    {
        if (empty == null)
        {
            return;
        }

        casterOrbitAngle += casterOrbitSpeed * dt;
        float radius = ArenaRadius + Mathf.Max(0.2f, casterOrbitOutsideOffset);
        Vector2 direction =
            Quaternion.Euler(0f, 0f, casterOrbitAngle) * Vector2.right;
        Vector2 position = ArenaCenter + direction * radius;

        empty.transform.position = new Vector3(
            position.x,
            position.y,
            empty.transform.position.z);
    }

    private bool AllFormationStarsReady()
    {
        if (formedStars.Count == 0)
        {
            return true;
        }

        for (int i = 0; i < formedStars.Count; i++)
        {
            FormationStar star = formedStars[i];
            if (star != null && star.bullet != null && !star.bullet.IsFormationReady)
            {
                return false;
            }
        }

        return true;
    }

    private void LaunchVertices()
    {
        for (int i = 0; i < formedStars.Count; i++)
        {
            FormationStar star = formedStars[i];
            if (star == null || !star.isVertex || star.bullet == null)
            {
                continue;
            }

            if (vertexLaunchEffectPrefab != null)
            {
                GameObject effect = Instantiate(
                    vertexLaunchEffectPrefab,
                    star.bullet.transform.position,
                    Quaternion.identity);

                if (vertexLaunchEffectLifetime > 0f)
                {
                    Destroy(effect, vertexLaunchEffectLifetime);
                }
            }

            Vector2 position = star.bullet.transform.position;
            star.bullet.LaunchVertex(
                patternCenter,
                usedCenterRadius,
                patternCenter - position,
                vertexStartSpeed,
                vertexAcceleration,
                vertexExitPadding,
                vertexLifetime);
        }
    }

    private bool AllVerticesExited()
    {
        if (vertexBullets.Count == 0)
        {
            return true;
        }

        for (int i = 0; i < vertexBullets.Count; i++)
        {
            MimicStarBullet bullet = vertexBullets[i];
            if (bullet != null && !bullet.HasExitedCenterCircle)
            {
                return false;
            }
        }

        return true;
    }

    private void LaunchTangents()
    {
        for (int i = 0; i < formedStars.Count; i++)
        {
            FormationStar star = formedStars[i];
            if (star == null || star.isVertex || star.bullet == null)
            {
                continue;
            }

            Vector2 position = star.bullet.transform.position;
            bool clockwise = star.ringIndex == 0
                ? largeClockwiseTangents
                : smallClockwiseTangents;
            Vector2 tangentPoint = CalculateTangentPoint(
                position,
                patternCenter,
                usedCenterRadius,
                clockwise);

            star.bullet.LaunchTangent(
                patternCenter,
                tangentPoint,
                tangentTravelDuration,
                tangentScatterAcceleration,
                tangentBulletLifetime);
        }
    }

    private IEnumerator ReturnCasterDuringObservation(float observationTime)
    {
        float duration = Mathf.Max(0.7f, observationTime);
        float returnDuration = Mathf.Min(
            Mathf.Max(0.7f, casterReturnDuration),
            duration);
        Vector3 returnStart = empty != null
            ? empty.transform.position
            : casterOriginalPosition;
        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;

            if (empty != null && returnDuration > 0f)
            {
                float t = Mathf.Clamp01(timer / returnDuration);
                float smooth = t * t * (3f - 2f * t);
                empty.transform.position = Vector3.Lerp(
                    returnStart,
                    casterOriginalPosition,
                    smooth);
            }

            yield return null;
        }

        if (empty != null)
        {
            empty.transform.position = casterOriginalPosition;
        }
    }

    private static Vector2 CalculateTangentPoint(
        Vector2 point,
        Vector2 circleCenter,
        float radius,
        bool clockwise)
    {
        Vector2 relative = point - circleCenter;
        float distanceSquared = relative.sqrMagnitude;
        float radiusSquared = radius * radius;

        if (distanceSquared <= radiusSquared + 0.0001f)
        {
            Vector2 fallback = relative.sqrMagnitude > 0.0001f
                ? relative.normalized
                : Vector2.right;
            return circleCenter + fallback * radius;
        }

        float root = Mathf.Sqrt(
            Mathf.Max(0f, distanceSquared - radiusSquared));
        Vector2 perpendicular = new Vector2(-relative.y, relative.x);
        float sign = clockwise ? -1f : 1f;

        Vector2 tangentRelative =
            relative * (radiusSquared / distanceSquared) +
            perpendicular *
            (sign * radius * root / distanceSquared);

        return circleCenter + tangentRelative;
    }

    private void CreateCenterVisual()
    {
        DestroyCenterVisual();
        centerVisual = MewSigilVisual.Create(null, patternCenter, usedCenterRadius,
            MewSigilVisual.Style.Sanctuary, fallbackSortingOrder).gameObject;
    }
    private void DestroyCenterVisual()
    {
        if (centerVisual != null)
        {
            Destroy(centerVisual);
            centerVisual = null;
        }

        if (fallbackCircleMaterial != null)
        {
            Destroy(fallbackCircleMaterial);
            fallbackCircleMaterial = null;
        }
    }

    private static void DestroyUnlaunchedBullets(
        List<MimicStarBullet> bullets)
    {
        for (int i = bullets.Count - 1; i >= 0; i--)
        {
            MimicStarBullet bullet = bullets[i];
            if (bullet != null && !bullet.HasLaunched)
            {
                bullet.GetComponent<BarrageProjectile>().Despawn();
            }
        }
    }
}
