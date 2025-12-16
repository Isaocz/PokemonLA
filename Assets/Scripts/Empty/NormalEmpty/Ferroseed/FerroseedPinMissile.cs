using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FerroseedPinMissile : Projectile
{
    bool isDestory;
    public float MoveSpeed;

    bool isCanNotMove;

    float PinSpeed = 11.5f;

    Vector2 PinDirection;

    float ProjectileTimer = 0.0f;


    private void Awake()
    {
        AwakeProjectile();
        Timer.Start(this, 10.0f, () => { if (!isDestory) { isDestory = true; } });
    }


    private void Start()
    {
        PinDirection = Quaternion.AngleAxis(transform.rotation.eulerAngles.z , Vector3.forward ) * Vector3.right;
    }


    private void Update()
    {
        //this.transform.localScale += new Vector3(Time.deltaTime * 2, 0, 0);
        if (!isCanNotMove)
        {
            transform.Translate(Vector3.right * MoveSpeed * Time.deltaTime);
        }
        if (isDestory)
        {
            CollisionDestory();
            SpikeBreak();
        }
        else
        {
            ProjectileTimer += Time.deltaTime;
            if (ProjectileTimer < 2.2f)
            {
                float RotationSpeed = 360.0f;
                if (ProjectileTimer >= (180.0f/RotationSpeed)) { RotationSpeed /= 2.0f; }
                PinDirection = Quaternion.AngleAxis(RotationSpeed * Time.deltaTime, Vector3.forward) * PinDirection;
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, _mTool.Angle_360Y((Vector3)PinDirection, Vector3.right)));
                transform.position = transform.position + new Vector3(PinDirection.x * PinSpeed * Time.deltaTime, PinDirection.y * PinSpeed * Time.deltaTime, 0);
            }
            else
            {
                isDestory = true;
            }
        }

    }

    void SpikeBreak()
    {
        if (!isCanNotMove) {
            isCanNotMove = true;
            //GetComponent<Animator>().SetTrigger("Break");
            var Emission1 = transform.GetChild(0).GetComponent<ParticleSystem>().emission;
            var Main1 = transform.GetChild(0).GetComponent<ParticleSystem>().main;
            Emission1.enabled = false;
            Main1.loop = false;
            transform.DetachChildren();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.tag == ("Room") || other.tag == ("Player") || (empty.isEmptyInfatuationDone && other.gameObject != empty.gameObject && other.tag == ("Empty")))
        {
            SpikeBreak();
            isDestory = true;
            Destroy(rigidbody2D);

            float WeatherAlpha = ((Weather.GlobalWeather.isRain) ? (Weather.GlobalWeather.isRainPlus ? 1.8f : 1.3f) : 1) * ((Weather.GlobalWeather.isSunny) ? 0.5f : 1);

            if (other.tag == ("Player") && !empty.isEmptyInfatuationDone)
            {
                PlayerControler playerControler = other.GetComponent<PlayerControler>();
                Pokemon.PokemonHpChange(empty.gameObject, other.gameObject, Dmage, 0, 0, PokemonType.TypeEnum.Grass);
                if (playerControler != null)
                {
                    playerControler.KnockOutPoint = 3.0f;
                    playerControler.KnockOutDirection = (playerControler.transform.position - transform.position).normalized;
                }
            }
            if (other.tag == ("Empty") && empty.isEmptyInfatuationDone)
            {
                Empty e = other.GetComponent<Empty>();
                Pokemon.PokemonHpChange(empty.gameObject, e.gameObject, Dmage, 0, 0, PokemonType.TypeEnum.Grass);
            }
        }
    }
}
