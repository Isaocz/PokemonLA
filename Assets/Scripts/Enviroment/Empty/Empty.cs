using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Empty : Pokemon
{
    //攻击到玩家时造成的击退值声明4个变量，一个代表对玩家造成的伤害，一个代表击退值，一个表示移动的距离,一个表示移动速度，一个表示初始血量
    public float Knock;
    //敌人的当前血量和最大血量
    public int EmptyHp;
    protected int maxHP;
    //敌人的等级
    public int Emptylevel;
    //敌人是否正处于出生动画中，如果正在出生不会行动
    public bool isBorn;
    //敌人的血条ui
    public EmptyHpBar uIHealth;

    //声明两个整形变量，表示敌人的两个属性
    public int EmptyType01;
    public int EmptyType02;

    //声明六个整形数据，表示角色的六项种族值,以及六项当前能力值
    public int HpEmptyPoint;
    public int AtkEmptyPoint;
    public int SpAEmptyPoint;
    public int DefEmptyPoint;
    public int SpdEmptyPoint;
    public int SpeedEmptyPoint;

    public int AtkAbilityPoint { get { return AtkAbility; } set { AtkAbility = value; } }
    int AtkAbility;
    public int SpAAbilityPoint { get { return SpAAbility; } set { SpAAbility = value; } }
    int SpAAbility;
    public int DefAbilityPoint { get { return DefAbility; } set { DefAbility = value; } }
    int DefAbility;
    public int SpdAbilityPoint { get { return SpDAbility; } set { SpDAbility = value; } }
    int SpDAbility;
    public int SpeedAbilityPoint { get { return SpeedAbility; } set { SpeedAbility = value; } }
    int SpeedAbility;



    /// <summary>
    /// 敌人对于某一属性的抗性，关于属性的Index请参考Type.cs
    /// </summary>
    public int[] TypeDef = new int[20] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };



    //敌人摧毁时的时间
    public delegate void EmptyEvent();
    public EmptyEvent DestoryEvent;

    //声明2个布尔值，表示目标对象是否死亡,表示目标是否被攻击,一个击退计时器，一个被击退值
    public bool isDie = false;
    public bool isHit = false;
    float KOTimer = 0;
    float KOPoint;


    //声明一个变量，表示该目标被击倒后获得的经验，Exp根据BaseExp换算而来
    public int Exp;
    public int BaseExp;
    /// <summary>
    /// 二维变量分别为获得努力值的种类和点数(数值参考PlayerControler.ChangeHPW())
    /// </summary>
    public Vector2Int HWP;



    //获取玩家对象
    public PlayerControler player;

    //声明一个刚体变量 一个动画管理者变量(动画管理者已经转到父类Pokeomn)
    public new Rigidbody2D rigidbody2D;


    //表示异常状态的各种变量
    /// <summary>
    /// 敌人是否陷入沉默状态
    /// </summary>
    public bool isSilence = false;

    /// <summary>
    /// 敌人是否是boss
    /// </summary>
    public bool isBoos;


    /// <summary>
    /// 敌人被打死后掉落的道具
    /// </summary>
    public GameObject DropItem;

    //表示敌人是否处于被白雾【精通】击中
    public bool isMistPlus;






    //=============================初始化敌人数据================================

    /// <summary>
    /// ---start中调用---，根据玩家等级动态设定敌人等级，
    /// </summary>
    /// <param name="PlayerLevel"></param>
    /// <param name="MaxLevel"></param>
    /// <returns></returns>
    protected int SetLevel(int PlayerLevel,int MaxLevel)
    {
        int OutPut;
        if (!isBoos)
        {
            if (PlayerLevel <= 10)
            {
                OutPut = (player.playerData.IsPassiveGetList[29] ? (Random.Range(11, 13)) : (Random.Range(4, 8)));
            }
            else
            {
                OutPut = PlayerLevel - (player.playerData.IsPassiveGetList[29] ? (Random.Range(-5, -2)) : (Random.Range(2, 6)));
                if (OutPut > MaxLevel) { OutPut = MaxLevel; }
            }
        }
        else
        {
            OutPut = Mathf.Clamp(player.Level+ (player.playerData.IsPassiveGetList[29] ? (Random.Range(-5,0)):(Random.Range(-2, 3))), (player.playerData.IsPassiveGetList[29] ? 22 : 15), (player.playerData.IsPassiveGetList[29] ? 60 : 50));
        }
        return OutPut;
    }

    /// <summary>
    /// ---start中调用---，根据种族值初始化敌人血量
    /// </summary>
    /// <param name="level"></param>
    protected void EmptyHpForLevel(int level)
    {
        EmptyHp = (int)((level + 10 + (int)(((float)level * HpEmptyPoint * 2) / 100.0f))*(isBoos?1.7f:1));
        maxHP = EmptyHp;
    }

    /// <summary>
    /// ---start中调用---，根据种族值初始化敌人其他能力值
    /// </summary>
    /// <param name="level"></param>
    /// <param name="Ability"></param>
    /// <returns></returns>
    protected int AbilityForLevel(int level, int Ability)
    {
        return (Ability * 2 * level) / 100 + 5;
    }

    //=============================初始化敌人数据================================










    //====================敌人血量改变======================
    /// <summary>
    ///     //声明一个函数，改变敌人对象的血量
    /// </summary>
    /// <param name="Dmage">物攻伤害</param>
    /// <param name="SpDmage">特攻伤害</param>
    /// <param name="SkillType">伤害属性（数字参考Type.cs）</param>
    public void EmptyHpChange(float  Dmage , float SpDmage , int SkillType)
    {
        float typeDef = (TypeDef[SkillType] < 0 ? (Mathf.Pow(1.2f, -TypeDef[SkillType])) : 1) * (TypeDef[SkillType] > 0 ? (Mathf.Pow(0.8f, TypeDef[SkillType])) : 1);
            if (Dmage + SpDmage >= 0)
            {
                if (SkillType != 19)
                {
                    EmptyHp -= (int)((Dmage + SpDmage) * (Type.TYPE[SkillType][EmptyType01]) * (Type.TYPE[SkillType][EmptyType02]) * typeDef);
                }
                else
                {
                    EmptyHp -= (int)((Dmage + SpDmage) * typeDef);
                }
            }
            else
            {
                EmptyHp = Mathf.Clamp(EmptyHp - (int)(Dmage + SpDmage), 0, maxHP);
            }

            Debug.Log(((Dmage + SpDmage) * (Type.TYPE[SkillType][EmptyType01]) * (Type.TYPE[SkillType][EmptyType02])) * typeDef);
            if ((int)Dmage + (int)SpDmage > 0)
            {
                animator.SetTrigger("Hit");
                uIHealth.Per = (float)EmptyHp / (float)maxHP;
                uIHealth.ChangeHpDown();
            }
            else
            {
                uIHealth.Per = (float)EmptyHp / (float)maxHP;
                uIHealth.ChangeHpUp();
            }
    }
    //====================敌人血量改变======================









    //========================敌人被击退相关=========================
    /// <summary>
    /// 声明一个函数，设置敌人被击退的距离
    /// </summary>
    /// <param name="KnockOutPoint"></param>
    public void EmptyKnockOut(float KnockOutPoint)
    {
        isHit = true;
        KOPoint = KnockOutPoint;
    }

    /// <summary>
    /// ---Update中调用---，当敌人被击退时移动敌人
    /// </summary>
    protected void EmptyBeKnock()
    {
        if (!isDie && isHit)
        {
            KOTimer += Time.deltaTime;
            Vector2 position = rigidbody2D.position;
            Vector2 KODirection = new Vector2(player.transform.position.x - transform.position.x, player.transform.position.y - transform.position.y);
            KODirection.Normalize();
            position.x = position.x - KOPoint * KODirection.x * Time.deltaTime;
            position.y = position.y - KOPoint * KODirection.y * Time.deltaTime;
            rigidbody2D.position = position;

        }
        if (KOTimer >= 0.15f)
        {
            isHit = false;
            KOTimer = 0;
        }
    }
    //========================敌人被击退相关=========================








    //========================敌人碰撞伤害========================

    /// <summary>
    /// ---OnColliderEnter2D中调用---，对于玩家造成触碰伤害
    /// </summary>
    /// <param name="player"></param>
    public void EmptyTouchHit(GameObject player)
    {
        //如果触碰到的是玩家，使玩家扣除一点血量
        PlayerControler playerControler = player.gameObject.GetComponent<PlayerControler>();
        if (playerControler != null)
        {
            playerControler.ChangeHp(-(10 * AtkAbilityPoint * (2 * Emptylevel + 10) / 250 ) ,0, 0);
            playerControler.KnockOutPoint = Knock;
            playerControler.KnockOutDirection = (playerControler.transform.position - transform.position).normalized;
        }
    }

    //========================敌人碰撞伤害========================










    //=============================死亡事件===========================

    /// <summary>
    /// ---Update中调用---，血量小于等于0时玩家获得经验和努力，敌人进入死亡动画
    /// </summary>
    public void EmptyDie()
    {
        //每帧检测一次，当目标血量小于0时销毁目标
        if (EmptyHp <= 0)
        {
            FrozenRemove();
            Destroy(rigidbody2D);
            RemoveChild();
            if (!isDie)
            {
                player.ChangeEx(Exp);
                player.ChangeHPW(HWP);
                transform.parent.parent.GetComponent<Room>().isClear -= 1;
                if (DestoryEvent != null) { DestoryEvent(); }
            }
            isDie = true;
            animator.SetTrigger("Die");
        }
    }
    /// <summary>
    /// 销毁，仅在死亡动画播放完毕后通过动画调用
    /// </summary>
    public void EmptyDestroy()
    {
        if(DropItem != null)
        {
            Instantiate(DropItem ,transform.position , Quaternion.identity , transform.parent);
        }
        Destroy(gameObject);
    }

    //=============================死亡事件===========================







    //=========================中毒事件========================

    /// <summary>
    /// 敌人的中毒计时器，每计时两秒中一次毒
    /// </summary>
    protected float EmptyToxicTimer;
    /// <summary>
    /// 根据中毒时间敌人掉血
    /// </summary>
    public void EmptyToxic()
    {
        EmptyToxicTimer += Time.deltaTime;
        if(EmptyToxicTimer >= 2)
        {
            EmptyToxicTimer += Time.deltaTime;
            EmptyHpChange( Mathf.Clamp((((float)maxHP)/16)*OtherStateResistance ,1,10) , 0, 19);
            EmptyToxicTimer = 0;
        }

    }
    //=========================中毒事件========================








    /// <summary>
    /// ---Update和FixedUpdate中调用---，声明一个函数，当玩家进化后因为之前的玩家对象被销毁，所以需要重新获取玩家
    /// </summary>
    public void ResetPlayer()
    {
        if (player == null)
        {
            player = GameObject.FindWithTag("Player").GetComponent<PlayerControler>();
        }
    }


    /// <summary>
    /// 当敌人死亡时，把敌人的所有粒子效果子对象的父子关系移除，防止粒子效果随着敌人一起突然消失
    /// </summary>
    void RemoveChild()
    {
        foreach (Transform child in transform)
        {
            if(child.GetComponent<ParticleSystem>() != null)
            {
                child.transform.parent = transform.parent.parent;
                ParticleSystem _ps = child.GetComponent<ParticleSystem>();
                var main = _ps.main;
                _ps.Stop(true,ParticleSystemStopBehavior.StopEmitting);
                main.loop = false;
            }
        }
    }



}
