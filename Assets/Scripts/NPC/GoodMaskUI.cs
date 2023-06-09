using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoodMaskUI : MonoBehaviour
{
    public GoodBlock goodBlock;
    public UIDescribe uIDescribe;

    public Item SharpStone;
    public Item PokemonBall;
    public GameObject ExpCandyList;
    public GameObject PotionList;
    public GameObject StateHealyList;
    public GameObject FeatherList;

    public GameObject TypeDefBerryList; 
    public GameObject TeraShadeList;
    public GameObject XAbllityList;
    public GameObject SpaceItemList;
    public PassiveList PassiveItemList;

    // Start is called before the first frame update
    void Start()
    {
        GoodBlock SharpStoneGoodBlock = Instantiate(goodBlock , transform.position , Quaternion.identity , transform);
        SharpStoneGoodBlock.Good = SharpStone;
        SharpStoneGoodBlock.GoodsDescribeUI = uIDescribe;

        GoodBlock PokemonBallGoodBlock = Instantiate(goodBlock, transform.position, Quaternion.identity, transform);
        PokemonBallGoodBlock.Good = PokemonBall;
        PokemonBallGoodBlock.GoodsDescribeUI = uIDescribe;

        GoodBlock ExpCandyGoodBlock = Instantiate(goodBlock, transform.position, Quaternion.identity, transform);
        ExpCandyGoodBlock.Good = ExpCandyList.transform.GetChild(Random.Range(0,ExpCandyList.transform.childCount)).GetComponent<Item>();
        ExpCandyGoodBlock.GoodsDescribeUI = uIDescribe;

        GoodBlock PotionGoodBlock = Instantiate(goodBlock, transform.position, Quaternion.identity, transform);
        PotionGoodBlock.Good = PotionList.transform.GetChild(Random.Range(0, PotionList.transform.childCount)).GetComponent<Item>();
        PotionGoodBlock.GoodsDescribeUI = uIDescribe;

        GoodBlock StateHealGoodBlock = Instantiate(goodBlock, transform.position, Quaternion.identity, transform);
        StateHealGoodBlock.Good = StateHealyList.transform.GetChild(Random.Range(0, StateHealyList.transform.childCount)).GetComponent<Item>();
        StateHealGoodBlock.GoodsDescribeUI = uIDescribe;

        GoodBlock FeatherGoodBlock = Instantiate(goodBlock, transform.position, Quaternion.identity, transform);
        FeatherGoodBlock.Good = FeatherList.transform.GetChild(Random.Range(0, FeatherList.transform.childCount)).GetComponent<Item>();
        FeatherGoodBlock.GoodsDescribeUI = uIDescribe;

        if (Random.Range(0.0f,1.0f) >= 0.1f)
        {
            GoodBlock TypeDefBerryGoodBlock = Instantiate(goodBlock, transform.position, Quaternion.identity, transform);
            TypeDefBerryGoodBlock.Good = TypeDefBerryList.transform.GetChild(Random.Range(0, TypeDefBerryList.transform.childCount)).GetComponent<Item>();
            TypeDefBerryGoodBlock.GoodsDescribeUI = uIDescribe;
        }

        if (Random.Range(0.0f, 1.0f) >= 0.1f)
        {
            GoodBlock TerashardGoodBlock = Instantiate(goodBlock, transform.position, Quaternion.identity, transform);
            TerashardGoodBlock.Good = TeraShadeList.transform.GetChild(Random.Range(0, TeraShadeList.transform.childCount)).GetComponent<Item>();
            TerashardGoodBlock.GoodsDescribeUI = uIDescribe;
        }

        if (Random.Range(0.0f, 1.0f) >= 0.25f)
        {
            GoodBlock XAbllityGoodBlock = Instantiate(goodBlock, transform.position, Quaternion.identity, transform);
            XAbllityGoodBlock.Good = XAbllityList.transform.GetChild(Random.Range(0, XAbllityList.transform.childCount)).GetComponent<Item>();
            XAbllityGoodBlock.GoodsDescribeUI = uIDescribe;
        }

        if (Random.Range(0.0f, 1.0f) >= 0.35f)
        {
            GoodBlock SpaceItemGoodBlock = Instantiate(goodBlock, transform.position, Quaternion.identity, transform);
            SpaceItemGoodBlock.Good = SpaceItemList.transform.GetChild(Random.Range(0, SpaceItemList.transform.childCount)).GetComponent<Item>();
            SpaceItemGoodBlock.GoodsDescribeUI = uIDescribe;
        }

        GoodBlock PassiveItemGoodBlock = Instantiate(goodBlock, transform.position, Quaternion.identity, transform);
        PassiveItemGoodBlock.Good = PassiveItemList.transform.GetChild(PassiveItemList.GetARandomItemIndex(0)).GetComponent<Item>();
        PassiveItemGoodBlock.GoodsDescribeUI = uIDescribe;

        if (Random.Range(0.0f, 1.0f) >= 0.75f)
        {
            GoodBlock PassiveItemGoodBlock2 = Instantiate(goodBlock, transform.position, Quaternion.identity, transform);
            PassiveItemGoodBlock2.Good = PassiveItemList.transform.GetChild(PassiveItemList.GetARandomItemIndex(0)).GetComponent<Item>();
            PassiveItemGoodBlock2.GoodsDescribeUI = uIDescribe;
        }
    }

}
