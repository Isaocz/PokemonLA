using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drifloon : Empty
{
    /// <summary>
    /// 目标位置
    /// </summary>
    private Vector3 TargetPosition;

    public Vector3 InitialDirection;
    public GameObject explosion;
    private Vector3 direction;
    //private string currentState;

    //private Vector3 currentPosition;
    //private float timer;
    //private bool IsReflect;

    bool isAngry = false;
    bool isExploson = false;
    bool isExplosonOBJBorn = false;

    public static float SPEEDALPHA_ANGRY = 2.7f;

    public static float DISTENCE_EXPLOSON = 1.5f;


    void Start()
    {
        EmptyType01 = PokemonType.TypeEnum.Ghost;
        EmptyType02 = PokemonType.TypeEnum.Flying;
        player = GameObject.FindObjectOfType<PlayerControler>();
        Emptylevel = SetLevel(player.Level, MaxLevel);
        EmptyHpForLevel(Emptylevel);
        AtkAbilityPoint = AbilityForLevel(Emptylevel, AtkEmptyPoint);
        SpAAbilityPoint = AbilityForLevel(Emptylevel, SpAEmptyPoint);
        DefAbilityPoint = AbilityForLevel(Emptylevel, DefEmptyPoint);
        SpdAbilityPoint = AbilityForLevel(Emptylevel, SpdEmptyPoint);
        SpeedAbilityPoint = AbilityForLevel(Emptylevel, SpeedEmptyPoint);
        Exp = BaseExp * Emptylevel / 7;
        animator = GetComponent<Animator>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        direction = InitialDirection;
        
        //IsReflect = true;



        StartOverEvent();
    }



    void Update()
    {
        ResetPlayer();
        if (!isBorn)
        {
            EmptyDie();
            UpdateEmptyChangeHP();
            StateMaterialChange();


            if (!isEmptyFrozenDone && !isSleepDone && !isCanNotMoveWhenParalysis && !isSilence)
            {
                //生气加速
                if (!isAngry && EmptyHp <= maxHP / 2)
                {
                    isAngry = true;
                    AngryEffect(new Vector2(0.0f, 1.5f), this.gameObject, new Vector3(1.0f, 1.0f, 1.0f), true);

                }

                if (!isFearDone) {
                    //Debug.Log(Vector2.Distance(TargetPosition, transform.position));
                    //近距离自爆
                    if (!isExploson && Vector2.Distance(TargetPosition, transform.position) <= DISTENCE_EXPLOSON)
                    {
                        EmptyEcplosionEvent();
                    }
                }
            }

                /*
                timer += Time.deltaTime;
                if(timer > 0.6f)
                {   //检测是否被卡住
                    if(!isEmptyFrozenDone && !isSleepDone && !isCanNotMoveWhenParalysis && !isSilence && !isFearDone && !isHit && !IsReflect && (!(Mathf.Abs(currentPosition.x - transform.position.x) > 0.8f && Mathf.Abs(currentPosition.y - transform.position.y) > 0.8f)))
                    {
                        RaycastHit2D raycastHit2Da = Physics2D.Raycast(transform.position, new Vector2(1, 1), 20f, LayerMask.GetMask("Room"));
                        RaycastHit2D raycastHit2Db = Physics2D.Raycast(transform.position, new Vector2(1, -1), 20f, LayerMask.GetMask("Room"));
                        RaycastHit2D raycastHit2Dc = Physics2D.Raycast(transform.position, new Vector2(-1, 1), 20f, LayerMask.GetMask("Room"));
                        RaycastHit2D raycastHit2Dd = Physics2D.Raycast(transform.position, new Vector2(-1, -1), 20f, LayerMask.GetMask("Room"));
                        float maxDistance = Mathf.Max(raycastHit2Da.distance, raycastHit2Db.distance, raycastHit2Dc.distance, raycastHit2Dd.distance);
                        if (maxDistance == raycastHit2Da.distance)
                        {
                            direction = new Vector2(1, 1);
                        }
                        else if (maxDistance == raycastHit2Db.distance)
                        {
                            direction = new Vector2(1, -1);
                        }
                        else if (maxDistance == raycastHit2Dc.distance)
                        {
                            direction = new Vector2(-1, 1);
                        }
                        else
                        {
                            direction = new Vector2(-1, -1);
                        }
                    }
                    timer = 0f;
                    IsReflect = false;
                    currentPosition = transform.position;
                }
                */
            }
    
    }

    private void FixedUpdate()
    {
        ResetPlayer();
        if (isEmptyInfatuationDone) { UpdateInfatuationDmageCDTimer(); }
        if (!isBorn && !isDie)
        {

            EmptyBeKnock();
            if (!isEmptyFrozenDone && !isSleepDone && !isSilence && !isFearDone)
            { animator.SetBool("Exploson", true); }
            else
            { animator.SetBool("Exploson", false); }
            //根据魅惑情况确实目标位置
            Transform InfatuationTarget = InfatuationForDistanceEmpty();
            if (!isEmptyInfatuationDone || (ParentPokemonRoom.GetEmptyList().Count + ParentPokemonRoom.GetEmptyCloneList().Count) <= 1 || InfatuationTarget == null)
            {
                TargetPosition = player.transform.position;
                if (isSubsititue && SubsititueTarget != null) { TargetPosition = SubsititueTarget.transform.position; }
            }
            else { TargetPosition = InfatuationTarget.transform.position; }



            if (!isEmptyFrozenDone && !isSleepDone && !isCanNotMoveWhenParalysis && !isSilence && !isExploson)
            {
               

                //移动
                rigidbody2D.position = new Vector2(Mathf.Clamp(rigidbody2D.position.x + (float)direction.x * Time.deltaTime * speed * (isAngry ? SPEEDALPHA_ANGRY : 1.0f) * (isFearDone ? 1.6f : 1.0f), -15f + transform.parent.position.x, 15f + transform.parent.position.x), Mathf.Clamp(rigidbody2D.position.y + (float)direction.y * Time.deltaTime * speed * (isAngry ? SPEEDALPHA_ANGRY : 1.0f) * (isFearDone ? 1.6f : 1.0f), -10f + transform.parent.position.y, 10f + transform.parent.position.y));

                /*
                if (direction.x > 0)
                {
                    if (direction.y > 0) ChangeAnimationState("DrifloonMoveNE");
                    else ChangeAnimationState("DrifloonMoveSE");
                }
                else
                {
                    if (direction.y > 0) ChangeAnimationState("DrifloonMoveNW");
                    else ChangeAnimationState("DrifloonMoveSW");
                }
                */
            }
        }
    }


    private void OnCollisionEnter2D(Collision2D other)
    {
        if (!isEmptyInfatuationDone && !isExploson && other.transform.tag == ("Player"))
        {
            EmptyTouchHit(other.gameObject);

        }
        if (isEmptyInfatuationDone && !isExploson && other.transform.tag == ("Empty"))
        {
            InfatuationEmptyTouchHit(other.gameObject);
        }

        /*
        if(other.transform.tag == "Room")
        {
            //反弹操作
            Vector2 newDirection = Vector2.Reflect(direction, other.GetContact(0).normal);
            Vector2[] comparisonValues = new Vector2[]
            {
                new Vector2(-1, -1),
                new Vector2(-1, 1),
                new Vector2(1, 1),
                new Vector2(1, -1)
            };

            float minDistance = Mathf.Infinity;
            int closestIndex = -1;
            for (int i = 0; i < comparisonValues.Length; i++)
            {
                float distance = Vector2.SqrMagnitude(newDirection - comparisonValues[i]);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closestIndex = i;
                }
            }

            direction = comparisonValues[closestIndex];
            IsReflect = true;
        }
        */
    }


    //=================================碰壁反弹=======================================


    private void OnCollisionStay2D(Collision2D other)
    {
        Rebound_BeLunch();
    }


    /// <summary>
    /// 反弹限制计时器的时间上限
    /// </summary>
    static float TIME_REBOUND_CD = 0.1f;

    /// <summary>
    /// 反弹限制计时器 防止在一次碰撞中多次反弹
    /// </summary>
    float ReboundTimer;

    /// <summary>
    /// 被发射时发生反弹
    /// </summary>
    bool Rebound_BeLunch()
    {
        //当前时间
        float now = Time.time;

        if (now - ReboundTimer < TIME_REBOUND_CD) { return false; } // 防抖：0.1s 内忽略

        ReboundTimer = now;

        //碰撞方向
        Vector2 HitVector = Vector2.right;
        {
            Vector2 EmptyCenter = (Vector2)transform.position + GetComponent<Collider2D>().offset;
            float[] DistenceList = new float[] { 0.0f, 0.0f, 0.0f, 0.0f };
            RaycastHit2D RPRay = Physics2D.Raycast(EmptyCenter, Vector2.right, LayerMask.GetMask("Room"));
            RaycastHit2D UPRay = Physics2D.Raycast(EmptyCenter, Vector2.up, LayerMask.GetMask("Room"));
            RaycastHit2D LPRay = Physics2D.Raycast(EmptyCenter, Vector2.left, LayerMask.GetMask("Room"));
            RaycastHit2D DPRay = Physics2D.Raycast(EmptyCenter, Vector2.down, LayerMask.GetMask("Room"));
            if (RPRay) { DistenceList[0] = RPRay.distance; Debug.DrawLine(EmptyCenter, RPRay.point, Color.red, 0.1f); }
            if (UPRay) { DistenceList[1] = UPRay.distance; Debug.DrawLine(EmptyCenter, UPRay.point, Color.red, 0.1f); }
            if (LPRay) { DistenceList[2] = LPRay.distance; Debug.DrawLine(EmptyCenter, LPRay.point, Color.red, 0.1f); }
            if (DPRay) { DistenceList[3] = DPRay.distance; Debug.DrawLine(EmptyCenter, DPRay.point, Color.red, 0.1f); }
            float MinDistence = DistenceList[0];
            int MinIndex = 0;
            for (int i = 1; i < 4; i++)
            {
                if (MinDistence > DistenceList[i])
                {
                    MinDistence = DistenceList[i];
                    MinIndex = i;
                }
            }
            Debug.Log("R" + DistenceList[0] + "U" + DistenceList[1] + "L" + DistenceList[2] + "D" + DistenceList[3] + "+" + MinDistence);
            HitVector = _mTool.MainVector2(Quaternion.AngleAxis(MinIndex * 90.0f, Vector3.forward) * HitVector);
        }

        direction = new Vector2(direction.x * (HitVector.x == 0 ? 1 : -1), direction.y * (HitVector.y == 0 ? 1 : -1));
        if (isEmptyConfusionDone) { direction = ConfusionDir(direction , 30.0f); }
        SetDirector(direction);
        return true;

    }


    //=================================碰壁反弹=======================================


    /// <summary>
    /// 设置敌人的动画机方向
    /// </summary>
    void SetDirector(Vector2 director)
    {
        direction = director;
        animator.SetFloat("LookX", director.x);
        animator.SetFloat("LookY", director.y);
    }


    public void Explosion()
    {
        if (IsDeadrattle && !isExplosonOBJBorn) {
            isExplosonOBJBorn = true;
            GameObject boom = Instantiate(explosion, transform.position, Quaternion.identity);
            DrifloonExplosion e = boom.transform.GetChild(0).GetComponent<DrifloonExplosion>();
            e.empty = this;
            //cb.SetEmptyInfo(this);
            //cb.SetAimTag(isEmptyInfatuationDone ? "Empty" : "Player");
            //cb.SetType(PokemonType.TypeEnum.Normal);
            //cb.Dmage = 200;
            Destroy(boom, 5f);
        }
    }
    /**
    void ChangeAnimationState(string newState)
    {   //动画管理
        if (!isHit)
        {
            if (currentState == newState)
                return;

            currentState = newState;
            animator.Play(newState);
        }
    }
    **/

    //爆炸
    public override void EmptyEcplosionEvent()
    {
        base.EmptyEcplosionEvent();
        animator.SetTrigger("Exploson");
        Pokemon.PokemonHpChange(null, this.gameObject, this.maxHP, 0, 0, PokemonType.TypeEnum.IgnoreType);
        ParentPokemonRoom.CameraShake(0.6f, 3.0f, true);
        isExploson = true;
    }


    /// <summary>
    /// 混乱时角度偏转
    /// </summary>
    Vector2 ConfusionDir(Vector2 dir, float Alpha)
    {
        if (isEmptyConfusionDone)
        {
            return Quaternion.AngleAxis(Random.Range(-Alpha, Alpha), Vector3.forward) * dir;
        }
        return dir;
    }
}
