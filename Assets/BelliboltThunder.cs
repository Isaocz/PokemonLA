using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BelliboltThunder : Projectile
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke("SetPSActive" , 0.3f);
    }

    void SetPSActive()
    {
        transform.GetChild(0).gameObject.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!empty.isEmptyInfatuationDone && collision.tag == "Player")
        {
            // 对玩家造成伤害
            PlayerControler playerControler = collision.GetComponent<PlayerControler>();
            Pokemon.PokemonHpChange(empty.gameObject, collision.gameObject, 0, SpDmage, 0, Type.TypeEnum.Electric);
            if (playerControler != null)
            {
                playerControler.KnockOutPoint = 5f;
                playerControler.KnockOutDirection = (playerControler.transform.position - transform.position).normalized;
                if (Random.Range(0.0f, 1.0f) > 0.9f) { playerControler.ParalysisFloatPlus(0.4f); }
            }

        }
        else if (empty.isEmptyInfatuationDone && collision.tag == "Empty" && collision.gameObject != empty.gameObject)
        {
            Empty e = collision.GetComponent<Empty>();
            Pokemon.PokemonHpChange(empty.gameObject, collision.gameObject, 0, SpDmage, 0, Type.TypeEnum.Electric);
            if (Random.Range(0.0f, 1.0f) > 0.9f)
            {
                e.EmptyParalysisDone(1,10);
            }
            
        }
        if(empty.isEmptyConfusionDone && collision.gameObject == empty.gameObject)
        {
            Pokemon.PokemonHpChange(empty.gameObject, collision.gameObject, 0, SpDmage, 0, Type.TypeEnum.Electric);
        }
    }
}
