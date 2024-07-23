using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeanLook : Skill
{
    public float radius;
    public Empty target;

    void Start()
    {
        
    }

    void Update()
    {
        StartExistenceTimer();
        if(target != null)
        {
            float distance = Vector2.Distance(target.transform.position, transform.position);
            if (distance > radius)
            {
                Vector3 direction = (target.transform.position - transform.position).normalized;
                target.transform.position = transform.position + direction * radius;
            }
        }
    }
}
