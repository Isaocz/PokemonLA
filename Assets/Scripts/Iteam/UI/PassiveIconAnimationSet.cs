using System;
using UnityEngine;

/// <summary>Backpack-only animation data. Static item sprites remain the fallback.</summary>
[CreateAssetMenu(menuName = "UI/Passive Icon Animations")]
public sealed class PassiveIconAnimationSet : ScriptableObject
{
    [Serializable]
    public sealed class Entry
    {
        public int itemId;
        [Min(1f)] public float framesPerSecond = 8f;
        public Sprite[] frames;
    }

    public Entry[] entries;
    private static PassiveIconAnimationSet cached;
    private static bool loaded;

    public static Entry Find(int itemId)
    {
        if (!loaded)
        {
            cached = Resources.Load<PassiveIconAnimationSet>("UI/PassiveIconAnimations");
            loaded = true;
        }
        if (cached == null || cached.entries == null) return null;
        foreach (var entry in cached.entries)
            if (entry != null && entry.itemId == itemId && entry.framesPerSecond > 0f
                && entry.frames != null && entry.frames.Length > 0) return entry;
        return null;
    }
}
