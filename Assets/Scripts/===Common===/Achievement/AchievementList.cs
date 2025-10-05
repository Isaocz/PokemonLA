using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// �ɾ��б�
/// </summary>
public static class AchievementList 
{

    public enum AchStatus
    {
        Locked,         //δ����
        InProgress,     //��ʼ ��һ��ð�ս��������
        Completed,      //���
    }


    /// <summary>
    /// �ɾ͵Ľṹ
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
        /// �ɾ͵����к�
        /// </summary>
        public int AchiIndex;
        /// <summary>
        /// �ɾ͵�����
        /// </summary>
        public string AchiName;
        /// <summary>
        /// �ɾ͵�����
        /// </summary>
        public string AchiDescribe;

        /// <summary>
        /// �ɾ͵Ľ���
        /// </summary>
        public string AchiReward;

        /// <summary>
        /// �ɾ͵�ί��������
        /// </summary>
        public string AchiClientMessage;

        /// <summary>
        /// �ɾ͵�ί����
        /// </summary>
        public int AchiClientIndex;

        /// <summary>
        /// �ɾ͵��Ѷ�
        /// </summary>
        public int AchLevel;


        /// <summary>
        /// �ɾ͵�Ŀ��������������ĳ��AchiCountTarget�غ󣬳ɾͽ�����
        /// </summary>
        public int AchiCountTarget;

        /// <summary>
        /// �ɾͿ�ʼʱ��״̬�������������
        /// </summary>
        public AchStatus StartState;

        /// <summary>
        /// �óɾ͵�ǰ�óɾ�
        /// </summary>
        public List<int> LockedList;


        /// <summary>
        /// �óɾ͵�ǰ�ÿ�����Ŀ
        /// </summary>
        public List<int> LockedTDPList;

    }


    public static List<Achievement> AllAchievementList = new List<Achievement>
    {
                                                                                                                                                                                                                                                                                                                                 //NPC  �����Ѷ�����Ŀ����
        new Achievement(   0 ,      "��ѿð�ռ�" ,                                                          "��ܴ��ں���ɭ�ֵ�ĳֻͷĿ�����Ρ�" ,                          "ð���ŵȼ����Ը���һ����" ,                                                                      "�¸ҵ�ȥð�հɣ�ȥ���������ɭ�ֵ�ͷĿ�����Σ�" , 0  ,0 ,     1  , AchStatus.InProgress ,new List<int>{ } ,new List<int>{ }),
        new Achievement(   1 ,      "����ð�ռ�" ,                                                          "��ܴ���¡¡ɽ����ĳֻͷĿ�����Ρ�" ,                          "ð���ŵȼ����Ը���һ����" ,                                                                      "�¸ҵ�ȥð�հɣ�ȥ������¡¡ɽ����ͷĿ�����Σ�" , 0  ,0 ,     1  , AchStatus.Locked     ,new List<int>{0} ,new List<int>{ }),
        new Achievement(   2 ,      "̤ѩð�ռ�" ,                                                          "��ܴ�������ѩɽ��ĳֻͷĿ�����Ρ�" ,                          "ð���ŵȼ����Ը���һ����" ,                                                                      "�¸ҵ�ȥð�հɣ�ȥ����������ѩɽ��ͷĿ�����Σ�" , 0  ,1 ,     1  , AchStatus.Locked     ,new List<int>{1} ,new List<int>{ }),
        new Achievement(   3 ,    "ð�ռҳ��ٹ�" ,                                                                              "ʳ��10��������" ,                              "С�����޻ᶨ��ð����" ,                          "ɭ����������úóԣ�������Щ����̫���ˣ�������λð�ռҰ��ҳ���������������������������ζ����" , 1  ,0 ,    10  , AchStatus.InProgress ,new List<int>{ } ,new List<int>{ }),
        new Achievement(   4 ,          "��ľ��" ,                                                                        "����40�Ŵ�����С����" ,                            "ͼͼ����������<�Ӻ�ն>" ,"��������ڴ���һ���µļ��ܻ������Ѿ���Щ����ˣ������ܸо�����Щʲô���������ȥ����С���������Ӣ������һ���ܻ�����" , 4  ,1 ,    40  , AchStatus.Locked     ,new List<int>{ } ,new List<int>{9}),
        new Achievement(   5 ,    "���������̹�" ,                                                                        "���������̽����̹ݡ�" ,                                  "�ܵ������̵ĸм�" ,                                                                    "�����̹ݵ�Ԥ�㲻���ˡ�����ϣ���к����˰���ҡ�����" , 2  ,0 ,     1  , AchStatus.Locked     ,new List<int>{ } ,new List<int>{0}),
        new Achievement(   6 ,    "�߶��İ�����" ,                                                                          "Ѱ���߶��İ����̡�" ,                                "�����̻ᶨ��ð����" ,                                    "�š����������ƻ�һ����ð����Ļ�鰮����С���ƺ��߶��ˡ�����ϣ�����˰�æ��������" , 3  ,0 ,     1  , AchStatus.InProgress ,new List<int>{ } ,new List<int>{ }),
    };

}
