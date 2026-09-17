using UnityEngine;

/// <summary>
/// 拟态群星专用轨迹组件。
/// BarrageProjectile 继续负责伤害、碰撞和淡出；本组件只负责组阵、顶点穿心、同步切点与散开。
/// </summary>
[DisallowMultipleComponent]
[RequireComponent(typeof(BarrageProjectile))]
[RequireComponent(typeof(Rigidbody2D))]
public sealed class MimicStarBullet : MonoBehaviour
{
    private enum MotionMode
    {
        Forming,
        Held,
        Vertex,
        TangentApproach,
        Scatter,
        Finished
    }

    private BarrageProjectile projectile;
    private Rigidbody2D body;
    private Collider2D[] hitColliders;

    private MotionMode mode = MotionMode.Forming;
    private bool initialized;
    private bool attackLaunched;

    private Vector2 formationStartPosition;
    private Vector2 formationTarget;
    private float formationTravelDuration;
    private float formationTimer;

    private Vector2 center;
    private float centerRadius;
    private float centerExitPadding;

    private Vector2 direction;
    private float speed;
    private float acceleration;

    private Vector2 startPosition;
    private Vector2 tangentPoint;
    private float tangentDistance;
    private float tangentTravelDuration;
    private float tangentTimer;
    private float scatterAcceleration;

    private float lifeTimer;
    private float maximumLifetime;
    private bool enteredCenterCircle;

    public bool HasExitedCenterCircle { get; private set; }
    public bool HasLaunched => attackLaunched;
    public bool IsFormationReady => initialized && mode == MotionMode.Held;

    private void Awake()
    {
        projectile = GetComponent<BarrageProjectile>();
        body = GetComponent<Rigidbody2D>();
        hitColliders = GetComponentsInChildren<Collider2D>(true);
    }

    public void ResetForReuse()
    {
        initialized = attackLaunched = enteredCenterCircle = false;
        HasExitedCenterCircle = false;
        mode = MotionMode.Forming;
        formationTimer = tangentTimer = lifeTimer = 0f;
        speed = acceleration = 0f;
    }

    public void PrepareFormation(
        Empty owner,
        Vector2 target,
        float travelDuration,
        float reservedLifetime)
    {
        if (!PrepareCommon(owner, reservedLifetime))
        {
            return;
        }

        formationStartPosition = CurrentPosition;
        formationTarget = target;
        formationTravelDuration = Mathf.Max(0.05f, travelDuration);
        formationTimer = 0f;
        mode = MotionMode.Forming;
        attackLaunched = false;
        SetCollisionEnabled(false);
    }

    public void SetFormationTarget(Vector2 target, float facingAngleDegrees)
    {
        if (!initialized || attackLaunched || mode == MotionMode.Finished)
        {
            return;
        }

        formationTarget = target;
        transform.rotation = Quaternion.Euler(0f, 0f, facingAngleDegrees);
    }

    public void LaunchVertex(
        Vector2 usedCenter,
        float usedCenterRadius,
        Vector2 inwardDirection,
        float startSpeed,
        float usedAcceleration,
        float exitPadding,
        float lifetime)
    {
        if (!initialized || attackLaunched || mode == MotionMode.Finished)
        {
            return;
        }

        center = usedCenter;
        centerRadius = Mathf.Max(0.05f, usedCenterRadius);
        centerExitPadding = Mathf.Max(0.01f, exitPadding);
        direction = inwardDirection.sqrMagnitude > 0.0001f
            ? inwardDirection.normalized
            : Vector2.down;
        speed = Mathf.Max(0f, startSpeed);
        acceleration = Mathf.Max(0f, usedAcceleration);

        lifeTimer = 0f;
        maximumLifetime = Mathf.Max(0.5f, lifetime);
        enteredCenterCircle = false;
        HasExitedCenterCircle = false;
        attackLaunched = true;
        mode = MotionMode.Vertex;
        SetCollisionEnabled(true);
        SetFacing(direction);
    }

    public void LaunchTangent(
        Vector2 usedCenter,
        Vector2 usedTangentPoint,
        float travelDuration,
        float usedScatterAcceleration,
        float lifetime)
    {
        if (!initialized || attackLaunched || mode == MotionMode.Finished)
        {
            return;
        }

        center = usedCenter;
        startPosition = CurrentPosition;
        tangentPoint = usedTangentPoint;

        Vector2 path = tangentPoint - startPosition;
        tangentDistance = path.magnitude;
        direction = path.sqrMagnitude > 0.0001f
            ? path.normalized
            : Vector2.right;

        tangentTravelDuration = Mathf.Max(0.1f, travelDuration);
        scatterAcceleration = Mathf.Max(0f, usedScatterAcceleration);
        tangentTimer = 0f;
        lifeTimer = 0f;
        maximumLifetime = Mathf.Max(0.5f, lifetime);
        attackLaunched = true;
        mode = MotionMode.TangentApproach;
        SetCollisionEnabled(true);
        SetFacing(direction);
    }

    private bool PrepareCommon(Empty owner, float reservedLifetime)
    {
        ResetForReuse();
        enabled = true;
        if (projectile == null)
        {
            projectile = GetComponent<BarrageProjectile>();
        }

        if (body == null)
        {
            body = GetComponent<Rigidbody2D>();
        }

        if (hitColliders == null || hitColliders.Length == 0)
        {
            hitColliders = GetComponentsInChildren<Collider2D>(true);
        }

        if (projectile == null || body == null)
        {
            Debug.LogError(
                name + "：MimicStarBullet 需要 BarrageProjectile 和 Rigidbody2D。",
                this);
            initialized = false;
            return false;
        }

        projectile.empty = owner;
        projectile.SetBehavior(BarrageProjectile.projectileBehavior.Idle);
        projectile.ExistTime = Mathf.Max(
            projectile.ExistTime,
            Mathf.Max(1f, reservedLifetime));

        body.velocity = Vector2.zero;
        body.angularVelocity = 0f;
        // Formation is harmless: move the visual without maintaining a simulated body for each star.
        body.simulated = false;

        initialized = true;
        mode = MotionMode.Forming;
        attackLaunched = false;
        lifeTimer = 0f;
        SetCollisionEnabled(false);
        return true;
    }

    private void FixedUpdate()
    {
        if (!initialized || mode == MotionMode.Finished)
        {
            return;
        }

        if (projectile == null || projectile.FadeMode != 0)
        {
            StopWithoutFade();
            return;
        }

        float dt = Time.fixedDeltaTime;

        if (!attackLaunched)
        {
            switch (mode)
            {
                case MotionMode.Forming:
                    UpdateFormation(dt);
                    break;
                case MotionMode.Held:
                    SetVelocity(Vector2.zero);
                    MoveTo(formationTarget);
                    break;
            }
            return;
        }

        lifeTimer += dt;
        if (lifeTimer >= maximumLifetime)
        {
            FadeOut();
            return;
        }

        switch (mode)
        {
            case MotionMode.Vertex:
                UpdateVertex(dt);
                break;
            case MotionMode.TangentApproach:
                UpdateTangentApproach(dt);
                break;
            case MotionMode.Scatter:
                UpdateScatter(dt);
                break;
        }
    }

    private void UpdateFormation(float dt)
    {
        SetVelocity(Vector2.zero);
        formationTimer += dt;
        float t = Mathf.Clamp01(formationTimer / formationTravelDuration);
        float smooth = t * t * (3f - 2f * t);
        MoveTo(Vector2.Lerp(formationStartPosition, formationTarget, smooth));

        if (t >= 1f)
        {
            MoveTo(formationTarget);
            mode = MotionMode.Held;
        }
    }

    private void UpdateVertex(float dt)
    {
        speed += acceleration * dt;
        SetVelocity(direction * speed);
        SetFacing(direction);

        Vector2 relative = CurrentPosition - center;
        float distance = relative.magnitude;

        if (!enteredCenterCircle && distance <= centerRadius)
        {
            enteredCenterCircle = true;
        }

        if (enteredCenterCircle &&
            !HasExitedCenterCircle &&
            distance >= centerRadius + centerExitPadding &&
            Vector2.Dot(relative, direction) > 0f)
        {
            HasExitedCenterCircle = true;
        }
    }

    private void UpdateTangentApproach(float dt)
    {
        tangentTimer += dt;
        float t = Mathf.Clamp01(tangentTimer / tangentTravelDuration);

        // 所有弹幕使用同一个持续时间，并用 t² 从近似静止开始加速，确保同时经过切点。
        float acceleratedT = t * t;
        Vector2 previous = CurrentPosition;
        Vector2 next = Vector2.Lerp(startPosition, tangentPoint, acceleratedT);
        MoveTo(next);

        Vector2 frameDirection = next - previous;
        if (frameDirection.sqrMagnitude > 0.000001f)
        {
            SetFacing(frameDirection.normalized);
        }

        if (t < 1f)
        {
            return;
        }

        MoveTo(tangentPoint);
        speed = tangentDistance <= 0.001f
            ? 0f
            : 2f * tangentDistance / tangentTravelDuration;
        mode = MotionMode.Scatter;
        SetVelocity(direction * speed);
        SetFacing(direction);
    }

    private void UpdateScatter(float dt)
    {
        speed += scatterAcceleration * dt;
        SetVelocity(direction * speed);
        SetFacing(direction);
    }

    private Vector2 CurrentPosition
    {
        get
        {
            return body != null && body.simulated
                ? body.position
                : (Vector2)transform.position;
        }
    }

    private void MoveTo(Vector2 position)
    {
        if (body != null && body.simulated)
        {
            body.MovePosition(position);
        }
        else
        {
            transform.position = new Vector3(
                position.x,
                position.y,
                transform.position.z);
        }
    }

    private void SetVelocity(Vector2 velocity)
    {
        if (body != null)
        {
            body.velocity = velocity;
        }
    }

    private void SetFacing(Vector2 usedDirection)
    {
        if (usedDirection.sqrMagnitude < 0.0001f)
        {
            return;
        }

        float angle = Mathf.Atan2(usedDirection.y, usedDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    private void SetCollisionEnabled(bool enabledState)
    {
        if (enabledState && body != null && !body.simulated)
        {
            body.position = transform.position;
            body.simulated = true;
        }
        if (hitColliders == null)
        {
            return;
        }

        for (int i = 0; i < hitColliders.Length; i++)
        {
            Collider2D target = hitColliders[i];
            if (target != null)
            {
                target.enabled = enabledState;
            }
        }
    }

    private void FadeOut()
    {
        mode = MotionMode.Finished;
        SetVelocity(Vector2.zero);
        SetCollisionEnabled(false);

        if (projectile != null)
        {
            projectile.SetBehavior(BarrageProjectile.projectileBehavior.Idle);
            projectile.FadeMode = 1;
        }

        if (projectile != null) projectile.ScheduleDespawn(0.55f);
        else Destroy(gameObject, 0.55f);
    }

    private void StopWithoutFade()
    {
        mode = MotionMode.Finished;
        SetVelocity(Vector2.zero);
        SetCollisionEnabled(false);
        enabled = false;
    }
}
