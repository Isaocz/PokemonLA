using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectRolePanelAbilityButton : MonoBehaviour
{
    public Image ToggleImage;
    public Text text;
    public Image ToggleMask;
    bool isOn;
    public Sprite SpriteNormal;
    public Sprite SpriteHL;

    StartPanelPlayerData pData;
    UICallDescribe CallUI;

    public Color TextColorNormal;
    public Color TextColorHL;

    public int AbilityIndex;
    public Button GamestartButton;
    public List<Toggle> TogglesList = new List<Toggle> { };
    


    /// <summary>
    /// …Ë÷√∞¥≈•
    /// </summary>
    /// <param name="ability"></param>
    public void SetAbilityToggle( PokemonType.Ability ability , int Index , UIDescribe uIDescribe , Button GameStart)
    {


        pData = StartPanelPlayerData.PlayerData;
        ToggleImage.sprite = SpriteNormal;

        ToggleMask.color = ability.AbilityToggleColor;
        text.color = ability.AbilityToggleTextColor;
        text.text = ability.AbilityChineseName;
        if (text.text.Length <= 4)
        {
            text.text = _mTool.AddSpaceInString(text.text);
        }

        TextColorHL = ability.AbilityToggleTextColorHL;
        TextColorNormal = ability.AbilityToggleTextColor;
        AbilityIndex = Index;
        GamestartButton = GameStart;

        CallUI = GetComponent<UICallDescribe>();
        CallUI.DescribeUI = uIDescribe;
        if (ability.AbilityDescribe02 == "")
        {
            CallUI.TwoMode = false;
            CallUI.DescribeText = ability.AbilityDescribe01;
            CallUI.FirstText = ability.AbilityDescribe02;
        }
        else
        {
            CallUI.TwoMode = true;
            CallUI.DescribeText = ability.AbilityDescribe02;
            CallUI.FirstText = ability.AbilityDescribe01;
        }
    }

    public void ChangeSprite()
    {
        if (isOn) { 
            isOn = false; 
            ToggleImage.sprite = SpriteNormal;
            text.color = TextColorNormal;
        }
        else {
            isOn = true;
            ToggleImage.sprite = SpriteHL;
            text.color = TextColorHL;
        }
    }

    public void SelectAbility()
    {
        if (!GetComponent<Toggle>().isOn)
        {
            CancelSelect();
        }
        else
        {
            for (int i = 0; i < TogglesList.Count; i++)
            {
                if (i != AbilityIndex) { TogglesList[i].isOn = false; }
            }

            pData.PlayerAbilityIndex = AbilityIndex;
            GamestartButton.interactable = true;
        }
    }

    void CancelSelect()
    {
        pData.PlayerAbilityIndex = -1;
        GamestartButton.interactable = false;
    }

    public void _OnMouseEnter()
    {
        ToggleImage.sprite = SpriteHL;
        text.color = TextColorHL;
        CallUI.MouseEnter();
    }

    public void _OnMouseExit()
    {
        CallUI.MouseExit();
        if (!isOn) {
            ToggleImage.sprite = SpriteNormal;
            text.color = TextColorNormal;
            
        }
    }
}
