using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeanLookEmpty : Projectile
{
    public List<Transform> flamesList; // 火焰粒子对象的列表
    public float rotationSpeed; // 旋转速度
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 3.5f);
    }

    // Update is called once per frame
    void Update()
    {
        float rotationAngle = rotationSpeed * Time.deltaTime;

        // 遍历火焰粒子对象列表，对每个对象进行旋转
        foreach (Transform flame in flamesList)
        {
            flame.RotateAround(transform.position, transform.forward, rotationAngle);
        }
    }


}
