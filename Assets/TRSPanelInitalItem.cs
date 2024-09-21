using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// 初始道具选择界面
/// </summary>
public class TRSPanelInitalItem : MonoBehaviour
{
    /// <summary>
    /// 选择道具按钮的预制件
    /// </summary>
    public InitalItemButton ItemButton;
    /// <summary>
    /// 生成按钮的父对象
    /// </summary>
    public Transform ButtonPanent;
    /// <summary>
    /// 一次性道具列表
    /// </summary>
    public GameObject SpaceItemList;
    /// <summary>
    /// 红叉
    /// </summary>
    public Sprite NoneSprite;
    /// <summary>
    /// 当前选择的一次性道具图片
    /// </summary>
    public Image ItemIcon;


    /// <summary>
    /// 说明框右
    /// </summary>
    public UIDescribe uIDescribeR;
    /// <summary>
    /// 说明框左
    /// </summary>
    public UIDescribe uIDescribeL;


    bool RefreshPanel;

    /// <summary>
    /// 设置初始一次性道具界面
    /// </summary>
    public void SetInitalItemPanel(SaveData save)
    {
        int index = StartPanelPlayerData.PlayerData.PlayerInitialItem;

        SetItem(index);


        ButtonPanent.transform.GetChild(0).gameObject.GetComponent<InitalItemButton>().SetItemButton(-1, null, uIDescribeR , this);
        if (ButtonPanent.transform.childCount > 1) {
            for (int i = 1; i < ButtonPanent.transform.childCount; i++)
            {
                Destroy(ButtonPanent.transform.GetChild(i).gameObject);
            }
        }

        if (save.UnlockInitalItem.Count != 0) {
            for (int i = 0; i < save.UnlockInitalItem.Count; i++)
            {
                InitalItemButton b = Instantiate(ItemButton, ButtonPanent);
                b.SetItemButton(save.UnlockInitalItem[i], SpaceItemList.transform.GetChild(save.UnlockInitalItem[i]).gameObject, uIDescribeR , this);
            }
        }

        RefreshPanel = true;
    }


    public void SetItem(int ItemIndex)
    {
        StartPanelPlayerData.PlayerData.PlayerInitialItem = ItemIndex;
        if (ItemIndex != -1) { ItemIcon.sprite = SpaceItemList.transform.GetChild(ItemIndex).GetComponent<Item>().StoreImage; }
        else { ItemIcon.sprite = NoneSprite; }
    }


    private void LateUpdate()
    {
        if (RefreshPanel)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
            LayoutRebuilder.ForceRebuildLayoutImmediate(transform.GetChild(0).GetComponent<RectTransform>());
            LayoutRebuilder.ForceRebuildLayoutImmediate(ButtonPanent.GetComponent<RectTransform>());
            RefreshPanel = false;
        }
    }

}
