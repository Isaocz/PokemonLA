using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvaluggFrozenMistCollision : MonoBehaviour
{
    //�䶳ʱ��͵���
    public float FrozenPoint;
    public float FrozenTime;
    //�Ƿ����ڲ���
    public bool isPlay = true;
    /// <summary>
    /// ��ײ��
    /// </summary>
    Collider2D FrozenMistCollider;
    /// <summary>
    /// ����Ч��
    /// </summary>
    ParticleSystem PS;

    private void Awake()
    {
        FrozenMistCollider = transform.GetComponent<Collider2D>();
        PS = transform.GetComponent<ParticleSystem>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    public void StartFrozenMist()
    {
        FrozenMistCollider.enabled = true;
        PS.Play();
        isPlay = true;
    }

    public void StopFrozenMist()
    {
        FrozenMistCollider.enabled = false;
        PS.Stop();
        isPlay = false;
        
        ParticleSystem.Particle[] particles = new ParticleSystem.Particle[PS.main.maxParticles];
        int count = PS.GetParticles(particles);

        for (int i = 0; i < count; i++)
        {
            particles[i].remainingLifetime *= 0.4f; // ��ʣ����������Ϊԭ���� 15%
        }

        PS.SetParticles(particles, count);
        
    }


    private void OnTriggerStay2D(Collider2D other)
    {
        PlayerControler p = other.GetComponent<PlayerControler>();
        if (p != null)
        {
            p.PlayerFrozenFloatPlus(FrozenPoint, FrozenTime);
            Pokemon.PokemonHpChange(null, other.gameObject, 1, 0, 0, PokemonType.TypeEnum.IgnoreType);
            p.KnockOutPoint = 3f;
            p.KnockOutDirection = (p.transform.position - transform.position).normalized;
        }
    }
}
