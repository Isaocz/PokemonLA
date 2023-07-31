using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snore : Skill
{

    CircleCollider2D SnoreCollider2D;
    float SnoreCollider2DRadius;


    // Start is called before the first frame update
    void Start()
    {
        SnoreCollider2D = GetComponent<CircleCollider2D>();
        SnoreCollider2DRadius = SnoreCollider2D.radius;
        SnoreCollider2D.radius = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (ExistenceTime >= 2.5f) { SnoreCollider2D.radius = Mathf.Clamp(SnoreCollider2D.radius + (Time.deltaTime) * 2 * SnoreCollider2DRadius, 0 , SnoreCollider2DRadius); }
        if (ExistenceTime <= 0.5f) { SnoreCollider2D.radius = Mathf.Clamp(SnoreCollider2D.radius - (Time.deltaTime) * 2 * SnoreCollider2DRadius, 0 , SnoreCollider2DRadius); }
        
        StartExistenceTimer();
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Empty")
        {
            Empty target = other.GetComponent<Empty>();
            HitAndKo(target);
            if (Random.Range(0.0f, 1.0f) + (float)player.LuckPoint / 30 >= 0.7f)
            {
                target.Fear(2.5f, 1);
            }
        }
    }
}
