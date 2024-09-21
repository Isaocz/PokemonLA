using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// �ɾ��б�
/// </summary>
public static class AchievementList 
{
    /// <summary>
    /// �ɾ͵Ľṹ
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
        /// �ɾ͵����к�
        /// </summary>
        public int AchiIndex { get; set; }
        /// <summary>
        /// �ɾ͵�����
        /// </summary>
        public string AchiName { get; set; }
        /// <summary>
        /// �ɾ͵�����
        /// </summary>
        public string AchiDescribe { get; set; }
        /// <summary>
        /// �ɾ͵�Ŀ��������������ĳ��AchiCountTarget�غ󣬳ɾͽ�����
        /// </summary>
        public int AchiCountTarget { get; set; }


    }


    public static List<Achievement> AllAchievementList = new List<Achievement>
    {
        new Achievement(   0 ,   "��ѿð�ռ�" ,                                                          "��ܴ��ں���ɭ�ֵ�ĳֻͷĿ������" ,     1 ),
        new Achievement(   1 ,   "����ð�ռ�" ,                                                          "��ܴ���¡¡ɽ����ĳֻͷĿ������" ,     1 ),
        new Achievement(   2 ,   "̤ѩð�ռ�" ,                                                          "��ܴ�������ѩɽ��ĳֻͷĿ������" ,     1 ),
        new Achievement(   3 ,       "��θ��" ,                                                                            "ʳ��һ�ٸ�����" ,   100 ),
        new Achievement(   3 ,       "��ľ��" ,                                                                        "����40�Ŵ�����С��" ,    40 ),
    };

}
