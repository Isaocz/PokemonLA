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
    public NPCState npcState;

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

    /// <summary>
    /// NPC����
    /// </summary>
    public Vector2 Director;


    //����������������������������������������!!TODO!!��������������������������������������������
    //��������TODO��ʼ��״̬��npcState�ķ���
    //��������TODO����λ�ó�ʼ��״̬��location�ķ���


    //�����������������������������������������ƶ���ء�������������������������������������������

    /// <summary>
    /// ���ó���Ͷ���������
    /// </summary>
    public void SetDirector(Vector2 director)
    {
        Director = director;
        animator.SetFloat("LookX", director.x);
        animator.SetFloat("LookY", director.y);
    }

    Vector3 LastPosition;//���㵱ǰ�ٶ�,����ʱ�����õ���һʱ�䵥λ��λ������,ͨ��Я��ִ��

    /// <summary>
    /// ���ö������ٶ�
    /// </summary>
    /// <returns></returns>
    public IEnumerator CheckLook()
    {
        while (true)
        {
            //һ��״̬ʱ�����ٶȺͳ���
            if (npcState == NPCState.Walk)
            {
                //���ݵ�ǰλ�ú���һ��FixedUpdate����ʱ��λ�ò�����ٶ�
                animator.SetFloat("Speed", Mathf.Abs((transform.position - LastPosition).magnitude));
                //���ݵ�ǰλ�ú���һ��FixedUpdate����ʱ��λ�ò���㳯�� �������������
                //SetDirector(_mTool.MainVector2((transform.position - LastPosition)));
                //����λ��
                LastPosition = transform.position;
            }
            else
            {
                animator.SetFloat("Speed", 0);
            }
            yield return new WaitForSeconds(0.1f);
        }

    }

    //�����������������������������������������ƶ���ء�������������������������������������������

}
