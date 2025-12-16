// Assets/Editor/CreatEmptyStateMachineGeneratorWindow.cs
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

public class CreatEmptyStateMachineGeneratorWindow : EditorWindow
{
    //目标脚本
    private MonoScript targetScript;
    //是否备份
    private bool BackupOriginal = true;
    //是否按照有副状态的状态机生成脚本
    private bool HaveSubState = true;

    private Vector2 scroll;

    [MenuItem("Window/Creat Empty State MachineGenerator")]
    public static void OpenWindow() => GetWindow<CreatEmptyStateMachineGeneratorWindow>("Creat Empty State MachineGenerator");

    private void OnGUI()
    {


        scroll = EditorGUILayout.BeginScrollView(scroll);
        EditorGUILayout.LabelField("Creat Empty State MachineGenerator", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        targetScript = EditorGUILayout.ObjectField("Target Script", targetScript, typeof(MonoScript), false) as MonoScript;
        BackupOriginal = EditorGUILayout.Toggle("Backup Original Before Editing", BackupOriginal);
        HaveSubState = EditorGUILayout.Toggle("Have Sub State Mechine", HaveSubState);

        EditorGUILayout.Space();
        using (new EditorGUILayout.HorizontalScope())
        {
            if (GUILayout.Button("Refresh"))
            {
            }
        }

        EditorGUILayout.Space();
        if (GUILayout.Button("Generate"))
        {
            if (targetScript == null)
            {
                EditorUtility.DisplayDialog("Error", "Please assign a script (MonoScript).", "OK");
            }
            else
            {
                try
                {
                    var path = AssetDatabase.GetAssetPath(targetScript);
                    if (!File.Exists(path)) throw new Exception("Cannot find script file path.");
                    if (HaveSubState)
                    {
                        ProcessScript_SubState(path);
                    }
                    else
                    {
                        ProcessScript(path);
                    }
                    AssetDatabase.Refresh();
                    EditorUtility.DisplayDialog("Success", "Generation finished", "OK");
                }
                catch (Exception ex)
                {
                    Debug.LogException(ex);
                    EditorUtility.DisplayDialog("Error", ex.Message, "OK");
                }
            }
        }

        EditorGUILayout.EndScrollView();
    }











    //==========================单一状态机=============================



    /// <summary>
    /// 生成单一状态机所需的代码
    /// </summary>
    /// <param name="scriptPath"></param>
    private void ProcessScript(string scriptPath)
    {
        var source = File.ReadAllText(scriptPath, Encoding.UTF8);


        //创建备份
        if (BackupOriginal)
        {
            var bak = scriptPath + ".bak";
            File.Copy(scriptPath, bak, true);
            Debug.Log("CreatEmptyStateMachineGenerator: backup created " + bak);
        }


        // 获取命名空间
        var nsMatch = Regex.Match(source, @"namespace\s+([A-Za-z0-9_.]+)\s*\{", RegexOptions.Singleline);
        var ns = nsMatch.Success ? nsMatch.Groups[1].Value : null;


        // 获取Empty类名
        var classMatch = Regex.Match(source, @"public\s+class\s+([A-Za-z0-9_]+)\s*:\s*Empty");
        if (!classMatch.Success) classMatch = Regex.Match(source, @"class\s+([A-Za-z0-9_]+)\s*:\s*Empty");
        if (!classMatch.Success) throw new Exception("Target script must be a Empty class.");
        var className = classMatch.Groups[1].Value;








        //找到状态机枚举
        var enumMatch = Regex.Match(source, @"enum\s+([A-Za-z0-9_]+)\s*\{([^}]*)\}", RegexOptions.Singleline);
        if (!enumMatch.Success) throw new Exception("No public enum found in the script file. Place the main-state enum in the same file.");

        var enumName = enumMatch.Groups[1].Value;
        var membersRaw = enumMatch.Groups[2].Value;


        // 对每一行尝试提取 成员 和 行尾注释
        var linePattern = new Regex(@"([A-Za-z0-9_]+)\s*(?:=\s*[^,]+)?\s*,?\s*(//\s*(.*))?");
        string[] memberNames = { };
        string[] memberNamesComment = { };
        foreach (Match m in linePattern.Matches(membersRaw))
        {
            var member = m.Groups[1].Value;
            var comment = m.Groups[3].Success ? m.Groups[3].Value.Trim() : "<no comment>";
            List<string> _listName = new List<string>(memberNames);
            _listName.Add(member);
            memberNames = _listName.ToArray();
            List<string> _listComment = new List<string>(memberNamesComment);
            _listComment.Add(comment);
            memberNamesComment = _listComment.ToArray();
        }
        if (memberNames.Length == 0) throw new Exception("Enum has no members.");



        // 找状态机实例的名字
        var fieldPattern = new Regex(@"\b" + Regex.Escape(enumName) + @"\s+([A-Za-z0-9_]+)\s*;");
        var f = fieldPattern.Match(source);
        var enumStateName = "";
        if (f.Success) {
            enumStateName = f.Groups[1].Value;
        }
        else { Console.WriteLine("Field not found"); }





        // 生成代码
        var sb = new StringBuilder();
        sb = BuildCodeStateStartOver(memberNames, memberNamesComment, enumName, enumStateName, sb);
        var StateSource = File.ReadAllText(scriptPath, Encoding.UTF8);
        var StateInjection = sb.ToString();
        var StatePattern = @"//\s*InsertStateFunction\b[^\r\n]*\r?\n";
        var StateReplaced = Regex.Replace(StateSource, StatePattern, m => m.Value + "\n" + StateInjection, RegexOptions.Singleline);
        //添加失败
        if (StateReplaced == StateSource)
        {
            // 没找到插入目标
            var updateMatch = Regex.Match(StateSource, @"void\s+Update\s*\(\s*\)\s*\{", RegexOptions.Singleline);
            if (updateMatch.Success)
            {
                var pos = updateMatch.Index + updateMatch.Length;
                StateReplaced = StateSource.Insert(pos, "\n            // ！！！添加状态Start&Over失败！！！\n");
            }
        }
        File.WriteAllText(scriptPath, StateReplaced, new System.Text.UTF8Encoding(true));



        // 在Update中加入状态机变化
        var UpdatedSource = File.ReadAllText(scriptPath, Encoding.UTF8);
        var UpdateInjection = BuildCodeUpdateSwitch(memberNames , memberNamesComment , enumName, enumStateName, sb);
        var UpdatePattern = @"//\s*InsertStateMechineSwitch\b[^\r\n]*\r?\n";
        var UpdateReplaced = Regex.Replace(UpdatedSource, UpdatePattern, m => m.Value + "\n" + UpdateInjection, RegexOptions.Singleline);

        if (UpdateReplaced == UpdatedSource)
        {
            // no clearHP(); found: try to insert reminder in Update method
            var updateMatch = Regex.Match(UpdatedSource, @"void\s+Update\s*\(\s*\)\s*\{", RegexOptions.Singleline);
            if (updateMatch.Success)
            {
                var pos = updateMatch.Index + updateMatch.Length;
                UpdateReplaced = UpdatedSource.Insert(pos, "\n            // ！！！添加UpdateSwitch失败！！！\n");
            }
        }
        File.WriteAllText(scriptPath, UpdateReplaced, new System.Text.UTF8Encoding(true));
        //Debug.Log("CreatEmptyStateMachineGenerator: injection applied (if clearHP() existed).");



    }



    /// <summary>
    /// 生成单一状态机的状态开始结束的方法
    /// </summary>
    /// <param name="enumNameList"></param>
    /// <param name="dict"></param>
    /// <param name="fields"></param>
    /// <returns></returns>
    private StringBuilder BuildCodeStateStartOver(string[] enumNameList , string[] enumCommentList , string enumMechineName, string enumStateName, StringBuilder sb)
    {
        for (int i = 0; i < enumNameList.Length; i++) {
            sb.AppendLine();
            sb.AppendLine();
            sb.AppendLine();
            sb.AppendLine("    //=========================" + enumCommentList[i] + "============================");
            sb.AppendLine();
            sb.AppendLine();
            //如果为Idle状态 生成其他状态变化为idle状态时的idle时间
            if (enumNameList[i].IndexOf("Idle", StringComparison.OrdinalIgnoreCase) >= 0
                || enumNameList[i].Equals("Idle", StringComparison.Ordinal))
            {
                sb.AppendLine("    //开始后的冷却时间");
                sb.AppendLine("    static float TIME_" + enumNameList[i].ToUpper() + "_START = 0.0f; //TODO需修改时间");
                for (int j = 0; j < enumNameList.Length; j++)
                {
                    if (i != j)
                    {
                        sb.AppendLine();
                        sb.AppendLine("    //" + enumCommentList[j] + "后的冷却时间");
                        sb.AppendLine("    static float TIME_" + enumNameList[i].ToUpper() + "_" + enumNameList[j].ToUpper() + " = 0.0f; //TODO需修改时间");
                        sb.AppendLine();
                    }
                }
            }
            sb.AppendLine();
            sb.AppendLine();
            sb.AppendLine("    /// <summary>");
            sb.AppendLine("    /// " + enumCommentList[i] + "计时器");
            sb.AppendLine("    /// <summary>");
            sb.AppendLine("    float " + enumNameList[i] + "Timer = 0;");
            sb.AppendLine();
            sb.AppendLine("    /// <summary>");
            sb.AppendLine("    /// " + enumCommentList[i] + "开始");
            sb.AppendLine("    /// <summary>");
            sb.AppendLine("    public void " + enumNameList[i] + "Start(float Timer)");
            sb.AppendLine("    {");
            sb.AppendLine("        " + enumNameList[i] + "Timer = Timer;");
            sb.AppendLine("        " + enumStateName + " = " + enumMechineName + "." + enumNameList[i] + ";");
            sb.AppendLine("    }");
            sb.AppendLine();
            sb.AppendLine("    /// <summary>");
            sb.AppendLine("    /// " + enumCommentList[i] + "结束");
            sb.AppendLine("    /// <summary>");
            sb.AppendLine("    public void " + enumNameList[i] + "Over()");
            sb.AppendLine("    {");
            sb.AppendLine("        " + enumNameList[i] + "Timer = 0;");
            sb.AppendLine("    }");
            sb.AppendLine();
            sb.AppendLine();
            sb.AppendLine("    //=========================" + enumCommentList[i] + "============================");
            sb.AppendLine();
            sb.AppendLine();
            sb.AppendLine();
        }
        return sb;

    }



    /// <summary>
    /// 生成单一状态机的Update中的Switch文
    /// </summary>
    /// <param name="enumName"></param>
    /// <param name="dict"></param>
    /// <param name="fields"></param>
    /// <returns></returns>
    private string BuildCodeUpdateSwitch(string[] enumNameList , string[] enumCommentList , string enumMechineName, string enumStateName, StringBuilder sb)
    {
        sb = new StringBuilder();
        sb.AppendLine();
        sb.AppendLine("            //■■开始判断状态机 当处于冰冻 睡眠 致盲 麻痹状态时状态机停运");
        sb.AppendLine("            if (!isEmptyFrozenDone && !isSleepDone && !isSilence && !isCanNotMoveWhenParalysis) /* TODO状态机停运的额外条件 */");
        sb.AppendLine("            {");
        sb.AppendLine("                switch (" + enumStateName + ")");
        sb.AppendLine("                {");
        for (int i = 0; i < enumNameList.Length; i++)
        {
            sb.AppendLine("                    //" + enumCommentList[i] + "状态");
            sb.AppendLine("                    case " + enumMechineName + "." + enumNameList[i] + ":");
            sb.AppendLine("                        " + enumNameList[i] + "Timer -= Time.deltaTime;//" + enumCommentList[i] + "计时器时间减少");
            sb.AppendLine("                        if ( " + enumNameList[i] + "Timer <= 0 )         //计时器时间到时间，结束" + enumCommentList[i] + "状态");
            sb.AppendLine("                        {");
            sb.AppendLine("                            " + enumNameList[i] + "Over();");
            sb.AppendLine("                            //TODO添加下一个状态的开始方法");
            sb.AppendLine("                        }");
            sb.AppendLine("                        break;");
        }
        sb.AppendLine("                }");
        sb.AppendLine("            }");
        sb.AppendLine("            //■■结束判断状态机");
        sb.AppendLine();
        return sb.ToString();
    }




    //==========================单一状态机=============================


















    //==========================有副状态的状态机=============================


    //todo 添加check 是否所有副状态都和主状态匹配

    /// <summary>
    /// 副状态机结构
    /// </summary>
    public class SubStateMechine
    {
        public string MainStateMechineName;
        public Dictionary<string, string> MainMembersWithComments = new();
        public string MainFieldName;

        public string SubStateMechineName;
        public Dictionary<string, string> SubMembersWithComments = new();
        public string SubFieldName;

        public Dictionary<string, List<string>> StateMap = new();
    }



    /// <summary>
    /// 生成有副状态的状态机所需的代码
    /// </summary>
    /// <param name="scriptPath"></param>
    private void ProcessScript_SubState(string scriptPath)
    {
        var source = File.ReadAllText(scriptPath, Encoding.UTF8);


        //创建备份
        if (BackupOriginal)
        {
            var bak = scriptPath + ".bak";
            File.Copy(scriptPath, bak, true);
            Debug.Log("CreatEmptyStateMachineGenerator: backup created " + bak);
        }


        // 获取命名空间
        var nsMatch = Regex.Match(source, @"namespace\s+([A-Za-z0-9_.]+)\s*\{", RegexOptions.Singleline);
        var ns = nsMatch.Success ? nsMatch.Groups[1].Value : null;


        // 获取Empty类名
        var classMatch = Regex.Match(source, @"public\s+class\s+([A-Za-z0-9_]+)\s*:\s*Empty");
        if (!classMatch.Success) classMatch = Regex.Match(source, @"class\s+([A-Za-z0-9_]+)\s*:\s*Empty");
        if (!classMatch.Success) throw new Exception("Target script must be a Empty class.");
        var className = classMatch.Groups[1].Value;




        var r = new SubStateMechine();

        // 抽取 enum 块及成员（支持 /// 注释 / 行尾 // 注释）
        // 匹配 enum 定义（public 或无修饰），捕获名字与花括号内内容
        string enumPattern = @"enum\s+([A-Za-z0-9_]+)\s*\{([^}]*)\}";
        foreach (Match m in Regex.Matches(source, enumPattern, RegexOptions.Singleline))
        {

            var enumName = m.Groups[1].Value;
            var membersRaw = m.Groups[2].Value;

            // 对每一行尝试提取 成员 和 行尾注释
            var memberPattern = new Regex(@"([A-Za-z0-9_]+)\s*(?:=\s*[^,]+)?\s*,?\s*(//\s*(.*))?");
            var members = new Dictionary<string, string>();
            foreach (Match mm in memberPattern.Matches(membersRaw))
            {
                var name = mm.Groups[1].Value;
                if (string.IsNullOrWhiteSpace(name)) continue;
                var comment = mm.Groups[3].Success ? mm.Groups[3].Value.Trim() : null;
                Debug.Log(name);
                Debug.Log(comment);
                members[name] = string.IsNullOrEmpty(comment) ? null : comment;
            }




            // 根据名字判断是 MainState 还是 SubState
            if (enumName.IndexOf("Main", StringComparison.OrdinalIgnoreCase) >= 0
                || enumName.Equals("MainState", StringComparison.Ordinal))
            {
                r.MainStateMechineName = enumName;
                r.MainMembersWithComments = members;
            }
            else if (enumName.IndexOf("Sub", StringComparison.OrdinalIgnoreCase) >= 0
                     || enumName.Equals("SubState", StringComparison.Ordinal))
            {
                r.SubStateMechineName = enumName;
                r.SubMembersWithComments = members;
            }
            else
            {
                // 若无法凭名字判断，可基于成员名或文件上下文决定，这里优先填充未占位槽
                if (string.IsNullOrEmpty(r.MainStateMechineName))
                {
                    r.MainStateMechineName = enumName;
                    r.MainMembersWithComments = members;
                }
                else if (string.IsNullOrEmpty(r.SubStateMechineName))
                {
                    r.SubStateMechineName = enumName;
                    r.SubMembersWithComments = members;
                }
            }
        }

        // 找到对应的字段声明：Pattern: EnumType fieldName;
        string fieldPatternTemplate = @"\b{0}\s+([A-Za-z0-9_]+)\s*;";
        if (!string.IsNullOrEmpty(r.MainStateMechineName))
        {
            var fieldPattern = string.Format(fieldPatternTemplate, Regex.Escape(r.MainStateMechineName));
            var fm = Regex.Match(source, fieldPattern);
            if (fm.Success) r.MainFieldName = fm.Groups[1].Value;
        }
        if (!string.IsNullOrEmpty(r.SubStateMechineName))
        {
            var fieldPattern = string.Format(fieldPatternTemplate, Regex.Escape(r.SubStateMechineName));
            var fm = Regex.Match(source, fieldPattern);
            if (fm.Success) r.SubFieldName = fm.Groups[1].Value;
        }

        // 解析 StateMap 初始化体
        // 找到 "Dictionary<MainState, SubState[]>" 或 "StateMap = new()" 初始化块并读取键值对
        // 一个宽松的模式：在等号后的花括号内找 { MainState.Frozen, new[] { SubState.X, ... } }
        var mapPattern = @"StateMap\s*=\s*new\s*\(\s*\)\s*\s*\{\s*(?<body>.*?)\s*\}\s*;";
        var mapMatch = Regex.Match(source, mapPattern, RegexOptions.Singleline);
        if (!mapMatch.Success)
        {
            // 也尝试没使用 new() 而是有类型显式 new Dictionary<...> { ... }
            mapPattern = @"StateMap\s*=\s*new\s+[^\{]+\{\s*(?<body>.*?)\s*\}\s*;";
            mapMatch = Regex.Match(source, mapPattern, RegexOptions.Singleline);
        }

        if (mapMatch.Success)
        {
            var body = mapMatch.Groups["body"].Value;
            // 匹配每一项 { MainState.Frozen, new[] { SubState.Idle_Frozen, SubState.Walk_Frozen } }
            var entryPattern = new Regex(@"\{\s*(?<main>[A-Za-z0-9_.]+)\s*,\s*new\s*\[\]\s*\{\s*(?<subs>[^}]+)\}\s*\}", RegexOptions.Singleline);
            foreach (Match em in entryPattern.Matches(body))
            {
                var mainFull = em.Groups["main"].Value.Trim(); // e.g. MainState.Frozen
                var mainName = mainFull.Contains('.') ? mainFull.Split('.')[1] : mainFull;
                var subsRaw = em.Groups["subs"].Value;
                // split by comma and trim; accept SubState.X or X
                var subs = subsRaw.Split(',')
                                  .Select(s => s.Trim())
                                  .Where(s => !string.IsNullOrEmpty(s))
                                  .Select(s => s.Contains('.') ? s.Split('.')[1] : s)
                                  .ToList();
                r.StateMap[mainName] = subs;
            }
        }
        /**
        //test输出状态机
        Debug.Log($"MainEnum: {r.MainStateMechineName}  Field: {r.MainFieldName}");
        foreach (var kv in r.MainMembersWithComments) Debug.Log($"  {kv.Key} -> {kv.Value}");
        Debug.Log($"SubEnum: {r.SubStateMechineName}  Field: {r.SubFieldName}");
        foreach (var kv in r.SubMembersWithComments) Debug.Log($"  {kv.Key} -> {kv.Value}");
        Debug.Log("StateMap:");
        foreach (var kv in r.StateMap) Debug.Log($"  {kv.Key} -> [{string.Join(", ", kv.Value)}]");
        **/









        
        // 生成StatrOver代码
        var sb = new StringBuilder();
        sb = BuildCodeStateStartOver_SubState(r, sb);
        var StateSource = File.ReadAllText(scriptPath, Encoding.UTF8);
        var StateInjection = sb.ToString();
        var StatePattern = @"//\s*InsertStateFunction\b[^\r\n]*\r?\n";
        var StateReplaced = Regex.Replace(StateSource, StatePattern, m => m.Value + "\n" + StateInjection, RegexOptions.Singleline);
        //添加失败
        if (StateReplaced == StateSource)
        {
            // 没找到插入目标
            var updateMatch = Regex.Match(StateSource, @"void\s+Update\s*\(\s*\)\s*\{", RegexOptions.Singleline);
            if (updateMatch.Success)
            {
                var pos = updateMatch.Index + updateMatch.Length;
                StateReplaced = StateSource.Insert(pos, "\n            // ！！！添加状态Start&Over失败！！！\n");
            }
        }
        File.WriteAllText(scriptPath, StateReplaced, new System.Text.UTF8Encoding(true));


        // 生成副状态切换代码
        var sb2 = new StringBuilder();
        sb2 = BuildCodeChangeSubState_SubState(r, sb2);
        var ChangeSource = File.ReadAllText(scriptPath, Encoding.UTF8);
        var ChangeInjection = sb2.ToString();
        var ChangePattern = @"//\s*InsertSubStateChange\b[^\r\n]*\r?\n";
        var ChangeReplaced = Regex.Replace(ChangeSource, ChangePattern, m => m.Value + "\n" + ChangeInjection, RegexOptions.Singleline);
        //添加失败
        if (ChangeReplaced == ChangeSource)
        {
            // 没找到插入目标
            var updateMatch = Regex.Match(ChangeSource, @"void\s+Update\s*\(\s*\)\s*\{", RegexOptions.Singleline);
            if (updateMatch.Success)
            {
                var pos = updateMatch.Index + updateMatch.Length;
                ChangeReplaced = ChangeSource.Insert(pos, "\n            // ！！！添加副状态切换失败！！！\n");
            }
        }
        File.WriteAllText(scriptPath, ChangeReplaced, new System.Text.UTF8Encoding(true));


        
        // 在Update中加入状态机变化
        var UpdatedSource = File.ReadAllText(scriptPath, Encoding.UTF8);
        var UpdateInjection = BuildCodeUpdateSwitch_SubState(r, sb);
        var UpdatePattern = @"//\s*InsertStateMechineSwitch\b[^\r\n]*\r?\n";
        var UpdateReplaced = Regex.Replace(UpdatedSource, UpdatePattern, m => m.Value + "\n" + UpdateInjection, RegexOptions.Singleline);

        if (UpdateReplaced == UpdatedSource)
        {
            // no clearHP(); found: try to insert reminder in Update method
            var updateMatch = Regex.Match(UpdatedSource, @"void\s+Update\s*\(\s*\)\s*\{", RegexOptions.Singleline);
            if (updateMatch.Success)
            {
                var pos = updateMatch.Index + updateMatch.Length;
                UpdateReplaced = UpdatedSource.Insert(pos, "\n            // ！！！添加UpdateSwitch失败！！！\n");
            }
        }
        File.WriteAllText(scriptPath, UpdateReplaced, new System.Text.UTF8Encoding(true));
        //Debug.Log("CreatEmptyStateMachineGenerator: injection applied (if clearHP() existed).");
        
    }



    /// <summary>
    /// 生成有副状态的状态机的状态开始结束的方法
    /// </summary>
    /// <returns></returns>
    private StringBuilder BuildCodeStateStartOver_SubState( SubStateMechine m , StringBuilder sb)
    {
        foreach (KeyValuePair<string , List<string>> p in m.StateMap)
        {
            string mainC = "";
            m.MainMembersWithComments.TryGetValue(p.Key, out mainC);
            Debug.Log(mainC);
            sb.AppendLine();
            sb.AppendLine();
            sb.AppendLine();
            sb.AppendLine("    //==■==■==■==■==■==■==■主状态：" + mainC + "状态■==■==■==■==■==■==■==");
            List<string> CommentList = new List<string> { };
            for (int i = 0; i < p.Value.Count; i++)
            {

                string subC = "";
                m.SubMembersWithComments.TryGetValue(p.Value[i], out subC);
                Debug.Log(subC);
                CommentList.Add(subC);
            }
            AddCodeStateStartOver_SubState(p.Value.ToArray(), CommentList.ToArray(), m.SubStateMechineName, m.SubFieldName, sb) ;
            sb.AppendLine("    //==■==■==■==■==■==■==■主状态：" + mainC + "状态■==■==■==■==■==■==■==");
            sb.AppendLine();
            sb.AppendLine();
            sb.AppendLine();
            sb.AppendLine();
            sb.AppendLine();
            sb.AppendLine();
            sb.AppendLine();
        }
        return sb;

    }



    /// <summary>
    /// 生成有副状态的状态机的Update中的Switch文
    /// </summary>
    /// <param name="enumName"></param>
    /// <param name="dict"></param>
    /// <param name="fields"></param>
    /// <returns></returns>
    private string BuildCodeUpdateSwitch_SubState( SubStateMechine m , StringBuilder sb)
    {
        sb = new StringBuilder();
        sb.AppendLine();
        sb.AppendLine("            //■■开始判断状态机");
        sb.AppendLine();
        sb.AppendLine("            switch (" + m.MainFieldName + ")");
        sb.AppendLine("            {");
        foreach (KeyValuePair<string , List<string>> kvp in m.StateMap)
        {
            string mainC = "";
            m.MainMembersWithComments.TryGetValue(kvp.Key , out mainC);
            sb.AppendLine("                //●主状态：【" + mainC + "】状态");
            sb.AppendLine("                case " + m.MainStateMechineName + "." + kvp.Key + ":");
            sb.AppendLine("                    // 当处于冰冻 睡眠 致盲 麻痹状态时主状态【" + mainC + "】停运");
            sb.AppendLine("                    if (!isEmptyFrozenDone && !isSleepDone && !isSilence && !isCanNotMoveWhenParalysis) /* TODO【" + mainC + "】状态停运的额外条件 */");
            sb.AppendLine("                    {");
            sb.AppendLine("                        //判断副状态");
            sb.AppendLine("                        switch (" + m.SubFieldName + ")");
            sb.AppendLine("                        {");
            foreach (string sub in kvp.Value)
            {
                string subC = "";
                m.SubMembersWithComments.TryGetValue(sub, out subC);
                sb.AppendLine("                            //【" + subC + "】状态");
                sb.AppendLine("                            case " + m.SubStateMechineName + "." + sub + ":");
                sb.AppendLine("                                " + sub + "Timer -= Time.deltaTime;//【" + subC + "】计时器时间减少");
                sb.AppendLine("                                if (" + sub + "Timer <= 0)         //计时器时间到时间，结束【" + subC + "】状态");
                sb.AppendLine("                                {");
                sb.AppendLine("                                    " + sub + "Over();");
                sb.AppendLine("                                    //TODO添加下一个状态的开始方法");
                sb.AppendLine("                                }");
                sb.AppendLine("                                break;");
            }
            sb.AppendLine("                        }");
            sb.AppendLine("                    }");
            sb.AppendLine("                    break;");
        }
        sb.AppendLine("            }");
        sb.AppendLine("            //■■结束判断状态机");
        return sb.ToString();
    }



    /// <summary>
    /// 生成切换副状态的代码 生成在共通部分
    /// </summary>
    /// <param name="m"></param>
    /// <param name="sb"></param>
    /// <returns></returns>
    private StringBuilder BuildCodeChangeSubState_SubState(SubStateMechine m, StringBuilder sb)
    {
        sb.AppendLine();
        sb.AppendLine("    /// <summary>");
        sb.AppendLine("    /// 切换副状态");
        sb.AppendLine("    /// </summary>");
        sb.AppendLine("    void Change" + m.SubStateMechineName + "(" + m.SubStateMechineName + " targetSubstate)");
        sb.AppendLine("    {");
        sb.AppendLine("        " + m.SubFieldName + " = targetSubstate;");
        sb.AppendLine("        var mainState = GetMainBySub(targetSubstate);");
        sb.AppendLine("        " + m.MainFieldName + " = mainState;");
        sb.AppendLine("    }");
        sb.AppendLine();
        sb.AppendLine();
        sb.AppendLine("    /// <summary>");
        sb.AppendLine("    /// 通过副状态查找主状态");
        sb.AppendLine("    /// </summary>");
        sb.AppendLine("    private " + m.MainStateMechineName + " GetMainBySub(" + m.SubStateMechineName + " SearchSub)");
        sb.AppendLine("    {");
        sb.AppendLine("        foreach (KeyValuePair<" + m.MainStateMechineName + ", " + m.SubStateMechineName + "[]> kvp in StateMap)");
        sb.AppendLine("        {");
        sb.AppendLine("            foreach (" + m.SubStateMechineName + " sub in kvp.Value)");
        sb.AppendLine("            {");
        sb.AppendLine("                if (sub == SearchSub) { return kvp.Key; }");
        sb.AppendLine("            }");
        sb.AppendLine("        }");
        sb.AppendLine("        return 0;");
        sb.AppendLine("    }");
        return sb;

    }



    /// <summary>
    /// 根据主状态添加副状态机的开始和结束代码
    /// </summary>
    /// <param name="enumNameList"></param>
    /// <param name="enumCommentList"></param>
    /// <param name="enumMechineName"></param>
    /// <param name="enumStateName"></param>
    /// <param name="sb"></param>
    /// <returns></returns>
    private StringBuilder AddCodeStateStartOver_SubState(string[] enumNameList, string[] enumCommentList, string enumMechineName, string enumStateName, StringBuilder sb)
    {
        for (int i = 0; i < enumNameList.Length; i++)
        {
            sb.AppendLine();
            sb.AppendLine();
            sb.AppendLine();
            sb.AppendLine("    //=========================" + enumCommentList[i] + "============================");
            sb.AppendLine();
            sb.AppendLine();
            //如果为Idle状态 生成其他状态变化为idle状态时的idle时间
            if (enumNameList[i].IndexOf("Idle", StringComparison.OrdinalIgnoreCase) >= 0
                || enumNameList[i].Equals("Idle", StringComparison.Ordinal))
            {
                sb.AppendLine("    //开始后的冷却时间");
                sb.AppendLine("    static float TIME_" + enumNameList[i].ToUpper() + "_START = 0.0f; //TODO需修改时间");
                for (int j = 0; j < enumNameList.Length; j++)
                {
                    if (i != j)
                    {
                        sb.AppendLine();
                        sb.AppendLine("    //" + enumCommentList[j] + "后的冷却时间");
                        sb.AppendLine("    static float TIME_" + enumNameList[i].ToUpper() + "_" + enumNameList[j].ToUpper() + " = 0.0f; //TODO需修改时间");
                        sb.AppendLine();
                    }
                }
            }
            sb.AppendLine();
            sb.AppendLine();
            sb.AppendLine("    /// <summary>");
            sb.AppendLine("    /// " + enumCommentList[i] + "计时器");
            sb.AppendLine("    /// <summary>");
            sb.AppendLine("    float " + enumNameList[i] + "Timer = 0;");
            sb.AppendLine();
            sb.AppendLine("    /// <summary>");
            sb.AppendLine("    /// " + enumCommentList[i] + "开始");
            sb.AppendLine("    /// <summary>");
            sb.AppendLine("    public void " + enumNameList[i] + "Start(float Timer)");
            sb.AppendLine("    {");
            sb.AppendLine("        " + enumNameList[i] + "Timer = Timer;");
            sb.AppendLine("        ChangeSubState(" + enumMechineName + "." + enumNameList[i] + ");");
            sb.AppendLine("    }");
            sb.AppendLine();
            sb.AppendLine("    /// <summary>");
            sb.AppendLine("    /// " + enumCommentList[i] + "结束");
            sb.AppendLine("    /// <summary>");
            sb.AppendLine("    public void " + enumNameList[i] + "Over()");
            sb.AppendLine("    {");
            sb.AppendLine("        " + enumNameList[i] + "Timer = 0;");
            sb.AppendLine("    }");
            sb.AppendLine();
            sb.AppendLine();
            sb.AppendLine("    //=========================" + enumCommentList[i] + "============================");
            sb.AppendLine();
            sb.AppendLine();
            sb.AppendLine();
        }
        return sb;

    }




    //==========================有副状态的状态机=============================


}



















