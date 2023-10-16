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

    private Dictionary<string, KeyCode> keybinds = new Dictionary<string, KeyCode>();

    private bool isBinding = false;
    private string bindingKeyName;
    private Text text;

    private void Awake()
    {
        instance = this;
        ResetRegister();
    }

    private void Start()
    {
        SettingPanel.SetActive(false);
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
                        keybinds[bindingKeyName] = keyCode;
                        UpdateSkillKeyText(text, bindingKeyName);
                        isBinding = false;
                        UISkillButton.Instance.isEscEnable = true;
                        break;
                    }
                }
            }
        }
    }

    /// <summary>
    /// �����޸İ���
    /// </summary>
    /// <param name="keyName">��������</param>
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
        }
    }
    //���°�ť���ı�
    private void UpdateSkillKeyText(Text KeyText, string keyname)
    {
        KeyText.text = keybinds[keyname].ToString(); // �������Ƹ��¶�Ӧ�������ı�
    }
    /// <summary>
    /// ������д��Dirctionary��
    /// </summary>
    /// <param name="keyName"></param>
    /// <param name="defaultKeyCode"></param>
    public void RegisterKeybind(string keyName, KeyCode defaultKeyCode)
    {
        if (!keybinds.ContainsKey(keyName))
        {
            keybinds.Add(keyName, defaultKeyCode);
        }
    }
    /// <summary>
    /// ��ȡע��ļ�λ
    /// </summary>
    /// <param name="keyName">ע��ļ�λ��</param>
    /// <returns>���ؼ�λ</returns>
    public static KeyCode GetKeybind(string keyName)
    {
        if (instance != null && instance.keybinds.ContainsKey(keyName))
        {
            return instance.keybinds[keyName];
        }
        return KeyCode.None;
    }

    private void ResetRegister()
    {
        RegisterKeybind("Skill1", KeyCode.Q); // ע��Ĭ�ϵļ��ܰ���
        RegisterKeybind("Skill2", KeyCode.W);
        RegisterKeybind("Skill3", KeyCode.E);
        RegisterKeybind("Skill4", KeyCode.R);
        RegisterKeybind("UseItem", KeyCode.Space);
        RegisterKeybind("OpenMenu", KeyCode.Escape);
        RegisterKeybind("Up", KeyCode.UpArrow);
        RegisterKeybind("Down", KeyCode.DownArrow);
        RegisterKeybind("Left", KeyCode.LeftArrow);
        RegisterKeybind("Right", KeyCode.RightArrow);
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
}
