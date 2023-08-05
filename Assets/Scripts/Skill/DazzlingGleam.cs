using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DazzlingGleam : Skill
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        StartExistenceTimer();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null && collision.tag == "Empty")
        {
            Empty target = collision.GetComponent<Empty>();
            //强化会造成混乱效果1秒
            if(SkillFrom == 2)
            {
                target.EmptyConfusion(1f, 1);
            }
            HitAndKo(target);
        }
    }
}
