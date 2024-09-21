using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// AP�Ľ�������
/// </summary>
public static class APBounsPoint 
{

    /// <summary>
    /// ͨ��ĳһ����AP����ֵ�б�
    /// </summary>
    public static int[] FloorBounsList = new int[] {300 , 600 , 1200 , 2500 , 5000 };
    /// <summary>
    /// ����¥�㷵��AP����
    /// </summary>
    /// <param name="FloorNum"></param>
    /// <returns></returns>
    public static int FloorBouns(int FloorNum)
    {
        return FloorBounsList[FloorNum];
    }

    /// <summary>
    /// �򵹵��˺��AP����
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
    /// ̽�������AP����
    /// </summary>
    public static int RoomBouns(int FloorNum)
    {
        return (int)(12 * (Mathf.Pow(1.2f, FloorNum))); 
    }

    /// <summary>
    /// ��õ��ߺ��AP����
    /// </summary>
    public static int ItemBouns = 20;

    /// <summary>
    /// ѧϰ���ܺ��AP����
    /// </summary>
    public static int SkillBouns = 20;

    /// <summary>
    /// ��ʱ��AP�ͷ�
    /// </summary>
    public static int TimePunish(float Second , int FloorNum)
    {
        return (int)(Second * 0.6f * (Mathf.Pow(1.35f, FloorNum)));
    }

    /// <summary>
    /// ���˺��AP�ͷ�
    /// </summary>
    public static int DmagePunish(int Dmage) 
    {
        return Dmage * 3;
    }

    /// <summary>
    /// ͨ��ĳһ�����ǹ�����ֵ�б�
    /// </summary>
    public static int[] FloorCandyBounsList = new int[] { 1, 2, 2, 3, 3 };
    /// <summary>
    /// ����¥�㷵���ǹ�����
    /// </summary>
    /// <param name="FloorNum"></param>
    /// <returns></returns>
    public static int FloorCandyBouns( int FloorNum )
    {
        return FloorCandyBounsList[FloorNum];
    }

    /// <summary>
    /// ��ͨ��Ϸδ�����Ľ���ϵ��
    /// </summary>
    public static float ClearGameBouns = 0.5f;


    /// <summary>
    /// ԾԾ���Խ�ɫ�Ľ���ϵ��
    /// </summary>
    public static float RoleBouns = 0.2f;

}
