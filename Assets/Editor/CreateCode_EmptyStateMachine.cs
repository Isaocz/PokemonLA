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
    //Ŀ��ű�
    private MonoScript targetScript;
    //�Ƿ񱸷�
    private bool BackupOriginal = true;
    //�Ƿ����и�״̬��״̬�����ɽű�
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











    //==========================��һ״̬��=============================



    /// <summary>
    /// ���ɵ�һ״̬������Ĵ���
    /// </summary>
    /// <param name="scriptPath"></param>
    private void ProcessScript(string scriptPath)
    {
        var source = File.ReadAllText(scriptPath, Encoding.UTF8);


        //��������
        if (BackupOriginal)
        {
            var bak = scriptPath + ".bak";
            File.Copy(scriptPath, bak, true);
            Debug.Log("CreatEmptyStateMachineGenerator: backup created " + bak);
        }


        // ��ȡ�����ռ�
        var nsMatch = Regex.Match(source, @"namespace\s+([A-Za-z0-9_.]+)\s*\{", RegexOptions.Singleline);
        var ns = nsMatch.Success ? nsMatch.Groups[1].Value : null;


        // ��ȡEmpty����
        var classMatch = Regex.Match(source, @"public\s+class\s+([A-Za-z0-9_]+)\s*:\s*Empty");
        if (!classMatch.Success) classMatch = Regex.Match(source, @"class\s+([A-Za-z0-9_]+)\s*:\s*Empty");
        if (!classMatch.Success) throw new Exception("Target script must be a Empty class.");
        var className = classMatch.Groups[1].Value;








        //�ҵ�״̬��ö��
        var enumMatch = Regex.Match(source, @"enum\s+([A-Za-z0-9_]+)\s*\{([^}]*)\}", RegexOptions.Singleline);
        if (!enumMatch.Success) throw new Exception("No public enum found in the script file. Place the main-state enum in the same file.");

        var enumName = enumMatch.Groups[1].Value;
        var membersRaw = enumMatch.Groups[2].Value;


        // ��ÿһ�г�����ȡ ��Ա �� ��βע��
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



        // ��״̬��ʵ��������
        var fieldPattern = new Regex(@"\b" + Regex.Escape(enumName) + @"\s+([A-Za-z0-9_]+)\s*;");
        var f = fieldPattern.Match(source);
        var enumStateName = "";
        if (f.Success) {
            enumStateName = f.Groups[1].Value;
        }
        else { Console.WriteLine("Field not found"); }





        // ���ɴ���
        var sb = new StringBuilder();
        sb = BuildCodeStateStartOver(memberNames, memberNamesComment, enumName, enumStateName, sb);
        var StateSource = File.ReadAllText(scriptPath, Encoding.UTF8);
        var StateInjection = sb.ToString();
        var StatePattern = @"//\s*InsertStateFunction\b[^\r\n]*\r?\n";
        var StateReplaced = Regex.Replace(StateSource, StatePattern, m => m.Value + "\n" + StateInjection, RegexOptions.Singleline);
        //���ʧ��
        if (StateReplaced == StateSource)
        {
            // û�ҵ�����Ŀ��
            var updateMatch = Regex.Match(StateSource, @"void\s+Update\s*\(\s*\)\s*\{", RegexOptions.Singleline);
            if (updateMatch.Success)
            {
                var pos = updateMatch.Index + updateMatch.Length;
                StateReplaced = StateSource.Insert(pos, "\n            // ���������״̬Start&Overʧ�ܣ�����\n");
            }
        }
        File.WriteAllText(scriptPath, StateReplaced, new System.Text.UTF8Encoding(true));



        // ��Update�м���״̬���仯
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
                UpdateReplaced = UpdatedSource.Insert(pos, "\n            // ���������UpdateSwitchʧ�ܣ�����\n");
            }
        }
        File.WriteAllText(scriptPath, UpdateReplaced, new System.Text.UTF8Encoding(true));
        //Debug.Log("CreatEmptyStateMachineGenerator: injection applied (if clearHP() existed).");



    }



    /// <summary>
    /// ���ɵ�һ״̬����״̬��ʼ�����ķ���
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
            //���ΪIdle״̬ ��������״̬�仯Ϊidle״̬ʱ��idleʱ��
            if (enumNameList[i].IndexOf("Idle", StringComparison.OrdinalIgnoreCase) >= 0
                || enumNameList[i].Equals("Idle", StringComparison.Ordinal))
            {
                sb.AppendLine("    //��ʼ�����ȴʱ��");
                sb.AppendLine("    static float TIME_" + enumNameList[i].ToUpper() + "_START = 0.0f; //TODO���޸�ʱ��");
                for (int j = 0; j < enumNameList.Length; j++)
                {
                    if (i != j)
                    {
                        sb.AppendLine();
                        sb.AppendLine("    //" + enumCommentList[j] + "�����ȴʱ��");
                        sb.AppendLine("    static float TIME_" + enumNameList[i].ToUpper() + "_" + enumNameList[j].ToUpper() + " = 0.0f; //TODO���޸�ʱ��");
                        sb.AppendLine();
                    }
                }
            }
            sb.AppendLine();
            sb.AppendLine();
            sb.AppendLine("    /// <summary>");
            sb.AppendLine("    /// " + enumCommentList[i] + "��ʱ��");
            sb.AppendLine("    /// <summary>");
            sb.AppendLine("    float " + enumNameList[i] + "Timer = 0;");
            sb.AppendLine();
            sb.AppendLine("    /// <summary>");
            sb.AppendLine("    /// " + enumCommentList[i] + "��ʼ");
            sb.AppendLine("    /// <summary>");
            sb.AppendLine("    public void " + enumNameList[i] + "Start(float Timer)");
            sb.AppendLine("    {");
            sb.AppendLine("        " + enumNameList[i] + "Timer = Timer;");
            sb.AppendLine("        " + enumStateName + " = " + enumMechineName + "." + enumNameList[i] + ";");
            sb.AppendLine("    }");
            sb.AppendLine();
            sb.AppendLine("    /// <summary>");
            sb.AppendLine("    /// " + enumCommentList[i] + "����");
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
    /// ���ɵ�һ״̬����Update�е�Switch��
    /// </summary>
    /// <param name="enumName"></param>
    /// <param name="dict"></param>
    /// <param name="fields"></param>
    /// <returns></returns>
    private string BuildCodeUpdateSwitch(string[] enumNameList , string[] enumCommentList , string enumMechineName, string enumStateName, StringBuilder sb)
    {
        sb = new StringBuilder();
        sb.AppendLine();
        sb.AppendLine("            //������ʼ�ж�״̬�� �����ڱ��� ˯�� ��ä ���״̬ʱ״̬��ͣ��");
        sb.AppendLine("            if (!isEmptyFrozenDone && !isSleepDone && !isSilence && !isCanNotMoveWhenParalysis) /* TODO״̬��ͣ�˵Ķ������� */");
        sb.AppendLine("            {");
        sb.AppendLine("                switch (" + enumStateName + ")");
        sb.AppendLine("                {");
        for (int i = 0; i < enumNameList.Length; i++)
        {
            sb.AppendLine("                    //" + enumCommentList[i] + "״̬");
            sb.AppendLine("                    case " + enumMechineName + "." + enumNameList[i] + ":");
            sb.AppendLine("                        " + enumNameList[i] + "Timer -= Time.deltaTime;//" + enumCommentList[i] + "��ʱ��ʱ�����");
            sb.AppendLine("                        if ( " + enumNameList[i] + "Timer <= 0 )         //��ʱ��ʱ�䵽ʱ�䣬����" + enumCommentList[i] + "״̬");
            sb.AppendLine("                        {");
            sb.AppendLine("                            " + enumNameList[i] + "Over();");
            sb.AppendLine("                            //TODO�����һ��״̬�Ŀ�ʼ����");
            sb.AppendLine("                        }");
            sb.AppendLine("                        break;");
        }
        sb.AppendLine("                }");
        sb.AppendLine("            }");
        sb.AppendLine("            //���������ж�״̬��");
        sb.AppendLine();
        return sb.ToString();
    }




    //==========================��һ״̬��=============================


















    //==========================�и�״̬��״̬��=============================


    /// <summary>
    /// ��״̬���ṹ
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
    /// �����и�״̬��״̬������Ĵ���
    /// </summary>
    /// <param name="scriptPath"></param>
    private void ProcessScript_SubState(string scriptPath)
    {
        var source = File.ReadAllText(scriptPath, Encoding.UTF8);


        //��������
        if (BackupOriginal)
        {
            var bak = scriptPath + ".bak";
            File.Copy(scriptPath, bak, true);
            Debug.Log("CreatEmptyStateMachineGenerator: backup created " + bak);
        }


        // ��ȡ�����ռ�
        var nsMatch = Regex.Match(source, @"namespace\s+([A-Za-z0-9_.]+)\s*\{", RegexOptions.Singleline);
        var ns = nsMatch.Success ? nsMatch.Groups[1].Value : null;


        // ��ȡEmpty����
        var classMatch = Regex.Match(source, @"public\s+class\s+([A-Za-z0-9_]+)\s*:\s*Empty");
        if (!classMatch.Success) classMatch = Regex.Match(source, @"class\s+([A-Za-z0-9_]+)\s*:\s*Empty");
        if (!classMatch.Success) throw new Exception("Target script must be a Empty class.");
        var className = classMatch.Groups[1].Value;




        var r = new SubStateMechine();

        // ��ȡ enum �鼰��Ա��֧�� /// ע�� / ��β // ע�ͣ�
        // ƥ�� enum ���壨public �������Σ������������뻨����������
        string enumPattern = @"enum\s+([A-Za-z0-9_]+)\s*\{([^}]*)\}";
        foreach (Match m in Regex.Matches(source, enumPattern, RegexOptions.Singleline))
        {

            var enumName = m.Groups[1].Value;
            var membersRaw = m.Groups[2].Value;

            // ��ÿһ�г�����ȡ ��Ա �� ��βע��
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




            // ���������ж��� MainState ���� SubState
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
                // ���޷�ƾ�����жϣ��ɻ��ڳ�Ա�����ļ������ľ����������������δռλ��
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

        // �ҵ���Ӧ���ֶ�������Pattern: EnumType fieldName;
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

        // ���� StateMap ��ʼ����
        // �ҵ� "Dictionary<MainState, SubState[]>" �� "StateMap = new()" ��ʼ���鲢��ȡ��ֵ��
        // һ�����ɵ�ģʽ���ڵȺź�Ļ��������� { MainState.Frozen, new[] { SubState.X, ... } }
        var mapPattern = @"StateMap\s*=\s*new\s*\(\s*\)\s*\s*\{\s*(?<body>.*?)\s*\}\s*;";
        var mapMatch = Regex.Match(source, mapPattern, RegexOptions.Singleline);
        if (!mapMatch.Success)
        {
            // Ҳ����ûʹ�� new() ������������ʽ new Dictionary<...> { ... }
            mapPattern = @"StateMap\s*=\s*new\s+[^\{]+\{\s*(?<body>.*?)\s*\}\s*;";
            mapMatch = Regex.Match(source, mapPattern, RegexOptions.Singleline);
        }

        if (mapMatch.Success)
        {
            var body = mapMatch.Groups["body"].Value;
            // ƥ��ÿһ�� { MainState.Frozen, new[] { SubState.Idle_Frozen, SubState.Walk_Frozen } }
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
        //test���״̬��
        Debug.Log($"MainEnum: {r.MainStateMechineName}  Field: {r.MainFieldName}");
        foreach (var kv in r.MainMembersWithComments) Debug.Log($"  {kv.Key} -> {kv.Value}");
        Debug.Log($"SubEnum: {r.SubStateMechineName}  Field: {r.SubFieldName}");
        foreach (var kv in r.SubMembersWithComments) Debug.Log($"  {kv.Key} -> {kv.Value}");
        Debug.Log("StateMap:");
        foreach (var kv in r.StateMap) Debug.Log($"  {kv.Key} -> [{string.Join(", ", kv.Value)}]");
        **/









        
        // ����StatrOver����
        var sb = new StringBuilder();
        sb = BuildCodeStateStartOver_SubState(r, sb);
        var StateSource = File.ReadAllText(scriptPath, Encoding.UTF8);
        var StateInjection = sb.ToString();
        var StatePattern = @"//\s*InsertStateFunction\b[^\r\n]*\r?\n";
        var StateReplaced = Regex.Replace(StateSource, StatePattern, m => m.Value + "\n" + StateInjection, RegexOptions.Singleline);
        //���ʧ��
        if (StateReplaced == StateSource)
        {
            // û�ҵ�����Ŀ��
            var updateMatch = Regex.Match(StateSource, @"void\s+Update\s*\(\s*\)\s*\{", RegexOptions.Singleline);
            if (updateMatch.Success)
            {
                var pos = updateMatch.Index + updateMatch.Length;
                StateReplaced = StateSource.Insert(pos, "\n            // ���������״̬Start&Overʧ�ܣ�����\n");
            }
        }
        File.WriteAllText(scriptPath, StateReplaced, new System.Text.UTF8Encoding(true));


        // ���ɸ�״̬�л�����
        var sb2 = new StringBuilder();
        sb2 = BuildCodeChangeSubState_SubState(r, sb2);
        var ChangeSource = File.ReadAllText(scriptPath, Encoding.UTF8);
        var ChangeInjection = sb2.ToString();
        var ChangePattern = @"//\s*InsertSubStateChange\b[^\r\n]*\r?\n";
        var ChangeReplaced = Regex.Replace(ChangeSource, ChangePattern, m => m.Value + "\n" + ChangeInjection, RegexOptions.Singleline);
        //���ʧ��
        if (ChangeReplaced == ChangeSource)
        {
            // û�ҵ�����Ŀ��
            var updateMatch = Regex.Match(ChangeSource, @"void\s+Update\s*\(\s*\)\s*\{", RegexOptions.Singleline);
            if (updateMatch.Success)
            {
                var pos = updateMatch.Index + updateMatch.Length;
                ChangeReplaced = ChangeSource.Insert(pos, "\n            // ��������Ӹ�״̬�л�ʧ�ܣ�����\n");
            }
        }
        File.WriteAllText(scriptPath, ChangeReplaced, new System.Text.UTF8Encoding(true));


        
        // ��Update�м���״̬���仯
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
                UpdateReplaced = UpdatedSource.Insert(pos, "\n            // ���������UpdateSwitchʧ�ܣ�����\n");
            }
        }
        File.WriteAllText(scriptPath, UpdateReplaced, new System.Text.UTF8Encoding(true));
        //Debug.Log("CreatEmptyStateMachineGenerator: injection applied (if clearHP() existed).");
        
    }



    /// <summary>
    /// �����и�״̬��״̬����״̬��ʼ�����ķ���
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
            sb.AppendLine("    //==��==��==��==��==��==��==����״̬��" + mainC + "״̬��==��==��==��==��==��==��==");
            List<string> CommentList = new List<string> { };
            for (int i = 0; i < p.Value.Count; i++)
            {

                string subC = "";
                m.SubMembersWithComments.TryGetValue(p.Value[i], out subC);
                Debug.Log(subC);
                CommentList.Add(subC);
            }
            AddCodeStateStartOver_SubState(p.Value.ToArray(), CommentList.ToArray(), m.SubStateMechineName, m.SubFieldName, sb) ;
            sb.AppendLine("    //==��==��==��==��==��==��==����״̬��" + mainC + "״̬��==��==��==��==��==��==��==");
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
    /// �����и�״̬��״̬����Update�е�Switch��
    /// </summary>
    /// <param name="enumName"></param>
    /// <param name="dict"></param>
    /// <param name="fields"></param>
    /// <returns></returns>
    private string BuildCodeUpdateSwitch_SubState( SubStateMechine m , StringBuilder sb)
    {
        sb = new StringBuilder();
        sb.AppendLine();
        sb.AppendLine("            //������ʼ�ж�״̬��");
        sb.AppendLine();
        sb.AppendLine("            switch (" + m.MainFieldName + ")");
        sb.AppendLine("            {");
        foreach (KeyValuePair<string , List<string>> kvp in m.StateMap)
        {
            string mainC = "";
            m.MainMembersWithComments.TryGetValue(kvp.Key , out mainC);
            sb.AppendLine("                //����״̬����" + mainC + "��״̬");
            sb.AppendLine("                case " + m.MainStateMechineName + "." + kvp.Key + ":");
            sb.AppendLine("                    // �����ڱ��� ˯�� ��ä ���״̬ʱ��״̬��" + mainC + "��ͣ��");
            sb.AppendLine("                    if (!isEmptyFrozenDone && !isSleepDone && !isSilence && !isCanNotMoveWhenParalysis) /* TODO��" + mainC + "��״̬ͣ�˵Ķ������� */");
            sb.AppendLine("                    {");
            sb.AppendLine("                        //�жϸ�״̬");
            sb.AppendLine("                        switch (" + m.SubFieldName + ")");
            sb.AppendLine("                        {");
            foreach (string sub in kvp.Value)
            {
                string subC = "";
                m.SubMembersWithComments.TryGetValue(sub, out subC);
                sb.AppendLine("                            //��" + subC + "��״̬");
                sb.AppendLine("                            case " + m.SubStateMechineName + "." + sub + ":");
                sb.AppendLine("                                " + sub + "Timer -= Time.deltaTime;//��" + subC + "����ʱ��ʱ�����");
                sb.AppendLine("                                if (" + sub + "Timer <= 0)         //��ʱ��ʱ�䵽ʱ�䣬������" + subC + "��״̬");
                sb.AppendLine("                                {");
                sb.AppendLine("                                    " + sub + "Over();");
                sb.AppendLine("                                    //TODO�����һ��״̬�Ŀ�ʼ����");
                sb.AppendLine("                                }");
                sb.AppendLine("                                break;");
            }
            sb.AppendLine("                        }");
            sb.AppendLine("                    }");
            sb.AppendLine("                    break;");
        }
        sb.AppendLine("            }");
        sb.AppendLine("            //���������ж�״̬��");
        return sb.ToString();
    }



    /// <summary>
    /// �����л���״̬�Ĵ��� �����ڹ�ͨ����
    /// </summary>
    /// <param name="m"></param>
    /// <param name="sb"></param>
    /// <returns></returns>
    private StringBuilder BuildCodeChangeSubState_SubState(SubStateMechine m, StringBuilder sb)
    {
        sb.AppendLine();
        sb.AppendLine("    /// <summary>");
        sb.AppendLine("    /// �л���״̬");
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
        sb.AppendLine("    /// ͨ����״̬������״̬");
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
    /// ������״̬��Ӹ�״̬���Ŀ�ʼ�ͽ�������
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
            //���ΪIdle״̬ ��������״̬�仯Ϊidle״̬ʱ��idleʱ��
            if (enumNameList[i].IndexOf("Idle", StringComparison.OrdinalIgnoreCase) >= 0
                || enumNameList[i].Equals("Idle", StringComparison.Ordinal))
            {
                sb.AppendLine("    //��ʼ�����ȴʱ��");
                sb.AppendLine("    static float TIME_" + enumNameList[i].ToUpper() + "_START = 0.0f; //TODO���޸�ʱ��");
                for (int j = 0; j < enumNameList.Length; j++)
                {
                    if (i != j)
                    {
                        sb.AppendLine();
                        sb.AppendLine("    //" + enumCommentList[j] + "�����ȴʱ��");
                        sb.AppendLine("    static float TIME_" + enumNameList[i].ToUpper() + "_" + enumNameList[j].ToUpper() + " = 0.0f; //TODO���޸�ʱ��");
                        sb.AppendLine();
                    }
                }
            }
            sb.AppendLine();
            sb.AppendLine();
            sb.AppendLine("    /// <summary>");
            sb.AppendLine("    /// " + enumCommentList[i] + "��ʱ��");
            sb.AppendLine("    /// <summary>");
            sb.AppendLine("    float " + enumNameList[i] + "Timer = 0;");
            sb.AppendLine();
            sb.AppendLine("    /// <summary>");
            sb.AppendLine("    /// " + enumCommentList[i] + "��ʼ");
            sb.AppendLine("    /// <summary>");
            sb.AppendLine("    public void " + enumNameList[i] + "Start(float Timer)");
            sb.AppendLine("    {");
            sb.AppendLine("        " + enumNameList[i] + "Timer = Timer;");
            sb.AppendLine("        ChangeSubState(" + enumMechineName + "." + enumNameList[i] + ");");
            sb.AppendLine("    }");
            sb.AppendLine();
            sb.AppendLine("    /// <summary>");
            sb.AppendLine("    /// " + enumCommentList[i] + "����");
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




    //==========================�и�״̬��״̬��=============================


}



















