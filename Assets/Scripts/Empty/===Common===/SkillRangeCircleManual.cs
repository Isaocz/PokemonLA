using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillRangeCircleManual : MonoBehaviour
{
    public float StartDelay;

    public float FadeInTime;
    public float FadeOutTime;
    SpriteRenderer CircleSprite;
    public float Timer;
    float MaxAlpha;

    bool isOver = false;

    // Start is called before the first frame update
    void Start()
    {
        CircleSprite = GetComponent<SpriteRenderer>();
        MaxAlpha = CircleSprite.color.a;
        CircleSprite.color = new Color(CircleSprite.color.r, CircleSprite.color.g, CircleSprite.color.b, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (isOver) { 
            Timer += Time.deltaTime;
            CircleSprite.color = new Color(CircleSprite.color.r, CircleSprite.color.g, CircleSprite.color.b, Mathf.Clamp(CircleSprite.color.a - (Time.deltaTime * MaxAlpha) / (FadeOutTime), 0, MaxAlpha));
            if (CircleSprite.color.a <= 0) { Destroy(gameObject); }
        }
        else
        {
            if (Timer < (StartDelay + FadeInTime))
            {
                Timer += Time.deltaTime;

                if (Timer > StartDelay)
                {
                    CircleSprite.color = new Color(CircleSprite.color.r, CircleSprite.color.g, CircleSprite.color.b, Mathf.Clamp(CircleSprite.color.a + (Time.deltaTime * MaxAlpha) / (FadeInTime), 0, MaxAlpha));

                }
            }
        }
    }

    public void SkillCircleOver()
    {
        isOver = true;
    }
}
