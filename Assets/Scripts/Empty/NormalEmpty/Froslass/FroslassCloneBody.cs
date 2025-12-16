using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FroslassCloneBody : NormalEmptyCloneBody
{

    /// <summary>
    /// 发射影子球角度
    /// </summary>
    public Vector2 LunchDir;

    /// <summary>
    /// 假影子球
    /// </summary>
    public FakeFroslassShadowBalll Fakesb;


    private void Start()
    {
        Timer.Start(this, DispearTime, () => {
            animator.SetTrigger("Over");
        });
    }


    /// <summary>
    /// 设置方向
    /// </summary>
    public void SetDirector( Vector2 dir )
    {
        animator.SetFloat("LookX" , dir.x);
        animator.SetFloat("LookY" , dir.y);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag != "Room" && collision.gameObject.tag != "Enviroment")
        {
            animator.SetTrigger("Over");
        }
    }



    /// <summary>
    /// 玩家不在极光慕内发射假影子球
    /// </summary>
    public void LunchShadowBall_PlayerNotInAurora(Vector2 TargetPosition , float DeflectionAngle, float Speed)
    {
        //Vector2 LunchDir2 = (TargetPosition - (Vector2)transform.position).normalized;
        Vector2 LunchDir2 = (LunchDir).normalized;
        LunchOneShadowBall(LunchDir2, Speed);
        LunchOneShadowBall(Quaternion.AngleAxis( DeflectionAngle, Vector3.forward) * LunchDir2, Speed);
        LunchOneShadowBall(Quaternion.AngleAxis(-DeflectionAngle, Vector3.forward) * LunchDir2, Speed);
    }


    /// <summary>
    /// 玩家在极光慕内发射假影子球
    /// </summary>
   public void LunchShadowBall_PlayerInAurora(float Speed)
    {
        LunchOneShadowBall(LunchDir , Speed);
    }


    /// <summary>
    /// 发射一个假影子球
    /// </summary>
    void LunchOneShadowBall(Vector2 Dir , float Speed)
    {
        Dir = Dir.normalized;
        FakeFroslassShadowBalll s = Instantiate(Fakesb, (Vector3)Dir * 1.0f + transform.position + Vector3.up * 0.4f, Quaternion.identity);
        s.LaunchNotForce(Dir, Speed);
    }

}
