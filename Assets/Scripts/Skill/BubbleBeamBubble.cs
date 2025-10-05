using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleBeamBubble : MonoBehaviour
{
    Vector2 direction;
    Vector3 StartPostion;
    bool isCanNotMove;
    BubbleBeam ParentBubbleBeam;
    Animator animator;
    float Speed;
    float BlastTime;


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        direction = (transform.rotation * Vector2.right).normalized;
        StartPostion = transform.position;
        transform.rotation = Quaternion.Euler(0,0,0);
        ParentBubbleBeam = transform.parent.GetComponent<BubbleBeam>();
        Speed = Random.Range(6.0f , 8.5f);
        BlastTime = Random.Range(2.0f,5.0f);
        transform.localScale *= Random.Range(0.8f,1.2f);
    }


    private void FixedUpdate()
    {
        if (!isCanNotMove)
        {
            Vector3 postion = transform.position;
            postion.x += direction.x * Mathf.Clamp(Speed , 0.0f , 10.0f) * Time.deltaTime;
            postion.y += direction.y * Mathf.Clamp(Speed, 0.0f, 10.0f) * Time.deltaTime;
            Speed -= 7.0f * Time.deltaTime;
            transform.position = postion;
            if ((StartPostion - transform.position).magnitude > ParentBubbleBeam.MaxRange || Speed <= -BlastTime)
            {
                BubbleBreak();

            }

        }
    }

    void BubbleBreak()
    {
        if (!isCanNotMove)
        {
            transform.GetComponent<Collider2D>().enabled = false;
            isCanNotMove = true;
            animator.SetTrigger("Blast");
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.isTrigger == false)
        {
            if (other.tag == "Empty")
            {
                Empty target = other.GetComponent<Empty>();
                BubbleBreak();
                if (target != null)
                {
                    if (ParentBubbleBeam.SkillFrom == 2 && target.isSpeedChange) { ParentBubbleBeam.SpDamage += 25; }
                    ParentBubbleBeam.HitAndKo(target);
                    if (Random.Range(0.0f , 1.0f) + ((float)ParentBubbleBeam.player.LuckPoint / 30.0f) > 0.9f ) { target.SpeedChange(); target.SpeedRemove01(3.0f * target.OtherStateResistance); }
                }
            }
            else if (other.tag == "Room" || other.tag == "Enviroment")
            {
                BubbleBreak();
            }
        }
    }

    void DestroySelf()
    {
        Destroy(gameObject);
    }
}
