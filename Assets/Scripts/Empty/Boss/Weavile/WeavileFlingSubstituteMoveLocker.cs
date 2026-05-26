using System.Collections.Generic;
using UnityEngine;

public class WeavileFlingSubstituteMoveLocker : MonoBehaviour
{



    /// <summary>
    /// 玩家列表
    /// </summary>
    public List<PlayerControler> PlayerList = new List<PlayerControler>();
    /// <summary>
    /// 半径
    /// </summary>
    public float SubstituteRadius;
    /// <summary>
    /// 中心
    /// </summary>
    Vector2 SubstituteCenter { get { return transform.position; } }
    /// <summary>
    /// 父替身
    /// </summary>
    public WeavileSubstituteOBJ ParentOBJ;



    /// <summary>
    /// 碰撞箱
    /// </summary>
    public SkillColliderRangeChangeByTimeManual SkillColliderCircle;
    /// <summary>
    /// 指示环
    /// </summary>
    public SkillRangeCircleManual SkillRangeCircle;






    /// <summary>
    /// 时时更新位置 超出范围把玩家拉回
    /// </summary>
    private void LateUpdate()
    {
        if (ParentOBJ != null && ParentOBJ.ParentRoom != null) {
            // LateUpdate 避免与玩家移动逻辑冲突
            for (int i = PlayerList.Count - 1; i >= 0; i--)
            {
                if (ParentOBJ.ParentRoom.RoomIndex == PlayerList[i].NowRoom) {
                    var player = PlayerList[i];
                    if (player == null)
                    {
                        PlayerList.RemoveAt(i);
                        continue;
                    }

                    Vector2 dir = (Vector2)player.transform.position - SubstituteCenter;
                    float dist = dir.magnitude;

                    if (dist > SubstituteRadius)
                    {
                        player.transform.position = SubstituteCenter + dir.normalized * SubstituteRadius;
                    }
                } else
                {
                    if (PlayerList[i] != null && PlayerList.Contains(PlayerList[i]))
                    {
                        PlayerList.Remove(PlayerList[i]);
                    }
                }
            }
        }
    }

    /// <summary>
    /// 获取Player
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerControler p = collision.GetComponent<PlayerControler>();
            if (p != null && !PlayerList.Contains(p))
            {
                PlayerList.Add(p);
                //p.SpeedChange
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerControler p = collision.GetComponent<PlayerControler>();
            if (p != null && PlayerList.Contains(p))
            {
                PlayerList.Remove(p);
            }
        }
    }


    //碰撞结束
    public void CollidorOver() {

        if (SkillColliderCircle != null) { SkillColliderCircle.SkillCircleOver(); }
        if (SkillRangeCircle != null) { SkillRangeCircle.SkillCircleOver(); }
        for (int i = PlayerList.Count - 1; i >= 0; i--)
        {
            PlayerList[i].SpeedRemove01(0.0f);
        }
        PlayerList.Clear();
    }

    private void OnDestroy()
    {
        CollidorOver();
    }


}
