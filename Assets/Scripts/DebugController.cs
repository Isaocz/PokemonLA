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

        KILL_ALL = new DebugCommand("kill_all", "消灭场景内所有敌怪。", "kill_all", () =>
        {
            PlayerControler player = FindObjectOfType<PlayerControler>();
            GameObject EmptyParent = MapCreater.StaticMap.RRoom[player.NowRoom].transform.GetChild(3).gameObject;
            for (int i = 0; i < EmptyParent.transform.childCount; i++)
            {
                Empty e = EmptyParent.transform.GetChild(i).GetComponent<Empty>();
                if (e != null) { Pokemon.PokemonHpChange(null, e.gameObject, e.maxHP, 0, 0, PokemonType.TypeEnum.IgnoreType); }
            }

            AddResultMessage("已经消灭了场景内所有敌对宝可梦");
        });

        GIVE = new DebugCommand<ItemType, int>("give", "给予玩家游戏物品", "give <item> <amount>", (item, amount) =>
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

            AddResultMessage($"已给予玩家{amount}个{item}");
        });

        HIGHLIGHT = new DebugCommand<bool>("highlight", "高亮玩家显示", "highlight <bool>", (judge) =>
        {
            PlayerPrefs.SetInt("Highlight", judge ? 1 : 0);
            InitializePlayerSetting.GlobalPlayerSetting.isHighlight = judge;

            AddResultMessage($"已将高亮显示设置为{judge}");
        });

        LIGHTUP = new DebugCommand("lightup", "点亮小地图的所有地图块", "lightup", () =>
        {
            UiMiniMap.Instance.LightUpAllRooms();

            AddResultMessage("已点亮所有地图块");
        });

        INVINCIBLE = new DebugCommand<bool>("invincible", "玩家是否不再伤害", "invincible <bool>", (judge) =>
        {
            PlayerControler player = FindObjectOfType<PlayerControler>();
            player.isInvincibleAlways = judge;

            AddResultMessage($"已将玩家不再受伤设置为{judge}");
        });

        CHANGEHP = new DebugCommand<int>("changehp", "改变玩家血量", "changehp <amount>", (amount) =>
        {
            PlayerControler player = FindObjectOfType<PlayerControler>();
            player.ChangeHp(amount);

            AddResultMessage($"玩家血量已经修改了{amount}点");
        });

        HELP = new DebugCommand("help", "列出所有命令的列表", "help", () =>
        {
            AddResultMessage("可用指令列表：");
            foreach (var command in commandList)
            {
                DebugCommandBase commandBase = command as DebugCommandBase;
                AddResultMessage($"{commandBase.commandFormat} - {commandBase.commandDescription}\n");
            }
        });

        EXP = new DebugCommand<int>("exp", "给予玩家经验值", "exp <amount>", (amount) =>
        {
            PlayerControler player = FindObjectOfType<PlayerControler>();
            player.ChangeEx(amount);
            AddResultMessage($"已经给予玩家{amount}点经验值");
        });

        CLEAR = new DebugCommand("clear", "清空输出栏", "clear", () =>
        {
            resultMessage.Clear();
        });

        FONT_SIZE = new DebugCommand<int>("font_size", "调整控制台字体大小", "font_size <size>", (size) =>
        {
            fontSize = Mathf.Clamp(size, 10, 50);
            consoleStyle.fontSize = fontSize;
            AddResultMessage($"控制台字体大小已设置为{fontSize}");
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

        //杀戮光环
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

        //无敌
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

        GUI.enabled = true; // 确保GUI可用
        GUI.depth = -1000; // 确保GUI在其他UI之上

        GUIStyle style = new GUIStyle(GUI.skin.label);
        style.fontSize = fontSize;
        
        GUI.contentColor = Color.white;

        float y = 0f;

        //输出框
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

        //输入框
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
                    //获取泛型参数类型
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
                        AddResultMessage($"参数解析失败: {input}");
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
            AddResultMessage($"指令未找到{input}");
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
                value = Enum.Parse(targetType, input, true); //true表示忽略大小写
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
            resultMessage.RemoveAt(0); // 移除最早的一条记录
        }

        scroll.y = float.MaxValue;
    }

    private void AutoCompleteCommand()
    {
        if (string.IsNullOrEmpty(input))
        {
            return;
        }

        //补全命令部分
        string[] parts = input.Split(' ');
        List<string> matchedCommands = FindMatchedCommands(input);

        if (matchedCommands.Count == 1)
        {
            input = matchedCommands[0];
        }
        else if (matchedCommands.Count > 1)
        {
            AddResultMessage("匹配的命令:");
            foreach (var cmd in matchedCommands)
            {
                AddResultMessage(cmd);
            }
        }

        //补全参数部分
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
                    AddResultMessage("可用参数:");
                    foreach (var suggestion in suggestions)
                    {
                        AddResultMessage(suggestion);
                    }
                }
                else
                {
                    AddResultMessage("没有可用的参数建议");
                }
            }
            else
            {
                AddResultMessage("未找到匹配的命令");
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
                    AddResultMessage("需要一个 int 类型的数值");
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
