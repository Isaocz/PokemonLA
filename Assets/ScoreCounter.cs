using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreCounter : MonoBehaviour
{

    //分数计算
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

    //层数奖励
    public int FloorBounsAP
    {
        get { return floorBounsAP; }
        set { floorBounsAP = value; }
    }
    public int floorBounsAP;

    //房间探索奖励
    public int RoomBounsAP
    {
        get { return roomBounsAP; }
        set { roomBounsAP = value; }
    }
    public int roomBounsAP;


    //打倒敌人奖励
    public int EmptyBounsAP
    {
        get { return emptyBounsAP; }
        set { emptyBounsAP = value; }
    }
    public int emptyBounsAP;


    //道具奖励
    public int ItemBounsAP
    {
        get { return itemBounsAP; }
        set { itemBounsAP = value; }
    }
    public int itemBounsAP;


    //技能奖励
    public int SkillBounsAP
    {
        get { return skillBounsAP; }
        set { skillBounsAP = value; }
    }
    public int skillBounsAP;





    //时间惩罚
    public int TimePunishAP
    {
        get { return timePunishAP; }
        set { timePunishAP = value; }
    }
    public int timePunishAP;

    /// <summary>
    /// 时间惩罚不会超过总数的三分之一
    /// </summary>
    /// <returns></returns>
    public int TimePunishAPMax()
    {
        int Output = Mathf.Clamp(timePunishAP , 0 ,( floorBounsAP + roomBounsAP + emptyBounsAP + itemBounsAP + skillBounsAP )/3 );
        return Output;

    }



    //受伤惩罚
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
    /// 全部分数
    /// </summary>
    /// <returns></returns>
    public int TotalAP()
    {
        int Output = floorBounsAP + roomBounsAP + emptyBounsAP + itemBounsAP + skillBounsAP - TimePunishAPMax() - DmagePunishAPMax();
        Output = (int)((float)Output * (1.0f + (ClearGameBouns ? 0.5f : 0.0f) + (RoleBouns ? 0.2f : 0.0f)));
        return Output;
    }





    //奖励糖果
    public int CandyBouns
    {
        get { return candyBouns; }
        set { candyBouns = value; }
    }
    public int candyBouns;



    //通关奖励
    public bool ClearGameBouns
    {
        get { return clearGameBouns; }
        set { clearGameBouns = value; }
    }
    public bool clearGameBouns;

    //跃跃欲试奖励
    public bool RoleBouns
    {
        get { return roleBouns; }
        set { roleBouns = value; }
    }
    public bool roleBouns;


    //冒险团是否升级
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
