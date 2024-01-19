using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPanleSkillBar : MonoBehaviour
{
    int PanleSkillBarColor;
    public UIDescribe SkillPanelDescribeImage ;
    public Skill PanelSkill;
    PlayerControler player;

    public void MouseEnter()
    {
        SkillPanelDescribeImage.MoveDescribe();
        SkillPanelDescribeImage.GetDescribeString(PanelSkill.SkillDiscribe,"",false);
        SkillPanelDescribeImage.gameObject.SetActive(true);
    }

    public void MouseExit()
    {
        SkillPanelDescribeImage.MoveDescribe();
        SkillPanelDescribeImage.gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        SkillPanelDescribeImage.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        player = FindObjectOfType<PlayerControler>();
    }

    void InstanceSkillBar()
    {
        transform.GetChild(12).gameObject.SetActive(false);
        transform.GetChild(13).gameObject.SetActive(false);
        transform.GetChild(14).gameObject.SetActive(false);
        transform.GetChild(17).gameObject.SetActive(false);
    }

    public void GetSkill_Panle(Skill TargetSkill , PlayerControler player)
    {
        InstanceSkillBar();
        PanelSkill = TargetSkill;

        if (TargetSkill.Damage == 0 && TargetSkill.SpDamage ==0)
        {
            transform.GetChild(14).gameObject.SetActive(true);
        }else if(TargetSkill.Damage == 0 && TargetSkill.SpDamage != 0)
        {
            transform.GetChild(13).gameObject.SetActive(true);
        }
        else if (TargetSkill.SpDamage == 0 && TargetSkill.Damage != 0)
        {
            transform.GetChild(12).gameObject.SetActive(true);
        }
        PanleSkillBarColor = TargetSkill.SkillType;
        transform.GetChild(15).gameObject.GetComponent<UiSkillBarTypeMark>().ChangeTypeMark(PanleSkillBarColor);
        transform.GetChild(15).gameObject.GetComponent<Image>().color = new Color(0.7f,0.7f,0.7f,0.15f);
        transform.GetChild(16).gameObject.GetComponent<UiSkillBarTypeMark>().ChangeTypeMark(PanleSkillBarColor);
        transform.GetChild(16).gameObject.GetComponent<Image>().color = new Color(0.7f, 0.7f, 0.7f, 0.7f);

        for (int i = 0;i < 12; i++)
        {
            GameObject NowObject = transform.GetChild(i).gameObject;
            if (player == null) { player = FindObjectOfType<PlayerControler>(); }
            if      (i == 6)  { NowObject.GetComponent<Text>().text = TargetSkill.SkillName; }
            else if (i == 7)  { NowObject.GetComponent<Text>().text = TargetSkill.SkillChineseName; }
            else if (i == 8)  { NowObject.GetComponent<Text>().text = Type.TypeChineseName[PanleSkillBarColor]; }
            else if (i == 9)  { NowObject.GetComponent<Text>().text = (TargetSkill.Damage+TargetSkill.SpDamage).ToString(); }
            else if (i == 10) { NowObject.GetComponent<Text>().text = TargetSkill.KOPoint.ToString(); }
            else if (i == 11) { NowObject.GetComponent<Text>().text = player.GetSkillCD(TargetSkill).ToString("#0.00"); }

            if (i == 0) { NowObject.GetComponent<Image>().color = Type.TypeColor[PanleSkillBarColor]; }
            else
            {
                NowObject.GetComponent<Text>().color = Type.TypeColor[PanleSkillBarColor] - new Vector4(0.3f, 0.3f, 0.3f, -1);
            }
        }

        if (TargetSkill.IsDamageChangeable)
        {
            transform.GetChild(9).gameObject.SetActive(false);
            transform.GetChild(17).gameObject.SetActive(true);
            if (transform.GetChild(17).GetComponent<Text>() != null)
            {
                transform.GetChild(17).GetComponent<Text>().color = Type.TypeColor[PanleSkillBarColor] - new Vector4(0.3f, 0.3f, 0.3f, -1);
            }

        }
        else
        {
            transform.GetChild(17).gameObject.SetActive(false);
            transform.GetChild(9).gameObject.SetActive(true);
        }

        Text FromText = transform.GetChild(18).GetComponent<Text>();
        UICallDescribe FromTextDiscrebe = transform.GetChild(18).GetComponent<UICallDescribe>();
        switch (PanelSkill.SkillFrom)
        {
            case 0:
                FromText.text = "「天赋技能」";
                FromTextDiscrebe.TwoMode = false; FromTextDiscrebe.FirstText = ""; FromTextDiscrebe.DescribeText = "此技能可能会在灵感突发时精通";
                break;
            case 1:
                FromText.text = "「学习技能」";
                FromTextDiscrebe.TwoMode = false; FromTextDiscrebe.FirstText = ""; FromTextDiscrebe.DescribeText = "此技能来自于传授或者技能学习机，无法在灵感突发时精通";
                break;
            case 2:
                FromText.text = "「精通技能」";
                FromTextDiscrebe.TwoMode = true; FromTextDiscrebe.FirstText = "此技能已经精通！"; FromTextDiscrebe.DescribeText = "精通效果:" + PanelSkill.PlusSkillDiscribe;
                break;
        }
        FromText.color = Type.TypeColor[PanleSkillBarColor] - new Vector4(0.3f, 0.3f, 0.3f, -1);

    }
}
