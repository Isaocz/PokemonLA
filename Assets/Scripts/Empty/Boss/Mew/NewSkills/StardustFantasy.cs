using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 梦幻第三阶段最终技能：终符「星尘幻想」Hard 风格重制。
///
/// 设计目标：尽量还原《东方永夜抄》高难版本的视觉与运动逻辑：
/// 1. 使用 9 个使魔同时布置七色星弹；
/// 2. 每轮依次经历“中心螺旋展开 -> 大范围花瓣回环 -> 向中心收束”；
/// 3. 下一轮反向镜像，避免图案始终朝同一方向卷动；
/// 4. 星弹沿使魔经过的位置形成连续曲线，短暂停留后先内收，再向外返还；
/// 5. 前 45 秒维持黑洞边缘式九使魔星轨；
/// 6. 最后 15 秒解除限制圈，以收缩技能圈和使魔蓄积 128 颗星弹；
/// 7. 最后 5 秒中，2 秒沿直线散成八瓣闭合弹幕花；
///    随后保持 2.5 秒，并用 BarrageProjectile 的 0.5 秒碰撞消失动画收尾。
///
/// 60 秒计时仍由 Mew.cs 统一控制；限制圈会在前 45 秒缩至 16，
/// 进入终幕后由独立技能圈继续提供无文字预警。
/// </summary>
public sealed class StardustFantasy : MewBaseSkill
{
    private const int FamiliarCount = 9;

    private enum PatternStage
    {
        SpiralOut,
        Rosette,
        Collapse,
        Rest
    }

    #region Inspector

    [Header("运行时 Hard 预设")]
    [Tooltip("开启后，ConfigureRuntime 会覆盖旧 Prefab 中残留的 Easy 参数。关闭后完全使用 Inspector 手动数值。")]
    [SerializeField] private bool applyHardPresetAtRuntime = true;

    [Header("星光与使魔外观")]
    [Tooltip("普通 StarLight Prefab，必须带 BarrageProjectile、Rigidbody2D 和伤害碰撞体。")]
    [SerializeField] private GameObject projectilePrefab;

    [Tooltip("可选的纯视觉使魔 Prefab。留空时复制 StarLight 的 Sprite 创建无伤害使魔。")]
    [SerializeField] private GameObject familiarVisualPrefab;

    [SerializeField, Min(0.1f)] private float familiarVisualScale = 1.75f;
    [SerializeField, Min(0f)] private float familiarPulseAmount = 0.10f;
    [SerializeField, Min(0f)] private float familiarPulseSpeed = 4.2f;
    [SerializeField, Range(0.2f, 1.5f)] private float projectileScaleMultiplier = 0.68f;

    [Header("持续时间")]
    [SerializeField, Min(1f)] private float spellDuration = 60f;
    [SerializeField, Min(1f)] private float endingArenaRadius = 16f;

    [Tooltip("终幕持续时间。默认最后 15 秒：10 秒蓄积、2 秒散花、3 秒淡出。")]
    [SerializeField, Min(5f)] private float finalPhaseDuration = 15f;

    [Header("Hard：一轮图案结构")]
    [Tooltip("从梦幻附近向场地外展开螺旋臂的持续时间。")]
    [SerializeField, Min(0.5f)] private float spiralOutDuration = 5.4f;

    [Tooltip("使魔在大范围花瓣轨道上回环的持续时间。")]
    [SerializeField, Min(0.5f)] private float rosetteDuration = 5.2f;

    [Tooltip("使魔携带星轨向中心收束的持续时间。")]
    [SerializeField, Min(0.5f)] private float collapseDuration = 3.5f;

    [Tooltip("第一轮结束后的短暂停顿。")]
    [SerializeField, Min(0f)] private float firstRestDuration = 0.85f;

    [Tooltip("第二轮开始后的停顿。原作后续波次之间会稍有喘息。")]
    [SerializeField, Min(0f)] private float laterRestDuration = 1.30f;

    [Header("Hard：使魔轨迹")]
    [Tooltip("整组九使魔的基础旋转速度。")]
    [SerializeField] private float patternAngularSpeed = 24f;

    [Tooltip("螺旋展开阶段额外旋转的圈数。")]
    [SerializeField, Range(0.1f, 2f)] private float spiralExtraTurns = 0.72f;

    [Tooltip("使魔距离中心的最小半径。")]
    [SerializeField, Min(0.1f)] private float innerOrbitRadius = 1.15f;

    [Tooltip("螺旋展开阶段最外半径占限制圈半径的比例。")]
    [SerializeField, Range(0.55f, 0.95f)] private float spiralOuterRadiusRatio = 0.82f;

    [Tooltip("花瓣轨道的主圆半径比例。")]
    [SerializeField, Range(0.2f, 0.7f)] private float rosetteMainRadiusRatio = 0.47f;

    [Tooltip("花瓣轨道的副圆半径比例。")]
    [SerializeField, Range(0.1f, 0.55f)] private float rosetteLoopRadiusRatio = 0.32f;

    [Tooltip("副圆相对主圆的角速度倍率。-2 会形成三瓣式大回环。")]
    [SerializeField] private float rosetteSecondarySpeed = -2f;

    [Tooltip("螺旋臂的横向摆动，避免九条轨迹只是完全笔直的径向线。")]
    [SerializeField, Min(0f)] private float spiralSideWaveAmplitude = 0.65f;

    [SerializeField] private float initialPatternAngle = 90f;
    [SerializeField] private bool randomizeInitialPatternAngle;

    [Header("Hard：布弹节奏")]
    [Tooltip("九个使魔每隔该时间各设置一颗星弹。0.19 秒约为 47 发/秒。")]
    [SerializeField, Min(0.06f)] private float placementInterval = 0.19f;

    [Tooltip("终盘布弹间隔倍率。限制圈缩小时只轻微加密，防止突然无解。")]
    [SerializeField, Range(0.70f, 1f)] private float endingPlacementIntervalMultiplier = 0.88f;

    [Tooltip("阶段切换前后暂停布弹的时间，防止轨迹插值处出现一条突兀的直线。")]
    [SerializeField, Range(0f, 0.5f)] private float transitionPlacementBlank = 0.12f;

    [Header("Hard：星弹运动")]
    [Tooltip("星弹显现期间关闭碰撞。")]
    [SerializeField, Min(0f)] private float warningDuration = 0.13f;

    [Tooltip("星弹沿使魔轨迹停留的时间。停留期间已经具有伤害。")]
    [SerializeField, Min(0f)] private float placedHoldDuration = 0.68f;

    [Tooltip("停留结束后先向中心移动的最大距离。")]
    [SerializeField, Min(0f)] private float inwardTravelDistance = 2.6f;

    [SerializeField, Min(0.1f)] private float inwardSpeed = 3.8f;
    [SerializeField, Min(0.1f)] private float outwardSpeed = 4.6f;

    [Tooltip("星弹内收/返还期间的轻微角度漂移。奇偶使魔方向相反。")]
    [SerializeField] private float bulletAngularSpeed = 5.5f;

    [Tooltip("终盘星弹速度倍率。")]
    [SerializeField, Range(1f, 1.4f)] private float endingBulletSpeedMultiplier = 1.14f;

    [SerializeField, Min(0f)] private float despawnOutsidePadding = 1.0f;
    [SerializeField] private float projectileRotationOffset;
    [SerializeField] private float projectileSpriteSpinSpeed = 110f;

    [Header("终幕 15 秒：技能圈、蓄弹与八瓣弹幕花")]
    [Tooltip("进入终幕时原限制圈淡出的时间。终幕由独立技能圈继续提示中心危险。")]
    [SerializeField, Min(0f)] private float arenaReleaseFadeDuration = 0.45f;

    [Tooltip("终幕前段：技能圈与九个使魔向中心收缩，并逐步生成最终花所需星弹。")]
    [SerializeField, Min(1f)] private float finalBuildDuration = 10f;

    [Tooltip("倒数第二段：已生成完毕的星弹从中心星核散成弹幕花。")]
    [SerializeField, Min(0.5f)] private float finalBloomDuration = 2f;

    [Tooltip("最后一段总时长：弹幕花先保持，再在末尾播放碰撞同款消失动画。")]
    [SerializeField, Min(0.5f)] private float finalFlowerFadeDuration = 3f;

    [Tooltip("使用 BarrageProjectile.FadeMode 的销毁动画时长；项目碰撞销毁默认是 0.5 秒。")]
    [SerializeField, Range(0.1f, 1.5f)] private float finalDestroyAnimationDuration = 0.5f;

    [Header("终幕技能圈")]
    [SerializeField, Min(0.1f)] private float finaleCircleEndRadius = 2.4f;
    [SerializeField, Min(0.01f)] private float finaleCircleWidth = 0.16f;
    [SerializeField, Range(24, 160)] private int finaleCircleSegments = 96;
    [SerializeField] private Color finaleCircleColor =
        new Color(0.86f, 0.54f, 1f, 0.92f);

    [Header("最终八瓣花")]
    [Tooltip("草图使用八个相互交叠的闭合花瓣。")]
    [SerializeField, Range(4, 12)] private int finalFlowerPetalCount = 8;

    [Tooltip("每片花瓣轮廓上的星弹数。8×16 = 128 颗。")]
    [SerializeField, Range(8, 24)] private int finalFlowerPointsPerPetal = 16;

    [Tooltip("花瓣长度会自动适配 60×48 房间；该值为边界内缩留白。")]
    [SerializeField, Min(0f)] private float finalFlowerRoomPadding = 2.2f;

    [Tooltip("花瓣横向宽度。默认较窄，给相邻花瓣之间留下清晰躲避区域。")]
    [SerializeField, Min(0.5f)] private float finalFlowerHalfWidth = 4.4f;

    [Tooltip("花瓣长度占可用房间边界距离的比例。降低该值会扩大墙边安全空间。")]
    [SerializeField, Range(0.5f, 0.95f)] private float finalFlowerLengthRatio = 0.80f;

    [Tooltip("0 度时八片花瓣分别朝水平、垂直和四个斜向展开。")]
    [SerializeField] private float finalFlowerRotation = 0f;

    [Tooltip("所有弹幕已经在蓄力阶段生成，散花时仅允许很小的错峰。")]
    [SerializeField, Min(0f)] private float finalFlowerDepartureSpread = 0.08f;

    [SerializeField, Min(0f)] private float finalFlowerWarningDuration = 0.05f;

    [Header("三阶段房间尺寸")]
    [Tooltip("二、三阶段房间半尺寸。60×48 房间对应 30×24。")]
    [SerializeField] private Vector2 roomHalfExtents = new Vector2(30f, 24f);

    [Header("安全与性能")]
    [SerializeField, Min(64)] private int maximumLiveProjectiles = 620;

    [Tooltip("达到上限时销毁最早生成的弹幕，而不是停止新图案。")]
    [SerializeField] private bool recycleOldestWhenFull = true;

    [Tooltip("技能结束或被取消时立即清理本技能生成的星弹。")]
    [SerializeField] private bool cleanupProjectilesOnFinish = true;

    #endregion

    #region Runtime

    private readonly List<GameObject> familiarObjects =
        new List<GameObject>(FamiliarCount);
    private readonly List<SpriteRenderer> familiarRenderers =
        new List<SpriteRenderer>(FamiliarCount);
    private readonly List<Vector3> familiarBaseScales =
        new List<Vector3>(FamiliarCount);
    private readonly List<GameObject> spawnedProjectiles =
        new List<GameObject>();
    private readonly List<GameObject> retiredProjectiles =
        new List<GameObject>();

    private StardustFinaleCircle finaleCircle;

    private readonly Color[] stardustColors =
    {
        new Color(1.00f, 0.36f, 0.44f, 1f),
        new Color(1.00f, 0.62f, 0.22f, 1f),
        new Color(1.00f, 0.90f, 0.28f, 1f),
        new Color(0.40f, 1.00f, 0.50f, 1f),
        new Color(0.28f, 0.94f, 1.00f, 1f),
        new Color(0.43f, 0.58f, 1.00f, 1f),
        new Color(0.94f, 0.38f, 1.00f, 1f),
    };

    private float usedInitialPatternAngle;
    private bool contextValid;
    private int cycleIndex;
    private int placementBeat;

    #endregion

    private void Reset()
    {
        skillName = "终符「星尘幻想」Hard·终幕花";
        skillStartup = 0f;
        skillEndup = 0f;
        existTime = 0f;
        repeat = 1;
        repeatTime = 0f;
    }

    /// <summary>
    /// 兼容旧接入方式。默认使用 15 秒终幕和 60×48 房间。
    /// </summary>
    public void ConfigureRuntime(
        float duration,
        float finalRadius,
        GameObject projectileOverride = null)
    {
        ConfigureRuntime(
            duration,
            finalRadius,
            15f,
            new Vector2(30f, 24f),
            projectileOverride);
    }

    /// <summary>
    /// 兼容上一版只传入终幕时长的接入方式。
    /// </summary>
    public void ConfigureRuntime(
        float duration,
        float finalRadius,
        float finaleDuration,
        GameObject projectileOverride = null)
    {
        ConfigureRuntime(
            duration,
            finalRadius,
            finaleDuration,
            new Vector2(30f, 24f),
            projectileOverride);
    }

    /// <summary>
    /// 由 Mew.cs 调用，使技能总时长、终幕时长、房间尺寸、缩圈终点
    /// 和弹幕 Prefab 与第三阶段统一。
    /// </summary>
    public void ConfigureRuntime(
        float duration,
        float finalRadius,
        float finaleDuration,
        Vector2 usedRoomHalfExtents,
        GameObject projectileOverride = null)
    {
        spellDuration = Mathf.Max(1f, duration);
        endingArenaRadius = Mathf.Max(1f, finalRadius);
        finalPhaseDuration = Mathf.Clamp(
            finaleDuration,
            5f,
            Mathf.Max(5f, spellDuration - 0.5f));
        roomHalfExtents = new Vector2(
            Mathf.Max(4f, Mathf.Abs(usedRoomHalfExtents.x)),
            Mathf.Max(4f, Mathf.Abs(usedRoomHalfExtents.y)));

        if (projectileOverride != null)
        {
            projectilePrefab = projectileOverride;
        }

        if (applyHardPresetAtRuntime)
        {
            ApplyHardPreset();
            finalPhaseDuration = Mathf.Clamp(
                finaleDuration,
                5f,
                Mathf.Max(5f, spellDuration - 0.5f));
            roomHalfExtents = new Vector2(
                Mathf.Max(4f, Mathf.Abs(usedRoomHalfExtents.x)),
                Mathf.Max(4f, Mathf.Abs(usedRoomHalfExtents.y)));
        }
    }

    private void ApplyHardPreset()
    {
        familiarVisualScale = 1.75f;

        // 保留玩家实测后确认的常规 Hard 参数。
        projectileScaleMultiplier = 0.68f;
        placementInterval = 0.19f;
        placedHoldDuration = 0.68f;
        outwardSpeed = 4.6f;

        spiralOutDuration = 5.4f;
        rosetteDuration = 5.2f;
        collapseDuration = 3.5f;
        firstRestDuration = 0.85f;
        laterRestDuration = 1.30f;

        patternAngularSpeed = 24f;
        spiralExtraTurns = 0.72f;
        innerOrbitRadius = 1.15f;
        spiralOuterRadiusRatio = 0.82f;
        rosetteMainRadiusRatio = 0.47f;
        rosetteLoopRadiusRatio = 0.32f;
        rosetteSecondarySpeed = -2f;
        spiralSideWaveAmplitude = 0.65f;

        endingPlacementIntervalMultiplier = 0.88f;
        transitionPlacementBlank = 0.12f;

        warningDuration = 0.13f;
        inwardTravelDistance = 2.6f;
        inwardSpeed = 3.8f;
        bulletAngularSpeed = 5.5f;
        endingBulletSpeedMultiplier = 1.14f;
        projectileSpriteSpinSpeed = 110f;

        // 15 秒终幕：10 秒蓄积、2 秒散花、3 秒淡出。
        finalPhaseDuration = 15f;
        arenaReleaseFadeDuration = 0.45f;
        finalBuildDuration = 10f;
        finalBloomDuration = 2f;
        finalFlowerFadeDuration = 3f;
        finalDestroyAnimationDuration = 0.5f;

        finaleCircleEndRadius = 2.4f;
        finaleCircleWidth = 0.16f;
        finaleCircleSegments = 96;

        finalFlowerPetalCount = 8;
        finalFlowerPointsPerPetal = 16;
        finalFlowerRoomPadding = 2.2f;
        finalFlowerHalfWidth = 4.4f;
        finalFlowerLengthRatio = 0.80f;
        finalFlowerRotation = 0f;
        finalFlowerDepartureSpread = 0.08f;
        finalFlowerWarningDuration = 0.05f;

        // 正常 Hard 阶段仍允许较高密度；终幕固定仅使用 128 颗。
        maximumLiveProjectiles = 520;
        recycleOldestWhenFull = true;
    }

    protected override void OnContextInitialized(MewSkillContext context)
    {
        contextValid =
            empty != null &&
            context.Player != null &&
            context.ArenaRadius > 1f;

        usedInitialPatternAngle = randomizeInitialPatternAngle
            ? Random.Range(0f, 360f)
            : initialPatternAngle;

        cycleIndex = 0;
        placementBeat = 0;

        if (!contextValid)
        {
            Debug.LogError(
                name + "：星尘幻想需要 Player、ArenaCenter 和 ArenaRadius 的三阶段上下文。",
                this);
        }
    }

    public override IEnumerator CoreLogic()
    {
        if (!ValidateConfiguration())
        {
            yield break;
        }

        CreateFamiliarVisuals();

        float normalDuration = Mathf.Max(
            0.5f,
            spellDuration - finalPhaseDuration);
        float elapsed = 0f;
        float cycleElapsed = 0f;
        float nextPlacementTime = 0f;

        // 前 45 秒：九使魔黑洞边缘式星轨。
        while (elapsed < normalDuration)
        {
            float progress = Mathf.Clamp01(elapsed / normalDuration);
            float currentArenaRadius = Mathf.Lerp(
                ArenaRadius,
                endingArenaRadius,
                progress);

            PatternStage stage = GetPatternStage(cycleElapsed);
            UpdateFamiliarVisuals(
                elapsed,
                cycleElapsed,
                currentArenaRadius,
                stage);

            bool canPlace = CanPlaceDuringStage(cycleElapsed, stage);
            if (!canPlace)
            {
                // 跳过阶段切换留白期间错过的拍点，防止恢复布弹时爆发追帧。
                nextPlacementTime = Mathf.Max(
                    nextPlacementTime,
                    cycleElapsed);
            }
            else if (cycleElapsed >= nextPlacementTime)
            {
                PlaceStardustBeat(
                    elapsed,
                    cycleElapsed,
                    currentArenaRadius,
                    progress,
                    stage);
                placementBeat++;

                float usedInterval = placementInterval * Mathf.Lerp(
                    1f,
                    endingPlacementIntervalMultiplier,
                    progress);
                nextPlacementTime = cycleElapsed +
                    Mathf.Max(0.06f, usedInterval);
            }

            float deltaTime = Time.deltaTime;
            elapsed += deltaTime;
            cycleElapsed += deltaTime;

            float cycleDuration = GetCycleDuration(cycleIndex);
            if (cycleElapsed >= cycleDuration)
            {
                cycleElapsed -= cycleDuration;
                cycleIndex++;
                placementBeat = 0;
                nextPlacementTime = 0f;
            }

            RemoveDestroyedProjectileReferences();
            yield return null;
        }

        // 最后 15 秒：技能圈蓄积 128 颗星弹，随后展开八瓣弹幕花。
        yield return RunFinale(finalPhaseDuration);
    }

    protected override void OnSkillFinished(MewSkillFinishReason reason)
    {
        DestroyFamiliarVisuals();
        DestroyFinaleCircle();

        if (cleanupProjectilesOnFinish)
        {
            DestroySpawnedProjectiles();
            DestroyRetiredProjectiles();
        }
    }

    protected override void OnDestroy()
    {
        DestroyFamiliarVisuals();
        DestroyFinaleCircle();

        if (cleanupProjectilesOnFinish)
        {
            DestroySpawnedProjectiles();
            DestroyRetiredProjectiles();
        }

        base.OnDestroy();
    }

    private bool ValidateConfiguration()
    {
        if (!contextValid || empty == null)
        {
            return false;
        }

        if (projectilePrefab == null)
        {
            Debug.LogError(
                name + "：没有设置 projectilePrefab / StardustProjectilePrefab。",
                this);
            return false;
        }

        if (projectilePrefab.GetComponent<BarrageProjectile>() == null)
        {
            Debug.LogError(
                name + "：星光 Prefab 缺少 BarrageProjectile。",
                projectilePrefab);
            return false;
        }

        if (projectilePrefab.GetComponent<Rigidbody2D>() == null)
        {
            Debug.LogError(
                name + "：星光 Prefab 缺少 Rigidbody2D。",
                projectilePrefab);
            return false;
        }

        spellDuration = Mathf.Max(6f, spellDuration);
        finalPhaseDuration = Mathf.Clamp(
            finalPhaseDuration,
            5f,
            spellDuration - 0.5f);
        endingArenaRadius = Mathf.Clamp(
            endingArenaRadius,
            1f,
            Mathf.Max(1f, ArenaRadius));
        spiralOutDuration = Mathf.Max(0.5f, spiralOutDuration);
        rosetteDuration = Mathf.Max(0.5f, rosetteDuration);
        collapseDuration = Mathf.Max(0.5f, collapseDuration);
        placementInterval = Mathf.Max(0.06f, placementInterval);
        inwardSpeed = Mathf.Max(0.1f, inwardSpeed);
        outwardSpeed = Mathf.Max(0.1f, outwardSpeed);

        finalBuildDuration = Mathf.Max(1f, finalBuildDuration);
        finalBloomDuration = Mathf.Max(0.5f, finalBloomDuration);
        finalFlowerFadeDuration = Mathf.Max(0.5f, finalFlowerFadeDuration);
        finalDestroyAnimationDuration = Mathf.Clamp(
            finalDestroyAnimationDuration,
            0.1f,
            finalFlowerFadeDuration);

        float requiredFinaleDuration =
            finalBuildDuration +
            finalBloomDuration +
            finalFlowerFadeDuration;
        if (requiredFinaleDuration > finalPhaseDuration)
        {
            finalBuildDuration = Mathf.Max(
                1f,
                finalPhaseDuration -
                finalBloomDuration -
                finalFlowerFadeDuration);
        }

        finaleCircleEndRadius = Mathf.Clamp(
            finaleCircleEndRadius,
            0.5f,
            endingArenaRadius);
        finaleCircleSegments = Mathf.Clamp(
            finaleCircleSegments,
            24,
            160);

        finalFlowerPetalCount = Mathf.Clamp(
            finalFlowerPetalCount,
            4,
            12);
        finalFlowerPointsPerPetal = Mathf.Clamp(
            finalFlowerPointsPerPetal,
            8,
            24);
        finalFlowerRoomPadding = Mathf.Max(0f, finalFlowerRoomPadding);
        finalFlowerHalfWidth = Mathf.Max(0.5f, finalFlowerHalfWidth);
        finalFlowerLengthRatio = Mathf.Clamp(
            finalFlowerLengthRatio,
            0.5f,
            0.95f);
        roomHalfExtents = new Vector2(
            Mathf.Max(4f, Mathf.Abs(roomHalfExtents.x)),
            Mathf.Max(4f, Mathf.Abs(roomHalfExtents.y)));

        int finalFlowerProjectileCount =
            finalFlowerPetalCount *
            finalFlowerPointsPerPetal;
        maximumLiveProjectiles = Mathf.Max(
            Mathf.Max(64, maximumLiveProjectiles),
            finalFlowerProjectileCount + 4);
        return true;
    }

    #region 图案时间轴与轨迹

    private float GetActiveDuration()
    {
        return spiralOutDuration + rosetteDuration + collapseDuration;
    }

    private float GetRestDuration(int usedCycleIndex)
    {
        return usedCycleIndex <= 0
            ? firstRestDuration
            : laterRestDuration;
    }

    private float GetCycleDuration(int usedCycleIndex)
    {
        return GetActiveDuration() + GetRestDuration(usedCycleIndex);
    }

    private PatternStage GetPatternStage(float cycleElapsed)
    {
        if (cycleElapsed < spiralOutDuration)
        {
            return PatternStage.SpiralOut;
        }

        if (cycleElapsed < spiralOutDuration + rosetteDuration)
        {
            return PatternStage.Rosette;
        }

        if (cycleElapsed < GetActiveDuration())
        {
            return PatternStage.Collapse;
        }

        return PatternStage.Rest;
    }

    private bool CanPlaceDuringStage(
        float cycleElapsed,
        PatternStage stage)
    {
        if (stage == PatternStage.Rest)
        {
            return false;
        }

        float localStageTime;
        float stageDuration;

        switch (stage)
        {
            case PatternStage.SpiralOut:
                localStageTime = cycleElapsed;
                stageDuration = spiralOutDuration;
                break;

            case PatternStage.Rosette:
                localStageTime = cycleElapsed - spiralOutDuration;
                stageDuration = rosetteDuration;
                break;

            default:
                localStageTime =
                    cycleElapsed - spiralOutDuration - rosetteDuration;
                stageDuration = collapseDuration;
                break;
        }

        return localStageTime >= transitionPlacementBlank &&
               localStageTime <= stageDuration - transitionPlacementBlank;
    }

    private Vector2 GetFamiliarPosition(
        int familiarIndex,
        float elapsed,
        float cycleElapsed,
        float currentArenaRadius)
    {
        PatternStage stage = GetPatternStage(cycleElapsed);
        float direction = cycleIndex % 2 == 0 ? 1f : -1f;
        float symmetryOffset = familiarIndex * (360f / FamiliarCount);
        float globalAngle = usedInitialPatternAngle +
                            direction * patternAngularSpeed * elapsed;

        switch (stage)
        {
            case PatternStage.SpiralOut:
                return GetSpiralOutPosition(
                    familiarIndex,
                    symmetryOffset,
                    globalAngle,
                    direction,
                    cycleElapsed,
                    currentArenaRadius);

            case PatternStage.Rosette:
                return GetRosettePosition(
                    symmetryOffset,
                    globalAngle,
                    direction,
                    cycleElapsed - spiralOutDuration,
                    currentArenaRadius);

            case PatternStage.Collapse:
                return GetCollapsePosition(
                    symmetryOffset,
                    globalAngle,
                    direction,
                    cycleElapsed - spiralOutDuration - rosetteDuration,
                    currentArenaRadius);

            default:
                return GetRestPosition(
                    symmetryOffset,
                    globalAngle,
                    direction,
                    currentArenaRadius);
        }
    }

    private Vector2 GetSpiralOutPosition(
        int familiarIndex,
        float symmetryOffset,
        float globalAngle,
        float direction,
        float stageElapsed,
        float currentArenaRadius)
    {
        float progress = Mathf.Clamp01(stageElapsed / spiralOutDuration);
        float eased = SmoothStep(progress);
        float outerRadius = Mathf.Max(
            innerOrbitRadius + 0.5f,
            currentArenaRadius * spiralOuterRadiusRatio);
        float radius = Mathf.Lerp(innerOrbitRadius, outerRadius, eased);

        float angle = globalAngle + symmetryOffset +
                      direction * spiralExtraTurns * 360f * eased;

        Vector2 radial = RotateVector(Vector2.right, angle);
        Vector2 tangent = new Vector2(-radial.y, radial.x);
        float sideWave = Mathf.Sin(
            progress * Mathf.PI * 2f +
            familiarIndex * Mathf.PI * 0.42f) *
            spiralSideWaveAmplitude *
            Mathf.Sin(progress * Mathf.PI);

        return ArenaCenter + radial * radius + tangent * sideWave;
    }

    private Vector2 GetRosettePosition(
        float symmetryOffset,
        float globalAngle,
        float direction,
        float stageElapsed,
        float currentArenaRadius)
    {
        float transition = Mathf.Clamp01(stageElapsed / 0.55f);
        transition = SmoothStep(transition);

        float mainRadius = currentArenaRadius * rosetteMainRadiusRatio;
        float loopRadius = currentArenaRadius * rosetteLoopRadiusRatio;
        float mainAngle = globalAngle + symmetryOffset;
        float secondaryAngle =
            usedInitialPatternAngle +
            direction * rosetteSecondarySpeed *
            (globalAngle - usedInitialPatternAngle) +
            symmetryOffset;

        Vector2 rosetteOffset =
            RotateVector(Vector2.right, mainAngle) * mainRadius +
            RotateVector(Vector2.right, secondaryAngle) * loopRadius;

        // 与螺旋展开终点平滑衔接，避免阶段切换瞬移。
        Vector2 spiralEnd = GetSpiralOutPosition(
            Mathf.RoundToInt(
                symmetryOffset / (360f / FamiliarCount)),
            symmetryOffset,
            globalAngle,
            direction,
            spiralOutDuration,
            currentArenaRadius);

        return Vector2.Lerp(spiralEnd, ArenaCenter + rosetteOffset, transition);
    }

    private Vector2 GetCollapsePosition(
        float symmetryOffset,
        float globalAngle,
        float direction,
        float stageElapsed,
        float currentArenaRadius)
    {
        float progress = Mathf.Clamp01(stageElapsed / collapseDuration);
        float eased = SmoothStep(progress);

        float mainRadius = currentArenaRadius * rosetteMainRadiusRatio;
        float loopRadius = currentArenaRadius * rosetteLoopRadiusRatio;
        float mainAngle = globalAngle + symmetryOffset;
        float secondaryAngle =
            usedInitialPatternAngle +
            direction * rosetteSecondarySpeed *
            (globalAngle - usedInitialPatternAngle) +
            symmetryOffset;

        Vector2 rosetteOffset =
            RotateVector(Vector2.right, mainAngle) * mainRadius +
            RotateVector(Vector2.right, secondaryAngle) * loopRadius;

        Vector2 innerDirection = RotateVector(
            Vector2.right,
            mainAngle + direction * 130f * eased);
        Vector2 collapsedOffset = innerDirection * innerOrbitRadius;

        return ArenaCenter + Vector2.Lerp(
            rosetteOffset,
            collapsedOffset,
            eased);
    }

    private Vector2 GetRestPosition(
        float symmetryOffset,
        float globalAngle,
        float direction,
        float currentArenaRadius)
    {
        float radius = Mathf.Min(
            currentArenaRadius * 0.18f,
            innerOrbitRadius + 1.2f);
        float angle = globalAngle + symmetryOffset + direction * 20f;
        return ArenaCenter + RotateVector(Vector2.right, angle) * radius;
    }

    private static float SmoothStep(float value)
    {
        value = Mathf.Clamp01(value);
        return value * value * (3f - 2f * value);
    }

    private static Vector2 RotateVector(Vector2 vector, float degrees)
    {
        return Quaternion.Euler(0f, 0f, degrees) * vector;
    }

    #endregion

    #region 使魔视觉

    private void CreateFamiliarVisuals()
    {
        DestroyFamiliarVisuals();

        for (int i = 0; i < FamiliarCount; i++)
        {
            GameObject familiar = CreateSingleFamiliarVisual(i);
            familiarObjects.Add(familiar);

            SpriteRenderer renderer = familiar != null
                ? familiar.GetComponentInChildren<SpriteRenderer>()
                : null;
            familiarRenderers.Add(renderer);
            familiarBaseScales.Add(
                familiar != null
                    ? familiar.transform.localScale
                    : Vector3.one);

            if (familiar != null)
            {
                RegisterTemporaryObject(familiar);
            }
        }
    }

    private GameObject CreateSingleFamiliarVisual(int familiarIndex)
    {
        GameObject familiar;

        if (familiarVisualPrefab != null)
        {
            familiar = Instantiate(
                familiarVisualPrefab,
                ArenaCenter,
                Quaternion.identity);
            DisableFamiliarDamageComponents(familiar);
        }
        else
        {
            familiar = new GameObject(
                "Stardust Familiar Hard " + (familiarIndex + 1));
            familiar.transform.position = ArenaCenter;

            SpriteRenderer renderer = familiar.AddComponent<SpriteRenderer>();
            SpriteRenderer sourceRenderer =
                projectilePrefab.GetComponentInChildren<SpriteRenderer>();

            if (sourceRenderer != null)
            {
                renderer.sprite = sourceRenderer.sprite;
                renderer.sharedMaterial = sourceRenderer.sharedMaterial;
                renderer.sortingLayerID = sourceRenderer.sortingLayerID;
                renderer.sortingOrder = sourceRenderer.sortingOrder + 3;
                familiar.transform.localScale =
                    projectilePrefab.transform.localScale *
                    familiarVisualScale;
            }
        }

        SpriteRenderer familiarRenderer =
            familiar.GetComponentInChildren<SpriteRenderer>();
        if (familiarRenderer != null)
        {
            familiarRenderer.color = stardustColors[
                familiarIndex % stardustColors.Length];
        }

        return familiar;
    }

    private static void DisableFamiliarDamageComponents(GameObject familiar)
    {
        if (familiar == null)
        {
            return;
        }

        Collider2D[] colliders =
            familiar.GetComponentsInChildren<Collider2D>(true);
        for (int i = 0; i < colliders.Length; i++)
        {
            colliders[i].enabled = false;
        }

        Rigidbody2D[] bodies =
            familiar.GetComponentsInChildren<Rigidbody2D>(true);
        for (int i = 0; i < bodies.Length; i++)
        {
            bodies[i].velocity = Vector2.zero;
            bodies[i].angularVelocity = 0f;
            bodies[i].simulated = false;
        }

        BarrageProjectile[] barrages =
            familiar.GetComponentsInChildren<BarrageProjectile>(true);
        for (int i = 0; i < barrages.Length; i++)
        {
            barrages[i].enabled = false;
        }

        StardustFantasyBullet[] motions =
            familiar.GetComponentsInChildren<StardustFantasyBullet>(true);
        for (int i = 0; i < motions.Length; i++)
        {
            motions[i].enabled = false;
        }
    }

    private void UpdateFamiliarVisuals(
        float elapsed,
        float cycleElapsed,
        float currentArenaRadius,
        PatternStage stage)
    {
        for (int i = 0; i < familiarObjects.Count; i++)
        {
            GameObject familiar = familiarObjects[i];
            if (familiar == null)
            {
                continue;
            }

            familiar.transform.position = GetFamiliarPosition(
                i,
                elapsed,
                cycleElapsed,
                currentArenaRadius);

            float pulse = 1f + familiarPulseAmount * Mathf.Sin(
                elapsed * familiarPulseSpeed +
                i * Mathf.PI * 0.52f);
            Vector3 baseScale = i < familiarBaseScales.Count
                ? familiarBaseScales[i]
                : Vector3.one;
            familiar.transform.localScale = baseScale * pulse;

            SpriteRenderer renderer = i < familiarRenderers.Count
                ? familiarRenderers[i]
                : null;
            if (renderer != null)
            {
                Color color = stardustColors[i % stardustColors.Length];
                color.a = stage == PatternStage.Rest ? 0.42f : 0.96f;
                renderer.color = color;
            }
        }
    }

    private void DestroyFamiliarVisuals()
    {
        for (int i = familiarObjects.Count - 1; i >= 0; i--)
        {
            GameObject familiar = familiarObjects[i];
            if (familiar != null)
            {
                UnregisterTemporaryObject(familiar);
                Destroy(familiar);
            }
        }

        familiarObjects.Clear();
        familiarRenderers.Clear();
        familiarBaseScales.Clear();
    }

    #endregion

    #region 终幕：技能圈蓄积、八瓣散花与淡出

    private IEnumerator RunFinale(float finaleDuration)
    {
        float usedFinaleDuration = Mathf.Max(5f, finaleDuration);
        float usedFadeDuration = Mathf.Min(
            finalFlowerFadeDuration,
            usedFinaleDuration - 1f);
        float usedBloomDuration = Mathf.Min(
            finalBloomDuration,
            usedFinaleDuration - usedFadeDuration - 0.5f);
        float usedBuildDuration = Mathf.Max(
            0.5f,
            usedFinaleDuration -
            usedBloomDuration -
            usedFadeDuration);

        Mew mew = empty as Mew;
        if (mew != null)
        {
            // 不显示文字。旧限制圈淡出后，由收缩技能圈承担空间提示。
            mew.ReleasePhaseThreeArenaBoundary(
                arenaReleaseFadeDuration);
        }

        RetireCurrentProjectiles();
        CreateFinaleCircle(usedBuildDuration);

        List<Vector3> familiarStartPositions =
            CaptureFamiliarPositions();

        int targetProjectileCount =
            finalFlowerPetalCount *
            finalFlowerPointsPerPetal;
        int generatedCount = 0;
        float buildElapsed = 0f;

        // 终幕前段不会瞬间生成 128 颗弹幕。
        // 按时间均匀生成，单帧通常只创建 0～1 颗，降低卡顿风险。
        while (buildElapsed < usedBuildDuration)
        {
            float progress = Mathf.Clamp01(
                buildElapsed / usedBuildDuration);
            UpdateFinalBuildFamiliarVisuals(
                familiarStartPositions,
                progress);

            int desiredCount = Mathf.Min(
                targetProjectileCount,
                Mathf.FloorToInt(
                    progress * targetProjectileCount));

            while (generatedCount < desiredCount)
            {
                SpawnFinalSeedStar(
                    generatedCount,
                    targetProjectileCount);
                generatedCount++;
            }

            RetireOldProjectilesGradually(
                Time.deltaTime,
                Mathf.Max(
                    0.05f,
                    usedBuildDuration - buildElapsed));

            buildElapsed += Time.deltaTime;
            RemoveDestroyedProjectileReferences();
            yield return null;
        }

        // 补齐因低帧率或最后一帧取整未生成的少量星弹。
        while (generatedCount < targetProjectileCount)
        {
            SpawnFinalSeedStar(
                generatedCount,
                targetProjectileCount);
            generatedCount++;
            yield return null;
        }

        DestroyRetiredProjectiles();
        DestroyFamiliarVisuals();

        if (finaleCircle != null)
        {
            finaleCircle.FadeOut(0.22f);
        }

        StartFinalFlower(usedBloomDuration);

        // 2 秒直线散花。
        float bloomElapsed = 0f;
        while (bloomElapsed < usedBloomDuration)
        {
            bloomElapsed += Time.deltaTime;
            RemoveDestroyedProjectileReferences();
            yield return null;
        }

        // 剩余阶段先保持花形与伤害，最后 0.5 秒统一播放
        // BarrageProjectile 在碰撞障碍物时使用的消失动画。
        float destroyAnimationDuration = Mathf.Min(
            finalDestroyAnimationDuration,
            usedFadeDuration);
        float flowerHoldDuration = Mathf.Max(
            0f,
            usedFadeDuration - destroyAnimationDuration);

        if (flowerHoldDuration > 0f)
        {
            yield return new WaitForSeconds(flowerHoldDuration);
        }

        AnimateFinalFlowerDespawn(destroyAnimationDuration);
        yield return new WaitForSeconds(destroyAnimationDuration);
        RemoveDestroyedProjectileReferences();
    }

    private List<Vector3> CaptureFamiliarPositions()
    {
        List<Vector3> positions =
            new List<Vector3>(familiarObjects.Count);

        for (int i = 0; i < familiarObjects.Count; i++)
        {
            GameObject familiar = familiarObjects[i];
            positions.Add(
                familiar != null
                    ? familiar.transform.position
                    : (Vector3)ArenaCenter);
        }

        return positions;
    }

    private void UpdateFinalBuildFamiliarVisuals(
        List<Vector3> startPositions,
        float progress)
    {
        float eased = SmoothStep(progress);
        float targetOrbitRadius = Mathf.Lerp(
            Mathf.Min(endingArenaRadius * 0.42f, 6.4f),
            finaleCircleEndRadius * 0.42f,
            eased);

        for (int i = 0; i < familiarObjects.Count; i++)
        {
            GameObject familiar = familiarObjects[i];
            if (familiar == null)
            {
                continue;
            }

            Vector2 startOffset =
                (Vector2)startPositions[i] - ArenaCenter;
            float startRadius = Mathf.Max(
                0.05f,
                startOffset.magnitude);
            float startAngle = startOffset.sqrMagnitude > 0.0001f
                ? Mathf.Atan2(
                    startOffset.y,
                    startOffset.x) * Mathf.Rad2Deg
                : i * (360f / FamiliarCount);

            float direction = i % 2 == 0 ? 1f : -1f;
            float angle = startAngle +
                          direction *
                          (540f + i * 9f) * eased;
            float radius = Mathf.Lerp(
                startRadius,
                targetOrbitRadius,
                eased);

            familiar.transform.position =
                ArenaCenter +
                RotateVector(Vector2.right, angle) * radius;

            Vector3 baseScale = i < familiarBaseScales.Count
                ? familiarBaseScales[i]
                : Vector3.one;
            float pulse = 1f + 0.10f * Mathf.Sin(
                progress * 24f + i * 0.7f);
            familiar.transform.localScale =
                baseScale *
                Mathf.Lerp(1f, 0.58f, eased) *
                pulse;

            SpriteRenderer renderer = i < familiarRenderers.Count
                ? familiarRenderers[i]
                : null;
            if (renderer != null)
            {
                Color color = stardustColors[
                    i % stardustColors.Length];
                color.a = Mathf.Lerp(0.96f, 0.68f, eased);
                renderer.color = color;
            }
        }
    }

    private void CreateFinaleCircle(float shrinkDuration)
    {
        DestroyFinaleCircle();

        GameObject circleObject =
            new GameObject("Stardust Finale Skill Circle");
        circleObject.transform.position = ArenaCenter;

        finaleCircle =
            circleObject.AddComponent<StardustFinaleCircle>();
        finaleCircle.Initialize(
            ArenaCenter,
            endingArenaRadius,
            finaleCircleEndRadius,
            shrinkDuration,
            finaleCircleWidth,
            finaleCircleSegments,
            finaleCircleColor);

        RegisterTemporaryObject(circleObject);
    }

    private void DestroyFinaleCircle()
    {
        if (finaleCircle == null)
        {
            return;
        }

        GameObject circleObject = finaleCircle.gameObject;
        finaleCircle = null;

        if (circleObject != null)
        {
            UnregisterTemporaryObject(circleObject);
            Destroy(circleObject);
        }
    }

    private void RetireCurrentProjectiles()
    {
        RemoveDestroyedProjectileReferences();
        retiredProjectiles.Clear();

        for (int i = 0; i < spawnedProjectiles.Count; i++)
        {
            GameObject projectileObject = spawnedProjectiles[i];
            if (projectileObject == null)
            {
                continue;
            }

            // 先停用渲染、碰撞和 FixedUpdate，随后在 10 秒蓄力期分批销毁。
            // 这样既不会让旧弹幕干扰终幕，也避免同一帧集中 Destroy。
            projectileObject.SetActive(false);
            retiredProjectiles.Add(projectileObject);
        }

        spawnedProjectiles.Clear();
    }

    private void RetireOldProjectilesGradually(
        float deltaTime,
        float remainingTime)
    {
        if (retiredProjectiles.Count <= 0)
        {
            return;
        }

        int batchSize = Mathf.Max(
            1,
            Mathf.CeilToInt(
                retiredProjectiles.Count *
                Mathf.Max(0.001f, deltaTime) /
                Mathf.Max(0.05f, remainingTime)));

        for (int i = 0;
             i < batchSize && retiredProjectiles.Count > 0;
             i++)
        {
            int lastIndex = retiredProjectiles.Count - 1;
            GameObject projectileObject =
                retiredProjectiles[lastIndex];
            retiredProjectiles.RemoveAt(lastIndex);

            if (projectileObject != null)
            {
                Destroy(projectileObject);
            }
        }
    }

    private void DestroyRetiredProjectiles()
    {
        for (int i = retiredProjectiles.Count - 1; i >= 0; i--)
        {
            GameObject projectileObject = retiredProjectiles[i];
            if (projectileObject != null)
            {
                Destroy(projectileObject);
            }
        }

        retiredProjectiles.Clear();
    }

    private void SpawnFinalSeedStar(
        int projectileIndex,
        int targetCount)
    {
        int familiarIndex = projectileIndex % FamiliarCount;
        Vector2 spawnPosition = ArenaCenter;

        if (familiarIndex < familiarObjects.Count &&
            familiarObjects[familiarIndex] != null)
        {
            spawnPosition =
                familiarObjects[familiarIndex].transform.position;
        }

        const float goldenAngle = 137.50776f;
        float normalizedIndex =
            (projectileIndex + 0.5f) /
            Mathf.Max(1f, targetCount);
        float coreRadius =
            finaleCircleEndRadius *
            0.42f *
            Mathf.Sqrt(normalizedIndex);
        float coreAngle = projectileIndex * goldenAngle;
        Vector2 targetOffset =
            RotateVector(Vector2.right, coreAngle) *
            coreRadius;
        Color color = stardustColors[
            projectileIndex % stardustColors.Length];

        GameObject projectileObject = Instantiate(
            projectilePrefab,
            spawnPosition,
            Quaternion.Euler(
                0f,
                0f,
                coreAngle + projectileRotationOffset));

        BarrageProjectile barrage =
            projectileObject.GetComponent<BarrageProjectile>();
        Rigidbody2D body =
            projectileObject.GetComponent<Rigidbody2D>();
        if (barrage == null || body == null)
        {
            Destroy(projectileObject);
            return;
        }

        projectileObject.transform.localScale *=
            projectileScaleMultiplier;

        SpriteRenderer renderer =
            projectileObject.GetComponentInChildren<SpriteRenderer>();
        if (renderer != null)
        {
            renderer.color = color;
        }

        StardustFantasyBullet motion =
            projectileObject.GetComponent<StardustFantasyBullet>();
        if (motion == null)
        {
            motion =
                projectileObject.AddComponent<StardustFantasyBullet>();
        }

        motion.InitializeFinalCore(
            empty,
            ArenaCenter,
            spawnPosition,
            projectileSpriteSpinSpeed *
                (projectileIndex % 2 == 0 ? 1f : -1f),
            color,
            false);

        motion.BeginFinalGather(
            ArenaCenter,
            targetOffset,
            0.72f,
            projectileIndex % 2 == 0 ? 0.42f : -0.42f,
            999f,
            false,
            false);

        spawnedProjectiles.Add(projectileObject);
    }

    private void StartFinalFlower(float bloomDuration)
    {
        RemoveDestroyedProjectileReferences();

        int pointsPerPetal = Mathf.Max(
            1,
            finalFlowerPointsPerPetal);
        int expectedCount =
            finalFlowerPetalCount * pointsPerPetal;
        int usedCount = Mathf.Min(
            expectedCount,
            spawnedProjectiles.Count);

        for (int i = 0; i < usedCount; i++)
        {
            GameObject projectileObject = spawnedProjectiles[i];
            if (projectileObject == null)
            {
                continue;
            }

            StardustFantasyBullet motion =
                projectileObject.GetComponent<StardustFantasyBullet>();
            if (motion == null)
            {
                continue;
            }

            int petalIndex = i / pointsPerPetal;
            int pointIndex = i % pointsPerPetal;
            float u =
                (pointIndex + 0.5f) / pointsPerPetal;

            float petalAngle =
                finalFlowerRotation +
                petalIndex *
                (360f / finalFlowerPetalCount);
            Vector2 axis = RotateVector(
                Vector2.right,
                petalAngle);
            Vector2 tangent = new Vector2(
                -axis.y,
                axis.x);

            float petalLength = GetPetalLength(axis);

            // 单条闭合参数曲线形成一片花瓣：
            // forward 在 u=0/1 回到中心，在 u=0.5 到达花瓣尖端；
            // side 在前后半段分别位于花瓣两侧。
            float forward =
                petalLength *
                Mathf.Sin(Mathf.PI * u);
            float side =
                finalFlowerHalfWidth *
                Mathf.Sin(Mathf.PI * 2f * u);
            Vector2 targetOffset =
                axis * forward +
                tangent * side;

            float launchDelay =
                finalFlowerDepartureSpread * u;
            float usedTravelDuration = Mathf.Max(
                0.1f,
                bloomDuration -
                launchDelay -
                finalFlowerWarningDuration);
            Color color = stardustColors[
                (petalIndex + pointIndex / 3) %
                stardustColors.Length];

            motion.BeginFinalBloomStraight(
                ArenaCenter,
                targetOffset,
                launchDelay,
                finalFlowerWarningDuration,
                usedTravelDuration,
                color);
        }
    }

    private void AnimateFinalFlowerDespawn(float duration)
    {
        RemoveDestroyedProjectileReferences();

        for (int i = 0; i < spawnedProjectiles.Count; i++)
        {
            GameObject projectileObject = spawnedProjectiles[i];
            if (projectileObject == null)
            {
                continue;
            }

            StardustFantasyBullet motion =
                projectileObject.GetComponent<StardustFantasyBullet>();
            if (motion != null)
            {
                motion.BeginAnimatedDespawn(duration);
                continue;
            }

            BarrageProjectile barrage =
                projectileObject.GetComponent<BarrageProjectile>();
            if (barrage != null)
            {
                barrage.SetBehavior(
                    BarrageProjectile.projectileBehavior.Idle);
                barrage.SetSpeed(0f, 0f);
                barrage.FadeMode = 1;
            }

            Collider2D[] hitboxes =
                projectileObject.GetComponentsInChildren<Collider2D>(true);
            for (int colliderIndex = 0;
                 colliderIndex < hitboxes.Length;
                 colliderIndex++)
            {
                hitboxes[colliderIndex].enabled = false;
            }

            Destroy(projectileObject, Mathf.Max(0.05f, duration));
        }
    }

    private float GetPetalLength(Vector2 axis)
    {
        float usableHalfWidth = Mathf.Max(
            3f,
            roomHalfExtents.x - finalFlowerRoomPadding);
        float usableHalfHeight = Mathf.Max(
            3f,
            roomHalfExtents.y - finalFlowerRoomPadding);

        float xLimit = Mathf.Abs(axis.x) > 0.001f
            ? usableHalfWidth / Mathf.Abs(axis.x)
            : float.MaxValue;
        float yLimit = Mathf.Abs(axis.y) > 0.001f
            ? usableHalfHeight / Mathf.Abs(axis.y)
            : float.MaxValue;

        float boundaryLength = Mathf.Min(xLimit, yLimit);
        return Mathf.Max(
            6f,
            boundaryLength * finalFlowerLengthRatio);
    }

    #endregion

    #region 星弹设置

    private void PlaceStardustBeat(
        float elapsed,
        float cycleElapsed,
        float currentArenaRadius,
        float progress,
        PatternStage stage)
    {
        float speedMultiplier = Mathf.Lerp(
            1f,
            endingBulletSpeedMultiplier,
            progress);
        float direction = cycleIndex % 2 == 0 ? 1f : -1f;

        for (int familiarIndex = 0;
             familiarIndex < FamiliarCount;
             familiarIndex++)
        {
            EnsureProjectileCapacity();

            Vector2 spawnPosition = GetFamiliarPosition(
                familiarIndex,
                elapsed,
                cycleElapsed,
                currentArenaRadius);

            Color bulletColor = stardustColors[
                familiarIndex % stardustColors.Length];

            float familiarAngularDirection =
                ((familiarIndex + cycleIndex) % 2 == 0 ? 1f : -1f) *
                direction;

            SpawnPlacedStar(
                spawnPosition,
                currentArenaRadius,
                bulletColor,
                familiarIndex,
                inwardSpeed * speedMultiplier,
                outwardSpeed * speedMultiplier,
                bulletAngularSpeed * familiarAngularDirection,
                stage);
        }
    }

    private void EnsureProjectileCapacity()
    {
        RemoveDestroyedProjectileReferences();

        if (spawnedProjectiles.Count < maximumLiveProjectiles)
        {
            return;
        }

        if (!recycleOldestWhenFull)
        {
            return;
        }

        int removeCount =
            spawnedProjectiles.Count - maximumLiveProjectiles + 1;
        for (int i = 0; i < removeCount; i++)
        {
            if (spawnedProjectiles.Count <= 0)
            {
                break;
            }

            GameObject oldest = spawnedProjectiles[0];
            spawnedProjectiles.RemoveAt(0);
            if (oldest != null)
            {
                Destroy(oldest);
            }
        }
    }

    private void SpawnPlacedStar(
        Vector2 spawnPosition,
        float currentArenaRadius,
        Color bulletColor,
        int familiarIndex,
        float usedInwardSpeed,
        float usedOutwardSpeed,
        float usedAngularSpeed,
        PatternStage stage)
    {
        if (!recycleOldestWhenFull &&
            spawnedProjectiles.Count >= maximumLiveProjectiles)
        {
            return;
        }

        Vector2 fromCenter = spawnPosition - ArenaCenter;
        float startRadius = Mathf.Max(0.1f, fromCenter.magnitude);
        float startAngle = Mathf.Atan2(
            fromCenter.y,
            fromCenter.x) * Mathf.Rad2Deg;

        GameObject projectileObject = Instantiate(
            projectilePrefab,
            spawnPosition,
            Quaternion.Euler(
                0f,
                0f,
                startAngle + projectileRotationOffset));

        BarrageProjectile barrage =
            projectileObject.GetComponent<BarrageProjectile>();
        Rigidbody2D body =
            projectileObject.GetComponent<Rigidbody2D>();

        if (barrage == null || body == null)
        {
            Debug.LogError(
                projectilePrefab.name +
                "：星尘幻想弹幕必须同时带 BarrageProjectile 和 Rigidbody2D。",
                projectileObject);
            Destroy(projectileObject);
            return;
        }

        projectileObject.transform.localScale *= projectileScaleMultiplier;

        SpriteRenderer renderer =
            projectileObject.GetComponentInChildren<SpriteRenderer>();
        if (renderer != null)
        {
            renderer.color = bulletColor;
        }

        StardustFantasyBullet motion =
            projectileObject.GetComponent<StardustFantasyBullet>();
        if (motion == null)
        {
            motion = projectileObject.AddComponent<StardustFantasyBullet>();
        }

        float stageInwardMultiplier =
            stage == PatternStage.Collapse ? 1.18f : 1f;
        float targetRadius = currentArenaRadius + despawnOutsidePadding;

        motion.Initialize(
            empty,
            ArenaCenter,
            startRadius,
            targetRadius,
            startAngle,
            inwardTravelDistance * stageInwardMultiplier,
            usedInwardSpeed,
            usedOutwardSpeed,
            usedAngularSpeed,
            warningDuration,
            placedHoldDuration,
            projectileSpriteSpinSpeed *
                (familiarIndex % 2 == 0 ? 1f : -1f),
            bulletColor);

        spawnedProjectiles.Add(projectileObject);
    }

    private void RemoveDestroyedProjectileReferences()
    {
        for (int i = spawnedProjectiles.Count - 1; i >= 0; i--)
        {
            if (spawnedProjectiles[i] == null)
            {
                spawnedProjectiles.RemoveAt(i);
            }
        }
    }

    private void DestroySpawnedProjectiles()
    {
        for (int i = spawnedProjectiles.Count - 1; i >= 0; i--)
        {
            GameObject projectile = spawnedProjectiles[i];
            if (projectile != null)
            {
                Destroy(projectile);
            }
        }

        spawnedProjectiles.Clear();
    }

    #endregion
}
