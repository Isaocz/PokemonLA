using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TogepiFire : Projectile
{
    // Start is called before the first frame update


    private void Awake()
    {
        AwakeProjectile();
        Invoke("CallDestory", 10);

    }

    bool isDestory;
    bool isCanNotMove;
    int DestoryTimer;
    float BornTimer;

    private void Start()
    {
        transform.rotation = Quaternion.Euler(0,0,0);
    }

    private void Update()
    {
        if (BornTimer < 4.0f)
        {
            BornTimer += Time.deltaTime;
            if (gameObject.transform.localScale.x < 2)
            {
                gameObject.transform.localScale += new Vector3(Time.deltaTime * 0.25f, Time.deltaTime * 0.25f, Time.deltaTime * 0.25f);
            }
        }
        if (!isCanNotMove)
        {
            MoveNotForce();
        }
    }

    private void FixedUpdate()
    {
        if (isDestory)
        {
            DestoryTimer++;
            if (DestoryTimer >= 6)
            {
                gameObject.transform.localScale -= new Vector3(0.08f, 0.08f, 0.08f);
                if (gameObject.transform.localScale.x <= 0.05)
                {
                    Destroy(gameObject);
                }
                DestoryTimer = 0;
            }
        }
    }


    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enviroment" || other.tag == "Empty" || other.tag == "Room")
        {
            isDestory = true;
            isCanNotMove = true;
            if (other.tag == "Empty")
            {
                Empty target = other.GetComponent<Empty>();
                Pokemon.PokemonHpChange(Baby.gameObject, target.gameObject, Dmage, SpDmage, 0, ProType);
            }
            gameObject.transform.localScale -= new Vector3(0.15f, 0.15f, 0.15f);
            if (gameObject.transform.localScale.x <= 0.3)
            {
                Destroy(gameObject);
            }
        }
    }
}
