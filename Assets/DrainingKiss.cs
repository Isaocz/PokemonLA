using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrainingKiss : Skill
{
    Vector2 direction;
    Vector3 StartPostion;
    bool isCanNotMove;
    ParticleSystem DrainingKissPS;
    ParticleSystem DrainingKissOverPS;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        direction = (transform.rotation * Vector2.right).normalized;
        StartPostion = transform.position;
        DrainingKissOverPS = transform.GetChild(1).GetComponent<ParticleSystem>();
        DrainingKissPS = GetComponent<ParticleSystem>();
        transform.rotation = Quaternion.Euler(Vector3.zero);
        if (SkillFrom == 2)
        {
            player.ButterflyManger.BornABF(FairyButterfly.ButterflyType.浅黄色回血型);
            if (Random.Range(0.0f, 1.0f) + (float)player.LuckPoint / 30 > 0.8f)
            {
                player.ButterflyManger.BornABF(FairyButterfly.ButterflyType.浅黄色回血型);
            }
        }
    }

    private void FixedUpdate()
    {
        if (!isCanNotMove)
        {
            Vector3 postion = transform.position;
            postion.x += direction.x * 5f * Time.deltaTime;
            postion.y += direction.y * 5f * Time.deltaTime;
            transform.position = postion;
            if ((StartPostion - transform.position).magnitude > MaxRange)
            {
                isCanNotMove = true;
                DrainingKissOverPS.gameObject.SetActive(true);
                DrainingKissOverPS.Play();
                DrainingKissPS.Stop();
                animator.SetTrigger("Over");
                GetComponent<Collider2D>().enabled = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Empty" && other.isTrigger == false)
        {
            Empty target = other.GetComponent<Empty>();
            int hp = target.EmptyHp;
            HitAndKo(target);
            Pokemon.PokemonHpChange(null , player.gameObject , 0 , 0 , Mathf.Clamp( (int)((float)(hp-target.EmptyHp)*0.75f) ,1,100) , Type.TypeEnum.IgnoreType );
            isCanNotMove = true;
            DrainingKissOverPS.gameObject.SetActive(true);
            DrainingKissOverPS.Play();
            DrainingKissPS.Stop();
            animator.SetTrigger("Over");
            GetComponent<Collider2D>().enabled = false;
        }
        else if ((other.tag == "Room" || other.tag == "Enviroment") && other.isTrigger == false)
        {
            isCanNotMove = true;
            DrainingKissOverPS.gameObject.SetActive(true);
            DrainingKissOverPS.Play();
            DrainingKissPS.Stop();
            animator.SetTrigger("Over");
            GetComponent<Collider2D>().enabled = false;
        }
    }

}
