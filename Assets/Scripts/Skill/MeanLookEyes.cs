using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeanLookEyes : MonoBehaviour
{

    public Pokemon target;

    Empty EmptyTarget;



    void Start()
    {
        Destroy(gameObject, 3.5f);
        EmptyTarget = target.GetComponent<Empty>();
    }

    void Update()
    {
        if(target!= null)
        {
            transform.position = target.transform.GetChild(2).position + new Vector3(0, 0.5f);
        }

        if (EmptyTarget != null && EmptyTarget.isDie) { Destroy(gameObject); }


    }
}
