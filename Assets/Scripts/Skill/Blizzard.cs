using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blizzard : Skill
{
    bool IsFrozenDone = false;

    float BlizzardRange;
    float ColliderSizeChangeTimer;
    CircleCollider2D BlizzardCollider2D;
    ParticleSystem BlizzardPS1;
    ParticleSystem BlizzardPS2;
    ParticleSystem BlizzardPS3;

    // Start is called before the first frame update
    void Start()
    {
        BlizzardCollider2D = GetComponent<CircleCollider2D>();
        BlizzardPS1 = transform.GetChild(0).GetComponent<ParticleSystem>();
        BlizzardPS2 = transform.GetChild(1).GetComponent<ParticleSystem>();
        BlizzardPS3 = transform.GetChild(2).GetComponent<ParticleSystem>();
        var BlizzardPS1Shape = BlizzardPS1.shape;
        var BlizzardPS2Shape = BlizzardPS2.shape;
        var BlizzardPS3Shape = BlizzardPS3.shape;
        if (SkillFrom == 2)
        {
            if (Weather.GlobalWeather.isHail) {
                BlizzardRange = 3.2f;
                BlizzardPS1Shape.radius = 2.1f; BlizzardPS1Shape.donutRadius = 1.1f;
                BlizzardPS2Shape.radius = 2.1f; BlizzardPS2Shape.donutRadius = 1.1f;
                BlizzardPS3Shape.radius = 2.1f; BlizzardPS3Shape.donutRadius = 1.1f;
            }
            else
            {
                BlizzardRange = 2.6f;
                BlizzardPS1Shape.radius = 1.7f; BlizzardPS1Shape.donutRadius = 0.9f;
                BlizzardPS2Shape.radius = 1.7f; BlizzardPS2Shape.donutRadius = 0.9f;
                BlizzardPS3Shape.radius = 1.7f; BlizzardPS3Shape.donutRadius = 0.9f;
            }
        }
        else
        {
            if (Weather.GlobalWeather.isHail)
            {
                BlizzardRange = 2.6f;
                BlizzardPS1Shape.radius = 1.7f; BlizzardPS1Shape.donutRadius = 0.9f;
                BlizzardPS2Shape.radius = 1.7f; BlizzardPS2Shape.donutRadius = 0.9f;
                BlizzardPS3Shape.radius = 1.7f; BlizzardPS3Shape.donutRadius = 0.9f;
            }
            else
            {
                BlizzardRange = 1.8f;
                BlizzardPS1Shape.radius = 1.25f; BlizzardPS1Shape.donutRadius = 0.55f;
                BlizzardPS2Shape.radius = 1.25f; BlizzardPS2Shape.donutRadius = 0.55f;
                BlizzardPS3Shape.radius = 1.25f; BlizzardPS3Shape.donutRadius = 0.55f;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        StartExistenceTimer();

        ColliderSizeChangeTimer += Time.deltaTime;
        if (ColliderSizeChangeTimer <= 1.1f ) { BlizzardCollider2D.radius = Mathf.Clamp(ColliderSizeChangeTimer * BlizzardRange, 0, BlizzardRange); }
        if (ColliderSizeChangeTimer >= 5.5f ) { BlizzardCollider2D.radius = Mathf.Clamp((6.5f-ColliderSizeChangeTimer) * BlizzardRange, 0, BlizzardRange); }
    }
    private void OnTriggerStay2D(Collider2D other)
    {

        if (other.tag == "Empty")
        {
            Empty target = other.GetComponent<Empty>();
            HitAndKo(target);
            if (!IsFrozenDone)
            {
                target.Frozen(7.5f, 1, 0.1f + (float)player.LuckPoint / 30);
                IsFrozenDone = true;
            }
        }
    }
}
