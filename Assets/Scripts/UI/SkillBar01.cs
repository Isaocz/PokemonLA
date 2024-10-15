using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillBar01 : MonoBehaviour
{

    Skill skill;
    public Image Mask;
    public UIPanleSkillBar uiPanleSkillBar;


    float originalsize;
    float SkillCdTime;
    float Timer = 0;
    int SkillBarColor = 0;
    public PlayerControler player;

    public bool isCDStart = false;


    // Start is called before the first frame update
    void Start()
    {
        originalsize = Mask.rectTransform.rect.width;
        Mask.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 0);
        Mask.transform.parent.GetComponent<Image>().color = PokemonType.TypeColor[SkillBarColor];
    }

    public void GetSkill(Skill targetskill )
    {
        if (targetskill == null) { Mask.transform.parent.GetComponent<Image>().color = PokemonType.TypeColor[0]; }
        else
        {
            skill = targetskill;
            if (uiPanleSkillBar != null)
            {
                uiPanleSkillBar.gameObject.SetActive(true);
                uiPanleSkillBar.GetSkill_Panle(skill , player);
            }
            SkillCdTime = skill.ColdDown;
            SkillBarColor = skill.SkillType;
            if (Mask != null) {
                Mask.transform.parent.GetComponent<Image>().color = PokemonType.TypeColor[SkillBarColor];
                Mask.transform.parent.Find("Text").GetComponent<Text>().text = skill.SkillName;
                Mask.transform.parent.Find("Text").GetComponent<Text>().color = PokemonType.TypeColor[SkillBarColor] - new Vector4(0.3f, 0.3f, 0.3f, -1);
                Mask.transform.parent.Find("TypeMark").GetComponent<UiSkillBarTypeMark>().ChangeTypeMark(SkillBarColor);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        if (isCDStart && skill != null)
        {
            Mask.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, originalsize - originalsize * (1/ player.GetSkillCD(skill) ) * Timer);
            Timer += Time.deltaTime;
            if (Timer > player.GetSkillCD(skill) ) { isCDStart = false;Timer = 0; }
        }
    }
    public void CDPlus(float CDPlusPoint)
    {
        if(isCDStart && skill != null)
        {
            Timer += CDPlusPoint;
            Mask.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, originalsize - originalsize * (1 / player.GetSkillCD(skill)) * Timer);
            
        }
    }

    public void SetZero()
    {
        if (skill != null) {
            Timer = 0;
            Mask.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, originalsize - originalsize * (1 / player.GetSkillCD(skill)) * Timer);
        }
    }
}
