using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swift : Skill
{
    public float moveSpeed;
    private bool isMove;

    private void Start()
    {
        isMove = true;
    }
    void Update()
    {
        StartExistenceTimer();
        if (isMove)
        {
            transform.Translate(moveSpeed * Vector3.right * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Empty"))
        {
            Empty target = collision.GetComponent<Empty>();
            if (target != null)
            {
                HitAndKo(target);
                Destroy(gameObject);
            }
        }
        else if ((collision.CompareTag("Enviroment") || collision.CompareTag("Room")) && SkillFrom != 2)
        {
            isMove = false;
        }
    }
}
