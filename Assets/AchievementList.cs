using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 成就列表
/// </summary>
public static class AchievementList 
{
    /// <summary>
    /// 成就的结构
    /// </summary>
    public struct Achievement
    {
        public Achievement(int achiIndex, string achiName, string achiDescribe, int achiCountTarget)
        {
            AchiIndex = achiIndex;
            AchiName = achiName;
            AchiDescribe = achiDescribe;
            AchiCountTarget = achiCountTarget;
        }

        /// <summary>
        /// 成就的序列号
        /// </summary>
        public int AchiIndex { get; set; }
        /// <summary>
        /// 成就的名字
        /// </summary>
        public string AchiName { get; set; }
        /// <summary>
        /// 成就的描述
        /// </summary>
        public string AchiDescribe { get; set; }
        /// <summary>
        /// 成就的目标回数，当完成了某事AchiCountTarget回后，成就解锁。
        /// </summary>
        public int AchiCountTarget { get; set; }


    }


    public static List<Achievement> AllAchievementList = new List<Achievement>
    {
        new Achievement(   0 ,   "萌芽冒险家" ,                                                          "打败处于呼呼森林的某只头目宝可梦" ,     1 ),
        new Achievement(   1 ,   "碎岩冒险家" ,                                                          "打败处于隆隆山洞的某只头目宝可梦" ,     1 ),
        new Achievement(   2 ,   "踏雪冒险家" ,                                                          "打败处于萧萧雪山的某只头目宝可梦" ,     1 ),
        new Achievement(   3 ,       "大胃王" ,                                                                            "食用一百个树果" ,   100 ),
        new Achievement(   3 ,       "伐木工" ,                                                                        "砍倒40颗脆弱的小树" ,    40 ),
    };

}
