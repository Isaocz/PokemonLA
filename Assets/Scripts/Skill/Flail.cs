using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flail : Skill
{
    bool IsTriggerDone = false;
    public GameObject FlailWater;

    //初始化动画组件和撞击造成的伤害
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        player = GameObject.FindWithTag("Player").GetComponent<PlayerControler>();
        Instantiate(FlailWater, player.transform.position + Vector3.up*0.5f, Quaternion.identity , player.transform);
    }


    //每帧减少飞弹的存在时间，当存在时间等于0时销毁飞弹
    // Update is called once per frame
    void Update()
    {
        if (!IsTriggerDone) { StartExistenceTimer(); }
    }

    void FlailDamage(float PlayerHPPer)
    {
        if      (PlayerHPPer >= 0.6875f) { Damage = 20; }
        else if (PlayerHPPer >= 0.3542f && PlayerHPPer < 0.6875f) { Damage = 40; }
        else if (PlayerHPPer >= 0.2083f && PlayerHPPer < 0.3542f) { Damage = 80; }
        else if (PlayerHPPer >= 0.1042f && PlayerHPPer < 0.2083f) { Damage = 100; }
        else if (PlayerHPPer >= 0.0417f && PlayerHPPer < 0.1042f) { Damage = 150; }
        else if (PlayerHPPer < 0.0417f) { Damage = 200; }



    }

    //当飞弹与目标碰撞时，如果目标时敌人，获取敌人的血量，并使敌人扣血
    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.tag == "Empty")
        {
            IsTriggerDone = true;
            Empty target = other.GetComponent<Empty>();
            FlailDamage(((float)(player.Hp)) / ((float)(player.maxHp)));
            Debug.Log(((float)(player.Hp)) / ((float)(player.maxHp)));
            HitAndKo(target);
            if (animator != null) { animator.SetTrigger("Hit"); }
            gameObject.transform.position = other.transform.position;
        }
    }
}
