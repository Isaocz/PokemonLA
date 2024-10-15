using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GulpinTalkPanel : GameNPCTalkPanel
{
    SkillShopBuyPanel BuyPanel;
    SkillShopStrengthenPanel StrengthenPanel;

    private void Awake()
    {
        TalkTextList = new DialogString[] {
            new DialogString("�ޅޅޣ���ӭ�����������ӵ꣡" , DialogString.Face.Normal)  , 
            new DialogString("�ޅޅޣ��´�����ŷ��" , DialogString.Face.Happy)  , 
        };
        NPCTPAwake();
        BuyPanel = transform.parent.GetChild(1).GetComponent<SkillShopBuyPanel>();
        StrengthenPanel = transform.parent.GetChild(2).GetComponent<SkillShopStrengthenPanel>();
    }

    public void ChangeSkillBuyPanel()
    {
        Debug.Log(player);
        BuyPanel.player = player;
        BuyPanel.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }

    public void ChangeSkillStrengthenPanel()
    {
        StrengthenPanel.player = player;
        StrengthenPanel.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }
}
