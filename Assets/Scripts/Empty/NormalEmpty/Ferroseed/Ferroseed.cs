using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ferroseed : Empty
{

    public bool TurnToU;
    public bool TurnToD;
    public bool TurnToL;
    public bool TurnToR;


    bool isRoll;
    bool isRollStart;
    public float RollTimer;

    /// <summary>
    /// 是否在发呆状态
    /// </summary>
    bool isIdle;

    bool isTurn;
    public int TurnCount;


    /// <summary>
    /// 飞弹针
    /// </summary>
    public FerroseedPinMissile Pin;



    /// <summary>
    /// 敌人朝向
    /// </summary>
    Vector2 Director;

    /**
    /// <summary>
    /// 计算当前速度,朝向时，采用的上一时间单位的位置坐标,通过携程执行
    /// </summary>
    Vector3 LastPosition;
    **/

    /// <summary>
    /// 敌人的目标的坐标
    /// </summary>
    public Vector2 TARGET_POSITION
    {
        get { return TargetPosition; }
        set { TargetPosition = value; }
    }
    Vector2 TargetPosition;




    // Start is called before the first frame update
    void Start()
    {
        EmptyType01 = PokemonType.TypeEnum.Grass;
        EmptyType02 = PokemonType.TypeEnum.Steel;
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


        RollTimer = 8.0f;



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
        }
        if (!isBorn && !isDie && !isHit && isRoll && !isSleepDone && !isCanNotMoveWhenParalysis)
        {
            CheckTurn();
        }
    }

    private void FixedUpdate()
    {

        ResetPlayer();
        if (isEmptyInfatuationDone) { UpdateInfatuationDmageCDTimer(); }
        if (!isBorn)
        {
            if (!isSleepDone && !isCanNotMoveWhenParalysis && !isFearDone && !isEmptyFrozenDone && !isSilence)
            {

                animator.ResetTrigger("Sleep");
                if (!isDie && !isHit)
                {
                    if (!isRoll)
                    {
                        RollTimer += Time.deltaTime;
                        if (RollTimer >= 9.0f)
                        {
                            RollTimer = 0.0f;
                            isRollStart = true;
                            animator.SetBool("Turn" , true);
                            isIdle = false;
                            Director = new Vector2Int(Random.Range(0.0f, 1.0f) > 0.5 ? -1 : 1, Random.Range(0.0f, 1.0f) > 0.5 ? -1 : 1);
                        }
                    }
                    else
                    {
                        float RollSpeed = Mathf.Clamp( speed * (Mathf.Pow(1.2f, ((float)TurnCount))) , speed , 8.5f );
                        if (isEmptyConfusionDone) {
                            int c = Mathf.Abs(((transform.GetInstanceID()) / (TurnCount+1)) % 7);
                            RollSpeed = Mathf.Clamp(speed * (Mathf.Pow(1.2f, ((float)c))), speed, 8.5f);
                        }
                        MoveBySpeedAndDir(Director , RollSpeed , 1.0f , 5.0f , 5.0f , 3.5f , 3.5f);
                        //transform.position += new Vector3(Director.x * RollSpeed * Time.deltaTime, Director.y * RollSpeed * Time.deltaTime, 0);
                    }
                }
                Vector3 TargetPosition = player.transform.position;
                if (isSubsititue && SubsititueTarget != null) { TargetPosition = SubsititueTarget.transform.position; }
                if (isEmptyInfatuationDone && InfatuationForDistanceEmpty() != null)
                {
                    TargetPosition = InfatuationForDistanceEmpty().transform.position;
                }

            }
            if ((isSleepDone || isFearDone || isEmptyFrozenDone || isSilence) && !isIdle)
            {
                isIdle = true;
                animator.SetBool("Turn", false);
                animator.SetTrigger("Sleep");
                RollTimer = 0.0f;
                isRoll = false;
                isRollStart = false;
                TurnCount = 0;
                if (isSilence)
                {
                    RollTimer = 7.0f;
                }
                if ((isSleepDone || isFearDone || isEmptyFrozenDone ))
                {
                    RollTimer = 4.0f;
                }
            }
            EmptyBeKnock();
            StateMaterialChange();

        }
    }



    void CheckTurn()
    {
        if (!isTurn)
        {
            if (TurnToU) { Director.y *= -1; isTurn = true; TurnCount += (Random.Range(0.0f, 1.0f) > 0.3f && !isFearDone) ? 1 : 0; Invoke("CallisTurnFalse", 0.2f); }
            if (TurnToD) { Director.y *= -1; isTurn = true; TurnCount += (Random.Range(0.0f, 1.0f) > 0.3f && !isFearDone) ? 1 : 0; Invoke("CallisTurnFalse", 0.2f); }
            if (TurnToR) { Director.x *= -1; isTurn = true; TurnCount += (Random.Range(0.0f, 1.0f) > 0.3f && !isFearDone) ? 1 : 0; Invoke("CallisTurnFalse", 0.2f); }
            if (TurnToL) { Director.x *= -1; isTurn = true; TurnCount += (Random.Range(0.0f, 1.0f) > 0.3f && !isFearDone) ? 1 : 0; Invoke("CallisTurnFalse", 0.2f); }
            if (TurnCount >= (isEmptyConfusionDone ? (Random.Range(7, 13)) : 10))
            {
                //RollOver();
                Invoke("RollOver", 0.7f);
            }
        }
    }

    void RollOver()
    {
        if (isRoll) { LunchPin(); }
        isRoll = false;
        isRollStart = false;
        TurnCount = 0;
        animator.SetBool("Turn", false);
        
    }

    /// <summary>
    /// 结束旋转动画时的事件
    /// </summary>
    void CallTurnStop()
    {
        isIdle = true;
    }

    void CallisTurnFalse()
    {
        isTurn = false;
    }

    public void CallisRoll()
    {
        isRoll = true;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        int dmage = 8 * TurnCount;
        if (other.transform.tag == ("Player"))
        {
            //EmptyTouchHit(other.gameObject);
            PlayerControler playerControler = other.gameObject.GetComponent<PlayerControler>();
            Pokemon.PokemonHpChange(gameObject, other.gameObject, dmage, 0, 0, PokemonType.TypeEnum.Steel);
            if (playerControler != null)
            {
                playerControler.KnockOutPoint = (float)TurnCount * 2;
                playerControler.KnockOutDirection = (playerControler.transform.position - transform.position).normalized;
            }
        }
        if (isEmptyInfatuationDone && other.transform.tag == ("Empty"))
        {
            //InfatuationEmptyTouchHit(other.gameObject);
            Empty e = other.gameObject.GetComponent<Empty>();
            Pokemon.PokemonHpChange(gameObject, e.gameObject, dmage, 0, 0, PokemonType.TypeEnum.Steel);
        }
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



    /// <summary>
    /// 发射飞弹针
    /// </summary>
    public void LunchPin()
    {
        if (!isFearDone) {
            float d1 = 90.0f;
            float d2 = 210.0f;
            float d3 = 330.0f;
            if (isEmptyConfusionDone)
            {
                d1 = 0.0f;
                d2 = 180.0f;
            }

            FerroseedPinMissile p1 = Instantiate(Pin, transform.position + Quaternion.AngleAxis(d1, Vector3.forward) * Vector3.up * 0.5f, Quaternion.Euler(0, 0, d1));
            p1.empty = this;
            FerroseedPinMissile p2 = Instantiate(Pin, transform.position + Quaternion.AngleAxis(d2, Vector3.forward) * Vector3.up * 0.5f, Quaternion.Euler(0, 0, d2));
            p2.empty = this;
            if (!isEmptyConfusionDone) {
                FerroseedPinMissile p3 = Instantiate(Pin, transform.position + Quaternion.AngleAxis(d3, Vector3.forward) * Vector3.up * 0.5f, Quaternion.Euler(0, 0, d3));
                p3.empty = this;
            }
        }
    }

}
