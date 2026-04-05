using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvalanchePS : MonoBehaviour
{

    Avalanche ParentAva;
    SubAvalanche ParentSubAva;
    List<Empty> TargetList = new List<Empty> { };

    // Start is called before the first frame update
    void Start()
    {
        ParentAva = transform.parent.GetComponent<Avalanche>();
        ParentSubAva = transform.parent.GetComponent<SubAvalanche>();
    }

    private void OnParticleCollision(GameObject other)
    {
        if (other.tag == "Empty")
        {

            if (ParentAva != null)
            {
                ParentAva.HitAndKo(other.gameObject);
            }
            else if (ParentSubAva != null)
            {
                ParentSubAva.HitAndKo(other.gameObject);
            }


        }
    }
}
