using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 成就列表
/// </summary>
public static class AchievementList 
{

    public enum AchStatus
    {
        Locked,         //未解锁
        InProgress,     //开始 下一次冒险结束后完成
        Completed,      //完成
    }


    /// <summary>
    /// 成就的结构
    /// </summary>
    [Serializable]
    public struct Achievement
    {
        public Achievement(int achiIndex, string achiName, string achiDescribe,string reward , string clientmessage , int clentIndex , int level , int achiCountTarget , AchStatus s, List<int> l, List<int> l2)
        {
            AchiIndex = achiIndex;
            AchiName = achiName;
            AchiDescribe = achiDescribe;
            AchiCountTarget = achiCountTarget;
            AchLevel = level;
            AchiReward = reward;
            AchiClientMessage = clientmessage;
            AchiClientIndex = clentIndex;
            StartState = s;
            LockedList = l;
            LockedTDPList = l2;
        }


        /// <summary>
        /// 成就的序列号
        /// </summary>
        public int AchiIndex;
        /// <summary>
        /// 成就的名字
        /// </summary>
        public string AchiName;
        /// <summary>
        /// 成就的描述
        /// </summary>
        public string AchiDescribe;

        /// <summary>
        /// 成就的奖励
        /// </summary>
        public string AchiReward;

        /// <summary>
        /// 成就的委托人留言
        /// </summary>
        public string AchiClientMessage;

        /// <summary>
        /// 成就的委托人
        /// </summary>
        public int AchiClientIndex;

        /// <summary>
        /// 成就的难度
        /// </summary>
        public int AchLevel;


        /// <summary>
        /// 成就的目标回数，当完成了某事AchiCountTarget回后，成就解锁。
        /// </summary>
        public int AchiCountTarget;

        /// <summary>
        /// 成就开始时的状态（被锁或解锁）
        /// </summary>
        public AchStatus StartState;

        /// <summary>
        /// 该成就的前置成就
        /// </summary>
        public List<int> LockedList;


        /// <summary>
        /// 该成就的前置开发项目
        /// </summary>
        public List<int> LockedTDPList;

    }


    public static List<Achievement> AllAchievementList = new List<Achievement>
    {
                                                                                                                                                                                                                                                                                                                                 //NPC  任务难度任务目标量
        new Achievement(   0 ,      "萌芽冒险家" ,                                                          "打败处于呼呼森林的某只头目宝可梦。" ,                          "冒险团等级可以更进一步。" ,                                                                      "勇敢的去冒险吧！去打倒坐镇呼呼森林的头目宝可梦！" , 0  ,0 ,     1  , AchStatus.InProgress ,new List<int>{ } ,new List<int>{ }),
        new Achievement(   1 ,      "碎岩冒险家" ,                                                          "打败处于隆隆山洞的某只头目宝可梦。" ,                          "冒险团等级可以更进一步。" ,                                                                      "勇敢的去冒险吧！去打倒坐镇隆隆山洞的头目宝可梦！" , 0  ,0 ,     1  , AchStatus.Locked     ,new List<int>{0} ,new List<int>{ }),
        new Achievement(   2 ,      "踏雪冒险家" ,                                                          "打败处于萧萧雪山的某只头目宝可梦。" ,                          "冒险团等级可以更进一步。" ,                                                                      "勇敢的去冒险吧！去打倒坐镇萧萧雪山的头目宝可梦！" , 0  ,1 ,     1  , AchStatus.Locked     ,new List<int>{1} ,new List<int>{ }),
        new Achievement(   3 ,    "冒险家尝百果" ,                                                                              "食用10种树果。" ,                              "小卡比兽会定居冒险镇" ,                          "森林里的树果好好吃，不过有些树果太辣了！我想找位冒险家帮我尝尝个种树果，看看各种树果的味道。" , 1  ,0 ,    10  , AchStatus.InProgress ,new List<int>{ } ,new List<int>{ }),
        new Achievement(   4 ,          "伐木工" ,                                                                        "砍倒40颗脆弱的小树。" ,                            "图图创作出新作<居合斩>" ,"最近我正在创作一种新的技能机器，已经有些灵感了，但是总感觉还差些什么。想拜托你去砍倒小树，从你的英姿中我一定能获得灵感" , 4  ,1 ,    40  , AchStatus.Locked     ,new List<int>{ } ,new List<int>{9}),
        new Achievement(   5 ,    "建立哞哞奶馆" ,                                                                        "帮助爱管侍建设奶馆。" ,                                  "受到爱管侍的感激" ,                                                                    "建立奶馆的预算不够了。。。希望有好心人帮帮我。。。" , 2  ,0 ,     1  , AchStatus.Locked     ,new List<int>{ } ,new List<int>{0}),
        new Achievement(   6 ,    "走丢的爱管侍" ,                                                                          "寻找走丢的爱管侍。" ,                                "爱管侍会定居冒险镇" ,                                    "嗯。。。本来计划一起建设冒险镇的伙伴爱管侍小姐似乎走丢了。。。希望有人帮忙找找她。" , 3  ,0 ,     1  , AchStatus.InProgress ,new List<int>{ } ,new List<int>{ }),
    };

}
