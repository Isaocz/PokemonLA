using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MilkBarPanel : MonoBehaviour
{

    /// <summary>
    /// 显示当前AP余额
    /// </summary>
    public Text NowAP;

    /// <summary>
    /// 切换按钮
    /// </summary>
    public MilkPanelSwitch Switch;

    /// <summary>
    /// 饮品界面
    /// </summary>
    public Transform DrinkPanel;

    /// <summary>
    /// 餐点界面
    /// </summary>
    public Transform FoodPanel;

    /// <summary>
    /// 描述对话框
    /// </summary>
    public UIDescribe Describe;

    /// <summary>
    /// 角色设定
    /// </summary>
    StartPanelPlayerData playerData;


    public void SetPanel()
    {

        playerData = StartPanelPlayerData.PlayerData;

        if (Switch.SwitchIndex == 0)        //显示饮品界面
        {
            DrinkPanel.gameObject.SetActive(true);
            FoodPanel.gameObject.SetActive(false);
        }
        else if (Switch.SwitchIndex == 1)        //显示餐点界面
        {
            DrinkPanel.gameObject.SetActive(false);
            FoodPanel.gameObject.SetActive(true);
        }





    }


    /// <summary>
    /// 设所有饮品按钮为false
    /// </summary>
    void SetAllDrinkButtonFalse()
    {
        for (int i = 0; i < DrinkPanel.childCount; i++)
        {
            DrinkPanel.GetChild(i).GetComponent<Button>().interactable = false;
        }
    }

    /// <summary>
    /// 设所有饮品按钮为true
    /// </summary>
    void SetAllDrinkButtonTrue()
    {
        for (int i = 0; i < DrinkPanel.childCount; i++)
        {
            DrinkPanel.GetChild(i).GetComponent<Button>().interactable = true;
        }
    }

    /// <summary>
    /// 设所有餐点按钮为false
    /// </summary>
    void SetAllFoodButtonFalse()
    {
        for (int i = 0; i < FoodPanel.childCount; i++)
        {
            FoodPanel.GetChild(i).GetComponent<Button>().interactable = false;
        }
    }

    /// <summary>
    /// 设所有餐点按钮为true
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
