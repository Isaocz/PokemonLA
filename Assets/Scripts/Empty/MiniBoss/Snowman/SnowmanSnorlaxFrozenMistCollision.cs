using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowmanSnorlaxFrozenMistCollision : MonoBehaviour
{
    public float FrozenPoint;
    public float FrozenTime;
    private void OnParticleCollision(GameObject other)
    {

        PlayerControler p = other.GetComponent<PlayerControler>();
        if (p != null)
        {
            p.PlayerFrozenFloatPlus(FrozenPoint, FrozenTime);
        }
    }

    private void OnDestroy()
    {
        Destroy(transform.parent.gameObject);
    }
}
