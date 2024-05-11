using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HardPressCollider : MonoBehaviour
{
    HardPress ParentHardPress;

    private void Start()
    {
        ParentHardPress = transform.parent.GetComponent<HardPress>();
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Empty")
        {
            Empty e = other.GetComponent<Empty>();
            if (e != null)
            {
                ParentHardPress.Damage = (int)((100.0f * (((float)e.EmptyHp) / ((float)e.maxHP))) * ParentHardPress.DmageImproveAlpha());
                ParentHardPress.HitAndKo(e);
                Debug.Log(ParentHardPress.Damage);
            }
        }
    }
}
