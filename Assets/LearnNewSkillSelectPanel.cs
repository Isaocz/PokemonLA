using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LearnNewSkillSelectPanel : MonoBehaviour
{
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
    }

    public void SetLNSSPanel(string pokemonChineseName, Skill skill01, Skill skill02, Skill skill03) 
    {
        Skill01 = skill01;
        Skill02 = skill02;
        Skill03 = skill03;
        PokemonChineseName = pokemonChineseName;

        if (skill01 == null) { SkillBar01.gameObject.SetActive(false); Skill01ButtonText.transform.parent.gameObject.SetActive(false);  }
        if (skill02 == null) { SkillBar02.gameObject.SetActive(false); Skill02ButtonText.transform.parent.gameObject.SetActive(false);  }
        if (skill03 == null) { SkillBar03.gameObject.SetActive(false); Skill03ButtonText.transform.parent.gameObject.SetActive(false);  }
        //Debug.Log(PokemonChineseName);
        //Debug.Log(text);
        text.text = PokemonChineseName + "���ͻ�����뵽���µļ��ܣ�\nѡ��һ��ѧϰ�ɣ�";
        SkillBar01.GetSkill_Panle(skill01);
        Skill01ButtonText.text = "ѧϰ����" + Skill01.SkillChineseName;
        SkillBar02.GetSkill_Panle(skill02);
        Skill02ButtonText.text = "ѧϰ����" + Skill02.SkillChineseName;
        SkillBar03.GetSkill_Panle(skill03);
        Skill03ButtonText.text = "ѧϰ����" + Skill03.SkillChineseName;
    }

    public void LearnSkill01()
    {
        GetNewSkillPanel.NewSkillPanzelJump(Skill01 , true);
        gameObject.SetActive(false);
    }
    public void LearnSkill02()
    {
        GetNewSkillPanel.NewSkillPanzelJump(Skill02 , true);
        gameObject.SetActive(false);
    }
    public void LearnSkill03()
    {
        GetNewSkillPanel.NewSkillPanzelJump(Skill03 , true);
        gameObject.SetActive(false);
    }
    public void GiveUpLearnSkill()
    {
        transform.parent.GetChild(4).gameObject.SetActive(true);
        gameObject.SetActive(false);
    }
}
