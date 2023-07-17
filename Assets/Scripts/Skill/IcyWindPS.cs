using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IcyWindPS : MonoBehaviour
{
    Empty target;
    List<Empty> Empties = new List<Empty>();
    IcyWind ParentIW;

    private void Awake()
    {
        ParentIW = gameObject.transform.parent.GetComponent<IcyWind>();
    }

    // Start is called before the first frame update
    void OnParticleCollision(GameObject other)
    {
        if (other.tag == "Empty")
        {

            target = other.GetComponent<Empty>();
            if (target.isEmptyFrozenDone) { ParentIW.SpDamage *= 2; }
            ParentIW.HitAndKo(target);
            if (!Empties.Contains(target))
            {
                Empties.Add(target);
                target.SpeedChange();
                target.SpeedRemove01(3 * target.OtherStateResistance);
                target.Frozen(7.5f, 1, 0.1f + (float)ParentIW.player.LuckPoint / 30); 
            }
        }
    }
}
