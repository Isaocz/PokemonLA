using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PincurchinSpikeMove : Projectile
{

    //极坐标转换用的角度和长度
    public float R;
    public float RSpeed;
    float L;
    //原点
    public Vector2 StartPosition;

    bool isDestory;
    ParticleSystem PS;


    private void Awake()
    {
        AwakeProjectile();
    }

    void Start()
    {
        if (RSpeed == 0) { RSpeed = 130; }
        L = 1;
        PS = transform.GetChild(0).GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        //this.transform.localScale += new Vector3(Time.deltaTime * 2, 0, 0);
        DestoryByRange(7.5f);
        if (spriteRenderer.color.a <= 0.5f)
        {
            PS.Stop();
            Debug.Log("xxx");
        }
        if (isDestory)
        {
            CollisionDestory();
            PS.gameObject.SetActive(false);
        }
        else
        {
            L += Time.deltaTime * 1.3f;
            R += Time.deltaTime * RSpeed;
            transform.position = new Vector3(Mathf.Cos(R * Mathf.Deg2Rad) * L + StartPosition.x, Mathf.Sin(R * Mathf.Deg2Rad) * L + StartPosition.y, 0);
            //transform.rotation = Quaternion.AngleAxis(R, Vector3.forward) * Quaternion.Euler(0,0,0); 
            transform.rotation = Quaternion.Euler(0, 0, R);
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if ( other.tag == ("Room") || other.tag == ("Player") || (empty.isEmptyInfatuationDone && other.gameObject != empty.gameObject && other.tag == ("Empty")))
        {
            isDestory = true;
            Destroy(rigidbody2D);
            PS.Stop();

            if (other.tag == ("Player") && !empty.isEmptyInfatuationDone)
            {
                PlayerControler playerControler = other.GetComponent<PlayerControler>();
                if (empty != null) { Pokemon.PokemonHpChange(empty.gameObject, other.gameObject, Dmage, 0, 0, PokemonType.TypeEnum.Electric); }
                else { Pokemon.PokemonHpChange(null, other.gameObject, Dmage, 0, 0, PokemonType.TypeEnum.Electric); }
                if (playerControler != null)
                {
                    //playerControler.ChangeHp(0, -(SpDmage * empty.SpAAbilityPoint * (2 * empty.Emptylevel + 10) / 250), 14);
                    playerControler.KnockOutPoint = 2;
                    playerControler.KnockOutDirection = (playerControler.transform.position - transform.position).normalized;
                    if (Random.Range(0.0f,1.0f) > 0.8f) { playerControler.ParalysisFloatPlus(0.2f); }
                }
            }
            if (other.tag == ("Empty") && empty.isEmptyInfatuationDone)
            {
                Empty e = other.GetComponent<Empty>();
                Pokemon.PokemonHpChange(empty.gameObject, e.gameObject, Dmage, 0, 0, PokemonType.TypeEnum.Electric);
                e.EmptyParalysisDone(1f, 5, 0.2f);
                //e.EmptyHpChange(0, (SpDmage * empty.SpAAbilityPoint * (2 * empty.Emptylevel + 10) / (250 * e.SpdAbilityPoint * ((Weather.GlobalWeather.isHail ? ((e.EmptyType01 == Type.TypeEnum.Ice || e.EmptyType02 == Type.TypeEnum.Ice) ? 1.5f : 1) : 1))) + 2), 14);
            }
        }
    }
    



}
