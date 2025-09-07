using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class BarrageProjectile : Projectile
{
    public enum projectileBehavior
    {
        Idle,           //ֹͣ�ƶ�
        Straight,       //ֱ���ƶ�
        Target,         //׷��Ŀ��
        Spiral,          //��ת�ƶ�
        CloseTarget      //����׷��
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

    [Header("��Ļ��������")]
    public float knockPoint;
    public string[] Blocktags;

    [Header("��Ļ��������")]
    public float ExistTime = 7f;
    public List<EffectData> effects = new List<EffectData>(); // ֧�ֶ��Ч��

    [Header("�ƶ�����")]
    public projectileBehavior moveBehavior = projectileBehavior.Idle;
    public float moveSpeed = 5f;
    public Vector2 direction = Vector2.right;

    [Header("�����ƶ�����")]
    public float accerate = 0f;
    public Transform Target;
    public bool NoInterval = false;                //�л�״̬����ı�״̬��ʱ
    public float TargetStrength;                    //׷��ǿ��
    public float CloseTargetDistance;               //׷���ж�����
    public float CloseTargetLeaveDistance;          //ȡ��׷���ж�����

    [Header("��Ϊ����")]
    public bool IsSpin;                             //��Ļ��������ת

    [Header("��Ϊ����")]
    public float SpinSpeed;                         //��ת�ٶ�
    public bool isTargeting = false;
    public int FadeMode = 0;                        //��ʧ���䣨0Ϊδ��ʧ��2Ϊ������ʧ��

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
    //ʹ��Ļֹͣ�ƶ�
    private void moveIdle()
    {
        isStopping = false;
        if (rigidbody2D != null)
        {
            rigidbody2D.velocity = Vector3.zero;
        }
    }
    //ʹ��Ļ����ֱ���ƶ�
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
    //ʹ��Ļ׷��Ŀ��
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
                //����ָ��Ŀ��ķ���
                Vector2 targetDirection = (Target.position - transform.position).normalized;
                float interpolationFactor = Mathf.Clamp01(TargetStrength * Time.deltaTime);
                direction = Vector2.Lerp(direction, targetDirection, interpolationFactor).normalized;

                //���ת��Ƕ�����
                float maxTurnAngle = 30f * Time.deltaTime; //ÿ�����ת��30��
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
    /// �����ƶ�ģʽ
    /// </summary>
    /// <param name="behavior">�ƶ��߼�</param>
    public void SetBehavior(projectileBehavior behavior)
    {
        this.moveBehavior = behavior;
    }
    /// <summary>
    /// ���õ�Ļ�ٶ�
    /// </summary>
    /// <param name="speed">��Ļ�ٶ�</param>
    /// <param name="accerate">��Ļ���ٶ�</param>
    public void SetSpeed(float speed, float accerate = 0f)
    {
        this.moveSpeed = speed;
        this.accerate = accerate;
    }
    /// <summary>
    /// ���õ�Ļ����
    /// </summary>
    /// <param name="direction">��Ļ����</param>
    public void SetDirection(Vector2 direction)
    {
        this.direction = direction.normalized;
    }
    /// <summary>
    /// ���õ�ĻĿ�꣨����׷�٣�
    /// </summary>
    /// <param name="target">��ĻĿ��</param>
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
