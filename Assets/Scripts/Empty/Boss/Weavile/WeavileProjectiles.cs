using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 玛狃拉投掷物
/// </summary>
public class WeavileProjectiles : Projectile
{
    /// <summary>
    /// 投掷物是否摧毁
    /// </summary>
    protected bool isDestory;
    /// <summary>
    /// 投掷物是否还可以移动
    /// </summary>
    protected bool isCanNotMove;
    /// <summary>
    /// 投掷物最远距离
    /// </summary>
    protected float MaxDistence;
    /// <summary>
    /// 球体sprite
    /// </summary>
    public SpriteRenderer BallSprite;
    /// <summary>
    /// 影子1sprite
    /// </summary>
    public SpriteRenderer Shadow1Sprite;
    /// <summary>
    /// 影子2sprite
    /// </summary>
    public SpriteRenderer Shadow2Sprite;
    /// <summary>
    /// 指示圈
    /// </summary>
    public SkillRangeCircle RangeCircle;
    /// <summary>
    /// 是否是假的
    /// </summary>
    public bool isFake;

    /// <summary>
    /// 是否和玩家碰撞
    /// </summary>
    public bool isNotTriggerWithPlaye = false;


    //投掷物的运动轨迹
    protected float a_Bomb = 0;
    protected float b_Bomb = 0;


    /// <summary>
    /// 父玛狃拉
    /// </summary>
    public Weavile ParentWeavile;


    /// <summary>
    /// 一次性音效播放器生成
    /// </summary>
    public EnemyAudioPlayer audioPlayer;



    /// <summary>
    /// 设置新发射的投掷物
    /// </summary>
    public virtual void SetWeavileProjectiles(Empty e, Vector2 Dir, float distence)
    {
        empty = e;
        float Speed = distence / 0.5f;
        LaunchNotForce(Dir, Speed);
        isCanNotMove = false;
        MaxDistence = distence;
        distence = Mathf.Clamp(distence, 0.01f, 10000.0f);
        a_Bomb = -(8.0f / (distence * distence));
        b_Bomb = (8.0f / (distence));

        if (RangeCircle != null) { 
            RangeCircle.transform.parent = null;
            RangeCircle.transform.position = transform.position + (Vector3)Dir * distence;
        }
        ParentWeavile = empty.GetComponent<Weavile>();
    }


    /// <summary>
    /// 随距离被摧毁
    /// </summary>
    /// <param name="ProjectileRange"></param>
    public override void DestoryByRange(float ProjectileRange)
    {
        if ((transform.position - BornPosition).magnitude >= ProjectileRange)
        {
            ProjectilesBreak();
        }
    }

    /// <summary>
    /// 投掷物爆炸
    /// </summary>
    protected virtual void ProjectilesBreak()
    {
        if (!isCanNotMove)
        {
            isCanNotMove = true;
            BallSprite.transform.localPosition = new Vector3(0, 0, 0);
            transform.GetComponent<Collider2D>().enabled = false;
            GetComponent<Animator>().SetTrigger("Over");
            if (RangeCircle != null)
            {
                RangeCircle.transform.position = transform.position;
            }
        }
        else
        {
            BallSprite.color -= new Color(0, 0, 0, 5 * Time.deltaTime);
            //BallSprite.transform.localScale += new Vector3(5 * Time.deltaTime, 5 * Time.deltaTime, 5 * Time.deltaTime);
            Shadow1Sprite.color -= new Color(0, 0, 0, Time.deltaTime);
            Shadow1Sprite.transform.localScale += new Vector3(Time.deltaTime, Time.deltaTime, Time.deltaTime);
            Shadow2Sprite.color -= new Color(0, 0, 0, Time.deltaTime);
            Shadow2Sprite.transform.localScale += new Vector3(Time.deltaTime, Time.deltaTime, Time.deltaTime);
            if (BallSprite.color.a <= 0 && Shadow1Sprite.color.a <= 0 && Shadow2Sprite.color.a <= 0) { Destroy(gameObject); }
        }
    }




















    protected void AwakeEvent()
    {
        AwakeProjectile();
        Timer.Start(this, 3.0f, () => { if (!isDestory) { isDestory = true; } });
    }

    protected void StartEvent()
    {

    }

    protected void UpdateEvent()
    {
        //this.transform.localScale += new Vector3(Time.deltaTime * 2, 0, 0);
        if (!isCanNotMove)
        {
            DestoryByRange(MaxDistence);
            if (isDestory)
            {
                ProjectilesBreak();
            }
            else
            {
                //投掷物运动
                MoveNotForce();



                //投掷物上下抛物线运动
                float BombDistence = Vector3.Distance(BornPosition, transform.position);
                Debug.Log(BornPosition);
                Debug.Log(transform.position);
                Debug.Log(BombDistence);

                BallSprite.transform.localPosition = new Vector3(0, a_Bomb * BombDistence * BombDistence + b_Bomb * BombDistence, 0);
            }
        }
        else
        {
            ProjectilesBreak();
        }
    }

    protected void OnTriggerEnter2DEvent(Collider2D other)
    {
        if (other.isTrigger == false)
        {
            if (other.tag == ("Player"))
            {
                if (isNotTriggerWithPlaye) {
                    ProjectilesBreak();
                }
                //PlayerControler playerControler = other.GetComponent<PlayerControler>();
                //Pokemon.PokemonHpChange(empty.gameObject, other.gameObject, 0, SpDmage, 0, PokemonType.TypeEnum.Poison);
                //if (playerControler != null)
                //{
                //    playerControler.KnockOutPoint = 3.0f;
                //    playerControler.KnockOutDirection = (playerControler.transform.position - transform.position).normalized;
                //    playerControler.ToxicFloatPlus(0.5f);
                //}
            }
            //触碰房间或环境物提前爆炸 防止刚放出来就爆
            else if ( (transform.position - BornPosition).magnitude >= 0.5f && (other.tag == "Room" || other.tag == "Enviroment"))
            {
                ProjectilesBreak();
            }
        }
    }

}
