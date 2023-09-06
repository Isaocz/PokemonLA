using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeafBlade : Skill
{
    private int attackCount = 0;
    private bool isMoving = true;
    private bool changingSpeed = false;
    private Empty targetEnemy;
    private float attackDetectionDelay = 1f;
    private float moveSpeed = 12f; // 控制移动速度
    private float timer;

    // Update is called once per frame
    void Update()
    {
        if (changingSpeed)
        {
            timer += Time.deltaTime;
            moveSpeed = 12f * Mathf.Abs(1f / 0.8f * timer - 1f);
            if(timer > 1.6f)
            {
                changingSpeed = false;
            }
        }
        if (isMoving)
        {
            transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Empty")
        {
            attackCount++;
            targetEnemy = other.GetComponent<Empty>();
            HitAndKo(targetEnemy);

            if (attackCount >= 3)
            {
                // 如果连续三次攻击到敌人，则停止移动并销毁对象
                StopMoving();
            }
            else
            {
                changingSpeed = true;
                if (changingSpeed)
                {
                    timer = 0f;
                }
                Invoke("ChangeDirection", attackDetectionDelay);
            }
        }
    }

    private void ChangeDirection()
    {
        if (targetEnemy != null)
        {
            Vector3 directionToEnemy = (targetEnemy.transform.position - transform.position).normalized;
            transform.right = directionToEnemy;
            Invoke("ResumeMoving", attackDetectionDelay);
        }
    }

    private void ResumeMoving()
    {
        isMoving = true;
    }

    private void StopMoving()
    {
        isMoving = false;
        Destroy(gameObject);
    }
}
