using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceShard : Skill
{
    Vector2 direction;
    Vector3 StartPostion;
    bool isCanNotMove;
    ParticleSystem IceShardPS;
    public GameObject BornGameOBJ;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        direction = (transform.rotation * Vector2.right).normalized;
        StartPostion = transform.position;
        IceShardPS = transform.GetChild(0).GetComponent<ParticleSystem>();
        if (BornGameOBJ == null) { BornGameOBJ = gameObject; } 
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
                IceBreak();
                isCanNotMove = true;
                IceShardPS.Stop();
            }
        }
    }

    
    void IceBreak()
    {
        var Emission1 = transform.GetChild(0).GetComponent<ParticleSystem>().emission;
        var Main1 = transform.GetChild(0).GetComponent<ParticleSystem>().main;
        Emission1.enabled = false;
        Main1.loop = false;

        var Emission2 = transform.GetChild(1).GetComponent<ParticleSystem>().emission;
        var Main2 = transform.GetChild(1).GetComponent<ParticleSystem>().main;
        Emission2.enabled = false;
        Main2.loop = false;

        transform.DetachChildren();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {   
        if (other.tag == "Empty")
        {
            Empty target = other.GetComponent<Empty>();
            HitAndKo(target);
            animator.SetTrigger("Break");
            IceBreak();
            if (SkillFrom == 2 && target.isEmptyFrozenDone && BornGameOBJ.gameObject != other.gameObject) {
                Debug.Log(transform.rotation.eulerAngles);
                BornGameOBJ = other.gameObject;
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
            if(SkillFrom == 2 && BornGameOBJ.gameObject != other.gameObject)
            {
                IcicleCrashOBJ ice = other.GetComponent<IcicleCrashOBJ>();
                if(ice != null)
                {
                    ice.ColliderCount++;
                    if (ice.ColliderCount >= Random.Range(1, 6))
                    {
                        ice.IceBreak();
                    }
                    BornGameOBJ = other.gameObject;
                    if (transform.rotation.eulerAngles != new Vector3(0, 0, 0)) Instantiate(gameObject, transform.position, Quaternion.Euler(new Vector3(0, 0, 0)));
                    if (transform.rotation.eulerAngles != new Vector3(0, 0, 90)) Instantiate(gameObject, transform.position, Quaternion.Euler(new Vector3(0, 0, 90)));
                    if (transform.rotation.eulerAngles != new Vector3(0, 0, 180)) Instantiate(gameObject, transform.position, Quaternion.Euler(new Vector3(0, 0, 180)));
                    if (transform.rotation.eulerAngles != new Vector3(0, 0, 270)) Instantiate(gameObject, transform.position, Quaternion.Euler(new Vector3(0, 0, 270)));
                }
            }
            animator.SetTrigger("Break");
            IceBreak();
            isCanNotMove = true;
            IceShardPS.Stop();
        }
    }
}
