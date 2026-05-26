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
}