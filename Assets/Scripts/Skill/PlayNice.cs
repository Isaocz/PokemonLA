using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class PlayNice : Skill
{
    List<Empty> influence = new List<Empty>();
    // Start is called before the first frame update
    void Awake()
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
            //��Ŀ��ֻ���һ�ν���
            if (target != influence.Find(t => t))
            {
                target.AtkChange(-1,0.0f);
                influence.Add(target);
                if(SkillFrom == 2)
                {
                    target.AtkChange(-1, 0.0f);
                    target.SpAChange(-1, 0.0f);
                }
            }
        }
    }

    void OnDestroy()
    {
        //����ʱ�ָ�������
        for (int i = 0; i < influence.Count; i++)
        {
            influence[i].AtkChange(1,0.0f);
            if (SkillFrom == 2)
            {
                influence[i].AtkChange(1, 0.0f);
                influence[i].SpAChange(1, 0.0f);
            }
        }
    }
}
