using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeavileFlingFullHealEffect : MonoBehaviour
{


    private void Awake()
    {
        Timer.Start(this, 5.0f, () => { Destroy(gameObject); });
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.name);
        //回复玩家
        if (collision.gameObject.tag == "Player")
        {
            PlayerControler p = collision.gameObject.GetComponent<PlayerControler>();
            if (p != null )
            {
                FullHealEmpty(p);
            }
        }
        //回复敌人
        if (collision.gameObject.tag == "Empty")
        {
            Empty e = collision.gameObject.GetComponent<Empty>();
            if (e != null )
            {
                FullHealEmpty(e);
            }
        }
    }


    void FullHealEmpty(Empty e)
    {
        e.GetEmptyFrozenPointFloat = 0;
        if (e.isEmptyFrozenDone)      { e.FrozenRemove(); }
        e.ToxicPointFloat = 0;
        if (e.isToxicDone)            { e.EmptyToxicRemove(); }
        e.ParalysisPointFloat = 0;
        if (e.isParalysisDone)        { e.EmptyParalysisRemove(); }
        e.BurnPointFloat = 0;
        if (e.isBurnDone)             { e.EmptyBurnRemove(); }
        e.SleepPointFloat = 0;
        if (e.isSleepDone)            { e.EmptySleepRemove(); }
        e.GetEmptyFearPointFloat = 0;
        if (e.isFearDone)             { e.FearRemove(); }
        e.EmptyBlindPoint = 0;
        if (e.isSilence)              { e.BlindRemove(); }
        e.EmptyConfusionPoint = 0;
        if (e.isEmptyConfusionDone)   { e.EmptyConfusionRemove(); }
        e.EmptyInfatuationPoint = 0;
        if (e.isEmptyInfatuationDone) { e.EmptyInfatuationRemove(); }
        if (e.isSpeedChange) { e.SpeedRemove01(0); }
    }

    void FullHealEmpty(PlayerControler p)
    {
        p.BurnRemove();
        p.ParalysisRemove();
        p.SleepRemove();
        p.ToxicRemove();
        p.PlayerFrozenRemove();
    }
}
