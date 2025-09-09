using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownNPC : NPC<PlayerPokemon>
{
    public enum NPCState
    {
        NotJoined,  //δ����С��
        NotinTown,  //����С�򵫲���С��
        Idle,       //վ�Ų���
        Walk,       //��·��
    }

    /// <summary>
    /// ��ʾNPC����λ��
    /// </summary>
    public enum NPCLocation
    {
        inMilkBar, //�ھư���
        inTown,    //������
        inWoodenHouse,    //�����ǽ�����˾ľ����
        inSkillMaker,    //��ͼͼ������������
        inDayCareF1,    //���ƿǱ���԰һ��
        inDayCareF2,    //���ƿǱ���԰����
        inItemShop,    //�ڵ����̵�
        inBossClub,    //��ͷĿ���ֲ�
        inPoliceStation,    //��ð�ռҾ��ֲ�
        inRockClub,    //�ڹ�ʯ���ֲ�
    }

    public NPCLocation location;
}
