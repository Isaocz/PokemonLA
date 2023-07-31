using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IronHead : Skill
{
    GameObject HitEffect;

    //初始化动画组件和撞击造成的伤害
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        HitEffect = transform.GetChild(0).gameObject;
        HitEffect.transform.rotation = Quaternion.Euler(Vector3.zero);
    }


    //每帧减少飞弹的存在时间，当存在时间等于0时销毁飞弹
    // Update is called once per frame
    void Update()
    {
        StartExistenceTimer();
    }


    bool isBounsDone;
    //当飞弹与目标碰撞时，如果目标时敌人，获取敌人的血量，并使敌人扣血
    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.tag == "Empty")
        {
            Empty target = other.GetComponent<Empty>();
            if (target != null)
            {
                HitAndKo(target);
                if(SkillFrom == 2 && target.EmptyHp <= 0 && !isBounsDone)
                {
                    isBounsDone = true;
                    if (Random.Range(0.0f, 1.0f) >= 0.5f && player.playerData.DefBounsJustOneRoom <= 8) {
                        player.playerData.DefBounsJustOneRoom++;
                    }
                    else {
                        if (player.playerData.SpDBounsJustOneRoom <= 8) { player.playerData.SpDBounsJustOneRoom++; }
                    }
                    player.ReFreshAbllityPoint();
                }
                HitEffect.transform.parent = target.transform;
                HitEffect.SetActive(true);
                if (animator != null) { animator.SetTrigger("Hit"); }
            }
        }
    }
}
