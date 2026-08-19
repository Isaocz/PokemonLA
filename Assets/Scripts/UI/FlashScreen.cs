using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 受击闪烁构造体
/// </summary>
[System.Serializable]
public struct FlashEffect
{
    public string name;        // 闪烁类型名称
    public Color color;        // 闪烁颜色
    public float fadeInTime;   // 淡入时间（快）
    public float fadeOutTime;  // 淡出时间（慢）
    public int priority;       // 优先级（高覆盖低）
}




public class FlashScreen : MonoBehaviour
{


    /// <summary>
    /// 小伤害受击闪烁
    /// </summary>
    public FlashEffect smallHit = new FlashEffect
    {
        name = "SmallHit",
        color = new Color(1, 0, 0, 0.4f),
        fadeInTime = 0.05f,
        fadeOutTime = 0.25f,
        priority = 100
    };

    /// <summary>
    /// 大伤害受击闪烁
    /// </summary>
    public FlashEffect bigHit = new FlashEffect
    {
        name = "BigHit",
        color = new Color(1, 0, 0, 0.8f),
        fadeInTime = 0.03f,
        fadeOutTime = 0.5f,
        priority = 200
    };



    public static FlashScreen instance;

    public SpriteRenderer img;

    private FlashEffect currentEffect;
    private bool isFlashing = false;

    private float timer = 0f;
    private float fadeInTime;
    private float fadeOutTime;

    void Awake()
    {
        instance = this;
        img.color = new Color(0, 0, 0, 0);
    }

    void Update()
    {
        if (!isFlashing) return;

        timer -= Time.deltaTime;

        float t;

        if (timer > fadeOutTime)
        {
            // 淡入阶段（快）
            float p = 1f - ((timer - fadeOutTime) / fadeInTime);
            t = Mathf.SmoothStep(0f, 1f, p);
        }
        else
        {
            // 淡出阶段（慢）
            float p = timer / fadeOutTime;
            t = Mathf.SmoothStep(0f, 1f, p);
        }

        Color c = currentEffect.color;
        c.a = Mathf.Lerp(0f, currentEffect.color.a, t);
        img.color = c;

        if (timer <= 0f)
        {
            isFlashing = false;
            img.color = new Color(0, 0, 0, 0);
        }
    }

    /// <summary>
    /// 触发闪烁效果（含优先级判断）
    /// </summary>
    public void PlayFlash(FlashEffect effect)
    {
        // 如果正在闪烁且新效果优先级不够 → 忽略
        if (isFlashing && effect.priority <= currentEffect.priority)
            return;

        // 否则覆盖当前效果
        currentEffect = effect;
        fadeInTime = effect.fadeInTime;
        fadeOutTime = effect.fadeOutTime;

        timer = fadeInTime + fadeOutTime;
        isFlashing = true;
    }
}
