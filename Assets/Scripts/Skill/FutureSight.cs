using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FutureSight : Skill
{
    public float moveSpeed;
    private bool ishit;

    bool isLunch;

    GameObject Sprite;
    Collider2D c;
    TraceEffect t;


    // Start is called before the first frame update
    void Start()
    {
        ishit = false;

        animator = GetComponent<Animator>();
        Sprite = transform.GetChild(0).gameObject;
        c = GetComponent<Collider2D>();
        t = GetComponent<TraceEffect>();
    }

    // Update is called once per frame
    void Update()
    {
        StartExistenceTimer();
        if (!isLunch && ExistenceTime <= 2.5f)
        {
            transform.parent = null;
            Sprite.SetActive(true);
            animator.enabled = true;
            c.enabled = true;
            t.enabled = true;
            isLunch = true;
            transform.rotation = Quaternion.Euler(0, 0, _mTool.Angle_360Y(player.look, Vector3.right));
            transform.position = player.transform.position + (Vector3.up * player.SkillOffsetforBodySize[0]) + ((Vector3)player.look * 1.0f) + ((Vector3)player.look * player.SkillOffsetforBodySize[1]);
        }
        if (isLunch) {
            if (!ishit && ExistenceTime >= 0.3f)
            {
                transform.Translate(moveSpeed * Time.deltaTime * Vector3.right);
            }
            else
            {
                animator.SetTrigger("Over");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isLunch) {
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
            if ((collision.CompareTag("Enviroment") || collision.CompareTag("Room")) && !ishit)
            {
                ishit = true;
                animator.SetTrigger("Over");
            }
        }
    }
}
