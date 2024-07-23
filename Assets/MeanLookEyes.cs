using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeanLookEyes : MonoBehaviour
{

    public Pokemon target;
    void Start()
    {
        Destroy(gameObject, 3.5f);
    }

    void Update()
    {
        if(target!= null)
        {
            transform.position = target.transform.position + new Vector3(0, 1.5f);
        }
    }
}
