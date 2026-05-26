using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CetitanIceShard : Projectile
{
    bool isDestory;
    public float MoveSpeed;

    bool isCanNotMove;


    //冰砾速度和最远距离
    float IceshardSpeed;
    float IceshardMaxDis = 20.0f;
    int IceshardDmage;



    private void Awake()
    {
        AwakeProjectile();
        Timer.Start(this, 3.0f, () => { if (!isDestory) { isDestory = true; } });
    }




    private void Update()
    {
        //this.transform.localScale += new Vector3(Time.deltaTime * 2, 0, 0);
        if (!isCanNotMove)
        {
            DestoryByRange(IceshardMaxDis);
            if (isDestory)
            {
                IceBreak();
            }
            else
            {
                MoveNotForce();
            }
        }


    }


    public override void DestoryByRange(float ProjectileRange)
    {
        if ((transform.position - BornPosition).magnitude >= ProjectileRange)
        {
            IceBreak();
        }
    }

    /// <summary>
    /// 冰砾裂开
    /// </summary>
    /// <param name="BreakReason">裂开原因 0距离到达极限 1撞到敌人 2撞到墙壁</param>
    public void IceBreak()
    {

        isCanNotMove = true;
        GetComponent<Animator>().SetTrigger("Break");
        if (transform.childCount > 0)
        {
            var Emission1 = transform.GetChild(0).GetComponent<ParticleSystem>().emission;
            var Main1 = transform.GetChild(0).GetComponent<ParticleSystem>().main;
            Emission1.enabled = false;
            Main1.loop = false;
        }
        if (transform.childCount > 1)
        {
            var Emission2 = transform.GetChild(1).GetComponent<ParticleSystem>().emission;
            var Main2 = transform.GetChild(1).GetComponent<ParticleSystem>().main;
            Emission2.enabled = false;
            Main2.loop = false;
        }

        transform.DetachChildren();


    }


    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.tag == ("Room") || other.tag == ("Player") )
        {

            IceBreak();

            isDestory = true;
            Destroy(rigidbody2D);

            float WeatherAlpha = ((Weather.GlobalWeather.isRain) ? (Weather.GlobalWeather.isRainPlus ? 1.8f : 1.3f) : 1) * ((Weather.GlobalWeather.isSunny) ? 0.5f : 1);


            if (other.tag == ("Player") )
            {
                PlayerControler playerControler = other.GetComponent<PlayerControler>();
                Pokemon.PokemonHpChange(empty.gameObject, other.gameObject, Dmage, 0, 0, PokemonType.TypeEnum.Ice);
                if (playerControler != null)
                {
                    playerControler.KnockOutPoint = 3;
                    playerControler.KnockOutDirection = (playerControler.transform.position - transform.position).normalized;
                    playerControler.PlayerFrozenFloatPlus(0.15f, 2.0f);
                }
            }


        }

    }

}
