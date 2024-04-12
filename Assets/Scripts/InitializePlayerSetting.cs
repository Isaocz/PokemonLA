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
    public int BGIndex;

    public bool isShowAndroidCtrInfo;


    public float SkillButtonXOffset;
    public float SkillButtonYOffset;
    public float SkillButtonScale;
    public int SkillButtonLayout;


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



            if (!PlayerPrefs.HasKey("ShowDamage")) { PlayerPrefs.SetInt("ShowDamage", 1); }
            isShowDamage = intToBool(PlayerPrefs.GetInt("ShowDamage"));

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
}
