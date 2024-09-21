using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TSRPanelRoleInfo : MonoBehaviour
{

    /// <summary>
    /// 说明框右
    /// </summary>
    public UIDescribe uIDescribeR;
    /// <summary>
    /// 说明框左
    /// </summary>
    public UIDescribe uIDescribeL;



    /// <summary>
    /// 角色名字文本
    /// </summary>
    public Text RoleNameText;
    /// <summary>
    /// 角色初始等级
    /// </summary>
    public Text RoleLevel;
    /// <summary>
    /// 角色头像
    /// </summary>
    public Image RoleHeadIcon;
    /// <summary>
    /// 角色糖果数
    /// </summary>
    public Text RoleCandyCount;
    /// <summary>
    /// 角色糖果图
    /// </summary>
    public Image RoleCandyIcon;
    /// <summary>
    /// 角色进化部分的父对象
    /// </summary>
    public Transform RoleEvolutionParent;
    /// <summary>
    /// 角色属性部分的父对象
    /// </summary>
    public Transform RoleTypeParent;

    /// <summary>
    /// 角色进化条件的列表
    /// </summary>
    public GameObject[] RoleEvolutionSituList;
    /// <summary>
    /// 属性标志
    /// </summary>
    public UIPokemonDataPanelTypemark TypeMark;


    /// <summary>
    /// 设置角色信息界面
    /// </summary>
    public void SetRoleInfoPanel(RoleInfo role )
    {

        _mTool.RemoveAllChild(RoleTypeParent.gameObject);
        _mTool.RemoveAllChild(RoleEvolutionParent.gameObject);

        RoleNameText.text = role.Role.PlayerNameChinese;
        RoleLevel.text = role.Role.Level.ToString();
        RoleHeadIcon.sprite = role.Role.PlayerHead;
        RoleHeadIcon.GetComponent<UICallDescribe>().DescribeUI = uIDescribeR;
        RoleCandyCount.text = role.Candy.ToString() ;
        RoleCandyIcon.sprite = role.Role.PlayerCandy ;

        GameObject EvoSitu = Instantiate(RoleEvolutionSituList[role.Role.PlayerIndex] , RoleEvolutionParent);
        for (int i = 0; i < EvoSitu.transform.childCount; i++)
        {
            EvoSitu.transform.GetChild(i).GetComponent<UICallDescribe>().DescribeUI = uIDescribeR;
        }
        if (role.Role.PlayerType01 != 0)
        {
            Debug.Log(role.Role.PlayerType01);
            UIPokemonDataPanelTypemark t = Instantiate(TypeMark, RoleTypeParent);
            t.GetChildTypeMark(role.Role.PlayerType01);
        }
        if (role.Role.PlayerType02 != 0)
        {
            Debug.Log(role.Role.PlayerType02);
            UIPokemonDataPanelTypemark t = Instantiate(TypeMark, RoleTypeParent);
            t.GetChildTypeMark(role.Role.PlayerType02);
        }
    }
}
