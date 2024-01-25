using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoarCollidor : MonoBehaviour
{
    Empty target;
    Roar ParentRoar;

    private void Awake()
    {
        ParentRoar = gameObject.transform.parent.GetComponent<Roar>();
    }

    // Start is called before the first frame update
    void OnParticleCollision(GameObject other)
    {
        if (other.tag == "Empty")
        {
            target = other.GetComponent<Empty>();
            if (ParentRoar.SkillFrom != 2)
            {
                target.Fear(3, 1);
            }
            else
            {
                target.Fear(5.5f, 2);
            }
        }
    }
}
