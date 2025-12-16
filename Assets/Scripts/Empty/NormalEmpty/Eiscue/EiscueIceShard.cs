using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EiscueIceShard : Projectile
{
    bool isDestory;
    public float MoveSpeed;

    bool isCanNotMove;

    public EiscueIceShard ChildIceShard;


    /**
    //±ùÀùÊÇ·ñ·ÖÁÑ
    public bool isSplit
    {
        get { return issplit; }
        set { issplit = value; }
    }
    bool issplit;


    //±ùÀùÊÇ·ñ²úÉú±ùÎí
    public bool isMist
    {
        get { return ismist; }
        set { ismist = value; }
    }
    bool ismist;
    **/

    private void Awake()
    {
        AwakeProjectile();
        Timer.Start(this, 0.9f, () => { if (!isDestory) { IceBreak(); isDestory = true; } });
    }




    private void Update()
    {
        //this.transform.localScale += new Vector3(Time.deltaTime * 2, 0, 0);
        if (!isCanNotMove)
        {
            MoveNotForce();

        }
        DestoryByRange(_MaxDistence);
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
        /**
        if (isSplit)
        {
            float startRotation = transform.rotation.eulerAngles.z;
            Debug.Log(startRotation);
            Vector2 LunchRotation1 = (Quaternion.AngleAxis(100.0f + startRotation, Vector3.forward) * Vector2.right).normalized;
            EiscueIceShard iS1 = Instantiate(ChildIceShard, transform.position + (Vector3)LunchRotation1, Quaternion.Euler(0, 0, _mTool.Angle_360Y(LunchRotation1 , Vector2.right) ));
            iS1.empty = empty;
            iS1.LaunchNotForce(LunchRotation1, 4.2f);
            iS1.isSplit = false;
            iS1.isMist = true;
            iS1.SetMaxDistence(1.6f);

            Vector2 LunchRotation2 = (Quaternion.AngleAxis(-100.0f + startRotation, Vector3.forward) * Vector2.right).normalized;
            EiscueIceShard iS2 = Instantiate(ChildIceShard, transform.position + (Vector3)LunchRotation2, Quaternion.Euler(0, 0, _mTool.Angle_360Y(LunchRotation2, Vector2.right)));
            iS2.empty = empty;
            iS2.LaunchNotForce(LunchRotation2, 4.2f);
            iS2.isSplit = false;
            iS2.isMist = true;
            iS2.SetMaxDistence(1.6f);
        }
        **/

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

        if (other.tag == ("Enviroment") || other.tag == ("Room") || other.tag == ("Player") || (empty.isEmptyInfatuationDone && other.gameObject != empty.gameObject && other.tag == ("Empty")))
        {
            IceBreak();
            isDestory = true;
            Destroy(rigidbody2D);

            float WeatherAlpha = ((Weather.GlobalWeather.isRain) ? (Weather.GlobalWeather.isRainPlus ? 1.8f : 1.3f) : 1) * ((Weather.GlobalWeather.isSunny) ? 0.5f : 1);

            if (other.tag == ("Player") && !empty.isEmptyInfatuationDone)
            {
                PlayerControler playerControler = other.GetComponent<PlayerControler>();
                Pokemon.PokemonHpChange(empty.gameObject, other.gameObject, Dmage, 0, 0, PokemonType.TypeEnum.Ice);
                if (playerControler != null)
                {
                    playerControler.KnockOutPoint = 1;
                    playerControler.KnockOutDirection = (playerControler.transform.position - transform.position).normalized;
                    playerControler.PlayerFrozenFloatPlus(0.3f, 1.2f);
                }
            }
            if (other.tag == ("Empty") && empty.isEmptyInfatuationDone)
            {
                Empty e = other.GetComponent<Empty>();
                Pokemon.PokemonHpChange(empty.gameObject, e.gameObject, Dmage, 0, 0, PokemonType.TypeEnum.Ice);
            }
        }
    }
}
