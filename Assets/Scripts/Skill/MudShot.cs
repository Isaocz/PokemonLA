using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MudShot : Skill
{
    ParticleSystem PS1;

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
}
