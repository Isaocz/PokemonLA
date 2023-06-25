using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PsyduckConfusion : Projectile
{
    // Start is called before the first frame update
    bool isDestory;
    private void Awake()
    {
        AwakeProjectile();
    }


    private void Update()
    {
        //this.transform.localScale += new Vector3(Time.deltaTime * 2, 0, 0);
        DestoryProjectile(10);
        if (isDestory)
        {
            CollisionDestory();
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == ("Enviroment") || other.tag == ("Room") || other.tag == ("Player"))
        {
            isDestory = true;
            Destroy(rigidbody2D);
            if (other.tag == ("Player"))
            {
                PlayerControler playerControler = other.GetComponent<PlayerControler>();
                playerControler.ChangeHp(0, -(50 * empty.SpAAbilityPoint * (2 * empty.Emptylevel + 10) / 250), 14);
                playerControler.KnockOutPoint = 5;
                playerControler.KnockOutDirection = (playerControler.transform.position - transform.position).normalized;
            }
        }
    }
}
