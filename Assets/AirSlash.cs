using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirSlash : Projectile
{
    // Start is called before the first frame update
    bool isDestory;
    ParticleSystem PS;
    Animator animator;
    TrailRenderer Trail;

    float StartTimer;

    private void Awake()
    {
        AwakeProjectile();
    }

    private void Start()
    {
        PS = GetComponent<ParticleSystem>();
        animator = GetComponent<Animator>();
        Trail = transform.GetChild(0).GetComponent<TrailRenderer>();
        GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.2f);
        transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
    }

    private void Update()
    {


        //this.transform.localScale += new Vector3(Time.deltaTime * 2, 0, 0);
        DestoryProjectile(13);
        if (isDestory)
        {
            animator.SetTrigger("Over");
            CollisionDestory();
            var Emit = PS.emission;
            Emit.enabled = false;
            Trail.enabled = false;
        }


    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == ("Player") || (empty.isEmptyInfatuationDone && other.gameObject != empty.gameObject && other.tag == ("Empty")))
        {
            isDestory = true;
            Destroy(rigidbody2D);
            GetComponent<Animator>().SetTrigger("Over");
            if (other.tag == ("Player") && !empty.isEmptyInfatuationDone)
            {
                PlayerControler playerControler = other.GetComponent<PlayerControler>();
                Pokemon.PokemonHpChange(empty.gameObject, other.gameObject, 0, SpDmage, 0, Type.TypeEnum.Flying);
                if (playerControler != null)
                {
                    playerControler.KnockOutPoint = 5;
                    playerControler.KnockOutDirection = (playerControler.transform.position - transform.position).normalized;
                }
            }
            if (other.tag == ("Empty") && empty.isEmptyInfatuationDone)
            {
                Empty e = other.GetComponent<Empty>();
                Pokemon.PokemonHpChange(empty.gameObject, e.gameObject, 0, SpDmage, 0, Type.TypeEnum.Flying);
                //e.EmptyHpChange(0, (SpDmage * empty.SpAAbilityPoint * (2 * empty.Emptylevel + 10) / (250 * e.SpdAbilityPoint * ((Weather.GlobalWeather.isHail ? ((e.EmptyType01 == Type.TypeEnum.Ice || e.EmptyType02 == Type.TypeEnum.Ice) ? 1.5f : 1) : 1))) + 2), 14);
            }
        }
    }
}
