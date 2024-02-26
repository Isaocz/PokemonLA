using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChillingWater : Skill
{

    List<Empty> Empties = new List<Empty>();
    public GameObject PowderSnow_02;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        StartExistenceTimer();
    }

    public void EffectTrigger( Empty target )
    {
        if (!Empties.Contains(target))
        {
            Empties.Add(target);
            target.AtkChange(-1, 5.0f);
            if (PowderSnow_02 != null)
            {
                Instantiate(PowderSnow_02, target.transform.position + new Vector3(Random.Range(-0.77f, 0.77f), Random.Range(0.0f, 0.3f), 0), Quaternion.identity, target.transform);
                Instantiate(PowderSnow_02, target.transform.position + new Vector3(Random.Range(-0.77f, 0.77f), Random.Range(0.0f, 0.3f), 0), Quaternion.Euler(0, 0, Random.Range(0, 180)), target.transform);
                Instantiate(PowderSnow_02, target.transform.position + new Vector3(Random.Range(-0.77f, 0.77f), Random.Range(0.3f, 1.3f), 0), Quaternion.Euler(0, 0, Random.Range(0, 180)), target.transform);
                Instantiate(PowderSnow_02, target.transform.position + new Vector3(Random.Range(-0.77f, 0.77f), Random.Range(0.3f, 1.3f), 0), Quaternion.Euler(0, 0, Random.Range(0, 180)), target.transform);
            }
            if (SkillFrom == 2)
            {
                if (Random.Range(0.0f, 1.0f) + ((float)player.LuckPoint / 20.0f) >= 0.7f ) { target.SpeedChange(); target.SpeedRemove01(3.0f*target.OtherStateResistance); }
                target.Cold(5);
            }
        }
        

    }
}
