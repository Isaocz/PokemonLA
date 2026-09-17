using UnityEngine;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Source Pool")]
    public int poolSize = 10;
    public AudioSource audioSourcePrefab;
    private List<AudioSource> pool;

    [Header("SFX Settings")]
    public int maxSimultaneousSameClip = 3;   // 同一音效最多同时开始数量（已修改含义）
    public float sameClipInterval = 0.1f;     // 新增：限制时间窗口（秒）
    public float randomPitchRange = 0.05f;
    public float randomVolumeRange = 0.1f;
    public float multiPlayVolumeScale = 0.7f;

    //==== 修改开始 ==== 新增：记录每个音效的播放时间戳
    private Dictionary<AudioClip, Queue<float>> clipPlayTimestamps = new Dictionary<AudioClip, Queue<float>>();
    //==== 修改结束 ====

    private Dictionary<AudioClip, int> clipPlayCount = new Dictionary<AudioClip, int>();

    //============================共通基本音效==================================
    public EnemyAudioPlayer CommonBasicSFXPlayer;

    public enum CommonBasicSFXList
    {
        NULL,
        Damage,
        HealUp,
        LevelUp,
        Explosion,
        ExplosionNoTail,
        LightningStrike,
        TPStart,
        TPOver,
        GetNormalItem,
        GetSpeaceItem,
        GetBerryItem,
        GetPassiveItem,
        GetNewSkill,
        GetBabyItem,
        BallOpen,
        ItemDrop,
        EmptyDamage,
        EmptyCTDamage,
        EmptyDie,
        
    }

    public enum CommonUISFXList
    {
        ButtonClick,
        GetScore,
        GetBigScore,
        Line,
        ScoreLoop,
        ScorePunish,
        ScorePanelFlower,
    }
    //============================共通基本音效==================================

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        InitPool();
    }

    void InitPool()
    {
        pool = new List<AudioSource>();
        for (int i = 0; i < poolSize; i++)
        {
            AudioSource src = Instantiate(audioSourcePrefab, transform);
            src.playOnAwake = false;
            pool.Add(src);
        }
    }

    AudioSource GetFreeSource()
    {
        foreach (var src in pool)
        {
            if (!src.isPlaying)
                return src;
        }
        return null;
    }

    /// <summary>
    /// 播放音效 ， 
    /// </summary>
    /// <param name="clip"></param>
    /// <param name="position"></param>
    /// <param name="isPosFree">不受位置限制</param>
    // Opt-in burst coalescing. Other callers retain the existing pool concurrency policy.
    private readonly Dictionary<AudioClip, float> groupedUntil = new Dictionary<AudioClip, float>();
    public void PlaySFXGrouped(AudioClip clip, Vector3 position, float interval = 0.08f, bool positionFree = false, float volumeScale = 1f)
    {
        if (clip == null || (!positionFree && !_mTool.IsInCameraView(position))) return;
        float now = Time.unscaledTime;
        if (groupedUntil.TryGetValue(clip, out float until) && now < until) return;
        groupedUntil[clip] = now + Mathf.Max(0.01f, interval);
        PlaySFX(clip, position, positionFree, 1f, volumeScale);
    }
    public void PlaySFX(AudioClip clip, Vector3 position , bool isPosFree = false , float Pitch = 1.0f, float volumeScale = 1f)
    {
        //Debug.Log(clip);
        //Debug.Log(position);
        //判空片段
        if (clip == null) return;

        //判断点是否在摄像机内
        if (!isPosFree) {
            if (!_mTool.IsInCameraView(position)) return; }

        //Debug.Log(clip);
        //Debug.Log(position);

        //======== 使用时间戳限制，而不是同时播放数量 ====
        if (!clipPlayTimestamps.ContainsKey(clip))
            clipPlayTimestamps[clip] = new Queue<float>();

        float now = Time.unscaledTime;

        // 移除过期的时间戳
        while (clipPlayTimestamps[clip].Count > 0 &&
               now - clipPlayTimestamps[clip].Peek() > sameClipInterval)
        {
            clipPlayTimestamps[clip].Dequeue();
        }

        // 如果时间窗口内触发次数过多 → 不播放
        if (clipPlayTimestamps[clip].Count >= maxSimultaneousSameClip)
            return;

        // 记录新的播放时间
        clipPlayTimestamps[clip].Enqueue(now);

        AudioSource src = GetFreeSource();
        if (src == null) return;

        StartCoroutine(PlayClipRoutine(src, clip, position , Pitch, volumeScale));
    }

    private System.Collections.IEnumerator PlayClipRoutine(AudioSource src, AudioClip clip, Vector3 pos , float Pitch, float volumeScale)
    {
        if (!clipPlayCount.ContainsKey(clip))
            clipPlayCount[clip] = 0;

        clipPlayCount[clip]++;

        src.transform.position = pos;
        src.clip = clip;

        src.pitch = Pitch;

        float volume = 1f + Random.Range(-randomVolumeRange, randomVolumeRange);

        if (clipPlayCount[clip] > 1)
            volume *= multiPlayVolumeScale;

        // Preserve each emitter original loudness when routing through the pool.
        // Previous: src.volume = Mathf.Clamp01(volume);
        src.volume = Mathf.Clamp01(volume * volumeScale);

        src.Play();

        yield return new WaitForSeconds(clip.length);

        clipPlayCount[clip]--;
    }
}
