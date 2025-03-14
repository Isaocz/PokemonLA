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
    /// 获得奋斗力时是否显示
    /// </summary>
    public bool isShowHardworking;
    /// <summary>
    /// 是否显示能力加成 0不显示 1显示于角色上方 2显示于状态栏下方
    /// </summary>
    public int isShowBouns;

    public float SkillButtonXOffset;
    public float SkillButtonYOffset;
    public float SkillButtonScale;
    public int SkillButtonLayout;

    public bool isJoystickFixed;

    /// <summary>
    /// 手机玩家的操作方式 0：十字键 1：自由摇杆 2：固定摇杆3：结合模式
    /// </summary>
    public int ControlTypr;
    /// <summary>
    /// 摇杆的大小
    /// </summary>
    public float JoystickScale;
    /// <summary>
    /// 固定摇杆的X轴偏移量
    /// </summary>
    public float JoystickXOffset;
    /// <summary>
    /// 固定摇杆的Y轴偏移量
    /// </summary>
    public float JoystickYOffset;
    /// <summary>
    /// 十字键的大小
    /// </summary>
    public float ArrowScale;
    /// <summary>
    /// 十字键的X轴偏移量
    /// </summary>
    public float ArrowXOffset;
    /// <summary>
    /// 十字键的Y轴偏移量
    /// </summary>
    public float ArrowYOffset;
    /// <summary>
    /// 十字键的间距
    /// </summary>
    public float ArrowSpacing;

    public int RoundSeed;
    public string SeedString;

    //测试模式，开启后种子固定为42
    public bool TestMode;

    string[][] SeedStringWord = new string[][] { 
        new string[] { "怕寂寞", "固执", "顽皮", "大胆", "淘气", "乐天", "内敛", "慢吞吞", "马虎", "冷静", "温和", "慎重", "胆小", "爽朗", "认真", "浮躁" },
        new string[] { "伊布" ,"小火龙" ,"杰尼龟","妙蛙种子" ,"皮卡丘","小山猪","利欧路", "圆陆鲨", "呆呆兽", "巨钳螳螂", "岩狗狗", "烛光灵", "索罗亚", "迷你龙", "帝王拿波", "胖可丁" },
        new string[] { "呼呼森林" ,"隆隆山洞" ,"萧萧雪山", "森之洋馆", "废弃发电厂", "大鍋湖", "玉虹遊戲城", "沙漠遗迹", "送神山", "密阿雷美术馆", "冠軍之路", "神闔寺院遺址", "铃铛塔", "呆呆兽之井", "烘烘沙漠", "迷光森林" },
        new string[] { "连环巴掌", "十万伏特", "大字爆炎", "冲浪", "飞叶快刀", "嬉闹", "铁尾", "祸不单行", "虫咬", "恶意追击", "绝对零度", "污泥炸弹", "地震", "岩石封", "逆鳞", "扑击" },
        new string[] { "击败", "魅惑", "逗笑", "威慑", "说服", "欺骗", "恐吓", "激怒", "吓唬", "挑衅", "哄骗", "感动", "鼓励", "感化", "启迪", "打伤" },
        new string[] { "耿鬼", "卡比兽", "烈箭鹰", "超坏星", "电飞鼠", "坚果哑铃", "雪妖女", "艾路雷朵", "焰后蜥", "暴飞龙", "波克基斯", "天蝎王", "坚盾剑怪", "玛力露丽", "大岩蛇", "红莲铠骑" },
        new string[] { "朽木妖", "土王", "列阵兵", "锹农炮虫", "花疗环环", "姆克鹰", "铳嘴大鸟", "乌鸦头头", "奇麒麟", "黏美龙", "三首恶龙", "布拨", "浩大鲸", "海豚侠", "超甲狂犀", "电肚蛙" },
        new string[] { "学习装置", "晶晶蜜", "月亮球", "凰梨果", "紫之书", "肌力之羽", "觉醒之石", "泥炭块", "特性膏药", "心之鳞片", "精通种子", "护符金币", "吃剩的东西", "安抚之铃", "达人带", "凸凸头盔" }
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

    //每次开局时重新放置种子
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


            //显示伤害
            if (!PlayerPrefs.HasKey("ShowDamage")) { PlayerPrefs.SetInt("ShowDamage", 1); }
            isShowDamage = intToBool(PlayerPrefs.GetInt("ShowDamage"));

            if (!PlayerPrefs.HasKey("JoystickFixed")) { PlayerPrefs.SetInt("JoystickFixed", 0); }
            isJoystickFixed = intToBool(PlayerPrefs.GetInt("JoystickFixed"));


            //显示努力值
            if (!PlayerPrefs.HasKey("ShowHardworking")) { PlayerPrefs.SetInt("ShowHardworking", 1); }
            isShowHardworking = intToBool(PlayerPrefs.GetInt("ShowHardworking"));

            //显示能力加成
            if (!PlayerPrefs.HasKey("ShowBouns")) { PlayerPrefs.SetInt("ShowBouns", 2); }
            isShowBouns = PlayerPrefs.GetInt("ShowBouns");

            //高亮显示
            if (!PlayerPrefs.HasKey("Highlight")) { PlayerPrefs.SetInt("Highlight", 0); }
            isHighlight = intToBool(PlayerPrefs.GetInt("Highlight"));

            //摇杆与十字键
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

        // 检查并处理补码表示
        if (longValue > 0x7FFFFFFF)
        {
            longValue -= 0x100000000;
        }
        int intValue = (int)longValue;
        Debug.Log(intValue);

        SeedString = "性格" + Words[0] + "的" + Words[1] + "在" + Words[2] + "使用" + Words[3] + ",\n" + Words[4] + "了" + Words[5] + "和" + Words[6] + ",\n" + "获得了它们的" + Words[7];
        Debug.Log(SeedString);
    }


}
