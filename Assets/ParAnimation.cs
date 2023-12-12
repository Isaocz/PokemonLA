using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParAnimation : MonoBehaviour
{
    public float radius = 2f;//半径
    public Color startColor;//起始颜色

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
        }
        else
        {
            Destroy(gameObject);
        }
        shapeModule.radius = radius * timerrate;
    }
}
