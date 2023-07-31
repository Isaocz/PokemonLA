using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulldozeCollider : MonoBehaviour
{
    Skill ParentBulldoze;
    List<Empty> Empties = new List<Empty> { };

    private void Start()
    {
        ParentBulldoze = transform.parent.GetComponent<Skill>();
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Empty")
        {
            Empty e = other.GetComponent<Empty>();
            if (e != null)
            {
                ParentBulldoze.HitAndKo(e);
                if (!Empties.Contains(e))
                {
                    Empties.Add(e);
                    e.SpeedChange();
                    e.SpeedRemove01(3 * e.OtherStateResistance);
                }

            }
        }
    }
}
