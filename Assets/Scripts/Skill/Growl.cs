using Pathfinding.Serialization;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
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
                target.AtkEmptyPoint = (int)(target.AtkEmptyPoint * 0.8);
            }
        }
    }

    void OnDestroy()
    {
        for(int i = 0; i < influence.Count; i++)
        {
            influence[i].AtkEmptyPoint = infnum[i];
        }
    }
}
