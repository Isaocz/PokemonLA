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

    public float transitionSpeed = 1.0f; // ���뵭�����ٶ�
    private float currentVolume = 0.0f; // ��ǰ����
    private float targetVolume = 0.0f; // Ŀ������
    private float transitionTimer = 0.0f; // ת����ʱ��
    private bool isFadeInorOut = false; // �Ƿ����ڵ��뵭�� ��ʱ�����ٵ���


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
        // ���Ŀ�������뵱ǰ������ͬ������н���
        if (isFadeInorOut)
        {
            // ���㽥������е�����ֵ
            float transitionProgress = Mathf.Clamp01(transitionTimer * transitionSpeed);
            float volume = Mathf.Lerp(currentVolume, targetVolume, transitionProgress);

            //Debug.Log(currentVolume + "+" + targetVolume);
            if (Mathf.Abs(volume - targetVolume) <= 0.01)
            {
                volume = targetVolume;
                isFadeInorOut = false;
            }


            // ������ƵԴ������
            BGM.volume = volume;

            // ���¼�ʱ��
            transitionTimer += Time.deltaTime;
        }
        if(BGM.clip == MewPhase2 && BGM.time > 201.1f)
        {
            BGM.time = 7.5f;
        }
    }


    /// <summary>
    /// BGM����Ч��
    /// </summary>
    /// <param name="Target">������Target����</param>
    /// <param name="Speed">������ٶ�</param>
    public void FadeIn(float Target , float Speed)
    {
        if(!isFadeInorOut)
        {
            isFadeInorOut = true;
            // ����Ŀ������Ϊ�������
            targetVolume = Target;
            currentVolume = BGM.volume;
            transitionTimer = 0.0f;
            transitionSpeed = Speed;
        }
    }

    /// <summary>
    /// BGM����Ч��
    /// </summary>
    /// <param name="Target">������Target����</param>
    /// <param name="Speed">�������ٶ�</param>
    public void FadeOut(float Target, float Speed)
    {
        if (!isFadeInorOut) {
            isFadeInorOut = true;
            // ����Ŀ������Ϊ����
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
