using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SweetScent : Skill
{
    ParticleSystem PS1;

    // Start is called before the first frame update
    void Start()
    {
        PS1 = transform.GetChild(0).GetComponent<ParticleSystem>();
        var PS1Main = PS1.main;
        PS1Main.loop = false;
    }

    // Update is called once per frame
    void Update()
    {
        StartExistenceTimer();
    }


    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Empty")
        {
            Empty Target = other.GetComponent<Empty>();
            if (Target != null)
            {
                Target.SpeedChange();
                    
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Empty")
        {
            Empty Target = other.GetComponent<Empty>();
            if (Target != null)
            {
                Target.SpeedRemove01(0.1f);
            }
        }
    }

}
