using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class PlayNice : Skill
{
    List<Empty> influence = new List<Empty>();
    List<int> infnum = new List<int>();
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
            //对目标只造成一次降攻
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
        //结束时恢复攻击力
        for (int i = 0; i < influence.Count; i++)
        {
            influence[i].AtkEmptyPoint = infnum[i];
        }
    }
}
