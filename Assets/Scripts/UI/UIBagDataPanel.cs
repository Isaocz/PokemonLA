using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBagDataPanel : MonoBehaviour
{
    public static UIBagDataPanel InBagData;
    public Text MoneyText;
    public Text StoneText;
    public UICallDescribe SpaceItemUI;
    public GameObject PassiveItemBlockPanel;
    public UICallDescribe PassiveItemBlock;
    public PassiveList passiveList;
    PlayerControler player;
    int PIIndexListPointer = 0;



    // Start is called before the first frame update
     private void Awake()
    {
        InBagData = this;

    }


    private void OnEnable()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControler>();
        Invoke("RestoreBagDataPanel", 0.0001f);
    }

    void RestoreBagDataPanel()
    {
        Debug.Log(1);
        MoneyText.text = player.Money.ToString();
        StoneText.text = player.Stone.ToString();
        if (player.spaceItem != null) { 
            SpaceItemUI.gameObject.SetActive(true);
            SpaceItemUI.TwoMode = true;
            SpaceItemUI.FirstText = player.spaceItem.GetComponent<Item>().ItemName;
            SpaceItemUI.DescribeText = player.spaceItem.GetComponent<Item>().ItemDescribe;
            SpaceItemUI.GetComponent<Image>().sprite = player.spaceItem.GetComponent<Item>().StoreImage;
        }
        else
        {
            SpaceItemUI.gameObject.SetActive(false);
        }
        PlayerData playerdata = player.playerData;
        for ( ; PIIndexListPointer < playerdata.GetPassiveItemList.Count ; PIIndexListPointer++)
        {
            UICallDescribe Block =  Instantiate(PassiveItemBlock, PassiveItemBlockPanel.transform.position, Quaternion.identity, PassiveItemBlockPanel.transform);
            Block.TwoMode = true;
            Block.FirstText = passiveList.transform.GetChild(playerdata.GetPassiveItemList[PIIndexListPointer]).GetComponent<PassiveItem>().ItemName;
            Block.DescribeText = passiveList.transform.GetChild(playerdata.GetPassiveItemList[PIIndexListPointer]).GetComponent<PassiveItem>().ItemDescribe;
            Block.transform.GetChild(0).GetComponent<Image>().sprite = passiveList.transform.GetChild(playerdata.GetPassiveItemList[PIIndexListPointer]).GetComponent<SpriteRenderer>().sprite;
            Block.DescribeUI = SpaceItemUI.DescribeUI;
        }
    }
}
