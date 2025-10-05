using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 任务委托NPC列表
/// </summary>
public class MissionNPCList : MonoBehaviour
{

    public static MissionNPCList list;

    private void Awake()
    {
        list = this;
    }

    [System.Serializable]
    public struct MissionNPC
    {
        public int NPCIndex;
        public string NPCChineseTitle;
        public string NPCChineseName;
        public Sprite NPCIcon;
    }


    public List<MissionNPC> NPCList = new List<MissionNPC> { };
}

