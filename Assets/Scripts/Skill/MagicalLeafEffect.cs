using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicalLeafEffect : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Empty"||other.tag == "EmptyFly")
        {
            Empty target = other.gameObject.GetComponent<Empty>();
            gameObject.transform.parent.GetComponent<MagicalLeaf>().HitAndKo(target);
            Destroy(gameObject);
        }
        if (other.tag == "Room")
        {
            Destroy(gameObject);
        }
    }
}
