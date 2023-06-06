using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPanelGwtNewSkill : MonoBehaviour
{
    public string PokemonNameChinese;
    public PlayerControler playerControler;

    // Start is called before the first frame update
    Text PokemonNameChineseText;
    Skill NewSkill;
    UIPanleSkillBar uIPanleNewSkillBar;
    GameObject GetNewSkillPanel2;

    public void NewSkillPanzelJump(Skill GetNewSkill )
    {
        PokemonNameChineseText = transform.GetChild(0).gameObject.GetComponent<Text>();
        uIPanleNewSkillBar = transform.GetChild(1).gameObject.GetComponent<UIPanleSkillBar>();
        playerControler = FindObjectOfType<PlayerControler>();
        NewSkill = GetNewSkill;
        //Debug.Log(playerControler);


        transform.GetChild(0).gameObject.GetComponent<Text>().text = PokemonNameChinese + "学会了新技能" + NewSkill.SkillChineseName + "!";
        uIPanleNewSkillBar.GetSkill_Panle(NewSkill);

        gameObject.SetActive(true);
        transform.parent.gameObject.SetActive(true);
        transform.parent.GetChild(0).gameObject.SetActive(true);
        transform.parent.GetChild(1).gameObject.SetActive(false);
        transform.parent.GetChild(2).gameObject.SetActive(false);
        transform.parent.GetChild(3).gameObject.SetActive(false);
        transform.parent.GetChild(4).gameObject.SetActive(false);

        if (playerControler.Skill01 != null && playerControler.Skill02 != null && playerControler.Skill03 != null && playerControler.Skill04 != null )
        {
            GetNewSkillPanel2 =  transform.GetChild(3).gameObject;
            GetNewSkillPanel2.SetActive(true);
            GetNewSkillPanel2.transform.GetChild(0).GetComponent<Text>().text = PokemonNameChinese + "学会了新技能" + NewSkill.SkillChineseName + "!";
            GetNewSkillPanel2.transform.GetChild(2).GetComponent<UIPanleSkillBar>().GetSkill_Panle(NewSkill);
            GetNewSkillPanel2.transform.GetChild(3).GetChild(0).GetComponent<Text>().text = "忘掉技能" + playerControler.Skill01.SkillChineseName;
            GetNewSkillPanel2.transform.GetChild(4).GetChild(0).GetComponent<Text>().text = "忘掉技能" + playerControler.Skill02.SkillChineseName;
            GetNewSkillPanel2.transform.GetChild(5).GetChild(0).GetComponent<Text>().text = "忘掉技能" + playerControler.Skill03.SkillChineseName;
            GetNewSkillPanel2.transform.GetChild(6).GetChild(0).GetComponent<Text>().text = "忘掉技能" + playerControler.Skill04.SkillChineseName;
            GetNewSkillPanel2.transform.GetChild(7).GetChild(0).GetComponent<Text>().text = "放弃学习技能"+NewSkill.SkillChineseName;
           

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
                playerControler.GetNewSkill(NewSkill, 1);
                break;
            case 2:
                GetNewSkillPanel2.transform.GetChild(8).GetChild(0).GetComponent<Text>().text = "...1...2...3...空！" + PokemonNameChinese + "忘掉了技能" + playerControler.Skill02.SkillChineseName + "...";
                GetNewSkillPanel2.transform.GetChild(8).GetChild(1).GetComponent<Text>().text = "并且记住了新技能" + NewSkill.SkillChineseName + "!";
                GetNewSkillPanel2.transform.GetChild(8).gameObject.SetActive(true);
                playerControler.GetNewSkill(NewSkill, 2);
                break;
            case 3:
                GetNewSkillPanel2.transform.GetChild(8).GetChild(0).GetComponent<Text>().text = "...1...2...3...空！" + PokemonNameChinese + "忘掉了技能" + playerControler.Skill03.SkillChineseName + "...";
                GetNewSkillPanel2.transform.GetChild(8).GetChild(1).GetComponent<Text>().text = "并且记住了新技能" + NewSkill.SkillChineseName + "!";
                GetNewSkillPanel2.transform.GetChild(8).gameObject.SetActive(true);
                playerControler.GetNewSkill(NewSkill, 3);
                break;
            case 4:
                GetNewSkillPanel2.transform.GetChild(8).GetChild(0).GetComponent<Text>().text = "...1...2...3...空！" + PokemonNameChinese + "忘掉了技能" + playerControler.Skill04.SkillChineseName + "...";
                GetNewSkillPanel2.transform.GetChild(8).GetChild(1).GetComponent<Text>().text = "并且记住了新技能" + NewSkill.SkillChineseName + "!";
                GetNewSkillPanel2.transform.GetChild(8).gameObject.SetActive(true);
                playerControler.GetNewSkill(NewSkill, 4);
                break;
            case 5:
                GetNewSkillPanel2.transform.GetChild(8).GetChild(0).GetComponent<Text>().text = PokemonNameChinese + "放弃了学习技能" + NewSkill.SkillChineseName;
                GetNewSkillPanel2.transform.GetChild(8).GetChild(1).GetComponent<Text>().text = null;
                GetNewSkillPanel2.transform.GetChild(8).gameObject.SetActive(true);
                break;
        }
    }


    public void NewSkillPanelClose()
    {
        if (playerControler.Skill01 == null) {
            playerControler.GetNewSkill(NewSkill,1);
            transform.parent.GetChild(4).gameObject.SetActive(true);
            gameObject.SetActive(false);
        }
        else if(playerControler.Skill02 == null)
        {
            playerControler.GetNewSkill(NewSkill, 2);
            transform.parent.GetChild(4).gameObject.SetActive(true);
            gameObject.SetActive(false);
        }
        else if (playerControler.Skill03 == null)
        {
            playerControler.GetNewSkill(NewSkill, 3);
            transform.parent.GetChild(4).gameObject.SetActive(true);
            gameObject.SetActive(false);
        }
        else if (playerControler.Skill04 == null)
        {
            playerControler.GetNewSkill(NewSkill, 4);
            transform.parent.GetChild(4).gameObject.SetActive(true);
            gameObject.SetActive(false);
        }
        else
        {
            transform.parent.GetChild(4).gameObject.SetActive(true);
            transform.GetChild(3).GetChild(8).gameObject.SetActive(false);
            transform.GetChild(3).gameObject.SetActive(false);
            gameObject.SetActive(false);
        }

    }


    


}
