using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
/// <summary>
/// 存档
/// </summary>
public class SaveData
{
    public static int[] ExpRequired = new int[] { 0, 5000, 18000, 40000, 80000, 180000, 300000, 550000, 900000, 1000000 };

    /// <summary>
    /// 冒险团等级的称号
    /// </summary>
    public static List<string> GroupLevelName = new List<string>
    {
        "萌芽级冒险团" , "磐石级冒险团" ,"坚冰级冒险团" ,
        "热火级冒险团" , "不屈级冒险团" ,"顶点级冒险团" ,
        "传说级冒险团" , "大地之冒险团" ,"天之冒险团" ,
    };

    /// <summary>
    /// 存档的序列号
    /// </summary>
    public int SaveIndex;
    /// <summary>
    /// 存档的版本号
    /// </summary>
    public string SaveVersion;
    /// <summary>
    /// 工会的名字
    /// </summary>
    public string SaveName;
    /// <summary>
    /// 总冒险点数
    /// </summary>
    public int APTotal;
    /// <summary>
    /// 总冒险次数
    /// </summary>
    public int GameCount;
    /// <summary>
    /// 上次游玩时间
    /// </summary>
    public string LastGameTime;


    /// <summary>
    /// 冒险点数余额
    /// </summary>
    public int AP;
    /// <summary>
    /// 冒险团等级
    /// </summary>
    public int GroupLevel;
    /// <summary>
    /// 玩家的成就
    /// </summary>
    public List<PlayerAchievement> PlayerAchievementList;
    /// <summary>
    /// 角色列表
    /// </summary>
    public List<RoleInfo> RoleList;
    /// <summary>
    /// 已解锁的初始一次性道具
    /// </summary>
    public List<int> UnlockInitalItem;


    public SaveData(int saveIndex, string saveName, string lastGameTime)
    {
        SaveIndex = saveIndex;
        SaveName = saveName;
        APTotal = 0;
        GameCount = 0;
        LastGameTime = lastGameTime;

        SaveVersion = Application.version;

        AP = 0;
        GroupLevel = 0;
        PlayerAchievementList = new List<PlayerAchievement> { };
        for (int i = 0;i < AchievementList.AllAchievementList.Count; i++)
        {
            Debug.Log(AchievementList.AllAchievementList[i].AchiCountTarget);
            PlayerAchievementList.Add(new PlayerAchievement(AchievementList.AllAchievementList[i].AchiCountTarget));
        }
        RoleList = new List<RoleInfo> { };
        UnlockInitalItem = new List<int> { };
    }
}

[System.Serializable]
/// <summary>
/// 记录某一个角色进度
/// </summary>
public class RoleInfo
{
    /// <summary>
    /// 角色是否解锁
    /// </summary>
    public bool isUnlock;
    /// <summary>
    /// 角色的冒险次数
    /// </summary>
    public int PlayCount;
    /// <summary>
    /// 角色的糖果数
    /// </summary>
    public int Candy;
    /// <summary>
    /// 角色的HP天赋值
    /// </summary>
    public int HPTalent;
    /// <summary>
    /// 角色的攻击天赋值
    /// </summary>
    public int AtkTalent;
    /// <summary>
    /// 角色的防御天赋值
    /// </summary>
    public int DefTalent;
    /// <summary>
    /// 角色的特攻天赋值
    /// </summary>
    public int SpATalent;
    /// <summary>
    /// 角色的特防天赋值
    /// </summary>
    public int SpDTalent;
    /// <summary>
    /// 角色的攻速天赋值
    /// </summary>
    public int SpeTalent;
    /// <summary>
    /// 角色的移速天赋值
    /// </summary>
    public int MoveSpeTalent;
    /// <summary>
    /// 角色的幸运天赋值
    /// </summary>
    public int LuckTalent;
    /// <summary>
    /// 角色的预制件
    /// </summary>
    public PlayerControler Role;
    /// <summary>
    /// 角色的初始技能
    /// </summary>
    public List<Skill> InitialSkills;
    /// <summary>
    /// 角色的梦特是否解锁
    /// </summary>
    public bool isDreamAbilityUnlock;
    /// <summary>
    /// 角色的特性列表
    /// </summary>
    public List<PlayerControler.PlayerAbilityList> RoleAbility;

    public RoleInfo()
    {
        this.isUnlock = false;
        PlayCount = 0;
        Candy = 0;
        HPTalent = 0;
        AtkTalent = 0;
        DefTalent = 0;
        SpATalent = 0;
        SpDTalent = 0;
        SpeTalent = 0;
        MoveSpeTalent = 0;
        LuckTalent = 0;
        Role = null;
        InitialSkills = new List<Skill> { };
        isDreamAbilityUnlock = false;
        RoleAbility = new List<PlayerControler.PlayerAbilityList> { };
    }
}

[System.Serializable]
/// <summary>
/// 记录存档成就
/// </summary>
public class PlayerAchievement
{
    /// <summary>
    /// 成就是否解锁
    /// </summary>
    public bool isAchievementUnlock()
    {
        if (Progress >= Target) { return true; }
        else { return false; }
    }
    /// <summary>
    /// 成就的当前进度
    /// </summary>
    public int Progress;
    /// <summary>
    /// 进度到达的目标，到达时解锁成就
    /// </summary>
    public int Target;

    public PlayerAchievement( int target)
    {
        Progress = 0;
        Target = target;
    }
}



public class SaveSystem : MonoBehaviour
{

    public static SaveSystem saveSystem;

    //角色的预制件
    public PlayerControler[] PlayerControlerList;

    private void Awake()
    {
        saveSystem = this;
        DontDestroyOnLoad(gameObject);
    }


    public void NewGame(int SaveIndex , string SaveName)
    {
        System.DateTime now = System.DateTime.Now;
        SaveData s = new SaveData(SaveIndex, SaveName,now.ToString("yyyy-MM-dd HH:mm"));

        for (int i = 0; i < PlayerControlerList.Length; i++)
        {
            RoleInfo r = new RoleInfo();
            if (i == 0) { r.isUnlock = true; }
            SaveRole(r, PlayerControlerList[i]);
            s.RoleList.Add(r);
        }
        SaveGame(s);
    }

    void SaveRole(RoleInfo SaveInfo, PlayerControler RolePerfabs)
    {
        SaveInfo.Role = RolePerfabs;
        if (RolePerfabs.InitialSkill01 != null ) { SaveInfo.InitialSkills.Add(RolePerfabs.InitialSkill01); }
        if (RolePerfabs.InitialSkill02 != null ) { SaveInfo.InitialSkills.Add(RolePerfabs.InitialSkill02); }
        if (RolePerfabs.InitialSkill03 != null ) { SaveInfo.InitialSkills.Add(RolePerfabs.InitialSkill03); }
        if (RolePerfabs.InitialSkill04 != null ) { SaveInfo.InitialSkills.Add(RolePerfabs.InitialSkill04); }
        if (RolePerfabs.playerAbility01 != PlayerControler.PlayerAbilityList.无特性 ) { SaveInfo.RoleAbility.Add(RolePerfabs.playerAbility01); }
        if (RolePerfabs.playerAbility02 != PlayerControler.PlayerAbilityList.无特性 ) { SaveInfo.RoleAbility.Add(RolePerfabs.playerAbility02); }
    }


    /// <summary>
    /// 存档
    /// </summary>
    /// <param name="data"></param>
    public void SaveGame(SaveData data)
    {
        System.DateTime now = System.DateTime.Now;
        data.LastGameTime = now.ToString("yyyy-MM-dd HH:mm");

        string path = Application.persistentDataPath + "/Save" + data.SaveIndex + ".json";
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(path, json);
    }


    /// <summary>
    /// 读档
    /// </summary>
    /// <param name="SaveIndex"></param>
    /// <returns></returns>
    public SaveData LoadGame(int SaveIndex)
    {
        string path = Application.persistentDataPath + "/Save" + SaveIndex + ".json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);

            SaveData s = JsonUtility.FromJson<SaveData>(json);

            //如果更新了新成就，加入到存档中
            if (s.PlayerAchievementList.Count < AchievementList.AllAchievementList.Count)
            {
                for (int i = s.PlayerAchievementList.Count; i < AchievementList.AllAchievementList.Count; i++)
                {
                    s.PlayerAchievementList.Add(new PlayerAchievement(AchievementList.AllAchievementList[i].AchiCountTarget));
                }
            }
            //如果更新了新角色，加入到存档中
            if (s.RoleList.Count < PlayerControlerList.Length)
            {
                for (int i = s.RoleList.Count; i < PlayerControlerList.Length; i++)
                {
                    RoleInfo r = new RoleInfo();
                    if (i == 0) { r.isUnlock = true; }
                    SaveRole(r, PlayerControlerList[i]);
                    s.RoleList.Add(r);
                }
            }
            for (int i = 0; i < PlayerControlerList.Length; i++)
            {
                s.RoleList[i].Role = PlayerControlerList[i];
            }
            SaveGame(s);

            return s;

        }
        return null;
    }

    public bool ExitSave(int SaveIndex)
    {
        string path = Application.persistentDataPath + "/Save" + SaveIndex + ".json";
        return File.Exists(path);
    }


    /// <summary>
    /// 删档
    /// </summary>
    /// <param name="SaveIndex"></param>
    public void DeleteSave(int SaveIndex)
    {
        string path = Application.persistentDataPath + "/Save" + SaveIndex + ".json";
        if (File.Exists(path))
        {
            File.Delete(path);
            Debug.Log("Save file deleted.");
        }
        else
        {
            Debug.LogWarning("Save file not found.");
        }
    }




}
