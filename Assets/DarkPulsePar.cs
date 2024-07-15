using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkPulsePar : Skill
{
    public float fadeTime = 0.5f;
    private float diffuseRadius;
    private float Duration;
    private SpriteRenderer sr;
    private Vector3 targetPosition;
    private Vector3 startPosition;

    public void Initialize(float radius, float duration, float spDamage)
    {
        diffuseRadius = radius;
        Duration = duration;
        SpDamage = spDamage;
    }

    private void Start()
    {
        ExistenceTime = Duration;
        sr = GetComponent<SpriteRenderer>();
        startPosition = transform.position;
        targetPosition = transform.position + (Vector3)Random.insideUnitCircle * diffuseRadius;
    }

    void Update()
    {
        StartExistenceTimer();
        transform.position = Vector3.Lerp(startPosition, targetPosition, (1 - ExistenceTime / Duration));
        if(ExistenceTime < fadeTime)
        {
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, ExistenceTime / fadeTime);
        }
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Empty"))
        {
            Empty enemy = collision.GetComponent<Empty>();
            if( enemy != null )
            {
                HitAndKo(enemy);
                targetPosition = transform.position;
                if (Random.Range(0.0f, 1.0f) + ((float)player.LuckPoint / 15.0f) > 0.8f)
                {
                    enemy.Fear(4.0f, 1);
                }
            }
        }
        if(collision.CompareTag("Enviroment") || collision.CompareTag("Room"))
        {
            targetPosition = transform.position;
        }
    }

}
