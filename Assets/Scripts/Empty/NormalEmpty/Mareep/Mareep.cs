using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mareep : Empty
{

    MIAStarAIEscape AStarAIEscap;
    Vector3 LastPostion;
    bool isEscape;
    int TurnTimer;
    Vector2 Direction;
    public MareepWool Wool;
    bool isDrop;
    float isDropTimer;
    public float isEscapeTimer;

    int EscapeOverCount = 0;


    //唱歌时的技能圈
    public EmptyEchoedVoice singCircle;
    EmptyEchoedVoice singCircleObj;


    /// <summary>
    /// 是否处于唱歌状态
    /// </summary>
    bool isEchoedVoice = false;
    /// <summary>
    /// 唱歌（轮唱）cd计时器
    /// </summary>
    float EchoedVoiceCDTimer = 0.0f;
    /// <summary>
    /// 唱歌（轮唱）CD时间
    /// </summary>
    public float TIME_CD_ECHOEDVOICE = 2.0f;


    // Start is called before the first frame update
    void Start()
    {

        EmptyType01 = PokemonType.TypeEnum.Electric;
        EmptyType02 = 0;
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
        AStarAIEscap = GetComponent<MIAStarAIEscape>();
        LastPostion = transform.position;
        Direction = RandomDirection();
        isEscapeTimer = 200;



        StartOverEvent();
    }

    public void EscapOver()
    {
        EscapeOverCount = 0;
        animator.SetTrigger("EscapeOver");
        isEscape = false;
    }

    // Update is called once per frame


    private void Update()
    {
        if (!isDie && !isBorn)
        {
            EmptyDie();
            StateMaterialChange();
            UpdateEmptyChangeHP();
        }
    }

    private void FixedUpdate()
    {
        ResetPlayer();
        if (isEmptyInfatuationDone) { UpdateInfatuationDmageCDTimer(); }
        if (!isDie && !isBorn) {
            EmptyBeKnock();//判定是否被击退
            if (!isEchoedVoice)
            {

                if (EchoedVoiceCDTimer > 0.0f)
                {
                    EchoedVoiceCDTimer -= Time.deltaTime;
                    if (EchoedVoiceCDTimer <= 0.0f)
                    {
                        EchoedVoiceCDTimer = 0.0f;
                    }
                }
                else
                {
                    if (isDrop && !isSleepDone && !isEmptyFrozenDone && !isSilence && !isCanNotMoveWhenParalysis)
                    {
                        if (isDropTimer % 20 == 0)
                        {
                            if (EscapeOverCount >= 8) { EscapOver(); }
                            if ((Random.Range(0.0f, 1.0f) <= 0.2f || isDropTimer == 0) && !isFearDone)
                            {
                                DropWool();
                                DropWool();
                                if (Random.Range(0.0f, 1.0f) >= 0.5f) { DropWool(); }
                            }
                        }
                        isDropTimer += 1;
                    }
                    if (!isBorn && !isDie)
                    {
                        if (!isSleepDone && !isEmptyFrozenDone && !isSilence && !isCanNotMoveWhenParalysis)
                        {
                            animator.SetFloat("Speed", Mathf.Abs((transform.position - LastPostion).magnitude));
                            if (transform.position.x - LastPostion.x >= 0) { animator.SetFloat("LookX", 1); } else { animator.SetFloat("LookX", -1); }
                            if (transform.position.y - LastPostion.y >= 0) { animator.SetFloat("LookY", 1); } else { animator.SetFloat("LookY", -1); }
                            LastPostion = transform.position;
                            TurnTimer += 1;
                            EmptyBeKnock();
                            if (!isEscape && !isDie && !isHit && !isSilence && !isSleepDone && !isEmptyFrozenDone)
                            {
                                rigidbody2D.position += Time.deltaTime * speed * Direction;
                                isDrop = false;
                                isDropTimer = 0;
                                isEscapeTimer += 1;
                            }
                            if (TurnTimer >= 90)
                            {
                                TurnTimer = 0;
                                Direction = RandomDirection();
                            }
                        }
                }
                    if ((isEscapeTimer >= 200 && !isEscape && !isDie && !isSilence && !isEmptyFrozenDone && !isSleepDone) && (isHit || (isFearDone && (transform.position - player.transform.position).magnitude <= 3)))
                    {
                        if (!isEmptyInfatuationDone)
                        {
                            if (!isSleepDone && !isCanNotMoveWhenParalysis) { Escape(); }
                        }
                        else
                        {
                            if (!isSleepDone && !isCanNotMoveWhenParalysis) { isDrop = true; }
                        }
                    }

                }

            }
            if ((isEmptyFrozenDone || isSleepDone || isSilence || (isFearDone && isEchoedVoice)))
            {
                animator.SetFloat("Speed", 0.0f);
                if ((isEscape || isEchoedVoice))
                {
                    if (isEchoedVoice) { SetSingOver(); }
                    animator.SetBool("Sing", false);

                    animator.SetTrigger("Sleep");
                    EchoedVoiceCDTimer = 0.0f;
                    EscapOver();
                    Debug.Log("Over");
                    TurnTimer = 0;
                    isDropTimer = 0.0f;
                    isEscapeTimer = 0.0f;
                    EscapeOverCount = 0;
                    //LastPostion = ParentEmptyByChild.transform.position;
                    if (singCircleObj != null) { Destroy(singCircleObj.gameObject); }
                }
            }
        }
    }

    void Escape()
    {
        if (!isEchoedVoice)
        {
            isEscape = true;
            animator.ResetTrigger("Escape");
            animator.ResetTrigger("EscapeOver");
            animator.SetTrigger("Escape");
            if (!isEmptyConfusionDone)
            {
                Vector3 t = Vector3.zero;
                switch (Random.Range(1, 5))
                {
                    case 1:
                        //AStarAIEscap.Escape(transform.parent.position + new Vector3(12.0f, 8.0f, 0));
                        //ParentPokemonRoom.RoomSize[3] - 2.0f;
                        //ParentPokemonRoom.RoomSize[2] + 2.0f;
                        //ParentPokemonRoom.RoomSize[1] + 1.3f;
                        //ParentPokemonRoom.RoomSize[0] - 1.3f;
                        t = new Vector3(Random.Range(ParentPokemonRoom.RoomSize[3] - 5.0f, ParentPokemonRoom.RoomSize[3] - 2.0f), Random.Range(ParentPokemonRoom.RoomSize[0] - 3.3f, ParentPokemonRoom.RoomSize[0] - 1.3f), 0);

                        break;
                    case 2:
                        //AStarAIEscap.Escape(transform.parent.position + new Vector3(12.0f, 8.0f, 0));
                        t = new Vector3(Random.Range(ParentPokemonRoom.RoomSize[3] - 5.0f, ParentPokemonRoom.RoomSize[3] - 2.0f), Random.Range(ParentPokemonRoom.RoomSize[1] + 3.3f, ParentPokemonRoom.RoomSize[1] + 1.3f), 0);
                        break;
                    case 3:
                        //AStarAIEscap.Escape(transform.parent.position + new Vector3(12.0f, 8.0f, 0));
                        t = new Vector3(Random.Range(ParentPokemonRoom.RoomSize[2] + 5.0f, ParentPokemonRoom.RoomSize[2] + 2.0f), Random.Range(ParentPokemonRoom.RoomSize[0] - 3.3f, ParentPokemonRoom.RoomSize[0] - 1.3f), 0);
                        break;
                    case 4:
                        //AStarAIEscap.Escape(transform.parent.position + new Vector3(12.0f, 8.0f, 0));
                        t = new Vector3(Random.Range(ParentPokemonRoom.RoomSize[2] + 5.0f, ParentPokemonRoom.RoomSize[2] + 2.0f), Random.Range(ParentPokemonRoom.RoomSize[1] + 3.3f, ParentPokemonRoom.RoomSize[1] + 1.3f), 0);
                        break;
                }
                Debug.Log(t);
                AStarAIEscap.Escape(transform.parent.position + t);

            }
            else
            {
                AStarAIEscap.Escape(transform.parent.position);
            }
            isEscapeTimer = 0;
            isDrop = true;
        }
    }

    void DropWool()
    {
        Projectile ProjectileObj = Instantiate(Wool, transform.position,  Quaternion.Euler(0, 0, Random.Range(0, 360)) , transform.parent);
        ProjectileObj.Launch(Quaternion.AngleAxis( Random.Range(-90,90) , Vector3.forward )*(LastPostion - transform.position), Random.Range(20,60));
        ProjectileObj.empty = transform.GetComponent<Empty>();
        EscapeOverCount++;

    }

    Vector2 RandomDirection()
    {
        if (Random.Range(0.0f , 1.0f) > 0.3f)
        {
            return (new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f))).normalized;
        }
        else
        {
            return Vector2.zero;
        }

    }


    private void OnCollisionEnter2D(Collision2D other)
    {
        if ((other.transform.tag == ("Player") ) && !isEmptyInfatuationDone)
        {
            EmptyTouchHit(other.gameObject);
            Escape();
        }
    }



    
    //=========================唱歌回声相关=============================

    /// <summary>
    /// 使用回声
    /// </summary>
    public override void UseEchoedVoice(int echoedVoiceLevel)
    {
        Debug.Log(name + "+" + transform.position + "+" + echoedVoiceLevel);
        animator.SetBool("Sing", true);
        isEchoedVoice = true;
        AStarAIEscap.isCanNotMove = true;
        animator.SetFloat("Speed", 0);
        animator.SetFloat("LookY", -1);

        singCircleObj = Instantiate(singCircle, this.transform.position, Quaternion.identity, transform);
        singCircleObj.SetEchoedVoiceLevel(echoedVoiceLevel);
        singCircleObj.ParentEmpty = this;
    }

    public void SetSingOver()
    {
        if (isEchoedVoice)
        {
            animator.SetBool("Sing", false);
            EchoedVoiceCDTimer = TIME_CD_ECHOEDVOICE;
            isEchoedVoice = false;
            AStarAIEscap.isCanNotMove = false;
        }
    }


    /// <summary>
    /// 判定当前敌人是否可以回声
    /// </summary>
    public override bool isEchoedVoiceisReady()
    {
        if (!isEscape && !isDrop && !isEchoedVoice && !isDie && !isBorn && !isSleepDone && !isEmptyFrozenDone && !isSilence && !isCanNotMoveWhenParalysis && !isFearDone) { return true; }
        else { return false; }
    }

    //=========================唱歌回声相关=============================
    
}
