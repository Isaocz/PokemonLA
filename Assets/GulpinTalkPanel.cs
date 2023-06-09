using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GulpinTalkPanel : NPCTalkPanel
{
    SkillShopBuyPanel BuyPanel;
    SkillShopStrengthenPanel StrengthenPanel;

    private void Awake()
    {
        TalkTextList = new string[] { "呣呣呣，欢迎来到技能连接店！", "呣呣呣，下次再来欧！" };
        NPCTPAwake();
        BuyPanel = transform.parent.GetChild(1).GetComponent<SkillShopBuyPanel>();
        StrengthenPanel = transform.parent.GetChild(2).GetComponent<SkillShopStrengthenPanel>();
    }

    public void ChangeSkillBuyPanel()
    {
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
