using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectRolePanelAbilityButton : MonoBehaviour
{
    public Image image;
    public Text text;
    bool isOn;

    public Sprite SpriteNormal;
    public Sprite SpriteHL;
    public Color TextColorNormal;
    public Color TextColorHL;
    Toggle PanertToogle;
    StartPanelPlayerData pData;
    public Button GamestartButton;
    public List<Toggle> TogglesList = new List<Toggle> { };
    UICallDescribe CallUI;

    private void Start()
    {
        PanertToogle = transform.parent.GetComponent<Toggle>();
        pData = StartPanelPlayerData.PlayerData;
        TogglesList.Add(PanertToogle.transform.GetChild(13).GetComponent<Toggle>()) ;
        TogglesList.Add(PanertToogle.transform.GetChild(14).GetComponent<Toggle>()) ;
        TogglesList.Add(PanertToogle.transform.GetChild(15).GetComponent<Toggle>()) ;
        CallUI = GetComponent<UICallDescribe>();
    }

    public void ChangeSprite()
    {
        if (isOn) { 
            isOn = false; 
            image.sprite = SpriteNormal;
            text.color = TextColorNormal;
        }
        else {
            isOn = true;
            image.sprite = SpriteHL;
            text.color = TextColorHL;
        }
    }

    public void SelectAbility( int Index )
    {
        if (!GetComponent<Toggle>().isOn)
        {
            CancelSelect();
        }
        else
        {
            for (int i = 0; i < TogglesList.Count; i++)
            {
                if (i != Index) { TogglesList[i].isOn = false; }
            }

            pData.PlayerAbilityIndex = Index;
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
        if (PanertToogle.isOn) {
            image.sprite = SpriteHL;
            text.color = TextColorHL;
            CallUI.MouseEnter();
        }
    }

    public void _OnMouseExit()
    {
        if (!isOn) {
            image.sprite = SpriteNormal;
            text.color = TextColorNormal;
            CallUI.MouseExit();
        }
    }
}
