using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterPulseEffect : MonoBehaviour
{
    Empty target;
    List<Empty> Empties = new List<Empty>();
    WaterPulse ParentWaterPulse;

    private void Start()
    {
        if (gameObject.transform.parent.GetComponent<WaterPulse>() != null)
        {
            ParentWaterPulse = gameObject.transform.parent.GetComponent<WaterPulse>();
        }
    }


    // Start is called before the first frame update
    void OnParticleCollision(GameObject other)
    {

        if (other.tag == "Empty")
        {
            target = other.GetComponent<Empty>();
            if (ParentWaterPulse != null && target != null)
            {
                /*
                if (target.EmptyHp <= target.maxHP / 2)
                {
                    ParentWaterPulse.SpDmageDouble();
                    if (!Empties.Contains(target))
                    {
                        Empties.Add(target);
                        target.SpeedChange();
                        target.SpeedRemove01(3 * target.OtherStateResistance);
                    }
                }
                else { ParentWaterPulse.SpDmageDoubleReset(); }
                */
                ParentWaterPulse.HitAndKo(target);
                ParentWaterPulse.Confusion(target);
            }


        }
    }
}
