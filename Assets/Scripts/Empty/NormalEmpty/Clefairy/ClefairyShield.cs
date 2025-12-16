using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClefairyShield : MonoBehaviour
{
    public Empty ParentEmpty;

    List<Empty> EList = new List<Empty> { };

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject != ParentEmpty.gameObject)
        {
            if (!ParentEmpty.isEmptyInfatuationDone && other.tag == "Empty")
            {
                Empty e = other.GetComponent<Empty>();
                if (e && !EList.Contains(e))
                {
                    EList.Add(e);
                    e.GetShield(e.maxHP / 5);
                }
            }
        }
    }
}
