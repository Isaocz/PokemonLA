using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillShopBuyBlock : MonoBehaviour
{
    public SkillBall skillBall;
    GameObject ParentGulpin;
    SkillShopBuyPanel BuyPanel;
    UIPanleSkillBar SkillBar;
    SkillShopBlockBuyButton BuyBotton;
    Skill BlockSkill;
    int SkillPrice;


    private void Awake()
    {
        SkillBar = transform.GetChild(0).GetComponent<UIPanleSkillBar>();
        BuyBotton = transform.GetChild(1).GetComponent<SkillShopBlockBuyButton>();
        BuyPanel = transform.parent.parent.parent.parent.GetComponent<SkillShopBuyPanel>();

    }
    private void Start()
    {
        
        ParentGulpin = transform.parent.parent.parent.parent.parent.parent.gameObject;
    }

    public void GetSkill(Skill s)
    {
        BlockSkill = s;
        SkillBar.GetSkill_Panle(BlockSkill , BuyPanel.player);
        if (BlockSkill.SkillQualityLevel <= 2) { SkillPrice = 1; }
        else if (BlockSkill.SkillQualityLevel > 2 && BlockSkill.SkillQualityLevel <= 4) { SkillPrice = 2; }
        if (BlockSkill.SkillQualityLevel > 4) { SkillPrice = 3; }
        BuyBotton.GetSkillPrice(SkillPrice);
    }

    public void BuySkill()
    {
        if (BuyPanel.player.HeartScale >= SkillPrice) {
            SkillBall GoodsBall = Instantiate(skillBall, ParentGulpin.transform.position + BuyPanel.BornPosition, Quaternion.identity);
            BuyPanel.BornPosition.x += 2;
            if (BuyPanel.BornPosition.x > 5) { BuyPanel.BornPosition.x = -3f; BuyPanel.BornPosition.y -= 1.5f; }
            GoodsBall.GetSkill = BlockSkill;
            BuyPanel.BuyASkill(SkillPrice);
            BuyBotton.SkillShopBlockPressed();
        }
    }
    
}
