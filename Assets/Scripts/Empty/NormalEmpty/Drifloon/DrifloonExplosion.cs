using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrifloonExplosion : Projectile
{
    public float KOPoint = 10.0f;

    bool isDestory;


    List<Collider2D> cList = new List<Collider2D> { };


    private void Awake()
    {
        AwakeProjectile();
        Timer.Start(this, 5.0f, () => { Destroy(gameObject); });
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (cList.Contains(collision)) { return; }
        else { cList.Add(collision); }
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

                    BreakableEnviroment en = collision.gameObject.GetComponent<BreakableEnviroment>();
                    //轰炸可摧毁环境物
                    if (en != null)
                    {
                        en.BeHit(en.MaxHP / 2);
                    }
                    //轰炸其他敌人
                    else
                    {
                        Pokemon.PokemonHpChange(empty.gameObject, collision.gameObject, Dmage, 0, 0, PokemonType.TypeEnum.Normal);
                        Empty e = collision.GetComponent<Empty>();
                        if (e != null && e != empty)
                        {
                            e.EmptyKnockOut(KOPoint);
                        }
                    }

                }
            }
        }
    }
}
