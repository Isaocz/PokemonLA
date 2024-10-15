using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Payback : Skill
{

    SpriteRenderer PBprite;
    Collider2D PBCollider;
    bool isSkill01;
    bool isSkill02;

    int HPBefore;
    int HPAfter;

    //初始化动画组件和撞击造成的伤害
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        PBprite = GetComponent<SpriteRenderer>();
        PBCollider = GetComponent<Collider2D>();
        animator.enabled = false;
        PBprite.enabled = false;
        PBCollider.enabled = false;

        GameObject ps = transform.GetChild(0).gameObject;
        ps.transform.parent = transform.parent;
        ps.transform.localPosition = Vector3.zero;
        ps.transform.rotation = Quaternion.Euler(0, 0, 0);
        ps.transform.localScale = new Vector3(1,1,1);

        HPBefore = player.Hp;
    }


    //每帧减少飞弹的存在时间，当存在时间等于0时销毁飞弹
    // Update is called once per frame
    void Update()
    {
        StartExistenceTimer();
        if (ExistenceTime < 0.35f)
        {
            if (!isSkill01)
            {
                isSkill01 = true;
                player.animator.SetTrigger("Skill");
            }

        }

        if (ExistenceTime < 0.25f)
        {
            if (!isSkill02)
            {
                HPAfter = player.Hp;
                isSkill02 = true;
                animator.enabled = true;
                PBprite.enabled = true;
                PBCollider.enabled = true;
            }

        }
    }


    //当飞弹与目标碰撞时，如果目标时敌人，获取敌人的血量，并使敌人扣血
    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.tag == "Empty")
        {
            Empty target = other.GetComponent<Empty>();
            if (target != null)
            {
                if (HPAfter < HPBefore) { Damage *= 2; }
                HitAndKo(target);
                if (animator != null) { animator.SetTrigger("Hit"); }
                if (SkillFrom == 2) { Pokemon.PokemonHpChange(null, player.gameObject, 0, 0, HPBefore - HPAfter, PokemonType.TypeEnum.IgnoreType); }
            }
        }

    }
}
