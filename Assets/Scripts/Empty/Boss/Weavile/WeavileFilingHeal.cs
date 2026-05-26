using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeavileFilingHeal : WeavileProjectiles
{

    /// <summary>
    /// 回复效果
    /// </summary>
    public WeavileFlingHealEffect HealEffect;

    /// <summary>
    /// 是否爆炸
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
            WeavileFlingHealEffect e = Instantiate(HealEffect, transform.position, Quaternion.identity);
            audioPlayer.Play(Weavile.WeavileSE.HealPosion, transform.position);
        }

    }
}
