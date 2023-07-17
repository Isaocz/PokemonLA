using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPanelGwtNewSkill : MonoBehaviour
{
    public static UIPanelGwtNewSkill StaticUIGNS;

    public string PokemonNameChinese;
    public PlayerControler playerControler;
    public LearnNewSkillSelectPanel LNSSPanel;
    bool IsLearnSkill;

    // Start is called before the first frame update
    Text PokemonNameChineseText;
    Skill NewSkill;
    UIPanleSkillBar uIPanleNewSkillBar;
    GameObject GetNewSkillPanel2;

    private void Awake()
    {
        StaticUIGNS = this;
        playerControler = FindObjectOfType<PlayerControler>();
        if (playerControler != null && playerControler.uIPanelGwtNewSkill == null)
        {
            playerControler.uIPanelGwtNewSkill = this;
            transform.gameObject.SetActive(false);
            transform.parent.gameObject.SetActive(false);
        }
    }

    public void SetPlayer(PlayerControler player)
    {
        if (player.uIPanelGwtNewSkill == null)
        {
            player.uIPanelGwtNewSkill = this;
            transform.gameObject.SetActive(false);
            transform.parent.gameObject.SetActive(false);
        }
    }

    public void SelectSkill( Skill skill01 , Skill skill02 , Skill skill03)
    {
        transform.parent.gameObject.SetActive(true);
        transform.parent.GetChild(0).gameObject.SetActive(true);
        transform.parent.GetChild(1).gameObject.SetActive(false);
        transform.parent.GetChild(2).gameObject.SetActive(false);
        transform.parent.GetChild(3).gameObject.SetActive(false);
        transform.parent.GetChild(4).gameObject.SetActive(false);
        LNSSPanel.gameObject.SetActive(true);
        LNSSPanel.SetLNSSPanel(PokemonNameChinese, skill01, skill02, skill03);
        gameObject.SetActive(false);
    }

    public void NewSkillPanzelJump(Skill GetNewSkill , bool isLearnSkill)
    {
        if (GetNewSkill.SkillFrom == 2)
        {
            IsLearnSkill = isLearnSkill;
            //Debug.Log(GetNewSkill);
            if (GetNewSkill == playerControler.Skill01.PlusSkill)      { playerControler.GetNewSkill(GetNewSkill, playerControler.Skill01, 1, IsLearnSkill); }
            else if(GetNewSkill == playerControler.Skill02.PlusSkill) { playerControler.GetNewSkill(GetNewSkill, playerControler.Skill02, 2, IsLearnSkill); }
            else if(GetNewSkill == playerControler.Skill03.PlusSkill) { playerControler.GetNewSkill(GetNewSkill, playerControler.Skill03, 3, IsLearnSkill); }
            else if(GetNewSkill == playerControler.Skill04.PlusSkill) { playerControler.GetNewSkill(GetNewSkill, playerControler.Skill04, 4, IsLearnSkill); }
            transform.parent.GetChild(4).gameObject.SetActive(true);

        }
        else
        {
            IsLearnSkill = isLearnSkill;
            PokemonNameChineseText = transform.GetChild(0).gameObject.GetComponent<Text>();
            uIPanleNewSkillBar = transform.GetChild(1).gameObject.GetComponent<UIPanleSkillBar>();
            playerControler = FindObjectOfType<PlayerControler>();
            NewSkill = GetNewSkill;
            //Debug.Log(playerControler);

            //Debug.Log(GetNewSkill.SkillChineseName);
            transform.GetChild(0).gameObject.GetComponent<Text>().text = PokemonNameChinese + "学会了新技能" + NewSkill.SkillChineseName + "!";
            transform.GetChild(2).GetChild(0).gameObject.GetComponent<Text>().text = "确定学习" + NewSkill.SkillChineseName;
            uIPanleNewSkillBar.GetSkill_Panle(NewSkill , playerControler);

            gameObject.SetActive(true);
            transform.parent.gameObject.SetActive(true);
            transform.parent.GetChild(0).gameObject.SetActive(true);
            transform.parent.GetChild(1).gameObject.SetActive(false);
            transform.parent.GetChild(2).gameObject.SetActive(false);
            transform.parent.GetChild(3).gameObject.SetActive(false);
            transform.parent.GetChild(4).gameObject.SetActive(false);

            if (playerControler.Skill01 != null && playerControler.Skill02 != null && playerControler.Skill03 != null && playerControler.Skill04 != null)
            {
                GetNewSkillPanel2 = transform.GetChild(4).gameObject;
                GetNewSkillPanel2.SetActive(true);
                GetNewSkillPanel2.transform.GetChild(0).GetComponent<Text>().text = PokemonNameChinese + "学会了新技能" + NewSkill.SkillChineseName + "!";
                GetNewSkillPanel2.transform.GetChild(2).GetComponent<UIPanleSkillBar>().GetSkill_Panle(NewSkill, playerControler);
                GetNewSkillPanel2.transform.GetChild(3).GetChild(0).GetComponent<Text>().text = "忘掉技能" + playerControler.Skill01.SkillChineseName;
                GetNewSkillPanel2.transform.GetChild(4).GetChild(0).GetComponent<Text>().text = "忘掉技能" + playerControler.Skill02.SkillChineseName;
                GetNewSkillPanel2.transform.GetChild(5).GetChild(0).GetComponent<Text>().text = "忘掉技能" + playerControler.Skill03.SkillChineseName;
                GetNewSkillPanel2.transform.GetChild(6).GetChild(0).GetComponent<Text>().text = "忘掉技能" + playerControler.Skill04.SkillChineseName;
                if (GetNewSkill.SkillFrom == 1 || !IsLearnSkill) { GetNewSkillPanel2.transform.GetChild(7).GetChild(0).GetComponent<Text>().text = "不学习" + NewSkill.SkillChineseName; }
                else { GetNewSkillPanel2.transform.GetChild(7).GetChild(0).GetComponent<Text>().text = "学习其他技能"; }
            }
            else
            {
                if (GetNewSkill.SkillFrom == 1 || !IsLearnSkill) { transform.GetChild(3).GetChild(0).GetComponent<Text>().text = "不学习" + NewSkill.SkillChineseName; }
                else { transform.GetChild(3).GetChild(0).GetComponent<Text>().text = "学习其他技能"; }
            }
        }
    }

    public void GetNewSkillForNUmber(int num)
    {
        switch (num)
        {
            case 1:
                GetNewSkillPanel2.transform.GetChild(8).GetChild(0).GetComponent<Text>().text = "...1...2...3...空！" + PokemonNameChinese + "忘掉了技能" + playerControler.Skill01.SkillChineseName + "...";
                GetNewSkillPanel2.transform.GetChild(8).GetChild(1).GetComponent<Text>().text = "并且记住了新技能" + NewSkill.SkillChineseName + "!";
                GetNewSkillPanel2.transform.GetChild(8).gameObject.SetActive(true);
                playerControler.GetNewSkill(NewSkill, playerControler.Skill01, 1, IsLearnSkill);
                break;
            case 2:
                GetNewSkillPanel2.transform.GetChild(8).GetChild(0).GetComponent<Text>().text = "...1...2...3...空！" + PokemonNameChinese + "忘掉了技能" + playerControler.Skill02.SkillChineseName + "...";
                GetNewSkillPanel2.transform.GetChild(8).GetChild(1).GetComponent<Text>().text = "并且记住了新技能" + NewSkill.SkillChineseName + "!";
                GetNewSkillPanel2.transform.GetChild(8).gameObject.SetActive(true);
                playerControler.GetNewSkill(NewSkill, playerControler.Skill02, 2, IsLearnSkill);
                break;
            case 3:
                GetNewSkillPanel2.transform.GetChild(8).GetChild(0).GetComponent<Text>().text = "...1...2...3...空！" + PokemonNameChinese + "忘掉了技能" + playerControler.Skill03.SkillChineseName + "...";
                GetNewSkillPanel2.transform.GetChild(8).GetChild(1).GetComponent<Text>().text = "并且记住了新技能" + NewSkill.SkillChineseName + "!";
                GetNewSkillPanel2.transform.GetChild(8).gameObject.SetActive(true);
                playerControler.GetNewSkill(NewSkill, playerControler.Skill03, 3, IsLearnSkill);
                break;
            case 4:
                GetNewSkillPanel2.transform.GetChild(8).GetChild(0).GetComponent<Text>().text = "...1...2...3...空！" + PokemonNameChinese + "忘掉了技能" + playerControler.Skill04.SkillChineseName + "...";
                GetNewSkillPanel2.transform.GetChild(8).GetChild(1).GetComponent<Text>().text = "并且记住了新技能" + NewSkill.SkillChineseName + "!";
                GetNewSkillPanel2.transform.GetChild(8).gameObject.SetActive(true);
                playerControler.GetNewSkill(NewSkill, playerControler.Skill04, 4, IsLearnSkill);
                break;
            case 5:
                GetNewSkillPanel2.transform.GetChild(8).GetChild(0).GetComponent<Text>().text = PokemonNameChinese + "放弃了学习技能" + NewSkill.SkillChineseName;
                GetNewSkillPanel2.transform.GetChild(8).GetChild(1).GetComponent<Text>().text = null;
                GetNewSkillPanel2.transform.GetChild(8).gameObject.SetActive(true);
                break;
        }
    }

    public void ReturnSelectPanel()
    {
        if (IsLearnSkill)
        {
            LNSSPanel.gameObject.SetActive(true);
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(false);
            transform.parent.GetChild(4).gameObject.SetActive(true);

        }
    }

    public void NewSkillPanelClose()
    {
        if (playerControler.Skill01 == null) {
            playerControler.GetNewSkill(NewSkill,null,1, IsLearnSkill);
            transform.parent.GetChild(4).gameObject.SetActive(true);
            gameObject.SetActive(false);
        }
        else if(playerControler.Skill02 == null)
        {
            playerControler.GetNewSkill(NewSkill,null, 2, IsLearnSkill);
            transform.parent.GetChild(4).gameObject.SetActive(true);
            gameObject.SetActive(false);
        }
        else if (playerControler.Skill03 == null)
        {
            playerControler.GetNewSkill(NewSkill,null, 3, IsLearnSkill);
            transform.parent.GetChild(4).gameObject.SetActive(true);
            gameObject.SetActive(false);
        }
        else if (playerControler.Skill04 == null)
        {
            playerControler.GetNewSkill(NewSkill,null, 4, IsLearnSkill);
            transform.parent.GetChild(4).gameObject.SetActive(true);
            gameObject.SetActive(false);
        }
        else
        {
            transform.parent.GetChild(4).gameObject.SetActive(true);
            transform.GetChild(4).GetChild(8).gameObject.SetActive(false);
            transform.GetChild(4).gameObject.SetActive(false);
            gameObject.SetActive(false);
        }

    }


    


}
