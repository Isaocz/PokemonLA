using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GulpinTalkPanel : NPCTalkPanel
{
    SkillShopBuyPanel BuyPanel;

    private void Awake()
    {
        TalkTextList = new string[] { "�ޅޅޣ���ӭ�����������ӵ꣡", "�ޅޅޣ��´�����ŷ��" };
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
