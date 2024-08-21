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
    public Material TeraMaterial;


    public float FrozenResistance = 1f;
    public float ToxicResistance = 1f;
    public float ParalysisResistance = 1f;
    public float BurnResistance = 1f;
    public float SleepResistance = 1f;
    public float OtherStateResistance = 1f;

    public float TimeStateInvincible;
    protected float StateInvincileTimer = 0.0f;
    protected bool isStateInvincible = false;


    //当前宝可梦处于青草场地中
    public bool isInGrassyTerrain
    {
        get { return isinGrassyTerrain; }
        set { isinGrassyTerrain = value; }
    }
    bool isinGrassyTerrain = false;

    //当前宝可梦处于精神场地中Psychic Terrain
    public bool isInPsychicTerrain
    {
        get { return isinPsychicTerrain; }
        set { isinPsychicTerrain = value; }
    }
    bool isinPsychicTerrain = false;

    //当前宝可梦处于电气场地中Electric Terrain
    public bool isInElectricTerrain
    {
        get { return isinElectricTerrain; }
        set { isinElectricTerrain = value; }
    }
    bool isinElectricTerrain = false;

    //当前宝可梦处于薄雾场地中Misty Terrain
    public bool isInMistyTerrain
    {
        get { return isinMistyTerrain; }
        set { isinMistyTerrain = value; }
    }
    bool isinMistyTerrain = false;



    //当前宝可梦处超级于青草场地中
    public bool isInSuperGrassyTerrain
    {
        get { return isinSuperGrassyTerrain; }
        set { isinSuperGrassyTerrain = value; }
    }
    bool isinSuperGrassyTerrain = false;

    //当前宝可梦处于超级精神场地中Psychic Terrain
    public bool isInSuperPsychicTerrain
    {
        get { return isinSuperPsychicTerrain; }
        set { isinSuperPsychicTerrain = value; }
    }
    bool isinSuperPsychicTerrain = false;

    //当前宝可梦处于超级电气场地中Electric Terrain
    public bool isInSuperElectricTerrain
    {
        get { return isinSuperElectricTerrain; }
        set { isinSuperElectricTerrain = value; }
    }
    bool isinSuperElectricTerrain = false;

    //当前宝可梦处于超级薄雾场地中Misty Terrain
    public bool isInSuperMistyTerrain
    {
        get { return isinSuperMistyTerrain; }
        set { isinSuperMistyTerrain = value; }
    }
    bool isinSuperMistyTerrain = false;

    //处于神秘守护状态
    public bool isSafeguard;



    [Tooltip("pokemon主体的渲染组件集")]
    public List<SpriteRenderer> skinRenderers;

    //声明一个动画管理者变量，获取动画管理者
    public Animator animator;

    public void MarterialChangeToNurmal()
    {
        SetSkinRenderersMaterial(NormalMaterial);
        PlayerControler player = GetComponent<PlayerControler>();

        if (isEmptyFrozenDone)
        {
            MarterialChangeToFrozen();
        }
        else if (player != null &&( player.PlayerTeraTypeJOR != 0 || player.PlayerTeraType != 0))
        {
            if (player.PlayerTeraType != 0) { MarterialChangeToTera(player.PlayerTeraType); }
            else if (player.PlayerTeraTypeJOR != 0) { MarterialChangeToTera(player.PlayerTeraTypeJOR); }
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

    }


    public void MarterialChangeToFrozen()
    {
        if (!GetSkinRenderersMaterial().HasFloat("_MaterialPriority") || FrozenMaterial.GetFloat("_MaterialPriority") >= GetSkinRenderersMaterial().GetFloat("_MaterialPriority"))
        {
            SetSkinRenderersMaterial(FrozenMaterial);
        }
        animator.speed = 0;
    }
    public void MarterialChangeToSpeedDown()
    {
        if (!GetSkinRenderersMaterial().HasFloat("_MaterialPriority") || SpeedDownMaterial.GetFloat("_MaterialPriority") >= GetSkinRenderersMaterial().GetFloat("_MaterialPriority"))
        {
            SetSkinRenderersMaterial(SpeedDownMaterial);
        }
    }
    public void MarterialChangeToToxic()
    {
        if (!GetSkinRenderersMaterial().HasFloat("_MaterialPriority") || ToxicMaterial.GetFloat("_MaterialPriority") >= GetSkinRenderersMaterial().GetFloat("_MaterialPriority"))
        {
            SetSkinRenderersMaterial(ToxicMaterial);
        }
    }

    public void MarterialChangeToParalysis()
    {
        if (!GetSkinRenderersMaterial().HasFloat("_MaterialPriority") || ParalysisMaterial.GetFloat("_MaterialPriority") >= GetSkinRenderersMaterial().GetFloat("_MaterialPriority"))
        {
            SetSkinRenderersMaterial(ParalysisMaterial);
        }
    }

    public void MarterialChangeToBurn()
    {
        if (!GetSkinRenderersMaterial().HasFloat("_MaterialPriority") || BurnMaterial.GetFloat("_MaterialPriority") >= GetSkinRenderersMaterial().GetFloat("_MaterialPriority"))
        {
            SetSkinRenderersMaterial(BurnMaterial);
        }
    }

    public void MarterialChangeToSleep()
    {
        if (!GetSkinRenderersMaterial().HasFloat("_MaterialPriority") || SleepMaterial.GetFloat("_MaterialPriority") >= GetSkinRenderersMaterial().GetFloat("_MaterialPriority"))
        {
            SetSkinRenderersMaterial(SleepMaterial);
            //animator.speed = 0.55f;
        }
    }

    public void MarterialChangeToFear()
    {
        if (!GetSkinRenderersMaterial().HasFloat("_MaterialPriority") || FearMaterial.GetFloat("_MaterialPriority") >= GetSkinRenderersMaterial().GetFloat("_MaterialPriority"))
        {
            SetSkinRenderersMaterial(FearMaterial);
        }
    }

    public void MarterialChangeToInfatuation()
    {
        if (!GetSkinRenderersMaterial().HasFloat("_MaterialPriority") || InfatuationMaterial.GetFloat("_MaterialPriority") >= GetSkinRenderersMaterial().GetFloat("_MaterialPriority"))
        {
            SetSkinRenderersMaterial(InfatuationMaterial);
        }
    }

    public void MarterialChangeToTera(int TeraType)
    {
        if (!GetSkinRenderersMaterial().HasFloat("_MaterialPriority") || TeraMaterial.GetFloat("_MaterialPriority") >= GetSkinRenderersMaterial().GetFloat("_MaterialPriority") ) {
            TeraMaterial.SetColor("_Color", Type.TeraTypeColor[TeraType]);
            SetSkinRenderersMaterial(TeraMaterial);
        }
    }

    //获取 pokemon 主体的渲染组件
    public List<SpriteRenderer> GetSkinRenderers()
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

    Material GetSkinRenderersMaterial()
    {
        List<SpriteRenderer> skinRendererList = GetSkinRenderers();
        return skinRendererList[0].material;
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
    public bool isFrozenDef;
    bool isFrozenDone = false;
    bool isFrozenStart = false;
    public float GetEmptyFrozenPointFloat
    {
        get { return EmptyFrozenPointFloat; }
        set { EmptyFrozenPointFloat = value; }
    }
    float EmptyFrozenPointFloat;
    float SpeedBefoerChange;
    public float FrozenTimeFloat
    {
        get { return frozenTimeFloat; }
        set { frozenTimeFloat = value; }
    }
    float frozenTimeFloat = 0.0f;


    //调用此函数时，如果还未被冰冻，冰冻，状态变为被冰冻
    public void Frozen(float FrozenTime, float FrozenPoint, float FrozenPer)
    {
        FrozenTimeFloat = FrozenTime;
        if (GetComponent<Empty>() != null && isColdDown != 0) { FrozenPer += 0.25f * isColdDown; }
        if (!isFrozenDef && Random.Range(0.0f, 1.0f) <= FrozenPer) {
            if (!isInMistyTerrain && !isFrozenDone)
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
                    Debug.Log(animator.speed);
                    foreach (SpriteRenderer s in skinRenderers)
                    {
                        Animator A = s.gameObject.GetComponent<Animator>();
                        if (A != null)
                        {
                            A.speed = 0;
                            
                        }
                    }
                    Invoke("FrozenRemove", FrozenTime * FrozenResistance);
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
            foreach (SpriteRenderer s in skinRenderers)
            {
                Animator A = s.gameObject.GetComponent<Animator>();
                if (A != null)
                {
                    A.speed = 1;
                }
            }
            FrozenTimeFloat = 0;
        }
    }

    //===========================================================================冷冻的函数=====================================================================================

    //===========================================================================害怕的函数=====================================================================================

    public bool isFearDone = false;
    public bool isFearStart = false;
    public bool isFearDef = false;
    public float GetEmptyFearPointFloat
    {
        get { return EmptyFearPointFloat; }
        set { EmptyFearPointFloat = value; }
    }
    float EmptyFearPointFloat;
    public float FearTimeFloat
    {
        get { return fearTimeFloat; }
        set { fearTimeFloat = value; }
    }
    float fearTimeFloat = 0.0f;

    //调用此函数时，如果还未害怕，状态变为害怕
    public void Fear(float FearTime, float FearPoint)
    {
        FearTimeFloat = FearTime;
        if (!isFearDone && !isFearDef)
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
    public void FearRemove()
    {
        if (isFearStart)
        {
            EmptyFearPointFloat = 0;
            playerUIState.StateSlowUP(7, 0);
            isFearStart = false;
            isFearDone = false;
            playerUIState.StateDestory(7);
            MarterialChangeToNurmal();
            FearTimeFloat = 0;
        }
    }

    //===========================================================================害怕的函数=====================================================================================


    //===========================================================================敌人致盲的函数=====================================================================================

    public bool isBlindDone = false;
    public bool isBlindStart = false;
    public bool isBlindDef = false;
    public float EmptyBlindPoint;
    public float BlindTimeFloat
    {
        get { return blindTimeFloat; }
        set { blindTimeFloat = value; }
    }
    float blindTimeFloat = 0.0f;

    //调用此函数时，如果还未害怕，状态变为害怕
    public void Blind(float BlindTimer, float BlinderPoint)
    {
        BlindTimeFloat = BlindTimer;
        if (!isBlindDone && !isBlindDef)
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
                if (BlindTimer != 0) { Invoke("BlindRemove", BlindTimer);  }
            }
        }

    }
    //只可被上一个函数延迟调用，代表速度害怕的函数
    public void BlindRemove()
    {
        if (isBlindStart)
        {
            EmptyBlindPoint = 0;
            playerUIState.StateSlowUP(1, 0);
            isBlindStart = false;
            isBlindDone = false;
            GetComponent<Empty>().isSilence = false;
            playerUIState.StateDestory(1);
            BlindTimeFloat = 0;
        }
    }

    //===========================================================================敌人致盲的函数=====================================================================================


    //===========================================================================敌人中毒的函数=====================================================================================
    public float ToxicTimeFloat
    {
        get { return toxicTimeFloat; }
        set { toxicTimeFloat = value; }
    }
    float toxicTimeFloat = 0.0f;
    //调用此函数时，如果还未开始中毒，开始中毒
    public void EmptyToxicDone(float ToxicPoint, float ToxicTime, float ToxicPer)
    {
        ToxicTimeFloat = ToxicTime;
        if (!isToxicDef && Random.Range(0.0f, 1.0f) <= ToxicPer)
        {
            Empty EmptyObj = GetComponent<Empty>();
            if (!isInMistyTerrain && !isToxicDef && EmptyObj.EmptyType01 != Type.TypeEnum.Poison && EmptyObj.EmptyType01 != Type.TypeEnum.Steel && EmptyObj.EmptyType02 != Type.TypeEnum.Poison && EmptyObj.EmptyType02 != Type.TypeEnum.Steel && !isToxicDone)
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
                    Invoke("EmptyToxicRemove", ToxicTime);
                    EmptyObj.SpAChange(-2,0.0f);

                }
            }
        }
    }

    public void EmptyToxicRemove()
    {
        if (isToxicStart)
        {
            ToxicPointFloat = 0;
            playerUIState.StateSlowUP(3, 0);
            isToxicStart = false;
            isToxicDone = false;
            playerUIState.StateDestory(3);
            MarterialChangeToNurmal();
            GetComponent<Empty>().SpAChange(2, 0.0f);
            ToxicTimeFloat = 0;
        }
    }

    //===========================================================================敌人中毒的函数=====================================================================================




    //===========================================================================敌人烧伤的函数=====================================================================================
    public float BurnTimeFloat
    {
        get { return burnTimeFloat; }
        set { burnTimeFloat = value; }
    }
    float burnTimeFloat = 0.0f;
    //调用此函数时，如果还未开始烧伤，开始烧伤
    public void EmptyBurnDone(float BurnPoint, float BurnTime, float BurnPer)
    {
        BurnTimeFloat = BurnTime;
        if (!isBurnDef && Random.Range(0.0f, 1.0f) <= BurnPer)
        {
            Empty EmptyObj = GetComponent<Empty>();
            if (!isInMistyTerrain && !isBurnDef && EmptyObj.EmptyType01 != Type.TypeEnum.Fire && EmptyObj.EmptyType02 != Type.TypeEnum.Fire && !isBurnDone)
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
                    EmptyObj.AtkChange(-2 , 0.0f);
                }
            }
        }
    }
    public void EmptyBurnRemove()
    {
        if (isBurnStart)
        {
            BurnPointFloat = 0;
            playerUIState.StateSlowUP(5, 0);
            isBurnStart = false;
            isBurnDone = false;
            playerUIState.StateDestory(5);
            MarterialChangeToNurmal();
            GetComponent<Empty>().AtkChange(2, 0.0f);
            BurnTimeFloat = 0;
        }
    }

    //===========================================================================敌人烧伤的函数=====================================================================================

    //===========================================================================敌人睡眠的函数=====================================================================================
    public float SleepTimeFloat
    {
        get { return sleepTimeFloat; }
        set { sleepTimeFloat = value; }
    }
    float sleepTimeFloat = 0.0f;
    public bool IsInUproarState
    {
        get { return isInUproarState; }
        set { isInUproarState = value; }
    }
    bool isInUproarState;
    //调用此函数时，如果还未开始睡眠，开始睡眠
    public void EmptySleepDone(float SleepPoint, float SleepTime, float SleepPer)
    {
        SleepTimeFloat = SleepTime;
        if (!isSleepDef && Random.Range(0.0f, 1.0f) <= SleepPer)
        {
            Empty EmptyObj = GetComponent<Empty>();
            if (!isInMistyTerrain && !isSleepDef && !isSleepDone && !isInElectricTerrain && !isInUproarState)
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
    }
    public void EmptySleepRemove()
    {

        if (isSleepStart)
        {

            SleepPointFloat = 0;
            playerUIState.StateSlowUP(6, 0);
            isSleepStart = false;
            isSleepDone = false;
            MarterialChangeToNurmal();
            playerUIState.StateDestory(6);
            SleepTimeFloat = 0;
        }
    }

    //===========================================================================敌人睡眠的函数=====================================================================================



    //===========================================================================敌人麻痹的函数=====================================================================================
    public float ParalysisTimeFloat
    {
        get { return paralysisTimeFloat; }
        set { paralysisTimeFloat = value; }
    }
    float paralysisTimeFloat = 0.0f;
    //调用此函数时，如果还未开始麻痹，开始麻痹
    public void EmptyParalysisDone(float ParalysisPoint, float ParalysisTime, float ParalysisPer)
    {
        ParalysisTimeFloat = ParalysisTime;
        if (!isParalysisDef && Random.Range(0.0f, 1.0f) <= ParalysisPer + ((isInSuperElectricTerrain) ? 0.2f : 0))
        {
            Empty EmptyObj = GetComponent<Empty>();
            if (!isInMistyTerrain && !isParalysisDef && EmptyObj.EmptyType01 != Type.TypeEnum.Electric && EmptyObj.EmptyType02 != Type.TypeEnum.Electric)
            {
                if (!isParalysisDone)
                {
                    ParalysisPointFloat += (ParalysisPoint + ((isInSuperElectricTerrain) ? 0.3f : 0)) * ParalysisResistance;
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
    }
    public void EmptyParalysisRemove()
    {
        paralysisTimeFloat = 0;

        if (isParalysisStart)
        {
            Debug.Log("xxx");
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
    public float EmptyConfusionPoint;
    public float ConfusionTimeFloat
    {
        get { return confusionTimeFloat; }
        set { confusionTimeFloat = value; }
    }
    float confusionTimeFloat = 0.0f;

    //调用此函数时，如果还未混乱，状态变为混论
    public void EmptyConfusion(float ConfusionTimer, float ConfusionPoint)
    {
        
        ConfusionTimeFloat = ConfusionTimer;
        if (!isInMistyTerrain && !isEmptyConfusionDone && !isConfusionDef)
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
        if (isEmptyConfusionStart)
        {
            EmptyConfusionPoint = 0;
            playerUIState.StateSlowUP(9, 0);
            isEmptyConfusionStart = false;
            isEmptyConfusionDone = false;
            playerUIState.StateDestory(9);
            ConfusionTimeFloat = 0;
        }
    }

    //===========================================================================敌人混乱的函数=====================================================================================















    //===========================================================================敌人着迷的函数=====================================================================================

    public bool isEmptyInfatuationDone = false;
    public bool isEmptyInfatuationStart = false;
    public bool EmptyInfatuationDef = false;
    public float EmptyInfatuationPoint;
    public float InfatuationTimeFloat
    {
        get { return infatuationTimeFloat; }
        set { infatuationTimeFloat = value; }
    }
    float infatuationTimeFloat = 0.0f;

    /// <summary>
    /// 调用此函数时，如果还未着迷，状态变为着迷
    /// </summary>
    /// <param name="InfatuationTimer">着迷的持续时间</param>
    /// <param name="InfatuationPoint">着迷的点数</param>
    public void EmptyInfatuation(float InfatuationTimer, float InfatuationPoint)
    {
        InfatuationTimeFloat = InfatuationTimer;
        if (!isEmptyInfatuationDone && !EmptyInfatuationDef)
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
                if (Boss != null && Boss.isBoos)
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
        if (isEmptyInfatuationStart)
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
            InfatuationTimeFloat = 0;
        }
    }

    //===========================================================================敌人着迷的函数=====================================================================================







    /// <summary>
    /// 能力变化的函数，level是能力变化等级，type是能力变化类型
    /// </summary>
    /// <param name="level"></param>
    /// <param name="type"></param>

    public void LevelChange(int level, string type)
    {
        Empty PokemonEmpty = GetComponent<Empty>();
        if (PokemonEmpty != null)
        {
            if (type == "Atk")
            {
                if (level > 0) { PokemonEmpty.AtkAbilityPoint = PokemonEmpty.AbilityForLevel(PokemonEmpty.Emptylevel , PokemonEmpty.AtkEmptyPoint) * Mathf.Pow(1.2f, level); }
                else { PokemonEmpty.AtkAbilityPoint = PokemonEmpty.AbilityForLevel(PokemonEmpty.Emptylevel, PokemonEmpty.AtkEmptyPoint) * Mathf.Pow(0.8f, -level); }
            }
            if (type == "Def")
            {
                if (level > 0) { PokemonEmpty.DefAbilityPoint = PokemonEmpty.AbilityForLevel(PokemonEmpty.Emptylevel, PokemonEmpty.DefEmptyPoint) * Mathf.Pow(1.2f, level); }
                else { PokemonEmpty.DefAbilityPoint = PokemonEmpty.AbilityForLevel(PokemonEmpty.Emptylevel, PokemonEmpty.DefEmptyPoint) * Mathf.Pow(0.8f, -level); }
            }
            if (type == "SpA")
            {
                if (level > 0) { PokemonEmpty.SpAAbilityPoint = PokemonEmpty.AbilityForLevel(PokemonEmpty.Emptylevel, PokemonEmpty.SpAEmptyPoint) * Mathf.Pow(1.2f, level); }
                else { PokemonEmpty.SpAAbilityPoint = PokemonEmpty.AbilityForLevel(PokemonEmpty.Emptylevel, PokemonEmpty.SpAEmptyPoint) * Mathf.Pow(0.8f, -level); }
            }
            if (type == "SpD")
            {
                if (level > 0) { PokemonEmpty.SpdAbilityPoint = PokemonEmpty.AbilityForLevel(PokemonEmpty.Emptylevel, PokemonEmpty.SpdEmptyPoint) * Mathf.Pow(1.2f, level); }
                else { PokemonEmpty.SpdAbilityPoint = PokemonEmpty.AbilityForLevel(PokemonEmpty.Emptylevel, PokemonEmpty.SpdEmptyPoint) * Mathf.Pow(0.8f, -level); }
            }
        }
    }


    //===========================================================================敌人攻击力改变的函数=====================================================================================
    /// <summary>
    /// 当前的攻击力等级
    /// </summary>
    public int AtkUpLevel
    {
        get { return atkChangeLevel; }
        set{ atkChangeLevel = value; }
    }
    int atkChangeLevel = 0;
    public bool isAtkChangeDef;
    List<Vector2> AtkTimeChange = new List<Vector2> { };

    /// <summary>
    /// 改变敌人攻击力的方法，ChangeLevel代表改变的等级，ChangeTime代表改变的时间，如果ChangeTime == 0，那么攻击力改变状态不会随着时间消失，需要手动改变
    /// </summary>
    /// <param name="ChangeLevel"></param>
    /// <param name="ChangeTime"></param>
    public virtual void AtkChange(int ChangeLevel ,  float ChangeTime)
    {
        if (null != this && GetComponent<Empty>() != null && !isAtkChangeDef)
        {
            AtkUpLevel += ChangeLevel;
            playerUIState.AbllityChange(0,AtkUpLevel);
            LevelChange(AtkUpLevel, "Atk");
            if (ChangeTime != 0) {
                float UpIndex = Time.time;
                AtkTimeChange.Add(new Vector2(UpIndex , ChangeLevel) );
                Timer.Start(this, ChangeTime, () =>
                {
                    while (AtkTimeChange.Count != 0 && UpIndex > AtkTimeChange[0].x)
                    {
                        AtkTimeChange.Remove(AtkTimeChange[0]);
                    }

                    if (AtkTimeChange.Count != 0 && new Vector2(UpIndex, ChangeLevel) == AtkTimeChange[0])
                    {
                        AtkTimeChange.Remove(AtkTimeChange[0]);
                        AtkUpLevel -= ChangeLevel;
                        playerUIState.AbllityChange(0, AtkUpLevel);
                        LevelChange(AtkUpLevel, "Atk");
                    }

                });
            }
        }
    }

    public virtual void ResetAtk()
    {
        AtkTimeChange.Clear();
    }

    //===========================================================================敌人攻击力改变的函数=====================================================================================





    //===========================================================================敌人防御力改变的函数=====================================================================================
    /// <summary>
    /// 当前的防御力等级
    /// </summary>
    public int DefUpLevel
    {
        get { return defChangeLevel; }
        set { defChangeLevel = value; }
    }
    int defChangeLevel = 0;
    public bool isDefChangeDef;
    List<Vector2> DefTimeChange = new List<Vector2> { };

    /// <summary>
    /// 改变敌人防御力的方法，ChangeLevel代表改变的等级，ChangeTime代表改变的时间，如果ChangeTime == 0，那么防御力改变状态不会随着时间消失，需要手动改变
    /// </summary>
    /// <param name="ChangeLevel"></param>
    /// <param name="ChangeTime"></param>
    public virtual void DefChange(int ChangeLevel, float ChangeTime)
    {
        if (null != this && GetComponent<Empty>() != null && !isDefChangeDef)
        {
            DefUpLevel += ChangeLevel;
            playerUIState.AbllityChange(1, DefUpLevel);
            LevelChange(DefUpLevel, "Def");
            if (ChangeTime != 0)
            {
                float UpIndex = Time.time;
                DefTimeChange.Add(new Vector2(UpIndex, ChangeLevel));
                Timer.Start(this, ChangeTime, () =>
                {

                    while (DefTimeChange.Count != 0 && UpIndex > DefTimeChange[0].x)
                    {
                        DefTimeChange.Remove(DefTimeChange[0]);
                    }

                    if (DefTimeChange.Count != 0 && new Vector2(UpIndex, ChangeLevel) == DefTimeChange[0])
                    {
                        DefTimeChange.Remove(DefTimeChange[0]);
                        DefUpLevel -= ChangeLevel;
                        playerUIState.AbllityChange(1, DefUpLevel);
                        LevelChange(DefUpLevel, "Def");
                    }
                });
            }
        }
    }

    public virtual void ResetDef()
    {
        DefTimeChange.Clear();
    }

    //===========================================================================敌人防御力改变的函数=====================================================================================






    //===========================================================================敌人特攻力改变的函数=====================================================================================
    /// <summary>
    /// 当前的特攻力等级
    /// </summary>
    public int SpAUpLevel
    {
        get { return spAChangeLevel; }
        set { spAChangeLevel = value; }
    }
    int spAChangeLevel = 0;
    public bool isSpAChangeDef;
    List<Vector2> SpATimeChange = new List<Vector2> { };

    /// <summary>
    /// 改变敌人特攻力的方法，ChangeLevel代表改变的等级，ChangeTime代表改变的时间，如果ChangeTime == 0，那么特攻力改变状态不会随着时间消失，需要手动改变
    /// </summary>
    /// <param name="ChangeLevel"></param>
    /// <param name="ChangeTime"></param>
    public virtual void SpAChange(int ChangeLevel, float ChangeTime)
    {
        if (null != this && GetComponent<Empty>() != null && !isSpAChangeDef)
        {
            SpAUpLevel += ChangeLevel;
            playerUIState.AbllityChange(2, SpAUpLevel);
            LevelChange(SpAUpLevel, "SpA");
            if (ChangeTime != 0)
            {
                float UpIndex = Time.time;
                SpATimeChange.Add(new Vector2(UpIndex, ChangeLevel));
                Timer.Start(this, ChangeTime, () =>
                {

                    while (SpATimeChange.Count != 0 && UpIndex > SpATimeChange[0].x)
                    {
                        SpATimeChange.Remove(SpATimeChange[0]);
                    }

                    if (SpATimeChange.Count != 0 && new Vector2(UpIndex, ChangeLevel) == SpATimeChange[0])
                    {
                        SpATimeChange.Remove(SpATimeChange[0]);
                        SpAUpLevel -= ChangeLevel;
                        playerUIState.AbllityChange(2, SpAUpLevel);
                        LevelChange(SpAUpLevel, "SpA");
                    }
                });
            }
        }
    }

    public virtual void ResetSpA()
    {
        SpATimeChange.Clear();
    }

    //===========================================================================敌人特攻力改变的函数=====================================================================================





    //===========================================================================敌人特防力改变的函数=====================================================================================
    /// <summary>
    /// 当前的特防力等级
    /// </summary>
    public int SpDUpLevel
    {
        get { return spDChangeLevel; }
        set { spDChangeLevel = value; }
    }
    int spDChangeLevel = 0;
    public bool isSpDChangeDef;
    List<Vector2> SpDTimeChange = new List<Vector2> { };

    /// <summary>
    /// 改变敌人特防力的方法，ChangeLevel代表改变的等级，ChangeTime代表改变的时间，如果ChangeTime == 0，那么特防力改变状态不会随着时间消失，需要手动改变
    /// </summary>
    /// <param name="ChangeLevel"></param>
    /// <param name="ChangeTime"></param>
    public virtual void SpDChange(int ChangeLevel, float ChangeTime)
    {
        if (null != this && GetComponent<Empty>() != null && !isSpDChangeDef)
        {
            SpDUpLevel += ChangeLevel;
            playerUIState.AbllityChange(3, SpDUpLevel);
            LevelChange(SpDUpLevel, "SpD");
            if (ChangeTime != 0)
            {
                float UpIndex = Time.time;
                SpDTimeChange.Add(new Vector2(UpIndex, ChangeLevel));
                Timer.Start(this, ChangeTime, () =>
                {
                    while (SpDTimeChange.Count != 0 && UpIndex > SpDTimeChange[0].x)
                    {
                        SpDTimeChange.Remove(SpDTimeChange[0]);
                    }

                    if (SpDTimeChange.Count != 0 && new Vector2(UpIndex, ChangeLevel) == SpDTimeChange[0])
                    {
                        SpDTimeChange.Remove(SpDTimeChange[0]);
                        SpDUpLevel -= ChangeLevel;
                        playerUIState.AbllityChange(3, SpDUpLevel);
                        LevelChange(SpDUpLevel, "SpD");
                    }
                });
            }
        }
    }

    public virtual void ResetSpD()
    {
        SpDTimeChange.Clear();
    }

    //===========================================================================敌人特防力改变的函数=====================================================================================




    //===========================================================================敌人寒冷（更容易被冰冻）的函数=====================================================================================

    /// <summary>
    /// 代表是否寒冷
    /// </summary>
    public int isColdDown;
    public bool isColdDef;

    /// <summary>
    /// 调用此函数时，如果未被寒冷，状态变为寒冷
    /// </summary>
    /// <param name="Time"> 下降的时间，如果为零时间为无限，需要手动Remove </param>
    public virtual void Cold(float Time)
    {
        if (GetComponent<Empty>() != null)
        {
            isColdDown += 1;
            playerUIState.StatePlus(11);
            if (Time != 0) { Invoke("ColdRemove", Time); }
        }
    }
    /// <summary>
    /// 如果Cold( float Time )的Time不等于0，不需要调用此函数，
    /// </summary>
    public void ColdRemove()
    {
        if (isColdDown != 0)
        {
            if (GetComponent<Empty>() != null)
            {
                isColdDown = 0;
                playerUIState.StateDestory(11);
            }
        }
    }

    //===========================================================================敌人寒冷（更容易被冰冻）的函数=====================================================================================






    //===========================================================================敌人被诅咒的函数=====================================================================================

    public bool isEmptyCurseDone = false;
    public bool isEmptyCurseStart = false;
    public bool EmptyCurseDef = false;
    public float EmptyCursePoint;
    public float CurseTimeFloat
    {
        get { return curseTimeFloat; }
        set { curseTimeFloat = value; }
    }
    float curseTimeFloat = 0.0f;

    /// <summary>
    /// 调用此函数时，如果还未诅咒，状态变为诅咒
    /// </summary>
    /// <param name="InfatuationTimer">诅咒的持续时间（通常为0）</param>
    /// <param name="InfatuationPoint">诅咒的点数</param>
    public void EmptyCurse(float CurseTimer, float CursePoint)
    {
        CurseTimeFloat = CurseTimer;
        if (!isEmptyCurseDone && !EmptyCurseDef)
        {
            EmptyCursePoint += CursePoint * OtherStateResistance;
            if (!isEmptyCurseStart && EmptyCursePoint < 1)
            {
                playerUIState.StatePlus(12);
                playerUIState.StateSlowUP(12, EmptyCursePoint);
                isEmptyCurseStart = true;
            }
            else if (isEmptyCurseStart && EmptyCursePoint < 1)
            {
                playerUIState.StateSlowUP(12, EmptyCursePoint);
            }
            else if (EmptyCursePoint >= 1 && !isEmptyCurseDone)
            {
                if (!isEmptyCurseStart)
                {
                    playerUIState.StatePlus(12);
                    playerUIState.StateSlowUP(12, EmptyCursePoint);
                    isEmptyCurseStart = true;
                }
                isEmptyCurseDone = true;
                playerUIState.StateSlowUP(12, EmptyCursePoint);
                if (CurseTimer != 0) { Invoke("EmptyCurseRemove", CurseTimer); }
            }
        }

    }
    //只可被上一个函数延迟调用，代表移除诅咒的函数
    public void EmptyCurseRemove()
    {
        if (isEmptyCurseStart)
        {
            EmptyCursePoint = 0;
            playerUIState.StateSlowUP(12, 0);
            isEmptyCurseStart = false;
            isEmptyCurseDone = false;
            playerUIState.StateDestory(12);
            CurseTimeFloat = 0;
        }
    }


    //===========================================================================敌人被诅咒的函数=====================================================================================









    //***************************************************************************对敌人的函数*********************************************************************************









    //***************************************************************************对自己的函数*********************************************************************************


    //===========================================================================改变速度的函数=====================================================================================

    //一个变量代表速度是否被改变，一个代表改变的倍率
    public bool isSpeedChange;
    public bool isSppedDownDef { get { return issppedDownDef; } set { issppedDownDef = value; } }
    bool issppedDownDef;

    //调用此函数时，如果速度还未被改变，改变速度并改变颜色，状态变为被改变
    public void SpeedChange()
    {
        if (!isSpeedChange && !isSppedDownDef &&!isSafeguard && !isObliviousTrigger && !isLeafGuardTrigger)
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


    //===========================================================================冰冻的函数=====================================================================================



    //一个变量代表是否冰冻，一个代表冰冻的程度
    public bool isPlayerFrozenDone;
    public bool isPlayerFrozenStart;
    public float PlayerFrozenPointFloat;


    //调用此函数时，如果还未开始冰冻，开始冰冻
    public void PlayerFrozenFloatPlus(float FrozenPoint , float FrozenTime)
    {
        PlayerControler playerchecktype = transform.GetComponent<PlayerControler>();
        if (!(playerchecktype.PlayerType01 == (int)Type.TypeEnum.Ice || playerchecktype.PlayerType02 == (int)Type.TypeEnum.Ice || playerchecktype.PlayerTeraType == (int)Type.TypeEnum.Ice || playerchecktype.PlayerTeraTypeJOR == (int)Type.TypeEnum.Ice))
        {
            if (!isInMistyTerrain && !isStateInvincible && !isFrozenDef && !isSafeguard && !isObliviousTrigger && !isLeafGuardTrigger)
            {
                PlayerFrozenPointFloat += FrozenPoint;
                PlayerFrozenPointFloat = (PlayerFrozenPointFloat > 1 ? 1 : PlayerFrozenPointFloat);
                if (!isPlayerFrozenStart && PlayerFrozenPointFloat < 1)
                {
                    playerUIState.StatePlus(2);
                    playerUIState.StateSlowUP(2, PlayerFrozenPointFloat);
                    isPlayerFrozenStart = true;
                }
                else if (isPlayerFrozenStart && PlayerFrozenPointFloat < 1)
                {
                    playerUIState.StateSlowUP(2, PlayerFrozenPointFloat);
                }
                else if (PlayerFrozenPointFloat >= 1 && !isPlayerFrozenDone)
                {
                    if (!isPlayerFrozenStart)
                    {
                        playerUIState.StatePlus(2);
                        playerUIState.StateSlowUP(2, PlayerFrozenPointFloat);
                        isPlayerFrozenStart = true;
                    }
                    isPlayerFrozenDone = true;
                    playerUIState.StateSlowUP(2, PlayerFrozenPointFloat);
                    if (transform.GetComponent<PlayerControler>() != null)
                    {
                        /*
                        
                        player.KnockOutPoint = 1;
                        player.playerData.SpABounsAlways--;
                        SpAHWBeforeChange = player.SpAAbilityPoint * 0.3f;
                        player.playerData.SpAHardWorkAlways -= SpAHWBeforeChange;
                        
                        */
                        //冰冻的效果
                        PlayerControler player = transform.GetComponent<PlayerControler>();
                        Timer.Start(this , FrozenTime , ()=> { PlayerFrozenRemove(); });
                        player.ReFreshAbllityPoint();
                    }
                    MarterialChangeToFrozen();
                }
                if (GetComponent<PlayerControler>() != null)
                {
                    isStateInvincible = true;
                    StateInvincileTimer = TimeStateInvincible;
                }
            }
        }
    }

    //只可被上一个函数延迟调用，代表解冻的函数
    public void PlayerFrozenRemove()
    {
        if (isPlayerFrozenStart)
        {
            //道具135 清净坠饰
            if (GetComponent<PlayerControler>().playerData.IsPassiveGetList[135])
            {
                GetComponent<PlayerControler>().ChangeHPW(new Vector2Int(Random.Range(1, 7), (int)(PlayerFrozenPointFloat * 4.0)));
            }
            PlayerFrozenPointFloat = 0;
            playerUIState.StateSlowUP(2, 0);
            playerUIState.StateDestory(2);
            if (transform.GetComponent<PlayerControler>() != null && isPlayerFrozenDone)
            {
                /*
                
                player.playerData.SpABounsAlways++;
                player.playerData.SpAHardWorkAlways += SpAHWBeforeChange;
                
                */
                //冰冻解除的效果
                PlayerControler player = transform.GetComponent<PlayerControler>();
                animator.speed = 1;
                player.ReFreshAbllityPoint();
            }
            isPlayerFrozenStart = false;
            isPlayerFrozenDone = false;
            MarterialChangeToNurmal();

        }
    }



    //===========================================================================中毒的函数=====================================================================================





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
        PlayerControler playerchecktype = transform.GetComponent<PlayerControler>();
        if (!(playerchecktype.PlayerType01 == (int)Type.TypeEnum.Poison || playerchecktype.PlayerType02 == (int)Type.TypeEnum.Poison || playerchecktype.PlayerTeraType == (int)Type.TypeEnum.Poison || playerchecktype.PlayerTeraTypeJOR == (int)Type.TypeEnum.Poison || playerchecktype.PlayerType01 == (int)Type.TypeEnum.Steel || playerchecktype.PlayerType02 == (int)Type.TypeEnum.Steel || playerchecktype.PlayerTeraType == (int)Type.TypeEnum.Steel || playerchecktype.PlayerTeraTypeJOR == (int)Type.TypeEnum.Steel))
        {
            if (!isInMistyTerrain && !isStateInvincible && !isToxicDef && !isSafeguard && !isObliviousTrigger && !isLeafGuardTrigger)
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
                if (GetComponent<PlayerControler>() != null)
                {
                    isStateInvincible = true;
                    StateInvincileTimer = TimeStateInvincible;
                }
            }
        }
    }

    //只可被上一个函数延迟调用，代表解冻的函数
    public void ToxicRemove()
    {
        if (isToxicStart)
        {
            //道具135 清净坠饰
            if (GetComponent<PlayerControler>().playerData.IsPassiveGetList[135])
            {
                GetComponent<PlayerControler>().ChangeHPW(new Vector2Int(Random.Range(1, 7), (int)(ToxicPointFloat * 4.0)));
            }
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
        PlayerControler playerchecktype = transform.GetComponent<PlayerControler>();
        if (!(playerchecktype.PlayerType01 == (int)Type.TypeEnum.Electric || playerchecktype.PlayerType02 == (int)Type.TypeEnum.Electric || playerchecktype.PlayerTeraType == (int)Type.TypeEnum.Electric || playerchecktype.PlayerTeraTypeJOR == (int)Type.TypeEnum.Electric))
        {
            if (!isInMistyTerrain && !isStateInvincible && !isParalysisDef && !isSafeguard && !isObliviousTrigger && !isLeafGuardTrigger)
            {
                ParalysisPointFloat += ParalysisPoint + ((isInSuperElectricTerrain) ? 0.3f : 0);
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
    }

    //只可被上一个函数延迟调用，代表解冻的函数
    public void ParalysisRemove()
    {
        if (isParalysisStart)
        {
            //道具135 清净坠饰
            if (GetComponent<PlayerControler>().playerData.IsPassiveGetList[135])
            {
                GetComponent<PlayerControler>().ChangeHPW(new Vector2Int(Random.Range(1, 7), (int)(ParalysisPointFloat * 4.0)));
            }
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
        PlayerControler playerchecktype = transform.GetComponent<PlayerControler>();
        if (!(playerchecktype.PlayerType01 == (int)Type.TypeEnum.Fire || playerchecktype.PlayerType02 == (int)Type.TypeEnum.Fire || playerchecktype.PlayerTeraType == (int)Type.TypeEnum.Fire || playerchecktype.PlayerTeraTypeJOR == (int)Type.TypeEnum.Fire))
        {
            if (!isInMistyTerrain && !isStateInvincible && !isBurnDef && !isSafeguard && !isObliviousTrigger && !isLeafGuardTrigger)
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
    }

    //只可被上一个函数延迟调用，代表解冻的函数
    public void BurnRemove()
    {
        if (isBurnStart)
        {
            //道具135 清净坠饰
            if (GetComponent<PlayerControler>().playerData.IsPassiveGetList[135])
            {
                GetComponent<PlayerControler>().ChangeHPW(new Vector2Int(Random.Range(1, 7), (int)(BurnPointFloat * 4.0)));
            }
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
        if (!isInMistyTerrain && !isStateInvincible && !isSleepDef && !isInElectricTerrain && !isSafeguard && !isObliviousTrigger && !isLeafGuardTrigger && !IsInUproarState)
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

            //道具135 清净坠饰
            if (GetComponent<PlayerControler>().playerData.IsPassiveGetList[135])
            {
                GetComponent<PlayerControler>().ChangeHPW(new Vector2Int(Random.Range(1, 7), (int)(SleepPointFloat * 4.0)));
            }
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

    //===========================================================================混乱的函数=====================================================================================


    //一个变量代表是否混乱，一个代表混乱的程度

    public bool isConfusionDef;
    public bool isConfusionDone;
    public bool isConfusionStart;
    public float ConfusionPointFloat;
    //调用此函数时，如果还未开始混乱，开始混乱
    public void ConfusionFloatPlus(float ConfusionPoint)
    {

        if (!isInMistyTerrain && !isStateInvincible && !isConfusionDef && !isConfusionDone && !isSafeguard && !isObliviousTrigger && !isLeafGuardTrigger)
        {
            ConfusionPointFloat += ConfusionPoint;
            ConfusionPointFloat = (ConfusionPointFloat > 1 ? 1 : ConfusionPointFloat);

            if (!isConfusionStart && ConfusionPointFloat < 1)
            {
                playerUIState.StatePlus(9);
                playerUIState.StateSlowUP(9, ConfusionPointFloat);
                isConfusionStart = true;
            }
            else if (isConfusionStart && ConfusionPointFloat < 1)
            {
                playerUIState.StateSlowUP(9, ConfusionPointFloat);
            }
            else if (ConfusionPointFloat >= 1 && !isConfusionDone)
            {
                if (!isConfusionStart)
                {
                    playerUIState.StatePlus(9);
                    playerUIState.StateSlowUP(9,ConfusionPointFloat);
                    isConfusionStart = true;
                }
                isConfusionDone = true;
                playerUIState.StateSlowUP(9, ConfusionPointFloat);
            }
            if (GetComponent<PlayerControler>() != null)
            {
                isStateInvincible = true;
                StateInvincileTimer = TimeStateInvincible;
            }
        }
    }


    //只可被上一个函数延迟调用，代表解除混乱的函数
    public void ConfusionRemove()
    {
        if (isConfusionStart)
        {
            ConfusionPointFloat = 0;
            playerUIState.StateSlowUP(9, 0);
            playerUIState.StateDestory(9);
            isConfusionStart = false;
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
        CopyTarget.isConfusionStart = isConfusionStart;
        CopyTarget.ConfusionPointFloat = ConfusionPointFloat;

        for(int i = 0; i< playerUIState.transform.childCount; i++)
        {
            Instantiate(playerUIState.transform.GetChild(i), Vector3.zero, Quaternion.identity, CopyTarget.playerUIState.transform);
        }
        CopyTarget.playerUIState.InstanceObjWhenEvlo(playerUIState);

    }

    //===========================================================================进化时复制目前状态的函数=====================================================================================





    //=================================特性=========================================
    public bool isSnowCloakTrigger { get { return issnowCloakTrigger; } set { issnowCloakTrigger = value; } }
    bool issnowCloakTrigger;
    public bool isObliviousTrigger { get { return isobliviousTrigger; } set { isobliviousTrigger = value; } }
    bool isobliviousTrigger;
    public bool isLeafGuardTrigger { get { return isleafGuardTrigger; } set { isleafGuardTrigger = value; } }
    bool isleafGuardTrigger;
    //=================================特性=========================================




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

    public static void PokemonHpChange(GameObject Attacker , GameObject Attacked , float AtkPower , float SpAPower ,int HpUpValue , Type.TypeEnum SkillType, bool Critial = false)
    {
        //决定攻击者
        int AttackerATK = 1;
        int AttackerSpA = 1;
        int AttackerLevel = 1;
        float EmptyTypeAlpha = 1;

        //和场地有关的伤害加成
        float TerrainAlpha = 1;
        if (Attacker != null && Attacker.GetComponent<Pokemon>() != null && Attacker.GetComponent<Pokemon>().isInGrassyTerrain && SkillType == Type.TypeEnum.Grass) { TerrainAlpha *= 1.3f; }
        if (Attacker != null && Attacker.GetComponent<Pokemon>() != null && Attacker.GetComponent<Pokemon>().isInElectricTerrain && SkillType == Type.TypeEnum.Electric) { TerrainAlpha *= 1.3f; }
        if (Attacker != null && Attacker.GetComponent<Pokemon>() != null && Attacker.GetComponent<Pokemon>().isInPsychicTerrain && SkillType == Type.TypeEnum.Psychic) { TerrainAlpha *= 1.3f; }
        if (Attacked != null && Attacked.GetComponent<Pokemon>() != null && Attacked.GetComponent<Pokemon>().isInMistyTerrain && SkillType == Type.TypeEnum.Dragon) { TerrainAlpha *= 0.5f; }
        if (Attacked != null && Attacked.GetComponent<Substitute>() != null && Attacked.GetComponent<Substitute>().isInMistyTerrain && SkillType == Type.TypeEnum.Dragon) { TerrainAlpha *= 0.5f; }


        if (Attacker != null)
        {
            
            Empty EmptyAttacker = Attacker.GetComponent<Empty>();
            PlayerControler PlayerAttacker = Attacker.GetComponent<PlayerControler>();
            FollowBaby FollowBabyAttacker = Attacker.GetComponent<FollowBaby>();
            if (EmptyAttacker != null)  { 
                AttackerATK = (int)EmptyAttacker.AtkAbilityPoint ;  AttackerSpA = (int)EmptyAttacker.SpAAbilityPoint ; AttackerLevel = EmptyAttacker.Emptylevel;
                EmptyTypeAlpha = ((SkillType == EmptyAttacker.EmptyType01) || (SkillType == EmptyAttacker.EmptyType02)) ? 1.5f : 1;
            }
            if (PlayerAttacker != null) {
                AttackerATK = PlayerAttacker.AtkAbilityPoint; AttackerSpA = PlayerAttacker.SpAAbilityPoint; AttackerLevel = PlayerAttacker.Level;
                EmptyTypeAlpha = ((int)SkillType == PlayerAttacker.PlayerType01 ? (PlayerAttacker.PlayerAbility == PlayerControler.PlayerAbilityList.适应力 ? 1.8f : 1.5f) : 1) * ((int)SkillType == PlayerAttacker.PlayerType02 ? (PlayerAttacker.PlayerAbility == PlayerControler.PlayerAbilityList.适应力 ? 1.8f : 1.5f) : 1) * (PlayerAttacker.PlayerTeraTypeJOR == 0 ? ((int)SkillType == PlayerAttacker.PlayerTeraType ? (PlayerAttacker.PlayerAbility == PlayerControler.PlayerAbilityList.适应力 ? 1.8f : 1.5f) : 1) : ((int)SkillType == PlayerAttacker.PlayerTeraTypeJOR ? (PlayerAttacker.PlayerAbility == PlayerControler.PlayerAbilityList.适应力 ? 1.8f : 1.5f) : 1));
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
                    ((AtkPower == 0) ? 0 : (Mathf.Clamp((AtkPower * (Attacker == null ? 1 : AttackerATK) * EmptyTypeAlpha * TerrainAlpha  * (Attacker == null ? 1 : (2 * AttackerLevel + 10))) / (250 * EmptyAttacked.DefAbilityPoint * WeatherDefAlpha + 2) , 1 , 10000))),
                    ((SpAPower == 0) ? 0 : (Mathf.Clamp((SpAPower * (Attacker == null ? 1 : AttackerSpA) * EmptyTypeAlpha * TerrainAlpha  * (Attacker == null ? 1 : (2 * AttackerLevel + 10))) / (250 * EmptyAttacked.SpdAbilityPoint * WeatherSpDAlpha + 2) , 1 , 10000))),
                    (int)SkillType, Critial);
                    
                    //if (SpAPower == 0){ Debug.Log((AtkPower * (Attacker == null ? 1 : AttackerATK) * EmptyTypeAlpha /* WeatherAlpha */ * (Attacker == null ? 1 : (2 * AttackerLevel + 10))) + " + " + (250 * EmptyAttacked.DefAbilityPoint * WeatherDefAlpha + 2)); }
                    //else if (AtkPower == 0) {  Debug.Log((SpAPower * (Attacker == null ? 1 : AttackerSpA) * EmptyTypeAlpha /* WeatherAlpha */ * (Attacker == null ? 1 : (2 * AttackerLevel + 10))) + " + " + (250 * EmptyAttacked.SpdAbilityPoint * WeatherDefAlpha + 2)); }

                }
                else
                {
                    EmptyAttacked.EmptyHpChange(AtkPower, SpAPower, 19, Critial);
                }

                //if（当前被攻击是梦妖
                //    && 攻击者是玩家
                //    && 检测玩家的四个技能 某个技能的（ 属性==SkillType ） && （（物攻威力 == AtkPower && 特攻威力 == SpAPower） || （技能的IsDamageChangable == true） ）
                //    && 梦妖的生命值《= 0 ）
                //    {
                //      则封印该技能
                //    }
                if(Attacker!= null && Attacker.GetComponent<PlayerControler>()!= null && Attacked != null &&  Attacked.GetComponent<Misdreavus>()!= null)
                {
                    if(Attacked.GetComponent<Misdreavus>().EmptyHp <= 0)
                    {
                        var player = Attacker.GetComponent<PlayerControler>();
                        var MisdreavusObj = Attacked.GetComponent<Misdreavus>();
                        MisdreavusObj.IsDeadrattle = (!MisdreavusObj.isEmptyFrozenDone) && (!MisdreavusObj.isFearDone) && (!MisdreavusObj.isBlindDone);
                        if (player.Skill01 != null)
                        {
                            var skill01 = player.Skill01.GetComponent<Skill>();
                            if (skill01 != null && (Type.TypeEnum)skill01.SkillType == SkillType && (skill01.Damage == AtkPower && skill01.SpDamage == SpAPower))
                            {
                                MisdreavusObj.ImpoisonSkillIndex = 1;
                                MisdreavusObj.IsKilledBySkill = true;
                                Debug.Log(MisdreavusObj.IsDeadrattle + "+" + MisdreavusObj.IsKilledBySkill);
                            }
                        }
                        if (player.Skill02 != null)
                        {
                            var skill02 = player.Skill02.GetComponent<Skill>();
                            if (skill02 != null && (Type.TypeEnum)skill02.SkillType == SkillType && (skill02.Damage == AtkPower && skill02.SpDamage == SpAPower))
                            {
                                MisdreavusObj.ImpoisonSkillIndex = 2;
                                MisdreavusObj.IsKilledBySkill = true;
                                Debug.Log(MisdreavusObj.IsDeadrattle + "+" + MisdreavusObj.IsKilledBySkill);
                            }
                        }
                        if(player.Skill03 != null)
                        {
                            var skill03 = player.Skill03.GetComponent<Skill>();
                            if (skill03 != null && (Type.TypeEnum)skill03.SkillType == SkillType && (skill03.Damage == AtkPower && skill03.SpDamage == SpAPower))
                            {
                                MisdreavusObj.ImpoisonSkillIndex = 3;
                                MisdreavusObj.IsKilledBySkill = true;
                                Debug.Log(MisdreavusObj.IsDeadrattle + "+" + MisdreavusObj.IsKilledBySkill);
                            }
                        }
                        if (player.Skill04 != null)
                        {
                            var skill04 = player.Skill04.GetComponent<Skill>();
                            if (skill04 != null && (Type.TypeEnum)skill04.SkillType == SkillType && (skill04.Damage == AtkPower && skill04.SpDamage == SpAPower))
                            {
                                MisdreavusObj.ImpoisonSkillIndex = 4;
                                MisdreavusObj.IsKilledBySkill = true;
                                Debug.Log(MisdreavusObj.IsDeadrattle + "+" + MisdreavusObj.IsKilledBySkill);
                            }
                        }
                    }
                }
            }
            else
            {
                EmptyAttacked.EmptyHpChange(-HpUpValue, 0, 19, Critial);
            }
        }
        if (Attacked.GetComponent<PlayerControler>() != null)
        {
            PlayerControler PlayerAttacked = Attacked.GetComponent<PlayerControler>();
            if (HpUpValue == 0)
            {
                Empty AttackerEmpty = null; SubEmptyBody AttackerSubEmpty = null;
                if (Attacker != null && Attacker.GetComponent<Empty>()) {  AttackerEmpty = Attacker.GetComponent<Empty>(); }
                if (Attacker != null && Attacker.GetComponent<SubEmptyBody>()) { AttackerSubEmpty = Attacker.GetComponent<SubEmptyBody>(); }

                //触发特性女王的威严
                if ((PlayerAttacked != null && PlayerAttacked.PlayerAbility == PlayerControler.PlayerAbilityList.女王的威严) && 
                    (Attacker != null && AttackerEmpty != null &&  ((AttackerSubEmpty != null && AttackerSubEmpty.ParentEmpty.EmptyHp == AttackerSubEmpty.ParentEmpty.maxHP ) || (AttackerSubEmpty == null && AttackerEmpty.EmptyHp == AttackerEmpty.maxHP))  )
                    )
                {

                }
                else
                {


                    PlayerAttacked.ChangeHp(
                             ((AtkPower == 0) ? 0 : (Mathf.Clamp((-AtkPower * TerrainAlpha * (Attacker == null ? 1 : AttackerATK) * (Attacker == null ? 1 : (2 * AttackerLevel + 10))) / ((int)SkillType != 19 ? 250 : 1), -10000, -1))),
                             ((SpAPower == 0) ? 0 : (Mathf.Clamp((-SpAPower * TerrainAlpha * (Attacker == null ? 1 : AttackerSpA) * (Attacker == null ? 1 : (2 * AttackerLevel + 10))) / ((int)SkillType != 19 ? 250 : 1), -10000, -1))),
                            (int)SkillType, Critial);

                    if (PlayerAttacked.playerData.IsPassiveGetList[120] && Attacker != null)
                    {
                        Empty EmptyAttacker = Attacker.GetComponent<Empty>();
                        if (EmptyAttacker != null)
                        {
                            EmptyAttacker.Blind(3, 5);
                        }
                    }
                }
            }
            else
            {
                PlayerAttacked.ChangeHp(HpUpValue, 0, 19, Critial);
            }
        }
        if(Attacked.GetComponent<Substitute>() != null)
        {
            Substitute SubstotuteAttacked = Attacked.GetComponent<Substitute>();
            if (HpUpValue == 0)
            {
                SubstotuteAttacked.SubStituteChangeHp(
                     ((AtkPower == 0) ? 0 : Mathf.Clamp( (-AtkPower * TerrainAlpha * (Attacker == null ? 1 : AttackerATK) * (Attacker == null ? 1 : (2 * AttackerLevel + 10))) / (250) , -10000 , -1 ) ),
                     ((SpAPower == 0) ? 0 : Mathf.Clamp( (-SpAPower * TerrainAlpha * (Attacker == null ? 1 : AttackerSpA) * (Attacker == null ? 1 : (2 * AttackerLevel + 10))) / (250) , -10000 , -1 ) ),
                    (int)SkillType);
            }
        }
    }


    /// <summary>
    /// 特殊攻击的种类:扑击
    /// </summary>
    public enum SpecialAttackTypes
    {
        None,            //普通的攻击
        BodyPress,       //扑击
        Psyshock,        //精神冲击
        FoulPlay,        //欺诈
    }


    /// <summary>
    /// 当造成某些特殊伤害时（如以防御力计算伤害的扑击等），调用此重载
    /// </summary>
    /// <param name="Attacker">攻击的制造者</param>
    /// <param name="Attacked">被攻击者</param>
    /// <param name="AtkPower">此次伤害的物理威力</param>
    /// <param name="SpAPower">此次伤害的特攻威力</param>
    /// <param name="HpUpValue">此次改变不是伤害</param>
    /// <param name="SkillType">此次伤害的属性</param>
    /// <param name="AttackTypes">特殊攻击的种类</param>
    public static void PokemonHpChange(GameObject Attacker, GameObject Attacked, float AtkPower, float SpAPower, int HpUpValue, Type.TypeEnum SkillType , Pokemon.SpecialAttackTypes AttackTypes,bool Critial = false)
    {

        Debug.Log("BodyPress");
        //决定攻击者
        int AttackerATK = 1;
        int AttackerSpA = 1;
        int AttackerLevel = 1;
        float EmptyTypeAlpha = 1;

        //和场地有关的伤害加成
        float TerrainAlpha = 1;
        if (Attacker != null && Attacker.GetComponent<Pokemon>() != null && Attacker.GetComponent<Pokemon>().isInGrassyTerrain && SkillType == Type.TypeEnum.Grass) { TerrainAlpha *= 1.3f; }
        if (Attacker != null && Attacker.GetComponent<Pokemon>() != null && Attacker.GetComponent<Pokemon>().isInElectricTerrain && SkillType == Type.TypeEnum.Electric) { TerrainAlpha *= 1.3f; }
        if (Attacker != null && Attacker.GetComponent<Pokemon>() != null && Attacker.GetComponent<Pokemon>().isInPsychicTerrain && SkillType == Type.TypeEnum.Psychic) { TerrainAlpha *= 1.3f; }
        if (Attacked != null && Attacked.GetComponent<Pokemon>() != null && Attacked.GetComponent<Pokemon>().isInMistyTerrain && SkillType == Type.TypeEnum.Dragon) { TerrainAlpha *= 0.5f; }
        if (Attacked != null && Attacked.GetComponent<Substitute>() != null && Attacked.GetComponent<Substitute>().isInMistyTerrain && SkillType == Type.TypeEnum.Dragon) { TerrainAlpha *= 0.5f; }


        if (Attacker.gameObject != null || Attacker != null)
        {

            Empty EmptyAttacker = Attacker.GetComponent<Empty>();
            PlayerControler PlayerAttacker = Attacker.GetComponent<PlayerControler>();
            FollowBaby FollowBabyAttacker = Attacker.GetComponent<FollowBaby>();
            if (EmptyAttacker != null)
            {
                switch (AttackTypes) {
                    case Pokemon.SpecialAttackTypes.BodyPress:
                        AttackerATK = (int)EmptyAttacker.DefAbilityPoint; AttackerSpA = (int)EmptyAttacker.DefAbilityPoint;
                        break;
                    case Pokemon.SpecialAttackTypes.FoulPlay:
                        AttackerATK = (int)Attacked.GetComponent<PlayerControler>().AtkAbilityPoint; AttackerSpA = (int)EmptyAttacker.DefAbilityPoint;
                        break;
                    default:
                        AttackerATK = (int)EmptyAttacker.AtkAbilityPoint; AttackerSpA = (int)EmptyAttacker.SpAAbilityPoint;
                        break;
                }
                AttackerLevel = EmptyAttacker.Emptylevel;
                if (AttackTypes == SpecialAttackTypes.FoulPlay)
                {
                    PlayerControler p = Attacked.GetComponent<PlayerControler>();
                    EmptyTypeAlpha = ((int)SkillType == p.PlayerType01 ? (p.PlayerAbility == PlayerControler.PlayerAbilityList.适应力 ? 1.8f : 1.5f) : 1) * ((int)SkillType == p.PlayerType02 ? (p.PlayerAbility == PlayerControler.PlayerAbilityList.适应力 ? 1.8f : 1.5f) : 1) * (p.PlayerTeraTypeJOR == 0 ? ((int)SkillType == p.PlayerTeraType ? (p.PlayerAbility == PlayerControler.PlayerAbilityList.适应力 ? 1.8f : 1.5f) : 1) : ((int)SkillType == p.PlayerTeraTypeJOR ? (p.PlayerAbility == PlayerControler.PlayerAbilityList.适应力 ? 1.8f : 1.5f) : 1));
                }
                else
                {
                    EmptyTypeAlpha = ((SkillType == EmptyAttacker.EmptyType01) || (SkillType == EmptyAttacker.EmptyType02)) ? 1.5f : 1;

                }
            }
            if (PlayerAttacker != null)
            {
                switch (AttackTypes)
                {
                    case Pokemon.SpecialAttackTypes.BodyPress:
                        AttackerATK = PlayerAttacker.DefAbilityPoint; AttackerSpA = PlayerAttacker.DefAbilityPoint;
                        break;
                    case SpecialAttackTypes.FoulPlay:
                        AttackerATK = (int)Attacked.GetComponent<Empty>().AtkAbilityPoint; AttackerSpA = PlayerAttacker.SpAAbilityPoint;
                        break;
                    default:
                        AttackerATK = PlayerAttacker.AtkAbilityPoint; AttackerSpA = PlayerAttacker.SpAAbilityPoint;
                        break;
                }
                AttackerLevel = PlayerAttacker.Level;
                if (AttackTypes == SpecialAttackTypes.FoulPlay)
                {
                    EmptyTypeAlpha = (SkillType == Attacked.GetComponent<Empty>().EmptyType01 ? 1.5f : 1) * (SkillType == Attacked.GetComponent<Empty>().EmptyType02 ? 1.5f : 1);
                }
                else
                {
                    EmptyTypeAlpha = ((int)SkillType == PlayerAttacker.PlayerType01 ? (PlayerAttacker.PlayerAbility == PlayerControler.PlayerAbilityList.适应力 ? 1.8f : 1.5f) : 1) * ((int)SkillType == PlayerAttacker.PlayerType02 ? (PlayerAttacker.PlayerAbility == PlayerControler.PlayerAbilityList.适应力 ? 1.8f : 1.5f) : 1) * (PlayerAttacker.PlayerTeraTypeJOR == 0 ? ((int)SkillType == PlayerAttacker.PlayerTeraType ? (PlayerAttacker.PlayerAbility == PlayerControler.PlayerAbilityList.适应力 ? 1.8f : 1.5f) : 1) : ((int)SkillType == PlayerAttacker.PlayerTeraTypeJOR ? (PlayerAttacker.PlayerAbility == PlayerControler.PlayerAbilityList.适应力 ? 1.8f : 1.5f) : 1));
                }
            }
            if (FollowBabyAttacker != null) {
                switch (AttackTypes)
                {
                    case Pokemon.SpecialAttackTypes.BodyPress:
                        AttackerATK = FollowBabyAttacker.BabyDef();
                        AttackerSpA = FollowBabyAttacker.BabyDef();
                        break;
                    case Pokemon.SpecialAttackTypes.FoulPlay:
                        AttackerATK = (int)Attacked.GetComponent<Empty>().AtkAbilityPoint;
                        AttackerSpA = FollowBabyAttacker.BabyDef();
                        break;
                    default:
                        AttackerATK = FollowBabyAttacker.BabyAtk(); AttackerSpA = FollowBabyAttacker.BabySpA();
                        break;
                }
                AttackerLevel = FollowBabyAttacker.BabyLevel(); }
        }


        //决定被攻击者
        if (Attacked.GetComponent<Empty>() != null)
        {
            Empty EmptyAttacked = Attacked.GetComponent<Empty>();
            if (HpUpValue == 0)
            {
                float AbillityAlpha = 1;
                if (Attacked.GetComponent<Empty>().Abillity == Empty.EmptyAbillity.Levitate && SkillType == Type.TypeEnum.Ground) { AbillityAlpha *= 0.64f; }


                float WeatherDefAlpha = ((Weather.GlobalWeather.isSandstorm ? ((EmptyAttacked.EmptyType01 == Type.TypeEnum.Rock || EmptyAttacked.EmptyType02 == Type.TypeEnum.Rock) ? 1.5f : 1) : 1));
                float WeatherSpDAlpha = ((Weather.GlobalWeather.isHail ? ((EmptyAttacked.EmptyType01 == Type.TypeEnum.Ice || EmptyAttacked.EmptyType02 == Type.TypeEnum.Ice) ? 1.5f : 1) : 1));

                float AttackedDEF = EmptyAttacked.DefAbilityPoint;
                float AttackedSPD = EmptyAttacked.SpdAbilityPoint;
                if (AttackTypes == SpecialAttackTypes.Psyshock) 
                {
                    AttackedDEF = EmptyAttacked.SpdAbilityPoint;
                    AttackedSPD = EmptyAttacked.DefAbilityPoint;
                }
                /*float WeatherAlpha = ((Weather.GlobalWeather.isRain && SkillType == Type.TypeEnum.Water) ? (Weather.GlobalWeather.isRainPlus ? 1.8f : 1.3f) : 1)
                    * ((Weather.GlobalWeather.isRain && SkillType == Type.TypeEnum.Fire) ? 0.5f : 1)
                    * ((Weather.GlobalWeather.isSunny && SkillType == Type.TypeEnum.Water) ? 0.5f : 1)
                    * ((Weather.GlobalWeather.isSunny && SkillType == Type.TypeEnum.Fire) ? (Weather.GlobalWeather.isSunnyPlus ? 1.8f : 1.3f) : 1);
                */

                if (SkillType != Type.TypeEnum.IgnoreType)
                {

                    Debug.Log("SpAPower = " + SpAPower + " AttackerSpA = " + AttackerATK + "EmptyTypeAlpha = " + EmptyTypeAlpha + "AttackerLevel = " + AttackerLevel + "AttackedSPD = " + AttackedSPD);

                    EmptyAttacked.EmptyHpChange(
                    ((AtkPower == 0) ? 0 : (Mathf.Clamp((AtkPower * (Attacker == null ? 1 : AttackerATK) * EmptyTypeAlpha * TerrainAlpha * AbillityAlpha * (Attacker == null ? 1 : (2 * AttackerLevel + 10))) / (250 * AttackedDEF * WeatherDefAlpha + 2), 1, 10000))),
                    ((SpAPower == 0) ? 0 : (Mathf.Clamp((SpAPower * (Attacker == null ? 1 : AttackerSpA) * EmptyTypeAlpha * TerrainAlpha * AbillityAlpha * (Attacker == null ? 1 : (2 * AttackerLevel + 10))) / (250 * AttackedSPD * WeatherSpDAlpha + 2), 1, 10000))),
                    (int)SkillType, Critial);

                    //if (SpAPower == 0){ Debug.Log((AtkPower * (Attacker == null ? 1 : AttackerATK) * EmptyTypeAlpha /* WeatherAlpha */ * (Attacker == null ? 1 : (2 * AttackerLevel + 10))) + " + " + (250 * EmptyAttacked.DefAbilityPoint * WeatherDefAlpha + 2)); }
                    //else if (AtkPower == 0) {  Debug.Log((SpAPower * (Attacker == null ? 1 : AttackerSpA) * EmptyTypeAlpha /* WeatherAlpha */ * (Attacker == null ? 1 : (2 * AttackerLevel + 10))) + " + " + (250 * EmptyAttacked.SpdAbilityPoint * WeatherDefAlpha + 2)); }

                }
                else
                {
                    EmptyAttacked.EmptyHpChange(AtkPower, SpAPower, 19, Critial);
                }
                //if（当前被攻击是梦妖
                //    && 攻击者是玩家
                //    && 检测玩家的四个技能 某个技能的（ 属性==SkillType ） && （（物攻威力 == AtkPower && 特攻威力 == SpAPower） || （技能的IsDamageChangable == true） ）
                //    && 梦妖的生命值《= 0 ）
                //    {
                //      则封印该技能
                //    }
                if (Attacker != null && Attacker.GetComponent<PlayerControler>() != null && Attacked != null && Attacked.GetComponent<Misdreavus>() != null)
                {
                    if (Attacked.GetComponent<Misdreavus>().EmptyHp <= 0)
                    {
                        var player = Attacker.GetComponent<PlayerControler>();
                        var MisdreavusObj = Attacked.GetComponent<Misdreavus>();
                        MisdreavusObj.IsDeadrattle = (!MisdreavusObj.isEmptyFrozenDone) && (!MisdreavusObj.isFearDone) && (!MisdreavusObj.isBlindDone);
                        if (player.Skill01 != null)
                        {
                            var skill01 = player.Skill01.GetComponent<Skill>();
                            if (skill01 != null && (Type.TypeEnum)skill01.SkillType == SkillType && (skill01.Damage == AtkPower && skill01.SpDamage == SpAPower))
                            {
                                MisdreavusObj.ImpoisonSkillIndex = 1;
                                MisdreavusObj.IsKilledBySkill = true;
                                Debug.Log(MisdreavusObj.IsDeadrattle + "+" + MisdreavusObj.IsKilledBySkill);
                            }
                        }
                        if (player.Skill02 != null)
                        {
                            var skill02 = player.Skill02.GetComponent<Skill>();
                            if (skill02 != null && (Type.TypeEnum)skill02.SkillType == SkillType && (skill02.Damage == AtkPower && skill02.SpDamage == SpAPower))
                            {
                                MisdreavusObj.ImpoisonSkillIndex = 2;
                                MisdreavusObj.IsKilledBySkill = true;
                                Debug.Log(MisdreavusObj.IsDeadrattle + "+" + MisdreavusObj.IsKilledBySkill);
                            }
                        }
                        if (player.Skill03 != null)
                        {
                            var skill03 = player.Skill03.GetComponent<Skill>();
                            if (skill03 != null && (Type.TypeEnum)skill03.SkillType == SkillType && (skill03.Damage == AtkPower && skill03.SpDamage == SpAPower))
                            {
                                MisdreavusObj.ImpoisonSkillIndex = 3;
                                MisdreavusObj.IsKilledBySkill = true;
                                Debug.Log(MisdreavusObj.IsDeadrattle + "+" + MisdreavusObj.IsKilledBySkill);
                            }
                        }
                        if (player.Skill04 != null)
                        {
                            var skill04 = player.Skill04.GetComponent<Skill>();
                            if (skill04 != null && (Type.TypeEnum)skill04.SkillType == SkillType && (skill04.Damage == AtkPower && skill04.SpDamage == SpAPower))
                            {
                                MisdreavusObj.ImpoisonSkillIndex = 4;
                                MisdreavusObj.IsKilledBySkill = true;
                                Debug.Log(MisdreavusObj.IsDeadrattle + "+" + MisdreavusObj.IsKilledBySkill);
                            }
                        }
                    }
                }

            }
            else
            {
                EmptyAttacked.EmptyHpChange(-HpUpValue, 0, 19, Critial);
            }
        }
        if (Attacked.GetComponent<PlayerControler>() != null)
        {
            PlayerControler PlayerAttacked = Attacked.GetComponent<PlayerControler>();
            if (HpUpValue == 0)
            {
                Empty AttackerEmpty = null; SubEmptyBody AttackerSubEmpty = null;
                if (Attacker != null && Attacker.GetComponent<Empty>()) { AttackerEmpty = Attacker.GetComponent<Empty>(); }
                if (Attacker != null && Attacker.GetComponent<SubEmptyBody>()) { AttackerSubEmpty = Attacker.GetComponent<SubEmptyBody>(); }

                //触发特性女王的威严
                if ((PlayerAttacked != null && PlayerAttacked.PlayerAbility == PlayerControler.PlayerAbilityList.女王的威严) &&
                    (Attacker != null && AttackerEmpty != null && ((AttackerSubEmpty != null && AttackerSubEmpty.ParentEmpty.EmptyHp == AttackerSubEmpty.ParentEmpty.maxHP) || (AttackerSubEmpty == null && AttackerEmpty.EmptyHp == AttackerEmpty.maxHP)))
                    )
                {

                }
                else
                {
                    PlayerAttacked.ChangeHp(
                             ((AtkPower == 0) ? 0 : (Mathf.Clamp((-AtkPower * TerrainAlpha * (Attacker == null ? 1 : AttackerATK) * (Attacker == null ? 1 : (2 * AttackerLevel + 10))) / ((int)SkillType != 19 ? 250 : 1), -10000, -1))),
                             ((SpAPower == 0) ? 0 : (Mathf.Clamp((-SpAPower * TerrainAlpha * (Attacker == null ? 1 : AttackerSpA) * (Attacker == null ? 1 : (2 * AttackerLevel + 10))) / ((int)SkillType != 19 ? 250 : 1), -10000, -1))),
                            (int)SkillType, Critial);
                    if (PlayerAttacked.playerData.IsPassiveGetList[120])
                    {
                        Empty EmptyAttacker = Attacker.GetComponent<Empty>();
                        if (EmptyAttacker != null)
                        {
                            EmptyAttacker.Blind(3, 5);
                        }
                    }
                }
            }
            else
            {
                PlayerAttacked.ChangeHp(HpUpValue, 0, 19, Critial);
            }
        }
        if (Attacked.GetComponent<Substitute>() != null)
        {
            Substitute SubstotuteAttacked = Attacked.GetComponent<Substitute>();
            if (HpUpValue == 0)
            {
                SubstotuteAttacked.SubStituteChangeHp(
                     ((AtkPower == 0) ? 0 : Mathf.Clamp((-AtkPower * TerrainAlpha * (Attacker == null ? 1 : AttackerATK) * (Attacker == null ? 1 : (2 * AttackerLevel + 10))) / (250), -10000, -1)),
                     ((SpAPower == 0) ? 0 : Mathf.Clamp((-SpAPower * TerrainAlpha * (Attacker == null ? 1 : AttackerSpA) * (Attacker == null ? 1 : (2 * AttackerLevel + 10))) / (250), -10000, -1)),
                    (int)SkillType);
            }
        }
    }

    //***************************************************************************对全体的函数*********************************************************************************





}