using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParAnimation : MonoBehaviour
{
    public float radius = 2f;//�뾶
    public Color startColor;//��ʼ��ɫ

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
        }
        else
        {
            Destroy(gameObject);
        }
        shapeModule.radius = radius * timerrate;
    }
}
