using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerTrip : Skill
{
    //初始化动画组件和撞击造成的伤害
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


    //每帧减少飞弹的存在时间，当存在时间等于0时销毁飞弹
    // Update is called once per frame
    void Update()
    {
        StartExistenceTimer();
    }


    //当飞弹与目标碰撞时，如果目标时敌人，获取敌人的血量，并使敌人扣血
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
