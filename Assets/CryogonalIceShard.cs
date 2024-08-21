using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CryogonalIceShard : Projectile
{
    bool isDestory;
    public float MoveSpeed;

    bool isCanNotMove;


    private void Awake()
    {
        AwakeProjectile();
        Timer.Start(this, 3.0f, () => { if (!isDestory) { isDestory = true; } });
    }




    private void Update()
    {
        //this.transform.localScale += new Vector3(Time.deltaTime * 2, 0, 0);
        if (!isCanNotMove) {
            transform.Translate(Vector3.right * MoveSpeed * Time.deltaTime);
        }
        DestoryByRange(20);
        if (isDestory)
        {
            CollisionDestory();
        }
        else
        {
            MoveNotForce();
        }

    }

    void IceBreak()
    {
        isCanNotMove = true;
        GetComponent<Animator>().SetTrigger("Break");
        if (transform.childCount > 0) {
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

        if (other.tag == ("Enviroment") || other.tag == ("Room") || other.tag == ("Player") || (empty.isEmptyInfatuationDone && other.gameObject != empty.gameObject && other.tag == ("Empty")))
        {
            IceBreak();
            isDestory = true;
            Destroy(rigidbody2D);

            float WeatherAlpha = ((Weather.GlobalWeather.isRain) ? (Weather.GlobalWeather.isRainPlus ? 1.8f : 1.3f) : 1) * ((Weather.GlobalWeather.isSunny) ? 0.5f : 1);

            if (other.tag == ("Player") && !empty.isEmptyInfatuationDone)
            {
                PlayerControler playerControler = other.GetComponent<PlayerControler>();
                Pokemon.PokemonHpChange(empty.gameObject, other.gameObject, Dmage, 0, 0, Type.TypeEnum.Ice);
                if (playerControler != null)
                {
                    playerControler.KnockOutPoint = 1;
                    playerControler.KnockOutDirection = (playerControler.transform.position - transform.position).normalized;
                    playerControler.PlayerFrozenFloatPlus(0.3f , 1.2f);
                }
            }
            if (other.tag == ("Empty") && empty.isEmptyInfatuationDone)
            {
                Empty e = other.GetComponent<Empty>();
                Pokemon.PokemonHpChange(empty.gameObject, e.gameObject, Dmage, 0, 0, Type.TypeEnum.Ice);
            }
        }
    }
}
