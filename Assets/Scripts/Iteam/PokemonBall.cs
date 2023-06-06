using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PokemonBall : IteamPickUp
{
    bool isPickUp;
    Animator animator;
    public GameObject RandomMoney;
    public GameObject RandomDropItem;
    bool isOpen;
    public float ItemDropPer;
    public float PassiveDropPer = -1;
    public int PassiveDropIndex = -1;
    public PassiveList passiveList;

    bool isThereArePassiveItem;
    GameObject PassiveItemObj;
    PassiveItem PassiveItem;

    //public float PerMinusPoint;

    private void Start()
    {
        animator = GetComponent<Animator>();
        PassiveItemObj = transform.GetChild(3).gameObject;
    }

    // Start is called before the first frame update
    private void FixedUpdate()
    {
        if (!CanBePickUp)
        {
            if (isLunch)
            {
                LunchItem();
            }
            else
            {
                DoNotLunch();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if ( !isOpen && isPickUp && CanBePickUp)
        {
            if (PassiveDropPer == -1) { PassiveDropPer = 0.1f; }
            if (Random.Range(0.0f, 1.0f) <= (1-PassiveDropPer))
            {
                animator.SetTrigger("Normal");
                PokemonNormalOpen();                
            }
            else
            {
                if (PassiveDropIndex == -1) { PassiveDropIndex = passiveList.GetARandomItemIndex(0); }
                animator.SetTrigger("Item");
                PassiveItemObj.GetComponent<SpriteRenderer>().sprite = passiveList.SpritesList[PassiveDropIndex];
                PassiveItem = passiveList.transform.GetChild(PassiveDropIndex).GetComponent<PassiveItem>();
                isThereArePassiveItem = true;
            }
            isOpen = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        PlayerControler playerControler = other.gameObject.GetComponent<PlayerControler>();
        if (playerControler != null)
        {
            isPickUp = true;
            if (isThereArePassiveItem)
            {
                
                UIGetANewItem.UI.GetANewItem(PassiveItem.ItemTag , PassiveItem.ItemName);
                isThereArePassiveItem = false;
                playerControler.playerData.GetPassiveItem(PassiveItem);
                playerControler.animator.SetTrigger("Happy");
                playerControler.PassiveItemGetUI.GetComponent<Image>().sprite = PassiveItemObj.GetComponent<SpriteRenderer>().sprite;
                Destroy(PassiveItemObj);
            }
        }
    }

    void PokemonNormalOpen()
    {
        Instantiate(RandomMoney, transform.position, Quaternion.identity, transform);
        Instantiate(RandomMoney, transform.position, Quaternion.identity, transform);
        Instantiate(RandomDropItem, transform.position, Quaternion.identity, transform);
        while(Random.Range(0.0f,1.0f) <= ItemDropPer)
        {
            Instantiate(RandomDropItem, transform.position, Quaternion.identity, transform);
        }

        Invoke("LunchChild" , 0.01f);
    }

    void LunchChild()
    {
        List<IteamPickUp> ListRemove = new List<IteamPickUp> { };
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).GetComponent<IteamPickUp>() != null)
            {
                transform.GetChild(i).GetComponent<IteamPickUp>().isLunch = true;
                ListRemove.Add(transform.GetChild(i).GetComponent<IteamPickUp>());
            }
        }
        foreach(IteamPickUp RemoveChild in ListRemove)
        {
            RemoveChild.transform.parent = transform.parent;
        }
    }
}
