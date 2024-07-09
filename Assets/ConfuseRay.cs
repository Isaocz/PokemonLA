using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfuseRay : Skill
{
    public float moveSpeed = 5f;
    public GameObject confusionEffect;
    void Update()
    {
        transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);
        StartExistenceTimer();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (SkillFrom != 2)
        {
            if (collision.tag == "Enviroment" || collision.tag == "Room")
            {
                Destroy(gameObject);
            }
        }

        if (collision.tag == "Empty")
        {
            Empty target = collision.GetComponent<Empty>();
            if (target != null)
            {
                target.ConfusionFloatPlus(1);
                Destroy(gameObject);
                GameObject effect = Instantiate(confusionEffect, target.transform.position + new Vector3(0,1,2f), Quaternion.identity);
                effect.GetComponent<ConfuseRayEffect>().target = target.transform;
                Destroy(effect, 1f);
            }
        }

    }
}
