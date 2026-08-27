using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

/// <summary>
/// 梦幻近战/冲刺命中对象。
/// 冲刺开始时锁定目标位置，移动对象同时带动 Empty 本体。
/// </summary>
public class MeleeProjectile : Projectile
{
    public enum projectileBehavior
    {
        None,
        Straight,
        Curse,
        TargetStraight
    }

    public enum MeleeWay
    {
        Linear,
        Quadratic,
        Cubic,
        Exponential,
        Sine,
        Tangent,
        TangentHyperbola
    }

    public enum ProjectileEffect
    {
        None,
        Freeze,
        Toxic,
        Paralysis,
        Burn,
        Sleep,
        Confusion
    }

    [System.Serializable]
    public struct EffectData
    {
        public ProjectileEffect projectileEffect;
        public float2 frozenPoint;
        public float toxicPoint;
        public float paralysisPoint;
        public float burnPoint;
        public float sleepPoint;
        public float confusionPoint;
    }

    [Header("基础设置")]
    public float KnockPoint;
    public float HitRadius = 1f;

    [Header("基础参数设置")]
    public float ExistTime = 7f;
    [Tooltip("默认关闭。星之冲刺会自行管理冲刺对象生命周期；旧 PounceMew 的 ExistTime=0.7 小于前摇 0.8。")]
    public bool AutoDestroyByExistTime;
    public List<EffectData> effects = new List<EffectData>();
    public Vector3 TransformOffset = new Vector3(0f, 0.5f, 0f);

    [Header("移动设置")]
    public projectileBehavior moveBehavior = projectileBehavior.Straight;
    public float moveTime = 1f;
    public Vector2 direction = Vector2.right;

    [Header("特殊移动设置")]
    public float HitInterval = 0.5f;
    public MeleeWay meleeWay = MeleeWay.Sine;
    public Transform Target;
    public float SurpassRate;
    public LayerMask obstacleLayerMask;

    public bool IsMovementFinished { get; private set; }

    private float hitTimer;
    private float moveTimer;
    private bool hasLockedPosition;
    private Vector2 endPosition;
    private Vector2 startPosition;

    private void Start()
    {
        hitTimer = 0f;
        moveTimer = 0f;
        hasLockedPosition = false;
        IsMovementFinished = false;
        if (AutoDestroyByExistTime && ExistTime > 0f)
        {
            Destroy(gameObject, ExistTime);
        }
    }

    private void Update()
    {
        hitTimer += Time.deltaTime;
        UpdateMovement();
        CheckHit();
    }

    protected virtual void CheckHit()
    {
        if (hitTimer < Mathf.Max(0.01f, HitInterval) || empty == null)
        {
            return;
        }

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, HitRadius);
        for (int i = 0; i < hits.Length; i++)
        {
            Collider2D hit = hits[i];
            if (!hit.CompareTag("Player"))
            {
                continue;
            }

            PlayerControler playerControler = hit.GetComponent<PlayerControler>();
            Pokemon.PokemonHpChange(
                empty.gameObject,
                hit.gameObject,
                0,
                SpDmage,
                0,
                ProType);

            hitTimer = 0f;

            if (playerControler == null)
            {
                continue;
            }

            playerControler.KnockOutPoint = KnockPoint;
            playerControler.KnockOutDirection =
                (playerControler.transform.position - transform.position).normalized;

            if (effects == null)
            {
                continue;
            }

            for (int effectIndex = 0; effectIndex < effects.Count; effectIndex++)
            {
                EffectData effect = effects[effectIndex];
                switch (effect.projectileEffect)
                {
                    case ProjectileEffect.Freeze:
                        playerControler.PlayerFrozenFloatPlus(
                            effect.frozenPoint.x,
                            effect.frozenPoint.y);
                        break;
                    case ProjectileEffect.Toxic:
                        playerControler.ToxicFloatPlus(effect.toxicPoint);
                        break;
                    case ProjectileEffect.Paralysis:
                        playerControler.ParalysisFloatPlus(effect.paralysisPoint);
                        break;
                    case ProjectileEffect.Burn:
                        playerControler.BurnFloatPlus(effect.burnPoint);
                        break;
                    case ProjectileEffect.Sleep:
                        playerControler.SleepFloatPlus(effect.sleepPoint);
                        break;
                    case ProjectileEffect.Confusion:
                        playerControler.ConfusionFloatPlus(effect.confusionPoint);
                        break;
                }
            }
        }
    }

    private void UpdateMovement()
    {
        switch (moveBehavior)
        {
            case projectileBehavior.None:
                MoveNone();
                break;
            case projectileBehavior.Straight:
                MoveStraight();
                break;
            case projectileBehavior.Curse:
                MoveCurse();
                break;
            case projectileBehavior.TargetStraight:
                MoveTargetStraight();
                break;
        }
    }

    private void MoveNone()
    {
        if (empty != null)
        {
            transform.position = empty.transform.position + TransformOffset;
        }
    }

    private void MoveStraight()
    {
        transform.position += (Vector3)(direction.normalized * Time.deltaTime);
    }

    private void MoveCurse()
    {
        // 保留入口，当前梦幻第一阶段未使用。
    }

    private void MoveTargetStraight()
    {
        if (!hasLockedPosition)
        {
            if (Target == null || empty == null)
            {
                return;
            }

            LockDashPosition();
        }

        if (IsMovementFinished)
        {
            return;
        }

        moveTimer += Time.deltaTime;
        float normalizedTime = Mathf.Clamp01(moveTimer / Mathf.Max(0.01f, moveTime));
        float evaluatedTime = EvaluateMovement(normalizedTime);
        Vector2 nextPosition = Vector2.Lerp(startPosition, endPosition, evaluatedTime);

        transform.position = nextPosition;

        if (empty != null)
        {
            empty.transform.position = transform.position - TransformOffset;
        }

        if (normalizedTime >= 1f)
        {
            IsMovementFinished = true;
        }
    }

    private float EvaluateMovement(float normalizedTime)
    {
        switch (meleeWay)
        {
            case MeleeWay.Quadratic:
                return normalizedTime * normalizedTime;
            case MeleeWay.Cubic:
                return normalizedTime * normalizedTime * normalizedTime;
            case MeleeWay.Exponential:
                return Mathf.Pow(2f, normalizedTime) - 1f;
            case MeleeWay.Sine:
                return Mathf.Sin(normalizedTime * Mathf.PI * 0.5f);
            case MeleeWay.Tangent:
                return Mathf.Tan(normalizedTime * Mathf.PI * 0.25f);
            case MeleeWay.TangentHyperbola:
                return (float)System.Math.Tanh(normalizedTime * 3f) /
                       (float)System.Math.Tanh(3f);
            default:
                return normalizedTime;
        }
    }

    private void LockDashPosition()
    {
        transform.position = empty.transform.position + TransformOffset;
        startPosition = transform.position;

        Vector2 targetWithOffset = (Vector2)Target.position + (Vector2)TransformOffset;
        direction = targetWithOffset - startPosition;
        if (direction.sqrMagnitude < 0.0001f)
        {
            direction = Vector2.right;
        }
        direction.Normalize();

        Vector2 desiredEndPosition = targetWithOffset + direction * SurpassRate;
        float castDistance = Vector2.Distance(startPosition, desiredEndPosition);

        RaycastHit2D hit = Physics2D.Raycast(
            startPosition,
            direction,
            castDistance,
            obstacleLayerMask);

        endPosition = hit.collider != null
            ? hit.point - direction * 0.3f
            : desiredEndPosition;

        moveTimer = 0f;
        hasLockedPosition = true;
        IsMovementFinished = false;
    }

    public void SetBehavior(
        projectileBehavior behavior,
        MeleeWay movementWay = MeleeWay.Linear)
    {
        moveBehavior = behavior;
        meleeWay = movementWay;
    }

    public void SetDirection(Vector2 newDirection)
    {
        if (newDirection.sqrMagnitude > 0.0001f)
        {
            direction = newDirection.normalized;
        }
    }

    public void SetTime(float movementTime)
    {
        moveTime = Mathf.Max(0.01f, movementTime);
    }

    public void SetTarget(Transform target)
    {
        Target = target;
    }

    public void ResetAttack()
    {
        moveTimer = 0f;
        hasLockedPosition = false;
        IsMovementFinished = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, HitRadius);
    }
}
