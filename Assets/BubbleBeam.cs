using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleBeam : Skill
{
    public BubbleBeamBubble Bubble;
    float Timer;
    float NextTimer = 0.2f;

    // Start is called before the first frame update
    void Start()
    {
        LunchBubble();
    }

    // Update is called once per frame
    void Update()
    {
        StartExistenceTimer();
        if (ExistenceTime >= 2.0f)
        {
            Timer += Time.deltaTime;
            if (Timer >= NextTimer)
            {
                NextTimer = Random.Range(0.1f,0.2f);
                Timer = 0;
                LunchBubble();
            }
        }
    }

    void LunchBubble()
    {
        Instantiate(Bubble, transform.position, Quaternion.AngleAxis(Random.Range(-7,7) , Vector3.forward) * transform.rotation , transform);
    }

}
