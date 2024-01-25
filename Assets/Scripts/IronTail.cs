using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IronTail : Skill
{
    GameObject HitEffect;
    Vector3 Direction;

    //初始化动画组件和撞击造成的伤害
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        HitEffect = transform.GetChild(0).gameObject;
        HitEffect.transform.rotation = Quaternion.Euler(Vector3.zero);

        transform.rotation = Quaternion.AngleAxis(180,Vector3.forward) * transform.rotation;
        Direction = Quaternion.AngleAxis(transform.rotation.eulerAngles.z , Vector3.forward) * Vector3.right;
        transform.position = player.transform.position + (Vector3.up * 0.4f) + (Direction * DirctionDistance) + new Vector3(Direction.x + player.SkillOffsetforBodySize[1] , Direction.y + player.SkillOffsetforBodySize[2], 0) ;
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
                if (SkillFrom == 2)
                {
                    if (Random.Range(0.0f , 1.0f) > 0.5f)
                    {
                        if (player.playerData.DefBounsAlways + player.playerData.DefBounsJustOneRoom > 0)
                        {
                            player.playerData.DefBounsJustOneRoom -= 1;
                            player.ReFreshAbllityPoint();
                            Damage += 50;
                        }
                        else
                        {
                            if (player.playerData.SpDBounsAlways + player.playerData.SpDBounsJustOneRoom > 0)
                            {
                                player.playerData.SpDBounsJustOneRoom -= 1;
                                player.ReFreshAbllityPoint();
                                Damage += 50;
                            }
                        }
                    }
                    else
                    {
                        if (player.playerData.SpDBounsAlways + player.playerData.SpDBounsJustOneRoom > 0)
                        {
                            player.playerData.SpDBounsJustOneRoom -= 1;
                            player.ReFreshAbllityPoint();
                            Damage += 50;
                        }
                        else
                        {
                            if (player.playerData.DefBounsAlways + player.playerData.DefBounsJustOneRoom > 0)
                            {
                                player.playerData.DefBounsJustOneRoom -= 1;
                                player.ReFreshAbllityPoint();
                                Damage += 50;
                            }
                        }
                    }
                }

                HitAndKo(target);
                if (Random.Range(0.0f, 1.0f) + (float)player.LuckPoint / 30 >= 0.7f)
                {
                    target.DefChange(-1, 0.0f);
                }
                HitEffect.transform.parent = null;
                HitEffect.SetActive(true);
                HitEffect.transform.localScale = new Vector3(1,1,1);
                if (animator != null) { animator.SetTrigger("Hit"); }
            }
        }
    }
}
