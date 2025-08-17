using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HaunterSludgeBomb : Projectile
{
    /// <summary>
    /// 污泥炸弹是否摧毁
    /// </summary>
    bool isDestory;
    /// <summary>
    /// 污泥炸弹是否还可以移动
    /// </summary>
    bool isCanNotMove;
    /// <summary>
    /// 污泥炸弹最远距离
    /// </summary>
    float MaxDistence;
    /// <summary>
    /// 球体sprite
    /// </summary>
    SpriteRenderer BallSprite;
    /// <summary>
    /// 影子1sprite
    /// </summary>
    SpriteRenderer Shadow1Sprite;
    /// <summary>
    /// 影子2sprite
    /// </summary>
    SpriteRenderer Shadow2Sprite;

    //污泥炸弹的运动轨迹
    float a_Bomb = 0;
    float b_Bomb = 0;


    /// <summary>
    /// 毒雾
    /// </summary>
    public HaunterSludgeBombMist PosionMist;

    /// <summary>
    /// 污泥炸弹炸开后的范围伤害
    /// </summary>
    public HaunterSludgeBombWaveCollider Wave;




    private void Awake()
    {
        AwakeProjectile();
        Timer.Start(this, 3.0f, () => { if (!isDestory) { isDestory = true; } });
    }

    
    private void Start()
    {
        //获取球体sprite
        BallSprite = transform.GetChild(0).GetComponent<SpriteRenderer>();
        Shadow1Sprite = transform.GetChild(3).GetChild(0).GetComponent<SpriteRenderer>();
        Shadow2Sprite = transform.GetChild(3).GetChild(1).GetComponent<SpriteRenderer>();
    }

    /// <summary>
    /// 设置新发射的冰砾
    /// </summary>
    public void SetNewSludgeBomb( Empty e , Vector2 Dir , float distence)
    {
        empty = e;
        float Speed = distence / 1.0f;
        LaunchNotForce(Dir, Speed);
        isCanNotMove = false;
        MaxDistence = distence;
        a_Bomb = -(4.0f / (distence * distence));
        b_Bomb = (4.0f / (distence));
    }



    private void Update()
    {

        //this.transform.localScale += new Vector3(Time.deltaTime * 2, 0, 0);
        if (!isCanNotMove)
        {
            DestoryByRange(MaxDistence);
            if (isDestory)
            {
                SludgeBombBreak();
            }
            else
            {
                //污泥炸弹运动
                MoveNotForce();
                //污泥炸弹上下抛物线运动
                float BombDistence = Vector3.Distance(BornPosition, transform.position);
                BallSprite.transform.localPosition = new Vector3(0, a_Bomb * BombDistence * BombDistence + b_Bomb * BombDistence, 0);
            }
        }
        else
        {
            SludgeBombBreak();
        }

        
    }



    public override void DestoryByRange(float ProjectileRange)
    {
        if ((transform.position - BornPosition).magnitude >= ProjectileRange)
        {
            SludgeBombBreak();
        }
    }

    /// <summary>
    /// 污泥炸弹爆炸
    /// </summary>
    void SludgeBombBreak()
    {
        if (!isCanNotMove)
        {
            isCanNotMove = true;
            transform.GetChild(1).gameObject.SetActive(true);
            transform.GetChild(2).gameObject.SetActive(true);
            BallSprite.transform.GetChild(0).parent = null;
            transform.GetChild(1).parent = null;
            transform.GetChild(1).parent = null;
            BallSprite.transform.localPosition = new Vector3(0, 0, 0);
            transform.GetComponent<Collider2D>().enabled = false;

            foreach (Transform t in BallSprite.transform)
            {
                var ps0 = t.GetComponent<ParticleSystem>().main;
                ps0.loop = false;
            }
            BallSprite.transform.DetachChildren();

            Instantiate(PosionMist , transform.position , Quaternion.identity).SetTime(20.0f);
            Instantiate(Wave, transform.position , Quaternion.identity).empty = empty;
        }
        else
        {
            BallSprite.color -= new Color(0, 0, 0, 5 * Time.deltaTime);
            BallSprite.transform.localScale += new Vector3(5 * Time.deltaTime, 5 * Time.deltaTime, 5 * Time.deltaTime);
            Shadow1Sprite.color -= new Color(0, 0, 0,  Time.deltaTime);
            Shadow1Sprite.transform.localScale += new Vector3(Time.deltaTime,Time.deltaTime,Time.deltaTime);
            Shadow2Sprite.color -= new Color(0, 0, 0, Time.deltaTime);
            Shadow2Sprite.transform.localScale += new Vector3(Time.deltaTime, Time.deltaTime, Time.deltaTime);
            if (BallSprite.color.a <= 0 && Shadow1Sprite.color.a <= 0 && Shadow2Sprite.color.a <= 0) { Destroy(gameObject); }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.isTrigger == false)
        {
            if (other.tag == ("Player"))
            {
                SludgeBombBreak();
                if (!empty.isEmptyInfatuationDone) {
                    PlayerControler playerControler = other.GetComponent<PlayerControler>();
                    Pokemon.PokemonHpChange(empty.gameObject, other.gameObject, 0, SpDmage, 0, PokemonType.TypeEnum.Poison);
                    if (playerControler != null)
                    {
                        playerControler.KnockOutPoint = 3.0f;
                        playerControler.KnockOutDirection = (playerControler.transform.position - transform.position).normalized;
                        playerControler.ToxicFloatPlus(0.5f);
                    }
                }
            }
            else if ((empty.isEmptyInfatuationDone && other.gameObject != empty.gameObject && other.tag == ("Empty")))
            {
                SludgeBombBreak();
                Empty e = other.GetComponent<Empty>();
                Pokemon.PokemonHpChange(empty.gameObject, e.gameObject, 0, SpDmage, 0, PokemonType.TypeEnum.Poison);
                e.EmptyToxicDone(1f , 5.0f , 0.5f);
            }
            else if (other.tag == "Room" || other.tag == "Enviroment")
            {
                SludgeBombBreak();
            }
        }
    }


    


}
