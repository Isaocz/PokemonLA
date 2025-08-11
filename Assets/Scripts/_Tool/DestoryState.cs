using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestoryState : MonoBehaviour
{
    public void DestorySelf()
    {
        Destroy(gameObject);
    }

    public void RemoveChild()
    {
        transform.DetachChildren();
    }


    public void RemovePSChild()
    {
        foreach (Transform t in transform)
        {
            if (t.GetComponent<ParticleSystem>())
            {
                t.transform.parent = null;
            }
        }
        //transform.DetachChildren();
    }
}
