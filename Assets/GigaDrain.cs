using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GigaDrain : GrassSkill
{
    Vector2 direction;
    Vector3 StartPostion;
    bool isCanNotMove;
    GameObject OverPS1;
    GameObject OverPS2;

    public float DrainPer = 0.5f;


    // Start is called before the first frame update
    void Start()
    {
        DrainPer = 0.5f;
        animator = GetComponent<Animator>();
        direction = (transform.rotation * Vector2.right).normalized;
        StartPostion = transform.position;
        transform.rotation = Quaternion.Euler(0, 0, 0);
        OverPS1 = transform.GetChild(3).gameObject;
        OverPS2 = transform.GetChild(4).gameObject;
    }


    private void FixedUpdate()
    {
        if (!isCanNotMove)
        {
            Vector3 postion = transform.position;
            postion.x += direction.x * 9f * Time.deltaTime;
            postion.y += direction.y * 9f * Time.deltaTime;
            transform.position = postion;
            if ((StartPostion - transform.position).magnitude > MaxRange)
            {
                BallBreak();

            }
        }
    }

    void BallBreak()
    {
        if (!isCanNotMove)
        {
            transform.GetComponent<Collider2D>().enabled = false;
            animator.SetTrigger("Over");
            OverPS1.gameObject.SetActive(true);
            OverPS1.transform.parent = transform.parent;
            isCanNotMove = true;
        }
    }

    void DrainPS()
    {
        OverPS2.gameObject.SetActive(true);
        OverPS2.transform.rotation = Quaternion.Euler(0, 0, _mTool.Angle_360Y((player.transform.position - transform.position).normalized, Vector2.right));
        OverPS2.transform.parent = transform.parent;
        ParticleSystem p = OverPS2.GetComponent<ParticleSystem>();
        float D = ((player.transform.position - transform.position).magnitude - 0.2f) * 1.5f;
        var v = p.velocityOverLifetime;
        var r = p.velocityOverLifetime.radial;
        r.curveMultiplier = D;
        v.xMultiplier = D;
        v.radial = r;
        
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.isTrigger == false)
        {
            if (other.tag == "Empty")
            {
                Empty target = other.GetComponent<Empty>();
                int hp = target.EmptyHp;
                HitAndKo(target);
                Drain(hp,target.EmptyHp,DrainPer);
                DrainPS();
                BallBreak();
            }
            else if (other.tag == "Room" || other.tag == "Enviroment")
            {
                BallBreak();
            }
        }
        if (SkillFrom == 2)
        {
            if (other.tag == "Grass")
            {
                NormalGress n = other.GetComponent<NormalGress>();
                GressPlayerINOUT g = other.GetComponent<GressPlayerINOUT>();
                if (n != null)
                {
                    SpDamage += 5;
                    DrainPer = Mathf.Clamp(DrainPer + 0.05f, 0.0f, 1.0f);
                    n.GrassDie();
                }
                if (g != null)
                {
                    SpDamage += 5;
                    DrainPer = Mathf.Clamp( DrainPer + 0.05f , 0.0f , 1.0f);
                    g.GrassDie();
                }
            }
        }
    }
}
