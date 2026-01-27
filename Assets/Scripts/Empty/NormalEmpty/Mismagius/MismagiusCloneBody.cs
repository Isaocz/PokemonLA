using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MismagiusCloneBody : NormalEmptyCloneBody
{
    /// <summary>
    /// 催眠术
    /// </summary>
    public MismagiusHypnosis Hypnosis;

    /// <summary>
    /// 主状态
    /// </summary>
    public enum MainState
    {
        Idle,              //发呆
        Rush,              //分身冲刺
        TakeTurnsLaunch,   //转圈发射弹幕
    }
    MainState NowState = MainState.Rush;



    Mismagius ParentMismagius;





    //是否可以消失
    public bool IsCanDispear
    {
        get { return isCanDispear; }
        set { isCanDispear = value; }
    }
    bool isCanDispear = false;



    //冲刺的目的地
    public Vector2 RushTarget
    {
        get { return rushTarget; }
        set { rushTarget = value; }
    }
    Vector2 rushTarget = Vector2.zero;


    /// <summary>
    /// 冲刺速度
    /// </summary>
    public float RushSpeed
    {
        get { return rushSpeed; }
        set { rushSpeed = value; }
    }
    float rushSpeed = 0.0f;


    private void Awake()
    {
        RushStart();
    }


    private void Start()
    {
        
    }




    private void Update()
    {
        if (ParentMismagius == null)
        {
            ParentMismagius = ParentEmpty.GetComponent<Mismagius>();
        }
        if (!isDespear && !ParentMismagius.isDie && !ParentMismagius.isBorn && !ParentMismagius.isEmptyFrozenDone && !ParentMismagius.isSleepDone && !ParentMismagius.isSilence && !ParentMismagius.isCanNotMoveWhenParalysis) {
            switch (NowState)
            {
                case MainState.Idle:
                    IdleTimer -= Time.deltaTime;//发呆计时器时间减少
                    if (IdleTimer <= 0)         //计时器时间到时间，结束发呆状态
                    {
                        IdleOver();
                        //TODO添加下一个状态的开始方法
                        switch (NextState_IdleOver)
                        {
                            //下一个状态为发射弹幕时切换为发射弹幕
                            case MainState.TakeTurnsLaunch:
                                TakeTurnsLaunchStart();
                                break;
                            //下一个状态为其他时切换为发射弹幕(可能更改)
                            default:
                                TakeTurnsLaunchStart();
                                break;
                        }
                    }
                    break;
                case MainState.Rush:
                    MoveBySpeedAndDir((RushTarget - (Vector2)transform.position).normalized, RushSpeed, 1.0f, 0.0f, 0.0f, 0.0f, 0.0f);
                    //Debug.Log(Vector2.Distance((Vector2)ParentMismagius.CloneBodyCenter, RushTarget) +"+"+ Vector2.Distance((Vector2)transform.position, (Vector2)ParentMismagius.CloneBodyCenter));
                    if ( Vector2.Distance((Vector2)transform.position, (Vector2)RushTarget) <= 0.6f )
                    {
                        RushOver();
                        //Debug.Log("IdleStart" + "+" + RushTarget + "+" + ParentMismagius.CloneBodyRadius + "+" + (Vector2.Distance((Vector2)transform.position, ParentMismagius.CloneBodyCenter)  ));
                        IdleStart(TIME_IDLE_CLONESHADOW, MainState.TakeTurnsLaunch);
                    }
                    break;
                case MainState.TakeTurnsLaunch:
                    TakeTurnsLaunchTimer += Time.deltaTime;//转圈发射弹幕计时器时间增加
                    if ( DispeableTimer < TIME_DISPEABLE ) { DispeableTimer += Time.deltaTime; }
                    if (TakeTurnsLaunchTimer >= (float)CloneBodyIndex * Mismagius.TIME_TAKETURNLAUNCH_INTERVAL)         //计时器时间到时间，结束转圈发射弹幕状态
                    {
                        animator.SetTrigger("Atk");
                        TakeTurnsLaunchTimer -= (float)Mismagius.COUNT_CLONESHADOW * Mismagius.TIME_TAKETURNLAUNCH_INTERVAL;
                        LunchDir = (ParentMismagius.CloneBodyCenter - (Vector2)transform.position).normalized;
                        LunchOneShadowBall(LunchDir, Mismagius.SPEED_SHADOWBALL);
                        //TakeTurnsLaunchOver();
                        //TODO添加下一个状态的开始方法
                    }
                    break;
            }
        }
    }












    /// <summary>
    /// 设置方向
    /// </summary>
    public void SetDirector(Vector2 dir)
    {
        animator.SetFloat("LookX", dir.x);
        animator.SetFloat("LookY", dir.y);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {

        if(NowState == MainState.Idle ||  (NowState == MainState.TakeTurnsLaunch ))
        {
            if (collision.gameObject.tag != "Room" && collision.gameObject.tag != "Enviroment")
            {
                
                SetCloneShadowOver(true);
            }
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

        transform.position = new Vector2(
            Mathf.Clamp(transform.position.x
                + (float)dir.x * Time.deltaTime * Speed * SpeedAlpha,                    //方向*速度
            ParentEmpty.ParentPokemonRoom.RoomSize[2] - RoomLeftAlpha + transform.parent.position.x, //最小值
            ParentEmpty.ParentPokemonRoom.RoomSize[3] + RoomRightAlpha + transform.parent.position.x),//最大值
            Mathf.Clamp(transform.position.y
                + (float)dir.y * Time.deltaTime * Speed * SpeedAlpha,                     //方向*速度 
            ParentEmpty.ParentPokemonRoom.RoomSize[1] - RoomDownAlpha + transform.parent.position.y,  //最小值
            ParentEmpty.ParentPokemonRoom.RoomSize[0] + RoomUpAlpha + transform.parent.position.y));//最大值
    }


    public void SetCloneShadowOver(bool UseHypnosis)
    {
        //Debug.Log(NowState);
        if (ParentMismagius.CloneBodyList.Contains(this))
        {
            ParentMismagius.CloneBodyList.Remove(this);
        }
        //Debug.Log(name);
        animator.SetTrigger("Over");
        //是否使用催眠术
        if (UseHypnosis && ParentMismagius != null)
        {
            MismagiusHypnosis hy = Instantiate(Hypnosis, transform.position, Quaternion.identity);
            hy.empty = ParentMismagius;
        }
        //关闭残影
        if (ShadowCoroutine != null)
        {
            StopShadowCoroutine();
        }
    }








    //=========================发呆============================

    //分身后的冷却时间
    static float TIME_IDLE_CLONESHADOW = 0.2f; //TODO需修改时间




    /// <summary>
    /// 发呆计时器
    /// <summary>
    float IdleTimer = 0;

    /// <summary>
    /// 发呆后的下一个状态
    /// </summary>
    MainState NextState_IdleOver;

    /// <summary>
    /// 发呆开始
    /// <summary>
    public void IdleStart(float Timer, MainState nextState)
    {
        IdleTimer = Timer;
        NowState = MainState.Idle;
        NextState_IdleOver = nextState;
    }

    /// <summary>
    /// 发呆结束
    /// <summary>
    public void IdleOver()
    {
        IdleTimer = 0;
    }


    //=========================发呆============================













    //=========================分身冲刺============================


    //分身的序列位置
    public int CloneBodyIndex
    {
        get { return cloneBodyIndex; }
        set { cloneBodyIndex = value; }
    }
    int cloneBodyIndex = 0;

    /// <summary>
    /// 发呆计时器
    /// <summary>
    //float RushTimer = 0;

    /// <summary>
    /// 发呆开始
    /// <summary>
    public void RushStart(/*float Timer*/)
    {
        //RushTimer = Timer;
        NowState = MainState.Rush;
        animator.SetBool("Rush", true);
        //开启残影
        if (ShadowCoroutine == null)
        {
            StartShadowCoroutine(0.08f, 1.6f, new Color(0.6603774f, 0.6603774f, 0.6603774f, 0.6f));
        }
    }

    /// <summary>
    /// 发呆结束
    /// <summary>
    public void RushOver()
    {
        //RushTimer = 0;
        animator.SetBool("Rush", false);
        //关闭残影
        if (ShadowCoroutine != null)
        {
            StopShadowCoroutine();
        }
    }


    //=========================分身冲刺============================










    //=========================转圈发射弹幕============================

    /// <summary>
    /// 可以触发消失的时间限制
    /// </summary>
    static float TIME_DISPEABLE = 1.0f;


    /// <summary>
    /// 发射影子球角度
    /// </summary>
    public Vector2 LunchDir;

    /// <summary>
    /// 假影子球
    /// </summary>
    public FakeMismagiusShadowBalll Fakesb;

    /// <summary>
    /// 转圈发射弹幕计时器
    /// <summary>
    float TakeTurnsLaunchTimer = 0;

    /// <summary>
    /// 转圈发射弹幕计时器
    /// <summary>
    float DispeableTimer = 0;

    /// <summary>
    /// 转圈发射弹幕开始
    /// <summary>
    public void TakeTurnsLaunchStart()
    {
        TakeTurnsLaunchTimer = 0;
        NowState = MainState.TakeTurnsLaunch;
        SetDirector(_mTool.TiltMainVector2((ParentMismagius.CloneBodyCenter - (Vector2)transform.position).normalized));
        Timer.Start(this, DispearTime, () => {
            SetCloneShadowOver(false);
        });
    }

    /// <summary>
    /// 转圈发射弹幕结束
    /// <summary>
    public void TakeTurnsLaunchOver()
    {
        TakeTurnsLaunchTimer = 0;
    }

    /// <summary>
    /// 发射一个假影子球
    /// </summary>
    void LunchOneShadowBall(Vector2 Dir, float Speed)
    {
        Dir = Dir.normalized;
        FakeMismagiusShadowBalll s = Instantiate(Fakesb, (Vector3)Dir * 1.0f + transform.position + Vector3.up * 0.4f, Quaternion.identity);
        s.LaunchNotForce(Dir, Speed);
    }


    //=========================转圈发射弹幕============================

}
