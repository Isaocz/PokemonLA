using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class BarrageProjectile : Projectile
{
    public enum projectileBehavior
    {
        Idle,           //停止移动
        Straight,       //直线移动
        Target,         //追踪目标
        Spiral,          //旋转移动
        CloseTarget      //靠近追踪
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
    public List<EffectData> effects = new List<EffectData>(); // 支持多个效果

    [Header("移动设置")]
    public projectileBehavior moveBehavior = projectileBehavior.Idle;
    public float moveSpeed = 5f;
    public Vector2 direction = Vector2.right;

    [Header("特殊移动参数")]
    public float accerate = 0f;
    public Transform Target;
    public bool NoInterval = false;                //切换状态不会改变状态计时
    public float TargetStrength;                    //追踪强度
    public float CloseTargetDistance;               //追踪判定距离
    public float CloseTargetLeaveDistance;          //取消追踪判定距离

    [Header("行为设置")]
    public bool IsSpin;                             //弹幕绕自身旋转

    [Header("行为参数")]
    public float SpinSpeed;                         //旋转速度
    public bool isTargeting = false;
    public int FadeMode = 0;                        //消失渐变（0为未消失，2为即将消失）

    private bool isStopping = false;
    private float timer = 0f;
    private SpriteRenderer sr;

    private void Start()
    {
        Destroy(gameObject, ExistTime);
        rigidbody2D = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        Spin();
        Fade();

        switch (moveBehavior)
        {
            case projectileBehavior.Idle:
                moveIdle();
                break;
            case projectileBehavior.Straight:
                moveStaight();
                break;
            case projectileBehavior.Target:
                moveTarget();
                break;
            case projectileBehavior.Spiral:
                moveSpiral();
                break;
            case projectileBehavior.CloseTarget:
                moveCloseTarget();
                break;
        }
    }
    //使弹幕停止移动
    private void moveIdle()
    {
        isStopping = false;
        if (rigidbody2D != null)
        {
            rigidbody2D.velocity = Vector3.zero;
        }
    }
    //使弹幕变速直线移动
    private void moveStaight()
    {
        if (isStopping)
        {
            timer += Time.deltaTime;
            if (moveSpeed != 0) { moveSpeed = Mathf.Lerp(moveSpeed, 0, Mathf.Clamp01(timer)); }
            else { isStopping = false; }
        }
        else
        {
            if (accerate != 0)
            {
                moveSpeed += accerate * Time.deltaTime;
            }

            if(rigidbody2D!= null)
            {
                rigidbody2D.velocity = moveSpeed * direction;
            }

        }
    }
    //使弹幕追踪目标
    private void moveTarget()
    {
        if (Target != null)
        {
            direction = (Target.position - transform.position).normalized;
        }

        if (accerate != 0)
        {
            moveSpeed += accerate * Time.deltaTime;
        }

        if(rigidbody2D != null)
        {
            rigidbody2D.velocity = moveSpeed * direction;
        }
    }

    private void moveSpiral()
    {

    }

    private void moveCloseTarget()
    {
        if (accerate != 0)
        {
            moveSpeed += accerate * Time.deltaTime;
        }

        //transform.Translate(direction * moveSpeed * Time.deltaTime, Space.World);

        if (Target != null)
        {
            
            float distance = Vector2.Distance(transform.position, Target.position);
            if(distance < CloseTargetDistance && distance > CloseTargetLeaveDistance && !isTargeting)
            {
                //计算指向目标的方向
                Vector2 targetDirection = (Target.position - transform.position).normalized;
                float interpolationFactor = Mathf.Clamp01(TargetStrength * Time.deltaTime);
                direction = Vector2.Lerp(direction, targetDirection, interpolationFactor).normalized;

                //最大转向角度限制
                float maxTurnAngle = 30f * Time.deltaTime; //每秒最多转向30度
                float currentAngle = Vector2.Angle(direction, targetDirection);
                if (currentAngle > maxTurnAngle)
                {
                    float t = maxTurnAngle / currentAngle;
                    direction = Vector2.Lerp(direction, targetDirection, t).normalized;
                }

            }
            else if (distance < CloseTargetLeaveDistance)
            {
                isTargeting = true;
            }
        }

        if(rigidbody2D != null)
        {
            rigidbody2D.velocity = moveSpeed * direction;
        }

    }

    private void Spin()
    {
        if (IsSpin)
        {
            transform.Rotate(0, 0, SpinSpeed);
        }

    }

    private void Fade()
    {
        if (FadeMode == 1 && sr != null)
        {
            timer = 0f;
            FadeMode = 2;
        }
        else if(FadeMode == 2)
        {
            timer += Time.deltaTime;
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, Mathf.Lerp(1, 0, (timer / 0.5f)));
        }
    }

    /// <summary>
    /// 设置移动模式
    /// </summary>
    /// <param name="behavior">移动逻辑</param>
    public void SetBehavior(projectileBehavior behavior)
    {
        this.moveBehavior = behavior;
    }
    /// <summary>
    /// 设置弹幕速度
    /// </summary>
    /// <param name="speed">弹幕速度</param>
    /// <param name="accerate">弹幕加速度</param>
    public void SetSpeed(float speed, float accerate = 0f)
    {
        this.moveSpeed = speed;
        this.accerate = accerate;
    }
    /// <summary>
    /// 设置弹幕方向
    /// </summary>
    /// <param name="direction">弹幕方向</param>
    public void SetDirection(Vector2 direction)
    {
        this.direction = direction.normalized;
    }
    /// <summary>
    /// 设置弹幕目标（用于追踪）
    /// </summary>
    /// <param name="target">弹幕目标</param>
    public void SetTarget(Transform target, float targetStrength = 2f, float closeTargetDistance = 5f, float closeTargetLeaveDistance = 2f)
    {
        this.Target = target;
        this.TargetStrength = targetStrength;
        this.CloseTargetDistance = closeTargetDistance;
        this.CloseTargetLeaveDistance = closeTargetLeaveDistance;
    }


    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (Blocktags != null)
        {
            foreach (var blocktag in Blocktags)
            {
                if (collision.tag == blocktag)
                {
                    moveBehavior = projectileBehavior.Idle;
                    FadeMode = 1;
                    Destroy(this.gameObject, 0.5f);
                    return;
                }
            }
        }

        if (collision.tag == "Player" && empty.gameObject && FadeMode == 0)
        {
            PlayerControler playerControler = collision.GetComponent<PlayerControler>();
            Pokemon.PokemonHpChange(empty.gameObject, collision.gameObject, 0, SpDmage, 0, ProType);
            if (playerControler != null)
            {
                playerControler.KnockOutPoint = knockPoint;
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
