using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherBall : Projectile
{
    // Start is called before the first frame update
    bool isDestory;
    public Type.TypeEnum WeatherBallType;
    bool isPSDie;

    SpriteRenderer s1;
    private void Awake()
    {
        AwakeProjectile();
        s1 = transform.GetChild(0).GetComponent<SpriteRenderer>();
    }


    private void Update()
    {
        //this.transform.localScale += new Vector3(Time.deltaTime * 2, 0, 0);
        if ((transform.position - BornPosition).magnitude >= 20)
        {
            if (spriteRenderer.material.color.a >= 0)
            {
                PSDie();
                spriteRenderer.material.color = spriteRenderer.material.color - new Color(0, 0, 0, 3f * Time.deltaTime);
                s1.material.color = s1.material.color - new Color(0, 0, 0, 3f * Time.deltaTime);
            }
            if ((transform.position - BornPosition).magnitude >= 10 + 3)
            {
                Destroy(gameObject);
            }
        }
        if (isDestory)
        {
            PSDie();
            s1.material.color = s1.material.color - new Color(0, 0, 0, 3f * Time.deltaTime);
            CollisionDestory();
        }
        else
        {
            MoveNotForce();
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
                if (empty != null) { Pokemon.PokemonHpChange(empty.gameObject, other.gameObject, 0, SpDmage, 0, WeatherBallType); }
                else { Pokemon.PokemonHpChange(null, other.gameObject, 0, SpDmage, 0, WeatherBallType) ; }
                
                if (playerControler != null)
                {
                    playerControler.KnockOutPoint = 3;
                    playerControler.KnockOutDirection = (playerControler.transform.position - transform.position).normalized;
                }
            }
        }
    }

    void PSDie()
    {
        if (!isPSDie)
        {
            isPSDie = true;
            var PSSizeDie = transform.GetChild(1).GetComponent<ParticleSystem>().sizeOverLifetime;
            PSSizeDie.enabled = true;
            var PSColorDie = transform.GetChild(1).GetComponent<ParticleSystem>().colorOverLifetime;
            PSColorDie.enabled = true;
        }
    }

    public void Die()
    {
        isDestory = true;
    }

}
