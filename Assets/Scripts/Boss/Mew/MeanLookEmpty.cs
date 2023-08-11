using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeanLookEmpty : Projectile
{
    public List<Transform> flamesList; // �������Ӷ�����б�
    public float rotationSpeed; // ��ת�ٶ�
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 3.5f);
    }

    // Update is called once per frame
    void Update()
    {
        float rotationAngle = rotationSpeed * Time.deltaTime;

        // �����������Ӷ����б���ÿ�����������ת
        foreach (Transform flame in flamesList)
        {
            flame.RotateAround(transform.position, transform.forward, rotationAngle);
        }
    }


}
