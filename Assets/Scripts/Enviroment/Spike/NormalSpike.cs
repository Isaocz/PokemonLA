using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalSpike : Spike
{


    private void Update()
    {
        SpikesUpdate();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        SpikeOnTriggerStay2D(other);
    }
}
