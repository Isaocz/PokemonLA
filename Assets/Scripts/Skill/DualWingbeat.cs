using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DualWingbeat : Skill
{
    public SubDualWingbeat sub;

    private void Start()
    {
        float MaxAlpha = 1.0f + Mathf.Max(player.SkillOffsetforBodySize[1] , player.SkillOffsetforBodySize[2]);
        transform.GetChild(0).localPosition = new Vector3( 0 , MaxAlpha / 1.5f , 0);
        transform.GetChild(1).localPosition = new Vector3( 0 , -MaxAlpha / 1.5f, 0);
    }


    //ÿ֡���ٷɵ��Ĵ���ʱ�䣬������ʱ�����0ʱ���ٷɵ�
    // Update is called once per frame
    void Update()
    {
        StartExistenceTimer();
    }

    public void AddSubDW()
    {
        player.AddASubSkill(sub);
    }
}
