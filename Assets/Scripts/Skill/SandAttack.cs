using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandAttack : Skill
{
    List<Empty> Empties = new List<Empty>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        StartExistenceTimer();
    }

    void OnParticleCollision(GameObject other)
    {
        if (other.tag == "Empty")
        {

            Empty target = other.GetComponent<Empty>();
            if (!Empties.Contains(target))
            {
                Empties.Add(target);
                target.Blind(2, 1);
            }
        }
    }
}
