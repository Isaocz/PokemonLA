using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
/// <summary>
/// �浵
/// </summary>
public class SaveData
{
    public static int[] ExpRequired = new int[] { 0, 5000, 18000, 40000, 80000, 180000, 300000, 550000, 900000, 1000000 };

    /// <summary>
    /// ð���ŵȼ��ĳƺ�
    /// </summary>
    public static List<string> GroupLevelName = new List<string>
    {
        "��ѿ��ð����" , "��ʯ��ð����" ,"�����ð����" ,
        "�Ȼ�ð����" , "������ð����" ,"���㼶ð����" ,
        "��˵��ð����" , "���֮ð����" ,"��֮ð����" ,
    };

    /// <summary>
    /// �浵�����к�
    /// </summary>
    public int SaveIndex;
    /// <summary>
    /// �浵�İ汾��
    /// </summary>
    public string SaveVersion;
    /// <summary>
    /// ���������
    /// </summary>
    public string SaveName;
    /// <summary>
    /// ��ð�յ���
    /// </summary>
    public int APTotal;
    /// <summary>
    /// ��ð�մ���
    /// </summary>
    public int GameCount;
    /// <summary>
    /// �ϴ�����ʱ��
    /// </summary>
    public string LastGameTime;


    /// <summary>
    /// ð�յ������
    /// </summary>
    public int AP;
    /// <summary>
    /// ð���ŵȼ�
    /// </summary>
    public int GroupLevel;
    /// <summary>
    /// ��ҵĳɾ�
    /// </summary>
    public List<PlayerAchievement> PlayerAchievementList;
    /// <summary>
    /// ��ɫ�б�
    /// </summary>
    public List<RoleInfo> RoleList;
    /// <summary>
    /// �ѽ����ĳ�ʼһ���Ե���
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
/// ��¼ĳһ����ɫ����
/// </summary>
public class RoleInfo
{
    /// <summary>
    /// ��ɫ�Ƿ����
    /// </summary>
    public bool isUnlock;
    /// <summary>
    /// ��ɫ��ð�մ���
    /// </summary>
    public int PlayCount;
    /// <summary>
    /// ��ɫ���ǹ���
    /// </summary>
    public int Candy;
    /// <summary>
    /// ��ɫ��HP�츳ֵ
    /// </summary>
    public int HPTalent;
    /// <summary>
    /// ��ɫ�Ĺ����츳ֵ
    /// </summary>
    public int AtkTalent;
    /// <summary>
    /// ��ɫ�ķ����츳ֵ
    /// </summary>
    public int DefTalent;
    /// <summary>
    /// ��ɫ���ع��츳ֵ
    /// </summary>
    public int SpATalent;
    /// <summary>
    /// ��ɫ���ط��츳ֵ
    /// </summary>
    public int SpDTalent;
    /// <summary>
    /// ��ɫ�Ĺ����츳ֵ
    /// </summary>
    public int SpeTalent;
    /// <summary>
    /// ��ɫ�������츳ֵ
    /// </summary>
    public int MoveSpeTalent;
    /// <summary>
    /// ��ɫ�������츳ֵ
    /// </summary>
    public int LuckTalent;
    /// <summary>
    /// ��ɫ��Ԥ�Ƽ�
    /// </summary>
    public PlayerControler Role;
    /// <summary>
    /// ��ɫ�ĳ�ʼ����
    /// </summary>
    public List<Skill> InitialSkills;
    /// <summary>
    /// ��ɫ�������Ƿ����
    /// </summary>
    public bool isDreamAbilityUnlock;
    /// <summary>
    /// ��ɫ�������б�
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
/// ��¼�浵�ɾ�
/// </summary>
public class PlayerAchievement
{
    /// <summary>
    /// �ɾ��Ƿ����
    /// </summary>
    public bool isAchievementUnlock()
    {
        if (Progress >= Target) { return true; }
        else { return false; }
    }
    /// <summary>
    /// �ɾ͵ĵ�ǰ����
    /// </summary>
    public int Progress;
    /// <summary>
    /// ���ȵ����Ŀ�꣬����ʱ�����ɾ�
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

    //��ɫ��Ԥ�Ƽ�
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
        if (RolePerfabs.playerAbility01 != PlayerControler.PlayerAbilityList.������ ) { SaveInfo.RoleAbility.Add(RolePerfabs.playerAbility01); }
        if (RolePerfabs.playerAbility02 != PlayerControler.PlayerAbilityList.������ ) { SaveInfo.RoleAbility.Add(RolePerfabs.playerAbility02); }
    }


    /// <summary>
    /// �浵
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
    /// ����
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

            //����������³ɾͣ����뵽�浵��
            if (s.PlayerAchievementList.Count < AchievementList.AllAchievementList.Count)
            {
                for (int i = s.PlayerAchievementList.Count; i < AchievementList.AllAchievementList.Count; i++)
                {
                    s.PlayerAchievementList.Add(new PlayerAchievement(AchievementList.AllAchievementList[i].AchiCountTarget));
                }
            }
            //����������½�ɫ�����뵽�浵��
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
    /// ɾ��
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
