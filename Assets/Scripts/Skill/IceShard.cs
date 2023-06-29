using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceShard : Skill
{
    Vector2 direction;
    Vector3 StartPostion;
    bool isCanNotMove;
    ParticleSystem IceShardPS;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        direction = (transform.rotation * Vector2.right).normalized;
        StartPostion = transform.position;
        IceShardPS = transform.GetChild(0).GetComponent<ParticleSystem>();
    }


    private void FixedUpdate()
    {
        if (!isCanNotMove)
        {
            Vector3 postion = transform.position;
            postion.x += direction.x * 4.5f * Time.deltaTime;
            postion.y += direction.y * 4.5f * Time.deltaTime;
            transform.position = postion;
            if ((StartPostion - transform.position).magnitude > MaxRange)
            {
                animator.SetTrigger("Break");
                isCanNotMove = true;
                IceShardPS.Stop();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {   
        if (other.tag == "Empty")
        {
            Empty target = other.GetComponent<Empty>();
            HitAndKo(target);
            animator.SetTrigger("Break");
            if (SkillFrom == 2 && target.isEmptyFrozenDone) {
                Debug.Log(transform.rotation.eulerAngles);
                if (transform.rotation.eulerAngles != new Vector3(0, 0, 0)) Instantiate(gameObject,transform.position,Quaternion.Euler(new Vector3(0,0,0)));
                if (transform.rotation.eulerAngles != new Vector3(0, 0, 90)) Instantiate(gameObject,transform.position,Quaternion.Euler(new Vector3(0,0,90)));
                if (transform.rotation.eulerAngles != new Vector3(0, 0, 180)) Instantiate(gameObject,transform.position,Quaternion.Euler(new Vector3(0,0,180)));
                if (transform.rotation.eulerAngles != new Vector3(0, 0, 270)) Instantiate(gameObject,transform.position,Quaternion.Euler(new Vector3(0,0,270))); 
            }
            isCanNotMove = true;
            IceShardPS.Stop();
        }
        else if (other.tag == "Projectel")
        {
            Destroy(other.gameObject);
        }else if (other.tag == "Room" || other.tag == "Enviroment")
        {
            animator.SetTrigger("Break");
            isCanNotMove = true;
            IceShardPS.Stop();
        }
    }
}
