using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LashOut : Skill
{
    public SubLashOut sub;

    void Start()
    {
        if (player.playerData.AtkBounsJustOneRoom + player.playerData.DefBounsJustOneRoom + player.playerData.SpABounsJustOneRoom + player.playerData.SpDBounsJustOneRoom +
            player.playerData.HPBounsJustOneRoom + player.playerData.SpeBounsJustOneRoom + player.playerData.MoveSpeBounsJustOneRoom + player.playerData.LuckBounsJustOneRoom +
            player.playerData.AtkBounsAlways + player.playerData.DefBounsAlways + player.playerData.SpABounsAlways + player.playerData.SpDBounsAlways +
            player.playerData.HPBounsAlways + player.playerData.SpeBounsAlways + player.playerData.MoveSpwBounsAlways + player.playerData.LuckBounsAlways < 0
            )
        {
            Damage *= 2;
        }
        animator = GetComponent<Animator>();
        if (SkillFrom == 2) {
            sub.BeforeHp = player.Hp;
            player.AddASubSkill(sub);
        }
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
