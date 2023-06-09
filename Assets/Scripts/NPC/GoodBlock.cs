using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoodBlock : MonoBehaviour
{
    public Item Good;
    public PokemonBall PassiveItemPokemonBall;
    PlayerControler playerControler;
    Image GoodImage;
    Text GoodPrice;
    Meowth ParentMeowth;
    public UIDescribe GoodsDescribeUI;


    void Start()
    {
        
        ParentMeowth = transform.parent.parent.parent.parent.parent.parent.parent.GetComponent<Meowth>();
        playerControler = ParentMeowth.playerControler;
        GoodImage = transform.GetChild(4).GetComponent<Image>();
        GoodPrice = transform.GetChild(6).GetComponent<Text>();
        GoodPrice.text = Good.Price.ToString();
        GoodImage.sprite = Good.StoreImage;
    }

    public void MouseEnter()
    {
        GoodsDescribeUI.MoveDescribe();
        GoodsDescribeUI.GetDescribeString(Good.ItemDescribe,"",false);
        GoodsDescribeUI.gameObject.SetActive(true);
    }
    public void MouseExit()
    {
        GoodsDescribeUI.MoveDescribe();
        GoodsDescribeUI.gameObject.SetActive(false);
    }

    public void BuyGoods()
    {
        if(Good.Price <= playerControler.Money)
        {
            playerControler.ChangeMoney(-Good.Price);
            if (Good.ItemTag != 2)
            {
                Instantiate(Good, ParentMeowth.transform.position + ParentMeowth.GoodInstancePlace, Quaternion.identity, ParentMeowth.transform.parent);
            }
            else
            {
                PokemonBall DropBall =  Instantiate(PassiveItemPokemonBall, ParentMeowth.transform.position + ParentMeowth.GoodInstancePlace, Quaternion.identity, ParentMeowth.transform.parent);
                DropBall.PassiveDropPer = 1;
                DropBall.PassiveDropIndex = Good.GetComponent<PassiveItem>().PassiveItemIndex;
            }
            ParentMeowth.GoodInstancePlace += Vector3.right;
            if (ParentMeowth.GoodInstancePlace.x >= 2)
            {
                ParentMeowth.GoodInstancePlace.x = -1;
                ParentMeowth.GoodInstancePlace.y -= 1;
            }
            gameObject.GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, 1);
            gameObject.transform.GetChild(4).GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, 1);
            gameObject.transform.GetChild(5).GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, 1);
            gameObject.transform.GetChild(6).GetComponent<Text>().color = new Color(0.2f, 0.2f, 0.2f, 1);
            gameObject.transform.GetChild(7).GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, 1);
            gameObject.transform.GetChild(7).GetComponent<Button>().interactable = false;
            gameObject.transform.GetChild(8).gameObject.SetActive(true); 
            gameObject.transform.GetChild(9).gameObject.SetActive(true);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
