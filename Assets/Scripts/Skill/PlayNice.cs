using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class PlayNice : Skill
{
    List<Empty> influence = new List<Empty>();
    List<int> infnum = new List<int>();
    //��ת��
    float zAngle = 0.1f;
    // Start is called before the first frame update
    void Awake()
    {

    }

    // Update is called once per frame
    void Update()
    {
        StartExistenceTimer();
        //������ת���ٶ�0.1f
        transform.RotateAround(transform.position, Vector3.forward, zAngle);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Empty")
        {
            Empty target = other.GetComponent<Empty>();
            //��Ŀ��ֻ���һ�ν���
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
        //����ʱ�ָ�������
        for (int i = 0; i < influence.Count; i++)
        {
            influence[i].AtkEmptyPoint = infnum[i];
        }
    }
}
