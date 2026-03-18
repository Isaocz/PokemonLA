using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectrodeVoltSwitchThunderWave : Projectile
{

    private void Awake()
    {
        Timer.Start(this, 1.0f, () => { Destroy(this.gameObject); });
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player"|| (empty.isEmptyInfatuationDone && collision.tag == "Empty" && collision.gameObject != empty.gameObject))
        {
            if (empty != null)
            {
                if (!empty.isEmptyInfatuationDone && collision.tag == "Player")
                {
                    PlayerControler playerControler = collision.GetComponent<PlayerControler>();
                    Pokemon.PokemonHpChange(empty.gameObject, collision.gameObject, 0, 20, 0, PokemonType.TypeEnum.Electric);
                    if (playerControler != null)
                    {
                        playerControler.KnockOutPoint = 1;
                        playerControler.KnockOutDirection = (playerControler.transform.position - transform.position).normalized;
                        playerControler.ParalysisFloatPlus(0.5f);
                    }
                }
                else if (empty.isEmptyInfatuationDone && collision.tag == "Empty" && collision.gameObject != empty.gameObject)
                {
                    Empty e = collision.GetComponent<Empty>();
                    Pokemon.PokemonHpChange(empty.gameObject, e.gameObject, 0, 20, 0, PokemonType.TypeEnum.Electric);
                }
            }
        }
    }
}
