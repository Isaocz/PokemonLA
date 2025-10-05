using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// ��ʼ����ѡ�����
/// </summary>
public class TRSPanelInitalItem : MonoBehaviour
{
    /// <summary>
    /// ѡ����߰�ť��Ԥ�Ƽ�
    /// </summary>
    public InitalItemButton ItemButton;
    /// <summary>
    /// ���ɰ�ť�ĸ�����
    /// </summary>
    public Transform ButtonPanent;
    /// <summary>
    /// һ���Ե����б�
    /// </summary>
    public GameObject SpaceItemList;
    /// <summary>
    /// ���
    /// </summary>
    public Sprite NoneSprite;
    /// <summary>
    /// ��ǰѡ���һ���Ե���ͼƬ
    /// </summary>
    public Image ItemIcon;


    /// <summary>
    /// ˵������
    /// </summary>
    public UIDescribe uIDescribeR;
    /// <summary>
    /// ˵������
    /// </summary>
    public UIDescribe uIDescribeL;


    bool RefreshPanel;

    /// <summary>
    /// ���ó�ʼһ���Ե��߽���
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
