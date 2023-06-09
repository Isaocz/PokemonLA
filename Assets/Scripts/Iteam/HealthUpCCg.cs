using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthUpCCg : IteamPickUp
{

    //����һ������������õ��ߵĻظ���
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



    //����Ʒ�������ײʱ��������ҵĽű����������Ϊ���ҵ�ǰѪ��С�����Ѫ������Ʒ�����ʰȡ״̬
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
            if (playerControler.playerData.IsPassiveGetList[28] && ItemTypeTag != null) {
                foreach (int i in ItemTypeTag)
                {
                    Debug.Log(1);
                    if (i == 1) { playerControler.ChangeHp(Mathf.Clamp(playerControler.maxHp / 16, 1, 10), 0, 19); }
            }
            }
            animator.SetTrigger("Eat");
            UIGetANewItem.UI.GetANewItem(ItemTag, ItemName);
            Destroy(gameObject);
        }
    }

}
