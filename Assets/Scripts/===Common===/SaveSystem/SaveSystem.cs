using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using UnityEngine;


/// <summary>
/// 使用类封装List
/// </summary>
/// <typeparam name="T"></typeparam>
[Serializable]
public class SaveDataWrapper
{
    public SaveData save;

    public SaveDataWrapper(SaveData save)
    {
        this.save = save;
    }
}


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
    /// <summary>
    /// 当前存档的小镇建设情况
    /// </summary>
    public List<TownDevelopmentProject> TownDevelopmentProjectsList;
    /// <summary>
    /// 小镇中NPC的对话情况
    /// </summary>
    public NPCDialogState TownNPCDialogState;

    //===================TODO=======================
    //增加小镇时间TownGlobalTimer的存储
    //===================TODO=======================

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
            PlayerAchievementList.Add(new PlayerAchievement(AchievementList.AllAchievementList[i]));
        }
        RoleList = new List<RoleInfo> { };
        UnlockInitalItem = new List<int> { };
        TownDevelopmentProjectsList = TownLoader.TDPList;

        TownNPCDialogState = new NPCDialogState();
    }

    /// <summary>
    /// 检查成就的解锁情况
    /// </summary>
    public static void CheckforAchUnlock()
    {
        if (SaveLoader.saveLoader != null)
        {
            foreach (PlayerAchievement ach in SaveLoader.saveLoader.saveData.PlayerAchievementList)
            {
                //检查所有已解锁的项目 
                if (ach.State == AchievementList.AchStatus.Locked)
                {
                    bool canUnlockAchi = true;
                    bool canUnlockTDP = true;
                    if (ach.achievement.LockedList.Count != 0)
                    {
                        //检查这些任务的前置任务是否已经处于完成状态。
                        foreach (int i in ach.achievement.LockedList)
                        {
                            if (SaveLoader.saveLoader.saveData.PlayerAchievementList[i].State != AchievementList.AchStatus.Completed)
                            {
                                canUnlockAchi = false;
                                break;
                            }
                        }
                    }
                    if (ach.achievement.LockedTDPList.Count != 0)
                    {
                        //检查这些任务的前置任务是否已经处于完成状态。
                        foreach (int i in ach.achievement.LockedTDPList)
                        {
                            if (SaveLoader.saveLoader.saveData.TownDevelopmentProjectsList[i].ProjectProgress != TownDevelopmentProject.ProjectStatus.Completed)
                            {
                                canUnlockTDP = false;
                                break;
                            }
                        }
                    }
                    if (canUnlockAchi && canUnlockTDP)
                    {
                        ach.State = AchievementList.AchStatus.InProgress;
                    }
                }
            }
        }

    }


    /// <summary>
    /// 某项成就的进度增加
    /// </summary>
    public static void AchProgressPlus(int AchIndex , int value)
    {
        PlayerAchievement a = SaveLoader.saveLoader.saveData.PlayerAchievementList[AchIndex];
        a.Progress = Mathf.Clamp(a.Progress+value, 0 , a.Target);
        a.isAchievementUnlock();
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
        SaveData.CheckforAchUnlock();
        if (State == AchievementList.AchStatus.InProgress)
        {
            if (Progress >= Target) { 
                State = AchievementList.AchStatus.Completed;
                return true;  }
            else { return false; }
        }
        else { return false; }
    }



    /// <summary>
    /// 成就的状态
    /// </summary>
    public AchievementList.AchStatus State = AchievementList.AchStatus.Locked;
    /// <summary>
    /// 成就的内容
    /// </summary>
    public AchievementList.Achievement achievement;
    /// <summary>
    /// 成就的当前进度
    /// </summary>
    public int Progress;
    /// <summary>
    /// 进度到达的目标，到达时解锁成就
    /// </summary>
    public int Target;
    /// <summary>
    /// 记录成就的某个部分已经被完成了，如成就4大胃王，当吃过蓝菊果后，向CompletePartList种记录0 ， 下次使用蓝菊果时，如果CompletePartList中有0，则不在增加
    /// </summary>
    public List<string> CompletePartList = new List<string> { };


    public PlayerAchievement(AchievementList.Achievement a )
    {
        achievement = a;
        Progress = 0;
        Target = a.AchiCountTarget;
        State = a.StartState;
    }

    /// <summary>
    /// 增加成就的进度;
    /// </summary>
    public void AchievementProgressPlus(int Value , string Part)
    {
        if (Part == null || !(CompletePartList.Contains(Part)) )
        {
            if (Part != null) { CompletePartList.Add(Part); }
            Progress += Mathf.Clamp(Progress + Value, 0 , Target);
            isAchievementUnlock();
        }
    }

}



public class SaveSystem : MonoBehaviour
{
    /// <summary>
    /// 当前创建的密钥
    /// </summary>
    private static byte[] currentKey;
    /// <summary>
    /// 当前创建的初始化向量
    /// </summary>
    private static byte[] currentIV;
    /// <summary>
    /// 保存密钥的文件名
    /// </summary>
    private const string KEYFILENAME = "KeyData.txt";
    /// <summary>
    /// 保存初始化向量的文件名
    /// </summary>
    private const string IVFILENAME = "IVData.txt";
    /// <summary>
    /// 使用对称算法加密(Aes算法)
    /// </summary>
    /// <param name="targetStr">目标内容</param>
    /// <param name="key">对称算法的密钥</param>
    /// <param name="iv">对称算法的初始化向量</param>
    /// <returns></returns>
    private static byte[] AesEncryption(string targetStr, byte[] key, byte[] iv)
    {
        #region 检测参数
        //判断内容是否为空
        if (targetStr == null || targetStr.Length <= 0)
        {
            Debug.LogError("目标内容为空！");
            return null;
        }
        //判断密钥与初始化向量
        if (key == null || key.Length <= 0)
        {
            Debug.LogError("输入密钥为空！");
            return null;
        }
        if (iv == null || iv.Length <= 0)
        {
            Debug.LogError("输入初始化向量为空！");
            return null;
        }
        #endregion

        #region 加密
        byte[] encryptionData;          //将数据加密后得到的字节数组

        using (Aes aes = Aes.Create())  //新建密钥
        {
            aes.Key = key;
            aes.IV = iv;
            //创建加密程序以执行流转换
            ICryptoTransform cryptoTF = aes.CreateEncryptor(aes.Key, aes.IV);
            //创建用于加密的流
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoTF, CryptoStreamMode.Write))
                {
                    using (StreamWriter streamWriter = new StreamWriter(cryptoStream))
                    {
                        streamWriter.Write(targetStr);  //将数据写入到流中
                    }
                    encryptionData = memoryStream.ToArray();
                }
            }
            return encryptionData;
        }
        #endregion
    }
    /// <summary>
    /// 对称算法解密
    /// </summary>
    /// <param name="targetBtData">需要解密的数据</param>
    /// <param name="key">密钥</param>
    /// <param name="iv">初始化向量</param>
    /// <returns></returns>
    private static string AesDecrypt(byte[] targetBtData, byte[] key, byte[] iv)
    {
        #region 检测参数
        if (targetBtData == null || targetBtData.Length <= 0)
        {
            Debug.LogError("解密数据为空！");
            return null;
        }
        if (key == null || key.Length <= 0)
        {
            Debug.LogError("输入密钥为空！");
            return null;
        }
        if (iv == null || iv.Length <= 0)
        {
            Debug.LogError("输入初始化向量为空！");
            return null;
        }
        #endregion

        #region 解密
        string decryptStr;

        using (Aes aes = Aes.Create())  //实例化算法类
        {
            aes.Key = key;
            aes.IV = iv;
            //创建解密程序以执行流转换
            ICryptoTransform cryptoTF = aes.CreateDecryptor(aes.Key, aes.IV);
            using (MemoryStream memoryStream = new MemoryStream(targetBtData))
            {
                using (CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoTF, CryptoStreamMode.Read))
                {
                    using (StreamReader streamReader = new StreamReader(cryptoStream)){
                        decryptStr = streamReader.ReadToEnd();
                    }
                }
            }
            return decryptStr;
        }
        #endregion
    }
    /// <summary>
    /// 保存当前密钥以及初始化向量
    /// </summary>
    /// <param name="keyBt">密钥</param>
    /// <param name="ivBt">初始化向量</param>
    private static void SaveCurrentKeyAndIv(byte[] keyBt, byte[] ivBt , int Index)
    {
        //保存key
        string keyDataPath = Path.Combine(Application.persistentDataPath, Index.ToString()+KEYFILENAME);
        File.WriteAllBytes(keyDataPath, keyBt);
        //保存iv
        string ivDataPath = Path.Combine(Application.persistentDataPath, Index.ToString() + IVFILENAME);
        File.WriteAllBytes(ivDataPath, ivBt);
    }
    /// <summary>
    /// 加载当前密钥
    /// </summary>
    /// <returns></returns>
    private static byte[] LoadCurrentKey( int Index)
    {
        //从数据文件中读取
        byte[] keyData = File.ReadAllBytes(Path.Combine(Application.persistentDataPath, Index.ToString() + KEYFILENAME));
        return keyData;
    }
    /// <summary>
    /// 加载当前初始化向量
    /// </summary>
    /// <returns></returns>
    private static byte[] LoadCurrentIV( int Index)
    {
        byte[] ivData = File.ReadAllBytes(Path.Combine(Application.persistentDataPath, Index.ToString() + IVFILENAME));
        return ivData;
    }









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
        SaveDataWrapper DataWrapper = new SaveDataWrapper(data);
        string json = JsonUtility.ToJson(DataWrapper);

        if (DataWrapper.save.RoleList.Count > 0 && DataWrapper.save.RoleList[0].InitialSkills.Count > 0 && DataWrapper.save.RoleList[0].InitialSkills[0] == null) { Debug.LogError("InitialSkills Null!"); }

        using (Aes aes = Aes.Create())
        {
            currentKey = aes.Key;
            currentIV = aes.IV;
            //将当前密钥、初始化向量保存在数据文件中
            SaveCurrentKeyAndIv(currentKey, currentIV, data.SaveIndex);

            byte[] EncryptionData = AesEncryption(json, aes.Key, aes.IV);
            //将数据写入到文件中
            File.WriteAllBytes(path, EncryptionData);
        }
        //File.WriteAllText(path, json);
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
            byte[] readDataBt = File.ReadAllBytes(path);


            //文件数据是否为空
            if (readDataBt.Length <= 0)
            {
                Debug.LogError("无数据！");
                return null;
            }
            currentKey = LoadCurrentKey(SaveIndex);
            currentIV = LoadCurrentIV(SaveIndex);
            string decryptData = AesDecrypt(readDataBt, currentKey, currentIV);
            SaveDataWrapper sw = JsonUtility.FromJson<SaveDataWrapper>(decryptData);
            SaveData s = sw.save;

            //如果角色的初始技能为空，报错
            if (s.RoleList.Count > 0 && s.RoleList[0].InitialSkills.Count> 0 && s.RoleList[0].InitialSkills[0] == null) { Debug.LogError("InitialSkills Null!");  }


 


            //如果更新了新成就，加入到存档中
            if (s.PlayerAchievementList.Count < AchievementList.AllAchievementList.Count)
            {
                for (int i = s.PlayerAchievementList.Count; i < AchievementList.AllAchievementList.Count; i++)
                {
                    s.PlayerAchievementList.Add(new PlayerAchievement(AchievementList.AllAchievementList[i]));
                }
            }
            else //成就结构改变时，刷新
            {
                for (int i = 0; i < AchievementList.AllAchievementList.Count; i++)
                {
                    s.PlayerAchievementList[i].achievement = AchievementList.AllAchievementList[i];
                    Debug.Log(s.PlayerAchievementList[i].achievement.AchiName);
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
            //如果新增了小镇建设项目，加入到存档中
            s.TownDevelopmentProjectsList = TownLoader.AddNewProjectWhenLoadSave(s.TownDevelopmentProjectsList);

            for (int i = 0; i < PlayerControlerList.Length; i++)
            {
                s.RoleList[i].Role = PlayerControlerList[i];
            }
            SaveGame(s);

            //如果角色的初始技能为空，报错
            if (s.RoleList.Count > 0 && s.RoleList[0].InitialSkills.Count > 0 && s.RoleList[0].InitialSkills[0] == null) { Debug.LogError("InitialSkills Null!"); }

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
