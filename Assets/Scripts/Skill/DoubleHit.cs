using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleHit : Skill
{

    public SubDoubleHit SubDH;

    private void Start()
    {
        if (SkillFrom == 2 && SubDH != null)
        {
            player.AddASubSkill(SubDH);
        }
    }

    //ÿ֡���ٷɵ��Ĵ���ʱ�䣬������ʱ�����0ʱ���ٷɵ�
    // Update is called once per frame
    void Update()
    {
        StartExistenceTimer();
    }
}
