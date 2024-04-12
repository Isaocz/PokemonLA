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

    public float transitionDuration = 1.0f; // 持续时间
    private float currentVolume = 0.0f; // 当前音量
    private float targetVolume = 0.0f; // 目标音量
    private float transitionTimer = 0.0f; // 转换计时器


    private void Awake()
    {
        StaticBGM = this;
    }

    void Start()
    {
        BGM.Play();
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
        if (currentVolume != targetVolume)
        {
            // 计算渐变过程中的音量值
            float transitionProgress = Mathf.Clamp01(transitionTimer / transitionDuration);
            float volume = Mathf.Lerp(currentVolume, targetVolume, transitionProgress);

            // 更新音频源的音量
            BGM.volume = volume;

            // 更新计时器
            transitionTimer += Time.deltaTime;
        }
    }
    public void FadeIn()
    {
        // 设置目标音量为最大音量
        targetVolume = 1.0f;

        transitionTimer = 0.0f;
    }

    // 启动缓出效果
    public void FadeOut()
    {
        // 设置目标音量为静音
        targetVolume = 0.0f;

        transitionTimer = 0.0f;
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
