using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using UnityEngine;


/// <summary>
/// ʹ�����װList
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
    /// <summary>
    /// ��ǰ�浵��С�������
    /// </summary>
    public List<TownDevelopmentProject> TownDevelopmentProjectsList;
    /// <summary>
    /// С����NPC�ĶԻ����
    /// </summary>
    public NPCDialogState TownNPCDialogState;


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
    /// ���ɾ͵Ľ������
    /// </summary>
    public static void CheckforAchUnlock()
    {
        if (SaveLoader.saveLoader != null)
        {
            foreach (PlayerAchievement ach in SaveLoader.saveLoader.saveData.PlayerAchievementList)
            {
                //��������ѽ�������Ŀ 
                if (ach.State == AchievementList.AchStatus.Locked)
                {
                    bool canUnlockAchi = true;
                    bool canUnlockTDP = true;
                    if (ach.achievement.LockedList.Count != 0)
                    {
                        //�����Щ�����ǰ�������Ƿ��Ѿ��������״̬��
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
                        //�����Щ�����ǰ�������Ƿ��Ѿ��������״̬��
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
    /// ĳ��ɾ͵Ľ�������
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
    /// �ɾ͵�״̬
    /// </summary>
    public AchievementList.AchStatus State = AchievementList.AchStatus.Locked;
    /// <summary>
    /// �ɾ͵�����
    /// </summary>
    public AchievementList.Achievement achievement;
    /// <summary>
    /// �ɾ͵ĵ�ǰ����
    /// </summary>
    public int Progress;
    /// <summary>
    /// ���ȵ����Ŀ�꣬����ʱ�����ɾ�
    /// </summary>
    public int Target;
    /// <summary>
    /// ��¼�ɾ͵�ĳ�������Ѿ�������ˣ���ɾ�4��θ�������Թ����չ�����CompletePartList�ּ�¼0 �� �´�ʹ�����չ�ʱ�����CompletePartList����0����������
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
    /// ���ӳɾ͵Ľ���;
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
    /// ��ǰ��������Կ
    /// </summary>
    private static byte[] currentKey;
    /// <summary>
    /// ��ǰ�����ĳ�ʼ������
    /// </summary>
    private static byte[] currentIV;
    /// <summary>
    /// ������Կ���ļ���
    /// </summary>
    private const string KEYFILENAME = "KeyData.txt";
    /// <summary>
    /// �����ʼ���������ļ���
    /// </summary>
    private const string IVFILENAME = "IVData.txt";
    /// <summary>
    /// ʹ�öԳ��㷨����(Aes�㷨)
    /// </summary>
    /// <param name="targetStr">Ŀ������</param>
    /// <param name="key">�Գ��㷨����Կ</param>
    /// <param name="iv">�Գ��㷨�ĳ�ʼ������</param>
    /// <returns></returns>
    private static byte[] AesEncryption(string targetStr, byte[] key, byte[] iv)
    {
        #region ������
        //�ж������Ƿ�Ϊ��
        if (targetStr == null || targetStr.Length <= 0)
        {
            Debug.LogError("Ŀ������Ϊ�գ�");
            return null;
        }
        //�ж���Կ���ʼ������
        if (key == null || key.Length <= 0)
        {
            Debug.LogError("������ԿΪ�գ�");
            return null;
        }
        if (iv == null || iv.Length <= 0)
        {
            Debug.LogError("�����ʼ������Ϊ�գ�");
            return null;
        }
        #endregion

        #region ����
        byte[] encryptionData;          //�����ݼ��ܺ�õ����ֽ�����

        using (Aes aes = Aes.Create())  //�½���Կ
        {
            aes.Key = key;
            aes.IV = iv;
            //�������ܳ�����ִ����ת��
            ICryptoTransform cryptoTF = aes.CreateEncryptor(aes.Key, aes.IV);
            //�������ڼ��ܵ���
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoTF, CryptoStreamMode.Write))
                {
                    using (StreamWriter streamWriter = new StreamWriter(cryptoStream))
                    {
                        streamWriter.Write(targetStr);  //������д�뵽����
                    }
                    encryptionData = memoryStream.ToArray();
                }
            }
            return encryptionData;
        }
        #endregion
    }
    /// <summary>
    /// �Գ��㷨����
    /// </summary>
    /// <param name="targetBtData">��Ҫ���ܵ�����</param>
    /// <param name="key">��Կ</param>
    /// <param name="iv">��ʼ������</param>
    /// <returns></returns>
    private static string AesDecrypt(byte[] targetBtData, byte[] key, byte[] iv)
    {
        #region ������
        if (targetBtData == null || targetBtData.Length <= 0)
        {
            Debug.LogError("��������Ϊ�գ�");
            return null;
        }
        if (key == null || key.Length <= 0)
        {
            Debug.LogError("������ԿΪ�գ�");
            return null;
        }
        if (iv == null || iv.Length <= 0)
        {
            Debug.LogError("�����ʼ������Ϊ�գ�");
            return null;
        }
        #endregion

        #region ����
        string decryptStr;

        using (Aes aes = Aes.Create())  //ʵ�����㷨��
        {
            aes.Key = key;
            aes.IV = iv;
            //�������ܳ�����ִ����ת��
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
    /// ���浱ǰ��Կ�Լ���ʼ������
    /// </summary>
    /// <param name="keyBt">��Կ</param>
    /// <param name="ivBt">��ʼ������</param>
    private static void SaveCurrentKeyAndIv(byte[] keyBt, byte[] ivBt , int Index)
    {
        //����key
        string keyDataPath = Path.Combine(Application.persistentDataPath, Index.ToString()+KEYFILENAME);
        File.WriteAllBytes(keyDataPath, keyBt);
        //����iv
        string ivDataPath = Path.Combine(Application.persistentDataPath, Index.ToString() + IVFILENAME);
        File.WriteAllBytes(ivDataPath, ivBt);
    }
    /// <summary>
    /// ���ص�ǰ��Կ
    /// </summary>
    /// <returns></returns>
    private static byte[] LoadCurrentKey( int Index)
    {
        //�������ļ��ж�ȡ
        byte[] keyData = File.ReadAllBytes(Path.Combine(Application.persistentDataPath, Index.ToString() + KEYFILENAME));
        return keyData;
    }
    /// <summary>
    /// ���ص�ǰ��ʼ������
    /// </summary>
    /// <returns></returns>
    private static byte[] LoadCurrentIV( int Index)
    {
        byte[] ivData = File.ReadAllBytes(Path.Combine(Application.persistentDataPath, Index.ToString() + IVFILENAME));
        return ivData;
    }









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
        SaveDataWrapper DataWrapper = new SaveDataWrapper(data);
        string json = JsonUtility.ToJson(DataWrapper);

        if (DataWrapper.save.RoleList.Count > 0 && DataWrapper.save.RoleList[0].InitialSkills.Count > 0 && DataWrapper.save.RoleList[0].InitialSkills[0] == null) { Debug.LogError("InitialSkills Null!"); }

        using (Aes aes = Aes.Create())
        {
            currentKey = aes.Key;
            currentIV = aes.IV;
            //����ǰ��Կ����ʼ�����������������ļ���
            SaveCurrentKeyAndIv(currentKey, currentIV, data.SaveIndex);

            byte[] EncryptionData = AesEncryption(json, aes.Key, aes.IV);
            //������д�뵽�ļ���
            File.WriteAllBytes(path, EncryptionData);
        }
        //File.WriteAllText(path, json);
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
            byte[] readDataBt = File.ReadAllBytes(path);


            //�ļ������Ƿ�Ϊ��
            if (readDataBt.Length <= 0)
            {
                Debug.LogError("�����ݣ�");
                return null;
            }
            currentKey = LoadCurrentKey(SaveIndex);
            currentIV = LoadCurrentIV(SaveIndex);
            string decryptData = AesDecrypt(readDataBt, currentKey, currentIV);
            SaveDataWrapper sw = JsonUtility.FromJson<SaveDataWrapper>(decryptData);
            SaveData s = sw.save;

            //�����ɫ�ĳ�ʼ����Ϊ�գ�����
            if (s.RoleList.Count > 0 && s.RoleList[0].InitialSkills.Count> 0 && s.RoleList[0].InitialSkills[0] == null) { Debug.LogError("InitialSkills Null!");  }


 


            //����������³ɾͣ����뵽�浵��
            if (s.PlayerAchievementList.Count < AchievementList.AllAchievementList.Count)
            {
                for (int i = s.PlayerAchievementList.Count; i < AchievementList.AllAchievementList.Count; i++)
                {
                    s.PlayerAchievementList.Add(new PlayerAchievement(AchievementList.AllAchievementList[i]));
                }
            }
            else //�ɾͽṹ�ı�ʱ��ˢ��
            {
                for (int i = 0; i < AchievementList.AllAchievementList.Count; i++)
                {
                    s.PlayerAchievementList[i].achievement = AchievementList.AllAchievementList[i];
                    Debug.Log(s.PlayerAchievementList[i].achievement.AchiName);
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
            //���������С������Ŀ�����뵽�浵��
            s.TownDevelopmentProjectsList = TownLoader.AddNewProjectWhenLoadSave(s.TownDevelopmentProjectsList);

            for (int i = 0; i < PlayerControlerList.Length; i++)
            {
                s.RoleList[i].Role = PlayerControlerList[i];
            }
            SaveGame(s);

            //�����ɫ�ĳ�ʼ����Ϊ�գ�����
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
