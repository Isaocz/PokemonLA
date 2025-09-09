using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownNPC : NPC<PlayerPokemon>
{
    public enum NPCState
    {
        NotJoined,  //未加入小镇
        NotinTown,  //加入小镇但不在小镇
        Idle,       //站着不动
        Walk,       //走路中
    }
    public NPCState npcState;

    /// <summary>
    /// 表示NPC所处位置
    /// </summary>
    public enum NPCLocation
    {
        inMilkBar, //在酒吧中
        inTown,    //在镇上
        inWoodenHouse,    //在铁骨建筑公司木屋内
        inSkillMaker,    //在图图技能艺术廊内
        inDayCareF1,    //在破壳宝育园一层
        inDayCareF2,    //在破壳宝育园二层
        inItemShop,    //在道具商店
        inBossClub,    //在头目俱乐部
        inPoliceStation,    //在冒险家俱乐部
        inRockClub,    //在滚石俱乐部
    }
    public NPCLocation location;

    /// <summary>
    /// NPC朝向
    /// </summary>
    public Vector2 Director;


    //■■■■■■■■■■■■■■■■■■■■!!TODO!!■■■■■■■■■■■■■■■■■■■■■■
    //■■！！TODO初始化状态机npcState的方法
    //■■！！TODO根据位置初始化状态机location的方法


    //■■■■■■■■■■■■■■■■■■■■移动相关■■■■■■■■■■■■■■■■■■■■■■

    /// <summary>
    /// 设置朝向和动画机方向
    /// </summary>
    public void SetDirector(Vector2 director)
    {
        Director = director;
        animator.SetFloat("LookX", director.x);
        animator.SetFloat("LookY", director.y);
    }

    Vector3 LastPosition;//计算当前速度,朝向时，采用的上一时间单位的位置坐标,通过携程执行

    /// <summary>
    /// 设置动画机速度
    /// </summary>
    /// <returns></returns>
    public IEnumerator CheckLook()
    {
        while (true)
        {
            //一般状态时更改速度和朝向
            if (npcState == NPCState.Walk)
            {
                //根据当前位置和上一次FixedUpdate调用时的位置差计算速度
                animator.SetFloat("Speed", Mathf.Abs((transform.position - LastPosition).magnitude));
                //根据当前位置和上一次FixedUpdate调用时的位置差计算朝向 并传给动画组件
                //SetDirector(_mTool.MainVector2((transform.position - LastPosition)));
                //重置位置
                LastPosition = transform.position;
            }
            else
            {
                animator.SetFloat("Speed", 0);
            }
            yield return new WaitForSeconds(0.1f);
        }

    }

    //■■■■■■■■■■■■■■■■■■■■移动相关■■■■■■■■■■■■■■■■■■■■■■

}
