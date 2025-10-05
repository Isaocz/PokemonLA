using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// 选择初始一次性道具的按钮
/// </summary>
public class InitalItemButton : MonoBehaviour
{

    /// <summary>
    /// 按钮对应一次性道具的图片
    /// </summary>
    public Image ButtonItemImage;
    /// <summary>
    /// 按钮说明用UI
    /// </summary>
    public UICallDescribe UICallDes;
    /// <summary>
    /// 按钮对应一次性道具的序列号
    /// </summary>
    public int ItemIndex;

    TRSPanelInitalItem ParentItemPanel;

    /// <summary>
    /// 设置选择初始一次性道具的按钮
    /// </summary>
    /// <param name="Index">一次性道具序列号</param>
    /// <param name="des">描述UI</param>
    public void SetItemButton( int Index , GameObject SpaceItem , UIDescribe des , TRSPanelInitalItem ParentPanel)
    {
        ParentItemPanel = ParentPanel;
        if (Index != -1) {
            Item i = SpaceItem.GetComponent<Item>();
            UICallDes.DescribeUI = des;
            UICallDes.TwoMode = true;
            UICallDes.FirstText = i.ItemName;
            UICallDes.DescribeText = i.ItemDescribe;

            ItemIndex = Index;
            ButtonItemImage.sprite = i.StoreImage;
        }
        else
        {
            UICallDes.DescribeUI = des;
            UICallDes.TwoMode = false;
            UICallDes.DescribeText = "不携带初始一次性道具";
            ItemIndex = -1;
        }
    }

    public void SelectItem()
    {
        ParentItemPanel.SetItem(ItemIndex);
    }
}
