using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwiftStar : Skill
{
    public int index;
    public float angleIncrement;
    public float rotateSpeed;//星星自转速度
    public float surrundSpeed;//星星公转速度
    public float Speed;//攻击时的速度
    public Transform center;//中心点
    public float radius;//半径

    public bool attack;
    public GameObject target;
    private float progress = 0;
    private bool hit;
    private float timer;

    private void Start()
    {
        attack = false;
        hit = false;
    }

    void Update()
    {
        if (!hit)
        {
            StartExistenceTimer();
            if (target == null)
            {
                progress += Time.deltaTime * surrundSpeed;
                if (progress >= 360)
                {
                    progress -= 360;
                }
                float x1 = center.position.x + radius * Mathf.Cos(progress + index * angleIncrement * Mathf.Deg2Rad);
                float y1 = center.position.y + radius * Mathf.Sin(progress + index * angleIncrement * Mathf.Deg2Rad) + 0.5f;
                transform.position = new Vector3(x1, y1);
                transform.Rotate(0, 0, rotateSpeed);
            }
            else
            {
                transform.Rotate(0, 0, rotateSpeed);
                transform.position = Vector2.MoveTowards(transform.position, target.transform.position, Speed * Time.deltaTime);
            }
        }
        else
        {
            timer += Time.deltaTime;
            GetComponent<SpriteRenderer>().enabled = false;
            GameObject HitEffect = transform.GetChild(0).gameObject;
            HitEffect.SetActive(true);
            HitEffect.transform.localScale = Vector3.Lerp(new Vector3(0, 0, 0), new Vector3(2f, 2f, 0), timer * 5f);
            SpriteRenderer sr = HitEffect.GetComponent<SpriteRenderer>();
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, Mathf.Lerp(1, 0, timer * 5f));
            if(timer > 1f)
            {
                Destroy(this.gameObject);
            }
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
                GetComponent<Collider2D>().enabled = false;
                hit = true;
            }
        }
    }
}
