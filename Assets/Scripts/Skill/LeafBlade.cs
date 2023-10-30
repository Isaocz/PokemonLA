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
    private float moveSpeed = 14f; // 控制移动速度
    private float timer;
    private float atkTimer = 1f;
    private int Counts;

    bool isCountPlus;

    private void Start()
    {
        if(SkillFrom == 2)
        {
            Counts = 3;
        }
        else
        {
            Counts = 2;
        }
    }
    // Update is called once per frame
    void Update()
    {
        StartExistenceTimer();
        atkTimer += Time.deltaTime;
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

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Empty" && atkTimer > 1f)
        {
            atkTimer = 0f;
            attackCount++;
            targetEnemy = other.GetComponent<Empty>();
            HitAndKo(targetEnemy);

            if (attackCount >= Counts)
            {
                // 如果连续三次攻击到敌人，则停止移动并销毁对象（1级2次）
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
        if (SkillFrom == 2 && attackCount >= 1 && !isCountPlus)
        {
            if (other.tag == "Grass")
            {
                NormalGress n = other.GetComponent<NormalGress>();
                GressPlayerINOUT g = other.GetComponent<GressPlayerINOUT>();
                if (n != null)
                {
                    Counts++;
                    isCountPlus = true;
                    n.GrassDie();
                }
                if (g != null)
                {
                    Counts++;
                    isCountPlus = true;
                    g.GrassDie();
                }
            }
        }
    }

    private void ChangeDirection()
    {
        if (targetEnemy != null)
        {
            isCountPlus = false;
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
