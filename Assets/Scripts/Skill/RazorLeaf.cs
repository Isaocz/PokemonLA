using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RazorLeaf : Skill
{
    // Start is called before the first frame update
    void Start()
    {
        //更容易击中要害
        Damage = 55;
        if(Random.Range(0,7) + (float)player.LuckPoint >= 7)
        {
            Damage = 110;
        }
    }

    // Update is called once per frame
    void Update()
    {
        StartExistenceTimer();
    }
}
