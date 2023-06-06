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
}
