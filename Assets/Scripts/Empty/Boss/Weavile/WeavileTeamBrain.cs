using System.Collections.Generic;
using UnityEngine;

public class WeavileTeamBrain : MonoBehaviour
{


    /// <summary>
    /// 目标玩家
    /// </summary>
    PlayerControler player;



    /// <summary>
    /// 团队主脑的目标的坐标
    /// </summary>
    public Vector2 TARGET_POSITION
    {
        get { return TargetPosition; }
        set { TargetPosition = value; }
    }
    Vector2 TargetPosition;












    //==============================状态机枚举===================================


    /// <summary>
    /// 主状态
    /// </summary>
    public enum MainState
    {
        Team3,  //三人小队
        Team2,  //二人小队
        Team1,  //一人小队
        Team0,  //0人小队
    }
    public MainState NowMainState;


    /// <summary>
    /// 副状态
    /// </summary>
    public enum SubState
    {
        TeamState_300_Idle,    //三人小队_发呆状态	
        TeamState_301_Run,    //三人小队_巡逻状态	
        TeamState_302_CloseAtk,    //三人小队_近战连招	
        TeamState_303_Substitute,    //三人小队_替身连招	
        TeamState_304_CloneShadow,    //三人小队_分身连招	
        TeamState_305_Maze,    //三人小队_迷宫连招	
        TeamState_306_CircleRun,    //三人小队_绕圈连招	
        TeamState_307_Heal1,    //三人小队_回血连招1	
        TeamState_308_Heal2,    //三人小队_回血连招2	
        TeamState_309_FullHeal,    //三人小队_万灵药连招
        TeamState_200_Idle,    //二人小队_发呆状态	
        TeamState_201_Run,    //二人小队_巡逻状态	
        TeamState_202_CloseAtk,    //二人小队_近战连招	
        TeamState_203_Substitute,    //二人小队_替身连招	
        TeamState_204_Substitute2,    //二人小队_替身连招2	
        TeamState_205_CircleRun,    //二人小队_绕圈连招	
        TeamState_206_Heal,    //二人小队_回血连招	
        TeamState_207_Maze,    //二人小队_迷宫连招	
        TeamState_208_FullHeal,    //二人小队_万灵药连招	
        TeamState_100_Normal,    //一人小队_巡逻状态	
    }
    public SubState NowSubState;


    /// <summary>
    /// 状态映射关系
    /// </summary>
    private static Dictionary<MainState, SubState[]> StateMap = new()
    {
        { MainState.Team3, new[] { SubState.TeamState_300_Idle, SubState.TeamState_301_Run, SubState.TeamState_302_CloseAtk, SubState.TeamState_303_Substitute, SubState.TeamState_304_CloneShadow, SubState.TeamState_305_Maze, SubState.TeamState_306_CircleRun, SubState.TeamState_307_Heal1, SubState.TeamState_308_Heal2, SubState.TeamState_309_FullHeal } },
        { MainState.Team2, new[] { SubState.TeamState_200_Idle, SubState.TeamState_201_Run, SubState.TeamState_202_CloseAtk, SubState.TeamState_203_Substitute, SubState.TeamState_204_Substitute2, SubState.TeamState_205_CircleRun, SubState.TeamState_206_Heal, SubState.TeamState_207_Maze, SubState.TeamState_208_FullHeal } },
        { MainState.Team1, new[] { SubState.TeamState_100_Normal} },
    };



    //==============================状态机枚举===================================









    //■■■■■■■■■■■■■■■■■■■■玛狃拉小队■■■■■■■■■■■■■■■■■■■■■■


    /// <summary>
    /// 玛狃拉成员列表
    /// </summary>
    public List<Weavile> WeavileMembersList = new List<Weavile> { null, null, null };


    /// <summary>
    /// 连招时玛狃拉01
    /// </summary>
    public Weavile CombatWeavile01 { get; private set; } = null;

    /// <summary>
    /// 连招时玛狃拉02
    /// </summary>
    public Weavile CombatWeavile02 { get; private set; } = null;

    /// <summary>
    /// 连招时玛狃拉03
    /// </summary>
    public Weavile CombatWeavile03 { get; private set; } = null;





    private void Start()
    {
        
    }



    private void OnEnable()
    {
        //排空玛狃拉列表
        _mTool.RemoveNullInList<Weavile>(WeavileMembersList);
        //忽略碰撞
        IgnoreCollisionBetweenTeamMembers();
        //开局发呆
        TeamState_300_Idle_Start();
    }


    private void OnDisable()
    {
        //引爆所有分身
        DetonateAllCloneBodys();

        //排空玛狃拉列表
        _mTool.RemoveNullInList<Weavile>(WeavileMembersList);
        _mTool.RemoveNullInList<Transform>(WeavileAndCloneShadowList);

        //完全重置当前玛狃拉连招分配
        CombatWeavile01 = null;
        CombatWeavile02 = null;
        CombatWeavile03 = null;

        //重置主脑状态机
        switch (NowSubState)
        {
            case SubState.TeamState_300_Idle:    //三人小队_发呆状态	
                TeamState_300_Idle_Over(); break;
            case SubState.TeamState_301_Run:    //三人小队_巡逻状态	
                TeamState_301_Run_Over(); break;
            case SubState.TeamState_302_CloseAtk:    //三人小队_近战连招
                TeamState_302_CloseAtk_Over(); break;
            case SubState.TeamState_303_Substitute:    //三人小队_替身连招	
                TeamState_303_Substitute_Over(); break;
            case SubState.TeamState_304_CloneShadow:    //三人小队_分身连招	
                TeamState_304_CloneShadow_Over(); break;
            case SubState.TeamState_305_Maze:    //三人小队_迷宫连招	
                TeamState_305_Maze_Over(); break;
            case SubState.TeamState_306_CircleRun:    //三人小队_绕圈连招	
                TeamState_306_CircleRun_Over(); break;
            case SubState.TeamState_307_Heal1:    //三人小队_回血连招1	
                TeamState_307_Heal1_Over(); break;
            case SubState.TeamState_308_Heal2:    //三人小队_回血连招2	
                TeamState_308_Heal2_Over(); break;
            case SubState.TeamState_309_FullHeal:    //三人小队_万灵药连招	
                TeamState_309_FullHeal_Over(); break;
            case SubState.TeamState_200_Idle:    //二人小队_发呆状态	
                TeamState_200_Idle_Over(); break;
            case SubState.TeamState_201_Run:    //二人小队_巡逻状态	
                TeamState_201_Run_Over(); break;
            case SubState.TeamState_202_CloseAtk:    //二人小队_近战连招
                TeamState_202_CloseAtk_Over(); break;
            case SubState.TeamState_203_Substitute:    //二人小队_替身连招	
                TeamState_203_Substitute_Over(); break;
            case SubState.TeamState_204_Substitute2:    //二人小队_替身连招2	
                TeamState_204_Substitute2_Over(); break;
            case SubState.TeamState_205_CircleRun:    //二人小队_绕圈连招
                TeamState_205_CircleRun_Over(); break;
            case SubState.TeamState_206_Heal:    //二人小队_回血连招	
                TeamState_206_Heal_Over(); break;
            case SubState.TeamState_207_Maze:    //二人小队_迷宫连招	
                TeamState_207_Maze_Over(); break;
            case SubState.TeamState_208_FullHeal:    //二人小队_万灵药连招	
                TeamState_208_FullHeal_Over(); break;
            case SubState.TeamState_100_Normal:    //一人小队_巡逻状态	
                TeamState_100_Normal_Over(); break;
        }
        //重置状态机通用参数
        Timer_HealCD_TeamState3 = 0.0f;
        Timer_HealCD_TeamState2 = 0.0f;

        //消除实例
        if (TeamState303_Substitute_SubstituteOBJ != null) { TeamState303_Substitute_SubstituteOBJ.BeHit(TeamState303_Substitute_SubstituteOBJ.MaxHP); };
        if (TeamState203_Substitute_SubstituteOBJ != null) { TeamState203_Substitute_SubstituteOBJ.BeHit(TeamState203_Substitute_SubstituteOBJ.MaxHP); };
        if (TeamState204_Substitute2_SubstituteOBJ != null) { TeamState204_Substitute2_SubstituteOBJ.BeHit(TeamState204_Substitute2_SubstituteOBJ.MaxHP); };
        WeavileSmokeList.Clear();

        //开局发呆
        TeamState_300_Idle_Start();
    }




    private void Update()
    {
        //■获取玩家
        ResetPlayer();

        //■检测成员变化
        MembersCheck();

        //Debug.Log(CheckWeavileMapping());
        //■判断主状态机
        //连招成员和小队成员是否对应
        if (CheckWeavileMapping()) {
            switch (NowMainState)
            {
                //■三人小队
                case MainState.Team3:
                    //●确定连招职位分配是否正确
                    if (!(CombatWeavile01 != null && CombatWeavile01.teamCombatActIndex == 1 &&
                        CombatWeavile02 != null && CombatWeavile02.teamCombatActIndex == 2 &&
                        CombatWeavile03 != null && CombatWeavile03.teamCombatActIndex == 3))
                    {
                        SetWeavileCombatIndex_Random();
                        Debug.Log("Error:三人小队 连招职位分配错误");
                        break;
                    }
                    //●回复连招的冷却时间减少
                    if (Timer_HealCD_TeamState3 > 0.0f)
                    {
                        Timer_HealCD_TeamState3 -= Time.deltaTime;
                    }
                    //●有玛狃拉陷入异常状态 转入万灵药状态
                    if  ((NowSubState != SubState.TeamState_309_FullHeal || (NowSubState == SubState.TeamState_309_FullHeal && NowTeamState309SubState != TEAMSTATE309_SUBSTATE.Heal) ) && (CombatWeavile01 != null && CombatWeavile01.isWeavileUnusualState() ||
                        CombatWeavile02 != null && CombatWeavile02.isWeavileUnusualState() ||
                        CombatWeavile03 != null && CombatWeavile03.isWeavileUnusualState() ))
                    {
                        ResetState();
                        TeamState_309_FullHeal_Start();
                    }
                    //●判断副状态机
                    switch (NowSubState)
                    {
                        //●三人小队_发呆状态
                        case SubState.TeamState_300_Idle:
                            {
                                //全部休息结束
                                if (CombatWeavile01.NowMainState == Weavile.MainState.Guard &&
                                    CombatWeavile02.NowMainState == Weavile.MainState.Guard &&
                                    CombatWeavile03.NowMainState == Weavile.MainState.Guard
                                    )
                                {
                                    TeamState_300_Idle_Over();
                                    TeamState_301_Run_Start();
                                }
                            }
                            break;
                        //●三人小队_巡逻状态
                        case SubState.TeamState_301_Run:
                            //转入回血连招检测
                            {
                                //冷却结束 开始判断
                                if (Timer_HealCD_TeamState3 <= 0.0f)
                                {
                                    //检测回复连招2条件
                                    if (Condition_TeamState308_Heal2())
                                    {
                                        Timer_HealCD_TeamState3 = 0.0f;
                                        TeamState_301_Run_Over();
                                        SetWeavileCombatIndex_TeamState_307_Heal1_TeamState_308_Heal2();
                                        TeamState_308_Heal2_Start();
                                        break;
                                    }
                                    //检测回复连招1条件
                                    if (Condition_TeamState307_Heal1())
                                    {
                                        Timer_HealCD_TeamState3 = 0.0f;
                                        TeamState_301_Run_Over();
                                        SetWeavileCombatIndex_TeamState_307_Heal1_TeamState_308_Heal2();
                                        TeamState_307_Heal1_Start();
                                        break;
                                    }
                                }
                            }
                            //巡逻逻辑
                            {
                                //判断包围是否形成
                                //包围成功
                                if (TeamState301_Run_CheckSurroundSccess(TargetPosition))
                                {
                                    //包围还没成功过,设置成功过旗
                                    if (!isSurroundSccessOnceFlg_TeamState301Run) { isSurroundSccessOnceFlg_TeamState301Run = true; }
                                    Timer_TeamState_301_Run_Surround_Success += Time.deltaTime;
                                    Timer_TeamState_301_Run_Surround_Fail = 0.0f;
                                    Debug.Log("Surround_Success" + "+" + Timer_TeamState_301_Run_Surround_Success);
                                }
                                //包围失败
                                else
                                {
                                    //包围成功过之后，才开始计算包围失败
                                    if (isSurroundSccessOnceFlg_TeamState301Run)
                                    {
                                        Timer_TeamState_301_Run_Surround_Success = 0.0f;
                                        Timer_TeamState_301_Run_Surround_Fail += Time.deltaTime;
                                        Debug.Log("Surround_Fail" + "+" + Timer_TeamState_301_Run_Surround_Fail);
                                    }
                                }
                                //设置关系 重设巡逻位置关系
                                SetWeavileCombatIndex_TeamState_301_Run(TargetPosition);

                                //随机点数
                                float r = Random.Range(0.0f , 1.0f);
                                //触发联招条件 巡逻结束 转入其他连招
                                //包围时间足够长，转入绕圈跑
                                if (Timer_TeamState_301_Run_Surround_Success >= TIME_TEAM301_RUN_2_306_CIRCLERUN_SURROUND_SUCCESS)
                                {
                                    TeamState_301_Run_Over();
                                    //93%概率转入绕圈跑
                                    if (r >= 0.0f && r < 1.0f - PERCENT_ANOTHERCOMBAT2MAZE)
                                    {
                                        TeamState_306_CircleRun_Start();
                                        Debug.Log("CircleRun");
                                    }
                                    //7%概率转入迷宫
                                    else
                                    {
                                        TeamState_305_Maze_Start();
                                        Debug.Log("Maze_CircleRun");
                                    }
                                }
                                //包围失败时间足够长，转入替身连招或分身连招
                                else if (Timer_TeamState_301_Run_Surround_Fail >= TIME_TEAM301_RUN_2_303_Substitute_304_CloneShadow_SURROUND_FAIL)
                                {
                                    TeamState_301_Run_Over();
                                    //46.5%概率转入替身连招
                                    if (r >= 0.0f && r < ((1.0f - PERCENT_ANOTHERCOMBAT2MAZE) / 2.0f))
                                    {
                                        TeamState_303_Substitute_Start();
                                        Debug.Log("Substitute");
                                    }
                                    //46.5%概率转入分身连招
                                    else if (r >= ((1.0f - PERCENT_ANOTHERCOMBAT2MAZE) / 2.0f) && r < 1.0f - PERCENT_ANOTHERCOMBAT2MAZE)
                                    {
                                        TeamState_304_CloneShadow_Start();
                                        Debug.Log("CloneShadow");
                                    }
                                    //7%概率转入迷宫
                                    else
                                    {
                                        TeamState_305_Maze_Start();
                                        Debug.Log("Maze_Substitute");
                                    }
                                }
                                //有玛狃拉被接近
                                Weavile closedW = TeamState301_Run_CheckAllWeaileBeClosed();
                                if (closedW != null)
                                {
                                    TeamState_301_Run_Over();
                                    //93%概率转入近战连招
                                    if (r >= 0.0f && r < 1.0f - PERCENT_ANOTHERCOMBAT2MAZE)
                                    {
                                        SetWeavileCombatIndex_TeamState_302_CloseAtk_FirstTime(TargetPosition, closedW);
                                        TeamState_302_CloseAtk_Start();
                                        Debug.Log("CloseAtk");
                                    }
                                    //7%概率转入迷宫
                                    else
                                    {
                                        TeamState_305_Maze_Start();
                                        Debug.Log("Maze_CloseAtk");
                                    }
                                }
                                //随着时间增加 判定转入迷宫连招
                                Timer_TeamState_301_Run_TeamState301Run2TeamState305Maze += Time.deltaTime;
                                //大于当前等级的所定时间 按照当前概率转入迷宫
                                if (Level_TeamState301Run2TeamState305Maze_MazeCheck < Dictionary_Percent_Run2Maze.Count &&
                                    Timer_TeamState_301_Run_TeamState301Run2TeamState305Maze > Dictionary_Percent_Run2Maze[Level_TeamState301Run2TeamState305Maze_MazeCheck].MazeTime)
                                {
                                    float rMaze = Random.Range(0.0f, 1.0f);
                                    Debug.Log("MazeCheck "+ rMaze + ">"+ (1.0f - Dictionary_Percent_Run2Maze[Level_TeamState301Run2TeamState305Maze_MazeCheck].Percent));
                                    if (rMaze > (1.0f - Dictionary_Percent_Run2Maze[Level_TeamState301Run2TeamState305Maze_MazeCheck].Percent))
                                    {
                                        TeamState_301_Run_Over();
                                        TeamState_305_Maze_Start();
                                        Debug.Log("MazeSuccess");
                                    }
                                    Level_TeamState301Run2TeamState305Maze_MazeCheck++;
                                }
                            }
                            break;
                        //●三人小队_近战连招
                        case SubState.TeamState_302_CloseAtk:
                            {
                                //三个玛狃拉全部完成当前动作
                                if (CombatWeavile01.IsCombatComplite_TeamState302_CloseAtk &&
                                    CombatWeavile02.IsCombatComplite_TeamState302_CloseAtk &&
                                    CombatWeavile03.IsCombatComplite_TeamState302_CloseAtk
                                    )
                                {
                                    //攻击次数达到
                                    if (Count_Teamstate302_Close_Atk >= Count_Teamstate302_CloseAtk_Random)
                                    {
                                        TeamState_302_CloseAtk_Over();
                                        TeamState_300_Idle_Start();
                                    }
                                    //攻击次数未达到
                                    else
                                    {
                                        //连招动作之间休息时间
                                        Timer_Teamstate302_CloseAtk_IntervalIdle += Time.deltaTime;
                                        if (Timer_Teamstate302_CloseAtk_IntervalIdle >= TIME_TEAMSTATE302_CLOSEATK_MAX_INTERVAL)
                                        {
                                            Timer_Teamstate302_CloseAtk_IntervalIdle = 0;
                                            //近战计数器增加
                                            Count_Teamstate302_Close_Atk++;
                                            //重新设置连招分配
                                            if (Count_Teamstate302_Close_Atk == 0) { Debug.Log("Error:连招次数错误"); }
                                            else
                                            {
                                                SetWeavileCombatIndex_TeamState_302_CloseAtk_Second();
                                                CombatWeavile01.TeamCombatActOverEvent_TeamState302_CloseAtk();
                                                CombatWeavile02.TeamCombatActOverEvent_TeamState302_CloseAtk();
                                                CombatWeavile03.TeamCombatActOverEvent_TeamState302_CloseAtk();
                                            }
                                        }
                                    }
                                }
                            }
                            break;
                        //●三人小队_替身连招
                        case SubState.TeamState_303_Substitute:
                            {
                                //替身被击毁或被脱离或有玛狃拉被攻击
                                if (NowTeamState303SubState != TEAMSTATE303_SUBSTATE.Rush &&
                                    ((IsBorn_TeamState303_Substitute_Substitute == true && TeamState303_Substitute_SubstituteOBJ == null) || 
                                    (TeamState303_Substitute_SubstituteOBJ != null && TeamState303_Substitute_SubstituteOBJ.isBreak) || 
                                    (TeamState303_Substitute_SubstituteOBJ != null && !TeamState303_Substitute_SubstituteOBJ.isBornOver && TeamState303_Substitute_SubstituteOBJ.ChileMoveLocker.PlayerList.Count == 0) ||
                                    Flg_IsBeHIt_TeamState303_Substitute))
                                {
                                    //清空状态机
                                    CombatWeavile01.ResetAllState();
                                    CombatWeavile02.ResetAllState();
                                    CombatWeavile03.ResetAllState();
                                    NowTeamState303SubState = TEAMSTATE303_SUBSTATE.Rush;
                                }
                                //冲刺状态 开始冲刺下一个动作
                                if (NowTeamState303SubState == TEAMSTATE303_SUBSTATE.Rush &&
                                    CombatWeavile01.NowMainState != Weavile.MainState.Rush &&
                                    CombatWeavile02.NowMainState != Weavile.MainState.Rush &&
                                    CombatWeavile03.NowMainState != Weavile.MainState.Rush 
                                    )
                                {
                                    CombatWeavile01.TeamCombatActOverEvent_TeamState303_Substitute();
                                    CombatWeavile02.TeamCombatActOverEvent_TeamState303_Substitute();
                                    CombatWeavile03.TeamCombatActOverEvent_TeamState303_Substitute();
                                }
                                //三个玛狃拉全部完成当前动作
                                if (CombatWeavile01.IsCombatComplite_TeamState303_Substitute &&
                                    CombatWeavile02.IsCombatComplite_TeamState303_Substitute &&
                                    CombatWeavile03.IsCombatComplite_TeamState303_Substitute
                                    )
                                {
                                    //冲刺状态完成后 转入休息
                                    if (NowTeamState303SubState == TEAMSTATE303_SUBSTATE.Rush) {
                                        TeamState_303_Substitute_Over();
                                        TeamState_300_Idle_Start();
                                    }
                                    //其他状态完成后 转入下一个动作
                                    else
                                    {
                                        //连招动作之间休息时间
                                        Timer_TeamState303_Substitute_IntervalIdle += Time.deltaTime;
                                        //设定休息时间
                                        float IdleTMax = 0.0f;
                                        switch (NowTeamState303SubState)
                                        {
                                            case TEAMSTATE303_SUBSTATE.Fling: IdleTMax = TIME_TEAMSTATE303SUBSTTITUTE_FLING_IDLEINTERVAL; break;
                                            case TEAMSTATE303_SUBSTATE.Jump: IdleTMax = TIME_TEAMSTATE303SUBSTTITUTE_JUMP_IDLEINTERVAL; break;
                                            case TEAMSTATE303_SUBSTATE.Protect: IdleTMax = TIME_TEAMSTATE303SUBSTTITUTE_PROTECT_IDLEINTERVAL; break;
                                            case TEAMSTATE303_SUBSTATE.Toxic: IdleTMax = TIME_TEAMSTATE303SUBSTTITUTE_TOXIC_IDLEINTERVAL; break;
                                        }
                                        if (Timer_TeamState303_Substitute_IntervalIdle >= IdleTMax)
                                        {
                                            Timer_TeamState303_Substitute_IntervalIdle = 0.0f;

                                            //切换副状态 投掷→跳跃→保护→炸弹（毒弹）→保护→炸弹（毒弹）→
                                            switch (NowTeamState303SubState)
                                            {
                                                case TEAMSTATE303_SUBSTATE.Fling: NowTeamState303SubState = TEAMSTATE303_SUBSTATE.Jump; break;
                                                case TEAMSTATE303_SUBSTATE.Jump: NowTeamState303SubState = TEAMSTATE303_SUBSTATE.Protect; break;
                                                case TEAMSTATE303_SUBSTATE.Protect: NowTeamState303SubState = TEAMSTATE303_SUBSTATE.Toxic; break;
                                                case TEAMSTATE303_SUBSTATE.Toxic: NowTeamState303SubState = TEAMSTATE303_SUBSTATE.Protect; break;
                                            }
                                            //执行下一个动作
                                            CombatWeavile01.TeamCombatActOverEvent_TeamState303_Substitute();
                                            CombatWeavile02.TeamCombatActOverEvent_TeamState303_Substitute();
                                            CombatWeavile03.TeamCombatActOverEvent_TeamState303_Substitute();
                                        }
                                    }
                                }
                            }
                            break;
                        //●三人小队_分身连招
                        case SubState.TeamState_304_CloneShadow:
                            {
                                //三只玛狃拉都完成动作 执行下一步
                                if ( CombatWeavile01.BodyAndClone_IsCombatComplite_TeamState304_CloneShadow() &&
                                     CombatWeavile02.BodyAndClone_IsCombatComplite_TeamState304_CloneShadow() &&
                                     CombatWeavile03.BodyAndClone_IsCombatComplite_TeamState304_CloneShadow() )
                                {
                                    //终结冲刺结束后 连招结束 转入休息
                                    if (NowTeamState304CloneShadow == TEAMSTATE304_CLONESHADOW.OverRush)
                                    {
                                        TeamState_304_CloneShadow_Over();
                                        TeamState_300_Idle_Start();
                                    }
                                    //意外冲刺结束后 连招结束 转入休息
                                    else if (NowTeamState304CloneShadow == TEAMSTATE304_CLONESHADOW.AccidentRush)
                                    {
                                        TeamState_304_CloneShadow_Over();
                                        TeamState_300_Idle_Start();
                                    }
                                    //首次跳跃和初次跳跃
                                    else
                                    {
                                        float TimeMax = 0.0f;
                                        //根据状态决定休息时间
                                        switch (NowTeamState304CloneShadow)
                                        {
                                            case TEAMSTATE304_CLONESHADOW.CloneShadow:TimeMax = TIME_TEAMSTATE304CLONESHADOW_JUMP_IDLEINTERVAL;break;
                                            case TEAMSTATE304_CLONESHADOW.Jump:       TimeMax = TIME_TEAMSTATE304CLONESHADOW_JUMP_IDLEINTERVAL; break;
                                            case TEAMSTATE304_CLONESHADOW.IceShard:   TimeMax = TIME_TEAMSTATE304CLONESHADOW_ICESHARD_IDLEINTERVAL;break;
                                        }
                                        Timer_CloneShadow_IntervalIdle += Time.deltaTime;
                                        //休息结束
                                        if (Timer_CloneShadow_IntervalIdle > TimeMax)
                                        {
                                            //if (Count_TeamState304CloneShadow_Jump == 0) { Debug.Log("Error:三人小队分身连招 状态机错误 跳跃次数叠加错误"); }
                                            //首次跳跃或二次跳跃后转入冰粒
                                            if (NowTeamState304CloneShadow != TEAMSTATE304_CLONESHADOW.IceShard)
                                            {
                                                NowTeamState304CloneShadow = TEAMSTATE304_CLONESHADOW.IceShard;
                                            }
                                            //冰粒后增加跳跃次数
                                            else
                                            {
                                                //跳跃次数增加
                                                Count_TeamState304CloneShadow_Jump++;
                                                //跳跃次数达到最大值 转入冲刺
                                                if (Count_TeamState304CloneShadow_Jump > COUNT_TEAMSTATE304CLONESHADOW_JUMP_MAXCOUNT)
                                                {
                                                    NowTeamState304CloneShadow = TEAMSTATE304_CLONESHADOW.OverRush;
                                                }
                                                //未达到最大值 转入跳跃
                                                else
                                                {
                                                    NowTeamState304CloneShadow = TEAMSTATE304_CLONESHADOW.Jump;
                                                }
                                            }


                                            //休息结束后判断是否转入意外冲刺
                                            //触发意外冲刺,仅在跳跃or冰粒时触发，分身开始首跳/终结冲刺/意外冲刺时不会转入意外冲刺
                                            if (NowTeamState304CloneShadow == TEAMSTATE304_CLONESHADOW.IceShard || NowTeamState304CloneShadow == TEAMSTATE304_CLONESHADOW.Jump)
                                            {
                                                //分身本体总数小于等于4 或目标已经在冲刺路径后侧了 触发意外冲刺
                                                {
                                                    //决定跳跃方向 左侧为正右侧为负数
                                                    float dir = (isLeft_TeamState304CloneShadow_JumpStart) ? 1.0f : -1.0f;
                                                    //确定横向冲刺开始线
                                                    float RushLine = (isLeft_TeamState304CloneShadow_JumpStart) ? WeavileMembersList[0].ParentPokemonRoom.RoomSize[2] : WeavileMembersList[0].ParentPokemonRoom.RoomSize[3];
                                                    RushLine = (RushLine + dir * 0.8f) + WeavileMembersList[0].ParentPokemonRoom.transform.position.x;
                                                    Debug.Log(RushLine +"+"+ isLeft_TeamState304CloneShadow_JumpStart + "+" + TargetPosition.x);
                                                    bool isTargetBehindRushLine = isLeft_TeamState304CloneShadow_JumpStart ? (TargetPosition.x < RushLine) : (TargetPosition.x > RushLine);
                                                    if (isTargetBehindRushLine || WeavileAndCloneShadowList.Count <= 4)
                                                    {
                                                        NowTeamState304CloneShadow = TEAMSTATE304_CLONESHADOW.AccidentRush;
                                                        DetonateAllCloneBodys();//引爆其他分身
                                                    }
                                                }
                                            }


                                            //重洗本体和分身列表
                                            RandomSetBodyAndCloneShadowList();
                                            //下一步动作
                                            CombatWeavile01.TeamCombatActOverEvent_TeamState304_CloneShadow();
                                            CombatWeavile02.TeamCombatActOverEvent_TeamState304_CloneShadow();
                                            CombatWeavile03.TeamCombatActOverEvent_TeamState304_CloneShadow();
                                        }
                                    }
                                }
                            }
                            break;
                        //●三人小队_迷宫连招
                        case SubState.TeamState_305_Maze:
                            {
                                //三只玛狃拉都完成动作 执行下一步
                                if (CombatWeavile01.IsCombatComplite_TeamState305_Maze &&
                                     CombatWeavile02.IsCombatComplite_TeamState305_Maze &&
                                     CombatWeavile03.IsCombatComplite_TeamState305_Maze)
                                {
                                    TeamState_305_Maze_Over();
                                    TeamState_300_Idle_Start();
                                }
                            }
                            break;
                        //●三人小队_绕圈连招
                        case SubState.TeamState_306_CircleRun:
                            {
                                switch (NowTeamState306SubState)
                                {
                                    //奔跑时
                                    case TEAMSTATE306_SUBSTATE.Run:
                                        {
                                            SortWeavileAndCloneBodtyByAngle_TeamState306CircleRun();
                                            //获取距离
                                            float dis = Vector2.Distance(TARGET_POSITION, Position_Center_TeamState_306_CircleRun);
                                            //计时器
                                            //目标处于内环
                                            if (dis <= RADIUS_TEAMSTATE_306_CIRCLERUN_INNER)
                                            {
                                                //增加内环计时器 清空其他两个计时器
                                                Timer_TeamState306CircleRun_Inner_Atk += Time.deltaTime;
                                                Timer_TeamState306CircleRun_Outer_Atk = 0.0f;
                                                Timer_TeamState306CircleRun_Escape_Atk = 0.0f;
                                            }
                                            //目标处于外环
                                            else if (dis > RADIUS_TEAMSTATE_306_CIRCLERUN_INNER && dis <= RADIUS_TEAMSTATE_306_CIRCLERUN_OUTER)
                                            {
                                                //增加外环计时器 清空逃脱计时器
                                                Timer_TeamState306CircleRun_Outer_Atk += Time.deltaTime;
                                                Timer_TeamState306CircleRun_Escape_Atk = 0.0f;
                                            }
                                            //目标彻底离开绕圈
                                            else if (dis > RADIUS_TEAMSTATE_306_CIRCLERUN_OUTER)
                                            {
                                                //增加内环计时器 清空外环两个计时器
                                                Timer_TeamState306CircleRun_Outer_Atk = 0.0f;
                                                Timer_TeamState306CircleRun_Escape_Atk += Time.deltaTime;
                                            }

                                            //内环计时结束 触发内环攻击连招
                                            if (Timer_TeamState306CircleRun_Inner_Atk >= TIME_TEAMSTATE_306_CIRCLERUN_INNERATK_CONDITION)
                                            {
                                                //清空计时器
                                                Timer_TeamState306CircleRun_Inner_Atk = 0.0f;
                                                Timer_TeamState306CircleRun_Outer_Atk = 0.0f;
                                                Timer_TeamState306CircleRun_Escape_Atk = 0.0f;

                                                if (Random.Range(0.0f , 1.0f) > 0.5f)
                                                {
                                                    NowTeamState306SubState = TEAMSTATE306_SUBSTATE.InnerAtk1;
                                                }
                                                else
                                                {
                                                    NowTeamState306SubState = TEAMSTATE306_SUBSTATE.InnerAtk2;
                                                }
                                                Debug.Log("Inner");
                                                DetonateAllCloneBodys();
                                            }
                                            //外环计时结束 触发外环攻击连招
                                            if (Timer_TeamState306CircleRun_Outer_Atk >= TIME_TEAMSTATE_306_CIRCLERUN_OUTERATK_CONDITION)
                                            {
                                                //清空计时器
                                                Timer_TeamState306CircleRun_Inner_Atk = 0.0f;
                                                Timer_TeamState306CircleRun_Outer_Atk = 0.0f;
                                                Timer_TeamState306CircleRun_Escape_Atk = 0.0f;

                                                //根据距离排序玛狃拉
                                                SetWeavileCombatIndex_TeamState_306_CircleRun_OuterAyk_ByDistance(TargetPosition);

                                                //转入外环攻击
                                                NowTeamState306SubState = TEAMSTATE306_SUBSTATE.OuterAtk;
                                                Debug.Log("Outer");
                                                DetonateAllCloneBodys();
                                            }
                                            //逃脱计时结束 触发逃脱攻击连招
                                            if (Timer_TeamState306CircleRun_Escape_Atk >= TIME_TEAMSTATE_306_CIRCLERUN_ESCAPEATK_CONDITION)
                                            {
                                                //清空计时器
                                                Timer_TeamState306CircleRun_Inner_Atk = 0.0f;
                                                Timer_TeamState306CircleRun_Outer_Atk = 0.0f;
                                                Timer_TeamState306CircleRun_Escape_Atk = 0.0f;

                                                //转入逃离攻击
                                                NowTeamState306SubState = TEAMSTATE306_SUBSTATE.EscapeAtk;
                                                Debug.Log("Escape");
                                                DetonateAllCloneBodys();
                                            }
                                        }
                                        break;
                                    //内环攻击1
                                    case TEAMSTATE306_SUBSTATE.InnerAtk1:
                                        {
                                            //三个玛狃拉全部完成当前动作
                                            if (CombatWeavile01.IsCombatComplite_TeamState306_CircleRun_InnerAtk &&
                                                CombatWeavile02.IsCombatComplite_TeamState306_CircleRun_InnerAtk &&
                                                CombatWeavile03.IsCombatComplite_TeamState306_CircleRun_InnerAtk
                                                )
                                            {
                                                //连招动作之间休息时间
                                                Timer_TeamState306CircleRun_Atk_IntervalIdle += Time.deltaTime;
                                                if (Timer_TeamState306CircleRun_Atk_IntervalIdle >= TIME_TEAMSTATE_306_CIRCLERUN_INNERATK_INTERVAL)
                                                {
                                                    Timer_TeamState306CircleRun_Atk_IntervalIdle = 0;
                                                    //重设绕圈中心 以目标为中心
                                                    NowTeamState306SubState = TEAMSTATE306_SUBSTATE.Run;
                                                    SetCircleRunCenter_Teamstate306CirclrRun_SecondTime();
                                                    //开始下次绕圈奔跑
                                                    CombatWeavile01.TeamCombatActOverEvent_TeamState306_CircleRun_InnerAtk();
                                                    CombatWeavile02.TeamCombatActOverEvent_TeamState306_CircleRun_InnerAtk();
                                                    CombatWeavile03.TeamCombatActOverEvent_TeamState306_CircleRun_InnerAtk();
                                                }
                                            }
                                        }
                                        break;
                                    //内环攻击2
                                    case TEAMSTATE306_SUBSTATE.InnerAtk2:
                                        {
                                            //三个玛狃拉全部完成当前动作
                                            if (CombatWeavile01.IsCombatComplite_TeamState306_CircleRun_InnerAtk &&
                                                CombatWeavile02.IsCombatComplite_TeamState306_CircleRun_InnerAtk &&
                                                CombatWeavile03.IsCombatComplite_TeamState306_CircleRun_InnerAtk
                                                )
                                            {
                                                //连招动作之间休息时间
                                                Timer_TeamState306CircleRun_Atk_IntervalIdle += Time.deltaTime;
                                                if (Timer_TeamState306CircleRun_Atk_IntervalIdle >= TIME_TEAMSTATE_306_CIRCLERUN_INNERATK_INTERVAL)
                                                {
                                                    Timer_TeamState306CircleRun_Atk_IntervalIdle = 0;
                                                    //重设绕圈中心 以目标为中心
                                                    NowTeamState306SubState = TEAMSTATE306_SUBSTATE.Run;
                                                    SetCircleRunCenter_Teamstate306CirclrRun_SecondTime();
                                                    //开始下次绕圈奔跑
                                                    CombatWeavile01.TeamCombatActOverEvent_TeamState306_CircleRun_InnerAtk();
                                                    CombatWeavile02.TeamCombatActOverEvent_TeamState306_CircleRun_InnerAtk();
                                                    CombatWeavile03.TeamCombatActOverEvent_TeamState306_CircleRun_InnerAtk();
                                                }
                                            }
                                        }
                                        break;
                                    //外环攻击
                                    case TEAMSTATE306_SUBSTATE.OuterAtk:
                                        {
                                            //三个玛狃拉全部完成当前动作
                                            if (CombatWeavile01.IsCombatComplite_TeamState306_CircleRun_OuterAtk &&
                                                CombatWeavile02.IsCombatComplite_TeamState306_CircleRun_OuterAtk &&
                                                CombatWeavile03.IsCombatComplite_TeamState306_CircleRun_OuterAtk
                                                )
                                            {
                                                //达到最大次数
                                                if (Count_TeamState306CircleRun_OuterAtk >= COUNT_TEAMSTATE_306_CIRCLERUN_OUTERATK_MAX)
                                                {
                                                    TeamState_306_CircleRun_Over();
                                                    TeamState_300_Idle_Start();
                                                }
                                                else
                                                {
                                                    //连招动作之间休息时间
                                                    Timer_TeamState306CircleRun_Atk_IntervalIdle += Time.deltaTime;
                                                    if (Timer_TeamState306CircleRun_Atk_IntervalIdle >= TIME_TEAMSTATE_306_CIRCLERUN_OUTERATK_INTERVAL)
                                                    {
                                                        //计数器增加
                                                        Count_TeamState306CircleRun_OuterAtk += 1;

                                                        Timer_TeamState306CircleRun_Atk_IntervalIdle = 0;

                                                        //开始下次冲刺
                                                        CombatWeavile01.TeamCombatActOverEvent_TeamState306_CircleRun_OuterAtk();
                                                        CombatWeavile02.TeamCombatActOverEvent_TeamState306_CircleRun_OuterAtk();
                                                        CombatWeavile03.TeamCombatActOverEvent_TeamState306_CircleRun_OuterAtk();
                                                    }
                                                }
                                            }
                                        }
                                        break;
                                    //逃离攻击
                                    case TEAMSTATE306_SUBSTATE.EscapeAtk:
                                        {
                                            //三个玛狃拉全部完成当前动作 脱离攻击
                                            if (CombatWeavile01.IsCombatComplite_TeamState306_CircleRun_EscapeAtk &&
                                                CombatWeavile02.IsCombatComplite_TeamState306_CircleRun_EscapeAtk &&
                                                CombatWeavile03.IsCombatComplite_TeamState306_CircleRun_EscapeAtk
                                                )
                                            {
                                                //投掷数达到 转入休息
                                                if (Count_Fling_TeamState306CircleRun_EscapeAtk >= COUNT_FLING_TEAMSTATE_306_CIRCLERUN_ESCAPEATK-1)
                                                {
                                                    TeamState_306_CircleRun_Over();
                                                    TeamState_300_Idle_Start();
                                                }
                                                else
                                                {
                                                    Count_Fling_TeamState306CircleRun_EscapeAtk += 1;

                                                    CombatWeavile01.TeamCombatActOverEvent_TeamState306_CircleRun_EscapeAtk_Fling();
                                                    CombatWeavile02.TeamCombatActOverEvent_TeamState306_CircleRun_EscapeAtk_Fling();
                                                    CombatWeavile03.TeamCombatActOverEvent_TeamState306_CircleRun_EscapeAtk_Fling();
                                                }
                                                
                                            }
                                        }
                                        break;
                                }
                            }
                            break;
                        //●三人小队_回血连招1
                        case SubState.TeamState_307_Heal1:
                            {
                                //三只玛狃拉都完成动作 执行下一步
                                if (CombatWeavile01.IsCombatComplite_TeamState307_Heal &&
                                     CombatWeavile02.IsCombatComplite_TeamState307_Heal &&
                                     CombatWeavile03.IsCombatComplite_TeamState307_Heal)
                                {
                                    TeamState_307_Heal1_Over();
                                    Timer_HealCD_TeamState3 = TIME_HEAL1_CD;
                                    TeamState_300_Idle_Start();
                                }
                            }
                            break;
                        //●三人小队_回血连招2
                        case SubState.TeamState_308_Heal2:
                            {
                                //三只玛狃拉都完成动作 执行下一步
                                if (CombatWeavile01.IsCombatComplite_TeamState308_Heal2 &&
                                     CombatWeavile02.IsCombatComplite_TeamState308_Heal2 &&
                                     CombatWeavile03.IsCombatComplite_TeamState308_Heal2)
                                {
                                    TeamState_308_Heal2_Over();
                                    Timer_HealCD_TeamState3 = TIME_HEAL2_CD;
                                    TeamState_300_Idle_Start();
                                }
                            }
                            break;
                        //●三人小队_万灵药连招
                        case SubState.TeamState_309_FullHeal:
                            {
                                switch (NowTeamState309SubState)
                                {
                                    case TEAMSTATE309_SUBSTATE.Heal:
                                        //三只玛狃拉都处于守卫状态 且没有异常状态 执行下一步
                                        if (CombatWeavile01.NowMainState == Weavile.MainState.Guard && !CombatWeavile01.isWeavileUnusualState() &&
                                            CombatWeavile02.NowMainState == Weavile.MainState.Guard && !CombatWeavile02.isWeavileUnusualState() &&
                                            CombatWeavile03.NowMainState == Weavile.MainState.Guard && !CombatWeavile03.isWeavileUnusualState())
                                        {
                                            NowTeamState309SubState = TEAMSTATE309_SUBSTATE.Rush;
                                            CombatWeavile01.GuardOver();
                                            CombatWeavile02.GuardOver();
                                            CombatWeavile03.GuardOver();
                                            CombatWeavile01.TeamCombatActOverEvent_TeamState309_FullHeal();
                                            CombatWeavile02.TeamCombatActOverEvent_TeamState309_FullHeal();
                                            CombatWeavile03.TeamCombatActOverEvent_TeamState309_FullHeal();
                                        }
                                        break;
                                    case TEAMSTATE309_SUBSTATE.Rush:
                                        //二只玛狃拉都执行完冲刺和投掷
                                        if (CombatWeavile01.IsCombatComplite_TeamState309_FullHeal &&
                                            CombatWeavile02.IsCombatComplite_TeamState309_FullHeal &&
                                            CombatWeavile03.IsCombatComplite_TeamState309_FullHeal  )
                                        {

                                            TeamState_309_FullHeal_Over();
                                            TeamState_300_Idle_Start();
                                        }
                                        break;
                                }
                            }
                            break;
                    }
                    break;
                //■二人小队
                case MainState.Team2:
                    //Debug.Log("switch (NowMainState)");
                    //●确定连招职位分配是否正确
                    if (!(CombatWeavile01 != null && CombatWeavile01.teamCombatActIndex == 1 &&
                        CombatWeavile02 != null && CombatWeavile02.teamCombatActIndex == 2 &&
                        CombatWeavile03 == null))
                    {
                        SetWeavileCombatIndex_Random();
                        Debug.Log("Error:二人小队 连招职位分配错误");
                        break;
                    }
                    //●回复连招的冷却时间减少
                    if (Timer_HealCD_TeamState2 > 0.0f)
                    {
                        Timer_HealCD_TeamState2 -= Time.deltaTime;
                    }
                    //●有玛狃拉陷入异常状态 转入万灵药状态
                    if ((NowSubState != SubState.TeamState_208_FullHeal || (NowSubState == SubState.TeamState_208_FullHeal && NowTeamState208SubState == TEAMSTATE309_SUBSTATE.Rush) ) && 
                        (CombatWeavile01 != null && CombatWeavile01.isWeavileUnusualState() ||
                        CombatWeavile02 != null && CombatWeavile02.isWeavileUnusualState()))
                    {
                        ResetState();
                        TeamState_208_FullHeal_Start();
                    }
                    //●判断副状态机
                    switch (NowSubState)
                    {
                        //●二人小队_发呆状态
                        case SubState.TeamState_200_Idle:
                            {
                                //全部休息结束
                                if (CombatWeavile01.NowMainState == Weavile.MainState.Guard &&
                                    CombatWeavile02.NowMainState == Weavile.MainState.Guard 
                                    )
                                {
                                    TeamState_200_Idle_Over();
                                    TeamState_201_Run_Start();
                                }
                            }
                            break;
                        //●二人小队_巡逻状态
                        case SubState.TeamState_201_Run:
                            {
                                //转入回血连招检测
                                {
                                    //冷却结束 开始判断
                                    if (Timer_HealCD_TeamState2 <= 0.0f)
                                    {
                                        //检测二人小队 回复连招条件
                                        if (Condition_TeamState206_Heal())
                                        {
                                            //如果有玛狃拉不在奔跑（使用冰粒） 不检测回血
                                            {
                                                if (CombatWeavile01.NowMainState != Weavile.MainState.Run || CombatWeavile02.NowMainState != Weavile.MainState.Run)
                                                {
                                                    break;
                                                }
                                            }
                                            Timer_HealCD_TeamState2 = 0.0f;
                                            TeamState_201_Run_Over();
                                            SetWeavileCombatIndex_TeamState_206_Heal();
                                            TeamState_206_Heal_Start();
                                            Debug.Log("Heal");
                                            break;
                                        }
                                    }
                                }
                                //巡逻逻辑
                                {
                                    //判断包围是否形成
                                    //包围成功
                                    if (TeamState201_Run_CheckSurroundSccess(TargetPosition))
                                    {
                                        //包围还没成功过,设置成功过旗
                                        if (!isSurroundSccessOnceFlg_TeamState201Run) { isSurroundSccessOnceFlg_TeamState201Run = true; }
                                        Timer_TeamState_201_Run_Surround_Success += Time.deltaTime;
                                        Timer_TeamState_201_Run_Surround_Fail = 0.0f;
                                        Debug.Log("Surround_Success" + "+" + Timer_TeamState_201_Run_Surround_Success);
                                    }
                                    //包围失败
                                    else
                                    {
                                        //包围成功过之后，才开始计算包围失败
                                        if (isSurroundSccessOnceFlg_TeamState201Run)
                                        {
                                            Timer_TeamState_201_Run_Surround_Success = 0.0f;
                                            Timer_TeamState_201_Run_Surround_Fail += Time.deltaTime;
                                            Debug.Log("Surround_Fail" + "+" + Timer_TeamState_201_Run_Surround_Fail);
                                        }
                                    }
                                    //随机点数
                                    float r = Random.Range(0.0f, 1.0f);
                                    //触发联招条件 巡逻结束 转入其他连招
                                    //包围时间足够长，转入绕圈跑
                                    if (Timer_TeamState_201_Run_Surround_Success >= TIME_TEAM201_RUN_2_205_CIRCLERUN_SURROUND_SUCCESS)
                                    {
                                        //如果有玛狃拉不在奔跑（使用冰粒） 不检测其他连招
                                        {
                                            if (CombatWeavile01.NowMainState != Weavile.MainState.Run || CombatWeavile02.NowMainState != Weavile.MainState.Run)
                                            {
                                                break;
                                            }
                                        }
                                        TeamState_201_Run_Over();
                                        //93%概率转入绕圈跑
                                        if (r >= 0.0f && r < 1.0f - PERCENT_ANOTHERCOMBAT2MAZE)
                                        {
                                            TeamState_205_CircleRun_Start();
                                            Debug.Log("CircleRun");
                                        }
                                        //7%概率转入迷宫
                                        else
                                        {
                                            TeamState_207_Maze_Start();
                                            Debug.Log("Maze_CircleRun");
                                        }
                                    }
                                    //包围失败时间足够长，转入替身连招或分身连招
                                    else if (Timer_TeamState_201_Run_Surround_Fail >= TIME_TEAM201_RUN_2_203_Substitute_204_Substitute2_SURROUND_FAIL)
                                    {
                                        //如果有玛狃拉不在奔跑（使用冰粒） 不检测其他连招
                                        {
                                            if (CombatWeavile01.NowMainState != Weavile.MainState.Run || CombatWeavile02.NowMainState != Weavile.MainState.Run)
                                            {
                                                break;
                                            }
                                        }
                                        TeamState_201_Run_Over();
                                        //46.5%概率转入替身连招
                                        if (r >= 0.0f && r < ((1.0f - PERCENT_ANOTHERCOMBAT2MAZE) / 2.0f))
                                        {
                                            TeamState_203_Substitute_Start();
                                            Debug.Log("Substitute");
                                        }
                                        //46.5%概率转入分身连招
                                        else if (r >= ((1.0f - PERCENT_ANOTHERCOMBAT2MAZE) / 2.0f) && r < 1.0f - PERCENT_ANOTHERCOMBAT2MAZE)
                                        {
                                            TeamState_204_Substitute2_Start();
                                            Debug.Log("CloneShadow");
                                        }
                                        //7%概率转入迷宫
                                        else
                                        {
                                            TeamState_207_Maze_Start();
                                            Debug.Log("Maze_CircleRun");
                                        }
                                    }
                                    //有玛狃拉被接近
                                    Weavile closedW = TeamState201_Run_CheckAllWeaileBeClosed();
                                    if (closedW != null)
                                    {
                                        //如果有玛狃拉不在奔跑（使用冰粒） 不检测其他连招
                                        {
                                            if (CombatWeavile01.NowMainState != Weavile.MainState.Run || CombatWeavile02.NowMainState != Weavile.MainState.Run)
                                            {
                                                break;
                                            }
                                        }
                                        TeamState_201_Run_Over();
                                        //93%概率转入近战连招
                                        if (r >= 0.0f && r < 1.0f - PERCENT_ANOTHERCOMBAT2MAZE)
                                        {
                                            SetWeavileCombatIndex_TeamState_202_CloseAtk_FirstTime(TargetPosition, closedW);
                                            TeamState_202_CloseAtk_Start();
                                            Debug.Log("CloseAtk");
                                        }
                                        //7%概率转入迷宫
                                        else
                                        {
                                            TeamState_207_Maze_Start();
                                            Debug.Log("Maze_CircleRun");
                                        }
                                    }
                                    //随着时间增加 判定转入迷宫连招
                                    Timer_TeamState_201_Run_TeamState201Run2TeamState207Maze += Time.deltaTime;
                                    //大于当前等级的所定时间 按照当前概率转入迷宫
                                    if (Level_TeamState201Run2TeamState207Maze_MazeCheck < Dictionary_Percent_Run2Maze.Count &&
                                        Timer_TeamState_201_Run_TeamState201Run2TeamState207Maze > Dictionary_Percent_Run2Maze[Level_TeamState201Run2TeamState207Maze_MazeCheck].MazeTime)
                                    {
                                        float rMaze = Random.Range(0.0f, 1.0f);
                                        Debug.Log("MazeCheck " + rMaze + ">" + (1.0f - Dictionary_Percent_Run2Maze[Level_TeamState201Run2TeamState207Maze_MazeCheck].Percent));
                                        if (rMaze > (1.0f - Dictionary_Percent_Run2Maze[Level_TeamState201Run2TeamState207Maze_MazeCheck].Percent))
                                        {
                                            //如果有玛狃拉不在奔跑（使用冰粒） 不检测其他连招
                                            {
                                                if (CombatWeavile01.NowMainState != Weavile.MainState.Run || CombatWeavile02.NowMainState != Weavile.MainState.Run)
                                                {
                                                    break;
                                                }
                                            }
                                            TeamState_201_Run_Over();
                                            TeamState_207_Maze_Start();
                                            Debug.Log("MazeSuccess");
                                        }
                                        Level_TeamState201Run2TeamState207Maze_MazeCheck++;
                                    }
                                }
                            }
                            break;
                        //●二人小队_近战连招
                        case SubState.TeamState_202_CloseAtk:
                            {
                                //两个玛狃拉全部完成当前动作
                                if (CombatWeavile01.IsCombatComplite_TeamState202_CloseAtk &&
                                    CombatWeavile02.IsCombatComplite_TeamState202_CloseAtk
                                    )
                                {
                                    //攻击次数达到 且扔完冰粒
                                    if (Count_Teamstate202_Close_Atk >= Count_Teamstate202_CloseAtk_Random && NowTeamState202CloseAtk == TEAMSTATE202_CLOSEATK.Jump)
                                    {
                                        TeamState_202_CloseAtk_Over();
                                        TeamState_200_Idle_Start();
                                    }
                                    //攻击次数未达到
                                    else
                                    {
                                        //连招动作之间休息时间
                                        Timer_Teamstate202_CloseAtk_IntervalIdle += Time.deltaTime;
                                        float tMax = 0.0f;
                                        switch (NowTeamState202CloseAtk)
                                        {
                                            case TEAMSTATE202_CLOSEATK.Rush: tMax = TIME_TEAMSTATE202_CLOSEATK_MAX_INTERVAL_RUSH; break;
                                            case TEAMSTATE202_CLOSEATK.Jump: tMax = TIME_TEAMSTATE202_CLOSEATK_MAX_INTERVAL_JUMP; break;
                                        }
                                        if (Timer_Teamstate202_CloseAtk_IntervalIdle >= tMax)
                                        {
                                            Timer_Teamstate202_CloseAtk_IntervalIdle = 0;
                                            //转入下次攻击
                                            switch (NowTeamState202CloseAtk)
                                            {
                                                //本次攻击为冲刺 转入跳跃
                                                case TEAMSTATE202_CLOSEATK.Rush:
                                                    NowTeamState202CloseAtk = TEAMSTATE202_CLOSEATK.Jump; 
                                                    break;
                                                //本次攻击为跳跃 转入冲刺 并且增加攻击次数
                                                case TEAMSTATE202_CLOSEATK.Jump:
                                                    NowTeamState202CloseAtk = TEAMSTATE202_CLOSEATK.Rush;
                                                    //近战计数器增加
                                                    Count_Teamstate202_Close_Atk++;
                                                    //重新设置连招分配
                                                    SetWeavileCombatIndex_TeamState_202_CloseAtk_Second();
                                                    break;
                                            }
                                            //下一步
                                            CombatWeavile01.TeamCombatActOverEvent_TeamState202_CloseAtk();
                                            CombatWeavile02.TeamCombatActOverEvent_TeamState202_CloseAtk();
                                        }
                                    }
                                }
                            }
                            break;
                        //●二人小队_替身连招
                        case SubState.TeamState_203_Substitute:
                            {
                                //替身被击毁或被脱离或有玛狃拉被攻击
                                if (NowTeamState203SubState != TEAMSTATE203_SUBSTATE.Rush &&
                                    ((IsBorn_TeamState203_Substitute_Substitute == true && TeamState203_Substitute_SubstituteOBJ == null) ||
                                    (TeamState203_Substitute_SubstituteOBJ != null && TeamState203_Substitute_SubstituteOBJ.isBreak) ||
                                    (TeamState203_Substitute_SubstituteOBJ != null && !TeamState203_Substitute_SubstituteOBJ.isBornOver && TeamState203_Substitute_SubstituteOBJ.ChileMoveLocker.PlayerList.Count == 0) ||
                                    Flg_IsBeHIt_TeamState203_Substitute))
                                {
                                    //清空状态机
                                    CombatWeavile01.ResetAllState();
                                    CombatWeavile02.ResetAllState();
                                    NowTeamState203SubState = TEAMSTATE203_SUBSTATE.Rush;
                                }
                                //冲刺状态 开始冲刺下一个动作
                                if (NowTeamState203SubState == TEAMSTATE203_SUBSTATE.Rush &&
                                    CombatWeavile01.NowMainState != Weavile.MainState.Rush &&
                                    CombatWeavile02.NowMainState != Weavile.MainState.Rush 
                                    )
                                {
                                    CombatWeavile01.TeamCombatActOverEvent_TeamState203_Substitute();
                                    CombatWeavile02.TeamCombatActOverEvent_TeamState203_Substitute();
                                }
                                //两个玛狃拉全部完成当前动作
                                if (CombatWeavile01.IsCombatComplite_TeamState203_Substitute &&
                                    CombatWeavile02.IsCombatComplite_TeamState203_Substitute 
                                    )
                                {
                                    //冲刺状态完成后 转入休息
                                    if (NowTeamState203SubState == TEAMSTATE203_SUBSTATE.Rush)
                                    {
                                        TeamState_203_Substitute_Over();
                                        TeamState_200_Idle_Start();
                                    }
                                    //其他状态完成后 转入下一个动作
                                    else
                                    {
                                        //连招动作之间休息时间
                                        Timer_TeamState203_Substitute_IntervalIdle += Time.deltaTime;
                                        //设定休息时间
                                        float IdleTMax = 0.0f;
                                        switch (NowTeamState203SubState)
                                        {
                                            case TEAMSTATE203_SUBSTATE.Fling: IdleTMax = TIME_TEAMSTATE203SUBSTTITUTE_FLING_IDLEINTERVAL; break;
                                            case TEAMSTATE203_SUBSTATE.Jump: IdleTMax = TIME_TEAMSTATE203SUBSTTITUTE_JUMP_IDLEINTERVAL; break;
                                            case TEAMSTATE203_SUBSTATE.Protect: IdleTMax = TIME_TEAMSTATE203SUBSTTITUTE_PROTECT_IDLEINTERVAL; break;
                                            case TEAMSTATE203_SUBSTATE.Toxic: IdleTMax = TIME_TEAMSTATE203SUBSTTITUTE_TOXIC_IDLEINTERVAL; break;
                                        }
                                        if (Timer_TeamState203_Substitute_IntervalIdle >= IdleTMax)
                                        {
                                            Timer_TeamState203_Substitute_IntervalIdle = 0.0f;

                                            //切换副状态 投掷→跳跃→保护→炸弹（毒弹）→保护→炸弹（毒弹）→
                                            switch (NowTeamState203SubState)
                                            {
                                                case TEAMSTATE203_SUBSTATE.Fling: NowTeamState203SubState = TEAMSTATE203_SUBSTATE.Jump; break;
                                                case TEAMSTATE203_SUBSTATE.Jump: NowTeamState203SubState = TEAMSTATE203_SUBSTATE.Protect; break;
                                                case TEAMSTATE203_SUBSTATE.Protect: NowTeamState203SubState = TEAMSTATE203_SUBSTATE.Toxic; break;
                                                case TEAMSTATE203_SUBSTATE.Toxic: NowTeamState203SubState = TEAMSTATE203_SUBSTATE.Protect; break;
                                            }
                                            //执行下一个动作
                                            CombatWeavile01.TeamCombatActOverEvent_TeamState203_Substitute();
                                            CombatWeavile02.TeamCombatActOverEvent_TeamState203_Substitute();
                                        }
                                    }
                                }
                            }
                            break;
                        //●二人小队_替身连招2
                        case SubState.TeamState_204_Substitute2:
                            {
                                //替身被击毁或被脱离 或有 玛狃拉被攻击 或分身总数小于3
                                if (NowTeamState204SubState != TEAMSTATE204_SUBSTATE.Rush &&
                                    ((IsBorn_TeamState204_Substitute2_Substitute == true && TeamState204_Substitute2_SubstituteOBJ == null) ||
                                    (TeamState204_Substitute2_SubstituteOBJ != null && TeamState204_Substitute2_SubstituteOBJ.isBreak) ||
                                    (TeamState204_Substitute2_SubstituteOBJ != null && !TeamState204_Substitute2_SubstituteOBJ.isBornOver && TeamState204_Substitute2_SubstituteOBJ.ChileMoveLocker.PlayerList.Count == 0) ||
                                    Flg_IsBeHIt_TeamState204_Substitute2 || 
                                    (WeavileAndCloneShadowList.Count <= 3 && NowTeamState204SubState != TEAMSTATE204_SUBSTATE.Fling && NowTeamState204SubState != TEAMSTATE204_SUBSTATE.CloneShadow)))
                                {
                                    //清空状态机
                                    CombatWeavile01.ResetAllState();
                                    CombatWeavile02.ResetAllState();
                                    NowTeamState204SubState = TEAMSTATE204_SUBSTATE.Rush;
                                }
                                //冲刺状态 开始冲刺下一个动作
                                if (NowTeamState204SubState == TEAMSTATE204_SUBSTATE.Rush &&
                                    CombatWeavile01.NowMainState != Weavile.MainState.Rush &&
                                    CombatWeavile02.NowMainState != Weavile.MainState.Rush
                                    )
                                {
                                    CombatWeavile01.TeamCombatActOverEvent_TeamState204_Substitute2();
                                    CombatWeavile02.TeamCombatActOverEvent_TeamState204_Substitute2();
                                }
                                //两个玛狃拉全部完成当前动作
                                if (CombatWeavile01.BodyAndClone_IsCombatComplite_TeamState204_Substitute2() &&
                                    CombatWeavile02.BodyAndClone_IsCombatComplite_TeamState204_Substitute2()
                                    )
                                {
                                    //冲刺状态完成后 转入休息
                                    if (NowTeamState204SubState == TEAMSTATE204_SUBSTATE.Rush)
                                    {
                                        TeamState_204_Substitute2_Over();
                                        TeamState_200_Idle_Start();
                                    }
                                    //其他状态完成后 转入下一个动作
                                    else
                                    {
                                        //连招动作之间休息时间
                                        Timer_TeamState204_Substitute2_IntervalIdle += Time.deltaTime;
                                        //设定休息时间
                                        float IdleTMax = 0.0f;
                                        switch (NowTeamState204SubState)
                                        {
                                            case TEAMSTATE204_SUBSTATE.Fling: IdleTMax = TIME_TEAMSTATE204SUBSTTITUTE2_FLING_IDLEINTERVAL; break;
                                            case TEAMSTATE204_SUBSTATE.CloneShadow: IdleTMax = TIME_TEAMSTATE204SUBSTTITUTE2_CLONESHADOW_IDLEINTERVAL; break;
                                            case TEAMSTATE204_SUBSTATE.Jump: IdleTMax = TIME_TEAMSTATE204SUBSTTITUTE2_JUMP_IDLEINTERVAL; break;
                                            case TEAMSTATE204_SUBSTATE.Bomb: IdleTMax = TIME_TEAMSTATE204SUBSTTITUTE2_BOMB_IDLEINTERVAL; break;
                                        }
                                        if (Timer_TeamState204_Substitute2_IntervalIdle >= IdleTMax)
                                        {
                                            Timer_TeamState204_Substitute2_IntervalIdle = 0.0f;

                                            //切换副状态 投掷→分身→跳跃→炸弹→跳跃→炸弹→
                                            switch (NowTeamState204SubState)
                                            {
                                                case TEAMSTATE204_SUBSTATE.Fling:       NowTeamState204SubState = TEAMSTATE204_SUBSTATE.CloneShadow; break;
                                                case TEAMSTATE204_SUBSTATE.CloneShadow: NowTeamState204SubState = TEAMSTATE204_SUBSTATE.Jump; break;
                                                case TEAMSTATE204_SUBSTATE.Jump:        NowTeamState204SubState = TEAMSTATE204_SUBSTATE.Bomb; break;
                                                case TEAMSTATE204_SUBSTATE.Bomb:        NowTeamState204SubState = TEAMSTATE204_SUBSTATE.Jump; RandomSetBodyAndCloneShadowList(); ; break;
                                            }
                                            //执行下一个动作
                                            CombatWeavile01.TeamCombatActOverEvent_TeamState204_Substitute2();
                                            CombatWeavile02.TeamCombatActOverEvent_TeamState204_Substitute2();
                                        }
                                    }
                                }
                            }
                            break;
                        //●二人小队_绕圈连招
                        case SubState.TeamState_205_CircleRun:
                            {
                                switch (NowTeamState205SubState)
                                {
                                    //奔跑时
                                    case TEAMSTATE306_SUBSTATE.Run:
                                        {
                                            SortWeavileAndCloneBodtyByAngle_TeamState205CircleRun();
                                            //获取距离
                                            float dis = Vector2.Distance(TARGET_POSITION, Position_Center_TeamState_205_CircleRun);
                                            //计时器
                                            //目标处于内环
                                            if (dis <= RADIUS_TEAMSTATE_306_CIRCLERUN_INNER)
                                            {
                                                //增加内环计时器 清空其他两个计时器
                                                Timer_TeamState205CircleRun_Inner_Atk += Time.deltaTime;
                                                Timer_TeamState205CircleRun_Outer_Atk = 0.0f;
                                                Timer_TeamState205CircleRun_Escape_Atk = 0.0f;
                                            }
                                            //目标处于外环
                                            else if (dis > RADIUS_TEAMSTATE_306_CIRCLERUN_INNER && dis <= RADIUS_TEAMSTATE_306_CIRCLERUN_OUTER)
                                            {
                                                //增加外环计时器 清空逃脱计时器
                                                Timer_TeamState205CircleRun_Outer_Atk += Time.deltaTime;
                                                Timer_TeamState205CircleRun_Escape_Atk = 0.0f;
                                            }
                                            //目标彻底离开绕圈
                                            else if (dis > RADIUS_TEAMSTATE_306_CIRCLERUN_OUTER)
                                            {
                                                //增加内环计时器 清空外环两个计时器
                                                Timer_TeamState205CircleRun_Outer_Atk = 0.0f;
                                                Timer_TeamState205CircleRun_Escape_Atk += Time.deltaTime;
                                            }

                                            //内环计时结束 触发内环攻击连招
                                            if (Timer_TeamState205CircleRun_Inner_Atk >= TIME_TEAMSTATE_306_CIRCLERUN_INNERATK_CONDITION)
                                            {
                                                //清空计时器
                                                Timer_TeamState205CircleRun_Inner_Atk = 0.0f;
                                                Timer_TeamState205CircleRun_Outer_Atk = 0.0f;
                                                Timer_TeamState205CircleRun_Escape_Atk = 0.0f;

                                                if (Random.Range(0.0f, 1.0f) > 0.5f)
                                                {
                                                    NowTeamState205SubState = TEAMSTATE306_SUBSTATE.InnerAtk1;
                                                }
                                                else
                                                {
                                                    NowTeamState205SubState = TEAMSTATE306_SUBSTATE.InnerAtk2;
                                                }
                                                Debug.Log("Inner");
                                                DetonateAllCloneBodys();
                                            }
                                            //外环计时结束 触发外环攻击连招
                                            if (Timer_TeamState205CircleRun_Outer_Atk >= TIME_TEAMSTATE_306_CIRCLERUN_OUTERATK_CONDITION)
                                            {
                                                //清空计时器
                                                Timer_TeamState205CircleRun_Inner_Atk = 0.0f;
                                                Timer_TeamState205CircleRun_Outer_Atk = 0.0f;
                                                Timer_TeamState205CircleRun_Escape_Atk = 0.0f;

                                                //根据距离排序玛狃拉
                                                SetWeavileCombatIndex_TeamState_205_CircleRun_OuterAyk_ByDistance(TargetPosition);

                                                //转入外环攻击
                                                NowTeamState205SubState = TEAMSTATE306_SUBSTATE.OuterAtk;
                                                Debug.Log("Outer");
                                                DetonateAllCloneBodys();
                                            }
                                            //逃脱计时结束 触发逃脱攻击连招
                                            if (Timer_TeamState205CircleRun_Escape_Atk >= TIME_TEAMSTATE_306_CIRCLERUN_ESCAPEATK_CONDITION)
                                            {
                                                //清空计时器
                                                Timer_TeamState205CircleRun_Inner_Atk = 0.0f;
                                                Timer_TeamState205CircleRun_Outer_Atk = 0.0f;
                                                Timer_TeamState205CircleRun_Escape_Atk = 0.0f;

                                                //转入逃离攻击
                                                NowTeamState205SubState = TEAMSTATE306_SUBSTATE.EscapeAtk;
                                                Debug.Log("Escape");
                                                DetonateAllCloneBodys();
                                            }
                                        }
                                        break;
                                    //内环攻击1
                                    case TEAMSTATE306_SUBSTATE.InnerAtk1:
                                        {
                                            //三个玛狃拉全部完成当前动作
                                            if (CombatWeavile01.IsCombatComplite_TeamState205_CircleRun_InnerAtk &&
                                                CombatWeavile02.IsCombatComplite_TeamState205_CircleRun_InnerAtk 
                                                )
                                            {
                                                //连招动作之间休息时间
                                                Timer_TeamState205CircleRun_Atk_IntervalIdle += Time.deltaTime;
                                                if (Timer_TeamState205CircleRun_Atk_IntervalIdle >= TIME_TEAMSTATE_306_CIRCLERUN_INNERATK_INTERVAL)
                                                {
                                                    Timer_TeamState205CircleRun_Atk_IntervalIdle = 0;
                                                    //重设绕圈中心 以目标为中心
                                                    NowTeamState205SubState = TEAMSTATE306_SUBSTATE.Run;
                                                    SetCircleRunCenter_Teamstate205CirclrRun_SecondTime();
                                                    //开始下次绕圈奔跑
                                                    CombatWeavile01.TeamCombatActOverEvent_TeamState205_CircleRun_InnerAtk();
                                                    CombatWeavile02.TeamCombatActOverEvent_TeamState205_CircleRun_InnerAtk();
                                                }
                                            }
                                        }
                                        break;
                                    //内环攻击2
                                    case TEAMSTATE306_SUBSTATE.InnerAtk2:
                                        {
                                            //三个玛狃拉全部完成当前动作
                                            if (CombatWeavile01.IsCombatComplite_TeamState205_CircleRun_InnerAtk &&
                                                CombatWeavile02.IsCombatComplite_TeamState205_CircleRun_InnerAtk 
                                                )
                                            {
                                                //连招动作之间休息时间
                                                Timer_TeamState205CircleRun_Atk_IntervalIdle += Time.deltaTime;
                                                if (Timer_TeamState205CircleRun_Atk_IntervalIdle >= TIME_TEAMSTATE_306_CIRCLERUN_INNERATK_INTERVAL)
                                                {
                                                    Timer_TeamState205CircleRun_Atk_IntervalIdle = 0;
                                                    //重设绕圈中心 以目标为中心
                                                    NowTeamState205SubState = TEAMSTATE306_SUBSTATE.Run;
                                                    SetCircleRunCenter_Teamstate205CirclrRun_SecondTime();
                                                    //开始下次绕圈奔跑
                                                    CombatWeavile01.TeamCombatActOverEvent_TeamState205_CircleRun_InnerAtk();
                                                    CombatWeavile02.TeamCombatActOverEvent_TeamState205_CircleRun_InnerAtk();
                                                }
                                            }
                                        }
                                        break;
                                    //外环攻击
                                    case TEAMSTATE306_SUBSTATE.OuterAtk:
                                        {
                                            //三个玛狃拉全部完成当前动作
                                            if (CombatWeavile01.IsCombatComplite_TeamState205_CircleRun_OuterAtk &&
                                                CombatWeavile02.IsCombatComplite_TeamState205_CircleRun_OuterAtk 
                                                )
                                            {
                                                //达到最大次数
                                                if (Count_TeamState205CircleRun_OuterAtk >= COUNT_TEAMSTATE_205_CIRCLERUN_OUTERATK_MAX)
                                                {
                                                    TeamState_205_CircleRun_Over();
                                                    TeamState_200_Idle_Start();
                                                }
                                                else
                                                {
                                                    //连招动作之间休息时间
                                                    Timer_TeamState205CircleRun_Atk_IntervalIdle += Time.deltaTime;
                                                    if (Timer_TeamState205CircleRun_Atk_IntervalIdle >= TIME_TEAMSTATE_306_CIRCLERUN_OUTERATK_INTERVAL)
                                                    {
                                                        //计数器增加
                                                        Count_TeamState205CircleRun_OuterAtk += 1;

                                                        Timer_TeamState205CircleRun_Atk_IntervalIdle = 0;

                                                        //开始下次冲刺
                                                        CombatWeavile01.TeamCombatActOverEvent_TeamState205_CircleRun_OuterAtk();
                                                        CombatWeavile02.TeamCombatActOverEvent_TeamState205_CircleRun_OuterAtk();
                                                    }
                                                }
                                            }
                                        }
                                        break;
                                    //逃离攻击
                                    case TEAMSTATE306_SUBSTATE.EscapeAtk:
                                        {
                                            //三个玛狃拉全部完成当前动作 脱离攻击
                                            if (CombatWeavile01.IsCombatComplite_TeamState205_CircleRun_EscapeAtk &&
                                                CombatWeavile02.IsCombatComplite_TeamState205_CircleRun_EscapeAtk 
                                                )
                                            {
                                                //投掷数达到 转入休息
                                                if (Count_Fling_TeamState205CircleRun_EscapeAtk >= COUNT_FLING_TEAMSTATE_306_CIRCLERUN_ESCAPEATK - 1)
                                                {
                                                    TeamState_205_CircleRun_Over();
                                                    TeamState_200_Idle_Start();
                                                }
                                                else
                                                {
                                                    Count_Fling_TeamState205CircleRun_EscapeAtk += 1;

                                                    CombatWeavile01.TeamCombatActOverEvent_TeamState205_CircleRun_EscapeAtk_Fling();
                                                    CombatWeavile02.TeamCombatActOverEvent_TeamState205_CircleRun_EscapeAtk_Fling();
                                                }

                                            }
                                        }
                                        break;
                                }
                            }
                            break;
                        //●二人小队_回血连招
                        case SubState.TeamState_206_Heal:
                            {
                                //三只玛狃拉都完成动作 执行下一步
                                if (CombatWeavile01.IsCombatComplite_TeamState206_Heal &&
                                     CombatWeavile02.IsCombatComplite_TeamState206_Heal)
                                {
                                    TeamState_206_Heal_Over();
                                    Timer_HealCD_TeamState2 = TIME_TEAMSTATE206_HEAL_CD;
                                    TeamState_200_Idle_Start();
                                }
                            }
                            break;
                        //●二人小队_迷宫连招
                        case SubState.TeamState_207_Maze:
                            {
                                //三只玛狃拉都完成动作 执行下一步
                                if ( CombatWeavile01.IsCombatComplite_TeamState207_Maze &&
                                     CombatWeavile02.IsCombatComplite_TeamState207_Maze )
                                {
                                    TeamState_207_Maze_Over();
                                    TeamState_200_Idle_Start();
                                }
                            }
                            break;
                        //●二人小队_万灵药连招
                        case SubState.TeamState_208_FullHeal:
                            {
                                switch(NowTeamState208SubState){
                                    case TEAMSTATE309_SUBSTATE.Heal:
                                        //二只玛狃拉都处于守卫状态 且没有异常状态 执行下一步
                                        if (CombatWeavile01.NowMainState == Weavile.MainState.Guard && !CombatWeavile01.isWeavileUnusualState() &&
                                             CombatWeavile02.NowMainState == Weavile.MainState.Guard && !CombatWeavile02.isWeavileUnusualState())
                                        {
                                            NowTeamState208SubState = TEAMSTATE309_SUBSTATE.Rush;
                                            CombatWeavile01.GuardOver();
                                            CombatWeavile02.GuardOver();
                                            CombatWeavile01.TeamCombatActOverEvent_TeamState208_FullHeal();
                                            CombatWeavile02.TeamCombatActOverEvent_TeamState208_FullHeal();
                                        }
                                        break;
                                    case TEAMSTATE309_SUBSTATE.Rush:
                                        //二只玛狃拉都执行完冲刺和投掷
                                        if (CombatWeavile01.IsCombatComplite_TeamState208_FullHeal &&
                                            CombatWeavile01.IsCombatComplite_TeamState208_FullHeal)
                                        {

                                            TeamState_208_FullHeal_Over();
                                            TeamState_200_Idle_Start();
                                        }
                                        break;
                                }
                            }
                            break;
                    }
                    break;
                //■一人小队
                case MainState.Team1:
                    //●确定连招职位分配是否正确
                    if (!(CombatWeavile01 != null && CombatWeavile01.teamCombatActIndex == 1 &&
                        CombatWeavile02 == null &&
                        CombatWeavile03 == null))
                    {
                        SetWeavileCombatIndex_Random();
                        Debug.Log("Error:一人小队 连招职位分配错误");
                        break;
                    }
                    //●判断副状态机
                    switch (NowSubState)
                    {
                        //●一人小队_巡逻状态
                        case SubState.TeamState_100_Normal:
                            {
                            }
                            break;
                    }
                    break;
                case MainState.Team0:
                    break;
            }
        }
        //不对应
        else
        {

        }
    }



    private void FixedUpdate()
    {
        //确实目标位置
        if (player != null) { TargetPosition = player.transform.position; }
        else { ResetPlayer(); }
        
    }



    /// <summary>
    /// 重置状态机
    /// </summary>
    void ResetState()
    {
        //排空玛狃拉列表
        _mTool.RemoveNullInList<Weavile>(WeavileMembersList);
        //重置主脑状态机
        switch (NowSubState)
        {
            case SubState.TeamState_300_Idle:    //三人小队_发呆状态	
                TeamState_300_Idle_Over(); break;
            case SubState.TeamState_301_Run:    //三人小队_巡逻状态	
                TeamState_301_Run_Over(); break;
            case SubState.TeamState_302_CloseAtk:    //三人小队_近战连招
                TeamState_302_CloseAtk_Over();  break;	
            case SubState.TeamState_303_Substitute:    //三人小队_替身连招	
                TeamState_303_Substitute_Over(); break;
            case SubState.TeamState_304_CloneShadow:    //三人小队_分身连招	
                TeamState_304_CloneShadow_Over(); break;
            case SubState.TeamState_305_Maze:    //三人小队_迷宫连招	
                TeamState_305_Maze_Over(); break;
            case SubState.TeamState_306_CircleRun:    //三人小队_绕圈连招	
                TeamState_306_CircleRun_Over(); break;
            case SubState.TeamState_307_Heal1:    //三人小队_回血连招1	
                TeamState_307_Heal1_Over(); break;
            case SubState.TeamState_308_Heal2:    //三人小队_回血连招2	
                TeamState_308_Heal2_Over(); break;
            case SubState.TeamState_309_FullHeal:    //三人小队_万灵药连招	
                TeamState_309_FullHeal_Over(); break;
            case SubState.TeamState_200_Idle:    //二人小队_发呆状态	
                TeamState_200_Idle_Over(); break;
            case SubState.TeamState_201_Run:    //二人小队_巡逻状态	
                TeamState_201_Run_Over(); break;
            case SubState.TeamState_202_CloseAtk:    //二人小队_近战连招
                TeamState_202_CloseAtk_Over(); break;
            case SubState.TeamState_203_Substitute:    //二人小队_替身连招	
                TeamState_203_Substitute_Over(); break;
            case SubState.TeamState_204_Substitute2:    //二人小队_替身连招2	
                TeamState_204_Substitute2_Over(); break;
            case SubState.TeamState_205_CircleRun:    //二人小队_绕圈连招
                TeamState_205_CircleRun_Over(); break;	
            case SubState.TeamState_206_Heal:    //二人小队_回血连招	
                TeamState_206_Heal_Over(); break;
            case SubState.TeamState_207_Maze:    //二人小队_迷宫连招	
                TeamState_207_Maze_Over(); break;
            case SubState.TeamState_208_FullHeal:    //二人小队_万灵药连招	
                TeamState_208_FullHeal_Over(); break;
            case SubState.TeamState_100_Normal:    //一人小队_巡逻状态	
                TeamState_100_Normal_Over(); break;
        }
        //重置状态机通用参数
        Timer_HealCD_TeamState3 = 0.0f;
        Timer_HealCD_TeamState2 = 0.0f;

        //消除实例
        if (TeamState303_Substitute_SubstituteOBJ != null) { TeamState303_Substitute_SubstituteOBJ.BeHit(TeamState303_Substitute_SubstituteOBJ.MaxHP); };
        if (TeamState203_Substitute_SubstituteOBJ != null) { TeamState203_Substitute_SubstituteOBJ.BeHit(TeamState203_Substitute_SubstituteOBJ.MaxHP); };
        if (TeamState204_Substitute2_SubstituteOBJ != null) { TeamState204_Substitute2_SubstituteOBJ.BeHit(TeamState204_Substitute2_SubstituteOBJ.MaxHP); };
        WeavileSmokeList.Clear();

        //引爆所有分身
        DetonateAllCloneBodys();

        //重置所有玛狃拉的状态机
        for (int i = 0; i < WeavileMembersList.Count; i++)
        {
            WeavileMembersList[i].ResetAllState();
        }

        //完全重置当前玛狃拉连招分配
        CombatWeavile01 = null;
        CombatWeavile02 = null;
        CombatWeavile03 = null;
        for (int i = 0; i < WeavileMembersList.Count; i++)
        {
            WeavileMembersList[i].TeamCombatActIndex = 0 ;
        }
        //随机重置玛狃拉序列号
        SetWeavileCombatIndex_Random();
    }











    //■■■■■■■■■■■■■■■■■■■■玛狃拉小队■■■■■■■■■■■■■■■■■■■■■■










































    //■■■■■■■■■■■■■■■■■■■■共通■■■■■■■■■■■■■■■■■■■■■■





    /// <summary>
        /// 切换副状态
        /// </summary>
    void ChangeSubState(SubState targetSubstate)
    {
        NowSubState = targetSubstate;
        var mainState = GetMainBySub(targetSubstate);
        NowMainState = mainState;
    }


    /// <summary>
    /// 通过副状态查找主状态
    /// </summary>
    private MainState GetMainBySub(SubState SearchSub)
    {
        foreach (KeyValuePair<MainState, SubState[]> kvp in StateMap)
        {
            foreach (SubState sub in kvp.Value)
            {
                if (sub == SearchSub) { return kvp.Key; }
            }
        }
        return 0;
    }



    /// <summary>
    /// ---Update和FixedUpdate中调用---，声明一个函数，当玩家进化后因为之前的玩家对象被销毁，所以需要重新获取玩家
    /// </summary>
    public void ResetPlayer()
    {
        if (player == null)
        {
            player = GameObject.FindObjectOfType<PlayerControler>();
        }
    }


    /// <summary>
    /// 忽略 WeavileMembersList 成员之间的碰撞（每只只有一个 Collider2D）
    /// </summary>
    void IgnoreCollisionBetweenTeamMembers()
    {
        for (int i = 0; i < WeavileMembersList.Count; i++)
        {
            var a = WeavileMembersList[i];
            if (a == null) continue;

            for (int j = i + 1; j < WeavileMembersList.Count; j++)
            {
                var b = WeavileMembersList[j];
                if (b == null) continue;

                Physics2D.IgnoreCollision(
                    a.GetComponent<Collider2D>(),
                    b.GetComponent<Collider2D>(),
                    true
                );
            }
        }
    }


    /// <summary>
    /// 获取血量最少的玛狃拉
    /// </summary>
    /// <returns></returns>
    public Weavile GetLeastHPWeavile( Weavile Self )
    {
        if (WeavileMembersList == null || WeavileMembersList.Count == 0)
            return null;

        Weavile result = null;
        float minHP = float.MaxValue;

        for (int i = 0; i < WeavileMembersList.Count; i++)
        {
            Weavile w = WeavileMembersList[i];
            if (w == null || w == Self)
                continue;

            float hp = w.GetHPPercent;
            if (hp < minHP)
            {
                minHP = hp;
                result = w;
            }
        }

        return result;
    }




    //□□□■■■□□□■■■□□□■■■小队成员检测■■■□□□■■■□□□■■■□□□

     
    /// <summary>
    /// 检测小队成员数 成员数变更时输出true
    /// </summary>
    public bool MembersCheck()
    {
        int membersNum = GetMembersNum();
        switch (membersNum)
        {
            case 3:
                if (NowMainState != MainState.Team3) { 
                    ResetState();
                    NowMainState = MainState.Team3;
                    TeamState_300_Idle_Start();
                    return true;
                }
                break;
            case 2:
                if (NowMainState != MainState.Team2) { 
                    ResetState();
                    NowMainState = MainState.Team2;
                    TeamState_200_Idle_Start();
                    return true;
                }
                break;
            case 1:
                if (NowMainState != MainState.Team1) { 
                    ResetState(); 
                    NowMainState = MainState.Team1;
                    TeamState_100_Normal_Start();
                    return true;
                }
                break;
            case 0:
                if (NowMainState != MainState.Team0)
                {
                    ResetState();
                    NowMainState = MainState.Team0;
                    return true;
                }
                break;
        }
        return false;
    }

    /// <summary>
    /// 获取小队成员数
    /// </summary>
    /// <returns></returns>
    int GetMembersNum()
    {
        int output = 0;
        List<Weavile> checkList = new List<Weavile> { };
        for (int i = 0; i < WeavileMembersList.Count; i++)
        {
            if (WeavileMembersList[i] != null && !checkList.Contains(WeavileMembersList[i]))
            {
                checkList.Add(WeavileMembersList[i]);
                output++;
            }
        }
        return output;
    }

    /// <summary>
    /// 检查 WeavileMembersList 与 CombatWeavile01/02/03 是否完全对应（包含 null）
    /// </summary>
    /// <returns>true = 完全对应；false = 不对应</returns>
    private bool CheckWeavileMapping()
    {
        // 左侧：WeavileMembersList
        List<Weavile> left = new List<Weavile>(WeavileMembersList);

        // 右侧：CombatWeavile01/02/03
        List<Weavile> right = new List<Weavile>
        {
            CombatWeavile01,
            CombatWeavile02,
            CombatWeavile03
        };

        //排空左右
        _mTool.RemoveNullInList<Weavile>(left);
        _mTool.RemoveNullInList<Weavile>(right);
        
        // 如果数量不同，直接 false（虽然你现在都是 3 个，但保持通用性）
        if (left.Count != right.Count)
            return false;

        // 逐个匹配（包含 null）
        // 使用一个临时列表来“消耗”匹配项，避免重复匹配同一个对象
        List<Weavile> tempRight = new List<Weavile>(right);

        foreach (var item in left)
        {
            // 找到对应元素（null 也能匹配）
            int index = tempRight.FindIndex(r => r == item);

            if (index < 0)
            {
                // 未找到对应项
                Debug.Log("ERROR:小队连招玛狃拉分配错误");
                return false;
            }

            // 找到后移除，避免重复匹配
            tempRight.RemoveAt(index);
        }
        // 所有元素都成功匹配
        return true;
    }


    //□□□■■■□□□■■■□□□■■■小队成员检测■■■□□□■■■□□□■■■□□□



    //■■■■■■■■■■■■■■■■■■■■共通■■■■■■■■■■■■■■■■■■■■■■







































    //■■■■■■■■■■■■■■■■■■■■状态机事件■■■■■■■■■■■■■■■■■■■■■■



    //□□□■■■□□□■■■□□□■■■通用变量■■■□□□■■■□□□■■■□□□

    /// <summary>
    /// 玛狃拉幻影的列表
    /// </summary>
    public List<Transform> WeavileAndCloneShadowList = new List<Transform> { };

    /// <summary>
    /// 添加分身
    /// </summary>
    /// <param name="t"></param>
    public void AddWeavileAndCloneShadowList(Transform t)
    {
        //排空
        _mTool.RemoveNullInList<Transform>(WeavileAndCloneShadowList);
        //不再队中加入
        if (!WeavileAndCloneShadowList.Contains(t))
        {
            WeavileAndCloneShadowList.Add(t);
        }
    }

    /// <summary>
    /// 移除分身
    /// </summary>
    /// <param name="t"></param>
    public void RemoveWeavileAndCloneShadowList(Transform t)
    {
        //排空
        _mTool.RemoveNullInList<Transform>(WeavileAndCloneShadowList);
        //再队中删除
        if (WeavileAndCloneShadowList.Contains(t))
        {
            WeavileAndCloneShadowList.Remove(t);
        }
    }

    /// <summary>
    /// 引爆所有分身
    /// </summary>
    public void DetonateAllCloneBodys()
    {
        List<WeavileCloneBody> wl = new List<WeavileCloneBody> { };
        _mTool.RemoveNullInList<Transform>(WeavileAndCloneShadowList);

        for (int i = 0; i < WeavileAndCloneShadowList.Count; i++)
        {
            var wc = WeavileAndCloneShadowList[i].GetComponent<WeavileCloneBody>();
            if (wc != null)
            {
                wl.Add(wc);
            }
        }
        Debug.Log(wl.Count);
        for (int i = 0; i < wl.Count; i++)
        {
            wl[i].SetCloneShadowOver();
        }
    }

    /// <summary>
    /// 获取某个玛狃拉或者分身在列表中的位序
    /// </summary>
    /// <returns></returns>
    public int GetWeavileOrCloneShadowIndex(Transform t)
    {
        int Output = -1;
        for (int i = 0; i < WeavileAndCloneShadowList.Count; i++)
        {
            if (WeavileAndCloneShadowList[i].gameObject.GetInstanceID() == t.gameObject.GetInstanceID()) { Output = i; }
        }
        if (Output == -1) { Debug.Log("Error:获取分身或本体位序失败 分身或本体不在列表中 或列表为空"); }
        //Debug.Log(t.gameObject.name + ":" + Output);
        return Output;
    }




    /// <summary>
    /// 玛狃拉 一人小队 烟雾弹的列表
    /// </summary>
    public List<WeavileFlingSmoke> WeavileSmokeList = new List<WeavileFlingSmoke> { };

    /// <summary>
    /// 添加烟雾
    /// </summary>
    /// <param name="t"></param>
    public void AddWeavileSmokeList(WeavileFlingSmoke t)
    {
        //排空
        _mTool.RemoveNullInList<WeavileFlingSmoke>(WeavileSmokeList);
        //不再队中加入
        if (!WeavileSmokeList.Contains(t))
        {
            WeavileSmokeList.Add(t);
        }
    }

    /// <summary>
    /// 移除烟雾
    /// </summary>
    /// <param name="t"></param>
    public void RemoveWeavileSmokeList(WeavileFlingSmoke t)
    {
        //排空
        _mTool.RemoveNullInList<WeavileFlingSmoke>(WeavileSmokeList);
        //再队中删除
        if (WeavileSmokeList.Contains(t))
        {
            WeavileSmokeList.Remove(t);
        }
    }

    /// <summary>
    /// 烟雾总数
    /// </summary>
    /// <returns></returns>
    public int GetCountWeavileSmokeList()
    {
        //排空
        _mTool.RemoveNullInList<WeavileFlingSmoke>(WeavileSmokeList);
        return WeavileSmokeList.Count;
    }


    //□□□■■■□□□■■■□□□■■■通用变量■■■□□□■■■□□□■■■□□□






    //□□□■■■□□□■■■□□□■■■小队连招分配■■■□□□■■■□□□■■■□□□


    /// <summary>
    /// 重置玛狃拉小队连招分配
    /// </summary>
    void ResetWeavileCombatIndex()
    {
        CombatWeavile01 = null;
        CombatWeavile02 = null;
        CombatWeavile03 = null;
    }

    /// <summary>
    /// 随机分配剩余玛狃拉至连招剩余空位
    /// </summary>
    void SetWeavileCombatIndex_Random()
    {
        int i = 0;
        // 当前已被占用的成员
        HashSet<Weavile> used = new HashSet<Weavile>();

        if (CombatWeavile01 != null) used.Add(CombatWeavile01);
        if (CombatWeavile02 != null) used.Add(CombatWeavile02);
        if (CombatWeavile03 != null) used.Add(CombatWeavile03);

        // 可用成员池（未被占用）
        List<Weavile> available = new List<Weavile>();

        //排空总队列
        _mTool.RemoveNullInList<Weavile>(WeavileMembersList);
        //将总队列所有成员加入成员池
        foreach (var w in WeavileMembersList)
        {
            if (w != null && !used.Contains(w))
                available.Add(w);
        }

        // 随机工具
        System.Random rand = new System.Random();

        // 分配函数
        Weavile GetRandomAvailable()
        {
            if (available.Count == 0)
                return null;

            int index = rand.Next(available.Count);
            Weavile chosen = available[index];
            available.RemoveAt(index); // 移除避免重复
            return chosen;
        }
        // 依次填补空位
        if (GetMembersNum() >= 1 && CombatWeavile01 == null)
            SetCombatWeavile01(GetRandomAvailable()); i++;

        if (GetMembersNum() >= 2 && CombatWeavile02 == null)
            SetCombatWeavile02(GetRandomAvailable()); i++;

        if (GetMembersNum() >= 3 && CombatWeavile03 == null)
            SetCombatWeavile03(GetRandomAvailable()); i++;
        Debug.Log("RandomCount:"+i);
        Debug.Log(CombatWeavile01);
        Debug.Log(CombatWeavile02);
        Debug.Log(CombatWeavile03);
    }

    /// <summary>
    /// 设置连招玛狃拉1
    /// </summary>
    void SetCombatWeavile01(Weavile w)
    {
        CombatWeavile01 = w;
        w.TeamCombatActIndex = 1;
        //Debug.Log("Weavile01");
    }

    /// <summary>
    /// 设置连招玛狃拉2
    /// </summary>
    void SetCombatWeavile02(Weavile w)
    {
        CombatWeavile02 = w;
        w.TeamCombatActIndex = 2;
        //Debug.Log("Weavile02");
    }

    /// <summary>
    /// 设置连招玛狃拉3
    /// </summary>
    void SetCombatWeavile03(Weavile w)
    {
        CombatWeavile03 = w;
        w.TeamCombatActIndex = 3;
        //Debug.Log("Weavile03");
    }

    /// <summary>
    /// 输入三个 transform 和目标点，输出 Left / Middle / Right
    /// </summary>
    /// <param name="targetPos"></param>
    public void SetWeavileCombatIndex_TeamState_301_Run(Vector3 targetPos)
    {
        //设置角度
        Vector2 v1 = (Vector2)(WeavileMembersList[0].transform.position - targetPos);
        Vector2 v2 = (Vector2)(WeavileMembersList[1].transform.position - targetPos);
        Vector2 v3 = (Vector2)(WeavileMembersList[2].transform.position - targetPos);

        float a1 = _mTool.Angle_360Y(v1, Vector2.right);
        float a2 = _mTool.Angle_360Y(v2, Vector2.right);
        float a3 = _mTool.Angle_360Y(v3, Vector2.right);

        //中间角度
        int mid = GetMiddleIndex(a1, a2, a3);

        float[] angles = { a1, a2, a3 };

        for (int i = 0; i < 3; i++)
        {
            if (i == mid)
            {
                SetCombatWeavile02(WeavileMembersList[i]);
            }
            else
            {
                int lr = JudgeLeftRight(angles[i], angles[mid]);
                if (lr < 0)
                {
                    SetCombatWeavile03(WeavileMembersList[i]);
                }
                else
                {
                    SetCombatWeavile01(WeavileMembersList[i]);
                }
            }
        }

    }

    /// <summary>
    /// 输入三个角度，返回中间角度的索引（0/1/2）
    /// </summary>
    /// <param name="a1"></param>
    /// <param name="a2"></param>
    /// <param name="a3"></param>
    /// <returns></returns>
    public static int GetMiddleIndex(float a1, float a2, float a3)
    {
        float[] arr = { a1, a2, a3 };
        int[] idx = { 0, 1, 2 };

        // 排序（角度和索引一起排）
        System.Array.Sort(arr, idx);

        float d1 = arr[1] - arr[0];
        float d2 = arr[2] - arr[1];
        float d3 = (arr[0] + 360f) - arr[2];

        // 最大夹角对应的“对面那条”是中间向量
        if (d1 >= d2 && d1 >= d3) return idx[2];
        if (d2 >= d1 && d2 >= d3) return idx[0];
        return idx[1];
    }

    /// <summary>
    /// 判断 angle 相对 middleAngle 是左还是右 返回 -1 = 左, 0 = 中间, 1 = 右
    /// </summary>
    /// <param name="angle"></param>
    /// <param name="middleAngle"></param>
    /// <returns></returns>
    public static int JudgeLeftRight(float angle, float middleAngle)
    {
        float diff = (angle - middleAngle + 540f) % 360f - 180f;

        if (Mathf.Abs(diff) < 0.0001f)
            return 0; // 完全相等

        return diff > 0 ? 1 : -1;
    }

    /// <summary>
    /// 三人小队_近战连招的连招序列设置_第一次
    /// 01固定 02 03根据距离
    /// </summary>
    void SetWeavileCombatIndex_TeamState_302_CloseAtk_FirstTime(Vector2 Target, Weavile closeOne)
    {
        ResetWeavileCombatIndex();


        // 1. 先分配 closeOne
        SetCombatWeavile01(closeOne);

        // 2. 找出剩余两只
        List<Weavile> others = new List<Weavile>();

        foreach (var w in WeavileMembersList)
        {
            if (w != null && w != closeOne)
                others.Add(w);
        }

        if (others.Count != 2)
        {
            Debug.LogWarning("WeavileMembersList 成员数量异常，无法正确分配 CombatWeavile02/03");
            return;
        }

        // 3. 按距离 target 排序（近的排前）
        others.Sort((a, b) =>
        {
            float da = Vector2.Distance(a.transform.position, Target);
            float db = Vector2.Distance(b.transform.position, Target);
            return da.CompareTo(db);
        });

        // 4. 分配
        SetCombatWeavile02(others[0]); // 离 target 最近
        SetCombatWeavile03(others[1]); // 稍远

    }

    /// <summary>
    /// 三人小队_近战连招的连招序列设置_第二次
    /// </summary>
    void SetWeavileCombatIndex_TeamState_302_CloseAtk_Second()
    {
        for (int i = 0; i < WeavileMembersList.Count; i++)
        {
            //检查每一个玛狃拉
            if (WeavileMembersList[i] != null)
            {
                switch (WeavileMembersList[i].TeamCombatActIndex)
                {
                    //玛狃拉1设置为玛狃拉3
                    case 1:
                        SetCombatWeavile03(WeavileMembersList[i]);
                        break;
                    //玛狃拉2设置为玛狃拉1
                    case 2:
                        SetCombatWeavile01(WeavileMembersList[i]);
                        break;
                    //玛狃拉3设置为玛狃拉2
                    case 3:
                        SetCombatWeavile02(WeavileMembersList[i]);
                        break;
                }
            }
        }
    }

    /// <summary>
    /// 三人小队_绕圈连招 外环攻击 的连招序列设置_完全根据距离分配 01 / 02 / 03
    /// 距离最近 → 01
    /// 中间 → 02
    /// 最远 → 03
    /// </summary>
    void SetWeavileCombatIndex_TeamState_306_CircleRun_OuterAyk_ByDistance(Vector2 target)
    {
        // 1. 收集全部有效成员
        List<Weavile> list = new List<Weavile>();

        foreach (var w in WeavileMembersList)
        {
            if (w != null)
                list.Add(w);
        }

        if (list.Count != 3)
        {
            Debug.LogWarning("WeavileMembersList 成员数量异常，无法正确分配 CombatWeavile01/02/03");
            return;
        }

        // 2. 按距离 target 排序（近的排前）
        list.Sort((a, b) =>
        {
            float da = Vector2.Distance(a.transform.position, target);
            float db = Vector2.Distance(b.transform.position, target);
            return da.CompareTo(db);
        });

        // 3. 分配
        SetCombatWeavile01(list[0]); // 最近
        SetCombatWeavile02(list[1]); // 中间
        SetCombatWeavile03(list[2]); // 最远
    }

    /// <summary>
    /// 三人小队 替身连招 分配303替身连招的连招分配 根据距离分配三只 最远的为01
    /// </summary>
    /// <param name="Target"></param>
    /// <param name="closeOne"></param>
    void SetWeavileCombatIndex_TeamState_303_Substitute(Vector2 Target)
    {
        ResetWeavileCombatIndex();

        // 1. 收集所有有效成员
        List<Weavile> list = new List<Weavile>();

        foreach (var w in WeavileMembersList)
        {
            if (w != null)
                list.Add(w);
        }

        if (list.Count != 3)
        {
            Debug.LogWarning("WeavileMembersList 成员数量异常，无法正确分配 CombatWeavile01/02/03");
            return;
        }

        // 2. 按距离 target 排序（近的排前）
        list.Sort((a, b) =>
        {
            float da = Vector2.Distance(a.transform.position, Target);
            float db = Vector2.Distance(b.transform.position, Target);
            return da.CompareTo(db); // 近 → 远
        });

        // list[0] = 最近
        // list[1] = 中间
        // list[2] = 最远

        // 3. 分配（你要求：最远=01，中间=02，最近=03）
        SetCombatWeavile01(list[2]); // 最远
        SetCombatWeavile02(list[1]); // 中间
        SetCombatWeavile03(list[0]); // 最近

    }

    /// <summary>
    /// 三人小队 回血连招12 分配307回血连招1/308回血连招2的连招分配 根据剩余血量百分比 最低的为01 
    /// </summary>
    /// <param name="Target"></param>
    /// <param name="closeOne"></param>
    void SetWeavileCombatIndex_TeamState_307_Heal1_TeamState_308_Heal2()
    {
        ResetWeavileCombatIndex();


        // 1. 收集所有有效成员
        List<Weavile> list = new List<Weavile>();

        foreach (var w in WeavileMembersList)
        {
            if (w != null)
                list.Add(w);
        }

        if (list.Count != 3)
        {
            Debug.LogWarning("WeavileMembersList 成员数量异常，无法正确分配 CombatWeavile01/02/03");
            return;
        }

        // 2. 按距血线排序（近的排前）
        list.Sort((a, b) =>
        {
            float da = a.GetHPPercent;
            float db = b.GetHPPercent;
            return da.CompareTo(db); // 小 → 大
        });

        // list[0] = 最近
        // list[1] = 中间
        // list[2] = 最远
        Debug.Log(list[0].name);
        Debug.Log(list[1].name);
        Debug.Log(list[2].name);
        // 3. 分配（你要求：最低=01，次低=02，最高=03）
        SetCombatWeavile01(list[0]); // 最低
        SetCombatWeavile02(list[1]); // 次低
        SetCombatWeavile03(list[2]); // 最高
        Debug.Log(WeavileMembersList[0].TeamCombatActIndex);
        Debug.Log(WeavileMembersList[1].TeamCombatActIndex);
        Debug.Log(WeavileMembersList[2].TeamCombatActIndex);
    }







    /// <summary>
    /// 二人小队_近战连招的连招序列设置_第一次
    /// 01固定 另一个02
    /// </summary>
    void SetWeavileCombatIndex_TeamState_202_CloseAtk_FirstTime(Vector2 Target, Weavile closeOne)
    {
        ResetWeavileCombatIndex();

        _mTool.RemoveNullInList<Weavile>(WeavileMembersList);
        if (WeavileMembersList.Count != 2) { Debug.Log("Error:二人小队_近战连招 小队总玛狃拉人数不正确"); }

        // 1. 先分配 closeOne
        SetCombatWeavile01(closeOne);

        // 2. 找出剩余一只
        List<Weavile> others = new List<Weavile>();

        foreach (var w in WeavileMembersList)
        {
            if (w != null && w != closeOne)
                others.Add(w);
        }

        if (others.Count != 1)
        {
            Debug.LogWarning("WeavileMembersList 成员数量异常，无法正确分配 CombatWeavile02");
            return;
        }

        // 4. 分配
        SetCombatWeavile02(others[0]); 


    }


    /// <summary>
    /// 二人小队_近战连招的连招序列设置_第二次
    /// </summary>
    void SetWeavileCombatIndex_TeamState_202_CloseAtk_Second()
    {
        for (int i = 0; i < WeavileMembersList.Count; i++)
        {
            //检查每一个玛狃拉
            if (WeavileMembersList[i] != null)
            {
                switch (WeavileMembersList[i].TeamCombatActIndex)
                {
                    //玛狃拉1设置为玛狃拉2
                    case 1:
                        SetCombatWeavile02(WeavileMembersList[i]);
                        break;
                    //玛狃拉2设置为玛狃拉1
                    case 2:
                        SetCombatWeavile01(WeavileMembersList[i]);
                        break;
                }
            }
        }
    }



    /// <summary>
    /// 二人小队 替身连招 分配203替身连招的连招分配 根据距离分配两只 最远的为01
    /// </summary>
    /// <param name="Target"></param>
    /// <param name="closeOne"></param>
    void SetWeavileCombatIndex_TeamState_203_Substitute(Vector2 Target)
    {
        ResetWeavileCombatIndex();

        // 1. 收集所有有效成员
        List<Weavile> list = new List<Weavile>();

        foreach (var w in WeavileMembersList)
        {
            if (w != null)
                list.Add(w);
        }

        if (list.Count != 2)
        {
            Debug.LogWarning("WeavileMembersList 成员数量异常，无法正确分配 CombatWeavile01/02");
            return;
        }

        // 2. 按距离 target 排序（近的排前）
        list.Sort((a, b) =>
        {
            float da = Vector2.Distance(a.transform.position, Target);
            float db = Vector2.Distance(b.transform.position, Target);
            return da.CompareTo(db); // 近 → 远
        });

        // list[0] = 最近
        // list[1] = 中间

        // 3. 分配（你要求：最远=01，最近=02）
        SetCombatWeavile01(list[1]); // 最远
        SetCombatWeavile02(list[0]); // 最近

    }



    /// <summary>
    /// 二人小队_绕圈连招 外环攻击 的连招序列设置_完全根据距离分配 01 / 02 
    /// 距离最近 → 01
    /// 最远 → 02
    /// </summary>
    void SetWeavileCombatIndex_TeamState_205_CircleRun_OuterAyk_ByDistance(Vector2 target)
    {
        // 1. 收集全部有效成员
        List<Weavile> list = new List<Weavile>();

        foreach (var w in WeavileMembersList)
        {
            if (w != null)
                list.Add(w);
        }

        if (list.Count != 2)
        {
            Debug.LogWarning("WeavileMembersList 成员数量异常，无法正确分配 CombatWeavile01/02");
            return;
        }

        // 2. 按距离 target 排序（近的排前）
        list.Sort((a, b) =>
        {
            float da = Vector2.Distance(a.transform.position, target);
            float db = Vector2.Distance(b.transform.position, target);
            return da.CompareTo(db);
        });

        // 3. 分配
        SetCombatWeavile01(list[0]); // 最近
        SetCombatWeavile02(list[1]); // 最远
    }



    /// <summary>
    /// 二人小队 回血连招 分配206回血的连招分配 根据剩余血量百分比 最低的为01 
    /// </summary>
    /// <param name="Target"></param>
    /// <param name="closeOne"></param>
    void SetWeavileCombatIndex_TeamState_206_Heal()
    {
        ResetWeavileCombatIndex();


        // 1. 收集所有有效成员
        List<Weavile> list = new List<Weavile>();

        foreach (var w in WeavileMembersList)
        {
            if (w != null)
                list.Add(w);
        }

        if (list.Count != 2)
        {
            Debug.LogWarning("WeavileMembersList 成员数量异常，无法正确分配 CombatWeavile01/02");
            return;
        }

        // 2. 按距血线排序（近的排前）
        list.Sort((a, b) =>
        {
            float da = a.GetHPPercent;
            float db = b.GetHPPercent;
            return da.CompareTo(db); // 小 → 大
        });

        // list[0] = 最近
        // list[1] = 中间
        // 3. 分配（你要求：最低=01，最高=02）
        SetCombatWeavile01(list[0]); // 最低
        SetCombatWeavile02(list[1]); // 最高


    }

    //□□□■■■□□□■■■□□□■■■小队连招分配■■■□□□■■■□□□■■■□□□










    //□□□■■■□□□■■■□□□■■■主状态机：三人小队■■■□□□■■■□□□■■■□□□







    //○○○○●●○○○○●●○○○○●●三人小队_通用变量●●○○○○●●○○○○●●○○○○



    /// <summary>
    /// 三人小队 回复连招12通用的冷却计时器
    /// </summary>
    public float Timer_HealCD_TeamState3 = 0.0f;

     

    //○○○○●●○○○○●●○○○○●●三人小队_通用变量●●○○○○●●○○○○●●○○○○







    //○○○○●●○○○○●●○○○○●●三人小队_发呆状态●●○○○○●●○○○○●●○○○○

    /// <summary>
    /// 三人小队_发呆状态开始
    /// </summary>
    public void TeamState_300_Idle_Start()
    {
        ResetWeavileCombatIndex();
        SetWeavileCombatIndex_Random();
        ChangeSubState(SubState.TeamState_300_Idle);
    }

    /// <summary>
    /// 三人小队_发呆状态结束
    /// </summary>
    public void TeamState_300_Idle_Over()
    {
        
    }




    //○○○○●●○○○○●●○○○○●●三人小队_发呆状态●●○○○○●●○○○○●●○○○○











    //○○○○●●○○○○●●○○○○●●三人小队_巡逻状态●●○○○○●●○○○○●●○○○○

    /// <summary>
    /// 三人小队巡逻状态时 触发小队302近战连招时 被接近的玛狃拉所需的最小距离
    /// </summary>
    static float DISTENCE_TEAM301_RUN_2_302_CloseAtk = 1.7f;
    /// <summary>
    /// 三人小队巡逻状态时 触发小队302近战连招时 被接近的玛狃拉所需的角度
    /// </summary>
    static float ANGLE_TEAM301_RUN_2_302_CloseAtk = 25.0f * Mathf.Deg2Rad;


    /// <summary>
    /// 巡逻状态转到替身连招或分身连招需要的包围失败所需的时间
    /// </summary>
    static float TIME_TEAM301_RUN_2_303_Substitute_304_CloneShadow_SURROUND_FAIL = 1.2f;


    /// <summary>
    /// 三人小队 巡逻转入迷宫连招状态的数据型
    /// </summary>
    struct PERCENT_Run2Maze
    {
        public float MazeTime;
        public float Percent;
    }
    /// <summary>
    /// 三人小队 巡逻转入迷宫招状态的概率时间等级表
    /// </summary>
    static List<PERCENT_Run2Maze> Dictionary_Percent_Run2Maze = new List<PERCENT_Run2Maze>
    {
        new PERCENT_Run2Maze{ MazeTime =  1.8f , Percent = 0.05f },
        new PERCENT_Run2Maze{ MazeTime =  2.8f , Percent = 0.10f },
        new PERCENT_Run2Maze{ MazeTime =  4.2f , Percent = 0.22f },
        new PERCENT_Run2Maze{ MazeTime =  6.5f , Percent = 0.44f },
        new PERCENT_Run2Maze{ MazeTime =  9.0f , Percent = 1.00f },
    };
    /// <summary>
    /// 三人小队巡逻状态时 触发其他连招时 触发小队305迷宫连招的概率
    /// </summary>
    static float PERCENT_ANOTHERCOMBAT2MAZE = 0.07f;


    /// <summary>
    /// 转到绕圈连招时，形成包围时时所需的玛狃拉最小夹角（两玛狃拉夹角小于此则包围失败）
    /// </summary>
    static float ANGLE_MIN_TEAM301_RUN_2_306_CIRCLERUN_SURROUND_SUCCESS = 108;
    /// <summary>
    /// 转到绕圈连招时，形成包围时时所需的玛狃拉最大夹角（两玛狃拉夹角大于此则包围失败）
    /// </summary>
    static float ANGLE_MAX_TEAM301_RUN_2_306_CIRCLERUN_SURROUND_SUCCESS = 132;
    /// <summary>
    /// 巡逻状态转到绕圈连招需要的形成包围所需的时间
    /// </summary>
    static float TIME_TEAM301_RUN_2_306_CIRCLERUN_SURROUND_SUCCESS = 0.65f;





    /// <summary>
    /// 三人小队 巡逻连招 包围成功计时器
    /// </summary>
    float Timer_TeamState_301_Run_Surround_Success = 0.0f;
    /// <summary>
    /// 三人小队 巡逻连招 包围失败计时器
    /// </summary>
    float Timer_TeamState_301_Run_Surround_Fail = 0.0f;
    /// <summary>
    /// 三人小队 巡逻连招 包围是否成功一次，成果过一次后如何长时间失败转入303或304
    /// </summary>
    bool isSurroundSccessOnceFlg_TeamState301Run = false;
    /// <summary>
    /// 三人小队 巡逻连招转入迷宫连招 检查等级 到达相应的等级后检查一次 并且提升检查等级
    /// </summary>
    int Level_TeamState301Run2TeamState305Maze_MazeCheck = 0;
    /// <summary>
    /// 三人小队 巡逻连招转入迷宫连招的计时器
    /// </summary>
    float Timer_TeamState_301_Run_TeamState301Run2TeamState305Maze = 0.0f;



    /// <summary>
    /// 三人小队_巡逻状态开始
    /// </summary>
    public void TeamState_301_Run_Start()
    {
        ResetWeavileCombatIndex();
        SetWeavileCombatIndex_TeamState_301_Run(TargetPosition);
        ChangeSubState(SubState.TeamState_301_Run);

        Timer_TeamState_301_Run_Surround_Success = 0.0f;
        Timer_TeamState_301_Run_Surround_Fail = 0.0f;
        isSurroundSccessOnceFlg_TeamState301Run = false;
        Level_TeamState301Run2TeamState305Maze_MazeCheck = 0;
        Timer_TeamState_301_Run_TeamState301Run2TeamState305Maze = 0.0f;
    }

    /// <summary>
    /// 三人小队_巡逻状态结束
    /// </summary>
    public void TeamState_301_Run_Over()
    {
        Timer_TeamState_301_Run_Surround_Success = 0.0f;
        Timer_TeamState_301_Run_Surround_Fail = 0.0f;
        isSurroundSccessOnceFlg_TeamState301Run = false;
        Level_TeamState301Run2TeamState305Maze_MazeCheck = 0;
        Timer_TeamState_301_Run_TeamState301Run2TeamState305Maze = 0.0f;
    }





    //=========================检查是否形成包围============================
    /// <summary>
    /// 检查三个玛狃拉是否形成包围
    /// </summary>
    bool TeamState301_Run_CheckSurroundSccess(Vector3 targetPos)
    {
        //设置角度
        Vector2 v1 = (Vector2)(WeavileMembersList[0].transform.position - targetPos);
        Vector2 v2 = (Vector2)(WeavileMembersList[1].transform.position - targetPos);
        Vector2 v3 = (Vector2)(WeavileMembersList[2].transform.position - targetPos);

        float a1 = _mTool.Angle_360Y(v1, Vector2.right);
        float a2 = _mTool.Angle_360Y(v2, Vector2.right);
        float a3 = _mTool.Angle_360Y(v3, Vector2.right);


        // 所有三对都必须满足
        return
            IsAngleBetween(a1, a2) &&
            IsAngleBetween(a1, a3) &&
            IsAngleBetween(a2, a3);
    }

    /// <summary>
    /// 计算两个角度的最小夹角（0~180）
    /// </summary>
    float MinAngle(float a, float b)
    {
        float diff = Mathf.Abs(a - b);
        return diff > 180f ? 360f - diff : diff;
    }

    /// <summary>
    /// 判断两个角度的夹角是否在 [minA, maxA] 区间内
    /// </summary>
    bool IsAngleBetween(float a, float b)
    {
        float ang = MinAngle(a, b);
        return ang >= ANGLE_MIN_TEAM301_RUN_2_306_CIRCLERUN_SURROUND_SUCCESS && ang <= ANGLE_MAX_TEAM301_RUN_2_306_CIRCLERUN_SURROUND_SUCCESS;
    }

    //=========================检查是否形成包围============================










    //=========================检查是否有玛狃拉被接近，并且与目标角度在正方向============================

    /// <summary>
    /// 检查每一只玛狃拉 ， 其中是否有接近目标者
    /// </summary>
    /// <returns></returns>
    Weavile TeamState301_Run_CheckAllWeaileBeClosed()
    {
        if (CheckOneWeaileBeClosed(WeavileMembersList[0])) { return WeavileMembersList[0]; }
        if (CheckOneWeaileBeClosed(WeavileMembersList[1])) { return WeavileMembersList[1]; }
        if (CheckOneWeaileBeClosed(WeavileMembersList[2])) { return WeavileMembersList[2]; }
        return null;
    }

    /// <summary>
    /// 检查某只玛狃拉是否接近目标 如果被接近输出true
    /// </summary>
    /// <returns></returns>
    bool CheckOneWeaileBeClosed( Weavile w)
    {
        if (
            w != null &&
            Vector2.Distance(w.TARGET_POSITION, (Vector2)w.transform.position) <= DISTENCE_TEAM301_RUN_2_302_CloseAtk && /*玛狃拉和玩家距离小于DISTENCE_TEAM301_RUN_2_302_CloseAtk*/
            ((Mathf.Abs((w.TARGET_POSITION - (Vector2)w.transform.position).normalized.x) < Mathf.Sin(ANGLE_TEAM301_RUN_2_302_CloseAtk)) || ((Mathf.Abs((w.TARGET_POSITION - (Vector2)w.transform.position).normalized.y) < Mathf.Sin(ANGLE_TEAM301_RUN_2_302_CloseAtk))))  /*和玩家夹角小于ANGLE_COMBAT_CONDITION_030*/
            )
        {
            return true;
        }
        return false;
    }


    //=========================检查是否有玛狃拉被接近，并且与目标角度在正方向============================




    //○○○○●●○○○○●●○○○○●●三人小队_巡逻状态●●○○○○●●○○○○●●○○○○















    //○○○○●●○○○○●●○○○○●●三人小队_近战连招●●○○○○●●○○○○●●○○○○


    /// <summary>
    /// 三人小队_近战攻击的最大次数
    /// </summary>
    public static int COUNT_TEAMSTATE302_CLOSEATK_MAX = 10;
    /// <summary>
    /// 三人小队_近战攻击的最小次数
    /// </summary>
    public static int COUNT_TEAMSTATE302_CLOSEATK_MIN = 5;
    /// <summary>
    /// 三人小队_近战攻击之间间隔的休息时间
    /// </summary>
    public static float TIME_TEAMSTATE302_CLOSEATK_MAX_INTERVAL = 1.0f;





    /// <summary>
    /// 三人小队_近战连招 已经使用近战连招的次数
    /// </summary>
    public int Count_Teamstate302_Close_Atk = 0;
    /// <summary>
    /// 三人小队_近战连招 使用近战连招多次动作之间的休息时间计时器
    /// </summary>
    public float Timer_Teamstate302_CloseAtk_IntervalIdle = 0.0f;
    /// <summary>
    /// 三人小队_近战连招 本次近战连招的预计的使用次数（随机）
    /// </summary>
    public int Count_Teamstate302_CloseAtk_Random = 0;




    /// <summary>
    /// 三人小队_近战连招开始
    /// </summary>
    public void TeamState_302_CloseAtk_Start()
    {
        ChangeSubState(SubState.TeamState_302_CloseAtk);
        Count_Teamstate302_Close_Atk = 0;
        Count_Teamstate302_CloseAtk_Random = Random.Range(COUNT_TEAMSTATE302_CLOSEATK_MIN ,COUNT_TEAMSTATE302_CLOSEATK_MAX);
        Timer_Teamstate302_CloseAtk_IntervalIdle = 0.0f;
        CombatWeavile01.IsCombatComplite_TeamState302_CloseAtk = false;
        CombatWeavile02.IsCombatComplite_TeamState302_CloseAtk = false;
        CombatWeavile03.IsCombatComplite_TeamState302_CloseAtk = false;
    }

    /// <summary>
    /// 三人小队_近战连招结束
    /// </summary>
    public void TeamState_302_CloseAtk_Over()
    {
        if (CombatWeavile01 != null) { CombatWeavile01.TeamCombatCompliteEvent_TeamState302_CloseAtk(); }
        if (CombatWeavile02 != null) { CombatWeavile02.TeamCombatCompliteEvent_TeamState302_CloseAtk(); }
        if (CombatWeavile03 != null) { CombatWeavile03.TeamCombatCompliteEvent_TeamState302_CloseAtk(); }
        Count_Teamstate302_Close_Atk = 0;
        Count_Teamstate302_CloseAtk_Random = 0;
        Timer_Teamstate302_CloseAtk_IntervalIdle = 0.0f;
    }





    //○○○○●●○○○○●●○○○○●●三人小队_近战连招●●○○○○●●○○○○●●○○○○










    //○○○○●●○○○○●●○○○○●●三人小队_替身连招●●○○○○●●○○○○●●○○○○


    /// <summary>
    /// 三人小队_替身连招 投掷动作之间间隔的休息时间
    /// </summary>
    public static float TIME_TEAMSTATE303SUBSTTITUTE_FLING_IDLEINTERVAL = 0.0f;
    /// <summary>
    /// 三人小队_替身连招 跳跃动作之间间隔的休息时间
    /// </summary>
    public static float TIME_TEAMSTATE303SUBSTTITUTE_JUMP_IDLEINTERVAL = 1.0f;
    /// <summary>
    /// 三人小队_替身连招 保护作之间间隔的休息时间
    /// </summary>
    public static float TIME_TEAMSTATE303SUBSTTITUTE_PROTECT_IDLEINTERVAL = 1.2f;
    /// <summary>
    /// 三人小队_替身连招 毒炸弹动作之间间隔的休息时间
    /// </summary>
    public static float TIME_TEAMSTATE303SUBSTTITUTE_TOXIC_IDLEINTERVAL = 1.2f;

     
    public enum TEAMSTATE303_SUBSTATE
    {
        Fling,
        Jump,
        Protect,
        Toxic, 
        Rush,
    }
    /// <summary>
    /// 三人小队 替身连招 303 副状态
    /// </summary>
    public TEAMSTATE303_SUBSTATE NowTeamState303SubState;



    /// <summary>
    /// 替身实例
    /// </summary>
    public WeavileSubstituteOBJ TeamState303_Substitute_SubstituteOBJ;
    /// <summary>
    /// 使用替身·连招多次动作之间的休息时间计时器
    /// </summary>
    public float Timer_TeamState303_Substitute_IntervalIdle = 0.0f;
    /// <summary>
    /// 替身实例是否被生成
    /// </summary>
    bool IsBorn_TeamState303_Substitute_Substitute = false;
    /// <summary>
    /// 替身连招是被攻击 转出替身连招
    /// </summary>
    public bool Flg_IsBeHIt_TeamState303_Substitute = false;



    /// <summary>
    /// 三人小队_替身连招开始
    /// </summary>
    public void TeamState_303_Substitute_Start()
    {
        NowTeamState303SubState = TEAMSTATE303_SUBSTATE.Fling;
        if (TeamState303_Substitute_SubstituteOBJ != null) { TeamState303_Substitute_SubstituteOBJ = null; }//清空替身实例缓存
        Timer_TeamState303_Substitute_IntervalIdle = 0.0f;
        IsBorn_TeamState303_Substitute_Substitute = false;
        Flg_IsBeHIt_TeamState303_Substitute = false;

        ChangeSubState(SubState.TeamState_303_Substitute);
        //根据距离分配
        SetWeavileCombatIndex_TeamState_303_Substitute(TargetPosition);
        
    }

    /// <summary>
    /// 三人小队_替身连招结束
    /// </summary>
    public void TeamState_303_Substitute_Over()
    {
        if (CombatWeavile01 != null) { CombatWeavile01.TeamCombatCompliteEvent_TeamState303_Substitue(); }
        if (CombatWeavile02 != null) { CombatWeavile02.TeamCombatCompliteEvent_TeamState303_Substitue(); }
        if (CombatWeavile03 != null) { CombatWeavile03.TeamCombatCompliteEvent_TeamState303_Substitue(); }

        NowTeamState303SubState = TEAMSTATE303_SUBSTATE.Fling;
        if (TeamState303_Substitute_SubstituteOBJ != null) { TeamState303_Substitute_SubstituteOBJ = null; }//清空替身实例缓存
        Timer_TeamState303_Substitute_IntervalIdle = 0.0f;
        IsBorn_TeamState303_Substitute_Substitute = false;
        Flg_IsBeHIt_TeamState303_Substitute = false;
        ResetWeavileCombatIndex();
    }


    /// <summary>
    /// 设置替身实例
    /// </summary>
    public void TeamState303_Substitute_SetSubstitute(WeavileSubstituteOBJ s)
    {
        TeamState303_Substitute_SubstituteOBJ = s;
        IsBorn_TeamState303_Substitute_Substitute = true;
    }

    /// <summary>
    /// 获取替身的位置
    /// </summary>
    /// <returns></returns>
    public Vector2 TeamState303_Substitute_GetSubstitutePostion
    {
        get
        {
            if (TeamState303_Substitute_SubstituteOBJ != null) { return TeamState303_Substitute_SubstituteOBJ.transform.position; }
            else  { return TARGET_POSITION; }
        }
    }






    //○○○○●●○○○○●●○○○○●●三人小队_替身连招●●○○○○●●○○○○●●○○○○











    //○○○○●●○○○○●●○○○○●●三人小队_分身连招●●○○○○●●○○○○●●○○○○
    /// <summary>
    /// 三人小队_分身连招 跳跃之后的休息时间 极短
    /// </summary>
    public static float TIME_TEAMSTATE304CLONESHADOW_JUMP_IDLEINTERVAL = 0.1f;
    /// <summary>
    /// 三人小队_分身连招 之后跳跃冰粒之后的休息时间
    /// </summary>
    public static float TIME_TEAMSTATE304CLONESHADOW_ICESHARD_IDLEINTERVAL = 0.9f;
    /// <summary>
    /// 三人小队_分身连招 跳跃的最大次数（不包含分身后的首次跳跃）
    /// </summary>
    public static int COUNT_TEAMSTATE304CLONESHADOW_JUMP_MAXCOUNT = 3;

    /// <summary>
    /// 每个玛狃拉之间的间距
    /// </summary>
    public static float DISTENCE_SPACING_TEAMSTATE304CLONESHADOW_BETWWEN_OTHER = 2.3f;
    /// <summary>
    /// 每次跳跃之间的间距
    /// </summary>
    public static float DISTENCE_SPACING_TEAMSTATE304CLONESHADOW_EVERYSTEP = 1.8f;



    public enum TEAMSTATE304_CLONESHADOW
    {
        CloneShadow,    //分身+首次跳跃
        Jump,           //后几次跳跃
        IceShard,       //冰粒
        OverRush,       //终结冲刺
        AccidentRush,   //意外冲刺
    }
    /// <summary>
    /// 三人小队 分身连招的子状态
    /// </summary>
    public TEAMSTATE304_CLONESHADOW NowTeamState304CloneShadow;


    /// <summary>
    /// 三人小队 分身连招 条约的次数
    /// </summary>
    public int Count_TeamState304CloneShadow_Jump = 0;

    /// <summary>
    /// 三人小队 分身连招 是否从房间的左侧开始跳跃（相反为右侧）
    /// </summary>
    public bool isLeft_TeamState304CloneShadow_JumpStart = false;

    /// <summary>
    /// 使用分身连招多次动作之间的休息时间计时器
    /// </summary>
    public float Timer_CloneShadow_IntervalIdle = 0.0f;




    /// <summary>
    /// 三人小队_分身连招开始
    /// </summary>
    public void TeamState_304_CloneShadow_Start()
    {
        NowTeamState304CloneShadow = TEAMSTATE304_CLONESHADOW.CloneShadow;
        Count_TeamState304CloneShadow_Jump = 0;
        isLeft_TeamState304CloneShadow_JumpStart = false;
        Timer_CloneShadow_IntervalIdle = 0.0f;

        ChangeSubState(SubState.TeamState_304_CloneShadow);
        //随机重置玛狃拉连招
        SetWeavileCombatIndex_Random();
    }

    /// <summary>
    /// 三人小队_分身连招结束
    /// </summary>
    public void TeamState_304_CloneShadow_Over()
    {
        if (CombatWeavile01 != null) { CombatWeavile01.TeamCombatCompliteEvent_TeamState304_CloneShadow(); }
        if (CombatWeavile02 != null) { CombatWeavile02.TeamCombatCompliteEvent_TeamState304_CloneShadow(); }
        if (CombatWeavile03 != null) { CombatWeavile03.TeamCombatCompliteEvent_TeamState304_CloneShadow(); }

        //引爆所有替身
        DetonateAllCloneBodys();

        NowTeamState304CloneShadow = TEAMSTATE304_CLONESHADOW.CloneShadow;
        Count_TeamState304CloneShadow_Jump = 0;
        isLeft_TeamState304CloneShadow_JumpStart = false;
        Timer_CloneShadow_IntervalIdle = 0.0f;

        //随机重置玛狃拉连招
        SetWeavileCombatIndex_Random();
    }


    /// <summary>
    /// 三人小队分身连招 根据当前的跳跃次数 和玛狃拉在队列中处于的位置 决定玛狃拉跳跃的位置
    /// </summary>
    /// <param name="t"></param>
    public Vector2 TeamState304CloneShadow_GetJumpTargetPosition(Transform t)
    {
        Vector2 Output = t.transform.position;
        //决定跳跃方向 左侧为正右侧为负数
        float dir = (isLeft_TeamState304CloneShadow_JumpStart) ? 1.0f : -1.0f;
        int i = GetWeavileOrCloneShadowIndex(t);
        //仅当分身或本体在列表中时才计算位置
        if (i != -1)
        {
            //确定横向位置
            //根据冲刺方向确定起始位置是房间右侧还是左侧
            float StartX = (isLeft_TeamState304CloneShadow_JumpStart) ? WeavileMembersList[0].ParentPokemonRoom.RoomSize[2] : WeavileMembersList[0].ParentPokemonRoom.RoomSize[3];
            StartX = (StartX + dir * 0.8f) + WeavileMembersList[0].ParentPokemonRoom.transform.position.x;
            float tX = StartX + dir * ((float)Count_TeamState304CloneShadow_Jump) * DISTENCE_SPACING_TEAMSTATE304CLONESHADOW_EVERYSTEP;
            if(Count_TeamState304CloneShadow_Jump < COUNT_TEAMSTATE304CLONESHADOW_JUMP_MAXCOUNT) { tX += dir * ((i % 2 == 0) ? 0 : 1); }
            

            //确定纵向位置
            float allDis = DISTENCE_SPACING_TEAMSTATE304CLONESHADOW_BETWWEN_OTHER * (float)(WeavileAndCloneShadowList.Count - 1);
            float StartY = allDis/2.0f + WeavileMembersList[0].ParentPokemonRoom.transform.position.y;
            float tY = StartY - DISTENCE_SPACING_TEAMSTATE304CLONESHADOW_BETWWEN_OTHER * ((float)i);
            Output = new Vector2(tX, tY);
            Output = WeavileMembersList[0].ParentPokemonRoom.EnsurePointReachesRoom(Output);
        }
        else { Debug.Log("Error:分身列表错误"); }
        return Output;
    }

    /// <summary>
    /// 三人小队分身连招 判断冲刺的方向
    /// </summary>
    /// <returns></returns>
    public void TeamState304CloneShadow_JudgeRushDir()
    {
        //目标点在房间的左侧 从右侧开冲
        if ((TargetPosition - (Vector2)WeavileMembersList[0].ParentPokemonRoom.transform.position).x <= 0)
        {
            isLeft_TeamState304CloneShadow_JumpStart = false;
        }
        else
        {
            isLeft_TeamState304CloneShadow_JumpStart = true;
        }
    }

    /// <summary>
    /// 重洗本体和分身列表
    /// </summary>
    public void RandomSetBodyAndCloneShadowList()
    {
        _mTool.RandomShuffleList<Transform>(WeavileAndCloneShadowList);
    }





    //○○○○●●○○○○●●○○○○●●三人小队_分身连招●●○○○○●●○○○○●●○○○○












    //○○○○●●○○○○●●○○○○●●三人小队_迷宫连招●●○○○○●●○○○○●●○○○○




    /// <summary>
    /// 迷宫是否结束
    /// </summary>
    public bool isOver_Maze_TeamState305Maze = false;



    /// <summary>
    /// 三人小队_迷宫连招开始
    /// </summary>
    public void TeamState_305_Maze_Start()
    {
        isOver_Maze_TeamState305Maze = false;

        ChangeSubState(SubState.TeamState_305_Maze);
        
        //根据距离分配玛狃拉
        SetWeavileCombatIndex_TeamState_303_Substitute(TargetPosition);
    }

    /// <summary>
    /// 三人小队_迷宫连招结束
    /// </summary>
    public void TeamState_305_Maze_Over()
    {
        //转入下一个状态
        if (CombatWeavile01 != null) { CombatWeavile01.TeamCombatCompliteEvent_TeamState305_Mazes(); }
        if (CombatWeavile02 != null) { CombatWeavile02.TeamCombatCompliteEvent_TeamState305_Mazes(); }
        if (CombatWeavile03 != null) { CombatWeavile03.TeamCombatCompliteEvent_TeamState305_Mazes(); }

        isOver_Maze_TeamState305Maze = false;
    }



    //○○○○●●○○○○●●○○○○●●三人小队_迷宫连招●●○○○○●●○○○○●●○○○○









    //○○○○●●○○○○●●○○○○●●三人小队_绕圈连招●●○○○○●●○○○○●●○○○○

    /// <summary>
    /// 三人小队绕圈连招 绕圈半径（玛狃拉们奔跑的轨迹半径）
    /// </summary>
    public static float RADIUS_TEAMSTATE_306_CIRCLERUN = 4.5f;
    /// <summary>
    /// 三人小队绕圈连招 绕圈半径内径 长期处于内径触发一起冲刺攻击
    /// </summary>
    public static float RADIUS_TEAMSTATE_306_CIRCLERUN_INNER = 2.0f;
    /// <summary>
    /// 三人小队绕圈连招 绕圈半径内径 长期处于内径触发一起冲刺攻击
    /// </summary>
    public static float RADIUS_TEAMSTATE_306_CIRCLERUN_OUTER = 5.0f;


    /// <summary>
    /// 三人小队绕圈连招 触发内环攻击的所需时间
    /// </summary>
    public static float TIME_TEAMSTATE_306_CIRCLERUN_INNERATK_CONDITION = 2.8f;
    /// <summary>
    /// 三人小队绕圈连招 内环攻击攻击之间间隔的休息时间
    /// </summary>
    public static float TIME_TEAMSTATE_306_CIRCLERUN_INNERATK_INTERVAL = 0.5f;


    /// <summary>
    /// 三人小队绕圈连招 触发外环攻击的所需时间
    /// </summary>
    public static float TIME_TEAMSTATE_306_CIRCLERUN_OUTERATK_CONDITION = 1.3f;
    /// <summary>
    /// 三人小队绕圈连招 外环攻击攻击之间间隔的休息时间
    /// </summary>
    public static float TIME_TEAMSTATE_306_CIRCLERUN_OUTERATK_INTERVAL = 0.5f;
    /// <summary>
    /// 三人小队绕圈连招 外环攻击 攻击最大次数
    /// </summary>
    public static float COUNT_TEAMSTATE_306_CIRCLERUN_OUTERATK_MAX = 3;


    /// <summary>
    /// 三人小队绕圈连招 触发脱离攻击的所需时间
    /// </summary>
    public static float TIME_TEAMSTATE_306_CIRCLERUN_ESCAPEATK_CONDITION = 1.2f;
    /// <summary>
    /// 三人小队绕圈连招 脱离攻击 投掷的次数
    /// </summary>
    public static int COUNT_FLING_TEAMSTATE_306_CIRCLERUN_ESCAPEATK = 3;


    /// <summary>
    /// 三人小队绕圈连招 绕圈速度加成
    /// </summary>
    public static float SPEEDALPHA_TEAMSTATE_306_CIRCLERUN = 2.5f;
    /// <summary>
    /// 三人小队绕圈连招 绕圈速度加成慢速
    /// </summary>
    public static float SPEEDALPHA_TEAMSTATE_306_CIRCLERUN_SLOW = 1.2f;
    /// <summary>
    /// 三人小队绕圈连招 绕圈速度加成快速
    /// </summary>
    public static float SPEEDALPHA_TEAMSTATE_306_CIRCLERUN_FAST = 4.0f;



    public enum TEAMSTATE306_SUBSTATE
    {
        Run,       //跑
        InnerAtk1, //内环攻击1
        InnerAtk2, //内环攻击2
        OuterAtk,  //外环攻击
        EscapeAtk, //逃离攻击
    }
    /// <summary>
    /// 三人连招 绕圈连招 副状态
    /// </summary>
    public TEAMSTATE306_SUBSTATE NowTeamState306SubState;

    /// <summary>
    /// 绕圈跑的中心
    /// </summary>
    public Vector2 Position_Center_TeamState_306_CircleRun = Vector2.zero;
    /// <summary>
    /// 内环攻击触发时间计时器 会随着时间累计
    /// </summary>
    float Timer_TeamState306CircleRun_Inner_Atk = 0.0f;
    /// <summary>
    /// 外环攻击触发时间计时器 进入内环后清空
    /// </summary>
    float Timer_TeamState306CircleRun_Outer_Atk = 0.0f;
    /// <summary>
    /// 三人小队 绕圈连招 外环攻击进行的次数
    /// </summary>
    public int Count_TeamState306CircleRun_OuterAtk = 0;
    /// <summary>
    /// 三人小队 绕圈连招 脱离攻击 投掷的次数
    /// </summary>
    public int Count_Fling_TeamState306CircleRun_EscapeAtk = 0;
    /// <summary>
    /// 逃离攻击触发时间计时器 进入内环或外环后清空
    /// </summary>
    float Timer_TeamState306CircleRun_Escape_Atk = 0.0f;
    /// <summary>
    /// 三人小队绕圈连招 使用攻击连招多次动作之间的休息时间计时器
    /// </summary>
    public float Timer_TeamState306CircleRun_Atk_IntervalIdle = 0.0f;





    /// <summary>
    /// 三人小队_绕圈连招开始
    /// </summary>
    public void TeamState_306_CircleRun_Start()
    {
        WeavileAndCloneShadowList.Clear();
        Position_Center_TeamState_306_CircleRun = Vector2.zero;
        Timer_TeamState306CircleRun_Inner_Atk = 0.0f;
        Timer_TeamState306CircleRun_Outer_Atk = 0.0f;
        Count_TeamState306CircleRun_OuterAtk = 0;
        Timer_TeamState306CircleRun_Escape_Atk = 0.0f;
        Timer_TeamState306CircleRun_Atk_IntervalIdle = 0.0f;
        Count_Fling_TeamState306CircleRun_EscapeAtk = 0;
        NowTeamState306SubState = TEAMSTATE306_SUBSTATE.Run;

        ResetWeavileCombatIndex();
        SetWeavileCombatIndex_Random();
        SetCircleRunCenter_Teamstate306CirclrRun_FirstTime();
        ChangeSubState(SubState.TeamState_306_CircleRun);

    }

    /// <summary>
    /// 三人小队_绕圈连招结束
    /// </summary>
    public void TeamState_306_CircleRun_Over()
    {
        switch (NowTeamState306SubState)
        {
            //外环攻击
            case TEAMSTATE306_SUBSTATE.OuterAtk:
                if (CombatWeavile01 != null) { CombatWeavile01.TeamCombatCompliteEvent_TeamState306_CircleRun_OuterAtk(); }
                if (CombatWeavile02 != null) { CombatWeavile02.TeamCombatCompliteEvent_TeamState306_CircleRun_OuterAtk(); }
                if (CombatWeavile03 != null) { CombatWeavile03.TeamCombatCompliteEvent_TeamState306_CircleRun_OuterAtk(); }
                break;
            //脱离攻击
            case TEAMSTATE306_SUBSTATE.EscapeAtk:
                if (CombatWeavile01 != null) { CombatWeavile01.TeamCombatCompliteEvent_TeamState306_CircleRun_EscapeAtk(); }
                if (CombatWeavile02 != null) { CombatWeavile02.TeamCombatCompliteEvent_TeamState306_CircleRun_EscapeAtk(); }
                if (CombatWeavile03 != null) { CombatWeavile03.TeamCombatCompliteEvent_TeamState306_CircleRun_EscapeAtk(); }
                break;
            default:
                break;
        }



        WeavileAndCloneShadowList.Clear();
        Position_Center_TeamState_306_CircleRun = Vector2.zero;
        Timer_TeamState306CircleRun_Inner_Atk = 0.0f;
        Timer_TeamState306CircleRun_Outer_Atk = 0.0f;
        Count_TeamState306CircleRun_OuterAtk = 0;
        Timer_TeamState306CircleRun_Escape_Atk = 0.0f;
        Timer_TeamState306CircleRun_Atk_IntervalIdle = 0.0f;
        Count_Fling_TeamState306CircleRun_EscapeAtk = 0;
        NowTeamState306SubState = TEAMSTATE306_SUBSTATE.Run;
        ResetWeavileCombatIndex();
    }



    /// <summary>
    /// 根据角度排序玛狃拉和残影
    /// </summary>
    void SortWeavileAndCloneBodtyByAngle_TeamState306CircleRun()
    {
        // 排空
        _mTool.RemoveNullInList<Transform>(WeavileAndCloneShadowList);

        // 临时列表：存储 Transform + 角度
        List<(Transform tf, float angle)> temp = new List<(Transform, float)>();

        for (int i = 0; i < WeavileAndCloneShadowList.Count; i++)
        {
            Transform t = WeavileAndCloneShadowList[i];

            // 计算相对中心的方向
            Vector2 dir = (Vector2)t.position - Position_Center_TeamState_306_CircleRun;

            // 计算 0~360° 角度
            float angle = _mTool.Angle_360Y(dir, Vector2.right);

            temp.Add((t, angle));
        }

        // 按角度排序
        temp.Sort((a, b) => a.angle.CompareTo(b.angle));

        // 回写排序后的 Transform 列表
        WeavileAndCloneShadowList.Clear();
        for (int i = 0; i < temp.Count; i++)
        {
            WeavileAndCloneShadowList.Add(temp[i].tf);
        }

        // 排序后进行速度调整
        AdjustSpeedByAngle(temp);
    }



    /// <summary>
    /// 根据角度差距自动加速/减速，使所有对象等距绕圈
    /// </summary>
    void AdjustSpeedByAngle(List<(Transform tf, float angle)> sorted)
    {
        int count = sorted.Count;
        if (count < 2) return;

        float targetGap = 360f / count;

        for (int i = 0; i < count; i++)
        {
            Transform current = sorted[i].tf;
            float angleCurrent = sorted[i].angle;

            // 下一个（循环）
            int nextIndex = (i + 1) % count;
            float angleNext = sorted[nextIndex].angle;

            // 计算角度差（归一化到 0~360）
            float gap = angleNext - angleCurrent;
            if (gap < 0) gap += 360f;

            // 判断是否需要加速或减速
            if (gap < targetGap * 0.95f)
            {
                // 太近 → 减速
                SpeedDown(current);
            }
            else if (gap > targetGap * 1.05f)
            {
                // 太远 → 加速
                SpeedUp(current);
            }
            else
            {
                // 在合理范围内 → 可选：保持当前速度
                KeepSpeed(current);
            }
        }
    }

    //高速
    void SpeedUp(Transform t)
    {
        var w = t.GetComponent<Weavile>();
        var wc = t.GetComponent<WeavileCloneBody>();
        if (w != null)  { w.SPEEDALPHA_CircleRun = SPEEDALPHA_TEAMSTATE_306_CIRCLERUN_FAST; }
        if (wc != null) { wc.SPEEDALPHA_TeamState306CircleRun = SPEEDALPHA_TEAMSTATE_306_CIRCLERUN_FAST; }
    }
    //低速
    void SpeedDown(Transform t)
    {
        var w = t.GetComponent<Weavile>();
        var wc = t.GetComponent<WeavileCloneBody>();
        if (w != null) { w.SPEEDALPHA_CircleRun = SPEEDALPHA_TEAMSTATE_306_CIRCLERUN_SLOW; }
        if (wc != null) { wc.SPEEDALPHA_TeamState306CircleRun = SPEEDALPHA_TEAMSTATE_306_CIRCLERUN_SLOW; }
    }
    //常速
    void KeepSpeed(Transform t)
    {
        var w = t.GetComponent<Weavile>();
        var wc = t.GetComponent<WeavileCloneBody>();
        if (w != null) { w.SPEEDALPHA_CircleRun = SPEEDALPHA_TEAMSTATE_306_CIRCLERUN; }
        if (wc != null) { wc.SPEEDALPHA_TeamState306CircleRun = SPEEDALPHA_TEAMSTATE_306_CIRCLERUN; }
    }

    /// <summary>
    /// 设置绕圈跑中心 首次为三只玛狃拉的中心点
    /// </summary>
    void SetCircleRunCenter_Teamstate306CirclrRun_FirstTime()
    {
        //设置奔跑中心
        Position_Center_TeamState_306_CircleRun = new Vector2(
            ((WeavileMembersList[0].transform.position.x + WeavileMembersList[1].transform.position.x + WeavileMembersList[2].transform.position.x) / 3.0f),
            ((WeavileMembersList[0].transform.position.x + WeavileMembersList[1].transform.position.x + WeavileMembersList[2].transform.position.x) / 3.0f));
        float r = RADIUS_TEAMSTATE_306_CIRCLERUN + 1.0f;
        Position_Center_TeamState_306_CircleRun = new Vector2(
            Mathf.Clamp(Position_Center_TeamState_306_CircleRun.x,                    //方向*速度
                WeavileMembersList[0].ParentPokemonRoom.RoomSize[2] + r + transform.parent.position.x, //最小值
                WeavileMembersList[0].ParentPokemonRoom.RoomSize[3] - r + transform.parent.position.x),//最大值
            Mathf.Clamp(Position_Center_TeamState_306_CircleRun.y,                     //方向*速度 
                WeavileMembersList[0].ParentPokemonRoom.RoomSize[1] + r + transform.parent.position.y,  //最小值
                WeavileMembersList[0].ParentPokemonRoom.RoomSize[0] - r + transform.parent.position.y));//最大值
        Debug.Log(Position_Center_TeamState_306_CircleRun);
    }

    /// <summary>
    /// 设置绕圈跑中心 第二次开始为以目标点为中心
    /// </summary>
    void SetCircleRunCenter_Teamstate306CirclrRun_SecondTime()
    {
        //设置奔跑中心
        Position_Center_TeamState_306_CircleRun = TargetPosition;
        float r = RADIUS_TEAMSTATE_306_CIRCLERUN + 1.0f;
        Position_Center_TeamState_306_CircleRun = new Vector2(
            Mathf.Clamp(Position_Center_TeamState_306_CircleRun.x,                    //方向*速度
                WeavileMembersList[0].ParentPokemonRoom.RoomSize[2] + r + transform.parent.position.x, //最小值
                WeavileMembersList[0].ParentPokemonRoom.RoomSize[3] - r + transform.parent.position.x),//最大值
            Mathf.Clamp(Position_Center_TeamState_306_CircleRun.y,                     //方向*速度 
                WeavileMembersList[0].ParentPokemonRoom.RoomSize[1] + r + transform.parent.position.y,  //最小值
                WeavileMembersList[0].ParentPokemonRoom.RoomSize[0] - r + transform.parent.position.y));//最大值
        Debug.Log(Position_Center_TeamState_306_CircleRun);
    }




    //○○○○●●○○○○●●○○○○●●三人小队_绕圈连招●●○○○○●●○○○○●●○○○○










    //○○○○●●○○○○●●○○○○●●三人小队_回血连招1●●○○○○●●○○○○●●○○○○


    /// <summary>
    /// 三人连招 回血连招1 使用后的冷却时间
    /// </summary>
    static float TIME_HEAL1_CD = 30.0f;
    /// <summary>
    /// 三人连招 回血连招1 触发连招的血线
    /// </summary>
    static float HPLINE_TEAMSYAYE307HEAL1_CONDITION = 0.35f;



    /// <summary>
    /// 回复是否结束
    /// </summary>
    public bool isOver_Heal_TeamState307Heal1;




    /// <summary>
    /// 三人小队_回血连招1开始
    /// </summary>
    public void TeamState_307_Heal1_Start()
    {
        isOver_Heal_TeamState307Heal1 = false;

        ChangeSubState(SubState.TeamState_307_Heal1);
    }

    /// <summary>
    /// 三人小队_回血连招1结束
    /// </summary>
    public void TeamState_307_Heal1_Over()
    {
        if (CombatWeavile01 != null) { CombatWeavile01.TeamCombatCompliteEvent_TeamState307_Heal(); }
        if (CombatWeavile02 != null) { CombatWeavile02.TeamCombatCompliteEvent_TeamState307_Heal(); }
        if (CombatWeavile03 != null) { CombatWeavile03.TeamCombatCompliteEvent_TeamState307_Heal(); }

        isOver_Heal_TeamState307Heal1 = false;
    }


    /// <summary>
    /// 是否达成进入回血连招1的条件
    /// </summary>
    /// <returns></returns>
    public bool Condition_TeamState307_Heal1()
    {
        bool output = false;
        if (WeavileMembersList.Count != 3) {
            Debug.Log("Error:三人小队回血连招1 条件判断 小队人数错误");
            return false; }
        //获取血线
        float h1 = WeavileMembersList[0].GetHPPercent;
        float h2 = WeavileMembersList[1].GetHPPercent;
        float h3 = WeavileMembersList[2].GetHPPercent;
        //最大最小差大于目标血线
        if (Mathf.Max(h1, h2, h3) - Mathf.Min(h1, h2, h3) > HPLINE_TEAMSYAYE307HEAL1_CONDITION) {
            output = true;
        }

        return output;
    }



    //○○○○●●○○○○●●○○○○●●三人小队_回血连招1●●○○○○●●○○○○●●○○○○









    //○○○○●●○○○○●●○○○○●●三人小队_回血连招2●●○○○○●●○○○○●●○○○○


    /// <summary>
    /// 三人连招 回血连招1 使用后的冷却时间
    /// </summary>
    static float TIME_HEAL2_CD = 50.0f;
    /// <summary>
    /// 三人连招 回血连招2 触发连招的血线
    /// </summary>
    static float HPLINE_TEAMSYAYE308HEAL2_CONDITION = 0.27f;




    /// <summary>
    /// 回复是否结束
    /// </summary>
    public bool isOver_Heal_TeamState308Heal2;



    /// <summary>
    /// 三人小队_回血连招2开始
    /// </summary>
    public void TeamState_308_Heal2_Start()
    {
        isOver_Heal_TeamState308Heal2 = false;

        ChangeSubState(SubState.TeamState_308_Heal2);
    }

    /// <summary>
    /// 三人小队_回血连招2结束
    /// </summary>
    public void TeamState_308_Heal2_Over()
    {

        if (CombatWeavile01 != null) { CombatWeavile01.TeamCombatCompliteEvent_TeamState308_Heal2(); }
        if (CombatWeavile02 != null) { CombatWeavile02.TeamCombatCompliteEvent_TeamState308_Heal2(); }
        if (CombatWeavile03 != null) { CombatWeavile03.TeamCombatCompliteEvent_TeamState308_Heal2(); }

        isOver_Heal_TeamState308Heal2 = false;
    }


    /// <summary>
    /// 是否达成进入回血连招1的条件
    /// </summary>
    /// <returns></returns>
    public bool Condition_TeamState308_Heal2()
    {
        List<float> hpList = new List<float>()
        {
            WeavileMembersList[0].GetHPPercent,
            WeavileMembersList[1].GetHPPercent,
            WeavileMembersList[2].GetHPPercent
        };

        //Debug.Log(hpList[0]) ;
        //Debug.Log(hpList[1]) ;
        //Debug.Log(hpList[2]) ;

        // 取最大值
        float max = Mathf.Max(hpList[0], hpList[1], hpList[2]);

        // 找出所有不是最大值的项（剩余两个）
        //List<float> others = hpList.Where(h => h != max).ToList();
        hpList.Remove(max);

        // 如果三个都相等（others.Count == 0），直接返回 false
        //if (others.Count == 0)
        //    return false;

        // 判断剩余两个是否都满足差值条件
        foreach (float h in hpList)
        {
            //Debug.Log(h);
            if (max - h <= HPLINE_TEAMSYAYE308HEAL2_CONDITION)
                return false;
        }

        return true;


    }



    //○○○○●●○○○○●●○○○○●●三人小队_回血连招2●●○○○○●●○○○○●●○○○○












    //○○○○●●○○○○●●○○○○●●三人小队_万灵药连招●●○○○○●●○○○○●●○○○○



    public enum TEAMSTATE309_SUBSTATE
    {
        Heal,           //回复
        Rush,           //冲刺
    }
    /// <summary>
    /// 三人小队 万灵药连招的子状态
    /// </summary>
    public TEAMSTATE309_SUBSTATE NowTeamState309SubState ;





    /// <summary>
    /// 三人小队_万灵药连招开始
    /// </summary>
    public void TeamState_309_FullHeal_Start()
    {
        NowTeamState309SubState = TEAMSTATE309_SUBSTATE.Heal;
        ChangeSubState(SubState.TeamState_309_FullHeal);
    }

    /// <summary>
    /// 三人小队_万灵药连招结束
    /// </summary>
    public void TeamState_309_FullHeal_Over()
    {
        if (CombatWeavile01 != null) { CombatWeavile01.TeamCombatCompliteEvent_TeamState309_FullHeal(); }
        if (CombatWeavile02 != null) { CombatWeavile02.TeamCombatCompliteEvent_TeamState309_FullHeal(); }
        if (CombatWeavile03 != null) { CombatWeavile03.TeamCombatCompliteEvent_TeamState309_FullHeal(); }

        NowTeamState309SubState = TEAMSTATE309_SUBSTATE.Heal;
    }


    /// <summary>
    /// 获取一个异常状态的玛狃拉 且该玛狃拉不是自己
    /// </summary>
    /// <returns></returns>
    public Weavile GetAUnusualWeavile(Weavile self)
    {
        Weavile Output = null;
        for (int i = 0; i < WeavileMembersList.Count; i++)
        {
            //如果有异常状态 且不是自己的玛狃拉 设其为投掷目标
            Debug.Log(WeavileMembersList[i].gameObject.GetInstanceID() + "+" + self.GetInstanceID());
            if (WeavileMembersList[i].isWeavileUnusualState() && (WeavileMembersList[i].gameObject.GetInstanceID() != self.gameObject.GetInstanceID()))
            {
                Output = WeavileMembersList[i];
                return Output;
            }
        }
        return Output;
    }


    //○○○○●●○○○○●●○○○○●●三人小队_万灵药连招●●○○○○●●○○○○●●○○○○




















    //□□□■■■□□□■■■□□□■■■主状态机：三人小队■■■□□□■■■□□□■■■□□□





































    //□□□■■■□□□■■■□□□■■■主状态机：二人小队■■■□□□■■■□□□■■■□□□





    //○○○○●●○○○○●●○○○○●●二人小队_通用变量●●○○○○●●○○○○●●○○○○

    /// <summary>
    /// 二人小队 回复连招通用的冷却计时器
    /// </summary>
    public float Timer_HealCD_TeamState2 = 0.0f;


    //○○○○●●○○○○●●○○○○●●二人小队_通用变量●●○○○○●●○○○○●●○○○○










    //○○○○●●○○○○●●○○○○●●二人小队_发呆状态●●○○○○●●○○○○●●○○○○


    /// <summary>
    /// 二人小队_发呆状态开始
    /// </summary>
    public void TeamState_200_Idle_Start()
    {
        ResetWeavileCombatIndex();
        SetWeavileCombatIndex_Random();
        ChangeSubState(SubState.TeamState_200_Idle);
    }

    /// <summary>
    /// 二人小队_发呆状态结束
    /// </summary>
    public void TeamState_200_Idle_Over()
    {
    }



    //○○○○●●○○○○●●○○○○●●二人小队_发呆状态●●○○○○●●○○○○●●○○○○










    //○○○○●●○○○○●●○○○○●●二人小队_巡逻状态●●○○○○●●○○○○●●○○○○


    /// <summary>
    /// 二人小队 巡逻状态 转到替身连招1或2 需要的包围失败所需的时间
    /// </summary>
    static float TIME_TEAM201_RUN_2_203_Substitute_204_Substitute2_SURROUND_FAIL = 1.2f;

    /// <summary>
    /// 二人小队 转到绕圈连招时，形成包围时时所需的玛狃拉最小夹角（两玛狃拉夹角小于此则包围失败）
    /// </summary>
    static float ANGLE_MIN_TEAM201_RUN_2_205_CIRCLERUN_SURROUND_SUCCESS = 163;
    /// <summary>
    /// 二人小队 转到绕圈连招时，形成包围时时所需的玛狃拉最大夹角（两玛狃拉夹角大于此则包围失败）
    /// </summary>
    static float ANGLE_MAX_TEAM201_RUN_2_205_CIRCLERUN_SURROUND_SUCCESS = 197;
    /// <summary>
    /// 二人小队 巡逻状态 转到绕圈连招需要的形成包围所需的时间
    /// </summary>
    static float TIME_TEAM201_RUN_2_205_CIRCLERUN_SURROUND_SUCCESS = 0.65f;




    /// <summary>
    /// 二人小队 巡逻连招 包围成功计时器
    /// </summary>
    float Timer_TeamState_201_Run_Surround_Success = 0.0f;
    /// <summary>
    /// 二人小队 巡逻连招 包围失败计时器
    /// </summary>
    float Timer_TeamState_201_Run_Surround_Fail = 0.0f;
    /// <summary>
    /// 二人小队 巡逻连招 包围是否成功一次，成果过一次后如何长时间失败转入203或204
    /// </summary>
    bool isSurroundSccessOnceFlg_TeamState201Run = false;
    /// <summary>
    /// 三人小队 巡逻连招转入迷宫连招 检查等级 到达相应的等级后检查一次 并且提升检查等级
    /// </summary>
    int Level_TeamState201Run2TeamState207Maze_MazeCheck = 0;
    /// <summary>
    /// 三人小队 巡逻连招转入迷宫连招的计时器
    /// </summary>
    float Timer_TeamState_201_Run_TeamState201Run2TeamState207Maze = 0.0f;




    /// <summary>
    /// 二人小队_巡逻状态开始
    /// </summary>
    public void TeamState_201_Run_Start()
    {
        ResetWeavileCombatIndex();
        SetWeavileCombatIndex_Random();
        ChangeSubState(SubState.TeamState_201_Run);

        Timer_TeamState_201_Run_Surround_Success = 0.0f;
        Timer_TeamState_201_Run_Surround_Fail = 0.0f;
        isSurroundSccessOnceFlg_TeamState201Run = false;
        Level_TeamState201Run2TeamState207Maze_MazeCheck = 0;
        Timer_TeamState_201_Run_TeamState201Run2TeamState207Maze = 0.0f;
    }

    /// <summary>
    /// 二人小队_巡逻状态结束
    /// </summary>
    public void TeamState_201_Run_Over()
    {

        Timer_TeamState_201_Run_Surround_Success = 0.0f;
        Timer_TeamState_201_Run_Surround_Fail = 0.0f;
        isSurroundSccessOnceFlg_TeamState201Run = false;
        Level_TeamState201Run2TeamState207Maze_MazeCheck = 0;
        Timer_TeamState_201_Run_TeamState201Run2TeamState207Maze = 0.0f;
    }



    //=========================检查是否形成包围============================
    /// <summary>
    /// 检查两个玛狃拉是否形成包围
    /// </summary>
    bool TeamState201_Run_CheckSurroundSccess(Vector3 targetPos)
    {
        if (WeavileMembersList.Count != 2) {
            Debug.Log("Error:二人小队 巡逻状态 小队成员不为2");
            return false;
        }

        //设置角度
        Vector2 v1 = (Vector2)(WeavileMembersList[0].transform.position - targetPos);
        Vector2 v2 = (Vector2)(WeavileMembersList[1].transform.position - targetPos);

        float a1 = _mTool.Angle_360Y(v1, Vector2.right);
        float a2 = _mTool.Angle_360Y(v2, Vector2.right);

        //计算两个角度的最小夹角
        float diff = Mathf.Abs(a1 - a2);
        diff = diff > 180f ? 360f - diff : diff;
        //最小夹角满足包围条件
        return diff >= ANGLE_MIN_TEAM201_RUN_2_205_CIRCLERUN_SURROUND_SUCCESS && diff <= ANGLE_MAX_TEAM201_RUN_2_205_CIRCLERUN_SURROUND_SUCCESS;
    }



    //=========================检查是否形成包围============================




    //=========================检查是否有玛狃拉被接近，并且与目标角度在正方向============================

    /// <summary>
    /// 检查每一只玛狃拉 ， 其中是否有接近目标者
    /// </summary>
    /// <returns></returns>
    Weavile TeamState201_Run_CheckAllWeaileBeClosed()
    {
        if (CheckOneWeaileBeClosed(WeavileMembersList[0])) { return WeavileMembersList[0]; }
        if (CheckOneWeaileBeClosed(WeavileMembersList[1])) { return WeavileMembersList[1]; }
        return null;
    }


    //=========================检查是否有玛狃拉被接近，并且与目标角度在正方向============================




    //○○○○●●○○○○●●○○○○●●二人小队_巡逻状态●●○○○○●●○○○○●●○○○○















    //○○○○●●○○○○●●○○○○●●二人小队_近战连招●●○○○○●●○○○○●●○○○○




    /// <summary>
    /// 二人小队_近战攻击的最大次数
    /// </summary>
    public static int COUNT_TEAMSTATE202_CLOSEATK_MAX = 6;
    /// <summary>
    /// 二人小队_近战攻击的最小次数
    /// </summary>
    public static int COUNT_TEAMSTATE202_CLOSEATK_MIN = 3;
    /// <summary>
    /// 二人小队_近战攻击之间间隔的休息时间 冲刺后的休息时间
    /// </summary>
    public static float TIME_TEAMSTATE202_CLOSEATK_MAX_INTERVAL_RUSH = 0.0f;
    /// <summary>
    /// 二人小队_近战攻击之间间隔的休息时间 跳跃后的休息时间
    /// </summary>
    public static float TIME_TEAMSTATE202_CLOSEATK_MAX_INTERVAL_JUMP = 0.6f;


    public enum TEAMSTATE202_CLOSEATK
    {
        Rush,
        Jump,
    }
    /// <summary>
    /// 三人小队 替身连招 303 副状态
    /// </summary>
    public TEAMSTATE202_CLOSEATK NowTeamState202CloseAtk;




    /// <summary>
    /// 二人小队_近战连招 已经使用近战连招的次数
    /// </summary>
    public int Count_Teamstate202_Close_Atk = 0;
    /// <summary>
    /// 二人小队_近战连招 使用近战连招多次动作之间的休息时间计时器
    /// </summary>
    public float Timer_Teamstate202_CloseAtk_IntervalIdle = 0.0f;
    /// <summary>
    /// 二人小队_近战连招 本次近战连招的预计的使用次数（随机）
    /// </summary>
    public int Count_Teamstate202_CloseAtk_Random = 0;




    /// <summary>
    /// 二人小队_近战连招开始
    /// </summary>
    public void TeamState_202_CloseAtk_Start()
    {
        ChangeSubState(SubState.TeamState_202_CloseAtk);

        NowTeamState202CloseAtk = TEAMSTATE202_CLOSEATK.Rush;

        Count_Teamstate202_Close_Atk = 0;
        Count_Teamstate202_CloseAtk_Random = Random.Range(COUNT_TEAMSTATE202_CLOSEATK_MIN, COUNT_TEAMSTATE202_CLOSEATK_MAX);
        Timer_Teamstate202_CloseAtk_IntervalIdle = 0.0f;
        CombatWeavile01.IsCombatComplite_TeamState202_CloseAtk = false;
        CombatWeavile02.IsCombatComplite_TeamState202_CloseAtk = false;
    }

    /// <summary>
    /// 二人小队_近战连招结束
    /// </summary>
    public void TeamState_202_CloseAtk_Over()
    {
        if (CombatWeavile01 != null) { CombatWeavile01.TeamCombatCompliteEvent_TeamState202_CloseAtk(); }
        if (CombatWeavile02 != null) { CombatWeavile02.TeamCombatCompliteEvent_TeamState202_CloseAtk(); }

        NowTeamState202CloseAtk = TEAMSTATE202_CLOSEATK.Rush;
        Count_Teamstate202_Close_Atk = 0;
        Count_Teamstate202_CloseAtk_Random = 0;
        Timer_Teamstate202_CloseAtk_IntervalIdle = 0.0f;
    }



    //○○○○●●○○○○●●○○○○●●二人小队_近战连招●●○○○○●●○○○○●●○○○○










    //○○○○●●○○○○●●○○○○●●二人小队_替身连招1●●○○○○●●○○○○●●○○○○



    /// <summary>
    /// 二人小队_替身连招 投掷动作之间间隔的休息时间
    /// </summary>
    public static float TIME_TEAMSTATE203SUBSTTITUTE_FLING_IDLEINTERVAL = 0.0f;
    /// <summary>
    /// 二人小队_替身连招 跳跃动作之间间隔的休息时间
    /// </summary>
    public static float TIME_TEAMSTATE203SUBSTTITUTE_JUMP_IDLEINTERVAL = 1.0f;
    /// <summary>
    /// 二人小队_替身连招 保护作之间间隔的休息时间
    /// </summary>
    public static float TIME_TEAMSTATE203SUBSTTITUTE_PROTECT_IDLEINTERVAL = 1.2f;
    /// <summary>
    /// 二人小队_替身连招 毒炸弹动作之间间隔的休息时间
    /// </summary>
    public static float TIME_TEAMSTATE203SUBSTTITUTE_TOXIC_IDLEINTERVAL = 1.2f;


    public enum TEAMSTATE203_SUBSTATE
    {
        Fling,
        Jump,
        Protect,
        Toxic,
        Rush,
    }
    /// <summary>
    /// 三人小队 替身连招 203 副状态
    /// </summary>
    public TEAMSTATE203_SUBSTATE NowTeamState203SubState;



    /// <summary>
    /// 替身实例
    /// </summary>
    public WeavileSubstituteOBJ TeamState203_Substitute_SubstituteOBJ;
    /// <summary>
    /// 使用替身·连招多次动作之间的休息时间计时器
    /// </summary>
    public float Timer_TeamState203_Substitute_IntervalIdle = 0.0f;
    /// <summary>
    /// 替身实例是否被生成
    /// </summary>
    bool IsBorn_TeamState203_Substitute_Substitute = false;
    /// <summary>
    /// 替身连招是被攻击 转出替身连招
    /// </summary>
    public bool Flg_IsBeHIt_TeamState203_Substitute = false;









    /// <summary>
    /// 二人小队_替身连招开始
    /// </summary>
    public void TeamState_203_Substitute_Start()
    {
        NowTeamState203SubState = TEAMSTATE203_SUBSTATE.Fling;
        if (TeamState203_Substitute_SubstituteOBJ != null) { TeamState203_Substitute_SubstituteOBJ = null; }//清空替身实例缓存
        Timer_TeamState203_Substitute_IntervalIdle = 0.0f;
        IsBorn_TeamState203_Substitute_Substitute = false;
        Flg_IsBeHIt_TeamState203_Substitute = false;

        ChangeSubState(SubState.TeamState_203_Substitute);
        //根据距离分配
        SetWeavileCombatIndex_TeamState_203_Substitute(TargetPosition);
    }

    /// <summary>
    /// 二人小队_替身连招结束
    /// </summary>
    public void TeamState_203_Substitute_Over()
    {
        if (CombatWeavile01 != null) { CombatWeavile01.TeamCombatCompliteEvent_TeamState203_Substitue(); }
        if (CombatWeavile02 != null) { CombatWeavile02.TeamCombatCompliteEvent_TeamState203_Substitue(); }

        NowTeamState203SubState = TEAMSTATE203_SUBSTATE.Fling;
        if (TeamState203_Substitute_SubstituteOBJ != null) { TeamState203_Substitute_SubstituteOBJ = null; }//清空替身实例缓存
        Timer_TeamState203_Substitute_IntervalIdle = 0.0f;
        IsBorn_TeamState203_Substitute_Substitute = false;
        Flg_IsBeHIt_TeamState203_Substitute = false;
        ResetWeavileCombatIndex();
    }



    /// <summary>
    /// 设置替身实例
    /// </summary>
    public void TeamState203_Substitute_SetSubstitute(WeavileSubstituteOBJ s)
    {
        TeamState203_Substitute_SubstituteOBJ = s;
        IsBorn_TeamState203_Substitute_Substitute = true;
    }

    /// <summary>
    /// 获取替身的位置
    /// </summary>
    /// <returns></returns>
    public Vector2 TeamState203_Substitute_GetSubstitutePostion
    {
        get
        {
            if (TeamState203_Substitute_SubstituteOBJ != null) { return TeamState203_Substitute_SubstituteOBJ.transform.position; }
            else { return TARGET_POSITION; }
        }
    }




    //○○○○●●○○○○●●○○○○●●二人小队_替身连招1●●○○○○●●○○○○●●○○○○










    //○○○○●●○○○○●●○○○○●●二人小队_替身连招2●●○○○○●●○○○○●●○○○○

    /// <summary>
    /// 二人小队_替身连招2 投掷动作(投掷替身)之间间隔的休息时间
    /// </summary>
    public static float TIME_TEAMSTATE204SUBSTTITUTE2_FLING_IDLEINTERVAL = 0.0f;
    /// <summary>
    /// 二人小队_替身连招2 分身动作之间间隔的休息时间
    /// </summary>
    public static float TIME_TEAMSTATE204SUBSTTITUTE2_CLONESHADOW_IDLEINTERVAL = 0.5f;
    /// <summary>
    /// 二人小队_替身连招2 跳跃动作之间间隔的休息时间
    /// </summary>
    public static float TIME_TEAMSTATE204SUBSTTITUTE2_JUMP_IDLEINTERVAL = 0.5f;
    /// <summary>
    /// 二人小队_替身连招2 炸弹动作之间间隔的休息时间
    /// </summary>
    public static float TIME_TEAMSTATE204SUBSTTITUTE2_BOMB_IDLEINTERVAL = 1.2f;
    /// <summary>
    /// 二人小队_替身连招2 跳跃动作时跳跃到距离替身中心的距离
    /// </summary>
    public static float DISTENCE_TEAMSTATE204SUBSTTITUTE2_JUMP_JUMPBASERADIUS = 7.5f;
    /// <summary>
    /// 二人小队_替身连招2 炸弹动作时投掷炸弹距离替身中心的距离
    /// </summary>
    public static float DISTENCE_TEAMSTATE204SUBSTTITUTE2_JUMP_BOMBBASERADIUS = 2.7f;


    public enum TEAMSTATE204_SUBSTATE
    {
        Fling,
        CloneShadow,
        Jump,
        Bomb,
        Rush,
    }
    /// <summary>
    /// 三人小队 替身连招 204 副状态
    /// </summary>
    public TEAMSTATE204_SUBSTATE NowTeamState204SubState;



    /// <summary>
    /// 替身实例
    /// </summary>
    public WeavileSubstituteOBJ TeamState204_Substitute2_SubstituteOBJ;
    /// <summary>
    /// 使用替身·连招多次动作之间的休息时间计时器
    /// </summary>
    public float Timer_TeamState204_Substitute2_IntervalIdle = 0.0f;
    /// <summary>
    /// 替身实例是否被生成
    /// </summary>
    bool IsBorn_TeamState204_Substitute2_Substitute = false;
    /// <summary>
    /// 替身连招是被攻击 转出替身连招
    /// </summary>
    public bool Flg_IsBeHIt_TeamState204_Substitute2 = false;






    /// <summary>
    /// 二人小队_替身连招2开始
    /// </summary>
    public void TeamState_204_Substitute2_Start()
    {
        NowTeamState204SubState = TEAMSTATE204_SUBSTATE.Fling;
        if (TeamState204_Substitute2_SubstituteOBJ != null) { TeamState204_Substitute2_SubstituteOBJ = null; }//清空替身实例缓存
        Timer_TeamState204_Substitute2_IntervalIdle = 0.0f;
        IsBorn_TeamState204_Substitute2_Substitute = false;
        Flg_IsBeHIt_TeamState204_Substitute2 = false;


        ChangeSubState(SubState.TeamState_204_Substitute2);
        //根据距离分配 复用203
        SetWeavileCombatIndex_TeamState_203_Substitute(TargetPosition);
    }

    /// <summary>
    /// 二人小队_替身连招2结束
    /// </summary>
    public void TeamState_204_Substitute2_Over()
    {
        if (CombatWeavile01 != null) { CombatWeavile01.TeamCombatCompliteEvent_TeamState204_Substitue2(); }
        if (CombatWeavile02 != null) { CombatWeavile02.TeamCombatCompliteEvent_TeamState204_Substitue2(); }

        NowTeamState204SubState = TEAMSTATE204_SUBSTATE.Fling;
        if (TeamState204_Substitute2_SubstituteOBJ != null) { TeamState204_Substitute2_SubstituteOBJ = null; }//清空替身实例缓存
        Timer_TeamState204_Substitute2_IntervalIdle = 0.0f;
        IsBorn_TeamState204_Substitute2_Substitute = false;
        Flg_IsBeHIt_TeamState204_Substitute2 = false;
        ResetWeavileCombatIndex();
    }



    /// <summary>
    /// 设置替身实例
    /// </summary>
    public void TeamState204_Substitute2_SetSubstitute(WeavileSubstituteOBJ s)
    {
        TeamState204_Substitute2_SubstituteOBJ = s;
        IsBorn_TeamState204_Substitute2_Substitute = true;
    }

    /// <summary>
    /// 获取替身的位置
    /// </summary>
    /// <returns></returns>
    public Vector2 TeamState204_Substitute2_GetSubstitutePostion
    {
        get
        {
            if (TeamState204_Substitute2_SubstituteOBJ != null) { return TeamState204_Substitute2_SubstituteOBJ.transform.position; }
            else { return TARGET_POSITION; }
        }
    }



    /// <summary>
    /// 二人小队 替身连招2 根据当前圆心和半径以及房间上下左右边界 确定在房间内的有效弧度 并且根据count均分这些有效弧度 输出被均分后的弧的中心
    /// </summary>
    /// <param name="center">圆心</param>
    /// <param name="Radiu">半径</param>
    /// <param name="Count">分割数</param>
    /// <param name="RoomBoardUp">房间上边界</param>
    /// <param name="RoomBoardDown">房间下边界</param>
    /// <param name="RoomBoardLeft">房间左边界</param>
    /// <param name="RoomBoardRight">房间右边界</param>
    /// <returns></returns>
    public Vector2 TeamState204Substitute2_GetJumpTargetPosition(
        Vector2 center, float Radiu, int Count,
        float RoomBoardUp, float RoomBoardDown, float RoomBoardLeft, float RoomBoardRight,
        Transform self)
    {
        List<float> resultList = new List<float>();

        // 1. 找出所有有效弧段（0-360）
        List<(float start, float end)> validArcs = new List<(float, float)>();

        // 扫描 0~360，每 1° 判断该点是否在房间内
        bool lastInside = false;
        float arcStart = 0f;

        for (int ang = 0; ang <= 360; ang++)
        {
            float rad = ang * Mathf.Deg2Rad;
            Vector2 p = center + new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)) * Radiu;

            bool inside =
                p.x >= RoomBoardLeft &&
                p.x <= RoomBoardRight &&
                p.y >= RoomBoardDown &&
                p.y <= RoomBoardUp;

            if (inside && !lastInside)
            {
                arcStart = ang;
            }
            else if (!inside && lastInside)
            {
                validArcs.Add((arcStart, ang));
            }

            lastInside = inside;
        }

        // 如果最后仍在 inside，则补上
        if (lastInside)
            validArcs.Add((arcStart, 360f));

        // 2. 计算有效弧度总长
        float totalArc = 0f;
        foreach (var arc in validArcs)
            totalArc += arc.end - arc.start;

        if (totalArc <= 0.01f)
            return self.transform.position;

        // 每段长度
        float segmentLen = totalArc / Count;

        // 3. 对每一段求中心点
        for (int i = 0; i < Count; i++)
        {
            float targetPos = segmentLen * i + segmentLen * 0.5f; // 该段中心在有效弧度中的“累计长度位置”

            float accumulated = 0f;
            float angle = 0f;

            foreach (var arc in validArcs)
            {
                float arcLen = arc.end - arc.start;

                if (accumulated + arcLen >= targetPos)
                {
                    float offset = targetPos - accumulated;
                    angle = arc.start + offset;
                    break;
                }

                accumulated += arcLen;
            }

            resultList.Add(angle);
        }

        if (WeavileAndCloneShadowList.Count != resultList.Count) { Debug.Log("Error:谜之错误 理论不存在"); }
        // 结果角度
        float rAngle = 0.0f;
        //return resultList;
        for(int i = 0; i < WeavileAndCloneShadowList.Count; i++)
        {
            //输出目标位置
            if (WeavileAndCloneShadowList[i].transform.gameObject.GetInstanceID() == self.transform.gameObject.GetInstanceID())
            {
                rAngle = resultList[i];
            }
        }
        Vector2 result = (Vector2)(Quaternion.AngleAxis(rAngle, Vector3.forward) * Vector2.right * Radiu) + center;
        return result;
    }



    //○○○○●●○○○○●●○○○○●●二人小队_替身连招2●●○○○○●●○○○○●●○○○○










    //○○○○●●○○○○●●○○○○●●二人小队_绕圈连招●●○○○○●●○○○○●●○○○○


    /// <summary>
    /// 二人小队绕圈连招 外环攻击 攻击最大次数
    /// </summary>
    public static float COUNT_TEAMSTATE_205_CIRCLERUN_OUTERATK_MAX = 2;





    /// <summary>
    /// 二人连招 绕圈连招 副状态
    /// </summary>
    public TEAMSTATE306_SUBSTATE NowTeamState205SubState;

    /// <summary>
    /// 绕圈跑的中心
    /// </summary>
    public Vector2 Position_Center_TeamState_205_CircleRun = Vector2.zero;
    /// <summary>
    /// 内环攻击触发时间计时器 会随着时间累计
    /// </summary>
    float Timer_TeamState205CircleRun_Inner_Atk = 0.0f;
    /// <summary>
    /// 外环攻击触发时间计时器 进入内环后清空
    /// </summary>
    float Timer_TeamState205CircleRun_Outer_Atk = 0.0f;
    /// <summary>
    /// 二人小队 绕圈连招 外环攻击进行的次数
    /// </summary>
    public int Count_TeamState205CircleRun_OuterAtk = 0;
    /// <summary>
    /// 二人小队 绕圈连招 脱离攻击 投掷的次数
    /// </summary>
    public int Count_Fling_TeamState205CircleRun_EscapeAtk = 0;
    /// <summary>
    /// 逃离攻击触发时间计时器 进入内环或外环后清空
    /// </summary>
    float Timer_TeamState205CircleRun_Escape_Atk = 0.0f;
    /// <summary>
    /// 二人小队绕圈连招 使用攻击连招多次动作之间的休息时间计时器
    /// </summary>
    public float Timer_TeamState205CircleRun_Atk_IntervalIdle = 0.0f;






    /// <summary>
    /// 二人小队_绕圈连招开始
    /// </summary>
    public void TeamState_205_CircleRun_Start()
    {
        WeavileAndCloneShadowList.Clear();
        Position_Center_TeamState_205_CircleRun = Vector2.zero;
        Timer_TeamState205CircleRun_Inner_Atk = 0.0f;
        Timer_TeamState205CircleRun_Outer_Atk = 0.0f;
        Count_TeamState205CircleRun_OuterAtk = 0;
        Timer_TeamState205CircleRun_Escape_Atk = 0.0f;
        Timer_TeamState205CircleRun_Atk_IntervalIdle = 0.0f;
        Count_Fling_TeamState205CircleRun_EscapeAtk = 0;
        NowTeamState205SubState = TEAMSTATE306_SUBSTATE.Run;

        ResetWeavileCombatIndex();
        SetWeavileCombatIndex_Random();
        SetCircleRunCenter_Teamstate205CirclrRun_FirstTime();
        ChangeSubState(SubState.TeamState_205_CircleRun);
    }

    /// <summary>
    /// 二人小队_绕圈连招结束
    /// </summary>
    public void TeamState_205_CircleRun_Over()
    {
        switch (NowTeamState205SubState)
        {
            //外环攻击
            case TEAMSTATE306_SUBSTATE.OuterAtk:
                if (CombatWeavile01 != null) { CombatWeavile01.TeamCombatCompliteEvent_TeamState205_CircleRun_OuterAtk(); }
                if (CombatWeavile02 != null) { CombatWeavile02.TeamCombatCompliteEvent_TeamState205_CircleRun_OuterAtk(); }
                break;
            //脱离攻击
            case TEAMSTATE306_SUBSTATE.EscapeAtk:
                if (CombatWeavile01 != null) { CombatWeavile01.TeamCombatCompliteEvent_TeamState205_CircleRun_EscapeAtk(); }
                if (CombatWeavile02 != null) { CombatWeavile02.TeamCombatCompliteEvent_TeamState205_CircleRun_EscapeAtk(); }
                break;
            default:
                break;
        }



        WeavileAndCloneShadowList.Clear();
        Position_Center_TeamState_205_CircleRun = Vector2.zero;
        Timer_TeamState205CircleRun_Inner_Atk = 0.0f;
        Timer_TeamState205CircleRun_Outer_Atk = 0.0f;
        Count_TeamState205CircleRun_OuterAtk = 0;
        Timer_TeamState205CircleRun_Escape_Atk = 0.0f;
        Timer_TeamState205CircleRun_Atk_IntervalIdle = 0.0f;
        Count_Fling_TeamState205CircleRun_EscapeAtk = 0;
        NowTeamState205SubState = TEAMSTATE306_SUBSTATE.Run;
        ResetWeavileCombatIndex();
    }



    /// <summary>
    /// 设置绕圈跑中心 首次为两只玛狃拉的中心点
    /// </summary>
    void SetCircleRunCenter_Teamstate205CirclrRun_FirstTime()
    {
        //设置奔跑中心
        Position_Center_TeamState_205_CircleRun = new Vector2(
            ((WeavileMembersList[0].transform.position.x + WeavileMembersList[1].transform.position.x ) / 2.0f),
            ((WeavileMembersList[0].transform.position.x + WeavileMembersList[1].transform.position.x ) / 2.0f));
        float r = RADIUS_TEAMSTATE_306_CIRCLERUN + 1.0f;
        Position_Center_TeamState_205_CircleRun = new Vector2(
            Mathf.Clamp(Position_Center_TeamState_205_CircleRun.x,                    //方向*速度
                WeavileMembersList[0].ParentPokemonRoom.RoomSize[2] + r + transform.parent.position.x, //最小值
                WeavileMembersList[0].ParentPokemonRoom.RoomSize[3] - r + transform.parent.position.x),//最大值
            Mathf.Clamp(Position_Center_TeamState_205_CircleRun.y,                     //方向*速度 
                WeavileMembersList[0].ParentPokemonRoom.RoomSize[1] + r + transform.parent.position.y,  //最小值
                WeavileMembersList[0].ParentPokemonRoom.RoomSize[0] - r + transform.parent.position.y));//最大值
        Debug.Log(Position_Center_TeamState_205_CircleRun);
    }

    /// <summary>
    /// 设置绕圈跑中心 第二次开始为以目标点为中心
    /// </summary>
    void SetCircleRunCenter_Teamstate205CirclrRun_SecondTime()
    {
        //设置奔跑中心
        Position_Center_TeamState_205_CircleRun = TargetPosition;
        float r = RADIUS_TEAMSTATE_306_CIRCLERUN + 1.0f;
        Position_Center_TeamState_205_CircleRun = new Vector2(
            Mathf.Clamp(Position_Center_TeamState_205_CircleRun.x,                    //方向*速度
                WeavileMembersList[0].ParentPokemonRoom.RoomSize[2] + r + transform.parent.position.x, //最小值
                WeavileMembersList[0].ParentPokemonRoom.RoomSize[3] - r + transform.parent.position.x),//最大值
            Mathf.Clamp(Position_Center_TeamState_205_CircleRun.y,                     //方向*速度 
                WeavileMembersList[0].ParentPokemonRoom.RoomSize[1] + r + transform.parent.position.y,  //最小值
                WeavileMembersList[0].ParentPokemonRoom.RoomSize[0] - r + transform.parent.position.y));//最大值
        Debug.Log(Position_Center_TeamState_205_CircleRun);
    }


    /// <summary>
    /// 根据角度排序玛狃拉和残影
    /// </summary>
    void SortWeavileAndCloneBodtyByAngle_TeamState205CircleRun()
    {
        // 排空
        _mTool.RemoveNullInList<Transform>(WeavileAndCloneShadowList);

        // 临时列表：存储 Transform + 角度
        List<(Transform tf, float angle)> temp = new List<(Transform, float)>();

        for (int i = 0; i < WeavileAndCloneShadowList.Count; i++)
        {
            Transform t = WeavileAndCloneShadowList[i];

            // 计算相对中心的方向
            Vector2 dir = (Vector2)t.position - Position_Center_TeamState_205_CircleRun;

            // 计算 0~360° 角度
            float angle = _mTool.Angle_360Y(dir, Vector2.right);

            temp.Add((t, angle));
        }

        // 按角度排序
        temp.Sort((a, b) => a.angle.CompareTo(b.angle));

        // 回写排序后的 Transform 列表
        WeavileAndCloneShadowList.Clear();
        for (int i = 0; i < temp.Count; i++)
        {
            WeavileAndCloneShadowList.Add(temp[i].tf);
        }

        // 排序后进行速度调整
        AdjustSpeedByAngle(temp);
    }


    //○○○○●●○○○○●●○○○○●●二人小队_绕圈连招●●○○○○●●○○○○●●○○○○










    //○○○○●●○○○○●●○○○○●●二人小队_回血连招●●○○○○●●○○○○●●○○○○



    /// <summary>
    /// 二人小队 回血连招 使用后的冷却时间
    /// </summary>
    static float TIME_TEAMSTATE206_HEAL_CD = 40.0f;
    /// <summary>
    /// 二人小队 回血连招 触发连招的血线
    /// </summary>
    static float HPLINE_TEAMSYAYE206HEAL_CONDITION = 0.3f;




    /// <summary>
    /// 回复是否结束
    /// </summary>
    public bool isOver_Heal_TeamState206Heal;




    /// <summary>
    /// 二人小队_回血连招开始
    /// </summary>
    public void TeamState_206_Heal_Start()
    {
        ChangeSubState(SubState.TeamState_206_Heal);
        isOver_Heal_TeamState206Heal = false;
    }

    /// <summary>
    /// 二人小队_回血连招结束
    /// </summary>
    public void TeamState_206_Heal_Over()
    {
        if (CombatWeavile01 != null) { CombatWeavile01.TeamCombatCompliteEvent_TeamState206_Heal(); }
        if (CombatWeavile02 != null) { CombatWeavile02.TeamCombatCompliteEvent_TeamState206_Heal(); }

        isOver_Heal_TeamState206Heal = false;
    }




    /// <summary>
    /// 是否达成进入回血连招1的条件
    /// </summary>
    /// <returns></returns>
    public bool Condition_TeamState206_Heal()
    {
        if (WeavileMembersList.Count != 2)
        {
            Debug.Log("Error:二人小队 回血连招 总玛狃拉列表人数错误");
            return false;
        }

        List<float> hpList = new List<float>()
        {
            WeavileMembersList[0].GetHPPercent,
            WeavileMembersList[1].GetHPPercent,
        };

        //Debug.Log(hpList[0]) ;
        //Debug.Log(hpList[1]) ;

        // 取最大值
        float max = Mathf.Max(hpList[0], hpList[1]);

        // 找出所有不是最大值的项（剩余一个）
        //List<float> others = hpList.Where(h => h != max).ToList();
        hpList.Remove(max);

        // 判断剩余一个是否都满足差值条件
        foreach (float h in hpList)
        {
            //Debug.Log(h);
            if (max - h <= HPLINE_TEAMSYAYE206HEAL_CONDITION)
                return false;
        }

        return true;


    }



    //○○○○●●○○○○●●○○○○●●二人小队_回血连招●●○○○○●●○○○○●●○○○○

















    //○○○○●●○○○○●●○○○○●●二人小队_迷宫连招●●○○○○●●○○○○●●○○○○



    /// <summary>
    /// 迷宫是否结束
    /// </summary>
    public bool isOver_Maze_TeamState207Maze = false;


    /// <summary>
    /// 二人小队_迷宫连招开始
    /// </summary>
    public void TeamState_207_Maze_Start()
    {
        isOver_Maze_TeamState207Maze = false;

        ChangeSubState(SubState.TeamState_207_Maze);

        //根据距离分配玛狃拉
        SetWeavileCombatIndex_TeamState_203_Substitute(TargetPosition);
    }

    /// <summary>
    /// 二人小队_迷宫连招结束
    /// </summary>
    public void TeamState_207_Maze_Over()
    {
        //转入下一个状态
        if (CombatWeavile01 != null) { CombatWeavile01.TeamCombatCompliteEvent_TeamState207_Mazes(); }
        if (CombatWeavile02 != null) { CombatWeavile02.TeamCombatCompliteEvent_TeamState207_Mazes(); }

        isOver_Maze_TeamState207Maze = false;
    }


    //○○○○●●○○○○●●○○○○●●二人小队_迷宫连招●●○○○○●●○○○○●●○○○○
















    //○○○○●●○○○○●●○○○○●●二人小队_万灵药连招●●○○○○●●○○○○●●○○○○


    /// <summary>
    /// 二人小队 万灵药连招的子状态
    /// </summary>
    public TEAMSTATE309_SUBSTATE NowTeamState208SubState ;


    /// <summary>
    /// 二人小队_万灵药连招开始
    /// </summary>
    public void TeamState_208_FullHeal_Start()
    {
        NowTeamState208SubState = TEAMSTATE309_SUBSTATE.Heal;
        ChangeSubState(SubState.TeamState_208_FullHeal);
    }

    /// <summary>
    /// 二人小队_万灵药连招结束
    /// </summary>
    public void TeamState_208_FullHeal_Over()
    {
        if (CombatWeavile01 != null) { CombatWeavile01.TeamCombatCompliteEvent_TeamState208_FullHeal(); }
        if (CombatWeavile02 != null) { CombatWeavile02.TeamCombatCompliteEvent_TeamState208_FullHeal(); }

        NowTeamState208SubState = TEAMSTATE309_SUBSTATE.Heal;
    }


    //○○○○●●○○○○●●○○○○●●二人小队_万灵药招●●○○○○●●○○○○●●○○○○



















    //□□□■■■□□□■■■□□□■■■主状态机：二人小队■■■□□□■■■□□□■■■□□□
















    //□□□■■■□□□■■■□□□■■■主状态机：一人小队■■■□□□■■■□□□■■■□□□


    //○○○○●●○○○○●●○○○○●●一人小队_通用变量●●○○○○●●○○○○●●○○○○
    //○○○○●●○○○○●●○○○○●●一人小队_通用变量●●○○○○●●○○○○●●○○○○




    //○○○○●●○○○○●●○○○○●●一人小队_巡逻状态●●○○○○●●○○○○●●○○○○






    public enum TEAMSTATE100_SUBSTATE
    {
        FlingSmoke,
        RunToSmoke,
        RunInSmoke,
        GuardInSmoke,
        Rush,
        FlingBomb,
        Idle,
    }
    /// <summary>
    /// 三人小队 替身连招 204 副状态
    /// </summary>
    public TEAMSTATE100_SUBSTATE NowTeamState100SubState;



    /// <summary>
    /// 一人小队_巡逻状态开始
    /// </summary>
    public void TeamState_100_Normal_Start()
    {
        NowTeamState100SubState = TEAMSTATE100_SUBSTATE.FlingSmoke;

        ChangeSubState(SubState.TeamState_100_Normal);


    }

    /// <summary>
    /// 一人小队_巡逻状态结束
    /// </summary>
    public void TeamState_100_Normal_Over()
    {
        NowTeamState100SubState = TEAMSTATE100_SUBSTATE.FlingSmoke;

    }



    //○○○○●●○○○○●●○○○○●●一人小队_巡逻状态●●○○○○●●○○○○●●○○○○


    //□□□■■■□□□■■■□□□■■■主状态机：一人小队■■■□□□■■■□□□■■■□□□



    //■■■■■■■■■■■■■■■■■■■■状态机事件■■■■■■■■■■■■■■■■■■■■■■










}

























