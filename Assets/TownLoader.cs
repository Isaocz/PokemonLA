using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
/// <summary>
/// С�򿪷���Ŀ ����������� �ǽ��� �������� ����װ����
/// </summary>
public class TownDevelopmentProject
{
    /// <summary>
    /// ��Ŀ��״̬ 
    /// </summary>
    public enum ProjectStatus
    {
        Locked,         //δ����
        NotStarted,     //�ѽ�������δ��ʼ
        InProgress,     //��ʼ ��һ��ð�ս��������
        Completed,      //���
        NotSelected,    //��Ŀ����˵�δ��ѡ��
    }

    /// <summary>
    /// ��Ŀ������λ��
    /// </summary>
    public enum ProjectLocation
    {
        Town=0,               //С��
        WoodenHouse=1,        //�޽�ľ��
        PoliceStation=2,      //ð���߹���
        MilkBar=3,            //�̰�
        SkillMaker=4,         //���ܻ���
        Daycare=5,            //����Բ
        DaycareF2=6,          //����Բ��¥
        ItemShop=7,           //�����̵�
        BossClub=8,           //ͷĿ���ֲ�
        RockClub=9,           //��ʯ���ֲ�
    }

    public static string ProjectLocationChinese(ProjectLocation p)
    {
        switch (p)
        {
            case ProjectLocation.Town:          return "ð������Ŀ";
            case ProjectLocation.WoodenHouse:   return "���ǽ�����˾��Ŀ";
            case ProjectLocation.PoliceStation: return "ð���߹�����Ŀ";
            case ProjectLocation.MilkBar:       return "�����̰���Ŀ";
            case ProjectLocation.SkillMaker:    return "ͼͼ������������Ŀ";
            case ProjectLocation.Daycare:       return "�ƿǱ���԰��Ŀ";
            case ProjectLocation.DaycareF2:     return "�ƿǱ���԰��Ϣ����Ŀ";
            case ProjectLocation.ItemShop:      return "�ŵµ��ߵ���Ŀ";
            case ProjectLocation.BossClub:      return "ͷĿ�����ξ��ֲ���Ŀ";
            case ProjectLocation.RockClub:      return "��ʯ���ֲ���Ŀ";
            default: return "";
        }
    }

    /// <summary>
    /// ��Ŀ���
    /// </summary>
    public enum ProjectTypeEnum
    {
        DevelopmentProjects,         //��չ��Ŀ
        BuildProjects,               //������Ŀ
        DecorationProjects,          //װ������Ŀ
    }

    public string ProjectTypeChinese()
    {
        switch (ProjectType)
        {
            case ProjectTypeEnum.DevelopmentProjects: return "��չ��Ŀ";
            case ProjectTypeEnum.BuildProjects:       return "������Ŀ";
            case ProjectTypeEnum.DecorationProjects:  return "װ������Ŀ";
            default: return "";
        }
    }

    /// <summary>
    /// ��Ŀ�����к�
    /// </summary>
    public int ProjectIndex;
    /// <summary>
    /// ��Ŀ������
    /// </summary>
    public ProjectTypeEnum ProjectType;
    /// <summary>
    /// ��Ŀ������
    /// </summary>
    public string ProjectName;
    /// <summary>
    /// ��Ŀ�Ĵ��ڵ�λ��
    /// </summary>
    public ProjectLocation ProjectLoca;
    /// <summary>
    /// ��Ŀ������
    /// </summary>
    public string ProjectDescribe;
    /// <summary>
    /// ��Ŀ��AP����
    /// </summary>
    public int ProjectAPPrice;
    /// <summary>
    /// ��Ŀ��ǰ����
    /// </summary>
    public ProjectStatus ProjectProgress;
    /// <summary>
    /// ǰ����Ŀ�����кţ���ɺ���Ŀ�ſɽ���
    /// </summary>
    public List<int> ProjectDependencies;

    /// <summary>
    /// �͵�ǰ��Ŀ�������Ŀ��������ĿxxxΪ�޸�ɳ��ΪƤ������yyyΪ�޸�ɳ��Ϊ�ٱ�ַ��xxx��yyy��Ϊ���⣬xxx��ɺ���yyyΪ��ɵ�δ��ѡ�á�
    /// </summary>
    public List<int> ProjectMutuallyExclusive;

    /// <summary>
    /// ��Ŀ����ʱ�����¼������ -1Ϊ����Ҫ�ȴ�ֱ����� 0Ϊ�ȴ��ڼ䲻�����κ�����
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
    /// ��Ŀ���ʱ����
    /// </summary>
    public void Completed()
    {
        ProjectProgress = ProjectStatus.Completed;
        //�軥����Ŀ
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
    /// ��Ŀ��ʼִ�У���������� �ǽ��� ����������ִ���ⲿ����ʱ������ȥ�ǣ�����С�����װ���������ⲽ��
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
    /// ������Ŀ�Ķ������� ��npc�ĶԻ����Ƿ����ĳ������
    /// </summary>
    public bool UnlockAdditionalConditions()
    {
        switch (ProjectIndex)
        {

            default: return true;//���������Ŀû��
                
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
    /// ��Ŀ�б�
    /// </summary>
    public static List<TownDevelopmentProject> TDPList = new List<TownDevelopmentProject>    
    {
                new TownDevelopmentProject(0 ,TownDevelopmentProject.ProjectTypeEnum.DevelopmentProjects , "С�����ţ���������" ,TownDevelopmentProject.ProjectLocation.Town ,  "�����С�򶫱�������Ӳݺ����֣�С����򶫱��������š�" ,          1500, TownDevelopmentProject.ProjectStatus.NotStarted,  new List<int>{ } , new List<int>{ },0),
                new TownDevelopmentProject(1 ,TownDevelopmentProject.ProjectTypeEnum.DevelopmentProjects , "С�����ţ���������"  ,TownDevelopmentProject.ProjectLocation.Town , "�����С������������Ӳݺ����֣�С����������������š�"  ,         2500, TownDevelopmentProject.ProjectStatus.NotStarted,  new List<int> { }, new List<int>{ }, 0),
                new TownDevelopmentProject(2 ,TownDevelopmentProject.ProjectTypeEnum.DevelopmentProjects , "С�����ţ����Ϸ���"  ,TownDevelopmentProject.ProjectLocation.Town , "�����С�򶫱�������Ӳݺ����֣�С��������Ϸ������š�"  ,         2500, TownDevelopmentProject.ProjectStatus.Locked,  new List<int> { 0, 1 }, new List<int>{ }, 0),
                new TownDevelopmentProject(3 ,TownDevelopmentProject.ProjectTypeEnum.DevelopmentProjects , "С�����ţ����Ϸ���"  ,TownDevelopmentProject.ProjectLocation.Town , "�����С�򶫱�������Ӳݺ����֣�С������Ϸ������š�"  ,         4000, TownDevelopmentProject.ProjectStatus.Locked,  new List<int> { 0, 1 }, new List<int>{ }, 0),
                new TownDevelopmentProject(4 ,TownDevelopmentProject.ProjectTypeEnum.DecorationProjects  , "ɳ����Ƥ������"  ,TownDevelopmentProject.ProjectLocation.PoliceStation , "Ϊð�չ�����ϴ󻻸���ɳ����"  ,   0, TownDevelopmentProject.ProjectStatus.Completed,  new List<int> { }, new List<int>{ 4, 5 }, -1),
                new TownDevelopmentProject(5 ,TownDevelopmentProject.ProjectTypeEnum.DecorationProjects  , "ɳ�����ٱ�ַ��"  ,TownDevelopmentProject.ProjectLocation.PoliceStation , "Ϊð�չ�����ϴ󻻸���ɳ����"  , 500, TownDevelopmentProject.ProjectStatus.NotStarted,  new List<int> { }, new List<int>{ 4 , 5 }, -1),
            
    };



    /// <summary>
    /// ��������˿�����Ŀ���ڶ�ȡ�浵ʱ�¼�����Щ����Ŀ
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
    /// ��ʼĳ����Ŀ
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
    /// ��Ŀ���ʱ�������¼�
    /// </summary>
    /// <param name="Index"></param>
    public static void CompleteProjectEvent(int Index)
    {
        switch (Index)
        {
            case 0:// �Ƴ�С�򶫱���������
                TownMap.townMap.TreeRU.SetActive(false);
                TownMap.townMap.FenceRU.SetActive(true);
                break;
            case 1:// �Ƴ�С��������������
                TownMap.townMap.TreeLU.SetActive(false);
                TownMap.townMap.FenceLU.SetActive(true);
                break;
            case 2:// �Ƴ�С�����Ϸ�������
                TownMap.townMap.TreeLD.SetActive(false);
                TownMap.townMap.FenceLD.SetActive(true);
                break;
            case 3:// �Ƴ�С���Ϸ�������
                TownMap.townMap.TreeRD.SetActive(false);
                TownMap.townMap.FenceRD.SetActive(true);
                break;

            case 4:// ����ɳ����Ƥ������
                TownMap.townMap.PoliceStation.SwitchSofaSprite(0);
                break;
            case 5:// ����ɳ�����ٱ�ַ��
                TownMap.townMap.PoliceStation.SwitchSofaSprite(1);
                break;
        }
    }


    /// <summary>
    /// ��Ŀ��ʼʱ�������¼�
    /// </summary>
    /// <param name="Index"></param>
    public static void InprogressProjectEvent(int Index)
    {
        switch (Index)
        {
            case 0:// ����״̬���κα仯�����ȴ�һ��ð��
                break;
        }
    }


    /// <summary>
    /// ���ĳ����Ŀ
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
    /// �����Ŀ�Ľ������
    /// </summary>
    public static void CheckforUnlock()
    {
        if (SaveLoader.saveLoader != null)
        {
            List<TownDevelopmentProject> List = SaveLoader.saveLoader.saveData.TownDevelopmentProjectsList;
            foreach (TownDevelopmentProject project in List)
            {
                //��������ѽ�������Ŀ 
                if (project.ProjectProgress == TownDevelopmentProject.ProjectStatus.Locked)
                {
                    bool canUnlock = true;
                    if (project.ProjectDependencies.Count != 0)
                    {
                        //�����Щ�����ǰ�������Ƿ��Ѿ��������״̬��
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
    /// �����Ŀ�Ľ��������ÿ�ν���ð�պ���ã������ĳ����Ŀ���ڽ����У�����һ��ð�պ���ɽ����еĸ���Ŀ
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
                //����������ڽ��е���Ŀ 
                if (project.ProjectProgress == TownDevelopmentProject.ProjectStatus.InProgress)
                {
                    CompleteProject(project.ProjectIndex);
                    SaveLoader.saveLoader.saveData.TownNPCDialogState.isProjectFinshThisRound = true;
                }
            }
        }

    }


    /// <summary>
    /// ��ȡ��ׯ
    /// </summary>
    public void LoadTown()
    {
        if (SaveLoader.saveLoader != null)
        {
            List<TownDevelopmentProject> List = SaveLoader.saveLoader.saveData.TownDevelopmentProjectsList;
            foreach (TownDevelopmentProject project in List)
            {
                //����������ڽ��е���Ŀ 
                if (project.ProjectProgress == TownDevelopmentProject.ProjectStatus.Completed)
                {
                    CompleteProjectEvent(project.ProjectIndex);
                }
            }
        }
    }


}
