using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swift : Skill
{
    public float moveSpeed;
    private float timer;
    private bool isMove;
    private bool isHit;
    private bool isRotate;

    GameObject star1;
    GameObject star2;
    GameObject star3;
    GameObject star4;

    private void Start()
    {
        isMove = true;
        isRotate = true;
        star1 = transform.GetChild(0).gameObject;
        star2 = star1.transform.GetChild(0).gameObject;
        star3 = star1.transform.GetChild(1).gameObject;
        star4 = star1.transform.GetChild(2).gameObject;
    }
    void Update()
    {
        if (isMove)
        {
            transform.Translate(moveSpeed * Vector3.right * Time.deltaTime);
            if(SkillFrom == 2)
            {
                GetComponent<TraceEffect>().moveSpeed = 0;
            }
        }
        if (isHit)
        {//让四个星星开始消失，为了保留他们的轨迹
            timer += Time.deltaTime;
            starFade(star1, timer);
            starFade(star2, timer);
            starFade(star3, timer);
            starFade(star4, timer);
            if (isRotate)
            {//停止星星旋转
                star1.GetComponent<Rotate>().zAngle = 0;
                isRotate = false;
            }
            if (timer > 1f)
            {
                Destroy(gameObject);
            }
            if(SkillFrom == 2)
            {
                transform.GetChild(1).gameObject.GetComponent<ParticleSystem>().Stop();
            }
        }
        else
        {
            StartExistenceTimer();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {//击中敌怪或者墙壁停止移动
        if (collision.CompareTag("Empty"))
        {
            Empty target = collision.GetComponent<Empty>();
            if (target != null)
            {
                HitAndKo(target);
                isHit = true;
                isMove = false;
            }
        }
        else if ((collision.CompareTag("Enviroment") || collision.CompareTag("Room")) && SkillFrom != 2)
        {
            isHit = true;
            isMove = false;
        }
    }

    void starFade(GameObject star, float timer)
    {//使星星变淡
        SpriteRenderer sr = star.GetComponent<SpriteRenderer>();
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, Mathf.Lerp(1f, 0f, timer));
    }
}
