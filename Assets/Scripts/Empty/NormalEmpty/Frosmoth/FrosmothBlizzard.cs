using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrosmothBlizzard : MonoBehaviour
{
    //暴风雪等级
    public int BlizzardLevel ;
    //雪融蛾
    public Empty ParentEmpty;
    //威力
    public int SpDmage;

    //碰撞箱
    public CircleCollider2D SkillCollider2D;
    static List<float> Size_SkillCollider2D = new List<float> { 1.55f, 1.9f, 2.25f ,2.6f };

    //指示圈
    public GameObject SkillCircle;
    static List<float> Size_SkillCircle = new List<float> { 0.79f, 0.97f, 1.15f, 1.32f };

    //粒子1
    public ParticleSystem BlizzardPS1;
    static List<float> Size_BlizzardPS1_Emission = new List<float> { 172.0f, 211.0f, 250.0f, 287.5f };
    static List<float> Size_BlizzardPS1_SizeMax = new List<float> { 1.24f, 1.52f, 1.8f, 2.07f };
    static List<float> Size_BlizzardPS1_SizeMin = new List<float> { 0.55f, 0.676f, 0.8f, 0.92f };
    static List<float> Size_BlizzardPS1_Radius = new List<float> { 1.24f, 1.52f, 1.8f, 2.07f };

    //粒子2
    public ParticleSystem BlizzardPS2;
    static List<float> Size_BlizzardPS2_Emission = new List<float> { 6.89f, 8.45f, 10.0f, 11.5f };
    static List<float> Size_BlizzardPS2_Radius = new List<float> { 1.17f, 1.435f, 1.7f, 1.995f };

    //粒子3
    public ParticleSystem BlizzardPS3;
    static List<float> Size_BlizzardPS3_Emission = new List<float> { 68.9f, 84.5f, 100.0f, 115.0f };
    static List<float> Size_BlizzardPS3_Radius = new List<float> { 1.24f, 1.52f, 1.8f, 2.07f };


    /// <summary>
    /// 设置暴风雪的大小
    /// </summary>
    public void setBlizzardSize()
    {
        BlizzardLevel = Mathf.Clamp(BlizzardLevel , 0 , 3);

        //设置碰撞箱半径
        SkillCollider2D.radius = Size_SkillCollider2D[BlizzardLevel];
        //设置指示圈大小
        SkillCircle.transform.localScale = new Vector3(Size_SkillCircle[BlizzardLevel] , Size_SkillCircle[BlizzardLevel] , 0);
        //设置粒子1
        var main1 = BlizzardPS1.main;
        var emission1 = BlizzardPS1.emission;
        var shape1 = BlizzardPS1.shape;
        emission1.rateOverTime = Size_BlizzardPS1_Emission[BlizzardLevel];
        main1.startSize = new ParticleSystem.MinMaxCurve(Size_BlizzardPS1_SizeMin[BlizzardLevel] , Size_BlizzardPS1_SizeMax[BlizzardLevel]);
        shape1.radius = Size_BlizzardPS1_Radius[BlizzardLevel];
        //设置粒子2
        var emission2 = BlizzardPS2.emission;
        var shape2 = BlizzardPS2.shape;
        emission2.rateOverTime = Size_BlizzardPS2_Emission[BlizzardLevel];
        shape2.radius = Size_BlizzardPS2_Radius[BlizzardLevel];
        //设置粒子3
        var emission3 = BlizzardPS3.emission;
        var shape3 = BlizzardPS3.shape;
        emission3.rateOverTime = Size_BlizzardPS3_Emission[BlizzardLevel];
        shape3.radius = Size_BlizzardPS3_Radius[BlizzardLevel];

        BlizzardPS1.Play();
        BlizzardPS2.Play();
        BlizzardPS3.Play();
    }


    private void Start()
    {
        Destroy(gameObject , 8.0f);
    }


    private void OnTriggerStay2D(Collider2D other)
    {
        if (ParentEmpty != null)
        {
            //未被魅惑时
            if (!ParentEmpty.isEmptyInfatuationDone && other.tag == "Player")
            {
                PlayerControler p = other.GetComponent<PlayerControler>();
                Pokemon.PokemonHpChange(ParentEmpty.gameObject, other.gameObject, 0, SpDmage, 0, PokemonType.TypeEnum.Ice);
                p.PlayerFrozenFloatPlus(0.3f, 1.2f);
                if (p != null)
                {
                    p.KnockOutPoint = 1.5f;
                    p.KnockOutDirection = (p.transform.position - transform.position).normalized;
                }
            }
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (ParentEmpty != null)
        {
            //被魅惑时
            if (ParentEmpty.isEmptyInfatuationDone && other.tag == "Empty" && other.gameObject != ParentEmpty.gameObject)
            {
                Empty e = other.GetComponent<Empty>();
                e.Frozen(3.5f , 0.5f , 1.0f);
                Pokemon.PokemonHpChange(ParentEmpty.gameObject, other.gameObject, 0, SpDmage, 0, PokemonType.TypeEnum.Ice);
            }
        }
    }



}
