using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionDestoryObject : MonoBehaviour
{
    /// <summary>
    /// ±¬Õ¨´Ý»Ù±¬Õ¨Îï
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.isTrigger)
        {
            Debug.Log(other.gameObject.name);
            ObjectBeKickDown obj = other.GetComponent<ObjectBeKickDown>();
            if (obj != null)
            {
                obj.BeKickDown();
            }
        }
    }
}
