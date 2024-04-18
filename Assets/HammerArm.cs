using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerArm : Skill
{
    // Start is called before the first frame update
    void Start()
    {
        transform.rotation = Quaternion.Euler(0,0,0);
    }

    // Update is called once per frame
    void Update()
    {
        StartExistenceTimer();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Empty")
        {
            Empty e = other.GetComponent<Empty>();
            if (e != null)
            {
                HitAndKo(e);
                player.SpeedChange();
                player.SpeedRemove01(5.0f);
            }
        }
    }
}
