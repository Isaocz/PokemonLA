using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetGroupPanel : MonoBehaviour
{



    /// <summary>
    /// 冒险团等级条
    /// </summary>
    public InfoPanelGLBar Bar;
    /// <summary>
    /// 剩余的AP
    /// </summary>
    public Text AP;
    /// <summary>
    /// 冒险团名字
    /// </summary>
    public Text GroupName;
    /// <summary>
    /// 冒险团等级
    /// </summary>
    public Text GroupLevel;
    /// <summary>
    /// 总冒险次数
    /// </summary>
    public Text PlayCount;
    /// <summary>
    /// 成员数
    /// </summary>
    public Text Members;

    private void OnEnable()
    {
        SaveData save = SaveLoader.saveLoader.saveData;
        Bar.SetLevel();
        AP.text = save.AP.ToString() ;
        GroupName.text = save.SaveName+"冒险团";
        GroupLevel.text = SaveData.GroupLevelName[save.GroupLevel]+"：";
        PlayCount.text = save.GameCount.ToString();

        int s = 0;
        for (int i = 0; i < save.RoleList.Count;i++)
        {
            if (save.RoleList[i].isUnlock) { s++; }
        }
        Members.text = s.ToString();
    }
}
