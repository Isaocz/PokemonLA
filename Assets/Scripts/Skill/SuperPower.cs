using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperPower : Skill
{
    GameObject EndurePS;
    GameObject HitPS;
    bool isAbllityUODone;


    //初始化动画组件和撞击造成的伤害
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        EndurePS = transform.GetChild(0).gameObject;
        HitPS = transform.GetChild(1).gameObject;
        EndurePS.transform.rotation = Quaternion.Euler(Vector3.zero);
        HitPS.transform.rotation = Quaternion.Euler(Vector3.zero);
        EndurePS.transform.parent = player.transform;
        EndurePS.transform.localPosition = Vector3.zero;
        EndurePS.SetActive(true);
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
            if (target != null) {
                
                if (animator != null) { animator.SetTrigger("Hit"); }
                HitPS.transform.parent = target.transform.parent;
                HitPS.transform.position = target.transform.position;
                HitPS.SetActive(true);

                if(SkillFrom == 2 && (player.playerData.AtkBounsJustOneRoom + player.playerData.AtkBounsAlways) > 0)
                {
                    Debug.Log("SuperPowerPlus");
                    int N = player.playerData.AtkBounsJustOneRoom + player.playerData.AtkBounsAlways;
                    player.playerData.AtkBounsJustOneRoom -= N;
                    player.playerData.DefBounsJustOneRoom -= N;
                    player.ReFreshAbllityPoint();
                    CTLevel += N;
                    CTDamage += N;
                    HitAndKo(target);
                    
                }
                else
                {
                    HitAndKo(target);
                }

                if (!isAbllityUODone)
                {
                    isAbllityUODone = true;
                    if (player.playerData.AtkBounsJustOneRoom >= -8) player.playerData.AtkBounsJustOneRoom -= 1;
                    if (player.playerData.DefBounsJustOneRoom >= -8) player.playerData.DefBounsJustOneRoom -= 1;
                    player.ReFreshAbllityPoint();
                }
            }
        }
    }
}
