using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AndoridUICatchCamera : MonoBehaviour
{
    Canvas c;

    // Start is called before the first frame update
    private void Awake()
    {
        c = gameObject.GetComponent<Canvas>();
        c.worldCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
    }

    private void Update()
    {
        if (c.worldCamera == null)
        {
            c.worldCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        }
    }
}
