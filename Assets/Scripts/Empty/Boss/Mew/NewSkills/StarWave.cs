using System.Collections;
using UnityEngine;

/// <summary>
/// 梦幻二阶段强技能④：星辉波动。
///
/// 新版流程：
/// 1. 每次冲刺前重新采样玩家速度并计算预判点；
/// 2. 使用 MewDashArrow 显示“梦幻当前位置 -> 锁定预判点”的箭头；
/// 3. 梦幻高速冲到预判点；
/// 4. 到达预判点后保持原方向继续滑行一小段，并迅速减速至停止；
/// 5. 重复三次，冲刺和刹车阶段持续释放移动水波弹幕。
///
/// predictionLinePrefab 可以直接填入 MewDashArrow.prefab。
/// 直接调用项目中的 SkillArrow.SetTarget，避免根对象与箭头子节点发生双重旋转。
/// </summary>
public sealed class StarWave : MewBaseSkill
{
    public float FinalTrialSlotSeconds => Mathf.Ceil(skillStartup + skillEndup + 1f + postDashObservationDuration +
        Mathf.Max(4, dashCount) * (velocitySampleDuration + predictionWarningDuration + maximumApproachDuration +
        brakeDuration + delayBetweenDashes));
    public void ConfigureFinalTrial()
    {
        waveInterval = Mathf.Max(0.06f, waveInterval * 0.75f);
        dashCount = Mathf.Max(4, dashCount);
    }
    #region Inspector

    [Header("预判箭头")]
    [Tooltip("直接填写 MewDashArrow.prefab。留空时使用运行时 LineRenderer 预警。")]
    [SerializeField] private GameObject predictionLinePrefab;

    [Tooltip("可选的独立预判点 Prefab。MewDashArrow 已有箭头头部时可以留空。")]
    [SerializeField] private GameObject predictionPointPrefab;

    [Tooltip("采样玩家实际位移和刚体速度的时间。每次冲刺都会重新采样。")]
    [SerializeField, Min(0f)] private float velocitySampleDuration = 0.14f;

    [Tooltip("预判玩家多少秒后的坐标。")]
    [SerializeField, Min(0f)] private float predictionLeadTime = 0.52f;

    [Tooltip("限制参与预判的玩家速度，避免击退或瞬移造成异常远预判。")]
    [SerializeField, Min(0.1f)] private float maximumPredictedPlayerSpeed = 11f;

    [Tooltip("预判点与限制圈边缘至少保留的距离。")]
    [SerializeField, Min(0f)] private float predictionEdgePadding = 1.35f;

    [Tooltip("箭头出现后路线锁定的时间。")]
    [SerializeField, Min(0f)] private float predictionWarningDuration = 0.72f;

    [Tooltip("MewDashArrow 的 ArrowScale。")]
    [SerializeField, Min(0.05f)] private float predictionArrowScale = 1.35f;

    [Tooltip("SkillArrow 使用的预警颜色。")]
    [SerializeField] private Color predictionArrowColor =
        new Color(1f, 0.28f, 0.82f, 1f);

    [Tooltip("MewDashArrow 箭头沿本地 Y 轴指向目标，应保持开启。")]
    [SerializeField] private bool predictionSpriteUsesLocalY = true;

    [Tooltip("找不到箭头控制器时，是否由本脚本缩放普通 Sprite 预警图。")]
    [SerializeField] private bool configureFallbackPrefabTransform = true;

    [SerializeField] private bool pulsePredictionWarning = true;
    [SerializeField, Min(0f)] private float predictionPulseSpeed = 8f;
    [SerializeField] private Color fallbackWarningColor =
        new Color(1f, 0.24f, 0.78f, 0.88f);
    [SerializeField] private Color fallbackTargetColor =
        new Color(0.35f, 0.95f, 1f, 0.95f);
    [SerializeField, Min(0.05f)] private float fallbackWarningWidth = 1.2f;
    [SerializeField, Range(16, 96)] private int fallbackTargetSegments = 40;
    [SerializeField] private int warningSortingOrder = 105;

    [Header("三段乱舞冲刺")]
    [SerializeField, Range(1, 6)] private int dashCount = 3;

    [Tooltip("第一次冲刺前，将梦幻放到限制圈外、玩家相反方向。后两次从上一次停止点继续冲刺。")]
    [SerializeField] private bool placeFirstDashOutsideArena = true;

    [SerializeField, Min(0f)] private float firstDashOutsideOffset = 2.8f;

    [Tooltip("冲向预判点的主体速度。主体阶段近似匀速，抵达点位后才开始急减速。")]
    [SerializeField, Min(1f)] private float dashTravelSpeed = 31f;

    [SerializeField, Min(0.05f)] private float minimumApproachDuration = 0.16f;
    [SerializeField, Min(0.05f)] private float maximumApproachDuration = 0.72f;

    [Tooltip("抵达预判点后继续向前滑行的距离。")]
    [SerializeField, Min(0f)] private float brakeDistance = 4.8f;

    [Tooltip("急减速阶段持续时间。")]
    [SerializeField, Min(0.05f)] private float brakeDuration = 0.36f;

    [Tooltip("急减速曲线强度；越大越早完成大部分位移、越快停下。")]
    [SerializeField, Min(1f)] private float brakePower = 2.45f;

    [SerializeField, Min(0f)] private float delayBetweenDashes = 0.32f;

    [Tooltip("当梦幻与预判点过近时，至少保留该距离，避免出现几乎原地的冲刺。")]
    [SerializeField, Min(0f)] private float minimumDashDistance = 3.2f;

    [Tooltip("可复用星之冲刺的 MeleeProjectile Prefab 作为冲刺命中体。")]
    [SerializeField] private GameObject dashHitboxPrefab;
    [SerializeField] private Vector3 dashHitboxOffset = new Vector3(0f, 0.5f, 0f);

    [Tooltip("未配置 dashHitboxPrefab 时使用内置圆形伤害。")]
    [SerializeField] private bool useBuiltInDamageWithoutHitbox = true;
    [SerializeField, Min(0.05f)] private float builtInHitRadius = 0.85f;
    [SerializeField, Min(0f)] private float builtInHitCooldown = 0.28f;
    [SerializeField, Min(0f)] private float builtInSpecialDamage = 115f;
    [SerializeField, Min(0f)] private float builtInKnockback = 3.5f;

    [Header("冲刺残影")]
    [SerializeField] private bool useCasterShadowTrail = true;
    [SerializeField, Min(0.01f)] private float shadowInterval = 0.045f;
    [SerializeField, Min(0.01f)] private float shadowDisappearSpeed = 0.16f;
    [SerializeField] private Color shadowColor =
        new Color(1f, 0.32f, 0.82f, 0.58f);

    [Header("移动水波弹幕")]
    [SerializeField] private GameObject waveProjectilePrefab;
    [SerializeField] private GameObject waveReleaseEffectPrefab;
    [SerializeField, Range(6, 48)] private int projectilesPerWave = 14;
    [SerializeField, Min(0.03f)] private float waveInterval = 0.14f;

    [Tooltip("冲刺前段的水波初速度。")]
    [SerializeField, Min(0f)] private float earlyWaveSpeed = 4.8f;
    [Tooltip("急减速阶段的水波更慢，形成波纹堆叠。")]
    [SerializeField, Min(0f)] private float lateWaveSpeed = 2.35f;
    [SerializeField] private float waveAcceleration = 0.7f;
    [SerializeField] private float waveAngleStep = 11f;
    [SerializeField, Min(0f)] private float waveProjectileLifetime = 7f;
    [SerializeField] private bool spinWaveProjectiles = true;
    [SerializeField] private float waveProjectileSpinSpeed = 135f;

    [Header("结束观察")]
    [SerializeField, Min(0f)] private float postDashObservationDuration = 1.15f;

    #endregion

    #region Runtime

    private struct DashPlan
    {
        public Vector2 Start;
        public Vector2 PredictedPoint;
        public Vector2 BrakeEnd;
        public Vector2 Direction;
        public float ApproachDuration;
    }

    private Rigidbody2D casterBody;
    private Rigidbody2D playerBody;
    private GameObject warningObject;
    private MewDashPreview dashPreview;
    private GameObject dashSigilObject;
    private MewSigilVisual dashSigil;
    private GameObject warningPointObject;
    private GameObject dashHitboxObject;
    private Material fallbackWarningMaterial;
    private float nextBuiltInHitTime;
    private bool shadowRunning;
    private bool contextValid;
    private float waveAngleOffset;

    #endregion

    #region Lifecycle

    private void Reset()
    {
        skillName = "星辉波动";
        skillStartup = 0.1f;
        skillEndup = 0.2f;
        existTime = 0f;
        repeat = 1;
        repeatTime = 0f;
    }

    protected override void OnContextInitialized(MewSkillContext context)
    {
        contextValid =
            empty != null &&
            context.Player != null &&
            context.ArenaRadius > 0.1f;

        casterBody = empty != null
            ? empty.GetComponent<Rigidbody2D>()
            : null;
        playerBody = context.Player != null
            ? context.Player.GetComponent<Rigidbody2D>()
            : null;

        if (!contextValid)
        {
            Debug.LogError(
                name +
                "：星辉波动必须由 Mew 使用带 Player、ArenaCenter、ArenaRadius 的上下文初始化。",
                this);
        }
    }

    public override IEnumerator CoreLogic()
    {
        if (!ValidateConfiguration())
        {
            yield break;
        }

        Transform target = GetPlayerTransform();
        if (target == null || empty == null)
        {
            yield break;
        }

        if (placeFirstDashOutsideArena)
        {
            PlaceCasterAtFirstDashStart(target.position);
        }

        // 旧版 StarWave Prefab 可能仍序列化为 dashCount = 1；新版至少强制执行三次。
        int usedDashCount = Mathf.Max(3, dashCount);

        for (int dashIndex = 0; dashIndex < usedDashCount; dashIndex++)
        {
            target = GetPlayerTransform();
            if (target == null || empty == null)
            {
                yield break;
            }

            Vector2 estimatedVelocity = Vector2.zero;
            yield return EstimatePlayerVelocity(
                target,
                velocitySampleDuration,
                value => estimatedVelocity = value);

            DashPlan plan = BuildDashPlan(
                target,
                estimatedVelocity,
                dashIndex);

            CreatePredictionWarning(plan);
            float warningDuration = Mathf.Max(0.35f, predictionWarningDuration);
            for (float t = 0f; t < warningDuration; t += Time.deltaTime)
            {
                if (target == null || empty == null) yield break;
                if (t < warningDuration - 0.25f) plan = BuildDashPlan(target, estimatedVelocity, dashIndex);
                dashPreview.Show(plan.Start, plan.BrakeEnd, Mathf.Clamp01(t / (warningDuration * 0.65f)), false, fallbackWarningWidth);
                dashSigil.transform.position = plan.PredictedPoint;
                dashSigil.SetCharge(t / warningDuration);
                yield return null;
            }

            CreateDashHitbox();
            StartCasterShadow();
            yield return ExecuteDash(plan);
            DestroyPredictionWarning();
            StopCasterShadow();
            DestroyDashHitbox();

            if (dashIndex < usedDashCount - 1 && delayBetweenDashes > 0f)
            {
                yield return new WaitForSeconds(delayBetweenDashes);
            }
        }

        if (postDashObservationDuration > 0f)
        {
            yield return new WaitForSeconds(postDashObservationDuration);
        }
    }

    protected override void OnSkillFinished(MewSkillFinishReason reason)
    {
        StopCasterShadow();
        DestroyPredictionWarning();
        DestroyDashHitbox();
        ZeroCasterVelocity();
        CleanupTemporaryObjects();
        DestroyFallbackWarningMaterial();
    }

    protected override void OnDestroy()
    {
        StopCasterShadow();
        DestroyFallbackWarningMaterial();
        base.OnDestroy();
    }

    #endregion

    #region Validation and Prediction

    private bool ValidateConfiguration()
    {
        if (!contextValid || empty == null || GetPlayerTransform() == null)
        {
            return false;
        }

        if (waveProjectilePrefab == null)
        {
            Debug.LogError(
                name + "：StarWave 没有设置 waveProjectilePrefab。",
                this);
            return false;
        }

        if (waveProjectilePrefab.GetComponent<BarrageProjectile>() == null)
        {
            Debug.LogError(
                name + "：waveProjectilePrefab 缺少 BarrageProjectile。",
                waveProjectilePrefab);
            return false;
        }

        return true;
    }

    private void PlaceCasterAtFirstDashStart(Vector2 playerPosition)
    {
        Vector2 outward = playerPosition - ArenaCenter;
        if (outward.sqrMagnitude < 0.001f)
        {
            outward = Random.insideUnitCircle;
        }
        if (outward.sqrMagnitude < 0.001f)
        {
            outward = Vector2.right;
        }

        // 放在玩家相反方向，使第一段能形成明显的长距离冲刺。
        Vector2 start = ArenaCenter -
            outward.normalized *
            (ArenaRadius + Mathf.Max(0f, firstDashOutsideOffset));
        MoveCaster(start);
        ZeroCasterVelocity();
    }

    private IEnumerator EstimatePlayerVelocity(
        Transform target,
        float duration,
        System.Action<Vector2> receiveVelocity)
    {
        if (target == null)
        {
            receiveVelocity(Vector2.zero);
            yield break;
        }

        duration = Mathf.Max(0f, duration);
        Vector2 startPosition = target.position;
        Vector2 accumulatedBodyVelocity = Vector2.zero;
        int bodySamples = 0;

        if (duration <= 0.001f)
        {
            Vector2 immediateVelocity = playerBody != null
                ? playerBody.velocity
                : Vector2.zero;
            receiveVelocity(Vector2.ClampMagnitude(
                immediateVelocity,
                maximumPredictedPlayerSpeed));
            yield break;
        }

        float timer = 0f;
        while (timer < duration)
        {
            timer += Time.deltaTime;

            if (playerBody != null)
            {
                accumulatedBodyVelocity += playerBody.velocity;
                bodySamples++;
            }

            yield return null;
        }

        Vector2 observedVelocity =
            ((Vector2)target.position - startPosition) /
            Mathf.Max(0.001f, timer);

        Vector2 bodyVelocity = bodySamples > 0
            ? accumulatedBodyVelocity / bodySamples
            : Vector2.zero;

        Vector2 blendedVelocity = playerBody != null
            ? Vector2.Lerp(observedVelocity, bodyVelocity, 0.35f)
            : observedVelocity;

        receiveVelocity(Vector2.ClampMagnitude(
            blendedVelocity,
            maximumPredictedPlayerSpeed));
    }

    private DashPlan BuildDashPlan(
        Transform target,
        Vector2 estimatedVelocity,
        int dashIndex)
    {
        Vector2 start = empty != null
            ? (Vector2)empty.transform.position
            : ArenaCenter;

        Vector2 playerPosition = target.position;
        Vector2 predictedPoint =
            playerPosition + estimatedVelocity * predictionLeadTime;

        float safePredictionRadius = Mathf.Max(
            0.5f,
            ArenaRadius - predictionEdgePadding);
        predictedPoint = ArenaCenter + Vector2.ClampMagnitude(
            predictedPoint - ArenaCenter,
            safePredictionRadius);

        Vector2 direction = predictedPoint - start;
        float distance = direction.magnitude;

        if (distance < Mathf.Max(0.1f, minimumDashDistance))
        {
            Vector2 fallbackDirection = estimatedVelocity;
            if (fallbackDirection.sqrMagnitude < 0.01f)
            {
                fallbackDirection = playerPosition - ArenaCenter;
            }
            if (fallbackDirection.sqrMagnitude < 0.01f)
            {
                float angle = 90f + dashIndex * 83f;
                fallbackDirection =
                    Quaternion.Euler(0f, 0f, angle) * Vector2.right;
            }

            fallbackDirection.Normalize();
            predictedPoint = start +
                fallbackDirection * Mathf.Max(minimumDashDistance, distance);
            predictedPoint = ArenaCenter + Vector2.ClampMagnitude(
                predictedPoint - ArenaCenter,
                safePredictionRadius);

            direction = predictedPoint - start;
            distance = direction.magnitude;
        }

        if (direction.sqrMagnitude < 0.001f)
        {
            direction = Vector2.right;
            distance = minimumDashDistance;
            predictedPoint = start + direction * distance;
        }
        else
        {
            direction.Normalize();
        }

        float usedTravelSpeed = Mathf.Max(1f, dashTravelSpeed);
        float approachDuration = Mathf.Clamp(
            distance / usedTravelSpeed,
            Mathf.Max(0.05f, minimumApproachDuration),
            Mathf.Max(minimumApproachDuration, maximumApproachDuration));

        DashPlan plan = new DashPlan();
        plan.Start = start;
        plan.PredictedPoint = predictedPoint;
        plan.Direction = direction;
        plan.BrakeEnd = predictedPoint +
            direction * Mathf.Max(0f, brakeDistance);
        plan.ApproachDuration = approachDuration;
        return plan;
    }

    #endregion

    #region Prediction Warning

    private void CreatePredictionWarning(DashPlan plan)
    {
        DestroyPredictionWarning();
        dashPreview = MewDashPreview.Create(predictionLinePrefab);
        warningObject = dashPreview.gameObject;
        RegisterTemporaryObject(warningObject);
        dashPreview.Show(plan.Start, plan.BrakeEnd, 0f, false, fallbackWarningWidth);
        dashSigil = MewSigilVisual.Create(null, plan.PredictedPoint, Mathf.Max(0.85f, builtInHitRadius), MewSigilVisual.Style.Target, warningSortingOrder - 3);
        dashSigilObject = dashSigil.gameObject;
        RegisterTemporaryObject(dashSigilObject);
    }
    /// <summary>
    /// 使用项目原生 SkillArrow 接口配置预警。
    ///
    /// 关键点：
    /// 1. 箭头根对象始终保持 Quaternion.identity；
    /// 2. 不手动写 ArrowAngle；
    /// 3. 只调用 SkillArrow.SetTarget，让 SkillArrow 使用项目统一的
    ///    _mTool.Angle_360Y 角度规则；
    /// 4. 锁定的是本次预判点，不继续跟随玩家。
    ///
    /// 旧版同时旋转根对象并写入已经减过 90 度的 ArrowAngle，
    /// 而 SkillArrow.SetArrow() 内部还会再执行 ArrowAngle - 90，
    /// 因而发生双重旋转。
    /// </summary>
    private bool ConfigureSkillArrow(
        GameObject arrowObject,
        DashPlan plan,
        float warningDuration)
    {
        if (arrowObject == null)
        {
            return false;
        }

        SkillArrow arrow = arrowObject.GetComponent<SkillArrow>();
        if (arrow == null)
        {
            arrow = arrowObject.GetComponentInChildren<SkillArrow>(true);
        }
        if (arrow == null)
        {
            return false;
        }

        arrowObject.transform.position = plan.Start;
        arrowObject.transform.rotation = Quaternion.identity;

        arrow.IsTargetMode = false;
        arrow.targetTransform = null;
        arrow.ArrowScale = Mathf.Max(0.05f, predictionArrowScale);
        arrow.ArrowColor = predictionArrowColor;
        arrow.FadeInTime = Mathf.Min(
            0.1f,
            Mathf.Max(0f, warningDuration) * 0.2f);
        arrow.FadeOutTime = Mathf.Min(
            0.16f,
            Mathf.Max(0f, warningDuration) * 0.25f);

        // 由 StarWave 控制预警销毁时机，防止 SkillArrow 自己提前进入淡出。
        arrow.Duration = -1f;
        arrow.IsOver = false;

        // 必须最后调用，让长度与角度都基于最终起点和锁定点计算。
        arrow.SetTarget(plan.PredictedPoint);
        return true;
    }

    private static void ConfigureOrdinaryPredictionPrefab(
        GameObject target,
        Vector2 start,
        Vector2 end,
        float desiredWidth,
        bool usesLocalY)
    {
        if (target == null)
        {
            return;
        }

        Vector2 direction = end - start;
        float length = Mathf.Max(0.01f, direction.magnitude);
        direction /= length;

        float angle =
            Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        target.transform.position = (start + end) * 0.5f;
        target.transform.rotation = Quaternion.Euler(
            0f,
            0f,
            usesLocalY ? angle - 90f : angle);

        SpriteRenderer spriteRenderer =
            target.GetComponentInChildren<SpriteRenderer>();
        if (spriteRenderer == null || spriteRenderer.sprite == null)
        {
            Vector3 fallbackScale = target.transform.localScale;
            if (usesLocalY)
            {
                fallbackScale.y = length;
                fallbackScale.x = desiredWidth;
            }
            else
            {
                fallbackScale.x = length;
                fallbackScale.y = desiredWidth;
            }
            target.transform.localScale = fallbackScale;
            return;
        }

        Vector2 nativeSize = spriteRenderer.sprite.bounds.size;
        float nativeLength = usesLocalY
            ? Mathf.Max(0.001f, nativeSize.y)
            : Mathf.Max(0.001f, nativeSize.x);
        float nativeWidth = usesLocalY
            ? Mathf.Max(0.001f, nativeSize.x)
            : Mathf.Max(0.001f, nativeSize.y);

        Vector3 scale = target.transform.localScale;
        if (usesLocalY)
        {
            scale.y = length / nativeLength;
            scale.x = desiredWidth / nativeWidth;
        }
        else
        {
            scale.x = length / nativeLength;
            scale.y = desiredWidth / nativeWidth;
        }
        target.transform.localScale = scale;
    }

    private GameObject CreateFallbackPredictionWarning(DashPlan plan)
    {
        EnsureFallbackWarningMaterial();

        GameObject root = new GameObject("Star Wave Prediction Warning");

        LineRenderer path = root.AddComponent<LineRenderer>();
        path.useWorldSpace = true;
        path.positionCount = 2;
        path.SetPosition(0, plan.Start);
        path.SetPosition(1, plan.PredictedPoint);
        path.startWidth = fallbackWarningWidth;
        path.endWidth = fallbackWarningWidth;
        path.numCapVertices = 8;
        path.sharedMaterial = fallbackWarningMaterial;
        path.sortingOrder = warningSortingOrder;
        path.startColor = fallbackWarningColor;
        path.endColor = fallbackWarningColor;

        GameObject targetRing = new GameObject("Predicted Point");
        targetRing.transform.SetParent(root.transform, true);
        targetRing.transform.position = plan.PredictedPoint;

        LineRenderer ring = targetRing.AddComponent<LineRenderer>();
        ring.useWorldSpace = false;
        ring.loop = true;
        ring.positionCount = Mathf.Max(16, fallbackTargetSegments);
        ring.startWidth = 0.14f;
        ring.endWidth = 0.14f;
        ring.numCornerVertices = 4;
        ring.sharedMaterial = fallbackWarningMaterial;
        ring.sortingOrder = warningSortingOrder + 1;
        ring.startColor = fallbackTargetColor;
        ring.endColor = fallbackTargetColor;

        float ringRadius = Mathf.Max(0.45f, fallbackWarningWidth * 0.58f);
        for (int i = 0; i < ring.positionCount; i++)
        {
            float angle = i * Mathf.PI * 2f / ring.positionCount;
            ring.SetPosition(
                i,
                new Vector3(
                    Mathf.Cos(angle) * ringRadius,
                    Mathf.Sin(angle) * ringRadius,
                    0f));
        }

        return root;
    }

    private IEnumerator HoldPredictionWarning(float duration)
    {
        duration = Mathf.Max(0f, duration);
        if (duration <= 0f)
        {
            yield break;
        }

        float timer = 0f;
        while (timer < duration)
        {
            timer += Time.deltaTime;

            if (dashSigil != null) dashSigil.SetCharge(timer / duration);
            if (pulsePredictionWarning)
            {
                float pulse =
                    0.64f +
                    0.36f * Mathf.Abs(
                        Mathf.Sin(timer * predictionPulseSpeed));
                SetPredictionWarningAlpha(pulse);
            }

            yield return null;
        }

        SetPredictionWarningAlpha(1f);
    }

    private void SetPredictionWarningAlpha(float multiplier)
    {
        multiplier = Mathf.Clamp01(multiplier);
        SetObjectAlpha(warningObject, multiplier);
        SetObjectAlpha(warningPointObject, multiplier);
    }

    private static void SetObjectAlpha(GameObject target, float multiplier)
    {
        if (target == null)
        {
            return;
        }

        SpriteRenderer[] sprites =
            target.GetComponentsInChildren<SpriteRenderer>(true);
        for (int i = 0; i < sprites.Length; i++)
        {
            Color color = sprites[i].color;
            color.a = multiplier;
            sprites[i].color = color;
        }

        LineRenderer[] lines =
            target.GetComponentsInChildren<LineRenderer>(true);
        for (int i = 0; i < lines.Length; i++)
        {
            Color start = lines[i].startColor;
            Color end = lines[i].endColor;
            start.a = multiplier;
            end.a = multiplier;
            lines[i].startColor = start;
            lines[i].endColor = end;
        }
    }

    private void DestroyPredictionWarning()
    {
        dashPreview = null;
        DestroyRegisteredObject(ref dashSigilObject);
        dashSigil = null;
        DestroyRegisteredObject(ref warningObject);
        DestroyRegisteredObject(ref warningPointObject);
    }

    private void EnsureFallbackWarningMaterial()
    {
        if (fallbackWarningMaterial != null)
        {
            return;
        }

        Shader shader = Shader.Find("Sprites/Default");
        if (shader == null)
        {
            shader = Shader.Find("Unlit/Color");
        }

        if (shader != null)
        {
            fallbackWarningMaterial = new Material(shader);
            fallbackWarningMaterial.name =
                "Star Wave Runtime Warning Material";
        }
    }

    private void DestroyFallbackWarningMaterial()
    {
        if (fallbackWarningMaterial != null)
        {
            Destroy(fallbackWarningMaterial);
            fallbackWarningMaterial = null;
        }
    }

    #endregion

    #region Dash

    private IEnumerator ExecuteDash(DashPlan plan)
    {
        nextBuiltInHitTime = 0f;
        float waveTimer = 0f;

        // 第一段：高速、近似匀速冲到锁定的预判点。
        float approachTimer = 0f;
        float approachDuration = Mathf.Max(0.05f, plan.ApproachDuration);
        while (approachTimer < approachDuration && empty != null)
        {
            approachTimer += Time.deltaTime;
            float t = Mathf.Clamp01(approachTimer / approachDuration);
            Vector2 position = Vector2.Lerp(
                plan.Start,
                plan.PredictedPoint,
                t);
            MoveCaster(position);

            if (dashPreview != null) dashPreview.Show(position, plan.BrakeEnd, 1f, true, fallbackWarningWidth);

            UpdateWaveEmission(
                ref waveTimer,
                position,
                t * 0.62f,
                plan.Direction);
            TryApplyBuiltInDashDamage(plan.Direction);
            yield return null;
        }

        MoveCaster(plan.PredictedPoint);

        // 第二段：已经到达预判点，继续沿原方向短距离滑行并急速减速。
        float usedBrakeDuration = Mathf.Max(0.05f, brakeDuration);
        float brakeTimer = 0f;
        while (brakeTimer < usedBrakeDuration && empty != null)
        {
            brakeTimer += Time.deltaTime;
            float t = Mathf.Clamp01(brakeTimer / usedBrakeDuration);
            float brakeProgress =
                1f - Mathf.Pow(
                    1f - t,
                    Mathf.Max(1f, brakePower));

            Vector2 position = Vector2.Lerp(
                plan.PredictedPoint,
                plan.BrakeEnd,
                brakeProgress);
            MoveCaster(position);

            if (dashPreview != null) dashPreview.Show(position, plan.BrakeEnd, 1f, true, fallbackWarningWidth);

            UpdateWaveEmission(
                ref waveTimer,
                position,
                Mathf.Lerp(0.62f, 1f, t),
                plan.Direction);
            TryApplyBuiltInDashDamage(plan.Direction);
            yield return null;
        }

        MoveCaster(plan.BrakeEnd);
        ZeroCasterVelocity();
    }

    private void UpdateWaveEmission(
        ref float waveTimer,
        Vector2 position,
        float dashProgress,
        Vector2 dashDirection)
    {
        waveTimer -= Time.deltaTime;
        if (waveTimer > 0f)
        {
            return;
        }

        EmitMovingWave(position, dashProgress, dashDirection);
        waveTimer = Mathf.Max(0.03f, waveInterval);
    }

    private void MoveCaster(Vector2 position)
    {
        if (empty == null)
        {
            return;
        }

        if (casterBody != null)
        {
            casterBody.velocity = Vector2.zero;
            casterBody.position = position;
            return;
        }

        Vector3 current = empty.transform.position;
        empty.transform.position =
            new Vector3(position.x, position.y, current.z);
    }

    private void ZeroCasterVelocity()
    {
        if (casterBody != null)
        {
            casterBody.velocity = Vector2.zero;
            casterBody.angularVelocity = 0f;
        }
    }

    private void CreateDashHitbox()
    {
        DestroyDashHitbox();

        if (dashHitboxPrefab == null || empty == null)
        {
            return;
        }

        dashHitboxObject = Instantiate(
            dashHitboxPrefab,
            empty.transform.position + dashHitboxOffset,
            Quaternion.identity);
        RegisterTemporaryObject(dashHitboxObject);

        MeleeProjectile melee =
            dashHitboxObject.GetComponent<MeleeProjectile>();
        if (melee == null)
        {
            Debug.LogWarning(
                name +
                "：dashHitboxPrefab 没有 MeleeProjectile，将只保留其原有组件。",
                dashHitboxObject);
            return;
        }

        melee.empty = empty;
        melee.TransformOffset = dashHitboxOffset;
        melee.SetBehavior(MeleeProjectile.projectileBehavior.None);
    }

    private void DestroyDashHitbox()
    {
        DestroyRegisteredObject(ref dashHitboxObject);
    }

    private void TryApplyBuiltInDashDamage(Vector2 dashDirection)
    {
        if (!useBuiltInDamageWithoutHitbox ||
            dashHitboxObject != null ||
            empty == null ||
            Time.time < nextBuiltInHitTime)
        {
            return;
        }

        Collider2D[] hits = Physics2D.OverlapCircleAll(
            empty.transform.position + dashHitboxOffset,
            Mathf.Max(0.05f, builtInHitRadius));

        for (int i = 0; i < hits.Length; i++)
        {
            PlayerControler target =
                hits[i].GetComponent<PlayerControler>();
            if (target == null)
            {
                target = hits[i].GetComponentInParent<PlayerControler>();
            }
            if (target == null)
            {
                continue;
            }

            Pokemon.PokemonHpChange(
                empty.gameObject,
                target.gameObject,
                0,
                builtInSpecialDamage,
                0,
                PokemonType.TypeEnum.Psychic);

            Vector2 knockDirection = dashDirection.sqrMagnitude > 0.001f
                ? dashDirection.normalized
                : ((Vector2)target.transform.position -
                   (Vector2)empty.transform.position).normalized;
            target.KnockOutPoint = builtInKnockback;
            target.KnockOutDirection = knockDirection;

            nextBuiltInHitTime =
                Time.time + Mathf.Max(0.02f, builtInHitCooldown);
            break;
        }
    }

    private void StartCasterShadow()
    {
        if (!useCasterShadowTrail ||
            empty == null ||
            empty.emptyShadow == null ||
            shadowRunning)
        {
            return;
        }

        empty.StartShadowCoroutine(
            Mathf.Max(0.01f, shadowInterval),
            Mathf.Max(0.01f, shadowDisappearSpeed),
            shadowColor);
        shadowRunning = true;
    }

    private void StopCasterShadow()
    {
        if (!shadowRunning || empty == null)
        {
            shadowRunning = false;
            return;
        }

        empty.StopShadowCoroutine();
        shadowRunning = false;
    }

    #endregion

    #region Waves

    private void EmitMovingWave(
        Vector2 center,
        float dashProgress,
        Vector2 dashDirection)
    {
        if (waveProjectilePrefab == null || empty == null)
        {
            return;
        }

        if (waveReleaseEffectPrefab != null)
        {
            GameObject effect = Instantiate(
                waveReleaseEffectPrefab,
                center,
                Quaternion.identity);
            Destroy(effect, 1.2f);
        }

        int count = Mathf.Max(6, projectilesPerWave);
        float usedSpeed = Mathf.Lerp(
            earlyWaveSpeed,
            lateWaveSpeed,
            Mathf.Clamp01(dashProgress));

        float dashAngle = dashDirection.sqrMagnitude > 0.001f
            ? Mathf.Atan2(dashDirection.y, dashDirection.x) * Mathf.Rad2Deg
            : 0f;

        float startAngle = waveAngleOffset + dashAngle * 0.08f;

        for (int i = 0; i < count; i++)
        {
            float angle = startAngle + i * 360f / count;
            Vector2 direction =
                Quaternion.Euler(0f, 0f, angle) * Vector2.right;

            GameObject projectile = MewStarPool.Spawn(
                waveProjectilePrefab,
                center,
                Quaternion.identity, empty);

            BarrageProjectile barrage =
                projectile.GetComponent<BarrageProjectile>();
            if (barrage == null)
            {
                Destroy(projectile);
                continue;
            }

            barrage.empty = empty;
            barrage.ExistTime = Mathf.Max(0.2f, waveProjectileLifetime);
            barrage.SetBehavior(
                BarrageProjectile.projectileBehavior.Straight);
            barrage.SetDirection(direction);
            barrage.SetSpeed(usedSpeed, waveAcceleration);
            barrage.IsSpin = spinWaveProjectiles;
            barrage.SpinSpeed =
                waveProjectileSpinSpeed * (i % 2 == 0 ? 1f : -1f);
        }

        waveAngleOffset = Mathf.Repeat(
            waveAngleOffset + waveAngleStep,
            360f);
    }

    #endregion

    #region Object Cleanup

    private void DestroyRegisteredObject(ref GameObject target)
    {
        if (target == null)
        {
            return;
        }

        UnregisterTemporaryObject(target);
        Destroy(target);
        target = null;
    }

    #endregion
}
