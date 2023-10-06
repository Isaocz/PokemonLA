using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowBall : Skill
{
    public float moveSpeed;
    void Update()
    {
        transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Empty"))
        {
            Empty target = collision.GetComponent<Empty>();
            if (target != null)
            {
                HitAndKo(target);
                if(Random.Range(0f,1f) + (float)player.LuckPoint/30 > 0.8f)
                {
                    target.SpdAbilityPoint -= 1;
                }
            }
        }
        if (collision.CompareTag("Enviroment") || collision.CompareTag("Room"))
        {
            Destroy(gameObject);
        }
    }
}
