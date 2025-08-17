using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HaunterSludgeBombMist : MonoBehaviour
{
    public float MistTime;
    

    public void SetTime(float mistTime)
    {
        MistTime = mistTime;
        SetMistTime(MistTime);
    }



    // Start is called before the first frame update
    void Start()
    {
        //SetMistTime(MistTime);
    }


    void SetMistTime(float time)
    {
        ParticleSystem ps1 = transform.GetComponent<ParticleSystem>();
        ParticleSystem ps2 = transform.GetComponent<ParticleSystem>();
        ParticleSystem ps3 = transform.GetComponent<ParticleSystem>();
        var m1 = transform.GetComponent<ParticleSystem>().main;
        var m2 = transform.GetChild(0).GetComponent<ParticleSystem>().main;
        var m3 = transform.GetChild(1).GetComponent<ParticleSystem>().main;
        m1.startLifetime = time;
        m2.duration = time - 1.0f;
        m3.duration = 5.0f * (time - 1.0f);
        ps1.Play();
        ps2.Play();
        ps3.Play();
    }
}
