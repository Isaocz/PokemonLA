using UnityEngine;



[System.Serializable]
/// <summary>
/// 记录NPC对话状态
/// </summary>
public class NPCDialogState 
{

    /// <summary>
    /// 修建老匠 是否和修建老匠对话过
    /// </summary>
    public bool isStateWithWoodhouseOldMan01 = false;
    /// <summary>
    /// 修建老匠 是否知道你是冒险团负责人
    /// </summary>
    public bool isStateWithWoodhouseOldMan02 = false;
    /// <summary>
    /// 修建老匠 是否完成所有拓展项目
    /// </summary>
    public bool isStateWithWoodhouseOldMan03 = false;
    /// <summary>
    /// 修建老匠 是否开始了一个项目
    /// </summary>
    public bool isStartAProject = false;
    /// <summary>
    /// 修建老匠 是否有正在进行的项目
    /// </summary>
    public bool isInProjectNow = false;
    /// <summary>
    /// 修建老匠 本回合是否有项目完成
    /// </summary>
    public bool isProjectFinshThisRound = false;



    /// <summary>
    /// 是否和路卡利欧表明身份
    /// </summary>
    public bool isStateWithLucario01 = false;

    /// <summary>
    /// 是否和路卡利欧谈起爱管侍
    /// </summary>
    public bool isStateWithLucario02 = false;




    /// <summary>
    /// 爱管侍是否回到冒险镇
    /// </summary>
    public bool isStateWithIndeedee01 = false;
    /// <summary>
    /// 爱管侍是否完成奶馆的开设
    /// </summary>
    public bool isStateWithIndeedee02 = false;
    /// <summary>
    /// 是否告诉爱管侍回小镇的路
    /// </summary>
    public bool isStateWithIndeedee03 = false;
    /// <summary>
    /// 是否答应帮助爱管侍开拓村庄
    /// </summary>
    public bool isStateWithIndeedee04 = false;
    /// <summary>
    /// 是否完成东北部村庄开拓
    /// </summary>
    public bool isStateWithIndeedee05 = false;
    /// <summary>
    /// 
    /// </summary>
    public bool isStateWithIndeedee06 = false;





    public NPCDialogState()
    {
        isStateWithWoodhouseOldMan01 = false;
        isStateWithWoodhouseOldMan02 = false;
        isStateWithWoodhouseOldMan02 = false;
        isStartAProject = false;
        isInProjectNow = false;
        isProjectFinshThisRound = false;
        isStateWithLucario01 = false;
        isStateWithLucario02 = false;

        isStateWithIndeedee01 = false;
        isStateWithIndeedee02 = false;
        isStateWithIndeedee03 = false;
        isStateWithIndeedee04 = false;
        isStateWithIndeedee05 = false;
        isStateWithIndeedee06 = false;

    }


    public void SelectOption(Vector2Int b)
    {
        switch (b.x)
        {
            case 0: isStateWithWoodhouseOldMan01 = (b.y == 1); break;
            case 1: isStateWithWoodhouseOldMan02 = (b.y == 1); break;
        }
    }

}
