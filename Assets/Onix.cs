using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Onix : Empty
{


    public Vector2 Director;
    Vector3 TargetPosition;
    //float InvinicibleTimer;
    //float TimeInvinicible;
    OnixBodyShadow OnixBodyTop;
    float CalibrationPositionTimer;
    Room ParentRoom;

    bool isAngry;


    List<Vector3> BodyPositionSL = new List<Vector3>
    {
        new Vector3 ( 0.0f , 0.0f , 9.337f ) ,new Vector3 ( 0.5387115f , 0.5324163f , -8.164f ) ,new Vector3 ( 1.131994f , 1.137687f , -11.501f ) ,new Vector3 ( 1.601007f , 1.603853f , 1.741f ) ,new Vector3 ( 2.078749f , 2.081404f , -5.665f ) ,
        new Vector3 ( 2.539835f , 2.545402f , 0.405f ) ,new Vector3 ( 2.907215f , 2.910489f , 8.566f ) ,new Vector3 ( 3.425272f , 3.425938f , 8.66f ) ,new Vector3 ( 3.786847f , 3.797668f , 9.806f ) ,new Vector3 ( 4.101731f , 4.114922f , 4.435f ) ,
        new Vector3 ( 4.421704f , 4.429767f , 3.98f ) ,new Vector3 ( 4.670557f , 4.673618f , 10.445f ) ,new Vector3 ( 4.938525f , 4.941389f , -5.512f ) ,new Vector3 ( 5.193859f , 5.202371f , 9.646f ) ,new Vector3 ( 5.449053f , 5.454249f , 4.99f ) ,
        new Vector3 ( 5.645489f , 5.647483f , 8.818f ) ,new Vector3 ( 5.765093f , 5.765412f , 9.81f ) ,new Vector3 ( 5.873575f , 5.877534f , 0.0f )
    };
    List<Vector3> BodyPositionSR = new List<Vector3>
    {
        new Vector3 ( 0.0f , 0.0f , 100.407f ) ,new Vector3 ( -0.5253296f , 0.5323792f , 81.217f ) ,new Vector3 ( -1.09013f , 1.109592f , 77.86f ) ,new Vector3 ( -1.539925f , 1.56792f , 91.287f ) ,new Vector3 ( -2.004392f , 2.043923f , 82.824f ) ,
        new Vector3 ( -2.444695f , 2.499022f , 89.368f ) ,new Vector3 ( -2.782534f , 2.84841f , 97.535f ) ,new Vector3 ( -3.292412f , 3.37397f , 96.483f ) ,new Vector3 ( -3.642521f , 3.73279f , 96.564f ) ,new Vector3 ( -4.024558f , 4.047782f , 91.219f ) ,
        new Vector3 ( -4.216694f , 4.341998f , 92.616f ) ,new Vector3 ( -4.415884f , 4.551056f , 100.078f ) ,new Vector3 ( -4.627419f , 4.769031f , 77.693f ) ,new Vector3 ( -4.821568f , 4.965748f , 98.166f ) ,new Vector3 ( -5.1f , 5.186281f , 92.543f ) ,
        new Vector3 ( -5.185859f , 5.360336f , 98.877f ) ,new Vector3 ( -5.284847f , 5.466155f , 96.151f ) ,new Vector3 ( -5.39992f , 5.582242f , 0.0f )
    };
    List<Vector3> BodyPositionBL = new List<Vector3>
    {
        new Vector3 ( 0.0f , 0.0f , -79.569f ) ,new Vector3 ( 0.5237846f , -0.530941f , -99.816f ) ,new Vector3 ( 1.081894f , -1.122769f , -103.312f ) ,new Vector3 ( 1.529011f , -1.600444f , -91.419f ) ,new Vector3 ( 1.978621f , -2.103881f , -98.343f ) ,
        new Vector3 ( 2.421096f , -2.586145f , -90.973f ) ,new Vector3 ( 2.781038f , -2.966007f , -83.856f ) ,new Vector3 ( 3.278985f , -3.510703f , -92.447f ) ,new Vector3 ( 3.57936f , -3.94316f , -88.944f ) ,new Vector3 ( 3.849605f , -4.308763f , -74.67f ) ,
        new Vector3 ( 4.226536f , -4.571086f , -73.712f ) ,new Vector3 ( 4.525435f , -4.767829f , -66.703f ) ,new Vector3 ( 4.852585f , -4.973877f , -83.645f ) ,new Vector3 ( 5.167604f , -5.174687f , -70.734f ) ,new Vector3 ( 5.461758f , -5.386539f , -74.586f ) ,
        new Vector3 ( 5.689465f , -5.546322f , -70.211f ) ,new Vector3 ( 5.829055f , -5.642061f , -67.902f ) ,new Vector3 ( 5.961698f , -5.724193f , 0.0f )
    };
    List<Vector3> BodyPositionBR = new List<Vector3>
    {
        new Vector3 ( 0.0f , 0.0f , -166.343f ) ,new Vector3 ( -0.5007095f , -0.5530157f , 166.595f ) ,new Vector3 ( -1.12553f , -1.071918f , 170.572f ) ,new Vector3 ( -1.571569f , -1.549938f , -173.89f ) ,new Vector3 ( -2.012486f , -2.062023f , 178.885f ) ,
        new Vector3 ( -2.436981f , -2.561802f , -177.302f ) ,new Vector3 ( -2.791319f , -2.947653f , -169.203f ) ,new Vector3 ( -3.295116f , -3.485638f , -172.98f ) ,new Vector3 ( -3.666327f , -3.852791f , -169.833f ) ,new Vector3 ( -3.978693f , -4.169188f , -172.357f ) ,
        new Vector3 ( -4.278411f , -4.503508f , -173.301f ) ,new Vector3 ( -4.515337f , -4.759399f , -166.883f ) ,new Vector3 ( -4.772206f , -5.040583f , 174.468f ) ,new Vector3 ( -5.030421f , -5.304748f , -172.001f ) ,new Vector3 ( -5.295437f , -5.551644f , -175.891f ) ,
        new Vector3 ( -5.49692f , -5.743516f , -176.724f ) ,new Vector3 ( -5.628447f , -5.850085f , 175.592f ) ,new Vector3 ( -5.761145f , -5.93213f , 0.0f )
    };
    List<Vector3> MoveOutPosition = new List<Vector3>
    {
        new Vector3 ( 10.8f , -5.0f , 0 ) ,new Vector3 ( 6.2f , -5.0f , 0 ) , new Vector3 ( -10.8f , -5.0f , 0 ) ,new Vector3 ( -6.2f , -5.0f , 0 ) ,
        new Vector3 ( 10.8f , 5.6f , 0 ) ,new Vector3 ( 6.2f , 5.6f , 0 ) , new Vector3 ( -10.8f , 5.6f , 0 ) ,new Vector3 ( -6.2f , 5.6f , 0 ) ,
        new Vector3 ( 10.4f , 5.3f , 0 ) ,new Vector3 ( 10.4f , -5.0f , 0 ) ,new Vector3 ( -10.4f , 5.3f , 0 ) ,new Vector3 ( -10.4f , -5.0f , 0 ) ,
        new Vector3 ( 0.0f , 5.6f , 0 ) ,new Vector3 ( 0.0f , -4.8f , 0 ) ,new Vector3 ( 10.4f , 0 , 0 ) ,new Vector3 ( -10.4f , 0 , 0 ) ,
    };
    List<ParticleSystem> MovePSList = new List<ParticleSystem> { };

    public enum State
    {
        NormalState,
        MoveIntoWallState,
        MoveOutWallState,
        Idle,
        BodyPress,
    }
    public State NowState;




    int SpeedAlpha;
    float SpeedAChangeTimer;
    bool isSpeedAChange;

    bool isMoveIntoWallStateIns;
    public OnixDigAnimatorPause OnixDigEffect;

    float MoveOutTimer;
    OnixDigAnimatorPause DigOutEffect;
    bool isTopHeadMoveOut;
    bool isAllBodyMoveOut;

    public List<Collider2D> IgnoreColliderList = new List<Collider2D> { };

    float IdleTime;



    public OnixRockTomb RockTomb;
    float RockTombCDTimer;
    bool isRockTombCD;

    public OnixStoneEdgeManger StoneEdge;
    public OnixTailDig TaigDig;
    bool isDig01Done;
    bool isDig02Done;
    bool isDig03Done;


    public bool isIronHead
    {
        get { return isih; }
        set { isih = value; }
    }
    bool isih = false;
    ParticleSystem IHShinePS;


    Vector2 BodyPressCenter;
    bool isBodyPressIns;
    float BodyPressFloatTimer;
    float BodyPressR;
    float BodyPressAlphaAngel;
    int BodyPressD;
    Vector2 BodyPressNowPosition;
    bool isBodyPressJump;
    Vector2 BodyPressJumpCenter;
    public GameObject BodyPressJumpCollider;
    Vector2 BodyPressTargetPosition;
    bool ResetBody;
    public OnixBodyPressCollider BodyPressCollider;
    OnixBodyPressCollider bp;
    public bool isBodyPressDamage;
    public OnixRStoneEdge RStoneEdge;
    OnixRStoneEdge RStoneEdgeObj;

    // Start is called before the first frame update
    void Start()
    {
        EmptyType01 = Type.TypeEnum.Rock;
        EmptyType02 = Type.TypeEnum.Ground;
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
        for (int i = 0; i < transform.GetChild(3).GetChild(0).childCount; i++)
        {
            SubEmptyBodyList.Add(transform.GetChild(3).GetChild(0).GetChild(i).GetComponent<OnixBodyShadow>());
            if (transform.GetChild(3).GetChild(0).GetChild(i).GetChild(0).childCount != 0) {
                MovePSList.Add(transform.GetChild(3).GetChild(0).GetChild(i).GetChild(0).GetChild(0).GetComponent<ParticleSystem>());
                MovePSList.Add(transform.GetChild(3).GetChild(0).GetChild(i).GetChild(0).GetChild(1).GetComponent<ParticleSystem>());
            }
        }
        //TimeInvinicible = 0.45f;
        OnixBodyTop = SubEmptyBodyList[0].GetComponent<OnixBodyShadow>();
        for (int i = 0; i < SubEmptyBodyList.Count; i++)
        {
            for (int j = i + 1; j < SubEmptyBodyList.Count; j++)
            {
                Physics2D.IgnoreCollision(SubEmptyBodyList[i].GetComponent<OnixBodyShadow>().BodyCollider2D, SubEmptyBodyList[j].GetComponent<OnixBodyShadow>().BodyCollider2D);
            }
        }
        NowState = State.NormalState;
        ParentRoom = transform.parent.parent.GetComponent<Room>();
        SpeedAlpha = 0;
        RandomD();
        SetNowDirAndSubBodyPos(Director);
        IHShinePS = transform.GetChild(3).GetChild(0).GetChild(0).GetChild(0).GetChild(2).GetComponent<ParticleSystem>();
        RemoveIronHeadMode();
    }


    // Update is called once per frame
    void Update()
    {
        ResetPlayer();
        if (!isDie && !isBorn)
        {
            /*if (Invincible) {
                InvinicibleTimer += Time.deltaTime;
                if (InvinicibleTimer >= TimeInvinicible) {
                    InvinicibleTimer = 0;
                    Invincible = false;
                }
            }*/
            if (isSpeedAChange)
            {
                SpeedAChangeTimer += Time.deltaTime;
                if (SpeedAChangeTimer > Mathf.Clamp(1.6f * Mathf.Pow(0.75f, SpeedAlpha), 0.0f, 1.8f)) { isSpeedAChange = false; SpeedAChangeTimer = 0; }
            }
            if (isEmptyFrozenDone && animator.speed != 0)
            {
                animator.speed = 0;
            }
            if (isRockTombCD)
            {
                RockTombCDTimer += Time.deltaTime;
                if (RockTombCDTimer > 0.45f) { isRockTombCD = false; RockTombCDTimer = 0; }
            }
            TargetPosition = player.transform.position;
            if (isSubsititue && SubsititueTarget != null) { TargetPosition = SubsititueTarget.transform.position; }
            EmptyDie();
            UpdateEmptyChangeHP();
            StateMaterialChange();
            UpdataMulitBodyEmptyState();
            if (NowState != State.BodyPress) {
                animator.SetFloat("LookX", Director.x);
                animator.SetFloat("LookY", Director.y);
            }
            CalibrationPositionTimer += Time.deltaTime;
            if (CalibrationPositionTimer >= 1.0f) { CalibrationPositionTimer = 0; CalibrationPosition(); }

        }


    }


    private void FixedUpdate()
    {

        ResetPlayer();
        if (!isDie && !isBorn && OnixBodyTop.rigidbody2D) {


            if (!isEmptyFrozenDone && !isSleepDone && !isCanNotMoveWhenParalysis && !isSilence) {
                
                switch (NowState)
                {
                    case State.NormalState:
                        CheckTurn();
                        if (isCanHitAnimation) { isCanHitAnimation = false; }
                        OnixBodyTop.rigidbody2D.MovePosition(new Vector2(OnixBodyTop.rigidbody2D.position.x + Director.x * Time.deltaTime * speed * Mathf.Pow(1.3f, SpeedAlpha), OnixBodyTop.rigidbody2D.position.y + Director.y * Time.deltaTime * speed * Mathf.Pow(1.3f, SpeedAlpha)));
                        if (!isPSPlaying) { PlayAllPS(); }
                        if (SpeedAlpha <= -3 && !isSpeedAChange) { NowState = State.MoveIntoWallState; }
                        if (EmptyHp <= maxHP * 0.5f)
                        {
                            isAngry = true;
                        }
                        //if (ResetBody && Mathf.Abs((OnixBodyTop.transform.position - transform.parent.position).x) < 6.0f && Mathf.Abs((OnixBodyTop.transform.position - transform.parent.position).x) < 1.0f) { ReserSubBosy(); }
                        if (SpeedAlpha == 0 && isAngry) { NowState = State.BodyPress; }
                        break;
                    case State.MoveIntoWallState:
                        //初始化穿墙状态
                        if (!isMoveIntoWallStateIns) {
                            isMoveIntoWallStateIns = true;
                            SetSubBodyMoveIntoWallMode();

                        }
                        OnixBodyTop.rigidbody2D.MovePosition(new Vector2(OnixBodyTop.rigidbody2D.position.x + Director.x * Time.deltaTime * speed * Mathf.Pow(1.3f, SpeedAlpha), OnixBodyTop.rigidbody2D.position.y + Director.y * Time.deltaTime * speed * Mathf.Pow(1.3f, SpeedAlpha)));
                        if (!isPSPlaying) { PlayAllPS(); }
                        MoveInStateOver();
                        break;
                    case State.MoveOutWallState:
                        MoveOutTimer += Time.deltaTime;
                        if (!isTopHeadMoveOut) { CheckHeadMoveOut(); }
                        if (MoveOutTimer <= 1.0f) { SetNowDirAndSubBodyPos(Director); }
                        if (!isDig01Done && MoveOutTimer >= 1.8f)
                        {
                            isDig01Done = true;
                            if (!isFearDone) { Instantiate(TaigDig, TargetPosition - Vector3.down * 0.05f + (!isEmptyConfusionDone ? Vector3.zero : new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), 0).normalized * 1.5f), Quaternion.identity).ParentOnix = this; }
                        }
                        if (!isDig02Done && MoveOutTimer >= 3.6f)
                        {
                            isDig02Done = true;
                            if (!isFearDone) { Instantiate(TaigDig, TargetPosition - Vector3.down * 0.05f + (!isEmptyConfusionDone ? Vector3.zero : new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), 0).normalized * 1.5f), Quaternion.identity).ParentOnix = this; }
                        }
                        if (!isDig03Done && MoveOutTimer >= 5.4f)
                        {
                            isDig03Done = true;
                            if (!isFearDone) { Instantiate(TaigDig, TargetPosition - Vector3.down * 0.05f + (!isEmptyConfusionDone ? Vector3.zero : new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), 0).normalized * 1.5f), Quaternion.identity).ParentOnix = this; }
                        }
                        if (MoveOutTimer >= 8.5f)
                        {
                            CheckAllBodyMoveOut();
                            if (isTopHeadMoveOut) { CheckTurn(); }
                            OnixBodyTop.rigidbody2D.MovePosition(new Vector2(OnixBodyTop.rigidbody2D.position.x + Director.x * Time.deltaTime * speed * Mathf.Pow(1.3f, SpeedAlpha), OnixBodyTop.rigidbody2D.position.y + Director.y * Time.deltaTime * speed * Mathf.Pow(1.3f, SpeedAlpha)));
                            if (!isPSPlaying) { PlayAllPS(); }
                            if (isAllBodyMoveOut)
                            {
                                isMoveIntoWallStateIns = false;
                                OnixDigEffect = null;
                                MoveOutTimer = 0;
                                isTopHeadMoveOut = false;
                                isAllBodyMoveOut = false;
                                isDig01Done = false; isDig02Done = false; isDig03Done = false;
                                foreach (OnixBodyShadow b in SubEmptyBodyList)
                                {
                                    b.isCanInsideWall = false;
                                    b.ResetMaskMode();
                                }
                                RemoveIgnoreCollider();
                                NowState = State.NormalState;
                            }
                        }
                        break;
                    case State.Idle:
                        break;
                    case State.BodyPress:
                        BodyPressFloatTimer += Time.deltaTime;
                        if (!isBodyPressIns)
                        {

                            isBodyPressIns = true;
                            if (OnixBodyTop.transform.position.x < 0) { BodyPressCenter = new Vector2(-5.5f, 0.0f); }
                            else { BodyPressCenter = new Vector2(5.5f, 0.0f); }
                            SpeedAlpha = -1; RemoveIronHeadMode();
                            BodyPressR = (BodyPressCenter - (Vector2)OnixBodyTop.transform.position).magnitude;
                            BodyPressAlphaAngel = _mTool.Angle_360Y(((Vector2)OnixBodyTop.transform.position - BodyPressCenter).normalized, Vector2.right);
                            BodyPressD = (Director.x == Director.y) ? -1 : 1;
                            BodyPressNowPosition = OnixBodyTop.transform.position;
                            RStoneEdgeObj = Instantiate(RStoneEdge, transform.position, Quaternion.identity, transform);
                            RStoneEdgeObj.onix = this;
                            RStoneEdgeObj.TurnR = -BodyPressD;
                        }

                        if (BodyPressFloatTimer <= 8.7f) {
                            if (!isPSPlaying) { PlayAllPS(); }
                            BodyPressR += (5.0f - BodyPressR) * Time.deltaTime;
                            if (BodyPressFloatTimer <= 8.5f) { OnixBodyTop.rigidbody2D.MovePosition(new Vector2(BodyPressCenter.x + BodyPressR * Mathf.Cos(Mathf.Deg2Rad * (BodyPressD * BodyPressFloatTimer * Mathf.Clamp(10 + BodyPressFloatTimer * 100.0f, 10.0f, 800.0f) + BodyPressAlphaAngel)), BodyPressCenter.y + BodyPressR * Mathf.Sin(Mathf.Deg2Rad * (BodyPressD * BodyPressFloatTimer * Mathf.Clamp(10.0f + BodyPressFloatTimer * 100.0f, 10.0f, 800.0f) + BodyPressAlphaAngel)))); }
                            else { OnixBodyTop.rigidbody2D.MovePosition(new Vector2(BodyPressCenter.x + BodyPressR * Mathf.Cos(Mathf.Deg2Rad * (BodyPressD * BodyPressFloatTimer * Mathf.Clamp(500.0f, 500.0f, 1000.0f) + BodyPressAlphaAngel)), BodyPressCenter.y + BodyPressR * Mathf.Sin(Mathf.Deg2Rad * (BodyPressD * BodyPressFloatTimer * Mathf.Clamp(500.0f, 500.0f, 1000.0f) + BodyPressAlphaAngel)))); }
                            BodyPressCenter += (isFearDone ? -1 : 1) * ((Vector2)TargetPosition - BodyPressCenter).normalized * Time.deltaTime * 1.3f;
                            Vector2 D = (OnixBodyTop.rigidbody2D.position - BodyPressNowPosition).normalized;
                            Vector2Int dir = new Vector2Int(1, 1);
                            if (D.x >= 0) { dir.x = 1; } else { dir.x = -1; }
                            if (D.y >= 0) { dir.y = 1; } else { dir.y = -1; }
                            animator.SetFloat("LookX", dir.x); animator.SetFloat("LookY", dir.y);
                            BodyPressNowPosition = OnixBodyTop.transform.position;
                            if (BodyPressFloatTimer >= 8.2f && BodyPressTargetPosition == Vector2.zero) { BodyPressTargetPosition = TargetPosition; }
                            if (BodyPressFloatTimer >= 6.5f && RStoneEdgeObj != null) { Destroy(RStoneEdgeObj.gameObject); }
                        }

                        if (BodyPressFloatTimer > 8.7f && BodyPressFloatTimer <= 9.7f)
                        {
                            if (!isBodyPressJump)
                            {
                                float Xmax = int.MinValue; float Xmin = int.MaxValue; float Ymax = int.MinValue; float Ymin = int.MaxValue;
                                foreach (OnixBodyShadow b in SubEmptyBodyList)
                                {
                                    if (b.transform.position.x > Xmax) { Xmax = b.transform.position.x; }
                                    if (b.transform.position.x < Xmin) { Xmin = b.transform.position.x; }
                                    if (b.transform.position.y > Ymax) { Ymax = b.transform.position.y; }
                                    if (b.transform.position.y < Ymin) { Ymin = b.transform.position.y; }
                                }
                                isBodyPressJump = true;
                                BodyPressJumpCenter = new Vector2((Xmax + Xmin) / 2.0f, (Ymax + Ymin) / 2.0f);
                                //Debug.Log((BodyPressTargetPosition - BodyPressJumpCenter).normalized + OnixBodyTop.rigidbody2D.position + TargetPosition.ToString());
                                animator.SetTrigger("Jump"); PauseAllPS();
                                Timer.Start(this, 0.5f, () => { 
                                    bp = Instantiate(BodyPressCollider, BodyPressJumpCenter, Quaternion.identity, transform);
                                    bp.empty = this;
                                });
                            }
                            if (bp != null) { bp.transform.position = new Vector3(Mathf.Clamp(transform.position.x, ParentRoom.transform.position.x - 12.5f, ParentRoom.transform.position.x + 12.5f), Mathf.Clamp(transform.position.y, ParentRoom.transform.position.y - 7.2f, ParentRoom.transform.position.y + 7.2f), 0); }
                            Vector2 d = Quaternion.AngleAxis( (isEmptyConfusionDone ? Random.Range(-45,45) : 0 ) , Vector3.forward) * (BodyPressTargetPosition - BodyPressJumpCenter).normalized;
                            transform.position += new Vector3(Mathf.Clamp((isFearDone ? -1 : 1) * d.x * 20.0f * Time.deltaTime, -10.0f + ParentRoom.transform.position.x, 10.0f + ParentRoom.transform.position.x), Mathf.Clamp((isFearDone ? -1 : 1) * d.y * 20.0f * Time.deltaTime, -4.2f + ParentRoom.transform.position.y, 4.2f + ParentRoom.transform.position.y), 0);
                        }


                        //Debug.Log(Mathf.Cos(BodyPressD * Mathf.Deg2Rad  * (BodyPressFloatTimer * 100 + BodyPressAlphaAngel)) + "+" + BodyPressD * Mathf.Deg2Rad  * (BodyPressFloatTimer * 100 + BodyPressAlphaAngel));
                        //Debug.Log(BodyPressCenter +"+"+ BodyPressR  +"+"+ BodyPressFloatTimer + "+" + BodyPressAlphaAngel + "+" + Mathf.Cos(BodyPressD * Mathf.Deg2Rad  * (BodyPressFloatTimer * 100 + BodyPressAlphaAngel)) + "+" + Mathf.Sin(BodyPressD * Mathf.Deg2Rad  * (BodyPressFloatTimer * 100 + BodyPressAlphaAngel)));
                        break;
                }
            }
            else
            {
                if (isPSPlaying) { PauseAllPS(); }

            }
        }
    }

    /// <summary>
    /// 离开扑击状态
    /// </summary>
    public void OutBodyPressState()
    {
        BodyPressFloatTimer = 0;
        isBodyPressIns = false;
        BodyPressCenter = Vector2.zero;
        BodyPressR = 0;
        BodyPressAlphaAngel = 0;
        BodyPressD = 0;
        BodyPressNowPosition = Vector2.zero;
        isBodyPressJump = false;
        BodyPressJumpCenter = Vector2.zero;
        BodyPressTargetPosition = Vector2.zero;
        AllBodyBodyPressJumpOver();
        NowState = State.NormalState;
        ResetBody = true;
        isBodyPressDamage = false;
        RStoneEdgeObj = null;
        if (!isPSPlaying) { PlayAllPS(); }
    }

    public void JumpShake() { CameraShake( 1.0f , 6.0f ,false ); if (isPSPlaying) { PauseAllPS(); } }

    public void AllBodyBodyPressJumpStart()
    {
        foreach ( OnixBodyShadow b in SubEmptyBodyList )
        {
            b.JumpStart();
        }
    }

    public void AllBodyBodyPressJumpOver()
    {
        foreach (OnixBodyShadow b in SubEmptyBodyList)
        {
            b.JumpOver();
        }
    }

    /// <summary>
    /// 随机移动方向
    /// </summary>
    void RandomD()
    {
        switch (Random.Range(0, 4))
        {
            case 0:
                Director = new Vector2(1, 1);transform.position += (Vector3)Director * 3;
                break;
            case 1:
                Director = new Vector2(-1, 1); transform.position += (Vector3)Director * 3;
                break;
            case 2:
                Director = new Vector2(1, -1); transform.position += (Vector3)Director * 3;
                break;
            case 3:
                Director = new Vector2(-1, -1); transform.position += (Vector3)Director * 3;
                break;
        }
        CalibrationPosition();
        SetNowDirAndSubBodyPos(Director);
    }

    /// <summary>
    /// 移除对所有体节被忽略的碰撞
    /// </summary>
    void RemoveIgnoreCollider()
    {
        foreach (OnixBodyShadow b in SubEmptyBodyList)
        {
            for (int i = 0; i < IgnoreColliderList.Count; i++)
            {
                Physics2D.IgnoreCollision(b.BodyCollider2D, IgnoreColliderList[i] , false);
            }
        }
    }

    /// <summary>
    /// 窗墙状态时检测是否完全进入墙体中
    /// </summary>
    void MoveInStateOver()
    {
        Vector2 x = (SubEmptyBodyList[SubEmptyBodyList.Count - 1].transform.position - transform.parent.position);
        if ((Mathf.Abs(x.x) > 14 || Mathf.Abs(x.y) > 8) && (OnixDigEffect != null)) { 
            OnixDigEffect.GetComponent<Animator>().SetTrigger("Over");
            isMoveIntoWallStateIns = false;
            NowState = State.MoveOutWallState;
            MoveInWall();
            PauseAllPS();
        }
    }

    /// <summary>
    /// 检测头是否从墙里出来
    /// </summary>
    void CheckHeadMoveOut()
    {
        if (Mathf.Abs(OnixBodyTop.transform.position.x - ParentRoom.transform.position.x) <= 12.2f && Mathf.Abs(OnixBodyTop.transform.position.y - ParentRoom.transform.position.y) <= 7.2f) {
            isTopHeadMoveOut = true;
            BornStoneEdge();
        }
    }

    /// <summary>
    /// 检测是否所有体节都从墙里出来
    /// </summary>
    void CheckAllBodyMoveOut()
    {
        bool x = true;
        foreach( SubEmptyBody b in SubEmptyBodyList )
        {
            if ( Mathf.Abs(b.transform.position.x - ParentRoom.transform.position.x) >= 12.5f || Mathf.Abs(b.transform.position.y - ParentRoom.transform.position.y) >= 7.0f) { x = false; }
        }
        if (x) { isAllBodyMoveOut = true; }
        isCanHitAnimation = false;
    }

    /// <summary>
    /// 完全进入墙中后移动到另一位置
    /// </summary>
    void MoveInWall()
    {
        CalibrationPosition();
        int r = (int)Random.Range(0, 16);
        while ( (!ParentRoom.isWallAround[0] && r == 12) || (!ParentRoom.isWallAround[1] && r == 13) || (!ParentRoom.isWallAround[2] && r == 14) || (!ParentRoom.isWallAround[3] && r == 15) ){  r = (int)Random.Range(0, 16); }
        //Debug.Log(ParentRoom.isWallAround[0].ToString() + ParentRoom.isWallAround[1].ToString() + ParentRoom.isWallAround[2].ToString() + ParentRoom.isWallAround[3].ToString() + r.ToString());

        switch (r)
        {
            case 0:
                DigOutEffect = Instantiate(OnixBodyTop.GetComponent<OnixBodyShadow>().DigEffect, transform.parent.position + MoveOutPosition[r] , Quaternion.identity );
                transform.position = MoveOutPosition[r] + Vector3.down * 4.0f;
                SetNowDirAndSubBodyPos(new Vector2(-1, 1)); transform.position += Vector3.right * 2.0f;
                break;
            case 1:
                DigOutEffect = Instantiate(OnixBodyTop.GetComponent<OnixBodyShadow>().DigEffect, transform.parent.position + MoveOutPosition[r], Quaternion.identity);
                transform.position = MoveOutPosition[r] + Vector3.down * 4.0f;
                if (Random.Range(0.0f, 1.0f) > 0.5f) { SetNowDirAndSubBodyPos(new Vector2(1, 1)); transform.position += Vector3.left * 2.0f; } else { SetNowDirAndSubBodyPos(new Vector2(-1, 1)); transform.position += Vector3.right * 2.0f; }
                break;
            case 2:
                DigOutEffect = Instantiate(OnixBodyTop.GetComponent<OnixBodyShadow>().DigEffect, transform.parent.position + MoveOutPosition[r], Quaternion.identity);
                transform.position = MoveOutPosition[r] + Vector3.down * 4.0f;
                SetNowDirAndSubBodyPos(new Vector2(1, 1)); transform.position += Vector3.left * 2.0f;
                break;
            case 3:
                DigOutEffect = Instantiate(OnixBodyTop.GetComponent<OnixBodyShadow>().DigEffect, transform.parent.position + MoveOutPosition[r], Quaternion.identity);
                transform.position = MoveOutPosition[r] + Vector3.down * 4.0f;
                if (Random.Range(0.0f, 1.0f) > 0.5f) { SetNowDirAndSubBodyPos(new Vector2(1, 1)); transform.position += Vector3.left * 2.0f; } else { SetNowDirAndSubBodyPos(new Vector2(-1, 1)); transform.position += Vector3.right * 2.0f; }
                break;


            case 4:
                DigOutEffect = Instantiate(OnixBodyTop.GetComponent<OnixBodyShadow>().DigEffect, transform.parent.position + MoveOutPosition[r], Quaternion.Euler(0,0,180));
                transform.position = MoveOutPosition[r] + Vector3.up * 4.0f;
                SetNowDirAndSubBodyPos(new Vector2(-1, -1)); transform.position += Vector3.right * 1.5f;
                break;
            case 5:
                DigOutEffect = Instantiate(OnixBodyTop.GetComponent<OnixBodyShadow>().DigEffect, transform.parent.position + MoveOutPosition[r], Quaternion.Euler(0, 0, 180));
                transform.position = MoveOutPosition[r] + Vector3.up * 4.0f;
                if (Random.Range(0.0f, 1.0f) > 0.5f) { SetNowDirAndSubBodyPos(new Vector2(1, -1)); transform.position += Vector3.left * 1.5f; } else { SetNowDirAndSubBodyPos(new Vector2(-1, -1)); transform.position += Vector3.right * 1.5f; }
                break;
            case 6:
                DigOutEffect = Instantiate(OnixBodyTop.GetComponent<OnixBodyShadow>().DigEffect, transform.parent.position + MoveOutPosition[r], Quaternion.Euler(0, 0, 180));
                transform.position = MoveOutPosition[r] + Vector3.up * 4.0f;
                SetNowDirAndSubBodyPos(new Vector2(1, -1)); transform.position += Vector3.left * 1.5f;
                break;
            case 7:
                DigOutEffect = Instantiate(OnixBodyTop.GetComponent<OnixBodyShadow>().DigEffect, transform.parent.position + MoveOutPosition[r], Quaternion.Euler(0, 0, 180));
                transform.position = MoveOutPosition[r] + Vector3.up * 4.0f;
                if (Random.Range(0.0f, 1.0f) > 0.5f) { SetNowDirAndSubBodyPos(new Vector2(1, -1)); transform.position += Vector3.left * 1.5f; } else { SetNowDirAndSubBodyPos(new Vector2(-1, -1)); transform.position += Vector3.right * 1.5f; }
                break;


            case 8:
                DigOutEffect = Instantiate(OnixBodyTop.GetComponent<OnixBodyShadow>().DigEffect, transform.parent.position + MoveOutPosition[r], Quaternion.Euler(0, 0, 90));
                transform.position = MoveOutPosition[r] + Vector3.right * 4.0f;
                SetNowDirAndSubBodyPos(new Vector2(-1, -1)); transform.position += Vector3.up * 1.5f;
                break;
            case 9:
                DigOutEffect = Instantiate(OnixBodyTop.GetComponent<OnixBodyShadow>().DigEffect, transform.parent.position + MoveOutPosition[r], Quaternion.Euler(0, 0, 90));
                transform.position = MoveOutPosition[r] + Vector3.right * 4.0f;
                SetNowDirAndSubBodyPos(new Vector2(-1, 1)); transform.position += Vector3.down * 1.5f;
                break;


            case 10:
                DigOutEffect = Instantiate(OnixBodyTop.GetComponent<OnixBodyShadow>().DigEffect, transform.parent.position + MoveOutPosition[r], Quaternion.Euler(0, 0, -90));
                transform.position = MoveOutPosition[r] + Vector3.left * 4.0f;
                SetNowDirAndSubBodyPos(new Vector2(1, -1)); transform.position += Vector3.up * 1.5f;
                break;
            case 11:
                DigOutEffect = Instantiate(OnixBodyTop.GetComponent<OnixBodyShadow>().DigEffect, transform.parent.position + MoveOutPosition[r], Quaternion.Euler(0, 0, -90));
                transform.position = MoveOutPosition[r] + Vector3.left * 4.0f;
                SetNowDirAndSubBodyPos(new Vector2(1, 1)); transform.position += Vector3.down * 1.5f;
                break;


            case 12:
                DigOutEffect = Instantiate(OnixBodyTop.GetComponent<OnixBodyShadow>().DigEffect, transform.parent.position + MoveOutPosition[r], Quaternion.Euler(0, 0, 180));
                transform.position = MoveOutPosition[r] + Vector3.up * 4.0f;
                if (Random.Range(0.0f, 1.0f) > 0.5f) { SetNowDirAndSubBodyPos(new Vector2(1, -1)); transform.position += Vector3.left * 1.5f; } else { SetNowDirAndSubBodyPos(new Vector2(-1, -1)); transform.position += Vector3.right * 1.5f; }
                break;
            case 13:
                DigOutEffect = Instantiate(OnixBodyTop.GetComponent<OnixBodyShadow>().DigEffect, transform.parent.position + MoveOutPosition[r], Quaternion.identity);
                transform.position = MoveOutPosition[r] + Vector3.down * 4.0f;
                if (Random.Range(0.0f, 1.0f) > 0.5f) { SetNowDirAndSubBodyPos(new Vector2(1, 1)); transform.position += Vector3.left * 2.0f; } else { SetNowDirAndSubBodyPos(new Vector2(-1, 1)); transform.position += Vector3.right * 2.0f; }
                break;
            case 14:
                DigOutEffect = Instantiate(OnixBodyTop.GetComponent<OnixBodyShadow>().DigEffect, transform.parent.position + MoveOutPosition[r], Quaternion.Euler(0, 0, 90));
                transform.position = MoveOutPosition[r] + Vector3.right * 4.0f;
                if (Random.Range(0.0f, 1.0f) > 0.5f) { SetNowDirAndSubBodyPos(new Vector2(-1, 1)); transform.position += Vector3.down * 1.5f; } else { SetNowDirAndSubBodyPos(new Vector2(-1, -1)); transform.position += Vector3.up * 1.5f; }
                break;
            case 15:
                DigOutEffect = Instantiate(OnixBodyTop.GetComponent<OnixBodyShadow>().DigEffect, transform.parent.position + MoveOutPosition[r], Quaternion.Euler(0, 0, -90));
                transform.position = MoveOutPosition[r] + Vector3.left * 4.0f;
                if (Random.Range(0.0f, 1.0f) > 0.5f) { SetNowDirAndSubBodyPos(new Vector2(1, -1)); transform.position += Vector3.up * 1.5f; } else { SetNowDirAndSubBodyPos(new Vector2(1, 1)); transform.position += Vector3.down * 1.5f; }
                break;
                //new Vector3(10.8f, -5.0f, 0) ,new Vector3(6.2f, -5.0f, 0) , new Vector3(-10.8f, -5.0f, 0) ,new Vector3(-6.2f, -5.0f, 0) ,
                //new Vector3(10.8f, 5.6f, 0) ,new Vector3(6.2f, 5.6f, 0) , new Vector3(-10.8f, 5.6f, 0) ,new Vector3(-6.2f, 5.6f, 0) ,
                //new Vector3(10.4f, 5.3f, 0) ,new Vector3(10.4f, -5.0f, 0) ,new Vector3(-10.4f, 5.3f, 0) ,new Vector3(-10.4f, -5.0f, 0) ,
                //new Vector3(0.0f, 5.6f, 0) ,new Vector3(0.0f, -4.8f, 0) ,new Vector3(10.4f, 0, 0) ,new Vector3(-10.4f, 0, 0) ,
        }
        SpeedAlpha = 7;
        DigOutEffect.empty = this;
        Timer.Start(this, 8.0f, () =>
        {
            DigOutEffect.transform.GetChild(4).gameObject.SetActive(true);
        });
        Timer.Start(this, 8.5f, () =>
        {
            IronHeadMode();
            DigOutEffect.GetComponent<Animator>().SetTrigger("Start");
        });

        Timer.Start(this, 11.0f, () =>
        {
            DigOutEffect.GetComponent<Animator>().SetTrigger("Over");
            DigOutEffect = null;
        });

    }

    /// <summary>
    /// 进入穿墙状态后把所有体节改为穿墙状态
    /// </summary>
    void SetSubBodyMoveIntoWallMode()
    {
        foreach (OnixBodyShadow b in SubEmptyBodyList)
        {
            b.isCanInsideWall = true;
        }
    }

    /// <summary>
    /// 改变方向，并且根据当前方向重置其他体节位置
    /// </summary>
    /// <param name="d"></param>
    public void SetNowDirAndSubBodyPos( Vector2 d )
    {
        Director = d;
        if (d == new Vector2(1.0f, 1.0f)) 
        {
            for (int i = 0; i < SubEmptyBodyList.Count; i++)
            {
                SubEmptyBodyList[i].transform.localPosition = new Vector3(BodyPositionBR[(int)Mathf.Clamp(i, 0, 17)].x , BodyPositionBR[(int)Mathf.Clamp(i, 0, 17)].y , 0);
                SubEmptyBodyList[i].transform.rotation = Quaternion.Euler(0,0, BodyPositionBR[(int)Mathf.Clamp(i , 0 , 17)].z);
            }
        }
        else if (d == new Vector2(-1.0f, 1.0f)) 
        {
            for (int i = 0; i < SubEmptyBodyList.Count; i++)
            {
                SubEmptyBodyList[i].transform.localPosition = new Vector3(BodyPositionBL[(int)Mathf.Clamp(i, 0, 17)].x, BodyPositionBL[(int)Mathf.Clamp(i, 0, 17)].y, 0);
                SubEmptyBodyList[i].transform.rotation = Quaternion.Euler(0, 0, BodyPositionBL[(int)Mathf.Clamp(i, 0, 17)].z);
            }
        }
        else if (d == new Vector2(1.0f, -1.0f)) 
        {
            for (int i = 0; i < SubEmptyBodyList.Count; i++)
            {
                SubEmptyBodyList[i].transform.localPosition = new Vector3(BodyPositionSR[(int)Mathf.Clamp(i, 0, 17)].x, BodyPositionSR[(int)Mathf.Clamp(i, 0, 17)].y, 0);
                SubEmptyBodyList[i].transform.rotation = Quaternion.Euler(0, 0, BodyPositionSR[(int)Mathf.Clamp(i, 0, 17)].z);
            }
        }
        else
        {
            for (int i = 0; i < SubEmptyBodyList.Count; i++)
            {
                SubEmptyBodyList[i].transform.localPosition = new Vector3(BodyPositionSL[(int)Mathf.Clamp(i, 0, 17)].x, BodyPositionSL[(int)Mathf.Clamp(i, 0, 17)].y, 0);
                SubEmptyBodyList[i].transform.rotation = Quaternion.Euler(0, 0, BodyPositionSL[(int)Mathf.Clamp(i, 0, 17)].z);
            }
        }
    }

    /// <summary>
    /// 在一般状态时检测是否撞墙，撞墙后改变方向
    /// </summary>
    void CheckTurn()
    {
        if (Director.x == -1) {
            RaycastHit2D SearchPlayerL01 = Physics2D.Raycast(new Vector2(OnixBodyTop.transform.position.x, OnixBodyTop.transform.position.y), Vector3.left, 1.03f, LayerMask.GetMask("Room"));
            RaycastHit2D SearchPlayerL02 = Physics2D.Raycast(new Vector2(OnixBodyTop.transform.position.x, OnixBodyTop.transform.position.y + 0.35f), Vector3.left, 1.03f, LayerMask.GetMask("Room"));
            RaycastHit2D SearchPlayerL03 = Physics2D.Raycast(new Vector2(OnixBodyTop.transform.position.x, OnixBodyTop.transform.position.y - 0.35f), Vector3.left, 1.03f, LayerMask.GetMask("Room"));
            if ((SearchPlayerL01.collider != null && (SearchPlayerL01.transform.tag == "Room" ))
                || (SearchPlayerL02.collider != null && (SearchPlayerL02.transform.tag == "Room"))
                || (SearchPlayerL03.collider != null && (SearchPlayerL03.transform.tag == "Room")))
            {
                Director.x = -Director.x; SpeedAChangeTimer = 0;
                if ( NowState == State.NormalState && !isSpeedAChange) { SpeedAlpha--; isSpeedAChange = true;  }
                CameraShake(0.6f, 3.0f, true); FallARockTomb();
                if (isIronHead && SpeedAlpha <= 1 ) { RemoveIronHeadMode(); }
            }

        }

        if (Director.x == 1) { 
            RaycastHit2D SearchPlayerR01 = Physics2D.Raycast(new Vector2(OnixBodyTop.transform.position.x, OnixBodyTop.transform.position.y), Vector3.right, 1.03f, LayerMask.GetMask("Room"));
            RaycastHit2D SearchPlayerR02 = Physics2D.Raycast(new Vector2(OnixBodyTop.transform.position.x, OnixBodyTop.transform.position.y + 0.35f), Vector3.right, 1.03f, LayerMask.GetMask("Room"));
            RaycastHit2D SearchPlayerR03 = Physics2D.Raycast(new Vector2(OnixBodyTop.transform.position.x, OnixBodyTop.transform.position.y - 0.35f), Vector3.right, 1.03f, LayerMask.GetMask("Room"));
            if ((SearchPlayerR01.collider != null && (SearchPlayerR01.transform.tag == "Room"))
                || (SearchPlayerR02.collider != null && (SearchPlayerR02.transform.tag == "Room" ))
                || (SearchPlayerR03.collider != null && (SearchPlayerR03.transform.tag == "Room")))
            {
                Director.x = -Director.x; SpeedAChangeTimer = 0; if (NowState == State.NormalState && !isSpeedAChange) { SpeedAlpha--; isSpeedAChange = true;  }
                CameraShake(0.6f, 3.0f, true); FallARockTomb();
                if (isIronHead && SpeedAlpha <= 1) { RemoveIronHeadMode(); }
            }
        }

        if (Director.y == -1) {
            RaycastHit2D SearchPlayerD01 = Physics2D.Raycast(new Vector2(OnixBodyTop.transform.position.x, OnixBodyTop.transform.position.y), Vector3.down, 1.03f, LayerMask.GetMask("Room"));
            RaycastHit2D SearchPlayerD02 = Physics2D.Raycast(new Vector2(OnixBodyTop.transform.position.x + 0.35f, OnixBodyTop.transform.position.y), Vector3.down, 1.03f, LayerMask.GetMask("Room"));
            RaycastHit2D SearchPlayerD03 = Physics2D.Raycast(new Vector2(OnixBodyTop.transform.position.x - 0.35f, OnixBodyTop.transform.position.y), Vector3.down, 1.03f, LayerMask.GetMask("Room"));


            if ((SearchPlayerD01.collider != null && (SearchPlayerD01.transform.tag == "Room" ))
                || (SearchPlayerD02.collider != null && (SearchPlayerD02.transform.tag == "Room" ))
                || (SearchPlayerD03.collider != null && (SearchPlayerD03.transform.tag == "Room")))
            {
                Director.y = -Director.y; SpeedAChangeTimer = 0; if (NowState == State.NormalState && !isSpeedAChange) { SpeedAlpha--; isSpeedAChange = true; }
                CameraShake(0.6f, 3.0f, true); FallARockTomb();
                if (isIronHead && SpeedAlpha <= 1) { RemoveIronHeadMode(); }
            }
        }

        if (Director.y == 1) {
            RaycastHit2D SearchPlayerU01 = Physics2D.Raycast(new Vector2(OnixBodyTop.transform.position.x, OnixBodyTop.transform.position.y), Vector3.up, 1.03f, LayerMask.GetMask("Room"));
            RaycastHit2D SearchPlayerU02 = Physics2D.Raycast(new Vector2(OnixBodyTop.transform.position.x + 0.35f, OnixBodyTop.transform.position.y), Vector3.up, 1.03f, LayerMask.GetMask("Room"));
            RaycastHit2D SearchPlayerU03 = Physics2D.Raycast(new Vector2(OnixBodyTop.transform.position.x - 0.35f, OnixBodyTop.transform.position.y), Vector3.up, 1.03f, LayerMask.GetMask("Room"));
            if ((SearchPlayerU01.collider != null && (SearchPlayerU01.transform.tag == "Room"))
                || (SearchPlayerU02.collider != null && (SearchPlayerU02.transform.tag == "Room"))
                || (SearchPlayerU03.collider != null && (SearchPlayerU03.transform.tag == "Room" )))
            {
                
                Director.y = -Director.y; SpeedAChangeTimer = 0; if (NowState == State.NormalState && !isSpeedAChange) { SpeedAlpha--; isSpeedAChange = true; }
                CameraShake(0.6f, 3.0f, true); FallARockTomb();
                if (isIronHead && SpeedAlpha <= 1) { RemoveIronHeadMode(); }
            }
        }
    }

    /// <summary>
    /// 调整体节的位置
    /// </summary>
    public void CalibrationPosition()
    {
        transform.position = OnixBodyTop.transform.position;
        Vector3 Move = OnixBodyTop.transform.localPosition;
        foreach (SubEmptyBody b in SubEmptyBodyList)
        {
            b.transform.position -= Move;
        }
    }

    /// <summary>
    /// SetActive所有运动粒子
    /// </summary>
    bool isPSPlaying;
    public void EnableAllPS()
    {
        Debug.Log("PS");
        foreach ( ParticleSystem p in MovePSList )
        {
            p.gameObject.SetActive(true);
            isPSPlaying = true;
        }
    }

    /// <summary>
    /// Play所有运动粒子
    /// </summary>
    public void PlayAllPS()
    {
        foreach (ParticleSystem p in MovePSList)
        {
            p.Play();
            isPSPlaying = true;
            var E = p.emission;
            E.enabled = true;
        }
    }

    /// <summary>
    /// 暂停发射所有运动粒子
    /// </summary>
    public void PauseAllPS()
    {
        foreach (ParticleSystem p in MovePSList)
        {
            isPSPlaying = false;
            var E = p.emission;
            E.enabled = false;
        }
    }

    /// <summary>
    /// 释放石刃
    /// </summary>
    public void BornStoneEdge()
    {
        if (!isFearDone) {
            OnixStoneEdgeManger s = Instantiate(StoneEdge, OnixBodyTop.transform.position, Quaternion.identity);
            s.empty = this;
        }
    }

    /// <summary>
    /// 重置身体
    /// </summary>
    public void ReserSubBosy()
    {
        Timer.Start( this , 0.56f , () =>
        {
            if (ResetBody) { ResetBody = false; SetNowDirAndSubBodyPos(Director); }
        });
    }

    /// <summary>
    /// 暂停发射所有运动粒子
    /// </summary>
    public void FallARockTomb()
    {
        if (!isRockTombCD && !isFearDone) {

            if (!isEmptyConfusionDone) {
                isRockTombCD = true;
                RockTombCDTimer = 0;
                int count = Random.Range(2, 4);
                while (count != 0) {
                    count--;
                    Vector3 FallPosition;
                    FallPosition = (Vector3)TargetPosition + (new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), 0).normalized) * Random.Range(0.0f, 5.0f);
                    int TPCount = 0;
                    while (!isThisPointEmpty(FallPosition) || Mathf.Abs(FallPosition.x - ParentRoom.transform.position.x) >= 10.5f || Mathf.Abs(FallPosition.y - ParentRoom.transform.position.y) >= 6.0f)
                    {
                        FallPosition = (Vector3)TargetPosition + (new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), 0).normalized) * Random.Range(0.0f, 5.0f);
                        TPCount++;
                        if (TPCount > 100)
                        {
                            FallPosition = TargetPosition;
                            break;
                        }
                    }
                    OnixRockTomb r = Instantiate(RockTomb, FallPosition, Quaternion.identity, ParentRoom.transform.GetChild(2));
                    r.ParentOnix = this;
                }

                Vector3 FallPosition02 = (Vector3)TargetPosition + (new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), 0).normalized) * Random.Range(7.0f, 10.0f);
                int TPCount02 = 0;
                while (!isThisPointEmpty(FallPosition02) || Mathf.Abs(FallPosition02.x - ParentRoom.transform.position.x) >= 10.5f || Mathf.Abs(FallPosition02.y - ParentRoom.transform.position.y) >= 6.0f)
                {
                    FallPosition02 = (Vector3)TargetPosition + (new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), 0).normalized) * Random.Range(7.0f, 10.0f);
                    TPCount02++;
                    if (TPCount02 > 100)
                    {
                        FallPosition02 = TargetPosition;
                        break;
                    }
                }
                OnixRockTomb r02 = Instantiate(RockTomb, FallPosition02, Quaternion.identity, ParentRoom.transform.GetChild(2));
                r02.ParentOnix = this;

            }
            else
            {
                isRockTombCD = true;
                RockTombCDTimer = 0;
                int count = Random.Range(2, 4);
                while (count != 0)
                {
                    count--;
                    Vector3 FallPosition;
                    FallPosition = new Vector3(Random.Range(-10.5f, 10.5f) + ParentRoom.transform.position.x, Random.Range(-6.0f, 6.0f) + ParentRoom.transform.position.y, 0);
                    int TPCount = 0;
                    while (!isThisPointEmpty(FallPosition) || Mathf.Abs(FallPosition.x - ParentRoom.transform.position.x) >= 10.5f || Mathf.Abs(FallPosition.y - ParentRoom.transform.position.y) >= 6.0f)
                    {
                        FallPosition = new Vector3(Random.Range(-10.5f, 10.5f) + ParentRoom.transform.position.x, Random.Range(-6.0f, 6.0f) + ParentRoom.transform.position.y, 0);
                        TPCount++;
                        if (TPCount > 100)
                        {
                            FallPosition = TargetPosition;
                            break;
                        }
                    }
                    OnixRockTomb r = Instantiate(RockTomb, FallPosition, Quaternion.identity, ParentRoom.transform.GetChild(2));
                    r.ParentOnix = this;
                }

            }
        }
    }

    /// <summary>
    /// 变为铁头状态
    /// </summary>
    void IronHeadMode()
    {
        isIronHead = true;
        var p = IHShinePS.emission;
        p.enabled = true;
        animator.SetBool("isIH", true);
    }

    /// <summary>
    /// 变为非铁头状态
    /// </summary>
    void RemoveIronHeadMode()
    {
        isIronHead = false;
        var p = IHShinePS.emission;
        p.enabled = false;
        animator.SetBool("isIH", false);
    }


   

}
