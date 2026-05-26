using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeavileCloneBody : NormalEmptyCloneBody
{









    //■■■■■■■■■■■■■■■■■■■■状态机枚举■■■■■■■■■■■■■■■■■■■■

    /// <summary>
    /// 主状态
    /// </summary>
    public enum MainState
    {
        Idle,    //发呆
        Run,     //奔跑
        JumpMove,//跳跃移动
        Rush,    //冲刺
        Guard,   //原地防守
        UseProtect,  //使用保护
        IceShard,    //发射冰粒
        DarkPulse,   //发射恶波
        Fling,      //投掷
        CircleRun,  //绕圈跑
        Maze,    //释放迷宫
        MazeWatcher, //看护迷宫
        CloneShadow, //分身
        Heal,    //回复
    }
    public MainState NowMainState;


    //■■■■■■■■■■■■■■■■■■■■状态机枚举■■■■■■■■■■■■■■■■■■■■









    //■■■■■■■■■■■■■■■■■■■■玛狃拉小队主脑■■■■■■■■■■■■■■■■■■■■■■

    /// <summary>
    /// 父主脑
    /// </summary>
    public WeavileTeamBrain TeamBrain;






    /// <summary>
    /// 【TODO】小队连招 全员默认设为发呆状态 清空所有已完成动作
    /// </summary>
    public void TeamCombatReset_Default()
    {
        isCombatComplite_TeamState304_CloneShadow = false;
        isCombatComplite_TeamState204_Substitute2 = false;
    }



    //=========================三人小队 分身连招============================

    /// <summary>
    /// 三人小队_分身连招 连招动作是否完成
    /// </summary>
    public bool IsCombatComplite_TeamState304_CloneShadow
    {
        get { return isCombatComplite_TeamState304_CloneShadow; }
        set { isCombatComplite_TeamState304_CloneShadow = value; }
    }
    public bool isCombatComplite_TeamState304_CloneShadow = false;

    /// <summary>
    /// 三人小队_分身连招 团队成员连招动作都完成时，进行下一次连招的事件
    /// </summary>
    public void TeamCombatActOverEvent_TeamState304_CloneShadow()
    {
        if (TeamBrain.NowSubState == WeavileTeamBrain.SubState.TeamState_304_CloneShadow)
        {
            //设置下一次连招动作为未完成
            isCombatComplite_TeamState304_CloneShadow = false;
            //根据当前主脑副状态决定下一步连招
            switch (TeamBrain.NowTeamState304CloneShadow)
            {
                //当前为分身 状态机错误
                case WeavileTeamBrain.TEAMSTATE304_CLONESHADOW.CloneShadow:
                    Debug.Log("Error:三人小队 分身连招 状态机错误 执行下一次动作时仍处于分身状态");
                    //强制修改为跳跃状态
                    TeamBrain.NowTeamState304CloneShadow = WeavileTeamBrain.TEAMSTATE304_CLONESHADOW.Jump;
                    JumpMoveStart();
                    break;
                //当前为跳跃
                case WeavileTeamBrain.TEAMSTATE304_CLONESHADOW.Jump:
                    JumpMoveStart();
                    break;
                //当前为冰粒
                case WeavileTeamBrain.TEAMSTATE304_CLONESHADOW.IceShard:
                    IceShardStart(Weavile.TIME_ICESHARD_TEMASTATE304IDLE_JUMP);
                    break;
                //当前为终结冲刺
                case WeavileTeamBrain.TEAMSTATE304_CLONESHADOW.OverRush:
                    RushStart();
                    break;
                //当前为意外冲刺
                case WeavileTeamBrain.TEAMSTATE304_CLONESHADOW.AccidentRush:
                    break;
            }

        }
    }

    /// <summary>
    /// (弃用)三人小队_分身连招 连招彻底结束，转入下一个团队连招
    /// </summary>
    public void TeamCombatCompliteEvent_TeamState304_CloneShadow()
    {
        if (TeamBrain.NowSubState == WeavileTeamBrain.SubState.TeamState_304_CloneShadow)
        {
            //设置下一次连招动作为未完成
            isCombatComplite_TeamState304_CloneShadow = false;
            //【TODO】休息时间
            //IdleStart(Weavile.TIME_IDLE_TEAMSTATE303_SUBSTITUTE_OVER_01RUSH);
            SetCloneShadowOver();
        }
    }

    //=========================三人小队 分身连招============================




    //=========================二人小队 替身连招2============================

    /// <summary>
    /// 二人小队_替身连招2 连招动作是否完成
    /// </summary>
    public bool IsCombatComplite_TeamState204_Substitute2
    {
        get { return isCombatComplite_TeamState204_Substitute2; }
        set { isCombatComplite_TeamState204_Substitute2 = value; }
    }
    public bool isCombatComplite_TeamState204_Substitute2 = false;

    /// <summary>
    /// 二人小队_替身连招2 团队成员连招动作都完成时，进行下一次连招的事件
    /// </summary>
    public void TeamCombatActOverEvent_TeamState204_Substitute2()
    {
        if (TeamBrain.NowSubState == WeavileTeamBrain.SubState.TeamState_204_Substitute2)
        {
            //设置下一次连招动作为未完成
            isCombatComplite_TeamState204_Substitute2 = false;
            //根据当前主脑副状态决定下一步连招
            switch (TeamBrain.NowTeamState204SubState)
            {
                //当前为投掷 状态机错误
                case WeavileTeamBrain.TEAMSTATE204_SUBSTATE.Fling:
                    Debug.Log("Error 204替身连招 状态机错误");
                    //强制修改为分身阶段 分身开始
                    TeamBrain.NowTeamState204SubState = WeavileTeamBrain.TEAMSTATE204_SUBSTATE.CloneShadow;
                    isCombatComplite_TeamState204_Substitute2 = true;
                    break;
                case WeavileTeamBrain.TEAMSTATE204_SUBSTATE.CloneShadow:
                    //强制修改为跳跃阶段 跳跃开始 跳跃至房间角落
                    isCombatComplite_TeamState204_Substitute2 = true;
                    break;
                case WeavileTeamBrain.TEAMSTATE204_SUBSTATE.Jump:
                    JumpMoveStart();
                    break;
                case WeavileTeamBrain.TEAMSTATE204_SUBSTATE.Bomb:
                    FlingStart();
                    break;
                case WeavileTeamBrain.TEAMSTATE204_SUBSTATE.Rush:
                    //开始冲刺
                    RushStart();
                    break;
            }
        }
    }

    /// <summary>
    /// 二人小队_替身连招2 连招彻底结束，转入下一个团队连招
    /// </summary>
    public void TeamCombatCompliteEvent_TeamState204_Substitue2()
    {
        if (TeamBrain.NowSubState == WeavileTeamBrain.SubState.TeamState_204_Substitute2)
        {
            //设置下一次连招动作为未完成 分身直接消失
            isCombatComplite_TeamState204_Substitute2 = false;
            SetCloneShadowOver();
        }
    }

    //=========================二人小队 替身连招2============================

















    /// <summary>
    /// 父玛狃拉
    /// </summary>
    public Weavile ParentWeavile;

    /// <summary>
    /// 敌人幻影朝向
    /// </summary>
    Vector2 Director;


    /// <summary>
    /// 计算当前速度,朝向时，采用的上一时间单位的位置坐标,通过携程执行
    /// </summary>
    Vector3 LastPosition;


    /// <summary>
    /// 父敌人的目标的坐标
    /// </summary>
    public Vector2 TARGET_POSITION
    {
        get { return ParentWeavile.TARGET_POSITION; }
    }

    /// <summary>
    /// 父敌人的速度
    /// </summary>
    public float speed
    {
        get { return ParentWeavile.speed; }
    }


    //设置幻影初始化
    public override void SetCloneBody(Empty ParentE)
    {
        base.SetCloneBody(ParentE);
        ParentWeavile = ParentEmpty.GetComponent<Weavile>();
        TeamBrain = ParentWeavile.TeamBrain;
    }

    private void Start()
    {
        transform.rotation = Quaternion.identity;

        Timer.Start(this, DispearTime, () => {
            SetCloneShadowOver();
        });

        //启动计算方向携程
        StartCoroutine(CheckLook());

        //初始化方向状态机
        SetDirector(Vector2.down);

        //0秒发呆
        IdleStart(0.0f);

    }



    private void Update()
    {
        //父玛狃拉和主脑不为空时判断状态机
        if (ParentWeavile != null && TeamBrain != null)
        {
            if (!isDespear && !ParentWeavile.isDie && !ParentWeavile.isBorn && !ParentWeavile.isEmptyFrozenDone && !ParentWeavile.isSleepDone && !ParentWeavile.isSilence && !ParentWeavile.isCanNotMoveWhenParalysis)
            {
                switch (NowMainState)
                {
                    case MainState.Idle:
                        {
                            IdleTimer -= Time.deltaTime;//发呆计时器时间减少
                            if (IdleTimer <= 0)         //计时器时间到时间，结束发呆状态
                            {
                                IdleOver();
                                switch (TeamBrain.NowSubState)
                                {
                                    //主脑休息状态时转入防守
                                    case WeavileTeamBrain.SubState.TeamState_300_Idle:
                                        GuardStart();
                                        break;
                                    //三人小队 分身连招 转入防守
                                    case WeavileTeamBrain.SubState.TeamState_304_CloneShadow:
                                        GuardStart();
                                        break;
                                    //二人小队 主脑休息状态时转入防守
                                    case WeavileTeamBrain.SubState.TeamState_200_Idle:
                                        GuardStart();
                                        break;
                                    //二人小队 替身连招 转入防守
                                    case WeavileTeamBrain.SubState.TeamState_204_Substitute2:
                                        GuardStart();
                                        break;
                                    default:
                                        GuardStart();
                                        break;
                                }
                                //TODO添加下一个状态的开始方法
                            }
                        }
                        break;
                    case MainState.Run:
                        break;
                    case MainState.JumpMove:
                        {
                            //跳跃移动的准备期间
                            if (isPrepare_Jump)
                            {
                                JumpMoveTimer += Time.deltaTime;//跳跃移动计时器时间增加
                                switch (TeamBrain.NowSubState)
                                {
                                    //三人连招_分身连招
                                    case WeavileTeamBrain.SubState.TeamState_304_CloneShadow:
                                        //无准备时间
                                        if (JumpMoveTimer >= 0)
                                        {
                                            JumpMoveTimer = 0;
                                            isPrepare_Jump = false;
                                            animator.SetInteger("Jump", 1);
                                        }
                                        break;
                                    //二人连招_替身连招2
                                    case WeavileTeamBrain.SubState.TeamState_204_Substitute2:
                                        //无准备时间
                                        if (JumpMoveTimer >= 0)
                                        {
                                            JumpMoveTimer = 0;
                                            isPrepare_Jump = false;
                                            animator.SetInteger("Jump", 1);
                                        }
                                        break;
                                }

                            }
                            //开始跳跃移动
                            if (isMove_Jump)
                            {
                                JumpMoveTimer += Time.deltaTime;//跳跃移动计时器时间增加
                                switch (TeamBrain.NowSubState)
                                {
                                    //三人连招_分身连招
                                    case WeavileTeamBrain.SubState.TeamState_304_CloneShadow:
                                        //移动
                                        if (Dir_Jump != Vector2.zero) { MoveBySpeedAndDir(Dir_Jump, ParentWeavile.speed, Weavile.SPEEDALPHA_JUMP_TEMASTATE304_LONESHADOW, 0.0f, 0.0f, 0.0f, 0.0f); }
                                        //时间足够 移动结束
                                        //Debug.Log(JumpMoveTimer +"+"+ Time_Jump_Max);
                                        if (JumpMoveTimer >= Time_Jump_Max)
                                        {
                                            animator.SetInteger("Jump", 2);
                                        }
                                        break;
                                    //二人连招_替身连招2
                                    case WeavileTeamBrain.SubState.TeamState_204_Substitute2:
                                        //移动
                                        if (Dir_Jump != Vector2.zero){MoveBySpeedAndDir(Dir_Jump, speed, Weavile.SPEEDALPHA_JUMP_TEMASTATE302_CLOSEATK, 0.0f, 0.0f, 0.0f, 0.0f);}
                                        //时间足够 移动结束
                                        //Debug.Log(JumpMoveTimer +"+"+ Time_Jump_Max);
                                        if (JumpMoveTimer >= Time_Jump_Max)
                                        {
                                            animator.SetInteger("Jump", 2);
                                        }
                                        break;
                                }
                            }
                        }
                        break;
                    case MainState.Rush:
                        {
                            //蓄力或者冲刺时计时器增加
                            if (isCharge_Rush || isMove_Rush)
                            {
                                RushTimer += Time.deltaTime;//冲刺计时器时间增加
                            }
                            //蓄力时可以转向(不同连招不一样 部分连招不可改变)
                            if (isCharge_Rush)
                            {
                                switch (TeamBrain.NowSubState)
                                {
                                    //分身连招
                                    case WeavileTeamBrain.SubState.TeamState_304_CloneShadow:
                                        //意外冲刺可以转向
                                        if (TeamBrain.NowTeamState304CloneShadow == WeavileTeamBrain.TEAMSTATE304_CLONESHADOW.AccidentRush)
                                        {
                                            Dir_Rush = (ParentWeavile.TARGET_POSITION - (Vector2)transform.position).normalized;
                                            //SetRushArrow(NowRushType, );
                                            if (RushArrowObj != null) { RushArrowObj.SetTarget(ParentWeavile.ParentPokemonRoom.GetRoomBoundaryByRaycastTo(transform.position, Dir_Rush)); }
                                            SetDirector(_mTool.MainVector2(Dir_Rush));
                                        }
                                        break;
                                    default:
                                        Dir_Rush = (ParentWeavile.TARGET_POSITION - (Vector2)transform.position).normalized;
                                        SetDirector(_mTool.MainVector2(Dir_Rush));
                                        break;
                                }

                            }
                            //蓄力完毕后转进冲刺(不同连招蓄力时间不一样)
                            if (isCharge_Rush)
                            {
                                switch (TeamBrain.NowSubState)
                                {
                                    //分身连招
                                    case WeavileTeamBrain.SubState.TeamState_304_CloneShadow:
                                        //终结冲刺
                                        if (TeamBrain.NowTeamState304CloneShadow == WeavileTeamBrain.TEAMSTATE304_CLONESHADOW.OverRush)
                                        {
                                            if (RushTimer >= Weavile.TIME_CHARGE_RUSH_TEMASTATE304_CLONESHADOW_OVERRUSH)
                                            {
                                                animator.SetInteger("Rush", 2);
                                                RushTimer = 0;//蓄力结束
                                            }
                                        }
                                        //意外冲刺
                                        else if (TeamBrain.NowTeamState304CloneShadow == WeavileTeamBrain.TEAMSTATE304_CLONESHADOW.AccidentRush)
                                        {
                                            if (RushTimer >= Weavile.TIME_CHARGE_RUSH_TEMASTATE304_CLONESHADOW_ACCIDENTRUSH + 1.5f * (float)(ParentWeavile.TeamCombatActIndex - 1))
                                            {
                                                animator.SetInteger("Rush", 2);
                                                RushTimer = 0;//蓄力结束
                                            }
                                        }

                                        break;
                                }

                            }
                            //冲刺完毕后转进冲刺停止
                            if (isMove_Rush)
                            {
                                switch (TeamBrain.NowSubState)
                                {
                                    //分身连招
                                    case WeavileTeamBrain.SubState.TeamState_304_CloneShadow:
                                        if (RushTimer >= Weavile.TIME_RUSHMOVE_RUSH_TEMASTATE304_CLONESHADOW) { animator.SetInteger("Rush", 3); RushTimer = 0; }
                                        break;
                                }
                            }
                            //冲刺
                            if (isMove_Rush)
                            {
                                switch (TeamBrain.NowSubState)
                                {
                                    //分身连招
                                    case WeavileTeamBrain.SubState.TeamState_304_CloneShadow:
                                        //冲刺沿途效果
                                        switch (NowRushType)
                                        {
                                            //冲刺为恶系时 沿途释放小恶波
                                            case RushType.Dark:
                                                Timer_Rush_MoveEffect_Interval += Time.deltaTime;
                                                if (Timer_Rush_MoveEffect_Interval >= Weavile.TIME_INTERVAL_RUSH_TEMASTATE304_CLONESHADOW_SMALLDARKPULSE)
                                                {
                                                    Timer_Rush_MoveEffect_Interval = 0.0f;
                                                    LaunchDarkPulseTiny(transform.position);
                                                }
                                                break;
                                            //冲刺为毒系时 沿途释放毒雾
                                            case RushType.Posion:
                                                Timer_Rush_MoveEffect_Interval += Time.deltaTime;
                                                if (Timer_Rush_MoveEffect_Interval >= Weavile.TIME_INTERVAL_RUSH_TEMASTATE304_CLONESHADOW_SMALLDARKPULSE)
                                                {
                                                    Timer_Rush_MoveEffect_Interval = 0.0f;
                                                    LaunchPosionMist(Dir_Rush);
                                                }
                                                break;
                                        }
                                        MoveBySpeedAndDir(Dir_Rush, speed, Weavile.SPEEDALPHA_RUSH_TEMASTATE302_CLOSEATK, 0.0f, 0.0f, 0.0f, 0.0f);
                                        break;
                                }
                            }
                            // 调整预判冲刺角度指示线
                            if (isPredict_RushAngle)
                            {
                                float Angle_PredictLastAngle_Rush02 = _mTool.Angle_360Y((ParentWeavile.TARGET_POSITION - (Vector2)transform.position).normalized, Vector2.right);
                                float PredictAngle = 2.0f * Angle_PredictLastAngle_Rush02 - Angle_PredictLastAngle_Rush;
                                Vector2 d = Quaternion.AngleAxis(PredictAngle, Vector3.forward) * Vector3.right;
                                if (RushArrowObj != null) { RushArrowObj.SetTarget(ParentWeavile.ParentPokemonRoom.GetRoomBoundaryByRaycastTo(transform.position, d)); }
                            }
                        }
                        break;
                    case MainState.Guard:
                        //判断主脑 主脑为其他状态时转出
                        switch (TeamBrain.NowSubState)
                        {
                            //三人小队奔跑状态
                            case WeavileTeamBrain.SubState.TeamState_301_Run:
                                GuardOver();//防守结束
                                RunStart();//开始奔跑
                                break;
                            //三人小队近战状态
                            case WeavileTeamBrain.SubState.TeamState_302_CloseAtk:
                                GuardOver();//防守结束
                                RushStart();//转入冲刺
                                            //根据序列号转入近战分支动作
                                            //switch (TeamCombatActIndex)
                                            //{
                                            //    case 1: RushStart(); break;
                                            //    case 2: JumpMoveStart(); break;
                                            //    case 3: UseProtectStart(TIME_USEPROTECT_TEMASTATE302_CLOSEATK); break;
                                            //}
                                break;
                            //三人小队替身连招
                            case WeavileTeamBrain.SubState.TeamState_303_Substitute:
                                GuardOver();//防守结束
                                            //【TODO】
                                break;
                            //三人小队分身连招
                            case WeavileTeamBrain.SubState.TeamState_304_CloneShadow:
                                GuardOver();//防守结束
                                JumpMoveStart();
                                JumpMoveStart();//开始跳跃
                                break;
                            //三人小队迷宫连招
                            case WeavileTeamBrain.SubState.TeamState_305_Maze:
                                GuardOver();//防守结束
                                            //【TODO】
                                break;
                            //三人小队绕圈连招
                            case WeavileTeamBrain.SubState.TeamState_306_CircleRun:
                                GuardOver();//防守结束
                                CircleRunStart();
                                break;
                            //三人小队回血连招
                            case WeavileTeamBrain.SubState.TeamState_307_Heal1:
                                GuardOver();//防守结束
                                            //【TODO】
                                break;
                            //三人小队回血连招2
                            case WeavileTeamBrain.SubState.TeamState_308_Heal2:
                                GuardOver();//防守结束
                                            //【TODO】
                                break;
                            //二人小队奔跑连招
                            case WeavileTeamBrain.SubState.TeamState_201_Run:
                                GuardOver();//防守结束
                                            //【TODO】
                                break;
                            //二人小队近战连招
                            case WeavileTeamBrain.SubState.TeamState_202_CloseAtk:
                                GuardOver();//防守结束
                                            //【TODO】
                                break;
                            //二人小队替身连招1
                            case WeavileTeamBrain.SubState.TeamState_203_Substitute:
                                GuardOver();//防守结束
                                            //【TODO】
                                break;
                            //二人小队替身连招2
                            case WeavileTeamBrain.SubState.TeamState_204_Substitute2:
                                GuardOver();//防守结束
                                switch (TeamBrain.NowTeamState204SubState)
                                {
                                    case WeavileTeamBrain.TEAMSTATE204_SUBSTATE.Fling:
                                        break;
                                    case WeavileTeamBrain.TEAMSTATE204_SUBSTATE.CloneShadow:
                                        break;
                                    case WeavileTeamBrain.TEAMSTATE204_SUBSTATE.Jump:
                                        JumpMoveStart();
                                        break;
                                    case WeavileTeamBrain.TEAMSTATE204_SUBSTATE.Bomb:
                                        FlingStart();
                                        break;
                                    case WeavileTeamBrain.TEAMSTATE204_SUBSTATE.Rush:
                                        break;
                                }
                                break;
                            //二人小队绕圈连招
                            case WeavileTeamBrain.SubState.TeamState_205_CircleRun:
                                GuardOver();//防守结束
                                CircleRunStart();
                                break;
                            //二人小队回血连招
                            case WeavileTeamBrain.SubState.TeamState_206_Heal:
                                GuardOver();//防守结束
                                            //【TODO】
                                break;
                            //二人小队迷宫连招
                            case WeavileTeamBrain.SubState.TeamState_207_Maze:
                                GuardOver();//防守结束
                                            //【TODO】
                                break;
                            //一人小队巡逻模式
                            case WeavileTeamBrain.SubState.TeamState_100_Normal:
                                GuardOver();//防守结束
                                            //【TODO】
                                break;
                        }
                        break;
                    case MainState.UseProtect:
                        break;
                    case MainState.IceShard:
                        {
                            IceShardTimer -= Time.deltaTime;//发射冰粒计时器时间减少
                            if (IceShardTimer <= 0)         //计时器时间到时间，结束发射冰粒状态
                            {
                                IceShardOver();
                                //根据主脑状态连招不同 下一步执行不同
                                switch (TeamBrain.NowSubState)
                                {
                                    //三人小队分身连招 用完冰粒后连招动作完成
                                    case WeavileTeamBrain.SubState.TeamState_304_CloneShadow:
                                        isCombatComplite_TeamState304_CloneShadow = true;
                                        break;
                                    //默认小休息
                                    default:
                                        IdleStart(Weavile.TIME_IDLE_START);
                                        break;
                                }
                            }
                        }
                        break;
                    case MainState.DarkPulse:
                        break;
                    case MainState.Fling:
                        {
                            FlingTimer -= Time.deltaTime;//投掷计时器时间减少
                            //准备期间
                            if (isPrepare_Fling)
                            {
                                if (FlingTimer <= 0)         //准备计时器时间到时间，结束准备 开始投掷
                                {
                                    switch (TeamBrain.NowSubState)
                                    {
                                        //二人小队 替身连招2 
                                        case WeavileTeamBrain.SubState.TeamState_204_Substitute2:
                                            {
                                                //默认为向目标投掷炸弹 如果状态机错误就会调用默认值
                                                NowFlingType = Weavile.FlingType.Bomb;
                                                Vector2 t204 = ParentWeavile.TARGET_POSITION;
                                                //根据当前主脑状态判断投掷种类和投掷目标点
                                                switch (TeamBrain.NowTeamState204SubState)
                                                {
                                                    case WeavileTeamBrain.TEAMSTATE204_SUBSTATE.Fling:
                                                        //投掷状态无人投掷 状态机错误
                                                        Debug.Log("Error:二人小队 替身连招2 投掷阶段有玛狃拉分身尝试投掷");
                                                        break;
                                                    case WeavileTeamBrain.TEAMSTATE204_SUBSTATE.CloneShadow:
                                                        //跳跃状态无人投掷 状态机错误
                                                        Debug.Log("Error:二人小队 替身连招2 分身阶段有玛狃拉分身尝试投掷");
                                                        break;
                                                    case WeavileTeamBrain.TEAMSTATE204_SUBSTATE.Jump:
                                                        //跳跃状态无人投掷 状态机错误
                                                        Debug.Log("Error:二人小队 替身连招2 跳跃阶段有玛狃拉分身尝试投掷");
                                                        break;
                                                    case WeavileTeamBrain.TEAMSTATE204_SUBSTATE.Bomb:
                                                        //炸弹状态时 玛狃拉全体使用投掷 炸弹100%
                                                        NowFlingType = Weavile.FlingType.Bomb;
                                                        t204 = TeamBrain.TeamState204Substitute2_GetJumpTargetPosition(
                                                            TeamBrain.TeamState204_Substitute2_GetSubstitutePostion, WeavileTeamBrain.DISTENCE_TEAMSTATE204SUBSTTITUTE2_JUMP_BOMBBASERADIUS, TeamBrain.WeavileAndCloneShadowList.Count,
                                                            ParentWeavile.ParentPokemonRoom.RoomSize[0] + ParentWeavile.ParentPokemonRoom.transform.position.y, ParentWeavile.ParentPokemonRoom.RoomSize[1] + ParentWeavile.ParentPokemonRoom.transform.position.y, ParentWeavile.ParentPokemonRoom.RoomSize[2] + ParentWeavile.ParentPokemonRoom.transform.position.x, ParentWeavile.ParentPokemonRoom.RoomSize[3] + ParentWeavile.ParentPokemonRoom.transform.position.x
                                                            , transform);
                                                        //Debug.Log(t204);
                                                        t204 = ParentWeavile.ParentPokemonRoom.EnsurePointReachesRoom(t204);
                                                        break;
                                                }
                                                t204 = ParentWeavile.ParentPokemonRoom.EnsurePointReachesRoom(t204);
                                                LaunchFlingProjectiles(NowFlingType, t204);
                                                FlingTimer = Weavile.TIME_PREPARE_FLING_TEMASTATE306_CIRCLERUN_ESCAPEATK;//设置时间
                                                SetDirector(_mTool.MainVector2((t204 - (Vector2)transform.position).normalized));
                                            }
                                            break;
                                    }
                                    animator.SetTrigger("Atk");
                                    isPrepare_Fling = false;
                                }
                            }
                            //准备完毕
                            else
                            {
                                if (FlingTimer <= 0)         //计时器时间到时间，结束投掷状态
                                {
                                    FlingOver();
                                    switch (TeamBrain.NowSubState)
                                    {
                                        //二人小队 替身连招2 投掷结束后连招动作结束 设定动作完成旗帜为真
                                        case WeavileTeamBrain.SubState.TeamState_204_Substitute2:
                                            isCombatComplite_TeamState204_Substitute2 = true;
                                            break;
                                    }
                                }
                            }
                        }
                        break;
                    case MainState.CircleRun:
                        CircleRunTimer += Time.deltaTime;//绕圈跑计时器时间增加
                        switch (TeamBrain.NowSubState)
                        {
                            //三人小队 绕圈跑
                            case WeavileTeamBrain.SubState.TeamState_306_CircleRun:
                                {
                                    Vector2 center = TeamBrain.Position_Center_TeamState_306_CircleRun;
                                    float radius = WeavileTeamBrain.RADIUS_TEAMSTATE_306_CIRCLERUN;
                                    Dir_CircleRun = Quaternion.AngleAxis(90.0f, Vector3.forward) * ((Vector2)transform.position - center).normalized;

                                    //如果移动半径过小向内或外调节
                                    if (Mathf.Abs(Vector2.Distance(center, (Vector2)transform.position) - radius) > 0.5f)
                                    {
                                        //在直径外
                                        if ((Vector2.Distance(center, (Vector2)transform.position) - radius) > 0)
                                        {
                                            //Debug.Log("Ou");
                                            Dir_CircleRun += (center - (Vector2)transform.position).normalized;
                                            Dir_CircleRun.Normalize();
                                        }
                                        //在直径内
                                        else
                                        {
                                            //Debug.Log("In");
                                            Dir_CircleRun -= (center - (Vector2)transform.position).normalized;
                                            Dir_CircleRun.Normalize();
                                        }
                                    }
                                    MoveBySpeedAndDir(Dir_CircleRun, speed, SpeedAlpha_TeamState306CircleRun, 0.0f, 0.0f, 0.0f, 0.0f);
                                }
                                break;
                            //二人小队 绕圈跑
                            case WeavileTeamBrain.SubState.TeamState_205_CircleRun:
                                {
                                    Vector2 center = TeamBrain.Position_Center_TeamState_205_CircleRun;
                                    float radius = WeavileTeamBrain.RADIUS_TEAMSTATE_306_CIRCLERUN;
                                    Dir_CircleRun = Quaternion.AngleAxis(90.0f, Vector3.forward) * ((Vector2)transform.position - center).normalized;

                                    //如果移动半径过小向内或外调节
                                    if (Mathf.Abs(Vector2.Distance(center, (Vector2)transform.position) - radius) > 0.5f)
                                    {
                                        //在直径外
                                        if ((Vector2.Distance(center, (Vector2)transform.position) - radius) > 0)
                                        {
                                            //Debug.Log("Ou");
                                            Dir_CircleRun += (center - (Vector2)transform.position).normalized;
                                            Dir_CircleRun.Normalize();
                                        }
                                        //在直径内
                                        else
                                        {
                                            //Debug.Log("In");
                                            Dir_CircleRun -= (center - (Vector2)transform.position).normalized;
                                            Dir_CircleRun.Normalize();
                                        }
                                    }
                                    MoveBySpeedAndDir(Dir_CircleRun, speed, SpeedAlpha_TeamState306CircleRun, 0.0f, 0.0f, 0.0f, 0.0f);
                                }
                                break;
                            //主脑为其他状态 转入休息
                            default:
                                CircleRunOver();
                                IdleStart(Weavile.TIME_IDLE_START);
                                break;
                        }
                        break;
                    case MainState.Maze:
                        break;
                    case MainState.MazeWatcher:
                        break;
                    case MainState.CloneShadow:
                        break;
                    case MainState.Heal:
                        break;
                }
            }

        }
    }



    /// <summary>
    /// 幻影消失
    /// </summary>
    public override void SetCloneShadowOver()
    {
        animator.SetTrigger("Over");
        isDespear = true;
        if(ParentWeavile != null) {
            ParentWeavile.LaunchDarkPulseSmall(transform.position);
            ParentWeavile.TeamBrain.RemoveWeavileAndCloneShadowList(this.transform);
        }
        if (ShadowCoroutine != null)
        {
            StopShadowCoroutine();
        }
        if (RushObj != null)
        {
            RushObj.RushOver();
        }
    }














    //■■■■■■■■■■■■■■■■■■■■碰撞■■■■■■■■■■■■■■■■■■■■■■



    private void OnCollisionEnter2D(Collision2D collision)
    {
        //和别的玛狃拉或玛狃拉残影的碰撞被忽略碰撞
        if (collision.gameObject.tag == "Empty")
        {
            if (collision.gameObject.GetComponent<Weavile>() != null || collision.gameObject.GetComponent<WeavileCloneBody>() != null)
            {
                Physics2D.IgnoreCollision(collision.gameObject.GetComponent<Collider2D>(), GetComponent<Collider2D>());
            }
        }

        if (collision.gameObject.tag != "Room" && collision.gameObject.tag != "Enviroment" && collision.gameObject.tag != "Empty")
        {
            SetCloneShadowOver();
        }
    }


    void CollisionRoomOrEvnviromentRushuStop()
    {

    }



    //■■■■■■■■■■■■■■■■■■■■碰撞■■■■■■■■■■■■■■■■■■■■■■





















    //■■■■■■■■■■■■■■■■■■■■共通■■■■■■■■■■■■■■■■■■■■■■
    /// <summary>
    /// 设置方向
    /// </summary>
    public void SetDirector(Vector2 dir)
    {
        animator.SetFloat("LookX", dir.x);
        animator.SetFloat("LookY", dir.y);
    }


    /// <summary>
    /// 检查是否在移动和朝向
    /// </summary>
    /// <returns></returns>
    public IEnumerator CheckLook()
    {
        while (true)
        {
            //一般状态时更改速度和朝向
            if (  (NowMainState == MainState.Run || NowMainState == MainState.CircleRun))
            {
                //根据当前位置和上一次FixedUpdate调用时的位置差计算速度
                animator.SetFloat("Speed", Mathf.Abs((transform.position - LastPosition).magnitude));
                //根据当前位置和上一次FixedUpdate调用时的位置差计算朝向 并传给动画组件
                Director = _mTool.MainVector2((transform.position - LastPosition));
                animator.SetFloat("LookX", Director.x);
                animator.SetFloat("LookY", Director.y);
                //Debug.Log(Director);
                //重置位置
                LastPosition = transform.position;
            }
            yield return new WaitForSeconds(0.1f);
        }

    }



    /// <summary>
    /// 敌人分身在房间限制内移动
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

        Vector2 p = new Vector2(transform.position.x + (float)dir.x * Time.deltaTime * Speed * SpeedAlpha,
                                transform.position.y + (float)dir.y * Time.deltaTime * Speed * SpeedAlpha);

        transform.position = new Vector2(
            Mathf.Clamp(transform.position.x
                + (float)dir.x * Time.deltaTime * Speed * SpeedAlpha,                    //方向*速度
            ParentWeavile.ParentPokemonRoom.RoomSize[2] - RoomLeftAlpha + ParentWeavile.ParentPokemonRoom.transform.position.x, //最小值
            ParentWeavile.ParentPokemonRoom.RoomSize[3] + RoomRightAlpha + ParentWeavile.ParentPokemonRoom.transform.position.x),//最大值
            Mathf.Clamp(transform.position.y
                + (float)dir.y * Time.deltaTime * Speed * SpeedAlpha,                     //方向*速度 
            ParentWeavile.ParentPokemonRoom.RoomSize[1] - RoomDownAlpha + ParentWeavile.ParentPokemonRoom.transform.position.y,  //最小值
            ParentWeavile.ParentPokemonRoom.RoomSize[0] + RoomUpAlpha + ParentWeavile.ParentPokemonRoom.transform.position.y));//最大值

        if (p.x != transform.position.x || p.y != transform.position.y) { CollisionRoomOrEvnviromentRushuStop(); }
    }



    /// <summary>
    /// 生成冲刺落地尘埃
    /// </summary>
    public void InstantiateRunDust()
    {
        Instantiate(RushDust, transform.position, Quaternion.identity);
        //ParentPokemonRoom.CameraShake(0.3f, 0.8f, true);
        //【音效】audioPlayer.Play(CetitanSE.Step, transform.position);
    }
    //InsertSubStateChange
    //■■■■■■■■■■■■■■■■■■■■共通■■■■■■■■■■■■■■■■■■■■■■


















    //■■■■■■■■■■■■■■■■■■■■动画机事件■■■■■■■■■■■■■■■■■■■■■■


    //=========================跳跃事件============================

    /// <summary>
    /// 跳跃作为动画开始
    /// </summary>
    public void AnimatorEvent_JumpIn()
    {
        isMove_Jump = false;
        isPrepare_Jump = false;
    }

    /// <summary>
    /// 跳跃作为动作开始
    /// </summary>
    public void AnimatorEvent_JumpStart()
    {
        isMove_Jump = true;
        isPrepare_Jump = false;
        switch (TeamBrain.NowSubState)
        {
            //分身连招
            case WeavileTeamBrain.SubState.TeamState_304_CloneShadow:
                Vector2 t304 = Vector2.zero;//分身连招跳跃目标点
                //根据分身连招状态不同 跳跃不同
                switch (TeamBrain.NowTeamState304CloneShadow)
                {
                    //第一次跳跃
                    case WeavileTeamBrain.TEAMSTATE304_CLONESHADOW.CloneShadow:
                        t304 = TeamBrain.TeamState304CloneShadow_GetJumpTargetPosition(transform);
                        Dir_Jump = (t304 - (Vector2)transform.position).normalized;
                        Time_Jump_Max = Vector2.Distance(t304, (Vector2)transform.position) / (speed * Weavile.SPEEDALPHA_JUMP_TEMASTATE304_LONESHADOW);
                        //Time_Jump_Max = Weavile.TIME_JUMP_TEMASTATE304_CLONESHADOW_FIRSTJUMP;
                        //SpeedAlpha_Jump = Vector2.Distance(t304, (Vector2)transform.position) / (speed * Time_Jump_Max);
                        SetDirector(_mTool.MainVector2(Dir_Jump));
                        break;
                    //之后的跳跃
                    case WeavileTeamBrain.TEAMSTATE304_CLONESHADOW.Jump:
                        t304 = TeamBrain.TeamState304CloneShadow_GetJumpTargetPosition(transform);
                        Dir_Jump = (t304 - (Vector2)transform.position).normalized;
                        Time_Jump_Max = Vector2.Distance(t304, (Vector2)transform.position) / (speed * Weavile.SPEEDALPHA_JUMP_TEMASTATE304_LONESHADOW);
                        //Time_Jump_Max = Weavile.TIME_JUMP_TEMASTATE304_CLONESHADOW_SECONDJUMP;
                        //SpeedAlpha_Jump = Vector2.Distance(t304, (Vector2)transform.position) / (speed * Time_Jump_Max);
                        SetDirector(_mTool.MainVector2(Dir_Jump));
                        break;
                    //其他状态进入跳跃 说明状态机错误
                    default:
                        Debug.Log("Error:三人小队 分身连招 状态机错误 不该跳跃");
                        t304 = transform.position;
                        Dir_Jump = Vector3.right;
                        Time_Jump_Max = 0;
                        SetDirector(_mTool.MainVector2(Dir_Jump));
                        break;
                }
                break;
            case WeavileTeamBrain.SubState.TeamState_204_Substitute2:
                {
                    switch (TeamBrain.NowTeamState204SubState)
                    {
                        //投掷阶段玛狃拉02接近玛狃拉01
                        //向玛狃拉1跳跃 跳跃至玛狃拉和目标连线上一定距离 并且在房间内
                        case WeavileTeamBrain.TEAMSTATE204_SUBSTATE.Fling:
                            {
                                Vector2 t204 = (((Vector2)transform.position - (Vector2)TeamBrain.CombatWeavile01.transform.position).normalized) * Weavile.DISTENCE_TARGET_JUMP_TEMASTATE303_SUBSTITUTE + (Vector2)TeamBrain.CombatWeavile01.transform.position;
                                t204 = ParentWeavile.ParentPokemonRoom.EnsurePointReachesRoom(t204);
                                Dir_Jump = (t204 - (Vector2)transform.position).normalized;
                                Time_Jump_Max = Vector2.Distance(t204, (Vector2)transform.position) / (speed * Weavile.SPEEDALPHA_JUMP_TEMASTATE302_CLOSEATK);
                                SetDirector(_mTool.MainVector2(Dir_Jump));
                            }
                            break;
                        //跳跃阶段 跳跃 按照规律跳跃
                        case WeavileTeamBrain.TEAMSTATE204_SUBSTATE.Jump:
                            {
                                Vector2 t204 = transform.position;
                                //检查替身存在
                                //Debug.Log(TeamBrain.TeamState203_Substitute_SubstituteOBJ);
                                if (TeamBrain.TeamState204_Substitute2_SubstituteOBJ)
                                {
                                    t204 = TeamBrain.TeamState204Substitute2_GetJumpTargetPosition(
                                        TeamBrain.TeamState204_Substitute2_GetSubstitutePostion, WeavileTeamBrain.DISTENCE_TEAMSTATE204SUBSTTITUTE2_JUMP_JUMPBASERADIUS, TeamBrain.WeavileAndCloneShadowList.Count,
                                        ParentWeavile.ParentPokemonRoom.RoomSize[0] + ParentWeavile.ParentPokemonRoom.transform.position.y, ParentWeavile.ParentPokemonRoom.RoomSize[1] + ParentWeavile.ParentPokemonRoom.transform.position.y, ParentWeavile.ParentPokemonRoom.RoomSize[2] + ParentWeavile.ParentPokemonRoom.transform.position.x, ParentWeavile.ParentPokemonRoom.RoomSize[3] + ParentWeavile.ParentPokemonRoom.transform.position.x
                                        , transform);
                                    //Debug.Log(f.Count);
                                    //_mTool.DebugLogList<float>(f);
                                    t204 = ParentWeavile.ParentPokemonRoom.EnsurePointReachesRoom(t204);
                                    Dir_Jump = (t204 - (Vector2)transform.position).normalized;
                                    Time_Jump_Max = Vector2.Distance(t204, (Vector2)transform.position) / (speed * Weavile.SPEEDALPHA_JUMP_TEMASTATE302_CLOSEATK);
                                    SetDirector(_mTool.MainVector2(Dir_Jump));
                                    if (Vector2.Distance(t204, (Vector2)transform.position) < 0.5f) { Dir_Jump = Vector2.zero; Time_Jump_Max = 0.0f; }
                                }
                                else
                                {
                                    Debug.Log("Error:二人小队 替身连招2 替身已经被摧毁 但仍在执行");
                                    t204 = transform.position;
                                    Dir_Jump = Vector3.right;
                                    Time_Jump_Max = 0;
                                    SetDirector(_mTool.MainVector2(Dir_Jump));
                                }
                            }
                            break;
                        //其他状态 状态机错误 原地跳一下
                        default:
                            {
                                Debug.Log("Error:二人小队 替身连招2 状态机错误");
                                Vector2 t204 = transform.position;
                                Dir_Jump = Vector3.right;
                                Time_Jump_Max = 0;
                                SetDirector(_mTool.MainVector2(Dir_Jump));
                            }
                            break;
                    }
                }
                break;
            default:
                Debug.Log("Error:分身幻影 状态机错误 不该跳跃");
                Vector2 tDe = transform.position;
                Dir_Jump = Vector3.right;
                Time_Jump_Max = 0;
                SetDirector(_mTool.MainVector2(Dir_Jump));
                break;
        }
    }

    /// <summary>
    /// 跳跃作为动作结束
    /// </summary>
    public void AnimatorEvent_JumpOver()
    {
        isMove_Jump = false;
        isPrepare_Jump = false;
        //InstantiateRunDust();
    }

    /// <summary>
    /// 跳跃作为动作结束
    /// </summary>
    public void AnimatorEvent_JumpOut()
    {
        isMove_Jump = false;
        isPrepare_Jump = false;
        switch (TeamBrain.NowSubState)
        {
            //三人小队分身连招 跳跃完毕 
            case WeavileTeamBrain.SubState.TeamState_304_CloneShadow:
                JumpMoveOver();
                //根据分身连招状态不同 跳跃结束事件不同
                switch (TeamBrain.NowTeamState304CloneShadow)
                {
                    //第一次跳跃
                    case WeavileTeamBrain.TEAMSTATE304_CLONESHADOW.CloneShadow:
                        //根据冲刺方向确认朝向 发射冰粒
                        SetDirector(new Vector2(TeamBrain.isLeft_TeamState304CloneShadow_JumpStart ? 1 : -1, 0.0f));
                        //IceShardStart(Weavile.TIME_ICESHARD_TEMASTATE304IDLE_JUMP);
                        isCombatComplite_TeamState304_CloneShadow = true;
                        break;
                    //之后的跳跃
                    case WeavileTeamBrain.TEAMSTATE304_CLONESHADOW.Jump:
                        SetDirector(new Vector2(TeamBrain.isLeft_TeamState304CloneShadow_JumpStart ? 1 : -1, 0.0f));
                        //IceShardStart(Weavile.TIME_ICESHARD_TEMASTATE304IDLE_JUMP);
                        isCombatComplite_TeamState304_CloneShadow = true;
                        break;
                    //其他状态进入跳跃 说明状态机错误
                    default:
                        Debug.Log("Error:三人小队 分身连招 状态机错误 不该跳跃");
                        break;
                }
                break;
            //三人小队绕圈连招 脱离进攻 跳跃结束 开始冲刺
            case WeavileTeamBrain.SubState.TeamState_306_CircleRun:
                if (TeamBrain.NowTeamState306SubState == WeavileTeamBrain.TEAMSTATE306_SUBSTATE.EscapeAtk)
                {
                    JumpMoveOver();
                    RushStart();
                }
                break;
            //二人小队替身连招2 跳跃完毕 
            case WeavileTeamBrain.SubState.TeamState_204_Substitute2:
                JumpMoveOver();
                isCombatComplite_TeamState204_Substitute2 = true;
                //三人小队替身连招2 跳跃阶段 跳跃完毕后面向玩家
                if (TeamBrain.NowTeamState204SubState == WeavileTeamBrain.TEAMSTATE204_SUBSTATE.Jump)
                {
                    SetDirector(_mTool.MainVector2((ParentWeavile.TARGET_POSITION - (Vector2)transform.position).normalized));
                }
                break;
            //二人小队绕圈连招 脱离进攻 跳跃结束 开始冲刺
            case WeavileTeamBrain.SubState.TeamState_205_CircleRun:
                if (TeamBrain.NowTeamState205SubState == WeavileTeamBrain.TEAMSTATE306_SUBSTATE.EscapeAtk)
                {
                    JumpMoveOver();
                    RushStart();
                }
                break;
        }
    }

    //=========================跳跃事件============================






    //=========================冲刺事件============================

    /// <summary>
    /// 冲刺作为动画开始
    /// </summary>
    public void AnimatorEvent_RushIn()
    {
        isCharge_Rush = false;
        isMove_Rush = false;
    }

    /// <summary>
    /// 冲刺作为蓄力开始
    /// </summary>
    public void AnimatorEvent_RushCharge()
    {
        isCharge_Rush = true;
        isMove_Rush = false;
    }

    /// <summary>
    /// 冲刺作为蓄力结束
    /// </summary>
    public void AnimatorEvent_RushLaunch()
    {
        isCharge_Rush = false;
        isMove_Rush = false;
        //根据连招决定是否预判
        switch (TeamBrain.NowSubState)
        {
            case WeavileTeamBrain.SubState.TeamState_304_CloneShadow:
                //三人小队_近战连招 不是第一次冲刺 预判
                //if (TeamBrain.Count_Close_Atk != 0)
                //{
                //    Angle_PredictLastAngle_Rush = _mTool.Angle_360Y((TargetPosition - (Vector2)transform.position).normalized, Vector2.right);
                //    isPredict_RushAngle = true;
                //}
                break;
        }
    }

    /// <summary>
    /// 冲刺作为动作开始
    /// </summary>
    public void AnimatorEvent_RushStart()
    {
        isCharge_Rush = false;
        isMove_Rush = true;
        OverRushArrow();//冲刺开始后箭头消失
        //根据连招决定是否预判
        switch (TeamBrain.NowSubState)
        {
            case WeavileTeamBrain.SubState.TeamState_304_CloneShadow:
                //三人小队_近战连招 不是第一次冲刺预判
                //if (TeamBrain.Count_Close_Atk != 0)
                //{
                //    float Angle_PredictLastAngle_Rush02 = _mTool.Angle_360Y((TargetPosition - (Vector2)transform.position).normalized, Vector2.right);
                //    float PredictAngle = 2.0f * Angle_PredictLastAngle_Rush02 - Angle_PredictLastAngle_Rush;
                //    Dir_Rush = Quaternion.AngleAxis(PredictAngle, Vector3.forward) * Vector3.right;
                //    isPredict_RushAngle = false;
                //}
                break;
        }
        //Dir_Rush = ConfusionDir(Dir_Rush, 30.0f);
        if (ShadowCoroutine == null)
        {
            StartShadowCoroutine(0.05f, 1.5f, new Color(0.6603774f, 0.6603774f, 0.6603774f, 0.6f));
        }
        LaunchRushEffect(NowRushType, Dir_Rush);//特效生成
    }

    /// <summary>
    /// 冲刺作为动作结束
    /// </summary>
    public void AnimatorEvent_RushOver()
    {
        isCharge_Rush = false;
        isMove_Rush = false;
        //InstantiateRunDust(); //冲刺落地烟尘
        OverRushEffect();     //冲刺特效终结
        //落点特效
        //switch (NowRushType)
        //{
        //    case RushType.Ice:
        //        //冰系冲刺落点发射冰粒
        //        LaunchRushOverIceShard();
        //        break;
        //    case RushType.Dark:
        //        //恶习冲刺落点发射恶波
        //        LaunchDarkPulse();
        //        break;
        //}
        if (ShadowCoroutine != null)
        {
            StopShadowCoroutine();
        }
    }

    /// <summary>
    /// 冲刺作为动作结束
    /// </summary>
    public void AnimatorEvent_RushOut()
    {
        isCharge_Rush = false;
        isMove_Rush = false;
        switch (TeamBrain.NowSubState)
        {
            //三人小队分身连招 连招动作完毕 
            case WeavileTeamBrain.SubState.TeamState_304_CloneShadow:
                RushOver();
                //冲刺完后直接爆炸
                //SetCloneShadowOver();
                isCombatComplite_TeamState304_CloneShadow = true;
                break;
        }
    }

    //=========================冲刺事件============================










    //■■■■■■■■■■■■■■■■■■■■动画机事件■■■■■■■■■■■■■■■■■■■■■■







































    //■■■■■■■■■■■■■■■■■■■■预制件■■■■■■■■■■■■■■■■■■■■■■

    /// <summary>
    /// 冲刺箭头
    /// </summary>
    public SkillArrow WeavileRushArrow;
    /// <summary>
    /// 冲刺箭头实例
    /// </summary>
    SkillArrow RushArrowObj;

    /// <summary>
    /// 恶系冲刺实例
    /// </summary>
    WeavileRush RushObj;
    /// <summary>
    /// 恶系冲刺预制件
    /// </summary>
    public WeavileDarkRush FakeWeavileDarkRushPrefab;
    /// <summary>
    /// 毒系冲刺预制件
    /// </summary>
    public WeavilePosionRush FakeWeavilePosionRushPrefab;
    /// <summary>
    /// 冰系冲刺预制件
    /// </summary>
    public WeavileIceRush FakeWeavileIceRushPrefab;

    /// <summary>
    /// 冲刺落地尘埃特效
    /// </summary>
    public GameObject RushDust;


    /// <summary>
    /// 假冰粒
    /// </summary>
    public WeavileCloneBodyFakeIceShard FakeIceShardPrefabs;

    /// <summary>
    /// 假小恶波
    /// </summary>
    public WeavileCloneBodyFakeDarkPulseSmall FakeDarkPulseSmallPrefabs;

    /// <summary>
    /// 假微小恶波
    /// </summary>
    public WeavileCloneBodyFakeDarkPulseSmall FakeDarkPulseTinyPrefabs;


    /// <summary>
    /// 假投掷炸弹预制件
    /// </summary>
    public WeavileFilingBomb FakeWeavileFilingBombPrefabs;
    /// <summary>
    /// 假投掷毒雾炸弹预制件
    /// </summary>
    public WeavileFilingPoisionBomb FakeWeavileFilingPoisionBombPrefabs;
    /// <summary>
    /// 假投掷烟雾炸弹预制件
    /// </summary>
    public WeavileFilingSmokeBomb FakeWeavileFilingSmokeBombPrefabs;
    /// <summary>
    /// 假投掷伤药预制件
    /// </summary>
    public WeavileFilingHeal FakeWeavileFilingHealPrefabs;
    /// <summary>
    /// 假投掷替身预制件
    /// </summary>
    public WeavileFilingSubstitute FakeWeavileFilingSubstitutePrefabs;


    //■■■■■■■■■■■■■■■■■■■■预制件■■■■■■■■■■■■■■■■■■■■■■


























    //■■■■■■■■■■■■■■■■■■■■状态机部分■■■■■■■■■■■■■■■■■■■■■■
    //InsertStateFunction




    //=========================发呆============================







    /// <summary>
    /// 发呆计时器
    /// <summary>
    float IdleTimer = 0;

    /// <summary>
    /// 发呆开始
    /// <summary>
    public void IdleStart(float Timer)
    {
        IdleTimer = Timer;
        NowMainState = MainState.Idle;
    }

    /// <summary>
    /// 发呆结束
    /// <summary>
    public void IdleOver()
    {
        IdleTimer = 0;
    }


    //=========================发呆============================






    //=========================奔跑============================









    /// <summary>
    /// 奔跑计时器
    /// <summary>
    float RunTimer = 0;
    /// <summary>
    /// 奔跑方向
    /// </summary>
    Vector2 Dir_Run = Vector2.zero;






    /// <summary>
    /// 奔跑开始
    /// <summary>
    public void RunStart(/*float Timer*/)
    {
        RunTimer = 0;
        NowMainState = MainState.Run;
        Dir_Run = Vector2.zero;
    }

    /// <summary>
    /// 奔跑结束
    /// <summary>
    public void RunOver()
    {
        RunTimer = 0;
        Dir_Run = Vector2.zero;
        animator.SetFloat("Speed", 0.0f);
    }


    //根据距离目标距离设置偏转角度
    float SetTwoSideOffsetAngleByDistence(float Distence)
    {
        float d = Mathf.Clamp(Distence, Weavile.DISTENCE_WHEN_ANGLE_TWOSIDE_DEFLECTION_MIN, Weavile.DISTENCE_WHEN_ANGLE_TWOSIDE_DEFLECTION_MAX);
        float t = (d - Weavile.DISTENCE_WHEN_ANGLE_TWOSIDE_DEFLECTION_MIN) / (Weavile.DISTENCE_WHEN_ANGLE_TWOSIDE_DEFLECTION_MAX - Weavile.DISTENCE_WHEN_ANGLE_TWOSIDE_DEFLECTION_MIN);
        return Mathf.Lerp(Weavile.ANGLE_MAX_TEAMSTATE300_RUN_TOWSIDE_DEFLECTION, Weavile.ANGLE_MIN_TEAMSTATE300_RUN_TOWSIDE_DEFLECTION, t);
    }

    //根据距离目标距离设置速度加成
    float SetTwoSideSpeedAlphaByDistence(float Distence)
    {
        float d = Mathf.Clamp(Distence, Weavile.DISTENCE_WHEN_ANGLE_TWOSIDE_DEFLECTION_MIN, Weavile.DISTENCE_WHEN_ANGLE_TWOSIDE_DEFLECTION_MAX);
        float t = (d - Weavile.DISTENCE_WHEN_ANGLE_TWOSIDE_DEFLECTION_MIN) / (Weavile.DISTENCE_WHEN_ANGLE_TWOSIDE_DEFLECTION_MAX - Weavile.DISTENCE_WHEN_ANGLE_TWOSIDE_DEFLECTION_MIN);
        return Mathf.Lerp(Weavile.SPEEDALPHA_TEAMSTATE300_RUN_TOWSIDE_MAX, Weavile.SPEEDALPHA_TEAMSTATE300_RUN_TOWSIDE_MIN, t);
    }


    //=========================奔跑============================






    //=========================跳跃移动============================









    /// <summary>
    /// 跳跃移动计时器
    /// <summary>
    float JumpMoveTimer = 0;

    /// <summary>
    /// 跳跃移动的最大时间
    /// </summary>
    float Time_Jump_Max = 0.0f;

    /// <summary>
    /// 跳跃移动的速度加成（定时模式）
    /// </summary>
    float SpeedAlpha_Jump = 0.0f;

    /// <summary>
    /// 跳跃移动的方向
    /// </summary>
    Vector2 Dir_Jump = Vector2.zero;

    /// <summary>
    /// 跳跃是否开始移动
    /// </summary>
    bool isMove_Jump = false;

    /// <summary>
    /// 冲刺是否在准备期间
    /// </summary>
    bool isPrepare_Jump = false;




    /// <summary>
    /// 跳跃移动开始
    /// <summary>
    public void JumpMoveStart(/*float Timer*/)
    {
        JumpMoveTimer = 0;
        Time_Jump_Max = 0.0f;
        SpeedAlpha_Jump = 0.0f;
        Dir_Jump = Vector2.zero;
        isMove_Jump = false;
        isPrepare_Jump = true;
        NowMainState = MainState.JumpMove;
        //animator.SetInteger("Jump" , 1);
    }

    /// <summary>
    /// 跳跃移动结束
    /// <summary>
    public void JumpMoveOver()
    {
        JumpMoveTimer = 0;
        Time_Jump_Max = 0.0f;
        SpeedAlpha_Jump = 0.0f;
        Dir_Jump = Vector2.zero;
        isMove_Jump = false;
        isPrepare_Jump = false;
        animator.SetInteger("Jump", 0);
    }












    //=========================跳跃移动============================






    //=========================冲刺============================






    /// <summary>
    /// 冲刺的种类
    /// </summary>
    enum RushType
    {
        Ice,     //冰拳 落点释放八向冰粒
        Dark,    //暗袭要害  落点释放大范围恶波
        Posion,  //毒击 沿途释放毒雾
    }
    RushType NowRushType;










    /// <summary>
    /// 冲刺计时器
    /// <summary>
    float RushTimer = 0;
    /// <summary>
    /// 冲刺是否在蓄力
    /// </summary>
    bool isCharge_Rush = false;
    /// <summary>
    /// 冲刺是否在移动
    /// </summary>
    bool isMove_Rush = false;
    /// <summary>
    /// 是否开始预测冲刺角度
    /// </summary>
    bool isPredict_RushAngle = false;
    /// <summary>
    /// 冲刺角度
    /// </summary>
    Vector2 Dir_Rush = Vector2.zero;
    /// <summary>
    /// 用来预判的目标上一次目标点
    /// </summary>
    float Angle_PredictLastAngle_Rush = 0;
    /// <summary>
    /// 冲刺时发射移动特效的时间间隔计时器
    /// </summary>
    float Timer_Rush_MoveEffect_Interval = 0.0f;




    /// <summary>
    /// 冲刺开始
    /// <summary>
    public void RushStart(/*float Timer*/)
    {
        RushTimer = 0;
        NowMainState = MainState.Rush;
        isCharge_Rush = false;
        isMove_Rush = false;
        isPredict_RushAngle = false;
        Dir_Rush = Vector2.zero;
        Angle_PredictLastAngle_Rush = 0;
        Timer_Rush_MoveEffect_Interval = 0.0f;
        switch (TeamBrain.NowSubState)
        {
            //三人小队分身连招
            case WeavileTeamBrain.SubState.TeamState_304_CloneShadow:
                switch (TeamBrain.NowTeamState304CloneShadow)
                {
                    //终结冲刺
                    case WeavileTeamBrain.TEAMSTATE304_CLONESHADOW.OverRush:
                        //冲刺种类根据主脑冲刺方向决定
                        Dir_Rush = new Vector2((TeamBrain.isLeft_TeamState304CloneShadow_JumpStart) ? 1 : -1, 0.0f);
                        //冲刺种类恶
                        NowRushType = RushType.Dark;
                        SetRushArrow(NowRushType, ParentWeavile.ParentPokemonRoom.GetRoomBoundaryByRaycastTo(transform.position, Dir_Rush));
                        break;
                    //其他状态转入报错休息
                    default:
                        {
                            RushOver();
                            IdleStart(Weavile.TIME_IDLE_START);
                            Debug.Log("Error:绕圈连招攻击错误");
                        }
                        return;
                }
                break;
        }
        animator.SetInteger("Rush", 1);
        
    }

    /// <summary>
    /// 冲刺结束
    /// <summary>
    public void RushOver()
    {
        RushTimer = 0;
        isCharge_Rush = false;
        isMove_Rush = false;
        isPredict_RushAngle = false;
        Dir_Rush = Vector2.zero;
        animator.SetInteger("Rush", 0);
        Angle_PredictLastAngle_Rush = 0;
        Timer_Rush_MoveEffect_Interval = 0.0f;
    }

    /// <summary>
    /// 设置冲刺指示箭头
    /// </summary>
    void SetRushArrow(RushType t, Vector2 targetPosition)
    {
        if (RushArrowObj != null)
        {
            Destroy(RushArrowObj);
        }
        RushArrowObj = Instantiate(WeavileRushArrow, transform.position, Quaternion.identity, transform);
        RushArrowObj.SetTarget(targetPosition);
        //设置颜色
        switch (t)
        {
            case RushType.Ice: RushArrowObj.ArrowColor = SkillArrow.COLOR_SKILLARROW_ICE; break;
            case RushType.Dark: RushArrowObj.ArrowColor = SkillArrow.COLOR_SKILLARROW_DARK; break;
            case RushType.Posion: RushArrowObj.ArrowColor = SkillArrow.COLOR_SKILLARROW_POSION; break;
        }
    }

    /// <summary>
    /// 终结冲刺指示箭头
    /// </summary>
    void OverRushArrow()
    {
        if (RushArrowObj != null)
        {
            RushArrowObj.transform.parent = null;
            RushArrowObj.ArrowOver();
        }
    }

    /// <summary>
    /// 生成冲刺特效
    /// </summary>
    /// <param name="t"></param>
    /// <param name="d"></param>
    void LaunchRushEffect(RushType t, Vector2 d)
    {
        if (RushObj != null)
        {
            Destroy(RushObj.gameObject);
        }
        switch (t)
        {
            case RushType.Ice:
                LaunchIceRush(d);
                break;
            case RushType.Dark:
                LaunchDarkRush(d);
                break;
            case RushType.Posion:
                LaunchPosionRush(d);
                break;
        }
    }

    void OverRushEffect()
    {
        if (RushObj != null)
        {
            RushObj.RushOver();
            RushObj.transform.parent = null;
            RushObj = null;
        }
    }

    /// <summary>
    /// 生成冰系冲刺
    /// </summary>
    void LaunchIceRush(Vector2 d)
    {
        //RushObj = Instantiate(WeavileIceRushPrefab, transform.position, Quaternion.identity, transform);
        //RushObj.RushStart(d);
        //RushObj.ParentWeavile = this;
    }

    /// <summary>
    /// 生成恶系冲刺
    /// </summary>
    void LaunchDarkRush(Vector2 d)
    {
        RushObj = Instantiate(FakeWeavileDarkRushPrefab, transform.position, Quaternion.identity, transform);
        RushObj.RushStart(d);
        //RushObj.ParentWeavile = this;
    }

    /// <summary>
    /// 生成毒系冲刺
    /// </summary>
    void LaunchPosionRush(Vector2 d)
    {
        //RushObj = Instantiate(WeavilePosionRushPrefab, transform.position, Quaternion.identity, transform);
        //RushObj.RushStart(d);
        //RushObj.ParentWeavile = this;
    }


    /// <summary>
    /// 生成毒雾
    /// </summary>
    void LaunchPosionMist(Vector2 d)
    {
        //WeavilrPosionRushPosionMist pm = Instantiate(WeavilePosionMistPrefabs, transform.position, Quaternion.Euler(0, 0, _mTool.Angle_360Y(d, Vector2.right)));
        //pm.ParentWeavile = this;
    }


    /**

    /// <summary>
    /// 随机获取冲刺种类
    /// </summary>
    RushType RandomGetRushType()
    {
        RushType t = RushType.Ice;
        int i = Random.Range(0, 3);
        switch (i)
        {
            case 0: t = RushType.Ice; break;
            case 1: t = RushType.Dark; break;
            case 2: t = RushType.Posion; break;
        }
        return t;
    }




    /// <summary>
    /// 生成冰系冲刺落点冰粒散射
    /// </summary>
    void LaunchRushOverIceShard()
    {
        int count = COUNT_ICESHARD_ICSRUSHOVER_ICBREAK;
        float angle = 360.0f / (float)count;
        for (int i = 0; i < count; i++)
        {
            Vector2 LaunchDir = Quaternion.AngleAxis(angle * i, Vector3.forward) * Vector2.right;
            LaunchIceShard(LaunchDir);
        }
    }
    **/

    //=========================冲刺============================






    //=========================原地防守============================




    /// <summary>
    /// 原地防守计时器
    /// <summary>
    float GuardTimer = 0;

    /// <summary>
    /// 原地防守开始
    /// <summary>
    public void GuardStart(/*float Timer*/)
    {
        GuardTimer = 0;
        NowMainState = MainState.Guard;
    }

    /// <summary>
    /// 原地防守结束
    /// <summary>
    public void GuardOver()
    {
        GuardTimer = 0;
    }


    //=========================原地防守============================






    //=========================使用保护============================





    /// <summary>
    /// 使用保护计时器
    /// <summary>
    float UseProtectTimer = 0;




    /// <summary>
    /// 使用保护开始
    /// <summary>
    public void UseProtectStart(float Timer)
    {
        UseProtectTimer = Timer;
        NowMainState = MainState.UseProtect;
        animator.SetInteger("Maze", 1);
        SetDirector(Vector2.down);
    }

    /// <summary>
    /// 使用保护结束
    /// <summary>
    public void UseProtectOver()
    {
        UseProtectTimer = 0;
        animator.SetInteger("Maze", 0);
    }





    //=========================使用保护============================






    //=========================发射冰粒============================






     

    /// <summary>
    /// 发射冰粒计时器
    /// <summary>
    float IceShardTimer = 0;



    /// <summary>
    /// 发射冰粒开始
    /// <summary>
    public void IceShardStart(float Timer)
    {
        IceShardTimer = Timer;
        NowMainState = MainState.IceShard;
        animator.SetTrigger("Atk");
        switch (TeamBrain.NowSubState)
        {
            //三人小队 分身连招 跳跃完毕后发射3连发冰粒
            case WeavileTeamBrain.SubState.TeamState_304_CloneShadow:
                LaunchIceShard3_TeamState304CloneShadow(TeamBrain.isLeft_TeamState304CloneShadow_JumpStart);
                break;
        }

    }

    /// <summary>
    /// 发射冰粒结束
    /// <summary>
    public void IceShardOver()
    {
        IceShardTimer = 0;
    }



    /// <summary>
    /// 三人小队 分身连招 跳跃完毕后发射3连发冰粒
    /// </summary>
    public void LaunchIceShard3_TeamState304CloneShadow(bool isLeft)
    {

        Vector2 d1 = new Vector2(isLeft ? 1 : -1, 0.0f);
        Vector2 d2 = Quaternion.AngleAxis(Weavile.ANGLE_ICESHARD_TEMASTATE303CLONESHADOW_JUMPOVER_OFFSET, Vector3.forward) * d1;
        Vector2 d3 = Quaternion.AngleAxis(-Weavile.ANGLE_ICESHARD_TEMASTATE303CLONESHADOW_JUMPOVER_OFFSET, Vector3.forward) * d1;
        LaunchIceShard(d1);
        LaunchIceShard(d2);
        LaunchIceShard(d3);
    }

    /**
    /// <summary>
    /// 防守时发射三发冰粒
    /// </summary>
    public void LaunchIceShard3_Guard()
    {
        Vector2 d1 = (TargetPosition - (Vector2)transform.position).normalized;
        Vector2 d2 = Quaternion.AngleAxis(ANGLE_ICESHARD_TEMASTATE300IDLE_GUARD_OFFSET, Vector3.forward) * d1;
        Vector2 d3 = Quaternion.AngleAxis(-ANGLE_ICESHARD_TEMASTATE300IDLE_GUARD_OFFSET, Vector3.forward) * d1;
        LaunchIceShard(d1);
        LaunchIceShard(d2);
        LaunchIceShard(d3);
    }
    **/

    /// <summary>
    /// 发射冰粒
    /// </summary>
    /// <param name="d"></param>
    public void LaunchIceShard(Vector2 d)
    {
        d = d.normalized;
        //生成假冰粒
        WeavileCloneBodyFakeIceShard fisobj = Instantiate(FakeIceShardPrefabs, transform.position + (Vector3)d.normalized * 0.5f, Quaternion.Euler(0, 0, _mTool.Angle_360Y(d, Vector2.right)));
        //isobj.empty = this;
        fisobj.LaunchNotForce(d, fisobj.MoveSpeed);
    }

    //=========================发射冰粒============================






    //=========================发射恶波============================




    /// <summary>
    /// 发射恶波计时器
    /// <summary>
    float DarkPulseTimer = 0;

    /// <summary>
    /// 发射恶波开始
    /// <summary>
    public void DarkPulseStart(float Timer)
    {
        DarkPulseTimer = Timer;
        NowMainState = MainState.DarkPulse;
    }

    /// <summary>
    /// 发射恶波结束
    /// <summary>
    public void DarkPulseOver()
    {
        DarkPulseTimer = 0;
    }

    /// <summary>
    /// 发射小恶波
    /// </summary>
    void LaunchDarkPulseSmall(Vector2 p)
    {
        WeavileCloneBodyFakeDarkPulseSmall dp = Instantiate(FakeDarkPulseSmallPrefabs, p, Quaternion.identity);
        //dp.ParentWeavile = this;
    }


    void LaunchDarkPulseTiny(Vector2 p)
    {
        WeavileCloneBodyFakeDarkPulseSmall dp = Instantiate(FakeDarkPulseTinyPrefabs, p, Quaternion.identity);
        //dp.ParentWeavile = this;
    }

    /**
    /// <summary>
    /// 发射恶波
    /// </summary>
    void LaunchDarkPulse()
    {
        WeavileDarkPulse dp = Instantiate(WeavileDarkPulsePrefabs, transform.position, Quaternion.identity, transform);
        dp.ParentWeavile = this;
    }


    **/


    //=========================发射恶波============================






    //=========================投掷============================

    /// <summary>
    /// 投掷种类
    /// <summary>
    Weavile.FlingType NowFlingType;


    /// <summary>
    /// 投掷计时器
    /// <summary>
    float FlingTimer = 0;
    /// <summary>
    /// 投掷目标点
    /// </summary>
    Vector2 Position_Fling_Target = Vector2.zero;
    /// <summary>
    /// 投掷是否在准备期间
    /// </summary>
    bool isPrepare_Fling = false;


    /// <summary>
    /// 投掷开始
    /// <summary>
    public void FlingStart(/*float Timer*/)
    {
        FlingTimer = 0;
        Position_Fling_Target = Vector2.zero;
        isPrepare_Fling = true;

        //设置准备时间
        switch (TeamBrain.NowSubState)
        {
            //二人小队 替身连招2
            case WeavileTeamBrain.SubState.TeamState_204_Substitute2:
                //无准备时间
                FlingTimer = 0.0f;
                break;
        }

        NowMainState = MainState.Fling;
    }

    /// <summary>
    /// 投掷结束
    /// <summary>
    public void FlingOver()
    {
        FlingTimer = 0;
        Position_Fling_Target = Vector2.zero;
        isPrepare_Fling = false;
    }


    /// <summary>
    /// 根据种类向目标发射投掷物
    /// </summary>
    /// <param name="type"></param>
    /// <param name="target"></param>
    void LaunchFlingProjectiles(Weavile.FlingType type, Vector2 target)
    {
        WeavileProjectiles p = null;
        //生成投掷物
        switch (type)
        {
            //炸弹
            case Weavile.FlingType.Bomb:
                p = Instantiate(FakeWeavileFilingBombPrefabs, transform.position, Quaternion.identity);
                break;
            //毒雾炸弹
            case Weavile.FlingType.PoisonBomb:
                p = Instantiate(FakeWeavileFilingPoisionBombPrefabs, transform.position, Quaternion.identity);
                break;
            //烟雾炸弹
            case Weavile.FlingType.SmokeBomb:
                p = Instantiate(FakeWeavileFilingSmokeBombPrefabs, transform.position, Quaternion.identity);
                break;
            //伤药
            case Weavile.FlingType.Heal:
                p = Instantiate(FakeWeavileFilingHealPrefabs, transform.position, Quaternion.identity);
                break;
            //替身
            case Weavile.FlingType.Substitute:
                p = Instantiate(FakeWeavileFilingSubstitutePrefabs, transform.position, Quaternion.identity);
                break;
        }

        //发射
        Vector2 dir = (target - (Vector2)transform.position).normalized;
        float dis = Vector2.Distance(target, (Vector2)transform.position);
        p.SetWeavileProjectiles(ParentWeavile, dir, dis);


    }

    //=========================投掷============================






    //=========================释放迷宫============================




    /// <summary>
    /// 释放迷宫计时器
    /// <summary>
    float MazeTimer = 0;

    /// <summary>
    /// 释放迷宫开始
    /// <summary>
    public void MazeStart(float Timer)
    {
        MazeTimer = Timer;
        NowMainState = MainState.Maze;
    }

    /// <summary>
    /// 释放迷宫结束
    /// <summary>
    public void MazeOver()
    {
        MazeTimer = 0;
    }


    //=========================释放迷宫============================






    //=========================看护迷宫============================




    /// <summary>
    /// 看护迷宫计时器
    /// <summary>
    float MazeWatcherTimer = 0;

    /// <summary>
    /// 看护迷宫开始
    /// <summary>
    public void MazeWatcherStart(float Timer)
    {
        MazeWatcherTimer = Timer;
        NowMainState = MainState.MazeWatcher;
    }

    /// <summary>
    /// 看护迷宫结束
    /// <summary>
    public void MazeWatcherOver()
    {
        MazeWatcherTimer = 0;
    }


    //=========================看护迷宫============================









    //=========================绕圈跑============================




    /// <summary>
    /// 绕圈跑计时器
    /// <summary>
    float CircleRunTimer = 0;

    /// <summary>
    /// 绕圈跑的速度加成
    /// </summary>
    public float SPEEDALPHA_TeamState306CircleRun
    {
        get { return SpeedAlpha_TeamState306CircleRun; }
        set { SpeedAlpha_TeamState306CircleRun = value; }
    }
    float SpeedAlpha_TeamState306CircleRun = 0.0f;

    /// <summary>
    /// 绕圈跑的方向
    /// </summary>
    Vector2 Dir_CircleRun = Vector2.zero;



    /// <summary>
    /// 绕圈跑开始
    /// <summary>
    public void CircleRunStart()
    {
        CircleRunTimer = 0;
        SpeedAlpha_TeamState306CircleRun = 0.0f;
        Dir_CircleRun = Vector2.zero;
        NowMainState = MainState.CircleRun;
        if (ShadowCoroutine == null)
        {
            StartShadowCoroutine(0.05f, 1.5f, new Color(0.6603774f, 0.6603774f, 0.6603774f, 0.6f));
        }
    }

    /// <summary>
    /// 绕圈跑结束
    /// <summary>
    public void CircleRunOver()
    {
        CircleRunTimer = 0;
        SpeedAlpha_TeamState306CircleRun = 0.0f;
        Dir_CircleRun = Vector2.zero;
        if (ShadowCoroutine != null)
        {
            StopShadowCoroutine();
        }
    }


    //=========================绕圈跑============================











    //=========================分身============================




    /// <summary>
    /// 分身计时器
    /// <summary>
    float CloneShadowTimer = 0;

    /// <summary>
    /// 分身开始
    /// <summary>
    public void CloneShadowStart(float Timer)
    {
        CloneShadowTimer = Timer;
        NowMainState = MainState.CloneShadow;

    }

    /// <summary>
    /// 分身结束
    /// <summary>
    public void CloneShadowOver()
    {
        CloneShadowTimer = 0;
    }


    //=========================分身============================






    //=========================回复============================




    /// <summary>
    /// 回复计时器
    /// <summary>
    float HealTimer = 0;

    /// <summary>
    /// 回复开始
    /// <summary>
    public void HealStart(float Timer)
    {
        HealTimer = Timer;
        NowMainState = MainState.Heal;
    }

    /// <summary>
    /// 回复结束
    /// <summary>
    public void HealOver()
    {
        HealTimer = 0;
    }


    //=========================回复============================




    //■■■■■■■■■■■■■■■■■■■■状态机部分■■■■■■■■■■■■■■■■■■■■■■






}
