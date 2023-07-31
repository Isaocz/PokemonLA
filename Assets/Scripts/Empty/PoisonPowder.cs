using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonPowder : MonoBehaviour
{

    Empty ParentEmpty;

    private void Start()
    {
        ParentEmpty = transform.parent.GetComponent<Empty>();
    }

    // Start is called before the first frame update

    void OnParticleCollision(GameObject other)
    {
        if (!ParentEmpty.isEmptyInfatuationDone)
        {
            if (other.tag == "Player" && other.GetComponent<PlayerControler>() != null)
            {
                other.GetComponent<PlayerControler>().ToxicFloatPlus(0.15f);
            }
        }
    }
}
