using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// ѡ���ʼһ���Ե��ߵİ�ť
/// </summary>
public class InitalItemButton : MonoBehaviour
{

    /// <summary>
    /// ��ť��Ӧһ���Ե��ߵ�ͼƬ
    /// </summary>
    public Image ButtonItemImage;
    /// <summary>
    /// ��ť˵����UI
    /// </summary>
    public UICallDescribe UICallDes;
    /// <summary>
    /// ��ť��Ӧһ���Ե��ߵ����к�
    /// </summary>
    public int ItemIndex;

    TRSPanelInitalItem ParentItemPanel;

    /// <summary>
    /// ����ѡ���ʼһ���Ե��ߵİ�ť
    /// </summary>
    /// <param name="Index">һ���Ե������к�</param>
    /// <param name="des">����UI</param>
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
            UICallDes.DescribeText = "��Я����ʼһ���Ե���";
            ItemIndex = -1;
        }
    }

    public void SelectItem()
    {
        ParentItemPanel.SetItem(ItemIndex);
    }
}
