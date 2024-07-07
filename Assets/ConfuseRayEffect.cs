using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfuseRayEffect : MonoBehaviour
{
    public Transform target;

    void Update()
    {
        if (target != null)
        {
            transform.position = target.position + new Vector3(0,1.2f);
        }
    }
}
