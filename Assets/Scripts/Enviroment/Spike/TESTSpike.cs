using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TESTSpike : Spike
{

    public enum TestSpikeMode
    {
        Toxic,
        Burn,
        Sleep,
        Paralysis,
        Frozen,
    }
    public TestSpikeMode testmode;

    public bool isHit;


    bool isDamageUp;

    public override void SpikeOnTriggerStay2D(Collider2D other)
    {
        if (other.tag == ("Player"))
        {
            PlayerControler playerControler = other.GetComponent<PlayerControler>();
            if (playerControler != null && !playerControler.playerData.IsPassiveGetList[13] && !playerControler.isRapidSpin)
            {
                switch (testmode)
                {
                    case TestSpikeMode.Toxic:
                        playerControler.ToxicFloatPlus(0.18f);
                        if (playerControler.isToxicDone && !isDamageUp) { Damage *= 2; isDamageUp = true; }
                        else if (!playerControler.isToxicDone && isDamageUp) { Damage /= 2; isDamageUp = false; }
                        break;
                    case TestSpikeMode.Burn:
                        playerControler.BurnFloatPlus(0.18f);
                        if (playerControler.isBurnDone && !isDamageUp) { Damage *= 2; isDamageUp = true; }
                        else if (!playerControler.isBurnDone && isDamageUp) { Damage /= 2; isDamageUp = false; }
                        break;
                    case TestSpikeMode.Sleep:
                        playerControler.SleepFloatPlus(0.18f);
                        if (playerControler.isSleepDone && !isDamageUp) { Damage *= 2; isDamageUp = true; }
                        else if (!playerControler.isSleepDone && isDamageUp) { Damage /= 2; isDamageUp = false; }
                        break;
                    case TestSpikeMode.Paralysis:
                        playerControler.ParalysisFloatPlus(0.18f);
                        if (playerControler.isParalysisDone && !isDamageUp) { Damage *= 2; isDamageUp = true; }
                        else if (!playerControler.isParalysisDone && isDamageUp) { Damage /= 2; isDamageUp = false; }
                        break;
                    case TestSpikeMode.Frozen:
                        playerControler.PlayerFrozenFloatPlus(0.18f , 10.0f);
                        if (playerControler.isPlayerFrozenDone && !isDamageUp) { Damage *= 2; isDamageUp = true; }
                        else if (!playerControler.isPlayerFrozenDone && isDamageUp) { Damage /= 2; isDamageUp = false; }
                        break;
                }
            }
        }
        else if (other.tag == "Empty")
        {
            Empty target = other.GetComponent<Empty>();
            if (target != null)
            {
                switch (testmode)
                {
                    case TestSpikeMode.Toxic:
                        target.EmptyToxicDone(0.35f, 10, 1);
                        break;
                    case TestSpikeMode.Burn:
                        target.EmptyBurnDone(0.35f, 10, 1);
                        break;
                    case TestSpikeMode.Sleep:
                        target.EmptySleepDone(0.35f, 10, 1);
                        break;
                    case TestSpikeMode.Paralysis:
                        target.EmptyParalysisDone(0.35f, 10, 1);
                        break;
                    case TestSpikeMode.Frozen:
                        target.Frozen(0.35f, 10, 1);
                        break;
                }
            }
        }
        if (isHit)
        {
            base.SpikeOnTriggerStay2D(other);
        }
    }



    private void Update()
    {
        SpikesUpdate();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        SpikeOnTriggerStay2D(other);
    }
}
