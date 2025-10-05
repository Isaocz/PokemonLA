using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetGroupPanel : MonoBehaviour
{



    /// <summary>
    /// ð���ŵȼ���
    /// </summary>
    public InfoPanelGLBar Bar;
    /// <summary>
    /// ʣ���AP
    /// </summary>
    public Text AP;
    /// <summary>
    /// ð��������
    /// </summary>
    public Text GroupName;
    /// <summary>
    /// ð���ŵȼ�
    /// </summary>
    public Text GroupLevel;
    /// <summary>
    /// ��ð�մ���
    /// </summary>
    public Text PlayCount;
    /// <summary>
    /// ��Ա��
    /// </summary>
    public Text Members;

    private void OnEnable()
    {
        SaveData save = SaveLoader.saveLoader.saveData;
        Bar.SetLevel();
        AP.text = save.AP.ToString() ;
        GroupName.text = save.SaveName+"ð����";
        GroupLevel.text = SaveData.GroupLevelName[save.GroupLevel]+"��";
        PlayCount.text = save.GameCount.ToString();

        int s = 0;
        for (int i = 0; i < save.RoleList.Count;i++)
        {
            if (save.RoleList[i].isUnlock) { s++; }
        }
        Members.text = s.ToString();
    }
}
