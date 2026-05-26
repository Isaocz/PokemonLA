using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeavileDarkPulse : MonoBehaviour
{
    /// <summary>
    /// 父玛狃拉
    /// </summary>
    public Weavile ParentWeavile;

    /// <summary>
    /// 伤害
    /// </summary>
    public int SpDmage;

    /// <summary>
    /// 击退值
    /// </summary>
    public float KOPoint;

    /// <summary>
    /// 销毁时间
    /// </summary>
    public float DestoryTime;


    //public EnemyAudioPlayer audioPlayer;

    private void Start()
    {
        Timer.Start(this , DestoryTime , ()=> { Destroy(gameObject); });
        //audioPlayer.Play(Weavile.WeavileSE.DarkPusle , transform.position);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && ParentWeavile != null)
        {
            PlayerControler p = collision.GetComponent<PlayerControler>();
            Pokemon.PokemonHpChange(ParentWeavile.gameObject, collision.gameObject, 0, SpDmage, 0, PokemonType.TypeEnum.Dark);
            if (p != null)
            {
                p.KnockOutPoint = KOPoint;
                p.KnockOutDirection = (p.transform.position - this.transform.position).normalized;
            }
        }

       
    }
}
