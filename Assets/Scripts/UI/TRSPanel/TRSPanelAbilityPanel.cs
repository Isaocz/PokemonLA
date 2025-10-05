using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// ��ɫѡ�����ѡ������
/// </summary>
public class TRSPanelAbilityPanel : MonoBehaviour
{
    /// <summary>
    /// ˵������
    /// </summary>
    public UIDescribe uIDescribeR;
    /// <summary>
    /// ˵������
    /// </summary>
    public UIDescribe uIDescribeL;
    /// <summary>
    /// ��Ϸ��ʼ��ť
    /// </summary>
    public Button GamestartButton;

    /// <summary>
    /// ���԰�ť��Ԥ�Ƽ�
    /// </summary>
    public SelectRolePanelAbilityButton AbilityToogle;


    public Transform AbilityTooglrParent;

    /// <summary>
    /// ��������ѡ�����
    /// </summary>
    public void SetAbilityToggle( RoleInfo role )
    {
        int count = 0;
        _mTool.RemoveAllChild(AbilityTooglrParent.gameObject);

        List<Toggle> ToggleList = new List<Toggle> { };

        if (role.Role.playerAbility01 != PlayerControler.PlayerAbilityList.������) {
            Toggle t = Instantiate(AbilityToogle, AbilityTooglrParent).GetComponent<Toggle>();
            t.GetComponent<SelectRolePanelAbilityButton>().SetAbilityToggle(PokemonType.AbilityList[(int)role.Role.playerAbility01], count, uIDescribeR, GamestartButton);
            ToggleList.Add(t);
            count++;
            t.isOn = true;
        }
        if (role.Role.playerAbility02 != PlayerControler.PlayerAbilityList.������) {
            Toggle t = Instantiate(AbilityToogle, AbilityTooglrParent).GetComponent<Toggle>();
            t.GetComponent<SelectRolePanelAbilityButton>().SetAbilityToggle(PokemonType.AbilityList[(int)role.Role.playerAbility02], count, uIDescribeR, GamestartButton);
            ToggleList.Add(t);
            count++;
        }
        if (role.isDreamAbilityUnlock && role.Role.playerAbilityDream != PlayerControler.PlayerAbilityList.������) {
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
