using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyPressCollider : MonoBehaviour
{
    BodyPress ParentBodyPress;

    private void Start()
    {
        ParentBodyPress = transform.parent.GetComponent<BodyPress>();
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Empty")
        {
            Empty e = other.GetComponent<Empty>();
            if (e != null)
            {
                ParentBodyPress.HitAndKo(e);
            }
        }
    }
}
