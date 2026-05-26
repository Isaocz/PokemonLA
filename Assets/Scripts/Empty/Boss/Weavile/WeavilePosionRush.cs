using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeavilePosionRush : WeavileRush
{

    /// <summary>
    /// 抓痕特效
    /// </summary>
    public ClawEffect ClawEffect;

    /// <summary>
    /// 碰撞箱控制器
    /// </summary>
    public SkillColliderRangeChangeByTimeManual colliderRangeChangeByTimeManual;

    /// <summary>
    /// 结束粒子特效
    /// </summary>
    public GameObject OverPS;


    public override void RushStart(Vector2 d)
    {
        base.RushStart(d);
        // 设置冲刺方向
        ClawEffect.transform.rotation = Quaternion.Euler(0, 0, _mTool.Angle_360Y(d, Vector2.right));
    }

    public override void RushOver()
    {
        base.RushOver();
        ClawEffect.ClawEffectOver();
        OverPS.SetActive(true);
        colliderRangeChangeByTimeManual.SkillCircleOver();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isFake && collision.gameObject.tag == "Player")
        {
            ParentWeavile.CollisionEnter2DEvent_Rush(collision.gameObject);
        }
    }
}
