using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smoke : MonoBehaviour
{
    Empty target;
    List<Empty> Empties = new List<Empty>();


    // Start is called before the first frame update

    private void OnParticleCollision(GameObject other)
    {
        if (other.tag == "Empty")
        {
            target = other.GetComponent<Empty>();
            Empties.Add(target);
            target.Blind(8 * target.OtherStateResistance, 1);
        }
    }

    private void Update()
    {
        if (GetComponent<ParticleSystem>().isStopped)
        {
            Destroy(gameObject.transform.parent.gameObject);
        }
    }

}
