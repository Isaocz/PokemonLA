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
    public int maxSimultaneousSameClip = 3;   // 同一音效最多同时播放数量
    public float randomPitchRange = 0.05f;    // 随机音高
    public float randomVolumeRange = 0.1f;    // 随机音量
    public float multiPlayVolumeScale = 0.7f; // 多音效叠加时自动降低音量

    private Dictionary<AudioClip, int> clipPlayCount = new Dictionary<AudioClip, int>();

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
        return null; // 全部占用时不播放
    }

    public void PlaySFX(AudioClip clip, Vector3 position)
    {
        if (clip == null) return;

        // 限制同一音效的最大同时播放数量
        if (!clipPlayCount.ContainsKey(clip))
            clipPlayCount[clip] = 0;

        if (clipPlayCount[clip] >= maxSimultaneousSameClip)
            return;

        AudioSource src = GetFreeSource();
        if (src == null) return;

        StartCoroutine(PlayClipRoutine(src, clip, position));
    }

    private System.Collections.IEnumerator PlayClipRoutine(AudioSource src, AudioClip clip, Vector3 pos)
    {
        clipPlayCount[clip]++;

        src.transform.position = pos;
        src.clip = clip;

        // 随机 Pitch
        src.pitch = 1f + Random.Range(-randomPitchRange, randomPitchRange);

        // 随机 Volume
        float volume = 1f + Random.Range(-randomVolumeRange, randomVolumeRange);

        // 多音效叠加自动降噪
        if (clipPlayCount[clip] > 1)
            volume *= multiPlayVolumeScale;

        src.volume = Mathf.Clamp01(volume);

        src.Play();

        yield return new WaitForSeconds(clip.length);

        clipPlayCount[clip]--;
    }
}