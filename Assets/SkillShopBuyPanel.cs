using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillShopBuyPanel : MonoBehaviour
{
    // Start is called before the first frame update

    public SkillShopBuyBlock SkillShopBlockPerfabs;
    public PlayerControler player;
    GameObject CoinNumPanel;
    Text HSText;
    Text PPUPText;
    Text MSText;
    GameObject ViewPanel;
    List<Skill> IsGoodAlready = new List<Skill> { };
    public Vector3 BornPosition;

    void Start()
    {
        ViewPanel = transform.GetChild(1).GetChild(0).GetChild(0).gameObject;
        CoinNumPanel = transform.GetChild(3).gameObject;
        HSText = CoinNumPanel.transform.GetChild(0).GetComponent<Text>();
        PPUPText = CoinNumPanel.transform.GetChild(1).GetComponent<Text>();
        MSText = CoinNumPanel.transform.GetChild(2).GetComponent<Text>();

        InstantiateSkillBlock();

        ChangeCoinNum();
        BornPosition = new Vector3(-3, -5, 0);
    }

    void InstantiateSkillBlock() 
    {
        int BlockNum = 4;
        float r = Random.Range(0.0f , 1.0f);
        SkillShopBuyBlock B = null;
        if (r <= 0.2f) { BlockNum = 3; }
        else if (r >= 0.8) { BlockNum = 5; }
        for(int i = 0; i < BlockNum; )
        {
            Skill s = player.playerSkillList.RandomGetASkillMachine();
            if (IsGoodAlready.Contains(s))
            {
                continue;
            }
            else
            {
                B = Instantiate(SkillShopBlockPerfabs, ViewPanel.transform.position, Quaternion.identity, ViewPanel.transform);
                B.GetSkill(s);
                IsGoodAlready.Add(s);
                i++;
            }

        }

    }

    public void ChangeCoinNum()
    {
        HSText.text = ": " + player.HeartScale.ToString();
        PPUPText.text = ": " + player.PPUp.ToString();
        MSText.text = ": " + player.SeedofMastery.ToString();
    }

    public void BuyASkill(int Price)
    {
        player.ChangeHearsScale(-Price);
        ChangeCoinNum();
    }

}
