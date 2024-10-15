using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndeedeeNPC : TownNPC
{
    /// <summary>
    /// �����̵�״̬
    /// </summary>
    public enum IndeedeeState
    {
        Dizzy,          //��·
        InTown,         //�̹���δ���� ��С����
        InMilkBar,      //�̹ݽ��� �ڰ�̨��
    }
    public IndeedeeState State;
    


    void Start()
    {
        NPCStart();
        JudgeStartState();
    }

    /// <summary>
    /// ���ݽ������ó�ʼ״̬
    /// </summary>
    void JudgeStartState()
    {
        if (SaveLoader.saveLoader == null) { State = IndeedeeState.Dizzy; } //�޴浵
        else
        {
            if (!SaveLoader.saveLoader.saveData.TownNPCDialogState.isStateWithIndeedee01)
            {
                State = IndeedeeState.Dizzy; //��·��
            }
            else
            {
                if (SaveLoader.saveLoader.saveData.TownNPCDialogState.isStateWithIndeedee02)
                {
                    State = IndeedeeState.InMilkBar; //�̹ݽ���
                }
                else
                {
                    State = IndeedeeState.InTown; //�̹���δ����
                }
            }
        }
        SetStartPosition();
    }

    /// <summary>
    /// ����״̬���ó�ʼλ��
    /// </summary>
    void SetStartPosition()
    {
        switch (State)
        {
            case IndeedeeState.Dizzy:
                gameObject.SetActive(false);
                break;
            case IndeedeeState.InTown:
                gameObject.SetActive(true);
                transform.parent = TownMap.townMap.TownNPCParent;
                transform.localPosition = new Vector3(3.33f , 19.47f , 0);
                break;
            case IndeedeeState.InMilkBar:
                gameObject.SetActive(true);
                transform.parent = TownMap.townMap.TownNPCParent;
                transform.localPosition = new Vector3(2.1f, 1.3f, 0);
                break;
            default:
                gameObject.SetActive(true);
                transform.parent = TownMap.townMap.TownNPCParent;
                transform.localPosition = new Vector3(3.33f, 19.47f, 0);
                break;

        }
    }


    // Update is called once per frame
    void Update()
    {
        PlayerisinTrigger();

        if (TalkPanel.gameObject.activeSelf == false)
        {

            NPCUpdate();
        }
    }


    private void OnTriggerStay2D(Collider2D other)
    {
        NPCOnTriggerStay2D(other);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        NPCOnTriggerExit2D(other);
    }
}
