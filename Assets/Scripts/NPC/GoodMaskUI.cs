using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    Meowth ParentMeowth;
    PlayerControler playerControler;
    int RefreshCount;
    int RefreshMoney;
    public Text RefreshButtonText;

    // Start is called before the first frame update
    void Start()
    {
        ParentMeowth = transform.parent.parent.parent.parent.parent.parent.GetComponent<Meowth>();
        playerControler = ParentMeowth.playerControler;
        RefreshCount = 0;
        RefreshMoney = 2;
        RefreshButtonText.text = RefreshMoney.ToString();

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
        PassiveItemGoodBlock.Good = PassiveItemList.transform.GetChild(PassiveItemList.GetARandomItemIndex(0 , PassiveItemPool.Store )).GetComponent<Item>();
        PassiveItemGoodBlock.GoodsDescribeUI = uIDescribe;

        if (Random.Range(0.0f, 1.0f) >= 0.75f)
        {
            GoodBlock PassiveItemGoodBlock2 = Instantiate(goodBlock, transform.position, Quaternion.identity, transform);
            PassiveItemGoodBlock2.Good = PassiveItemList.transform.GetChild(PassiveItemList.GetARandomItemIndex(0 , PassiveItemPool.Store)).GetComponent<Item>();
            PassiveItemGoodBlock2.GoodsDescribeUI = uIDescribe;
        }
    }

    public void ResetPanel()
    {
        if (ParentMeowth == null) { ParentMeowth = transform.parent.parent.parent.parent.parent.parent.GetComponent<Meowth>(); }
        if (playerControler == null) { playerControler = ParentMeowth.playerControler; }


        if (playerControler.Money >= RefreshMoney) {

            playerControler.ChangeMoney(-RefreshMoney);


            for (int i = 0; i < transform.childCount; i++)
            {
                Destroy(transform.GetChild(i).gameObject);
            }

            if (Random.Range(0.0f, 1.0f) - RefreshMoney*0.04f >= 0.0f)
            {
                GoodBlock SharpStoneGoodBlock = Instantiate(goodBlock, transform.position, Quaternion.identity, transform);
                SharpStoneGoodBlock.Good = SharpStone;
                SharpStoneGoodBlock.GoodsDescribeUI = uIDescribe;
            }

            if (Random.Range(0.0f, 1.0f) - RefreshMoney * 0.04f >= 0.0f)
            {
                GoodBlock PokemonBallGoodBlock = Instantiate(goodBlock, transform.position, Quaternion.identity, transform);
                PokemonBallGoodBlock.Good = PokemonBall;
                PokemonBallGoodBlock.GoodsDescribeUI = uIDescribe;
            }

            if (Random.Range(0.0f, 1.0f) - RefreshMoney * 0.02f >= 0.0f)
            {
                GoodBlock ExpCandyGoodBlock = Instantiate(goodBlock, transform.position, Quaternion.identity, transform);
                ExpCandyGoodBlock.Good = ExpCandyList.transform.GetChild(Random.Range(0, ExpCandyList.transform.childCount)).GetComponent<Item>();
                ExpCandyGoodBlock.GoodsDescribeUI = uIDescribe;
            }

            if (Random.Range(0.0f, 1.0f) - RefreshMoney * 0.02f >= 0.0f)
            {
                GoodBlock PotionGoodBlock = Instantiate(goodBlock, transform.position, Quaternion.identity, transform);
                PotionGoodBlock.Good = PotionList.transform.GetChild(Random.Range(0, PotionList.transform.childCount)).GetComponent<Item>();
                PotionGoodBlock.GoodsDescribeUI = uIDescribe;
            }

            if (Random.Range(0.0f, 1.0f) - RefreshMoney * 0.02f >= 0.0f)
            {
                GoodBlock StateHealGoodBlock = Instantiate(goodBlock, transform.position, Quaternion.identity, transform);
                StateHealGoodBlock.Good = StateHealyList.transform.GetChild(Random.Range(0, StateHealyList.transform.childCount)).GetComponent<Item>();
                StateHealGoodBlock.GoodsDescribeUI = uIDescribe;
            }

            if (Random.Range(0.0f, 1.0f) - RefreshMoney * 0.02f >= 0.0f)
            {
                GoodBlock FeatherGoodBlock = Instantiate(goodBlock, transform.position, Quaternion.identity, transform);
                FeatherGoodBlock.Good = FeatherList.transform.GetChild(Random.Range(0, FeatherList.transform.childCount)).GetComponent<Item>();
                FeatherGoodBlock.GoodsDescribeUI = uIDescribe;
            }

            if (Random.Range(0.0f, 1.0f) - RefreshMoney * 0.02f >= 0.1f)
            {
                GoodBlock TypeDefBerryGoodBlock = Instantiate(goodBlock, transform.position, Quaternion.identity, transform);
                TypeDefBerryGoodBlock.Good = TypeDefBerryList.transform.GetChild(Random.Range(0, TypeDefBerryList.transform.childCount)).GetComponent<Item>();
                TypeDefBerryGoodBlock.GoodsDescribeUI = uIDescribe;
            }

            if (Random.Range(0.0f, 1.0f) - RefreshMoney * 0.02f >= 0.1f)
            {
                GoodBlock TerashardGoodBlock = Instantiate(goodBlock, transform.position, Quaternion.identity, transform);
                TerashardGoodBlock.Good = TeraShadeList.transform.GetChild(Random.Range(0, TeraShadeList.transform.childCount)).GetComponent<Item>();
                TerashardGoodBlock.GoodsDescribeUI = uIDescribe;
            }

            if (Random.Range(0.0f, 1.0f) - RefreshMoney * 0.04f >= 0.25f)
            {
                GoodBlock XAbllityGoodBlock = Instantiate(goodBlock, transform.position, Quaternion.identity, transform);
                XAbllityGoodBlock.Good = XAbllityList.transform.GetChild(Random.Range(0, XAbllityList.transform.childCount)).GetComponent<Item>();
                XAbllityGoodBlock.GoodsDescribeUI = uIDescribe;
            }

            if (Random.Range(0.0f, 1.0f) - RefreshMoney * 0.04f >= 0.35f)
            {
                GoodBlock SpaceItemGoodBlock = Instantiate(goodBlock, transform.position, Quaternion.identity, transform);
                SpaceItemGoodBlock.Good = SpaceItemList.transform.GetChild(Random.Range(0, SpaceItemList.transform.childCount)).GetComponent<Item>();
                SpaceItemGoodBlock.GoodsDescribeUI = uIDescribe;
            }

            if (Random.Range(0.0f, 1.0f) - RefreshMoney * 0.03f >= 0.0f)
            {
                GoodBlock PassiveItemGoodBlock = Instantiate(goodBlock, transform.position, Quaternion.identity, transform);
                PassiveItemGoodBlock.Good = PassiveItemList.transform.GetChild(PassiveItemList.GetARandomItemIndex(0, PassiveItemPool.Store)).GetComponent<Item>();
                PassiveItemGoodBlock.GoodsDescribeUI = uIDescribe;
            }

            if (Random.Range(0.0f, 1.0f) - RefreshMoney * 0.04f >= 0.75f)
            {
                GoodBlock PassiveItemGoodBlock2 = Instantiate(goodBlock, transform.position, Quaternion.identity, transform);
                PassiveItemGoodBlock2.Good = PassiveItemList.transform.GetChild(PassiveItemList.GetARandomItemIndex(0, PassiveItemPool.Store)).GetComponent<Item>();
                PassiveItemGoodBlock2.GoodsDescribeUI = uIDescribe;
            }


            RefreshCount++;
            RefreshMoney = (int)Mathf.Pow(2, Mathf.Clamp(RefreshCount + 1, 1, 4));
            RefreshButtonText.text = RefreshMoney.ToString();

        }
    }

}
