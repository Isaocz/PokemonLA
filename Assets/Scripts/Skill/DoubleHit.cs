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

    //每帧减少飞弹的存在时间，当存在时间等于0时销毁飞弹
    // Update is called once per frame
    void Update()
    {
        StartExistenceTimer();
    }
}
