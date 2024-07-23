using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkPulse : Skill
{
    public float diffuseRadius;
    public int particalNumber;
    public float duration;
    public GameObject darkPulsePartical;
    private float initialExistenceTime;
    private bool isRelease;

    void Start()
    {
        initialExistenceTime = ExistenceTime;
        isRelease = false;
    }

    // Update is called once per frame
    void Update()
    {
        StartExistenceTimer();
        if (ExistenceTime < (initialExistenceTime - 0.75f) && !isRelease)
        {
            isRelease = true;
            for (int i = 0; i < particalNumber; i++)
            {
                GameObject dpp = Instantiate(darkPulsePartical, this.transform);
                dpp.GetComponent<DarkPulsePar>().Initialize(diffuseRadius, duration, SpDamage);
            }
        }
        else if (ExistenceTime > (initialExistenceTime - 0.75f))
        {
            transform.position = player.transform.position;
        }
    }
}
