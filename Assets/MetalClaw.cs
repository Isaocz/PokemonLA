using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetalClaw : Skill
{

    bool isAbllityUp;


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        if (transform.rotation.eulerAngles.z == 0)        { animator.SetFloat("LookX", 1); animator.SetFloat("LookY", 0); }
        else if (transform.rotation.eulerAngles.z == 90)  { animator.SetFloat("LookX", 0); animator.SetFloat("LookY", 1); }
        else if (transform.rotation.eulerAngles.z == 180) { animator.SetFloat("LookX", -1); animator.SetFloat("LookY", 0); }
        else if (transform.rotation.eulerAngles.z == 270) { animator.SetFloat("LookX", 0); animator.SetFloat("LookY", -1); }
        transform.rotation = new Quaternion(0, 0, 0, 0);
    }

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
                if (!isHitDone && !isAbllityUp)
                {
                    isAbllityUp = true;
                    if (Random.Range(0.0f,1.0f) + ((float)player.LuckPoint / 20.0f) >= 0.8f ) {
                        player.playerData.AtkBounsJustOneRoom++;
                        player.ReFreshAbllityPoint();
                    }
                    if (SkillFrom == 2) {
                        if (Random.Range(0.0f, 1.0f) + ((float)player.LuckPoint / 20.0f) >= 0.7f)
                        {
                            if (Random.Range(0.0f , 1.0f) >= 0.5f ) {
                                player.playerData.DefBounsJustOneRoom = Mathf.Clamp( player.playerData.DefBounsJustOneRoom + 1 , -8 , 8);
                                player.ReFreshAbllityPoint();
                            }
                            else
                            {
                                player.playerData.SpDBounsJustOneRoom = Mathf.Clamp(player.playerData.SpDBounsJustOneRoom + 1, -8, 8); ;
                                player.ReFreshAbllityPoint();

                            }
                        }
                    }
                }
                HitAndKo(target);
            }
        }
    }
}
