using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 玛狃拉迷宫墙壁
/// </summary>
public class WeavileMazeWall : BreakableEnviroment
{
    /// <summary>
    /// 父支援件
    /// </summary>
    public WeavileMazeParent MazeSupportParent;


    public Weavile ParentWeavile;


    public Collider2D WallCollider2;


    //生成是否完成
    public bool isBornOver = false;


    //public EnemyAudioPlayer audioPlayer;


    /// <summary>
    /// 破碎的时间
    /// </summary>
    public float BreakTime;




    /// <summary>
    /// 随时间破裂
    /// </summary>
    /// <returns></returns>
    IEnumerator BeHitbByTime()
    {
        while (ColliderCount < MaxHP)
        {
            if (!isBornOver)
            {
                BeHit(2);
            }

            yield return new WaitForSeconds(2.0f);
        }
    }


    private void Awake()
    {
        isBornOver = true;
    }


    private void Start()
    {
        StartCoroutine(BeHitbByTime());
        //audioPlayer.Play(Weavile.WeavileSE.Wall , transform.position);
    }


    /// <summary>
    ///  墙壁被破坏
    /// </summary>
    public override void Break()
    {
        if (gameObject != null && !isBreak)
        {
            isBreak = true;
            GetComponent<Animator>().SetTrigger("Over");
        }
    }



    public override int GetHitHitPoint(int Dmage, PokemonType.TypeEnum SkillType)
    {
        float DmageAlpha = Mathf.Clamp((float)(Dmage) / (float)50, 1.0f, 10.0f);
        int TypeAlpha = 1;
        int d = (int)(DmageAlpha * TypeAlpha * (float)HitHitPoint);
        Debug.Log(d + "+" + Dmage + "+" + (float)(Dmage) / (float)50 + "+" + DmageAlpha + "+" + TypeAlpha);
        return d;
    }


    /// <summary>
    /// 碰撞事件 忽略玛狃拉和墙壁的碰撞
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //忽略玛狃拉和墙壁的碰撞
        Weavile w = collision.gameObject.GetComponent<Weavile>();
        if (w != null)
        {
            Debug.Log("w");
            Physics2D.IgnoreCollision(WallCollider2, collision.gameObject.GetComponent<Collider2D>());
            return;
        }
    }

}
