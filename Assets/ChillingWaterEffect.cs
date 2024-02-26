using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChillingWaterEffect : MonoBehaviour
{
    Empty target;
    List<Empty> Empties = new List<Empty>();
    ChillingWater ParentChillingWater;

    private void Start()
    {
        if (gameObject.transform.parent.GetComponent<ChillingWater>() != null)
        {
            ParentChillingWater = gameObject.transform.parent.GetComponent<ChillingWater>();
        }
    }


    // Start is called before the first frame update
    void OnParticleCollision(GameObject other)
    {

        if (other.tag == "Empty")
        {
            target = other.GetComponent<Empty>();
            if (ParentChillingWater != null && target != null)
            {
                ParentChillingWater.HitAndKo(target);
                ParentChillingWater.EffectTrigger( target );
            }


        }
    }
}
