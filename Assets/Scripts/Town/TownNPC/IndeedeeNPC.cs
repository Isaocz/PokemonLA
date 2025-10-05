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
        InTownCenter,   //�̹���δ���� ���������� ������δ����
        InTownUR,         //�̹���δ���� ����������
        InMilkBar,      //�̹ݽ��� �ڰ�̨��
    }
    public IndeedeeState State;
    


    void Start()
    {
        NPCStart();
        Invoke("JudgeStartState" , 0.03f);
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
                    Debug.Log(SaveLoader.saveLoader.saveData.TownNPCDialogState.isStateWithIndeedee05);
                    if (!SaveLoader.saveLoader.saveData.TownNPCDialogState.isStateWithIndeedee05) {
                        State = IndeedeeState.InTownCenter; //�̹���δ���� ����δ����
                    }
                    else {
                        State = IndeedeeState.InTownUR; //�̹���δ���� ������ɿ���
                    }
                    
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
            case IndeedeeState.InTownCenter:
                gameObject.SetActive(true);
                transform.parent = TownMap.townMap.TownNPCParent;
                transform.localPosition = new Vector3(3.33f , 19.47f , 0);
                break;
            case IndeedeeState.InTownUR:
                gameObject.SetActive(true);
                transform.parent = TownMap.townMap.TownNPCParent;
                transform.localPosition = new Vector3(9.05f, 21.9f, 0);
                break;
            case IndeedeeState.InMilkBar:
                gameObject.SetActive(true);
                transform.parent = TownMap.townMap.buildhouse.MilkBar.NPCParent.transform;
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
