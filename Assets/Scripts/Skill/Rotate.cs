using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Rotate : MonoBehaviour
{
    //旋转角
    public float zAngle = 0.1f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //刚体旋转，速度zAngle9
        transform.RotateAround(transform.position, Vector3.forward, zAngle);
    }
}
