using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CetitanIcicleCrash : MonoBehaviour
{


    //==============================音效枚举===================================

    /// <summary>
    /// 音效种类枚举
    /// </summary>
    public enum IcicleCrashSE
    {
        IcicleCrashBreak,
        IcicleCrashFall
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
    /// 冰柱碰撞物
    /// </summary>
    public CetitanIcicleCrashOBJ icOBJ;




    private void Start()
    {
        
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (ParentCetitan != null && collision.gameObject.tag == "Player")
        {
            PlayerControler p = collision.gameObject.GetComponent<PlayerControler>();
            Pokemon.PokemonHpChange(ParentCetitan.gameObject, collision.gameObject, Dmage, 0, 0, PokemonType.TypeEnum.Ice);
            if (p != null)
            {
                p.KnockOutPoint = KOPoint;
                p.KnockOutDirection = (p.transform.position - this.transform.position).normalized;
                p.PlayerFrozenFloatPlus(0.35f , 2.0f);
            }
        }

    }

    public void IcdBreak()
    {
        if (ParentCetitan != null)
        {
            RemoveFromList();
            ParentCetitan.LunchIceShard(transform.position);
            audioPlayer.Play(IcicleCrashSE.IcicleCrashBreak, transform.position);
        }
    }

    public void RemoveFromList()
    {
        if (ParentCetitan.ICObjList.Contains(this))
        {
            ParentCetitan.ICObjList.Remove(this);
        }
    }


    public void RoomParentShake()
    {
        ParentCetitan.ParentPokemonRoom.CameraShake(0.5f, 3.5f, false);
        audioPlayer.Play(IcicleCrashSE.IcicleCrashFall , transform.position);
    }

    private void OnDestroy()
    {
        RemoveFromList();
    }
}
