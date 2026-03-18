using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectivireEarthquake : MonoBehaviour
{
    /// <summary>
    /// 地震时间
    /// </summary>
    public float EarthquakeTime;

    /// <summary>
    /// 父电击魔兽
    /// </summary>
    public Electivire ParentElectivire;

    /// <summary>
    /// 威力
    /// </summary>
    public int Dmage;

    /// <summary>
    /// 击退值
    /// </summary>
    public float KOPoint;


    /// <summary>
    /// 动画机
    /// </summary>
    Animator animator;

    /// <summary>
    /// 粒子特效1
    /// </summary>
    ParticleSystem PS1;

    /// <summary>
    /// 粒子特效1
    /// </summary>
    ParticleSystem PS2;



    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        PS1 = transform.GetChild(2).GetChild(0).GetComponent<ParticleSystem>();
        PS2 = transform.GetChild(2).GetChild(1).GetComponent<ParticleSystem>();

        Timer.Start(this , EarthquakeTime, ()=> { EarthquakeOver(); });
    }


    public void EarthquakeOver()
    {
        animator.SetTrigger("Over");
        var m1 = PS1.main;
        m1.loop = false;
        var m2 = PS2.main;
        m2.loop = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (ParentElectivire != null && collision.gameObject.tag == "Player")
        {
            PlayerControler p = collision.gameObject.GetComponent<PlayerControler>();
            if (p != null)
            {
                Pokemon.PokemonHpChange(ParentElectivire.gameObject, p.gameObject, Dmage, 0, 0, PokemonType.TypeEnum.Ground);
                p.KnockOutPoint = KOPoint;
                p.KnockOutDirection = (p.transform.position - this.transform.position).normalized;
            }
        }
    }


}
