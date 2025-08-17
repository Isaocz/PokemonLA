using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundMusic : MonoBehaviour
{

    public AudioSource BGM;
    public static BackGroundMusic StaticBGM;

    public AudioClip MiSiRoTown;
    public AudioClip CaveMusic;
    public AudioClip PC;
    public AudioClip Store;
    public AudioClip Boss;
    public AudioClip BossWin;
    public AudioClip MewPhase1;
    public AudioClip MewPhase2;

    public float transitionSpeed = 1.0f; // 淡入淡出的速度
    private float currentVolume = 0.0f; // 当前音量
    private float targetVolume = 0.0f; // 目标音量
    private float transitionTimer = 0.0f; // 转换计时器
    private bool isFadeInorOut = false; // 是否正在淡入淡出 此时不能再调用


    private void Awake()
    {
        StaticBGM = this;
    }

    void Start()
    {
        BGM.Play();
    }

    public void UnPause()
    {
        BGM.UnPause();
    }

    public void Pause()
    {
        BGM.Pause();
    }

    public void ChangeBGMToTown()
    {
        if (BGM.clip != TownMusic()) { BGM.clip = TownMusic(); BGM.Play(); }
    }

    public void ChangeBGMToPC()
    {
        if (BGM.clip != PC) { BGM.clip = PC; BGM.Play(); }
    }

    public void ChangeBGMToStore()
    {
        if (BGM.clip != Store) { BGM.clip = Store; BGM.Play(); }
    }

    public void ChangeBGMToBoss()
    {
        if (BGM.clip != Boss) { BGM.clip = Boss; BGM.Play(); }
    }

    public void ChangeBGMToBossWin()
    {
        if (BGM.clip != BossWin) { BGM.clip = BossWin; BGM.Play(); }
    }
    public void ChangeBGMToMew(int phase)
    {
        switch (phase)
        {
            case 1: if (BGM.clip != MewPhase1) { BGM.clip = MewPhase1; BGM.Play(); } break;
            case 2: if (BGM.clip != MewPhase2) { BGM.clip = MewPhase2; BGM.Play(); } break;
            default:break;
        }
    
    }
    void Update()
    {
        // 如果目标音量与当前音量不同，则进行渐变
        if (isFadeInorOut)
        {
            // 计算渐变过程中的音量值
            float transitionProgress = Mathf.Clamp01(transitionTimer * transitionSpeed);
            float volume = Mathf.Lerp(currentVolume, targetVolume, transitionProgress);

            //Debug.Log(currentVolume + "+" + targetVolume);
            if (Mathf.Abs(volume - targetVolume) <= 0.01)
            {
                volume = targetVolume;
                isFadeInorOut = false;
            }


            // 更新音频源的音量
            BGM.volume = volume;

            // 更新计时器
            transitionTimer += Time.deltaTime;
        }
        if(BGM.clip == MewPhase2 && BGM.time > 201.1f)
        {
            BGM.time = 7.5f;
        }
    }


    /// <summary>
    /// BGM缓入效果
    /// </summary>
    /// <param name="Target">缓入至Target音量</param>
    /// <param name="Speed">缓入的速度</param>
    public void FadeIn(float Target , float Speed)
    {
        if(!isFadeInorOut)
        {
            isFadeInorOut = true;
            // 设置目标音量为最大音量
            targetVolume = Target;
            currentVolume = BGM.volume;
            transitionTimer = 0.0f;
            transitionSpeed = Speed;
        }
    }

    /// <summary>
    /// BGM缓出效果
    /// </summary>
    /// <param name="Target">缓出至Target音量</param>
    /// <param name="Speed">缓出的速度</param>
    public void FadeOut(float Target, float Speed)
    {
        if (!isFadeInorOut) {
            isFadeInorOut = true;
            // 设置目标音量为静音
            targetVolume = Target;
            currentVolume = BGM.volume;
            transitionTimer = 0.0f;
            transitionSpeed = Speed;
        }
    }



    AudioClip TownMusic()
    {
        AudioClip OutPut = MiSiRoTown;
        switch (MapCreater.StaticMap.NowMapType)
        {
            case MapCreater.MapType.Forest:
                OutPut = MiSiRoTown;
                break;
            case MapCreater.MapType.Cave:
                OutPut = CaveMusic;
                break;
        }
        return OutPut;
    }


}
