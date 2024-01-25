using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlizzardCastformDmage : MonoBehaviour
{
    Empty ParentEmpty;
    int SpDmage;

    private void Start()
    {
        ParentEmpty = transform.parent.GetComponent<BlizzardCastform>().empty;
        SpDmage = transform.parent.GetComponent<BlizzardCastform>().SpDmage;
    }

    private void OnParticleCollision(GameObject other)
    {
        if (other.tag == "Player")
        {
            // 对玩家造成伤害
            PlayerControler playerControler = other.GetComponent<PlayerControler>();
            if (ParentEmpty != null) { Pokemon.PokemonHpChange(ParentEmpty.gameObject, other.gameObject, 0, SpDmage, 0, Type.TypeEnum.Electric); }
            else { Pokemon.PokemonHpChange(null, other.gameObject, 0, SpDmage, 0, Type.TypeEnum.Electric); }
            if (playerControler != null)
            {
                playerControler.KnockOutPoint = 6.5f;
                playerControler.KnockOutDirection = (transform.position - playerControler.transform.position).normalized;
            }

        }
    }
}
