using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Roost : Skill
{
    float RecoveryAmount;
    // Start is called before the first frame update
    void Start()
    {

        RecoveryAmount = player.maxHp / 2;


        Pokemon.PokemonHpChange(null, player.gameObject, 0, 0, (int)RecoveryAmount, PokemonType.TypeEnum.IgnoreType);
        player.isCanNotMove = true;
    }

    // Update is called once per frame
    void Update()
    {
        StartExistenceTimer();

    }

    private void OnDestroy()
    {
        if (player.isCanNotMove)
        {
            player.isCanNotMove = false;
        }
    }
}
