using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.Mathematics;
using UnityEngine;

public class MagicalLeaf : Skill
{
    public bool StartTimer;

    // Start is called before the first frame update
    void Start()
    {
        //��ȡ����Ч����������ת����
        transform.GetChild(0).rotation = quaternion.Euler(-90, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (StartTimer) {
            StartExistenceTimer();
        }
    }
}
