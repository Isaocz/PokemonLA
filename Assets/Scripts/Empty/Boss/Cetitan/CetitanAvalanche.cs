using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CetitanAvalanche : MonoBehaviour
{

    //==============================音效枚举===================================

    /// <summary>
    /// 音效种类枚举
    /// </summary>
    public enum CetitanAvalancheSE
    {
        Avalanche,
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
    /// 雪崩内径
    /// </summary>
    public float InnerRadius;



    /// <summary>
    /// 开始延迟
    /// </summary>
    public float StarDelay;


    // Start is called before the first frame update
    void Start()
    {

        Timer.Start(this, 20, () => {  Destroy(this.gameObject); });
        Timer.Start(this, StarDelay, () => {
            var clip = audioPlayer.sfxTable.GetClip(CetitanAvalancheSE.Avalanche.ToString());
            loopPlayer.PlayLoop(clip, 1f);
        });
        Timer.Start(this, StarDelay+3.7f, () => {
            loopPlayer.StopLoop(1f);
        });

    }



    private void OnTriggerStay2D(Collider2D collision)
    {
        if (/*ParentCetitan != null &&*/   collision.gameObject.tag == "Player")
        {
            //判断距离
            float dist = Vector2.Distance(collision.gameObject.transform.position, transform.position);

            if (dist > InnerRadius)
            {
                PlayerControler p = collision.gameObject.GetComponent<PlayerControler>();
                Pokemon.PokemonHpChange(this.gameObject, collision.gameObject, Dmage, 0, 0, PokemonType.TypeEnum.Ice);
                if (p != null)
                {
                    p.KnockOutPoint = KOPoint;
                    p.KnockOutDirection = (this.transform.position - p.transform.position).normalized;
                }
            }
        }
    }
}
