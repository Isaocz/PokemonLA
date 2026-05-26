using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CetitanEarthquake : MonoBehaviour
{



    //==============================音效枚举===================================

    /// <summary>
    /// 音效种类枚举
    /// </summary>
    public enum EarthquakeSE
    {
        Earthquake,
    }

    /// <summary>
    /// 一次性音效播放器生成
    /// </summary>
    public EnemyAudioPlayer audioPlayer;
    /// <summary>
    /// 循环音效音效播放器生成
    /// </summary>
    public LoopingSEAudioPlayer loopPlayer;

    //==============================音效枚举===================================










    /// <summary>
    /// 地震时间
    /// </summary>
    public float EarthquakeTime;

    /// <summary>
    /// 父浩大鲸
    /// </summary>
    public Cetitan ParentCetitan;

    /// <summary>
    /// 威力
    /// </summary>
    public int Dmage;

    /// <summary>
    /// 击退值
    /// </summary>
    public float KOPoint;


    /// <summary>
    /// 动画机
    /// </summary>
    Animator animator;

    /// <summary>
    /// 粒子特效List
    /// </summary>
    Transform PSList;





    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        PSList = transform.GetChild(2);

        Timer.Start(this, EarthquakeTime, () => { EarthquakeOver(); });

        var clip = audioPlayer.sfxTable.GetClip(EarthquakeSE.Earthquake.ToString());
        loopPlayer.PlayLoop(clip, 1f);
    }


    public void EarthquakeOver()
    {
        animator.SetTrigger("Over");
        _mTool.RemoveAllPSChild(PSList.gameObject);
        loopPlayer.StopLoop(1.0f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (ParentCetitan != null && collision.gameObject.tag == "Player")
        {
            PlayerControler p = collision.gameObject.GetComponent<PlayerControler>();
            Pokemon.PokemonHpChange(ParentCetitan.gameObject, collision.gameObject, Dmage, 0, 0, PokemonType.TypeEnum.Ground);
            if (p != null)
            {
                p.KnockOutPoint = KOPoint;
                p.KnockOutDirection = (p.transform.position - this.transform.position).normalized;
            }
        }
        //弃用 不适用碰撞销毁因为占用太多资源 改为坠落后自动清全图
        //if (collision.gameObject.tag == "Empty")
        //{
        //    //摧毁冰柱
            //CetitanIcicleCrashOBJ icOBJ = collision.gameObject.GetComponent<CetitanIcicleCrashOBJ>();
            //if (icOBJ != null)
            //{
            //    icOBJ.BreakByCetitan();
            //}
        //}

        
    }


}
