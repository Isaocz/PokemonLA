using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitializePlayerSetting : MonoBehaviour
{
    bool isInitializition;

    public static InitializePlayerSetting GlobalPlayerSetting;

    public float BGMVolumeValue;
    public float SEVolumeValue;
    public bool isShowDamage;
    public bool isHighlight;
    public int BGIndex;

    public bool isShowAndroidCtrInfo;

    /// <summary>
    /// ��÷ܶ���ʱ�Ƿ���ʾ
    /// </summary>
    public bool isShowHardworking;
    /// <summary>
    /// �Ƿ���ʾ�����ӳ� 0����ʾ 1��ʾ�ڽ�ɫ�Ϸ� 2��ʾ��״̬���·�
    /// </summary>
    public int isShowBouns;

    public float SkillButtonXOffset;
    public float SkillButtonYOffset;
    public float SkillButtonScale;
    public int SkillButtonLayout;

    public bool isJoystickFixed;

    /// <summary>
    /// �ֻ���ҵĲ�����ʽ 0��ʮ�ּ� 1������ҡ�� 2���̶�ҡ��3�����ģʽ
    /// </summary>
    public int ControlTypr;
    /// <summary>
    /// ҡ�˵Ĵ�С
    /// </summary>
    public float JoystickScale;
    /// <summary>
    /// �̶�ҡ�˵�X��ƫ����
    /// </summary>
    public float JoystickXOffset;
    /// <summary>
    /// �̶�ҡ�˵�Y��ƫ����
    /// </summary>
    public float JoystickYOffset;
    /// <summary>
    /// ʮ�ּ��Ĵ�С
    /// </summary>
    public float ArrowScale;
    /// <summary>
    /// ʮ�ּ���X��ƫ����
    /// </summary>
    public float ArrowXOffset;
    /// <summary>
    /// ʮ�ּ���Y��ƫ����
    /// </summary>
    public float ArrowYOffset;
    /// <summary>
    /// ʮ�ּ��ļ��
    /// </summary>
    public float ArrowSpacing;

    public int RoundSeed;
    public string SeedString;

    //����ģʽ�����������ӹ̶�Ϊ42
    public bool TestMode;

    string[][] SeedStringWord = new string[][] { 
        new string[] { "�¼�į", "��ִ", "��Ƥ", "��", "����", "����", "����", "������", "��", "�侲", "�º�", "����", "��С", "ˬ��", "����", "����" },
        new string[] { "����" ,"С����" ,"�����","��������" ,"Ƥ����","Сɽ��","��ŷ·", "Բ½��", "������", "��ǯ���", "�ҹ���", "�����", "������", "������", "�����ò�", "�ֿɶ�" },
        new string[] { "����ɭ��" ,"¡¡ɽ��" ,"����ѩɽ", "ɭ֮���", "�������糧", "��偺�", "����[���", "ɳĮ�ż�", "����ɽ", "�ܰ���������", "��܊֮·", "���H��Ժ�zַ", "������", "������֮��", "���ɳĮ", "�Թ�ɭ��" },
        new string[] { "��������", "ʮ�����", "���ֱ���", "����", "��Ҷ�쵶", "����", "��β", "��������", "��ҧ", "����׷��", "�������", "����ը��", "����", "��ʯ��", "����", "�˻�" },
        new string[] { "����", "�Ȼ�", "��Ц", "����", "˵��", "��ƭ", "����", "��ŭ", "�Ż�", "����", "��ƭ", "�ж�", "����", "�л�", "����", "����" },
        new string[] { "����", "������", "�Ҽ�ӥ", "������", "�����", "�������", "ѩ��Ů", "��·�׶�", "�����", "������", "���˻�˹", "��Ы��", "��ܽ���", "����¶��", "������", "��������" },
        new string[] { "��ľ��", "����", "�����", "��ũ�ڳ�", "���ƻ���", "ķ��ӥ", "������", "��ѻͷͷ", "������", "�����", "���׶���", "����", "�ƴ�", "������", "���׿�Ϭ", "�����" },
        new string[] { "ѧϰװ��", "������", "������", "�����", "��֮��", "����֮��", "����֮ʯ", "��̿��", "���Ը�ҩ", "��֮��Ƭ", "��ͨ����", "�������", "��ʣ�Ķ���", "����֮��", "���˴�", "͹͹ͷ��" }
    };




    public Dictionary<string, KeyCode> keybinds = new Dictionary<string, KeyCode>() {
            
        {"Skill1", KeyCode.Q},
        {"Skill2", KeyCode.W},
        {"Skill3", KeyCode.E},
        {"Skill4", KeyCode.R},
        {"UseItem", KeyCode.Space},      
        {"OpenMenu", KeyCode.Escape},
        {"Up", KeyCode.UpArrow},
        {"Down", KeyCode.DownArrow},
        {"Left", KeyCode.LeftArrow},
        {"Right", KeyCode.RightArrow},
        {"Map", KeyCode.Tab},
        {"Interact", KeyCode.Z},

    };

    private void Awake()
    {
        GlobalPlayerSetting = this;
        ResetSeed();
    }

    //ÿ�ο���ʱ���·�������
    public void ResetSeed()
    {
        Random.InitState((int)System.DateTime.Now.Ticks);
        if (TestMode) {
            RoundSeed = 42;
        }
        else
        {
            RoundSeed = Random.Range(int.MinValue, int.MaxValue);
        }


        DoSeed();
    }

    public void DoSeed()
    {
        Random.InitState(RoundSeed);
        SetStringBySeedint();
    }


    public void ChangeKey(string KeyName , KeyCode k )
    {
        InitializePlayerSetting.GlobalPlayerSetting.keybinds[KeyName] = k;
        switch (KeyName)
        {
            case "Skill1": PlayerPrefs.SetInt("Skill01Key", (int)k);break;
            case "Skill2": PlayerPrefs.SetInt("Skill02Key", (int)k); break;
            case "Skill3": PlayerPrefs.SetInt("Skill03Key", (int)k); break;
            case "Skill4": PlayerPrefs.SetInt("Skill04Key", (int)k); break;
            case "UseItem": PlayerPrefs.SetInt("UseItemKey", (int)k); break;
            case "OpenMenu": PlayerPrefs.SetInt("OpenMenuKey", (int)k); break;
            case "Up": PlayerPrefs.SetInt("UpKey", (int)k); break;
            case "Down": PlayerPrefs.SetInt("DownKey", (int)k); break;
            case "Left": PlayerPrefs.SetInt("LeftKey", (int)k); break;
            case "Right": PlayerPrefs.SetInt("RightKey", (int)k); break;
            case "Map": PlayerPrefs.SetInt("MapKey", (int)k); break;
            case "Interact": PlayerPrefs.SetInt("InteractKey", (int)k); break;
        }
    }

    public KeyCode GetKeybind(string keyName)
    {
        //Debug.Log(keyName);
        return keybinds[keyName];
    }

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);
        if (SystemInfo.operatingSystemFamily == OperatingSystemFamily.Other)
        {
            PlayerPrefs.SetInt("Skill01Key", 113);
            PlayerPrefs.SetInt("Skill02Key", 119);
            PlayerPrefs.SetInt("Skill03Key", 101);
            PlayerPrefs.SetInt("Skill04Key", 114);
            PlayerPrefs.SetInt("UseItemKey", 32);
            PlayerPrefs.SetInt("OpenMenuKey", 27);
            PlayerPrefs.SetInt("UpKey", 273);
            PlayerPrefs.SetInt("DownKey", 274); 
            PlayerPrefs.SetInt("LeftKey", 276);
            PlayerPrefs.SetInt("RightKey", 275);
            PlayerPrefs.SetInt("MapKey", 9);
            PlayerPrefs.SetInt("InteractKey", 122);
        }
        if (!isInitializition) {

            isInitializition = true;

            if (!PlayerPrefs.HasKey("BGMVolume")) { PlayerPrefs.SetFloat("BGMVolume" , 1.0f); }
            BGMVolumeValue = PlayerPrefs.GetFloat("BGMVolume");

            isInitializition = true;
            if (!PlayerPrefs.HasKey("SEVolume")) { PlayerPrefs.SetFloat("SEVolume", 1.0f); }
            SEVolumeValue = PlayerPrefs.GetFloat("SEVolume");


            //��ʾ�˺�
            if (!PlayerPrefs.HasKey("ShowDamage")) { PlayerPrefs.SetInt("ShowDamage", 1); }
            isShowDamage = intToBool(PlayerPrefs.GetInt("ShowDamage"));

            if (!PlayerPrefs.HasKey("JoystickFixed")) { PlayerPrefs.SetInt("JoystickFixed", 0); }
            isJoystickFixed = intToBool(PlayerPrefs.GetInt("JoystickFixed"));


            //��ʾŬ��ֵ
            if (!PlayerPrefs.HasKey("ShowHardworking")) { PlayerPrefs.SetInt("ShowHardworking", 1); }
            isShowHardworking = intToBool(PlayerPrefs.GetInt("ShowHardworking"));

            //��ʾ�����ӳ�
            if (!PlayerPrefs.HasKey("ShowBouns")) { PlayerPrefs.SetInt("ShowBouns", 2); }
            isShowBouns = PlayerPrefs.GetInt("ShowBouns");

            //������ʾ
            if (!PlayerPrefs.HasKey("Highlight")) { PlayerPrefs.SetInt("Highlight", 0); }
            isHighlight = intToBool(PlayerPrefs.GetInt("Highlight"));

            //ҡ����ʮ�ּ�
            {
                if (!PlayerPrefs.HasKey("ControlTypr")) { PlayerPrefs.SetInt("ControlTypr", 0); }
                ControlTypr = PlayerPrefs.GetInt("ControlTypr");

                if (!PlayerPrefs.HasKey("JoystickXOffset")) { PlayerPrefs.SetFloat("JoystickXOffset", 0.7f); }
                JoystickXOffset = PlayerPrefs.GetFloat("JoystickXOffset");

                if (!PlayerPrefs.HasKey("JoystickYOffset")) { PlayerPrefs.SetFloat("JoystickYOffset", 0.0f); }
                JoystickYOffset = PlayerPrefs.GetFloat("JoystickYOffset");

                if (!PlayerPrefs.HasKey("JoystickScale")) { PlayerPrefs.SetFloat("JoystickScale", 0.5f); }
                JoystickScale = PlayerPrefs.GetFloat("JoystickScale");

                if (!PlayerPrefs.HasKey("ArrowXOffset")) { PlayerPrefs.SetFloat("ArrowXOffset", 0.6f); }
                ArrowXOffset = PlayerPrefs.GetFloat("ArrowXOffset");

                if (!PlayerPrefs.HasKey("ArrowYOffset")) { PlayerPrefs.SetFloat("ArrowYOffset", 0.5f); }
                ArrowYOffset = PlayerPrefs.GetFloat("ArrowYOffset");

                if (!PlayerPrefs.HasKey("ArrowScale")) { PlayerPrefs.SetFloat("ArrowScale", 0.0f); }
                ArrowScale = PlayerPrefs.GetFloat("ArrowScale");

                if (!PlayerPrefs.HasKey("ArrowSpacing")) { PlayerPrefs.SetFloat("ArrowSpacing", 0.0f); }
                ArrowSpacing = PlayerPrefs.GetFloat("ArrowSpacing");
            }


            if (!PlayerPrefs.HasKey("BackGroundIndex")) { PlayerPrefs.SetInt("BackGroundIndex", 0); }
            BGIndex = PlayerPrefs.GetInt("BackGroundIndex");

            if (!PlayerPrefs.HasKey("ShowAndroidCtrInfo")) { PlayerPrefs.SetInt("ShowAndroidCtrInfo", 0); }
            isShowAndroidCtrInfo = intToBool(PlayerPrefs.GetInt("ShowAndroidCtrInfo"));



            if (!PlayerPrefs.HasKey("SkillButtonXOffset")) { PlayerPrefs.SetFloat("SkillButtonXOffset", 0.0f); }
            SkillButtonXOffset = PlayerPrefs.GetFloat("SkillButtonXOffset");

            if (!PlayerPrefs.HasKey("SkillButtonYOffset")) { PlayerPrefs.SetFloat("SkillButtonYOffset", 0.0f); }
            SkillButtonYOffset = PlayerPrefs.GetFloat("SkillButtonYOffset");

            if (!PlayerPrefs.HasKey("SkillButtonScale")) { PlayerPrefs.SetFloat("SkillButtonScale", 0.0f); }
            SkillButtonScale = PlayerPrefs.GetFloat("SkillButtonScale");

            if (!PlayerPrefs.HasKey("SkillButtonLayout")) { PlayerPrefs.SetInt("SkillButtonLayout", 0); }
            SkillButtonLayout = PlayerPrefs.GetInt("SkillButtonLayout");



            if (!PlayerPrefs.HasKey("Skill01Key")) { PlayerPrefs.SetInt("Skill01Key", 113); }
            keybinds["Skill1"] = (KeyCode)PlayerPrefs.GetInt("Skill01Key");

            if (!PlayerPrefs.HasKey("Skill02Key")) { PlayerPrefs.SetInt("Skill02Key", 119); }
            keybinds["Skill2"] = (KeyCode)PlayerPrefs.GetInt("Skill02Key");

            if (!PlayerPrefs.HasKey("Skill03Key")) { PlayerPrefs.SetInt("Skill03Key", 101); }
            keybinds["Skill3"] = (KeyCode)PlayerPrefs.GetInt("Skill03Key");

            if (!PlayerPrefs.HasKey("Skill04Key")) { PlayerPrefs.SetInt("Skill04Key", 114); }
            keybinds["Skill4"] = (KeyCode)PlayerPrefs.GetInt("Skill04Key");



            if (!PlayerPrefs.HasKey("UseItemKey")) { PlayerPrefs.SetInt("UseItemKey", 32); }
            keybinds["UseItem"] = (KeyCode)PlayerPrefs.GetInt("UseItemKey");

            if (!PlayerPrefs.HasKey("OpenMenuKey")) { PlayerPrefs.SetInt("OpenMenuKey", 27); }
            keybinds["OpenMenu"] = (KeyCode)PlayerPrefs.GetInt("OpenMenuKey");



            if (!PlayerPrefs.HasKey("UpKey")) { PlayerPrefs.SetInt("UpKey", 273); }
            keybinds["Up"] = (KeyCode)PlayerPrefs.GetInt("UpKey");

            if (!PlayerPrefs.HasKey("DownKey")) { PlayerPrefs.SetInt("DownKey", 274); }
            keybinds["Down"] = (KeyCode)PlayerPrefs.GetInt("DownKey");

            if (!PlayerPrefs.HasKey("LeftKey")) { PlayerPrefs.SetInt("LeftKey", 276); }
            keybinds["Left"] = (KeyCode)PlayerPrefs.GetInt("LeftKey");

            if (!PlayerPrefs.HasKey("RightKey")) { PlayerPrefs.SetInt("RightKey", 275); }
            keybinds["Right"] = (KeyCode)PlayerPrefs.GetInt("RightKey");

            if (!PlayerPrefs.HasKey("MapKey")) { PlayerPrefs.SetInt("MapKey", 9); }
            keybinds["Map"] = (KeyCode)PlayerPrefs.GetInt("MapKey");

            if (!PlayerPrefs.HasKey("InteractKey")) { PlayerPrefs.SetInt("InteractKey", 122); }
            keybinds["Interact"] = (KeyCode)PlayerPrefs.GetInt("InteractKey");
        }

    }
    int boolToInt(bool val)
    {
        return val ? 1 : 0;
    }

    bool intToBool(int val)
    {
        return val != 0;
    }


    void SetStringBySeedint()
    {
        SeedString = System.String.Format("{0:X}", RoundSeed);
        Debug.Log(SeedString);
        char[] Xw = SeedString.ToCharArray();
        string[] Words = new string[8] { "", "", "", "", "", "", "", ""};
        int[] IntSting = new int[8] { 0,0,0,0,0,0,0,0 };
        for (int i = Xw.Length - 1; i >= 0; i--)
        {
            IntSting[i] = int.Parse(Xw[i].ToString(), System.Globalization.NumberStyles.HexNumber);
        }
        for (int i = 7; i >= 0; i--)
        {
            Words[i] = SeedStringWord[i][IntSting[i]];
        }




        long longValue = System.Convert.ToInt32(SeedString, 16);

        // ��鲢�������ʾ
        if (longValue > 0x7FFFFFFF)
        {
            longValue -= 0x100000000;
        }
        int intValue = (int)longValue;
        Debug.Log(intValue);

        SeedString = "�Ը�" + Words[0] + "��" + Words[1] + "��" + Words[2] + "ʹ��" + Words[3] + ",\n" + Words[4] + "��" + Words[5] + "��" + Words[6] + ",\n" + "��������ǵ�" + Words[7];
        Debug.Log(SeedString);
    }


}
