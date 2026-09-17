using UnityEngine;

/// <summary>
/// 「星尘幻想」专用星弹控制器。
/// BarrageProjectile 继续负责伤害、异常状态和碰撞；
/// 本脚本负责常规星轨、终幕无伤害蓄积、最终八瓣花直线展开，
/// 并在结束时调用 BarrageProjectile 已有的碰撞消失动画。
/// </summary>
[DisallowMultipleComponent]
[RequireComponent(typeof(BarrageProjectile))]
[RequireComponent(typeof(Rigidbody2D))]
public sealed class StardustFantasyBullet : MonoBehaviour
{
    private enum MotionMode
    {
        Standard,
        RotatingFlower,
        FinalGather,
        FinalCore,
        FinalBloom
    }

    private BarrageProjectile projectile;
    private Rigidbody2D body;
    private SpriteRenderer[] renderers;
    private Collider2D[] colliders;
    private bool[] colliderEnabledStates;

    private MotionMode motionMode;
    private Vector2 center;

    // 常规星轨参数。
    public const float FlowerRotationSpeed = 32f;
    private float flowerEpoch;
    private bool flowerInward;
    private const float FlowerTravelSeconds = 4.5f;
    private float startRadius;
    private float turnRadius;
    private float targetRadius;
    private float startAngle;
    private float inwardSpeed;
    private float outwardSpeed;
    private float angularSpeed;
    private float warningDuration;
    private float holdDuration;
    private float inwardDuration;
    private float maximumLifetime;

    // 终幕凝聚参数。
    private float finalModeElapsed;
    private float gatherStartRadius;
    private float gatherTargetRadius;
    private float gatherStartAngle;
    private float gatherAngleDelta;
    private float gatherDuration;
    private float gatherCollisionDelay;
    private bool gatherShouldDamage;
    private bool coreShouldDamage;
    private float coreRadius;
    private float coreAngle;
    private float coreOrbitSpeed;

    // 最终弹幕花参数。使用世界坐标直线插值，避免极坐标插值绕圈。
    private Vector2 bloomStartPosition;
    private Vector2 bloomTargetPosition;
    private float bloomLaunchDelay;
    private float bloomWarningDuration;
    private float bloomTravelDuration;

    private bool animatedDespawnStarted;

    private float spriteSpinSpeed;
    private float elapsed;
    private Color usedColor;
    private bool collisionEnabled;
    private bool initialized;
    private Vector3 initialScale;

    private void Awake()
    {
        CacheComponents();
        initialScale = transform.localScale;
    }

    /// <summary>
    /// 初始化前 50 秒使用的常规星轨运动。
    /// </summary>
    public void Initialize(
        Empty owner,
        Vector2 arenaCenter,
        float initialRadius,
        float usedTargetRadius,
        float initialAngleDegrees,
        float inwardTravelDistance,
        float usedInwardSpeed,
        float usedOutwardSpeed,
        float usedAngularSpeed,
        float usedWarningDuration,
        float usedHoldDuration,
        float usedSpriteSpinSpeed,
        Color color)
    {
        if (!PrepareCommon(owner, color, usedSpriteSpinSpeed))
        {
            return;
        }

        motionMode = MotionMode.Standard;
        center = arenaCenter;
        startRadius = Mathf.Max(0.08f, initialRadius);
        targetRadius = Mathf.Max(startRadius + 0.1f, usedTargetRadius);
        startAngle = initialAngleDegrees;
        inwardSpeed = Mathf.Max(0.1f, usedInwardSpeed);
        outwardSpeed = Mathf.Max(0.1f, usedOutwardSpeed);
        angularSpeed = usedAngularSpeed;
        warningDuration = Mathf.Max(0f, usedWarningDuration);
        holdDuration = Mathf.Max(0f, usedHoldDuration);

        float availableInwardDistance = Mathf.Max(0f, startRadius - 0.82f);
        float usedInwardDistance = Mathf.Min(
            Mathf.Max(0f, inwardTravelDistance),
            availableInwardDistance);
        turnRadius = startRadius - usedInwardDistance;
        inwardDuration = usedInwardDistance / inwardSpeed;

        float outwardDuration =
            Mathf.Max(0f, targetRadius - turnRadius) / outwardSpeed;
        maximumLifetime =
            warningDuration +
            holdDuration +
            inwardDuration +
            outwardDuration +
            0.35f;

        projectile.ExistTime = Mathf.Max(
            projectile.ExistTime,
            maximumLifetime + 0.4f);

        ApplyStandardVisual(0f);
        ApplyStandardPosition(0f, true);
    }

    /// <summary>
    /// 为终幕补充生成的星弹建立中心状态。默认中心星核具有伤害，
    /// 兼容旧调用方式。
    /// </summary>
    public void InitializeFinalCore(
        Empty owner,
        Vector2 arenaCenter,
        Vector2 initialPosition,
        float usedSpriteSpinSpeed,
        Color color)
    {
        InitializeFinalCore(
            owner,
            arenaCenter,
            initialPosition,
            usedSpriteSpinSpeed,
            color,
            true);
    }

    /// <summary>
    /// enableCollisionInCore 为 false 时，蓄力星核仅作为预警视觉，
    /// 不会在中心形成不可见或难以辨认的伤害区。
    /// </summary>
    public void InitializeFinalCore(
        Empty owner,
        Vector2 arenaCenter,
        Vector2 initialPosition,
        float usedSpriteSpinSpeed,
        Color color,
        bool enableCollisionInCore)
    {
        if (!PrepareCommon(owner, color, usedSpriteSpinSpeed))
        {
            return;
        }

        motionMode = MotionMode.FinalCore;
        center = arenaCenter;
        transform.position = initialPosition;

        Vector2 offset = initialPosition - arenaCenter;
        coreRadius = Mathf.Max(0.02f, offset.magnitude);
        coreAngle = offset.sqrMagnitude > 0.0001f
            ? Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg
            : 0f;
        coreOrbitSpeed = 48f;
        coreShouldDamage = enableCollisionInCore;
        gatherShouldDamage = enableCollisionInCore;
        finalModeElapsed = 0f;

        projectile.ExistTime = Mathf.Max(projectile.ExistTime, 999f);
        SetCollisionEnabled(enableCollisionInCore);
        ApplyFinalVisual(1f, 0.55f);
    }

    /// <summary>
    /// 将当前星弹从现有位置卷向中心附近的目标槽位。
    /// 兼容旧调用方式：凝聚与中心星核都保持伤害。
    /// </summary>
    public void BeginFinalGather(
        Vector2 arenaCenter,
        Vector2 targetOffset,
        float usedDuration,
        float signedTurns,
        float collisionDelay)
    {
        BeginFinalGather(
            arenaCenter,
            targetOffset,
            usedDuration,
            signedTurns,
            collisionDelay,
            true,
            true);
    }

    /// <summary>
    /// 可分别控制凝聚途中和中心星核是否具有伤害。
    /// </summary>
    public void BeginFinalGather(
        Vector2 arenaCenter,
        Vector2 targetOffset,
        float usedDuration,
        float signedTurns,
        float collisionDelay,
        bool enableCollisionDuringGather,
        bool enableCollisionInCore)
    {
        if (!initialized)
        {
            return;
        }

        center = arenaCenter;
        Vector2 startOffset = (Vector2)transform.position - center;

        gatherStartRadius = Mathf.Max(0.02f, startOffset.magnitude);
        gatherStartAngle = startOffset.sqrMagnitude > 0.0001f
            ? Mathf.Atan2(startOffset.y, startOffset.x) * Mathf.Rad2Deg
            : 0f;

        gatherTargetRadius = Mathf.Max(0.02f, targetOffset.magnitude);
        float gatherTargetAngle = targetOffset.sqrMagnitude > 0.0001f
            ? Mathf.Atan2(targetOffset.y, targetOffset.x) * Mathf.Rad2Deg
            : gatherStartAngle;

        float shortAngle = Mathf.DeltaAngle(
            gatherStartAngle,
            gatherTargetAngle);
        gatherAngleDelta = shortAngle + signedTurns * 360f;
        gatherDuration = Mathf.Max(0.1f, usedDuration);
        gatherCollisionDelay = Mathf.Max(0f, collisionDelay);
        gatherShouldDamage = enableCollisionDuringGather;
        coreShouldDamage = enableCollisionInCore;

        coreRadius = gatherTargetRadius;
        coreAngle = gatherStartAngle + gatherAngleDelta;
        coreOrbitSpeed = Mathf.Sign(
            Mathf.Approximately(signedTurns, 0f) ? 1f : signedTurns) * 58f;

        finalModeElapsed = 0f;
        motionMode = MotionMode.FinalGather;
        maximumLifetime = 999f;
        projectile.ExistTime = Mathf.Max(projectile.ExistTime, 999f);

        body.velocity = Vector2.zero;
        body.angularVelocity = 0f;
        SetCollisionEnabled(false);
    }

    /// <summary>
    /// 从中心星核沿直线移动到指定花瓣槽位。
    /// 不再通过半径与角度分别插值，因此不会绕大半圈到达目标。
    /// </summary>
    public void BeginFinalBloomStraight(
        Vector2 arenaCenter,
        Vector2 targetOffset,
        float launchDelay,
        float usedWarningDuration,
        float usedTravelDuration,
        Color color)
    {
        if (!initialized)
        {
            return;
        }

        center = arenaCenter;
        usedColor = color;
        bloomStartPosition = transform.position;
        bloomTargetPosition = arenaCenter + targetOffset;
        bloomLaunchDelay = Mathf.Max(0f, launchDelay);
        bloomWarningDuration = Mathf.Max(0f, usedWarningDuration);
        bloomTravelDuration = Mathf.Max(0.1f, usedTravelDuration);

        finalModeElapsed = 0f;
        motionMode = MotionMode.FinalBloom;
        projectile.ExistTime = Mathf.Max(projectile.ExistTime, 999f);
        body.velocity = Vector2.zero;
        body.angularVelocity = 0f;
        SetCollisionEnabled(false);
        ApplyFinalVisual(1f, 0.55f);
    }

    /// <summary>
    /// 复用 BarrageProjectile 撞到障碍物时的消失流程：
    /// 停止移动、设置 FadeMode=1，并在动画结束后销毁。
    /// </summary>
    public void BeginAnimatedDespawn(float duration = 0.5f)
    {
        if (animatedDespawnStarted)
        {
            return;
        }

        CacheComponents();
        animatedDespawnStarted = true;
        SetCollisionEnabled(false);
        initialized = false;

        if (body != null)
        {
            body.velocity = Vector2.zero;
            body.angularVelocity = 0f;
        }

        if (projectile != null)
        {
            projectile.SetBehavior(BarrageProjectile.projectileBehavior.Idle);
            projectile.SetSpeed(0f, 0f);
            projectile.FadeMode = 1;
        }

        Destroy(gameObject, Mathf.Max(0.05f, duration));
    }

    private bool PrepareCommon(
        Empty owner,
        Color color,
        float usedSpriteSpinSpeed)
    {
        CacheComponents();
        if (projectile != null) projectile.ApplyEvolutionScale(owner);
        initialScale = transform.localScale;

        if (projectile == null || body == null)
        {
            Debug.LogError(
                name + "：缺少 BarrageProjectile 或 Rigidbody2D。",
                this);
            Destroy(gameObject);
            return false;
        }

        spriteSpinSpeed = usedSpriteSpinSpeed;
        usedColor = color;
        elapsed = 0f;
        finalModeElapsed = 0f;
        collisionEnabled = false;
        animatedDespawnStarted = false;

        projectile.empty = owner;
        projectile.SetBehavior(BarrageProjectile.projectileBehavior.Idle);
        projectile.SetSpeed(0f, 0f);

        body.velocity = Vector2.zero;
        body.angularVelocity = 0f;
        body.gravityScale = 0f;

        colliderEnabledStates = new bool[colliders.Length];
        for (int i = 0; i < colliders.Length; i++)
        {
            colliderEnabledStates[i] = colliders[i].enabled;
            colliders[i].enabled = false;
        }

        initialized = true;
        return true;
    }

    private void CacheComponents()
    {
        if (projectile == null)
        {
            projectile = GetComponent<BarrageProjectile>();
        }

        if (body == null)
        {
            body = GetComponent<Rigidbody2D>();
        }

        if (renderers == null || renderers.Length == 0)
        {
            renderers = GetComponentsInChildren<SpriteRenderer>(true);
        }

        if (colliders == null)
        {
            colliders = GetComponentsInChildren<Collider2D>(true);
        }
    }

    private void FixedUpdate()
    {
        if (!initialized)
        {
            return;
        }

        float deltaTime = Time.fixedDeltaTime;
        elapsed += deltaTime;

        switch (motionMode)
        {
            case MotionMode.RotatingFlower:
                UpdateRotatingFlower();
                break;
            case MotionMode.Standard:
                UpdateStandard(deltaTime);
                break;

            case MotionMode.FinalGather:
                UpdateFinalGather(deltaTime);
                break;

            case MotionMode.FinalCore:
                UpdateFinalCore(deltaTime);
                break;

            case MotionMode.FinalBloom:
                UpdateFinalBloom(deltaTime);
                break;
        }
    }

    public void InitializeRotatingFlower(Empty owner, Vector2 origin, float radius,
        float petalAngle, bool inward, float epoch, Color tint)
    {
        if (!PrepareCommon(owner, tint, 70f)) return;
        motionMode = MotionMode.RotatingFlower;
        center = origin;
        targetRadius = Mathf.Max(5f, radius);
        startAngle = petalAngle;
        flowerInward = inward;
        flowerEpoch = epoch;
        warningDuration = 0.65f;
        projectile.ExistTime = FlowerTravelSeconds + warningDuration + 1f;
        SetCollisionEnabled(false);
        UpdateRotatingFlower(true);
    }

    private void UpdateRotatingFlower(bool immediate = false)
    {
        float travel = Mathf.Clamp01((elapsed - warningDuration) / FlowerTravelSeconds);
        float radialProgress = flowerInward ? 1f - travel : travel;
        // The two sides meet at the tip and base of each petal. Every star uses one shared rotation clock.
        float radius = Mathf.Lerp(2.4f, targetRadius, radialProgress);
        float side = flowerInward ? -1f : 1f;
        float angle = startAngle + (Time.time - flowerEpoch) * FlowerRotationSpeed +
            side * 16f * Mathf.Sin(radialProgress * Mathf.PI);
        ApplyPolarPosition(radius, angle, immediate);
        SetCollisionEnabled(elapsed >= warningDuration && travel < 0.96f);
        float opacity = Mathf.Clamp01(elapsed / warningDuration) *
            Mathf.Clamp01((1f - travel) / 0.08f);
        // Preserve the prefab size throughout reveal and departure.
        ApplyFinalVisual(1f, opacity);
        if (travel >= 1f) Destroy(gameObject);
    }
    private void UpdateStandard(float deltaTime)
    {
        if (!collisionEnabled && elapsed >= warningDuration)
        {
            SetCollisionEnabled(true);
        }

        float movementTime = Mathf.Max(
            0f,
            elapsed - warningDuration - holdDuration);

        ApplyStandardPosition(movementTime, false);
        ApplyStandardVisual(elapsed);

        float currentRadius = EvaluateStandardRadius(movementTime);
        bool returnedOutward = movementTime >= inwardDuration;
        if ((returnedOutward && currentRadius >= targetRadius) ||
            elapsed >= maximumLifetime)
        {
            Destroy(gameObject);
        }
    }

    private void UpdateFinalGather(float deltaTime)
    {
        finalModeElapsed += deltaTime;

        if (gatherShouldDamage &&
            !collisionEnabled &&
            finalModeElapsed >= gatherCollisionDelay)
        {
            SetCollisionEnabled(true);
        }
        else if (!gatherShouldDamage && collisionEnabled)
        {
            SetCollisionEnabled(false);
        }

        float progress = Mathf.Clamp01(
            finalModeElapsed / gatherDuration);
        float eased = SmoothStep(progress);

        float radius = Mathf.Lerp(
            gatherStartRadius,
            gatherTargetRadius,
            eased);
        float angle = gatherStartAngle + gatherAngleDelta * eased;

        ApplyPolarPosition(radius, angle, false);
        ApplyFinalVisual(
            1f + 0.10f * Mathf.Sin(finalModeElapsed * 12f),
            1f);

        if (progress >= 1f)
        {
            coreRadius = gatherTargetRadius;
            coreAngle = gatherStartAngle + gatherAngleDelta;
            finalModeElapsed = 0f;
            motionMode = MotionMode.FinalCore;
        }
    }

    private void UpdateFinalCore(float deltaTime)
    {
        finalModeElapsed += deltaTime;
        coreAngle += coreOrbitSpeed * deltaTime;
        ApplyPolarPosition(coreRadius, coreAngle, false);

        if (collisionEnabled != coreShouldDamage)
        {
            SetCollisionEnabled(coreShouldDamage);
        }

        ApplyFinalVisual(
            1f + 0.12f * Mathf.Sin(finalModeElapsed * 15f),
            1f);
    }

    private void UpdateFinalBloom(float deltaTime)
    {
        finalModeElapsed += deltaTime;

        float movementStart =
            bloomLaunchDelay + bloomWarningDuration;
        float movementElapsed = finalModeElapsed - movementStart;

        if (movementElapsed < 0f)
        {
            SetCollisionEnabled(false);
            MoveToPosition(bloomStartPosition, false);

            float reveal = movementStart <= 0f
                ? 1f
                : Mathf.Clamp01(finalModeElapsed / movementStart);
            ApplyFinalVisual(
                Mathf.Lerp(0.72f, 1.08f, SmoothStep(reveal)),
                Mathf.Lerp(0.45f, 1f, SmoothStep(reveal)));
            return;
        }

        float progress = Mathf.Clamp01(
            movementElapsed / bloomTravelDuration);
        float eased = 1f - Mathf.Pow(1f - progress, 3f);
        Vector2 position = Vector2.Lerp(
            bloomStartPosition,
            bloomTargetPosition,
            eased);
        MoveToPosition(position, false);

        Vector2 direction = bloomTargetPosition - bloomStartPosition;
        if (direction.sqrMagnitude > 0.0001f)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(
                0f,
                0f,
                angle + spriteSpinSpeed * elapsed);
        }

        if (!collisionEnabled)
        {
            SetCollisionEnabled(true);
        }

        ApplyFinalVisual(
            1f + 0.06f * Mathf.Sin(finalModeElapsed * 10f),
            1f);
    }

    private void MoveToPosition(Vector2 position, bool immediate)
    {
        if (immediate || body == null)
        {
            transform.position = position;
        }
        else
        {
            body.MovePosition(position);
        }
    }

    private float EvaluateStandardRadius(float movementTime)
    {
        if (movementTime <= inwardDuration)
        {
            return Mathf.Max(
                turnRadius,
                startRadius - inwardSpeed * movementTime);
        }

        float outwardTime = movementTime - inwardDuration;
        return turnRadius + outwardSpeed * outwardTime;
    }

    private float EvaluateStandardAngle(float movementTime)
    {
        float inwardAngleTime = Mathf.Min(movementTime, inwardDuration);
        float outwardAngleTime = Mathf.Max(0f, movementTime - inwardDuration);
        return startAngle +
               angularSpeed * inwardAngleTime +
               angularSpeed * 1.24f * outwardAngleTime;
    }

    private void ApplyStandardPosition(
        float movementTime,
        bool immediate)
    {
        float radius = Mathf.Min(
            targetRadius,
            EvaluateStandardRadius(movementTime));
        float angle = EvaluateStandardAngle(movementTime);
        ApplyPolarPosition(radius, angle, immediate);
    }

    private void ApplyPolarPosition(
        float radius,
        float angle,
        bool immediate)
    {
        Vector2 direction =
            Quaternion.Euler(0f, 0f, angle) * Vector2.right;
        Vector2 position = center + direction * radius;

        if (immediate || body == null)
        {
            transform.position = position;
        }
        else
        {
            body.MovePosition(position);
        }

        transform.rotation = Quaternion.Euler(
            0f,
            0f,
            angle + spriteSpinSpeed * elapsed);
    }

    private void ApplyStandardVisual(float time)
    {
        float reveal = warningDuration <= 0f
            ? 1f
            : Mathf.Clamp01(time / warningDuration);
        reveal = SmoothStep(reveal);

        float remaining = maximumLifetime - time;
        float fadeOut = remaining < 0.32f
            ? Mathf.Clamp01(remaining / 0.32f)
            : 1f;

        float alpha = Mathf.Lerp(0.20f, 1f, reveal) * fadeOut;
        float scale = Mathf.Lerp(0.42f, 1f, reveal);
        ApplyFinalVisual(scale, alpha);
    }

    private void ApplyFinalVisual(float scale, float alpha)
    {
        transform.localScale = initialScale * Mathf.Max(0.05f, scale);

        if (renderers == null)
        {
            return;
        }

        for (int i = 0; i < renderers.Length; i++)
        {
            SpriteRenderer renderer = renderers[i];
            if (renderer == null)
            {
                continue;
            }

            Color color = usedColor;
            color.a *= Mathf.Clamp01(alpha);
            renderer.color = color;
        }
    }

    private void SetCollisionEnabled(bool enabled)
    {
        collisionEnabled = enabled;

        if (colliders == null || colliderEnabledStates == null)
        {
            return;
        }

        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i] != null)
            {
                colliders[i].enabled =
                    enabled && colliderEnabledStates[i];
            }
        }
    }

    private static float SmoothStep(float value)
    {
        value = Mathf.Clamp01(value);
        return value * value * (3f - 2f * value);
    }
}
