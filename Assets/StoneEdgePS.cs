using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneEdgePS : MonoBehaviour
{
    ParticleSystem StoneEdgePSObj;
    StoneEdge ParentStoneEdge;

    // Start is called before the first frame update
    void Start()
    {
        ParentStoneEdge = transform.parent.GetComponent<StoneEdge>();
        StoneEdgePSObj = GetComponent<ParticleSystem>();
        var Main = StoneEdgePSObj.main;
        Main.startRotation = (90 - (transform.parent.rotation).eulerAngles.z) * Mathf.Deg2Rad;
    }

    private void OnParticleCollision(GameObject other)
    {
        if (other.tag == "Empty")
        {
            Empty target = other.GetComponent<Empty>();
            if (target != null)
            {
                ParentStoneEdge.HitAndKo(target);
            }
        }
        
    }
}
