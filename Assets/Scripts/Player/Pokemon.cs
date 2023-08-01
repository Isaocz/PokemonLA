using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pokemon : MonoBehaviour
{
    public Material NormalMaterial;

    public Material FrozenMaterial;
    public Material SpeedDownMaterial;
    public Material ToxicMaterial;
    public Material ParalysisMaterial;
    public Material BurnMaterial;
    public Material SleepMaterial;
    public Material FearMaterial;
    public Material InfatuationMaterial;


    public float FrozenResistance = 1f;
    public float ToxicResistance = 1f;
    public float ParalysisResistance = 1f;
    public float BurnResistance = 1f;
    public float SleepResistance = 1f;
    public float OtherStateResistance = 1f;

    public float TimeStateInvincible;
    protected float StateInvincileTimer = 0.0f;
    protected bool isStateInvincible = false;

    [Tooltip("pokemon主体的渲染组件集")]
    public List<SpriteRenderer> skinRenderers;

    //声明一个动画管理者变量，获取动画管理者
    public Animator animator;

    public void MarterialChangeToNurmal()
    {
        if (isEmptyFrozenDone)
        {
            MarterialChangeToFrozen();
        }
        else if (isToxicDone)
        {
            MarterialChangeToToxic();
        }
        else if (isParalysisDone)
        {
            MarterialChangeToParalysis();
        }
        else if (isSleepDone)
        {
            MarterialChangeToSleep();
        }
        else if (isBurnDone)
        {
            MarterialChangeToBurn();
        }
        else if (isBurnDone)
        {
            MarterialChangeToBurn();
        }
        else if (isFearDone)
        {
            MarterialChangeToFear();
        }
        else if (isEmptyInfatuationDone)
        {
            MarterialChangeToInfatuation();
        }
        else
        {
            if (gameObject.GetComponent<SpriteRenderer>() == null) {
                if (gameObject.transform.GetChild(3).GetComponent<SpriteRenderer>() != null) { gameObject.transform.GetChild(3).GetComponent<SpriteRenderer>().material = NormalMaterial; }
                else { gameObject.transform.GetChild(3).GetChild(0).GetComponent<SpriteRenderer>().material = NormalMaterial; }
            }
            else { gameObject.GetComponent<SpriteRenderer>().material = NormalMaterial; }
        }
    }


    public void MarterialChangeToFrozen()
    {
        SetSkinRenderersMaterial(FrozenMaterial);
        animator.speed = 0;
    }
    public void MarterialChangeToSpeedDown()
    {
        SetSkinRenderersMaterial(SpeedDownMaterial);
    }
    public void MarterialChangeToToxic()
    {
        SetSkinRenderersMaterial(ToxicMaterial);
    }

    public void MarterialChangeToParalysis()
    {
        SetSkinRenderersMaterial(ParalysisMaterial);
    }

    public void MarterialChangeToBurn()
    {
        SetSkinRenderersMaterial(BurnMaterial);
    }

    public void MarterialChangeToSleep()
    {
        SetSkinRenderersMaterial(SleepMaterial);
        //animator.speed = 0.55f;
    }

    public void MarterialChangeToFear()
    {
        SetSkinRenderersMaterial(FearMaterial);
    }

    public void MarterialChangeToInfatuation()
    {
        SetSkinRenderersMaterial(InfatuationMaterial);
    }

    //获取 pokemon 主体的渲染组件
    private List<SpriteRenderer> GetSkinRenderers()
    {
        if (skinRenderers.Count > 0)
        {
            return skinRenderers;
        }
        List<SpriteRenderer> srs = new List<SpriteRenderer>();
        if (gameObject.GetComponent<SpriteRenderer>() != null)
        {
            srs.Add(gameObject.GetComponent<SpriteRenderer>());
            return srs;
        }

        Transform child3 = gameObject.transform.GetChild(3);
        if (child3.GetComponent<SpriteRenderer>() != null)
        {
            srs.Add(child3.GetComponent<SpriteRenderer>());
            return srs;
        }
        Transform child30 = child3.GetChild(0);
        srs.Add(child30.GetComponent<SpriteRenderer>());
        return srs;
    }

    /// <summary>
    /// 更新敌人的材质
    /// </summary>
    private void SetSkinRenderersMaterial(Material material)
    {
        List<SpriteRenderer> skinRendererList = GetSkinRenderers();
        foreach (SpriteRenderer skinRenderer in skinRendererList)
        {
            skinRenderer.material = material;
        }
    }

    /// <summary>
    /// 修改敌人的材质
    /// </summary>
    public void StateMaterialChange()
    {
        List<SpriteRenderer> skinRendererList = GetSkinRenderers();
        foreach (SpriteRenderer skinRenderer in skinRendererList)
        {
            var StateMat = skinRenderer.material;
            var playerTex = skinRenderer.sprite.texture;
            StateMat.SetTexture("_PlayerTex", playerTex);
        }
    }










    //获得玩家的UI状态栏
    public PlayerUIState playerUIState;

    //声明变量速度
    public float speed;






    //***************************************************************************对敌人的函数*********************************************************************************

    //===========================================================================冷冻的函数=====================================================================================


    //一个变量代表速度是否被改变，一个代表改变的倍率
    public bool isEmptyFrozenDone;
    bool isFrozenDone = false;
    bool isFrozenStart = false;
    float EmptyFrozenPointFloat;
    float SpeedBefoerChange;

    //调用此函数时，如果还未被冰冻，冰冻，状态变为被冰冻
    public void Frozen(float FrozenTime , float FrozenPoint , float FrozenPer)
    {
        if (GetComponent<Empty>() != null && isColdDown != 0) { FrozenPer += 0.25f * isColdDown; }
        Debug.Log(FrozenPer);
        if (Random.Range(0.0f , 1.0f) <= FrozenPer ) {
            if (!isFrozenDone)
            {
                EmptyFrozenPointFloat += FrozenPoint * FrozenResistance;
                if (!isFrozenStart && EmptyFrozenPointFloat < 1)
                {
                    playerUIState.StatePlus(2);
                    playerUIState.StateSlowUP(2, EmptyFrozenPointFloat);
                    isFrozenStart = true;
                }
                else if (isFrozenStart && EmptyFrozenPointFloat < 1)
                {
                    playerUIState.StateSlowUP(2, EmptyFrozenPointFloat);
                }
                else if (EmptyFrozenPointFloat >= 1 && !isFrozenDone)
                {
                    if (!isFrozenStart)
                    {
                        playerUIState.StatePlus(2);
                        playerUIState.StateSlowUP(2, EmptyFrozenPointFloat);
                        isFrozenStart = true;
                    }
                    isFrozenDone = true;
                    playerUIState.StateSlowUP(2, EmptyFrozenPointFloat);
                    MarterialChangeToFrozen();
                    SpeedBefoerChange = speed;
                    speed = 0;
                    isEmptyFrozenDone = true;
                    animator.speed = 0;
                    Invoke("FrozenRemove", FrozenTime);
                }
            }
        }
    }

    //只可被上一个函数延迟调用，代表解冻的函数
    public void FrozenRemove()
    {
        if (isFrozenStart)
        {
            EmptyFrozenPointFloat = 0;
            playerUIState.StateSlowUP(2, 0);
            isFrozenStart = false;
            isFrozenDone = false;
            isEmptyFrozenDone = false;
            speed = SpeedBefoerChange;
            SpeedBefoerChange = 0;
            playerUIState.StateDestory(2);
            isSpeedChange = false;
            isEmptyFrozenDone = false;
            MarterialChangeToNurmal();
            animator.speed = 1;
        }
    }

    //===========================================================================冷冻的函数=====================================================================================

    //===========================================================================害怕的函数=====================================================================================

    public bool isFearDone = false;
    public bool isFearStart = false;
    float EmptyFearPointFloat;

    //调用此函数时，如果还未害怕，状态变为害怕
    public void Fear(float FearTime , float FearPoint)
    {
        if (!isFearDone)
        {
            EmptyFearPointFloat += FearPoint * OtherStateResistance;
            if (!isFearStart && EmptyFearPointFloat < 1)
            {
                playerUIState.StatePlus(7);
                playerUIState.StateSlowUP(7, EmptyFearPointFloat);
                isFearStart = true;
            }
            else if (isFearStart && EmptyFearPointFloat < 1)
            {
                playerUIState.StateSlowUP(7, EmptyFearPointFloat);
            }
            else if (EmptyFearPointFloat >= 1 && !isFearDone)
            {
                if (!isFearStart)
                {
                    playerUIState.StatePlus(7);
                    playerUIState.StateSlowUP(7, EmptyFearPointFloat);
                    isFearStart = true;
                }
                isFearDone = true;
                playerUIState.StateSlowUP(7, EmptyFearPointFloat);
                MarterialChangeToFear();
                Invoke("FearRemove", FearTime);
            }
        }

    }
    //只可被上一个函数延迟调用，代表速度害怕的函数
    void FearRemove()
    {
        if (isFearStart)
        {
            EmptyFearPointFloat = 0;
            playerUIState.StateSlowUP(7, 0);
            isFearStart = false;
            isFearDone = false;
            playerUIState.StateDestory(7);
            MarterialChangeToNurmal();
        }
    }

    //===========================================================================害怕的函数=====================================================================================


    //===========================================================================敌人致盲的函数=====================================================================================
    
    public bool isBlindDone = false;
    public bool isBlindStart = false;
    float EmptyBlindPoint;

    //调用此函数时，如果还未害怕，状态变为害怕
    public void Blind(float BlindTimer, float BlinderPoint)
    {
        if (!isBlindDone)
        {
            EmptyBlindPoint += BlinderPoint * OtherStateResistance;
            Debug.Log(EmptyBlindPoint);
            if (!isBlindStart && EmptyBlindPoint < 1)
            {
                playerUIState.StatePlus(1);
                playerUIState.StateSlowUP(1, EmptyBlindPoint);
                isBlindStart = true;
            }
            else if (isBlindStart && EmptyBlindPoint < 1)
            {
                playerUIState.StateSlowUP(1, EmptyBlindPoint);
            }
            else if (EmptyBlindPoint >= 1 && !isBlindDone)
            {
                if (!isBlindStart)
                {
                    playerUIState.StatePlus(1);
                    playerUIState.StateSlowUP(1, EmptyBlindPoint);
                    isBlindStart = true;
                }
                isBlindDone = true;
                GetComponent<Empty>().isSilence = true;
                playerUIState.StateSlowUP(1, EmptyBlindPoint);
                if (BlindTimer != 0) { Invoke("BlindRemove", BlindTimer); }
            }
        }

    }
    //只可被上一个函数延迟调用，代表速度害怕的函数
    public void BlindRemove()
    {
        if (isBlindDone)
        {
            EmptyBlindPoint = 0;
            playerUIState.StateSlowUP(1, 0);
            isBlindStart = false;
            isBlindDone = false;
            GetComponent<Empty>().isSilence = false;
            playerUIState.StateDestory(1);
        }
    }

    //===========================================================================敌人致盲的函数=====================================================================================


    //===========================================================================敌人中毒的函数=====================================================================================
    //调用此函数时，如果还未开始中毒，开始中毒
    public void EmptyToxicDone(float ToxicPoint , float ToxicTime)
    {
        Empty EmptyObj = GetComponent<Empty>();
        if (!isToxicDef && EmptyObj.EmptyType01 != Type.TypeEnum.Poison && EmptyObj.EmptyType01 != Type.TypeEnum.Steel && EmptyObj.EmptyType02 != Type.TypeEnum.Poison && EmptyObj.EmptyType02 != Type.TypeEnum.Steel && !isToxicDone)
        {
            ToxicPointFloat += ToxicPoint * ToxicResistance;
            if (!isToxicStart && ToxicPointFloat < 1)
            {
                playerUIState.StatePlus(3);
                playerUIState.StateSlowUP(3, ToxicPointFloat);
                isToxicStart = true;
            }
            else if (isToxicStart && ToxicPointFloat < 1)
            {
                playerUIState.StateSlowUP(3, ToxicPointFloat);
            }
            else if (ToxicPointFloat >= 1 && !isToxicDone)
            {
                if (!isToxicStart)
                {
                    playerUIState.StatePlus(3);
                    playerUIState.StateSlowUP(3, ToxicPointFloat);
                    isToxicStart = true;
                }
                isToxicDone = true;
                playerUIState.StateSlowUP(3, ToxicPointFloat);
                MarterialChangeToToxic();
                Invoke("EmptyToxicRemove" , ToxicTime);
                EmptyObj.SpAAbilityPoint /= 2;

            }
        }
    }

    void EmptyToxicRemove()
    {
        if (isToxicDone)
        {
            ToxicPointFloat = 0;
            playerUIState.StateSlowUP(3, 0);
            isToxicStart = false;
            isToxicDone = false;
            playerUIState.StateDestory(3);
            MarterialChangeToNurmal();
            GetComponent<Empty>().SpAAbilityPoint *= 2;
        }
    }

    //===========================================================================敌人中毒的函数=====================================================================================




    //===========================================================================敌人烧伤的函数=====================================================================================
    //调用此函数时，如果还未开始烧伤，开始烧伤
    public void EmptyBurnDone(float BurnPoint , float BurnTime)
    {
        Empty EmptyObj = GetComponent<Empty>();
        if (!isToxicDef && EmptyObj.EmptyType01 != Type.TypeEnum.Fire && EmptyObj.EmptyType02 != Type.TypeEnum.Fire && !isBurnDone)
        {
            BurnPointFloat += BurnPoint * BurnResistance;
            if (!isBurnStart && BurnPointFloat < 1)
            {
                playerUIState.StatePlus(5);
                playerUIState.StateSlowUP(5, BurnPointFloat);
                isBurnStart = true;
            }
            else if (isBurnStart && BurnPointFloat < 1)
            {
                playerUIState.StateSlowUP(5, BurnPointFloat);
            }
            else if (BurnPointFloat >= 1 && !isBurnDone)
            {
                if (!isBurnStart)
                {
                    playerUIState.StatePlus(5);
                    playerUIState.StateSlowUP(5, BurnPointFloat);
                    isBurnStart = true;
                }
                isBurnDone = true;
                playerUIState.StateSlowUP(5, BurnPointFloat);
                MarterialChangeToBurn();
                Invoke("EmptyBurnRemove", BurnTime);
                EmptyObj.AtkAbilityPoint /= 2;
            }
        }
    }
    void EmptyBurnRemove()
    {
        if (isBurnDone)
        {
            BurnPointFloat = 0;
            playerUIState.StateSlowUP(5, 0);
            isBurnStart = false;
            isBurnDone = false;
            playerUIState.StateDestory(5);
            MarterialChangeToNurmal();
            GetComponent<Empty>().AtkAbilityPoint *= 2;
        }
    }

    //===========================================================================敌人烧伤的函数=====================================================================================

    //===========================================================================敌人睡眠的函数=====================================================================================
    //调用此函数时，如果还未开始睡眠，开始睡眠
    public void EmptySleepDone(float SleepPoint, float SleepTime)
    {
        Empty EmptyObj = GetComponent<Empty>();
        if (!isSleepDone)
        {
            SleepPointFloat += SleepPoint * SleepResistance;
            if (!isSleepStart && SleepPointFloat < 1)
            {
                playerUIState.StatePlus(6);
                playerUIState.StateSlowUP(6, SleepPointFloat);
                isSleepStart = true;
            }
            else if (isSleepStart && SleepPointFloat < 1)
            {
                playerUIState.StateSlowUP(6, SleepPointFloat);
            }
            else if (SleepPointFloat >= 1 && !isSleepDone)
            {
                if (!isSleepStart)
                {
                    playerUIState.StatePlus(6);
                    playerUIState.StateSlowUP(6, SleepPointFloat);
                    isSleepStart = true;
                }
                isSleepDone = true;
                playerUIState.StateSlowUP(6, SleepPointFloat);
                MarterialChangeToSleep();
                Invoke("EmptySleepRemove", SleepTime);
            }
        }
    }
    public void EmptySleepRemove()
    {

        if (isSleepDone)
        {

            SleepPointFloat = 0;
            playerUIState.StateSlowUP(6, 0);
            isSleepStart = false;
            isSleepDone = false;
            MarterialChangeToNurmal();
            playerUIState.StateDestory(6);
        }
    }

    //===========================================================================敌人睡眠的函数=====================================================================================



    //===========================================================================敌人麻痹的函数=====================================================================================
    //调用此函数时，如果还未开始麻痹，开始麻痹
    public void EmptyParalysisDone(float ParalysisPoint, float ParalysisTime)
    {
        Empty EmptyObj = GetComponent<Empty>();
        if (!isToxicDef && EmptyObj.EmptyType01 != Type.TypeEnum.Electric && EmptyObj.EmptyType02 != Type.TypeEnum.Electric)
        {
            if (!isParalysisDone)
            {
                ParalysisPointFloat += ParalysisPoint * ParalysisResistance;
                if (!isParalysisStart && ParalysisPointFloat < 1)
                {
                    playerUIState.StatePlus(4);
                    playerUIState.StateSlowUP(4, ParalysisPointFloat);
                    isParalysisStart = true;
                }
                else if (isParalysisStart && ParalysisPointFloat < 1)
                {
                    playerUIState.StateSlowUP(4, ParalysisPointFloat);
                }
                else if (ParalysisPointFloat >= 1 && !isParalysisDone)
                {
                    if (!isParalysisStart)
                    {
                        playerUIState.StatePlus(4);
                        playerUIState.StateSlowUP(4, ParalysisPointFloat);
                        isParalysisStart = true;
                    }
                    isParalysisDone = true;
                    playerUIState.StateSlowUP(4, ParalysisPointFloat);
                    MarterialChangeToParalysis();
                    speed *= 0.8f;
                    Invoke("EmptyParalysisRemove", ParalysisTime);
                }
            }
        }
    }
    public void EmptyParalysisRemove()
    {

        if (isParalysisDone)
        {

            ParalysisPointFloat = 0;
            playerUIState.StateSlowUP(4, 0);
            isParalysisStart = false;
            isParalysisDone = false;
            MarterialChangeToNurmal();
            speed /= 0.8f;
            playerUIState.StateDestory(4);
        }
    }

    //===========================================================================敌人麻痹的函数=====================================================================================






























    //===========================================================================敌人混乱的函数=====================================================================================

    public bool isEmptyConfusionDone = false;
    public bool isEmptyConfusionStart = false;
    float EmptyConfusionPoint;

    //调用此函数时，如果还未混乱，状态变为混论
    public void EmptyConfusion(float ConfusionTimer, float ConfusionPoint)
    {
        if (!isEmptyConfusionDone)
        {
            EmptyConfusionPoint += ConfusionPoint * OtherStateResistance;
            //Debug.Log(EmptyConfusionPoint);
            if (!isEmptyConfusionStart && EmptyConfusionPoint < 1)
            {
                playerUIState.StatePlus(9);
                playerUIState.StateSlowUP(9, EmptyConfusionPoint);
                isEmptyConfusionStart = true;
            }
            else if (isEmptyConfusionStart && EmptyConfusionPoint < 1)
            {
                playerUIState.StateSlowUP(9, EmptyConfusionPoint);
            }
            else if (EmptyConfusionPoint >= 1 && !isEmptyConfusionDone)
            {
                if (!isEmptyConfusionStart)
                {
                    playerUIState.StatePlus(9);
                    playerUIState.StateSlowUP(9, EmptyConfusionPoint);
                    isEmptyConfusionStart = true;
                }
                isEmptyConfusionDone = true;
                playerUIState.StateSlowUP(9, EmptyConfusionPoint);
                if (ConfusionTimer != 0) { Invoke("EmptyConfusionRemove", ConfusionTimer); }
            }
        }

    }
    //只可被上一个函数延迟调用，代表移除混论的函数
    public void EmptyConfusionRemove()
    {
        if (isEmptyConfusionDone)
        {
            EmptyConfusionPoint = 0;
            playerUIState.StateSlowUP(9, 0);
            isEmptyConfusionStart = false;
            isEmptyConfusionDone = false;
            playerUIState.StateDestory(9);
        }
    }

    //===========================================================================敌人混乱的函数=====================================================================================















    //===========================================================================敌人着迷的函数=====================================================================================

    public bool isEmptyInfatuationDone = false;
    public bool isEmptyInfatuationStart = false;
    float EmptyInfatuationPoint;

    /// <summary>
    /// 调用此函数时，如果还未着迷，状态变为着迷
    /// </summary>
    /// <param name="InfatuationTimer">着迷的持续时间</param>
    /// <param name="InfatuationPoint">着迷的点数</param>
    public void EmptyInfatuation(float InfatuationTimer, float InfatuationPoint)
    {
        if (!isEmptyInfatuationDone)
        {
            EmptyInfatuationPoint += InfatuationPoint * OtherStateResistance;
            if (!isEmptyInfatuationStart && EmptyInfatuationPoint < 1)
            {
                playerUIState.StatePlus(10);
                playerUIState.StateSlowUP(10, EmptyInfatuationPoint);
                isEmptyInfatuationStart = true;
            }
            else if (isEmptyInfatuationStart && EmptyInfatuationPoint < 1)
            {
                playerUIState.StateSlowUP(10, EmptyInfatuationPoint);
            }
            else if (EmptyInfatuationPoint >= 1 && !isEmptyInfatuationDone)
            {
                if (!isEmptyInfatuationStart)
                {
                    playerUIState.StatePlus(10);
                    playerUIState.StateSlowUP(10, EmptyInfatuationPoint);
                    isEmptyInfatuationStart = true;
                }
                isEmptyInfatuationDone = true;
                playerUIState.StateSlowUP(10, EmptyInfatuationPoint);
                MarterialChangeToInfatuation();
                Empty Boss = GetComponent<Empty>();
                if(Boss != null && Boss.isBoos)
                {
                    Boss.speed *= 0.5f;
                }
                if (InfatuationTimer != 0) { Invoke("EmptyInfatuationRemove", InfatuationTimer); }
            }
        }

    }
    //只可被上一个函数延迟调用，代表移除着迷的函数
    public void EmptyInfatuationRemove()
    {
        if (isEmptyInfatuationDone)
        {
            EmptyInfatuationPoint = 0;
            playerUIState.StateSlowUP(10, 0);
            isEmptyInfatuationStart = false;
            isEmptyInfatuationDone = false;
            MarterialChangeToNurmal();
            Empty Boss = GetComponent<Empty>();
            if (Boss != null && Boss.isBoos)
            {
                Boss.speed *= 2f;
            }
            playerUIState.StateDestory(10);
        }
    }

    //===========================================================================敌人着迷的函数=====================================================================================














    //===========================================================================敌人攻击力提升的函数=====================================================================================

    /// <summary>
    /// 代表攻击力是否提升
    /// </summary>
    bool isAtkUp;

    /// <summary>
    /// 此方法并不会直接改变敌人的数值，仅是给敌人添加一个UI标志.调用此函数时，如果攻击力未被提升，状态变为攻击力被提升
    /// </summary>
    /// <param name="Time"> 提升的时间，如果为零时间为无限，需要手动Remove </param>
    public void AtkUP( float Time )
    {
        if (GetComponent<Empty>() != null)
        {
            isAtkUp = true;
            playerUIState.StatePlus(11);
            if (Time != 0) { Invoke("AtkUpRemove", Time); }
        }
    }
    /// <summary>
    /// 如果AtkUP( float Time )的Time不等于0，不需要调用此函数，如果Time等于0，需要在攻击力恢复时调用此函数
    /// </summary>
    void AtkUpRemove()
    {
        if (isAtkUp)
        {
            if (GetComponent<Empty>() != null)
            {
                isAtkUp = false;
                playerUIState.StateDestory(11);
            }
        }
    }
    /// <summary>
    /// 能力变化的函数，level是能力变化等级，type是能力变化类型
    /// </summary>
    /// <param name="level"></param>
    /// <param name="type"></param>
    public void LevelChange(int level, string type)
    {
        if (GetComponent<Empty>() != null)
        {
            if(type == "Atk")
            {
                switch (level)
                {
                    case 1:gameObject.GetComponent<Empty>().AtkAbilityPoint = (int)(gameObject.GetComponent<Empty>().AtkAbilityPoint * 1.2);break;
                    case 2:gameObject.GetComponent<Empty>().AtkAbilityPoint = (int)(gameObject.GetComponent<Empty>().AtkAbilityPoint * 1.2 * 1.2);break;
                    case 3:gameObject.GetComponent<Empty>().AtkAbilityPoint = (int)(gameObject.GetComponent<Empty>().AtkAbilityPoint * 1.2 * 1.2 * 1.2);break;
                    case -1:gameObject.GetComponent<Empty>().AtkAbilityPoint = (int)(gameObject.GetComponent<Empty>().AtkAbilityPoint * 0.8); break;
                    case -2:gameObject.GetComponent<Empty>().AtkAbilityPoint = (int)(gameObject.GetComponent<Empty>().AtkAbilityPoint * 0.8 * 0.8); break;
                    case -3:gameObject.GetComponent<Empty>().AtkAbilityPoint = (int)(gameObject.GetComponent<Empty>().AtkAbilityPoint * 0.8 * 0.8 * 0.8); break;
                }
            }
            if(type == "Def")
            {
                switch (level)
                {
                    case 1:gameObject.GetComponent<Empty>().DefAbilityPoint = (int)(gameObject.GetComponent<Empty>().DefAbilityPoint * 1.2);break;
                    case 2:gameObject.GetComponent<Empty>().DefAbilityPoint = (int)(gameObject.GetComponent<Empty>().DefAbilityPoint * 1.2 * 1.2); break;
                    case 3:gameObject.GetComponent<Empty>().DefAbilityPoint = (int)(gameObject.GetComponent<Empty>().DefAbilityPoint * 1.2 * 1.2 * 1.2); break;
                    case -1:gameObject.GetComponent<Empty>().DefAbilityPoint = (int)(gameObject.GetComponent<Empty>().DefAbilityPoint * 0.8); break;
                    case -2:gameObject.GetComponent<Empty>().DefAbilityPoint = (int)(gameObject.GetComponent<Empty>().DefAbilityPoint * 0.8 * 0.8); break;
                    case -3:gameObject.GetComponent<Empty>().DefAbilityPoint = (int)(gameObject.GetComponent<Empty>().DefAbilityPoint * 0.8 * 0.8 * 0.8); break;
                }
            }
            if(type == "SpA")
            {
                switch (level)
                {
                    case 1:gameObject.GetComponent<Empty>().SpAAbilityPoint = (int)(gameObject.GetComponent<Empty>().SpAAbilityPoint * 1.2); break;
                    case 2:gameObject.GetComponent<Empty>().SpAAbilityPoint = (int)(gameObject.GetComponent<Empty>().SpAAbilityPoint * 1.2 * 1.2); break;
                    case 3:gameObject.GetComponent<Empty>().SpAAbilityPoint = (int)(gameObject.GetComponent<Empty>().SpAAbilityPoint * 1.2 * 1.2 * 1.2); break;
                    case -1:gameObject.GetComponent<Empty>().SpAAbilityPoint = (int)(gameObject.GetComponent<Empty>().SpAAbilityPoint * 0.8); break;
                    case -2:gameObject.GetComponent<Empty>().SpAAbilityPoint = (int)(gameObject.GetComponent<Empty>().SpAAbilityPoint * 0.8 * 0.8); break;
                    case -3:gameObject.GetComponent<Empty>().SpAAbilityPoint = (int)(gameObject.GetComponent<Empty>().SpAAbilityPoint * 0.8 * 0.8 * 0.8); break;
                }
            }
            if(type == "SpD")
            {
                switch (level)
                {
                    case 1:gameObject.GetComponent<Empty>().SpdAbilityPoint = (int)(gameObject.GetComponent<Empty>().SpdAbilityPoint * 1.2); break;
                    case 2:gameObject.GetComponent<Empty>().SpdAbilityPoint = (int)(gameObject.GetComponent<Empty>().SpdAbilityPoint * 1.2 * 1.2); break;
                    case 3:gameObject.GetComponent<Empty>().SpdAbilityPoint = (int)(gameObject.GetComponent<Empty>().SpdAbilityPoint * 1.2 * 1.2 * 1.2); break;
                    case -1:gameObject.GetComponent<Empty>().SpdAbilityPoint = (int)(gameObject.GetComponent<Empty>().SpdAbilityPoint * 0.8); break;
                    case -2:gameObject.GetComponent<Empty>().SpdAbilityPoint = (int)(gameObject.GetComponent<Empty>().SpdAbilityPoint * 0.8 * 0.8); break;
                    case -3:gameObject.GetComponent<Empty>().SpdAbilityPoint = (int)(gameObject.GetComponent<Empty>().SpdAbilityPoint * 0.8 * 0.8 * 0.8); break;
                }
            }
        }
    }

    //===========================================================================敌人攻击力提升的函数=====================================================================================




    //===========================================================================敌人攻击力下降的函数=====================================================================================

    /// <summary>
    /// 代表攻击力是否下降
    /// </summary>
    bool isAtkDown;

    /// <summary>
    /// 此方法并不会直接改变敌人的数值，仅是给敌人添加一个UI标志.调用此函数时，如果攻击力未被下降，状态变为攻击力被下降
    /// </summary>
    /// <param name="Time"> 下降的时间，如果为零时间为无限，需要手动Remove </param>
    public void AtkDown(float Time)
    {
        if (GetComponent<Empty>() != null)
        {
            isAtkDown = true;
            playerUIState.StatePlus(12);
            if (Time != 0) { Invoke("AtkDownRemove", Time); }
        }
    }
    /// <summary>
    /// 如果AtkDown( float Time )的Time不等于0，不需要调用此函数，如果Time等于0，需要在攻击力恢复时调用此函数
    /// </summary>
    public void AtkDownRemove()
    {
        if (isAtkDown)
        {
            if (GetComponent<Empty>() != null)
            {
                isAtkDown = false;
                playerUIState.StateDestory(12);
            }
        }
    }

    //===========================================================================敌人攻击力下降的函数=====================================================================================


    //===========================================================================敌人防御力提升的函数=====================================================================================

    /// <summary>
    /// 代表防御力是否提升
    /// </summary>
    bool isDefUp;

    /// <summary>
    /// 此方法并不会直接改变敌人的数值，仅是给敌人添加一个UI标志.调用此函数时，如果防御力未被提升，状态变为防御力被提升
    /// </summary>
    /// <param name="Time"> 提升的时间，如果为零时间为无限，需要手动Remove </param>
    public void DefUP(float Time)
    {
        if (GetComponent<Empty>() != null)
        {
            isDefUp = true;
            playerUIState.StatePlus(13);
            if (Time != 0) { Invoke("DefUpRemove", Time); }
        }
    }
    /// <summary>
    /// 如果DefUP( float Time )的Time不等于0，不需要调用此函数，如果Time等于0，需要在防御力恢复时调用此函数
    /// </summary>
    void DefUpRemove()
    {
        if (isDefUp)
        {
            if (GetComponent<Empty>() != null)
            {
                isDefUp = false;
                playerUIState.StateDestory(13);
            }
        }
    }

    //===========================================================================敌人防御力提升的函数=====================================================================================




    //===========================================================================敌人防御力下降的函数=====================================================================================

    /// <summary>
    /// 代表防御力是否下降
    /// </summary>
    bool isDefDown;

    /// <summary>
    /// 此方法并不会直接改变敌人的数值，仅是给敌人添加一个UI标志.调用此函数时，如果防御力未被下降，状态变为防御力被下降
    /// </summary>
    /// <param name="Time"> 下降的时间，如果为零时间为无限，需要手动Remove </param>
    public void DefDown(float Time)
    {
        if (GetComponent<Empty>() != null)
        {
            isDefDown = true;
            playerUIState.StatePlus(14);
            if (Time != 0) { Invoke("DefDownRemove", Time); }

        }
    }
    /// <summary>
    /// 如果DefDown( float Time )的Time不等于0，不需要调用此函数，如果Time等于0，需要在防御力恢复时调用此函数
    /// </summary>
    void DefDownRemove()
    {
        if (isDefDown)
        {
            if (GetComponent<Empty>() != null)
            {
                isDefDown = false;
                playerUIState.StateDestory(14);
            }
        }
    }

    //===========================================================================敌人防御力下降的函数=====================================================================================




    //===========================================================================敌人特攻力提升的函数=====================================================================================

    /// <summary>
    /// 代表特攻力是否提升
    /// </summary>
    bool isSpAUp;

    /// <summary>
    /// 此方法并不会直接改变敌人的数值，仅是给敌人添加一个UI标志.调用此函数时，如果特攻力未被提升，状态变为特攻力被提升
    /// </summary>
    /// <param name="Time"> 提升的时间，如果为零时间为无限，需要手动Remove </param>
    public void SpAUP(float Time)
    {
        if (GetComponent<Empty>() != null)
        {
            isSpAUp = true;
            playerUIState.StatePlus(15);
            if (Time != 0) { Invoke("SpAUpRemove", Time); }

        }
    }
    /// <summary>
    /// 如果SpAUP( float Time )的Time不等于0，不需要调用此函数，如果Time等于0，需要在特攻力恢复时调用此函数
    /// </summary>
    void SpAUpRemove()
    {
        if (isSpAUp)
        {
            if (GetComponent<Empty>() != null)
            {
                isSpAUp = false;
                playerUIState.StateDestory(15);
            }
        }
    }

    //===========================================================================敌人特攻力提升的函数=====================================================================================




    //===========================================================================敌人特攻力下降的函数=====================================================================================

    /// <summary>
    /// 代表特攻力是否下降
    /// </summary>
    bool isSpADown;

    /// <summary>
    /// 此方法并不会直接改变敌人的数值，仅是给敌人添加一个UI标志.调用此函数时，如果特攻力未被下降，状态变为特攻力被下降
    /// </summary>
    /// <param name="Time"> 下降的时间，如果为零时间为无限，需要手动Remove </param>
    public void SpADown(float Time)
    {
        if (GetComponent<Empty>() != null)
        {
            isSpADown = true;
            playerUIState.StatePlus(16);
            if (Time != 0) { Invoke("SpADownRemove", Time); }

        }
    }
    /// <summary>
    /// 如果SpADown( float Time )的Time不等于0，不需要调用此函数，如果Time等于0，需要在特攻力恢复时调用此函数
    /// </summary>
    void SpADownRemove()
    {
        if (isSpADown)
        {
            if (GetComponent<Empty>() != null)
            {
                isSpADown = false;
                playerUIState.StateDestory(16);
            }
        }
    }

    //===========================================================================敌人特攻力下降的函数=====================================================================================




    //===========================================================================敌人特防力提升的函数=====================================================================================

    /// <summary>
    /// 代表特防力是否提升
    /// </summary>
    bool isSpDUP;

    /// <summary>
    /// 此方法并不会直接改变敌人的数值，仅是给敌人添加一个UI标志.调用此函数时，如果特防力未被提升，状态变为特防力被提升
    /// </summary>
    /// <param name="Time"> 提升的时间，如果为零时间为无限，需要手动Remove </param>
    public void SpDUP(float Time)
    {
        if (GetComponent<Empty>() != null)
        {
            isSpDUP = true;
            playerUIState.StatePlus(17);
            if (Time != 0) { Invoke("SpDUpRemove", Time); }

        }
    }
    /// <summary>
    /// 如果SpDUP( float Time )的Time不等于0，不需要调用此函数，如果Time等于0，需要在特防力恢复时调用此函数
    /// </summary>
    void SpDUpRemove()
    {
        if (isSpDUP)
        {
            if (GetComponent<Empty>() != null)
            {
                isSpDUP = false;
                playerUIState.StateDestory(17);
            }
        }
    }

    //===========================================================================敌人特防力提升的函数=====================================================================================




    //===========================================================================敌人特防力下降的函数=====================================================================================

    /// <summary>
    /// 代表特防力是否下降
    /// </summary>
    public bool isSpdDown;

    /// <summary>
    /// 此方法并不会直接改变敌人的数值，仅是给敌人添加一个UI标志.调用此函数时，如果特防力未被下降，状态变为特防力被下降
    /// </summary>
    /// <param name="Time"> 下降的时间，如果为零时间为无限，需要手动Remove </param>
    public void SpDDown(float Time)
    {
        if (GetComponent<Empty>() != null)
        {
            isSpdDown = true;
            playerUIState.StatePlus(18);
            if (Time != 0) { Invoke("SpDDownRemove", Time); }

        }
    }
    /// <summary>
    /// 如果SpDDown( float Time )的Time不等于0，不需要调用此函数，如果Time等于0，需要在特防力恢复时调用此函数
    /// </summary>
    public void SpDDownRemove()
    {
        if (isSpdDown)
        {
            if (GetComponent<Empty>() != null)
            {
                isSpdDown = false;
                playerUIState.StateDestory(18);
            }
        }
    }

    //===========================================================================敌人特防力下降的函数=====================================================================================



    //===========================================================================敌人寒冷（更容易被冰冻）的函数=====================================================================================

    /// <summary>
    /// 代表是否寒冷
    /// </summary>
    public int isColdDown;

    /// <summary>
    /// 调用此函数时，如果未被寒冷，状态变为寒冷
    /// </summary>
    /// <param name="Time"> 下降的时间，如果为零时间为无限，需要手动Remove </param>
    public void Cold(float Time)
    {
        if (GetComponent<Empty>() != null)
        {
            isColdDown += 1;
            playerUIState.StatePlus(19);
            if (Time != 0) { Invoke("ColdRemove", Time); }
        }
    }
    /// <summary>
    /// 如果Cold( float Time )的Time不等于0，不需要调用此函数，
    /// </summary>
    public void ColdRemove()
    {
        Debug.Log(isColdDown);
        if (isColdDown != 0)
        {
            if (GetComponent<Empty>() != null)
            {
                isColdDown = 0;
                playerUIState.StateDestory(19);
            }
        }
    }

    //===========================================================================敌人寒冷（更容易被冰冻）的函数=====================================================================================






    //===========================================================================敌人被诅咒的函数=====================================================================================

    public bool isEmptyCurseDone = false;
    public bool isEmptyCurseStart = false;
    float EmptyCursePoint;

    /// <summary>
    /// 调用此函数时，如果还未诅咒，状态变为诅咒
    /// </summary>
    /// <param name="InfatuationTimer">诅咒的持续时间（通常为0）</param>
    /// <param name="InfatuationPoint">诅咒的点数</param>
    public void EmptyCurse(float CurseTimer, float CursePoint)
    {
        if (!isEmptyCurseDone)
        {
            EmptyCursePoint += CursePoint * OtherStateResistance;
            if (!isEmptyCurseStart && EmptyCursePoint < 1)
            {
                playerUIState.StatePlus(20);
                playerUIState.StateSlowUP(20, EmptyCursePoint);
                isEmptyCurseStart = true;
            }
            else if (isEmptyCurseStart && EmptyCursePoint < 1)
            {
                playerUIState.StateSlowUP(20, EmptyCursePoint);
            }
            else if (EmptyCursePoint >= 1 && !isEmptyCurseDone)
            {
                if (!isEmptyCurseStart)
                {
                    playerUIState.StatePlus(20);
                    playerUIState.StateSlowUP(20, EmptyCursePoint);
                    isEmptyCurseStart = true;
                }
                isEmptyCurseDone = true;
                playerUIState.StateSlowUP(20, EmptyCursePoint);
                if (CurseTimer != 0) { Invoke("EmptyCurseRemove", CurseTimer); }
            }
        }

    }
    //只可被上一个函数延迟调用，代表移除诅咒的函数
    public void EmptyCurseRemove()
    {
        if (isEmptyCurseDone)
        {
            EmptyCursePoint = 0;
            playerUIState.StateSlowUP(20, 0);
            isEmptyCurseStart = false;
            isEmptyCurseDone = false;
            playerUIState.StateDestory(20);
        }
    }


    //===========================================================================敌人被诅咒的函数=====================================================================================









    //***************************************************************************对敌人的函数*********************************************************************************









    //***************************************************************************对自己的函数*********************************************************************************


    //===========================================================================改变速度的函数=====================================================================================

    //一个变量代表速度是否被改变，一个代表改变的倍率
    bool isSpeedChange;

    //调用此函数时，如果速度还未被改变，改变速度并改变颜色，状态变为被改变
    public void SpeedChange()
    {
        if (!isSpeedChange)
        {
            PlayerData Pdata = GetComponent<PlayerData>();
            if (Pdata != null && !Pdata.isMist)
            {
                Pdata.MoveSpwBounsAlways -= 4;
                GetComponent<PlayerControler>().ReFreshAbllityPoint();
                isSpeedChange = true;
                playerUIState.StatePlus(0);
                MarterialChangeToSpeedDown();
                animator.speed = 0.55f;
            }
            else if (GetComponent<Empty>() != null)
            {
                speed *= 0.5f;
                isSpeedChange = true;
                playerUIState.StatePlus(0);
                MarterialChangeToSpeedDown();
                animator.speed = 0.7f;
            }
        }
    }
    //调用此函数时，延迟指定时间调用下一个函数
    public void SpeedRemove01(float SpeedChangeTime)
    {
        Invoke("SpeedRemove02", SpeedChangeTime);
    }
    //只可被上一个函数延迟调用，代表速度恢复的函数
    void SpeedRemove02()
    {
        if (isSpeedChange)
        {
            playerUIState.StateDestory(0);
            // GetComponent<SpriteRenderer>().material.color = new Color(1, 1, 1, 1);
            MarterialChangeToNurmal();
            animator.speed = 1;
            if (GetComponent<PlayerData>() != null)
            {
                GetComponent<PlayerData>().MoveSpwBounsAlways += 4;
                GetComponent<PlayerControler>().ReFreshAbllityPoint();
                isSpeedChange = false;
            }
            else if (GetComponent<Empty>() != null)
            {
                speed *= 2f;
                isSpeedChange = false;
            }
        }
    }

    //===========================================================================改变速度的函数=====================================================================================


    //===========================================================================中毒的函数=====================================================================================


    //一个变量代表是否中毒，一个代表中毒的程度
    public bool isToxicDef;
    public bool isToxicDone;
    public bool isToxicStart;
    public float ToxicPointFloat;
    float SpAHWBeforeChange;

    //调用此函数时，如果还未开始中毒，开始中毒
    public void ToxicFloatPlus(float ToxicPoint)
    {
        if (!isStateInvincible && !isToxicDef)
        {
            ToxicPointFloat += ToxicPoint;
            ToxicPointFloat = (ToxicPointFloat > 1 ? 1 : ToxicPointFloat);
            if (!isToxicStart && ToxicPointFloat < 1)
            {
                playerUIState.StatePlus(3);
                playerUIState.StateSlowUP(3, ToxicPointFloat);
                isToxicStart = true;
            }
            else if (isToxicStart && ToxicPointFloat < 1)
            {
                playerUIState.StateSlowUP(3, ToxicPointFloat);
            }
            else if (ToxicPointFloat >= 1 && !isToxicDone)
            {
                if (!isToxicStart)
                {
                    playerUIState.StatePlus(3);
                    playerUIState.StateSlowUP(3, ToxicPointFloat);
                    isToxicStart = true;
                }
                isToxicDone = true;
                playerUIState.StateSlowUP(3, ToxicPointFloat);
                if (transform.GetComponent<PlayerControler>() != null)
                {
                    PlayerControler player = transform.GetComponent<PlayerControler>();
                    player.KnockOutPoint = 1;
                    //player.ChangeHp((float)(-player.maxHp / 20), 0, 0);
                    player.playerData.SpABounsAlways--;
                    SpAHWBeforeChange = player.SpAAbilityPoint * 0.3f;
                    player.playerData.SpAHardWorkAlways -= SpAHWBeforeChange;
                    player.ReFreshAbllityPoint();
                }
                MarterialChangeToToxic();
            }
            if(GetComponent<PlayerControler>() != null)
            {
                isStateInvincible = true;
                StateInvincileTimer = TimeStateInvincible;
            }
        }
    }

    //只可被上一个函数延迟调用，代表解冻的函数
    public void ToxicRemove()
    {
        if (isToxicStart)
        {
            ToxicPointFloat = 0;
            playerUIState.StateSlowUP(3, 0);
            playerUIState.StateDestory(3);
            if (transform.GetComponent<PlayerControler>() != null && isToxicDone)
            {
                PlayerControler player = transform.GetComponent<PlayerControler>();
                player.playerData.SpABounsAlways++;
                player.playerData.SpAHardWorkAlways += SpAHWBeforeChange;
                player.ReFreshAbllityPoint();
            }
            isToxicStart = false;
            isToxicDone = false;
            if (transform.GetComponent<PlayerControler>() != null )
            {
                transform.GetComponent<PlayerControler>().ReFreshAbllityPoint();
            }
            MarterialChangeToNurmal();

        }
    }

    //===========================================================================中毒的函数=====================================================================================


    //===========================================================================麻痹的函数=====================================================================================


    public bool isParalysisDef;
    public delegate void ParalysisDoneHappend(PlayerControler player);
    public ParalysisDoneHappend ParalysisDoneHappendEvent;
    public ParalysisDoneHappend ParalysisRemoveHappendEvent;

    //一个变量代表是否麻痹，一个代表麻痹的程度

    public bool isParalysisDone;
    public bool isParalysisStart ;
    public float ParalysisPointFloat;
    float MoveSpeHWBeforeChange;

    //调用此函数时，如果还未开始中毒，开始中毒
    public void ParalysisFloatPlus(float ParalysisPoint)
    {
        if (!isStateInvincible && !isParalysisDef)
        {
            ParalysisPointFloat += ParalysisPoint;
            ParalysisPointFloat = (ParalysisPointFloat > 1 ? 1 : ParalysisPointFloat);

            if (!isParalysisStart && ParalysisPointFloat < 1)
            {
                playerUIState.StatePlus(4);
                playerUIState.StateSlowUP(4, ParalysisPointFloat);
                isParalysisStart = true;
            }
            else if (isParalysisStart && ParalysisPointFloat < 1)
            {
                playerUIState.StateSlowUP(4, ParalysisPointFloat);
            }
            else if (ParalysisPointFloat >= 1 && !isParalysisDone)
            {
                if (!isParalysisStart)
                {
                    playerUIState.StatePlus(4);
                    playerUIState.StateSlowUP(4, ParalysisPointFloat);
                    isParalysisStart = true;
                }
                isParalysisDone = true;
                playerUIState.StateSlowUP(4, ParalysisPointFloat);
                if (transform.GetComponent<PlayerControler>() != null)
                {
                    PlayerControler player = transform.GetComponent<PlayerControler>();
                    player.playerData.SpeBounsAlways--;
                    player.playerData.MoveSpwBounsAlways--;
                    MoveSpeHWBeforeChange = player.speed * 0.3f;
                    player.playerData.MoveSpeHardWorkAlways -= MoveSpeHWBeforeChange;
                    player.ReFreshAbllityPoint();
                    if (ParalysisDoneHappendEvent != null)
                    {
                        ParalysisDoneHappendEvent(player);
                        player.ReFreshAbllityPoint();

                    }

                }
                MarterialChangeToParalysis();
            }
            if (GetComponent<PlayerControler>() != null)
            {
                isStateInvincible = true;
                StateInvincileTimer = TimeStateInvincible;
            }
        }
    }

    //只可被上一个函数延迟调用，代表解冻的函数
    public void ParalysisRemove()
    {
        if (isParalysisStart)
        {
            ParalysisPointFloat = 0;
            playerUIState.StateSlowUP(4, 0);
            playerUIState.StateDestory(4);
            if (transform.GetComponent<PlayerControler>() != null && isParalysisDone)
            {
                PlayerControler player = transform.GetComponent<PlayerControler>();
                player.playerData.SpeBounsAlways++;
                player.playerData.MoveSpwBounsAlways++;
                player.playerData.MoveSpeHardWorkAlways += MoveSpeHWBeforeChange;
                player.ReFreshAbllityPoint();
                if(ParalysisRemoveHappendEvent != null)
                {
                    ParalysisRemoveHappendEvent(player);
                    player.ReFreshAbllityPoint();
                }
            }
            isParalysisStart = false;
            isParalysisDone = false;
            if (transform.GetComponent<PlayerControler>() != null )
            {
                transform.GetComponent<PlayerControler>().ReFreshAbllityPoint();
            }
            MarterialChangeToNurmal();

        }
    }

    //===========================================================================麻痹的函数=====================================================================================


    //===========================================================================烧伤的函数=====================================================================================

    //一个变量代表是否中毒，一个代表中毒的程度


    public bool isBurnDef;
    public bool isBurnDone;
    public bool isBurnStart;
    public float BurnPointFloat;
    float AtkHWBeforeChange;

    //调用此函数时，如果还未开始中毒，开始中毒
    public void BurnFloatPlus(float BurnPoint)
    {

        if (!isStateInvincible && !isBurnDef)
        {
            BurnPointFloat += BurnPoint;
            BurnPointFloat = (BurnPointFloat > 1 ? 1 : BurnPointFloat);

            if (!isBurnStart && BurnPointFloat < 1)
            {
                playerUIState.StatePlus(5);
                playerUIState.StateSlowUP(5, BurnPointFloat);
                isBurnStart = true;
            }
            else if (isBurnStart && BurnPointFloat < 1)
            {
                playerUIState.StateSlowUP(5, BurnPointFloat);
            }
            else if (BurnPointFloat >= 1 && !isBurnDone)
            {
                if (!isBurnStart)
                {
                    playerUIState.StatePlus(5);
                    playerUIState.StateSlowUP(5, BurnPointFloat);
                    isBurnStart = true;
                }
                isBurnDone = true;
                playerUIState.StateSlowUP(5, BurnPointFloat);
                if (transform.GetComponent<PlayerControler>() != null)
                {
                    PlayerControler player = transform.GetComponent<PlayerControler>();
                    player.KnockOutPoint = 1;
                    player.ChangeHp((float)(-player.maxHp / 20), 0, 0);
                    player.playerData.AtkBounsAlways--;
                    AtkHWBeforeChange = player.AtkAbilityPoint * 0.3f;
                    player.playerData.AtkHardWorkAlways -= AtkHWBeforeChange;
                    player.ReFreshAbllityPoint();
                }
                MarterialChangeToBurn();
            }
            if (GetComponent<PlayerControler>() != null)
            {
                isStateInvincible = true;
                StateInvincileTimer = TimeStateInvincible;
            }
        }
    }

    //只可被上一个函数延迟调用，代表解冻的函数
    public void BurnRemove()
    {
        if (isBurnStart)
        {
            
            BurnPointFloat = 0;
            playerUIState.StateSlowUP(5, 0);
            playerUIState.StateDestory(5);
            if (transform.GetComponent<PlayerControler>() != null && isBurnDone)
            {
                PlayerControler player = transform.GetComponent<PlayerControler>();
                player.playerData.AtkBounsAlways++;
                player.playerData.AtkHardWorkAlways += AtkHWBeforeChange;
                player.ReFreshAbllityPoint();
            }
            isBurnStart = false;
            isBurnDone = false;
            if (transform.GetComponent<PlayerControler>() != null )
            {
                transform.GetComponent<PlayerControler>().ReFreshAbllityPoint();
            }
            MarterialChangeToNurmal();
        }
    }

    //===========================================================================烧伤的函数=====================================================================================

    //===========================================================================睡眠的函数=====================================================================================

    //一个变量代表是否中毒，一个代表中毒的程度

    public bool isSleepDef;
    public bool isSleepDone;
    public bool isSleepStart;
    public float SleepPointFloat;

    //调用此函数时，如果还未开始中毒，开始中毒
    public void SleepFloatPlus(float SleepPoint)
    {
        if (!isStateInvincible && !isSleepDef)
        {
            SleepPointFloat += SleepPoint;
            SleepPointFloat = (SleepPointFloat > 1 ? 1 : SleepPointFloat);

            if (!isSleepStart && SleepPointFloat < 1)
            {
                playerUIState.StatePlus(6);
                playerUIState.StateSlowUP(6, SleepPointFloat);
                isSleepStart = true;
            }
            else if (isSleepStart && SleepPointFloat < 1)
            {
                playerUIState.StateSlowUP(6, SleepPointFloat);
            }
            else if (SleepPointFloat >= 1 && !isSleepDone)
            {
                if (!isSleepStart)
                {
                    playerUIState.StatePlus(6);
                    playerUIState.StateSlowUP(6, SleepPointFloat);
                    isSleepStart = true;
                }
                isSleepDone = true;
                playerUIState.StateSlowUP(6, SleepPointFloat);
                if (transform.GetComponent<PlayerControler>() != null)
                {
                    PlayerControler player = transform.GetComponent<PlayerControler>();
                    player.playerData.DefBounsAlways--;
                    player.playerData.SpDBounsAlways--;
                    player.ReFreshAbllityPoint();
                }
                MarterialChangeToSleep();
            }
            if (GetComponent<PlayerControler>() != null)
            {
                isStateInvincible = true;
                StateInvincileTimer = TimeStateInvincible;
            }
        }
    }

    //只可被上一个函数延迟调用，代表解冻的函数
    public void SleepRemove()
    {
        if (isSleepStart)
        {
            
            SleepPointFloat = 0;
            playerUIState.StateSlowUP(6, 0);
            animator.speed = 1;
            playerUIState.StateDestory(6);
            if (transform.GetComponent<PlayerControler>() != null && isSleepDone)
            {
                PlayerControler player = transform.GetComponent<PlayerControler>();
                player.playerData.DefBounsAlways++;
                player.playerData.SpDBounsAlways++;
                player.ReFreshAbllityPoint();
            }
            isSleepStart = false;
            isSleepDone = false;
            if (transform.GetComponent<PlayerControler>() != null)
            {
                transform.GetComponent<PlayerControler>().ReFreshAbllityPoint();
            }
            MarterialChangeToNurmal();
        }
    }

    //===========================================================================睡眠的函数=====================================================================================


    //一个变量代表是否中毒，一个代表中毒的程度

    public bool isConfusionDef;
    public bool isConfusionDone;

    //调用此函数时，如果还未开始中毒，开始中毒
    public void ConfusionFloatPlus()
    {
        if (!isStateInvincible && !isConfusionDef && !isConfusionDone)
        {
                
            playerUIState.StatePlus(9);
            playerUIState.StateSlowUP(9, 1);
            isConfusionDone = true;
        }
    }

    //只可被上一个函数延迟调用，代表解冻的函数
    public void ConfusionRemove()
    {
        if (isConfusionDone)
        {
            playerUIState.StateSlowUP(9, 0);
            playerUIState.StateDestory(9);
            isConfusionDone = false;
        }
    }


    //===========================================================================混乱的函数=====================================================================================






    //===========================================================================进化时复制目前状态的函数=====================================================================================

    protected void CopyState(PlayerControler CopyTarget)
    {

        CopyTarget.isToxicDef = isToxicDef;
        CopyTarget.isToxicDone = isToxicDone;
        if (isToxicDone) { CopyTarget.MarterialChangeToToxic(); }
        CopyTarget.isToxicStart = isToxicStart;
        CopyTarget.ToxicPointFloat = ToxicPointFloat;
        CopyTarget.SpAHWBeforeChange = SpAHWBeforeChange;

        CopyTarget.isParalysisDef = isParalysisDef;
        CopyTarget.ParalysisDoneHappendEvent = ParalysisDoneHappendEvent;
        CopyTarget.ParalysisRemoveHappendEvent = ParalysisRemoveHappendEvent;
        CopyTarget.isParalysisDone = isParalysisDone;
        if (isParalysisDone) { CopyTarget.MarterialChangeToParalysis(); }
        CopyTarget.isParalysisStart = isParalysisStart;
        CopyTarget.ParalysisPointFloat = ParalysisPointFloat;
        CopyTarget.MoveSpeHWBeforeChange = MoveSpeHWBeforeChange;

        CopyTarget.isBurnDef = isBurnDef;
        CopyTarget.isBurnDone = isBurnDone;
        if (isBurnDone) { CopyTarget.MarterialChangeToBurn(); }
        CopyTarget.isBurnStart = isBurnStart;
        CopyTarget.BurnPointFloat = BurnPointFloat;
        CopyTarget.AtkHWBeforeChange = AtkHWBeforeChange;

        CopyTarget.isSleepDef = isSleepDef;
        CopyTarget.isSleepDone = isSleepDone;
        if (isSleepDone) { CopyTarget.MarterialChangeToSleep(); }
        CopyTarget.isSleepStart = isSleepStart;
        CopyTarget.SleepPointFloat = SleepPointFloat;

        CopyTarget.isConfusionDef = isConfusionDef;
        CopyTarget.isConfusionDone = isConfusionDone;

        for(int i = 0; i< playerUIState.transform.childCount; i++)
        {
            Instantiate(playerUIState.transform.GetChild(i), Vector3.zero, Quaternion.identity, CopyTarget.playerUIState.transform);
        }


    }

    //===========================================================================进化时复制目前状态的函数=====================================================================================


    //***************************************************************************对自己的函数*********************************************************************************




    //***************************************************************************对全体的函数*********************************************************************************

    /// <summary>
    /// 当有宝可梦的生命值改变时，使用此方法，降低生命时选择两个威力，并设回复量为0。
    /// 此方法会自动把威力转换为实际的伤害值，如果想让宝可梦的生命值减少指定的“值”，选择属性为Type.TypeEnum.IgnoreType
    /// （例如当AtkPower=10，属性不等于IgnoreType时，对宝可梦造成“威力等于10的”一次伤害，属性等于IgnoreType时，对宝可梦造成“10点”伤害）
    /// </summary>
    /// <param name="Attacker">攻击的制造者</param>
    /// <param name="Attacked">被攻击者</param>
    /// <param name="AtkPower">此次伤害的物理威力</param>
    /// <param name="SpAPower">此次伤害的特攻威力</param>
    /// <param name="HpUpValue">此次改变不是伤害，而是回复</param>
    /// <param name="SkillType">此次伤害的属性</param>

    public static void PokemonHpChange(GameObject Attacker , GameObject Attacked , float AtkPower , float SpAPower ,int HpUpValue , Type.TypeEnum SkillType)
    {
        //决定攻击者
        int AttackerATK = 1;
        int AttackerSpA = 1;
        int AttackerLevel = 1;
        float EmptyTypeAlpha = 1;
        if (Attacker != null)
        {
            
            Empty EmptyAttacker = Attacker.GetComponent<Empty>();
            PlayerControler PlayerAttacker = Attacker.GetComponent<PlayerControler>();
            FollowBaby FollowBabyAttacker = Attacker.GetComponent<FollowBaby>();
            if (EmptyAttacker != null)  { 
                AttackerATK = EmptyAttacker.AtkAbilityPoint ;  AttackerSpA = EmptyAttacker.SpAAbilityPoint ; AttackerLevel = EmptyAttacker.Emptylevel;
                EmptyTypeAlpha = ((SkillType == EmptyAttacker.EmptyType01) || (SkillType == EmptyAttacker.EmptyType02)) ? 1.5f : 1;
            }
            if (PlayerAttacker != null) {
                AttackerATK = PlayerAttacker.AtkAbilityPoint; AttackerSpA = PlayerAttacker.SpAAbilityPoint; AttackerLevel = PlayerAttacker.Level;
                EmptyTypeAlpha = ((int)SkillType == PlayerAttacker.PlayerType01 ? 1.5f : 1) * ((int)SkillType == PlayerAttacker.PlayerType02 ? 1.5f : 1) * (PlayerAttacker.PlayerTeraTypeJOR == 0 ? ((int)SkillType == PlayerAttacker.PlayerTeraType ? 1.5f : 1) : ((int)SkillType == PlayerAttacker.PlayerTeraTypeJOR ? 1.5f : 1));
            }
            if (FollowBabyAttacker != null) { AttackerATK = FollowBabyAttacker.BabyAtk(); AttackerSpA = FollowBabyAttacker.BabySpA(); AttackerLevel = FollowBabyAttacker.BabyLevel(); }
        }


        //决定被攻击者
        if (Attacked.GetComponent<Empty>() != null)
        {
            Empty EmptyAttacked = Attacked.GetComponent<Empty>();
            if (HpUpValue == 0) {
                float WeatherDefAlpha = ((Weather.GlobalWeather.isSandstorm ? ((EmptyAttacked.EmptyType01 == Type.TypeEnum.Rock || EmptyAttacked.EmptyType02 == Type.TypeEnum.Rock) ? 1.5f : 1) : 1));
                float WeatherSpDAlpha = ((Weather.GlobalWeather.isHail ? ((EmptyAttacked.EmptyType01 == Type.TypeEnum.Ice || EmptyAttacked.EmptyType02 == Type.TypeEnum.Ice) ? 1.5f : 1) : 1));
                /*float WeatherAlpha = ((Weather.GlobalWeather.isRain && SkillType == Type.TypeEnum.Water) ? (Weather.GlobalWeather.isRainPlus ? 1.8f : 1.3f) : 1)
                    * ((Weather.GlobalWeather.isRain && SkillType == Type.TypeEnum.Fire) ? 0.5f : 1)
                    * ((Weather.GlobalWeather.isSunny && SkillType == Type.TypeEnum.Water) ? 0.5f : 1)
                    * ((Weather.GlobalWeather.isSunny && SkillType == Type.TypeEnum.Fire) ? (Weather.GlobalWeather.isSunnyPlus ? 1.8f : 1.3f) : 1);
                */

                if (SkillType != Type.TypeEnum.IgnoreType)
                {
                    EmptyAttacked.EmptyHpChange(
                    Mathf.Clamp((AtkPower * (Attacker == null ? 1 : AttackerATK) * EmptyTypeAlpha /* WeatherAlpha */ * (Attacker == null ? 1 : (2 * AttackerLevel + 10))) / (250 * EmptyAttacked.DefAbilityPoint * WeatherDefAlpha + 2) , 1 , 10000),
                    Mathf.Clamp((SpAPower * (Attacker == null ? 1 : AttackerSpA) * EmptyTypeAlpha /* WeatherAlpha */ * (Attacker == null ? 1 : (2 * AttackerLevel + 10))) / (250 * EmptyAttacked.SpdAbilityPoint * WeatherSpDAlpha + 2), 1, 10000),
                    (int)SkillType);
                }
                else
                {
                    EmptyAttacked.EmptyHpChange(AtkPower, SpAPower, 19);
                }
            }
            else
            {
                EmptyAttacked.EmptyHpChange(-HpUpValue, 0, 19);
            }
        }
        if (Attacked.GetComponent<PlayerControler>() != null)
        {
            PlayerControler PlayerAttacked = Attacked.GetComponent<PlayerControler>();
            if (HpUpValue == 0)
            {
                PlayerAttacked.ChangeHp(
                         Mathf.Clamp((-AtkPower * (Attacker == null ? 1 : AttackerATK) * (Attacker == null ? 1 : (2 * AttackerLevel + 10))) / ((int)SkillType != 19 ? 250 : 1), -10000, -1),
                         Mathf.Clamp((-SpAPower * (Attacker == null ? 1 : AttackerSpA) * (Attacker == null ? 1 : (2 * AttackerLevel + 10))) / ((int)SkillType != 19 ? 250 : 1), -10000, -1),
                        (int)SkillType);
            }
            else
            {
                PlayerAttacked.ChangeHp(HpUpValue, 0, 19);
            }
        }
        if(Attacked.GetComponent<Substitute>() != null)
        {
            Substitute SubstotuteAttacked = Attacked.GetComponent<Substitute>();
            if (HpUpValue == 0)
            {
                SubstotuteAttacked.SubStituteChangeHp(
                    (-AtkPower * (Attacker == null ? 1 : AttackerATK) * (Attacker == null ? 1 : (2 * AttackerLevel + 10))) / (250),
                    (-SpAPower * (Attacker == null ? 1 : AttackerSpA) * (Attacker == null ? 1 : (2 * AttackerLevel + 10))) / (250),
                    (int)SkillType);
            }
        }
    }

    //***************************************************************************对全体的函数*********************************************************************************


}

