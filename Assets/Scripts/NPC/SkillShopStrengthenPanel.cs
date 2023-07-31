using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillShopStrengthenPanel : MonoBehaviour
{
    // Start is called before the first frame update

    public SkillShopStrengthenBlock SkillShopBlockPerfabs;
    public PlayerControler player;
    GameObject CoinNumPanel;
    Text HSText;
    Text PPUPText;
    Text MSText;
    GameObject ViewPanel;

    Skill s1;
    Skill s2;
    Skill s3;
    Skill s4;
    SkillShopStrengthenBlock B1;
    SkillShopStrengthenBlock B2;
    SkillShopStrengthenBlock B3;
    SkillShopStrengthenBlock B4;



    private void OnEnable()
    {
        ViewPanel = transform.GetChild(1).GetChild(0).GetChild(0).gameObject;
        CoinNumPanel = transform.GetChild(3).gameObject;
        HSText = CoinNumPanel.transform.GetChild(0).GetComponent<Text>();
        PPUPText = CoinNumPanel.transform.GetChild(1).GetComponent<Text>();
        MSText = CoinNumPanel.transform.GetChild(2).GetComponent<Text>();
        InstantiateSkillBlock();
        ChangeCoinNum();
    }

    void InstantiateSkillBlock()
    {
        if (player.Skill01 != null && player.Skill01 != s1)        
        {
            if(B1 != null) Destroy(B1.gameObject);
            B1 = Instantiate(SkillShopBlockPerfabs, ViewPanel.transform.position, Quaternion.identity, ViewPanel.transform);  
            B1.GetSkill(player.Skill01);
            s1 = player.Skill01;
        }
        if (player.Skill02 != null && player.Skill02 != s2)
        {
            if (B2 != null) Destroy(B2.gameObject);
            B2 = Instantiate(SkillShopBlockPerfabs, ViewPanel.transform.position, Quaternion.identity, ViewPanel.transform);
            B2.GetSkill(player.Skill02);
            s2 = player.Skill02;
        }
        if (player.Skill03 != null && player.Skill03 != s3)
        {
            if (B3 != null) Destroy(B3.gameObject);
            B3 = Instantiate(SkillShopBlockPerfabs, ViewPanel.transform.position, Quaternion.identity, ViewPanel.transform);
            B3.GetSkill(player.Skill03);
            s3 = player.Skill03;
        }
        if (player.Skill04 != null && player.Skill04 != s4)
        {
            if (B4 != null) Destroy(B4.gameObject);
            B4 = Instantiate(SkillShopBlockPerfabs, ViewPanel.transform.position, Quaternion.identity, ViewPanel.transform);
            B4.GetSkill(player.Skill04);
            s4 = player.Skill04;
        }
    }

    public void ChangeCoinNum()
    {
        HSText.text = ": " + player.HeartScale.ToString();
        PPUPText.text = ": " + player.PPUp.ToString();
        MSText.text = ": " + player.SeedofMastery.ToString();
    }

    public void BuyASkill(bool isMS)
    {
        if (isMS) { player.ChangeMSeed(-1); }
        else { player.ChangePPUp(-1); }
        ChangeCoinNum();
    }

}
