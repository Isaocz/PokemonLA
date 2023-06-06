using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpaceItem : IteamPickUp
{
    public Sprite UIImage;
    public int ItemNum;



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


    //当物品和玩家碰撞时，捕获玩家的脚本，如果捕获不为空，物品进入可拾取状态


    private void OnTriggerEnter2D(Collider2D other)
    {

    }

    private void OnCollisionStay2D(Collision2D other)
    {
        PlayerControler playerControler = other.gameObject.GetComponent<PlayerControler>();
        if (playerControler != null && CanBePickUp)
        {
            Animator animator = targer.GetComponent<Animator>();
            if (playerControler.spaceItem != null) { Instantiate(playerControler.spaceItem, transform.position, Quaternion.identity); }
            animator.SetTrigger("Eat");
            playerControler.spaceItem = playerControler.SpaceItemList.transform.GetChild(ItemNum).gameObject;
            playerControler.SpaceItemImage.color = new Color(1, 1, 1, 1);
            playerControler.SpaceItemImage.sprite = UIImage;
            UIGetANewItem.UI.GetANewItem(ItemTag, ItemName);
            Destroy(gameObject);
        }
    }

}
