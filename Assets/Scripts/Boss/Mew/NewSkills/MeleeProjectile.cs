using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

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
    public float HitRadius;

    [Header("基础参数设置")]
    public float ExistTime = 7f;
    public List<EffectData> effects = new List<EffectData>();
    public Vector3 TransformOffset = new Vector3(0, 0.5f, 0);

    [Header("移动设置")]
    public projectileBehavior moveBehavior = projectileBehavior.Straight;
    public float moveTime = 1f;
    public Vector2 direction = Vector2.right;

    [Header("特殊移动设置")]
    public float HitInterval;
    public MeleeWay meleeWay = MeleeWay.Sine;
    public Transform Target;
    public float SurpassRate = 0f;//冲刺目标玩家会超越一定距离，根据速度与超越比例计算超越距离
    public LayerMask obstacleLayerMask;

    private float timer;
    private float MoveTimer = 0f;
    
    private bool IsSetPosition = false;
    private Vector2 endPosition;
    private Vector2 startPosition;

    private void Start()
    {
        MoveTimer = 0f;
        timer = 0f;
        IsSetPosition = false;
    }

    private void Update()
    {
        timer += Time.deltaTime;
        ChargeMovement();
        CheckHit();
    }

    protected virtual void CheckHit()
    {
        if(timer > HitInterval)
        {
            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, HitRadius);
            foreach (var hit in hits)
            {
                if (hit.CompareTag("Player"))
                {
                    PlayerControler playerControler = hit.GetComponent<PlayerControler>();
                    Pokemon.PokemonHpChange(empty.gameObject, hit.gameObject, 0, SpDmage, 0, ProType);
                    timer = 0f;
                    if (playerControler != null)
                    {
                        playerControler.KnockOutPoint = KnockPoint;
                        playerControler.KnockOutDirection = (playerControler.transform.position - transform.position).normalized;
                        foreach (var effect in effects)
                        {
                            switch (effect.projectileEffect)
                            {
                                case ProjectileEffect.None:
                                    break;
                                case ProjectileEffect.Freeze:
                                    playerControler.PlayerFrozenFloatPlus(effect.frozenPoint.x, effect.frozenPoint.y);
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
            }
        }

    }

    private void ChargeMovement()
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
        transform.position = empty.transform.position + TransformOffset;
    }

    private void MoveStraight()
    {

    }

    private void MoveCurse()
    {

    }

    private void MoveTargetStraight()
    {
        float t;
        if (Target != null && empty != null && !IsSetPosition)
        {
            setPosition();
            t = 0;
        }
        else if (IsSetPosition)
        {
            if(MoveTimer < moveTime)
            {
                MoveTimer += Time.deltaTime;
            }

            float normalizedTime = Mathf.Clamp01(MoveTimer / moveTime);

            switch (meleeWay)
            {
                case MeleeWay.Linear:
                    t = normalizedTime;
                    transform.position = Vector2.Lerp(startPosition, endPosition, t);
                    break;
                case MeleeWay.Quadratic:
                    t = normalizedTime * normalizedTime; // y = x^2
                    transform.position = Vector2.Lerp(startPosition, endPosition, t);
                    break;
                case MeleeWay.Cubic:
                    t = normalizedTime * normalizedTime * normalizedTime; // y = x^3
                    transform.position = Vector2.Lerp(startPosition, endPosition, t);
                    break;
                case MeleeWay.Exponential:
                    t = (Mathf.Pow(2, normalizedTime) - 1) / 1f; // 2^1 - 1 = 1，所以分母为1
                    transform.position = Vector2.Lerp(startPosition, endPosition, t);
                    break;
                case MeleeWay.Sine:
                    t = Mathf.Sin(MoveTimer * Mathf.PI / (2f * moveTime));
                    transform.position = Vector2.Lerp(startPosition, endPosition, t);
                    break;
                case MeleeWay.Tangent:
                    float maxTan = Mathf.Tan(Mathf.PI / 4f); // tan(π/4) = 1
                    t = Mathf.Tan(normalizedTime * Mathf.PI / 4f) / maxTan;
                    transform.position = Vector2.Lerp(startPosition, endPosition, t);
                    break;
                case MeleeWay.TangentHyperbola:
                    t = (float)System.Math.Tanh(normalizedTime * 3f); // 乘以3是为了让曲线在[0,1]区间内更明显
                    transform.position = Vector2.Lerp(startPosition, endPosition, t);
                    break;

            }

            if(MoveTimer <= moveTime)
            {
                empty.transform.position = transform.position - TransformOffset;
            }
        }
    }

    private void setPosition()
    {
        transform.position = empty.transform.position + TransformOffset;
        direction = (Target.position - transform.position).normalized;
        Vector2 desiredEndPosition = (Vector2)(Target.position + TransformOffset) + SurpassRate * direction;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction,
        Vector2.Distance(transform.position, desiredEndPosition), obstacleLayerMask);

        if (hit.collider != null)
        {
            endPosition = hit.point - (direction * 0.3f);
        }
        else
        {
            endPosition = desiredEndPosition;
        }

        startPosition = transform.position;
        IsSetPosition = true;
    }

    /// <summary>
    /// 设置冲刺模式
    /// </summary>
    /// <param name="behavior">冲刺行为</param>
    /// <param name="meleeWay">冲刺方式</param>
    public void SetBehavior(projectileBehavior behavior, MeleeWay meleeWay = MeleeWay.Linear)
    {
        this.moveBehavior = behavior;
        this.meleeWay = meleeWay;
    }
    public void SetDirection(Vector2 diretion)
    {
        this.direction = diretion;
    }

    public void SetTime(float movetime)
    {
        this.moveTime = movetime;
    }

    public void SetTarget(Transform target)
    {
        this.Target = target;
    }

    public void ResetAttack()
    {
        this.MoveTimer = 0f;
    }

}
