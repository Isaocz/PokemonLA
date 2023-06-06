using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExUpRareCandy : IteamPickUp
{

    //����һ��������ʾ����ֵ���ӵ���
    //public int ChangePoint = 10;

    private void FixedUpdate()
    {
        if (!CanBePickUp )
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



    //����Ʒ�������ײʱ��������ҵĽű����������Ϊ�գ���Ʒ�����ʰȡ״̬

    private void OnTriggerEnter2D(Collider2D other)
    {


    }

    private void OnCollisionStay2D(Collision2D other)
    {
        PlayerControler playerControler = other.gameObject.GetComponent<PlayerControler>();
        if (playerControler != null && CanBePickUp)
        {
            Animator animator = targer.GetComponent<Animator>();
            playerControler.ChangeEx(playerControler.maxEx - playerControler.Ex + 1);
            animator.SetTrigger("Eat");
            UIGetANewItem.UI.GetANewItem(ItemTag, ItemName);
            Destroy(gameObject);
        }
    }

}
