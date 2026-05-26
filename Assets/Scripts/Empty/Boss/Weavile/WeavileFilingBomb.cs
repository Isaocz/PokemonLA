using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeavileFilingBomb : WeavileProjectiles
{
    /// <summary>
    /// ±¬Õ¨Ô¤ÖÆ¼þ
    /// </summary>
    public WeavileFlingExplosion ExplosionPrefab;

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
        if (!IsBomb && !isFake) {
            IsBomb = true;
            WeavileFlingExplosion e = Instantiate(ExplosionPrefab, transform.position, Quaternion.identity);
            if (ParentWeavile != null) { 
                e.empty = ParentWeavile;
                ParentWeavile.ParentPokemonRoom.CameraShake(0.6f , 3.0f , true);
                audioPlayer.Play(Weavile.WeavileSE.Explosion, transform.position);
            }
        }

    }
}
