using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterPulse : Skill
{
    ParticleSystem PS1;
    bool isDouble;
    List<Empty> CpnfusionList = new List<Empty> { };

    // Start is called before the first frame update
    void Start()
    {
        PS1 = transform.GetChild(0).GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        StartExistenceTimer();
        //PS1.TriggerSubEmitter(0);
    }

    public void Confusion(Empty target)
    {

        if (!CpnfusionList.Contains(target)) {
            CpnfusionList.Add(target);
            if (Random.Range(0.0f, 1.0f) + ((float)player.LuckPoint / 20.0f) >= (target.isSpeedChange? 0.0f : 0.8f) )
            {
                target.EmptyConfusion(5f, 1);
                
            }
            Timer.Start(this, 0.9f, () => { if (target != null) { CpnfusionList.Remove(target); } });
            if (SkillFrom == 2 && Random.Range(0.0f, 1.0f) + ((float)player.LuckPoint / 10.0f) >= 0.5f) { target.SpeedChange(); target.SpeedRemove01(3.0f * target.OtherStateResistance); }Debug.Log("xsxs");
        }


    }

}
