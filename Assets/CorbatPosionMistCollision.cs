using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorbatPosionMistCollision : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnParticleCollision(GameObject other)
    {
        PlayerControler p = other.GetComponent<PlayerControler>();
        if ( p != null )
        {
            p.ToxicFloatPlus(0.2f);
        }
    }
}
