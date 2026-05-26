using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableEnviroment : MonoBehaviour
{
















    /// <summary>
    /// 满血
    /// </summary>
    public int MaxHP;

    /// <summary>
    /// 已经被击打次数
    /// </summary>
    public int ColliderCount;

    /// <summary>
    /// 血条
    /// </summary>
    public BreakableEnviromentHpBar HpBar;

    /// <summary>
    /// 是否已经被击碎
    /// </summary>
    public bool isBreak;

    /// <summary>
    /// 是否无法被击碎
    /// </summary>
    public bool isUnBreakable;

    /// <summary>
    /// 可破坏物被保护
    /// </summary>
    public bool InvincibleProtect
    {
        get { return invincibleProtect; }
        set { invincibleProtect = value; }
    }
    bool invincibleProtect;




    /// <summary>
    /// 被触碰时扣除血量
    /// </summary>
    public virtual int GetTouchHitPoint()
    {
        return TouchHitPoint;
    }

    /// <summary>
    /// 被击打时扣除血量
    /// </summary>
    public virtual int GetHitHitPoint(int Dmage, PokemonType.TypeEnum SkillType)
    {
        return HitHitPoint;
    }

    /// <summary>
    /// 被触碰时扣除血量默认值
    /// </summary>
    public int TouchHitPoint = 3;

    /// <summary>
    /// 被击打时扣除血量默认值
    /// </summary>
    public int HitHitPoint = 5;





    /// <summary>
    /// 受击
    /// </summary>
    /// <param name="point"></param>
    public void BeHit(int point)
    {
        
        if (!InvincibleProtect)
        {

            ColliderCount = Mathf.Clamp(ColliderCount + point, 0, MaxHP);
            HpBar.Per = (float)(MaxHP - ColliderCount) / (float)MaxHP;
            HpBar.ChangeHpDown();
            if (ColliderCount >= MaxHP)
            {
                Break();
            }
        }
    }


    /// <summary>
    /// 物品破碎
    /// </summary>
    public virtual void Break()
    {
        if (gameObject != null && !isBreak)
        {
            isBreak = true;
        }
    }


    /// <summary>
    /// 销毁
    /// </summary>
    public void DestorySelf()
    {
        Destroy(gameObject);
    }
}

