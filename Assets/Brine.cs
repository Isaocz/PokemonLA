using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brine : Skill
{
    ParticleSystem PS1;
    bool isDouble;

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

    public void SpDmageDouble()
    {
        if (!isDouble)
        {
            isDouble = true;
            SpDamage *= 2;
        }
    }

    public void SpDmageDoubleReset()
    {
        if (isDouble)
        {
            isDouble = false;
            SpDamage /= 2;
        }
    }


}
