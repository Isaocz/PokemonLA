using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RazorLeafEffect : MonoBehaviour
{
    List<Empty> enemy = new List<Empty>();
    // Start is called before the first frame update
    void OnParticleCollision(GameObject other)
    {
        if(other.tag == "Empty")
        {
            Empty target = other.GetComponent<Empty>();
            if (!enemy.Contains(target))
            {
                gameObject.transform.parent.GetComponent<RazorLeaf>().HitAndKo(target);
                enemy.Add(target);
            }
        }

    }
}
