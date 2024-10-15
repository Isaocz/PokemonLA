using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnixStoneEdge : Projectile
{
    // Start is called before the first frame update
    bool isDestory;
    ParticleSystem PS;


    float StartTimer;

    private void Awake()
    {
        AwakeProjectile();
    }

    private void Start()
    {
        PS = GetComponent<ParticleSystem>();
    }

    private void Update()
    {

        //this.transform.localScale += new Vector3(Time.deltaTime * 2, 0, 0);
        DestoryByRange(30);
        if (isDestory)
        {
            if (transform.childCount > 1) { transform.GetChild(1).gameObject.SetActive(true); transform.GetChild(1).parent = null; }
            CollisionDestory();
        }
        else
        {
            MoveNotForce();
        }


    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == ("Player") || other.tag == ("Room"))
        {
            isDestory = true;
            Destroy(rigidbody2D);
            if (other.tag == ("Player"))
            {
                PlayerControler playerControler = other.GetComponent<PlayerControler>();
                Pokemon.PokemonHpChange((empty == null ? null : empty.gameObject), other.gameObject, Dmage, 0, 0, PokemonType.TypeEnum.Rock);
                if (playerControler != null)
                {
                    Debug.Log(empty);
                    playerControler.KnockOutPoint = 5;
                    playerControler.KnockOutDirection = (playerControler.transform.position - transform.position).normalized;
                }
            }
        }
    }
}
