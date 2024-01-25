using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinecoBlast : Projectile
{
    // Start is called before the first frame update

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Empty")
        {
            Empty target = other.GetComponent<Empty>();
            if (target != null) {
                target.EmptyKnockOut(10);
                Pokemon.PokemonHpChange(Baby.gameObject, target.gameObject, Dmage, SpDmage, 0, ProType);
            }
        }
        else if (other.tag == "Player")
        {
            PlayerControler p = other.GetComponent<PlayerControler>();
            Pokemon.PokemonHpChange(Baby.gameObject, p.gameObject, Dmage, SpDmage, 0, ProType);
            if (p != null)
            {
                p.KnockOutPoint = 10;
                p.KnockOutDirection = (p.transform.position - transform.position).normalized;
            }

        }
    }

}
