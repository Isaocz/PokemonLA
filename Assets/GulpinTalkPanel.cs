using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GulpinTalkPanel : NPCTalkPanel
{
    SkillShopBuyPanel BuyPanel;

    private void Awake()
    {
        TalkTextList = new string[] { "呣呣呣，欢迎来到技能连接店！", "呣呣呣，下次再来欧！" };
        NPCTPAwake();
        BuyPanel = transform.parent.GetChild(1).GetComponent<SkillShopBuyPanel>();
    }

    public void ChangeSkillBuyPanel()
    {
        BuyPanel.player = player;
        BuyPanel.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }
}
