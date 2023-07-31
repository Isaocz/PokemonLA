using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubDoubleHit : SubSkill
{
    // Start is called before the first frame update
    void Start()
    {
        player.RemoveASubSkill(subskill);
    }

    // Update is called once per frame
    void Update()
    {

        StartExistenceTimer();
    }
}
