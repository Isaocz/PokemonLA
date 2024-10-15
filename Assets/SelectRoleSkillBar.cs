using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectRoleSkillBar : MonoBehaviour
{
    /// <summary>
    /// 技能条的遮罩，柑橘属性改变
    /// </summary>
    public Image BarMask;
    /// <summary>
    /// 技能中文名
    /// </summary>
    public Text SkillCnName;
    /// <summary>
    /// 技能英文名
    /// </summary>
    public Text SkillEnName;
    /// <summary>
    /// 技能属性的文本
    /// </summary>
    public Text SkillType;
    /// <summary>
    /// 技能属性的标志
    /// </summary>
    public Image SkillTypeMark;
    /// <summary>
    /// 技能属性的标志高清
    /// </summary>
    public Image SkillTypeMarkHD;
    /// <summary>
    /// 属性标志列表
    /// </summary>
    public Sprite[] TypeMarkList;
    /// <summary>
    /// 属性标志列表HD
    /// </summary>
    public Sprite[] TypeMarkListHD;
    /// <summary>
    /// 技能品质
    /// </summary>
    public Text SkillQuillity;


    /// <summary>
    /// 技能的伤害
    /// </summary>
    public Text SkillDmage;
    /// <summary>
    /// 技能的【威力：】文本，当为变化技能的时候改为【威力：变化】
    /// </summary>
    public Text SkillDmageText;
    /// <summary>
    /// 技能的种类标志
    /// </summary>
    public Image DmageType;
    /// <summary>
    /// 技能的种类标志列表
    /// </summary>
    public Sprite[] DmageTypeMarkList;


    /// <summary>
    /// 技能的击飞值
    /// </summary>
    public Text SkillKO;
    /// <summary>
    /// 技能的冷却时间
    /// </summary>
    public Text SkillCD;


    /// <summary>
    /// 技能条的技能
    /// </summary>
    public Skill BarSkill;

    private void Start()
    {
        //SetBar(BarSkill);
    }

    public void SetBar( Skill skill , UIDescribe DescribeL , UIDescribe DescribeR)
    {
        transform.GetComponent<UICallDescribe>().DescribeUI = DescribeL;
        SkillQuillity.transform.GetComponent<UICallDescribe>().DescribeUI = DescribeR;

        BarMask.color = PokemonType.TypeColor[skill.SkillType];
        SkillCnName.text = skill.SkillChineseName;
        SkillEnName.text = skill.SkillName;
        SkillType.text = PokemonType.TypeChineseName[skill.SkillType];
        SkillTypeMark.sprite = TypeMarkList[skill.SkillType];
        SkillTypeMarkHD.sprite = TypeMarkListHD[skill.SkillType];

        if ( skill.Damage == 0 && skill.SpDamage == 0)
        {
            SkillDmageText.text = "威力:变化";
            SkillDmage.text = "";
            DmageType.sprite = DmageTypeMarkList[0];
        }
        else if (skill.Damage != 0)
        {
            DmageType.sprite = DmageTypeMarkList[1];
            if (skill.Damage > 0) 
            {
                SkillDmageText.text = "威力:";
                SkillDmage.text = skill.Damage.ToString();
            }
            else
            {
                SkillDmageText.text = "威力:变化";
                SkillDmage.text = "";
            }
        }
        else if (skill.SpDamage != 0)
        {
            DmageType.sprite = DmageTypeMarkList[2];
            if (skill.SpDamage > 0)
            {
                SkillDmageText.text = "威力:";
                SkillDmage.text = skill.SpDamage.ToString();
            }
            else
            {
                SkillDmageText.text = "威力:变化";
                SkillDmage.text = "";
            }
        }


        SkillKO.text = skill.KOPoint.ToString("F2");
        SkillCD.text = skill.ColdDown.ToString("F2");
    }

}
