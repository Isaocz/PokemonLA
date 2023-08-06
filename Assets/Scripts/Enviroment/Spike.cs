using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour
{
    public float Damage;

    // Start is called before the first frame update
    private void OnTriggerStay2D(Collider2D other)
    {
        if(other.tag == ("Player"))
        {
            PlayerControler playerControler = other.GetComponent<PlayerControler>();
            //playerControler.ChangeHp(-Damage, 0, 19);
            
            if (playerControler != null && !playerControler.playerData.IsPassiveGetList[13])
            {
                Pokemon.PokemonHpChange(null, other.gameObject, Damage, 0, 0, Type.TypeEnum.IgnoreType);
                playerControler.KnockOutPoint = 1f;
                playerControler.KnockOutDirection = (playerControler.transform.position - transform.position).normalized;
            }
        }
        else if (other.tag == "Empty")
        {
            Empty target = other.GetComponent<Empty>();
            if (target != null) {
                Pokemon.PokemonHpChange(null, target.gameObject, 10, 0, 0, Type.TypeEnum.IgnoreType);
                target.EmptyKnockOut(0);
            }
        }
    }


}
