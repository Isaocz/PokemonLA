using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    //旋转角
    float zAngle = 0.1f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //刚体旋转，速度0.1f
        transform.RotateAround(transform.position, Vector3.forward, zAngle);
    }
}
