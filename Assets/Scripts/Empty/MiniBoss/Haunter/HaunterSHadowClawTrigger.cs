using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HaunterSHadowClawTrigger : MonoBehaviour
{

    public HaunterShadowClaw ParentShadowClaw;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!ParentShadowClaw.ParentHaunter.isEmptyInfatuationDone && other.transform.tag == ("Player"))
        {
            PlayerControler playerControler = other.gameObject.GetComponent<PlayerControler>();
            Pokemon.PokemonHpChange(ParentShadowClaw.ParentHaunter.gameObject, other.gameObject, Haunter.DMAGE_SHADOWCLAW, 0, 0, PokemonType.TypeEnum.Ghost);
            if (playerControler != null)
            {
                playerControler.KnockOutPoint = 3;
                playerControler.KnockOutDirection = (playerControler.transform.position - transform.position).normalized;
            }
            ParentShadowClaw.ParentHaunter.GetCTEffect(other.transform);
        }
        if (ParentShadowClaw.ParentHaunter.isEmptyInfatuationDone && other.transform.tag == ("Empty"))
        {
            Empty e = other.gameObject.GetComponent<Empty>();
            Pokemon.PokemonHpChange(ParentShadowClaw.ParentHaunter.gameObject, e.gameObject, Haunter.DMAGE_SHADOWCLAW, 0, 0, PokemonType.TypeEnum.Ghost);
            ParentShadowClaw.ParentHaunter.GetCTEffect(other.transform);
        }
    }
}
