using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SweetKiss : Projectile
{

    bool isDestory;
    GameObject OverPS;
    bool isKissDone;

    // Start is called before the first frame update
    private void Awake()
    {
        AwakeProjectile();
        OverPS = transform.GetChild(1).gameObject;
    }


    private void Update()
    {
        //this.transform.localScale += new Vector3(Time.deltaTime * 2, 0, 0);
        DestoryProjectile(10);
        if (isDestory)
        {
            CollisionDestory();
        }
        else
        {
            MoveNotForce();
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == ("Enviroment") || other.tag == ("Room") || other.tag == ("Empty"))
        {
            OverPS.SetActive(true);
            isDestory = true;
            Destroy(rigidbody2D);
            if(other.GetComponent<Empty>() != null)
            {
                if (!isKissDone)
                {
                    isKissDone = true;
                    //Debug.Log(transform.gameObject);
                    other.GetComponent<Empty>().EmptyConfusion(10, 0.4f);
                }
            }
        }
    }

}
