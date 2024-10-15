using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;







/// <summary>
/// 对话框的父类
/// </summary>

public class NPCTalkPanel : MonoBehaviour
{
    /// <summary>
    /// 对话框的Text组件
    /// </summary>
    protected TextMeshProUGUI TalkInformation;

    /// <summary>
    /// 角色头像
    /// </summary>
    public Image HeadIconImage;

    /// <summary>
    /// 对话是否暂停
    /// </summary>
    public bool isTalkPuse;
    /// <summary>
    /// 对话框的母NPC
    /// </summary>
    protected GameNPC ParentNPC;
    protected TownNPC ParentTownNPC;






    public void ZButtonDown()
    {
        ZButton.Z.IsZButtonDown = true;
    }


    public virtual void PlayerExit()
    {
        if (TalkInformation != null)
        {
            gameObject.SetActive(false);
            if (ParentNPC != null)
            {
                ParentNPC.PlayerSpaceItem(false);
            }
            
        }

    }



}