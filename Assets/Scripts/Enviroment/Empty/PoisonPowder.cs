using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonPowder : MonoBehaviour
{
    // Start is called before the first frame update

    void OnParticleCollision(GameObject other)
    {
        if(other.tag == "Player")
        {
            other.GetComponent<PlayerControler>().ToxicFloatPlus(0.15f);
        }
    }
}
