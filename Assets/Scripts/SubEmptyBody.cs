using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubEmptyBody : Empty{
    public Empty ParentEmpty;


    int NowHP;
    public float NowFrozenPoint;
    public float NowToxicPoint;
    public float NowParalysisPoint;
    public float NowBurnPoint;
    public float NowSleepPoint;
    public float NowConfusionPoint;
    public float NowFearPoint;
    public float NowBlindPoint;
    public float NowCursePoint;
    public float NowInfatuationPoint;

    /// <summary>
    /// 该体节锚定的体节，该体节距离AnchorsSubBody的距离范围在AnchorsDisMax和AnchorsDisMin之间；
    /// </summary>
    public SubEmptyBody AnchorsSubBody;

    /// <summary>
    /// 该体节距离锚定体节AnchorsSubBody的最远距离
    /// </summary>
    public float AnchorsDisMax;

    /// <summary>
    /// 该体节距离锚定体节AnchorsSubBody的最近距离
    /// </summary>
    public float AnchorsDisMin;



    // Start is called before the first frame update
    public void SubEmptyBodyStart()
    {



        EmptyType01 = ParentEmpty.EmptyType01;
        EmptyType02 = ParentEmpty.EmptyType02;
        player = ParentEmpty.player;
        Emptylevel = ParentEmpty.Emptylevel;
        EmptyHpForLevel(Emptylevel);
        AtkAbilityPoint = AbilityForLevel(Emptylevel, AtkEmptyPoint);
        SpAAbilityPoint = AbilityForLevel(Emptylevel, SpAEmptyPoint);
        DefAbilityPoint = AbilityForLevel(Emptylevel, DefEmptyPoint);
        SpdAbilityPoint = AbilityForLevel(Emptylevel, SpdEmptyPoint);
        SpeedAbilityPoint = AbilityForLevel(Emptylevel, SpeedEmptyPoint);

        animator = ParentEmpty.animator;
        rigidbody2D = GetComponent<Rigidbody2D>();
        NowHP = EmptyHp;
        NowFrozenPoint = GetEmptyFrozenPointFloat;
        NowToxicPoint = ToxicPointFloat;
        NowParalysisPoint = ParalysisPointFloat;
        NowBurnPoint = BurnPointFloat;
        NowSleepPoint = SleepPointFloat;
        NowConfusionPoint = EmptyConfusionPoint;
        NowFearPoint = GetEmptyFearPointFloat;
        NowBlindPoint = EmptyBlindPoint;
        NowCursePoint = EmptyCursePoint;
        NowInfatuationPoint = EmptyInfatuationPoint;

        if (GetComponent<HingeJoint2D>() && GetComponent<HingeJoint2D>().connectedBody)
        {
            AnchorsSubBody = GetComponent<HingeJoint2D>().connectedBody.gameObject.GetComponent<SubEmptyBody>();
        }
    }





    /// <summary>
    /// 根据锚定体节修正位置
    /// </summary>
    public void SetAnchorsPos()
    {
        if (AnchorsSubBody != null && ((AnchorsSubBody.transform.position - transform.position).magnitude < AnchorsDisMin || (AnchorsSubBody.transform.position - transform.position).magnitude > AnchorsDisMax))
        {
            transform.position = ( new Vector3(
                Mathf.Clamp(transform.position.x ,
                Mathf.Min(AnchorsSubBody.transform.position.x + ((transform.position - AnchorsSubBody.transform.position).normalized * AnchorsDisMin).x , AnchorsSubBody.transform.position.x + ((transform.position - AnchorsSubBody.transform.position).normalized * AnchorsDisMax).x),
                Mathf.Max(AnchorsSubBody.transform.position.x + ((transform.position - AnchorsSubBody.transform.position).normalized * AnchorsDisMin).x, AnchorsSubBody.transform.position.x + ((transform.position - AnchorsSubBody.transform.position).normalized * AnchorsDisMax).x)),


                Mathf.Clamp(transform.position.y ,
                Mathf.Min(AnchorsSubBody.transform.position.y + ((transform.position - AnchorsSubBody.transform.position).normalized * AnchorsDisMin).y , AnchorsSubBody.transform.position.y + ((transform.position - AnchorsSubBody.transform.position).normalized * AnchorsDisMax).y) ,
                Mathf.Max(AnchorsSubBody.transform.position.y + ((transform.position - AnchorsSubBody.transform.position).normalized * AnchorsDisMin).y, AnchorsSubBody.transform.position.y + ((transform.position - AnchorsSubBody.transform.position).normalized * AnchorsDisMax).y)) ,0 ) );
        }
    }


    // Update is called once per frame
    public void SubEmptyBodyUpdate()
    {

        if (player == null) { ParentEmpty.ResetPlayer(); player = ParentEmpty.player; }

        if (!ParentEmpty.isDie && !ParentEmpty.isBorn)
        {
            StateMaterialChange();
            if (NowHP != EmptyHp)
            {
                if (!ParentEmpty.isSubBodyEmptyInvincible) {
                    Debug.Log("Hit");
                    if (NowHP > EmptyHp) { Pokemon.PokemonHpChange(null, ParentEmpty.gameObject, NowHP - EmptyHp, 0, 0, Type.TypeEnum.IgnoreType); }
                    else { Pokemon.PokemonHpChange(null, ParentEmpty.gameObject, 0, 0, NowHP - EmptyHp, Type.TypeEnum.IgnoreType); }
                    ParentEmpty.isSubBodyEmptyInvincible = true;
                    Timer.Start(this, 1.0f, () => { ParentEmpty.isSubBodyEmptyInvincible = false; });
                }
                NowHP = EmptyHp;
            }

            if (!isFrozenDef && NowFrozenPoint != GetEmptyFrozenPointFloat)
            {
                Debug.Log(NowFrozenPoint.ToString() + GetEmptyFrozenPointFloat.ToString() + name);
                if (GetEmptyFrozenPointFloat != 0)
                {
                    ParentEmpty.Frozen(FrozenTimeFloat, (GetEmptyFrozenPointFloat - NowFrozenPoint) / FrozenResistance, 1);
                    ParentEmpty.isFrozenDef = true;
                    Timer.Start(this, 0.55f, () => { ParentEmpty.isFrozenDef = false; });
                    foreach (SubEmptyBody b in ParentEmpty.SubEmptyBodyList)
                    {
                        b.isFrozenDef = true;
                        b.FrozenRemove();
                        b.NowFrozenPoint = b.GetEmptyFrozenPointFloat;
                        Timer.Start(this, 0.55f, () => { b.FrozenRemove(); b.isFrozenDef = false; });
                    }
                    FrozenRemove();
                }
                else
                {
                    ParentEmpty.FrozenRemove();
                    foreach (SubEmptyBody b in ParentEmpty.SubEmptyBodyList)
                    {
                        b.FrozenRemove();
                    }
                }
                NowFrozenPoint = GetEmptyFrozenPointFloat;
            }

            if (!isBurnDef && NowBurnPoint != BurnPointFloat)
            {
                //Debug.Log(NowFrozenPoint.ToString() + GetEmptyFrozenPointFloat.ToString() + name);
                if (BurnPointFloat != 0)
                {
                    ParentEmpty.EmptyBurnDone((BurnPointFloat - NowBurnPoint) / BurnResistance, BurnTimeFloat, 1);
                    ParentEmpty.isBurnDef = true;
                    Timer.Start(this, 0.55f, () => { ParentEmpty.isBurnDef = false; });
                    foreach (SubEmptyBody b in ParentEmpty.SubEmptyBodyList)
                    {
                        b.isBurnDef = true;
                        b.EmptyBurnRemove();
                        b.NowBurnPoint = b.BurnPointFloat;
                        Timer.Start(this, 0.55f, () => { b.EmptyBurnRemove(); b.isBurnDef = false; });
                    }
                    EmptyBurnRemove();
                }
                else
                {
                    ParentEmpty.EmptyBurnRemove();
                    foreach (SubEmptyBody b in ParentEmpty.SubEmptyBodyList)
                    {
                        b.EmptyBurnRemove();
                    }
                }
                NowBurnPoint = BurnPointFloat;
            }

            if (!isSleepDef && NowSleepPoint != SleepPointFloat)
            {
                //Debug.Log(NowFrozenPoint.ToString() + GetEmptyFrozenPointFloat.ToString() + name);
                if (SleepPointFloat != 0)
                {
                    ParentEmpty.EmptySleepDone((SleepPointFloat - NowSleepPoint) / SleepResistance, SleepTimeFloat, 1);
                    ParentEmpty.isSleepDef = true;
                    Timer.Start(this, 0.55f, () => { ParentEmpty.isSleepDef = false; });
                    foreach (SubEmptyBody b in ParentEmpty.SubEmptyBodyList)
                    {
                        b.isSleepDef = true;
                        b.EmptySleepRemove();
                        b.NowSleepPoint = b.SleepPointFloat;
                        Timer.Start(this, 0.55f, () => { b.EmptySleepRemove(); b.isSleepDef = false; });
                    }
                    EmptySleepRemove();
                }
                else
                {
                    ParentEmpty.EmptySleepRemove();
                    foreach (SubEmptyBody b in ParentEmpty.SubEmptyBodyList)
                    {
                        b.EmptySleepRemove();
                    }
                }
                NowSleepPoint = SleepPointFloat;
            }

            if (!isParalysisDef && NowParalysisPoint != ParalysisPointFloat)
            {
                Debug.Log(NowParalysisPoint.ToString() + ParalysisPointFloat.ToString() + name);
                if (ParalysisPointFloat != 0)
                {
                    ParentEmpty.EmptyParalysisDone((ParalysisPointFloat - NowParalysisPoint) / ParalysisResistance, ParalysisTimeFloat, 1);
                    ParentEmpty.isParalysisDef = true;
                    Timer.Start(this, 0.55f, () => { ParentEmpty.isParalysisDef = false; });
                    foreach (SubEmptyBody b in ParentEmpty.SubEmptyBodyList)
                    {
                        b.isParalysisDef = true;
                        b.EmptyParalysisRemove();
                        b.NowParalysisPoint = b.ParalysisPointFloat;
                        Timer.Start(this, 0.55f, () => { b.EmptyParalysisRemove(); b.isParalysisDef = false; });
                    }
                    EmptyParalysisRemove();

                }
                else
                {
                    ParentEmpty.EmptyParalysisRemove();
                    foreach (SubEmptyBody b in ParentEmpty.SubEmptyBodyList)
                    {
                        b.EmptyParalysisRemove();
                    }
                }
                NowParalysisPoint = ParalysisPointFloat;
            }

            if (!isToxicDef && NowToxicPoint != ToxicPointFloat)
            {
                //Debug.Log(NowFrozenPoint.ToString() + GetEmptyFrozenPointFloat.ToString() + name);
                if (ToxicPointFloat != 0)
                {
                    ParentEmpty.EmptyToxicDone((ToxicPointFloat - NowToxicPoint) / ToxicResistance, ToxicTimeFloat, 1);
                    ParentEmpty.isToxicDef = true;
                    Timer.Start(this, 0.55f, () => { ParentEmpty.isToxicDef = false; });
                    foreach (SubEmptyBody b in ParentEmpty.SubEmptyBodyList)
                    {
                        b.isToxicDef = true;
                        b.EmptyToxicRemove();
                        b.NowToxicPoint = b.ToxicPointFloat;
                        Timer.Start(this, 0.55f, () => { b.EmptyToxicRemove(); b.isToxicDef = false; });
                    }
                    EmptyToxicRemove();
                }
                else
                {
                    ParentEmpty.EmptyToxicRemove();
                    foreach (SubEmptyBody b in ParentEmpty.SubEmptyBodyList)
                    {
                        b.EmptyToxicRemove();
                    }
                }
                NowToxicPoint = ToxicPointFloat;
            }

            if (!EmptyCurseDef && NowCursePoint != EmptyCursePoint)
            {
                //Debug.Log(NowFrozenPoint.ToString() + GetEmptyFrozenPointFloat.ToString() + name);
                if (EmptyCursePoint != 0)
                {
                    ParentEmpty.EmptyCurse(CurseTimeFloat, (EmptyCursePoint - NowCursePoint) / OtherStateResistance);
                    ParentEmpty.EmptyCurseDef = true;
                    Timer.Start(this, 0.55f, () => { ParentEmpty.EmptyCurseDef = false; });
                    foreach (SubEmptyBody b in ParentEmpty.SubEmptyBodyList)
                    {
                        b.EmptyCurseDef = true;
                        b.EmptyCurseRemove();
                        b.NowCursePoint = b.EmptyCursePoint;
                        Timer.Start(this, 0.55f, () => { b.EmptyCurseRemove(); b.EmptyCurseDef = false; });
                    }
                    EmptyCurseRemove();
                }
                else
                {
                    ParentEmpty.EmptyCurseRemove();
                    foreach (SubEmptyBody b in ParentEmpty.SubEmptyBodyList)
                    {
                        b.EmptyCurseRemove();
                    }
                }
                NowCursePoint = EmptyCursePoint;
            }

            if (!isConfusionDef && NowConfusionPoint != EmptyConfusionPoint)
            {
                Debug.Log(NowFrozenPoint.ToString() + GetEmptyFrozenPointFloat.ToString() + name);
                if (EmptyConfusionPoint != 0)
                {
                    ParentEmpty.EmptyConfusion(ConfusionTimeFloat, (EmptyConfusionPoint - NowConfusionPoint) / OtherStateResistance);
                    ParentEmpty.isConfusionDef = true;
                    Timer.Start(this, 0.55f, () => { ParentEmpty.isConfusionDef = false; });
                    foreach (SubEmptyBody b in ParentEmpty.SubEmptyBodyList)
                    {
                        b.isConfusionDef = true;
                        b.EmptyConfusionRemove();
                        b.NowConfusionPoint = b.EmptyConfusionPoint;
                        Timer.Start(this, 0.55f, () => { b.EmptyConfusionRemove(); b.isConfusionDef = false; });
                    }
                    EmptyConfusionRemove();
                }
                else
                {
                    ParentEmpty.EmptyConfusionRemove();
                    foreach (SubEmptyBody b in ParentEmpty.SubEmptyBodyList)
                    {
                        b.EmptyConfusionRemove();
                    }
                }
                NowConfusionPoint = EmptyConfusionPoint;
            }

            if (!EmptyInfatuationDef && NowInfatuationPoint != EmptyInfatuationPoint)
            {
                //Debug.Log(NowFrozenPoint.ToString() + GetEmptyFrozenPointFloat.ToString() + name);
                if (EmptyInfatuationPoint != 0)
                {
                    ParentEmpty.EmptyInfatuation(InfatuationTimeFloat, (EmptyInfatuationPoint - NowInfatuationPoint) / OtherStateResistance);
                    ParentEmpty.EmptyInfatuationDef = true;
                    Timer.Start(this, 0.55f, () => { ParentEmpty.EmptyInfatuationDef = false; });
                    foreach (SubEmptyBody b in ParentEmpty.SubEmptyBodyList)
                    {
                        b.EmptyInfatuationDef = true;
                        b.EmptyInfatuationRemove();
                        b.NowInfatuationPoint = b.EmptyInfatuationPoint;
                        Timer.Start(this, 0.55f, () => { b.EmptyInfatuationRemove(); b.EmptyInfatuationDef = false; });
                    }
                    EmptyInfatuationRemove();
                }
                else
                {
                    ParentEmpty.EmptyInfatuationRemove();
                    foreach (SubEmptyBody b in ParentEmpty.SubEmptyBodyList)
                    {
                        b.EmptyInfatuationRemove();
                    }
                }
                NowInfatuationPoint = EmptyInfatuationPoint;
            }

            if (!isBlindDef && NowBlindPoint != EmptyBlindPoint)
            {
                //Debug.Log(NowFrozenPoint.ToString() + GetEmptyFrozenPointFloat.ToString() + name);
                if (EmptyBlindPoint != 0)
                {
                    ParentEmpty.Blind(BlindTimeFloat, (EmptyBlindPoint - NowBlindPoint) / OtherStateResistance);
                    ParentEmpty.isBlindDef = true;
                    Timer.Start(this, 0.55f, () => { ParentEmpty.isBlindDef = false; });
                    foreach (SubEmptyBody b in ParentEmpty.SubEmptyBodyList)
                    {
                        b.isBlindDef = true;
                        b.BlindRemove();
                        b.NowBlindPoint = b.EmptyBlindPoint;
                        Timer.Start(this, 0.55f, () => { b.BlindRemove(); b.isBlindDef = false; });
                    }
                    BlindRemove();
                }
                else
                {
                    ParentEmpty.BlindRemove();
                    foreach (SubEmptyBody b in ParentEmpty.SubEmptyBodyList)
                    {
                        b.BlindRemove();
                    }
                }
                NowBlindPoint = EmptyBlindPoint;
            }

            if (!isFearDef && NowFearPoint != GetEmptyFearPointFloat)
            {
                //Debug.Log(NowFrozenPoint.ToString() + GetEmptyFrozenPointFloat.ToString() + name);
                if (GetEmptyFearPointFloat != 0)
                {
                    ParentEmpty.Fear(FearTimeFloat, (GetEmptyFearPointFloat - NowFearPoint) / OtherStateResistance);
                    ParentEmpty.isFearDef = true;
                    Timer.Start(this, 0.55f, () => { ParentEmpty.isFearDef = false; });
                    foreach (SubEmptyBody b in ParentEmpty.SubEmptyBodyList)
                    {
                        b.isFearDef = true;
                        b.FearRemove();
                        b.NowFearPoint = b.GetEmptyFearPointFloat;
                        Timer.Start(this, 0.55f, () => { b.FearRemove(); b.isFearDef = false; });
                    }
                    FearRemove();
                }
                else
                {
                    ParentEmpty.FearRemove();
                    foreach (SubEmptyBody b in ParentEmpty.SubEmptyBodyList)
                    {
                        b.FearRemove();
                    }
                }
                NowFearPoint = GetEmptyFearPointFloat;
            }



        }



    }




    public override void AtkChange(int ChangeLevel, float ChangeTime)
    {
        if (!isAtkChangeDef && ParentEmpty.GetComponent<Empty>() != null)
        {
            foreach (SubEmptyBody b in ParentEmpty.SubEmptyBodyList)
            {
                b.isAtkChangeDef = true;
                Timer.Start(this, 0.5f, () => { b.isAtkChangeDef = false; });
            }
            ParentEmpty.AtkChange(ChangeLevel, ChangeTime);
            ParentEmpty.isAtkChangeDef = true;
            Timer.Start(this, 0.5f, () => { ParentEmpty.isAtkChangeDef = false; });
        }
    }

    public override void ResetAtk()
    {
        ParentEmpty.ResetAtk();
    }

    public override void DefChange(int ChangeLevel, float ChangeTime)
    {
        if (!isDefChangeDef && ParentEmpty.GetComponent<Empty>() != null)
        {
            foreach (SubEmptyBody b in ParentEmpty.SubEmptyBodyList)
            {
                b.isDefChangeDef = true;
                Timer.Start(this, 0.5f, () => { b.isDefChangeDef = false; });
            }
            ParentEmpty.DefChange(ChangeLevel, ChangeTime);
            ParentEmpty.isDefChangeDef = true;
            Timer.Start(this, 0.5f, () => { ParentEmpty.isDefChangeDef = false; });
        }
    }

    public override void SpAChange(int ChangeLevel, float ChangeTime)
    {
        if (!isSpAChangeDef && ParentEmpty.GetComponent<Empty>() != null)
        {
            foreach (SubEmptyBody b in ParentEmpty.SubEmptyBodyList)
            {
                b.isSpAChangeDef = true;
                Timer.Start(this, 0.5f, () => { b.isSpAChangeDef = false; });
            }
            ParentEmpty.SpAChange(ChangeLevel, ChangeTime);
            ParentEmpty.isSpAChangeDef = true;
            Timer.Start(this, 0.5f, () => { ParentEmpty.isSpAChangeDef = false; });
        }
    }

    public override void SpDChange(int ChangeLevel, float ChangeTime)
    {
        if (!isSpDChangeDef && ParentEmpty.GetComponent<Empty>() != null)
        {
            foreach (SubEmptyBody b in ParentEmpty.SubEmptyBodyList)
            {
                b.isSpDChangeDef = true;
                Timer.Start(this, 0.5f, () => { b.isSpDChangeDef = false; });
            }
            ParentEmpty.SpDChange(ChangeLevel, ChangeTime);
            ParentEmpty.isSpDChangeDef = true;
            Timer.Start(this, 0.5f, () => { ParentEmpty.isSpDChangeDef = false; });
        }
    }

    public override void Cold(float Time)
    {
        if (!isColdDef && ParentEmpty.GetComponent<Empty>() != null)
        {
            foreach (SubEmptyBody b in ParentEmpty.SubEmptyBodyList)
            {
                b.isColdDef = true;
                Timer.Start(this, 0.5f, () => { b.isColdDef = false; });
            }
            ParentEmpty.Cold(Time);
            ParentEmpty.isColdDef = true;
            Timer.Start(this, 0.5f, () => { ParentEmpty.isColdDef = false; });
        }
    }

    public void SubEmptyBodyFixedUpdate()
    {
        SetAnchorsPos();
        if (!ParentEmpty.isDie && !ParentEmpty.isBorn)
        {
            EmptyBeKnock();
        }
    }

    

}
