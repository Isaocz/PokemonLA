using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiglettMudShot : MonoBehaviour
{
    public Diglett EmptyDiglett;
    bool isDmageDone;



    private void OnParticleCollision(GameObject other)
    {
        if (!isDmageDone) {
            if (other.tag == "Player" || other.tag == "Empty")
            {
                isDmageDone = true;
                if(EmptyDiglett == null)
                {
                    Pokemon.PokemonHpChange(EmptyDiglett.gameObject, other, 0, 55,0, Type.TypeEnum.Ground);
                }
                else
                {
                    if (other.tag == ("Player") && !EmptyDiglett.isEmptyInfatuationDone)
                    {
                        PlayerControler p = other.GetComponent<PlayerControler>();
                        Pokemon.PokemonHpChange(EmptyDiglett.gameObject, other.gameObject, 0, 55, 0, Type.TypeEnum.Ground);
                        if (p != null)
                        {
                            p.KnockOutPoint = 5;
                            p.KnockOutDirection = (p.transform.position - transform.position).normalized;
                        }
                    }
                    if (other.tag == ("Empty") && EmptyDiglett.isEmptyInfatuationDone)
                    {
                        Empty e = other.GetComponent<Empty>();
                        Pokemon.PokemonHpChange(EmptyDiglett.gameObject, e.gameObject, 0, 55, 0, Type.TypeEnum.Ground);
                    }
                }
            }
        }
    }


    private void OnDestroy()
    {
        Destroy(transform.parent.gameObject);
    }

}
