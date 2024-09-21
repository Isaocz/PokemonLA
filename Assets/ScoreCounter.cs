using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreCounter : MonoBehaviour
{

    //��������
    public static ScoreCounter Instance;

    public void ResetCounter()
    {
        floorBounsAP = 0;
        roomBounsAP = 0;
        emptyBounsAP = 0;
        itemBounsAP = 0;
        skillBounsAP = 0;
        timePunishAP = 0;
        dmagePunishAP = 0;
        candyBouns = 0;
        clearGameBouns = false;
        roleBouns = false;
    }

    //��������
    public int FloorBounsAP
    {
        get { return floorBounsAP; }
        set { floorBounsAP = value; }
    }
    public int floorBounsAP;

    //����̽������
    public int RoomBounsAP
    {
        get { return roomBounsAP; }
        set { roomBounsAP = value; }
    }
    public int roomBounsAP;


    //�򵹵��˽���
    public int EmptyBounsAP
    {
        get { return emptyBounsAP; }
        set { emptyBounsAP = value; }
    }
    public int emptyBounsAP;


    //���߽���
    public int ItemBounsAP
    {
        get { return itemBounsAP; }
        set { itemBounsAP = value; }
    }
    public int itemBounsAP;


    //���ܽ���
    public int SkillBounsAP
    {
        get { return skillBounsAP; }
        set { skillBounsAP = value; }
    }
    public int skillBounsAP;





    //ʱ��ͷ�
    public int TimePunishAP
    {
        get { return timePunishAP; }
        set { timePunishAP = value; }
    }
    public int timePunishAP;

    /// <summary>
    /// ʱ��ͷ����ᳬ������������֮һ
    /// </summary>
    /// <returns></returns>
    public int TimePunishAPMax()
    {
        int Output = Mathf.Clamp(timePunishAP , 0 ,( floorBounsAP + roomBounsAP + emptyBounsAP + itemBounsAP + skillBounsAP )/3 );
        return Output;

    }



    //���˳ͷ�
    public int DmagePunishAP
    {
        get { return dmagePunishAP; }
        set { dmagePunishAP = value; }
    }
    public int dmagePunishAP;
    public int DmagePunishAPMax()
    {
        int Output = Mathf.Clamp(dmagePunishAP, 0, (floorBounsAP + roomBounsAP + emptyBounsAP + itemBounsAP + skillBounsAP) / 3);
        return Output;

    }



    /// <summary>
    /// ȫ������
    /// </summary>
    /// <returns></returns>
    public int TotalAP()
    {
        int Output = floorBounsAP + roomBounsAP + emptyBounsAP + itemBounsAP + skillBounsAP - TimePunishAPMax() - DmagePunishAPMax();
        Output = (int)((float)Output * (1.0f + (ClearGameBouns ? 0.5f : 0.0f) + (RoleBouns ? 0.2f : 0.0f)));
        return Output;
    }





    //�����ǹ�
    public int CandyBouns
    {
        get { return candyBouns; }
        set { candyBouns = value; }
    }
    public int candyBouns;



    //ͨ�ؽ���
    public bool ClearGameBouns
    {
        get { return clearGameBouns; }
        set { clearGameBouns = value; }
    }
    public bool clearGameBouns;

    //ԾԾ���Խ���
    public bool RoleBouns
    {
        get { return roleBouns; }
        set { roleBouns = value; }
    }
    public bool roleBouns;


    //ð�����Ƿ�����
    public bool IsGroupLevelUp
    {
        get { return isGroupLevelUp; }
        set { isGroupLevelUp = value; }
    }
    public bool isGroupLevelUp;


    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(Instance.gameObject);
        
    }







}
