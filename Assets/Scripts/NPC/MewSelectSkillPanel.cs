using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MewSelectSkillPanel : MonoBehaviour
{
    public PlayerControler player;
    protected MewNPC ParentMewNPC;

    public UIPanelGwtNewSkill GetNewSkillPanel;
    Text text;
    UIPanleSkillBar SkillBar01;
    Text Skill01ButtonText;
    UIPanleSkillBar SkillBar02;
    Text Skill02ButtonText;
    UIPanleSkillBar SkillBar03;
    Text Skill03ButtonText;

    string PokemonChineseName;
    Skill Skill01;
    Skill Skill02;
    Skill Skill03;


    private void Awake()
    {
        text = transform.GetChild(1).GetComponent<Text>();
        SkillBar01 = transform.GetChild(2).GetComponent<UIPanleSkillBar>();
        Skill01ButtonText = transform.GetChild(3).GetChild(0).GetComponent<Text>();
        SkillBar02 = transform.GetChild(4).GetComponent<UIPanleSkillBar>();
        Skill02ButtonText = transform.GetChild(5).GetChild(0).GetComponent<Text>();
        SkillBar03 = transform.GetChild(6).GetComponent<UIPanleSkillBar>();
        Skill03ButtonText = transform.GetChild(7).GetChild(0).GetComponent<Text>();
        ParentMewNPC = transform.parent.parent.GetComponent<MewNPC>();
        SetLNSSPanel();
    }


    public void SetLNSSPanel()
    {
        Skill01 = player.playerSkillList.RandomGetAMEWSkill();
        Skill02 = player.playerSkillList.RandomGetAMEWSkill();
        Skill03 = player.playerSkillList.RandomGetAMEWSkill();

        PokemonChineseName = player.PlayerNameChinese;

        if (Skill01 == null) { SkillBar01.gameObject.SetActive(false); Skill01ButtonText.transform.parent.gameObject.SetActive(false); }
        if (Skill02 == null) { SkillBar02.gameObject.SetActive(false); Skill02ButtonText.transform.parent.gameObject.SetActive(false); }
        if (Skill03 == null) { SkillBar03.gameObject.SetActive(false); Skill03ButtonText.transform.parent.gameObject.SetActive(false); }
        //Debug.Log(PokemonChineseName);
        //Debug.Log(text);
        if (SkillBar01.PanelSkill == null) { 
            SkillBar01.GetSkill_Panle(Skill01, player);
            Skill01ButtonText.text = "学习技能" + Skill01.SkillChineseName;
        }
        if (SkillBar02.PanelSkill == null)
        {
            SkillBar02.GetSkill_Panle(Skill02, player);
            Skill02ButtonText.text = "学习技能" + Skill02.SkillChineseName;
        }
        if (SkillBar03.PanelSkill == null)
        {
            SkillBar03.GetSkill_Panle(Skill03, player);
            Skill03ButtonText.text = "学习技能" + Skill03.SkillChineseName;
        }
    }

    public void LearnSkill01()
    {
        player.LearnNewSkillByOtherWay(Skill01);
        gameObject.SetActive(false);
        ParentMewNPC.Beybey();
    }
    public void LearnSkill02()
    {
        player.LearnNewSkillByOtherWay(Skill02);
        gameObject.SetActive(false);
        ParentMewNPC.Beybey();
    }
    public void LearnSkill03()
    {
        player.LearnNewSkillByOtherWay(Skill03);
        gameObject.SetActive(false);
        ParentMewNPC.Beybey();
    }
    public void GiveUpLearnSkill()
    {
        gameObject.SetActive(false);
        ParentMewNPC.TalkPanel.gameObject.SetActive(true);
        ParentMewNPC.TalkPanel.GetComponent<MewTalkPamel>().Talked();
    }
}
