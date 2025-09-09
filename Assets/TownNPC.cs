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
}
