using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeanLookEmpty : Projectile
{
    public List<Transform> flamesList; // 火焰粒子对象的列表
    public float rotationSpeed; // 旋转速度
    public float radius;
    private PlayerControler player;
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerControler>();
        Destroy(gameObject, 3.5f);
    }

    // Update is called once per frame
    void Update()
    {
        float rotationAngle = rotationSpeed * Time.deltaTime;
        float distance = Vector2.Distance(player.transform.position, transform.position);
        if (distance > radius)
        {
            // 如果玩家距离黑色目光的距离大于圆半径，则将玩家移动回黑色目光的范围内
            Vector3 direction = (player.transform.position - transform.position).normalized;
            player.transform.position = transform.position + direction * radius;
        }

        // 遍历火焰粒子对象列表，对每个对象进行旋转
        foreach (Transform flame in flamesList)
        {
            flame.RotateAround(transform.position, transform.forward, rotationAngle);
        }
    }


}
