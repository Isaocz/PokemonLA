using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnixTailDig : MonoBehaviour
{
    public Empty ParentOnix;
    SubEmptyBody SubBody;

    private void Start()
    {
        SubBody = transform.GetComponent<SubEmptyBody>();
        SubBody.ParentEmpty = ParentOnix;
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == ("Player"))
        {
            PlayerControler playerControler = other.GetComponent<PlayerControler>();
            Pokemon.PokemonHpChange((ParentOnix == null ? null : ParentOnix.gameObject), other.gameObject, 80, 0, 0, PokemonType.TypeEnum.Ground);
            if (playerControler != null)
            {
                
                playerControler.KnockOutPoint = 5;
                playerControler.KnockOutDirection = (playerControler.transform.position - transform.position).normalized;
            }
        }
    }

    public void DestorySelf()
    {
        Destroy(gameObject);
    }
}
