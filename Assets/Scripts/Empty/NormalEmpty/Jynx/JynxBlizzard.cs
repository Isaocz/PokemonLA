using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JynxBlizzard : MonoBehaviour
{
    //迷纯姐
    public Empty ParentEmpty;
    //威力
    public int SpDmage;


    //粒子1
    public ParticleSystem BlizzardPS1;

    //粒子2
    public ParticleSystem BlizzardPS2;

    //粒子3
    public ParticleSystem BlizzardPS3;




    private void Start()
    {
        BlizzardPS1.Play();
        BlizzardPS2.Play();
        BlizzardPS3.Play();
        Destroy(gameObject, 8.0f);
    }


    private void OnTriggerStay2D(Collider2D other)
    {
        if (ParentEmpty != null)
        {
            //未被魅惑时
            if (!ParentEmpty.isEmptyInfatuationDone && other.tag == "Player")
            {
                PlayerControler p = other.GetComponent<PlayerControler>();
                Pokemon.PokemonHpChange(ParentEmpty.gameObject, other.gameObject, 0, SpDmage, 0, PokemonType.TypeEnum.Ice);
                p.PlayerFrozenFloatPlus(0.3f, 1.2f);
                if (p != null)
                {
                    p.KnockOutPoint = 1.5f;
                    p.KnockOutDirection = (p.transform.position - transform.position).normalized;
                }
            }
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (ParentEmpty != null)
        {
            //被魅惑时
            if (ParentEmpty.isEmptyInfatuationDone && other.tag == "Empty" && other.gameObject != ParentEmpty.gameObject)
            {
                Empty e = other.GetComponent<Empty>();
                e.Frozen(3.5f, 0.5f, 1.0f);
                Pokemon.PokemonHpChange(ParentEmpty.gameObject, other.gameObject, 0, SpDmage, 0, PokemonType.TypeEnum.Ice);
            }
        }
    }


}
