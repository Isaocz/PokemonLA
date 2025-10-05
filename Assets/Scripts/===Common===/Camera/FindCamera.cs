using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindCamera : MonoBehaviour
{
    Canvas c;
    private void Start()
    {
        c = gameObject.GetComponent<Canvas>();
    }


    // Update is called once per frame
    void Update()
    {
        if (c.worldCamera == null) {
            gameObject.GetComponent<Canvas>().worldCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        }
    }
}
