using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaryFace : Skill
{

    // Update is called once per frame
    void Update()
    {
        StartExistenceTimer();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Empty" && other.GetComponent<Empty>() != null)
        {
            Empty target = other.GetComponent<Empty>();
            target.SpeedChange();
            target.SpeedRemove01(3 * target.OtherStateResistance);
        }
    }
}
