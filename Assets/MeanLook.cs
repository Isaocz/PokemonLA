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
        if(target != null && !(target.EmptyBossLevel == Empty.emptyBossLevel.Boss || target.EmptyBossLevel == Empty.emptyBossLevel.EndBoss))
        {
            float distance = Vector2.Distance(target.transform.position, transform.position);
            if (distance > radius)
            {
                Vector3 direction = (target.transform.position - transform.position).normalized;
                target.transform.position = transform.position + direction * radius;
            }
        }
        if (target.isDie)
        {
            Destroy(gameObject);
        }
    }
}
