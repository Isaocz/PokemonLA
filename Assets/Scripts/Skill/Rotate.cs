using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    //��ת��
    float zAngle = 0.1f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //������ת���ٶ�0.1f
        transform.RotateAround(transform.position, Vector3.forward, zAngle);
    }
}
