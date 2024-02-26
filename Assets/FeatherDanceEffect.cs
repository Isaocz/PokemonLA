using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeatherDanceEffect : MonoBehaviour
{

    FeatherDance ParentFeatherDance;

    // Start is called before the first frame update
    void Start()
    {
        ParentFeatherDance = transform.parent.GetComponent<FeatherDance>();
    }

    private void OnParticleCollision(GameObject other)
    {
        if (other.tag == "Empty")
        {
            Empty target = other.GetComponent<Empty>();
            if (target != null) { 
                ParentFeatherDance.FeatherDanceEffect(target);
                if (ParentFeatherDance.SkillFrom == 2 && target.isHit)
                {
                    ParentFeatherDance.FeatherDancePlusEffect(target);
                }
            }
        }
    }
}
