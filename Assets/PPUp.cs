using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PPUp : IteamPickUp
{
    //声明一个变量表示PPUp增加点数
    public int ChangePoint = 1;

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


    private void OnCollisionStay2D(Collision2D other)
    {
        PlayerControler playerControler = other.gameObject.GetComponent<PlayerControler>();
        if (playerControler != null && CanBePickUp)
        {
            Animator animator = targer.GetComponent<Animator>();
            playerControler.ChangePPUp(ChangePoint);
            animator.SetTrigger("Eat");
            UIGetANewItem.UI.GetANewItem(ItemTag, ItemName);
            Destroy(gameObject);
        }
    }
}
