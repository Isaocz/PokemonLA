using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerTrip : Skill
{
    //��ʼ�����������ײ����ɵ��˺�
    // Start is called before the first frame update
    void Start()
    {
        int count = player.playerData.AtkBounsAlways +player.playerData.AtkBounsJustOneRoom
                  + player.playerData.DefBounsAlways + player.playerData.DefBounsJustOneRoom
                  + player.playerData.SpABounsAlways + player.playerData.SpABounsJustOneRoom
                  + player.playerData.SpDBounsAlways + player.playerData.SpDBounsJustOneRoom
                  + player.playerData.SpeBounsAlways + player.playerData.SpeBounsJustOneRoom
                  + player.playerData.HPBounsAlways + player.playerData.HPBounsJustOneRoom
                  + player.playerData.LuckBounsAlways + player.playerData.LuckBounsJustOneRoom
                  + player.playerData.MoveSpwBounsAlways + player.playerData.MoveSpeBounsJustOneRoom;

        int Improve = 20;
        float PlayerHPPer = ((float)(player.Hp)) / ((float)(player.maxHp));
        if (PlayerHPPer >= 0.3542f && PlayerHPPer < 0.6875f)      { Improve = 22; }
        else if (PlayerHPPer >= 0.2083f && PlayerHPPer < 0.3542f) { Improve = 24; }
        else if (PlayerHPPer >= 0.1042f && PlayerHPPer < 0.2083f) { Improve = 26; }
        else if (PlayerHPPer >= 0.0417f && PlayerHPPer < 0.1042f) { Improve = 28; }
        else if (PlayerHPPer < 0.0417f)                           { Improve = 30; ; }

        Damage += Improve + 1 + count * Improve;
        animator = GetComponent<Animator>();

    }


    //ÿ֡���ٷɵ��Ĵ���ʱ�䣬������ʱ�����0ʱ���ٷɵ�
    // Update is called once per frame
    void Update()
    {
        StartExistenceTimer();
    }


    //���ɵ���Ŀ����ײʱ�����Ŀ��ʱ���ˣ���ȡ���˵�Ѫ������ʹ���˿�Ѫ
    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.tag == "Empty")
        {
            Empty target = other.GetComponent<Empty>();
            if (target != null)
            {
                HitAndKo(target);
                if (animator != null) { animator.SetTrigger("Hit"); }
            }
        }

    }
}
