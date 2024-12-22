using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MilkBarPanel : MonoBehaviour
{

    /// <summary>
    /// ��ʾ��ǰAP���
    /// </summary>
    public Text NowAP;

    /// <summary>
    /// �л���ť
    /// </summary>
    public MilkPanelSwitch Switch;

    /// <summary>
    /// ��Ʒ����
    /// </summary>
    public Transform DrinkPanel;

    /// <summary>
    /// �͵����
    /// </summary>
    public Transform FoodPanel;

    /// <summary>
    /// �����Ի���
    /// </summary>
    public UIDescribe Describe;

    /// <summary>
    /// ��ɫ�趨
    /// </summary>
    StartPanelPlayerData playerData;


    public void SetPanel()
    {

        playerData = StartPanelPlayerData.PlayerData;

        if (Switch.SwitchIndex == 0)        //��ʾ��Ʒ����
        {
            DrinkPanel.gameObject.SetActive(true);
            FoodPanel.gameObject.SetActive(false);
        }
        else if (Switch.SwitchIndex == 1)        //��ʾ�͵����
        {
            DrinkPanel.gameObject.SetActive(false);
            FoodPanel.gameObject.SetActive(true);
        }





    }


    /// <summary>
    /// ��������Ʒ��ťΪfalse
    /// </summary>
    void SetAllDrinkButtonFalse()
    {
        for (int i = 0; i < DrinkPanel.childCount; i++)
        {
            DrinkPanel.GetChild(i).GetComponent<Button>().interactable = false;
        }
    }

    /// <summary>
    /// ��������Ʒ��ťΪtrue
    /// </summary>
    void SetAllDrinkButtonTrue()
    {
        for (int i = 0; i < DrinkPanel.childCount; i++)
        {
            DrinkPanel.GetChild(i).GetComponent<Button>().interactable = true;
        }
    }

    /// <summary>
    /// �����в͵㰴ťΪfalse
    /// </summary>
    void SetAllFoodButtonFalse()
    {
        for (int i = 0; i < FoodPanel.childCount; i++)
        {
            FoodPanel.GetChild(i).GetComponent<Button>().interactable = false;
        }
    }

    /// <summary>
    /// �����в͵㰴ťΪtrue
    /// </summary>
    void SetAllFoodButtonTrue()
    {
        for (int i = 0; i < FoodPanel.childCount; i++)
        {
            FoodPanel.GetChild(i).GetComponent<Button>().interactable = true;
        }
    }




    private void OnEnable()
    {
        playerData = StartPanelPlayerData.PlayerData;
        if (playerData != null && SaveLoader.saveLoader != null)
        {
            SetPanel();
        }

    }

}
