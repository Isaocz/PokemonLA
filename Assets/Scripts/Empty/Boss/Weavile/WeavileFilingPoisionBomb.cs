using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeavileFilingPoisionBomb : WeavileProjectiles
{

    /// <summary>
    /// ¶¾Îí
    /// </summary>
    public WeavilrPosionRushPosionMist PoisonMistPrefab;

    /// <summary>
    /// ÊÇ·ñ±¬Õ¨
    /// </summary>
    public bool IsBomb = false;


    private void Awake()
    {
        AwakeEvent();
    }


    private void Start()
    {
        StartEvent();
        
    }


    private void Update()
    {
        UpdateEvent();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        OnTriggerEnter2DEvent(collision);
    }



    protected override void ProjectilesBreak()
    {
        base.ProjectilesBreak();
        if (!IsBomb && !isFake)
        {
            IsBomb = true;
            WeavilrPosionRushPosionMist e = Instantiate(PoisonMistPrefab, transform.position, Quaternion.identity);
            if (ParentWeavile != null) { e.ParentWeavile = ParentWeavile; }
            audioPlayer.Play(Weavile.WeavileSE.Smoke, transform.position);
        }

    }
}
