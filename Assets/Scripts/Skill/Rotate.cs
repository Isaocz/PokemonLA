using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Rotate : MonoBehaviour
{
    //��ת��
    public float zAngle = 0.1f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //������ת���ٶ�zAngle9
        transform.RotateAround(transform.position, Vector3.forward, zAngle);
    }
}
