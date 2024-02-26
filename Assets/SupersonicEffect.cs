using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupersonicEffect : MonoBehaviour
{

    Supersonic ParentSuperSonic;

    private void Start()
    {
        ParentSuperSonic = transform.parent.GetComponent<Supersonic>();
    }

    private void OnParticleCollision(GameObject other)
    {
        if (other.tag == "Empty")
        {
            Empty target = other.GetComponent<Empty>();
            if (target != null)
            {
                ParentSuperSonic.SupersonicEmpty(target);
            }
        }
    }
}
