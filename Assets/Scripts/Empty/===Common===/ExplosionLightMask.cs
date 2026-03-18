using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionLightMask : MonoBehaviour
{
    /// <summary>
    /// 父敌人
    /// </summary>
    public Empty ParentEmpty;

    /// <summary>
    /// 是否开启闪烁
    /// </summary>
    public bool isOn = true;

    /// <summary>
    /// 爆炸
    /// </summary>
    public bool Explosion;

    /// <summary>
    /// 闪烁一次的周期时间
    /// </summary>
    public float BlinkCycleTime;

    /// <summary>
    /// 闪烁的最大亮度
    /// </summary>
    public float BlinkMaxAlpha;

    /// <summary>
    /// 爆炸遮罩
    /// </summary>
    public SpriteRenderer SpriteMask;

    /// <summary>
    /// 是否增加亮度
    /// </summary>
    bool isAlphaPlus = true;


    /// <summary>
    /// 闪烁的速度
    /// </summary>
    float BlinkSpeed
    {
        get { return BlinkMaxAlpha / BlinkCycleTime; }
    }



    //爆炸速度
    float ExplosionSpeed;



    // Update is called once per frame
    void Update()
    {
        if (isOn && SpriteMask != null && BlinkMaxAlpha > 0)
        {
            if (!Explosion) {
                var c = SpriteMask.color;
                c.a += (isAlphaPlus ? 1 : -1) * Time.deltaTime * BlinkSpeed;
                SpriteMask.color = new Color( c.r , c.g , c.b , c.a );
                if (c.a >= BlinkMaxAlpha) { isAlphaPlus = false; }
                else if (c.a <= 0) { isAlphaPlus = true; }
            }
            else
            {
                
                var c = SpriteMask.color;
                c.a += Time.deltaTime * ExplosionSpeed;
                //Debug.Log(c.a  + "+" + ExplosionSpeed);
                SpriteMask.color = new Color(c.r, c.g, c.b, c.a);
                if (c.a >= 1.0f)
                {
                    ExplosionOver();
                }
            }
        }
    }

    public void SetBlink(float time , float alpha)
    {
        BlinkCycleTime = time;
        BlinkMaxAlpha = alpha;
    }

    /// <summary>
    /// 关闭闪光
    /// </summary>
    public void TurnOff()
    {
        if (isOn)
        {
            isOn = false;
            var c = SpriteMask.color;
            SpriteMask.color = new Color(c.r, c.g, c.b, 0);
        }
    }



    /// <summary>
    /// 开启闪光
    /// </summary>
    public void TurnOn(float time, float alpha)
    {
        isOn = true;
        SetBlink(time , alpha);
    }

    /// <summary>
    /// 开启闪光
    /// </summary>
    public void ExplosionStart(float time)
    {
        ExplosionSpeed = (1 - SpriteMask.color.a)/time;
        Explosion = true;
    }


    /// <summary>
    /// 闪光结束 准备爆炸
    /// </summary>
    public void ExplosionOver()
    {
        ParentEmpty.EmptyEcplosionEvent();
    }
}



