using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectRoleSkillBar : MonoBehaviour
{
    /// <summary>
    /// �����������֣��������Ըı�
    /// </summary>
    public Image BarMask;
    /// <summary>
    /// ����������
    /// </summary>
    public Text SkillCnName;
    /// <summary>
    /// ����Ӣ����
    /// </summary>
    public Text SkillEnName;
    /// <summary>
    /// �������Ե��ı�
    /// </summary>
    public Text SkillType;
    /// <summary>
    /// �������Եı�־
    /// </summary>
    public Image SkillTypeMark;
    /// <summary>
    /// �������Եı�־����
    /// </summary>
    public Image SkillTypeMarkHD;
    /// <summary>
    /// ���Ա�־�б�
    /// </summary>
    public Sprite[] TypeMarkList;
    /// <summary>
    /// ���Ա�־�б�HD
    /// </summary>
    public Sprite[] TypeMarkListHD;
    /// <summary>
    /// ����Ʒ��
    /// </summary>
    public Text SkillQuillity;


    /// <summary>
    /// ���ܵ��˺�
    /// </summary>
    public Text SkillDmage;
    /// <summary>
    /// ���ܵġ����������ı�����Ϊ�仯���ܵ�ʱ���Ϊ���������仯��
    /// </summary>
    public Text SkillDmageText;
    /// <summary>
    /// ���ܵ������־
    /// </summary>
    public Image DmageType;
    /// <summary>
    /// ���ܵ������־�б�
    /// </summary>
    public Sprite[] DmageTypeMarkList;


    /// <summary>
    /// ���ܵĻ���ֵ
    /// </summary>
    public Text SkillKO;
    /// <summary>
    /// ���ܵ���ȴʱ��
    /// </summary>
    public Text SkillCD;


    /// <summary>
    /// �������ļ���
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
            SkillDmageText.text = "����:�仯";
            SkillDmage.text = "";
            DmageType.sprite = DmageTypeMarkList[0];
        }
        else if (skill.Damage != 0)
        {
            DmageType.sprite = DmageTypeMarkList[1];
            if (skill.Damage > 0) 
            {
                SkillDmageText.text = "����:";
                SkillDmage.text = skill.Damage.ToString();
            }
            else
            {
                SkillDmageText.text = "����:�仯";
                SkillDmage.text = "";
            }
        }
        else if (skill.SpDamage != 0)
        {
            DmageType.sprite = DmageTypeMarkList[2];
            if (skill.SpDamage > 0)
            {
                SkillDmageText.text = "����:";
                SkillDmage.text = skill.SpDamage.ToString();
            }
            else
            {
                SkillDmageText.text = "����:�仯";
                SkillDmage.text = "";
            }
        }


        SkillKO.text = skill.KOPoint.ToString("F2");
        SkillCD.text = skill.ColdDown.ToString("F2");
    }

}
