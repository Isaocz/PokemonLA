using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParAnimation : MonoBehaviour
{
    public float radius = 2f;//�뾶
    public Color startColor;//��ʼ��ɫ
    public bool SlowDown;

    private float duration;//����ʱ��
    private float timer;//��ʱ��
    private float timerrate;//ʣ��ʱ�����
    private new ParticleSystem particleSystem;
    private ParticleSystem.ShapeModule shapeModule;//��״ģ��
    private ParticleSystem.MainModule mainModule;//��ģ��
    void Start()
    {
        particleSystem = GetComponent<ParticleSystem>();
        mainModule = particleSystem.main;
        shapeModule = particleSystem.shape;

        duration = mainModule.duration + mainModule.startLifetime.constantMax;//�ܳ���ʱ��
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
                AnimationCurve curve = new AnimationCurve(new Keyframe(0, 1), new Keyframe(1, 0)); // ����һ���򵥵ļ�������
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
