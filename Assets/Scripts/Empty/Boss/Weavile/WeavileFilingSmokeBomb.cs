using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeavileFilingSmokeBomb : WeavileProjectiles
{

    /// <summary>
    /// ÑÌÎí
    /// </summary>
    public WeavileFlingSmoke SmokePrefab;

    /// <summary>
    /// ÊÇ·ñ±¬Õ¨
    /// </summary>
    public bool IsBomb = false;


    public Vector2 Target;


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
            WeavileFlingSmoke e = Instantiate(SmokePrefab, Target, Quaternion.identity);
            ParentWeavile.TeamBrain.AddWeavileSmokeList(e);
            audioPlayer.Play(Weavile.WeavileSE.Smoke, transform.position);
        }
    }
}
