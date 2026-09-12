using UnityEngine;
using UnityEngine.Audio;
using System.Collections;

public class LoopingSEAudioPlayer : MonoBehaviour
{
    [Header("Audio Mixer Output")]
    public AudioMixerGroup outputGroup;   // ← 在 Inspector 指定


    private AudioSource source;
    private Coroutine fadeRoutine;

    void Awake()
    {
        source = gameObject.AddComponent<AudioSource>();
        source.loop = true;
        source.playOnAwake = false;
        source.spatialBlend = 0.0f; // 3D 音效

        if (outputGroup != null)
            source.outputAudioMixerGroup = outputGroup;   // ← 指定输出组
    }

    public void PlayLoop(AudioClip clip, float volume = 1f)
    {
        if (clip == null) return;

        if (fadeRoutine != null)
            StopCoroutine(fadeRoutine);

        source.clip = clip;
        source.volume = volume;
        source.Play();
    }

    public void StopLoop(float fadeOutTime = 0.5f)
    {
        if (!source.isPlaying) return;

        if (fadeRoutine != null)
            StopCoroutine(fadeRoutine);

        fadeRoutine = StartCoroutine(FadeOutRoutine(fadeOutTime));
    }

    private IEnumerator FadeOutRoutine(float time)
    {
        float startVolume = source.volume;
        float t = 0f;

        while (t < time)
        {
            t += Time.deltaTime;
            source.volume = Mathf.Lerp(startVolume, 0f, t / time);
            yield return null;
        }

        source.Stop();
        source.volume = startVolume; // 重置音量
    }




    // 在销毁前提前缓存
    private AudioClip cachedClip;
    private float cachedVolume;
    private bool cachedIsPlaying;


    /**
    // -----------------------------
    // 销毁前缓存音效状态
    // -----------------------------
    private void OnDisable()
    {
        if (source != null)
        {
            cachedClip = source.clip;
            cachedVolume = source.volume;
            cachedIsPlaying = source.isPlaying;
        }
    }

    // -----------------------------
    // 对象销毁时独立音效并淡出
    // -----------------------------
    private void OnDestroy()
    {
        Debug.Log("Destory");
        if (source == null)
            return;

        // 1. 创建新的临时对象
        GameObject tempObj = new GameObject("DetachedLoopSE");
        DontDestroyOnLoad(tempObj); // 可选：跨场景淡出

        // 2. 复制 AudioSource
        AudioSource tempSource = tempObj.AddComponent<AudioSource>();
        tempSource.clip = source.clip;
        tempSource.volume = source.volume;
        tempSource.loop = false; // 不再循环
        tempSource.spatialBlend = source.spatialBlend;
        tempSource.outputAudioMixerGroup = source.outputAudioMixerGroup;

        tempSource.Play();

        // 3. 在新对象上执行淡出
        tempObj.AddComponent<DetachedFadeOut>().BeginFade(tempSource, 0.5f);
    }
}


// --------------------------------------
// 独立淡出组件（自动销毁）
// --------------------------------------
public class DetachedFadeOut : MonoBehaviour
{
    public void BeginFade(AudioSource src, float fadeTime)
    {
        StartCoroutine(FadeOutAndDestroy(src, fadeTime));
    }

    private IEnumerator FadeOutAndDestroy(AudioSource src, float fadeTime)
    {
        float startVolume = src.volume;
        float t = 0f;

        while (t < fadeTime)
        {
            t += Time.deltaTime;
            src.volume = Mathf.Lerp(startVolume, 0f, t / fadeTime);
            yield return null;
        }

        src.Stop();
        Destroy(gameObject); // ★ 自动销毁临时对象
    }

    **/
}