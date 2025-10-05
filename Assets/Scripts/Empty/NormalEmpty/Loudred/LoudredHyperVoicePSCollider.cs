using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoudredHyperVoicePSCollider : MonoBehaviour
{
    Empty target;
    LoudredHyperVoice ParentHV;

    private void Awake()
    {
        ParentHV = gameObject.transform.parent.GetComponent<LoudredHyperVoice>();
    }

    // Start is called before the first frame update
    void OnParticleCollision(GameObject other)
    {
        if (!ParentHV.empty.isEmptyInfatuationDone && other.tag == "Player")
        {
            PlayerControler p = other.GetComponent<PlayerControler>();
            Pokemon.PokemonHpChange(ParentHV.empty.gameObject, other.gameObject, 0, ParentHV.SpDmage, 0, PokemonType.TypeEnum.Normal);
            if (p != null)
            {
                p.KnockOutPoint = 5f;
                p.KnockOutDirection = (p.transform.position - transform.position).normalized;
            }
        }
        else if(ParentHV.empty.isEmptyInfatuationDone && other.tag == "Empty" && other.gameObject != ParentHV.empty.gameObject)
        {
            Empty e = other.GetComponent<Empty>();
            Pokemon.PokemonHpChange(ParentHV.empty.gameObject, other.gameObject, 0, ParentHV.SpDmage, 0, PokemonType.TypeEnum.Normal);
        }
    }
}
