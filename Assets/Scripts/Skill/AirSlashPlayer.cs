using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirSlashPlayer : Skill
{
    // Start is called before the first frame update
    bool isDestory;
    ParticleSystem PS;
    TrailRenderer Trail;
    Rigidbody2D ACrigidbody2D;

    float StartTimer;
    SpriteRenderer spriteRenderer;
    bool isPSStop;

    Vector2 direction;
    Vector3 StartPostion;



    private void Start()
    {
        PS = GetComponent<ParticleSystem>();
        animator = GetComponent<Animator>();
        direction = Quaternion.AngleAxis(transform.eulerAngles.z, Vector3.forward) * Vector2.right;
        ACrigidbody2D = GetComponent<Rigidbody2D>();
        StartPostion = transform.position;
        spriteRenderer = GetComponent<SpriteRenderer>();
        Trail = transform.GetChild(0).GetComponent<TrailRenderer>();
        GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.2f);
        transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        transform.GetChild(1).position = transform.position + Vector3.down * 0.25f;
        transform.GetChild(2).position = transform.position + Vector3.down * 0.25f;
        transform.rotation = Quaternion.Euler(0, 0, transform.eulerAngles.z - 90);

        Timer.Start(this, 0.1f, () => {
        if (SkillFrom == 2)
        {
            int PlusValue = (int)( Mathf.Ceil( Mathf.Clamp( KOPoint - 5 , 0 , 10) ));//向上取整
            SpDamage += PlusValue * 5;
        }
        });
    }

    private void Update()
    {

        //this.transform.localScale += new Vector3(Time.deltaTime * 2, 0, 0);
        StartExistenceTimer();
        if (ExistenceTime <= 0.4f)
        {
            isDestory = true;
        }
        if (isDestory)
        {
            animator.SetTrigger("Over");
            spriteRenderer.material.color = spriteRenderer.material.color - new Color(0, 0, 0, 3f * Time.deltaTime);
            if (spriteRenderer.material.color.a <= 0.1f)
            {
                Destroy(gameObject);
            }
            if (!isPSStop)
            {
                isPSStop = true;
                var Emit = PS.emission;
                Emit.enabled = false;
                Trail.enabled = false;
            }
        }
        else
        {
            Vector3 postion = transform.position;
            postion.x += direction.x * 12f * Time.deltaTime;
            postion.y += direction.y * 12f * Time.deltaTime;
            transform.position = postion;
            if ((StartPostion - transform.position).magnitude > MaxRange)
            {
                isDestory = true;

            }
        }


    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Empty" || other.tag == "Room")
        {
            isDestory = true;
            Empty target = other.GetComponent<Empty>();
            if (target != null)
            {
                HitAndKo(target);
                if (Random.Range(0.0f , 1.0f) + ((float)player.LuckPoint / 15.0f ) > 0.7f )
                {
                    target.Fear(4.0f , 1);
                }
            }
        }
    }
}
