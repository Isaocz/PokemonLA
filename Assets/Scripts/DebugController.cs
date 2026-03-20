using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class DebugController : MonoBehaviour
{

    //===========================================================
    // 游戏命令（只需加 Attribute 即可自动注册）
    //===========================================================

    [DebugCommand("help", "显示所有可用命令及其参数说明")]
    private void Help()
    {
        AddResultMessage("=== 可用指令列表 ===");

        foreach (var kv in commands)
        {
            DebugCommandBase cmd = kv.Value;

            string id = cmd.commandId;
            string desc = cmd.description;

            // 生成参数格式
            string paramFormat = "";
            var paramInfos = cmd.parameters;

            if (paramInfos.Length > 0)
            {
                paramFormat = string.Join(" ", paramInfos.Select(p => $"<{p.ParameterType.Name}>"));
            }

            string format = paramInfos.Length > 0 ? $"{id} {paramFormat}" : id;

            AddResultMessage($"{format}  ——  {desc}");
        }
    }


    [DebugCommand("kill_all", "消灭场景内所有敌怪")]
    private void KillAll()
    {
        GameObject EmptyParent = MapCreater.StaticMap.RRoom[player.NowRoom].transform.GetChild(3).gameObject;

        for (int i = 0; i < EmptyParent.transform.childCount; i++)
        {
            Empty e = EmptyParent.transform.GetChild(i).GetComponent<Empty>();
            if (e != null)
            {
                Pokemon.PokemonHpChange(null, e.gameObject, e.maxHP, 0, 0, PokemonType.TypeEnum.IgnoreType);
            }
        }

        AddResultMessage("已经消灭了场景内所有敌对宝可梦");
    }


    [DebugCommand("give", "给予玩家游戏物品")]
    private void Give(ItemType item, int amount)
    {
        switch (item)
        {
            case ItemType.Stone: player.ChangeStone(amount); break;
            case ItemType.Coin: player.ChangeMoney(amount); break;
            case ItemType.HearsScale: player.ChangeHearsScale(amount); break;
            case ItemType.PPUp: player.ChangePPUp(amount); break;
            case ItemType.SeedofMastery: player.ChangeMSeed(amount); break;
        }

        AddResultMessage($"已给予玩家 {amount} 个 {item}");
    }


    [DebugCommand("highlight", "高亮玩家显示")]
    private void Highlight(bool value)
    {
        PlayerPrefs.SetInt("Highlight", value ? 1 : 0);
        InitializePlayerSetting.GlobalPlayerSetting.isHighlight = value;

        AddResultMessage($"已将高亮显示设置为 {value}");
    }


    [DebugCommand("lightup", "点亮小地图所有房间")]
    private void LightUp()
    {
        UiMiniMap.Instance.LightUpAllRooms();
        AddResultMessage("已点亮所有地图块");
    }


    [DebugCommand("invincible", "玩家是否不再受伤")]
    private void Invincible(bool value)
    {
        player.isInvincibleAlways = value;
        AddResultMessage($"已将玩家不再受伤设置为 {value}");
    }


    [DebugCommand("changehp", "改变玩家血量")]
    private void ChangeHp(int amount)
    {
        player.ChangeHp(amount);
        AddResultMessage($"玩家血量已经修改了 {amount} 点");
    }


    [DebugCommand("exp", "给予玩家经验值")]
    private void AddExp(int amount)
    {
        player.ChangeEx(amount);
        AddResultMessage($"已经给予玩家 {amount} 点经验值");
    }


    [DebugCommand("clear", "清空控制台输出")]
    private void ClearConsole()
    {
        resultMessage.Clear();
    }


    [DebugCommand("font_size", "调整控制台字体大小")]
    private void SetFontSize(int size)
    {
        fontSize = Mathf.Clamp(size, 10, 50);
        consoleStyle.fontSize = fontSize;

        AddResultMessage($"控制台字体大小已设置为 {fontSize}");
    }


    [DebugCommand("get_skill", "获取技能球")]
    private void GetSkill(int index)
    {
        if (index % 2 != 0)
        {
            bool b = Instantiate(skillBall, player.NowRoom, Quaternion.identity)
                .GetComponent<SkillBall>()
                .SetSkill(index);
            if (b) { AddResultMessage($"获取技能 {index}"); }
            else { AddResultMessage($"技能不存在，随机生成技能"); }
        }
        else
        {
            AddResultMessage("！Warning：仅有非强化技能可用，请输入单数");
        }
    }


    [DebugCommand("get_randomskill", "获取随机技能技能球（全角色技能池）")]
    private void GetRandomSkill()
    {
        Instantiate(skillBall, player.NowRoom, Quaternion.identity)
                .GetComponent<SkillBall>()
                .RandomSetSkill();
        AddResultMessage($"获取随机技能技能球（全角色技能池）");
    }


    [DebugCommand("get_statetestskill", "获取状态测试技能")]
    private void GetStateTestSkill(TestSkill.StateTestType type)
    {
        bool b = Instantiate(skillBall, player.NowRoom, Quaternion.identity)
            .GetComponent<SkillBall>()
            .SetStateTestSkill(type);
        if (b) { AddResultMessage($"获取状态测试技能 {type}"); }
        else { AddResultMessage($"技能不存在，随机生成技能"); }
    }


    [DebugCommand("get_killallskill", "获取清图测试技能")]
    private void GetKillAllSkill()
    {
        Instantiate(skillBall, player.NowRoom, Quaternion.identity)
            .GetComponent<SkillBall>()
            .SetKillAllSkill();
        AddResultMessage($"获取清图测试技能");
    }


    [DebugCommand("plus_skill", "精通【某号】技能")]
    private void PlusSkill(int Index)
    {
        if (Index < 1 || Index > 4)
        {
            AddResultMessage($"请输入1，2，3，4 以精通几号技能");
        }
        else
        {
            switch(Index)
            {
                case 1:
                    if (player.Skill01 != null && player.Skill01.SkillFrom != 2 && player.Skill01.PlusSkill != null) { player.LearnNewSkillByOtherWay(player.Skill01.PlusSkill); AddResultMessage($"精通技能{Index}+{player.Skill01.SkillChineseName}"); }
                    else { AddResultMessage($"技能{Index}无法精通或不存在"); }
                    break;
                case 2:
                    if (player.Skill02 != null && player.Skill02.SkillFrom != 2 && player.Skill02.PlusSkill != null) { player.LearnNewSkillByOtherWay(player.Skill02.PlusSkill); AddResultMessage($"精通技能{Index}+{player.Skill02.SkillChineseName}"); }
                    else { AddResultMessage($"技能{Index}无法精通或不存在"); }
                    break;
                case 3:
                    if (player.Skill03 != null && player.Skill03.SkillFrom != 2 && player.Skill03.PlusSkill != null) { player.LearnNewSkillByOtherWay(player.Skill03.PlusSkill); AddResultMessage($"精通技能{Index}+{player.Skill03.SkillChineseName}"); }
                    else { AddResultMessage($"技能{Index}无法精通或不存在"); }
                    break;
                case 4:
                    if (player.Skill04 != null && player.Skill04.SkillFrom != 2 && player.Skill04.PlusSkill != null) { player.LearnNewSkillByOtherWay(player.Skill04.PlusSkill); AddResultMessage($"精通技能{Index}+{player.Skill04.SkillChineseName}"); }
                    else { AddResultMessage($"技能{Index}无法精通或不存在"); }
                    break;
            }
        }

    }


    [DebugCommand("get_Item", "获取指定道具")]
    private void GetItem(int index)
    {
        bool b = Instantiate(itemBall, player.NowRoom, Quaternion.identity)
            .GetComponent<PokemonBall>()
            .SetPassiveItemIndex(index);
        if (b) { AddResultMessage($"获取道具 {index}"); }
        else { AddResultMessage($"道具不存在，随机生成道具"); }
    }


    [DebugCommand("get_randomitem", "获取随机被动道具道具球（全道具池）")]
    private void GetRandomItem()
    {
        Instantiate(itemBall, player.NowRoom, Quaternion.identity)
                .GetComponent<PokemonBall>()
                .SetRandomPassiveItemIndex();
        AddResultMessage($"获取随机被动道具道具球（全道具池）");
    }



    //===========================================================
    // 游戏命令（只需加 Attribute 即可自动注册）
    //===========================================================















    //===========================================================

    public enum ItemType
    {
        Stone,
        Coin,
        HearsScale,
        PPUp,
        SeedofMastery
    }


    public enum TestSkillType
    {

    }


    public SkillBall skillBall;


    public PokemonBall itemBall;


    //===========================================================








    private Dictionary<string, DebugCommandBase> commands = new();
    private List<string> resultMessage = new();
    private string input = "";
    private bool showConsole = false;

    private GUIStyle consoleStyle;
    private int fontSize = 20;
    private Vector2 scroll;

    private PlayerControler player;

    private void Awake()
    {
        player = FindObjectOfType<PlayerControler>();

        consoleStyle = new GUIStyle();
        consoleStyle.fontSize = fontSize;
        consoleStyle.normal.textColor = Color.white;

        RegisterCommands();
    }

    private void RegisterCommands()
    {
        var methods = GetType().GetMethods(
            BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

        AddResultMessage($"不确定命令内容时输入【help】获取所有命令列表");
        AddResultMessage($"Tab键可自动补全命令");
        foreach (var m in methods)
        {
            var attr = m.GetCustomAttribute<DebugCommandAttribute>();
            if (attr != null)
            {
                var cmd = new DebugCommandBase(attr.CommandId, attr.Description, m, this);
                commands[attr.CommandId] = cmd;

                //AddResultMessage($"注册命令：{attr.CommandId}");
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
            showConsole = !showConsole;

        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (showConsole)
            {
                HandleInput();
                input = "";
            }
        }
    }

    private void OnGUI()
    {
        if (!showConsole)
            return;

        GUI.depth = -1000;

        float y = 0;

        GUI.Box(new Rect(0, y, Screen.width, 350), "");

        Rect viewport = new Rect(0, 0, Screen.width - 30, resultMessage.Count * 20);
        scroll = GUI.BeginScrollView(new Rect(0, y + 5, Screen.width, 340), scroll, viewport);

        for (int i = 0; i < resultMessage.Count; i++)
        {
            GUI.Label(new Rect(5, 20 * i, viewport.width - 10, 20), resultMessage[i], consoleStyle);
        }

        GUI.EndScrollView();
        y += 350;

        GUI.Box(new Rect(0, y, Screen.width, 30), "");
        GUI.SetNextControlName("ConsoleInput");
        input = GUI.TextField(new Rect(10, y + 5, Screen.width - 20, 20), input, consoleStyle);

        if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Tab)
        {
            Event.current.Use();
            AutoCompleteCommand();
            GUI.FocusControl("ConsoleInput");
        }
    }

    private void HandleInput()
    {
        if (string.IsNullOrWhiteSpace(input))
            return;

        string[] parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        string cmdName = parts[0].ToLower();

        if (!commands.TryGetValue(cmdName, out var cmd))
        {
            AddResultMessage($"未找到指令：{cmdName}");
            return;
        }

        var paramInfos = cmd.parameters;
        int paramCount = paramInfos.Length;

        if (parts.Length - 1 != paramCount)
        {
            AddResultMessage($"参数数量错误，应为 {paramCount} 个");
            return;
        }

        object[] parsed = new object[paramCount];

        for (int i = 0; i < paramCount; i++)
        {
            string raw = parts[i + 1];
            Type t = paramInfos[i].ParameterType;

            if (!TryParseParameter(raw, t, out object value))
            {
                AddResultMessage($"参数解析失败：{raw} → {t.Name}");
                return;
            }

            parsed[i] = value;
        }

        cmd.Invoke(parsed);
    }

    private bool TryParseParameter(string input, Type targetType, out object value)
    {
        value = null;

        if (targetType == typeof(int))
        {
            if (int.TryParse(input, out int v)) { value = v; return true; }
        }
        else if (targetType == typeof(bool))
        {
            if (bool.TryParse(input, out bool v)) { value = v; return true; }
        }
        else if (targetType.IsEnum)
        {
            try { value = Enum.Parse(targetType, input, true); return true; }
            catch { return false; }
        }
        else if (targetType == typeof(string))
        {
            value = input;
            return true;
        }

        return false;
    }

    private void AddResultMessage(string msg)
    {
        resultMessage.Add(msg);
        if (resultMessage.Count > 100)
            resultMessage.RemoveAt(0);

        scroll.y = float.MaxValue;
    }

    private List<string> FindMatchedCommands(string partial)
    {
        partial = partial.ToLower();
        return commands.Keys
            .Where(k => k.StartsWith(partial))
            .ToList();
    }

    private List<string> GetParameterSuggestions(DebugCommandBase cmd, int paramIndex, string partial)
    {
        List<string> list = new();

        var paramInfos = cmd.parameters;
        if (paramIndex >= paramInfos.Length)
            return list;

        Type t = paramInfos[paramIndex].ParameterType;

        if (t == typeof(bool))
        {
            return new[] { "true", "false" }
                .Where(s => s.StartsWith(partial, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }
        else if (t.IsEnum)
        {
            return Enum.GetNames(t)
                .Where(s => s.StartsWith(partial, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        return list;
    }

    private void AutoCompleteCommand()
    {
        if (string.IsNullOrEmpty(input))
            return;

        string[] parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        if (parts.Length == 1)
        {
            var matches = FindMatchedCommands(parts[0]);
            if (matches.Count == 1)
                input = matches[0] + " ";
            else if (matches.Count > 1)
            {
                AddResultMessage("匹配的命令：");
                foreach (var m in matches) AddResultMessage(m);
            }
            return;
        }

        string cmdName = parts[0];
        if (!commands.TryGetValue(cmdName, out var cmd))
            return;

        int paramIndex = parts.Length - 1;
        string partial = parts[paramIndex];

        var suggestions = GetParameterSuggestions(cmd, paramIndex - 1, partial);

        if (suggestions.Count == 1)
        {
            parts[paramIndex] = suggestions[0];
            input = string.Join(" ", parts) + " ";
        }
        else if (suggestions.Count > 1)
        {
            AddResultMessage("可用参数：");
            foreach (var s in suggestions) AddResultMessage(s);
        }
    }



















}