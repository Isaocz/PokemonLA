using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
/// <summary>
/// 小镇开发项目 包括清除树林 盖建筑 升级建筑 建造装饰物
/// </summary>
public class TownDevelopmentProject
{
    /// <summary>
    /// 项目的状态 
    /// </summary>
    public enum ProjectStatus
    {
        Locked,         //未解锁
        NotStarted,     //已解锁但尚未开始
        InProgress,     //开始 下一次冒险结束后完成
        Completed,      //完成
        NotSelected,    //项目完成了但未被选用
    }

    /// <summary>
    /// 项目所处的位置
    /// </summary>
    public enum ProjectLocation
    {
        Town=0,               //小镇
        WoodenHouse=1,        //修建木屋
        PoliceStation=2,      //冒险者工会
        MilkBar=3,            //奶吧
        SkillMaker=4,         //技能画廊
        Daycare=5,            //保育圆
        DaycareF2=6,          //保育圆二楼
        ItemShop=7,           //道具商店
        BossClub=8,           //头目俱乐部
        RockClub=9,           //推石俱乐部
    }

    public static string ProjectLocationChinese(ProjectLocation p)
    {
        switch (p)
        {
            case ProjectLocation.Town:          return "冒险镇项目";
            case ProjectLocation.WoodenHouse:   return "铁骨建筑公司项目";
            case ProjectLocation.PoliceStation: return "冒险者公会项目";
            case ProjectLocation.MilkBar:       return "哞哞奶吧项目";
            case ProjectLocation.SkillMaker:    return "图图技能艺术廊项目";
            case ProjectLocation.Daycare:       return "破壳宝育园项目";
            case ProjectLocation.DaycareF2:     return "破壳宝育园休息室项目";
            case ProjectLocation.ItemShop:      return "古德道具店项目";
            case ProjectLocation.BossClub:      return "头目宝可梦俱乐部项目";
            case ProjectLocation.RockClub:      return "滚石俱乐部项目";
            default: return "";
        }
    }

    /// <summary>
    /// 项目类别
    /// </summary>
    public enum ProjectTypeEnum
    {
        DevelopmentProjects,         //拓展项目
        BuildProjects,               //建筑项目
        DecorationProjects,          //装饰物项目
    }

    public string ProjectTypeChinese()
    {
        switch (ProjectType)
        {
            case ProjectTypeEnum.DevelopmentProjects: return "拓展项目";
            case ProjectTypeEnum.BuildProjects:       return "建筑项目";
            case ProjectTypeEnum.DecorationProjects:  return "装饰物项目";
            default: return "";
        }
    }

    /// <summary>
    /// 项目的序列号
    /// </summary>
    public int ProjectIndex;
    /// <summary>
    /// 项目的种类
    /// </summary>
    public ProjectTypeEnum ProjectType;
    /// <summary>
    /// 项目的名字
    /// </summary>
    public string ProjectName;
    /// <summary>
    /// 项目的处于的位置
    /// </summary>
    public ProjectLocation ProjectLoca;
    /// <summary>
    /// 项目的描述
    /// </summary>
    public string ProjectDescribe;
    /// <summary>
    /// 项目的AP花费
    /// </summary>
    public int ProjectAPPrice;
    /// <summary>
    /// 项目当前进度
    /// </summary>
    public ProjectStatus ProjectProgress;
    /// <summary>
    /// 前置项目的序列号，完成后本项目才可解锁
    /// </summary>
    public List<int> ProjectDependencies;

    /// <summary>
    /// 和当前项目互斥的项目，比如项目xxx为修改沙发为皮卡丘风格，yyy为修改沙发为百变怪风格，xxx和yyy则为互斥，xxx完成后设yyy为完成但未被选用。
    /// </summary>
    public List<int> ProjectMutuallyExclusive;

    /// <summary>
    /// 项目进行时发生事件的序号 -1为不需要等待直接完成 0为等待期间不发生任何事情
    /// </summary>
    public int onProjectInProgressEventIndex;


    public TownDevelopmentProject(int projectIndex, ProjectTypeEnum projectType , string projectName, ProjectLocation projectloca, string projectDescribe, int projectAPPrice, ProjectStatus projectStatus, List<int> projectDependencies, List<int> projectMutuallyExclusive, int ProjectInProgressEventIndex)
    {
        ProjectIndex = projectIndex;
        ProjectType = projectType;
        ProjectName = projectName;
        ProjectLoca = projectloca;
        ProjectDescribe = projectDescribe;
        ProjectAPPrice = projectAPPrice;
        ProjectDependencies = projectDependencies;
        ProjectMutuallyExclusive = projectMutuallyExclusive;
        onProjectInProgressEventIndex = ProjectInProgressEventIndex;
        ProjectProgress = projectStatus;
    }

    /// <summary>
    /// 项目完成时发生
    /// </summary>
    public void Completed()
    {
        ProjectProgress = ProjectStatus.Completed;
        //设互斥项目
        if (SaveLoader.saveLoader != null) {
            for (int i = 0; i < ProjectMutuallyExclusive.Count; i++)
            {
                if (SaveLoader.saveLoader.saveData.TownDevelopmentProjectsList[ProjectMutuallyExclusive[i]].ProjectProgress == ProjectStatus.Completed && ProjectMutuallyExclusive[i] != ProjectIndex)
                {
                    SaveLoader.saveLoader.saveData.TownDevelopmentProjectsList[ProjectMutuallyExclusive[i]].ProjectProgress = ProjectStatus.NotSelected;
                }
            }
        }
        TownLoader.CompleteProjectEvent(ProjectIndex);
    }

    /// <summary>
    /// 项目开始执行（如清除树林 盖建筑 升级建筑会执行这部，暂时会有民工去盖，建造小物体或装饰物无需这步）
    /// </summary>
    public void InProgress()
    {
        ProjectProgress = ProjectStatus.InProgress;
        if (onProjectInProgressEventIndex != -1)
        {
            TownLoader.InprogressProjectEvent(onProjectInProgressEventIndex);
        }
    }


    /// <summary>
    /// 解锁项目的额外条件 如npc的对话，是否完成某个任务
    /// </summary>
    public bool UnlockAdditionalConditions()
    {
        switch (ProjectIndex)
        {

            default: return true;//绝大多数项目没有
                
        }
    }

}



public class TownLoader : MonoBehaviour
{

    public static TownLoader townLoader;

    private void Awake()
    {
        townLoader = this;
        
    }

    private void Start()
    {
        CheckforInprogress();
        LoadTown();
    }


    /// <summary>
    /// 项目列表
    /// </summary>
    public static List<TownDevelopmentProject> TDPList = new List<TownDevelopmentProject>    
    {
                new TownDevelopmentProject(0 ,TownDevelopmentProject.ProjectTypeEnum.DevelopmentProjects , "小镇扩张（东北方）" ,TownDevelopmentProject.ProjectLocation.Town ,  "清理掉小镇东北方向的杂草和树林，小镇会向东北方向扩张。" ,          1500, TownDevelopmentProject.ProjectStatus.NotStarted,  new List<int>{ } , new List<int>{ },0),
                new TownDevelopmentProject(1 ,TownDevelopmentProject.ProjectTypeEnum.DevelopmentProjects , "小镇扩张（西北方）"  ,TownDevelopmentProject.ProjectLocation.Town , "清理掉小镇西北方向的杂草和树林，小镇会向西北方向扩张。"  ,         2500, TownDevelopmentProject.ProjectStatus.NotStarted,  new List<int> { }, new List<int>{ }, 0),
                new TownDevelopmentProject(2 ,TownDevelopmentProject.ProjectTypeEnum.DevelopmentProjects , "小镇扩张（西南方）"  ,TownDevelopmentProject.ProjectLocation.Town , "清理掉小镇东北方向的杂草和树林，小镇会向西南方向扩张。"  ,         2500, TownDevelopmentProject.ProjectStatus.Locked,  new List<int> { 0, 1 }, new List<int>{ }, 0),
                new TownDevelopmentProject(3 ,TownDevelopmentProject.ProjectTypeEnum.DevelopmentProjects , "小镇扩张（东南方）"  ,TownDevelopmentProject.ProjectLocation.Town , "清理掉小镇东北方向的杂草和树林，小镇会向东南方向扩张。"  ,         4000, TownDevelopmentProject.ProjectStatus.Locked,  new List<int> { 0, 1 }, new List<int>{ }, 0),
                new TownDevelopmentProject(4 ,TownDevelopmentProject.ProjectTypeEnum.DecorationProjects  , "沙发（皮卡丘风格）"  ,TownDevelopmentProject.ProjectLocation.PoliceStation , "为冒险公会的老大换个新沙发吧"  ,   0, TownDevelopmentProject.ProjectStatus.Completed,  new List<int> { }, new List<int>{ 4, 5 }, -1),
                new TownDevelopmentProject(5 ,TownDevelopmentProject.ProjectTypeEnum.DecorationProjects  , "沙发（百变怪风格）"  ,TownDevelopmentProject.ProjectLocation.PoliceStation , "为冒险公会的老大换个新沙发吧"  , 500, TownDevelopmentProject.ProjectStatus.NotStarted,  new List<int> { }, new List<int>{ 4 , 5 }, -1),
            
    };



    /// <summary>
    /// 如果新增了开发项目，在读取存档时新加入这些新项目
    /// </summary>
    public static List<TownDevelopmentProject> AddNewProjectWhenLoadSave(List<TownDevelopmentProject> OldList)
    {
        if (OldList.Count < TDPList.Count)
        {
            for (int i = OldList.Count; i < TDPList.Count; i++)
            {
                OldList.Add(TDPList[i]);
            }
        }
        for (int i = 0; i < TDPList.Count; i++)
        {
            OldList[i].ProjectIndex = TDPList[i].ProjectIndex;
            OldList[i].ProjectAPPrice = TDPList[i].ProjectAPPrice;
            OldList[i].onProjectInProgressEventIndex = TDPList[i].onProjectInProgressEventIndex;
            OldList[i].ProjectMutuallyExclusive = TDPList[i].ProjectMutuallyExclusive;
            OldList[i].ProjectDependencies = TDPList[i].ProjectDependencies;
            OldList[i].ProjectDescribe = TDPList[i].ProjectDescribe;
            OldList[i].ProjectLoca = TDPList[i].ProjectLoca;
            OldList[i].ProjectName = TDPList[i].ProjectName;
            OldList[i].ProjectType = TDPList[i].ProjectType;
        }
        return OldList;
    }


    /// <summary>
    /// 开始某个项目
    /// </summary>
    /// <param name="taskId"></param>
    public static void StartProgressProject(int Index)
    {
        if (SaveLoader.saveLoader != null && !SaveLoader.saveLoader.saveData.TownNPCDialogState.isInProjectNow)
        {
            List<TownDevelopmentProject> List = SaveLoader.saveLoader.saveData.TownDevelopmentProjectsList;

            if (List.Count > Index )
            {
                if (List[Index].onProjectInProgressEventIndex != -1)
                {
                    List[Index].InProgress();
                    SaveLoader.saveLoader.saveData.TownNPCDialogState.isInProjectNow = true;
                }
                else
                {
                    CompleteProject(List[Index].ProjectIndex);
                    
                }
                CheckforUnlock();
            }
        }

    }





    /// <summary>
    /// 项目完成时发生的事件
    /// </summary>
    /// <param name="Index"></param>
    public static void CompleteProjectEvent(int Index)
    {
        switch (Index)
        {
            case 0:// 移除小镇东北方的树林
                TownMap.townMap.TreeRU.SetActive(false);
                TownMap.townMap.FenceRU.SetActive(true);
                break;
            case 1:// 移除小镇西北方的树林
                TownMap.townMap.TreeLU.SetActive(false);
                TownMap.townMap.FenceLU.SetActive(true);
                break;
            case 2:// 移除小镇西南方的树林
                TownMap.townMap.TreeLD.SetActive(false);
                TownMap.townMap.FenceLD.SetActive(true);
                break;
            case 3:// 移除小镇东南方的树林
                TownMap.townMap.TreeRD.SetActive(false);
                TownMap.townMap.FenceRD.SetActive(true);
                break;

            case 4:// 公会沙发（皮卡丘风格）
                TownMap.townMap.PoliceStation.SwitchSofaSprite(0);
                break;
            case 5:// 公会沙发（百变怪风格）
                TownMap.townMap.PoliceStation.SwitchSofaSprite(1);
                break;
        }
    }


    /// <summary>
    /// 项目开始时发生的事件
    /// </summary>
    /// <param name="Index"></param>
    public static void InprogressProjectEvent(int Index)
    {
        switch (Index)
        {
            case 0:// 进行状态无任何变化，仅等待一次冒险
                break;
        }
    }


    /// <summary>
    /// 完成某个项目
    /// </summary>
    /// <param name="taskId"></param>
    public static void CompleteProject(int Index)
    {
        if (SaveLoader.saveLoader != null)
        {
            List<TownDevelopmentProject> List = SaveLoader.saveLoader.saveData.TownDevelopmentProjectsList;

            if (List.Count > Index)
            {
                List[Index].Completed();
                CheckforUnlock();
            }
        }

    }

    /// <summary>
    /// 检查项目的解锁情况
    /// </summary>
    public static void CheckforUnlock()
    {
        if (SaveLoader.saveLoader != null)
        {
            List<TownDevelopmentProject> List = SaveLoader.saveLoader.saveData.TownDevelopmentProjectsList;
            foreach (TownDevelopmentProject project in List)
            {
                //检查所有已解锁的项目 
                if (project.ProjectProgress == TownDevelopmentProject.ProjectStatus.Locked)
                {
                    bool canUnlock = true;
                    if (project.ProjectDependencies.Count != 0)
                    {
                        //检查这些任务的前置任务是否已经处于完成状态。
                        foreach (int i in project.ProjectDependencies)
                        {
                            if (List[i].ProjectProgress != TownDevelopmentProject.ProjectStatus.Completed)
                            {
                                canUnlock = false;
                                break;
                            }
                        }
                    }
                    if (canUnlock && project.UnlockAdditionalConditions())
                    {
                        project.ProjectProgress = TownDevelopmentProject.ProjectStatus.NotStarted;
                    }
                }
            }
        }
        
    }

    /// <summary>
    /// 检查项目的进行情况，每次结束冒险后调用，如果有某个项目正在进行中，结束一次冒险后完成进行中的该项目
    /// </summary>
    public void CheckforInprogress()
    {
        if (SaveLoader.saveLoader.saveData.TownNPCDialogState.isInProjectNow)
        {
            SaveLoader.saveLoader.saveData.TownNPCDialogState.isInProjectNow = false;
        }
        if (SaveLoader.saveLoader != null)
        {
            List<TownDevelopmentProject> List = SaveLoader.saveLoader.saveData.TownDevelopmentProjectsList;
            foreach (TownDevelopmentProject project in List)
            {
                //检查所有正在进行的项目 
                if (project.ProjectProgress == TownDevelopmentProject.ProjectStatus.InProgress)
                {
                    CompleteProject(project.ProjectIndex);
                    SaveLoader.saveLoader.saveData.TownNPCDialogState.isProjectFinshThisRound = true;
                }
            }
        }

    }


    /// <summary>
    /// 读取村庄
    /// </summary>
    public void LoadTown()
    {
        if (SaveLoader.saveLoader != null)
        {
            List<TownDevelopmentProject> List = SaveLoader.saveLoader.saveData.TownDevelopmentProjectsList;
            foreach (TownDevelopmentProject project in List)
            {
                //检查所有正在进行的项目 
                if (project.ProjectProgress == TownDevelopmentProject.ProjectStatus.Completed)
                {
                    CompleteProjectEvent(project.ProjectIndex);
                }
            }
        }
    }


}
