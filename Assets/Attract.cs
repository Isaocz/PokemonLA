using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attract : Skill
{
    Vector2 direction;
    Vector3 StartPostion;
    bool isCanNotMove;
    ParticleSystem AttractPS;
    ParticleSystem AttractOverPS;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        direction = (transform.rotation * Vector2.right).normalized;
        StartPostion = transform.position;
        AttractOverPS = transform.GetChild(1).GetComponent<ParticleSystem>();
        AttractPS = GetComponent<ParticleSystem>();
        transform.rotation = Quaternion.Euler(Vector3.zero);
    }

    private void FixedUpdate()
    {
        if (!isCanNotMove)
        {
            Vector3 postion = transform.position;
            postion.x += direction.x * 4.5f * Time.deltaTime;
            postion.y += direction.y * 4.5f * Time.deltaTime;
            transform.position = postion;
            if ((StartPostion - transform.position).magnitude > MaxRange)
            {
                isCanNotMove = true;
                AttractOverPS.gameObject.SetActive(true);
                AttractOverPS.Play();
                AttractPS.Stop();
                animator.SetTrigger("Break");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Empty")
        {
            Empty target = other.GetComponent<Empty>();
            target.EmptyInfatuation(30, 2.5f);
            isCanNotMove = true;
            AttractOverPS.gameObject.SetActive(true);
            AttractOverPS.Play();
            AttractPS.Stop();
            animator.SetTrigger("Break");
        }
        else if (other.tag == "Room" || (SkillFrom != 2 && other.tag == "Enviroment"))
        {
            isCanNotMove = true;
            AttractOverPS.gameObject.SetActive(true);
            AttractOverPS.Play();
            AttractPS.Stop();
            animator.SetTrigger("Break");
        }
    }
}
