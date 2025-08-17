using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DebugController : MonoBehaviour
{
    bool showConsole;

    string input;
    List<string> resultMessage = new List<string>();

    public static DebugCommand KILL_ALL;
    public static DebugCommand<bool> HIGHLIGHT;
    public static DebugCommand<bool> INVINCIBLE;
    public static DebugCommand<int> CHANGEHP;
    public static DebugCommand<int> EXP;
    public static DebugCommand HELP;
    public static DebugCommand LIGHTUP;
    public static DebugCommand CLEAR;
    public static DebugCommand<int> FONT_SIZE;
    public static DebugCommand<ItemType, int> GIVE;

    public List<object> commandList;

    private int fontSize = 20;
    private GUIStyle consoleStyle;

    public enum ItemType
    {
        Stone,
        Coin,
        HearsScale,
        PPUp,
        SeedofMastery
    }


    public void OnToggleDebug()
    {
        showConsole = !showConsole;
    }

    public void OnReturn()
    {
        if (showConsole)
        {
            HandleInput();
            input = "";
        }
    }

    public void Awake()
    {
        consoleStyle = new GUIStyle();
        consoleStyle.fontSize = fontSize;
        consoleStyle.normal.textColor = Color.white;

        KILL_ALL = new DebugCommand("kill_all", "���𳡾������ей֡�", "kill_all", () =>
        {
            PlayerControler player = FindObjectOfType<PlayerControler>();
            GameObject EmptyParent = MapCreater.StaticMap.RRoom[player.NowRoom].transform.GetChild(3).gameObject;
            for (int i = 0; i < EmptyParent.transform.childCount; i++)
            {
                Empty e = EmptyParent.transform.GetChild(i).GetComponent<Empty>();
                if (e != null) { Pokemon.PokemonHpChange(null, e.gameObject, e.maxHP, 0, 0, PokemonType.TypeEnum.IgnoreType); }
            }

            AddResultMessage("�Ѿ������˳��������ежԱ�����");
        });

        GIVE = new DebugCommand<ItemType, int>("give", "���������Ϸ��Ʒ", "give <item> <amount>", (item, amount) =>
        {
            PlayerControler player = FindObjectOfType<PlayerControler>();
            switch (item)
            {
                case ItemType.Stone: player.ChangeStone(amount); break;
                case ItemType.Coin: player.ChangeMoney(amount); break;
                case ItemType.HearsScale: player.ChangeHearsScale(amount); break;
                case ItemType.PPUp: player.ChangePPUp(amount); break;
                case ItemType.SeedofMastery: player.ChangeMSeed(amount); break;
            }

            AddResultMessage($"�Ѹ������{amount}��{item}");
        });

        HIGHLIGHT = new DebugCommand<bool>("highlight", "���������ʾ", "highlight <bool>", (judge) =>
        {
            PlayerPrefs.SetInt("Highlight", judge ? 1 : 0);
            InitializePlayerSetting.GlobalPlayerSetting.isHighlight = judge;

            AddResultMessage($"�ѽ�������ʾ����Ϊ{judge}");
        });

        LIGHTUP = new DebugCommand("lightup", "����С��ͼ�����е�ͼ��", "lightup", () =>
        {
            UiMiniMap.Instance.LightUpAllRooms();

            AddResultMessage("�ѵ������е�ͼ��");
        });

        INVINCIBLE = new DebugCommand<bool>("invincible", "����Ƿ����˺�", "invincible <bool>", (judge) =>
        {
            PlayerControler player = FindObjectOfType<PlayerControler>();
            player.isInvincibleAlways = judge;

            AddResultMessage($"�ѽ���Ҳ�����������Ϊ{judge}");
        });

        CHANGEHP = new DebugCommand<int>("changehp", "�ı����Ѫ��", "changehp <amount>", (amount) =>
        {
            PlayerControler player = FindObjectOfType<PlayerControler>();
            player.ChangeHp(amount);

            AddResultMessage($"���Ѫ���Ѿ��޸���{amount}��");
        });

        HELP = new DebugCommand("help", "�г�����������б�", "help", () =>
        {
            AddResultMessage("����ָ���б�");
            foreach (var command in commandList)
            {
                DebugCommandBase commandBase = command as DebugCommandBase;
                AddResultMessage($"{commandBase.commandFormat} - {commandBase.commandDescription}\n");
            }
        });

        EXP = new DebugCommand<int>("exp", "������Ҿ���ֵ", "exp <amount>", (amount) =>
        {
            PlayerControler player = FindObjectOfType<PlayerControler>();
            player.ChangeEx(amount);
            AddResultMessage($"�Ѿ��������{amount}�㾭��ֵ");
        });

        CLEAR = new DebugCommand("clear", "��������", "clear", () =>
        {
            resultMessage.Clear();
        });

        FONT_SIZE = new DebugCommand<int>("font_size", "��������̨�����С", "font_size <size>", (size) =>
        {
            fontSize = Mathf.Clamp(size, 10, 50);
            consoleStyle.fontSize = fontSize;
            AddResultMessage($"����̨�����С������Ϊ{fontSize}");
        });

        commandList = new List<object>()
        {
            KILL_ALL,
            GIVE,
            HIGHLIGHT,
            INVINCIBLE,
            CHANGEHP,
            EXP,
            LIGHTUP,
            HELP,
            CLEAR,
            FONT_SIZE
        };
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            OnToggleDebug();
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            OnReturn();
        }

        //ɱ¾�⻷
        if (Input.GetKey(KeyCode.F2))
        {
            PlayerControler player = FindObjectOfType<PlayerControler>();
            GameObject EmptyParent = MapCreater.StaticMap.RRoom[player.NowRoom].transform.GetChild(3).gameObject;
            for (int i = 0; i < EmptyParent.transform.childCount; i++)
            {
                Empty e = EmptyParent.transform.GetChild(i).GetComponent<Empty>();
                if (e != null) { Pokemon.PokemonHpChange(null, e.gameObject, e.maxHP, 0, 0, PokemonType.TypeEnum.IgnoreType); }
            }
        }

        //�޵�
        bool invincible = false;
        if (Input.GetKeyDown(KeyCode.F3))
        {
            if (!invincible)
            {
                invincible = true;
                PlayerControler player = FindObjectOfType<PlayerControler>();
                player.isInvincibleAlways = true;
            }
            else
            {
                invincible = false;
                PlayerControler player = FindObjectOfType<PlayerControler>();
                player.isInvincibleAlways = false;
            }

        }
    }

    Vector2 scroll;
    private void OnGUI()
    {
        if (!showConsole)
        {
            return;
        }

        GUI.enabled = true; // ȷ��GUI����
        GUI.depth = -1000; // ȷ��GUI������UI֮��

        GUIStyle style = new GUIStyle(GUI.skin.label);
        style.fontSize = fontSize;
        
        GUI.contentColor = Color.white;

        float y = 0f;

        //�����
        GUI.Box(new Rect(0, y, Screen.width, 200), "");

        Rect viewport = new Rect(0, 0, Screen.width - 30, 20 * commandList.Count);

        scroll.y = Mathf.Clamp(scroll.y, 0, Mathf.Max(0, resultMessage.Count * 20 - 190));

        scroll = GUI.BeginScrollView(new Rect(0, y + 5f, Screen.width, 190), scroll, viewport);

        for (int i = 0; i < resultMessage.Count; i++)
        {
            Rect labelRect = new Rect(5, 20 * i, viewport.width - 100, 20);
            GUI.Label(labelRect, resultMessage[i], consoleStyle);
        }
        GUI.EndScrollView();
        y += 200;

        //�����
        GUI.Box(new Rect(0, y, Screen.width, 30), "");
        GUI.backgroundColor = new Color(0, 0, 0);
        GUI.contentColor = Color.white;
        //input = GUI.TextField(new Rect(10f, y + 5f, Screen.width - 20f, 20f), input);

        GUI.SetNextControlName("ConsoleInput");
        input = GUI.TextField(new Rect(10f, y + 5f, Screen.width - 20f, 20f), input, consoleStyle);

        if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Tab)
        {
            Event.current.Use();
            AutoCompleteCommand();
            GUI.FocusControl("ConsoleInput");
        }
    }


    private void HandleInput()
    {
        string[] parts = input.Split(' ');
        bool commandFound = false;

        foreach (var command in commandList)
        {
            DebugCommandBase commandBase = command as DebugCommandBase;
            if (input.Contains(commandBase.commandId.ToLower()))
            {
                commandFound = true;
                Type commandType = command.GetType();
                if (commandType.IsGenericType)
                {
                    //��ȡ���Ͳ�������
                    Type[] genericTypes = commandType.GetGenericArguments();

                    object[] parameters = new object[genericTypes.Length];
                    bool parseSuccess = true;

                    for (int i = 0; i < genericTypes.Length; i++)
                    {
                        if (parts.Length > i + 1 && TryParseParameter(parts[i + 1], genericTypes[i], out object parsedValue))
                        {
                            parameters[i] = parsedValue;
                        }
                        else
                        {
                            parseSuccess = false;
                            break;
                        }
                    }

                    if (parseSuccess)
                    {
                        commandType.GetMethod("invoke").Invoke(command, parameters);
                    }
                    else
                    {
                        AddResultMessage($"��������ʧ��: {input}");
                    }
                }
                else
                {
                    (command as DebugCommand).invoke();
                }

                break;
            }
        }
        if (!commandFound)
        {
            AddResultMessage($"ָ��δ�ҵ�{input}");
        }
    }

    private bool TryParseParameter(string input, Type targetType, out object value)
    {
        value = null;

        if (targetType == typeof(bool))
        {
            if (bool.TryParse(input, out bool boolValue))
            {
                value = boolValue;
                return true;
            }
        }
        else if (targetType == typeof(int))
        {
            if (int.TryParse(input, out int intValue))
            {
                value = intValue;
                return true;
            }
        }
        else if (targetType.IsEnum)
        {
            try
            {
                value = Enum.Parse(targetType, input, true); //true��ʾ���Դ�Сд
                return true;
            }
            catch
            {
                return false;
            }
        }

        return false;
    }
    private void AddResultMessage(string message)
    {
        resultMessage.Add(message);
        if (resultMessage.Count > 100)
        {
            resultMessage.RemoveAt(0); // �Ƴ������һ����¼
        }

        scroll.y = float.MaxValue;
    }

    private void AutoCompleteCommand()
    {
        if (string.IsNullOrEmpty(input))
        {
            return;
        }

        //��ȫ�����
        string[] parts = input.Split(' ');
        List<string> matchedCommands = FindMatchedCommands(input);

        if (matchedCommands.Count == 1)
        {
            input = matchedCommands[0];
        }
        else if (matchedCommands.Count > 1)
        {
            AddResultMessage("ƥ�������:");
            foreach (var cmd in matchedCommands)
            {
                AddResultMessage(cmd);
            }
        }

        //��ȫ��������
        else if (parts.Length > 1)
        {
            string commandName = parts[0];
            int parameterIndex = parts.Length - 1;

            var command = commandList.FirstOrDefault(cmd =>
                (cmd as DebugCommandBase)?.commandId.Equals(commandName, StringComparison.OrdinalIgnoreCase) == true);

            if (command != null)
            {
                List<string> suggestions = GetParameterSuggestions(command, parameterIndex, parts[parameterIndex]);
                if (suggestions.Count == 1)
                {
                    input = string.Join(" ", parts.Take(parameterIndex)) + " " + suggestions[0] + " ";
                }
                else if (suggestions.Count > 1)
                {
                    AddResultMessage("���ò���:");
                    foreach (var suggestion in suggestions)
                    {
                        AddResultMessage(suggestion);
                    }
                }
                else
                {
                    AddResultMessage("û�п��õĲ�������");
                }
            }
            else
            {
                AddResultMessage("δ�ҵ�ƥ�������");
            }
        }
    }

    private List<string> FindMatchedCommands(string partialCommand)
    {
        List<string> matchedCommands = new List<string>();

        foreach (var command in commandList)
        {
            DebugCommandBase commandBase = command as DebugCommandBase;
            if (commandBase.commandId.StartsWith(partialCommand, StringComparison.OrdinalIgnoreCase))
            {
                matchedCommands.Add(commandBase.commandId);
            }
        }

        return matchedCommands;
    }

    private List<string> GetParameterSuggestions(object command, int parameterIndex, string partialParameter)
    {
        List<string> suggestions = new List<string>();

        Type commandType = command.GetType();
        if (commandType.IsGenericType)
        {
            Type[] genericTypes = commandType.GetGenericArguments();
            if (parameterIndex <= genericTypes.Length)
            {
                Type parameterType = genericTypes[parameterIndex - 1];

                if (parameterType == typeof(bool))
                {
                    suggestions.AddRange(new[] { "true", "false" }
                        .Where(s => s.StartsWith(partialParameter, StringComparison.OrdinalIgnoreCase)));
                }
                else if (parameterType.IsEnum)
                {
                    suggestions.AddRange(Enum.GetNames(parameterType)
                        .Where(s => s.StartsWith(partialParameter, StringComparison.OrdinalIgnoreCase)));
                }
                else if (parameterType == typeof(int))
                {
                    AddResultMessage("��Ҫһ�� int ���͵���ֵ");
                }
            }
        }

        return suggestions;
    }
}

/*
private void HandleInput()
{
    string[] parts = input.Split(' ');
    for (int i = 0; i < commandList.Count;i++)
    {
        DebugCommandBase commandbase= commandList[i] as DebugCommandBase;

        if (input.Contains(commandbase.commandId))
        {
            if (commandList[i] as DebugCommand != null)
            {
                (commandList[i] as DebugCommand).invoke();
            }
            else if (commandList[i] as DebugCommand<bool> != null)
            {

            }
            else if (commandList[i] as DebugCommand<ItemType, int> != null)
            {
                if (parts.Length == 3 && Enum.TryParse(parts[1], true, out ItemType item) && int.TryParse(parts[2], out int amount))
                {
                    (commandList[i] as DebugCommand<ItemType, int>).invoke(item, amount);
                }
            }
        }
    }
}
*/
