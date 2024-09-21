using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TSRPanelTelentRader : MonoBehaviour
{

    /// <summary>
    /// ˵������
    /// </summary>
    public UIDescribe uIDescribeR;
    /// <summary>
    /// ˵������
    /// </summary>
    public UIDescribe uIDescribeL;
    /// <summary>
    /// �츳ֵ��˵��
    /// </summary>
    public Text TalentInfo;
    /// <summary>
    /// �츳ֵ�״�Ķ����
    /// </summary>
    public UIPolygon TalentRaderUiPolygon;


    /// <summary>
    /// �Ը���ı�
    /// </summary>
    public Text NatureText;
    /// <summary>
    /// �Ը���������ͼ��
    /// </summary>
    public Image NaturePlus;
    /// <summary>
    /// �Ը�ļ������ͼ��
    /// </summary>
    public Image NatureMinus;
    /// <summary>
    /// ָʾ�Ը�Ӱ��������ͼ��0�� 1���� 2���� 3�ع� 4�ط� 5���� 6�������
    /// </summary>
    public Sprite[] NatureAbillitySprite;

    public void SetTalentRaderPanel(RoleInfo role)
    {
        TalentInfo.GetComponent<UICallDescribe>().DescribeUI = uIDescribeR;
        SetRader(role);
        SetNature();

    }


    /// <summary>
    /// �����츳ֵ�״�
    /// </summary>
    /// <param name="role"></param>
    void SetRader(RoleInfo role)
    {
        //�ط� ���� ���� ���� ���� ���� ���� �ع�
        Debug.Log((float)role.SpDTalent);
        Debug.Log((((float)role.SpDTalent + 1.0f) / 32.0f));
        float[] data = { 
            (((float)role.SpDTalent + 1.0f) / 32.0f ), (((float)role.MoveSpeTalent + 1.0f)  / 32.0f), (((float)role.LuckTalent  + 1.0f) / 32.0f), (((float)role.SpeTalent + 1.0f)  / 32.0f),
            (((float)role.DefTalent + 1.0f)  / 32.0f), (((float)role.AtkTalent + 1.0f)  / 32.0f),     (((float)role.HPTalent + 1.0f)  / 32.0f),   (((float)role.SpATalent  + 1.0f) / 32.0f), (((float)role.SpDTalent + 1.0f) / 32.0f ) };
        TalentRaderUiPolygon.VerticesDistances = data;
        TalentRaderUiPolygon.SetVerticesDirty();
    }

    /// <summary>
    /// ���ó�ʼ�Ը����
    /// </summary>
     void SetNature()
    {
        if (StartPanelPlayerData.PlayerData != null) {
            NatureText.text = "���";
            Vector2Int Nature = StartPanelPlayerData.PlayerData.PlayerNature;
            if (Nature.x == 0 || ( Nature.x == Nature.y && Nature.x != 6)) { NaturePlus.gameObject.SetActive(false); }
            else  { NaturePlus.gameObject.SetActive(true); NaturePlus.sprite = NatureAbillitySprite[Nature.x]; }
            if (Nature.y == 0 || ( Nature.x == Nature.y && Nature.x != 6)) { NatureMinus.gameObject.SetActive(false); }
            else { NatureMinus.gameObject.SetActive(true); NatureMinus.sprite = NatureAbillitySprite[Nature.y]; }


            if (Nature.x == 6 || Nature.y == 6)
            {
                NatureText.text = "���";
                switch (Nature.x)
                {
                    case 1: NatureText.text += "(���������׳ɳ�)"; break;
                    case 2: NatureText.text += "(���������׳ɳ�)"; break;
                    case 3: NatureText.text += "(�ع������׳ɳ�)"; break;
                    case 4: NatureText.text += "(�ط������׳ɳ�)"; break;
                    case 5: NatureText.text += "(���ٸ����׳ɳ�)"; break;
                }
                switch (Nature.y)
                {
                    case 1: NatureText.text += "(���������׳ɳ�)"; break;
                    case 2: NatureText.text += "(���������׳ɳ�)"; break;
                    case 3: NatureText.text += "(�ع������׳ɳ�)"; break;
                    case 4: NatureText.text += "(�ط������׳ɳ�)"; break;
                    case 5: NatureText.text += "(���ٸ����׳ɳ�)"; break;
                }
            }
            else if (Nature.x == 1 && Nature.y == 1) { NatureText.text = "�ڷ�"; }
            else if (Nature.x == 1 && Nature.y == 2) { NatureText.text = "�¼�į"; }
            else if (Nature.x == 1 && Nature.y == 3) { NatureText.text = "��ִ"; }
            else if (Nature.x == 1 && Nature.y == 4) { NatureText.text = "��Ƥ"; }
            else if (Nature.x == 1 && Nature.y == 5) { NatureText.text = "�¸�"; }

            else if (Nature.x == 2 && Nature.y == 1) { NatureText.text = "��"; }
            else if (Nature.x == 2 && Nature.y == 2) { NatureText.text = "̹��"; }
            else if (Nature.x == 2 && Nature.y == 3) { NatureText.text = "����"; }
            else if (Nature.x == 2 && Nature.y == 4) { NatureText.text = "����"; }
            else if (Nature.x == 2 && Nature.y == 5) { NatureText.text = "����"; }

            else if (Nature.x == 3 && Nature.y == 1) { NatureText.text = "����"; }
            else if (Nature.x == 3 && Nature.y == 2) { NatureText.text = "������"; }
            else if (Nature.x == 3 && Nature.y == 3) { NatureText.text = "����"; }
            else if (Nature.x == 3 && Nature.y == 4) { NatureText.text = "��"; }
            else if (Nature.x == 3 && Nature.y == 5) { NatureText.text = "�侲"; }

            else if (Nature.x == 4 && Nature.y == 1) { NatureText.text = "�º�"; }
            else if (Nature.x == 4 && Nature.y == 2) { NatureText.text = "��˳"; }
            else if (Nature.x == 4 && Nature.y == 3) { NatureText.text = "����"; }
            else if (Nature.x == 4 && Nature.y == 4) { NatureText.text = "����"; }
            else if (Nature.x == 4 && Nature.y == 5) { NatureText.text = "�Դ�"; }

            else if (Nature.x == 5 && Nature.y == 1) { NatureText.text = "��С"; }
            else if (Nature.x == 5 && Nature.y == 2) { NatureText.text = "����"; }
            else if (Nature.x == 5 && Nature.y == 3) { NatureText.text = "ˬ��"; }
            else if (Nature.x == 5 && Nature.y == 4) { NatureText.text = "����"; }
            else if (Nature.x == 5 && Nature.y == 5) { NatureText.text = "����"; }


        }
    }



}
