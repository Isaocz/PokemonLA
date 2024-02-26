using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrineEffect : MonoBehaviour
{
    Empty target;
    List<Empty> Empties = new List<Empty>();
    Brine ParentBrine;

    private void Start()
    {
        if (gameObject.transform.parent.GetComponent<Brine>() != null)
        {
            ParentBrine = gameObject.transform.parent.GetComponent<Brine>();
        }
    }


    // Start is called before the first frame update
    void OnParticleCollision(GameObject other)
    {

        if (other.tag == "Empty")
        {
            target = other.GetComponent<Empty>();
            if (ParentBrine != null && target != null) {
                if (target.EmptyHp <= target.maxHP / 2) { 
                    ParentBrine.SpDmageDouble();
                    if (!Empties.Contains(target))
                    {
                        Empties.Add(target);
                        target.SpeedChange();
                        target.SpeedRemove01(3 * target.OtherStateResistance);
                    }
                }
                else { ParentBrine.SpDmageDoubleReset(); }
                ParentBrine.HitAndKo(target);
            }
            

        }
    }

}
