using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioM : MonoBehaviour
{

    public static AudioM GlobalAudioM;
    public AudioMixer Mixer;
    public AudioSource[] BGMList;
    public AudioSource PlayingBGM = null;
    public float NowVolume;

    // Start is called before the first frame update

    private void Awake()
    {
        GlobalAudioM = this;
        NowVolume = 1;
        DontDestroyOnLoad(this);
    }

    float Float2dB(float Float0to1)
    {
        return Mathf.Clamp(Mathf.Clamp(Float0to1, 0.0f, 1.0f) * 80.0f - 80.0f, -80f, 0.0f);
    }


    internal void SetBgmVolume( float Volume )
    {
        Mixer.SetFloat("BGM" , Float2dB(Volume));
        NowVolume = Volume;
    }


    internal void SetSEVolume(float Volume)
    {
        Mixer.SetFloat("SE", Float2dB(Volume));
        NowVolume = Volume;
    }
}
