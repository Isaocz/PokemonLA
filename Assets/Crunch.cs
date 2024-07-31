using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crunch : Skill
{
    // Start is called before the first frame update
    void Start()
    {
        if (transform.rotation.eulerAngles.z == 0) { transform.position += Vector3.right * 0.5f; }
        else if (transform.rotation.eulerAngles.z == 90) { transform.position += Vector3.up * 0.5f; }
        else if (transform.rotation.eulerAngles.z == 180) { transform.position += Vector3.left * 0.5f; }
        else if (transform.rotation.eulerAngles.z == 270) { transform.position += Vector3.down * 0.5f; }
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

            //if (animator != null) { animator.SetTrigger("Hit"); }
            if (SkillFrom == 2 && target.DefUpLevel >= 0) { target.DefChange(-1, 0); Pokemon.PokemonHpChange(null, player.gameObject, player.maxHp / 4, 0, 0, Type.TypeEnum.IgnoreType); player.KnockOutDirection = Vector2.zero; player.KnockOutPoint = 0; }
            else {
                if (Random.Range(0.0f, 1.0f) + (float)player.LuckPoint / 30 >= 0.8f)
                {
                    target.DefChange(-1, 0);
                }
            }
            
            HitAndKo(target);
        }
    }
}
