using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassyTerrain : MonoBehaviour
{

    public enum TerrainType
    {
        青草场地,
        精神场地,
        电气场地,
        薄雾场地,
    }

    public bool isSuperMode;
    public TerrainType TType;


    float Timer;


    /// <summary>
    /// 开始音效枚举
    /// </summary>
    public enum StartSEEnum
    {
        ElectricTerrainStart,
        GrassyTerrainStart,
        MistyTerrainStart,
        PsychicTerrainStart
    }

    /// <summary>
    /// 单次音效播放器
    /// </summary>
    public EnemyAudioPlayer AudioPlayer;

    /// <summary>
    /// 循环音效播放器
    /// </summary>
    public LoopingSEAudioPlayer LoopAudioPlayer;

    /// <summary>
    /// 循环音效
    /// </summary>
    public AudioClip loopClip;


    private void Start()
    {
        //开启音效
        if (LoopAudioPlayer != null) { transform.GetComponent<LoopingSEAudioPlayer>(); }
        if (LoopAudioPlayer != null && loopClip != null)
        {
            LoopAudioPlayer.PlayLoop(loopClip, 1f);
        }
        if (AudioPlayer != null)
        {
            switch (TType)
            {
                case TerrainType.青草场地: AudioPlayer.Play(StartSEEnum.GrassyTerrainStart, transform.position); break;
                case TerrainType.精神场地: AudioPlayer.Play(StartSEEnum.PsychicTerrainStart, transform.position); break;
                case TerrainType.电气场地: AudioPlayer.Play(StartSEEnum.ElectricTerrainStart, transform.position); break;
                case TerrainType.薄雾场地: AudioPlayer.Play(StartSEEnum.MistyTerrainStart, transform.position); break;
            }
        }
    }


    private void Update()
    {
        Timer += Time.deltaTime;
        if (Timer >= 85)
        {
            Destroy(gameObject);

        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" || other.tag == "Empty")
        {
            Pokemon p = other.GetComponent<Pokemon>();
            Substitute s = other.GetComponent<Substitute>();
            if (p != null)
            {
                switch (TType)
                {
                    case TerrainType.青草场地:
                        p.GrassyTerrainCount += 1;
                        if (isSuperMode) { p.SuperGrassyTerrainCount += 1; }
                        break;
                    case TerrainType.精神场地:
                        p.PsychicTerrainCount += 1;
                        if (isSuperMode) { p.SuperPsychicTerrainCount += 1; }
                        break;
                    case TerrainType.电气场地:
                        p.ElectricTerrainCount += 1;
                        if (isSuperMode) { p.SuperElectricTerrainCount += 1; }
                        break;
                    case TerrainType.薄雾场地:
                        p.MistyTerrainCount += 1;
                        if (isSuperMode) { p.SuperMistyTerrainCount += 1; }
                        break;
                }
            }
            if (s != null)
            {
                switch (TType)
                {
                    case TerrainType.青草场地:
                        s.GrassyTerrainCount += 1;
                        if (isSuperMode) { s.SuperGrassyTerrainCount += 1; }
                        break;
                    case TerrainType.精神场地:
                        s.PsychicTerrainCount += 1;
                        if (isSuperMode) { s.SuperPsychicTerrainCount += 1; }
                        break;
                    case TerrainType.电气场地:
                        s.ElectricTerrainCount += 1;
                        if (isSuperMode) { s.SuperElectricTerrainCount += 1; }
                        break;
                    case TerrainType.薄雾场地:
                        s.MistyTerrainCount += 1;
                        if (isSuperMode) { s.SuperMistyTerrainCount += 1; }
                        break;
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player" || other.tag == "Empty")
        {
            Pokemon p = other.GetComponent<Pokemon>();
            Substitute s = other.GetComponent<Substitute>();
            if (p != null)
            {
                switch (TType)
                {
                    case TerrainType.青草场地:
                        p.GrassyTerrainCount -= 1;
                        if (isSuperMode) { p.SuperGrassyTerrainCount += 1; }
                        break;
                    case TerrainType.精神场地:
                        p.PsychicTerrainCount -= 1;
                        if (isSuperMode) { p.SuperPsychicTerrainCount += 1; }
                        break;
                    case TerrainType.电气场地:
                        p.ElectricTerrainCount -= 1;
                        if (isSuperMode) { p.SuperElectricTerrainCount += 1; }
                        break;
                    case TerrainType.薄雾场地:
                        p.MistyTerrainCount -= 1;
                        if (isSuperMode) { p.SuperMistyTerrainCount += 1; }
                        break;
                }
            }
            if (s != null)
            {
                switch (TType)
                {
                    case TerrainType.青草场地:
                        s.GrassyTerrainCount -= 1;
                        if (isSuperMode) { s.SuperGrassyTerrainCount += 1; }
                        break;
                    case TerrainType.精神场地:
                        s.PsychicTerrainCount -= 1;
                        if (isSuperMode) { s.SuperPsychicTerrainCount += 1; }
                        break;
                    case TerrainType.电气场地:
                        s.ElectricTerrainCount -= 1;
                        if (isSuperMode) { s.SuperElectricTerrainCount += 1; }
                        break;
                    case TerrainType.薄雾场地:
                        s.MistyTerrainCount -= 1;
                        if (isSuperMode) { s.SuperMistyTerrainCount += 1; }
                        break;
                }
            }
        }
    }
}
