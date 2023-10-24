using Pathfinding.Serialization;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
//using UnityEditor.Rendering;
using UnityEngine;

public class Growl : Skill
{
    List<Empty> influence = new List<Empty>();
    List<int> infnum = new List<int>();
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
                infnum.Add(target.AtkEmptyPoint);
                influence.Add(target);
                target.LevelChange(-1, "Atk");
                target.AtkDown(0);
                if(SkillFrom == 2)
                {
                    target.EmptyConfusion(5f,1);
                }
            }
        }
    }

    void OnDestroy()
    {
        for(int i = 0; i < influence.Count; i++)
        {
            influence[i].AtkEmptyPoint = infnum[i];
            influence[i].AtkDownRemove();
        }
    }
}
