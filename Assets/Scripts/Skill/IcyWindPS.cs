using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IcyWindPS : MonoBehaviour
{
    Empty target;
    List<Empty> Empties = new List<Empty>();


    // Start is called before the first frame update
    void OnParticleCollision(GameObject other)
    {
        if (other.tag == "Empty")
        {

            target = other.GetComponent<Empty>();
            gameObject.transform.parent.GetComponent<IcyWind>().HitAndKo(target);
            if (!Empties.Contains(target))
            {
                Empties.Add(target);
                target.SpeedChange();
                target.SpeedRemove01(3 * target.OtherStateResistance);
            }
        }
    }
}
