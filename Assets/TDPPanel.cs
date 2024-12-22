using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TDPPanel : MonoBehaviour
{


    /// <summary>
    /// ��ťԤ����
    /// </summary>
    public TownDevelopmentProjectButton Button;

    /// <summary>
    /// ��ť�ĸ�����
    /// </summary>
    public GameObject ButtonParent;

    /// <summary>
    /// ��ʾ��ǰAP���
    /// </summary>
    public Text NowAP;


    /// <summary>
    /// �л���ǰ��Ŀҳ���л���
    /// </summary>
    public TDPPanelSwitch tdpPanelSwitch;


    public UIDescribe uiDescribe;


    WoodenHouseConkeldurrTalkPanel ParentPanel;



    //������Ŀ�б�
    List<List<TownDevelopmentProject>> ProjectList = new List<List<TownDevelopmentProject>>
    {
        new List<TownDevelopmentProject>{ },//С��
        new List<TownDevelopmentProject>{ },//�޽�ľ��
        new List<TownDevelopmentProject>{ },//ð���߹���
        new List<TownDevelopmentProject>{ },//�̰�
        new List<TownDevelopmentProject>{ },//���ܻ���
        new List<TownDevelopmentProject>{ },//����Բ
        new List<TownDevelopmentProject>{ },//����Բ��¥
        new List<TownDevelopmentProject>{ },//�����̵�
        new List<TownDevelopmentProject>{ },//ͷĿ���ֲ�
        new List<TownDevelopmentProject>{ },//��ʯ���ֲ�
    };








    private void OnEnable()
    {
        ParentPanel = transform.parent.GetChild(0).GetComponent<WoodenHouseConkeldurrTalkPanel>();
        tdpPanelSwitch.RemoveAllSwitch();
        tdpPanelSwitch.ParentPanel = this;
        SetGroupPanel();
        
    }

    void ResetList()
    {
        ProjectList = new List<List<TownDevelopmentProject>>
        {
            new List<TownDevelopmentProject>{ },//С��
            new List<TownDevelopmentProject>{ },//�޽�ľ��
            new List<TownDevelopmentProject>{ },//ð���߹���
            new List<TownDevelopmentProject>{ },//�̰�
            new List<TownDevelopmentProject>{ },//���ܻ���
            new List<TownDevelopmentProject>{ },//����Բ
            new List<TownDevelopmentProject>{ },//����Բ��¥
            new List<TownDevelopmentProject>{ },//�����̵�
            new List<TownDevelopmentProject>{ },//ͷĿ���ֲ�
            new List<TownDevelopmentProject>{ },//��ʯ���ֲ�
        };
    }


    public void SetGroupPanel()
    {
        if (SaveLoader.saveLoader != null)
        {
            TownLoader.CheckforUnlock();

            _mTool.RemoveAllChild(ButtonParent);

            NowAP.text = SaveLoader.saveLoader.saveData.AP.ToString();
            List <TownDevelopmentProject> l = SaveLoader.saveLoader.saveData.TownDevelopmentProjectsList;
            ResetList();


            for (int i = 0; i < l.Count; i++)
            {
                if (l[i].ProjectProgress == TownDevelopmentProject.ProjectStatus.NotStarted || l[i].ProjectProgress == TownDevelopmentProject.ProjectStatus.NotSelected)
                {
                    ProjectList[(int)l[i].ProjectLoca].Add(l[i]);
                    //Instantiate(Button, ButtonParent.transform).SetTDPButton(l[i], uiDescribe , ParentPanel , this);
                }
                
            }
            for (int i = 0; i < ProjectList.Count; i ++)
            {
                if (ProjectList[i].Count != 0) 
                {
                    tdpPanelSwitch.AddTDPSwitch(ProjectList[i][0].ProjectLoca);
                }
            }

            SetTDPButton((int)tdpPanelSwitch.ProjectLocationList[tdpPanelSwitch.NowProjectLocationIndex]);
        }
    }


    public void SetTDPButton(int Index)
    {
        List<TownDevelopmentProject> l = ProjectList[Index];
        _mTool.RemoveAllChild(ButtonParent);
        Debug.Log("Remove");
        for (int i = 0; i < l.Count; i++)
        {
            Instantiate(Button, ButtonParent.transform).SetTDPButton(l[i], uiDescribe, ParentPanel, this);
        }
        RefreshPanel();
    }





    /// <summary>
    /// ������Ŀ��ˢ�½���
    /// </summary>
    public void RefreshPanel()
    {
        NowAP.text = SaveLoader.saveLoader.saveData.AP.ToString();
        for (int i = 0; i < ButtonParent.transform.childCount; i++)
        {
            ButtonParent.transform.GetChild(i).GetComponent<TownDevelopmentProjectButton>().SetButtonState();
        }
    }

}
