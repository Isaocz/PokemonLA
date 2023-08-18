using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thrash : Skill
{
    // Start is called before the first frame update
    void Start()
    {
        player.isCanNotMove = true;

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
            Empty target = other.GetComponent<Empty>();
            HitAndKo(target);
        }
    }

    private void OnDestroy()
    {
        player.isCanNotMove = false;
        player.ConfusionFloatPlus(1);
    }
}
