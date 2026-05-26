using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeavileFilingSubstitute : WeavileProjectiles
{

    /// <summary>
    /// 爆炸预制件
    /// </summary>
    public WeavileSubstituteOBJ SubstitutePrefab;

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
            WeavileSubstituteOBJ e = Instantiate(SubstitutePrefab, transform.position, Quaternion.identity);
            if (ParentWeavile != null) { 
                e.ParentWeavile = ParentWeavile;
                e.ParentRoom = ParentWeavile.ParentPokemonRoom;
                //主脑不为空 设定连招替身实例
                if (ParentWeavile.TeamBrain)
                {
                    switch (ParentWeavile.TeamBrain.NowSubState)
                    {
                        case WeavileTeamBrain.SubState.TeamState_303_Substitute:
                            ParentWeavile.TeamBrain.TeamState303_Substitute_SetSubstitute(e);
                            break;
                        case WeavileTeamBrain.SubState.TeamState_203_Substitute:
                            ParentWeavile.TeamBrain.TeamState203_Substitute_SetSubstitute(e);
                            break;
                        case WeavileTeamBrain.SubState.TeamState_204_Substitute2:
                            ParentWeavile.TeamBrain.TeamState204_Substitute2_SetSubstitute(e);
                            break;
                    } 
                }
            }
            audioPlayer.Play(Weavile.WeavileSE.JumpOver, transform.position);
        }

    }
}
