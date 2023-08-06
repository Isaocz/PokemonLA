using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Rotate : MonoBehaviour
{
    //旋转角
    public float zAngle = 0.1f;

    private Vector3 rotationCenter; // 旋转中心
    // Start is called before the first frame update
    void Start()
    {
        UpdateRotationCenter();
    }

    // Update is called once per frame
    void Update()
    {
        SetRotationCenter();
        //刚体旋转，速度zAngle
        transform.RotateAround(rotationCenter, Vector3.forward, zAngle);
    }
    void UpdateRotationCenter()
    {
        rotationCenter = transform.position; // 更新旋转中心为当前位置
    }

    // 如果对象中心位置可能发生变化的话，在适当的时候调用这个方法来更新旋转中心
    public void SetRotationCenter()
    {
        UpdateRotationCenter();
    }
}
