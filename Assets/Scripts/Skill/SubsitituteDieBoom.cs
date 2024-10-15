using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubsitituteDieBoom : Skill
{


    // Update is called once per frame
    void Update()
    {
        StartExistenceTimer();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Empty")
        {
            Empty e = other.GetComponent<Empty>();
            if (e != null)
            {
                Pokemon.PokemonHpChange(player.gameObject, e.gameObject, Damage, 0, 0, PokemonType.TypeEnum.Normal);
            }
        }
    }
}
