using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// AP的奖励点数
/// </summary>
public static class APBounsPoint 
{

    /// <summary>
    /// 通关某一层后的AP奖励值列表
    /// </summary>
    public static int[] FloorBounsList = new int[] {300 , 600 , 1200 , 2500 , 5000 };
    /// <summary>
    /// 根据楼层返回AP奖励
    /// </summary>
    /// <param name="FloorNum"></param>
    /// <returns></returns>
    public static int FloorBouns(int FloorNum)
    {
        return FloorBounsList[FloorNum];
    }

    /// <summary>
    /// 打倒敌人后的AP奖励
    /// </summary>
    public static int EmptyBouns(Empty e , int FloorNum)
    {
        int OutPut = 0;
        if (e.isBoos) 
        {
            OutPut = (int)(250 * (Mathf.Pow(1.2f, FloorNum)));
        }
        else 
        {
            switch (e.HWP.y)
            {
                case 1:
                    OutPut = (int)(10 * (Mathf.Pow(1.2f, FloorNum))); break;
                case 2:
                    OutPut = (int)(25 * (Mathf.Pow(1.2f, FloorNum))); break;
                case 3:
                    OutPut = (int)(45 * (Mathf.Pow(1.2f, FloorNum))); break;
                default:
                    OutPut = (int)(10 * (Mathf.Pow(1.2f, FloorNum))); break;
            }
        }
        return OutPut;
    }

    /// <summary>
    /// 探索房间的AP奖励
    /// </summary>
    public static int RoomBouns(int FloorNum)
    {
        return (int)(12 * (Mathf.Pow(1.2f, FloorNum))); 
    }

    /// <summary>
    /// 获得道具后的AP奖励
    /// </summary>
    public static int ItemBouns = 20;

    /// <summary>
    /// 学习技能后的AP奖励
    /// </summary>
    public static int SkillBouns = 20;

    /// <summary>
    /// 耗时的AP惩罚
    /// </summary>
    public static int TimePunish(float Second , int FloorNum)
    {
        return (int)(Second * 0.6f * (Mathf.Pow(1.35f, FloorNum)));
    }

    /// <summary>
    /// 受伤后的AP惩罚
    /// </summary>
    public static int DmagePunish(int Dmage) 
    {
        return Dmage * 3;
    }

    /// <summary>
    /// 通关某一层后的糖果奖励值列表
    /// </summary>
    public static int[] FloorCandyBounsList = new int[] { 1, 2, 2, 3, 3 };
    /// <summary>
    /// 根据楼层返回糖果奖励
    /// </summary>
    /// <param name="FloorNum"></param>
    /// <returns></returns>
    public static int FloorCandyBouns( int FloorNum )
    {
        return FloorCandyBounsList[FloorNum];
    }

    /// <summary>
    /// 打通游戏未死亡的奖励系数
    /// </summary>
    public static float ClearGameBouns = 0.5f;


    /// <summary>
    /// 跃跃欲试角色的奖励系数
    /// </summary>
    public static float RoleBouns = 0.2f;

}
