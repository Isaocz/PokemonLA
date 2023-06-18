using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartScale : IteamPickUp
{
    //����һ��������ʾ��֮��Ƭ���ӵ���
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


    //����Ʒ�������ײʱ��������ҵĽű����������Ϊ�գ���Ʒ�����ʰȡ״̬


    private void OnCollisionStay2D(Collision2D other)
    {
        PlayerControler playerControler = other.gameObject.GetComponent<PlayerControler>();
        if (playerControler != null && CanBePickUp)
        {
            Animator animator = targer.GetComponent<Animator>();
            playerControler.ChangeHearsScale(ChangePoint);
            animator.SetTrigger("Eat");
            UIGetANewItem.UI.GetANewItem(ItemTag, ItemName);
            Destroy(gameObject);
        }
    }
}
