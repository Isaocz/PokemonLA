using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParAnimation : MonoBehaviour
{
    public float radius = 2f;//半径
    public Color startColor;//起始颜色
    public bool SlowDown;

    private float duration;//持续时间
    private float timer;//计时器
    private float timerrate;//剩余时间比例
    private new ParticleSystem particleSystem;
    private ParticleSystem.ShapeModule shapeModule;//形状模组
    private ParticleSystem.MainModule mainModule;//主模块
    void Start()
    {
        particleSystem = GetComponent<ParticleSystem>();
        mainModule = particleSystem.main;
        shapeModule = particleSystem.shape;

        duration = mainModule.duration + mainModule.startLifetime.constantMax;//总持续时间
        mainModule.startColor = startColor;
        timer = duration;
    }

    void Update()
    {
        if(timer > 0)
        {
            timer -= Time.deltaTime;
            timerrate = timer / duration;
            if (SlowDown)
            {
                var velocityOverLifetime = particleSystem.velocityOverLifetime;
                AnimationCurve curve = new AnimationCurve(new Keyframe(0, 1), new Keyframe(1, 0)); // 定义一个简单的减速曲线
                velocityOverLifetime.x = new ParticleSystem.MinMaxCurve(1f, curve);
                velocityOverLifetime.y = new ParticleSystem.MinMaxCurve(1f, curve);
                velocityOverLifetime.z = new ParticleSystem.MinMaxCurve(1f, curve);
            }
        }
        else
        {
            Destroy(gameObject);
        }
        shapeModule.radius = radius * timerrate;
    }
}
