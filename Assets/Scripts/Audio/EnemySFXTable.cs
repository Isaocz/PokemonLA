using UnityEngine;
using System;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Audio/Enemy SFX Table")]
public class EnemySFXTable : ScriptableObject
{
    [Serializable]
    public class SFXEntry
    {
        public string key;        // enum 名称
        public AudioClip clip;    // 对应音效
    }

    public List<SFXEntry> entries = new List<SFXEntry>();

    private Dictionary<string, AudioClip> dict;

    void OnEnable()
    {
        dict = new Dictionary<string, AudioClip>();
        foreach (var e in entries)
        {
            if (!dict.ContainsKey(e.key))
                dict.Add(e.key, e.clip);
        }
    }

    public AudioClip GetClip(string key)
    {
        if (dict != null && dict.TryGetValue(key, out var clip))
            return clip;

        Debug.LogWarning($"SFX not found: {key}");
        return null;
    }
}