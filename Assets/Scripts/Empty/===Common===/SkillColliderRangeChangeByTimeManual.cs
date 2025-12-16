using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillColliderRangeChangeByTimeManual : MonoBehaviour
{
    public float FadeInTime;
    public float FadeOutTime;
    public float StartDelay;

    CircleCollider2D Collider2D;
    float MaxCollider2DRadius;

    Skill ParentSkill;

    /// <summary>
    /// 技能圈是否结束
    /// </summary>
    bool isOver;

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
        Collider2D.radius = 0;
        Timer = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale != 0)
        {
            if (isOver)
            {
                Timer += Time.deltaTime;
                Collider2D.radius = Mathf.Clamp(Collider2D.radius - ((Time.deltaTime) * MaxCollider2DRadius) / FadeOutTime, 0, MaxCollider2DRadius);
                if (Collider2D.radius < 0.05f) { Collider2D.enabled = false; }
            }
            else
            {
                if (Timer < StartDelay + FadeInTime)
                {
                    Timer += Time.deltaTime;
                    if (Timer > StartDelay)
                    {
                        Collider2D.enabled = true;
                        Collider2D.radius = Mathf.Clamp(Collider2D.radius + ((Time.deltaTime) * MaxCollider2DRadius) / FadeInTime, 0, MaxCollider2DRadius);

                    }
                }
            }
        }
    }

    public void SkillCircleOver()
    {
        isOver = true;
    }

}
