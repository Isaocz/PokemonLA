using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RazorLeafEffect : MonoBehaviour
{
    // Start is called before the first frame update

    Empty target;
    void OnParticleCollision(GameObject other)
    {
        if(other.tag == "Empty")
        {
            target = other.GetComponent<Empty>();
            gameObject.transform.parent.GetComponent<RazorLeaf>().HitAndKo(target);
        }

    }
}
