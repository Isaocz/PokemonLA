using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicalFire : Skill
{
    public float moveSpeed = 5f; // 移动速度
    public float rotationSpeed = 50f; // 旋转速度

    private Rigidbody2D rb;
    private Vector2 moveDirection;

    public bool isSpDownDone;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        moveDirection = transform.right;
    }

    void Update()
    {
        StartExistenceTimer();
        // 移动对象
        rb.velocity = moveDirection * moveSpeed;

        // 绕自身几何中心旋转
        transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
    }
}

