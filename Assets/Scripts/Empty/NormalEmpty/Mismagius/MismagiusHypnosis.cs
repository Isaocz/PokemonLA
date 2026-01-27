using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MismagiusHypnosis : Projectile
{

    public float DispearTime = 4.0f;


    private void Start()
    {
        Destroy(gameObject, DispearTime);
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == ("Enviroment") || other.tag == ("Room") || other.tag == ("Player") || (empty != null && empty.isEmptyInfatuationDone && other.gameObject != empty.gameObject && other.tag == ("Empty")))
        {
            if (other.tag == ("Player"))
            {
                PlayerControler playerControler = other.GetComponent<PlayerControler>();
                if (playerControler != null)
                {
                    playerControler.SleepFloatPlus(0.4f);
                }
            }
            if (empty != null && empty.isEmptyInfatuationDone && other.gameObject != empty.gameObject && other.tag == ("Empty"))
            {
                Empty e = other.GetComponent<Empty>();
                if (e != null)
                {
                    e.EmptySleepDone(0.5f, 7.5f, 1.0f);
                }
            }
        }
    }

}
