using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Psychic : Skill
{
    public float moveSpeed;
    private bool ishit;
    // Start is called before the first frame update
    void Start()
    {
        ishit = false;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        StartExistenceTimer();
        if (!ishit && ExistenceTime >= 0.3f)
        {
            transform.Translate(moveSpeed * Time.deltaTime * Vector3.right);
        }else
        {
            animator.SetTrigger("Over");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Empty") && !ishit)
        {
            Empty enemy = collision.GetComponent<Empty>();
            if (enemy != null)
            {
                HitAndKo(enemy);
                ishit = true;
                animator.SetTrigger("Over");

                if (Random.Range(0f, 1f) + player.LuckPoint / 30f > 0.8)
                    enemy.SpDChange(-1, 0f);
            }
        }
        if((collision.CompareTag("Enviroment") || collision.CompareTag("Room")) && !ishit)
        {
            ishit = true;
            animator.SetTrigger("Over");
        }
    }
}
