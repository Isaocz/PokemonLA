using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Corbat : Empty
{
    Vector2 Director;
    Vector2 MoveDirector;
    Vector3 TargetPosition;
    int TurnCount;
    bool isAngry;

    Vector3 LastPosition;

    public KrickeyunrMoveShadow MoveShadow;
    float MoveShadowTimer;

    float XCutTimer;
    bool isXCutStart;
    Vector2 XCutDirectior;
    public bool isXcutMove;
    Collider2D CorbatCollider;
    bool isXCutStop;

    bool isPosionDone;
    public CorbatPosionMist PosionMist;
    CorbatPosionMist PosionMistObj;
    Vector3 PosionMistCenter;
    float PosionMistTimer;
    bool isPosionMistFirstMoveDone;
    bool isPosionMistFirstInvicibel;
    int PosionMistXCutCount;

    public CorbatSupersonic SuperSonic;
    float SupersonicOrAirSlashTimer;
    bool isSupersonicBorn;


    public AirSlash CorbatAirSlash;
    bool isAirSlashXCutDone;
    bool isAirSlashBouns;


    float IdleTimer;


    public enum State
    {
        Idle,
        NormalState,
        PosionMistState,
        Supersonic,
        AirSlash,
    }
    public State NowState;

    // Start is called before the first frame update
    void Start()
    {
        EmptyType01 = Type.TypeEnum.Poison;
        EmptyType02 = Type.TypeEnum.Flying;
        player = GameObject.FindObjectOfType<PlayerControler>();
        Emptylevel = SetLevel(player.Level, MaxLevel);
        EmptyHpForLevel(Emptylevel);
        AtkAbilityPoint = AbilityForLevel(Emptylevel, AtkEmptyPoint) * BossAtkBouns;
        SpAAbilityPoint = AbilityForLevel(Emptylevel, SpAEmptyPoint) * BossAtkBouns;
        DefAbilityPoint = AbilityForLevel(Emptylevel, DefEmptyPoint) * BossDefBouns;
        SpdAbilityPoint = AbilityForLevel(Emptylevel, SpdEmptyPoint) * BossDefBouns;
        SpeedAbilityPoint = AbilityForLevel(Emptylevel, SpeedEmptyPoint);
        Exp = BaseExp * Emptylevel / 7;

        //获取刚体目标 动画管理者目标 并让刚体的初始x坐标带入FirstX中
        animator = GetComponent<Animator>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        CorbatCollider = GetComponent<Collider2D>();
        LastPosition = transform.position;
        TurnCount = 0;
        NowState = State.NormalState;

    }


    // Update is called once per frame
    void Update()
    {
        ResetPlayer();
        if (!isDie && !isBorn)
        {
            if (XCutTimer <= 1) { XCutTimer += Time.deltaTime; }
            EmptyDie();
            UpdateEmptyChangeHP();
            StateMaterialChange();
            if (!isEmptyFrozenDone && !isSleepDone && !isCanNotMoveWhenParalysis && !isSilence)
            {

                if (animator.speed == 0) { animator.speed = 1;}
                TargetPosition = player.transform.position;
                if (isSubsititue && SubsititueTarget != null) { TargetPosition = SubsititueTarget.transform.position; }

                switch (NowState)
                {
                    case State.NormalState:
                        //非XCut期间圆圈移动改变朝向
                        if (!isXcutMove)
                        {
                            MoveDirector = ((Vector2)TargetPosition - (Vector2)transform.position).normalized;
                            float Angle = _mTool.Angle_360Y(MoveDirector, Vector2.right);
                            if (Angle >= 0 && Angle < 90) { Director.x = 1; Director.y = 1; }
                            else if (Angle >= 90 && Angle < 180) { Director.x = -1; Director.y = 1; }
                            else if (Angle >= 180 && Angle < 270) { Director.x = -1; Director.y = -1; }
                            else { Director.x = 1; Director.y = -1; }
                        }
                        //非XCut期间按照方向移动
                        if (!isXCutStart)
                        {
                            XCutDirectior = CheckXCutTarget();
                            if (XCutDirectior != Vector2.zero && XCutTimer > 1) { isXCutStart = true; XCutDirectior = Quaternion.AngleAxis( (isEmptyConfusionDone ? Random.Range(-30,30) : 0) , Vector3.forward) * XCutDirectior; animator.SetTrigger("XCut"); }
                            animator.SetFloat("LookX", Director.x); animator.SetFloat("LookY", Director.y);
                            MoveDirector = Quaternion.AngleAxis((isFearDone? (TurnCount % 2 == 0 ? 1 : -1)*95   : (TurnCount % 2 == 0 ? 1 : -1) * (75 - (TurnCount * 10))       ), Vector3.forward) * MoveDirector;
                            if (!isFearDone && TurnCount >= (isAngry ? 2 : 3))
                            {
                                SupersonicOrAirSlashTimer += Time.deltaTime;
                                if (SupersonicOrAirSlashTimer >= 0.8f)
                                {
                                    animator.SetBool("Charge", true);
                                    isXcutMove = false; isXCutStart = false;
                                    TurnCount = 0;
                                    SupersonicOrAirSlashTimer = 0;
                                    if (Random.Range(0.0f, 1.0f) > 0.5f)
                                    {
                                        NowState = State.Supersonic;
                                    }
                                    else
                                    {
                                        NowState = State.AirSlash;
                                    }
                                }
                            }
                            rigidbody2D.position = new Vector2(Mathf.Clamp(rigidbody2D.position.x + (float)MoveDirector.x * Time.deltaTime * speed * (Mathf.Pow(1.2f, TurnCount)) * (isFearDone ? 1.4f : 1), -15f + transform.parent.position.x, 15f + transform.parent.position.x), Mathf.Clamp(rigidbody2D.position.y + (float)MoveDirector.y * Time.deltaTime * speed * (Mathf.Pow(1.2f, TurnCount)) * (isFearDone ? 1.4f : 1), -10f + transform.parent.position.y, 10f + transform.parent.position.y));
                        }
                        if (!isPosionDone && EmptyHp <= maxHP * 0.45f)
                        {
                            isAngry = true;
                            isPosionDone = true;
                            SupersonicOrAirSlashTimer = 0;
                            isXcutMove = false; isXCutStart = false;
                            TurnCount = 0;
                            NowState = State.PosionMistState;
                        }
                        break;

                    case State.PosionMistState:
                        
                        //初始化毒雾状态
                        if (PosionMistTimer == 0)
                        {
                            rigidbody2D.bodyType = RigidbodyType2D.Static;
                            isXcutMove = false; isXCutStart = false;
                            PosionMistObj = Instantiate(PosionMist, transform.position, Quaternion.identity);
                            PosionMistObj.ParentEmpty = this;
                            PosionMistCenter = transform.position;
                            animator.SetBool("Charge", true);
                        }
                        //毒雾爆散开后隐身 并移动到雾中的某一位置
                        if (!isPosionMistFirstMoveDone && PosionMistTimer >= 5.05f)
                        {
                            rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
                            CorbatCollider.enabled = false;
                            animator.SetBool("Charge", false);
                            isPosionMistFirstMoveDone = true;
                            transform.GetChild(3).GetChild(0).gameObject.SetActive(false);
                            transform.GetChild(0).gameObject.SetActive(false);
                            transform.GetChild(1).gameObject.SetActive(false);
                            transform.position = PosionMistFirstMove(PosionMistCenter);
                            Timer.Start(this, 2.8f, () =>
                            {
                                transform.GetChild(3).GetChild(0).gameObject.SetActive(true);
                                transform.GetChild(0).gameObject.SetActive(true);
                                transform.GetChild(1).gameObject.SetActive(true);
                            });
                        }
                        //五秒后使用十字毒刃冲向玩家
                        if (PosionMistTimer >= 10.05f)
                        {
                            if (!isPosionMistFirstInvicibel ){
                                CorbatCollider.enabled = true;
                                isPosionMistFirstInvicibel = true;
                                transform.GetChild(3).GetChild(0).gameObject.SetActive(true);
                                transform.GetChild(0).gameObject.SetActive(true);
                                transform.GetChild(1).gameObject.SetActive(true);
                                isXCutStart = true; animator.SetTrigger("XCut"); PosionMistXCutCount++;
                                XCutDirectior = Quaternion.AngleAxis(Random.Range(-30, 30), Vector3.forward) * XCutDirectior;
                            }
                            if (!isXcutMove) {
                                XCutDirectior = (TargetPosition - transform.position).normalized;
                                animator.SetFloat("LookX", XCutDirectior.x); animator.SetFloat("LookY", XCutDirectior.y);
                            }
                        }
                        //返回毒雾中，返回后重新回到5.05s的状态
                        if (PosionMistTimer >= 10.05f & !isXCutStart)
                        {
                            if (PosionMistXCutCount >= 3 || PosionMistObj == null)
                            {
                                NowState = State.Idle;
                                PosionMistTimer = 0;
                                isPosionMistFirstMoveDone = false;
                                isPosionMistFirstInvicibel = false;
                                PosionMistXCutCount = 0;
                                IdleTimer = 7.0f;
                            }
                            if ((transform.position - PosionMistCenter).magnitude > 8.5f) { MoveDirector = (PosionMistCenter - transform.position).normalized; }
                            else {
                                MoveDirector = (-PosionMistCenter + transform.position).normalized;
                                while ( Mathf.Abs(((Vector2)PosionMistCenter + (Vector2)MoveDirector*8.5f).x) > 12.5 || Mathf.Abs(((Vector2)PosionMistCenter + (Vector2)MoveDirector * 8.5f).y) > 7.0f)
                                {
                                    MoveDirector = (new Vector3(Random.Range(-1.0f , 1.0f) , Random.Range(-1.0f, 1.0f)  , 0)).normalized;
                                }
                            }
                            float Angle = _mTool.Angle_360Y(MoveDirector, Vector2.right);
                            if (Angle >= 0 && Angle < 90) { Director.x = 1; Director.y = 1; }
                            else if (Angle >= 90 && Angle < 180) { Director.x = -1; Director.y = 1; }
                            else if (Angle >= 180 && Angle < 270) { Director.x = -1; Director.y = -1; }
                            else { Director.x = 1; Director.y = -1; }
                            animator.SetFloat("LookX", Director.x); animator.SetFloat("LookY", Director.y);
                            rigidbody2D.position = new Vector2(Mathf.Clamp(rigidbody2D.position.x + (float)MoveDirector.x * Time.deltaTime * speed * (Mathf.Pow(1.2f, TurnCount)) * (isFearDone ? 1.4f : 1), -15f + transform.parent.position.x, 15f + transform.parent.position.x), Mathf.Clamp(rigidbody2D.position.y + (float)MoveDirector.y * Time.deltaTime * speed * (Mathf.Pow(1.2f, TurnCount)) * (isFearDone ? 1.4f : 1), -10f + transform.parent.position.y, 10f + transform.parent.position.y));
                            if (Mathf.Abs((transform.position - PosionMistCenter).magnitude - 8.5f) <= 0.1f)
                            {
                                PosionMistTimer = 5.05f;
                                isPosionMistFirstMoveDone = false;
                                isPosionMistFirstInvicibel = false;
                            }
                        }
                        PosionMistTimer += Time.deltaTime;
                        if (isFearDone) { ResetValue(); }
                        break;
                    case State.Idle:
                        animator.SetFloat("LookX", -1);
                        animator.SetFloat("LookY", -1);
                        IdleTimer -= Time.deltaTime;
                        if (IdleTimer <= 0) { IdleTimer = 0; NowState = State.NormalState; XCutTimer = 0; TurnCount = 0; }
                        if (isFearDone) { ResetValue(); }
                        break;
                    case State.Supersonic:
                        SupersonicOrAirSlashTimer += Time.deltaTime;
                        if (!isSupersonicBorn && SupersonicOrAirSlashTimer >= 0.3f) { isSupersonicBorn = true; Instantiate(SuperSonic, transform.position, Quaternion.Euler(0, 0, _mTool.Angle_360Y(Quaternion.AngleAxis(isEmptyConfusionDone ? Random.Range(-30, 30) : 0, Vector3.forward) * (TargetPosition - transform.position), Vector3.right))); animator.SetBool("Charge", false); }
                        if (SupersonicOrAirSlashTimer >= 3.5) { SupersonicOrAirSlashTimer = 0; NowState = State.NormalState; TurnCount = 0; isSupersonicBorn = false; XCutTimer = 0; }
                        if (isFearDone) { ResetValue(); }
                        break;
                    case State.AirSlash:
                        speed = 11;
                        isAirSlashBouns = true;
                        SupersonicOrAirSlashTimer += Time.deltaTime;
                        //发射空气斩
                        if (!isSupersonicBorn && SupersonicOrAirSlashTimer >= 0.3f)
                        {
                            isSupersonicBorn = true;
                            animator.SetBool("Charge", false);
                            Vector2 dir = ((TargetPosition - transform.position).normalized).normalized;
                            if ( isEmptyConfusionDone ) { dir = Quaternion.AngleAxis(Random.Range(-30, 30), Vector3.forward) * dir; }
                            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90;
                            AirSlash airSlash = Instantiate(CorbatAirSlash, transform.position + new Vector3(dir.x, dir.y) * 1, Quaternion.Euler(0f, 0f, angle));
                            airSlash.empty = this;
                            Timer.Start(this, 0.15f, () =>
                            {
                                if (airSlash)
                                {
                                    airSlash.LaunchNotForce(dir, 10);
                                    if (rigidbody2D != null) { rigidbody2D.AddForce(-dir * 1600); }
                                }
                            });
                        }
                        //被击退后开始曲线运动
                        if (SupersonicOrAirSlashTimer >= 0.7f)
                        {
                            MoveDirector = ((Vector2)TargetPosition - (Vector2)transform.position).normalized;
                            float Angle = _mTool.Angle_360Y(MoveDirector, Vector2.right);
                            if (Angle >= 0 && Angle < 90) { Director.x = 1; Director.y = 1; }
                            else if (Angle >= 90 && Angle < 180) { Director.x = -1; Director.y = 1; }
                            else if (Angle >= 180 && Angle < 270) { Director.x = -1; Director.y = -1; }
                            else { Director.x = 1; Director.y = -1; }
                            animator.SetFloat("LookX", Director.x); animator.SetFloat("LookY", Director.y);
                            MoveDirector = Quaternion.AngleAxis(-70, Vector3.forward) * MoveDirector;
                            rigidbody2D.position = new Vector2(Mathf.Clamp(rigidbody2D.position.x + (float)MoveDirector.x * Time.deltaTime * speed * (isAirSlashBouns ? 1.85f : 1) * (Mathf.Pow(1.2f, TurnCount)) * (isFearDone ? 1.4f : 1), -15f + transform.parent.position.x, 15f + transform.parent.position.x), Mathf.Clamp(rigidbody2D.position.y + (float)MoveDirector.y * Time.deltaTime * speed * (isAirSlashBouns ? 1.85f : 1) * (Mathf.Pow(1.2f, TurnCount)) * (isFearDone ? 1.4f : 1), -10f + transform.parent.position.y, 10f + transform.parent.position.y));
                        }
                        if (SupersonicOrAirSlashTimer >= 1.2f)
                        {
                            if (!isAirSlashXCutDone) {
                                isAirSlashXCutDone = true;
                                isXCutStart = true; animator.SetTrigger("XCut");
                                XCutDirectior = Quaternion.AngleAxis(Random.Range(-30, 30), Vector3.forward) * XCutDirectior;
                            }
                            if (!isXcutMove) { XCutDirectior = (TargetPosition - transform.position).normalized; }
                        }
                        if (SupersonicOrAirSlashTimer >= 2.5f)
                        {
                            isAirSlashXCutDone = false; isAirSlashBouns = false;
                             SupersonicOrAirSlashTimer = 0; NowState = State.Idle; TurnCount = 0; isSupersonicBorn = false; XCutTimer = 0; IdleTimer = 7.0f;
                        }
                        if (isFearDone) { ResetValue(); }
                        break;
                }
                


                //XCut期间直线移动
                if (isXcutMove && !isXCutStop)
                {
                    TurnCount = 0;
                    MoveShadowTimer += Time.deltaTime;
                    if (MoveShadowTimer >= 0.02f)
                    {
                        MoveShadowTimer = 0;
                        KrickeyunrMoveShadow S = Instantiate(MoveShadow, transform.position + 0.6f * Vector3.down, Quaternion.identity);
                        S.sprite01.sprite = skinRenderers[0].sprite;
                    }
                    XCutDirectior = (XCutDirectior).normalized;
                    float Angle = _mTool.Angle_360Y(XCutDirectior, Vector2.right);
                    if (Angle >= 0 && Angle < 90) { Director.x = 1; Director.y = 1; }
                    else if (Angle >= 90 && Angle < 180) { Director.x = -1; Director.y = 1; }
                    else if (Angle >= 180 && Angle < 270) { Director.x = -1; Director.y = -1; }
                    else { Director.x = 1; Director.y = -1; }
                    if (isFearDone) { Director = -Director; }
                    animator.SetFloat("LookX", Director.x); animator.SetFloat("LookY", Director.y);
                    
                    rigidbody2D.position = new Vector2(Mathf.Clamp(rigidbody2D.position.x + (float)XCutDirectior.x * (isFearDone? -1 : 1) * Time.deltaTime * (isAirSlashBouns ? 1.85f : 1) * speed * 5.0f, -13f + transform.parent.position.x, 13f + transform.parent.position.x), Mathf.Clamp(rigidbody2D.position.y + (float)XCutDirectior.y * Time.deltaTime * (isFearDone ? -1 : 1) * speed * (isAirSlashBouns ? 1.85f : 1) * 5.0f, -8 + transform.parent.position.y, 8 + transform.parent.position.y));
                }
            }

            if (isSilence)
            {
                animator.SetFloat("LookX", -1);
                animator.SetFloat("LookY", -1);
            }
            if (isSleepDone || isEmptyFrozenDone)
            {
                ResetValue();
            }
        }

    }

    void ResetValue()
    {
        transform.GetChild(3).GetChild(0).gameObject.SetActive(true);
        transform.GetChild(0).gameObject.SetActive(true);
        transform.GetChild(1).gameObject.SetActive(true);
        CorbatCollider.enabled = true; isAirSlashBouns = false;
        animator.SetBool("Charge", false); rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
        IdleTimer = 0; NowState = State.NormalState; XCutTimer = 0; TurnCount = 0;
        isXCutStart = false; isXcutMove = false; isXCutStop = false; isPosionDone = false;
        PosionMistObj = null; PosionMistCenter = Vector3.zero; PosionMistTimer = 0; isPosionMistFirstMoveDone = false;
        isPosionMistFirstInvicibel = false; PosionMistXCutCount = 0; SupersonicOrAirSlashTimer = 0; isSupersonicBorn = false; isAirSlashXCutDone = false;
    }


    Vector3 PosionMistFirstMove( Vector3 Center )
    {
        Vector3 OutPut = Vector3.zero;
        int a = 0;
        if ( (Center - TargetPosition).magnitude <= 8.5f)
        {
            a = Random.Range(0,360);
        }
        else
        {
            if (Center.x >= TargetPosition.x) { a = Random.Range(90, 270); }
            else { a = Random.Range(-90, 90); }
        }
        OutPut = Center + new Vector3(Mathf.Sin(a) * 8.5f, Mathf.Cos(a) * 8.5f, 0);

        while ( Mathf.Abs(OutPut.x- transform.parent.position.x) > 12 || Mathf.Abs(OutPut.y - transform.parent.position.y) > 7.5f)
        {
            if ((Center - TargetPosition).magnitude <= 8.5f)
            {
                a = Random.Range(0, 360);
            }
            else
            {
                if (Center.x >= TargetPosition.x) { a = Random.Range(90, 270); }
                else { a = Random.Range(-90, 90); }
            }
            OutPut = Center + new Vector3(Mathf.Cos(a) * 8.5f, Mathf.Sin(a) * 8.5f, 0);
        }
        Debug.Log(a);
        return OutPut;
    }


    private void FixedUpdate()
    {

        ResetPlayer();
        if (!isDie && !isBorn)
        {
            EmptyBeKnock();
            animator.SetFloat("Speed", (transform.position - LastPosition).magnitude * 100);
            LastPosition = transform.position;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.tag == ("Player"))
        {
            if (isXcutMove)
            {
                PlayerControler playerControler = other.gameObject.GetComponent<PlayerControler>();
                if (playerControler != null)
                {
                    playerControler.KnockOutPoint = Knock;
                    playerControler.KnockOutDirection = (playerControler.transform.position - transform.position).normalized;
                }
                if (Random.Range(0.0f , 1.0f) > 0.8f) { playerControler.ToxicFloatPlus(0.5f); }
                Pokemon.PokemonHpChange(this.gameObject, other.gameObject, 70, 0, 0, Type.TypeEnum.Poison);
            }
            else
            {
                EmptyTouchHit(other.gameObject);
            }
        }
        else if (other.transform.tag == ("Room"))
        {
            TurnCount++;
            if (isXcutMove) { isXCutStop = true; }
        }
    }

    public void CallXCutStartFalse()
    {
        isXCutStart = false;
        isXCutStop = false;
        TurnCount = 0;
        XCutTimer = 0;
    }
    Vector2 CheckXCutTarget()
    {

        RaycastHit2D SearchPlayerL01 = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y), Vector3.left, 6.5f, LayerMask.GetMask("Player", "Enviroment", "Room", "PlayerFly"));
        RaycastHit2D SearchPlayerL02 = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + 0.25f), Vector3.left, 6.5f, LayerMask.GetMask("Player", "Enviroment", "Room", "PlayerFly"));
        RaycastHit2D SearchPlayerL03 = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + 0.50f), Vector3.left, 6.5f, LayerMask.GetMask("Player", "Enviroment", "Room", "PlayerFly"));
        RaycastHit2D SearchPlayerL04 = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + 0.75f), Vector3.left, 6.5f, LayerMask.GetMask("Player", "Enviroment", "Room", "PlayerFly"));
        RaycastHit2D SearchPlayerL05 = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y - 0.25f), Vector3.left, 6.5f, LayerMask.GetMask("Player", "Enviroment", "Room", "PlayerFly"));
        RaycastHit2D SearchPlayerL06 = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y - 0.50f), Vector3.left, 6.5f, LayerMask.GetMask("Player", "Enviroment", "Room", "PlayerFly"));
        RaycastHit2D SearchPlayerL07 = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y - 0.75f), Vector3.left, 6.5f, LayerMask.GetMask("Player", "Enviroment", "Room", "PlayerFly"));

        if (SearchPlayerL01.collider != null && SearchPlayerL01.transform.tag == "Player" && (((isSubsititue && SubsititueTarget != null) && (SearchPlayerL01.collider.gameObject == SubsititueTarget.gameObject)) || ((!isSubsititue || SubsititueTarget == null) && SearchPlayerL01.collider.gameObject == player.gameObject))
            || SearchPlayerL02.collider != null && SearchPlayerL02.transform.tag == "Player" && (((isSubsititue && SubsititueTarget != null) && (SearchPlayerL02.collider.gameObject == SubsititueTarget.gameObject)) || ((!isSubsititue || SubsititueTarget == null) && SearchPlayerL02.collider.gameObject == player.gameObject))
            || SearchPlayerL03.collider != null && SearchPlayerL03.transform.tag == "Player" && (((isSubsititue && SubsititueTarget != null) && (SearchPlayerL03.collider.gameObject == SubsititueTarget.gameObject)) || ((!isSubsititue || SubsititueTarget == null) && SearchPlayerL03.collider.gameObject == player.gameObject))
            || SearchPlayerL04.collider != null && SearchPlayerL04.transform.tag == "Player" && (((isSubsititue && SubsititueTarget != null) && (SearchPlayerL04.collider.gameObject == SubsititueTarget.gameObject)) || ((!isSubsititue || SubsititueTarget == null) && SearchPlayerL04.collider.gameObject == player.gameObject))
            || SearchPlayerL05.collider != null && SearchPlayerL05.transform.tag == "Player" && (((isSubsititue && SubsititueTarget != null) && (SearchPlayerL05.collider.gameObject == SubsititueTarget.gameObject)) || ((!isSubsititue || SubsititueTarget == null) && SearchPlayerL05.collider.gameObject == player.gameObject))
            || SearchPlayerL06.collider != null && SearchPlayerL06.transform.tag == "Player" && (((isSubsititue && SubsititueTarget != null) && (SearchPlayerL06.collider.gameObject == SubsititueTarget.gameObject)) || ((!isSubsititue || SubsititueTarget == null) && SearchPlayerL06.collider.gameObject == player.gameObject))
            || SearchPlayerL07.collider != null && SearchPlayerL07.transform.tag == "Player" && (((isSubsititue && SubsititueTarget != null) && (SearchPlayerL07.collider.gameObject == SubsititueTarget.gameObject)) || ((!isSubsititue || SubsititueTarget == null) && SearchPlayerL07.collider.gameObject == player.gameObject)))
        {


            return Vector2.left;
        }

        RaycastHit2D SearchPlayerR01 = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y), Vector3.right, 6.5f, LayerMask.GetMask("Player", "Enviroment", "Room", "PlayerFly"));
        RaycastHit2D SearchPlayerR02 = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + 0.25f), Vector3.right, 6.5f, LayerMask.GetMask("Player", "Enviroment", "Room", "PlayerFly"));
        RaycastHit2D SearchPlayerR03 = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + 0.50f), Vector3.right, 6.5f, LayerMask.GetMask("Player", "Enviroment", "Room", "PlayerFly"));
        RaycastHit2D SearchPlayerR04 = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + 0.75f), Vector3.right, 6.5f, LayerMask.GetMask("Player", "Enviroment", "Room", "PlayerFly"));
        RaycastHit2D SearchPlayerR05 = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y - 0.25f), Vector3.right, 6.5f, LayerMask.GetMask("Player", "Enviroment", "Room", "PlayerFly"));
        RaycastHit2D SearchPlayerR06 = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y - 0.50f), Vector3.right, 6.5f, LayerMask.GetMask("Player", "Enviroment", "Room", "PlayerFly"));
        RaycastHit2D SearchPlayerR07 = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y - 0.75f), Vector3.right, 6.5f, LayerMask.GetMask("Player", "Enviroment", "Room", "PlayerFly"));

        if (SearchPlayerR01.collider != null && SearchPlayerR01.transform.tag == "Player" && (((isSubsititue && SubsititueTarget != null) && (SearchPlayerR01.collider.gameObject == SubsititueTarget.gameObject)) || ((!isSubsititue || SubsititueTarget == null) && SearchPlayerR01.collider.gameObject == player.gameObject))
            || SearchPlayerR02.collider != null && SearchPlayerR02.transform.tag == "Player" && (((isSubsititue && SubsititueTarget != null) && (SearchPlayerR02.collider.gameObject == SubsititueTarget.gameObject)) || ((!isSubsititue || SubsititueTarget == null) && SearchPlayerR02.collider.gameObject == player.gameObject))
            || SearchPlayerR03.collider != null && SearchPlayerR03.transform.tag == "Player" && (((isSubsititue && SubsititueTarget != null) && (SearchPlayerR03.collider.gameObject == SubsititueTarget.gameObject)) || ((!isSubsititue || SubsititueTarget == null) && SearchPlayerR03.collider.gameObject == player.gameObject))
            || SearchPlayerR04.collider != null && SearchPlayerR04.transform.tag == "Player" && (((isSubsititue && SubsititueTarget != null) && (SearchPlayerR04.collider.gameObject == SubsititueTarget.gameObject)) || ((!isSubsititue || SubsititueTarget == null) && SearchPlayerR04.collider.gameObject == player.gameObject))
            || SearchPlayerR05.collider != null && SearchPlayerR05.transform.tag == "Player" && (((isSubsititue && SubsititueTarget != null) && (SearchPlayerR05.collider.gameObject == SubsititueTarget.gameObject)) || ((!isSubsititue || SubsititueTarget == null) && SearchPlayerR05.collider.gameObject == player.gameObject))
            || SearchPlayerR06.collider != null && SearchPlayerR06.transform.tag == "Player" && (((isSubsititue && SubsititueTarget != null) && (SearchPlayerR06.collider.gameObject == SubsititueTarget.gameObject)) || ((!isSubsititue || SubsititueTarget == null) && SearchPlayerR06.collider.gameObject == player.gameObject))
            || SearchPlayerR07.collider != null && SearchPlayerR07.transform.tag == "Player" && (((isSubsititue && SubsititueTarget != null) && (SearchPlayerR07.collider.gameObject == SubsititueTarget.gameObject)) || ((!isSubsititue || SubsititueTarget == null) && SearchPlayerR07.collider.gameObject == player.gameObject)))
        {
            return Vector2.right;
        }

        RaycastHit2D SearchPlayerU01 = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y), Vector3.up, 6.5f, LayerMask.GetMask("Player", "Enviroment", "Room", "PlayerFly"));
        RaycastHit2D SearchPlayerU02 = Physics2D.Raycast(new Vector2(transform.position.x + 0.25f, transform.position.y), Vector3.up, 6.5f, LayerMask.GetMask("Player", "Enviroment", "Room", "PlayerFly"));
        RaycastHit2D SearchPlayerU03 = Physics2D.Raycast(new Vector2(transform.position.x + 0.50f, transform.position.y), Vector3.up, 6.5f, LayerMask.GetMask("Player", "Enviroment", "Room", "PlayerFly"));
        RaycastHit2D SearchPlayerU04 = Physics2D.Raycast(new Vector2(transform.position.x + 0.75f, transform.position.y), Vector3.up, 6.5f, LayerMask.GetMask("Player", "Enviroment", "Room", "PlayerFly"));
        RaycastHit2D SearchPlayerU05 = Physics2D.Raycast(new Vector2(transform.position.x - 0.25f, transform.position.y), Vector3.up, 6.5f, LayerMask.GetMask("Player", "Enviroment", "Room", "PlayerFly"));
        RaycastHit2D SearchPlayerU06 = Physics2D.Raycast(new Vector2(transform.position.x - 0.50f, transform.position.y), Vector3.up, 6.5f, LayerMask.GetMask("Player", "Enviroment", "Room", "PlayerFly"));
        RaycastHit2D SearchPlayerU07 = Physics2D.Raycast(new Vector2(transform.position.x - 0.75f, transform.position.y), Vector3.up, 6.5f, LayerMask.GetMask("Player", "Enviroment", "Room", "PlayerFly"));

        if (SearchPlayerU01.collider != null && SearchPlayerU01.transform.tag == "Player" && (((isSubsititue && SubsititueTarget != null) && (SearchPlayerU01.collider.gameObject == SubsititueTarget.gameObject)) || ((!isSubsititue || SubsititueTarget == null) && SearchPlayerU01.collider.gameObject == player.gameObject))
            || SearchPlayerU02.collider != null && SearchPlayerU02.transform.tag == "Player" && (((isSubsititue && SubsititueTarget != null) && (SearchPlayerU02.collider.gameObject == SubsititueTarget.gameObject)) || ((!isSubsititue || SubsititueTarget == null) && SearchPlayerU02.collider.gameObject == player.gameObject))
            || SearchPlayerU03.collider != null && SearchPlayerU03.transform.tag == "Player" && (((isSubsititue && SubsititueTarget != null) && (SearchPlayerU03.collider.gameObject == SubsititueTarget.gameObject)) || ((!isSubsititue || SubsititueTarget == null) && SearchPlayerU03.collider.gameObject == player.gameObject))
            || SearchPlayerU04.collider != null && SearchPlayerU04.transform.tag == "Player" && (((isSubsititue && SubsititueTarget != null) && (SearchPlayerU04.collider.gameObject == SubsititueTarget.gameObject)) || ((!isSubsititue || SubsititueTarget == null) && SearchPlayerU04.collider.gameObject == player.gameObject))
            || SearchPlayerU05.collider != null && SearchPlayerU05.transform.tag == "Player" && (((isSubsititue && SubsititueTarget != null) && (SearchPlayerU05.collider.gameObject == SubsititueTarget.gameObject)) || ((!isSubsititue || SubsititueTarget == null) && SearchPlayerU05.collider.gameObject == player.gameObject))
            || SearchPlayerU06.collider != null && SearchPlayerU06.transform.tag == "Player" && (((isSubsititue && SubsititueTarget != null) && (SearchPlayerU06.collider.gameObject == SubsititueTarget.gameObject)) || ((!isSubsititue || SubsititueTarget == null) && SearchPlayerU06.collider.gameObject == player.gameObject))
            || SearchPlayerU07.collider != null && SearchPlayerU07.transform.tag == "Player" && (((isSubsititue && SubsititueTarget != null) && (SearchPlayerU07.collider.gameObject == SubsititueTarget.gameObject)) || ((!isSubsititue || SubsititueTarget == null) && SearchPlayerU07.collider.gameObject == player.gameObject)))
        {
            return Vector2.up;
        }

        RaycastHit2D SearchPlayerD01 = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y), Vector3.down, 6.5f, LayerMask.GetMask("Player", "Enviroment", "Room", "PlayerFly"));
        RaycastHit2D SearchPlayerD02 = Physics2D.Raycast(new Vector2(transform.position.x + 0.25f, transform.position.y), Vector3.down, 6.5f, LayerMask.GetMask("Player", "Enviroment", "Room", "PlayerFly"));
        RaycastHit2D SearchPlayerD03 = Physics2D.Raycast(new Vector2(transform.position.x + 0.50f, transform.position.y), Vector3.down, 6.5f, LayerMask.GetMask("Player", "Enviroment", "Room", "PlayerFly"));
        RaycastHit2D SearchPlayerD04 = Physics2D.Raycast(new Vector2(transform.position.x + 0.75f, transform.position.y), Vector3.down, 6.5f, LayerMask.GetMask("Player", "Enviroment", "Room", "PlayerFly"));
        RaycastHit2D SearchPlayerD05 = Physics2D.Raycast(new Vector2(transform.position.x - 0.25f, transform.position.y), Vector3.down, 6.5f, LayerMask.GetMask("Player", "Enviroment", "Room", "PlayerFly"));
        RaycastHit2D SearchPlayerD06 = Physics2D.Raycast(new Vector2(transform.position.x - 0.50f, transform.position.y), Vector3.down, 6.5f, LayerMask.GetMask("Player", "Enviroment", "Room", "PlayerFly"));
        RaycastHit2D SearchPlayerD07 = Physics2D.Raycast(new Vector2(transform.position.x - 0.75f, transform.position.y), Vector3.down, 6.5f, LayerMask.GetMask("Player", "Enviroment", "Room", "PlayerFly"));

        if (SearchPlayerD01.collider != null && SearchPlayerD01.transform.tag == "Player" && (((isSubsititue && SubsititueTarget != null) && (SearchPlayerD01.collider.gameObject == SubsititueTarget.gameObject)) || ((!isSubsititue || SubsititueTarget == null) && SearchPlayerD01.collider.gameObject == player.gameObject))
            || SearchPlayerD02.collider != null && SearchPlayerD02.transform.tag == "Player" && (((isSubsititue && SubsititueTarget != null) && (SearchPlayerD02.collider.gameObject == SubsititueTarget.gameObject)) || ((!isSubsititue || SubsititueTarget == null) && SearchPlayerD02.collider.gameObject == player.gameObject))
            || SearchPlayerD03.collider != null && SearchPlayerD03.transform.tag == "Player" && (((isSubsititue && SubsititueTarget != null) && (SearchPlayerD03.collider.gameObject == SubsititueTarget.gameObject)) || ((!isSubsititue || SubsititueTarget == null) && SearchPlayerD03.collider.gameObject == player.gameObject))
            || SearchPlayerD04.collider != null && SearchPlayerD04.transform.tag == "Player" && (((isSubsititue && SubsititueTarget != null) && (SearchPlayerD04.collider.gameObject == SubsititueTarget.gameObject)) || ((!isSubsititue || SubsititueTarget == null) && SearchPlayerD04.collider.gameObject == player.gameObject))
            || SearchPlayerD05.collider != null && SearchPlayerD05.transform.tag == "Player" && (((isSubsititue && SubsititueTarget != null) && (SearchPlayerD05.collider.gameObject == SubsititueTarget.gameObject)) || ((!isSubsititue || SubsititueTarget == null) && SearchPlayerD05.collider.gameObject == player.gameObject))
            || SearchPlayerD06.collider != null && SearchPlayerD06.transform.tag == "Player" && (((isSubsititue && SubsititueTarget != null) && (SearchPlayerD06.collider.gameObject == SubsititueTarget.gameObject)) || ((!isSubsititue || SubsititueTarget == null) && SearchPlayerD06.collider.gameObject == player.gameObject))
            || SearchPlayerD07.collider != null && SearchPlayerD07.transform.tag == "Player" && (((isSubsititue && SubsititueTarget != null) && (SearchPlayerD07.collider.gameObject == SubsititueTarget.gameObject)) || ((!isSubsititue || SubsititueTarget == null) && SearchPlayerD07.collider.gameObject == player.gameObject)))
        {
            return Vector2.down;
        }


        return Vector2.zero;
    }
}
