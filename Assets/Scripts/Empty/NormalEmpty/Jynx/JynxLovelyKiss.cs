using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JynxLovelyKiss : Projectile
{
    bool isDestory;
    public float MoveSpeed;

    bool isCanNotMove;


    private void Awake()
    {
        AwakeProjectile();
        Timer.Start(this, 12.0f, () => { if (!isDestory) { isDestory = true; } });
    }




    private void Update()
    {
        //this.transform.localScale += new Vector3(Time.deltaTime * 2, 0, 0);
        if (!isCanNotMove)
        {
            transform.Translate(Vector3.right * MoveSpeed * Time.deltaTime);
        }
        DestoryByRange(40);
        if (isDestory)
        {
            CollisionDestory();
        }
        else
        {
            MoveNotForce();
        }

    }

    void KissBreak()
    {
        isCanNotMove = true;
        GetComponent<Animator>().SetTrigger("Over");
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).GetComponent<ParticleSystem>()) {
                var Emission1 = transform.GetChild(i).GetComponent<ParticleSystem>().emission;
                var Main1 = transform.GetChild(i).GetComponent<ParticleSystem>().main;
                Emission1.enabled = false;
                Main1.loop = false;
            }
        }

        //transform.DetachChildren();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.tag == ("Enviroment") || other.tag == ("Room") || other.tag == ("Player") || (empty.isEmptyInfatuationDone && other.gameObject != empty.gameObject && other.tag == ("Empty")))
        {
            KissBreak();
            isDestory = true;
            Destroy(rigidbody2D);
            if (other.tag == ("Player") && !empty.isEmptyInfatuationDone)
            {
                PlayerControler playerControler = other.GetComponent<PlayerControler>();
                if (playerControler != null)
                {
                    playerControler.SleepFloatPlus(0.4f);
                }
            }
            if (empty.isEmptyInfatuationDone && other.gameObject != empty.gameObject && other.tag == ("Empty"))
            {
                Empty e = other.GetComponent<Empty>();
                if (e != null)
                {
                    e.EmptySleepDone(0.5f, 7.5f, 1.0f);
                }
            }
        }
    }
}
