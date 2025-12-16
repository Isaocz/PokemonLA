using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FroslassAurora : MonoBehaviour
{


    /// <summary>
    /// 技能圈碰撞箱
    /// </summary>
    public SkillColliderRangeChangeByTimeManual SkillCollider;

    /// <summary>
    /// 技能指示圈
    /// </summary>
    public SkillRangeCircleManual SkillRangeCircle;

    /// <summary>
    /// 极光特效
    /// </summary>
    public AuroraEffect Aurora;

    /// <summary>
    /// 施加极光幕buff的敌人列表
    /// </summary>
    List<Empty> BuffList = new List<Empty> { };




    // Start is called before the first frame update
    void Start()
    {
        //Timer.Start( this , 12.0f , ()=> {
        //    AuroraOver();
        //} );
    }



    /// <summary>
    /// 进入碰撞箱
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Empty")
        {
            Empty e = other.GetComponent<Empty>();
            if (!BuffList.Contains(e))
            {
                BuffList.Add(e);
                e.DefChange(2, 0.0f);
                e.SpDChange(2, 0.0f);
            }

        }
    }


    /// <summary>
    /// 离开碰撞箱
    /// </summary>
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Empty")
        {
            Empty e = other.GetComponent<Empty>();
            if (BuffList.Contains(e))
            {
                BuffList.Remove(e);
                e.DefChange(-2, 0);
                e.SpDChange(-2, 0.0f);
            }

        }
    }








    /// <summary>
    /// 移除所有施加buff的敌人
    /// </summary>
    public void RemoveBuffEmptyList()
    {
        foreach (Empty e in BuffList)
        {

            e.AtkChange(-2, 0);
            e.SpAChange(-2, 0);
        }
        BuffList.Clear();
    }





    /// <summary>
    /// 极光慕结束时渐出碰撞圈指示圈特效
    /// </summary>
    public void AuroraOver()
    {
        Aurora.AuroraOverStart();
        SkillCollider.SkillCircleOver();
        SkillRangeCircle.SkillCircleOver();
    }



    private void OnDestroy()
    {
        RemoveBuffEmptyList();
    }










}
