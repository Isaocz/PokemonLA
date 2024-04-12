using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillShopStrengthenBlock : MonoBehaviour
{
    //public SkillBall skillBall;
    GameObject ParentGulpin;
    SkillShopStrengthenPanel StrengthenPanel;
    public UIPanleSkillBar SkillBar;
    SkillShopStrengthenMSButton MSButton;
    SkillShopStrengthenPPUPButton PPUPButton;
    Skill BlockSkill;
    int SkillPrice;


    private void Awake()
    {
        SkillBar = transform.GetChild(0).GetComponent<UIPanleSkillBar>();
        StrengthenPanel = transform.parent.parent.parent.parent.GetComponent<SkillShopStrengthenPanel>();
        MSButton = transform.GetChild(1).GetComponent<SkillShopStrengthenMSButton>();
        PPUPButton = transform.GetChild(2).GetComponent<SkillShopStrengthenPPUPButton>();

    }
    private void Start()
    {

        ParentGulpin = transform.parent.parent.parent.parent.parent.parent.gameObject;
    }

    public void GetSkill(Skill s)
    {
        BlockSkill = s;
        SkillBar.GetSkill_Panle(BlockSkill, StrengthenPanel.player);
        UICallDescribe MSButtonDescribe = MSButton.GetComponent<UICallDescribe>();
        UICallDescribe PPUPButtonDescribe = PPUPButton.GetComponent<UICallDescribe>();
        if (BlockSkill.SkillFrom == 2) { MSButtonDescribe.TwoMode = false; MSButtonDescribe.DescribeText = "�ü����Ѿ���ͨ"; }
        else {
            MSButtonDescribe.TwoMode = true; MSButtonDescribe.FirstText = "����һ����ͨ���ӿ��Ծ�ͨ�˼���"; MSButtonDescribe.DescribeText = "��ͨЧ����" + BlockSkill.PlusSkill.PlusSkillDiscribe;
        }

        if (BlockSkill.isPPUP) { PPUPButtonDescribe.TwoMode = false; PPUPButtonDescribe.DescribeText = "�ü��ܵ���ȴʱ���Ѿ���ǿ��"; }
        else { PPUPButtonDescribe.TwoMode = true; PPUPButtonDescribe.FirstText = "����һƿPPUP����ǿ���˼��ܣ�������̸ü��ܵ���ȴʱ��"; PPUPButtonDescribe.DescribeText = "�ü��ܵ���ȴʱ����" + StrengthenPanel.player.GetSkillCD(s).ToString("#0.00") + "������Ϊ" + (StrengthenPanel.player.GetSkillCD(s) * 0.625f).ToString("#0.00") + "��"; }
    }

    public void StrengthenSkill(bool isMS)
    {
        if (isMS)
        {
            if (StrengthenPanel.player.SeedofMastery >= 1 && BlockSkill.SkillFrom != 2)
            {
                StrengthenPanel.BuyASkill(true);
                MSButton.SkillShopBlockPressed();
                StrengthenPanel.player.LearnNewSkillByOtherWay(BlockSkill.PlusSkill);
                SkillBar.GetSkill_Panle(BlockSkill.PlusSkill, StrengthenPanel.player);
            }
        }
        else
        {
            if (StrengthenPanel.player.PPUp >= 1 && !BlockSkill.isPPUP)
            {
                StrengthenPanel.BuyASkill(false);
                PPUPButton.SkillShopBlockPressed();
                BlockSkill.isPPUP = true;
                SkillBar.GetSkill_Panle(BlockSkill, StrengthenPanel.player);
            }
        }
    }

    int GetChildNum()
    {
        int OutPut = 0;
        for (int i = 0; i < transform.parent.childCount; i++)
        {
            if (transform.parent.GetChild(i) == transform) { OutPut = i; }
        }
        return OutPut;
    }
}
