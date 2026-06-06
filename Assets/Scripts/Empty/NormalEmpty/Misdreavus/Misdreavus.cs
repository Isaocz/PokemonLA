using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Misdreavus : Empty
{
    /// <summary>
    /// 目标位置
    /// </summary>
    private Vector3 TargetPosition;
    /// <summary>
    /// 方向
    /// </summary>
    private Vector3 direction;
    private string currentState;

    /// <summary>
    /// 最后位置
    /// </summary>
    Vector2 LastPosition;

    /// <summary>
    /// 速度倍率
    /// </summary>
    float SpeedAlpha = 1.0f;



    /// <summary>
    /// 唱歌预制件
    /// </summary>
    public MisdreavusSing SingPrefab;
    /// <summary>
    /// 唱歌实例
    /// </summary>
    MisdreavusSing SingObj;
    /// <summary>
    /// 唱歌位置
    /// </summary>
    public static Vector2 SingOffset = new Vector2(0, 0.3f);



    /// <summary>
    /// 是否被技能杀死
    /// </summary>
    public bool IsKilledBySkill
    {
        get { return isKilledBySkill; }
        set { isKilledBySkill = value; }
    }
    bool isKilledBySkill;

    //亡语效果
    public GameObject killEffect;
    public GameObject impoisonEffect;

    /// <summary>
    /// 被封印的技能序号
    /// </summary>
    public int ImpoisonSkillIndex;
    /// <summary>
    /// 封印时间
    /// </summary>
    public float ImpoisonTime = 20.0f;

    // Start is called before the first frame update
    void Start()
    {
        EmptyType01 = PokemonType.TypeEnum.Ghost;
        EmptyType02 = PokemonType.TypeEnum.No;
        player = GameObject.FindObjectOfType<PlayerControler>();
        Emptylevel = SetLevel(player.Level, MaxLevel);
        EmptyHpForLevel(Emptylevel);
        AtkAbilityPoint = AbilityForLevel(Emptylevel, AtkEmptyPoint);
        SpAAbilityPoint = AbilityForLevel(Emptylevel, SpAEmptyPoint);
        DefAbilityPoint = AbilityForLevel(Emptylevel, DefEmptyPoint);
        SpdAbilityPoint = AbilityForLevel(Emptylevel, SpdEmptyPoint);
        SpeedAbilityPoint = AbilityForLevel(Emptylevel, SpeedEmptyPoint);
        Exp = BaseExp * Emptylevel / 7;

        //获取刚体目标 动画管理者目标 并让刚体的初始x坐标带入FirstX中
        animator = GetComponent<Animator>();
        rigidbody2D = GetComponent<Rigidbody2D>();

        LastPosition = transform.position;



        StartOverEvent();
    }

    // Update is called once per frame
    void Update()
    {
        ResetPlayer();
        if (!isBorn)
        {
            EmptyDie();
            UpdateEmptyChangeHP();
            StateMaterialChange();

            if (!isBorn && !isDie)
            {
                //唱歌
                if (!isEmptyFrozenDone && !isSleepDone && !isSilence && !isFearDone)
                {
                    //开始唱歌
                    if (SingObj == null)
                    {
                        SingStart();
                    }
                }
                else
                {
                    //结束唱歌
                    if (SingObj != null)
                    {
                        SingOver();
                    }
                }
            }

            
        }
    }

    /// <summary>
    /// 死亡时结束唱歌
    /// </summary>
    public override void DieEvent()
    {
        base.DieEvent();
        SingOver();
    }

    //碰撞
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (!isEmptyInfatuationDone && other.transform.tag == ("Player"))
        {
            EmptyTouchHit(other.gameObject);

        }
        if (isEmptyInfatuationDone && other.transform.tag == ("Empty"))
        {
            InfatuationEmptyTouchHit(other.gameObject);
        }
    }


    private void FixedUpdate()
    {
        ResetPlayer();
        if (isEmptyInfatuationDone) { UpdateInfatuationDmageCDTimer(); }
        if (!isBorn && !isDie)
        {
            EmptyBeKnock();

            //移动
            if (!isEmptyFrozenDone && !isSleepDone && !isCanNotMoveWhenParalysis && !isSilence)
            {
                //根据魅惑情况确实目标位置
                Transform InfatuationTarget = InfatuationForDistanceEmpty();
                if (!isEmptyInfatuationDone || (ParentPokemonRoom.GetEmptyList().Count + ParentPokemonRoom.GetEmptyCloneList().Count) <= 1 || InfatuationTarget == null)
                {
                    TargetPosition = player.transform.position;
                    if (isSubsititue && SubsititueTarget != null) { TargetPosition = SubsititueTarget.transform.position; }
                }
                else { TargetPosition = InfatuationTarget.transform.position; }

                //不恐惧时
                if (!isFearDone)
                {
                    //接近目标
                    //距离小于1 速度为1f 调整速度倍率为0.25
                    if ((TargetPosition - transform.position).magnitude <= 1.0f)
                    {
                        SpeedAlpha = 0.25f;
                    }
                    //距离小于3大于1 速度为2f 调整速度倍率为0.5
                    else if ((TargetPosition - transform.position).magnitude > 1.0f && (TargetPosition - transform.position).magnitude <= 3.0f)
                    {
                        SpeedAlpha = 0.5f; ;
                    }
                    //常态 速度为4f 调整速度倍率为1
                    else
                    {
                        SpeedAlpha = 1.0f;
                    }
                    direction = (TargetPosition - transform.position).normalized;
                    direction = (Quaternion.AngleAxis(isEmptyConfusionDone ? 30 : 0, Vector3.forward) * direction).normalized;
                    animator.SetFloat("LookX", (direction.x >= 0 ? 1 : -1));
                    animator.SetFloat("LookY", (direction.y >= 0 ? 1 : -1));
                    //rigidbody2D.position = new Vector2(Mathf.Clamp(rigidbody2D.position.x + (float)direction.x * Time.deltaTime * speed, -15f + transform.parent.position.x, 15f + transform.parent.position.x), Mathf.Clamp(rigidbody2D.position.y + (float)direction.y * Time.deltaTime * speed, -10f + transform.parent.position.y, 10f + transform.parent.position.y));
                    MoveBySpeedAndDir(direction , speed, SpeedAlpha ,0.0f, 0.0f, 0.0f, 0.0f);
                }
                //恐惧时 远离目标
                else
                {
                    SpeedAlpha = 1.5f;
                    direction = (TargetPosition - transform.position).normalized;
                    direction = (Quaternion.AngleAxis(isEmptyConfusionDone ? 150 : 180, Vector3.forward) * direction).normalized;
                    animator.SetFloat("LookX", (direction.x >= 0 ? 1 : -1));
                    animator.SetFloat("LookY", (direction.y >= 0 ? 1 : -1));
                    if ((TargetPosition - transform.position).magnitude <= 4.0f)
                    {
                        MoveBySpeedAndDir(direction, speed, SpeedAlpha, 0.0f, 0.0f, 0.0f, 0.0f);
                        //rigidbody2D.position = new Vector2(Mathf.Clamp(rigidbody2D.position.x + (float)direction.x * Time.deltaTime * speed, -15f + transform.parent.position.x, 15f + transform.parent.position.x), Mathf.Clamp(rigidbody2D.position.y + (float)direction.y * Time.deltaTime * speed, -10f + transform.parent.position.y, 10f + transform.parent.position.y));
                    }
                }

                /**
                if (!isFearDone)
                {
                    direction = (targetPosition - transform.position).normalized;
                    rigidbody2D.position = new Vector2(Mathf.Clamp(rigidbody2D.position.x + (float)direction.x * Time.deltaTime * speed, -15f + transform.parent.position.x, 15f + transform.parent.position.x), Mathf.Clamp(rigidbody2D.position.y + (float)direction.y * Time.deltaTime * speed, -10f + transform.parent.position.y, 10f + transform.parent.position.y));
                    if(direction.x > 0)
                    {
                        if (direction.y > 0) ChangeAnimationState("MisdreavusMoveNE");
                        else ChangeAnimationState("MisdreavusMoveSE");
                    }
                    else
                    {
                        if(direction.y > 0) ChangeAnimationState("MisdreavusMoveNW");
                        else ChangeAnimationState("MisdreavusMoveSW");
                    }
                }
                **/
            }


            //动画朝向
            animator.SetFloat("Speed", ((Vector2)transform.position - LastPosition).magnitude);
            LastPosition = transform.position;
        }
    }

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





    //开始唱歌
    void SingStart()
    {
        if (SingObj != null)
        {
            Destroy(SingObj.gameObject);
            SingObj = null;
        }
        SingObj = Instantiate(SingPrefab, transform.position + (Vector3)SingOffset, Quaternion.identity, transform);
        SingObj.ParentMisdreavus = this;
    }

    //结束唱歌
    void SingOver()
    {
        if (SingObj != null)
        {
            SingObj.SingOver();
            SingObj = null;
        }
    }








    public void Impoison()
    {
        if (IsDeadrattle && IsKilledBySkill) {
            switch (ImpoisonSkillIndex)
            {
                case 1:
                    player.Is01imprison = true;
                    player.SetZeroSkillCDTime(1);
                    player.imprisonTime01 = ImpoisonTime;
                    break;
                case 2:
                    player.Is02imprison = true;
                    player.SetZeroSkillCDTime(2);
                    player.imprisonTime02 = ImpoisonTime;
                    break;
                case 3:
                    player.Is03imprison = true;
                    player.SetZeroSkillCDTime(3);
                    player.imprisonTime03 = ImpoisonTime;
                    break;
                case 4:
                    player.Is04imprison = true;
                    player.SetZeroSkillCDTime(4);
                    player.imprisonTime04 = ImpoisonTime;
                    break;
            }
            Invoke("BornImpoison", 0.5f);
        }
    }

    void BornImpoison()
    {
        GameObject impoison = Instantiate(impoisonEffect, player.transform.position + Vector3.up * player.SkillOffsetforBodySize[0], Quaternion.identity);
        impoison.GetComponent<impoisonEffect>().player = player;
        Destroy(impoison, 20f);
    }

    public void Killeffect()
    {
        GameObject killeffect = Instantiate(killEffect, transform.position, Quaternion.identity);
        Destroy(killeffect, 1f);
    }





    /// <summary>
    /// 刚体敌人在房间限制内移动
    /// </summary>
    /// <param name="dir">移动方向</param>
    /// <param name="Speed">移动速度</param>
    /// <param name="SpeedAlpha">移动速度的加成系数（乘算）</param>
    /// <param name="RoomUpAlpha">房间上边界的限制系数</param>
    /// <param name="RoomDownAlpha">房间下边界的限制系数</param>
    /// <param name="RoomLeftAlpha">房间右边界的限制系数</param>
    /// <param name="RoomRightAlpha">房间左边界的限制系数</param>
    public void MoveBySpeedAndDir(Vector2 dir, float Speed, float SpeedAlpha, float RoomUpAlpha, float RoomDownAlpha, float RoomLeftAlpha, float RoomRightAlpha)
    {

        rigidbody2D.position = new Vector2(
            Mathf.Clamp(rigidbody2D.position.x
                + (float)dir.x * Time.deltaTime * Speed * SpeedAlpha,                    //方向*速度
            ParentPokemonRoom.RoomSize[2] - RoomLeftAlpha + transform.parent.position.x, //最小值
            ParentPokemonRoom.RoomSize[3] + RoomRightAlpha + transform.parent.position.x),//最大值
            Mathf.Clamp(rigidbody2D.position.y
                + (float)dir.y * Time.deltaTime * Speed * SpeedAlpha,                     //方向*速度 
            ParentPokemonRoom.RoomSize[1] - RoomDownAlpha + transform.parent.position.y,  //最小值
            ParentPokemonRoom.RoomSize[0] + RoomUpAlpha + transform.parent.position.y));//最大值
    }
}

