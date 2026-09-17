using UnityEngine;

/// <summary>
/// 星轨回收专用弹幕控制器。
/// 预演与闪烁阶段关闭碰撞；返回阶段恢复碰撞。
/// </summary>
[DisallowMultipleComponent]
[RequireComponent(typeof(BarrageProjectile))]
[RequireComponent(typeof(Rigidbody2D))]
public class StarRingReturnBullet : MonoBehaviour
{
    private enum MotionState
    {
        PreviewOut,
        WarningHold,
        Return,
        Finished
    }

    private BarrageProjectile projectile;
    private Rigidbody2D rigidbody2D;

    private Collider2D[] colliders;
    private bool[] colliderDefaultStates;

    private SpriteRenderer[] spriteRenderers;
    private Color[] spriteDefaultColors;

    private MotionState state;
    private bool initialized;
    private float stateTimer;

    private Vector2 startPosition;
    private Vector2 endPosition;
    private Vector2 returnCenter;

    private float previewOutDuration;
    private float warningHoldDuration;
    private float returnDuration;
    private float previewAlpha;
    private float warningBlinkSpeed;
    private float despawnRadius;

    private void Awake()
    {
        projectile = GetComponent<BarrageProjectile>();
        rigidbody2D = GetComponent<Rigidbody2D>();

        colliders = GetComponentsInChildren<Collider2D>(true);
        colliderDefaultStates = new bool[colliders.Length];

        for (int i = 0; i < colliders.Length; i++)
        {
            colliderDefaultStates[i] = colliders[i].enabled;
        }

        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        spriteDefaultColors = new Color[spriteRenderers.Length];

        for (int i = 0; i < spriteRenderers.Length; i++)
        {
            spriteDefaultColors[i] = spriteRenderers[i].color;
        }
    }

    public void Initialize(
        Empty owner,
        Vector2 start,
        Vector2 end,
        Vector2 center,
        float previewDuration,
        float holdDuration,
        float activeReturnDuration,
        float transparentAlpha,
        float blinkSpeed,
        float centerDespawnRadius)
    {
        if (projectile == null)
        {
            projectile = GetComponent<BarrageProjectile>();
        }

        if (rigidbody2D == null)
        {
            rigidbody2D = GetComponent<Rigidbody2D>();
        }

        projectile.empty = owner;
        projectile.SetBehavior(
            BarrageProjectile.projectileBehavior.Idle);

        startPosition = start;
        endPosition = end;
        returnCenter = center;

        previewOutDuration = Mathf.Max(0.05f, previewDuration);
        warningHoldDuration = Mathf.Max(0f, holdDuration);
        returnDuration = Mathf.Max(0.1f, activeReturnDuration);
        previewAlpha = Mathf.Clamp(transparentAlpha, 0.05f, 1f);
        warningBlinkSpeed = Mathf.Max(0.1f, blinkSpeed);
        despawnRadius = Mathf.Max(0.05f, centerDespawnRadius);

        float requiredExistTime =
            previewOutDuration
            + warningHoldDuration
            + returnDuration
            + 1f;

        projectile.ExistTime =
            Mathf.Max(projectile.ExistTime, requiredExistTime);

        state = MotionState.PreviewOut;
        stateTimer = 0f;
        initialized = true;

        SetDamageEnabled(false);
        SetVisualAlpha(previewAlpha);
        MoveTo(startPosition);
    }

    private void FixedUpdate()
    {
        if (!initialized || state == MotionState.Finished)
        {
            return;
        }

        if (projectile == null || projectile.FadeMode != 0)
        {
            SetDamageEnabled(false);
            StopMotion();
            enabled = false;
            return;
        }

        stateTimer += Time.fixedDeltaTime;

        switch (state)
        {
            case MotionState.PreviewOut:
                UpdatePreviewOut();
                break;

            case MotionState.WarningHold:
                UpdateWarningHold();
                break;

            case MotionState.Return:
                UpdateReturn();
                break;
        }
    }

    private void UpdatePreviewOut()
    {
        float normalizedTime =
            Mathf.Clamp01(stateTimer / previewOutDuration);

        float easedTime =
            Mathf.SmoothStep(0f, 1f, normalizedTime);

        MoveTo(Vector2.Lerp(
            startPosition,
            endPosition,
            easedTime));

        SetVisualAlpha(previewAlpha);

        if (stateTimer >= previewOutDuration)
        {
            ChangeState(MotionState.WarningHold);
            MoveTo(endPosition);
        }
    }

    private void UpdateWarningHold()
    {
        MoveTo(endPosition);

        float pulse =
            0.5f + 0.5f
            * Mathf.Sin(stateTimer * warningBlinkSpeed * Mathf.PI * 2f);

        SetVisualAlpha(
            Mathf.Lerp(previewAlpha, 1f, pulse));

        if (stateTimer >= warningHoldDuration)
        {
            SetDamageEnabled(true);
            SetVisualAlpha(1f);
            ChangeState(MotionState.Return);
        }
    }

    private void UpdateReturn()
    {
        float normalizedTime =
            Mathf.Clamp01(stateTimer / returnDuration);

        float easedTime =
            normalizedTime * normalizedTime;

        Vector2 nextPosition = Vector2.Lerp(
            endPosition,
            returnCenter,
            easedTime);

        MoveTo(nextPosition);

        if (Vector2.Distance(
                nextPosition,
                returnCenter) <= despawnRadius
            || stateTimer >= returnDuration)
        {
            Finish();
        }
    }

    private void ChangeState(MotionState newState)
    {
        state = newState;
        stateTimer = 0f;
    }

    private void Finish()
    {
        state = MotionState.Finished;

        SetDamageEnabled(false);
        StopMotion();

        if (projectile != null)
        {
            projectile.SetBehavior(
                BarrageProjectile.projectileBehavior.Idle);
            projectile.FadeMode = 1;
        }

        Destroy(gameObject, 0.5f);
    }

    private void SetDamageEnabled(bool enabledValue)
    {
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i] != null)
            {
                colliders[i].enabled =
                    colliderDefaultStates[i] && enabledValue;
            }
        }
    }

    private void SetVisualAlpha(float alpha)
    {
        for (int i = 0; i < spriteRenderers.Length; i++)
        {
            if (spriteRenderers[i] == null)
            {
                continue;
            }

            Color original = spriteDefaultColors[i];

            spriteRenderers[i].color = new Color(
                original.r,
                original.g,
                original.b,
                original.a * alpha);
        }
    }

    private void MoveTo(Vector2 position)
    {
        if (rigidbody2D != null)
        {
            rigidbody2D.MovePosition(position);
        }
        else
        {
            transform.position = position;
        }
    }

    private void StopMotion()
    {
        if (rigidbody2D != null)
        {
            rigidbody2D.velocity = Vector2.zero;
            rigidbody2D.angularVelocity = 0f;
        }
    }
}
