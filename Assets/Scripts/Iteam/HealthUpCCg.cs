using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthUpCCg : IteamPickUp
{

    //声明一个变量，代表该道具的回复量
    public int ChangePoint;
    public float ChangePer;

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



    //当物品和玩家碰撞时，捕获玩家的脚本，如果捕获不为空且当前血量小于最大血量，物品进入可拾取状态
    private void OnTriggerEnter2D(Collider2D other)
    {


    }

    private void OnCollisionStay2D(Collision2D other)
    {
        PlayerControler playerControler = other.gameObject.GetComponent<PlayerControler>();
        if (playerControler != null && playerControler.Hp < playerControler.maxHp && CanBePickUp)
        {
            Animator animator = targer.GetComponent<Animator>();
            playerControler.ChangeHp(ChangePoint + playerControler.maxHp * ChangePer, 0, 0);

            //道具28 树果烹饪指南
            if (playerControler.playerData.IsPassiveGetList[28] && ItemTypeTag != null) {
                foreach (int i in ItemTypeTag)
                {
                    Debug.Log(1);
                    if (i == 1) {
                        Pokemon.PokemonHpChange(null, playerControler.gameObject, 0, 0, Mathf.Clamp(playerControler.maxHp / 16, 1, 10), Type.TypeEnum.IgnoreType);
                        //playerControler.ChangeHp(Mathf.Clamp(playerControler.maxHp / 16, 1, 10), 0, 19);
                    }
                }
            }
            //道具100 宝可方块套装
            if (playerControler.playerData.IsPassiveGetList[100] && ItemTypeTag != null)
            {
                foreach (int i in ItemTypeTag)
                {
                    Debug.Log(1);
                    if (i == 1) { playerControler.ChangeHPW(new Vector2Int(Random.Range(1,7),1)); }
                }
            }
            animator.SetTrigger("Eat");
            UIGetANewItem.UI.GetANewItem(ItemTag, ItemName);
            Destroy(gameObject);
        }
    }

}
