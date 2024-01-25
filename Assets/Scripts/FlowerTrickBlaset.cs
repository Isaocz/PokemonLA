using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerTrickBlaset : Projectile
{

    public PlayerControler player;

    // Start is called before the first frame update

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Empty")
        {
            Empty target = other.GetComponent<Empty>();
            if (target != null)
            {
                target.EmptyKnockOut(6);
                Pokemon.PokemonHpChange(player.gameObject, target.gameObject, Dmage, SpDmage, 0, ProType);
            }
        }
    }
}
