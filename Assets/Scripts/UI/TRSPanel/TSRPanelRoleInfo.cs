using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TSRPanelRoleInfo : MonoBehaviour
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
    /// ��ɫ�����ı�
    /// </summary>
    public Text RoleNameText;
    /// <summary>
    /// ��ɫ��ʼ�ȼ�
    /// </summary>
    public Text RoleLevel;
    /// <summary>
    /// ��ɫͷ��
    /// </summary>
    public Image RoleHeadIcon;
    /// <summary>
    /// ��ɫ�ǹ���
    /// </summary>
    public Text RoleCandyCount;
    /// <summary>
    /// ��ɫ�ǹ�ͼ
    /// </summary>
    public Image RoleCandyIcon;
    /// <summary>
    /// ��ɫ�������ֵĸ�����
    /// </summary>
    public Transform RoleEvolutionParent;
    /// <summary>
    /// ��ɫ���Բ��ֵĸ�����
    /// </summary>
    public Transform RoleTypeParent;

    /// <summary>
    /// ��ɫ�����������б�
    /// </summary>
    public GameObject[] RoleEvolutionSituList;
    /// <summary>
    /// ���Ա�־
    /// </summary>
    public UIPokemonDataPanelTypemark TypeMark;


    /// <summary>
    /// ���ý�ɫ��Ϣ����
    /// </summary>
    public void SetRoleInfoPanel(RoleInfo role )
    {

        _mTool.RemoveAllChild(RoleTypeParent.gameObject);
        _mTool.RemoveAllChild(RoleEvolutionParent.gameObject);

        RoleNameText.text = role.Role.PlayerNameChinese;
        RoleLevel.text = role.Role.Level.ToString();
        RoleHeadIcon.sprite = role.Role.PlayerHead;
        RoleHeadIcon.GetComponent<UICallDescribe>().DescribeUI = uIDescribeR;
        RoleCandyCount.text = role.Candy.ToString() ;
        RoleCandyIcon.sprite = role.Role.PlayerCandy ;

        GameObject EvoSitu = Instantiate(RoleEvolutionSituList[role.Role.PlayerIndex] , RoleEvolutionParent);
        for (int i = 0; i < EvoSitu.transform.childCount; i++)
        {
            EvoSitu.transform.GetChild(i).GetComponent<UICallDescribe>().DescribeUI = uIDescribeR;
        }
        if (role.Role.PlayerType01 != 0)
        {
            Debug.Log(role.Role.PlayerType01);
            UIPokemonDataPanelTypemark t = Instantiate(TypeMark, RoleTypeParent);
            t.GetChildTypeMark(role.Role.PlayerType01);
        }
        if (role.Role.PlayerType02 != 0)
        {
            Debug.Log(role.Role.PlayerType02);
            UIPokemonDataPanelTypemark t = Instantiate(TypeMark, RoleTypeParent);
            t.GetChildTypeMark(role.Role.PlayerType02);
        }
    }
}
