using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SableyeConfuseRay : Projectile
{

    Animator animator;
    bool isDestory;

    // Start is called before the first frame update
    private void Awake()
    {
        AwakeProjectile();
    }
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }


    private void Update()
    {
        //this.transform.localScale += new Vector3(Time.deltaTime * 2, 0, 0);

        if (!isDestory)
        {
            MoveNotForce();
            if ((transform.position - BornPosition).magnitude >= 10)
            {
                BallBreak();
            }
        }

    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }


    void BallBreak()
    {
        if (!isDestory)
        {
            transform.GetComponent<Collider2D>().enabled = false;
            animator.SetTrigger("Over");
            isDestory = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == ("Enviroment") || other.tag == ("Room") || other.tag == ("Empty"))
        {
            BallBreak();
            Destroy(rigidbody2D);
            if (other.GetComponent<Empty>() != null)
            {
                other.GetComponent<Empty>().EmptyConfusion(10,5);
            }
        }
    }

}
