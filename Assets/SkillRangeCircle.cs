using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillRangeCircle : MonoBehaviour
{

    public float Duration;
    public float FadeInTime;
    public float FadeOutTime;
    SpriteRenderer CircleSprite;
    float Timer;
    float MaxAlpha;

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
        Timer += Time.deltaTime;

        if (Timer < (Duration - FadeOutTime)) { CircleSprite.color = new Color(CircleSprite.color.r, CircleSprite.color.g, CircleSprite.color.b, Mathf.Clamp(CircleSprite.color.a + (Time.deltaTime * MaxAlpha)/(FadeInTime) , 0,MaxAlpha) ); }
        else{ CircleSprite.color = new Color(CircleSprite.color.r, CircleSprite.color.g, CircleSprite.color.b, Mathf.Clamp(CircleSprite.color.a - (Time.deltaTime * MaxAlpha) / (FadeOutTime), 0, MaxAlpha)); }

        if (Timer > Duration) { Destroy(gameObject); }

    }
}
