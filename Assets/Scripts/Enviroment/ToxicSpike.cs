using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToxicSpike : Spike
{
    bool isDamageUp;

    public override void SpikeOnTriggerStay2D(Collider2D other)
    {
        if (other.tag == ("Player"))
        {
            PlayerControler playerControler = other.GetComponent<PlayerControler>();
            if (playerControler != null && !playerControler.playerData.IsPassiveGetList[13] && !playerControler.isRapidSpin)
            {
                playerControler.ToxicFloatPlus(0.18f);
                if (playerControler.isToxicDone && !isDamageUp) { Damage *= 2; isDamageUp = true; }
                else if (!playerControler.isToxicDone && isDamageUp) { Damage /= 2; isDamageUp = false; }
            }
        }
        else if (other.tag == "Empty")
        {
            Empty target = other.GetComponent<Empty>();
            if (target != null)
            {
                target.EmptyToxicDone(0.35f, 10, 1);
            }
        }
        base.SpikeOnTriggerStay2D(other);
    }



    private void Update()
    {
        SpikesUpdate();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        SpikeOnTriggerStay2D(other);
    }
}
