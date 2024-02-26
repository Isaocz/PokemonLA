using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterGun : Skill
{
    Vector2 direction;
    Vector3 StartPostion;
    bool isCanNotMove;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        direction = (transform.rotation * Vector2.right).normalized;
        StartPostion = transform.position;
    }

    private void Update()
    {
        StartExistenceTimer();

    }

    private void FixedUpdate()
    {
        if (!isCanNotMove)
        {
            Vector3 postion = transform.position;
            postion.x += direction.x * 9f * Time.deltaTime;
            postion.y += direction.y * 9f * Time.deltaTime;
            transform.position = postion;
            if ((StartPostion - transform.position).magnitude > MaxRange)
            {
                WaterGunBreak();

            }

        }
        if (isCanNotMove)
        {
            transform.GetComponent<SpriteRenderer>().color -= new Color(0, 0, 0, 3.0f * Time.deltaTime);
        }
    }

    void WaterGunBreak()
    {
        if (!isCanNotMove)
        {
            var e = transform.GetChild(1).GetComponent<ParticleSystem>().emission;
            e.enabled = false;
            transform.GetChild(0).gameObject.SetActive(true);
            transform.GetChild(0).rotation = Quaternion.Euler(0, 0, 0);
            transform.DetachChildren();
            transform.GetComponent<Collider2D>().enabled = false;
            isCanNotMove = true;

            
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.isTrigger == false)
        {
            if (other.tag == "Empty")
            {
                Empty target = other.GetComponent<Empty>();
                WaterGunBreak();
                if (target != null) {
                    HitAndKo(target);
                    if (SkillFrom == 2 && Random.Range(0.0f,1.0f) + ((float)player.LuckPoint/10.0f) >= 0.5f) { target.SpeedChange();target.SpeedRemove01(3.0f * target.OtherStateResistance); }
                }
            }
            else if (other.tag == "Room" || other.tag == "Enviroment")
            {
                WaterGunBreak();
            }
        }
    }
}
