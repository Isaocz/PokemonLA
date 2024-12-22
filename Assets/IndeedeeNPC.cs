using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndeedeeNPC : TownNPC
{
    /// <summary>
    /// 爱管侍的状态
    /// </summary>
    public enum IndeedeeState
    {
        Dizzy,          //迷路
        InTownCenter,   //奶馆尚未建立 在树林中心 树林尚未清理
        InTownUR,         //奶馆尚未建立 树林已清理
        InMilkBar,      //奶馆建立 在吧台里
    }
    public IndeedeeState State;
    


    void Start()
    {
        NPCStart();
        Invoke("JudgeStartState" , 0.03f);
    }

    /// <summary>
    /// 根据进度设置初始状态
    /// </summary>
    void JudgeStartState()
    {
        if (SaveLoader.saveLoader == null) { State = IndeedeeState.Dizzy; } //无存档
        else
        {
            if (!SaveLoader.saveLoader.saveData.TownNPCDialogState.isStateWithIndeedee01)
            {
                State = IndeedeeState.Dizzy; //迷路中
            }
            else
            {
                if (SaveLoader.saveLoader.saveData.TownNPCDialogState.isStateWithIndeedee02)
                {
                    State = IndeedeeState.InMilkBar; //奶馆建立
                }
                else
                {
                    Debug.Log(SaveLoader.saveLoader.saveData.TownNPCDialogState.isStateWithIndeedee05);
                    if (!SaveLoader.saveLoader.saveData.TownNPCDialogState.isStateWithIndeedee05) {
                        State = IndeedeeState.InTownCenter; //奶馆尚未建立 树林未开拓
                    }
                    else {
                        State = IndeedeeState.InTownUR; //奶馆尚未建立 树林完成开拓
                    }
                    
                }
            }
        }
        SetStartPosition();
    }

    /// <summary>
    /// 根据状态设置初始位置
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
