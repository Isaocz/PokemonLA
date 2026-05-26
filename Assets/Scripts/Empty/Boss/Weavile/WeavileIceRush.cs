using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeavileIceRush : WeavileRush
{
    /// <summary>
    /// 碰撞箱控制器
    /// </summary>
    public SkillColliderRangeChangeByTimeManual colliderRangeChangeByTimeManual;

    /// <summary>
    /// 结束粒子特效
    /// </summary>
    public GameObject OverPS;

    public Animator PunchAnimator;


    public override void RushStart(Vector2 d)
    {
        base.RushStart(d);
        // 设置冲刺方向
        //ClawEffect.transform.rotation = Quaternion.Euler(0, 0, _mTool.Angle_360Y(d, Vector2.right));
    }

    public override void RushOver()
    {
        base.RushOver();
        //ClawEffect.ClawEffectOver();
        PunchAnimator.SetTrigger("Over");
        OverPS.SetActive(true);
        _mTool.RemoveAllPSChild(OverPS);
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
