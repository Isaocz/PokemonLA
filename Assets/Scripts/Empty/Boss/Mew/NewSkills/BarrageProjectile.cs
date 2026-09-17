using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

/// <summary>
/// 梦幻弹幕通用运动与命中组件。
/// 保留原项目公开字段与方法，修正初始化、空引用、旋转帧率相关和渐隐计时问题。
/// </summary>
public class BarrageProjectile : Projectile
{
    public enum projectileBehavior
    {
        Idle,
        Straight,
        Target,
        Spiral,
        CloseTarget
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

    [Header("弹幕基础设置")]
    public float knockPoint;
    public string[] Blocktags;

    [Header("弹幕参数设置")]
    public float ExistTime = 7f;
    public List<EffectData> effects = new List<EffectData>();

    [Header("移动设置")]
    public projectileBehavior moveBehavior = projectileBehavior.Idle;
    public float moveSpeed = 5f;
    public Vector2 direction = Vector2.right;

    [Header("特殊移动参数")]
    public float accerate;
    public Transform Target;
    public bool NoInterval;
    public float TargetStrength = 2f;
    public float CloseTargetDistance = 5f;
    public float CloseTargetLeaveDistance = 2f;

    [Header("行为设置")]
    public bool IsSpin;

    [Header("行为参数")]
    [Tooltip("自转速度，单位为度/秒。60=每秒六分之一圈，180=每秒半圈。")]
    public float SpinSpeed = 180f;
    public bool isTargeting;
    public int FadeMode;

    private float fadeTimer;
    private float lifetime;
    private bool rescueFrozen;
    private bool cullOutsideArena;
    private Vector2 cullCenter;
    private float cullRadiusSquared;
    public System.Action<BarrageProjectile> ReturnToPool;
    private SpriteRenderer spriteRenderer;

    private AudioClip spawnClip;
    private float spawnVolume = 1f;
    private bool spawnSoundPending = true;
    private bool evolutionScaleApplied;
    public void ApplyEvolutionScale(Empty owner)
    {
        if (evolutionScaleApplied || !(owner is Mew boss)) return;
        transform.localScale *= boss.StarScaleForPlayer;
        evolutionScaleApplied = true;
    }

    private void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        // Previous prefab AudioSource.playOnAwake emitted once per bullet.
        foreach (var source in GetComponentsInChildren<AudioSource>(true))
        {
            if (spawnClip == null && !source.loop) { spawnClip = source.clip; spawnVolume = source.volume; }
            if (!source.loop) { source.playOnAwake = false; source.Stop(); source.enabled = false; }
        }
    }

    public void ResetLifetime()
    {
        spawnSoundPending = true;
        evolutionScaleApplied = false;
        lifetime = fadeTimer = 0f;
        rescueFrozen = false;
        cullOutsideArena = false;
    }

    private void Update()
    {
        if (spawnSoundPending && empty != null)
        {
            spawnSoundPending = false;
            if (empty is Mew && AudioManager.Instance != null)
                AudioManager.Instance.PlaySFXGrouped(spawnClip, transform.position, 0.08f, false, spawnVolume);
        }
        ApplyEvolutionScale(empty);
        if (rescueFrozen) return;
        lifetime += Time.deltaTime;
        if (lifetime >= Mathf.Max(0.05f, ExistTime))
        {
            Despawn();
            return;
        }
        UpdateSpin();
        UpdateFade();
        if (cullOutsideArena && moveBehavior == projectileBehavior.Straight && accerate >= 0f && moveSpeed >= 0f)
        {
            Vector2 delta = (Vector2)transform.position - cullCenter;
            if (delta.sqrMagnitude > cullRadiusSquared && Vector2.Dot(delta, direction) > 0f)
            {
                Despawn();
                return;
            }
        }

        switch (moveBehavior)
        {
            case projectileBehavior.Idle:
                break;
            case projectileBehavior.Straight:
                MoveStraight();
                break;
            case projectileBehavior.Target:
                MoveTarget();
                break;
            case projectileBehavior.Spiral:
                MoveSpiral();
                break;
            case projectileBehavior.CloseTarget:
                MoveCloseTarget();
                break;
        }
    }

    public void FreezeForRescue()
    {
        rescueFrozen = true;
        StopMovement();
        foreach (Collider2D hitbox in GetComponentsInChildren<Collider2D>()) hitbox.enabled = false;
        foreach (MonoBehaviour behaviour in GetComponentsInChildren<MonoBehaviour>())
            if (behaviour != this) { behaviour.StopAllCoroutines(); behaviour.enabled = false; }
    }

    public void Despawn()
    {
        if (ReturnToPool != null) ReturnToPool(this);
        else Destroy(gameObject);
    }

    public void ScheduleDespawn(float delay)
    {
        ExistTime = Mathf.Min(ExistTime, lifetime + Mathf.Max(0.01f, delay));
    }

    public void SetArenaCull(Vector2 center, float radius)
    {
        cullOutsideArena = true;
        cullCenter = center;
        cullRadiusSquared = radius * radius;
    }

    private void MoveIdle()
    {
        if (rigidbody2D != null)
        {
            rigidbody2D.velocity = Vector2.zero;
        }
    }

    private void MoveStraight()
    {
        moveSpeed += accerate * Time.deltaTime;

        if (rigidbody2D != null)
        {
            rigidbody2D.velocity = moveSpeed * direction;
        }
    }

    private void MoveTarget()
    {
        if (Target != null)
        {
            Vector2 targetDirection = Target.position - transform.position;
            if (targetDirection.sqrMagnitude > 0.0001f)
            {
                direction = targetDirection.normalized;
            }
        }

        moveSpeed += accerate * Time.deltaTime;

        if (rigidbody2D != null)
        {
            rigidbody2D.velocity = moveSpeed * direction;
        }
    }

    private void MoveSpiral()
    {
        // 当前第一阶段技能未使用 Spiral。
        // 保留枚举入口，避免破坏项目中已有 Prefab 序列化数据。
        MoveStraight();
    }

    private void MoveCloseTarget()
    {
        moveSpeed += accerate * Time.deltaTime;

        if (Target != null && !isTargeting)
        {
            float distance = Vector2.Distance(transform.position, Target.position);

            if (distance <= CloseTargetLeaveDistance)
            {
                isTargeting = true;
            }
            else if (distance <= CloseTargetDistance)
            {
                Vector2 targetDirection =
                    ((Vector2)Target.position - (Vector2)transform.position).normalized;

                float maxTurnAngle = Mathf.Max(0f, TargetStrength) * 30f * Time.deltaTime;
                Vector3 rotatedDirection = Vector3.RotateTowards(
                    direction.normalized,
                    targetDirection,
                    maxTurnAngle * Mathf.Deg2Rad,
                    0f);
                direction = ((Vector2)rotatedDirection).normalized;
            }
        }

        if (rigidbody2D != null)
        {
            rigidbody2D.velocity = moveSpeed * direction;
        }
    }

    private void UpdateSpin()
    {
        if (IsSpin)
        {
            // SpinSpeed 统一使用“度/秒”，不再混用“度/帧”。
            transform.Rotate(0f, 0f, SpinSpeed * Time.deltaTime);
        }
    }

    private void UpdateFade()
    {
        if (FadeMode == 1)
        {
            fadeTimer = 0f;
            FadeMode = 2;
        }

        if (FadeMode != 2 || spriteRenderer == null)
        {
            return;
        }

        fadeTimer += Time.deltaTime;
        Color color = spriteRenderer.color;
        color.a = Mathf.Lerp(color.a, 0f, Mathf.Clamp01(fadeTimer / 0.5f));
        spriteRenderer.color = color;
    }

    public void SetBehavior(projectileBehavior behavior)
    {
        moveBehavior = behavior;
        if (behavior == projectileBehavior.Idle) MoveIdle();
    }

    public void SetSpeed(float speed, float acceleration = 0f)
    {
        moveSpeed = speed;
        accerate = acceleration;
    }

    public void SetDirection(Vector2 newDirection)
    {
        if (newDirection.sqrMagnitude > 0.0001f)
        {
            direction = newDirection.normalized;
        }
    }

    public void SetTarget(
        Transform target,
        float targetStrength = 2f,
        float closeTargetDistance = 5f,
        float closeTargetLeaveDistance = 2f)
    {
        Target = target;
        TargetStrength = Mathf.Max(0f, targetStrength);
        CloseTargetDistance = Mathf.Max(0f, closeTargetDistance);
        CloseTargetLeaveDistance = Mathf.Clamp(
            closeTargetLeaveDistance,
            0f,
            CloseTargetDistance);
        isTargeting = false;
    }

    public void StopMovement()
    {
        SetBehavior(projectileBehavior.Idle);
        if (rigidbody2D != null)
        {
            rigidbody2D.velocity = Vector2.zero;
            rigidbody2D.angularVelocity = 0f;
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == null)
        {
            return;
        }

        if (Blocktags != null)
        {
            for (int i = 0; i < Blocktags.Length; i++)
            {
                string blockTag = Blocktags[i];
                if (!string.IsNullOrEmpty(blockTag) && collision.CompareTag(blockTag))
                {
                    StopMovement();
                    FadeMode = 1;
                    ExistTime = Mathf.Min(ExistTime, lifetime + 0.5f);
                    return;
                }
            }
        }

        if (!collision.CompareTag("Player") || FadeMode != 0 || empty == null)
        {
            return;
        }

        PlayerControler playerControler = collision.GetComponent<PlayerControler>();
        Pokemon.PokemonHpChange(
            empty.gameObject,
            collision.gameObject,
            0,
            SpDmage,
            0,
            ProType);

        ApplyEvolutionScale(empty);
        if (rescueFrozen) return;

        if (playerControler == null)
        {
            return;
        }

        playerControler.KnockOutPoint = knockPoint;
        playerControler.KnockOutDirection =
            (playerControler.transform.position - transform.position).normalized;

        if (effects == null)
        {
            return;
        }

        for (int i = 0; i < effects.Count; i++)
        {
            EffectData effect = effects[i];
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
