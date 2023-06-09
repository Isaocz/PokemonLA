using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToxicSpike : Spike
{
    bool isDamageUp;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == ("Player"))
        {
            PlayerControler playerControler = other.GetComponent<PlayerControler>();
            if (!playerControler.playerData.IsPassiveGetList[13]) {
                playerControler.ToxicFloatPlus(0.18f);
                if (playerControler.isToxicDone && !isDamageUp) { Damage *= 2; isDamageUp = true; }
            }
        }
    }
}
