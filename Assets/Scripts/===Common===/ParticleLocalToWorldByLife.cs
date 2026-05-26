using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class ParticleLocalToWorldByLife_NoColor : MonoBehaviour
{
    ParticleSystem ps;
    ParticleSystem.Particle[] particles;

    Vector3 lastWorldPos;

    void Awake()
    {
        ps = GetComponent<ParticleSystem>();
        particles = new ParticleSystem.Particle[ps.main.maxParticles];
        lastWorldPos = transform.position;
    }

    void LateUpdate()
    {
        int count = ps.GetParticles(particles);

        Vector3 worldDelta = transform.position - lastWorldPos; // 粒子系统本帧移动量

        for (int i = 0; i < count; i++)
        {
            float lifePercent = 1f - (particles[i].remainingLifetime / particles[i].startLifetime);

            if (lifePercent < 0.5f)
                continue;

            // 已切换的粒子：randomSeed 最低位为 1
            bool switched = (particles[i].randomSeed & 1) == 1;

            if (!switched)
            {
                // 第一次切换：把 local pos 转成 world pos
                Vector3 worldPos = transform.TransformPoint(particles[i].position);
                particles[i].position = worldPos;

                // 标记为已切换
                particles[i].randomSeed |= 1u;
            }
            else
            {
                // 已切换：补偿粒子系统的移动，使其保持在世界中
                particles[i].position -= worldDelta;
            }
        }

        ps.SetParticles(particles, count);
        lastWorldPos = transform.position;
    }
}