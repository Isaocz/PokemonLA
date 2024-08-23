using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIKeyBoard : MonoBehaviour
{

    public GameObject SettingPanel;

    public static UIKeyBoard instance;

    public Text skill1KeyText;
    public Text skill2KeyText;
    public Text skill3KeyText;
    public Text skill4KeyText;
    public Text UseItemKeyText;
    public Text OpenMenuKeyText;
    public Text UpKeyText;
    public Text DownKeyText;
    public Text LeftKeyText;
    public Text RightKeyText;
    public Text MapKeyText;
    public Text ZKeyText;


    private bool isBinding = false;
    private string bindingKeyName;
    private Text text;

    private void Awake()
    {
        instance = this;
        UpdateSkillKeyText(skill1KeyText, "Skill1");
        UpdateSkillKeyText(skill2KeyText, "Skill2");
        UpdateSkillKeyText(skill3KeyText, "Skill3");
        UpdateSkillKeyText(skill4KeyText, "Skill4");
        UpdateSkillKeyText(UseItemKeyText, "UseItem");
        UpdateSkillKeyText(OpenMenuKeyText, "OpenMenu");
        UpdateSkillKeyText(UpKeyText, "Up");
        UpdateSkillKeyText(DownKeyText, "Down");
        UpdateSkillKeyText(LeftKeyText, "Left");
        UpdateSkillKeyText(RightKeyText, "Right");
        UpdateSkillKeyText(MapKeyText, "Map");
        UpdateSkillKeyText(ZKeyText, "Interact");
    }

    private void Start()
    {

    }

    private void Update()
    {
        if (isBinding)
        {
            if (Input.anyKeyDown)
            {
                foreach (KeyCode keyCode in System.Enum.GetValues(typeof(KeyCode)))
                {
                    if (Input.GetKeyDown(keyCode))
                    {
                        bool isRepeat = false;
                        foreach (KeyCode k in InitializePlayerSetting.GlobalPlayerSetting.keybinds.Values)
                        {
                            if (k == keyCode) { isRepeat = true;break; }
                        }
                        if (!isRepeat) {
                            InitializePlayerSetting.GlobalPlayerSetting.ChangeKey(bindingKeyName, keyCode);
                            UpdateSkillKeyText(text, bindingKeyName);
                            isBinding = false;
                            UISkillButton.Instance.isEscEnable = true;
                            break;
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// 启用修改按键
    /// </summary>
    /// <param name="keyName">按键名称</param>
    public void OpenKeybindUI(string keyName)
    {
        isBinding = true;
        bindingKeyName = keyName;
        UISkillButton.Instance.isEscEnable = false;
        switch (keyName)
        {
            case "Skill1":text = skill1KeyText; break;
            case "Skill2":text = skill2KeyText; break;
            case "Skill3":text = skill3KeyText; break;
            case "Skill4":text = skill4KeyText; break;
            case "UseItem":text = UseItemKeyText;break;
            case "OpenMenu":text = OpenMenuKeyText; break;
            case "Up":text = UpKeyText; break;
            case "Down":text = DownKeyText; break;
            case "Left":text = LeftKeyText; break;
            case "Right":text = RightKeyText; break;
            case "Map":text = MapKeyText; break;
            case "Interact": text = ZKeyText; break;
        }
    }
    //更新按钮的文本
    private void UpdateSkillKeyText(Text KeyText, string keyname)
    {
        KeyText.text = InitializePlayerSetting.GlobalPlayerSetting.keybinds[keyname].ToString(); // 根据名称更新对应按键的文本
    }



    public void Skill1KeyBoard()
    {
        OpenKeybindUI("Skill1");
    }
    public void Skill2KeyBoard()
    {
        OpenKeybindUI("Skill2");
    }
    public void Skill3KeyBoard()
    {
        OpenKeybindUI("Skill3");
    }
    public void Skill4KeyBoard()
    {
        OpenKeybindUI("Skill4");
    }
    public void UseItemKeyBoard()
    {
        OpenKeybindUI("UseItem");
    }
    public void OpenMenuKeyBoard()
    {
        OpenKeybindUI("OpenMenu");
    }
    public void UpKeyBoard()
    {
        OpenKeybindUI("Up");
    }
    public void DownKeyBoard()
    {
        OpenKeybindUI("Down");
    }
    public void LeftKeyBoard()
    {
        OpenKeybindUI("Left");
    }
    public void RightKeyBoard()
    {
        OpenKeybindUI("Right");
    }

    public void MapKeyBoard()
    {
        OpenKeybindUI("Map");
    }

    public void InteractKeyBoard()
    {
        OpenKeybindUI("Interact");
    }
}
