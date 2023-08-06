using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Rotate : MonoBehaviour
{
    //��ת��
    public float zAngle = 0.1f;

    private Vector3 rotationCenter; // ��ת����
    // Start is called before the first frame update
    void Start()
    {
        UpdateRotationCenter();
    }

    // Update is called once per frame
    void Update()
    {
        SetRotationCenter();
        //������ת���ٶ�zAngle
        transform.RotateAround(rotationCenter, Vector3.forward, zAngle);
    }
    void UpdateRotationCenter()
    {
        rotationCenter = transform.position; // ������ת����Ϊ��ǰλ��
    }

    // �����������λ�ÿ��ܷ����仯�Ļ������ʵ���ʱ��������������������ת����
    public void SetRotationCenter()
    {
        UpdateRotationCenter();
    }
}
