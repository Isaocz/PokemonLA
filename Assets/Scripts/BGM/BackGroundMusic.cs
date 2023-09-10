using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundMusic : MonoBehaviour
{

    public AudioSource BGM;
    public static BackGroundMusic StaticBGM;

    public AudioClip MiSiRoTown;
    public AudioClip PC;
    public AudioClip Store;
    public AudioClip Boss;
    public AudioClip BossWin;
    public AudioClip FightMew;
    public AudioClip Kronos;

    public float transitionDuration = 1.0f; // ����ʱ��
    private float currentVolume = 0.0f; // ��ǰ����
    private float targetVolume = 0.0f; // Ŀ������
    private float transitionTimer = 0.0f; // ת����ʱ��


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
        if (BGM.clip != MiSiRoTown) { BGM.clip = MiSiRoTown; BGM.Play(); }
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
    public void ChangeBGMToMew()
    {
        if (BGM.clip != FightMew) { BGM.clip =  FightMew; BGM.Play();}
    }
    public void ChangeBGMINSIST()
    {
        if (BGM.clip != Kronos)
        {
            BGM.clip = Kronos;
            BGM.Play();
        }
    }
    void Update()
    {
        // ���Ŀ�������뵱ǰ������ͬ������н���
        if (currentVolume != targetVolume)
        {
            // ���㽥������е�����ֵ
            float transitionProgress = Mathf.Clamp01(transitionTimer / transitionDuration);
            float volume = Mathf.Lerp(currentVolume, targetVolume, transitionProgress);

            // ������ƵԴ������
            BGM.volume = volume;

            // ���¼�ʱ��
            transitionTimer += Time.deltaTime;
        }
    }
    public void FadeIn()
    {
        // ����Ŀ������Ϊ�������
        targetVolume = 1.0f;

        transitionTimer = 0.0f;
    }

    // ��������Ч��
    public void FadeOut()
    {
        // ����Ŀ������Ϊ����
        targetVolume = 0.0f;

        transitionTimer = 0.0f;
    }
}
