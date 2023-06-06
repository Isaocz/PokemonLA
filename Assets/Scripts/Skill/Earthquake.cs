using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Earthquake : Skill
{

    GameObject RollPS;
    GameObject Earthquake2;
    ParticleSystem RockPS;
    float RollDestoryTimer;
    bool isAnimatorOver;

    private void Start()
    {
        RollPS = transform.GetChild(0).gameObject;
        Earthquake2 = transform.GetChild(1).gameObject;
        RockPS = transform.GetChild(1).GetChild(0).GetComponent<ParticleSystem>();
    }

    // Start is called before the first frame update
    void Update()
    {
        if (RollPS != null) {
            RollDestoryTimer += Time.deltaTime;
            if (RollDestoryTimer >= 6f)
            {
                Destroy(RollPS);
            }
        }
        if(!isAnimatorOver && !RockPS.isEmitting)
        {
            GetComponent<Animator>().SetTrigger("Over");
            isAnimatorOver = true;
        }
        
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

}
