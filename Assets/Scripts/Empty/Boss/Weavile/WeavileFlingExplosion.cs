using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeavileFlingExplosion : Projectile
{
    public float KOPoint = 10.0f;

    bool isDestory;


    private void Awake()
    {
        AwakeProjectile();
        Timer.Start(this, 5.0f, () => { Destroy(gameObject); });
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" || collision.tag == "Empty")
        {
            if (empty != null)
            {
                if (collision.tag == "Player")
                {
                    Pokemon.PokemonHpChange(empty.gameObject, collision.gameObject, Dmage, 0, 0, PokemonType.TypeEnum.Normal);
                    PlayerControler playerControler = collision.GetComponent<PlayerControler>();
                    if (playerControler != null)
                    {
                        playerControler.KnockOutPoint = KOPoint;
                        playerControler.KnockOutDirection = (playerControler.transform.position - transform.position).normalized;
                    }
                }
                if (collision.tag == "Empty")
                {
                    
                    WeavileMazeWall wall = collision.gameObject.GetComponent<WeavileMazeWall>();
                    //∫‰’®¬Í·¿≠√‘π¨«Ω±⁄
                    if (wall != null)
                    {
                        wall.BeHit(wall.MaxHP/2);
                    }
                    //≤ª∫‰’®∆‰À˚µ–»À
                    else
                    {
                        //Pokemon.PokemonHpChange(null, collision.gameObject, 40, 0, 0, PokemonType.TypeEnum.Normal);
                        //Empty e = collision.GetComponent<Empty>();
                        //if (e != null && e != empty)
                        //{
                        //    e.EmptyKnockOut(KOPoint);
                        //}
                    }
                    
                }
            }
        }
    }
}
