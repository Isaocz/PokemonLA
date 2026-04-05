using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LogToFile : MonoBehaviour
{
    private static readonly HashSet<string> loggedMessages = new HashSet<string>();
    private static string logFilePath;

    private void Awake()
    {
        string folder;

#if UNITY_STANDALONE_WIN || UNITY_STANDALONE_LINUX
        // PC 平台：写入游戏目录
        string gameRoot = Directory.GetParent(Application.dataPath).FullName;
        folder = Path.Combine(gameRoot, "Logs");
#else
        // 其他平台：写入 persistentDataPath
        folder = Path.Combine(Application.persistentDataPath, "Logs");
#endif

        if (!Directory.Exists(folder))
            Directory.CreateDirectory(folder);

        logFilePath = Path.Combine(folder, "error.log");

        Application.logMessageReceived += OnLogMessage;
    }

    private void OnDestroy()
    {
        Application.logMessageReceived -= OnLogMessage;
    }

    private void OnLogMessage(string condition, string stackTrace, LogType type)
    {
        if (type != LogType.Error && type != LogType.Exception && type != LogType.Assert)
            return;

        string fullMessage = $"{condition}\n{stackTrace}";

        if (loggedMessages.Contains(fullMessage))
            return;

        loggedMessages.Add(fullMessage);

        File.AppendAllText(logFilePath,
            $"[{System.DateTime.Now}] {type}\n{fullMessage}\n\n");
    }
}