using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RazorLeafEffect : MonoBehaviour
{
    List<GameObject> enemy = new List<GameObject>();
    // Start is called before the first frame update
    void OnParticleCollision(GameObject other)
    {
        if(other.tag == "Empty")
        {
            Empty target = other.GetComponent<Empty>();
            if (!enemy.Contains(other))
            {
                gameObject.transform.parent.GetComponent<RazorLeaf>().HitAndKo(other.gameObject);
                enemy.Add(other.gameObject);
            }
        }

    }
}
