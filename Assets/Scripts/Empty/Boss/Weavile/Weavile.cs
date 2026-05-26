using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weavile : Empty
{
    /// <summary>
    /// 敌人朝向
    /// </summary>
    Vector2 Director;


    /// <summary>
    /// 计算当前速度,朝向时，采用的上一时间单位的位置坐标,通过携程执行
    /// </summary>
    Vector3 LastPosition;

    /// <summary>
    /// Sprite所在的Transform 跳跃时生成残影用
    /// </summary>
    public Transform SpriteTransform;


    /// <summary>
    /// 敌人的目标的坐标
    /// </summary>
    public Vector2 TARGET_POSITION
    {
        get { return TargetPosition; }
        set { TargetPosition = value; }
    }
    Vector2 TargetPosition;

    /// <summary>
    /// 获取血量百分比
    /// </summary>
    public float GetHPPercent
    {
        get { return ((float)EmptyHp) / ((float)maxHP); }
    }



    //■■■■■■■■■■■■■■■■■■■■状态机枚举■■■■■■■■■■■■■■■■■■■■■■

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


    //■■■■■■■■■■■■■■■■■■■■状态机枚举■■■■■■■■■■■■■■■■■■■■■■











    //■■■■■■■■■■■■■■■■■■■■玛狃拉小队主脑■■■■■■■■■■■■■■■■■■■■■■

    /// <summary>
    /// 小队主脑
    /// </summary>
    public WeavileTeamBrain TeamBrain;


    /// <summary>
    /// 小队行动编号
    /// </summary>
    public int TeamCombatActIndex
    {
        get { return teamCombatActIndex; }
        set { teamCombatActIndex = value; }
    }
    public int teamCombatActIndex = 0;




    /// <summary>
    /// 小队连招 全员默认设为发呆状态 清空所有已完成动作
    /// </summary>
    public void TeamCombatReset_Default()
    {
        //所有动作完成情况设为false
        isCombatComplite_TeamState302_CloseAtk = false;
        isCombatComplite_TeamState303_Substitute = false;
        isCombatComplite_TeamState304_CloneShadow = false;
        isCombatComplite_TeamState305_Maze = false;
        isCombatComplite_TeamState306_CirclrRun_InnerAtk = false;
        isCombatComplite_TeamState306_CircleRun_OuterAtk = false;
        isCombatComplite_TeamState306_CircleRun_EscapeAtk = false;
        isCombatComplite_TeamState307_Heal = false;
        isCombatComplite_TeamState308_Heal2 = false;
        isCombatComplite_TeamState309_FullHeal = false;

        isCombatComplite_TeamState202_CloseAtk = false;
        isCombatComplite_TeamState203_Substitute = false;
        isCombatComplite_TeamState204_Substitute2 = false;
        isCombatComplite_TeamState205_CirclrRun_InnerAtk = false;
        isCombatComplite_TeamState205_CircleRun_OuterAtk = false;
        isCombatComplite_TeamState205_CircleRun_EscapeAtk = false;
        isCombatComplite_TeamState206_Heal = false;
        isCombatComplite_TeamState207_Maze = false;
        isCombatComplite_TeamState208_FullHeal = false;

        //默认发呆
        IdleStart(TIME_IDLE_START);
        //清空所有分身
        _mTool.RemoveNullInList<WeavileCloneBody>(CloneBodyList);
        foreach (WeavileCloneBody b in CloneBodyList)
        {
            b.SetCloneShadowOver();
        }
    }





    //=========================三人小队 近战连招============================

    /// <summary>
    /// 三人小队_近战连招 连招是否完成
    /// </summary>
    public bool IsCombatComplite_TeamState302_CloseAtk
    {
        get { return isCombatComplite_TeamState302_CloseAtk; }
        set { isCombatComplite_TeamState302_CloseAtk = value; }
    }
    bool isCombatComplite_TeamState302_CloseAtk = false;

    /// <summary>
    /// 三人小队_近战连招 团队成员连招动作都完成时，进行下一次连招的事件
    /// </summary>
    public void TeamCombatActOverEvent_TeamState302_CloseAtk()
    {
        if (TeamBrain.NowSubState == WeavileTeamBrain.SubState.TeamState_302_CloseAtk) {
            //设置下一次连招动作为未完成
            isCombatComplite_TeamState302_CloseAtk = false;
            switch (TeamCombatActIndex)
            {
                case 1:
                    RushStart();
                    break;
                case 2:
                    JumpMoveStart();
                    break;
                case 3:
                    UseProtectStart(TIME_USEPROTECT_TEMASTATE302_CLOSEATK);
                    break;
            }
        }
    }

    /// <summary>
    /// 三人小队_近战连招 连招彻底结束，转入下一个团队连招
    /// </summary>
    public void TeamCombatCompliteEvent_TeamState302_CloseAtk()
    {
        if (TeamBrain.NowSubState == WeavileTeamBrain.SubState.TeamState_302_CloseAtk)
        {
            //设置下一次连招动作为未完成
            isCombatComplite_TeamState302_CloseAtk = false;
            switch (TeamCombatActIndex)
            {
                case 1:
                    IdleStart(TIME_IDLE_TEAMSTATE302_CLOSEATK_OVER_01RUSH);
                    break;
                case 2:
                    IdleStart(TIME_IDLE_TEAMSTATE302_CLOSEATK_OVER_02JUMP);
                    break;
                case 3:
                    IdleStart(TIME_IDLE_TEAMSTATE302_CLOSEATK_OVER_03USEPROTECT);
                    break;
            }
        }
    }

    //=========================三人小队 近战连招============================












    //=========================三人小队 替身连招============================

    /// <summary>
    /// 三人小队_替身连招 连招动作是否完成
    /// </summary>
    public bool IsCombatComplite_TeamState303_Substitute
    {
        get { return isCombatComplite_TeamState303_Substitute; }
        set { isCombatComplite_TeamState303_Substitute = value; }
    }
    bool isCombatComplite_TeamState303_Substitute = false;

    /// <summary>
    /// 三人小队_替身连招 团队成员连招动作都完成时，进行下一次连招的事件
    /// </summary>
    public void TeamCombatActOverEvent_TeamState303_Substitute()
    {
        if (TeamBrain.NowSubState == WeavileTeamBrain.SubState.TeamState_303_Substitute)
        {
            //设置下一次连招动作为未完成
            isCombatComplite_TeamState303_Substitute = false;
            //根据当前主脑副状态决定下一步连招
            switch (TeamBrain.NowTeamState303SubState)
            {
                //当前为投掷 状态机错误
                case WeavileTeamBrain.TEAMSTATE303_SUBSTATE.Fling:
                    Debug.Log("Error 303替身连招 状态机错误");
                    //强制修改为跳跃阶段 跳跃开始 跳跃至房间角落
                    TeamBrain.NowTeamState303SubState = WeavileTeamBrain.TEAMSTATE303_SUBSTATE.Jump;
                    JumpMoveStart();
                    break;
                case WeavileTeamBrain.TEAMSTATE303_SUBSTATE.Jump:
                    //强制修改为跳跃阶段 跳跃开始 跳跃至房间角落
                    JumpMoveStart();
                    break;
                case WeavileTeamBrain.TEAMSTATE303_SUBSTATE.Protect:
                    switch (TeamCombatActIndex) {
                        case 1: //直接转入发呆
                            isCombatComplite_TeamState303_Substitute = true;
                            break;
                        case 2: //使用保护
                            UseProtectStart(TIME_USEPROTECT_TEMASTATE302_CLOSEATK);
                            break;
                        case 3: //有大概率转入发呆 小概率转入投掷炸弹 
                            if (Random.Range(0.0f, 1.0f) < 0.75f)
                            { isCombatComplite_TeamState303_Substitute = true; }
                            else
                            { FlingStart(); }
                            break;
                    }
                    break;
                case WeavileTeamBrain.TEAMSTATE303_SUBSTATE.Toxic:
                    switch (TeamCombatActIndex)
                    {
                        case 1: //有大概率投掷伤药 有小概率使用保护
                            if (Random.Range(0.0f, 1.0f) < 0.75f)
                            { FlingStart(); }
                            else
                            { UseProtectStart(TIME_USEPROTECT_TEMASTATE302_CLOSEATK); }
                            break;
                        case 2: //直接转入发呆
                            isCombatComplite_TeamState303_Substitute = true;
                            break;
                        case 3: //转入投掷炸弹
                            FlingStart();
                            break;
                    }
                    break;
                case WeavileTeamBrain.TEAMSTATE303_SUBSTATE.Rush:
                    //开始冲刺
                    RushStart();
                    break;
            }
        }
    }

    /// <summary>
    /// 三人小队_替身连招 连招彻底结束，转入下一个团队连招
    /// </summary>
    public void TeamCombatCompliteEvent_TeamState303_Substitue()
    {
        if (TeamBrain.NowSubState == WeavileTeamBrain.SubState.TeamState_303_Substitute)
        {
            //设置下一次连招动作为未完成
            isCombatComplite_TeamState303_Substitute = false;
            switch (teamCombatActIndex)
            {
                case 1: IdleStart(TIME_IDLE_TEAMSTATE303_SUBSTITUTE_OVER_01RUSH); break;
                case 2: IdleStart(TIME_IDLE_TEAMSTATE303_SUBSTITUTE_OVER_02RUSH); break;
                case 3: IdleStart(TIME_IDLE_TEAMSTATE303_SUBSTITUTE_OVER_03RUSH); break;
            }
        }
    }

    //=========================三人小队 替身连招============================











    //=========================三人小队 分身连招============================

    /// <summary>
    /// 检测全体残影和本体是否完成304的动作 有一人未完成 则不为真
    /// </summary>
    /// <returns></returns>
    public bool BodyAndClone_IsCombatComplite_TeamState304_CloneShadow()
    {
        //先排空
        _mTool.RemoveNullInList<WeavileCloneBody>(CloneBodyList);

        //所有分身是否已完成 如果没有分身则已完成为true
        bool isAllCloneComplite = true;
        for (int i = 0; i < CloneBodyList.Count; i++)
        {
            //有一个残影未完成 则全体未完成
            if (CloneBodyList[i] != null && !CloneBodyList[i].IsCombatComplite_TeamState304_CloneShadow)
            {
                isAllCloneComplite = false;
                break;
            }
        }
        //返回全体残影和本体的完成情况
        return isAllCloneComplite && IsCombatComplite_TeamState304_CloneShadow;
    }



    /// <summary>
    /// 三人小队_分身连招 连招动作是否完成
    /// </summary>
    public bool IsCombatComplite_TeamState304_CloneShadow
    {
        get { return isCombatComplite_TeamState304_CloneShadow; }
        set { isCombatComplite_TeamState304_CloneShadow = value; }
    }
    bool isCombatComplite_TeamState304_CloneShadow = false;

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
                    IceShardStart(TIME_ICESHARD_TEMASTATE304IDLE_JUMP);
                    break;
                //当前为终结冲刺
                case WeavileTeamBrain.TEAMSTATE304_CLONESHADOW.OverRush:
                    RushStart();
                    break;
                //当前为意外冲刺
                case WeavileTeamBrain.TEAMSTATE304_CLONESHADOW.AccidentRush:
                    RushStart();
                    break;
            }


            
            //先排空
            _mTool.RemoveNullInList<WeavileCloneBody>(CloneBodyList);
            //让旗下所有残影执行下一次动作
            for (int i = 0; i < CloneBodyList.Count; i++)
            {
                CloneBodyList[i].TeamCombatActOverEvent_TeamState304_CloneShadow();
            }
        }
    }

    /// <summary>
    /// 三人小队_分身连招 连招彻底结束，转入下一个团队连招
    /// </summary>
    public void TeamCombatCompliteEvent_TeamState304_CloneShadow()
    {
        if (TeamBrain.NowSubState == WeavileTeamBrain.SubState.TeamState_304_CloneShadow)
        {
            //设置下一次连招动作为未完成
            isCombatComplite_TeamState304_CloneShadow = false;
            switch (TeamBrain.NowTeamState304CloneShadow)
            {
                //终结冲刺后 正常休息
                case WeavileTeamBrain.TEAMSTATE304_CLONESHADOW.OverRush:
                    IdleStart(TIME_IDLE_TEAMSTATE304_CLONESHADOW_OVERRUSH);
                    break;
                //意外冲刺后 分别休息
                case WeavileTeamBrain.TEAMSTATE304_CLONESHADOW.AccidentRush:
                    switch (teamCombatActIndex)
                    {
                        case 1: IdleStart(TIME_IDLE_TEAMSTATE304_CLONESHADOW_ADDCIENTRUSH_01RUSH); break;
                        case 2: IdleStart(TIME_IDLE_TEAMSTATE304_CLONESHADOW_ADDCIENTRUSH_02RUSH); break;
                        case 3: IdleStart(TIME_IDLE_TEAMSTATE304_CLONESHADOW_ADDCIENTRUSH_03RUSH); break;
                    }
                    break;
                //其他状态 状态机错误 默认休息
                default:
                    IdleStart(TIME_IDLE_START);
                    break;
            }

            
            
        }
    }

    //=========================三人小队 分身连招============================















    //=========================三人小队 迷宫连招============================



    /// <summary>
    /// 三人小队_迷宫连招 连招动作是否完成
    /// </summary>
    public bool IsCombatComplite_TeamState305_Maze
    {
        get { return isCombatComplite_TeamState305_Maze; }
        set { isCombatComplite_TeamState305_Maze = value; }
    }
    public bool isCombatComplite_TeamState305_Maze = false;

    /// <summary>
    /// 三人小队_迷宫连招 连招彻底结束，转入下一个团队连招
    /// </summary>
    public void TeamCombatCompliteEvent_TeamState305_Mazes()
    {
        if (TeamBrain.NowSubState == WeavileTeamBrain.SubState.TeamState_305_Maze)
        {
            //设置下一次连招动作为未完成
            isCombatComplite_TeamState305_Maze = false;
            //三人小队迷宫连招首次投掷重置为错
            isOver_TeamState305Maze_FirstFling = false;

            switch (teamCombatActIndex)
            {
                case 1: IdleStart(TIME_IDLE_TEAMSTATE305_MAZE_01MAZE); break;
                case 2: IdleStart(TIME_IDLE_TEAMSTATE305_MAZE_0203MAZEWATCH); break;
                case 3: IdleStart(TIME_IDLE_TEAMSTATE305_MAZE_0203MAZEWATCH); break;
            }
        }
    }

    //=========================三人小队 迷宫连招============================













    //=========================三人小队 绕圈连招============================



    /// <summary>
    /// 三人小队_绕圈连招 内环攻击 连招动作是否完成
    /// </summary>
    public bool IsCombatComplite_TeamState306_CircleRun_InnerAtk
    {
        get { return isCombatComplite_TeamState306_CirclrRun_InnerAtk; }
        set { isCombatComplite_TeamState306_CirclrRun_InnerAtk = value; }
    }
    bool isCombatComplite_TeamState306_CirclrRun_InnerAtk = false;
    /// <summary>
    /// 三人小队_绕圈连招 外环攻击 连招动作是否完成
    /// </summary>
    public bool IsCombatComplite_TeamState306_CircleRun_OuterAtk
    {
        get { return isCombatComplite_TeamState306_CircleRun_OuterAtk; }
        set { isCombatComplite_TeamState306_CircleRun_OuterAtk = value; }
    }
    bool isCombatComplite_TeamState306_CircleRun_OuterAtk = false;
    /// <summary>
    /// 三人小队_绕圈连招 脱离攻击 连招动作是否完成
    /// </summary>
    public bool IsCombatComplite_TeamState306_CircleRun_EscapeAtk
    {
        get { return isCombatComplite_TeamState306_CircleRun_EscapeAtk; }
        set { isCombatComplite_TeamState306_CircleRun_EscapeAtk = value; }
    }
    bool isCombatComplite_TeamState306_CircleRun_EscapeAtk = false;


    /// <summary>
    /// 三人小队_绕圈连招 内环攻击 团队成员连招动作都完成时，进行下一次连招的事件
    /// </summary>
    public void TeamCombatActOverEvent_TeamState306_CircleRun_InnerAtk()
    {
        if (TeamBrain.NowSubState == WeavileTeamBrain.SubState.TeamState_306_CircleRun && TeamBrain.NowTeamState306SubState == WeavileTeamBrain.TEAMSTATE306_SUBSTATE.Run)
        {
            isCombatComplite_TeamState306_CirclrRun_InnerAtk = false;
            CloneShadowStart(TIME_CLONESHADOW_TEAMSTATE_306_CIRCLERUN_IDLE);
        }
    }


    /// <summary>
    /// 三人小队_绕圈连招 外环攻击 团队成员连招动作都完成时，进行下一次连招的事件
    /// </summary>
    public void TeamCombatActOverEvent_TeamState306_CircleRun_OuterAtk()
    {
        if (TeamBrain.NowSubState == WeavileTeamBrain.SubState.TeamState_306_CircleRun && TeamBrain.NowTeamState306SubState == WeavileTeamBrain.TEAMSTATE306_SUBSTATE.OuterAtk)
        {
            //设置下一次连招动作为未完成
            isCombatComplite_TeamState306_CircleRun_OuterAtk = false;
            //自己的攻击序列号小于等于外环攻击次数时（+1）发动冲刺
            //也就是第一次01冲刺 第二次0102冲刺 第三次010203冲刺
            if (TeamCombatActIndex <= TeamBrain.Count_TeamState306CircleRun_OuterAtk + 1)
            {
                isCombatComplite_TeamState306_CircleRun_OuterAtk = false;
                RushStart();
            }
            //其他玛狃拉准备就绪
            else { isCombatComplite_TeamState306_CircleRun_OuterAtk = true; }
        }
    }

    /// <summary>
    /// 三人小队_绕圈连招 外环攻击 连招彻底结束，转入下一个团队连招
    /// </summary>
    public void TeamCombatCompliteEvent_TeamState306_CircleRun_OuterAtk()
    {
        if (TeamBrain.NowSubState == WeavileTeamBrain.SubState.TeamState_306_CircleRun && TeamBrain.NowTeamState306SubState == WeavileTeamBrain.TEAMSTATE306_SUBSTATE.OuterAtk)
        {
            //设置下一次连招动作为未完成
            isCombatComplite_TeamState306_CircleRun_OuterAtk = false;
            switch (teamCombatActIndex)
            {
                case 1: IdleStart(TIME_IDLE_TEAMSTATE306_CIRCLEOVER_OVER_OUTERATK_01RUSH); break;
                case 2: IdleStart(TIME_IDLE_TEAMSTATE306_CIRCLEOVER_OVER_OUTERATK_02RUSH); break;
                case 3: IdleStart(TIME_IDLE_TEAMSTATE306_CIRCLEOVER_OVER_OUTERATK_03RUSH); break;
            }
        }
    }


    /// <summary>
    /// 三人小队_绕圈连招 外环攻击 团队成员连招投掷动作都完成时，进行下一次连招的事件
    /// </summary>
    public void TeamCombatActOverEvent_TeamState306_CircleRun_EscapeAtk_Fling()
    {
        if (TeamBrain.NowSubState == WeavileTeamBrain.SubState.TeamState_306_CircleRun && TeamBrain.NowTeamState306SubState == WeavileTeamBrain.TEAMSTATE306_SUBSTATE.EscapeAtk)
        {
            //设置下一次连招动作为未完成
            isCombatComplite_TeamState306_CircleRun_EscapeAtk = false;
            //开始投掷
            FlingStart();
        }
    }

    /// <summary>
    /// 三人小队_绕圈连招 外环攻击 连招彻底结束，转入下一个团队连招
    /// </summary>
    public void TeamCombatCompliteEvent_TeamState306_CircleRun_EscapeAtk()
    {
        if (TeamBrain.NowSubState == WeavileTeamBrain.SubState.TeamState_306_CircleRun && TeamBrain.NowTeamState306SubState == WeavileTeamBrain.TEAMSTATE306_SUBSTATE.EscapeAtk)
        {
            //设置下一次连招动作为未完成
            isCombatComplite_TeamState306_CircleRun_EscapeAtk = false;
            IdleStart(TIME_IDLE_TEAMSTATE306_CIRCLEOVER_OVER_ESCAPEATK);
        }
    }



    //=========================三人小队 绕圈连招============================




















    //=========================三人小队 回血连招1============================

    /// <summary>
    /// 三人小队_回血连招1 连招是否完成
    /// </summary>
    public bool IsCombatComplite_TeamState307_Heal
    {
        get { return isCombatComplite_TeamState307_Heal; }
        set { isCombatComplite_TeamState307_Heal = value; }
    }
    bool isCombatComplite_TeamState307_Heal = false;



    /// <summary>
    /// 三人小队_回血连招1 连招彻底结束，转入下一个团队连招
    /// </summary>
    public void TeamCombatCompliteEvent_TeamState307_Heal()
    {
        if (TeamBrain.NowSubState == WeavileTeamBrain.SubState.TeamState_307_Heal1)
        {
            //设置下一次连招动作为未完成
            isCombatComplite_TeamState307_Heal = false;
            switch (TeamCombatActIndex)
            {
                case 1:
                    IdleStart(TIME_IDLE_TEAMSTATE307_HEAL1_01USEPROTECT);
                    break;
                case 2:
                    IdleStart(TIME_IDLE_TEAMSTATE307_HEAL1_02HEAL);
                    break;
                case 3:
                    IdleStart(TIME_IDLE_TEAMSTATE307_HEAL1_03RUSH);
                    break;
            }
        }
    }

    //=========================三人小队 回血连招1============================
















    //=========================三人小队 回血连招2============================

    /// <summary>
    /// 三人小队_回血连招1 连招是否完成
    /// </summary>
    public bool IsCombatComplite_TeamState308_Heal2
    {
        get { return isCombatComplite_TeamState308_Heal2; }
        set { isCombatComplite_TeamState308_Heal2 = value; }
    }
    bool isCombatComplite_TeamState308_Heal2 = false;



    /// <summary>
    /// 三人小队_回血连招1 连招彻底结束，转入下一个团队连招
    /// </summary>
    public void TeamCombatCompliteEvent_TeamState308_Heal2()
    {
        if (TeamBrain.NowSubState == WeavileTeamBrain.SubState.TeamState_308_Heal2)
        {
            //设置下一次连招动作为未完成
            isCombatComplite_TeamState308_Heal2 = false;
            switch (TeamCombatActIndex)
            {
                case 1:
                    IdleStart(5.0f);
                    break;
                case 2:
                    IdleStart(5.0f);
                    break;
                case 3:
                    IdleStart(5.0f);
                    break;
            }
        }
    }

    //=========================三人小队 回血连招1============================












    //=========================三人小队 万灵药连招============================

    /// <summary>
    /// 三人小队 万灵药连招 连招动作是否完成
    /// </summary>
    public bool IsCombatComplite_TeamState309_FullHeal
    {
        get { return isCombatComplite_TeamState309_FullHeal; }
        set { isCombatComplite_TeamState309_FullHeal = value; }
    }
    bool isCombatComplite_TeamState309_FullHeal = false;




    /// <summary>
    /// 三人小队_万灵药连招 治愈完后，进行下一次连招的事件
    /// </summary>
    public void TeamCombatActOverEvent_TeamState309_FullHeal()
    {
        if (TeamBrain.NowSubState == WeavileTeamBrain.SubState.TeamState_309_FullHeal && TeamBrain.NowTeamState309SubState == WeavileTeamBrain.TEAMSTATE309_SUBSTATE.Rush)
        {
            //设置下一次连招动作为未完成
            isCombatComplite_TeamState309_FullHeal = false;
            //开始冲刺
            RushStart();
        }
    }




    /// <summary>
    /// 三人小队 万灵药连招 连招彻底结束，转入下一个团队连招
    /// </summary>
    public void TeamCombatCompliteEvent_TeamState309_FullHeal()
    {
        if (TeamBrain.NowSubState == WeavileTeamBrain.SubState.TeamState_309_FullHeal)
        {
            //设置下一次连招动作为未完成
            isCombatComplite_TeamState309_FullHeal = false;
            switch (teamCombatActIndex)
            {
                case 1: IdleStart(TIME_IDLE_TEAMSTATE309_FULLHEAL); break;
                case 2: IdleStart(TIME_IDLE_TEAMSTATE309_FULLHEAL); break;
                case 3: IdleStart(TIME_IDLE_TEAMSTATE309_FULLHEAL); break;
            }
        }
    }

    //=========================三人小队 万灵药连招============================












    //=========================二人小队 近战连招============================

    /// <summary>
    /// 二人小队_近战连招 连招是否完成
    /// </summary>
    public bool IsCombatComplite_TeamState202_CloseAtk
    {
        get { return isCombatComplite_TeamState202_CloseAtk; }
        set { isCombatComplite_TeamState202_CloseAtk = value; }
    }
    bool isCombatComplite_TeamState202_CloseAtk = false;

    /// <summary>
    /// 二人小队_近战连招 团队成员连招动作都完成时，进行下一次连招的事件
    /// </summary>
    public void TeamCombatActOverEvent_TeamState202_CloseAtk()
    {
        if (TeamBrain.NowSubState == WeavileTeamBrain.SubState.TeamState_202_CloseAtk)
        {
            //设置下一次连招动作为未完成
            isCombatComplite_TeamState202_CloseAtk = false;
            switch (TeamBrain.NowTeamState202CloseAtk)
            {
                //冲刺子状态 玛狃拉01冲刺 玛狃拉02帮助
                case WeavileTeamBrain.TEAMSTATE202_CLOSEATK.Rush:
                    switch (TeamCombatActIndex)
                    {
                        case 1://玛狃拉01冲刺
                            RushStart();
                            break;
                        case 2://玛狃拉02帮助
                            UseProtectStart(TIME_USEPROTECT_TEMASTATE202_CLOSEATK);
                            break;
                    }
                    break;
                //冲刺子状态 玛狃拉01冰粒 玛狃拉02跳跃
                case WeavileTeamBrain.TEAMSTATE202_CLOSEATK.Jump:
                    switch (TeamCombatActIndex)
                    {
                        case 1://玛狃拉01冰粒
                            IceShardStart(TIME_ICESHARD_TEMASTATE202CLOSEATK);
                            break;
                        case 2://玛狃拉02跳跃
                            JumpMoveStart();
                            break;
                    }
                    break;
            }
        }
    }

    /// <summary>
    /// 二人小队_近战连招 连招彻底结束，转入下一个团队连招
    /// </summary>
    public void TeamCombatCompliteEvent_TeamState202_CloseAtk()
    {
        if (TeamBrain.NowSubState == WeavileTeamBrain.SubState.TeamState_202_CloseAtk)
        {
            //设置下一次连招动作为未完成
            isCombatComplite_TeamState202_CloseAtk = false;
            switch (TeamCombatActIndex)
            {
                case 1:
                    IdleStart(TIME_IDLE_TEAMSTATE202_CLOSEATK_OVER_01ICESHARD);
                    break;
                case 2:
                    IdleStart(TIME_IDLE_TEAMSTATE202_CLOSEATK_OVER_02JUMP);
                    break;
            }
        }
    }

    //=========================二人小队 近战连招============================









    //=========================二人小队 替身连招============================





    /// <summary>
    /// 二人小队_替身连招 连招动作是否完成
    /// </summary>
    public bool IsCombatComplite_TeamState203_Substitute
    {
        get { return isCombatComplite_TeamState203_Substitute; }
        set { isCombatComplite_TeamState203_Substitute = value; }
    }
    bool isCombatComplite_TeamState203_Substitute = false;

    /// <summary>
    /// 二人小队_替身连招 团队成员连招动作都完成时，进行下一次连招的事件
    /// </summary>
    public void TeamCombatActOverEvent_TeamState203_Substitute()
    {
        if (TeamBrain.NowSubState == WeavileTeamBrain.SubState.TeamState_203_Substitute)
        {
            //设置下一次连招动作为未完成
            isCombatComplite_TeamState203_Substitute = false;
            //根据当前主脑副状态决定下一步连招
            switch (TeamBrain.NowTeamState203SubState)
            {
                //当前为投掷 状态机错误
                case WeavileTeamBrain.TEAMSTATE203_SUBSTATE.Fling:
                    Debug.Log("Error 203替身连招 状态机错误");
                    //强制修改为跳跃阶段 跳跃开始 跳跃至房间角落
                    TeamBrain.NowTeamState203SubState = WeavileTeamBrain.TEAMSTATE203_SUBSTATE.Jump;
                    JumpMoveStart();
                    break;
                case WeavileTeamBrain.TEAMSTATE203_SUBSTATE.Jump:
                    //强制修改为跳跃阶段 跳跃开始 跳跃至房间角落
                    JumpMoveStart();
                    break;
                case WeavileTeamBrain.TEAMSTATE203_SUBSTATE.Protect:
                    switch (TeamCombatActIndex)
                    {
                        case 1: //直接转入发呆
                            isCombatComplite_TeamState203_Substitute = true;
                            break;
                        case 2: //使用保护
                            UseProtectStart(TIME_USEPROTECT_TEMASTATE302_CLOSEATK);
                            break;
                    }
                    break;
                case WeavileTeamBrain.TEAMSTATE203_SUBSTATE.Toxic:
                    switch (TeamCombatActIndex)
                    {
                        case 1: //使用投掷
                            FlingStart();
                            break;
                        case 2: //直接转入发呆
                            isCombatComplite_TeamState203_Substitute = true;
                            break;
                    }
                    break;
                case WeavileTeamBrain.TEAMSTATE203_SUBSTATE.Rush:
                    //开始冲刺
                    RushStart();
                    break;
            }
        }
    }

    /// <summary>
    /// 二人小队_替身连招 连招彻底结束，转入下一个团队连招
    /// </summary>
    public void TeamCombatCompliteEvent_TeamState203_Substitue()
    {
        if (TeamBrain.NowSubState == WeavileTeamBrain.SubState.TeamState_203_Substitute)
        {
            //设置下一次连招动作为未完成
            isCombatComplite_TeamState203_Substitute = false;
            switch (teamCombatActIndex)
            {
                case 1: IdleStart(TIME_IDLE_TEAMSTATE203_SUBSTITUTE_OVER_01RUSH); break;
                case 2: IdleStart(TIME_IDLE_TEAMSTATE203_SUBSTITUTE_OVER_02RUSH); break;
            }
        }
    }

    //=========================二人小队 替身连招============================











    //=========================二人小队 替身连招2============================

    /// <summary>
    /// 检测全体残影和本体是否完成204的动作 有一人未完成 则不为真
    /// </summary>
    /// <returns></returns>
    public bool BodyAndClone_IsCombatComplite_TeamState204_Substitute2()
    {
        //先排空
        _mTool.RemoveNullInList<WeavileCloneBody>(CloneBodyList);

        //所有分身是否已完成 如果没有分身则已完成为true
        bool isAllCloneComplite = true;
        for (int i = 0; i < CloneBodyList.Count; i++)
        {
            //有一个残影未完成 则全体未完成
            if (CloneBodyList[i] != null && !CloneBodyList[i].isCombatComplite_TeamState204_Substitute2)
            {
                isAllCloneComplite = false;
                break;
            }
        }
        //返回全体残影和本体的完成情况
        return isAllCloneComplite && isCombatComplite_TeamState204_Substitute2;
    }

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
                    CloneShadowStart(TIME_CLONESHADOW_TEAMSTATE_204_SUBSTITUTE2_IDLE);
                    break;
                case WeavileTeamBrain.TEAMSTATE204_SUBSTATE.CloneShadow:
                    //强制修改为跳跃阶段 跳跃开始 跳跃至房间角落
                    CloneShadowStart(TIME_CLONESHADOW_TEAMSTATE_204_SUBSTITUTE2_IDLE);
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


            //先排空
            _mTool.RemoveNullInList<WeavileCloneBody>(CloneBodyList);
            //让旗下所有残影执行下一次动作
            for (int i = 0; i < CloneBodyList.Count; i++)
            {
                CloneBodyList[i].TeamCombatActOverEvent_TeamState204_Substitute2();
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
            //设置下一次连招动作为未完成
            isCombatComplite_TeamState204_Substitute2 = false;
            switch (teamCombatActIndex)
            {
                case 1: IdleStart(TIME_IDLE_TEAMSTATE204_SUBSTITUTE2_OVER_01RUSH); break;
                case 2: IdleStart(TIME_IDLE_TEAMSTATE204_SUBSTITUTE2_OVER_02RUSH); break;
            }
        }
    }

    //=========================二人小队 替身连招2============================










    //=========================二人小队 绕圈连招============================



    /// <summary>
    /// 二人小队 内环攻击 连招动作是否完成
    /// </summary>
    public bool IsCombatComplite_TeamState205_CircleRun_InnerAtk
    {
        get { return isCombatComplite_TeamState205_CirclrRun_InnerAtk; }
        set { isCombatComplite_TeamState205_CirclrRun_InnerAtk = value; }
    }
    bool isCombatComplite_TeamState205_CirclrRun_InnerAtk = false;

    /// <summary>
    /// 二人小队_绕圈连招 外环攻击 连招动作是否完成
    /// </summary>
    public bool IsCombatComplite_TeamState205_CircleRun_OuterAtk
    {
        get { return isCombatComplite_TeamState205_CircleRun_OuterAtk; }
        set { isCombatComplite_TeamState205_CircleRun_OuterAtk = value; }
    }
    bool isCombatComplite_TeamState205_CircleRun_OuterAtk = false;
    /// <summary>
    /// 二人小队_绕圈连招 脱离攻击 连招动作是否完成
    /// </summary>
    public bool IsCombatComplite_TeamState205_CircleRun_EscapeAtk
    {
        get { return isCombatComplite_TeamState205_CircleRun_EscapeAtk; }
        set { isCombatComplite_TeamState205_CircleRun_EscapeAtk = value; }
    }
    bool isCombatComplite_TeamState205_CircleRun_EscapeAtk = false;


    /// <summary>
    /// 二人小队_绕圈连招 内环攻击 团队成员连招动作都完成时，进行下一次连招的事件
    /// </summary>
    public void TeamCombatActOverEvent_TeamState205_CircleRun_InnerAtk()
    {
        if (TeamBrain.NowSubState == WeavileTeamBrain.SubState.TeamState_205_CircleRun && TeamBrain.NowTeamState205SubState == WeavileTeamBrain.TEAMSTATE306_SUBSTATE.Run)
        {
            isCombatComplite_TeamState205_CirclrRun_InnerAtk = false;
            CloneShadowStart(TIME_CLONESHADOW_TEAMSTATE_306_CIRCLERUN_IDLE);
        }
    }


    /// <summary>
    /// 二人小队_绕圈连招 外环攻击 团队成员连招动作都完成时，进行下一次连招的事件
    /// </summary>
    public void TeamCombatActOverEvent_TeamState205_CircleRun_OuterAtk()
    {
        if (TeamBrain.NowSubState == WeavileTeamBrain.SubState.TeamState_205_CircleRun && TeamBrain.NowTeamState205SubState == WeavileTeamBrain.TEAMSTATE306_SUBSTATE.OuterAtk)
        {
            //设置下一次连招动作为未完成
            isCombatComplite_TeamState205_CircleRun_OuterAtk = false;
            //自己的攻击序列号小于等于外环攻击次数时（+1）发动冲刺
            //也就是第一次01冲刺 第二次0102冲刺 
            if (TeamCombatActIndex <= TeamBrain.Count_TeamState205CircleRun_OuterAtk + 1)
            {
                isCombatComplite_TeamState205_CircleRun_OuterAtk = false;
                RushStart();
            }
            //其他玛狃拉准备就绪
            else { isCombatComplite_TeamState205_CircleRun_OuterAtk = true; }
        }
    }

    /// <summary>
    /// 二人小队_绕圈连招 外环攻击 连招彻底结束，转入下一个团队连招
    /// </summary>
    public void TeamCombatCompliteEvent_TeamState205_CircleRun_OuterAtk()
    {
        if (TeamBrain.NowSubState == WeavileTeamBrain.SubState.TeamState_205_CircleRun && TeamBrain.NowTeamState205SubState == WeavileTeamBrain.TEAMSTATE306_SUBSTATE.OuterAtk)
        {
            //设置下一次连招动作为未完成
            isCombatComplite_TeamState205_CircleRun_OuterAtk = false;
            switch (teamCombatActIndex)
            {
                case 1: IdleStart(TIME_IDLE_TEAMSTATE306_CIRCLEOVER_OVER_OUTERATK_01RUSH); break;
                case 2: IdleStart(TIME_IDLE_TEAMSTATE306_CIRCLEOVER_OVER_OUTERATK_01RUSH); break;
            }
        }
    }


    /// <summary>
    /// 二人小队_绕圈连招 外环攻击 团队成员连招投掷动作都完成时，进行下一次连招的事件
    /// </summary>
    public void TeamCombatActOverEvent_TeamState205_CircleRun_EscapeAtk_Fling()
    {
        if (TeamBrain.NowSubState == WeavileTeamBrain.SubState.TeamState_205_CircleRun && TeamBrain.NowTeamState205SubState == WeavileTeamBrain.TEAMSTATE306_SUBSTATE.EscapeAtk)
        {
            //设置下一次连招动作为未完成
            isCombatComplite_TeamState205_CircleRun_EscapeAtk = false;
            //开始投掷
            FlingStart();
        }
    }

    /// <summary>
    /// 二人小队_绕圈连招 外环攻击 连招彻底结束，转入下一个团队连招
    /// </summary>
    public void TeamCombatCompliteEvent_TeamState205_CircleRun_EscapeAtk()
    {
        if (TeamBrain.NowSubState == WeavileTeamBrain.SubState.TeamState_205_CircleRun && TeamBrain.NowTeamState205SubState == WeavileTeamBrain.TEAMSTATE306_SUBSTATE.EscapeAtk)
        {
            //设置下一次连招动作为未完成
            isCombatComplite_TeamState205_CircleRun_EscapeAtk = false;
            IdleStart(TIME_IDLE_TEAMSTATE306_CIRCLEOVER_OVER_ESCAPEATK);
        }
    }



    //=========================二人小队 绕圈连招============================











    //=========================二人小队 回血连招============================

    /// <summary>
    /// 二人小队 回血连招 连招动作是否完成
    /// </summary>
    public bool IsCombatComplite_TeamState206_Heal
    {
        get { return isCombatComplite_TeamState206_Heal; }
        set { isCombatComplite_TeamState206_Heal = value; }
    }
    public bool isCombatComplite_TeamState206_Heal = false;


    /// <summary>
    /// 二人小队 回血连招 连招彻底结束，转入下一个团队连招
    /// </summary>
    public void TeamCombatCompliteEvent_TeamState206_Heal()
    {
        if (TeamBrain.NowSubState == WeavileTeamBrain.SubState.TeamState_206_Heal)
        {
            //设置下一次连招动作为未完成
            isCombatComplite_TeamState206_Heal = false;
            switch (teamCombatActIndex)
            {
                case 1: IdleStart(TIME_IDLE_TEAMSTATE206_HEAL1_01MAZE); break;
                case 2: IdleStart(TIME_IDLE_TEAMSTATE206_HEAL1_02HEAL); break;
            }
        }
    }

    //=========================二人小队 回血连招============================









    //=========================二人小队 迷宫连招============================



    /// <summary>
    /// 二人小队_迷宫连招 连招动作是否完成
    /// </summary>
    public bool IsCombatComplite_TeamState207_Maze
    {
        get { return isCombatComplite_TeamState207_Maze; }
        set { isCombatComplite_TeamState207_Maze = value; }
    }
    public bool isCombatComplite_TeamState207_Maze = false;

    /// <summary>
    /// 二人小队_迷宫连招 连招彻底结束，转入下一个团队连招
    /// </summary>
    public void TeamCombatCompliteEvent_TeamState207_Mazes()
    {
        if (TeamBrain.NowSubState == WeavileTeamBrain.SubState.TeamState_207_Maze)
        {
            //设置下一次连招动作为未完成
            isCombatComplite_TeamState207_Maze = false;
            //三人小队迷宫连招首次投掷重置为错
            isOver_TeamState207Maze_FirstFling = false;

            switch (teamCombatActIndex)
            {
                case 1: IdleStart(TIME_IDLE_TEAMSTATE207_MAZE_01MAZE); break;
                case 2: IdleStart(TIME_IDLE_TEAMSTATE207_MAZE_02MAZEWATCH); break;
            }
        }
    }

    //=========================二人小队 迷宫连招============================









    //=========================二人小队 万灵药连招============================

    /// <summary>
    /// 二人小队 万灵药连招 连招动作是否完成
    /// </summary>
    public bool IsCombatComplite_TeamState208_FullHeal
    {
        get { return isCombatComplite_TeamState208_FullHeal; }
        set { isCombatComplite_TeamState208_FullHeal = value; }
    }
    bool isCombatComplite_TeamState208_FullHeal = false;



    /// <summary>
    /// 二人小队_万灵药连招 治愈完后，进行下一次连招的事件
    /// </summary>
    public void TeamCombatActOverEvent_TeamState208_FullHeal()
    {
        if (TeamBrain.NowSubState == WeavileTeamBrain.SubState.TeamState_208_FullHeal && TeamBrain.NowTeamState208SubState == WeavileTeamBrain.TEAMSTATE309_SUBSTATE.Rush)
        {
            //设置下一次连招动作为未完成
            isCombatComplite_TeamState208_FullHeal = false;
            //开始投掷
            RushStart();
        }
    }


    /// <summary>
    /// 二人小队 万灵药连招 连招彻底结束，转入下一个团队连招
    /// </summary>
    public void TeamCombatCompliteEvent_TeamState208_FullHeal()
    {
        if (TeamBrain.NowSubState == WeavileTeamBrain.SubState.TeamState_208_FullHeal)
        {
            //设置下一次连招动作为未完成
            isCombatComplite_TeamState208_FullHeal = false;
            switch (teamCombatActIndex)
            {
                case 1: IdleStart(TIME_IDLE_TEAMSTATE309_FULLHEAL); break;
                case 2: IdleStart(TIME_IDLE_TEAMSTATE309_FULLHEAL); break;
            }
        }
    }

    //=========================二人小队 万灵药连招============================















    //■■■■■■■■■■■■■■■■■■■■玛狃拉小队主脑■■■■■■■■■■■■■■■■■■■■■■







    // Start is called before the first frame update
    void Start()
    {
        EmptyType01 = PokemonType.TypeEnum.Normal;//敌人第一属性
        EmptyType02 = PokemonType.TypeEnum.Normal;//敌人第二属性
        player = GameObject.FindObjectOfType<PlayerControler>();//获取玩家
        Emptylevel = SetLevel(player.Level, MaxLevel);//设定敌人等级
        EmptyHpForLevel(Emptylevel);//设定血量
        AtkAbilityPoint = AbilityForLevel(Emptylevel, AtkEmptyPoint);//设定攻击力
        SpAAbilityPoint = AbilityForLevel(Emptylevel, SpAEmptyPoint);//设定特攻
        DefAbilityPoint = AbilityForLevel(Emptylevel, DefEmptyPoint) * 1.5f; ;//设定防御力
        SpdAbilityPoint = AbilityForLevel(Emptylevel, SpdEmptyPoint) * 1.5f; ;//设定特防
        SpeedAbilityPoint = AbilityForLevel(Emptylevel, SpeedEmptyPoint);//设定速度
        Exp = BaseExp * Emptylevel / 7;//设定击败后获取的经验

        //获取刚体目标 动画管理者目标 并让刚体的初始x坐标带入FirstX中
        animator = GetComponent<Animator>();
        rigidbody2D = GetComponent<Rigidbody2D>();

        //启动计算方向携程
        StartCoroutine(CheckLook());

        //初始化方向状态机
        SetDirector(Vector2.down);
        IdleStart(TIME_IDLE_START);

        StartOverEvent();
        //先设置不会掉落凋落物
        IsHaveDropItem = false;

    }


    // Update is called once per frame
    void Update()
    {
        ResetPlayer();//如果玩家组件丢失，重新获取

        if (!isDie && !isBorn)//不处于正在死亡状态或正在出生状态时
        {
            EmptyDie();//判定是否执行死亡
            UpdateEmptyChangeHP();//判定生命值是否变化
            StateMaterialChange();//判定是否更换状态材质


            //InsertStateMechineSwitch


            //■■开始判断状态机 当处于冰冻 睡眠 致盲 麻痹状态时状态机停运
            if (!isEmptyFrozenDone && !isSleepDone && !isSilence && !isCanNotMoveWhenParalysis) 
            {
                switch (NowMainState)
                {
                    //发呆状态
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
                                    case WeavileTeamBrain.SubState.TeamState_200_Idle:
                                        GuardStart();
                                        break;
                                    case WeavileTeamBrain.SubState.TeamState_100_Normal:
                                        TeamBrain.NowTeamState100SubState = WeavileTeamBrain.TEAMSTATE100_SUBSTATE.RunToSmoke;
                                        RunStart();
                                        break;
                                    default:
                                        GuardStart();
                                        break;
                                }
                            }
                        }
                        break;
                    //奔跑状态
                    case MainState.Run:
                        {
                            switch (TeamBrain.NowSubState)
                            {
                                //三人小队发呆模式
                                case WeavileTeamBrain.SubState.TeamState_300_Idle:
                                    break;
                                //三人小队巡逻模式
                                case WeavileTeamBrain.SubState.TeamState_301_Run:
                                    //不恐惧
                                    if (!isFearDone)
                                    {
                                        float TargetDistence = Vector2.Distance(TeamBrain.TARGET_POSITION, (Vector2)transform.position);
                                        //根据行动编号不同方向不同
                                        switch (teamCombatActIndex)
                                        {
                                            //右边玛狃拉
                                            case 1:
                                                if (TargetDistence >= DISTENCE_TEAMSTATE300_RUN_MIDDLE_RUNSTOP / 1.5f)
                                                {
                                                    Dir_Run = (Quaternion.AngleAxis(-SetTwoSideOffsetAngleByDistence(TargetDistence), Vector3.forward) * (TeamBrain.TARGET_POSITION - (Vector2)transform.position)).normalized;
                                                    MoveBySpeedAndDir(Dir_Run, speed, SetTwoSideSpeedAlphaByDistence_TeamState301Run(TargetDistence), 0.0f, 0.0f, 0.0f, 0.0f);
                                                }
                                                break;
                                            //中间玛狃拉
                                            case 2:
                                                if (TargetDistence >= DISTENCE_TEAMSTATE300_RUN_MIDDLE_RUNSTOP)
                                                {
                                                    Dir_Run = (TeamBrain.TARGET_POSITION - (Vector2)transform.position).normalized;
                                                    MoveBySpeedAndDir(Dir_Run, speed, SPEEDALPHA_TEAMSTATE300_RUN_MIDDLE, 0.0f, 0.0f, 0.0f, 0.0f);
                                                }
                                                break;
                                            //左边玛狃拉
                                            case 3:
                                                if (TargetDistence >= DISTENCE_TEAMSTATE300_RUN_MIDDLE_RUNSTOP / 1.5f)
                                                {
                                                    Dir_Run = (Quaternion.AngleAxis(SetTwoSideOffsetAngleByDistence(TargetDistence), Vector3.forward) * (TeamBrain.TARGET_POSITION - (Vector2)transform.position)).normalized;
                                                    MoveBySpeedAndDir(Dir_Run, speed, SetTwoSideSpeedAlphaByDistence_TeamState301Run(TargetDistence), 0.0f, 0.0f, 0.0f, 0.0f);
                                                }
                                                break;
                                        }
                                    }
                                    //恐惧
                                    else
                                    {

                                    }
                                    break;
                                //三人小队近战连招
                                case WeavileTeamBrain.SubState.TeamState_302_CloseAtk:
                                    switch (TeamCombatActIndex)
                                    {
                                        case 1:
                                            RunOver();
                                            RushStart();
                                            break;
                                        case 2:
                                            RunOver();
                                            JumpMoveStart();
                                            break;
                                        case 3:
                                            RunOver();
                                            UseProtectStart(TIME_USEPROTECT_TEMASTATE302_CLOSEATK);
                                            break;
                                    }
                                    break;
                                //三人小队替身连招
                                case WeavileTeamBrain.SubState.TeamState_303_Substitute:
                                    switch (TeamCombatActIndex)
                                    {
                                        case 1:
                                            RunOver();
                                            FlingStart();//01投掷替身
                                            break;
                                        case 2:
                                            RunOver();
                                            JumpMoveStart();//0203跳跃接近01
                                            break;
                                        case 3:
                                            RunOver();
                                            JumpMoveStart();//0203跳跃接近01
                                            break;
                                    }
                                    break;
                                //三人小队分身连招
                                case WeavileTeamBrain.SubState.TeamState_304_CloneShadow:
                                    RunOver();
                                    CloneShadowStart(TIME_CLONESHADOW_TEAMSTATE_304_CLONESHADOW_IDLE);
                                    break;
                                //三人小队迷宫连招
                                case WeavileTeamBrain.SubState.TeamState_305_Maze:
                                    RunOver();
                                    JumpMoveStart();
                                    break;
                                //三人小队绕圈跑连招
                                case WeavileTeamBrain.SubState.TeamState_306_CircleRun:
                                    RunOver();
                                    CloneShadowStart(TIME_CLONESHADOW_TEAMSTATE_306_CIRCLERUN_IDLE);
                                    break;
                                //三人小队回血连招1
                                case WeavileTeamBrain.SubState.TeamState_307_Heal1:
                                    RunOver();
                                    switch (TeamCombatActIndex)
                                    {
                                        case 1:
                                            RunOver();
                                            JumpMoveStart();
                                            break;
                                        case 2:
                                            RunOver();
                                            JumpMoveStart();
                                            break;
                                        case 3:
                                            RunOver();
                                            RushStart();
                                            break;
                                    }
                                    break;
                                //三人小队回血连招2
                                case WeavileTeamBrain.SubState.TeamState_308_Heal2:
                                    RunOver();
                                    switch (TeamCombatActIndex)
                                    {
                                        case 1:
                                            RunOver();
                                            JumpMoveStart();
                                            break;
                                        case 2:
                                            RunOver();
                                            JumpMoveStart();
                                            break;
                                        case 3:
                                            RunOver();
                                            JumpMoveStart();
                                            break;
                                    }
                                    break;
                                //二人小队发呆模式
                                case WeavileTeamBrain.SubState.TeamState_200_Idle:
                                    break;
                                //二人小队奔跑模式
                                case WeavileTeamBrain.SubState.TeamState_201_Run:
                                    //不恐惧
                                    if (!isFearDone)
                                    {
                                        //确定另一个玛狃拉的位置
                                        Weavile anotherWeavile = this;
                                        switch (teamCombatActIndex)
                                        {
                                            //玛狃拉01
                                            case 1:
                                                {
                                                    anotherWeavile = TeamBrain.CombatWeavile02;
                                                }
                                                break;
                                            //玛狃拉02
                                            case 2:
                                                {
                                                    anotherWeavile = TeamBrain.CombatWeavile01;
                                                }
                                                break;
                                        }
                                        //移动
                                        {
                                            //朝向目标的方向
                                            Vector2 Dir_Target = (TeamBrain.TARGET_POSITION - (Vector2)transform.position).normalized;
                                            //基础方向
                                            Dir_Run = (Quaternion.AngleAxis(-90, Vector3.forward) * Dir_Target).normalized;
                                            //距离基准半径的距离
                                            float Distence_BaseRadius = Vector2.Distance(TeamBrain.TARGET_POSITION, (Vector2)transform.position) - DISTENCE_TEAMSTATE201_RUN_BASE_RADIUS;
                                            //在基准半径及其允许差值内
                                            if (Mathf.Abs(Distence_BaseRadius) <= DISTENCE_TEAMSTATE201_RUN_ALLOWSUB_BASE_RADIUS) { }
                                            //不在基准半径及其允许差值内
                                            else
                                            {
                                                //在基准半径内
                                                if (Distence_BaseRadius < 0)
                                                {
                                                    Dir_Run = Dir_Run - Dir_Target * Mathf.Clamp(Distence_BaseRadius / DISTENCE_TEAMSTATE201_RUN_BASE_RADIUS, 1.0f, 2.5f);
                                                }
                                                //在基准半径外
                                                else
                                                {
                                                    Dir_Run = Dir_Run + Dir_Target * Mathf.Clamp(Distence_BaseRadius / DISTENCE_TEAMSTATE201_RUN_BASE_RADIUS, 1.0f, 2.5f);
                                                }
                                            }
                                            //移动
                                            float sa = SetTwoSpeedAlphaByAnotherWeavile_TeamState301Run(anotherWeavile.transform, TeamBrain.TARGET_POSITION);
                                            //Debug.Log(name + sa);
                                            //速度为0的话 转入发射冰粒
                                            if (sa == 0.0f)
                                            {
                                                RunOver();
                                                IceShardStart(TIME_ICESHARD_TEMASTATE201RUN);
                                                break;
                                            }
                                            MoveBySpeedAndDir(Dir_Run, speed, sa, 0.0f, 0.0f, 0.0f, 0.0f);
                                        }
                                    }
                                    //恐惧
                                    else
                                    {

                                    }
                                    break;
                                //二人小队近战连招
                                case WeavileTeamBrain.SubState.TeamState_202_CloseAtk:
                                    RunOver();
                                    switch (TeamBrain.NowTeamState202CloseAtk)
                                    {
                                        //冲刺子状态 玛狃拉01冲刺 玛狃拉02帮助
                                        case WeavileTeamBrain.TEAMSTATE202_CLOSEATK.Rush:
                                            switch (TeamCombatActIndex)
                                            {
                                                case 1://玛狃拉01冲刺
                                                    RushStart();
                                                    break;
                                                case 2://玛狃拉02帮助
                                                    UseProtectStart(TIME_USEPROTECT_TEMASTATE202_CLOSEATK);
                                                    break;
                                            }
                                            break;
                                        //冲刺子状态 玛狃拉01冰粒 玛狃拉02跳跃
                                        case WeavileTeamBrain.TEAMSTATE202_CLOSEATK.Jump:
                                            switch (TeamCombatActIndex)
                                            {
                                                case 1://玛狃拉01冰粒
                                                    IceShardStart(TIME_ICESHARD_TEMASTATE202CLOSEATK);
                                                    break;
                                                case 2://玛狃拉02跳跃
                                                    JumpMoveStart();
                                                    break;
                                            }
                                            break;
                                    }
                                    break;
                                //二人小队替身模式
                                case WeavileTeamBrain.SubState.TeamState_203_Substitute:
                                    switch (TeamCombatActIndex)
                                    {
                                        case 1:
                                            RunOver();
                                            FlingStart();//01投掷替身
                                            break;
                                        case 2:
                                            RunOver();
                                            JumpMoveStart();//02跳跃接近01
                                            break;
                                    }
                                    break;
                                //二人小队替身模式2
                                case WeavileTeamBrain.SubState.TeamState_204_Substitute2:
                                    switch (TeamCombatActIndex)
                                    {
                                        case 1:
                                            RunOver();
                                            FlingStart();//01投掷替身
                                            break;
                                        case 2:
                                            RunOver();
                                            JumpMoveStart();//02跳跃接近01
                                            break;
                                    }
                                    break;
                                //二人小队 绕圈模式
                                case WeavileTeamBrain.SubState.TeamState_205_CircleRun:
                                    RunOver();
                                    CloneShadowStart(TIME_CLONESHADOW_TEAMSTATE_306_CIRCLERUN_IDLE);
                                    break;
                                //二人小队 回血模式
                                case WeavileTeamBrain.SubState.TeamState_206_Heal:
                                    RunOver();
                                    switch (TeamCombatActIndex)
                                    {
                                        case 1:
                                            JumpMoveStart();
                                            break;
                                        case 2:
                                            FlingStart();
                                            break;
                                    }
                                    break;
                                //二人小队 迷宫连招
                                case WeavileTeamBrain.SubState.TeamState_207_Maze:
                                    RunOver();
                                    JumpMoveStart();
                                    break;
                                //一人小队 
                                case WeavileTeamBrain.SubState.TeamState_100_Normal:
                                    {
                                        switch (TeamBrain.NowTeamState100SubState)
                                        {
                                            case WeavileTeamBrain.TEAMSTATE100_SUBSTATE.FlingSmoke:
                                                {
                                                    RunOver();
                                                    FlingStart();
                                                }
                                                break;
                                            case WeavileTeamBrain.TEAMSTATE100_SUBSTATE.RunToSmoke:
                                                {
                                                    //烟雾不足 释放烟雾
                                                    if (TeamBrain.GetCountWeavileSmokeList() < 4)
                                                    {
                                                        TeamBrain.NowTeamState100SubState = WeavileTeamBrain.TEAMSTATE100_SUBSTATE.FlingSmoke;
                                                        RunOver();
                                                        FlingStart();
                                                    }
                                                    //烟雾充足 跑入烟雾
                                                    else
                                                    {
                                                        Vector2 Target = GetClosestPointInSmoke(TeamBrain.WeavileSmokeList, 0.5f);
                                                        Dir_Run = ((Target - (Vector2)transform.position)).normalized;
                                                        MoveBySpeedAndDir(Dir_Run, speed, SPEEDALPHA_TEAMSTATE100NORMAL_RUN, 0.0f, 0.0f, 0.0f, 0.0f);
                                                        //到达（接近）目标点后 转入烟雾中移动模式
                                                        if (Vector2.Distance(Target , (Vector2)transform.position) <= 0.5f)
                                                        {
                                                            TeamBrain.NowTeamState100SubState = WeavileTeamBrain.TEAMSTATE100_SUBSTATE.RunInSmoke;
                                                            RunOver();
                                                            RunStart();
                                                        }
                                                    }
                                                }
                                                break;
                                            case WeavileTeamBrain.TEAMSTATE100_SUBSTATE.RunInSmoke:
                                                {
                                                    //烟雾不足 释放烟雾
                                                    if (TeamBrain.GetCountWeavileSmokeList() < 4)
                                                    {
                                                        TeamBrain.NowTeamState100SubState = WeavileTeamBrain.TEAMSTATE100_SUBSTATE.FlingSmoke;
                                                        RunOver();
                                                        FlingStart();
                                                    }
                                                    //烟雾充足 跑入烟雾
                                                    else
                                                    {
                                                        Dir_Run = ((Position_Target_Run - (Vector2)transform.position)).normalized;
                                                        MoveBySpeedAndDir(Dir_Run, speed, SPEEDALPHA_TEAMSTATE100NORMAL_RUN, 0.0f, 0.0f, 0.0f, 0.0f);
                                                        //到达（接近）目标点后 转入烟雾中守护模式
                                                        if (Vector2.Distance(Position_Target_Run, (Vector2)transform.position) <= 0.5f)
                                                        {
                                                            TeamBrain.NowTeamState100SubState = WeavileTeamBrain.TEAMSTATE100_SUBSTATE.GuardInSmoke;
                                                            RunOver();
                                                            GuardStart();
                                                        }
                                                    }
                                                }
                                                break;
                                            case WeavileTeamBrain.TEAMSTATE100_SUBSTATE.GuardInSmoke:
                                                {
                                                    RunOver();
                                                    GuardStart();
                                                }
                                                break;
                                            case WeavileTeamBrain.TEAMSTATE100_SUBSTATE.Rush:
                                                {
                                                    RunOver();
                                                    RushStart();
                                                }
                                                break;
                                            case WeavileTeamBrain.TEAMSTATE100_SUBSTATE.FlingBomb:
                                                {
                                                    RunOver();
                                                    FlingStart();
                                                }
                                                break;
                                            case WeavileTeamBrain.TEAMSTATE100_SUBSTATE.Idle:
                                                {
                                                    RunOver();
                                                    IdleStart(TIME_IDLE_TEAMSTATE100_NORMAL);
                                                }
                                                break;
                                        }
                                    }
                                    break;
                                //主脑其他模式还在奔跑的话 转入休息
                                default:
                                    RunOver();
                                    IdleStart(TIME_IDLE_START);
                                    break;
                            }
                        }
                        break;
                    //跳跃移动状态
                    case MainState.JumpMove:
                        {
                            //跳跃移动的准备期间
                            if (isPrepare_Jump)
                            {
                                JumpMoveTimer += Time.deltaTime;//跳跃移动计时器时间增加
                                switch (TeamBrain.NowSubState)
                                {
                                    //三人连招_近战连招
                                    case WeavileTeamBrain.SubState.TeamState_302_CloseAtk:
                                        //准备完毕
                                        if (JumpMoveTimer >= TIME_PREPARE_JUMP_TEMASTATE302_CLOSEATK)
                                        {
                                            JumpMoveTimer = 0;
                                            isPrepare_Jump = false;
                                            animator.SetInteger("Jump", 1);
                                        }
                                        break;
                                    //三人连招_替身连招
                                    case WeavileTeamBrain.SubState.TeamState_303_Substitute:
                                        //无准备时间
                                        if (JumpMoveTimer >= 0)
                                        {
                                            JumpMoveTimer = 0;
                                            isPrepare_Jump = false;
                                            animator.SetInteger("Jump", 1);
                                        }
                                        break;
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
                                    //三人连招_迷宫连招
                                    case WeavileTeamBrain.SubState.TeamState_305_Maze:
                                        //无准备时间
                                        if (JumpMoveTimer >= 0)
                                        {
                                            JumpMoveTimer = 0;
                                            isPrepare_Jump = false;
                                            animator.SetInteger("Jump", 1);
                                        }
                                        break;
                                    //三人连招_绕圈连招 脱离攻击
                                    case WeavileTeamBrain.SubState.TeamState_306_CircleRun:
                                        //准备完毕
                                        if (TeamBrain.NowTeamState306SubState == WeavileTeamBrain.TEAMSTATE306_SUBSTATE.EscapeAtk && JumpMoveTimer >= TIME_PREPARE_JUMP_TEMASTATE306_CIRCLERUN_ESCAPEATK)
                                        {
                                            JumpMoveTimer = 0;
                                            isPrepare_Jump = false;
                                            animator.SetInteger("Jump", 1);
                                        }
                                        break;
                                    //三人连招_回血连招1
                                    case WeavileTeamBrain.SubState.TeamState_307_Heal1:
                                        //无准备时间
                                        if (JumpMoveTimer >= 0)
                                        {
                                            JumpMoveTimer = 0;
                                            isPrepare_Jump = false;
                                            animator.SetInteger("Jump", 1);
                                        }
                                        break;
                                    //三人连招_回血连招2
                                    case WeavileTeamBrain.SubState.TeamState_308_Heal2:
                                        //无准备时间
                                        if (JumpMoveTimer >= 0)
                                        {
                                            JumpMoveTimer = 0;
                                            isPrepare_Jump = false;
                                            animator.SetInteger("Jump", 1);
                                        }
                                        break;
                                    //二人连招_近战连招
                                    case WeavileTeamBrain.SubState.TeamState_202_CloseAtk:
                                        //无准备时间
                                        if (JumpMoveTimer >= 0)
                                        {
                                            JumpMoveTimer = 0;
                                            isPrepare_Jump = false;
                                            animator.SetInteger("Jump", 1);
                                        }
                                        break;
                                    //二人连招_替身连招
                                    case WeavileTeamBrain.SubState.TeamState_203_Substitute:
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
                                    //二人连招_绕圈连招 脱离攻击
                                    case WeavileTeamBrain.SubState.TeamState_205_CircleRun:
                                        //准备完毕
                                        if (TeamBrain.NowTeamState205SubState == WeavileTeamBrain.TEAMSTATE306_SUBSTATE.EscapeAtk && JumpMoveTimer >= TIME_PREPARE_JUMP_TEMASTATE306_CIRCLERUN_ESCAPEATK)
                                        {
                                            JumpMoveTimer = 0;
                                            isPrepare_Jump = false;
                                            animator.SetInteger("Jump", 1);
                                        }
                                        break;
                                    //二人连招_回血连招
                                    case WeavileTeamBrain.SubState.TeamState_206_Heal:
                                        //无准备时间
                                        if (JumpMoveTimer >= 0)
                                        {
                                            JumpMoveTimer = 0;
                                            isPrepare_Jump = false;
                                            animator.SetInteger("Jump", 1);
                                        }
                                        break;
                                    //二人连招_迷宫连招
                                    case WeavileTeamBrain.SubState.TeamState_207_Maze:
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
                                    //三人连招_近战连招
                                    case WeavileTeamBrain.SubState.TeamState_302_CloseAtk:
                                        //移动
                                        MoveBySpeedAndDir(Dir_Jump, speed, SPEEDALPHA_JUMP_TEMASTATE302_CLOSEATK, 0.0f, 0.0f, 0.0f, 0.0f);
                                        //时间足够 移动结束
                                        //Debug.Log(JumpMoveTimer +"+"+ Time_Jump_Max);
                                        if (JumpMoveTimer >= Time_Jump_Max)
                                        {
                                            animator.SetInteger("Jump", 2);
                                        }
                                        break;
                                    //三人连招_替身连招
                                    case WeavileTeamBrain.SubState.TeamState_303_Substitute:
                                        //移动
                                        MoveBySpeedAndDir(Dir_Jump, speed, SPEEDALPHA_JUMP_TEMASTATE302_CLOSEATK, 0.0f, 0.0f, 0.0f, 0.0f);
                                        //时间足够 移动结束
                                        //Debug.Log(JumpMoveTimer +"+"+ Time_Jump_Max);
                                        if (JumpMoveTimer >= Time_Jump_Max)
                                        {
                                            animator.SetInteger("Jump", 2);
                                        }
                                        break;
                                    //三人连招_分身连招
                                    case WeavileTeamBrain.SubState.TeamState_304_CloneShadow:
                                        //移动
                                        if (Dir_Jump != Vector2.zero){MoveBySpeedAndDir(Dir_Jump, speed, SPEEDALPHA_JUMP_TEMASTATE304_LONESHADOW, 0.0f, 0.0f, 0.0f, 0.0f);}
                                        //时间足够 移动结束
                                        //Debug.Log(JumpMoveTimer +"+"+ Time_Jump_Max);
                                        if (JumpMoveTimer >= Time_Jump_Max)
                                        {
                                            animator.SetInteger("Jump", 2);
                                        }
                                        break;
                                    //三人连招_迷宫连招
                                    case WeavileTeamBrain.SubState.TeamState_305_Maze:
                                        //移动
                                        MoveBySpeedAndDir(Dir_Jump, speed, SpeedAlpha_Jump, 0.0f, 0.0f, 0.0f, 0.0f);
                                        //时间足够 移动结束
                                        //Debug.Log(JumpMoveTimer +"+"+ Time_Jump_Max);
                                        if (JumpMoveTimer >= Time_Jump_Max)
                                        {
                                            animator.SetInteger("Jump", 2);
                                        }
                                        break;
                                    //三人连招_绕圈连招 脱离攻击
                                    case WeavileTeamBrain.SubState.TeamState_306_CircleRun:
                                        if (TeamBrain.NowTeamState306SubState == WeavileTeamBrain.TEAMSTATE306_SUBSTATE.EscapeAtk)
                                        {
                                            //移动
                                            MoveBySpeedAndDir(Dir_Jump, speed, SPEEDALPHA_JUMP_TEMASTATE302_CLOSEATK, 0.0f, 0.0f, 0.0f, 0.0f);
                                            //时间足够 移动结束
                                            //Debug.Log(JumpMoveTimer +"+"+ Time_Jump_Max);
                                            if (JumpMoveTimer >= Time_Jump_Max)
                                            {
                                                animator.SetInteger("Jump", 2);
                                            }
                                        }
                                        break;
                                    //三人连招_回血连招1
                                    case WeavileTeamBrain.SubState.TeamState_307_Heal1:
                                        //移动
                                        MoveBySpeedAndDir(Dir_Jump, speed, SpeedAlpha_Jump, 0.0f, 0.0f, 0.0f, 0.0f);
                                        //时间足够 移动结束
                                        //Debug.Log(JumpMoveTimer +"+"+ Time_Jump_Max);
                                        if (JumpMoveTimer >= Time_Jump_Max)
                                        {
                                            animator.SetInteger("Jump", 2);
                                        }
                                        break;
                                    //三人连招_回血连招2
                                    case WeavileTeamBrain.SubState.TeamState_308_Heal2:
                                        //移动
                                        MoveBySpeedAndDir(Dir_Jump, speed, SpeedAlpha_Jump, 0.0f, 0.0f, 0.0f, 0.0f);
                                        //时间足够 移动结束
                                        //Debug.Log(JumpMoveTimer +"+"+ Time_Jump_Max);
                                        if (JumpMoveTimer >= Time_Jump_Max)
                                        {
                                            animator.SetInteger("Jump", 2);
                                        }
                                        break;
                                    //二人连招_近战连招
                                    case WeavileTeamBrain.SubState.TeamState_202_CloseAtk:
                                        //移动
                                        MoveBySpeedAndDir(Dir_Jump, speed, SPEEDALPHA_JUMP_TEMASTATE302_CLOSEATK, 0.0f, 0.0f, 0.0f, 0.0f);
                                        //时间足够 移动结束
                                        //Debug.Log(JumpMoveTimer +"+"+ Time_Jump_Max);
                                        if (JumpMoveTimer >= Time_Jump_Max)
                                        {
                                            animator.SetInteger("Jump", 2);
                                        }
                                        break;
                                    //二人连招_替身连招
                                    case WeavileTeamBrain.SubState.TeamState_203_Substitute:
                                        //移动
                                        MoveBySpeedAndDir(Dir_Jump, speed, SPEEDALPHA_JUMP_TEMASTATE302_CLOSEATK, 0.0f, 0.0f, 0.0f, 0.0f);
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
                                        if (Dir_Jump != Vector2.zero) { MoveBySpeedAndDir(Dir_Jump, speed, SPEEDALPHA_JUMP_TEMASTATE302_CLOSEATK, 0.0f, 0.0f, 0.0f, 0.0f); }
                                        //时间足够 移动结束
                                        //Debug.Log(JumpMoveTimer +"+"+ Time_Jump_Max);
                                        //Debug.Log("Dir_Jump" + Dir_Jump);
                                        if (JumpMoveTimer >= Time_Jump_Max)
                                        {
                                            animator.SetInteger("Jump", 2);
                                        }
                                        break;
                                    //二人连招_绕圈连招 脱离攻击
                                    case WeavileTeamBrain.SubState.TeamState_205_CircleRun:
                                        if (TeamBrain.NowTeamState205SubState == WeavileTeamBrain.TEAMSTATE306_SUBSTATE.EscapeAtk)
                                        {
                                            //移动
                                            MoveBySpeedAndDir(Dir_Jump, speed, SPEEDALPHA_JUMP_TEMASTATE302_CLOSEATK, 0.0f, 0.0f, 0.0f, 0.0f);
                                            //时间足够 移动结束
                                            //Debug.Log(JumpMoveTimer +"+"+ Time_Jump_Max);
                                            if (JumpMoveTimer >= Time_Jump_Max)
                                            {
                                                animator.SetInteger("Jump", 2);
                                            }
                                        }
                                        break;
                                    //二人连招_回血连招
                                    case WeavileTeamBrain.SubState.TeamState_206_Heal:
                                        //移动
                                        MoveBySpeedAndDir(Dir_Jump, speed, SpeedAlpha_Jump, 0.0f, 0.0f, 0.0f, 0.0f);
                                        //时间足够 移动结束
                                        //Debug.Log(JumpMoveTimer +"+"+ Time_Jump_Max);
                                        if (JumpMoveTimer >= Time_Jump_Max)
                                        {
                                            animator.SetInteger("Jump", 2);
                                        }
                                        break;
                                    //二人连招_迷宫连招
                                    case WeavileTeamBrain.SubState.TeamState_207_Maze:
                                        //移动
                                        MoveBySpeedAndDir(Dir_Jump, speed, SpeedAlpha_Jump, 0.0f, 0.0f, 0.0f, 0.0f);
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
                    //冲刺状态
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
                                    //三人小队近战连招
                                    case WeavileTeamBrain.SubState.TeamState_302_CloseAtk:
                                        //非首次才可以转向
                                        if (TeamBrain.Count_Teamstate302_Close_Atk != 0)
                                        {
                                            Dir_Rush = (TargetPosition - (Vector2)transform.position).normalized;
                                            //SetRushArrow(NowRushType, );
                                            if (RushArrowObj != null) { RushArrowObj.SetTarget(ParentPokemonRoom.GetRoomBoundaryByRaycastTo(transform.position, Dir_Rush)); }
                                            SetDirector(_mTool.MainVector2(Dir_Rush));
                                        }
                                        break;
                                    //三人小队替身连招
                                    case WeavileTeamBrain.SubState.TeamState_303_Substitute:
                                        //最后冲刺阶段可以转向
                                        if (TeamBrain.NowTeamState303SubState == WeavileTeamBrain.TEAMSTATE303_SUBSTATE.Rush)
                                        {
                                            Dir_Rush = (TargetPosition - (Vector2)transform.position).normalized;
                                            //SetRushArrow(NowRushType, );
                                            if (RushArrowObj != null) { RushArrowObj.SetTarget(ParentPokemonRoom.GetRoomBoundaryByRaycastTo(transform.position, Dir_Rush)); }
                                            SetDirector(_mTool.MainVector2(Dir_Rush));
                                        }
                                        break;
                                    //三人小队分身连招
                                    case WeavileTeamBrain.SubState.TeamState_304_CloneShadow:
                                        //意外冲刺可以转向
                                        if (TeamBrain.NowTeamState304CloneShadow == WeavileTeamBrain.TEAMSTATE304_CLONESHADOW.AccidentRush)
                                        {
                                            Dir_Rush = (TargetPosition - (Vector2)transform.position).normalized;
                                            //SetRushArrow(NowRushType, );
                                            if (RushArrowObj != null) { RushArrowObj.SetTarget(ParentPokemonRoom.GetRoomBoundaryByRaycastTo(transform.position, Dir_Rush)); }
                                            SetDirector(_mTool.MainVector2(Dir_Rush));
                                        }
                                        break;
                                    //三人小队迷宫连招
                                    case WeavileTeamBrain.SubState.TeamState_305_Maze:
                                        //不可以转向
                                        break;
                                    //三人小队绕圈连招
                                    case WeavileTeamBrain.SubState.TeamState_306_CircleRun:
                                        //仅外环攻击才可转向
                                        if (TeamBrain.NowTeamState306SubState == WeavileTeamBrain.TEAMSTATE306_SUBSTATE.OuterAtk)
                                        {
                                            Dir_Rush = (TargetPosition - (Vector2)transform.position).normalized;
                                            if (RushArrowObj != null) { RushArrowObj.SetTarget(ParentPokemonRoom.GetRoomBoundaryByRaycastTo(transform.position, Dir_Rush)); }
                                            SetDirector(_mTool.MainVector2(Dir_Rush));
                                        }
                                        break;
                                    //三人小队回血连招1
                                    case WeavileTeamBrain.SubState.TeamState_307_Heal1:
                                        //可转向
                                        if (true)
                                        {
                                            Dir_Rush = (TargetPosition - (Vector2)transform.position).normalized;
                                            if (RushArrowObj != null) { RushArrowObj.SetTarget(ParentPokemonRoom.GetRoomBoundaryByRaycastTo(transform.position, Dir_Rush)); }
                                            SetDirector(_mTool.MainVector2(Dir_Rush));
                                        }
                                        break;
                                    //三人小队万灵药连招
                                    case WeavileTeamBrain.SubState.TeamState_309_FullHeal:
                                        //仅冲刺阶段
                                        if (TeamBrain.NowTeamState309SubState == WeavileTeamBrain.TEAMSTATE309_SUBSTATE.Rush)
                                        {
                                            Dir_Rush = (TargetPosition - (Vector2)transform.position).normalized;
                                            if (RushArrowObj != null) { RushArrowObj.SetTarget(ParentPokemonRoom.GetRoomBoundaryByRaycastTo(transform.position, Dir_Rush)); }
                                            SetDirector(_mTool.MainVector2(Dir_Rush));
                                        }
                                        break;
                                    //二人小队近战连招
                                    case WeavileTeamBrain.SubState.TeamState_202_CloseAtk:
                                        //非首次才可以转向
                                        if (TeamBrain.Count_Teamstate202_Close_Atk != 0)
                                        {
                                            Dir_Rush = (TargetPosition - (Vector2)transform.position).normalized;
                                            if (RushArrowObj != null) { RushArrowObj.SetTarget(ParentPokemonRoom.GetRoomBoundaryByRaycastTo(transform.position, Dir_Rush)); }
                                            SetDirector(_mTool.MainVector2(Dir_Rush));
                                        }
                                        break;
                                    //二人小队替身连招
                                    case WeavileTeamBrain.SubState.TeamState_203_Substitute:
                                        //最后冲刺阶段可以转向
                                        if (TeamBrain.NowTeamState203SubState == WeavileTeamBrain.TEAMSTATE203_SUBSTATE.Rush)
                                        {
                                            Dir_Rush = (TargetPosition - (Vector2)transform.position).normalized;
                                            //SetRushArrow(NowRushType, );
                                            if (RushArrowObj != null) { RushArrowObj.SetTarget(ParentPokemonRoom.GetRoomBoundaryByRaycastTo(transform.position, Dir_Rush)); }
                                            SetDirector(_mTool.MainVector2(Dir_Rush));
                                        }
                                        break;
                                    //二人小队替身连招2
                                    case WeavileTeamBrain.SubState.TeamState_204_Substitute2:
                                        //最后冲刺阶段可以转向
                                        if (TeamBrain.NowTeamState204SubState == WeavileTeamBrain.TEAMSTATE204_SUBSTATE.Rush)
                                        {
                                            Dir_Rush = (TargetPosition - (Vector2)transform.position).normalized;
                                            //SetRushArrow(NowRushType, );
                                            if (RushArrowObj != null) { RushArrowObj.SetTarget(ParentPokemonRoom.GetRoomBoundaryByRaycastTo(transform.position, Dir_Rush)); }
                                            SetDirector(_mTool.MainVector2(Dir_Rush));
                                        }
                                        break;
                                    //二人小队绕圈连招
                                    case WeavileTeamBrain.SubState.TeamState_205_CircleRun:
                                        //仅外环攻击才可转向
                                        if (TeamBrain.NowTeamState205SubState == WeavileTeamBrain.TEAMSTATE306_SUBSTATE.OuterAtk)
                                        {
                                            Dir_Rush = (TargetPosition - (Vector2)transform.position).normalized;
                                            if (RushArrowObj != null) { RushArrowObj.SetTarget(ParentPokemonRoom.GetRoomBoundaryByRaycastTo(transform.position, Dir_Rush)); }
                                            SetDirector(_mTool.MainVector2(Dir_Rush));
                                        }
                                        break;
                                    //二人小队迷宫连招
                                    case WeavileTeamBrain.SubState.TeamState_207_Maze:
                                        //不可以转向
                                        break;
                                    //二人小队万灵药连招
                                    case WeavileTeamBrain.SubState.TeamState_208_FullHeal:
                                        //仅冲刺阶段
                                        if (TeamBrain.NowTeamState208SubState == WeavileTeamBrain.TEAMSTATE309_SUBSTATE.Rush)
                                        {
                                            Dir_Rush = (TargetPosition - (Vector2)transform.position).normalized;
                                            if (RushArrowObj != null) { RushArrowObj.SetTarget(ParentPokemonRoom.GetRoomBoundaryByRaycastTo(transform.position, Dir_Rush)); }
                                            SetDirector(_mTool.MainVector2(Dir_Rush));
                                        }
                                        break;
                                    //一人小队 
                                    case WeavileTeamBrain.SubState.TeamState_100_Normal:
                                        switch (TeamBrain.NowTeamState100SubState)
                                        {
                                            case WeavileTeamBrain.TEAMSTATE100_SUBSTATE.FlingSmoke:
                                                RushOver();
                                                FlingStart();
                                                break;
                                            case WeavileTeamBrain.TEAMSTATE100_SUBSTATE.RunToSmoke:
                                                RushOver();
                                                RunStart();
                                                break;
                                            case WeavileTeamBrain.TEAMSTATE100_SUBSTATE.RunInSmoke:
                                                RushOver();
                                                RunStart();
                                                break;
                                            case WeavileTeamBrain.TEAMSTATE100_SUBSTATE.GuardInSmoke:
                                                RushOver();
                                                GuardStart();
                                                break;
                                            case WeavileTeamBrain.TEAMSTATE100_SUBSTATE.Rush:
                                                //可转向
                                                if (true)
                                                {
                                                    Dir_Rush = (TargetPosition - (Vector2)transform.position).normalized;
                                                    if (RushArrowObj != null) { RushArrowObj.SetTarget(ParentPokemonRoom.GetRoomBoundaryByRaycastTo(transform.position, Dir_Rush)); }
                                                    SetDirector(_mTool.MainVector2(Dir_Rush));
                                                }
                                                break;
                                            case WeavileTeamBrain.TEAMSTATE100_SUBSTATE.FlingBomb:
                                                RushOver();
                                                FlingStart();
                                                break;
                                            case WeavileTeamBrain.TEAMSTATE100_SUBSTATE.Idle:
                                                RushOver();
                                                IdleStart(TIME_IDLE_TEAMSTATE100_NORMAL);
                                                break;
                                        }
                                        break;
                                    default:
                                        Dir_Rush = (TargetPosition - (Vector2)transform.position).normalized;
                                        SetDirector(_mTool.MainVector2(Dir_Rush));
                                        break;
                                }

                            }
                            //蓄力完毕后转进冲刺(不同连招蓄力时间不一样)
                            if (isCharge_Rush)
                            {
                                switch (TeamBrain.NowSubState)
                                {
                                    //三人小队近战连招
                                    case WeavileTeamBrain.SubState.TeamState_302_CloseAtk:
                                        if (RushTimer >= TIME_CHARGE_RUSH_TEMASTATE302_CLOSEATK)
                                        {
                                            animator.SetInteger("Rush", 2);
                                            RushTimer = 0;//蓄力结束
                                        }
                                        break;
                                    //三人小队替身连招
                                    case WeavileTeamBrain.SubState.TeamState_303_Substitute:
                                        if (RushTimer >= TIME_CHARGE_RUSH_TEMASTATE302_CLOSEATK + 1.5f * (float)(TeamCombatActIndex - 1))
                                        {
                                            animator.SetInteger("Rush", 2);
                                            RushTimer = 0;//蓄力结束
                                        }
                                        break;
                                    //三人小队分身连招
                                    case WeavileTeamBrain.SubState.TeamState_304_CloneShadow:
                                        //终结冲刺
                                        if (TeamBrain.NowTeamState304CloneShadow == WeavileTeamBrain.TEAMSTATE304_CLONESHADOW.OverRush)
                                        {
                                            if (RushTimer >= TIME_CHARGE_RUSH_TEMASTATE304_CLONESHADOW_OVERRUSH )
                                            {
                                                animator.SetInteger("Rush", 2);
                                                RushTimer = 0;//蓄力结束
                                            }
                                        }
                                        //意外冲刺
                                        else if (TeamBrain.NowTeamState304CloneShadow == WeavileTeamBrain.TEAMSTATE304_CLONESHADOW.AccidentRush)
                                        {
                                            if (RushTimer >= TIME_CHARGE_RUSH_TEMASTATE304_CLONESHADOW_ACCIDENTRUSH + 1.5f * (float)(TeamCombatActIndex - 1))
                                            {
                                                animator.SetInteger("Rush", 2);
                                                RushTimer = 0;//蓄力结束
                                            }
                                        }
                                        break;
                                    //三人小队迷宫连招
                                    case WeavileTeamBrain.SubState.TeamState_305_Maze:
                                        if (RushTimer >= TIME_CHARGE_RUSH_TEMASTATE305_MAZE)
                                        {
                                            animator.SetInteger("Rush", 2);
                                            RushTimer = 0;//蓄力结束
                                        }
                                        break;
                                    //三人小队绕圈连招
                                    case WeavileTeamBrain.SubState.TeamState_306_CircleRun:
                                        switch (TeamBrain.NowTeamState306SubState)
                                        {
                                            case WeavileTeamBrain.TEAMSTATE306_SUBSTATE.InnerAtk1:
                                                if (RushTimer >= TIME_CHARGE_RUSH_TEMASTATE306_CIRCLERUN_INNERATK1)
                                                {
                                                    animator.SetInteger("Rush", 2);
                                                    RushTimer = 0;//蓄力结束
                                                }
                                                break;
                                            case WeavileTeamBrain.TEAMSTATE306_SUBSTATE.InnerAtk2:
                                                if (RushTimer >= TIME_CHARGE_RUSH_TEMASTATE306_CIRCLERUN_INNERATK1)
                                                {
                                                    animator.SetInteger("Rush", 2);
                                                    RushTimer = 0;//蓄力结束
                                                }
                                                break;
                                            case WeavileTeamBrain.TEAMSTATE306_SUBSTATE.OuterAtk:
                                                //外环攻击每只玛狃拉有延迟
                                                if (RushTimer >= TIME_CHARGE_RUSH_TEMASTATE306_CIRCLERUN_OUTERATK + 1.1f * (float)(TeamCombatActIndex - 1))
                                                {
                                                    animator.SetInteger("Rush", 2);
                                                    RushTimer = 0;//蓄力结束
                                                }
                                                break;
                                            case WeavileTeamBrain.TEAMSTATE306_SUBSTATE.EscapeAtk:
                                                if (RushTimer >= TIME_CHARGE_RUSH_TEMASTATE306_CIRCLERUN_ESCAPEATK)
                                                {
                                                    animator.SetInteger("Rush", 2);
                                                    RushTimer = 0;//蓄力结束
                                                }
                                                break;
                                        }
                                        break;
                                    //三人小队回血连招1
                                    case WeavileTeamBrain.SubState.TeamState_307_Heal1:
                                        if (RushTimer >= TIME_CHARGE_RUSH_TEMASTATE307_HEAL1)
                                        {
                                            animator.SetInteger("Rush", 2);
                                            RushTimer = 0;//蓄力结束
                                        }
                                        break;
                                    //三人小队万灵药连招
                                    case WeavileTeamBrain.SubState.TeamState_309_FullHeal:
                                        //每只玛狃拉有延迟
                                        if (RushTimer >= TIME_CHARGE_RUSH_TEMASTATE309_FULLHEAL + 1.1f * (float)(TeamCombatActIndex - 1))
                                        {
                                            animator.SetInteger("Rush", 2);
                                            RushTimer = 0;//蓄力结束
                                        }
                                        break;
                                    //二人小队近战连招
                                    case WeavileTeamBrain.SubState.TeamState_202_CloseAtk:
                                        if (RushTimer >= TIME_CHARGE_RUSH_TEMASTATE302_CLOSEATK)
                                        {
                                            animator.SetInteger("Rush", 2);
                                            RushTimer = 0;//蓄力结束
                                        }
                                        break;
                                    //二人小队替身连招
                                    case WeavileTeamBrain.SubState.TeamState_203_Substitute:
                                        if (RushTimer >= TIME_CHARGE_RUSH_TEMASTATE302_CLOSEATK + 1.5f * (float)(TeamCombatActIndex - 1))
                                        {
                                            animator.SetInteger("Rush", 2);
                                            RushTimer = 0;//蓄力结束
                                        }
                                        break;
                                    //二人小队替身连招2
                                    case WeavileTeamBrain.SubState.TeamState_204_Substitute2:
                                        if (RushTimer >= TIME_CHARGE_RUSH_TEMASTATE302_CLOSEATK + 1.5f * (float)(TeamCombatActIndex - 1))
                                        {
                                            animator.SetInteger("Rush", 2);
                                            RushTimer = 0;//蓄力结束
                                        }
                                        break;
                                    //二人小队绕圈连招
                                    case WeavileTeamBrain.SubState.TeamState_205_CircleRun:
                                        switch (TeamBrain.NowTeamState205SubState)
                                        {
                                            case WeavileTeamBrain.TEAMSTATE306_SUBSTATE.InnerAtk1:
                                                if (RushTimer >= TIME_CHARGE_RUSH_TEMASTATE306_CIRCLERUN_INNERATK1)
                                                {
                                                    animator.SetInteger("Rush", 2);
                                                    RushTimer = 0;//蓄力结束
                                                }
                                                break;
                                            case WeavileTeamBrain.TEAMSTATE306_SUBSTATE.InnerAtk2:
                                                if (RushTimer >= TIME_CHARGE_RUSH_TEMASTATE306_CIRCLERUN_INNERATK1)
                                                {
                                                    animator.SetInteger("Rush", 2);
                                                    RushTimer = 0;//蓄力结束
                                                }
                                                break;
                                            case WeavileTeamBrain.TEAMSTATE306_SUBSTATE.OuterAtk:
                                                //外环攻击每只玛狃拉有延迟
                                                if (RushTimer >= TIME_CHARGE_RUSH_TEMASTATE306_CIRCLERUN_OUTERATK + 1.1f * (float)(TeamCombatActIndex - 1))
                                                {
                                                    animator.SetInteger("Rush", 2);
                                                    RushTimer = 0;//蓄力结束
                                                }
                                                break;
                                            case WeavileTeamBrain.TEAMSTATE306_SUBSTATE.EscapeAtk:
                                                if (RushTimer >= TIME_CHARGE_RUSH_TEMASTATE306_CIRCLERUN_ESCAPEATK)
                                                {
                                                    animator.SetInteger("Rush", 2);
                                                    RushTimer = 0;//蓄力结束
                                                }
                                                break;
                                        }
                                        break;
                                    //二人小队迷宫连招
                                    case WeavileTeamBrain.SubState.TeamState_207_Maze:
                                        if (RushTimer >= TIME_CHARGE_RUSH_TEMASTATE305_MAZE)
                                        {
                                            animator.SetInteger("Rush", 2);
                                            RushTimer = 0;//蓄力结束
                                        }
                                        break;
                                    //二人小队万灵药连招
                                    case WeavileTeamBrain.SubState.TeamState_208_FullHeal:
                                        //每只玛狃拉有延迟
                                        if (RushTimer >= TIME_CHARGE_RUSH_TEMASTATE309_FULLHEAL + 1.1f * (float)(TeamCombatActIndex - 1))
                                        {
                                            animator.SetInteger("Rush", 2);
                                            RushTimer = 0;//蓄力结束
                                        }
                                        break;
                                    //一人小队 
                                    case WeavileTeamBrain.SubState.TeamState_100_Normal:
                                        switch (TeamBrain.NowTeamState100SubState)
                                        {
                                            case WeavileTeamBrain.TEAMSTATE100_SUBSTATE.FlingSmoke:
                                                RushOver();
                                                FlingStart();
                                                break;
                                            case WeavileTeamBrain.TEAMSTATE100_SUBSTATE.RunToSmoke:
                                                RushOver();
                                                RunStart();
                                                break;
                                            case WeavileTeamBrain.TEAMSTATE100_SUBSTATE.RunInSmoke:
                                                RushOver();
                                                RunStart();
                                                break;
                                            case WeavileTeamBrain.TEAMSTATE100_SUBSTATE.GuardInSmoke:
                                                RushOver();
                                                GuardStart();
                                                break;
                                            case WeavileTeamBrain.TEAMSTATE100_SUBSTATE.Rush:
                                                //可转向
                                                if (RushTimer >= TIME_CHARGE_RUSH_TEMASTATE100_NORMAL)
                                                {
                                                    animator.SetInteger("Rush", 2);
                                                    RushTimer = 0;//蓄力结束
                                                }
                                                break;
                                            case WeavileTeamBrain.TEAMSTATE100_SUBSTATE.FlingBomb:
                                                RushOver();
                                                FlingStart();
                                                break;
                                            case WeavileTeamBrain.TEAMSTATE100_SUBSTATE.Idle:
                                                RushOver();
                                                IdleStart(TIME_IDLE_TEAMSTATE100_NORMAL);
                                                break;
                                        }
                                        break;
                                }

                            }
                            //冲刺完毕后转进冲刺停止
                            if (isMove_Rush)
                            {
                                switch (TeamBrain.NowSubState)
                                {
                                    //三人小队_近战连招
                                    case WeavileTeamBrain.SubState.TeamState_302_CloseAtk:
                                        if (RushTimer >= TIME_RUSHMOVE_RUSH_TEMASTATE302_CLOSEATK) { animator.SetInteger("Rush", 3); RushTimer = 0; }
                                        break;
                                    //三人小队_替身连招
                                    case WeavileTeamBrain.SubState.TeamState_303_Substitute:
                                        if (RushTimer >= TIME_RUSHMOVE_RUSH_TEMASTATE302_CLOSEATK) { animator.SetInteger("Rush", 3); RushTimer = 0; }
                                        break;
                                    //三人小队_分身连招
                                    case WeavileTeamBrain.SubState.TeamState_304_CloneShadow:
                                        if (RushTimer >= TIME_RUSHMOVE_RUSH_TEMASTATE304_CLONESHADOW) { animator.SetInteger("Rush", 3); RushTimer = 0; }
                                        break;
                                    //三人小队_迷宫连招
                                    case WeavileTeamBrain.SubState.TeamState_305_Maze:
                                        if (RushTimer >= TIME_RUSHMOVE_RUSH_TEMASTATE305_MAZE) { animator.SetInteger("Rush", 3); RushTimer = 0; }
                                        break;
                                    //三人小队_绕圈连招
                                    case WeavileTeamBrain.SubState.TeamState_306_CircleRun:
                                        switch (TeamBrain.NowTeamState306SubState)
                                        {
                                            //内环攻击1 定点冲刺
                                            case WeavileTeamBrain.TEAMSTATE306_SUBSTATE.InnerAtk1:
                                                if (RushTimer >= Time_Rush_TeamState306CircleRun_InnerAtk) { animator.SetInteger("Rush", 3); RushTimer = 0; }
                                                break;
                                            //内环攻击2 定时冲刺
                                            case WeavileTeamBrain.TEAMSTATE306_SUBSTATE.InnerAtk2:
                                                if (RushTimer >= TIME_RUSHMOVE_RUSH_TEMASTATE306_CIRCLERUN_INNERATK2) { animator.SetInteger("Rush", 3); RushTimer = 0; }
                                                break;
                                            //外环攻击 定时冲刺
                                            case WeavileTeamBrain.TEAMSTATE306_SUBSTATE.OuterAtk:
                                                if (RushTimer >= TIME_RUSHMOVE_RUSH_TEMASTATE306_CIRCLERUN_OUTERATK) { animator.SetInteger("Rush", 3); RushTimer = 0; }
                                                break;
                                            //脱离攻击 定时冲刺
                                            case WeavileTeamBrain.TEAMSTATE306_SUBSTATE.EscapeAtk:
                                                if (RushTimer >= TIME_RUSHMOVE_RUSH_TEMASTATE306_CIRCLERUN_ESCAPEATK) { animator.SetInteger("Rush", 3); RushTimer = 0; }
                                                break;
                                        }
                                        break;
                                    //三人小队_回血连招1
                                    case WeavileTeamBrain.SubState.TeamState_307_Heal1:
                                        if (RushTimer >= TIME_RUSHMOVE_RUSH_TEMASTATE307_HEAL1) { animator.SetInteger("Rush", 3); RushTimer = 0; }
                                        break;
                                    //三人小队_万灵药连招
                                    case WeavileTeamBrain.SubState.TeamState_309_FullHeal:
                                        if (RushTimer >= TIME_RUSHMOVE_RUSH_TEMASTATE302_CLOSEATK) { animator.SetInteger("Rush", 3); RushTimer = 0; }
                                        break;
                                    //二人小队_近战连招
                                    case WeavileTeamBrain.SubState.TeamState_202_CloseAtk:
                                        if (RushTimer >= TIME_RUSHMOVE_RUSH_TEMASTATE302_CLOSEATK) { animator.SetInteger("Rush", 3); RushTimer = 0; }
                                        break;
                                    //二人小队_替身连招
                                    case WeavileTeamBrain.SubState.TeamState_203_Substitute:
                                        if (RushTimer >= TIME_RUSHMOVE_RUSH_TEMASTATE302_CLOSEATK) { animator.SetInteger("Rush", 3); RushTimer = 0; }
                                        break;
                                    //二人小队_替身连招2
                                    case WeavileTeamBrain.SubState.TeamState_204_Substitute2:
                                        if (RushTimer >= TIME_RUSHMOVE_RUSH_TEMASTATE302_CLOSEATK) { animator.SetInteger("Rush", 3); RushTimer = 0; }
                                        break;
                                    //二人小队_绕圈连招
                                    case WeavileTeamBrain.SubState.TeamState_205_CircleRun:
                                        switch (TeamBrain.NowTeamState205SubState)
                                        {
                                            //内环攻击1 定点冲刺
                                            case WeavileTeamBrain.TEAMSTATE306_SUBSTATE.InnerAtk1:
                                                if (RushTimer >= Time_Rush_TeamState205CircleRun_InnerAtk) { animator.SetInteger("Rush", 3); RushTimer = 0; }
                                                break;
                                            //内环攻击2 定时冲刺
                                            case WeavileTeamBrain.TEAMSTATE306_SUBSTATE.InnerAtk2:
                                                if (RushTimer >= TIME_RUSHMOVE_RUSH_TEMASTATE306_CIRCLERUN_INNERATK2) { animator.SetInteger("Rush", 3); RushTimer = 0; }
                                                break;
                                            //外环攻击 定时冲刺
                                            case WeavileTeamBrain.TEAMSTATE306_SUBSTATE.OuterAtk:
                                                if (RushTimer >= TIME_RUSHMOVE_RUSH_TEMASTATE306_CIRCLERUN_OUTERATK) { animator.SetInteger("Rush", 3); RushTimer = 0; }
                                                break;
                                            //脱离攻击 定时冲刺
                                            case WeavileTeamBrain.TEAMSTATE306_SUBSTATE.EscapeAtk:
                                                if (RushTimer >= TIME_RUSHMOVE_RUSH_TEMASTATE306_CIRCLERUN_ESCAPEATK) { animator.SetInteger("Rush", 3); RushTimer = 0; }
                                                break;
                                        }
                                        break;
                                    //二人小队_迷宫连招
                                    case WeavileTeamBrain.SubState.TeamState_207_Maze:
                                        if (RushTimer >= TIME_RUSHMOVE_RUSH_TEMASTATE305_MAZE) { animator.SetInteger("Rush", 3); RushTimer = 0; }
                                        break;
                                    //二人小队_万灵药连招
                                    case WeavileTeamBrain.SubState.TeamState_208_FullHeal:
                                        if (RushTimer >= TIME_RUSHMOVE_RUSH_TEMASTATE302_CLOSEATK) { animator.SetInteger("Rush", 3); RushTimer = 0; }
                                        break;
                                    //一人小队 
                                    case WeavileTeamBrain.SubState.TeamState_100_Normal:
                                        switch (TeamBrain.NowTeamState100SubState)
                                        {
                                            case WeavileTeamBrain.TEAMSTATE100_SUBSTATE.FlingSmoke:
                                                RushOver();
                                                FlingStart();
                                                break;
                                            case WeavileTeamBrain.TEAMSTATE100_SUBSTATE.RunToSmoke:
                                                RushOver();
                                                RunStart();
                                                break;
                                            case WeavileTeamBrain.TEAMSTATE100_SUBSTATE.RunInSmoke:
                                                RushOver();
                                                RunStart();
                                                break;
                                            case WeavileTeamBrain.TEAMSTATE100_SUBSTATE.GuardInSmoke:
                                                RushOver();
                                                GuardStart();
                                                break;
                                            case WeavileTeamBrain.TEAMSTATE100_SUBSTATE.Rush:
                                                if (RushTimer >= TIME_RUSHMOVE_RUSH_TEMASTATE302_CLOSEATK) { animator.SetInteger("Rush", 3); RushTimer = 0; }
                                                break;
                                            case WeavileTeamBrain.TEAMSTATE100_SUBSTATE.FlingBomb:
                                                RushOver();
                                                FlingStart();
                                                break;
                                            case WeavileTeamBrain.TEAMSTATE100_SUBSTATE.Idle:
                                                RushOver();
                                                IdleStart(TIME_IDLE_TEAMSTATE100_NORMAL);
                                                break;
                                        }
                                        break;
                                }
                            }
                            //冲刺
                            if (isMove_Rush)
                            {
                                switch (TeamBrain.NowSubState)
                                {
                                    //三人小队_近战连招
                                    case WeavileTeamBrain.SubState.TeamState_302_CloseAtk:
                                        //冲刺沿途效果
                                        switch (NowRushType)
                                        {
                                            //冲刺为恶系时 沿途释放小恶波
                                            case RushType.Dark:
                                                Timer_Rush_MoveEffect_Interval += Time.deltaTime;
                                                if (Timer_Rush_MoveEffect_Interval >= TIME_INTERVAL_RUSH_TEMASTATE302_CLOSEATK_SMALLDARKPULSE)
                                                {
                                                    Timer_Rush_MoveEffect_Interval = 0.0f;
                                                    LaunchDarkPulseSmall(transform.position);
                                                }
                                                break;
                                            //冲刺为毒系时 沿途释放毒雾
                                            case RushType.Posion:
                                                Timer_Rush_MoveEffect_Interval += Time.deltaTime;
                                                if (Timer_Rush_MoveEffect_Interval >= TIME_INTERVAL_RUSH_TEMASTATE302_CLOSEATK_POSIONMIST)
                                                {
                                                    Timer_Rush_MoveEffect_Interval = 0.0f;
                                                    LaunchPosionMist(Dir_Rush);
                                                }
                                                break;
                                        }
                                        MoveBySpeedAndDir(Dir_Rush, speed, SPEEDALPHA_RUSH_TEMASTATE302_CLOSEATK, 0.0f, 0.0f, 0.0f, 0.0f);
                                        break;
                                    //三人小队_替身连招
                                    case WeavileTeamBrain.SubState.TeamState_303_Substitute:
                                        //冲刺沿途效果
                                        switch (NowRushType)
                                        {
                                            //冲刺为恶系时 沿途释放小恶波
                                            case RushType.Dark:
                                                Timer_Rush_MoveEffect_Interval += Time.deltaTime;
                                                if (Timer_Rush_MoveEffect_Interval >= TIME_INTERVAL_RUSH_TEMASTATE302_CLOSEATK_SMALLDARKPULSE)
                                                {
                                                    Timer_Rush_MoveEffect_Interval = 0.0f;
                                                    LaunchDarkPulseSmall(transform.position);
                                                }
                                                break;
                                            //冲刺为毒系时 沿途释放毒雾
                                            case RushType.Posion:
                                                Timer_Rush_MoveEffect_Interval += Time.deltaTime;
                                                if (Timer_Rush_MoveEffect_Interval >= TIME_INTERVAL_RUSH_TEMASTATE302_CLOSEATK_POSIONMIST)
                                                {
                                                    Timer_Rush_MoveEffect_Interval = 0.0f;
                                                    LaunchPosionMist(Dir_Rush);
                                                }
                                                break;
                                        }
                                        MoveBySpeedAndDir(Dir_Rush, speed, SPEEDALPHA_RUSH_TEMASTATE302_CLOSEATK, 0.0f, 0.0f, 0.0f, 0.0f);
                                        break;
                                    //三人小队_分身连招
                                    case WeavileTeamBrain.SubState.TeamState_304_CloneShadow:
                                        switch (TeamBrain.NowTeamState304CloneShadow)
                                        {
                                            //终结冲刺 沿途放微小恶波
                                            case WeavileTeamBrain.TEAMSTATE304_CLONESHADOW.OverRush:
                                                //冲刺沿途效果
                                                switch (NowRushType)
                                                {
                                                    //冲刺为恶系时 沿途释放小恶波
                                                    case RushType.Dark:
                                                        Timer_Rush_MoveEffect_Interval += Time.deltaTime;
                                                        if (Timer_Rush_MoveEffect_Interval >= TIME_INTERVAL_RUSH_TEMASTATE304_CLONESHADOW_SMALLDARKPULSE)
                                                        {
                                                            Timer_Rush_MoveEffect_Interval = 0.0f;
                                                            LaunchDarkPulseTiny(transform.position);
                                                        }
                                                        break;
                                                    //冲刺为毒系时 沿途释放毒雾
                                                    case RushType.Posion:
                                                        Timer_Rush_MoveEffect_Interval += Time.deltaTime;
                                                        if (Timer_Rush_MoveEffect_Interval >= TIME_INTERVAL_RUSH_TEMASTATE304_CLONESHADOW_SMALLDARKPULSE)
                                                        {
                                                            Timer_Rush_MoveEffect_Interval = 0.0f;
                                                            LaunchPosionMist(Dir_Rush);
                                                        }
                                                        break;
                                                }
                                                break;
                                            //意外冲刺 沿途放小恶波
                                            case WeavileTeamBrain.TEAMSTATE304_CLONESHADOW.AccidentRush:
                                                //冲刺沿途效果
                                                switch (NowRushType)
                                                {
                                                    //冲刺为恶系时 沿途释放小恶波
                                                    case RushType.Dark:
                                                        Timer_Rush_MoveEffect_Interval += Time.deltaTime;
                                                        if (Timer_Rush_MoveEffect_Interval >= TIME_INTERVAL_RUSH_TEMASTATE302_CLOSEATK_SMALLDARKPULSE)
                                                        {
                                                            Timer_Rush_MoveEffect_Interval = 0.0f;
                                                            LaunchDarkPulseSmall(transform.position);
                                                        }
                                                        break;
                                                    //冲刺为毒系时 沿途释放毒雾
                                                    case RushType.Posion:
                                                        Timer_Rush_MoveEffect_Interval += Time.deltaTime;
                                                        if (Timer_Rush_MoveEffect_Interval >= TIME_INTERVAL_RUSH_TEMASTATE302_CLOSEATK_POSIONMIST)
                                                        {
                                                            Timer_Rush_MoveEffect_Interval = 0.0f;
                                                            LaunchPosionMist(Dir_Rush);
                                                        }
                                                        break;
                                                }
                                                break;
                                        }
                                        MoveBySpeedAndDir(Dir_Rush, speed, SPEEDALPHA_RUSH_TEMASTATE302_CLOSEATK, 0.0f, 0.0f, 0.0f, 0.0f);
                                        break;
                                    //三人小队_迷宫连招
                                    case WeavileTeamBrain.SubState.TeamState_305_Maze:
                                        //冲刺沿途效果
                                        switch (NowRushType)
                                        {
                                            //冲刺为恶系时 沿途释放小恶波
                                            case RushType.Dark:
                                                Timer_Rush_MoveEffect_Interval += Time.deltaTime;
                                                if (Timer_Rush_MoveEffect_Interval >= TIME_INTERVAL_RUSH_TEMASTATE302_CLOSEATK_SMALLDARKPULSE)
                                                {
                                                    Timer_Rush_MoveEffect_Interval = 0.0f;
                                                    LaunchDarkPulseSmall(transform.position);
                                                }
                                                break;
                                            //冲刺为毒系时 沿途释放毒雾
                                            case RushType.Posion:
                                                Timer_Rush_MoveEffect_Interval += Time.deltaTime;
                                                if (Timer_Rush_MoveEffect_Interval >= TIME_INTERVAL_RUSH_TEMASTATE302_CLOSEATK_POSIONMIST)
                                                {
                                                    Timer_Rush_MoveEffect_Interval = 0.0f;
                                                    LaunchPosionMist(Dir_Rush);
                                                }
                                                break;
                                        }
                                        MoveBySpeedAndDir(Dir_Rush, speed, SPEEDALPHA_RUSH_TEMASTATE302_CLOSEATK, 0.0f, 0.0f, 0.0f, 0.0f);
                                        break;
                                    //三人小队_绕圈连招
                                    case WeavileTeamBrain.SubState.TeamState_306_CircleRun:
                                        //冲刺沿途效果
                                        switch (NowRushType)
                                        {
                                            //冲刺为恶系时 沿途释放小恶波
                                            case RushType.Dark:
                                                Timer_Rush_MoveEffect_Interval += Time.deltaTime;
                                                if (Timer_Rush_MoveEffect_Interval >= TIME_INTERVAL_RUSH_TEMASTATE302_CLOSEATK_SMALLDARKPULSE)
                                                {
                                                    Timer_Rush_MoveEffect_Interval = 0.0f;
                                                    LaunchDarkPulseSmall(transform.position);
                                                }
                                                break;
                                            //冲刺为毒系时 沿途释放毒雾
                                            case RushType.Posion:
                                                Timer_Rush_MoveEffect_Interval += Time.deltaTime;
                                                if (Timer_Rush_MoveEffect_Interval >= TIME_INTERVAL_RUSH_TEMASTATE302_CLOSEATK_POSIONMIST)
                                                {
                                                    Timer_Rush_MoveEffect_Interval = 0.0f;
                                                    LaunchPosionMist(Dir_Rush);
                                                }
                                                break;
                                        }
                                        if (TeamBrain.NowTeamState306SubState == WeavileTeamBrain.TEAMSTATE306_SUBSTATE.OuterAtk) { MoveBySpeedAndDir(Dir_Rush, speed, SPEEDALPHA_RUSH_TEMASTATE306_CIRCLERUN_OUTERATK, 0.0f, 0.0f, 0.0f, 0.0f); }
                                        else { MoveBySpeedAndDir(Dir_Rush, speed, SPEEDALPHA_RUSH_TEMASTATE306_CIRCLERUN_INNERATK, 0.0f, 0.0f, 0.0f, 0.0f); }
                                        break;
                                    //三人小队_回血连招1
                                    case WeavileTeamBrain.SubState.TeamState_307_Heal1:
                                        //冲刺沿途效果
                                        switch (NowRushType)
                                        {
                                            //冲刺为恶系时 沿途释放小恶波
                                            case RushType.Dark:
                                                Timer_Rush_MoveEffect_Interval += Time.deltaTime;
                                                if (Timer_Rush_MoveEffect_Interval >= TIME_INTERVAL_RUSH_TEMASTATE302_CLOSEATK_SMALLDARKPULSE)
                                                {
                                                    Timer_Rush_MoveEffect_Interval = 0.0f;
                                                    LaunchDarkPulseSmall(transform.position);
                                                }
                                                break;
                                            //冲刺为毒系时 沿途释放毒雾
                                            case RushType.Posion:
                                                Timer_Rush_MoveEffect_Interval += Time.deltaTime;
                                                if (Timer_Rush_MoveEffect_Interval >= TIME_INTERVAL_RUSH_TEMASTATE302_CLOSEATK_POSIONMIST)
                                                {
                                                    Timer_Rush_MoveEffect_Interval = 0.0f;
                                                    LaunchPosionMist(Dir_Rush);
                                                }
                                                break;
                                        }
                                        MoveBySpeedAndDir(Dir_Rush, speed, SPEEDALPHA_RUSH_TEMASTATE302_CLOSEATK, 0.0f, 0.0f, 0.0f, 0.0f);
                                        break;
                                    //二人小队_近战连招
                                    case WeavileTeamBrain.SubState.TeamState_202_CloseAtk:
                                        //冲刺沿途效果
                                        switch (NowRushType)
                                        {
                                            //冲刺为恶系时 沿途释放小恶波
                                            case RushType.Dark:
                                                Timer_Rush_MoveEffect_Interval += Time.deltaTime;
                                                if (Timer_Rush_MoveEffect_Interval >= TIME_INTERVAL_RUSH_TEMASTATE302_CLOSEATK_SMALLDARKPULSE)
                                                {
                                                    Timer_Rush_MoveEffect_Interval = 0.0f;
                                                    LaunchDarkPulseSmall(transform.position);
                                                }
                                                break;
                                            //冲刺为毒系时 沿途释放毒雾
                                            case RushType.Posion:
                                                Timer_Rush_MoveEffect_Interval += Time.deltaTime;
                                                if (Timer_Rush_MoveEffect_Interval >= TIME_INTERVAL_RUSH_TEMASTATE302_CLOSEATK_POSIONMIST)
                                                {
                                                    Timer_Rush_MoveEffect_Interval = 0.0f;
                                                    LaunchPosionMist(Dir_Rush);
                                                }
                                                break;
                                        }
                                        MoveBySpeedAndDir(Dir_Rush, speed, SPEEDALPHA_RUSH_TEMASTATE302_CLOSEATK, 0.0f, 0.0f, 0.0f, 0.0f);
                                        break;
                                    //三人小队_万灵药连招
                                    case WeavileTeamBrain.SubState.TeamState_309_FullHeal:
                                        //冲刺沿途效果
                                        switch (NowRushType)
                                        {
                                            //冲刺为恶系时 沿途释放小恶波
                                            case RushType.Dark:
                                                Timer_Rush_MoveEffect_Interval += Time.deltaTime;
                                                if (Timer_Rush_MoveEffect_Interval >= TIME_INTERVAL_RUSH_TEMASTATE302_CLOSEATK_SMALLDARKPULSE)
                                                {
                                                    Timer_Rush_MoveEffect_Interval = 0.0f;
                                                    LaunchDarkPulseSmall(transform.position);
                                                }
                                                break;
                                            //冲刺为毒系时 沿途释放毒雾
                                            case RushType.Posion:
                                                Timer_Rush_MoveEffect_Interval += Time.deltaTime;
                                                if (Timer_Rush_MoveEffect_Interval >= TIME_INTERVAL_RUSH_TEMASTATE302_CLOSEATK_POSIONMIST)
                                                {
                                                    Timer_Rush_MoveEffect_Interval = 0.0f;
                                                    LaunchPosionMist(Dir_Rush);
                                                }
                                                break;
                                        }
                                        MoveBySpeedAndDir(Dir_Rush, speed, SPEEDALPHA_RUSH_TEMASTATE302_CLOSEATK, 0.0f, 0.0f, 0.0f, 0.0f);
                                        break;
                                    //二人小队_替身连招
                                    case WeavileTeamBrain.SubState.TeamState_203_Substitute:
                                        //冲刺沿途效果
                                        switch (NowRushType)
                                        {
                                            //冲刺为恶系时 沿途释放小恶波
                                            case RushType.Dark:
                                                Timer_Rush_MoveEffect_Interval += Time.deltaTime;
                                                if (Timer_Rush_MoveEffect_Interval >= TIME_INTERVAL_RUSH_TEMASTATE302_CLOSEATK_SMALLDARKPULSE)
                                                {
                                                    Timer_Rush_MoveEffect_Interval = 0.0f;
                                                    LaunchDarkPulseSmall(transform.position);
                                                }
                                                break;
                                            //冲刺为毒系时 沿途释放毒雾
                                            case RushType.Posion:
                                                Timer_Rush_MoveEffect_Interval += Time.deltaTime;
                                                if (Timer_Rush_MoveEffect_Interval >= TIME_INTERVAL_RUSH_TEMASTATE302_CLOSEATK_POSIONMIST)
                                                {
                                                    Timer_Rush_MoveEffect_Interval = 0.0f;
                                                    LaunchPosionMist(Dir_Rush);
                                                }
                                                break;
                                        }
                                        MoveBySpeedAndDir(Dir_Rush, speed, SPEEDALPHA_RUSH_TEMASTATE302_CLOSEATK, 0.0f, 0.0f, 0.0f, 0.0f);
                                        break;
                                    //二人小队_替身连招
                                    case WeavileTeamBrain.SubState.TeamState_204_Substitute2:
                                        //冲刺沿途效果
                                        switch (NowRushType)
                                        {
                                            //冲刺为恶系时 沿途释放小恶波
                                            case RushType.Dark:
                                                Timer_Rush_MoveEffect_Interval += Time.deltaTime;
                                                if (Timer_Rush_MoveEffect_Interval >= TIME_INTERVAL_RUSH_TEMASTATE302_CLOSEATK_SMALLDARKPULSE)
                                                {
                                                    Timer_Rush_MoveEffect_Interval = 0.0f;
                                                    LaunchDarkPulseSmall(transform.position);
                                                }
                                                break;
                                            //冲刺为毒系时 沿途释放毒雾
                                            case RushType.Posion:
                                                Timer_Rush_MoveEffect_Interval += Time.deltaTime;
                                                if (Timer_Rush_MoveEffect_Interval >= TIME_INTERVAL_RUSH_TEMASTATE302_CLOSEATK_POSIONMIST)
                                                {
                                                    Timer_Rush_MoveEffect_Interval = 0.0f;
                                                    LaunchPosionMist(Dir_Rush);
                                                }
                                                break;
                                        }
                                        MoveBySpeedAndDir(Dir_Rush, speed, SPEEDALPHA_RUSH_TEMASTATE302_CLOSEATK, 0.0f, 0.0f, 0.0f, 0.0f);
                                        break;
                                    //二人小队_绕圈连招
                                    case WeavileTeamBrain.SubState.TeamState_205_CircleRun:
                                        //冲刺沿途效果
                                        switch (NowRushType)
                                        {
                                            //冲刺为恶系时 沿途释放小恶波
                                            case RushType.Dark:
                                                Timer_Rush_MoveEffect_Interval += Time.deltaTime;
                                                if (Timer_Rush_MoveEffect_Interval >= TIME_INTERVAL_RUSH_TEMASTATE302_CLOSEATK_SMALLDARKPULSE)
                                                {
                                                    Timer_Rush_MoveEffect_Interval = 0.0f;
                                                    LaunchDarkPulseSmall(transform.position);
                                                }
                                                break;
                                            //冲刺为毒系时 沿途释放毒雾
                                            case RushType.Posion:
                                                Timer_Rush_MoveEffect_Interval += Time.deltaTime;
                                                if (Timer_Rush_MoveEffect_Interval >= TIME_INTERVAL_RUSH_TEMASTATE302_CLOSEATK_POSIONMIST)
                                                {
                                                    Timer_Rush_MoveEffect_Interval = 0.0f;
                                                    LaunchPosionMist(Dir_Rush);
                                                }
                                                break;
                                        }
                                        if (TeamBrain.NowTeamState205SubState == WeavileTeamBrain.TEAMSTATE306_SUBSTATE.OuterAtk) { MoveBySpeedAndDir(Dir_Rush, speed, SPEEDALPHA_RUSH_TEMASTATE306_CIRCLERUN_OUTERATK, 0.0f, 0.0f, 0.0f, 0.0f); }
                                        else { MoveBySpeedAndDir(Dir_Rush, speed, SPEEDALPHA_RUSH_TEMASTATE306_CIRCLERUN_INNERATK, 0.0f, 0.0f, 0.0f, 0.0f); }
                                        break;
                                    //二人小队_迷宫连招
                                    case WeavileTeamBrain.SubState.TeamState_207_Maze:
                                        //冲刺沿途效果
                                        switch (NowRushType)
                                        {
                                            //冲刺为恶系时 沿途释放小恶波
                                            case RushType.Dark:
                                                Timer_Rush_MoveEffect_Interval += Time.deltaTime;
                                                if (Timer_Rush_MoveEffect_Interval >= TIME_INTERVAL_RUSH_TEMASTATE302_CLOSEATK_SMALLDARKPULSE)
                                                {
                                                    Timer_Rush_MoveEffect_Interval = 0.0f;
                                                    LaunchDarkPulseSmall(transform.position);
                                                }
                                                break;
                                            //冲刺为毒系时 沿途释放毒雾
                                            case RushType.Posion:
                                                Timer_Rush_MoveEffect_Interval += Time.deltaTime;
                                                if (Timer_Rush_MoveEffect_Interval >= TIME_INTERVAL_RUSH_TEMASTATE302_CLOSEATK_POSIONMIST)
                                                {
                                                    Timer_Rush_MoveEffect_Interval = 0.0f;
                                                    LaunchPosionMist(Dir_Rush);
                                                }
                                                break;
                                        }
                                        MoveBySpeedAndDir(Dir_Rush, speed, SPEEDALPHA_RUSH_TEMASTATE302_CLOSEATK, 0.0f, 0.0f, 0.0f, 0.0f);
                                        break;
                                    //二人小队_万灵药连招
                                    case WeavileTeamBrain.SubState.TeamState_208_FullHeal:
                                        //冲刺沿途效果
                                        switch (NowRushType)
                                        {
                                            //冲刺为恶系时 沿途释放小恶波
                                            case RushType.Dark:
                                                Timer_Rush_MoveEffect_Interval += Time.deltaTime;
                                                if (Timer_Rush_MoveEffect_Interval >= TIME_INTERVAL_RUSH_TEMASTATE302_CLOSEATK_SMALLDARKPULSE)
                                                {
                                                    Timer_Rush_MoveEffect_Interval = 0.0f;
                                                    LaunchDarkPulseSmall(transform.position);
                                                }
                                                break;
                                            //冲刺为毒系时 沿途释放毒雾
                                            case RushType.Posion:
                                                Timer_Rush_MoveEffect_Interval += Time.deltaTime;
                                                if (Timer_Rush_MoveEffect_Interval >= TIME_INTERVAL_RUSH_TEMASTATE302_CLOSEATK_POSIONMIST)
                                                {
                                                    Timer_Rush_MoveEffect_Interval = 0.0f;
                                                    LaunchPosionMist(Dir_Rush);
                                                }
                                                break;
                                        }
                                        MoveBySpeedAndDir(Dir_Rush, speed, SPEEDALPHA_RUSH_TEMASTATE302_CLOSEATK, 0.0f, 0.0f, 0.0f, 0.0f);
                                        break;
                                    //一人小队 
                                    case WeavileTeamBrain.SubState.TeamState_100_Normal:
                                        switch (TeamBrain.NowTeamState100SubState)
                                        {
                                            case WeavileTeamBrain.TEAMSTATE100_SUBSTATE.FlingSmoke:
                                                RushOver();
                                                FlingStart();
                                                break;
                                            case WeavileTeamBrain.TEAMSTATE100_SUBSTATE.RunToSmoke:
                                                RushOver();
                                                RunStart();
                                                break;
                                            case WeavileTeamBrain.TEAMSTATE100_SUBSTATE.RunInSmoke:
                                                RushOver();
                                                RunStart();
                                                break;
                                            case WeavileTeamBrain.TEAMSTATE100_SUBSTATE.GuardInSmoke:
                                                RushOver();
                                                GuardStart();
                                                break;
                                            case WeavileTeamBrain.TEAMSTATE100_SUBSTATE.Rush:
                                                //冲刺沿途效果
                                                switch (NowRushType)
                                                {
                                                    //冲刺为恶系时 沿途释放小恶波
                                                    case RushType.Dark:
                                                        Timer_Rush_MoveEffect_Interval += Time.deltaTime;
                                                        if (Timer_Rush_MoveEffect_Interval >= TIME_INTERVAL_RUSH_TEMASTATE302_CLOSEATK_SMALLDARKPULSE)
                                                        {
                                                            Timer_Rush_MoveEffect_Interval = 0.0f;
                                                            LaunchDarkPulseSmall(transform.position);
                                                        }
                                                        break;
                                                    //冲刺为毒系时 沿途释放毒雾
                                                    case RushType.Posion:
                                                        Timer_Rush_MoveEffect_Interval += Time.deltaTime;
                                                        if (Timer_Rush_MoveEffect_Interval >= TIME_INTERVAL_RUSH_TEMASTATE302_CLOSEATK_POSIONMIST)
                                                        {
                                                            Timer_Rush_MoveEffect_Interval = 0.0f;
                                                            LaunchPosionMist(Dir_Rush);
                                                        }
                                                        break;
                                                }
                                                MoveBySpeedAndDir(Dir_Rush, speed, SPEEDALPHA_RUSH_TEMASTATE302_CLOSEATK, 0.0f, 0.0f, 0.0f, 0.0f);
                                                break;
                                            case WeavileTeamBrain.TEAMSTATE100_SUBSTATE.FlingBomb:
                                                RushOver();
                                                FlingStart();
                                                break;
                                            case WeavileTeamBrain.TEAMSTATE100_SUBSTATE.Idle:
                                                RushOver();
                                                IdleStart(TIME_IDLE_TEAMSTATE100_NORMAL);
                                                break;
                                        }
                                        break;

                                }
                            }
                            // 调整预判冲刺角度指示线
                            if (isPredict_RushAngle)
                            {
                                float Angle_PredictLastAngle_Rush02 = _mTool.Angle_360Y((TargetPosition - (Vector2)transform.position).normalized, Vector2.right);
                                float PredictAngle = 2.0f * Angle_PredictLastAngle_Rush02 - Angle_PredictLastAngle_Rush;
                                Vector2 d = Quaternion.AngleAxis(PredictAngle, Vector3.forward) * Vector3.right;
                                if (RushArrowObj != null) { RushArrowObj.SetTarget(ParentPokemonRoom.GetRoomBoundaryByRaycastTo(transform.position, d)); }
                            }
                        }
                        break;
                    //原地防守状态
                    case MainState.Guard:
                        {
                            //判断主脑 主脑为其他状态时转出
                            switch (TeamBrain.NowSubState)
                            {
                                //三人小队发呆状态 守护状态的玛狃拉去守护还在发呆的玛狃拉
                                case WeavileTeamBrain.SubState.TeamState_300_Idle:
                                    {
                                        List<Weavile> wl = new List<Weavile> { };
                                        Vector2 GrundTargetPosition = Vector2.zero;
                                        if (TeamBrain.WeavileMembersList[0] != this && TeamBrain.WeavileMembersList[0].NowMainState == MainState.Idle) { wl.Add(TeamBrain.WeavileMembersList[0]); }
                                        if (TeamBrain.WeavileMembersList[1] != this && TeamBrain.WeavileMembersList[1].NowMainState == MainState.Idle) { wl.Add(TeamBrain.WeavileMembersList[1]); }
                                        if (TeamBrain.WeavileMembersList[2] != this && TeamBrain.WeavileMembersList[2].NowMainState == MainState.Idle) { wl.Add(TeamBrain.WeavileMembersList[2]); }

                                        float dis1 = 9999.0f;
                                        float dis2 = 9999.0f;
                                        if (wl.Count == 0) { Debug.Log("Error:三人小队 发呆状态 已经没有人在发呆"); break; }
                                        else if (wl.Count == 1) { dis1 = Vector2.Distance(transform.position, wl[0].transform.position); }
                                        else if (wl.Count == 2) { dis1 = Vector2.Distance(transform.position, wl[0].transform.position); dis2 = Vector2.Distance(transform.position, wl[1].transform.position); }
                                        else { Debug.Log("Error:三人小队 发呆状态 队伍人数错误"); }
                                        if (dis1 <= dis2) { GrundTargetPosition = wl[0].transform.position; }
                                        else { GrundTargetPosition = wl[1].transform.position; }

                                        //和守护目标距离与规定距离之间的差 帮助防守
                                        if (Mathf.Abs(Vector2.Distance(GrundTargetPosition, (Vector2)transform.position) - DISTENCE_GUARD_BETWEEN_TARGET_TEAMSTATE300IDLE) <= 0.3f)
                                        {
                                            isMove_Guard = false;
                                            //速度为0 看向目标
                                            animator.SetFloat("Speed", 0.0f);
                                            SetDirector(_mTool.MainVector2((TargetPosition - (Vector2)transform.position).normalized));
                                            //触发冰粒发射
                                            if (Vector2.Distance(TargetPosition, (Vector2)transform.position) <= DISTENCE_GUARD_LAUNCH_ICESHARD_TEAMSTATE300IDLE)
                                            {
                                                GuardOver();
                                                IceShardStart(TIME_ICESHARD_TEMASTATE300IDLE_GUARD);
                                            }
                                            //触发恶波发射
                                            if (Vector2.Distance(TargetPosition, (Vector2)transform.position) <= DISTENCE_GUARD_LAUNCH_DARKPULSE_TEAMSTATE300IDLE)
                                            {
                                                GuardOver();
                                                DarkPulseStart(TIME_DARKPULSE_TEMASTATE300IDLE_GUARD);
                                            }

                                        }
                                        //和守护目标距离和规定距离的差大于一个微小值 接近守护目标
                                        else
                                        {
                                            //在规定距离外 接近被守护者
                                            if (Vector2.Distance(GrundTargetPosition, (Vector2)transform.position) > DISTENCE_GUARD_BETWEEN_TARGET_TEAMSTATE300IDLE)
                                            {
                                                isMove_Guard = true;
                                                Dir_Guard_Move = (GrundTargetPosition - (Vector2)transform.position).normalized;
                                                Dir_Guard_Move = Quaternion.AngleAxis(-12 + ((float)(teamCombatActIndex - 1)) * 12, Vector3.forward) * Dir_Guard_Move;
                                                MoveBySpeedAndDir(Dir_Guard_Move, speed, SPEEDALPHA_GUARD_TEAMSTATE300IDLE, 0.0f, 0.0f, 0.0f, 0.0f);
                                            }
                                            //在规定距离内 远离被守护者
                                            else
                                            {
                                                isMove_Guard = true;
                                                Dir_Guard_Move = -(GrundTargetPosition - (Vector2)transform.position).normalized;
                                                Dir_Guard_Move = Quaternion.AngleAxis(12 - ((float)(teamCombatActIndex - 1)) * 12, Vector3.forward) * Dir_Guard_Move;
                                                MoveBySpeedAndDir(Dir_Guard_Move, speed, SPEEDALPHA_GUARD_TEAMSTATE300IDLE, 0.0f, 0.0f, 0.0f, 0.0f);
                                            }

                                        }

                                    }
                                    break;
                                //三人小队奔跑状态
                                case WeavileTeamBrain.SubState.TeamState_301_Run:
                                    {
                                        GuardOver();//防守结束
                                        RunStart();//开始奔跑
                                    }
                                    break;
                                //三人小队近战状态
                                case WeavileTeamBrain.SubState.TeamState_302_CloseAtk:
                                    {
                                        GuardOver();//防守结束
                                                    //根据序列号转入近战分支动作
                                        switch (TeamCombatActIndex)
                                        {
                                            case 1: RushStart(); break;
                                            case 2: JumpMoveStart(); break;
                                            case 3: UseProtectStart(TIME_USEPROTECT_TEMASTATE302_CLOSEATK); break;
                                        }
                                    }
                                    break;
                                //三人小队替身连招
                                case WeavileTeamBrain.SubState.TeamState_303_Substitute:
                                    {
                                        GuardOver();//防守结束
                                        switch (TeamCombatActIndex)
                                        {
                                            case 1:
                                                FlingStart();//01投掷替身
                                                break;
                                            case 2:
                                                JumpMoveStart();//0203跳跃接近01
                                                break;
                                            case 3:
                                                JumpMoveStart();//0203跳跃接近01
                                                break;
                                        }
                                    }
                                    break;
                                //三人小队分身连招
                                case WeavileTeamBrain.SubState.TeamState_304_CloneShadow:
                                    {
                                        GuardOver();//防守结束
                                        CloneShadowStart(TIME_CLONESHADOW_TEAMSTATE_304_CLONESHADOW_IDLE);
                                    }
                                    break;
                                //三人小队迷宫连招
                                case WeavileTeamBrain.SubState.TeamState_305_Maze:
                                    {
                                        GuardOver();//防守结束
                                        JumpMoveStart();
                                    }
                                    break;
                                //三人小队绕圈连招
                                case WeavileTeamBrain.SubState.TeamState_306_CircleRun:
                                    {
                                        GuardOver();//防守结束
                                        CircleRunStart();
                                    }
                                    break;
                                //三人小队回血连招
                                case WeavileTeamBrain.SubState.TeamState_307_Heal1:
                                    {
                                        GuardOver();//防守结束
                                        switch (TeamCombatActIndex)
                                        {
                                            case 1:
                                                RunOver();
                                                JumpMoveStart();
                                                break;
                                            case 2:
                                                RunOver();
                                                JumpMoveStart();
                                                break;
                                            case 3:
                                                RunOver();
                                                RushStart();
                                                break;
                                        }
                                    }
                                    break;
                                //三人小队回血连招2
                                case WeavileTeamBrain.SubState.TeamState_308_Heal2:
                                    {
                                        //玛狃拉03如果是守卫状态 转入跳跃 跳跃完后回血
                                        if(TeamCombatActIndex == 3){
                                            GuardOver();//防守结束
                                            JumpMoveStart();
                                        }
                                        //玛狃拉0102 守卫负责回血的玛狃拉03
                                        else
                                        {
                                            //治疗未完成
                                            if (!TeamBrain.isOver_Heal_TeamState308Heal2) {
                                                Vector2 GrundTargetPosition = Vector2.zero;
                                                if (TeamBrain.WeavileMembersList.Count != 3) { Debug.Log("Error:三人小队 回血连招2 玛狃拉队伍数不为3"); break; }
                                                GrundTargetPosition = (TeamBrain.WeavileMembersList[0].transform.position + TeamBrain.WeavileMembersList[1].transform.position + TeamBrain.WeavileMembersList[2].transform.position) / 3.0f;
                                                //Debug.Log(GrundTargetPosition + "+" + Vector2.Distance(GrundTargetPosition, (Vector2)transform.position));
                                                //和守护目标距离与规定距离之间的差 帮助防守
                                                if (Vector2.Distance(GrundTargetPosition, (Vector2)transform.position) <= DISTENCE_HEAL_BETWEEN_TARGET_TEAMSTATE308HEAL2_MAX)
                                                {
                                                    isMove_Guard = false;
                                                    //速度为0 看向目标
                                                    animator.SetFloat("Speed", 0.0f);
                                                    SetDirector(_mTool.MainVector2((TargetPosition - (Vector2)transform.position).normalized));
                                                    //触发冰粒发射
                                                    if (Vector2.Distance(TargetPosition, (Vector2)transform.position) <= DISTENCE_GUARD_LAUNCH_ICESHARD_TEAMSTATE300IDLE)
                                                    {
                                                        GuardOver();
                                                        IceShardStart(TIME_ICESHARD_TEMASTATE300IDLE_GUARD);
                                                    }
                                                    //触发恶波发射
                                                    if (Vector2.Distance(TargetPosition, (Vector2)transform.position) <= DISTENCE_GUARD_LAUNCH_DARKPULSE_TEAMSTATE300IDLE)
                                                    {
                                                        GuardOver();
                                                        DarkPulseStart(TIME_DARKPULSE_TEMASTATE300IDLE_GUARD);
                                                    }

                                                }
                                                //和守护目标距离和规定距离的差大于一个微小值 接近守护目标
                                                else
                                                {
                                                    isMove_Guard = true;
                                                    Dir_Guard_Move = (GrundTargetPosition - (Vector2)transform.position).normalized;
                                                    Dir_Guard_Move = Quaternion.AngleAxis(-12 + ((float)(teamCombatActIndex - 1)) * 12, Vector3.forward) * Dir_Guard_Move;
                                                    MoveBySpeedAndDir(Dir_Guard_Move, speed, SPEEDALPHA_GUARD_TEAMSTATE300IDLE, 0.0f, 0.0f, 0.0f, 0.0f);
                                                }
                                            }
                                            //回复完成
                                            else
                                            {
                                                GuardOver();//防守结束
                                                IsCombatComplite_TeamState308_Heal2 = true;
                                            }
                                        }
                                    }
                                    break;
                                //三人小队万灵药连招
                                case WeavileTeamBrain.SubState.TeamState_309_FullHeal:
                                    {
                                        //有投掷目标时为其投掷
                                        if (TeamBrain.GetAUnusualWeavile(this) != null)
                                        {
                                            GuardOver();
                                            FlingStart();
                                        }
                                        //没有目标时发呆
                                        else { }
                                    }
                                    break;
                                //二人小队发呆连招
                                case WeavileTeamBrain.SubState.TeamState_200_Idle:
                                    {
                                        List<Weavile> wl = new List<Weavile> { };
                                        Vector2 GrundTargetPosition = Vector2.zero;
                                        if (TeamBrain.WeavileMembersList[0] != this && TeamBrain.WeavileMembersList[0].NowMainState == MainState.Idle) { wl.Add(TeamBrain.WeavileMembersList[0]); }
                                        if (TeamBrain.WeavileMembersList[1] != this && TeamBrain.WeavileMembersList[1].NowMainState == MainState.Idle) { wl.Add(TeamBrain.WeavileMembersList[1]); }
                                        
                                        if (wl.Count == 0) { Debug.Log("Error:二人小队 发呆状态 已经没有人在发呆"); break; }
                                        else if (wl.Count == 1) { GrundTargetPosition = wl[0].transform.position; }
                                        else { Debug.Log("Error:二人小队 发呆状态 队伍人数错误"); }

                                        //和守护目标距离与规定距离之间的差 帮助防守
                                        if (Mathf.Abs(Vector2.Distance(GrundTargetPosition, (Vector2)transform.position) - DISTENCE_GUARD_BETWEEN_TARGET_TEAMSTATE300IDLE) <= 0.3f)
                                        {
                                            isMove_Guard = false;
                                            //速度为0 看向目标
                                            animator.SetFloat("Speed", 0.0f);
                                            SetDirector(_mTool.MainVector2((TargetPosition - (Vector2)transform.position).normalized));
                                            //触发冰粒发射
                                            if (Vector2.Distance(TargetPosition, (Vector2)transform.position) <= DISTENCE_GUARD_LAUNCH_ICESHARD_TEAMSTATE300IDLE)
                                            {
                                                GuardOver();
                                                IceShardStart(TIME_ICESHARD_TEMASTATE300IDLE_GUARD);
                                            }
                                            //触发恶波发射
                                            if (Vector2.Distance(TargetPosition, (Vector2)transform.position) <= DISTENCE_GUARD_LAUNCH_DARKPULSE_TEAMSTATE300IDLE)
                                            {
                                                GuardOver();
                                                DarkPulseStart(TIME_DARKPULSE_TEMASTATE300IDLE_GUARD);
                                            }

                                        }
                                        //和守护目标距离和规定距离的差大于一个微小值 接近守护目标
                                        else
                                        {
                                            //在规定距离外 接近被守护者
                                            if (Vector2.Distance(GrundTargetPosition, (Vector2)transform.position) > DISTENCE_GUARD_BETWEEN_TARGET_TEAMSTATE300IDLE)
                                            {
                                                isMove_Guard = true;
                                                Dir_Guard_Move = (GrundTargetPosition - (Vector2)transform.position).normalized;
                                                Dir_Guard_Move = Quaternion.AngleAxis(-12 + ((float)(teamCombatActIndex - 1)) * 12, Vector3.forward) * Dir_Guard_Move;
                                                MoveBySpeedAndDir(Dir_Guard_Move, speed, SPEEDALPHA_GUARD_TEAMSTATE300IDLE, 0.0f, 0.0f, 0.0f, 0.0f);
                                            }
                                            //在规定距离内 远离被守护者
                                            else
                                            {
                                                isMove_Guard = true;
                                                Dir_Guard_Move = -(GrundTargetPosition - (Vector2)transform.position).normalized;
                                                Dir_Guard_Move = Quaternion.AngleAxis(12 - ((float)(teamCombatActIndex - 1)) * 12, Vector3.forward) * Dir_Guard_Move;
                                                MoveBySpeedAndDir(Dir_Guard_Move, speed, SPEEDALPHA_GUARD_TEAMSTATE300IDLE, 0.0f, 0.0f, 0.0f, 0.0f);
                                            }

                                        }

                                    }
                                    break;
                                //二人小队奔跑连招
                                case WeavileTeamBrain.SubState.TeamState_201_Run:
                                    {
                                        GuardOver();//防守结束
                                        RunStart();//开始奔跑
                                    }
                                    break;
                                //二人小队近战连招
                                case WeavileTeamBrain.SubState.TeamState_202_CloseAtk:
                                    {
                                        GuardOver();//防守结束
                                        switch (TeamBrain.NowTeamState202CloseAtk)
                                        {
                                            //冲刺子状态 玛狃拉01冲刺 玛狃拉02帮助
                                            case WeavileTeamBrain.TEAMSTATE202_CLOSEATK.Rush:
                                                switch (TeamCombatActIndex)
                                                {
                                                    case 1://玛狃拉01冲刺
                                                        RushStart();
                                                        break;
                                                    case 2://玛狃拉02帮助
                                                        UseProtectStart(TIME_USEPROTECT_TEMASTATE202_CLOSEATK);
                                                        break;
                                                }
                                                break;
                                            //冲刺子状态 玛狃拉01冰粒 玛狃拉02跳跃
                                            case WeavileTeamBrain.TEAMSTATE202_CLOSEATK.Jump:
                                                switch (TeamCombatActIndex)
                                                {
                                                    case 1://玛狃拉01冰粒
                                                        IceShardStart(TIME_ICESHARD_TEMASTATE202CLOSEATK);
                                                        break;
                                                    case 2://玛狃拉02跳跃
                                                        JumpMoveStart();
                                                        break;
                                                }
                                                break;
                                        }
                                    }
                                    break;
                                //二人小队替身连招1
                                case WeavileTeamBrain.SubState.TeamState_203_Substitute:
                                    {
                                        GuardOver();//防守结束
                                        switch (TeamCombatActIndex)
                                        {
                                            case 1:
                                                FlingStart();//01投掷替身
                                                break;
                                            case 2:
                                                JumpMoveStart();//02跳跃接近01
                                                break;
                                        }
                                    }
                                    break;
                                //二人小队替身连招2
                                case WeavileTeamBrain.SubState.TeamState_204_Substitute2:
                                    {
                                        GuardOver();//防守结束
                                        switch (TeamCombatActIndex)
                                        {
                                            case 1:
                                                FlingStart();//01投掷替身
                                                break;
                                            case 2:
                                                JumpMoveStart();//02跳跃接近01
                                                break;
                                        }
                                    }
                                    break;
                                //二人小队绕圈连招
                                case WeavileTeamBrain.SubState.TeamState_205_CircleRun:
                                    {
                                        GuardOver();//防守结束
                                        CircleRunStart();
                                    }
                                    break;
                                //二人小队回血连招
                                case WeavileTeamBrain.SubState.TeamState_206_Heal:
                                    {
                                        GuardOver();//防守结束
                                        switch (TeamCombatActIndex)
                                        {
                                            case 1:
                                                JumpMoveStart();
                                                break;
                                            case 2:
                                                FlingStart();
                                                break;
                                        }
                                    }
                                    break;
                                //二人小队迷宫连招
                                case WeavileTeamBrain.SubState.TeamState_207_Maze:
                                    {
                                        GuardOver();//防守结束
                                        JumpMoveStart();
                                    }
                                    break;
                                //二人小队万灵药连招
                                case WeavileTeamBrain.SubState.TeamState_208_FullHeal:
                                    {
                                        //有投掷目标时为其投掷
                                        if (TeamBrain.GetAUnusualWeavile(this) != null)
                                        {
                                            GuardOver();
                                            FlingStart();
                                        }
                                        //没有目标时发呆
                                        else { }
                                    }
                                    break;
                                //一人小队巡逻模式
                                case WeavileTeamBrain.SubState.TeamState_100_Normal:
                                    {
                                        switch (TeamBrain.NowTeamState100SubState)
                                        {
                                            case WeavileTeamBrain.TEAMSTATE100_SUBSTATE.FlingSmoke:
                                                {
                                                    GuardOver();
                                                    FlingStart();
                                                }
                                                break;
                                            case WeavileTeamBrain.TEAMSTATE100_SUBSTATE.RunToSmoke:
                                                {
                                                    GuardOver();
                                                    RunStart();
                                                }
                                                break;
                                            case WeavileTeamBrain.TEAMSTATE100_SUBSTATE.RunInSmoke:
                                                {
                                                    GuardOver();
                                                    RunStart();
                                                }
                                                break;
                                            case WeavileTeamBrain.TEAMSTATE100_SUBSTATE.GuardInSmoke:
                                                {
                                                    //烟雾不足 释放烟雾
                                                    if (TeamBrain.GetCountWeavileSmokeList() < 4)
                                                    {
                                                        TeamBrain.NowTeamState100SubState = WeavileTeamBrain.TEAMSTATE100_SUBSTATE.FlingSmoke;
                                                        GuardOver();
                                                        FlingStart();
                                                    }

                                                    if (!isFearDone) {
                                                        //计时器增加 投掷炸弹
                                                        GuardTimer += Time.deltaTime;//计时器增加
                                                        if (GuardTimer >= TIME_GUARD_FLINGBOMB_TEAMSTATE100NORMAL)
                                                        {
                                                            GuardTimer = 0.0f;//计时器清空 转入投掷炸弹
                                                            TeamBrain.NowTeamState100SubState = WeavileTeamBrain.TEAMSTATE100_SUBSTATE.FlingBomb;
                                                            GuardOver();
                                                            FlingStart();
                                                        }

                                                        //目标接近 冲刺
                                                        if (Vector2.Distance(TargetPosition, (Vector2)transform.position) <= DISTENCE_GUARD_RUSH_TEAMSTATE100NORMAL)
                                                        {
                                                            TeamBrain.NowTeamState100SubState = WeavileTeamBrain.TEAMSTATE100_SUBSTATE.Rush;
                                                            GuardOver();
                                                            RushStart();
                                                        }
                                                    }
                                                }
                                                break;
                                            case WeavileTeamBrain.TEAMSTATE100_SUBSTATE.Rush:
                                                {
                                                    GuardOver();
                                                    RushStart();
                                                }
                                                break;
                                            case WeavileTeamBrain.TEAMSTATE100_SUBSTATE.FlingBomb:
                                                {
                                                    GuardOver();
                                                    FlingStart();
                                                }
                                                break;
                                            case WeavileTeamBrain.TEAMSTATE100_SUBSTATE.Idle:
                                                {
                                                    GuardOver();
                                                    IdleStart(TIME_IDLE_TEAMSTATE100_NORMAL);
                                                }
                                                break;
                                        }
                                    }
                                    break;
                            }
                        }
                        break;
                    //使用保护状态
                    case MainState.UseProtect:
                        {
                            UseProtectTimer -= Time.deltaTime;//使用保护计时器时间减少
                            if (UseProtectTimer <= 0)         //计时器时间到时间，结束使用保护状态
                            {
                                switch (TeamBrain.NowSubState)
                                {
                                    //三人小队 回血连招1 交替进行保护和休息
                                    case WeavileTeamBrain.SubState.TeamState_307_Heal1:
                                        //冷却期间 重新开始保护
                                        if (isInterval_UseProtect)
                                        {
                                            //回复结束 动作设为完成
                                            if (TeamBrain.isOver_Heal_TeamState307Heal1)
                                            {
                                                UseProtectOver();
                                                isCombatComplite_TeamState307_Heal = true;
                                            }
                                            else
                                            {
                                                UseProtectStart(TIME_USEPROTECT_TEMASTATE307_HEAL1);
                                            }
                                        }
                                        //非冷却期间 转入动画结束
                                        else
                                        {
                                            animator.SetInteger("Maze", 2);
                                        }
                                        break;
                                    //其他连招直接转入动画结束
                                    default:
                                        animator.SetInteger("Maze", 2);
                                        break;
                                }
                                
                            }
                        }
                        break;
                    //发射冰粒状态
                    case MainState.IceShard:
                        {
                            IceShardTimer -= Time.deltaTime;//发射冰粒计时器时间减少
                            if (IceShardTimer <= 0)         //计时器时间到时间，结束发射冰粒状态
                            {
                                IceShardOver();
                                //根据主脑状态连招不同 下一步执行不同
                                switch (TeamBrain.NowSubState)
                                {
                                    //三人小队 发呆连招 用完冰粒后返回守护模式
                                    case WeavileTeamBrain.SubState.TeamState_300_Idle:
                                        GuardStart();
                                        break;
                                    //三人小队 分身连招 用完冰粒后连招动作完成
                                    case WeavileTeamBrain.SubState.TeamState_304_CloneShadow:
                                        isCombatComplite_TeamState304_CloneShadow = true;
                                        break;
                                    //三人小队 迷宫连招 用完冰粒后继续看守迷宫
                                    case WeavileTeamBrain.SubState.TeamState_305_Maze:
                                        MazeWatcherStart();
                                        break;
                                    //三人小队 回血连招2 用完冰粒后返回守护模式
                                    case WeavileTeamBrain.SubState.TeamState_308_Heal2:
                                        GuardStart();
                                        break;
                                    //二人小队 发呆连招 用完冰粒后返回守护模式
                                    case WeavileTeamBrain.SubState.TeamState_200_Idle:
                                        GuardStart();
                                        break;
                                    //二人小队 巡逻状态 用完冰粒后返回奔跑模式
                                    case WeavileTeamBrain.SubState.TeamState_201_Run:
                                        RunStart();
                                        break;
                                    //二人小队 近战连招 用完冰粒后动作完成
                                    case WeavileTeamBrain.SubState.TeamState_202_CloseAtk:
                                        isCombatComplite_TeamState202_CloseAtk = true;
                                        break;
                                    //二人小队 迷宫连招 用完冰粒后继续看守迷宫
                                    case WeavileTeamBrain.SubState.TeamState_207_Maze:
                                        MazeWatcherStart();
                                        break;
                                    //默认小休息
                                    default:
                                        IdleStart(TIME_IDLE_START);
                                        break;
                                }
                            }
                        }
                        break;
                    //发射恶波状态
                    case MainState.DarkPulse:
                        {
                            DarkPulseTimer -= Time.deltaTime;//发射恶波计时器时间减少
                            if (DarkPulseTimer <= 0)         //计时器时间到时间，结束发射恶波状态
                            {
                                DarkPulseOver();
                                //根据主脑状态连招不同 下一步执行不同
                                switch (TeamBrain.NowSubState)
                                {
                                    //三人小队发呆连招 用完恶波后返回守护模式
                                    case WeavileTeamBrain.SubState.TeamState_300_Idle:
                                        GuardStart();
                                        break;
                                    //三人小队回血连招2 用完恶波后返回守护模式
                                    case WeavileTeamBrain.SubState.TeamState_308_Heal2:
                                        GuardStart();
                                        break;
                                    //二人小队发呆连招 用完恶波后返回守护模式
                                    case WeavileTeamBrain.SubState.TeamState_200_Idle:
                                        GuardStart();
                                        break;
                                    //默认小休息
                                    default:
                                        IdleStart(TIME_IDLE_START);
                                        break;
                                }
                            }
                        }
                        break;
                    //投掷状态
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
                                        //三人小队 替身连招 
                                        case WeavileTeamBrain.SubState.TeamState_303_Substitute:
                                            {
                                                //默认为向目标投掷炸弹 如果状态机错误就会调用默认值
                                                NowFlingType = FlingType.Bomb;
                                                Vector2 t303 = TargetPosition;
                                                //根据当前主脑状态判断投掷种类和投掷目标点
                                                switch (TeamBrain.NowTeamState303SubState)
                                                {
                                                    case WeavileTeamBrain.TEAMSTATE303_SUBSTATE.Fling:
                                                        //投掷状态仅玛狃拉1投掷替身
                                                        if (teamCombatActIndex == 1)
                                                        {
                                                            NowFlingType = FlingType.Substitute;
                                                            t303 = TargetPosition;
                                                        }
                                                        break;
                                                    case WeavileTeamBrain.TEAMSTATE303_SUBSTATE.Jump:
                                                        //跳跃状态无人投掷 状态机错误
                                                        Debug.Log("Error:三人小队 替身连招 跳跃阶段有玛狃拉尝试投掷");
                                                        break;
                                                    case WeavileTeamBrain.TEAMSTATE303_SUBSTATE.Protect:
                                                        //保护状态时 玛狃拉03有概率使用投掷 投掷炸弹（毒弹炸弹50%）
                                                        if (teamCombatActIndex == 3)
                                                        {
                                                            if (Random.Range(0.0f, 1.0f) > 0.5f) { NowFlingType = FlingType.Bomb; t303 = TargetPosition; }
                                                            else { NowFlingType = FlingType.PoisonBomb; t303 = TeamBrain.TeamState303_Substitute_GetSubstitutePostion; }
                                                        }
                                                        break;
                                                    case WeavileTeamBrain.TEAMSTATE303_SUBSTATE.Toxic:
                                                        //炸弹状态时 玛狃拉01有概率使用投掷 投掷伤药
                                                        if (teamCombatActIndex == 1)
                                                        {
                                                            NowFlingType = FlingType.Heal;
                                                            //向另外两只中血量低者投掷伤药
                                                            if (TeamBrain.CombatWeavile02 != null && TeamBrain.CombatWeavile03 != null)
                                                            {
                                                                if (TeamBrain.CombatWeavile02.EmptyHp <= TeamBrain.CombatWeavile03.EmptyHp) 
                                                                { 
                                                                    t303 = TeamBrain.CombatWeavile02.transform.position;
                                                                    if (TeamBrain.CombatWeavile02.GetHPPercent > 0.95f )
                                                                    {
                                                                        NowFlingType = FlingType.Bomb;
                                                                        t303 = TargetPosition;
                                                                    }
                                                                }
                                                                else { 
                                                                    t303 = TeamBrain.CombatWeavile03.transform.position;
                                                                    if (TeamBrain.CombatWeavile03.GetHPPercent > 0.95f)
                                                                    {
                                                                        NowFlingType = FlingType.Bomb;
                                                                        t303 = TargetPosition;
                                                                    }
                                                                }
                                                            }
                                                            //另外两只玛狃拉不存在 出现错误 给自己投掷
                                                            else
                                                            {
                                                                Debug.Log("Error:三人小队 替身连招 投掷伤药时 其他玛狃拉不存在");
                                                                t303 = this.transform.position;
                                                            }
                                                        }
                                                        //炸弹状态时 玛狃拉03有概率使用投掷 投掷炸弹（毒弹炸弹65%/25%/10%）
                                                        else if (teamCombatActIndex == 3)
                                                        {
                                                            float r = Random.Range(0.0f, 1.0f);
                                                            if (r >= 0.0f && r <= 0.65f)
                                                            {
                                                                NowFlingType = FlingType.PoisonBomb; t303 = TeamBrain.TeamState303_Substitute_GetSubstitutePostion;
                                                            }
                                                            else if (r >= 0.65f && r <= 0.95f)
                                                            {
                                                                NowFlingType = FlingType.Bomb; t303 = TargetPosition;
                                                            }
                                                            else
                                                            {
                                                                NowFlingType = FlingType.SmokeBomb; t303 = TeamBrain.TeamState303_Substitute_GetSubstitutePostion;
                                                            }
                                                        }
                                                        break;
                                                }
                                                t303 = ParentPokemonRoom.EnsurePointReachesRoom(t303);
                                                LaunchFlingProjectiles(NowFlingType, t303);
                                                FlingTimer = TIME_PREPARE_FLING_TEMASTATE306_CIRCLERUN_ESCAPEATK;//设置时间
                                                SetDirector(_mTool.MainVector2((t303 - (Vector2)transform.position).normalized));
                                            }
                                            break;
                                        //三人小队 迷宫连招
                                        case WeavileTeamBrain.SubState.TeamState_305_Maze:
                                            {
                                                //首次投掷 仅投掷烟雾弹 目标随机
                                                if (!isOver_TeamState305Maze_FirstFling)
                                                {
                                                    //首次投掷结束
                                                    isOver_TeamState305Maze_FirstFling = true;

                                                    NowFlingType = FlingType.SmokeBomb;
                                                    float SmokeBombRadius = 3.0f;
                                                    //目标点在房间内随机（考虑烟雾弹半径）
                                                    Vector2 t = new Vector2( Random.Range( ParentPokemonRoom.RoomSize[2] + SmokeBombRadius, ParentPokemonRoom.RoomSize[3] - SmokeBombRadius) ,
                                                                             Random.Range( ParentPokemonRoom.RoomSize[1] + SmokeBombRadius, ParentPokemonRoom.RoomSize[0] - SmokeBombRadius));
                                                    //Debug.Log(ParentPokemonRoom.RoomSize[2] + SmokeBombRadius);
                                                    //Debug.Log(ParentPokemonRoom.RoomSize[3] - SmokeBombRadius);
                                                    //Debug.Log(ParentPokemonRoom.RoomSize[1] + SmokeBombRadius);
                                                    //Debug.Log(ParentPokemonRoom.RoomSize[0] - SmokeBombRadius);
                                                    //Debug.Log(t);
                                                    t += (Vector2)ParentPokemonRoom.transform.position; 
                                                    LaunchFlingProjectiles(NowFlingType, t);
                                                    FlingTimer = TIME_FLING_TEAMSTATE305MAZE_FIRST;//设置时间
                                                    SetDirector(_mTool.MainVector2((t - (Vector2)transform.position).normalized));
                                                }
                                                //二次投掷 向目标投掷炸弹或 向同伴投掷伤药
                                                else
                                                {
                                                    float r = Random.Range(0.0f , 1.0f);
                                                    Vector2 t = Vector2.zero;
                                                    //25%炸弹
                                                    if (r >= 0.0f && r < 0.25f)
                                                    {
                                                        NowFlingType = FlingType.Bomb;
                                                        t = TargetPosition;
                                                    }
                                                    //25%毒弹
                                                    else if (r >= 0.25f && r < 0.50f)
                                                    {
                                                        NowFlingType = FlingType.PoisonBomb;
                                                        t = TargetPosition;
                                                    }
                                                    //50%回血
                                                    else
                                                    {
                                                        NowFlingType = FlingType.Heal;
                                                        Weavile w = TeamBrain.GetLeastHPWeavile(this);
                                                        t = w.transform.position;
                                                        if (w.GetHPPercent > 0.95f)
                                                        {
                                                            NowFlingType = FlingType.Bomb;
                                                            t = TargetPosition;
                                                        }
                                                    }
                                                    LaunchFlingProjectiles(NowFlingType, t);
                                                    FlingTimer = TIME_FLING_TEAMSTATE305MAZE_SECOND;//设置时间
                                                    SetDirector(_mTool.MainVector2((t - (Vector2)transform.position).normalized));
                                                }
                                            }
                                            break;
                                        //三人小队 绕圈连招 逃脱攻击
                                        case WeavileTeamBrain.SubState.TeamState_306_CircleRun:
                                            {
                                                if (TeamBrain.NowTeamState306SubState == WeavileTeamBrain.TEAMSTATE306_SUBSTATE.EscapeAtk)
                                                {
                                                    NowFlingType = FlingType.Bomb;
                                                    Vector2 t = Vector2.zero;
                                                    if (TeamBrain.Count_Fling_TeamState306CircleRun_EscapeAtk == 1)
                                                    {
                                                        //第二次投掷为毒雾弹
                                                        NowFlingType = FlingType.PoisonBomb;
                                                        t = TargetPosition + (Vector2)(Quaternion.AngleAxis(-30.0f + 120.0f * (float)teamCombatActIndex, Vector3.forward) * Vector2.right * RADIUS_FLING_TEMASTATE306_CIRCLERUN_ESCAPEATK_POISONBOMB);
                                                        t = ParentPokemonRoom.EnsurePointReachesRoom(t);
                                                    }
                                                    else
                                                    {
                                                        //其他投掷为普通炸弹
                                                        NowFlingType = FlingType.Bomb;
                                                        t = TargetPosition + (Vector2)(Quaternion.AngleAxis(30.0f + 120.0f * (float)teamCombatActIndex, Vector3.forward) * Vector2.right * RADIUS_FLING_TEMASTATE306_CIRCLERUN_ESCAPEATK_BOMB);
                                                        t = ParentPokemonRoom.EnsurePointReachesRoom(t);
                                                    }
                                                    LaunchFlingProjectiles(NowFlingType, t);
                                                    FlingTimer = TIME_PREPARE_FLING_TEMASTATE306_CIRCLERUN_ESCAPEATK;//设置时间
                                                    SetDirector(_mTool.MainVector2((t - (Vector2)transform.position).normalized));
                                                }
                                            }
                                            break;
                                        //三人小队 万灵药连招
                                        case WeavileTeamBrain.SubState.TeamState_309_FullHeal:
                                            {
                                                switch (TeamBrain.NowTeamState309SubState) {
                                                    case WeavileTeamBrain.TEAMSTATE309_SUBSTATE.Heal:
                                                        {
                                                            //投掷万灵药
                                                            Weavile tw = TeamBrain.GetAUnusualWeavile(this);
                                                            Vector2 t = Vector2.zero;
                                                            if (tw == null)
                                                            {
                                                                NowFlingType = FlingType.Bomb;
                                                                t = TargetPosition;
                                                            }
                                                            else
                                                            {
                                                                NowFlingType = FlingType.FullHeal;
                                                                t = tw.transform.position;
                                                            }
                                                            LaunchFlingProjectiles(NowFlingType, t);
                                                            FlingTimer = TIME_FLING_TEMASTATE309_FULLHEAL;//设置时间
                                                            SetDirector(_mTool.MainVector2((t - (Vector2)transform.position).normalized));
                                                        }
                                                        break;
                                                    case WeavileTeamBrain.TEAMSTATE309_SUBSTATE.Rush:
                                                        {
                                                            //投掷炸弹
                                                            NowFlingType = FlingType.Bomb;
                                                            Vector2 t = TargetPosition;
                                                            LaunchFlingProjectiles(NowFlingType, t);
                                                            FlingTimer = TIME_FLING_TEMASTATE309_FULLHEAL;//设置时间
                                                            SetDirector(_mTool.MainVector2((t - (Vector2)transform.position).normalized));
                                                        }
                                                        break;
                                                }
                                            }
                                            break;
                                        //二人小队 替身连招 
                                        case WeavileTeamBrain.SubState.TeamState_203_Substitute:
                                            {
                                                //默认为向目标投掷炸弹 如果状态机错误就会调用默认值
                                                NowFlingType = FlingType.Bomb;
                                                Vector2 t203 = TargetPosition;
                                                //根据当前主脑状态判断投掷种类和投掷目标点
                                                switch (TeamBrain.NowTeamState203SubState)
                                                {
                                                    case WeavileTeamBrain.TEAMSTATE203_SUBSTATE.Fling:
                                                        //投掷状态仅玛狃拉1投掷替身
                                                        if (teamCombatActIndex == 1)
                                                        {
                                                            NowFlingType = FlingType.Substitute;
                                                            t203 = TargetPosition;
                                                        }
                                                        break;
                                                    case WeavileTeamBrain.TEAMSTATE203_SUBSTATE.Jump:
                                                        //跳跃状态无人投掷 状态机错误
                                                        Debug.Log("Error:二人小队 替身连招 跳跃阶段有玛狃拉尝试投掷");
                                                        break;
                                                    case WeavileTeamBrain.TEAMSTATE203_SUBSTATE.Protect:
                                                        //保护状态 无人投掷 状态机错误
                                                        Debug.Log("Error:二人小队 替身连招 保护状态有玛狃拉尝试投掷");
                                                        break;
                                                    case WeavileTeamBrain.TEAMSTATE203_SUBSTATE.Toxic:
                                                        //炸弹状态时 玛狃拉01有概率使用投掷 投掷炸弹or伤药（毒弹45%/炸弹30%/伤药20%/烟雾弹5%）
                                                        if (teamCombatActIndex == 1)
                                                        {
                                                            float r = Random.Range(0.0f, 1.0f);
                                                            if (r >= 0.0f && r <= 0.45f)        //毒弹45%
                                                            {
                                                                NowFlingType = FlingType.PoisonBomb; 
                                                                t203 = TeamBrain.TeamState203_Substitute_GetSubstitutePostion;
                                                            }
                                                            else if (r >= 0.45f && r <= 0.75f)  //炸弹30%
                                                            {
                                                                NowFlingType = FlingType.Bomb; 
                                                                t203 = TargetPosition;
                                                            }
                                                            else if (r >= 0.75f && r <= 0.95f)  //伤药20%
                                                            {
                                                                NowFlingType = FlingType.Heal;
                                                                //向另外两只中血量低者投掷伤药
                                                                if (TeamBrain.CombatWeavile01 != null && TeamBrain.CombatWeavile02 != null)
                                                                {
                                                                    if (TeamBrain.CombatWeavile01.EmptyHp >= TeamBrain.CombatWeavile02.EmptyHp) { 
                                                                        t203 = TeamBrain.CombatWeavile02.transform.position;
                                                                        if (TeamBrain.CombatWeavile02.GetHPPercent > 0.95f)
                                                                        {
                                                                            NowFlingType = FlingType.Bomb;
                                                                            t203 = TargetPosition;
                                                                        }
                                                                    }
                                                                    else {
                                                                        t203 = TeamBrain.CombatWeavile01.transform.position;
                                                                        if (TeamBrain.CombatWeavile01.GetHPPercent > 0.95f)
                                                                        {
                                                                            NowFlingType = FlingType.Bomb;
                                                                            t203 = TargetPosition;
                                                                        }
                                                                    }
                                                                }
                                                                else { t203 = transform.position; }
                                                            }
                                                            else                                //烟雾弹5%
                                                            {
                                                                NowFlingType = FlingType.SmokeBomb; 
                                                                t203 = TeamBrain.TeamState203_Substitute_GetSubstitutePostion;
                                                            }
                                                        }
                                                        break;
                                                }
                                                t203 = ParentPokemonRoom.EnsurePointReachesRoom(t203);
                                                LaunchFlingProjectiles(NowFlingType, t203);
                                                FlingTimer = TIME_PREPARE_FLING_TEMASTATE306_CIRCLERUN_ESCAPEATK;//设置时间
                                                SetDirector(_mTool.MainVector2((t203 - (Vector2)transform.position).normalized));
                                            }
                                            break;
                                        //二人小队 替身连招2 
                                        case WeavileTeamBrain.SubState.TeamState_204_Substitute2:
                                            {
                                                //默认为向目标投掷炸弹 如果状态机错误就会调用默认值
                                                NowFlingType = FlingType.Bomb;
                                                Vector2 t204 = TargetPosition;
                                                //根据当前主脑状态判断投掷种类和投掷目标点
                                                switch (TeamBrain.NowTeamState204SubState)
                                                {
                                                    case WeavileTeamBrain.TEAMSTATE204_SUBSTATE.Fling:
                                                        //投掷状态仅玛狃拉1投掷替身
                                                        if (teamCombatActIndex == 1)
                                                        {
                                                            NowFlingType = FlingType.Substitute;
                                                            t204 = TargetPosition;
                                                        }
                                                        break;
                                                    case WeavileTeamBrain.TEAMSTATE204_SUBSTATE.CloneShadow:
                                                        //跳跃状态无人投掷 状态机错误
                                                        Debug.Log("Error:二人小队 替身连招2 分身阶段有玛狃拉尝试投掷");
                                                        break;
                                                    case WeavileTeamBrain.TEAMSTATE204_SUBSTATE.Jump:
                                                        //跳跃状态无人投掷 状态机错误
                                                        Debug.Log("Error:二人小队 替身连招2 跳跃阶段有玛狃拉尝试投掷");
                                                        break;
                                                    case WeavileTeamBrain.TEAMSTATE204_SUBSTATE.Bomb:
                                                        //炸弹状态时 玛狃拉全体使用投掷 炸弹100%
                                                        NowFlingType = FlingType.Bomb;
                                                        t204 = TeamBrain.TeamState204Substitute2_GetJumpTargetPosition(
                                                            TeamBrain.TeamState204_Substitute2_GetSubstitutePostion, WeavileTeamBrain.DISTENCE_TEAMSTATE204SUBSTTITUTE2_JUMP_BOMBBASERADIUS, TeamBrain.WeavileAndCloneShadowList.Count,
                                                            ParentPokemonRoom.RoomSize[0] + ParentPokemonRoom.transform.position.y, ParentPokemonRoom.RoomSize[1] + ParentPokemonRoom.transform.position.y, ParentPokemonRoom.RoomSize[2] + ParentPokemonRoom.transform.position.x, ParentPokemonRoom.RoomSize[3] + ParentPokemonRoom.transform.position.x
                                                            , transform);
                                                        //Debug.Log(t204);
                                                        t204 = ParentPokemonRoom.EnsurePointReachesRoom(t204);
                                                        break;
                                                }
                                                t204 = ParentPokemonRoom.EnsurePointReachesRoom(t204);
                                                LaunchFlingProjectiles(NowFlingType, t204);
                                                FlingTimer = TIME_PREPARE_FLING_TEMASTATE306_CIRCLERUN_ESCAPEATK;//设置时间
                                                SetDirector(_mTool.MainVector2((t204 - (Vector2)transform.position).normalized));
                                            }
                                            break;
                                        //二人小队 绕圈连招 逃脱攻击
                                        case WeavileTeamBrain.SubState.TeamState_205_CircleRun:
                                            {
                                                if (TeamBrain.NowTeamState205SubState == WeavileTeamBrain.TEAMSTATE306_SUBSTATE.EscapeAtk)
                                                {
                                                    NowFlingType = FlingType.Bomb;
                                                    Vector2 t = TargetPosition;
                                                    if (TeamBrain.Count_Fling_TeamState205CircleRun_EscapeAtk == 1)
                                                    {
                                                        //第二次投掷为毒雾弹
                                                        NowFlingType = FlingType.PoisonBomb;
                                                        t = TargetPosition + (Vector2)(Quaternion.AngleAxis(180.0f * (float)teamCombatActIndex, Vector3.forward) * Vector2.right * RADIUS_FLING_TEMASTATE306_CIRCLERUN_ESCAPEATK_POISONBOMB);
                                                        t = ParentPokemonRoom.EnsurePointReachesRoom(t);
                                                    }
                                                    else
                                                    {
                                                        //其他投掷为普通炸弹
                                                        NowFlingType = FlingType.Bomb;
                                                        t = TargetPosition + (Vector2)(Quaternion.AngleAxis(90.0f + 180.0f * (float)teamCombatActIndex, Vector3.forward) * Vector2.right * RADIUS_FLING_TEMASTATE306_CIRCLERUN_ESCAPEATK_BOMB);
                                                        t = ParentPokemonRoom.EnsurePointReachesRoom(t);
                                                    }
                                                    LaunchFlingProjectiles(NowFlingType, t);
                                                    FlingTimer = TIME_PREPARE_FLING_TEMASTATE306_CIRCLERUN_ESCAPEATK;//设置时间
                                                    SetDirector(_mTool.MainVector2((t - (Vector2)transform.position).normalized));
                                                }
                                            }
                                            break;
                                        //二人小队 回血连招
                                        case WeavileTeamBrain.SubState.TeamState_206_Heal:
                                            {
                                                NowFlingType = FlingType.SmokeBomb;
                                                List<Vector2> tList = LaunchFlingSmoke6();
                                                for (int i = 0; i < tList.Count; i++)
                                                {
                                                    Vector2 t = ParentPokemonRoom.EnsurePointReachesRoom(tList[i]);
                                                    LaunchFlingProjectiles(NowFlingType, t);
                                                    FlingTimer = TIME_FLING_TEAMSTATE306CIRCLERUN_ESCAPEATK;//设置时间
                                                }
                                                SetDirector(_mTool.MainVector2((TargetPosition - (Vector2)transform.position).normalized));
                                            }
                                            break;
                                        //二人小队 迷宫连招
                                        case WeavileTeamBrain.SubState.TeamState_207_Maze:
                                            {
                                                //首次投掷 仅投掷烟雾弹 目标随机
                                                if (!isOver_TeamState207Maze_FirstFling)
                                                {
                                                    //首次投掷结束
                                                    isOver_TeamState207Maze_FirstFling = true;

                                                    NowFlingType = FlingType.SmokeBomb;
                                                    float SmokeBombRadius = 3.0f;
                                                    //目标点在房间内随机（考虑烟雾弹半径）
                                                    Vector2 t = new Vector2(Random.Range(ParentPokemonRoom.RoomSize[2] + SmokeBombRadius, ParentPokemonRoom.RoomSize[3] - SmokeBombRadius),
                                                                             Random.Range(ParentPokemonRoom.RoomSize[1] + SmokeBombRadius, ParentPokemonRoom.RoomSize[0] - SmokeBombRadius));
                                                    t += (Vector2)ParentPokemonRoom.transform.position;
                                                    LaunchFlingProjectiles(NowFlingType, t);
                                                    FlingTimer = TIME_FLING_TEAMSTATE305MAZE_FIRST;//设置时间
                                                    SetDirector(_mTool.MainVector2((t - (Vector2)transform.position).normalized));
                                                }
                                                //二次投掷 向目标投掷炸弹或 向同伴投掷伤药
                                                else
                                                {
                                                    float r = Random.Range(0.0f, 1.0f);
                                                    Vector2 t = Vector2.zero;
                                                    //25%炸弹
                                                    if (r >= 0.0f && r < 0.25f)
                                                    {
                                                        NowFlingType = FlingType.Bomb;
                                                        t = TargetPosition;
                                                    }
                                                    //25%毒弹
                                                    else if (r >= 0.25f && r < 0.50f)
                                                    {
                                                        NowFlingType = FlingType.PoisonBomb;
                                                        t = TargetPosition;
                                                    }
                                                    //50%回血
                                                    else
                                                    {
                                                        NowFlingType = FlingType.Heal;
                                                        Weavile w = TeamBrain.GetLeastHPWeavile(this);
                                                        t = w.transform.position;
                                                        if (w.GetHPPercent > 0.95f)
                                                        {
                                                            NowFlingType = FlingType.Bomb;
                                                            t = TargetPosition;
                                                        }
                                                    }
                                                    LaunchFlingProjectiles(NowFlingType, t);
                                                    FlingTimer = TIME_FLING_TEAMSTATE305MAZE_SECOND;//设置时间
                                                    SetDirector(_mTool.MainVector2((t - (Vector2)transform.position).normalized));
                                                }
                                            }
                                            break;
                                        //二人小队 万灵药连招
                                        case WeavileTeamBrain.SubState.TeamState_208_FullHeal:
                                            {
                                                switch (TeamBrain.NowTeamState208SubState)
                                                {
                                                    case WeavileTeamBrain.TEAMSTATE309_SUBSTATE.Heal:
                                                        {
                                                            //投掷万灵药
                                                            Weavile tw = TeamBrain.GetAUnusualWeavile(this);
                                                            Vector2 t = Vector2.zero;
                                                            if (tw == null)
                                                            {
                                                                NowFlingType = FlingType.Bomb;
                                                                t = TargetPosition;
                                                            }
                                                            else
                                                            {
                                                                NowFlingType = FlingType.FullHeal;
                                                                t = tw.transform.position;
                                                            }
                                                            LaunchFlingProjectiles(NowFlingType, t);
                                                            FlingTimer = TIME_FLING_TEMASTATE309_FULLHEAL;//设置时间
                                                            SetDirector(_mTool.MainVector2((t - (Vector2)transform.position).normalized));
                                                        }
                                                        break;
                                                    case WeavileTeamBrain.TEAMSTATE309_SUBSTATE.Rush:
                                                        {
                                                            //投掷炸弹
                                                            NowFlingType = FlingType.Bomb;
                                                            Vector2 t = TargetPosition;
                                                            LaunchFlingProjectiles(NowFlingType, t);
                                                            FlingTimer = TIME_FLING_TEMASTATE309_FULLHEAL;//设置时间
                                                            SetDirector(_mTool.MainVector2((t - (Vector2)transform.position).normalized));
                                                        }
                                                        break;
                                                }

                                            }
                                            break;
                                        //一人小队 
                                        case WeavileTeamBrain.SubState.TeamState_100_Normal:
                                            {
                                                switch (TeamBrain.NowTeamState100SubState)
                                                {
                                                    case WeavileTeamBrain.TEAMSTATE100_SUBSTATE.FlingSmoke:
                                                        {
                                                            Vector2 RoomCorner = new Vector2(((Random.Range(0.0f, 1.0f) > 0.5f) ? ParentPokemonRoom.RoomSize[2] : ParentPokemonRoom.RoomSize[3]), ((Random.Range(0.0f, 1.0f) > 0.5f) ? ParentPokemonRoom.RoomSize[0] : ParentPokemonRoom.RoomSize[1])) + (Vector2)ParentPokemonRoom.transform.position;
                                                            Vector2 Dir = -(_mTool.TiltMainVector2((RoomCorner - (Vector2)ParentPokemonRoom.transform.position).normalized));
                                                            float r = RADIUS_SMOKE_TEAMSTATE100_NORMAL+2.5f;
                                                            Vector2 t1 = RoomCorner + Dir * RADIUS_SMOKE_TEAMSTATE100_NORMAL;
                                                            Vector2 t2 = RoomCorner + Dir * RADIUS_SMOKE_TEAMSTATE100_NORMAL + new Vector2(Dir.x, 0.0f) * r;
                                                            Vector2 t3 = RoomCorner + Dir * RADIUS_SMOKE_TEAMSTATE100_NORMAL + new Vector2(0.0f, Dir.y) * r;
                                                            Vector2 t4 = RoomCorner + Dir * RADIUS_SMOKE_TEAMSTATE100_NORMAL + new Vector2(Dir.x, Dir.y) * r;
                                                            //Vector2 t5 = RoomCorner + Dir * r + new Vector2(Dir.x, 0.0f) * r + new Vector2(Dir.x, 0.0f) * r;
                                                            //Vector2 t6 = RoomCorner + Dir * r + new Vector2(Dir.x, Dir.y) * r + new Vector2(Dir.x, 0.0f) * r;
                                                            Debug.Log(RoomCorner);
                                                            Debug.Log(t1);
                                                            Debug.Log(t2);
                                                            Debug.Log(t3);
                                                            Debug.Log(t4);
                                                            //Debug.Log(t5);
                                                            //Debug.Log(t6);
                                                            NowFlingType = FlingType.SmokeBomb;
                                                            LaunchFlingProjectiles(NowFlingType, t1);
                                                            LaunchFlingProjectiles(NowFlingType, t2);
                                                            LaunchFlingProjectiles(NowFlingType, t3);
                                                            LaunchFlingProjectiles(NowFlingType, t4);
                                                            //LaunchFlingProjectiles(NowFlingType, t5);
                                                            //LaunchFlingProjectiles(NowFlingType, t6);
                                                            FlingTimer = TIME_FLING_TEAMSTATE100NORMAL_SMOKE;//设置时间
                                                            SetDirector(_mTool.MainVector2((t1 - (Vector2)transform.position).normalized));
                                                        }
                                                        break;
                                                    case WeavileTeamBrain.TEAMSTATE100_SUBSTATE.RunToSmoke:
                                                        FlingOver();
                                                        RunStart();
                                                        break;
                                                    case WeavileTeamBrain.TEAMSTATE100_SUBSTATE.RunInSmoke:
                                                        FlingOver();
                                                        RunStart();
                                                        break;
                                                    case WeavileTeamBrain.TEAMSTATE100_SUBSTATE.GuardInSmoke:
                                                        FlingOver();
                                                        GuardStart();
                                                        break;
                                                    case WeavileTeamBrain.TEAMSTATE100_SUBSTATE.Rush:
                                                        FlingOver();
                                                        RushStart();
                                                        break;
                                                    case WeavileTeamBrain.TEAMSTATE100_SUBSTATE.FlingBomb:
                                                        {
                                                            float r = Random.Range(0.0f, 1.0f);
                                                            Vector2 t = Vector2.zero;
                                                            //45%炸弹
                                                            if (r >= 0.0f && r < 0.45f)
                                                            {
                                                                NowFlingType = FlingType.Bomb;
                                                                t = TargetPosition;
                                                            }
                                                            //40%毒弹
                                                            else if (r >= 0.45f && r < 0.85f)
                                                            {
                                                                NowFlingType = FlingType.PoisonBomb;
                                                                t = TargetPosition;
                                                            }
                                                            //15%回血
                                                            else
                                                            {
                                                                NowFlingType = FlingType.Heal;
                                                                t = transform.position;
                                                                if (GetHPPercent > 0.95f)
                                                                {
                                                                    NowFlingType = FlingType.Bomb;
                                                                    t = TargetPosition;
                                                                }
                                                            }
                                                            if (isEmptyConfusionDone)
                                                            {
                                                                t = (Vector2)(Quaternion.AngleAxis(Random.Range(0, 360), Vector3.forward) * Vector2.right * Random.Range(0.0f, 2.5f)) + t;
                                                            }
                                                            LaunchFlingProjectiles(NowFlingType, t);
                                                            FlingTimer = TIME_FLING_TEAMSTATE100NORMAL_BOMB;//设置时间
                                                            SetDirector(_mTool.MainVector2((t - (Vector2)transform.position).normalized));
                                                        }
                                                        break;
                                                    case WeavileTeamBrain.TEAMSTATE100_SUBSTATE.Idle:
                                                        FlingOver();
                                                        IdleStart(TIME_IDLE_TEAMSTATE100_NORMAL);
                                                        break;
                                                }
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
                                        //三人小队 替身连招 投掷结束后连招动作结束 设定动作完成旗帜为真
                                        case WeavileTeamBrain.SubState.TeamState_303_Substitute:
                                            isCombatComplite_TeamState303_Substitute = true;
                                            break;
                                        //三人小队 迷宫连招 投掷结束后继续守卫迷宫
                                        case WeavileTeamBrain.SubState.TeamState_305_Maze:
                                            MazeWatcherStart();
                                            break;
                                        //三人小队 绕圈连招 脱离攻击 投掷结束后连招结束 设定动作完成旗帜为真
                                        case WeavileTeamBrain.SubState.TeamState_306_CircleRun:
                                            if (TeamBrain.NowTeamState306SubState == WeavileTeamBrain.TEAMSTATE306_SUBSTATE.EscapeAtk)
                                            {
                                                isCombatComplite_TeamState306_CircleRun_EscapeAtk = true;
                                            }
                                            break;
                                        //三人小队 万灵药连招
                                        case WeavileTeamBrain.SubState.TeamState_309_FullHeal:
                                            switch (TeamBrain.NowTeamState309SubState)
                                            {
                                                case WeavileTeamBrain.TEAMSTATE309_SUBSTATE.Heal:
                                                    GuardStart();
                                                    break;
                                                case WeavileTeamBrain.TEAMSTATE309_SUBSTATE.Rush:
                                                    isCombatComplite_TeamState309_FullHeal = true;
                                                    break;
                                            }
                                            break;
                                        //二人小队 替身连招 投掷结束后连招动作结束 设定动作完成旗帜为真
                                        case WeavileTeamBrain.SubState.TeamState_203_Substitute:
                                            isCombatComplite_TeamState203_Substitute = true;
                                            break;
                                        //二人小队 替身连招2 投掷结束后连招动作结束 设定动作完成旗帜为真
                                        case WeavileTeamBrain.SubState.TeamState_204_Substitute2:
                                            isCombatComplite_TeamState204_Substitute2 = true;
                                            break;
                                        //二人小队 绕圈连招 脱离攻击 投掷结束后连招结束 设定动作完成旗帜为真
                                        case WeavileTeamBrain.SubState.TeamState_205_CircleRun:
                                            if (TeamBrain.NowTeamState205SubState == WeavileTeamBrain.TEAMSTATE306_SUBSTATE.EscapeAtk)
                                            {
                                                isCombatComplite_TeamState205_CircleRun_EscapeAtk = true;
                                            }
                                            break;
                                        //二人小队 回血连招 投掷结束后转入回复
                                        case WeavileTeamBrain.SubState.TeamState_206_Heal:
                                            HealStart(TIME_HEAL_TEMASTATE206_HEAL);
                                            break;
                                        //二人小队 迷宫连招 投掷结束后继续守卫迷宫
                                        case WeavileTeamBrain.SubState.TeamState_207_Maze:
                                            MazeWatcherStart();
                                            break;
                                        //二人小队 万灵药连招
                                        case WeavileTeamBrain.SubState.TeamState_208_FullHeal:
                                            switch (TeamBrain.NowTeamState208SubState)
                                            {
                                                case WeavileTeamBrain.TEAMSTATE309_SUBSTATE.Heal:
                                                    GuardStart();
                                                    break;
                                                case WeavileTeamBrain.TEAMSTATE309_SUBSTATE.Rush:
                                                    isCombatComplite_TeamState208_FullHeal = true;
                                                    break;
                                            }
                                            break;
                                        //一人小队 
                                        case WeavileTeamBrain.SubState.TeamState_100_Normal:
                                            switch (TeamBrain.NowTeamState100SubState)
                                            {
                                                case WeavileTeamBrain.TEAMSTATE100_SUBSTATE.FlingSmoke:
                                                    //烟雾不足 释放烟雾
                                                    if (TeamBrain.GetCountWeavileSmokeList() < 4) {
                                                        TeamBrain.NowTeamState100SubState = WeavileTeamBrain.TEAMSTATE100_SUBSTATE.FlingSmoke;
                                                        FlingStart();
                                                    }
                                                    //转入奔跑进入烟雾
                                                    else{
                                                        TeamBrain.NowTeamState100SubState = WeavileTeamBrain.TEAMSTATE100_SUBSTATE.RunToSmoke;
                                                        RunStart();
                                                    }
                                                    break;
                                                case WeavileTeamBrain.TEAMSTATE100_SUBSTATE.RunToSmoke:
                                                    RunStart();
                                                    break;
                                                case WeavileTeamBrain.TEAMSTATE100_SUBSTATE.RunInSmoke:
                                                    RunStart();
                                                    break;
                                                case WeavileTeamBrain.TEAMSTATE100_SUBSTATE.GuardInSmoke:
                                                    GuardStart();
                                                    break;
                                                case WeavileTeamBrain.TEAMSTATE100_SUBSTATE.Rush:
                                                    RushStart();
                                                    break;
                                                case WeavileTeamBrain.TEAMSTATE100_SUBSTATE.FlingBomb:
                                                    //烟雾不足 释放烟雾
                                                    if (TeamBrain.GetCountWeavileSmokeList() < 4)
                                                    {
                                                        TeamBrain.NowTeamState100SubState = WeavileTeamBrain.TEAMSTATE100_SUBSTATE.FlingSmoke;
                                                        FlingStart();
                                                    }
                                                    //转入在烟雾中移动
                                                    else
                                                    {
                                                        TeamBrain.NowTeamState100SubState = WeavileTeamBrain.TEAMSTATE100_SUBSTATE.RunInSmoke;
                                                        RunStart();
                                                    }
                                                    break;
                                                case WeavileTeamBrain.TEAMSTATE100_SUBSTATE.Idle:
                                                    IdleStart(TIME_IDLE_TEAMSTATE100_NORMAL);
                                                    break;
                                            }
                                            break;

                                    }
                                }
                            }
                        }
                        break;
                    //释放迷宫状态
                    case MainState.Maze:
                        {
                            MazeTimer -= Time.deltaTime;//释放迷宫计时器时间减少
                            if (MazeTimer <= 0)         //计时器时间到时间，结束释放迷宫状态
                            {
                                switch (TeamBrain.NowSubState)
                                {
                                    //三人小队 迷宫连招
                                    case WeavileTeamBrain.SubState.TeamState_305_Maze:
                                        //动画机完成
                                        animator.SetInteger("Maze", 2);
                                        break;
                                    //二人小队 回血连招
                                    case WeavileTeamBrain.SubState.TeamState_206_Heal:
                                        //动画机完成
                                        animator.SetInteger("Maze", 2);
                                        break;
                                    //二人小队 迷宫连招
                                    case WeavileTeamBrain.SubState.TeamState_207_Maze:
                                        //动画机完成
                                        animator.SetInteger("Maze", 2);
                                        break;
                                }
                            }
                            //二人小队 回复连招 回复结束时迷宫也结束
                            if (TeamBrain.NowSubState == WeavileTeamBrain.SubState.TeamState_206_Heal && TeamBrain.isOver_Heal_TeamState206Heal)
                            {
                                //动画机完成
                                animator.SetInteger("Maze", 2);
                            }
                        }
                        break;
                    //看护迷宫状态
                    case MainState.MazeWatcher:
                        {
                            switch (TeamBrain.NowSubState)
                            {
                                //三人小队 迷宫连招
                                case WeavileTeamBrain.SubState.TeamState_305_Maze:
                                    //迷宫连招已结束 转入动作结束
                                    if (TeamBrain.isOver_Maze_TeamState305Maze)
                                    {
                                        if (!isCombatComplite_TeamState305_Maze)
                                        {
                                            MazeWatcherOver();
                                            isCombatComplite_TeamState305_Maze = true;
                                        }
                                    }
                                    //迷宫连招还在继续
                                    else
                                    {
                                        switch(teamCombatActIndex){
                                            case 2://玛狃拉02纵向移动
                                                float TargetDistenceY = (TargetPosition.y - transform.position.y);
                                                //和目标纵向距离小于规定距离 发起冲刺
                                                if (Mathf.Abs(TargetDistenceY) <= DISTENCE_TEAMSTATE305_MAZE_MAZEWATCH2RUSH)
                                                {
                                                    MazeWatcherOver();
                                                    RushStart();
                                                    break;
                                                }
                                                Dir_MazeWatcher = new Vector2(0.0f , ((TargetDistenceY > 0) ? 1 : -1));
                                                MoveBySpeedAndDir(Dir_MazeWatcher, speed, SPEEDALPHA_TEAMSTATE305_MAZE, 0.0f, 0.0f, 0.0f, 0.0f);
                                                break;
                                            case 3://玛狃拉03横向移动
                                                float TargetDistenceX = (TargetPosition.x - transform.position.x);
                                                //和目标横向距离小于规定距离 发起冲刺
                                                if (Mathf.Abs(TargetDistenceX) <= DISTENCE_TEAMSTATE305_MAZE_MAZEWATCH2RUSH)
                                                {
                                                    MazeWatcherOver();
                                                    RushStart();
                                                    break;
                                                }
                                                Dir_MazeWatcher = new Vector2( ((TargetDistenceX > 0)? 1 : -1) , 0.0f);
                                                MoveBySpeedAndDir(Dir_MazeWatcher, speed, SPEEDALPHA_TEAMSTATE305_MAZE, 0.0f, 0.0f, 0.0f, 0.0f);
                                                break;
                                            default: //其他玛狃拉转入看护迷宫的话 错误
                                                Debug.Log("Error:三人小队 迷宫连招 连招错误 其他玛狃拉转入看护迷宫");
                                                break;
                                        }
                                        Dir_MazeWatcher = Vector2.zero;
                                    }
                                    break;
                                //二人小队 迷宫连招
                                case WeavileTeamBrain.SubState.TeamState_207_Maze:
                                    //迷宫连招已结束 转入动作结束
                                    if (TeamBrain.isOver_Maze_TeamState207Maze)
                                    {
                                        if (!isCombatComplite_TeamState207_Maze)
                                        {
                                            MazeWatcherOver();
                                            isCombatComplite_TeamState207_Maze = true;
                                            SetDirector(Vector2.down);
                                        }
                                    }
                                    //迷宫连招还在继续
                                    else
                                    {
                                        switch (teamCombatActIndex)
                                        {
                                            case 2://玛狃拉02斜向移动
                                                float TargetDistenceX = (TargetPosition.x - transform.position.x);
                                                float TargetDistenceY = (TargetPosition.y - transform.position.y);
                                                //和目标横向距离小于规定距离 发起冲刺
                                                if (Mathf.Abs(TargetDistenceX) <= DISTENCE_TEAMSTATE305_MAZE_MAZEWATCH2RUSH || Mathf.Abs(TargetDistenceY) <= DISTENCE_TEAMSTATE305_MAZE_MAZEWATCH2RUSH)
                                                {
                                                    MazeWatcherOver();
                                                    RushStart();
                                                    break;
                                                }
                                                Dir_MazeWatcher = new Vector2(((TargetDistenceX > 0) ? 1.1f : -1.1f), ((TargetDistenceY > 0) ? 1.0f : -1.0f));
                                                MoveBySpeedAndDir(Dir_MazeWatcher, speed, SPEEDALPHA_TEAMSTATE305_MAZE, 0.0f, 0.0f, 0.0f, 0.0f);
                                                break;
                                            default: //其他玛狃拉转入看护迷宫的话 错误
                                                Debug.Log("Error:二人小队 迷宫连招 连招错误 其他玛狃拉转入看护迷宫");
                                                break;
                                        }
                                        Dir_MazeWatcher = Vector2.zero;
                                    }
                                    break;
                            }
                        }
                        break;
                    //绕圈跑状态状态
                    case MainState.CircleRun:
                        {
                            CircleRunTimer += Time.deltaTime;//绕圈跑计时器时间增加
                            switch (TeamBrain.NowSubState)
                            {
                                //三人小队 绕圈跑
                                case WeavileTeamBrain.SubState.TeamState_306_CircleRun:
                                    switch (TeamBrain.NowTeamState306SubState)
                                    {
                                        //绕圈跑 跑步状态
                                        case WeavileTeamBrain.TEAMSTATE306_SUBSTATE.Run:
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
                                                MoveBySpeedAndDir(Dir_CircleRun, speed, SpeedAlpha_CircleRun, 0.0f, 0.0f, 0.0f, 0.0f);
                                            }
                                            break;
                                        //触发内环攻击1
                                        case WeavileTeamBrain.TEAMSTATE306_SUBSTATE.InnerAtk1:
                                            CircleRunOver();
                                            RushStart();
                                            break;
                                        //触发内环攻击2
                                        case WeavileTeamBrain.TEAMSTATE306_SUBSTATE.InnerAtk2:
                                            CircleRunOver();
                                            RushStart();
                                            break;
                                        //触发外环攻击
                                        case WeavileTeamBrain.TEAMSTATE306_SUBSTATE.OuterAtk:
                                            CircleRunOver();
                                            //自己的攻击序列号小于等于外环攻击次数时（+1）发动冲刺
                                            //也就是第一次01冲刺 第二次0102冲刺 第三次010203冲刺
                                            if (TeamCombatActIndex <= TeamBrain.Count_TeamState306CircleRun_OuterAtk + 1)
                                            {
                                                isCombatComplite_TeamState306_CircleRun_OuterAtk = false;
                                                RushStart();
                                            }
                                            //其他玛狃拉准备就绪
                                            else { isCombatComplite_TeamState306_CircleRun_OuterAtk = true; }
                                            break;
                                        //触发逃离攻击4
                                        case WeavileTeamBrain.TEAMSTATE306_SUBSTATE.EscapeAtk:
                                            CircleRunOver();
                                            //先跳跃 跳跃后转进投掷
                                            JumpMoveStart();
                                            break;
                                    }
                                    break;
                                //二人小队 绕圈跑
                                case WeavileTeamBrain.SubState.TeamState_205_CircleRun:
                                    switch (TeamBrain.NowTeamState205SubState)
                                    {
                                        //绕圈跑 跑步状态
                                        case WeavileTeamBrain.TEAMSTATE306_SUBSTATE.Run:
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
                                                MoveBySpeedAndDir(Dir_CircleRun, speed, SpeedAlpha_CircleRun, 0.0f, 0.0f, 0.0f, 0.0f);
                                            }
                                            break;
                                        //触发内环攻击1
                                        case WeavileTeamBrain.TEAMSTATE306_SUBSTATE.InnerAtk1:
                                            CircleRunOver();
                                            RushStart();
                                            break;
                                        //触发内环攻击2
                                        case WeavileTeamBrain.TEAMSTATE306_SUBSTATE.InnerAtk2:
                                            CircleRunOver();
                                            RushStart();
                                            break;
                                        //触发外环攻击
                                        case WeavileTeamBrain.TEAMSTATE306_SUBSTATE.OuterAtk:
                                            CircleRunOver();
                                            //自己的攻击序列号小于等于外环攻击次数时（+1）发动冲刺
                                            //也就是第一次01冲刺 第二次0102冲刺
                                            if (TeamCombatActIndex <= TeamBrain.Count_TeamState205CircleRun_OuterAtk + 1)
                                            {
                                                isCombatComplite_TeamState205_CircleRun_OuterAtk = false;
                                                RushStart();
                                            }
                                            //其他玛狃拉准备就绪
                                            else { isCombatComplite_TeamState205_CircleRun_OuterAtk = true; }
                                            break;
                                        //触发逃离攻击4
                                        case WeavileTeamBrain.TEAMSTATE306_SUBSTATE.EscapeAtk:
                                            CircleRunOver();
                                            //先跳跃 跳跃后转进投掷
                                            JumpMoveStart();
                                            break;
                                    }

                                    break;
                                //主脑为其他状态 转入休息
                                default:
                                    CircleRunOver();
                                    IdleStart(TIME_IDLE_START);
                                    break;
                            }
                        }
                        break;
                    //分身状态
                    case MainState.CloneShadow:
                        {
                            CloneShadowTimer -= Time.deltaTime;//分身计时器时间减少
                            if (CloneShadowTimer <= 0)         //计时器时间到时间，结束分身状态
                            {
                                CloneShadowOver();
                                //根据主脑连招不同下一步不同
                                switch (TeamBrain.NowSubState)
                                {
                                    //三人小队 分身连招 分身结束后直接转入跳跃 跳跃至版边
                                    case WeavileTeamBrain.SubState.TeamState_304_CloneShadow:
                                        if (TeamBrain.NowTeamState304CloneShadow == WeavileTeamBrain.TEAMSTATE304_CLONESHADOW.CloneShadow) {
                                            JumpMoveStart();
                                            //确定冲刺方向
                                            if (TeamCombatActIndex == 1) { TeamBrain.TeamState304CloneShadow_JudgeRushDir(); }
                                        }
                                        break;
                                    //三人小队 绕圈连招 分身结束后直接转入绕圈跑
                                    case WeavileTeamBrain.SubState.TeamState_306_CircleRun:
                                        CircleRunStart();
                                        break;
                                    //二人小队 替身连招2 分身结束后设动作完成
                                    case WeavileTeamBrain.SubState.TeamState_204_Substitute2:
                                        isCombatComplite_TeamState204_Substitute2 = true;
                                        break;
                                    //二人小队 绕圈连招 分身结束后直接转入绕圈跑
                                    case WeavileTeamBrain.SubState.TeamState_205_CircleRun:
                                        CircleRunStart();
                                        break;
                                    default:
                                        IdleStart(TIME_IDLE_START);
                                        break;
                                }
                            }
                        }
                        break;
                    //回复状态
                    case MainState.Heal:
                        {

                            //判断主脑 主脑为其他状态时转出
                            switch (TeamBrain.NowSubState)
                            {
                                //三人小队回血连招 回血状态的玛狃拉去给还在需要回血的玛狃拉回血
                                case WeavileTeamBrain.SubState.TeamState_307_Heal1:
                                    {
                                        if (HealTimer > 0 && !TeamBrain.isOver_Heal_TeamState307Heal1) {
                                            List<Weavile> wl = new List<Weavile> { };
                                            if (TeamBrain.CombatWeavile01 == null) { Debug.Log("Error:被回血的玛狃拉01为空"); break; }
                                            Vector2 GrundTargetPosition = TeamBrain.CombatWeavile01.transform.position;

                                            switch (NowHealSubState)
                                            {
                                                //在规定距离外 接近被守护者
                                                case HealSubState.Move:
                                                    //和守护目标距离小于规定距离 副状态转入回血转入
                                                    if (Vector2.Distance(GrundTargetPosition, (Vector2)transform.position) <= DISTENCE_HEAL_BETWEEN_TARGET_TEAMSTATE307HEAL1_MIN)
                                                    {
                                                        NowHealSubState = HealSubState.Heal;
                                                        animator.SetFloat("Speed", 0.0f);
                                                        SetDirector(Vector2.down);
                                                        //生成回血特效预制体 动画转入回血 设置颜色
                                                        animator.SetInteger("Maze", 1);
                                                        SetOutLineAndShadowColor(COLOR_OUTLINEANDSHADOW_HEALGREEN);
                                                        //发射回血组件
                                                        LaunchHeal(HealTimer);
                                                        break;
                                                    }
                                                    //接近目标
                                                    Dir_Heal_Move = (GrundTargetPosition - (Vector2)transform.position).normalized;
                                                    MoveBySpeedAndDir(Dir_Heal_Move, speed, SPEEDALPHA_HEAL_TEAMSTATE307HEAL1_308HEAL2, 0.0f, 0.0f, 0.0f, 0.0f);
                                                    break;
                                                case HealSubState.Heal:
                                                    //和守护目标距离大于规定距离 副状态转入回血转入
                                                    if (Vector2.Distance(GrundTargetPosition, (Vector2)transform.position) > DISTENCE_HEAL_BETWEEN_TARGET_TEAMSTATE307HEAL1_MAX)
                                                    {
                                                        NowHealSubState = HealSubState.Heal2Move;
                                                        //消除回血特效预制体 动画转入冥想结束 随后开始奔跑
                                                        animator.SetInteger("Maze", 2);
                                                        //结束回血组件
                                                        OverHealSupport();
                                                        break;
                                                    }
                                                    //计时器事件减少
                                                    HealTimer -= Time.deltaTime;//回复计时器时间减少
                                                    if (HealTimer <= 0 || TeamBrain.isOver_Heal_TeamState307Heal1)         //计时器时间到时间，结束回复状态
                                                    {
                                                        TeamBrain.isOver_Heal_TeamState307Heal1 = true;
                                                        animator.SetInteger("Maze", 2);
                                                    }
                                                    break;
                                                case HealSubState.Heal2Move:
                                                    // 不做任何逻辑，等待 AnimatorEvent_HealOut()
                                                    break;
                                            }
                                        }
                                    }
                                    break;
                                //三人小队回血连招2 回血状态的玛狃拉去给另外两只玛狃拉回血
                                case WeavileTeamBrain.SubState.TeamState_308_Heal2:
                                    {
                                        if (HealTimer > 0 && !TeamBrain.isOver_Heal_TeamState308Heal2)
                                        {
                                            if (TeamBrain.WeavileMembersList.Count != 3) { Debug.Log("Error:三人小队 回血连招2 玛狃拉队伍数不为3"); break; }
                                            Vector2 GrundTargetPosition = (TeamBrain.WeavileMembersList[0].transform.position + TeamBrain.WeavileMembersList[1].transform.position + TeamBrain.WeavileMembersList[2].transform.position) /3.0f;

                                            switch (NowHealSubState)
                                            {
                                                //在规定距离外 接近被守护者
                                                case HealSubState.Move:
                                                    //和守护目标距离小于规定距离 副状态转入回血转入
                                                    if (Vector2.Distance(GrundTargetPosition, (Vector2)transform.position) <= DISTENCE_HEAL_BETWEEN_TARGET_TEAMSTATE308HEAL2_MIN)
                                                    {
                                                        NowHealSubState = HealSubState.Heal;
                                                        animator.SetFloat("Speed", 0.0f);
                                                        SetDirector(Vector2.down);
                                                        //生成回血特效预制体 动画转入回血 设置颜色
                                                        animator.SetInteger("Maze", 1);
                                                        SetOutLineAndShadowColor(COLOR_OUTLINEANDSHADOW_HEALGREEN);
                                                        //发射回血组件
                                                        LaunchHeal(HealTimer);
                                                        break;
                                                    }
                                                    //接近目标
                                                    Dir_Heal_Move = (GrundTargetPosition - (Vector2)transform.position).normalized;
                                                    MoveBySpeedAndDir(Dir_Heal_Move, speed, SPEEDALPHA_HEAL_TEAMSTATE307HEAL1_308HEAL2, 0.0f, 0.0f, 0.0f, 0.0f);
                                                    break;
                                                case HealSubState.Heal:
                                                    //和守护目标距离大于规定距离 副状态转入回血转入
                                                    if (Vector2.Distance(GrundTargetPosition, (Vector2)transform.position) > DISTENCE_HEAL_BETWEEN_TARGET_TEAMSTATE308HEAL2_MAX)
                                                    {
                                                        NowHealSubState = HealSubState.Heal2Move;
                                                        //消除回血特效预制体 动画转入冥想结束 随后开始奔跑
                                                        animator.SetInteger("Maze", 2);
                                                        //结束回血组件
                                                        OverHealSupport();
                                                        break;
                                                    }
                                                    //计时器事件减少
                                                    HealTimer -= Time.deltaTime;//回复计时器时间减少
                                                    if (HealTimer <= 0 || TeamBrain.isOver_Heal_TeamState308Heal2)         //计时器时间到时间，结束回复状态
                                                    {
                                                        TeamBrain.isOver_Heal_TeamState308Heal2 = true;
                                                        animator.SetInteger("Maze", 2);
                                                    }
                                                    break;
                                                case HealSubState.Heal2Move:
                                                    // 不做任何逻辑，等待 AnimatorEvent_HealOut()
                                                    break;
                                            }
                                        }
                                    }
                                    break;
                                //二人小队回血连招 回血状态的玛狃拉02去给还在需要回血的玛狃拉01回血
                                case WeavileTeamBrain.SubState.TeamState_206_Heal:
                                    {
                                        if (HealTimer > 0 && !TeamBrain.isOver_Heal_TeamState206Heal)
                                        {
                                            if (TeamBrain.CombatWeavile01 == null) { Debug.Log("Error:被回血的玛狃拉01为空"); break; }
                                            Vector2 GrundTargetPosition = TeamBrain.CombatWeavile01.transform.position;

                                            switch (NowHealSubState)
                                            {
                                                //在规定距离外 接近被守护者
                                                case HealSubState.Move:
                                                    //和守护目标距离小于规定距离 副状态转入回血转入
                                                    if (Vector2.Distance(GrundTargetPosition, (Vector2)transform.position) <= DISTENCE_HEAL_BETWEEN_TARGET_TEAMSTATE307HEAL1_MIN)
                                                    {
                                                        NowHealSubState = HealSubState.Heal;
                                                        animator.SetFloat("Speed", 0.0f);
                                                        SetDirector(Vector2.down);
                                                        //生成回血特效预制体 动画转入回血 设置颜色
                                                        animator.SetInteger("Maze", 1);
                                                        SetOutLineAndShadowColor(COLOR_OUTLINEANDSHADOW_HEALGREEN);
                                                        //发射回血组件
                                                        LaunchHeal(HealTimer);
                                                        break;
                                                    }
                                                    //接近目标
                                                    Dir_Heal_Move = (GrundTargetPosition - (Vector2)transform.position).normalized;
                                                    MoveBySpeedAndDir(Dir_Heal_Move, speed, SPEEDALPHA_HEAL_TEAMSTATE307HEAL1_308HEAL2, 0.0f, 0.0f, 0.0f, 0.0f);
                                                    break;
                                                case HealSubState.Heal:
                                                    //和守护目标距离大于规定距离 副状态转入回血转入
                                                    if (Vector2.Distance(GrundTargetPosition, (Vector2)transform.position) > DISTENCE_HEAL_BETWEEN_TARGET_TEAMSTATE307HEAL1_MAX)
                                                    {
                                                        NowHealSubState = HealSubState.Heal2Move;
                                                        //消除回血特效预制体 动画转入冥想结束 随后开始奔跑
                                                        animator.SetInteger("Maze", 2);
                                                        //结束回血组件
                                                        OverHealSupport();
                                                        break;
                                                    }
                                                    //计时器事件减少
                                                    HealTimer -= Time.deltaTime;//回复计时器时间减少
                                                    if (HealTimer <= 0 || TeamBrain.isOver_Heal_TeamState206Heal)         //计时器时间到时间，结束回复状态
                                                    {
                                                        TeamBrain.isOver_Heal_TeamState206Heal = true;
                                                        animator.SetInteger("Maze", 2);
                                                    }
                                                    break;
                                                case HealSubState.Heal2Move:
                                                    // 不做任何逻辑，等待 AnimatorEvent_HealOut()
                                                    break;
                                            }
                                        }
                                    }
                                    break;
                            }
                        }
                        break;
                }
            }
            //■■结束判断状态机

            //■■一人小队 异常状态
            if(TeamBrain.NowMainState == WeavileTeamBrain.MainState.Team1)
            {
                //●冰冻沉默睡眠时结束当前状态机
                if ((isEmptyFrozenDone || isSilence || isSleepDone) && (NowMainState != MainState.Idle))
                {
                    ResetAllState();
                    IdleStart(TIME_IDLE_START);
                }
                //●恐惧时如果在冲刺 投掷 发呆 结束当前状态机 转入奔跑入烟雾状态
                if (isFearDone && (TeamBrain.NowTeamState100SubState == WeavileTeamBrain.TEAMSTATE100_SUBSTATE.FlingBomb || TeamBrain.NowTeamState100SubState == WeavileTeamBrain.TEAMSTATE100_SUBSTATE.Idle || TeamBrain.NowTeamState100SubState == WeavileTeamBrain.TEAMSTATE100_SUBSTATE.Rush))
                {
                    ResetAllState();
                    TeamBrain.NowTeamState100SubState = WeavileTeamBrain.TEAMSTATE100_SUBSTATE.RunToSmoke;
                    RunStart();
                }
            }

        }
    }

    private void FixedUpdate()
    {

        ResetPlayer();//如果玩家组件丢失，重新获取
        if (isEmptyInfatuationDone) { UpdateInfatuationDmageCDTimer(); }//如果被魅惑，计算魅惑时间
        if (!isDie && !isBorn)//不处于正在死亡状态或正在出生状态时
        {
            EmptyBeKnock();//判定是否被击退

            //确实目标位置
            TargetPosition = player.transform.position;
            if (isSubsititue && SubsititueTarget != null) { TargetPosition = SubsititueTarget.transform.position; }

        }
    }


    /// <summary>
    /// 受伤事件
    /// </summary>
    public override void HitEvent()
    {
        base.HitEvent();
        if (TeamBrain != null)
        {
            switch (NowMainState)
            {
                case MainState.Maze:
                    switch (TeamBrain.NowSubState)
                    {
                        //三人小队 迷宫连招 被打断时直接结束迷宫
                        case WeavileTeamBrain.SubState.TeamState_305_Maze:
                            MazeTimer = 0;//迷宫计时器归零
                            TeamBrain.isOver_Maze_TeamState305Maze = true;
                            animator.SetInteger("Maze", 2);
                            break;
                        //二人小队 回复连招 被打断时直接结束迷宫
                        case WeavileTeamBrain.SubState.TeamState_206_Heal:
                            MazeTimer = 0;//迷宫计时器归零
                            animator.SetInteger("Maze", 2);
                            break;
                        //二人小队 迷宫连招 被打断时直接结束迷宫
                        case WeavileTeamBrain.SubState.TeamState_207_Maze:
                            MazeTimer = 0;//迷宫计时器归零
                            TeamBrain.isOver_Maze_TeamState207Maze = true;
                            animator.SetInteger("Maze", 2);
                            break;
                    }
                    break;
                case MainState.Heal:
                    switch (TeamBrain.NowSubState)
                    {
                        //三人小队 回复连招1 被打断时直接结束回复 引发一个爆炸反噬
                        case WeavileTeamBrain.SubState.TeamState_307_Heal1:
                            if (NowHealSubState == HealSubState.Heal && !TeamBrain.isOver_Heal_TeamState307Heal1)
                            {
                                HealTimer = 0;//回复计时器归零
                                TeamBrain.isOver_Heal_TeamState307Heal1 = true;
                                animator.SetInteger("Maze", 2);
                                LaunchHealExplosion();
                            }
                            break;
                        //三人小队 回复连招2 被打断时直接结束回复 引发一个爆炸反噬
                        case WeavileTeamBrain.SubState.TeamState_308_Heal2:
                            if (NowHealSubState == HealSubState.Heal && !TeamBrain.isOver_Heal_TeamState308Heal2)
                            {
                                HealTimer = 0;//回复计时器归零
                                TeamBrain.isOver_Heal_TeamState308Heal2 = true;
                                animator.SetInteger("Maze", 2);
                                LaunchHealExplosion();
                            }
                            break;
                        //二人小队 回复连招 被打断时直接结束回复 引发一个爆炸反噬
                        case WeavileTeamBrain.SubState.TeamState_206_Heal:
                            if (NowHealSubState == HealSubState.Heal && !TeamBrain.isOver_Heal_TeamState206Heal)
                            {
                                HealTimer = 0;//回复计时器归零
                                TeamBrain.isOver_Heal_TeamState206Heal = true;
                                animator.SetInteger("Maze", 2);
                                LaunchHealExplosion();
                            }
                            break;
                    }       
                    break;
                case MainState.Guard:
                    //一人小队 巡逻模式 烟雾中防守模式时 被攻击转入在烟雾中移动
                    if (TeamBrain != null && TeamBrain.NowSubState == WeavileTeamBrain.SubState.TeamState_100_Normal && TeamBrain.NowTeamState100SubState == WeavileTeamBrain.TEAMSTATE100_SUBSTATE.GuardInSmoke)
                    {
                        // 被攻击时切换为在烟雾中移动
                        TeamBrain.NowTeamState100SubState = WeavileTeamBrain.TEAMSTATE100_SUBSTATE.RunInSmoke;
                        GuardOver();
                        RunStart();
                    }
                    break;
            }
            if (TeamBrain != null) {
                switch (TeamBrain.NowSubState)
                {
                    //三人连招 替身连招
                    case WeavileTeamBrain.SubState.TeamState_303_Substitute:
                        //守护或者扔炸弹状态时被攻击 终结替身连招
                        if(TeamBrain.NowTeamState303SubState == WeavileTeamBrain.TEAMSTATE303_SUBSTATE.Protect ||
                           TeamBrain.NowTeamState303SubState == WeavileTeamBrain.TEAMSTATE303_SUBSTATE.Toxic)
                        {
                            TeamBrain.Flg_IsBeHIt_TeamState303_Substitute = true;
                        }
                        break;
                    //二人连招 替身连招
                    case WeavileTeamBrain.SubState.TeamState_203_Substitute:
                        //守护或者扔炸弹状态时被攻击 终结替身连招
                        if (TeamBrain.NowTeamState203SubState == WeavileTeamBrain.TEAMSTATE203_SUBSTATE.Protect ||
                           TeamBrain.NowTeamState203SubState == WeavileTeamBrain.TEAMSTATE203_SUBSTATE.Toxic)
                        {
                            TeamBrain.Flg_IsBeHIt_TeamState203_Substitute = true;
                        }
                        break;
                    //二人连招 替身连招2
                    case WeavileTeamBrain.SubState.TeamState_204_Substitute2:
                        //守护或者扔炸弹状态时被攻击 终结替身连招
                        if (TeamBrain.NowTeamState204SubState == WeavileTeamBrain.TEAMSTATE204_SUBSTATE.Jump ||
                           TeamBrain.NowTeamState204SubState == WeavileTeamBrain.TEAMSTATE204_SUBSTATE.Bomb)
                        {
                            TeamBrain.Flg_IsBeHIt_TeamState204_Substitute2 = true;
                        }
                        break;
                }
            }
        }
    }


    /// <summary>
    /// 死亡事件
    /// </summary>
    public override void DieEvent()
    {
        //仅最后一只玛狃拉掉落掉落物
        IsHaveDropItem = false;
        if (TeamBrain != null && TeamBrain.WeavileMembersList.Count <= 1) { IsHaveDropItem = true; }

        //死亡时引爆所有替身
        _mTool.RemoveNullInList<WeavileCloneBody>(CloneBodyList);
        for (int i = 0; i < CloneBodyList.Count; i++)
        {
            CloneBodyList[i].SetCloneShadowOver();
        }

        //死亡时把自己从主脑列表中剔除
        if (TeamBrain.WeavileMembersList.Contains(this))
        {
            TeamBrain.WeavileMembersList.Remove(this);
        }
        _mTool.RemoveNullInList<Weavile>(TeamBrain.WeavileMembersList);
        base.DieEvent();
    }



    public override void StoreSaveData(Empty e)
    {
        base.StoreSaveData(e);
        TeamBrain = e.GetComponent<Weavile>().TeamBrain;
        if (!TeamBrain.WeavileMembersList.Contains(this))
        {
            TeamBrain.WeavileMembersList.Add(this);
        }
    }



















    //■■■■■■■■■■■■■■■■■■■■碰撞■■■■■■■■■■■■■■■■■■■■■■

    /// <summary>
    /// 冰系冲刺的碰撞伤害
    /// </summary>
    static int DMAGE_RUSH_ICE = 75;
    /// <summary>
    /// 冰系冲刺的击退值
    /// </summary>
    static float KOPOINT_RUSH_ICE = 5.0f;

    /// <summary>
    /// 冰系冲刺的碰撞伤害
    /// </summary>
    static int DMAGE_RUSH_DARK = 75;
    /// <summary>
    /// 冰系冲刺的击退值
    /// </summary>
    static float KOPOINT_RUSH_DARK = 5.0f;

    /// <summary>
    /// 冰系冲刺的碰撞伤害
    /// </summary>
    static int DMAGE_RUSH_POSION = 80;
    /// <summary>
    /// 冰系冲刺的击退值
    /// </summary>
    static float KOPOINT_RUSH_POSION = 5.0f;






    private void OnCollisionEnter2D(Collision2D other)
    {
        CollisionPlayer(other);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        CollisionRoomOrEnviroment(collision);
    }


    /// <summary>
    /// 与玩家碰撞
    /// </summary>
    /// <param name="other"></param>
    public void CollisionPlayer(Collision2D other)
    {
        if (other.transform.tag == ("Player"))//与玩家碰撞时
        {
            switch (NowMainState)
            {
                case MainState.Idle:    //发呆
                    EmptyTouchHit(other.gameObject);//触发触碰伤害
                    break;
                case MainState.Run:     //奔跑
                    EmptyTouchHit(other.gameObject);//触发触碰伤害
                    break;
                case MainState.JumpMove://跳跃移动
                    EmptyTouchHit(other.gameObject);//触发触碰伤害
                    break;
                case MainState.Rush:    //冲刺
                    CollisionEnter2DEvent_Rush(other.gameObject);
                    break;
                case MainState.Guard:   //原地防守
                    EmptyTouchHit(other.gameObject);//触发触碰伤害
                    break;
                case MainState.UseProtect:  //使用保护
                    EmptyTouchHit(other.gameObject);//触发触碰伤害
                    break;
                case MainState.IceShard:    //发射冰粒
                    EmptyTouchHit(other.gameObject);//触发触碰伤害
                    break;
                case MainState.DarkPulse:   //发射恶波
                    EmptyTouchHit(other.gameObject);//触发触碰伤害
                    break;
                case MainState.Fling:      //投掷
                    EmptyTouchHit(other.gameObject);//触发触碰伤害
                    break;
                case MainState.Maze:    //释放迷宫
                    EmptyTouchHit(other.gameObject);//触发触碰伤害
                    break;
                case MainState.MazeWatcher: //看护迷宫
                    EmptyTouchHit(other.gameObject);//触发触碰伤害
                    break;
                case MainState.CloneShadow: //分身
                    EmptyTouchHit(other.gameObject);//触发触碰伤害
                    break;
                case MainState.Heal:    //回复
                    EmptyTouchHit(other.gameObject);//触发触碰伤害
                    break;
            }
        }
    }


    public void CollisionEnter2DEvent_Rush(GameObject PlayerGameobj)
    {
        PlayerControler p = PlayerGameobj.GetComponent<PlayerControler>();
        switch (NowRushType)
        {
            case RushType.Ice:
                Pokemon.PokemonHpChange(this.gameObject, PlayerGameobj.gameObject, DMAGE_RUSH_ICE, 0, 0, PokemonType.TypeEnum.Ice);
                if (p != null)
                {
                    p.KnockOutPoint = KOPOINT_RUSH_ICE;
                    p.KnockOutDirection = (p.transform.position - this.transform.position).normalized;
                    p.PlayerFrozenFloatPlus(0.25f, 2.0f);
                }
                break;
            case RushType.Dark:
                Pokemon.PokemonHpChange(this.gameObject, PlayerGameobj.gameObject, DMAGE_RUSH_DARK, 0, 0, PokemonType.TypeEnum.Dark);
                if (p != null)
                {
                    p.KnockOutPoint = KOPOINT_RUSH_DARK;
                    p.KnockOutDirection = (p.transform.position - this.transform.position).normalized;
                }
                break;
            case RushType.Posion:
                Pokemon.PokemonHpChange(this.gameObject, PlayerGameobj.gameObject, DMAGE_RUSH_POSION, 0, 0, PokemonType.TypeEnum.Poison);
                if (p != null)
                {
                    p.KnockOutPoint = KOPOINT_RUSH_POSION;
                    p.KnockOutDirection = (p.transform.position - this.transform.position).normalized;
                    p.ToxicFloatPlus(0.25f);
                }
                break;
        }
    }




    /// <summary>
    /// 与房间或者环境物碰撞
    /// </summary>
    /// <param name="other"></param>
    private void CollisionRoomOrEnviroment(Collision2D other)
    {
        if (other.transform.tag == "Room")
        {
            CollisionRoomOrEvnviromentRushuStop();
        }
    }

    void CollisionRoomOrEvnviromentRushuStop()
    {
        switch (NowMainState)
        {
            case MainState.Rush:
                if (isMove_Rush && RushTimer >= TIME_RUSHMOVE_RUSH_TEMASTATE302_CLOSEATK / 3)
                {
                    animator.SetInteger("Rush", 3);
                }
                break;
        }
    }







    //■■■■■■■■■■■■■■■■■■■■碰撞■■■■■■■■■■■■■■■■■■■■■■














    //■■■■■■■■■■■■■■■■■■■■共通■■■■■■■■■■■■■■■■■■■■■■
    /// <summary>
    /// 设置敌人的动画机方向
    /// </summary>
    void SetDirector(Vector2 director)
    {
        Director = director;
        animator.SetFloat("LookX", director.x);
        animator.SetFloat("LookY", director.y);
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
            if (!isDie && !isBorn && !isSleepDone && !isCanNotMoveWhenParalysis && !isEmptyFrozenDone && !isSilence && 
                (NowMainState == MainState.Run || NowMainState == MainState.CircleRun || (NowMainState == MainState.Guard && isMove_Guard) || (NowMainState == MainState.Heal && NowHealSubState == HealSubState.Move) || (NowMainState == MainState.MazeWatcher)))
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
            else
            {
                animator.SetFloat("Speed", 0);
            }
            yield return new WaitForSeconds(0.1f);
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

        Vector2 p = new Vector2(rigidbody2D.position.x + (float)dir.x * Time.deltaTime * Speed * SpeedAlpha,
                                rigidbody2D.position.y + (float)dir.y * Time.deltaTime * Speed * SpeedAlpha);

        rigidbody2D.position = new Vector2(
            Mathf.Clamp(rigidbody2D.position.x
                + (float)dir.x * Time.deltaTime * Speed * SpeedAlpha,                    //方向*速度
            ParentPokemonRoom.RoomSize[2] - RoomLeftAlpha + transform.parent.position.x, //最小值
            ParentPokemonRoom.RoomSize[3] + RoomRightAlpha + transform.parent.position.x),//最大值
            Mathf.Clamp(rigidbody2D.position.y
                + (float)dir.y * Time.deltaTime * Speed * SpeedAlpha,                     //方向*速度 
            ParentPokemonRoom.RoomSize[1] - RoomDownAlpha + transform.parent.position.y,  //最小值
            ParentPokemonRoom.RoomSize[0] + RoomUpAlpha + transform.parent.position.y));//最大值

        if (p.x != rigidbody2D.position.x || p.y != rigidbody2D.position.y) { CollisionRoomOrEvnviromentRushuStop(); }
    }
    
    /// <summary>
    /// 混乱时角度偏转
    /// </summary>
    Vector2 ConfusionDir( Vector2 dir , float Alpha)
    {
        if (isEmptyConfusionDone)
        {
            return Quaternion.AngleAxis(Random.Range(-Alpha, Alpha), Vector3.forward) * dir;
        }
        return dir;
    }


    /// <summary>
    /// 生成冲刺落地尘埃
    /// </summary>
    public void InstantiateRunDust()
    {
        Instantiate(RushDust, transform.position, Quaternion.identity);
        ParentPokemonRoom.CameraShake(0.3f, 0.8f, true);
        audioPlayer.Play(WeavileSE.JumpOver, transform.position);
    }

    /// <summary>
    /// 生成奔跑尘埃
    /// </summary>
    public void InstantiateRunDustRun()
    {
        Instantiate(RunDust, transform.position, Quaternion.identity);
        //ParentPokemonRoom.CameraShake(0.3f, 0.8f, true);
        audioPlayer.Play(WeavileSE.Step, transform.position);
    }


    /// <summary>
    /// 清空所有状态机
    /// </summary>
    public void ResetAllState()
    {
        //关闭残影
        if (ShadowCoroutine != null)
        {
            StopShadowCoroutine();
        }
        
        //设置方向和速度
        SetDirector(Vector2.down);
        animator.SetFloat("Speed", 0.0f);

        //动画机转进Idle
        animator.SetTrigger("Sleep");
 
        //重置无敌
        IsDefStateByNormal = false;


        //重置主状态机参数
        isOver_TeamState305Maze_FirstFling = false;
        isOver_TeamState207Maze_FirstFling = false;

        //重置所有玛狃拉的完成状态
        TeamCombatReset_Default();

        //结束所有副状态机 重置所有副状态机参数
        switch (NowMainState)
        {
            case MainState.Idle:        IdleOver(); break;    //发呆
            case MainState.Run:         RunOver(); break;     //奔跑
            case MainState.JumpMove:    JumpMoveOver(); break;//跳跃移动
            case MainState.Rush:        RushOver(); break;    //冲刺
            case MainState.Guard:       GuardOver(); break;   //原地防守
            case MainState.UseProtect:  UseProtectOver(); break;  //使用保护
            case MainState.IceShard:    IceShardOver(); break;    //发射冰粒
            case MainState.DarkPulse:   DarkPulseOver(); break;   //发射恶波
            case MainState.Fling:       FlingOver(); break;     //投掷
            case MainState.CircleRun:   CircleRunOver(); break;  //绕圈跑
            case MainState.Maze:        MazeOver(); break;   //释放迷宫
            case MainState.MazeWatcher: MazeWatcherOver(); break; //看护迷宫
            case MainState.CloneShadow: CloneShadowOver(); break; //分身
            case MainState.Heal:        HealOver(); break;    //回复
        }

        //重置所有动画机参数状态
        animator.SetFloat("Speed", 0);
        animator.ResetTrigger("Hit");
        animator.ResetTrigger("Atk");
        animator.SetInteger("Heal", 0);
        animator.SetInteger("Maze", 0);
        animator.SetInteger("Jump", 0);
        animator.SetInteger("Rush", 0);


        //消除实例
        if (RushArrowObj != null)    { RushArrowObj.ArrowOver(); }
        if (RushObj != null)         { RushObj.RushOver(); }
        if (SupportParentObj != null){ SupportParentObj.SupportOver(); }

        //关闭循环音效
        //loopPlayer.StopLoop(0.0f);
    }

    /// <summary>
    /// 把所有分身加入到分身和本体列表
    /// </summary>
    void AddCloneBodyToBrainList()
    {
        //排空分身列表
        _mTool.RemoveNullInList<WeavileCloneBody>(CloneBodyList);
        TeamBrain.AddWeavileAndCloneShadowList(transform);
        for (int i = 0; i < CloneBodyList.Count; i++)
        {
            TeamBrain.AddWeavileAndCloneShadowList(CloneBodyList[i].transform);
        }
    }

    /// <summary>
    /// 重置睡眠Trigger（动画机重置Trigger）
    /// </summary>
    void ResetAnimatorSleep()
    {
        animator.ResetTrigger("Sleep");
    }

    /// <summary>
    /// 是否被异常状态
    /// </summary>
    /// <returns></returns>
    public bool isWeavileUnusualState()
    {
        bool Output = false;
        Output = (isEmptyFrozenDone) || (isToxicDone) || (isParalysisDone) || (isBurnDone) || (isSleepDone) ||
                 (isFearDone) || (isSilence) || (isEmptyConfusionDone) || (isEmptyInfatuationDone) || (isSpeedChange);
        return Output;
    }
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
        //判断状态机
        switch (TeamBrain.NowSubState)
        {
            //三人小队近战连招 跳跃 跳跃至玛狃拉和目标连线上一定距离 并且在房间内
            case WeavileTeamBrain.SubState.TeamState_302_CloseAtk:
                {
                    Vector2 t = (((Vector2)transform.position - TargetPosition).normalized) * DISTENCE_TARGET_JUMP_TEMASTATE302_CLOSEATK + TargetPosition;
                    t = new Vector2(
                        Mathf.Clamp(t.x,
                            ParentPokemonRoom.RoomSize[2] + transform.parent.position.x, //最小值
                            ParentPokemonRoom.RoomSize[3] + transform.parent.position.x),//最大值
                        Mathf.Clamp(t.y,
                            ParentPokemonRoom.RoomSize[1] + transform.parent.position.y,  //最小值
                            ParentPokemonRoom.RoomSize[0] + transform.parent.position.y));//最大值
                    Debug.Log(t);
                    Dir_Jump = (t - (Vector2)transform.position).normalized;
                    Time_Jump_Max = Vector2.Distance(t, (Vector2)transform.position) / (speed * SPEEDALPHA_JUMP_TEMASTATE302_CLOSEATK);
                    SetDirector(_mTool.MainVector2(Dir_Jump));
                }
                break;
            //三人小队替身连招
            case WeavileTeamBrain.SubState.TeamState_303_Substitute:
                {
                    //向玛狃拉1跳跃 跳跃至玛狃拉和目标连线上一定距离 并且在房间内
                    switch (TeamBrain.NowTeamState303SubState)
                    {
                        //投掷阶段另外两只玛狃拉接近玛狃拉01
                        case WeavileTeamBrain.TEAMSTATE303_SUBSTATE.Fling:
                            if (TeamCombatActIndex != 1)
                            {
                                Vector2 t3 = (((Vector2)transform.position - (Vector2)TeamBrain.CombatWeavile01.transform.position).normalized) * DISTENCE_TARGET_JUMP_TEMASTATE303_SUBSTITUTE + (Vector2)TeamBrain.CombatWeavile01.transform.position;
                                t3 = new Vector2(
                                    Mathf.Clamp(t3.x,
                                        ParentPokemonRoom.RoomSize[2] + transform.parent.position.x, //最小值
                                        ParentPokemonRoom.RoomSize[3] + transform.parent.position.x),//最大值
                                    Mathf.Clamp(t3.y,
                                        ParentPokemonRoom.RoomSize[1] + transform.parent.position.y,  //最小值
                                        ParentPokemonRoom.RoomSize[0] + transform.parent.position.y));//最大值
                                Dir_Jump = (t3 - (Vector2)transform.position).normalized;
                                Time_Jump_Max = Vector2.Distance(t3, (Vector2)transform.position) / (speed * SPEEDALPHA_JUMP_TEMASTATE302_CLOSEATK);
                                SetDirector(_mTool.MainVector2(Dir_Jump));
                            }
                            break;
                        //跳跃阶段 跳跃到远离替身的房间角落
                        case WeavileTeamBrain.TEAMSTATE303_SUBSTATE.Jump:
                            Vector2 t3032 = transform.position;
                            //检查替身存在
                            if (TeamBrain.TeamState303_Substitute_SubstituteOBJ)
                            {
                                //获取替身相对位置
                                Vector2 SubstituteRelativePosition = TeamBrain.TeamState303_Substitute_SubstituteOBJ.transform.position - ParentPokemonRoom.transform.position;
                                //替身在一四象限 目标点置于房间左
                                if (SubstituteRelativePosition.x >= 0) { t3032.x = ParentPokemonRoom.RoomSize[2] + 2.5f + ParentPokemonRoom.transform.position.x; }
                                //替身在二三象限 目标点置于房间右
                                else { t3032.x = ParentPokemonRoom.RoomSize[3] - 2.5f + ParentPokemonRoom.transform.position.x; }
                                //替身在一二象限 目标点置于房间下
                                if (SubstituteRelativePosition.y >= 0) { t3032.y = ParentPokemonRoom.RoomSize[1] + 2.5f + ParentPokemonRoom.transform.position.y; }
                                //替身在三四象限 目标点置于房间上
                                else { t3032.y = ParentPokemonRoom.RoomSize[0] - 2.5f + ParentPokemonRoom.transform.position.y; }
                                t3032 = t3032 + (Vector2)(Quaternion.AngleAxis(30.0f + 120.0f * (float)(teamCombatActIndex - 1), Vector3.forward) * Vector2.right * 1.5f);
                                Dir_Jump = (t3032 - (Vector2)transform.position).normalized;
                                Time_Jump_Max = Vector2.Distance(t3032, (Vector2)transform.position) / (speed * SPEEDALPHA_JUMP_TEMASTATE302_CLOSEATK);
                                SetDirector(_mTool.MainVector2(Dir_Jump));
                            }
                            else
                            {
                                Debug.Log("Error:三人小队 替身连招 替身已经被摧毁 但仍在执行");
                                t3032 = transform.position;
                                Dir_Jump = Vector3.right;
                                Time_Jump_Max = 0;
                                SetDirector(_mTool.MainVector2(Dir_Jump));
                            }
                            break;
                        //其他状态 状态机错误 原地跳一下
                        default:
                            Debug.Log("Error:三人小队 替身连招 状态机错误");
                            Vector2 t3033 = transform.position;
                            Dir_Jump = Vector3.right;
                            Time_Jump_Max = 0;
                            SetDirector(_mTool.MainVector2(Dir_Jump));
                            break;
                    }
                }
                break;
            //三人小队分身连招
            case WeavileTeamBrain.SubState.TeamState_304_CloneShadow:
                {
                    Vector2 t304 = Vector2.zero;//分身连招跳跃目标点
                                                //根据分身连招状态不同 跳跃不同
                    switch (TeamBrain.NowTeamState304CloneShadow)
                    {
                        //第一次跳跃
                        case WeavileTeamBrain.TEAMSTATE304_CLONESHADOW.CloneShadow:
                            t304 = TeamBrain.TeamState304CloneShadow_GetJumpTargetPosition(transform);
                            Dir_Jump = (t304 - (Vector2)transform.position).normalized;
                            Time_Jump_Max = Vector2.Distance(t304, (Vector2)transform.position) / (speed * SPEEDALPHA_JUMP_TEMASTATE304_LONESHADOW);
                            //Time_Jump_Max = TIME_JUMP_TEMASTATE304_CLONESHADOW_FIRSTJUMP;
                            //SpeedAlpha_Jump = Vector2.Distance(t304, (Vector2)transform.position) / (speed * Time_Jump_Max);
                            SetDirector(_mTool.MainVector2(Dir_Jump));
                            break;
                        //之后的跳跃
                        case WeavileTeamBrain.TEAMSTATE304_CLONESHADOW.Jump:
                            t304 = TeamBrain.TeamState304CloneShadow_GetJumpTargetPosition(transform);
                            Dir_Jump = (t304 - (Vector2)transform.position).normalized;
                            Time_Jump_Max = Vector2.Distance(t304, (Vector2)transform.position) / (speed * SPEEDALPHA_JUMP_TEMASTATE304_LONESHADOW);
                            //Time_Jump_Max = TIME_JUMP_TEMASTATE304_CLONESHADOW_SECONDJUMP;
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
                }
                break;
            //三人小队迷宫连招
            case WeavileTeamBrain.SubState.TeamState_305_Maze:
                {
                    //目标的相对位置（）
                    Vector2 nowP305 = TargetPosition - (Vector2)ParentPokemonRoom.transform.position;
                    //获取目标所在象限的房间对角角落
                    Vector2 t305 = new Vector2(
                            ((nowP305.x < 0) ? ParentPokemonRoom.RoomSize[3] : ParentPokemonRoom.RoomSize[2]),
                            ((nowP305.y < 0) ? ParentPokemonRoom.RoomSize[0] : ParentPokemonRoom.RoomSize[1])
                        );
                    Vector2 offsetD305 = (_mTool.TiltMainVector2(nowP305)).normalized;
                    t305 = t305 + (Vector2)ParentPokemonRoom.transform.position;
                    switch (teamCombatActIndex)
                    {
                        //玛狃拉01 使用迷宫 跳到目标所在象限对角角落稍微外一点
                        case 1:
                            t305 = offsetD305 * 1.5f + t305;
                            break;
                        //玛狃拉02 使用看护迷宫 跳到目标所在象限对角角落稍微外一点+纵轴的位置
                        case 2:
                            t305 = offsetD305 * 1.5f + new Vector2(0.0f, offsetD305.y) * 2.0f + t305;
                            break;
                        //玛狃拉03 使用看护迷宫 跳到目标所在象限对角角落稍微外一点+横轴的位置
                        case 3:
                            t305 = offsetD305 * 1.5f + new Vector2(offsetD305.x, 0.0f) * 2.0f + t305;
                            break;
                        default:
                            Debug.Log("Error:三人连招回血连招1 跳跃时连招序列号分配错误");
                            break;
                    }
                    ParentPokemonRoom.EnsurePointReachesRoom(t305);
                    //跳跃固定点 时间固定 速度可变
                    Dir_Jump = (t305 - (Vector2)transform.position).normalized;
                    //Time_Jump_Max = Vector2.Distance(t304, (Vector2)transform.position) / (speed * SPEEDALPHA_JUMP_TEMASTATE304_LONESHADOW);
                    Time_Jump_Max = TIME_JUMP_TEMASTATE305_MAZE_JUMP;
                    SpeedAlpha_Jump = Vector2.Distance(t305, (Vector2)transform.position) / (speed * Time_Jump_Max);
                    SetDirector(_mTool.MainVector2(Dir_Jump));
                }
                break;
            //三人小队绕圈连招 脱离攻击
            case WeavileTeamBrain.SubState.TeamState_306_CircleRun:
                {
                    //向玩家跳跃固定距离
                    if (TeamBrain.NowTeamState306SubState == WeavileTeamBrain.TEAMSTATE306_SUBSTATE.EscapeAtk)
                    {
                        Dir_Jump = (TargetPosition - (Vector2)transform.position).normalized;
                        Vector2 t2 = Dir_Jump * DISTENCE_TARGET_JUMP_TEMASTATE306_CIRCLERUN_ESCAPEATK + (Vector2)transform.position;
                        t2 = ParentPokemonRoom.EnsurePointReachesRoom(t2);
                        Time_Jump_Max = Vector2.Distance(t2, (Vector2)transform.position) / (speed * SPEEDALPHA_JUMP_TEMASTATE302_CLOSEATK);
                        SetDirector(_mTool.MainVector2(Dir_Jump));
                    }
                }
                break;
            //三人小队回血连招1
            case WeavileTeamBrain.SubState.TeamState_307_Heal1:
                {
                    //玛狃拉的相对位置（玛狃拉12的平均点）
                    Vector2 nowP = ((TeamBrain.CombatWeavile01.transform.position + TeamBrain.CombatWeavile02.transform.position) / 2.0f) - ParentPokemonRoom.transform.position;
                    //获取所在象限的房间角落
                    Vector2 t307 = new Vector2(
                            ((nowP.x > 0) ? ParentPokemonRoom.RoomSize[3] : ParentPokemonRoom.RoomSize[2]),
                            ((nowP.y > 0) ? ParentPokemonRoom.RoomSize[0] : ParentPokemonRoom.RoomSize[1])
                        );
                    t307 = t307 + (Vector2)ParentPokemonRoom.transform.position;
                    Vector2 offsetD = -(_mTool.TiltMainVector2(nowP)).normalized;
                    switch (teamCombatActIndex)
                    {
                        //玛狃拉01 使用保护 跳到角落稍微外一点
                        case 1:
                            t307 = offsetD * 2.5f + t307;
                            break;
                        //玛狃拉02 使用回复 跳到更角落
                        case 2:
                            t307 = offsetD * 1.0f + t307;
                            break;
                        default:
                            Debug.Log("Error:三人连招回血连招1 跳跃时连招序列号分配错误");
                            break;
                    }
                    //跳跃固定点 时间固定 速度可变
                    Dir_Jump = (t307 - (Vector2)transform.position).normalized;
                    //Time_Jump_Max = Vector2.Distance(t304, (Vector2)transform.position) / (speed * SPEEDALPHA_JUMP_TEMASTATE304_LONESHADOW);
                    Time_Jump_Max = TIME_JUMP_TEMASTATE307_HEAL1_JUMP;
                    SpeedAlpha_Jump = Vector2.Distance(t307, (Vector2)transform.position) / (speed * Time_Jump_Max);
                    SetDirector(_mTool.MainVector2(Dir_Jump));
                }
                break;
            //三人小队回血连招2
            case WeavileTeamBrain.SubState.TeamState_308_Heal2:
                {
                    //玛狃拉的相对位置（玛狃拉123的平均点）
                    Vector2 nowP308 = ((TeamBrain.CombatWeavile01.transform.position + TeamBrain.CombatWeavile02.transform.position + TeamBrain.CombatWeavile03.transform.position) / 3.0f) - ParentPokemonRoom.transform.position;
                    //获取所在象限的房间角落
                    Vector2 t308 = new Vector2(
                            ((nowP308.x > 0) ? ParentPokemonRoom.RoomSize[3] : ParentPokemonRoom.RoomSize[2]),
                            ((nowP308.y > 0) ? ParentPokemonRoom.RoomSize[0] : ParentPokemonRoom.RoomSize[1])
                        );
                    t308 = t308 + (Vector2)ParentPokemonRoom.transform.position;
                    Vector2 offsetD308 = -(_mTool.TiltMainVector2(nowP308)).normalized;
                    switch (teamCombatActIndex)
                    {
                        //玛狃拉01 使用守卫 跳到角落稍微外一点
                        case 1:
                            t308 = offsetD308 * 1.0f + new Vector2(0.0f , offsetD308.y) * 1.75f + t308;
                            break;
                        //玛狃拉02 使用守卫 跳到角落稍微外一点
                        case 2:
                            t308 = offsetD308 * 1.0f + new Vector2(offsetD308.x, 0.0f) * 1.75f + t308;
                            break;
                        //玛狃拉03 使用回复 跳到更角落
                        case 3:
                            t308 = offsetD308 * 1.0f + t308;
                            break;
                        default:
                            Debug.Log("Error:三人连招回血连招1 跳跃时连招序列号分配错误");
                            break;
                    }
                    //跳跃固定点 时间固定 速度可变
                    Dir_Jump = (t308 - (Vector2)transform.position).normalized;
                    //Time_Jump_Max = Vector2.Distance(t304, (Vector2)transform.position) / (speed * SPEEDALPHA_JUMP_TEMASTATE304_LONESHADOW);
                    Time_Jump_Max = TIME_JUMP_TEMASTATE307_HEAL1_JUMP;
                    SpeedAlpha_Jump = Vector2.Distance(t308, (Vector2)transform.position) / (speed * Time_Jump_Max);
                    SetDirector(_mTool.MainVector2(Dir_Jump));
                }
                break;
            //二人小队近战连招 跳跃 跳跃至玛狃拉和目标连线上一定距离 并且在房间内
            case WeavileTeamBrain.SubState.TeamState_202_CloseAtk:
                {
                    Vector2 t = (((Vector2)transform.position - TargetPosition).normalized) * DISTENCE_TARGET_JUMP_TEMASTATE302_CLOSEATK + TargetPosition;
                    t = new Vector2(
                        Mathf.Clamp(t.x,
                            ParentPokemonRoom.RoomSize[2] + transform.parent.position.x, //最小值
                            ParentPokemonRoom.RoomSize[3] + transform.parent.position.x),//最大值
                        Mathf.Clamp(t.y,
                            ParentPokemonRoom.RoomSize[1] + transform.parent.position.y,  //最小值
                            ParentPokemonRoom.RoomSize[0] + transform.parent.position.y));//最大值
                    Debug.Log(t);
                    Dir_Jump = (t - (Vector2)transform.position).normalized;
                    Time_Jump_Max = Vector2.Distance(t, (Vector2)transform.position) / (speed * SPEEDALPHA_JUMP_TEMASTATE302_CLOSEATK);
                    SetDirector(_mTool.MainVector2(Dir_Jump));
                }
                break;
            //二人小队 替身连招
            case WeavileTeamBrain.SubState.TeamState_203_Substitute:
                {
                    switch (TeamBrain.NowTeamState203SubState)
                    {
                        //投掷阶段另外两只玛狃拉接近玛狃拉01
                        //向玛狃拉1跳跃 跳跃至玛狃拉和目标连线上一定距离 并且在房间内
                        case WeavileTeamBrain.TEAMSTATE203_SUBSTATE.Fling:
                            {
                                if (TeamCombatActIndex != 1)
                                {
                                    Vector2 t203 = (((Vector2)transform.position - (Vector2)TeamBrain.CombatWeavile01.transform.position).normalized) * DISTENCE_TARGET_JUMP_TEMASTATE303_SUBSTITUTE + (Vector2)TeamBrain.CombatWeavile01.transform.position;
                                    t203 = ParentPokemonRoom.EnsurePointReachesRoom(t203);
                                    Dir_Jump = (t203 - (Vector2)transform.position).normalized;
                                    Time_Jump_Max = Vector2.Distance(t203, (Vector2)transform.position) / (speed * SPEEDALPHA_JUMP_TEMASTATE302_CLOSEATK);
                                    SetDirector(_mTool.MainVector2(Dir_Jump));
                                }
                            }
                            break;
                        //跳跃阶段 跳跃到远离替身的房间角落
                        case WeavileTeamBrain.TEAMSTATE203_SUBSTATE.Jump:
                            {
                                Vector2 t203 = transform.position;
                                //检查替身存在
                                //Debug.Log(TeamBrain.TeamState203_Substitute_SubstituteOBJ);
                                if (TeamBrain.TeamState203_Substitute_SubstituteOBJ)
                                {
                                    //获取替身相对位置
                                    Vector2 SubstituteRelativePosition = TeamBrain.TeamState203_Substitute_SubstituteOBJ.transform.position - ParentPokemonRoom.transform.position;
                                    //替身在一四象限 目标点置于房间左
                                    if (SubstituteRelativePosition.x >= 0) { t203.x = ParentPokemonRoom.RoomSize[2] + 2.5f + ParentPokemonRoom.transform.position.x; }
                                    //替身在二三象限 目标点置于房间右
                                    else                                   { t203.x = ParentPokemonRoom.RoomSize[3] - 2.5f + ParentPokemonRoom.transform.position.x; }
                                    //替身在一二象限 目标点置于房间下
                                    if (SubstituteRelativePosition.y >= 0) { t203.y = ParentPokemonRoom.RoomSize[1] + 2.5f + ParentPokemonRoom.transform.position.y; }
                                    //替身在三四象限 目标点置于房间上
                                    else                                   { t203.y = ParentPokemonRoom.RoomSize[0] - 2.5f + ParentPokemonRoom.transform.position.y; }
                                    t203 = t203 + (Vector2)(Quaternion.AngleAxis(45.0f + 180.0f * (float)(teamCombatActIndex - 1), Vector3.forward) * Vector2.right * 1.5f);
                                    Dir_Jump = (t203 - (Vector2)transform.position).normalized;
                                    Time_Jump_Max = Vector2.Distance(t203, (Vector2)transform.position) / (speed * SPEEDALPHA_JUMP_TEMASTATE302_CLOSEATK);
                                    SetDirector(_mTool.MainVector2(Dir_Jump));
                                }
                                else
                                {
                                    Debug.Log("Error:二人小队 替身连招 替身已经被摧毁 但仍在执行");
                                    t203 = transform.position;
                                    Dir_Jump = Vector3.right;
                                    Time_Jump_Max = 0;
                                    SetDirector(_mTool.MainVector2(Dir_Jump));
                                }
                            }
                            break;
                        //其他状态 状态机错误 原地跳一下
                        default:
                            {
                                Debug.Log("Error:二人小队 替身连招 状态机错误");
                                Vector2 t2033 = transform.position;
                                Dir_Jump = Vector3.right;
                                Time_Jump_Max = 0;
                                SetDirector(_mTool.MainVector2(Dir_Jump));
                            }
                            break;
                    }
                }
                break;
            //二人小队 替身连招2
            case WeavileTeamBrain.SubState.TeamState_204_Substitute2:
                {
                    switch (TeamBrain.NowTeamState204SubState)
                    {
                        //投掷阶段玛狃拉02接近玛狃拉01
                        //向玛狃拉1跳跃 跳跃至玛狃拉和目标连线上一定距离 并且在房间内
                        case WeavileTeamBrain.TEAMSTATE204_SUBSTATE.Fling:
                            {
                                if (TeamCombatActIndex != 1)
                                {
                                    Vector2 t204 = (((Vector2)transform.position - (Vector2)TeamBrain.CombatWeavile01.transform.position).normalized) * DISTENCE_TARGET_JUMP_TEMASTATE303_SUBSTITUTE + (Vector2)TeamBrain.CombatWeavile01.transform.position;
                                    t204 = ParentPokemonRoom.EnsurePointReachesRoom(t204);
                                    Dir_Jump = (t204 - (Vector2)transform.position).normalized;
                                    Time_Jump_Max = Vector2.Distance(t204, (Vector2)transform.position) / (speed * SPEEDALPHA_JUMP_TEMASTATE302_CLOSEATK);
                                    SetDirector(_mTool.MainVector2(Dir_Jump));
                                }
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
                                        ParentPokemonRoom.RoomSize[0] + ParentPokemonRoom.transform.position.y, ParentPokemonRoom.RoomSize[1] + ParentPokemonRoom.transform.position.y, ParentPokemonRoom.RoomSize[2] + ParentPokemonRoom.transform.position.x, ParentPokemonRoom.RoomSize[3] + ParentPokemonRoom.transform.position.x
                                        ,transform);
                                    //Debug.Log(t204);
                                    t204 = ParentPokemonRoom.EnsurePointReachesRoom(t204);
                                    Dir_Jump = (t204 - (Vector2)transform.position).normalized;
                                    Time_Jump_Max = Vector2.Distance(t204, (Vector2)transform.position) / (speed * SPEEDALPHA_JUMP_TEMASTATE302_CLOSEATK);
                                    SetDirector(_mTool.MainVector2(Dir_Jump));
                                    if (Vector2.Distance(t204, (Vector2)transform.position) < 0.5f) { Dir_Jump = Vector2.zero; Time_Jump_Max = 0.0f;  }
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
            //二人小队 绕圈连招 脱离攻击
            case WeavileTeamBrain.SubState.TeamState_205_CircleRun:
                {
                    //向玩家跳跃固定距离
                    if (TeamBrain.NowTeamState205SubState == WeavileTeamBrain.TEAMSTATE306_SUBSTATE.EscapeAtk)
                    {
                        Dir_Jump = (TargetPosition - (Vector2)transform.position).normalized;
                        Vector2 t2 = Dir_Jump * DISTENCE_TARGET_JUMP_TEMASTATE306_CIRCLERUN_ESCAPEATK + (Vector2)transform.position;
                        t2 = ParentPokemonRoom.EnsurePointReachesRoom(t2);
                        Time_Jump_Max = Vector2.Distance(t2, (Vector2)transform.position) / (speed * SPEEDALPHA_JUMP_TEMASTATE302_CLOSEATK);
                        SetDirector(_mTool.MainVector2(Dir_Jump));
                    }
                }
                break;
            //二人小队 回血连招
            case WeavileTeamBrain.SubState.TeamState_206_Heal:
                {
                    //目标的相对位置（）
                    Vector2 nowP206 = TargetPosition - (Vector2)ParentPokemonRoom.transform.position;
                    //获取目标所在象限的房间对角角落
                    Vector2 t206 = new Vector2(
                            ((nowP206.x < 0) ? ParentPokemonRoom.RoomSize[3] : ParentPokemonRoom.RoomSize[2]),
                            ((nowP206.y < 0) ? ParentPokemonRoom.RoomSize[0] : ParentPokemonRoom.RoomSize[1])
                        );
                    Vector2 offsetD206 = (_mTool.TiltMainVector2(nowP206)).normalized;
                    t206 = t206 + (Vector2)ParentPokemonRoom.transform.position;
                    switch (teamCombatActIndex)
                    {
                        //玛狃拉01 使用迷宫 跳到目标所在象限对角角落稍微外一点
                        case 1:
                            t206 = offsetD206 * 1.5f + t206;
                            break;
                        default:
                            Debug.Log("Error:三人连招回血连招1 跳跃时连招序列号分配错误");
                            break;
                    }
                    ParentPokemonRoom.EnsurePointReachesRoom(t206);
                    //跳跃固定点 时间固定 速度可变
                    Dir_Jump = (t206 - (Vector2)transform.position).normalized;
                    Time_Jump_Max = TIME_JUMP_TEMASTATE206_HEAL_JUMP;
                    SpeedAlpha_Jump = Vector2.Distance(t206, (Vector2)transform.position) / (speed * Time_Jump_Max);
                    SetDirector(_mTool.MainVector2(Dir_Jump));
                }
                break;
            //二人小队 迷宫连招
            case WeavileTeamBrain.SubState.TeamState_207_Maze:
                {
                    //目标的相对位置（）
                    Vector2 nowP207 = TargetPosition - (Vector2)ParentPokemonRoom.transform.position;
                    //获取目标所在象限的房间对角角落
                    Vector2 t207 = new Vector2(
                            ((nowP207.x < 0) ? ParentPokemonRoom.RoomSize[3] : ParentPokemonRoom.RoomSize[2]),
                            ((nowP207.y < 0) ? ParentPokemonRoom.RoomSize[0] : ParentPokemonRoom.RoomSize[1])
                        );
                    Vector2 offsetD207 = (_mTool.TiltMainVector2(nowP207)).normalized;
                    t207 = t207 + (Vector2)ParentPokemonRoom.transform.position;
                    switch (teamCombatActIndex)
                    {
                        //玛狃拉01 使用迷宫 跳到目标所在象限对角角落稍微外一点
                        case 1:
                            t207 = offsetD207 * 1.5f + t207;
                            break;
                        //玛狃拉02 使用看护迷宫 跳到目标所在象限对角角落稍微外一点+横轴的位置
                        case 2:
                            t207 = offsetD207 * 4.0f + t207;
                            break;
                        default:
                            Debug.Log("Error:三人连招回血连招1 跳跃时连招序列号分配错误");
                            break;
                    }
                    ParentPokemonRoom.EnsurePointReachesRoom(t207);
                    //跳跃固定点 时间固定 速度可变
                    Dir_Jump = (t207 - (Vector2)transform.position).normalized;
                    //Time_Jump_Max = Vector2.Distance(t304, (Vector2)transform.position) / (speed * SPEEDALPHA_JUMP_TEMASTATE304_LONESHADOW);
                    Time_Jump_Max = TIME_JUMP_TEMASTATE207_MAZE_JUMP;
                    SpeedAlpha_Jump = Vector2.Distance(t207, (Vector2)transform.position) / (speed * Time_Jump_Max);
                    SetDirector(_mTool.MainVector2(Dir_Jump));
                }
                break;
        }
        //开启残影
        if (ShadowCoroutine == null)
        {
            StartShadowCoroutine(0.1f, 1.5f, new Color(0.6603774f, 0.6603774f, 0.6603774f, 0.6f ),Vector2.zero, SpriteTransform);
        }
    }

    /// <summary>
    /// 跳跃作为动作结束
    /// </summary>
    public void AnimatorEvent_JumpOver()
    {
        isMove_Jump = false;
        isPrepare_Jump = false;
        InstantiateRunDust();
        //关闭残影
        if (ShadowCoroutine != null)
        {
            StopShadowCoroutine();
        }
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
            //三人小队近战连招 连招完毕 
            case WeavileTeamBrain.SubState.TeamState_302_CloseAtk:
                JumpMoveOver();
                isCombatComplite_TeamState302_CloseAtk = true;  
                break;
            //三人小队替身连招 跳跃完毕 
            case WeavileTeamBrain.SubState.TeamState_303_Substitute:
                JumpMoveOver();
                isCombatComplite_TeamState303_Substitute = true;
                //三人小队替身连招 跳跃阶段 跳跃完毕后面向玩家
                if (TeamBrain.NowTeamState303SubState == WeavileTeamBrain.TEAMSTATE303_SUBSTATE.Jump)
                {
                    SetDirector(_mTool.MainVector2((TargetPosition - (Vector2)transform.position).normalized));
                }
                break;
            //三人小队分身连招 跳跃完毕 
            case WeavileTeamBrain.SubState.TeamState_304_CloneShadow:
                JumpMoveOver();
                //根据分身连招状态不同 跳跃结束事件不同
                switch (TeamBrain.NowTeamState304CloneShadow)
                {
                    //第一次跳跃
                    case WeavileTeamBrain.TEAMSTATE304_CLONESHADOW.CloneShadow:
                        //根据冲刺方向确认朝向 发射冰粒
                        SetDirector(new Vector2(TeamBrain.isLeft_TeamState304CloneShadow_JumpStart ? 1:-1 , 0.0f));
                        //IceShardStart(TIME_ICESHARD_TEMASTATE304IDLE_JUMP);
                        isCombatComplite_TeamState304_CloneShadow = true;
                        break;
                    //之后的跳跃
                    case WeavileTeamBrain.TEAMSTATE304_CLONESHADOW.Jump:
                        SetDirector(new Vector2(TeamBrain.isLeft_TeamState304CloneShadow_JumpStart ? 1 : -1, 0.0f));
                        //IceShardStart(TIME_ICESHARD_TEMASTATE304IDLE_JUMP);
                        isCombatComplite_TeamState304_CloneShadow = true;
                        break;
                    //其他状态进入跳跃 说明状态机错误
                    default:
                        Debug.Log("Error:三人小队 分身连招 状态机错误 不该跳跃");
                        break;
                }
                break;
            //三人小队迷宫连招 跳跃完毕 
            case WeavileTeamBrain.SubState.TeamState_305_Maze:
                JumpMoveOver();
                //根据连招分配不同 跳跃结束事件不同
                switch (teamCombatActIndex)
                {
                    //玛狃拉01 使用释放迷宫
                    case 1:
                        MazeStart(TIME_MAZE_TEMASTATE305_MAZE);
                        break;
                    //玛狃拉02 使用首次投掷
                    case 2:
                        FlingStart();
                        break;
                    //玛狃拉03 使用首次投掷
                    case 3:
                        FlingStart();
                        break;
                    default:
                        Debug.Log("Error:三人连招回血连招1 跳跃时连招序列号分配错误");
                        break;
                }
                break;
            //三人小队绕圈连招 脱离进攻 跳跃结束 开始冲刺
            case WeavileTeamBrain.SubState.TeamState_306_CircleRun:
                if (TeamBrain.NowTeamState306SubState == WeavileTeamBrain.TEAMSTATE306_SUBSTATE.EscapeAtk) {
                    JumpMoveOver();
                    RushStart();
                }
                break;
            //三人小队回血连招1 玛狃拉01转入保护 玛狃拉02转入回血
            case WeavileTeamBrain.SubState.TeamState_307_Heal1:
                switch (teamCombatActIndex)
                {
                    //玛狃拉01 使用保护
                    case 1:
                        JumpMoveOver();
                        UseProtectStart(TIME_USEPROTECT_TEMASTATE307_HEAL1);
                        break;
                    //玛狃拉02 使用回复
                    case 2:
                        JumpMoveOver();
                        HealStart(TIME_HEAL_TEMASTATE307_HEAL1);
                        break;
                    default:
                        Debug.Log("Error:三人连招回血连招1 跳跃时连招序列号分配错误");
                        break;
                }
                break;
            //三人小队回血连招2 玛狃拉01转入保护 玛狃拉02转入回血
            case WeavileTeamBrain.SubState.TeamState_308_Heal2:
                switch (teamCombatActIndex)
                {
                    //玛狃拉01 使用守卫
                    case 1:
                        JumpMoveOver();
                        GuardStart();
                        break;
                    //玛狃拉02 使用守卫
                    case 2:
                        JumpMoveOver();
                        GuardStart();
                        break;
                    //玛狃拉03 使用回复
                    case 3:
                        JumpMoveOver();
                        HealStart(TIME_HEAL_TEMASTATE308_HEAL2);
                        break;
                    default:
                        Debug.Log("Error:三人连招回血连招1 跳跃时连招序列号分配错误");
                        break;
                }
                break;
            //二人小队近战连招 连招完毕 
            case WeavileTeamBrain.SubState.TeamState_202_CloseAtk:
                JumpMoveOver();
                isCombatComplite_TeamState202_CloseAtk = true;
                break;
            //二人小队替身连招 跳跃完毕 
            case WeavileTeamBrain.SubState.TeamState_203_Substitute:
                JumpMoveOver();
                isCombatComplite_TeamState203_Substitute = true;
                //三人小队替身连招 跳跃阶段 跳跃完毕后面向玩家
                if (TeamBrain.NowTeamState203SubState == WeavileTeamBrain.TEAMSTATE203_SUBSTATE.Jump)
                {
                    SetDirector(_mTool.MainVector2((TargetPosition - (Vector2)transform.position).normalized));
                }
                break;
            //二人小队替身连招2 跳跃完毕 
            case WeavileTeamBrain.SubState.TeamState_204_Substitute2:
                JumpMoveOver();
                isCombatComplite_TeamState204_Substitute2 = true;
                //三人小队替身连招2 跳跃阶段 跳跃完毕后面向玩家
                if (TeamBrain.NowTeamState204SubState == WeavileTeamBrain.TEAMSTATE204_SUBSTATE.Jump)
                {
                    SetDirector(_mTool.MainVector2((TargetPosition - (Vector2)transform.position).normalized));
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
            //二人小队 回血连招 跳跃完毕 
            case WeavileTeamBrain.SubState.TeamState_206_Heal:
                JumpMoveOver();
                //根据连招分配不同 跳跃结束事件不同
                switch (teamCombatActIndex)
                {
                    //玛狃拉01 使用释放迷宫
                    case 1:
                        MazeStart(TIME_MAZE_TEMASTATE206_HEAL);
                        break;
                    default:
                        Debug.Log("Error:三人连招回血连招1 跳跃时连招序列号分配错误");
                        break;
                }
                break;
            //二人小队 迷宫连招 跳跃完毕 
            case WeavileTeamBrain.SubState.TeamState_207_Maze:
                JumpMoveOver();
                //根据连招分配不同 跳跃结束事件不同
                switch (teamCombatActIndex)
                {
                    //玛狃拉01 使用释放迷宫
                    case 1:
                        MazeStart(TIME_MAZE_TEMASTATE207_MAZE);
                        break;
                    //玛狃拉02 使用首次投掷
                    case 2:
                        FlingStart();
                        break;
                    default:
                        Debug.Log("Error:二人小队 迷宫连招 跳跃时连招序列号分配错误");
                        break;
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
            case WeavileTeamBrain.SubState.TeamState_302_CloseAtk:
                //三人小队_近战连招 不是第一次冲刺 预判
                if (TeamBrain.Count_Teamstate302_Close_Atk != 0)
                {
                    Angle_PredictLastAngle_Rush = _mTool.Angle_360Y((TargetPosition - (Vector2)transform.position).normalized, Vector2.right);
                    isPredict_RushAngle = true;
                }
                break;
            case WeavileTeamBrain.SubState.TeamState_303_Substitute:
                //三人小队_替身连招 最后冲刺 预判
                if (TeamBrain.NowTeamState303SubState == WeavileTeamBrain.TEAMSTATE303_SUBSTATE.Rush)
                {
                    Angle_PredictLastAngle_Rush = _mTool.Angle_360Y((TargetPosition - (Vector2)transform.position).normalized, Vector2.right);
                    isPredict_RushAngle = true;
                }
                break;
            case WeavileTeamBrain.SubState.TeamState_304_CloneShadow:
                //三人小队_分身连招 意外冲刺 预判
                if (TeamBrain.NowTeamState304CloneShadow == WeavileTeamBrain.TEAMSTATE304_CLONESHADOW.AccidentRush)
                {
                    Angle_PredictLastAngle_Rush = _mTool.Angle_360Y((TargetPosition - (Vector2)transform.position).normalized, Vector2.right);
                    isPredict_RushAngle = true;
                }
                break;
            case WeavileTeamBrain.SubState.TeamState_305_Maze:
                //三人小队_迷宫连招 不预判
                break;
            case WeavileTeamBrain.SubState.TeamState_306_CircleRun:
                //三人小队_绕圈连招 外环攻击 预判
                if (TeamBrain.NowTeamState306SubState == WeavileTeamBrain.TEAMSTATE306_SUBSTATE.OuterAtk)
                {
                    Angle_PredictLastAngle_Rush = _mTool.Angle_360Y((TargetPosition - (Vector2)transform.position).normalized, Vector2.right);
                    isPredict_RushAngle = true;
                }
                break;
            case WeavileTeamBrain.SubState.TeamState_307_Heal1:
                //三人小队_回血连招1 预判
                Angle_PredictLastAngle_Rush = _mTool.Angle_360Y((TargetPosition - (Vector2)transform.position).normalized, Vector2.right);
                isPredict_RushAngle = true;
                break;
            case WeavileTeamBrain.SubState.TeamState_309_FullHeal:
                //三人小队_万灵药连招连招 预判
                if (TeamBrain.NowTeamState309SubState == WeavileTeamBrain.TEAMSTATE309_SUBSTATE.Rush)
                {
                    Angle_PredictLastAngle_Rush = _mTool.Angle_360Y((TargetPosition - (Vector2)transform.position).normalized, Vector2.right);
                    isPredict_RushAngle = true;
                }
                break;
            case WeavileTeamBrain.SubState.TeamState_202_CloseAtk:
                //二人小队_近战连招 不是第一次冲刺 预判
                if (TeamBrain.Count_Teamstate202_Close_Atk != 0)
                {
                    Angle_PredictLastAngle_Rush = _mTool.Angle_360Y((TargetPosition - (Vector2)transform.position).normalized, Vector2.right);
                    isPredict_RushAngle = true;
                }
                break;
            case WeavileTeamBrain.SubState.TeamState_203_Substitute:
                //二人小队_替身连招 最后冲刺 预判
                if (TeamBrain.NowTeamState203SubState == WeavileTeamBrain.TEAMSTATE203_SUBSTATE.Rush)
                {
                    Angle_PredictLastAngle_Rush = _mTool.Angle_360Y((TargetPosition - (Vector2)transform.position).normalized, Vector2.right);
                    isPredict_RushAngle = true;
                }
                break;
            case WeavileTeamBrain.SubState.TeamState_204_Substitute2:
                //二人小队_替身连招2 最后冲刺 预判
                if (TeamBrain.NowTeamState204SubState == WeavileTeamBrain.TEAMSTATE204_SUBSTATE.Rush)
                {
                    Angle_PredictLastAngle_Rush = _mTool.Angle_360Y((TargetPosition - (Vector2)transform.position).normalized, Vector2.right);
                    isPredict_RushAngle = true;
                }
                break;
            case WeavileTeamBrain.SubState.TeamState_205_CircleRun:
                //二人小队_绕圈连招 外环攻击 预判
                if (TeamBrain.NowTeamState205SubState == WeavileTeamBrain.TEAMSTATE306_SUBSTATE.OuterAtk)
                {
                    Angle_PredictLastAngle_Rush = _mTool.Angle_360Y((TargetPosition - (Vector2)transform.position).normalized, Vector2.right);
                    isPredict_RushAngle = true;
                }
                break;
            case WeavileTeamBrain.SubState.TeamState_207_Maze:
                //二人小队_迷宫连招 不预判
                break;
            case WeavileTeamBrain.SubState.TeamState_208_FullHeal:
                //二人小队_万灵药连招连招 预判
                if (TeamBrain.NowTeamState208SubState == WeavileTeamBrain.TEAMSTATE309_SUBSTATE.Rush)
                {
                    Angle_PredictLastAngle_Rush = _mTool.Angle_360Y((TargetPosition - (Vector2)transform.position).normalized, Vector2.right);
                    isPredict_RushAngle = true;
                }
                break;
            case WeavileTeamBrain.SubState.TeamState_100_Normal:
                //一人小队 
                switch (TeamBrain.NowTeamState100SubState)
                {
                    case WeavileTeamBrain.TEAMSTATE100_SUBSTATE.Rush:
                        //一人小队 预判
                        if (TeamBrain.NowTeamState100SubState == WeavileTeamBrain.TEAMSTATE100_SUBSTATE.Rush)
                        {
                            Angle_PredictLastAngle_Rush = _mTool.Angle_360Y((TargetPosition - (Vector2)transform.position).normalized, Vector2.right);
                            isPredict_RushAngle = true;
                        }
                        break;
                    default:
                        break;
                }
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
            case WeavileTeamBrain.SubState.TeamState_302_CloseAtk:
                //三人小队_近战连招 不是第一次冲刺预判
                if (TeamBrain.Count_Teamstate302_Close_Atk != 0)
                {
                    float Angle_PredictLastAngle_Rush02 = _mTool.Angle_360Y((TargetPosition - (Vector2)transform.position).normalized, Vector2.right);
                    float PredictAngle = 2.0f * Angle_PredictLastAngle_Rush02 - Angle_PredictLastAngle_Rush;
                    Dir_Rush = Quaternion.AngleAxis(PredictAngle, Vector3.forward) * Vector3.right;
                    isPredict_RushAngle = false;
                }
                break;
            case WeavileTeamBrain.SubState.TeamState_303_Substitute:
                //三人小队_替身连招 最后冲刺 预判
                if (TeamBrain.NowTeamState303SubState == WeavileTeamBrain.TEAMSTATE303_SUBSTATE.Rush)
                {
                    float Angle_PredictLastAngle_Rush02 = _mTool.Angle_360Y((TargetPosition - (Vector2)transform.position).normalized, Vector2.right);
                    float PredictAngle = 2.0f * Angle_PredictLastAngle_Rush02 - Angle_PredictLastAngle_Rush;
                    Dir_Rush = Quaternion.AngleAxis(PredictAngle, Vector3.forward) * Vector3.right;
                    isPredict_RushAngle = false;
                }
                break;
            case WeavileTeamBrain.SubState.TeamState_304_CloneShadow:
                //三人小队_分身连招 意外冲刺 预判
                if (TeamBrain.NowTeamState304CloneShadow == WeavileTeamBrain.TEAMSTATE304_CLONESHADOW.AccidentRush)
                {
                    float Angle_PredictLastAngle_Rush02 = _mTool.Angle_360Y((TargetPosition - (Vector2)transform.position).normalized, Vector2.right);
                    float PredictAngle = 2.0f * Angle_PredictLastAngle_Rush02 - Angle_PredictLastAngle_Rush;
                    Dir_Rush = Quaternion.AngleAxis(PredictAngle, Vector3.forward) * Vector3.right;
                    isPredict_RushAngle = false;
                }
                break;
            case WeavileTeamBrain.SubState.TeamState_305_Maze:
                //三人小队_迷宫连招 不预判
                break;
            case WeavileTeamBrain.SubState.TeamState_306_CircleRun:
                //三人小队_绕圈连招 外环攻击 预判
                if (TeamBrain.NowTeamState306SubState == WeavileTeamBrain.TEAMSTATE306_SUBSTATE.OuterAtk)
                {
                    float Angle_PredictLastAngle_Rush02 = _mTool.Angle_360Y((TargetPosition - (Vector2)transform.position).normalized, Vector2.right);
                    float PredictAngle = 2.0f * Angle_PredictLastAngle_Rush02 - Angle_PredictLastAngle_Rush;
                    Dir_Rush = Quaternion.AngleAxis(PredictAngle, Vector3.forward) * Vector3.right;
                    isPredict_RushAngle = false;
                }
                break;
            case WeavileTeamBrain.SubState.TeamState_307_Heal1:
                //三人小队_回血连招1 预判
                if (true)
                {
                    float Angle_PredictLastAngle_Rush02 = _mTool.Angle_360Y((TargetPosition - (Vector2)transform.position).normalized, Vector2.right);
                    float PredictAngle = 2.0f * Angle_PredictLastAngle_Rush02 - Angle_PredictLastAngle_Rush;
                    Dir_Rush = Quaternion.AngleAxis(PredictAngle, Vector3.forward) * Vector3.right;
                    isPredict_RushAngle = false;
                }
                break;
            case WeavileTeamBrain.SubState.TeamState_309_FullHeal:
                //三人小队_万灵药连招 预判
                if (TeamBrain.NowTeamState309SubState == WeavileTeamBrain.TEAMSTATE309_SUBSTATE.Rush)
                {
                    float Angle_PredictLastAngle_Rush02 = _mTool.Angle_360Y((TargetPosition - (Vector2)transform.position).normalized, Vector2.right);
                    float PredictAngle = 2.0f * Angle_PredictLastAngle_Rush02 - Angle_PredictLastAngle_Rush;
                    Dir_Rush = Quaternion.AngleAxis(PredictAngle, Vector3.forward) * Vector3.right;
                    isPredict_RushAngle = false;
                }
                break;
            case WeavileTeamBrain.SubState.TeamState_202_CloseAtk:
                //二人小队_近战连招 不是第一次冲刺预判
                if (TeamBrain.Count_Teamstate202_Close_Atk != 0)
                {
                    float Angle_PredictLastAngle_Rush02 = _mTool.Angle_360Y((TargetPosition - (Vector2)transform.position).normalized, Vector2.right);
                    float PredictAngle = 2.0f * Angle_PredictLastAngle_Rush02 - Angle_PredictLastAngle_Rush;
                    Dir_Rush = Quaternion.AngleAxis(PredictAngle, Vector3.forward) * Vector3.right;
                    isPredict_RushAngle = false;
                }
                break;
            case WeavileTeamBrain.SubState.TeamState_203_Substitute:
                //二人小队_替身连招 最后冲刺 预判
                if (TeamBrain.NowTeamState203SubState == WeavileTeamBrain.TEAMSTATE203_SUBSTATE.Rush)
                {
                    float Angle_PredictLastAngle_Rush02 = _mTool.Angle_360Y((TargetPosition - (Vector2)transform.position).normalized, Vector2.right);
                    float PredictAngle = 2.0f * Angle_PredictLastAngle_Rush02 - Angle_PredictLastAngle_Rush;
                    Dir_Rush = Quaternion.AngleAxis(PredictAngle, Vector3.forward) * Vector3.right;
                    isPredict_RushAngle = false;
                }
                break;
            case WeavileTeamBrain.SubState.TeamState_204_Substitute2:
                //二人小队_替身连招 最后冲刺 预判
                if (TeamBrain.NowTeamState204SubState == WeavileTeamBrain.TEAMSTATE204_SUBSTATE.Rush)
                {
                    float Angle_PredictLastAngle_Rush02 = _mTool.Angle_360Y((TargetPosition - (Vector2)transform.position).normalized, Vector2.right);
                    float PredictAngle = 2.0f * Angle_PredictLastAngle_Rush02 - Angle_PredictLastAngle_Rush;
                    Dir_Rush = Quaternion.AngleAxis(PredictAngle, Vector3.forward) * Vector3.right;
                    isPredict_RushAngle = false;
                }
                break;
            case WeavileTeamBrain.SubState.TeamState_205_CircleRun:
                //二人小队_绕圈连招 外环攻击 预判
                if (TeamBrain.NowTeamState205SubState == WeavileTeamBrain.TEAMSTATE306_SUBSTATE.OuterAtk)
                {
                    float Angle_PredictLastAngle_Rush02 = _mTool.Angle_360Y((TargetPosition - (Vector2)transform.position).normalized, Vector2.right);
                    float PredictAngle = 2.0f * Angle_PredictLastAngle_Rush02 - Angle_PredictLastAngle_Rush;
                    Dir_Rush = Quaternion.AngleAxis(PredictAngle, Vector3.forward) * Vector3.right;
                    isPredict_RushAngle = false;
                }
                break;
            case WeavileTeamBrain.SubState.TeamState_207_Maze:
                //二人小队_迷宫连招 不预判
                break;
            case WeavileTeamBrain.SubState.TeamState_208_FullHeal:
                //二人小队_万灵药连招 预判
                if (TeamBrain.NowTeamState208SubState == WeavileTeamBrain.TEAMSTATE309_SUBSTATE.Rush)
                {
                    float Angle_PredictLastAngle_Rush02 = _mTool.Angle_360Y((TargetPosition - (Vector2)transform.position).normalized, Vector2.right);
                    float PredictAngle = 2.0f * Angle_PredictLastAngle_Rush02 - Angle_PredictLastAngle_Rush;
                    Dir_Rush = Quaternion.AngleAxis(PredictAngle, Vector3.forward) * Vector3.right;
                    isPredict_RushAngle = false;
                }
                break;
            case WeavileTeamBrain.SubState.TeamState_100_Normal:
                //一人小队 
                switch (TeamBrain.NowTeamState100SubState)
                {
                    case WeavileTeamBrain.TEAMSTATE100_SUBSTATE.Rush:
                        //一人小队 预判
                        if (TeamBrain.NowTeamState100SubState == WeavileTeamBrain.TEAMSTATE100_SUBSTATE.Rush)
                        {
                            float Angle_PredictLastAngle_Rush02 = _mTool.Angle_360Y((TargetPosition - (Vector2)transform.position).normalized, Vector2.right);
                            float PredictAngle = 2.0f * Angle_PredictLastAngle_Rush02 - Angle_PredictLastAngle_Rush;
                            Dir_Rush = Quaternion.AngleAxis(PredictAngle, Vector3.forward) * Vector3.right;
                            isPredict_RushAngle = false;
                        }
                        break;
                    default:
                        break;
                }
                break;
        }
        //冲刺开始
        if (NowMainState == MainState.Rush)
        {
            Dir_Rush = ConfusionDir(Dir_Rush, 30.0f);
            if (ShadowCoroutine == null)
            {
                StartShadowCoroutine(0.05f, 1.5f, new Color(0.6603774f, 0.6603774f, 0.6603774f, 0.6f));
            }
            LaunchRushEffect(NowRushType, Dir_Rush);//特效生成
            audioPlayer.Play(WeavileSE.Rush, transform.position);
        }
    }

    /// <summary>
    /// 冲刺作为动作结束
    /// </summary>
    public void AnimatorEvent_RushOver()
    {
        isCharge_Rush = false;
        isMove_Rush = false;
        InstantiateRunDust(); //冲刺落地烟尘
        OverRushEffect();     //冲刺特效终结
        //落点特效
        switch (NowRushType)
        {
            case RushType.Ice:
                //冰系冲刺落点发射冰粒
                LaunchRushOverIceShard();
                break;
            case RushType.Dark:
                //恶习冲刺落点发射恶波
                LaunchDarkPulse();
                break;
        }
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
            //三人小队近战连招 连招动作完毕 
            case WeavileTeamBrain.SubState.TeamState_302_CloseAtk:
                RushOver();
                isCombatComplite_TeamState302_CloseAtk = true;
                break;
            //三人小队替身连招 连招动作完毕 
            case WeavileTeamBrain.SubState.TeamState_303_Substitute:
                RushOver();
                isCombatComplite_TeamState303_Substitute = true;
                break;
            //三人小队分身连招 连招动作完毕 
            case WeavileTeamBrain.SubState.TeamState_304_CloneShadow:
                RushOver();
                isCombatComplite_TeamState304_CloneShadow = true;
                break;
            case WeavileTeamBrain.SubState.TeamState_305_Maze:
                {
                    //三人小队_迷宫连招 冲刺完毕 发射冰粒或投掷 
                    RushOver();
                    float r = Random.Range(0.0f, 1.0f);
                    //33%的概率使用冰粒
                    if (r >= 0.0f && r < 0.33f) { IceShardStart(TIME_ICESHARD_TEMASTATE305MAZE_RUSH); }
                    //67%的概率使用投掷
                    else { FlingStart(); }
                }
                break;
            //三人小队_绕圈连招 连招动作完毕 
            case WeavileTeamBrain.SubState.TeamState_306_CircleRun:
                RushOver(); 
                switch (TeamBrain.NowTeamState306SubState)
                {
                    case WeavileTeamBrain.TEAMSTATE306_SUBSTATE.InnerAtk1: isCombatComplite_TeamState306_CirclrRun_InnerAtk = true;break;//内环攻击1
                    case WeavileTeamBrain.TEAMSTATE306_SUBSTATE.InnerAtk2: isCombatComplite_TeamState306_CirclrRun_InnerAtk = true;break;//内环攻击2
                    case WeavileTeamBrain.TEAMSTATE306_SUBSTATE.OuterAtk: isCombatComplite_TeamState306_CircleRun_OuterAtk = true; break;//外环攻击
                    case WeavileTeamBrain.TEAMSTATE306_SUBSTATE.EscapeAtk: FlingStart(); ; break;//脱离攻击 冲刺结束 转入投掷
                    default: IdleStart(TIME_IDLE_START) ;break;//其他状态
                }
                break;
            //三人小队_回血连招1 连招动作完毕 
            case WeavileTeamBrain.SubState.TeamState_307_Heal1:
                RushOver();
                //回复结束 动作设为完成
                if (TeamBrain.isOver_Heal_TeamState307Heal1)
                {
                    isCombatComplite_TeamState307_Heal = true;
                }
                //回复未结束 开启下一次冲刺
                else
                {
                    RushStart();
                }
                break;
            //三人小队_万灵药连招 接投掷
            case WeavileTeamBrain.SubState.TeamState_309_FullHeal:
                RushOver();
                FlingStart();
                break;
            //二人小队近战连招 连招动作完毕 
            case WeavileTeamBrain.SubState.TeamState_202_CloseAtk:
                RushOver();
                isCombatComplite_TeamState202_CloseAtk = true;
                break;
            //二人小队替身连招 连招动作完毕 
            case WeavileTeamBrain.SubState.TeamState_203_Substitute:
                RushOver();
                isCombatComplite_TeamState203_Substitute = true;
                break;
            //二人小队替身连招2 连招动作完毕 
            case WeavileTeamBrain.SubState.TeamState_204_Substitute2:
                RushOver();
                isCombatComplite_TeamState204_Substitute2 = true;
                break;
            //二人小队_绕圈连招 连招动作完毕 
            case WeavileTeamBrain.SubState.TeamState_205_CircleRun:
                RushOver();
                switch (TeamBrain.NowTeamState205SubState)
                {
                    case WeavileTeamBrain.TEAMSTATE306_SUBSTATE.InnerAtk1: isCombatComplite_TeamState205_CirclrRun_InnerAtk = true; break;//内环攻击1
                    case WeavileTeamBrain.TEAMSTATE306_SUBSTATE.InnerAtk2: isCombatComplite_TeamState205_CirclrRun_InnerAtk = true; break;//内环攻击2
                    case WeavileTeamBrain.TEAMSTATE306_SUBSTATE.OuterAtk: isCombatComplite_TeamState205_CircleRun_OuterAtk = true; break;//外环攻击
                    case WeavileTeamBrain.TEAMSTATE306_SUBSTATE.EscapeAtk: FlingStart(); ; break;//脱离攻击 冲刺结束 转入投掷
                    default: IdleStart(TIME_IDLE_START); break;//其他状态
                }
                break;
            case WeavileTeamBrain.SubState.TeamState_207_Maze:
                //二人小队_迷宫连招 冲刺完毕 发射冰粒或投掷 
                {
                    RushOver();
                    float r = Random.Range(0.0f, 1.0f);
                    //33%的概率使用冰粒
                    if (r >= 0.0f && r < 0.33f) { IceShardStart(TIME_ICESHARD_TEMASTATE305MAZE_RUSH); }
                    //67%的概率使用投掷
                    else { FlingStart(); }
                }
                break;
            //二人小队_万灵药连招 接投掷
            case WeavileTeamBrain.SubState.TeamState_208_FullHeal:
                RushOver();
                FlingStart();
                break;
            case WeavileTeamBrain.SubState.TeamState_100_Normal:
                //一人小队 
                {
                    RushOver();
                    TeamBrain.NowTeamState100SubState = WeavileTeamBrain.TEAMSTATE100_SUBSTATE.Idle;
                    IdleStart(TIME_IDLE_TEAMSTATE100_NORMAL);
                }
                break;
        }
    }

    //=========================冲刺事件============================










    //=========================迷宫冥想（使用保护）事件============================

    /// <summary>
    /// 帮助时的虚影颜色 红色
    /// </summary>
    public static Color COLOR_OUTLINEANDSHADOW_HELPINGRED = new Color(1,0,0);
    /// <summary>
    /// 守住时的虚影颜色 蓝色
    /// </summary>
    public static Color COLOR_OUTLINEANDSHADOW_PROTECTBLUE = new Color(0.3101193f, 0.6955579f, 0.9528302f);
    /// <summary>
    /// 回血时的虚影颜色 绿色
    /// </summary>
    public static Color COLOR_OUTLINEANDSHADOW_HEALGREEN = new Color(0, 1, 0.6050956f);
    /// <summary>
    /// 迷宫时的虚影颜色 粉色
    /// </summary>
    public static Color COLOR_OUTLINEANDSHADOW_MAZEPINK = new Color(1, 0.4103774f, 0.7141634f);

    public List<SpriteRenderer> OutlineAndShadowSpriteList;

    /// <summary>
    /// 设定描边和虚影的颜色
    /// </summary>
    void SetOutLineAndShadowColor(Color c)
    {
        for (int i = 0; i < OutlineAndShadowSpriteList.Count; i++)
        {
            OutlineAndShadowSpriteList[i].color = c;
        }
    }



    /// <summary>
    /// 迷宫冥想作为动作结束
    /// </summary>
    public void AnimatorEvent_MazeOut()
    {
        switch (NowMainState)
        {
            case MainState.UseProtect:
                switch (TeamBrain.NowSubState)
                {
                    //三人小队近战连招 连招完毕 
                    case WeavileTeamBrain.SubState.TeamState_302_CloseAtk:
                        UseProtectOver();
                        isCombatComplite_TeamState302_CloseAtk = true;
                        break;
                    //三人小队替身连招 连招完毕 
                    case WeavileTeamBrain.SubState.TeamState_303_Substitute:
                        UseProtectOver();
                        isCombatComplite_TeamState303_Substitute = true;
                        break;
                    //三人小队回血连招 保护完毕 进入保护的间隔等待期间 
                    case WeavileTeamBrain.SubState.TeamState_307_Heal1:
                        UseProtectOver();
                        UseProtectTimer = TIME_USEPROTECT_TEMASTATE307_HEAL1_INTERVAL;
                        //回复结束 动作设为完成
                        if (TeamBrain.isOver_Heal_TeamState307Heal1)
                        {
                            isCombatComplite_TeamState307_Heal = true;
                        }
                        //回复未结束 开始间隔时间
                        else
                        {
                            isInterval_UseProtect = true;
                        }
                        break;
                    //二人小队近战连招 连招完毕 
                    case WeavileTeamBrain.SubState.TeamState_202_CloseAtk:
                        UseProtectOver();
                        isCombatComplite_TeamState202_CloseAtk = true;
                        break;
                    //二人小队替身连招 连招完毕 
                    case WeavileTeamBrain.SubState.TeamState_203_Substitute:
                        UseProtectOver();
                        isCombatComplite_TeamState203_Substitute = true;
                        break;
                }
                break;
            case MainState.Maze:
                switch (TeamBrain.NowSubState)
                {
                    case WeavileTeamBrain.SubState.TeamState_305_Maze:
                        //三人连招 迷宫结束 设动作为完成
                        MazeOver();
                        TeamBrain.isOver_Maze_TeamState305Maze = true;
                        isCombatComplite_TeamState305_Maze = true;
                        break;
                    case WeavileTeamBrain.SubState.TeamState_206_Heal:
                        //二人连招 回血结束 迷宫玛狃拉 设动作为完成
                        if (teamCombatActIndex == 1) {
                            MazeOver();
                            isCombatComplite_TeamState206_Heal = true;
                        }
                        break;
                    case WeavileTeamBrain.SubState.TeamState_207_Maze:
                        //二人连招 迷宫结束 设动作为完成
                        MazeOver();
                        TeamBrain.isOver_Maze_TeamState207Maze = true;
                        isCombatComplite_TeamState207_Maze = true;
                        break;
                }
                break;
            case MainState.Heal:
                switch (TeamBrain.NowSubState)
                {
                    //三人小队 回复连招1
                    case WeavileTeamBrain.SubState.TeamState_307_Heal1:
                        //回复结束 动作设为完成
                        if (TeamBrain.isOver_Heal_TeamState307Heal1)
                        {
                            isCombatComplite_TeamState307_Heal = true;
                            HealOver();
                        }
                        //回复未结束 继续跟随回复目标移动
                        else
                        {
                            NowHealSubState = HealSubState.Move;
                        }
                        break;
                    //三人小队 回复连招2
                    case WeavileTeamBrain.SubState.TeamState_308_Heal2:
                        //回复结束 动作设为完成
                        if (TeamBrain.isOver_Heal_TeamState308Heal2)
                        {
                            isCombatComplite_TeamState308_Heal2 = true;
                            HealOver();
                        }
                        //回复未结束 继续跟随回复目标移动
                        else
                        {
                            NowHealSubState = HealSubState.Move;
                        }
                        break;
                    //二人连招 回血结束 回血玛狃拉 设动作为完成
                    case WeavileTeamBrain.SubState.TeamState_206_Heal:
                        //回复结束 动作设为完成
                        if (TeamBrain.isOver_Heal_TeamState206Heal)
                        {
                            isCombatComplite_TeamState206_Heal = true;
                            HealOver();
                        }
                        //回复未结束 继续跟随回复目标移动
                        else
                        {
                            NowHealSubState = HealSubState.Move;
                        }
                        break;
                }
                break;
        }
    }



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
    /// 冲刺落地尘埃特效
    /// </summary>
    public GameObject RushDust;

    /// <summary>
    /// 奔跑尘埃特效
    /// </summary>
    public GameObject RunDust;


    /// <summary>
    /// 冲刺实例
    /// </summary>
    WeavileRush RushObj;
    /// <summary>
    /// 恶系冲刺预制件
    /// </summary>
    public WeavileDarkRush WeavileDarkRushPrefab;
    /// <summary>
    /// 毒系冲刺预制件
    /// </summary>
    public WeavilePosionRush WeavilePosionRushPrefab;
    /// <summary>
    /// 冰系冲刺预制件
    /// </summary>
    public WeavileIceRush WeavileIceRushPrefab;


    /// <summary>
    /// 恶波预制件
    /// </summary>
    public WeavileDarkPulse WeavileDarkPulsePrefabs;
    /// <summary>
    /// 小恶波预制件
    /// </summary>
    public WeavileDarkPulse WeavileDarkPulseSmallPrefabs;
    /// <summary>
    /// 微小恶波预制件
    /// </summary>
    public WeavileDarkPulse WeavileDarkPulseTinyPrefabs;

    /// <summary>
    /// 毒雾预制件
    /// </summary>
    public WeavilrPosionRushPosionMist WeavilePosionMistPrefabs;

    /// <summary>
    /// 冰粒预制件
    /// </summary>
    public WeavileIceShard WeavilrIceShardPrefabs;

    /// <summary>
    /// 支援母件实例
    /// </summary>
    WeavileSupportParent SupportParentObj;
    /// <summary>
    /// 帮助母体预制件
    /// </summary>
    public WeavileHelpingParent WeavileHelpingParentPrefab;
    /// <summary>
    /// 保护母体预制件
    /// </summary>
    public WeavileUseProtectParent WeavileUseProtectParentPrefab;
    /// <summary>
    /// 回复母体预制件
    /// </summary>
    public WeavileHealParent WeavileHealParentPrefab;
    /// <summary>
    /// 回复被打断时的反噬爆炸
    /// </summary>
    public WeavileHealExplosion WeavileHealExplosionPrefab;
    /// <summary>
    /// 迷宫母体预制件
    /// </summary>
    public WeavileMazeParent WeavileMazeParentPrefab;

    /// <summary>
    /// 投掷炸弹预制件
    /// </summary>
    public WeavileFilingBomb WeavileFilingBombPrefabs;
    /// <summary>
    /// 投掷毒雾炸弹预制件
    /// </summary>
    public WeavileFilingPoisionBomb WeavileFilingPoisionBombPrefabs;
    /// <summary>
    /// 投掷烟雾炸弹预制件
    /// </summary>
    public WeavileFilingSmokeBomb WeavileFilingSmokeBombPrefabs;
    /// <summary>
    /// 投掷伤药预制件
    /// </summary>
    public WeavileFilingHeal WeavileFilingHealPrefabs;
    /// <summary>
    /// 投掷替身预制件
    /// </summary>
    public WeavileFilingSubstitute WeavileFilingSubstitutePrefabs;
    /// <summary>
    /// 投掷万灵药预制件
    /// </summary>
    public WeavileFilingFullHeal WeavileFilingFullHealPrefabs;

    //■■■■■■■■■■■■■■■■■■■■预制件■■■■■■■■■■■■■■■■■■■■■■





























    //■■■■■■■■■■■■■■■■■■■■状态机部分■■■■■■■■■■■■■■■■■■■■■■




    //=========================通用参数============================

    /// <summary>
    /// 一人小队 默认的烟雾弹烟雾半径大小
    /// </summary>
    public static float RADIUS_SMOKE_TEAMSTATE100_NORMAL = 3.2f;




    /// <summary>
    /// 三人小队 迷宫连招 是否完成首次投掷
    /// </summary>
    bool isOver_TeamState305Maze_FirstFling = false;

    /// <summary>
    /// 二人小队 迷宫连招 是否完成首次投掷
    /// </summary>
    bool isOver_TeamState207Maze_FirstFling = false;

    //=========================通用参数============================




    //=========================发呆============================


    /// <summary>
    /// 开始后的冷却时间
    /// </summary>
    static public float TIME_IDLE_START = 0.5f;

    /// <summary>
    ///  三人小队近战连招 连招结束后的休息时间 冲刺玛狃拉1的休息时间
    /// </summary>
    static float TIME_IDLE_TEAMSTATE302_CLOSEATK_OVER_01RUSH = 3.0f;
    /// <summary>
    /// 三人小队近战连招 连招结束后的休息时间 跳跃玛狃拉2的休息时间
    /// </summary>
    static float TIME_IDLE_TEAMSTATE302_CLOSEATK_OVER_02JUMP = 1.2f;
    /// <summary>
    /// 三人小队近战连招 连招结束后的休息时间 使用保护玛狃拉3的休息时间
    /// </summary>
    static float TIME_IDLE_TEAMSTATE302_CLOSEATK_OVER_03USEPROTECT = 5.5f;



    /// <summary>
    /// 三人小队替身连招 连招结束后的休息时间 玛狃拉01
    /// </summary>
    static float TIME_IDLE_TEAMSTATE303_SUBSTITUTE_OVER_01RUSH = 4.0f;
    /// <summary>
    /// 三人小队替身连招 连招结束后的休息时间 玛狃拉02
    /// </summary>
    static float TIME_IDLE_TEAMSTATE303_SUBSTITUTE_OVER_02RUSH = 6.0f;
    /// <summary>
    /// 三人小队替身连招 连招结束后的休息时间 玛狃拉03
    /// </summary>
    static float TIME_IDLE_TEAMSTATE303_SUBSTITUTE_OVER_03RUSH = 10.0f;


    /// <summary>
    /// 三人小队分身连招 终结冲刺 连招结束后的休息时间
    /// </summary>
    static float TIME_IDLE_TEAMSTATE304_CLONESHADOW_OVERRUSH = 6.0f;
    /// <summary>
    /// 三人小队分身连招 意外冲刺 连招结束后的休息时间 玛狃拉01
    /// </summary>
    static float TIME_IDLE_TEAMSTATE304_CLONESHADOW_ADDCIENTRUSH_01RUSH = 4.0f;
    /// <summary>
    /// 三人小队分身连招 意外冲刺 连招结束后的休息时间 玛狃拉02
    /// </summary>
    static float TIME_IDLE_TEAMSTATE304_CLONESHADOW_ADDCIENTRUSH_02RUSH = 6.0f;
    /// <summary>
    /// 三人小队分身连招 意外冲刺 连招结束后的休息时间 玛狃拉03
    /// </summary>
    static float TIME_IDLE_TEAMSTATE304_CLONESHADOW_ADDCIENTRUSH_03RUSH = 10.0f;


    /// <summary>
    /// 三人小队 迷宫连招 连招结束后的休息时间 玛狃拉01
    /// </summary>
    static float TIME_IDLE_TEAMSTATE305_MAZE_01MAZE = 3.0f;
    /// <summary>
    /// 三人小队 迷宫连招 连招结束后的休息时间 玛狃拉02
    /// </summary>
    static float TIME_IDLE_TEAMSTATE305_MAZE_0203MAZEWATCH = 9.0f;


    /// <summary>
    /// 三人小队绕圈连招 外环攻击 连招结束后的休息时间 玛狃拉01
    /// </summary>
    static float TIME_IDLE_TEAMSTATE306_CIRCLEOVER_OVER_OUTERATK_01RUSH = 2.0f;
    /// <summary>
    /// 三人小队绕圈连招 外环攻击 连招结束后的休息时间 玛狃拉02
    /// </summary>
    static float TIME_IDLE_TEAMSTATE306_CIRCLEOVER_OVER_OUTERATK_02RUSH = 5.0f;
    /// <summary>
    /// 三人小队绕圈连招 外环攻击 连招结束后的休息时间 玛狃拉03
    /// </summary>
    static float TIME_IDLE_TEAMSTATE306_CIRCLEOVER_OVER_OUTERATK_03RUSH = 10.0f;



    /// <summary>
    /// 三人小队回血连招1 连招结束后的休息时间 玛狃拉01（使用保护）
    /// </summary>
    static float TIME_IDLE_TEAMSTATE307_HEAL1_01USEPROTECT = 4.0f;
    /// <summary>
    /// 三人小队回血连招1 连招结束后的休息时间 玛狃拉02（回复）
    /// </summary>
    static float TIME_IDLE_TEAMSTATE307_HEAL1_02HEAL = 4.0f;
    /// <summary>
    /// 三人小队回血连招1 连招结束后的休息时间 玛狃拉03（冲刺）
    /// </summary>
    static float TIME_IDLE_TEAMSTATE307_HEAL1_03RUSH = 12.0f;
    /// <summary>
    /// 三人小队万灵药连招 连招结束后的休息时间
    /// </summary>
    static float TIME_IDLE_TEAMSTATE309_FULLHEAL = 0.25f;



    /// <summary>
    /// 三人小队绕圈连招 脱离攻击 连招结束后的休息时间
    /// </summary>
    static float TIME_IDLE_TEAMSTATE306_CIRCLEOVER_OVER_ESCAPEATK = 4.0f;





    /// <summary>
    ///  二人小队近战连招 连招结束后的休息时间 冰粒玛狃拉1的休息时间
    /// </summary>
    static float TIME_IDLE_TEAMSTATE202_CLOSEATK_OVER_01ICESHARD = 5.5f;
    /// <summary>
    /// 二人小队近战连招 连招结束后的休息时间 跳跃玛狃拉2的休息时间
    /// </summary>
    static float TIME_IDLE_TEAMSTATE202_CLOSEATK_OVER_02JUMP = 1.2f;


    /// <summary>
    /// 二人小队替身连招 连招结束后的休息时间 玛狃拉01
    /// </summary>
    static float TIME_IDLE_TEAMSTATE203_SUBSTITUTE_OVER_01RUSH = 3.0f;
    /// <summary>
    /// 二人小队替身连招 连招结束后的休息时间 玛狃拉02
    /// </summary>
    static float TIME_IDLE_TEAMSTATE203_SUBSTITUTE_OVER_02RUSH = 6.5f;

    /// <summary>
    /// 二人小队 替身连招2 连招结束后的休息时间 玛狃拉01
    /// </summary>
    static float TIME_IDLE_TEAMSTATE204_SUBSTITUTE2_OVER_01RUSH = 3.0f;
    /// <summary>
    /// 二人小队 替身连招2 连招结束后的休息时间 玛狃拉02
    /// </summary>
    static float TIME_IDLE_TEAMSTATE204_SUBSTITUTE2_OVER_02RUSH = 6.5f;



    /// <summary>
    /// 二人小队 回血连招 连招结束后的休息时间 玛狃拉01（迷宫）
    /// </summary>
    static float TIME_IDLE_TEAMSTATE206_HEAL1_01MAZE = 4.0f;
    /// <summary>
    /// 二人小队 回血连招 连招结束后的休息时间 玛狃拉02（回复）
    /// </summary>
    static float TIME_IDLE_TEAMSTATE206_HEAL1_02HEAL = 4.0f;


    /// <summary>
    /// 二人小队 迷宫连招 连招结束后的休息时间 玛狃拉01释放迷宫
    /// </summary>
    static float TIME_IDLE_TEAMSTATE207_MAZE_01MAZE = 3.0f;
    /// <summary>
    /// 二人小队 迷宫连招 连招结束后的休息时间 玛狃拉02守卫迷宫
    /// </summary>
    static float TIME_IDLE_TEAMSTATE207_MAZE_02MAZEWATCH = 11.0f;


    /// <summary>
    /// 一人小队 冲刺结束后的休息时间
    /// </summary>
    static float TIME_IDLE_TEAMSTATE100_NORMAL = 2.5f;



    /// <summary>
    /// 发呆计时器
    /// <summary>
    float IdleTimer = 0;

    /// <summary>
    /// 发呆开始
    /// <summary>
    public void IdleStart(float Timer)
    {
        //Debug.Log(name+"Idle");
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
    /// 三人小队巡逻状态时 两侧玛狃拉的角度偏转值最小值
    /// </summary>
    static public float ANGLE_MIN_TEAMSTATE300_RUN_TOWSIDE_DEFLECTION = 45.0f;

    /// <summary>
    /// 三人小队巡逻状态时 两侧玛狃拉的角度偏转值最大值
    /// </summary>
    static public float ANGLE_MAX_TEAMSTATE300_RUN_TOWSIDE_DEFLECTION = 85.0f;

    /// <summary>
    /// 两侧玛狃拉处于最大角度时的最远距离
    /// </summary>
    static public float DISTENCE_WHEN_ANGLE_TWOSIDE_DEFLECTION_MIN = 4.8f;

    /// <summary>
    /// 两侧玛狃拉处于最小角度时的最近距离
    /// </summary>
    static public float DISTENCE_WHEN_ANGLE_TWOSIDE_DEFLECTION_MAX = 8.0f;

    /// <summary>
    /// 三人小队巡逻状态时 两侧玛狃拉的速度加成最小值
    /// </summary>
    static public float SPEEDALPHA_TEAMSTATE300_RUN_TOWSIDE_MIN = 0.85f;

    /// <summary>
    /// 三人小队巡逻状态时 两侧玛狃拉的速度加成最大值
    /// </summary>
    static public float SPEEDALPHA_TEAMSTATE300_RUN_TOWSIDE_MAX = 1.2f;

    /// <summary>
    /// 三人小队巡逻状态时 中间玛狃拉的速度加成
    /// </summary>
    static float SPEEDALPHA_TEAMSTATE300_RUN_MIDDLE = 0.55f;

    /// <summary>
    /// 三人小队巡逻状态时 中间玛狃拉停止奔跑距离（接近到此距离后不再接近）
    /// </summary>
    static float DISTENCE_TEAMSTATE300_RUN_MIDDLE_RUNSTOP = 3.5f;




    /// <summary>
    /// 二人小队 巡逻状态 玛狃拉们绕圈的基准距离
    /// </summary>
    static float DISTENCE_TEAMSTATE201_RUN_BASE_RADIUS = 5.0f;
    /// <summary>
    /// 二人小队 巡逻状态 玛狃拉们绕圈时距离基准距离的允许差值
    /// </summary>
    static float DISTENCE_TEAMSTATE201_RUN_ALLOWSUB_BASE_RADIUS = 0.5f;
    /// <summary>
    /// 二人小队 巡逻状态 玛狃拉的低速 最小值
    /// </summary>
    static float SPEEDALPHA_TEAMSTATE201_RUN_LOW_MIN = 0.3f;
    /// <summary>
    /// 二人小队 巡逻状态 玛狃拉的低速 最大值
    /// </summary>
    static float SPEEDALPHA_TEAMSTATE201_RUN_LOW_MAX = 0.6f;
    /// <summary>
    /// 二人小队 巡逻状态 玛狃拉的中速
    /// </summary>
    static float SPEEDALPHA_TEAMSTATE201_RUN_NORMAL = 1.0f;
    /// <summary>
    /// 二人小队 巡逻状态 玛狃拉的高速 最小值
    /// </summary>
    static float SPEEDALPHA_TEAMSTATE201_RUN_HIGH_MIN = 1.35f;
    /// <summary>
    /// 二人小队 巡逻状态 玛狃拉的高速 最大值
    /// </summary>
    static float SPEEDALPHA_TEAMSTATE201_RUN_HIGH_MAX = 2.4f;

    /// <summary>
    /// 一人小队 移动的速度
    /// </summary>
    static float SPEEDALPHA_TEAMSTATE100NORMAL_RUN = 2.1f;







    /// <summary>
    /// 奔跑计时器
    /// <summary>
    float RunTimer = 0;
    /// <summary>
    /// 奔跑方向
    /// </summary>
    Vector2 Dir_Run = Vector2.zero;
    /// <summary>
    /// 奔跑目标点
    /// </summary>
    Vector2 Position_Target_Run = Vector2.zero;






    /// <summary>
    /// 奔跑开始
    /// <summary>
    public void RunStart(/*float Timer*/)
    {
        //成员数变更 直接结束
        if (TeamBrain != null)
        {
            if (TeamBrain.MembersCheck()) { return; }
        }

        RunTimer = 0;
        NowMainState = MainState.Run;
        Dir_Run = Vector2.zero;
        Position_Target_Run = Vector2.zero;

        //一人小队 在烟雾中移动时 设置目标位置
        if ( TeamBrain.NowSubState == WeavileTeamBrain.SubState.TeamState_100_Normal && TeamBrain.NowTeamState100SubState == WeavileTeamBrain.TEAMSTATE100_SUBSTATE.RunInSmoke) { Position_Target_Run = GetRandomPointInSmoke(TeamBrain.WeavileSmokeList , 1.0f); }
    }

    /// <summary>
    /// 奔跑结束
    /// <summary>
    public void RunOver()
    {
        RunTimer = 0;
        Dir_Run = Vector2.zero;
        Position_Target_Run = Vector2.zero;
        animator.SetFloat("Speed" , 0.0f);
    }


    //根据距离目标距离设置偏转角度
    float SetTwoSideOffsetAngleByDistence(float Distence)
    {
        float d = Mathf.Clamp(Distence, DISTENCE_WHEN_ANGLE_TWOSIDE_DEFLECTION_MIN, DISTENCE_WHEN_ANGLE_TWOSIDE_DEFLECTION_MAX);
        float t = (d - DISTENCE_WHEN_ANGLE_TWOSIDE_DEFLECTION_MIN) / (DISTENCE_WHEN_ANGLE_TWOSIDE_DEFLECTION_MAX - DISTENCE_WHEN_ANGLE_TWOSIDE_DEFLECTION_MIN);
        return Mathf.Lerp(ANGLE_MAX_TEAMSTATE300_RUN_TOWSIDE_DEFLECTION, ANGLE_MIN_TEAMSTATE300_RUN_TOWSIDE_DEFLECTION, t);
    }

    //三人小队 根据距离目标距离设置速度加成
    float SetTwoSideSpeedAlphaByDistence_TeamState301Run(float Distence)
    {
        float d = Mathf.Clamp(Distence, DISTENCE_WHEN_ANGLE_TWOSIDE_DEFLECTION_MIN, DISTENCE_WHEN_ANGLE_TWOSIDE_DEFLECTION_MAX);
        float t = (d - DISTENCE_WHEN_ANGLE_TWOSIDE_DEFLECTION_MIN) / (DISTENCE_WHEN_ANGLE_TWOSIDE_DEFLECTION_MAX - DISTENCE_WHEN_ANGLE_TWOSIDE_DEFLECTION_MIN);
        return Mathf.Lerp(SPEEDALPHA_TEAMSTATE300_RUN_TOWSIDE_MAX, SPEEDALPHA_TEAMSTATE300_RUN_TOWSIDE_MIN, t);
    }

    //二人小队 根据另一只玛狃拉设置速度
    float SetTwoSpeedAlphaByAnotherWeavile_TeamState301Run(Transform Another , Vector2 target)
    {
        float TAngle = _mTool.Angle_360Y(((Vector2)transform.position - target).normalized, Vector2.right);
        float AAngle = _mTool.Angle_360Y(((Vector2)Another.position - target).normalized, Vector2.right);
        if (AAngle < TAngle) { AAngle += 360.0f; }
        float s = AAngle - TAngle;
        //Debug.Log(name+TAngle +" "+ AAngle + " " + s);
        if (s >= 0.0f && s < 170.0f)
        {
            //夹角极小时静止不动
            if (s >= 0.0f && s < 90.0f) { return 0; }
            float t = Mathf.Clamp(s, 0.0f, 130.0f) / 130.0f;
            return Mathf.Lerp(SPEEDALPHA_TEAMSTATE201_RUN_LOW_MAX, SPEEDALPHA_TEAMSTATE201_RUN_LOW_MIN, t);
        }
        else if (s >= 190.0f && s < 360.0f)
        {
            float t = Mathf.Clamp((s - 190), 0.0f, 130.0f) / 130.0f;
            return Mathf.Lerp(SPEEDALPHA_TEAMSTATE201_RUN_HIGH_MAX, SPEEDALPHA_TEAMSTATE201_RUN_HIGH_MIN, t);
        }
        return SPEEDALPHA_TEAMSTATE201_RUN_NORMAL;
        //300 320  |300 10   | 20  10 | 180 359 
        //20 340   |70  290  | 350 10 | 179 181
    }



    /// <summary>
    /// 获取当前位置 距离烟雾范围的最接近的点 
    /// 烟雾范围为以smokeList的中心位置为圆心 ， Radiu为半径 ， 且在房间上下左右边界内
    /// 如果当前位置处于烟雾范围内 输出当前位置
    /// </summary>
    /// <returns></returns>
    Vector2 GetClosestPointInSmoke(List<WeavileFlingSmoke> smokeList , float Radiu )
    {
        Vector2 pos = transform.position;

        float Up = ParentPokemonRoom.RoomSize[0] + ParentPokemonRoom.transform.position.y;
        float Down = ParentPokemonRoom.RoomSize[1] + ParentPokemonRoom.transform.position.y;
        float Left = ParentPokemonRoom.RoomSize[2] + ParentPokemonRoom.transform.position.x;
        float Right = ParentPokemonRoom.RoomSize[3] + ParentPokemonRoom.transform.position.x;

        Vector2 bestPoint = pos;
        float bestDist = float.MaxValue;

        foreach (var smoke in smokeList)
        {
            Vector2 center = smoke.transform.position;

            float dist = Vector2.Distance(pos, center);

            // 1. 如果当前位置在这个烟雾圆内 → 直接返回当前位置
            if (dist <= Radiu)
                return pos;

            // 2. 求圆周上最接近当前位置的点
            Vector2 dir = (pos - center).normalized;
            Vector2 closest = center + dir * Radiu;

            // 3. 限制在房间边界内
            closest.x = Mathf.Clamp(closest.x, Left, Right);
            closest.y = Mathf.Clamp(closest.y, Down, Up);

            // 4. 计算这个点与当前位置的距离
            float d = Vector2.Distance(pos, closest);

            // 5. 记录最小值
            if (d < bestDist)
            {
                bestDist = d;
                bestPoint = closest;
            }
        }

        return bestPoint;
    }

    /// <summary>
    /// 获取烟雾范围的随机的点 
    /// 烟雾范围为以smokeList的中心位置为圆心 ， Radiu为半径 ， 且在房间上下左右边界内
    /// </summary>
    /// <returns></returns>
    Vector2 GetRandomPointInSmoke(List<WeavileFlingSmoke> smokeList, float Radiu)
    {
        float Up = ParentPokemonRoom.RoomSize[0] + ParentPokemonRoom.transform.position.y;
        float Down = ParentPokemonRoom.RoomSize[1] + ParentPokemonRoom.transform.position.y;
        float Left = ParentPokemonRoom.RoomSize[2] + ParentPokemonRoom.transform.position.x;
        float Right = ParentPokemonRoom.RoomSize[3] + ParentPokemonRoom.transform.position.x;

        // 1. 随机选择一个烟雾圆
        var smoke = smokeList[Random.Range(0, smokeList.Count)];
        Vector2 center = smoke.transform.position;

        // 2. 在圆内随机取点（均匀分布）
        float angle = Random.Range(0f, Mathf.PI * 2f);
        float r = Mathf.Sqrt(Random.Range(0f, 1f)) * Radiu; // sqrt 保证均匀分布

        Vector2 point = center + new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * r;

        // 3. 限制在房间边界内
        point.x = Mathf.Clamp(point.x, Left, Right);
        point.y = Mathf.Clamp(point.y, Down, Up);

        return point;
    }




    //=========================奔跑============================

















    //=========================跳跃移动============================



    /// <summary>
    /// 三人小队近战连招跳跃速度加成
    /// </summary>
    public static float SPEEDALPHA_JUMP_TEMASTATE302_CLOSEATK = 6.0f;
    /// <summary>
    /// 三人小队近战连招跳跃 跳跃至玩家周边的距离
    /// </summary>
    static float DISTENCE_TARGET_JUMP_TEMASTATE302_CLOSEATK = 3.5f;
    /// <summary>
    /// 三人小队近战连招跳跃 跳跃的准备时间
    /// </summary>
    static float TIME_PREPARE_JUMP_TEMASTATE302_CLOSEATK = 0.7f;


    /// <summary>
    /// 三人小队替身连招跳跃 跳跃至玛狃拉1周边的距离
    /// </summary>
    public static float DISTENCE_TARGET_JUMP_TEMASTATE303_SUBSTITUTE = 3.0f;


    /// <summary>
    /// 三人小队分身连招 跳跃速度加成
    /// </summary>
    static public float SPEEDALPHA_JUMP_TEMASTATE304_LONESHADOW = 2.0f;
    /// <summary>
    /// 三人小队分身连招 跳跃 首次跳跃的固定时间
    /// </summary>
    static public float TIME_JUMP_TEMASTATE304_CLONESHADOW_FIRSTJUMP = 0.9f;
    /// <summary>
    /// 三人小队分身连招 跳跃 第二次跳跃的固定时间
    /// </summary>
    static public float TIME_JUMP_TEMASTATE304_CLONESHADOW_SECONDJUMP = 0.5f;


    /// <summary>
    /// 三人小队迷宫连招 第一次跳跃的固定时间
    /// </summary>
    static public float TIME_JUMP_TEMASTATE305_MAZE_JUMP = 0.2f;


    /// <summary>
    /// 三人小队绕圈连招 脱离攻击 跳跃的距离
    /// </summary>
    static float DISTENCE_TARGET_JUMP_TEMASTATE306_CIRCLERUN_ESCAPEATK = 5.0f;
    /// <summary>
    /// 三人小队绕圈连招 脱离攻击 跳跃的准备时间=0
    /// </summary>
    static float TIME_PREPARE_JUMP_TEMASTATE306_CIRCLERUN_ESCAPEATK = 0.0f;


    /// <summary>
    /// 三人小队回血连招1 第一次跳跃的固定时间
    /// </summary>
    static public float TIME_JUMP_TEMASTATE307_HEAL1_JUMP = 0.5f;


    /// <summary>
    /// 三人小队回血连招2 第一次跳跃的固定时间
    /// </summary>
    static public float TIME_JUMP_TEMASTATE308_HEAL2_JUMP = 0.5f;



    /// <summary>
    /// 二人小队回血连招 第一次跳跃的固定时间
    /// </summary>
    static public float TIME_JUMP_TEMASTATE206_HEAL_JUMP = 0.2f;


    /// <summary>
    /// 二人小队迷宫连招 第一次跳跃的固定时间
    /// </summary>
    static public float TIME_JUMP_TEMASTATE207_MAZE_JUMP = 0.2f;

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
    /// 跳跃是否在准备期间
    /// </summary>
    bool isPrepare_Jump = false;




    /// <summary>
    /// 跳跃移动开始
    /// <summary>
    public void JumpMoveStart(/*float Timer*/)
    {
        //成员数变更 直接结束
        if (TeamBrain != null)
        {
            if (TeamBrain.MembersCheck()) { return; }
        }

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
        animator.SetInteger("Jump",0);
    }


    //=========================跳跃移动============================
















    //=========================冲刺============================




    /// <summary>
    /// 三人小队近战连招冲刺速度加成
    /// </summary>
    static public float SPEEDALPHA_RUSH_TEMASTATE302_CLOSEATK = 8.0f;

    /// <summary>
    /// 三人小队近战连招冲刺蓄力时间
    /// </summary>
    static float TIME_CHARGE_RUSH_TEMASTATE302_CLOSEATK = 0.3f;

    /// <summary>
    /// 三人小队近战连招冲刺移动时间
    /// </summary>
    static float TIME_RUSHMOVE_RUSH_TEMASTATE302_CLOSEATK = 0.8f;

    /// <summary>
    /// 三人小队近战连招 发射小恶波的时间间隔
    /// </summary>
    static float TIME_INTERVAL_RUSH_TEMASTATE302_CLOSEATK_SMALLDARKPULSE = 0.08f;
    /// <summary>
    /// 三人小队近战连招 发射小恶波的时间间隔
    /// </summary>
    static float TIME_INTERVAL_RUSH_TEMASTATE302_CLOSEATK_POSIONMIST = 0.08f;
    /// <summary>
    /// 三人小队近战连招 冰系冲刺落点散射的冰粒数
    /// </summary>
    static int COUNT_ICESHARD_ICSRUSHOVER_ICBREAK = 24;


    /// <summary>
    /// 三人小队分身连招 终结冲刺蓄力时间
    /// </summary>
    static public float TIME_CHARGE_RUSH_TEMASTATE304_CLONESHADOW_OVERRUSH = 0.3f;
    /// <summary>
    /// 三人小队分身连招 意外冲刺蓄力时间
    /// </summary>
    static public float TIME_CHARGE_RUSH_TEMASTATE304_CLONESHADOW_ACCIDENTRUSH = 0.6f;
    /// <summary>
    /// 三人小队分身连招 冲刺移动时间
    /// </summary>
    static public float TIME_RUSHMOVE_RUSH_TEMASTATE304_CLONESHADOW = 0.8f;
    /// <summary>
    /// 三人小队分身连招 发射小恶波的时间间隔
    /// </summary>
    static public float TIME_INTERVAL_RUSH_TEMASTATE304_CLONESHADOW_SMALLDARKPULSE = 0.08f;

    /// <summary>
    /// 三人小队 迷宫连招 冲刺蓄力时间
    /// </summary>
    static float TIME_CHARGE_RUSH_TEMASTATE305_MAZE = 0.6f;
    /// <summary>
    /// 三人小队 迷宫连招 冲刺移动时间
    /// </summary>
    static float TIME_RUSHMOVE_RUSH_TEMASTATE305_MAZE = 0.8f;

    /// <summary>
    /// 三人小队绕圈连招 内环攻击 冲刺速度加成
    /// </summary>
    static float SPEEDALPHA_RUSH_TEMASTATE306_CIRCLERUN_INNERATK = 8.0f;
    /// <summary>
    /// 三人小队绕圈连招 内环攻击1 冲刺蓄力时间
    /// </summary>
    static float TIME_CHARGE_RUSH_TEMASTATE306_CIRCLERUN_INNERATK1 = 1.3f;
    /// <summary>
    /// 三人小队绕圈连招 内环攻击1 发射小恶波的时间间隔
    /// </summary>
    static float TIME_INTERVAL_RUSH_TEMASTATE306_CIRCLERUN_INNERATK1_SMALLDARKPULSE = 0.08f;

    /// <summary>
    /// 三人小队绕圈连招 外环攻击 冲刺速度加成
    /// </summary>
    static float SPEEDALPHA_RUSH_TEMASTATE306_CIRCLERUN_OUTERATK = 6.5f;
    /// <summary>
    /// 三人小队绕圈连招 内环攻击2 冲刺蓄力时间
    /// </summary>
    static float TIME_CHARGE_RUSH_TEMASTATE306_CIRCLERUN_INNERATK2 = 1.3f;
    /// <summary>
    /// 三人小队绕圈连招 内环攻击2 发射小恶波的时间间隔
    /// </summary>
    static float TIME_INTERVAL_RUSH_TEMASTATE306_CIRCLERUN_INNERATK2_SMALLDARKPULSE = 0.08f;
    /// <summary>
    /// 三人小队绕圈连招 内环攻击2 冲刺蓄移动时间
    /// </summary>
    static float TIME_RUSHMOVE_RUSH_TEMASTATE306_CIRCLERUN_INNERATK2 = 0.25f;

    /// <summary>
    /// 三人小队绕圈连招 外环攻击 冲刺蓄力时间
    /// </summary>
    static float TIME_CHARGE_RUSH_TEMASTATE306_CIRCLERUN_OUTERATK = 1.3f;
    /// <summary>
    /// 三人小队绕圈连招 外环攻击 发射小恶波的时间间隔
    /// </summary>
    static float TIME_INTERVAL_RUSH_TEMASTATE306_CIRCLERUN_OUTER_SMALLDARKPULSE = 0.08f;
    /// <summary>
    /// 三人小队绕圈连招 外环攻击 冲刺蓄移动时间
    /// </summary>
    static float TIME_RUSHMOVE_RUSH_TEMASTATE306_CIRCLERUN_OUTERATK = 0.8f;

    /// <summary>
    /// 三人小队绕圈连招 脱离攻击 冲刺蓄力时间
    /// </summary>
    static float TIME_CHARGE_RUSH_TEMASTATE306_CIRCLERUN_ESCAPEATK = 0.1f;
    /// <summary>
    /// 三人小队绕圈连招 脱离攻击 冲刺蓄移动时间
    /// </summary>
    static float TIME_RUSHMOVE_RUSH_TEMASTATE306_CIRCLERUN_ESCAPEATK = 0.2f;
    /// <summary>
    /// 三人小队绕圈连招 脱离攻击 冰系冲刺落点散射的冰粒数
    /// </summary>
    static int COUNT_ICESHARD_ICSRUSHOVER_TEAMSTATE306CIRCLERUN_ESCAPEATK_ICBREAK = 10;

    /// <summary>
    /// 三人小队回血连招1 冲刺蓄力时间
    /// </summary>
    static float TIME_CHARGE_RUSH_TEMASTATE307_HEAL1 = 1.0f;
    /// <summary>
    /// 三人小队回血连招1 冲刺移动时间
    /// </summary>
    static float TIME_RUSHMOVE_RUSH_TEMASTATE307_HEAL1 = 0.8f;

    /// <summary>
    /// 三人小队万灵药连招 冲刺蓄力时间
    /// </summary>
    static float TIME_CHARGE_RUSH_TEMASTATE309_FULLHEAL = 0.4f;

    /// <summary>
    /// 一人小队 冲刺蓄力时间
    /// </summary>
    static float TIME_CHARGE_RUSH_TEMASTATE100_NORMAL = 0.1f;


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
    /// 三人连招 绕圈跑内环攻击冲刺的指定时间
    /// </summary>
    float Time_Rush_TeamState306CircleRun_InnerAtk = 0.0f;
    /// <summary>
    /// 二人连招 绕圈跑内环攻击冲刺的指定时间
    /// </summary>
    float Time_Rush_TeamState205CircleRun_InnerAtk = 0.0f;




    /// <summary>
    /// 冲刺开始
    /// <summary>
    public void RushStart(/*float Timer*/)
    {
        //成员数变更 直接结束
        if (TeamBrain != null)
        {
            if (TeamBrain.MembersCheck()) { return; }
        }

        RushTimer = 0;
        NowMainState = MainState.Rush;
        isCharge_Rush = false;
        isMove_Rush = false;
        isPredict_RushAngle = false;
        Dir_Rush = Vector2.zero;
        Angle_PredictLastAngle_Rush = 0;
        Timer_Rush_MoveEffect_Interval = 0.0f;
        Time_Rush_TeamState306CircleRun_InnerAtk = 0.0f;
        Time_Rush_TeamState205CircleRun_InnerAtk = 0.0f;
        switch (TeamBrain.NowSubState)
        {
            //三人小队近战连招
            case WeavileTeamBrain.SubState.TeamState_302_CloseAtk:
                //仅第一次角度为与目标角度主方向
                if (TeamBrain.Count_Teamstate302_Close_Atk == 0) { Dir_Rush = _mTool.MainVector2((TargetPosition - (Vector2)transform.position).normalized);SetDirector(Dir_Rush); }
                // 随机冲刺种类
                NowRushType = RandomGetRushType();
                //NowRushType = RushType.Posion;
                SetRushArrow(NowRushType , ParentPokemonRoom.GetRoomBoundaryByRaycastTo(transform.position, Dir_Rush));
                break;
            //三人小队替身连招
            case WeavileTeamBrain.SubState.TeamState_303_Substitute:
                // 冲刺种类恶
                NowRushType = RushType.Dark;
                SetRushArrow(NowRushType, ParentPokemonRoom.GetRoomBoundaryByRaycastTo(transform.position, Dir_Rush));
                break;
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
                        SetRushArrow(NowRushType, ParentPokemonRoom.GetRoomBoundaryByRaycastTo(transform.position, Dir_Rush));
                        break;
                    //意外冲刺
                    case WeavileTeamBrain.TEAMSTATE304_CLONESHADOW.AccidentRush:
                        // 冲刺种类恶
                        NowRushType = RushType.Dark;
                        SetRushArrow(NowRushType, ParentPokemonRoom.GetRoomBoundaryByRaycastTo(transform.position, Dir_Rush));
                        break;
                    //其他状态转入报错休息
                    default:
                        {
                            RushOver();
                            IdleStart(TIME_IDLE_START);
                            Debug.Log("Error:绕圈连招攻击错误");
                        }
                        return;
                }
                break;
            //三人小队迷宫连招
            case WeavileTeamBrain.SubState.TeamState_305_Maze:
                {
                    //冲刺角度为朝向玩家主方向
                    switch (teamCombatActIndex)
                    {
                        case 2: //玛狃拉02 纵向移动 横向冲刺
                            Dir_Rush = (TargetPosition.x - transform.position.x > 0) ? Vector2.right : Vector2.left;
                            SetDirector(Dir_Rush);
                            break;
                        case 3: //玛狃拉03 横向移动 纵向冲刺
                            Dir_Rush = (TargetPosition.y - transform.position.y > 0) ? Vector2.up : Vector2.down;
                            SetDirector(Dir_Rush);
                            break;
                        default:
                            Debug.Log("Error:三人小队 迷宫连招 连招错误 其他玛狃拉因转入看护迷宫而转入冲刺");
                            break;
                    }
                    // 75%恶冲刺25%毒冲刺
                    float r = Random.Range(0.0f, 1.0f);
                    if (r >= 0.0f && r < 0.75f) { NowRushType = RushType.Dark; }
                    else { NowRushType = RushType.Posion; }
                    SetRushArrow(NowRushType, ParentPokemonRoom.GetRoomBoundaryByRaycastTo(transform.position, Dir_Rush));
                }
                break;
            //三人小队绕圈连招
            case WeavileTeamBrain.SubState.TeamState_306_CircleRun:
                switch (TeamBrain.NowTeamState306SubState)
                {
                    //内环攻击1
                    case WeavileTeamBrain.TEAMSTATE306_SUBSTATE.InnerAtk1:
                        Dir_Rush = -((Vector2)transform.position - TeamBrain.Position_Center_TeamState_306_CircleRun).normalized;
                        NowRushType = RushType.Dark;
                        Vector2 t1 = TeamBrain.Position_Center_TeamState_306_CircleRun - ((Vector2)transform.position - TeamBrain.Position_Center_TeamState_306_CircleRun);
                        Time_Rush_TeamState306CircleRun_InnerAtk = Vector2.Distance(t1, (Vector2)transform.position) / (speed * SPEEDALPHA_RUSH_TEMASTATE302_CLOSEATK);
                        SetRushArrow(NowRushType, t1);
                        SetDirector(_mTool.MainVector2(Dir_Rush));
                        break;
                    //内环攻击2
                    case WeavileTeamBrain.TEAMSTATE306_SUBSTATE.InnerAtk2:
                        Dir_Rush = -((Vector2)TeamBrain.WeavileMembersList[0].transform.position - TeamBrain.Position_Center_TeamState_306_CircleRun).normalized;
                        if (TeamBrain.WeavileMembersList[0].gameObject.GetInstanceID() != this.gameObject.GetInstanceID()) { Dir_Rush = -Dir_Rush; }
                        NowRushType = RushType.Dark;
                        Vector2 t2 = Dir_Rush * TIME_RUSHMOVE_RUSH_TEMASTATE306_CIRCLERUN_INNERATK2 * speed * SPEEDALPHA_RUSH_TEMASTATE302_CLOSEATK + (Vector2)transform.position;
                        SetRushArrow(NowRushType, t2);
                        SetDirector(_mTool.MainVector2(Dir_Rush));
                        break;
                    //外环攻击
                    case WeavileTeamBrain.TEAMSTATE306_SUBSTATE.OuterAtk:
                        Dir_Rush = (TargetPosition - (Vector2)transform.position).normalized;
                        switch (TeamCombatActIndex)
                        {
                            case 1: NowRushType = RushType.Dark; break;
                            case 2: NowRushType = RushType.Ice; break;
                            case 3: NowRushType = RushType.Posion; break;
                        }
                        Vector2 t3 = Dir_Rush * TIME_RUSHMOVE_RUSH_TEMASTATE306_CIRCLERUN_OUTERATK * speed * SPEEDALPHA_RUSH_TEMASTATE302_CLOSEATK + (Vector2)transform.position;
                        SetRushArrow(NowRushType, t3);
                        SetDirector(_mTool.MainVector2(Dir_Rush));
                        break;
                    //脱离攻击
                    case WeavileTeamBrain.TEAMSTATE306_SUBSTATE.EscapeAtk:
                        Dir_Rush = (TargetPosition - (Vector2)transform.position).normalized;
                        NowRushType = RushType.Ice;
                        Vector2 t4 = Dir_Rush * TIME_RUSHMOVE_RUSH_TEMASTATE306_CIRCLERUN_ESCAPEATK * speed * SPEEDALPHA_RUSH_TEMASTATE302_CLOSEATK + (Vector2)transform.position;
                        SetRushArrow(NowRushType, t4);
                        SetDirector(_mTool.MainVector2(Dir_Rush));
                        break;
                    //其他状态转入报错休息
                    default:
                        RushOver();
                        IdleStart(TIME_IDLE_START);
                        Debug.Log("Error:绕圈连招攻击错误");
                        return;
                }
                break;
            //三人小队回血连招1
            case WeavileTeamBrain.SubState.TeamState_307_Heal1:
                // 随机冲刺种类
                //NowRushType = RandomGetRushType();
                NowRushType = RushType.Posion;
                SetRushArrow(NowRushType, ParentPokemonRoom.GetRoomBoundaryByRaycastTo(transform.position, Dir_Rush));
                break;
            //三人小队万灵药连招
            case WeavileTeamBrain.SubState.TeamState_309_FullHeal:
                {
                    Dir_Rush = (TargetPosition - (Vector2)transform.position).normalized;
                    switch (TeamCombatActIndex)
                    {
                        case 1: NowRushType = RushType.Dark; break;
                        case 2: NowRushType = RushType.Ice; break;
                        case 3: NowRushType = RushType.Posion; break;
                    }
                    Vector2 t3 = Dir_Rush * TIME_CHARGE_RUSH_TEMASTATE302_CLOSEATK * speed * SPEEDALPHA_RUSH_TEMASTATE302_CLOSEATK + (Vector2)transform.position;
                    SetRushArrow(NowRushType, t3);
                    SetDirector(_mTool.MainVector2(Dir_Rush));
                }
                break;
            //二人小队近战连招
            case WeavileTeamBrain.SubState.TeamState_202_CloseAtk:
                //仅第一次角度为与目标角度主方向
                if (TeamBrain.Count_Teamstate202_Close_Atk == 0) { Dir_Rush = _mTool.MainVector2((TargetPosition - (Vector2)transform.position).normalized); SetDirector(Dir_Rush); }
                // 随机冲刺种类
                NowRushType = RandomGetRushType();
                //NowRushType = RushType.Posion;
                SetRushArrow(NowRushType, ParentPokemonRoom.GetRoomBoundaryByRaycastTo(transform.position, Dir_Rush));
                break;
            //二人小队替身连招
            case WeavileTeamBrain.SubState.TeamState_203_Substitute:
                // 冲刺种类恶
                NowRushType = RushType.Dark;
                SetRushArrow(NowRushType, ParentPokemonRoom.GetRoomBoundaryByRaycastTo(transform.position, Dir_Rush));
                break;
            //二人小队替身连招2
            case WeavileTeamBrain.SubState.TeamState_204_Substitute2:
                // 冲刺种类恶
                NowRushType = RushType.Dark;
                SetRushArrow(NowRushType, ParentPokemonRoom.GetRoomBoundaryByRaycastTo(transform.position, Dir_Rush));
                break;
            //二人小队绕圈连招
            case WeavileTeamBrain.SubState.TeamState_205_CircleRun:
                switch (TeamBrain.NowTeamState205SubState)
                {
                    //内环攻击1
                    case WeavileTeamBrain.TEAMSTATE306_SUBSTATE.InnerAtk1:
                        Dir_Rush = -((Vector2)transform.position - TeamBrain.Position_Center_TeamState_205_CircleRun).normalized;
                        NowRushType = RushType.Dark;
                        Vector2 t1 = TeamBrain.Position_Center_TeamState_205_CircleRun - ((Vector2)transform.position - TeamBrain.Position_Center_TeamState_205_CircleRun);
                        Time_Rush_TeamState205CircleRun_InnerAtk = Vector2.Distance(t1, (Vector2)transform.position) / (speed * SPEEDALPHA_RUSH_TEMASTATE302_CLOSEATK);
                        SetRushArrow(NowRushType, t1);
                        SetDirector(_mTool.MainVector2(Dir_Rush));
                        break;
                    //内环攻击2
                    case WeavileTeamBrain.TEAMSTATE306_SUBSTATE.InnerAtk2:
                        Dir_Rush = -((Vector2)transform.position - TeamBrain.Position_Center_TeamState_205_CircleRun).normalized;
                        float rOffset = Random.Range(0.0f, 35.0f);
                        Dir_Rush = Quaternion.AngleAxis(rOffset, Vector3.forward) * Dir_Rush;
                        NowRushType = RushType.Dark;
                        Vector2 t2 = Dir_Rush * TIME_RUSHMOVE_RUSH_TEMASTATE306_CIRCLERUN_INNERATK2 * speed * SPEEDALPHA_RUSH_TEMASTATE302_CLOSEATK + (Vector2)transform.position;
                        SetRushArrow(NowRushType, t2);
                        SetDirector(_mTool.MainVector2(Dir_Rush));
                        break;
                    //外环攻击
                    case WeavileTeamBrain.TEAMSTATE306_SUBSTATE.OuterAtk:
                        Dir_Rush = (TargetPosition - (Vector2)transform.position).normalized;
                        NowRushType = RandomGetRushType();
                        Vector2 t3 = Dir_Rush * TIME_RUSHMOVE_RUSH_TEMASTATE306_CIRCLERUN_OUTERATK * speed * SPEEDALPHA_RUSH_TEMASTATE302_CLOSEATK + (Vector2)transform.position;
                        SetRushArrow(NowRushType, t3);
                        SetDirector(_mTool.MainVector2(Dir_Rush));
                        break;
                    //脱离攻击
                    case WeavileTeamBrain.TEAMSTATE306_SUBSTATE.EscapeAtk:
                        Dir_Rush = (TargetPosition - (Vector2)transform.position).normalized;
                        NowRushType = RushType.Ice;
                        Vector2 t4 = Dir_Rush * TIME_RUSHMOVE_RUSH_TEMASTATE306_CIRCLERUN_ESCAPEATK * speed * SPEEDALPHA_RUSH_TEMASTATE302_CLOSEATK + (Vector2)transform.position;
                        SetRushArrow(NowRushType, t4);
                        SetDirector(_mTool.MainVector2(Dir_Rush));
                        break;
                    //其他状态转入报错休息
                    default:
                        RushOver();
                        IdleStart(TIME_IDLE_START);
                        Debug.Log("Error:绕圈连招攻击错误");
                        return;
                }
                break;
            //二人小队 迷宫连招
            case WeavileTeamBrain.SubState.TeamState_207_Maze:
                {
                    //角度为与目标角度主方向
                    Dir_Rush = _mTool.MainVector2((TargetPosition - (Vector2)transform.position).normalized); SetDirector(Dir_Rush); 
                    // 75%恶冲刺25%毒冲刺
                    float r = Random.Range(0.0f, 1.0f);
                    if (r >= 0.0f && r < 0.75f) { NowRushType = RushType.Dark; }
                    else { NowRushType = RushType.Posion; }
                    SetRushArrow(NowRushType, ParentPokemonRoom.GetRoomBoundaryByRaycastTo(transform.position, Dir_Rush));
                }
                break;
            //二人小队万灵药连招
            case WeavileTeamBrain.SubState.TeamState_208_FullHeal:
                {
                    Dir_Rush = (TargetPosition - (Vector2)transform.position).normalized;
                    switch (TeamCombatActIndex)
                    {
                        case 1: NowRushType = RushType.Dark; break;
                        case 2: NowRushType = RushType.Ice; break;
                        case 3: NowRushType = RushType.Posion; break;
                    }
                    Vector2 t3 = Dir_Rush * TIME_CHARGE_RUSH_TEMASTATE302_CLOSEATK * speed * SPEEDALPHA_RUSH_TEMASTATE302_CLOSEATK + (Vector2)transform.position;
                    SetRushArrow(NowRushType, t3);
                    SetDirector(_mTool.MainVector2(Dir_Rush));
                }
                break;
            //一人小队 
            case WeavileTeamBrain.SubState.TeamState_100_Normal:
                switch (TeamBrain.NowTeamState100SubState)
                {
                    case WeavileTeamBrain.TEAMSTATE100_SUBSTATE.Rush:
                        // 冲刺种类随机
                        NowRushType = RandomGetRushType();
                        SetRushArrow(NowRushType, ParentPokemonRoom.GetRoomBoundaryByRaycastTo(transform.position, Dir_Rush));
                        break;
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
        Time_Rush_TeamState306CircleRun_InnerAtk = 0.0f;
        Time_Rush_TeamState205CircleRun_InnerAtk = 0.0f;
    }


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
    /// 设置冲刺指示箭头
    /// </summary>
    void SetRushArrow(RushType t , Vector2 targetPosition)
    {
        if (RushArrowObj != null)
        {
            RushArrowObj.ArrowOver();
            RushArrowObj = null;
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
    void LaunchRushEffect(RushType t , Vector2 d)
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
        RushObj = Instantiate(WeavileIceRushPrefab, transform.position, Quaternion.identity, transform);
        RushObj.RushStart(d);
        RushObj.ParentWeavile = this;
    }

    /// <summary>
    /// 生成恶系冲刺
    /// </summary>
    void LaunchDarkRush(Vector2 d)
    {
        RushObj = Instantiate(WeavileDarkRushPrefab, transform.position, Quaternion.identity, transform);
        RushObj.RushStart(d);
        RushObj.ParentWeavile = this;
    }

    /// <summary>
    /// 生成毒系冲刺
    /// </summary>
    void LaunchPosionRush(Vector2 d)
    {
        RushObj = Instantiate(WeavilePosionRushPrefab, transform.position, Quaternion.identity, transform);
        RushObj.RushStart(d);
        RushObj.ParentWeavile = this;
    }

    /// <summary>
    /// 生成毒雾
    /// </summary>
    void LaunchPosionMist(Vector2 d)
    {
        WeavilrPosionRushPosionMist pm = Instantiate(WeavilePosionMistPrefabs, transform.position, Quaternion.Euler(0,0,_mTool.Angle_360Y(d,Vector2.right)));
        pm.ParentWeavile = this;
    }

    /// <summary>
    /// 生成冰系冲刺落点冰粒散射
    /// </summary>
    void LaunchRushOverIceShard()
    {
        audioPlayer.Play(WeavileSE.IcePunch, transform.position);
        int count = COUNT_ICESHARD_ICSRUSHOVER_ICBREAK;
        //三人小队绕圈连招 脱离攻击 冰系冲刺冰粒数量不一样
        if (TeamBrain.NowSubState == WeavileTeamBrain.SubState.TeamState_306_CircleRun && TeamBrain.NowTeamState306SubState == WeavileTeamBrain.TEAMSTATE306_SUBSTATE.EscapeAtk)
        {
            count = COUNT_ICESHARD_ICSRUSHOVER_TEAMSTATE306CIRCLERUN_ESCAPEATK_ICBREAK;
        }
        float angle = 360.0f / (float)count;
        for (int i = 0; i < count; i++)
        {
            Vector2 LaunchDir = Quaternion.AngleAxis(angle * i, Vector3.forward) * Vector2.right;
            LaunchIceShard(LaunchDir);
        }
    }

    //=========================冲刺============================









    //=========================原地防守============================


    /// <summary>
    /// 三人小队发呆状态 接近防守目标时的速度加成
    /// </summary>
    static float SPEEDALPHA_GUARD_TEAMSTATE300IDLE = 2.3f;

    /// <summary>
    /// 三人小队发呆状态 接近到一定距离后不再接近的距离
    /// </summary>
    static float DISTENCE_GUARD_BETWEEN_TARGET_TEAMSTATE300IDLE = 2.0f;

    /// <summary>
    /// 三人小队发呆状态 触发冰粒的距离
    /// </summary>
    static float DISTENCE_GUARD_LAUNCH_ICESHARD_TEAMSTATE300IDLE = 9.0f;

    /// <summary>
    /// 三人小队发呆状态 触发恶波的距离
    /// </summary>
    static float DISTENCE_GUARD_LAUNCH_DARKPULSE_TEAMSTATE300IDLE = 4.0f;

    /// <summary>
    /// 一人小队 巡逻状态 触发投掷的时间
    /// </summary>
    static float TIME_GUARD_FLINGBOMB_TEAMSTATE100NORMAL = 2.0f;

    /// <summary>
    /// 一人小队 巡逻状态 触发冲刺的时间
    /// </summary>
    static float DISTENCE_GUARD_RUSH_TEAMSTATE100NORMAL = 8.0f;






    /// <summary>
    /// 原地防守计时器
    /// <summary>
    float GuardTimer = 0;
    /// <summary>
    /// 防守时移动方向
    /// </summary>
    Vector2 Dir_Guard_Move = Vector2.zero;
    /// <summary>
    /// 是否处于防守的移动状态
    /// </summary>
    bool isMove_Guard;


    /// <summary>
    /// 原地防守开始
    /// <summary>
    public void GuardStart(/*float Timer*/)
    {
        //成员数变更 直接结束
        if (TeamBrain != null)
        {
            if (TeamBrain.MembersCheck()) { return; }
        }

        GuardTimer = 0;
        Dir_Guard_Move = Vector2.zero;
        isMove_Guard = false;

        NowMainState = MainState.Guard;
    }

    /// <summary>
    /// 原地防守结束
    /// <summary>
    public void GuardOver()
    {
        GuardTimer = 0;
        Dir_Guard_Move = Vector2.zero;
        isMove_Guard = false;
    }


    //=========================原地防守============================






    //=========================使用保护============================


    /// <summary>
    /// 三人小队近战连招 保护时间
    /// </summary>
    static float TIME_USEPROTECT_TEMASTATE302_CLOSEATK = 2.0f;
    /// <summary>
    /// 三人小队回血连招1 保护时间
    /// </summary>
    static float TIME_USEPROTECT_TEMASTATE307_HEAL1 = 4.0f;
    /// <summary>
    /// 三人小队回血连招1 保护之间的间隔时间
    /// </summary>
    static float TIME_USEPROTECT_TEMASTATE307_HEAL1_INTERVAL = 2.0f;


    /// <summary>
    /// 二人小队近战连招 保护时间
    /// </summary>
    static float TIME_USEPROTECT_TEMASTATE202_CLOSEATK = 2.0f;


    /// <summary>
    /// 帮助的种类
    /// </summary>
    enum UseProtectType
    {
        Helping, //帮助
        Protect, //保护
    };
    UseProtectType NowUseProtectType;





    /// <summary>
    /// 使用保护计时器
    /// <summary>
    float UseProtectTimer = 0;
    /// <summary>
    /// 是否处于保护的间隔期间
    /// </summary>
    bool isInterval_UseProtect = false;



    /// <summary>
    /// 使用保护开始
    /// <summary>
    public void UseProtectStart(float Timer)
    {
        //成员数变更 直接结束
        if (TeamBrain != null) {
            if (TeamBrain.MembersCheck()) { return; }
        }

        UseProtectTimer = Timer;
        isInterval_UseProtect = false;
        NowMainState = MainState.UseProtect;
        animator.SetInteger("Maze", 1);
        SetDirector(Vector2.down);
        switch (TeamBrain.NowSubState)
        {
            //三人小队近战连招
            case WeavileTeamBrain.SubState.TeamState_302_CloseAtk:
                {
                    //随机帮助种类
                    NowUseProtectType = RandomGetUseProtectType();
                    //NowUseProtectType = UseProtectType.Protect;
                    //支援
                    List<Weavile> wl = new List<Weavile> { };
                    wl.Add(TeamBrain.WeavileMembersList[0]);
                    wl.Add(TeamBrain.WeavileMembersList[1]);
                    wl.Add(TeamBrain.WeavileMembersList[2]);
                    wl.Remove(this);
                    switch (NowUseProtectType)
                    {
                        case UseProtectType.Helping:
                            LaunchHelping(wl, TIME_USEPROTECT_TEMASTATE302_CLOSEATK);
                            break;
                        case UseProtectType.Protect:
                            LaunchProtect(wl, TIME_USEPROTECT_TEMASTATE302_CLOSEATK);
                            break;
                    }
                }
                break;
            //三人小队替身连招
            case WeavileTeamBrain.SubState.TeamState_303_Substitute:
                {
                    //保护状态时 玛狃拉02使用保护
                    //毒弹状态时 玛狃拉01有概率使用保护
                    if ((TeamBrain.NowTeamState303SubState == WeavileTeamBrain.TEAMSTATE303_SUBSTATE.Protect && teamCombatActIndex == 2) ||
                        (TeamBrain.NowTeamState303SubState == WeavileTeamBrain.TEAMSTATE303_SUBSTATE.Toxic && teamCombatActIndex == 1))
                    {
                        //帮助种类为保护
                        NowUseProtectType = UseProtectType.Protect;
                        //支援对象为替身
                        if (TeamBrain.TeamState303_Substitute_SubstituteOBJ != null) { LaunchProtect(new List<WeavileSubstituteOBJ> { TeamBrain.TeamState303_Substitute_SubstituteOBJ }, TIME_USEPROTECT_TEMASTATE302_CLOSEATK); }
                        else { LaunchProtect(TeamBrain.WeavileMembersList, TIME_USEPROTECT_TEMASTATE302_CLOSEATK); }
                    }
                }
                break;
            //三人小队回血连招1
            case WeavileTeamBrain.SubState.TeamState_307_Heal1:
                {
                    //帮助种类为保护
                    NowUseProtectType = UseProtectType.Protect;
                    //支援
                    List<Weavile> wl307 = new List<Weavile> { };
                    wl307.Add(TeamBrain.WeavileMembersList[0]);
                    wl307.Add(TeamBrain.WeavileMembersList[1]);
                    wl307.Add(TeamBrain.WeavileMembersList[2]);
                    //wl307.Remove(this);
                    LaunchProtect(wl307, TIME_USEPROTECT_TEMASTATE307_HEAL1);
                }
                break;
            //二人小队近战连招
            case WeavileTeamBrain.SubState.TeamState_202_CloseAtk:
                {
                    //随机帮助种类
                    NowUseProtectType = RandomGetUseProtectType();
                    //NowUseProtectType = UseProtectType.Protect;
                    //支援
                    List<Weavile> wl = new List<Weavile> { };
                    wl.Add(TeamBrain.WeavileMembersList[0]);
                    wl.Add(TeamBrain.WeavileMembersList[1]);
                    wl.Remove(this);
                    switch (NowUseProtectType)
                    {
                        case UseProtectType.Helping:
                            LaunchHelping(wl, TIME_USEPROTECT_TEMASTATE202_CLOSEATK);
                            break;
                        case UseProtectType.Protect:
                            LaunchProtect(wl, TIME_USEPROTECT_TEMASTATE202_CLOSEATK);
                            break;
                    }
                }
                break;
            //二人小队替身连招
            case WeavileTeamBrain.SubState.TeamState_203_Substitute:
                {
                    //保护状态时 玛狃拉02使用保护
                    if ((TeamBrain.NowTeamState203SubState == WeavileTeamBrain.TEAMSTATE203_SUBSTATE.Protect && teamCombatActIndex == 2))
                    {
                        //帮助种类为保护
                        NowUseProtectType = UseProtectType.Protect;
                        //支援对象为替身
                        if (TeamBrain.TeamState203_Substitute_SubstituteOBJ != null) { LaunchProtect(new List<WeavileSubstituteOBJ> { TeamBrain.TeamState203_Substitute_SubstituteOBJ }, TIME_USEPROTECT_TEMASTATE302_CLOSEATK); }
                        else { LaunchProtect(TeamBrain.WeavileMembersList, TIME_USEPROTECT_TEMASTATE302_CLOSEATK); }
                    }
                }
                break;
        }
        //设置颜色
        //开始帮助
        switch (NowUseProtectType)
        {
            case UseProtectType.Helping: SetOutLineAndShadowColor(COLOR_OUTLINEANDSHADOW_HELPINGRED); break;
            case UseProtectType.Protect: SetOutLineAndShadowColor(COLOR_OUTLINEANDSHADOW_PROTECTBLUE); break;
        }
    }

    /// <summary>
    /// 使用保护结束
    /// <summary>
    public void UseProtectOver()
    {
        UseProtectTimer = 0;
        isInterval_UseProtect = false;
        animator.SetInteger("Maze", 0);
    }


    /// <summary>
    /// 随机获取帮助种类
    /// </summary>
    UseProtectType RandomGetUseProtectType()
    {
        UseProtectType t = UseProtectType.Helping;
        int i = Random.Range(0, 2);
        switch (i)
        {
            case 0: t = UseProtectType.Helping; break;
            case 1: t = UseProtectType.Protect; break;
        }
        return t;
    }

    /// <summary>
    /// 发射帮助母件
    /// </summary>
    /// <param name="wl"></param>
    /// <param name="t"></param>
    void LaunchHelping(List<Weavile> wl , float t)
    {
        if (SupportParentObj != null)
        {
            SupportParentObj.SupportOver();
        }
        WeavileHelpingParent h = Instantiate(WeavileHelpingParentPrefab , transform.position , Quaternion.identity , transform);
        h.HelpingStart(wl, t);
        SupportParentObj = h;
    }


    /// <summary>
    /// 发射保护母件
    /// </summary>
    /// <param name="wl"></param>
    /// <param name="t"></param>
    void LaunchProtect(List<Weavile> wl, float t)
    {
        if (SupportParentObj != null)
        {
            SupportParentObj.SupportOver();
        }
        WeavileUseProtectParent h = Instantiate(WeavileUseProtectParentPrefab, transform.position, Quaternion.identity, transform);
        h.HelpingStart(wl, t);
        SupportParentObj = h;
    }

    /// <summary>
    /// 发射保护母件 保护替身
    /// </summary>
    /// <param name="wl"></param>
    /// <param name="t"></param>
    void LaunchProtect(List<WeavileSubstituteOBJ> wl, float t)
    {
        if (SupportParentObj != null)
        {
            SupportParentObj.SupportOver();
        }
        WeavileUseProtectParent h = Instantiate(WeavileUseProtectParentPrefab, transform.position, Quaternion.identity, transform);
        h.HelpingStart(wl, t);
        SupportParentObj = h;
    }


    //=========================使用保护============================






    //=========================发射冰粒============================


    /// <summary>
    /// 三人小队 发呆状态 防守时发射冰粒的时间 包含使用后冷却时间
    /// </summary>
    static float TIME_ICESHARD_TEMASTATE300IDLE_GUARD = 1.4f;
    /// <summary>
    /// 三人小队 发呆状态 防守时发射三发冰粒 左右两发冰粒的偏转角度
    /// </summary>
    static float ANGLE_ICESHARD_TEMASTATE300IDLE_GUARD_OFFSET = 20f;
    /// <summary>
    /// 三人小队 分身连招 跳跃完毕后发射三发冰粒 左右两发冰粒的偏转角度
    /// </summary>
    static public float ANGLE_ICESHARD_TEMASTATE303CLONESHADOW_JUMPOVER_OFFSET = 25f;

    /// <summary>
    /// 三人小队 分身状态 跳跃后时发射冰粒的时间 包含使用后冷却时间
    /// </summary>
    static public float TIME_ICESHARD_TEMASTATE304IDLE_JUMP = 0.5f;

    /// <summary>
    /// 三人小队 迷宫状态 冲刺后时发射冰粒的时间 包含使用后冷却时间
    /// </summary>
    static public float TIME_ICESHARD_TEMASTATE305MAZE_RUSH = 7.0f;



    /// <summary>
    /// 二人小队 巡逻状态 静止后发射冰粒的时间 包含使用后冷却时间
    /// </summary>
    static public float TIME_ICESHARD_TEMASTATE201RUN = 0.8f;
    /// <summary>
    /// 二人小队 巡逻状态 静止时发射三发冰粒 左右两发冰粒的偏转角度
    /// </summary>
    static float ANGLE_ICESHARD_TEMASTATE201_RUN_OFFSET = 20f;
    /// <summary>
    /// 二人小队 巡逻状态 静止后发射冰粒的时间 预判角度
    /// </summary>
    static public float ANGLE_ICESHARD_TEMASTATE201IDLE_RUN_OFFSET_PREDICT = 15f;



    /// <summary>
    /// 二人小队 近战连招 静止后发射冰粒的时间 包含使用后冷却时间
    /// </summary>
    static public float TIME_ICESHARD_TEMASTATE202CLOSEATK = 0.5f;



    /// <summary>
    /// 发射冰粒计时器
    /// <summary>
    float IceShardTimer = 0;



    /// <summary>
    /// 发射冰粒开始
    /// <summary>
    public void IceShardStart(float Timer)
    {
        //成员数变更 直接结束
        if (TeamBrain != null)
        {
            if (TeamBrain.MembersCheck()) { return; }
        }

        IceShardTimer = Timer;
        NowMainState = MainState.IceShard;
        animator.SetTrigger("Atk");
        switch (TeamBrain.NowSubState)
        {
            //三人小队 发呆连招 防守状态发射三连发冰粒
            case WeavileTeamBrain.SubState.TeamState_300_Idle:
                LaunchIceShard3_Guard();
                break;
            //三人小队 分身连招 跳跃完毕后发射3连发冰粒
            case WeavileTeamBrain.SubState.TeamState_304_CloneShadow:
                LaunchIceShard3_TeamState304CloneShadow(TeamBrain.isLeft_TeamState304CloneShadow_JumpStart);
                break;
            //三人小队 迷宫连招 冲刺完毕后发射3连发冰粒
            case WeavileTeamBrain.SubState.TeamState_305_Maze:
                LaunchIceShard3_Guard();
                break;
            //三人小队 回血连招2 防守状态发射三连发冰粒
            case WeavileTeamBrain.SubState.TeamState_308_Heal2:
                LaunchIceShard3_Guard();
                break;
            //二人小队 发呆连招 防守状态发射三连发冰粒
            case WeavileTeamBrain.SubState.TeamState_200_Idle:
                LaunchIceShard3_Guard();
                break;
            //二人小队 巡逻状态 静止后发射4连发冰粒
            case WeavileTeamBrain.SubState.TeamState_201_Run:
                LaunchIceShard4_201Run_Predict();
                break;
            //二人小队 近战连招 发射3连发冰粒
            case WeavileTeamBrain.SubState.TeamState_202_CloseAtk:
                LaunchIceShard3_Guard();
                break;
            //二人小队 迷宫连招 冲刺完毕后发射3连发冰粒
            case WeavileTeamBrain.SubState.TeamState_207_Maze:
                LaunchIceShard3_Guard();
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
        SetDirector(_mTool.MainVector2(d1.normalized));
        audioPlayer.Play(WeavileSE.IcePunch, transform.position);
    }

    /// <summary>
    /// 三人小队 分身连招 跳跃完毕后发射3连发冰粒
    /// </summary>
    public void LaunchIceShard3_TeamState304CloneShadow( bool isLeft )
    {

        Vector2 d1 = new Vector2(isLeft?1:-1,0.0f);
        Vector2 d2 = Quaternion.AngleAxis(ANGLE_ICESHARD_TEMASTATE303CLONESHADOW_JUMPOVER_OFFSET, Vector3.forward) * d1;
        Vector2 d3 = Quaternion.AngleAxis(-ANGLE_ICESHARD_TEMASTATE303CLONESHADOW_JUMPOVER_OFFSET, Vector3.forward) * d1;
        SetDirector(_mTool.MainVector2(d1));
        LaunchIceShard(d1);
        LaunchIceShard(d2);
        LaunchIceShard(d3);
        audioPlayer.Play(WeavileSE.IcePunch, transform.position);
    }

    /// <summary>
    /// 二人小队 巡逻时 发射4发预判冰粒
    /// </summary>
    public void LaunchIceShard4_201Run_Predict()
    {
        Vector2 d = (TargetPosition - (Vector2)transform.position).normalized;
        d = Quaternion.AngleAxis(ANGLE_ICESHARD_TEMASTATE201IDLE_RUN_OFFSET_PREDICT, Vector3.forward) * d;
        Vector2 d1 = Quaternion.AngleAxis(ANGLE_ICESHARD_TEMASTATE201_RUN_OFFSET * 0.5f , Vector3.forward) * d;
        Vector2 d2 = Quaternion.AngleAxis(-ANGLE_ICESHARD_TEMASTATE201_RUN_OFFSET * 0.5f, Vector3.forward) * d;
        Vector2 d3 = Quaternion.AngleAxis(ANGLE_ICESHARD_TEMASTATE201_RUN_OFFSET * 1.5f, Vector3.forward) * d;
        Vector2 d4 = Quaternion.AngleAxis(-ANGLE_ICESHARD_TEMASTATE201_RUN_OFFSET * 1.5f, Vector3.forward) * d;
        LaunchIceShard(d1);
        LaunchIceShard(d2);
        LaunchIceShard(d3);
        LaunchIceShard(d4);
        SetDirector(_mTool.MainVector2(d.normalized));
        audioPlayer.Play(WeavileSE.IcePunch, transform.position);
    }


    /// <summary>
    /// 发射冰粒
    /// </summary>
    /// <param name="d"></param>
    public void LaunchIceShard(Vector2 d)
    {
        d = d.normalized;
        WeavileIceShard isobj = Instantiate(WeavilrIceShardPrefabs , transform.position + (Vector3)d.normalized*0.5f , Quaternion.Euler(0,0,_mTool.Angle_360Y(d,Vector2.right)));
        isobj.empty = this;
        isobj.LaunchNotForce(d, isobj.MoveSpeed);
    }


    //=========================发射冰粒============================






    //=========================发射恶波============================




    /// <summary>
    /// 三人小队 发呆状态 防守时发射冰粒的时间
    /// </summary>
    static float TIME_DARKPULSE_TEMASTATE300IDLE_GUARD = 2.0f;



    /// <summary>
    /// 发射恶波计时器
    /// <summary>
    float DarkPulseTimer = 0;

    /// <summary>
    /// 发射恶波开始
    /// <summary>
    public void DarkPulseStart(float Timer)
    {
        //成员数变更 直接结束
        if (TeamBrain != null)
        {
            if (TeamBrain.MembersCheck()) { return; }
        }

        DarkPulseTimer = Timer;
        NowMainState = MainState.DarkPulse;
        animator.SetTrigger("Atk");
        LaunchDarkPulse();
    }

    /// <summary>
    /// 发射恶波结束
    /// <summary>
    public void DarkPulseOver()
    {
        DarkPulseTimer = 0;
    }


    /// <summary>
    /// 发射恶波
    /// </summary>
    void LaunchDarkPulse()
    {
        WeavileDarkPulse dp = Instantiate(WeavileDarkPulsePrefabs,transform.position , Quaternion.identity , transform);
        dp.ParentWeavile = this;
    }

    /// <summary>
    /// 发射小恶波
    /// </summary>
    public void LaunchDarkPulseSmall(Vector2 p)
    {
        WeavileDarkPulse dp = Instantiate(WeavileDarkPulseSmallPrefabs, p, Quaternion.identity);
        dp.ParentWeavile = this;
    }

    /// <summary>
    /// 发射微小恶波
    /// </summary>
    public void LaunchDarkPulseTiny(Vector2 p)
    {
        WeavileDarkPulse dp = Instantiate(WeavileDarkPulseTinyPrefabs, p, Quaternion.identity);
        dp.ParentWeavile = this;
    }




    //=========================发射恶波============================






    //=========================投掷============================
    /// <summary>
    /// 三人小队 迷宫连招 首次投掷时间 包含投掷后的冷却时间
    /// </summary>
    public static float TIME_FLING_TEAMSTATE305MAZE_FIRST = 1.0f;
    /// <summary>
    /// 三人小队 迷宫连招 第二次开始 投掷准备时间
    /// </summary>
    static float TIME_PREPARE_FLING_TEMASTATE305MAZE_SECOND = 0.4f;
    /// <summary>
    /// 三人小队 迷宫连招 第二次开始投掷时间 包含投掷后的冷却时间
    /// </summary>
    public static float TIME_FLING_TEAMSTATE305MAZE_SECOND = 7.0f;
    /// <summary>
    /// 三人小队 万灵药连招 投掷时间
    /// </summary>
    static float TIME_FLING_TEMASTATE309_FULLHEAL = 1.0f;

    /// <summary>
    /// 三人小队 绕圈连招 脱离攻击 投掷时间
    /// </summary>
    public static float TIME_FLING_TEAMSTATE306CIRCLERUN_ESCAPEATK = 1.0f;
    /// <summary>
    /// 三人小队 绕圈连招 脱离攻击 投掷准备时间
    /// </summary>
    public static float TIME_PREPARE_FLING_TEMASTATE306_CIRCLERUN_ESCAPEATK = 1.0f;
    /// <summary>
    /// 三人小队 绕圈连招 脱离攻击 炸弹投掷的距离目标的半径
    /// </summary>
    static float RADIUS_FLING_TEMASTATE306_CIRCLERUN_ESCAPEATK_BOMB = 4.5f;
    /// <summary>
    /// 三人小队 绕圈连招 脱离攻击 毒炸弹投掷的距离目标的半径
    /// </summary>
    static float RADIUS_FLING_TEMASTATE306_CIRCLERUN_ESCAPEATK_POISONBOMB = 6.5f;

    /// <summary>
    /// 二人小队 回血连招 首次投掷烟雾弹 投掷准备时间
    /// </summary>
    public static float TIME_PREPARE_FLING_TEMASTATE206_HEAL = 0.2f;


    /// <summary>
    /// 一人小队 烟雾弹 投掷准备时间
    /// </summary>
    public static float TIME_PREPARE_FLING_TEMASTATE100_NORMAL_SMOKE = 1.0f;


    /// <summary>
    /// 一人小队 投掷烟雾弹的时间 包含冷却时间
    /// </summary>
    public static float TIME_FLING_TEAMSTATE100NORMAL_SMOKE = 1.0f;
    /// <summary>
    /// 一人小队 投掷炸弹的时间 包含冷却时间
    /// </summary>
    public static float TIME_FLING_TEAMSTATE100NORMAL_BOMB = 1.0f;




    /// <summary>
    /// 投掷种类
    /// </summary>
    public enum FlingType
    {
        Bomb,       //炸弹
        PoisonBomb, //毒雾弹
        SmokeBomb,  //烟雾弹
        Heal,       //伤药
        Substitute, //替身
        FullHeal,   //万灵药
    }
    FlingType NowFlingType;



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
        //成员数变更 直接结束
        if (TeamBrain != null)
        {
            if (TeamBrain.MembersCheck()) { return; }
        }

        FlingTimer = 0;
        Position_Fling_Target = Vector2.zero;
        isPrepare_Fling = true;
       

        //设置准备时间
        switch (TeamBrain.NowSubState)
        {
            //三人小队 替身连招
            case WeavileTeamBrain.SubState.TeamState_303_Substitute:
                //无准备时间
                FlingTimer = 0.0f;
                break;
            //三人小队 迷宫连招
            case WeavileTeamBrain.SubState.TeamState_305_Maze:
                if (!isOver_TeamState305Maze_FirstFling) //首次投掷
                {
                    //无准备时间
                    FlingTimer = 0.0f;
                }
                else //第二次投掷 冲刺后投掷 
                {
                    //微小准备时间
                    FlingTimer = TIME_PREPARE_FLING_TEMASTATE305MAZE_SECOND;
                }
                break;
            //三人小队 绕圈连招 逃脱攻击
            case WeavileTeamBrain.SubState.TeamState_306_CircleRun:
                if (TeamBrain.NowTeamState306SubState == WeavileTeamBrain.TEAMSTATE306_SUBSTATE.EscapeAtk)
                {
                    //第一次投掷时有准备时间 之后没有准备时间
                    if (TeamBrain.Count_Fling_TeamState306CircleRun_EscapeAtk == 0)
                    {
                        FlingTimer = TIME_PREPARE_FLING_TEMASTATE306_CIRCLERUN_ESCAPEATK;
                    }
                    else { FlingTimer = 0.0f; }
                }
                break;
            //三人小队 万灵药连招
            case WeavileTeamBrain.SubState.TeamState_309_FullHeal:
                //无准备时间
                FlingTimer = 0.0f;
                break;
            //二人小队 替身连招
            case WeavileTeamBrain.SubState.TeamState_203_Substitute:
                //无准备时间
                FlingTimer = 0.0f;
                break;
            //二人小队 替身连招2
            case WeavileTeamBrain.SubState.TeamState_204_Substitute2:
                //无准备时间
                FlingTimer = 0.0f;
                break;
            //二人小队 绕圈连招 逃脱攻击
            case WeavileTeamBrain.SubState.TeamState_205_CircleRun:
                if (TeamBrain.NowTeamState205SubState == WeavileTeamBrain.TEAMSTATE306_SUBSTATE.EscapeAtk)
                {
                    //第一次投掷时有准备时间 之后没有准备时间
                    if (TeamBrain.Count_Fling_TeamState205CircleRun_EscapeAtk == 0)
                    {
                        FlingTimer = TIME_PREPARE_FLING_TEMASTATE306_CIRCLERUN_ESCAPEATK;
                    }
                    else { FlingTimer = 0.0f; }
                }
                break;
            //二人小队 替身连招2
            case WeavileTeamBrain.SubState.TeamState_206_Heal:
                //微小准备时间
                FlingTimer = TIME_PREPARE_FLING_TEMASTATE206_HEAL;
                break;
            //二人小队 迷宫连招
            case WeavileTeamBrain.SubState.TeamState_207_Maze:
                if (!isOver_TeamState207Maze_FirstFling) //首次投掷
                {
                    //无准备时间
                    FlingTimer = 0.0f;
                }
                else //第二次投掷 冲刺后投掷 
                {
                    //微小准备时间
                    FlingTimer = TIME_PREPARE_FLING_TEMASTATE305MAZE_SECOND;
                }
                break;
            //二人小队 替身连招2
            case WeavileTeamBrain.SubState.TeamState_100_Normal:
                //微小准备时间
                FlingTimer = TIME_PREPARE_FLING_TEMASTATE100_NORMAL_SMOKE;
                break;
            //二人小队 万灵药连招
            case WeavileTeamBrain.SubState.TeamState_208_FullHeal:
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
    void LaunchFlingProjectiles(FlingType type , Vector2 target)
    {
        WeavileProjectiles p = null;
        //生成投掷物
        switch (type)
        {
            //炸弹
            case FlingType.Bomb:
                p = Instantiate(WeavileFilingBombPrefabs , transform.position , Quaternion.identity);
                break;
            //毒雾炸弹
            case FlingType.PoisonBomb:
                p = Instantiate(WeavileFilingPoisionBombPrefabs, transform.position, Quaternion.identity);
                break;
            //烟雾炸弹
            case FlingType.SmokeBomb:
                p = Instantiate(WeavileFilingSmokeBombPrefabs, transform.position, Quaternion.identity);
                p.GetComponent<WeavileFilingSmokeBomb>().Target = target;
                break;
            //伤药
            case FlingType.Heal:
                p = Instantiate(WeavileFilingHealPrefabs, transform.position, Quaternion.identity);
                break;
            //替身
            case FlingType.Substitute:
                p = Instantiate(WeavileFilingSubstitutePrefabs, transform.position, Quaternion.identity);
                break;
            //万灵药
            case FlingType.FullHeal:
                p = Instantiate(WeavileFilingFullHealPrefabs, transform.position, Quaternion.identity);
                break;
        }

        //发射
        Vector2 dir = (target - (Vector2)transform.position).normalized;
        float dis = Vector2.Distance(target, (Vector2)transform.position);
        p.SetWeavileProjectiles(this , dir , dis);


    }


    List<Vector2> LaunchFlingSmoke6()
    {
        float Up =    ParentPokemonRoom.RoomSize[0] + ParentPokemonRoom.transform.position.y;
        float Down =  ParentPokemonRoom.RoomSize[1] + ParentPokemonRoom.transform.position.y;
        float Left =  ParentPokemonRoom.RoomSize[2] + ParentPokemonRoom.transform.position.x + 2.0f;
        float Right = ParentPokemonRoom.RoomSize[3] + ParentPokemonRoom.transform.position.x - 2.0f;

        // 计算宽度和高度
        float width = Right - Left;
        float height = Up - Down;



        // 每格的宽度和高度
        float cellW = width / 2f;   // 横向 3 格
        float cellH = height / 2f;  // 纵向 2 格

        List<Vector2> centers = new List<Vector2>();

        // 2 行 × 3 列
        for (int row = 0; row < 2; row++)
        {
            for (int col = 0; col < 2; col++)
            {
                float cx = Left + cellW * col + cellW / 2f;
                float cy = Down + cellH * row + cellH / 2f;

                centers.Add(new Vector2(cx, cy));
                
            }
        }
        _mTool.DebugLogList<Vector2>(centers);
        return centers;

    }


    //=========================投掷============================











    //=========================释放迷宫============================


    /// <summary>
    /// 三人小队迷宫连招 使用迷宫时间
    /// </summary>
    static float TIME_MAZE_TEMASTATE305_MAZE = 70.0f;

    /// <summary>
    /// 二人小队迷宫连招 使用迷宫时间
    /// </summary>
    static float TIME_MAZE_TEMASTATE206_HEAL = 40.0f;

    /// <summary>
    /// 二人小队迷宫连招 使用迷宫时间
    /// </summary>
    static float TIME_MAZE_TEMASTATE207_MAZE = 70.0f;



    /// <summary>
    /// 释放迷宫计时器
    /// <summary>
    float MazeTimer = 0;

    /// <summary>
    /// 释放迷宫开始
    /// <summary>
    public void MazeStart(float Timer)
    {
        //成员数变更 直接结束
        if (TeamBrain != null)
        {
            if (TeamBrain.MembersCheck()) { return; }
        }

        MazeTimer = Timer;
        NowMainState = MainState.Maze;
        animator.SetInteger("Maze", 1);
        SetDirector(Vector2.down);
        switch (TeamBrain.NowSubState)
        {
            //三人小队迷宫连招
            case WeavileTeamBrain.SubState.TeamState_305_Maze:
                //释放迷宫
                LaunchMaze(TIME_MAZE_TEMASTATE305_MAZE);
                //设置颜色
                SetOutLineAndShadowColor(COLOR_OUTLINEANDSHADOW_MAZEPINK);
                break;
            //二人小队回血连招
            case WeavileTeamBrain.SubState.TeamState_206_Heal:
                //释放迷宫
                LaunchMaze(TIME_MAZE_TEMASTATE206_HEAL);
                //设置颜色
                SetOutLineAndShadowColor(COLOR_OUTLINEANDSHADOW_MAZEPINK);
                break;
            //二人小队迷宫连招
            case WeavileTeamBrain.SubState.TeamState_207_Maze:
                //释放迷宫
                LaunchMaze(TIME_MAZE_TEMASTATE207_MAZE);
                //设置颜色
                SetOutLineAndShadowColor(COLOR_OUTLINEANDSHADOW_MAZEPINK);
                break;
        }
    }

    /// <summary>
    /// 释放迷宫结束
    /// <summary>
    public void MazeOver()
    {
        MazeTimer = 0;
        animator.SetInteger("Maze", 0);

        OverMazeSupport();
    }

    /// <summary>
    /// 释放迷宫
    /// </summary>
    public void LaunchMaze(float t)
    {
        if (SupportParentObj != null)
        {
            SupportParentObj.SupportOver();
        }
        WeavileMazeParent h = Instantiate(WeavileMazeParentPrefab, transform.position, Quaternion.identity, transform);
        h.ParentWeavile = this;
        h.HelpingStart(t);
        SupportParentObj = h;
    }

    /// <summary>
    /// 结束迷宫支援
    /// </summary>
    void OverMazeSupport()
    {
        if (SupportParentObj != null)
        {
            SupportParentObj.SupportOver();
        }
    }


    //=========================释放迷宫============================






    //=========================看护迷宫============================



    /// <summary>
    /// 三人小队迷宫连招 玛狃拉的速度加成
    /// </summary>
    static float SPEEDALPHA_TEAMSTATE305_MAZE = 0.35f;

    /// <summary>
    /// 三人小队迷宫连招 玛狃拉看护迷宫转入冲刺时 ，距离目标的分方向距离
    /// </summary>
    static float DISTENCE_TEAMSTATE305_MAZE_MAZEWATCH2RUSH = 1.6f;





    /// <summary>
    /// 迷宫看护时的移动方向
    /// </summary>
    Vector2 Dir_MazeWatcher = Vector2.zero;



    /// <summary>
    /// 看护迷宫开始
    /// <summary>
    public void MazeWatcherStart(/*float Timer*/)
    {
        //成员数变更 直接结束
        if (TeamBrain != null)
        {
            if (TeamBrain.MembersCheck()) { return; }
        }

        Dir_MazeWatcher = Vector2.zero;

        NowMainState = MainState.MazeWatcher;
    }

    /// <summary>
    /// 看护迷宫结束
    /// <summary>
    public void MazeWatcherOver()
    {
        Dir_MazeWatcher = Vector2.zero;
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
    public float SPEEDALPHA_CircleRun
    {
        get { return SpeedAlpha_CircleRun; }
        set { SpeedAlpha_CircleRun = value; }
    }
    float SpeedAlpha_CircleRun = 0.0f;

    /// <summary>
    /// 绕圈跑的方向
    /// </summary>
    Vector2 Dir_CircleRun = Vector2.zero;



    /// <summary>
    /// 绕圈跑开始
    /// <summary>
    public void CircleRunStart()
    {
        //成员数变更 直接结束
        if (TeamBrain != null)
        {
            if (TeamBrain.MembersCheck()) { return; }
        }

        CircleRunTimer = 0;
        SpeedAlpha_CircleRun = 0.0f;
        Dir_CircleRun = Vector2.zero;
        //AddCloneBodyToBrainList();
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
        SpeedAlpha_CircleRun = 0.0f;
        Dir_CircleRun = Vector2.zero;
        if (ShadowCoroutine != null)
        {
            StopShadowCoroutine();
        }
    }





    //=========================绕圈跑============================











    //=========================分身============================


    /// <summary>
    /// 三人小队分身连招 分身生成的时间
    /// </summary>
    static float TIME_CLONESHADOW_TEAMSTATE_304_CLONESHADOW_IDLE = 0.5f;


    /// <summary>
    /// 三人小队绕圈连招 分身生成的时间
    /// </summary>
    static float TIME_CLONESHADOW_TEAMSTATE_306_CIRCLERUN_IDLE = 0.35f;

    /// <summary>
    /// 二人小队替身连招2 分身生成的时间
    /// </summary>
    static float TIME_CLONESHADOW_TEAMSTATE_204_SUBSTITUTE2_IDLE = 0.2f;







    /// <summary>
    /// 分身计时器
    /// <summary>
    float CloneShadowTimer = 0;

    /// <summary>
    /// 分身开始
    /// <summary>
    public void CloneShadowStart(float Timer)
    {
        //成员数变更 直接结束
        if (TeamBrain != null)
        {
            if (TeamBrain.MembersCheck()) { return; }
        }

        CloneShadowTimer = Timer;
        NowMainState = MainState.CloneShadow;
        SetDirector(Vector2.down);
        switch (TeamBrain.NowSubState)
        {
            //三人小队 分身连招 分身
            case WeavileTeamBrain.SubState.TeamState_304_CloneShadow:
                //为分身状态时分身
                if (TeamBrain.NowTeamState304CloneShadow == WeavileTeamBrain.TEAMSTATE304_CLONESHADOW.CloneShadow) {
                    LaunchWeavileCloneBody_304CloneShadow();
                }
                break;
            //三人小队 绕圈跑连招 分身
            case WeavileTeamBrain.SubState.TeamState_306_CircleRun:
                LaunchWeavileCloneBody_306CircleRun();
                break;
            //二人小队 替身连招2 分身
            case WeavileTeamBrain.SubState.TeamState_204_Substitute2:
                LaunchWeavileCloneBody_204Substitute2();
                break;
            //二人小队 绕圈跑连招 分身
            case WeavileTeamBrain.SubState.TeamState_205_CircleRun:
                LaunchWeavileCloneBody_205CircleRun();
                break;
        }
        AddCloneBodyToBrainList();
    }

    /// <summary>
    /// 分身结束
    /// <summary>
    public void CloneShadowOver()
    {
        CloneShadowTimer = 0;
        //有分身的话把分身添加至主脑列表
        //if (CloneBodyList.Count != 0) { AddCloneBodyToBrainList(); }
    }


    /// <summary>
    /// 三人小队分身连招 生成分身 
    /// </summary>
    void LaunchWeavileCloneBody_304CloneShadow()
    {
        //获取三只玛狃拉的中心
        Vector2 center = new Vector2(
                (TeamBrain.WeavileMembersList[0].transform.position.x + TeamBrain.WeavileMembersList[1].transform.position.x + TeamBrain.WeavileMembersList[2].transform.position.x)/3.0f ,
                (TeamBrain.WeavileMembersList[0].transform.position.y + TeamBrain.WeavileMembersList[1].transform.position.y + TeamBrain.WeavileMembersList[2].transform.position.y)/3.0f 
            );
        //生成点为中心和自己的中间
        Vector2 p1 = new Vector2(
                (center.x + transform.position.x)/2.0f,
                (center.y + transform.position.y)/2.0f
            );
        //以自己为中心偏转90度
        Vector2 p2 = Quaternion.AngleAxis(90.0f , Vector3.forward)*(p1 - (Vector2)transform.position) + transform.position;
        p2 = ParentPokemonRoom.EnsurePointReachesRoom(p2);
        BornACloneBody(p2);
    }


    /// <summary>
    /// 三人小队绕圈连招 生成分身 
    /// </summary>
    void LaunchWeavileCloneBody_306CircleRun()
    {
        Vector2 dir = ((Vector2)transform.position - TargetPosition).normalized;
        Vector2 p1 = Quaternion.AngleAxis(60.0f , Vector3.forward) * dir + transform.position;
        Vector2 p2 = Quaternion.AngleAxis(-60.0f , Vector3.forward) * dir + transform.position;
        Vector2 p3 = (Vector3)dir*0.5f + transform.position;
        BornACloneBody(p1);
        BornACloneBody(p2);
        BornACloneBody(p3);
    }

    /// <summary>
    /// 二人小队 替身连招2 生成分身 
    /// </summary>
    void LaunchWeavileCloneBody_204Substitute2()
    {
        Vector2 dir = ((Vector2)transform.position - TargetPosition).normalized;
        Vector2 p1 = Quaternion.AngleAxis(60.0f, Vector3.forward) * dir + transform.position;
        Vector2 p2 = Quaternion.AngleAxis(-60.0f, Vector3.forward) * dir + transform.position;
        p1 = ParentPokemonRoom.EnsurePointReachesRoom(p1);
        p2 = ParentPokemonRoom.EnsurePointReachesRoom(p2);
        BornACloneBody(p1);
        BornACloneBody(p2);
    }

    /// <summary>
    /// 二人小队绕圈连招 生成分身 
    /// </summary>
    void LaunchWeavileCloneBody_205CircleRun()
    {
        Vector2 dir = ((Vector2)transform.position - TargetPosition).normalized;
        Vector2 p1 = Quaternion.AngleAxis(25.0f, Vector3.forward) * dir + transform.position;
        Vector2 p2 = Quaternion.AngleAxis(-25.0f, Vector3.forward) * dir + transform.position;
        Vector2 p3 = Quaternion.AngleAxis(75.0f, Vector3.forward) * dir + transform.position;
        Vector2 p4 = Quaternion.AngleAxis(-75.0f, Vector3.forward) * dir + transform.position;
        BornACloneBody(ParentPokemonRoom.EnsurePointReachesRoom(p1));
        BornACloneBody(ParentPokemonRoom.EnsurePointReachesRoom(p2));
        BornACloneBody(ParentPokemonRoom.EnsurePointReachesRoom(p3));
        BornACloneBody(ParentPokemonRoom.EnsurePointReachesRoom(p4));
    }

    //=========================分身============================






    //=========================回复============================


    /// <summary>
    /// 回复的副状态
    /// </summary>
    enum HealSubState
    {
        Move, //移动中
        Heal2Move, //移动转入回血
        Heal, //为目标回血中
    }

    /// <summary>
    /// 三人小队回复状态 接近防守目标时的速度加成
    /// </summary>
    static float SPEEDALPHA_HEAL_TEAMSTATE307HEAL1_308HEAL2 = 2.3f;


    /// <summary>
    /// 三人小队回复状态1 大于这个距离后转入接近的距离
    /// </summary>
    static float DISTENCE_HEAL_BETWEEN_TARGET_TEAMSTATE307HEAL1_MAX = 2.5f;
    /// <summary>
    /// 三人小队回复状态1 接近到一定距离后不再接近的距离
    /// </summary>
    static float DISTENCE_HEAL_BETWEEN_TARGET_TEAMSTATE307HEAL1_MIN = 2.0f;


    /// <summary>
    /// 三人小队回复状态2 大于这个距离后转入接近的距离
    /// </summary>
    static float DISTENCE_HEAL_BETWEEN_TARGET_TEAMSTATE308HEAL2_MAX = 1.2f;
    /// <summary>
    /// 三人小队回复状态2 接近到一定距离后不再接近的距离
    /// </summary>
    static float DISTENCE_HEAL_BETWEEN_TARGET_TEAMSTATE308HEAL2_MIN = 0.9f;



    /// <summary>
    /// 三人小队回血连招1 回血总时间
    /// </summary>
    static float TIME_HEAL_TEMASTATE307_HEAL1 = 30.0f;


    /// <summary>
    /// 三人小队回血连招2 回血总时间
    /// </summary>
    static float TIME_HEAL_TEMASTATE308_HEAL2 = 24.0f;


    /// <summary>
    /// 二人小队回血连招 回血总时间
    /// </summary>
    static float TIME_HEAL_TEMASTATE206_HEAL = 30.0f;

    /// <summary>
    /// 回复计时器
    /// <summary>
    float HealTimer = 0;
    /// <summary>
    /// 回复时移动方向
    /// </summary>
    Vector2 Dir_Heal_Move = Vector2.zero;
    /// <summary>
    /// 当前的回复副状态
    /// </summary>
    HealSubState NowHealSubState;







    /// <summary>
    /// 回复开始
    /// <summary>
    public void HealStart(float Timer)
    {
        //成员数变更 直接结束
        if (TeamBrain != null)
        {
            if (TeamBrain.MembersCheck()) { return; }
        }

        HealTimer = Timer;
        Dir_Heal_Move = Vector2.zero;
        NowHealSubState = HealSubState.Move;

        NowMainState = MainState.Heal;
        //LaunchHeal(Timer);
    }

    /// <summary>
    /// 回复结束
    /// <summary>
    public void HealOver()
    {
        HealTimer = 0;
        Dir_Heal_Move = Vector2.zero;
        NowHealSubState = HealSubState.Move;

        OverHealSupport();
    }


    /// <summary>
    /// 发射回复母件
    /// </summary>
    /// <param name="wl"></param>
    /// <param name="t"></param>
    void LaunchHeal( float t)
    {
        if (SupportParentObj != null)
        {
            SupportParentObj.SupportOver();
        }
        WeavileHealParent h = Instantiate(WeavileHealParentPrefab, transform.position, Quaternion.identity, transform);
        h.HelpingStart( t);
        h.ParentWeavile = this;
        SupportParentObj = h;
    }


    /// <summary>
    /// 发射回复反噬爆炸
    /// </summary>
    /// <param name="wl"></param>
    /// <param name="t"></param>
    void LaunchHealExplosion()
    {
        WeavileHealExplosion e = Instantiate(WeavileHealExplosionPrefab, transform.position, Quaternion.identity);
        e.empty = this;
    }

    void OverHealSupport()
    {
        if (SupportParentObj != null)
        {
            SupportParentObj.SupportOver();
        }
    }


    //=========================回复============================




    //■■■■■■■■■■■■■■■■■■■■状态机部分■■■■■■■■■■■■■■■■■■■■■■






































    //■■■■■■■■■■■■■■■■■■■■分身幻影■■■■■■■■■■■■■■■■■■■■■■

    /// <summary>
    /// 分身
    /// </summary>
    public WeavileCloneBody CloneBody;

    public List<WeavileCloneBody> CloneBodyList = new List<WeavileCloneBody> { };

    /// <summary>
    /// 移除所有幻影
    /// </summary>
    public void RemoveAllCloneBody()
    {
        foreach (WeavileCloneBody b in CloneBodyList)
        {
            b.SetCloneShadowOver();
        }
    }

    /// <summary>
    /// 生成一个分身幻影
    /// </summary>
    /// <param name="p"></param>
    public void BornACloneBody(Vector2 p)
    {
        p = new Vector2(
            Mathf.Clamp(p.x,                    //方向*速度
            ParentPokemonRoom.RoomSize[2] + transform.parent.position.x, //最小值
            ParentPokemonRoom.RoomSize[3] + transform.parent.position.x),//最大值
            Mathf.Clamp(p.y,                     //方向*速度 
            ParentPokemonRoom.RoomSize[1] + transform.parent.position.y,  //最小值
            ParentPokemonRoom.RoomSize[0] + transform.parent.position.y));//最大值
        WeavileCloneBody clone = Instantiate(CloneBody, p, Quaternion.identity);
        CloneBodyList.Add(clone);
        clone.SetCloneBody(this);
        clone.SetDirector(new Vector2(animator.GetFloat("LookX") , animator.GetFloat("LookY")));
    }

    //■■■■■■■■■■■■■■■■■■■■分身幻影■■■■■■■■■■■■■■■■■■■■■■














    //==============================音效枚举===================================

    /// <summary>
    /// 音效种类枚举
    /// </summary>
    public enum WeavileSE
    {

        HealPosion,
        Smoke,
        Explosion,

        JumpOver,
        Rush,
        DarkPusle,
        IcePunch,
        Step,

        Heal,
        Wall,
    }

    /// <summary>
    /// 一次性音效播放器生成
    /// </summary>
    public EnemyAudioPlayer audioPlayer;

    /// <summary>
    /// 循环音效音效播放器生成
    /// </summary>
    public LoopingSEAudioPlayer loopPlayer;


    //==============================音效枚举===================================



}
