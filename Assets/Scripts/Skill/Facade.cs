using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Facade : Skill
{
    bool IsTriggerDone = false;
    public GameObject FlailWater;

    //初始化动画组件和撞击造成的伤害
    // Start is called before the first frame update
    void Start()
    {
        if(SkillFrom == 2)
        {
            if (player.isSleepStart || player.isToxicStart || player.isParalysisStart || (player.isBurnStart && !player.isBurnDone))
            {
                Damage *= 2;
            }
            if (player.isBurnDone)
            {
                Damage *= 4;
            }
        }
        else
        {
            if(player.isToxicDone || player.isParalysisDone)
            {
                Damage *= 2;
            }
            if (player.isBurnDone)
            {
                Damage *= 4;
            }
        }
        animator = GetComponent<Animator>();
        Instantiate(FlailWater, player.transform.position + Vector3.up * 0.5f, Quaternion.identity, player.transform);
    }


    //每帧减少飞弹的存在时间，当存在时间等于0时销毁飞弹
    // Update is called once per frame
    void Update()
    {
        if (!IsTriggerDone) { StartExistenceTimer(); }
    }


    //当飞弹与目标碰撞时，如果目标时敌人，获取敌人的血量，并使敌人扣血
    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.tag == "Empty")
        {
            IsTriggerDone = true;
            Empty target = other.GetComponent<Empty>();
            HitAndKo(target);
            if (animator != null) { animator.SetTrigger("Hit"); }
            gameObject.transform.position = other.transform.position;
        }
    }

}
