using Pathfinding.Serialization;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
//using UnityEditor.Rendering;
using UnityEngine;

public class Growl : Skill
{
    List<Empty> influence = new List<Empty>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        StartExistenceTimer();
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Empty")
        {
            Empty target = other.GetComponent<Empty>();
            if (target != influence.Find(t => t))
            {
                influence.Add(target);
                target.AtkChange(-1,6.0f);
                if(SkillFrom == 2)
                {
                    target.EmptyConfusion(5f,1);
                }
            }
        }
    }

}
