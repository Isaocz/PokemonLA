using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicalFire : Skill
{
    public float moveSpeed = 5f; // �ƶ��ٶ�
    public float rotationSpeed = 50f; // ��ת�ٶ�

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
        // �ƶ�����
        rb.velocity = moveDirection * moveSpeed;

        // ��������������ת
        transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
    }
}

