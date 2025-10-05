using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayRough : Skill
{
    //初始化动画组件和撞击造成的伤害
    // Start is called before the first frame update
    void Start()
    {
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
            HitAndKo(target);
            if (SkillFrom == 2)
            {
                if (Random.Range(0.0f, 1.0f) + (float)player.LuckPoint / 30 < 0.8f) { player.ButterflyManger.BornABF(FairyButterfly.ButterflyType.红色慢速攻击型); }
                else { player.ButterflyManger.BornABF(FairyButterfly.ButterflyType.橙色增加攻击型); }
                if (Random.Range(0.0f, 1.0f) + (float)player.LuckPoint / 30 > 0.8f)
                {
                    if (Random.Range(0.0f, 1.0f) + (float)player.LuckPoint / 30 < 0.8f) { player.ButterflyManger.BornABF(FairyButterfly.ButterflyType.红色慢速攻击型); }
                    else { player.ButterflyManger.BornABF(FairyButterfly.ButterflyType.橙色增加攻击型); }
                }
            }
            if (animator != null) { animator.SetTrigger("Hit"); }
            if (transform.childCount != 0)
            {
                transform.GetChild(0).gameObject.SetActive(true);
                transform.rotation = Quaternion.Euler(0, 0, 0);
                transform.localScale = player.PlayerLocalScal;
                transform.DetachChildren();
            }
            if (Random.Range(0.0f, 1.0f) + (float)player.LuckPoint / 30 >= 0.9f)
            {
                target.AtkChange(-1, 0.0f);
            }
        }
    }
}
