using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoredPowerEffect : Skill
{
    private float moveSpeed;
    private float timer;
    public Empty target;

    // Start is called before the first frame update
    void Start()
    {
        timer = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        timer += 3 * Time.deltaTime;
        moveSpeed = Mathf.Exp(timer);
        Vector3 targetPos = target ? target.transform.position : transform.position;
        transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);

        if (timer >= 5f)
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Empty"))
        {
            Empty enemy = collision.GetComponent<Empty>();
            if (target != null)
            {
                HitAndKo(enemy);
                Destroy(gameObject);
            }
        }
        if ((collision.CompareTag("Enviroment") || collision.CompareTag("Room")) && SkillFrom != 2)
        {
            Destroy(gameObject);
        }
    }
}
