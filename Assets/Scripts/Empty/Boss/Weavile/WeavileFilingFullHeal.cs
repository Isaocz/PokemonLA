using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeavileFilingFullHeal : WeavileProjectiles
{






    /// <summary>
    /// 万灵药效果
    /// </summary>
    public WeavileFlingFullHealEffect FullHealEffect;

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
            WeavileFlingFullHealEffect e = Instantiate(FullHealEffect, transform.position, Quaternion.identity);
            audioPlayer.Play(Weavile.WeavileSE.HealPosion, transform.position);
        }

    }
}
