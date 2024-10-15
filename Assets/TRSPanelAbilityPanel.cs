using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// 角色选择见面选择特性
/// </summary>
public class TRSPanelAbilityPanel : MonoBehaviour
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
    /// 游戏开始按钮
    /// </summary>
    public Button GamestartButton;

    /// <summary>
    /// 特性按钮的预制件
    /// </summary>
    public SelectRolePanelAbilityButton AbilityToogle;


    public Transform AbilityTooglrParent;

    /// <summary>
    /// 设置特性选择界面
    /// </summary>
    public void SetAbilityToggle( RoleInfo role )
    {
        int count = 0;
        _mTool.RemoveAllChild(AbilityTooglrParent.gameObject);

        List<Toggle> ToggleList = new List<Toggle> { };

        if (role.Role.playerAbility01 != PlayerControler.PlayerAbilityList.无特性) {
            Toggle t = Instantiate(AbilityToogle, AbilityTooglrParent).GetComponent<Toggle>();
            t.GetComponent<SelectRolePanelAbilityButton>().SetAbilityToggle(PokemonType.AbilityList[(int)role.Role.playerAbility01], count, uIDescribeR, GamestartButton);
            ToggleList.Add(t);
            count++;
            t.isOn = true;
        }
        if (role.Role.playerAbility02 != PlayerControler.PlayerAbilityList.无特性) {
            Toggle t = Instantiate(AbilityToogle, AbilityTooglrParent).GetComponent<Toggle>();
            t.GetComponent<SelectRolePanelAbilityButton>().SetAbilityToggle(PokemonType.AbilityList[(int)role.Role.playerAbility02], count, uIDescribeR, GamestartButton);
            ToggleList.Add(t);
            count++;
        }
        if (role.isDreamAbilityUnlock && role.Role.playerAbilityDream != PlayerControler.PlayerAbilityList.无特性) {
            Toggle t = Instantiate(AbilityToogle, AbilityTooglrParent).GetComponent<Toggle>();
            t.GetComponent<SelectRolePanelAbilityButton>().SetAbilityToggle(PokemonType.AbilityList[(int)role.Role.playerAbilityDream], count, uIDescribeR, GamestartButton);
            ToggleList.Add(t);
            count++; 
        }

        for (int i = 0; i < ToggleList.Count; i++)
        {
            ToggleList[i].GetComponent<SelectRolePanelAbilityButton>().TogglesList = ToggleList;
        }
    }

}
