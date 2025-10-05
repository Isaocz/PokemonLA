using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillColliderRangeChangeByTime : MonoBehaviour
{
    public float FadeInTime; 
    public float FadeOutTime;
    public float StartDelay;

    CircleCollider2D Collider2D;
    float MaxCollider2DRadius;

    Skill ParentSkill;
    float Duration;
    public float EmptySkillDuration;

    float Timer;

    private void Awake()
    {
        Collider2D = GetComponent<CircleCollider2D>();
        Collider2D.enabled = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        ParentSkill = GetComponent<Skill>();
        MaxCollider2DRadius = Collider2D.radius;
        if (ParentSkill != null && EmptySkillDuration == 0) { Duration = ParentSkill.ExistenceTime; }
        else { Duration = EmptySkillDuration; }
        Collider2D.radius = 0;
        Timer = Duration;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale != 0) {
            Timer -= Time.deltaTime;
            if (Timer < Duration - StartDelay) {
                Collider2D.enabled = true;
                if (Timer <= FadeOutTime) { 
                    Collider2D.radius = Mathf.Clamp(Collider2D.radius - ((Time.deltaTime) * MaxCollider2DRadius) / FadeOutTime, 0, MaxCollider2DRadius);
                    if (Collider2D.radius < 0.01f)
                    {
                        Collider2D.enabled = false;
                    }
                }
                else { Collider2D.radius = Mathf.Clamp(Collider2D.radius + ((Time.deltaTime) * MaxCollider2DRadius) / FadeInTime, 0, MaxCollider2DRadius); }
            }
        }
    }

}
