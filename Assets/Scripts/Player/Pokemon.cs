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


    //��ǰ�����δ�����ݳ�����
    public bool isInGrassyTerrain
    {
        get { return isinGrassyTerrain; }
        set { isinGrassyTerrain = value; }
    }
    bool isinGrassyTerrain = false;

    //��ǰ�����δ��ھ��񳡵���Psychic Terrain
    public bool isInPsychicTerrain
    {
        get { return isinPsychicTerrain; }
        set { isinPsychicTerrain = value; }
    }
    bool isinPsychicTerrain = false;

    //��ǰ�����δ��ڵ���������Electric Terrain
    public bool isInElectricTerrain
    {
        get { return isinElectricTerrain; }
        set { isinElectricTerrain = value; }
    }
    bool isinElectricTerrain = false;

    //��ǰ�����δ��ڱ�������Misty Terrain
    public bool isInMistyTerrain
    {
        get { return isinMistyTerrain; }
        set { isinMistyTerrain = value; }
    }
    bool isinMistyTerrain = false;



    //��ǰ�����δ���������ݳ�����
    public bool isInSuperGrassyTerrain
    {
        get { return isinSuperGrassyTerrain; }
        set { isinSuperGrassyTerrain = value; }
    }
    bool isinSuperGrassyTerrain = false;

    //��ǰ�����δ��ڳ������񳡵���Psychic Terrain
    public bool isInSuperPsychicTerrain
    {
        get { return isinSuperPsychicTerrain; }
        set { isinSuperPsychicTerrain = value; }
    }
    bool isinSuperPsychicTerrain = false;

    //��ǰ�����δ��ڳ�������������Electric Terrain
    public bool isInSuperElectricTerrain
    {
        get { return isinSuperElectricTerrain; }
        set { isinSuperElectricTerrain = value; }
    }
    bool isinSuperElectricTerrain = false;

    //��ǰ�����δ��ڳ�����������Misty Terrain
    public bool isInSuperMistyTerrain
    {
        get { return isinSuperMistyTerrain; }
        set { isinSuperMistyTerrain = value; }
    }
    bool isinSuperMistyTerrain = false;

    //���������ػ�״̬
    public bool isSafeguard;



    [Tooltip("pokemon�������Ⱦ�����")]
    public List<SpriteRenderer> skinRenderers;

    //����һ�����������߱�������ȡ����������
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
            SetSkinRenderersMaterial(NormalMaterial);
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

    //��ȡ pokemon �������Ⱦ���
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
    /// ���µ��˵Ĳ���
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
    /// �޸ĵ��˵Ĳ���
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










    //�����ҵ�UI״̬��
    public PlayerUIState playerUIState;

    //���������ٶ�
    public float speed;






    //***************************************************************************�Ե��˵ĺ���*********************************************************************************

    //===========================================================================�䶳�ĺ���=====================================================================================


    //һ�����������ٶ��Ƿ񱻸ı䣬һ������ı�ı���
    public bool isEmptyFrozenDone;
    public bool isFrozenDef;
    bool isFrozenDone = false;
    bool isFrozenStart = false;
    float EmptyFrozenPointFloat;
    float SpeedBefoerChange;

    //���ô˺���ʱ�������δ��������������״̬��Ϊ������
    public void Frozen(float FrozenTime, float FrozenPoint, float FrozenPer)
    {

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

    //ֻ�ɱ���һ�������ӳٵ��ã�����ⶳ�ĺ���
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
        }
    }

    //===========================================================================�䶳�ĺ���=====================================================================================

    //===========================================================================���µĺ���=====================================================================================

    public bool isFearDone = false;
    public bool isFearStart = false;
    float EmptyFearPointFloat;

    //���ô˺���ʱ�������δ���£�״̬��Ϊ����
    public void Fear(float FearTime, float FearPoint)
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
    //ֻ�ɱ���һ�������ӳٵ��ã������ٶȺ��µĺ���
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
        }
    }

    //===========================================================================���µĺ���=====================================================================================


    //===========================================================================������ä�ĺ���=====================================================================================

    public bool isBlindDone = false;
    public bool isBlindStart = false;
    float EmptyBlindPoint;

    //���ô˺���ʱ�������δ���£�״̬��Ϊ����
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
    //ֻ�ɱ���һ�������ӳٵ��ã������ٶȺ��µĺ���
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

    //===========================================================================������ä�ĺ���=====================================================================================


    //===========================================================================�����ж��ĺ���=====================================================================================
    //���ô˺���ʱ�������δ��ʼ�ж�����ʼ�ж�
    public void EmptyToxicDone(float ToxicPoint, float ToxicTime, float ToxicPer)
    {
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
            GetComponent<Empty>().SpAChange(2, 0.0f);
        }
    }

    //===========================================================================�����ж��ĺ���=====================================================================================




    //===========================================================================�������˵ĺ���=====================================================================================
    //���ô˺���ʱ�������δ��ʼ���ˣ���ʼ����
    public void EmptyBurnDone(float BurnPoint, float BurnTime, float BurnPer)
    {
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
        if (isBurnDone)
        {
            BurnPointFloat = 0;
            playerUIState.StateSlowUP(5, 0);
            isBurnStart = false;
            isBurnDone = false;
            playerUIState.StateDestory(5);
            MarterialChangeToNurmal();
            GetComponent<Empty>().AtkChange(2, 0.0f);
        }
    }

    //===========================================================================�������˵ĺ���=====================================================================================

    //===========================================================================����˯�ߵĺ���=====================================================================================
    //���ô˺���ʱ�������δ��ʼ˯�ߣ���ʼ˯��
    public void EmptySleepDone(float SleepPoint, float SleepTime, float SleepPer)
    {
        if (!isSleepDef && Random.Range(0.0f, 1.0f) <= SleepPer)
        {
            Empty EmptyObj = GetComponent<Empty>();
            if (!isInMistyTerrain && !isSleepDef && !isSleepDone && !isInElectricTerrain)
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

    //===========================================================================����˯�ߵĺ���=====================================================================================



    //===========================================================================������Եĺ���=====================================================================================
    //���ô˺���ʱ�������δ��ʼ��ԣ���ʼ���
    public void EmptyParalysisDone(float ParalysisPoint, float ParalysisTime, float ParalysisPer)
    {
        if (!isParalysisDef && Random.Range(0.0f, 1.0f) <= ParalysisPer + ((isInSuperElectricTerrain) ? 0.2f : 0))
        {
            Empty EmptyObj = GetComponent<Empty>();
            if (!isInMistyTerrain && !isParalysisDef && EmptyObj.EmptyType01 != Type.TypeEnum.Electric && EmptyObj.EmptyType02 != Type.TypeEnum.Electric)
            {
                if (!isParalysisDone)
                {
                    ParalysisPointFloat += (ParalysisPoint + ((isInSuperElectricTerrain) ? 0.3f : 1)) * ParalysisResistance;
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

    //===========================================================================������Եĺ���=====================================================================================






























    //===========================================================================���˻��ҵĺ���=====================================================================================

    public bool isEmptyConfusionDone = false;
    public bool isEmptyConfusionStart = false;
    float EmptyConfusionPoint;

    //���ô˺���ʱ�������δ���ң�״̬��Ϊ����
    public void EmptyConfusion(float ConfusionTimer, float ConfusionPoint)
    {
        if (!isInMistyTerrain && !isEmptyConfusionDone)
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
    //ֻ�ɱ���һ�������ӳٵ��ã������Ƴ����۵ĺ���
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

    //===========================================================================���˻��ҵĺ���=====================================================================================















    //===========================================================================�������Եĺ���=====================================================================================

    public bool isEmptyInfatuationDone = false;
    public bool isEmptyInfatuationStart = false;
    float EmptyInfatuationPoint;

    /// <summary>
    /// ���ô˺���ʱ�������δ���ԣ�״̬��Ϊ����
    /// </summary>
    /// <param name="InfatuationTimer">���Եĳ���ʱ��</param>
    /// <param name="InfatuationPoint">���Եĵ���</param>
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
                if (Boss != null && Boss.isBoos)
                {
                    Boss.speed *= 0.5f;
                }
                if (InfatuationTimer != 0) { Invoke("EmptyInfatuationRemove", InfatuationTimer); }
            }
        }

    }
    //ֻ�ɱ���һ�������ӳٵ��ã������Ƴ����Եĺ���
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

    //===========================================================================�������Եĺ���=====================================================================================







    /// <summary>
    /// �����仯�ĺ�����level�������仯�ȼ���type�������仯����
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


    //===========================================================================���˹������ı�ĺ���=====================================================================================
    /// <summary>
    /// ��ǰ�Ĺ������ȼ�
    /// </summary>
    public int AtkUpLevel
    {
        get { return atkChangeLevel; }
        set{ atkChangeLevel = value; }
    }
    int atkChangeLevel = 0;

    /// <summary>
    /// �ı���˹������ķ�����ChangeLevel����ı�ĵȼ���ChangeTime����ı��ʱ�䣬���ChangeTime == 0����ô�������ı�״̬��������ʱ����ʧ����Ҫ�ֶ��ı�
    /// </summary>
    /// <param name="ChangeLevel"></param>
    /// <param name="ChangeTime"></param>
    public void AtkChange(int ChangeLevel ,  float ChangeTime)
    {
        if (GetComponent<Empty>() != null)
        {
            AtkUpLevel += ChangeLevel;
            playerUIState.AbllityChange(0,AtkUpLevel);
            LevelChange(AtkUpLevel, "Atk");
            if (ChangeTime != 0) {
                Timer.Start(this, ChangeTime, () =>
                {
                    AtkUpLevel -= ChangeLevel;
                    playerUIState.AbllityChange(0, AtkUpLevel);
                    LevelChange(AtkUpLevel, "Atk");
                });
            }
        }
    }

    //===========================================================================���˹������ı�ĺ���=====================================================================================





    //===========================================================================���˷������ı�ĺ���=====================================================================================
    /// <summary>
    /// ��ǰ�ķ������ȼ�
    /// </summary>
    public int DefUpLevel
    {
        get { return defChangeLevel; }
        set { defChangeLevel = value; }
    }
    int defChangeLevel = 0;
    /// <summary>
    /// �ı���˷������ķ�����ChangeLevel����ı�ĵȼ���ChangeTime����ı��ʱ�䣬���ChangeTime == 0����ô�������ı�״̬��������ʱ����ʧ����Ҫ�ֶ��ı�
    /// </summary>
    /// <param name="ChangeLevel"></param>
    /// <param name="ChangeTime"></param>
    public void DefChange(int ChangeLevel, float ChangeTime)
    {
        if (GetComponent<Empty>() != null)
        {
            DefUpLevel += ChangeLevel;
            playerUIState.AbllityChange(1, DefUpLevel);
            LevelChange(DefUpLevel, "Def");
            if (ChangeTime != 0)
            {
                Timer.Start(this, ChangeTime, () =>
                {
                    DefUpLevel -= ChangeLevel;
                    playerUIState.AbllityChange(1, DefUpLevel);
                    LevelChange(DefUpLevel, "Def");
                });
            }
        }
    }

    //===========================================================================���˷������ı�ĺ���=====================================================================================






    //===========================================================================�����ع����ı�ĺ���=====================================================================================
    /// <summary>
    /// ��ǰ���ع����ȼ�
    /// </summary>
    public int SpAUpLevel
    {
        get { return spAChangeLevel; }
        set { spAChangeLevel = value; }
    }
    int spAChangeLevel = 0;
    /// <summary>
    /// �ı�����ع����ķ�����ChangeLevel����ı�ĵȼ���ChangeTime����ı��ʱ�䣬���ChangeTime == 0����ô�ع����ı�״̬��������ʱ����ʧ����Ҫ�ֶ��ı�
    /// </summary>
    /// <param name="ChangeLevel"></param>
    /// <param name="ChangeTime"></param>
    public void SpAChange(int ChangeLevel, float ChangeTime)
    {
        if (GetComponent<Empty>() != null)
        {
            SpAUpLevel += ChangeLevel;
            playerUIState.AbllityChange(2, SpAUpLevel);
            LevelChange(SpAUpLevel, "SpA");
            if (ChangeTime != 0)
            {
                Timer.Start(this, ChangeTime, () =>
                {
                    SpAUpLevel -= ChangeLevel;
                    playerUIState.AbllityChange(2, SpAUpLevel);
                    LevelChange(SpAUpLevel, "SpA");
                });
            }
        }
    }

    //===========================================================================�����ع����ı�ĺ���=====================================================================================





    //===========================================================================�����ط����ı�ĺ���=====================================================================================
    /// <summary>
    /// ��ǰ���ط����ȼ�
    /// </summary>
    public int SpDUpLevel
    {
        get { return spDChangeLevel; }
        set { spDChangeLevel = value; }
    }
    int spDChangeLevel = 0;
    /// <summary>
    /// �ı�����ط����ķ�����ChangeLevel����ı�ĵȼ���ChangeTime����ı��ʱ�䣬���ChangeTime == 0����ô�ط����ı�״̬��������ʱ����ʧ����Ҫ�ֶ��ı�
    /// </summary>
    /// <param name="ChangeLevel"></param>
    /// <param name="ChangeTime"></param>
    public void SpDChange(int ChangeLevel, float ChangeTime)
    {
        if (GetComponent<Empty>() != null)
        {
            SpDUpLevel += ChangeLevel;
            playerUIState.AbllityChange(3, SpDUpLevel);
            LevelChange(SpDUpLevel, "SpD");
            if (ChangeTime != 0)
            {
                Timer.Start(this, ChangeTime, () =>
                {
                    SpDUpLevel -= ChangeLevel;
                    playerUIState.AbllityChange(3, SpDUpLevel);
                    LevelChange(SpDUpLevel, "SpD");
                });
            }
        }
    }

    //===========================================================================�����ط����ı�ĺ���=====================================================================================




    //===========================================================================���˺��䣨�����ױ��������ĺ���=====================================================================================

    /// <summary>
    /// �����Ƿ���
    /// </summary>
    public int isColdDown;

    /// <summary>
    /// ���ô˺���ʱ�����δ�����䣬״̬��Ϊ����
    /// </summary>
    /// <param name="Time"> �½���ʱ�䣬���Ϊ��ʱ��Ϊ���ޣ���Ҫ�ֶ�Remove </param>
    public void Cold(float Time)
    {
        if (GetComponent<Empty>() != null)
        {
            isColdDown += 1;
            playerUIState.StatePlus(11);
            if (Time != 0) { Invoke("ColdRemove", Time); }
        }
    }
    /// <summary>
    /// ���Cold( float Time )��Time������0������Ҫ���ô˺�����
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

    //===========================================================================���˺��䣨�����ױ��������ĺ���=====================================================================================






    //===========================================================================���˱�����ĺ���=====================================================================================

    public bool isEmptyCurseDone = false;
    public bool isEmptyCurseStart = false;
    float EmptyCursePoint;

    /// <summary>
    /// ���ô˺���ʱ�������δ���䣬״̬��Ϊ����
    /// </summary>
    /// <param name="InfatuationTimer">����ĳ���ʱ�䣨ͨ��Ϊ0��</param>
    /// <param name="InfatuationPoint">����ĵ���</param>
    public void EmptyCurse(float CurseTimer, float CursePoint)
    {
        if (!isEmptyCurseDone)
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
    //ֻ�ɱ���һ�������ӳٵ��ã������Ƴ�����ĺ���
    public void EmptyCurseRemove()
    {
        if (isEmptyCurseDone)
        {
            EmptyCursePoint = 0;
            playerUIState.StateSlowUP(12, 0);
            isEmptyCurseStart = false;
            isEmptyCurseDone = false;
            playerUIState.StateDestory(12);
        }
    }


    //===========================================================================���˱�����ĺ���=====================================================================================









    //***************************************************************************�Ե��˵ĺ���*********************************************************************************









    //***************************************************************************���Լ��ĺ���*********************************************************************************


    //===========================================================================�ı��ٶȵĺ���=====================================================================================

    //һ�����������ٶ��Ƿ񱻸ı䣬һ������ı�ı���
    public bool isSpeedChange;

    //���ô˺���ʱ������ٶȻ�δ���ı䣬�ı��ٶȲ��ı���ɫ��״̬��Ϊ���ı�
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
    //���ô˺���ʱ���ӳ�ָ��ʱ�������һ������
    public void SpeedRemove01(float SpeedChangeTime)
    {
        Invoke("SpeedRemove02", SpeedChangeTime);
    }
    //ֻ�ɱ���һ�������ӳٵ��ã������ٶȻָ��ĺ���
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

    //===========================================================================�ı��ٶȵĺ���=====================================================================================


    //===========================================================================�����ĺ���=====================================================================================


    //һ�����������Ƿ������һ����������ĳ̶�
    public bool isPlayerFrozenDone;
    public bool isPlayerFrozenStart;
    public float PlayerFrozenPointFloat;

    //���ô˺���ʱ�������δ��ʼ��������ʼ����
    public void PlayerFrozenFloatPlus(float FrozenPoint)
    {
        if (!isInMistyTerrain && !isStateInvincible && !isFrozenDef && !isSafeguard)
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
                playerUIState.StateSlowUP(3, PlayerFrozenPointFloat);
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
                MarterialChangeToFrozen();
            }
            if (GetComponent<PlayerControler>() != null)
            {
                isStateInvincible = true;
                StateInvincileTimer = TimeStateInvincible;
            }
        }
    }

    //ֻ�ɱ���һ�������ӳٵ��ã�����ⶳ�ĺ���
    public void PlayerFrozenRemove()
    {
        if (isPlayerFrozenStart)
        {
            PlayerFrozenPointFloat = 0;
            playerUIState.StateSlowUP(2, 0);
            playerUIState.StateDestory(2);
            isPlayerFrozenStart = false;
            isPlayerFrozenDone = false;
            MarterialChangeToNurmal();

        }
    }

    //===========================================================================�ж��ĺ���=====================================================================================





    //===========================================================================�ж��ĺ���=====================================================================================


    //һ�����������Ƿ��ж���һ�������ж��ĳ̶�
    public bool isToxicDef;
    public bool isToxicDone;
    public bool isToxicStart;
    public float ToxicPointFloat;
    float SpAHWBeforeChange;

    //���ô˺���ʱ�������δ��ʼ�ж�����ʼ�ж�
    public void ToxicFloatPlus(float ToxicPoint)
    {
            if (!isInMistyTerrain && !isStateInvincible && !isToxicDef && !isSafeguard)
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

    //ֻ�ɱ���һ�������ӳٵ��ã�����ⶳ�ĺ���
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

    //===========================================================================�ж��ĺ���=====================================================================================


    //===========================================================================��Եĺ���=====================================================================================


    public bool isParalysisDef;
    public delegate void ParalysisDoneHappend(PlayerControler player);
    public ParalysisDoneHappend ParalysisDoneHappendEvent;
    public ParalysisDoneHappend ParalysisRemoveHappendEvent;

    //һ�����������Ƿ���ԣ�һ��������Եĳ̶�

    public bool isParalysisDone;
    public bool isParalysisStart ;
    public float ParalysisPointFloat;
    float MoveSpeHWBeforeChange;

    //���ô˺���ʱ�������δ��ʼ�ж�����ʼ�ж�
    public void ParalysisFloatPlus(float ParalysisPoint)
    {
        if (!isInMistyTerrain && !isStateInvincible && !isParalysisDef && !isSafeguard)
        {
            ParalysisPointFloat += ParalysisPoint + ((isInSuperElectricTerrain) ? 0.3f : 1);
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

    //ֻ�ɱ���һ�������ӳٵ��ã�����ⶳ�ĺ���
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

    //===========================================================================��Եĺ���=====================================================================================


    //===========================================================================���˵ĺ���=====================================================================================

    //һ�����������Ƿ��ж���һ�������ж��ĳ̶�


    public bool isBurnDef;
    public bool isBurnDone;
    public bool isBurnStart;
    public float BurnPointFloat;
    float AtkHWBeforeChange;

    //���ô˺���ʱ�������δ��ʼ�ж�����ʼ�ж�
    public void BurnFloatPlus(float BurnPoint)
    {

        if (!isInMistyTerrain && !isStateInvincible && !isBurnDef && !isSafeguard)
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

    //ֻ�ɱ���һ�������ӳٵ��ã�����ⶳ�ĺ���
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

    //===========================================================================���˵ĺ���=====================================================================================

    //===========================================================================˯�ߵĺ���=====================================================================================

    //һ�����������Ƿ��ж���һ�������ж��ĳ̶�

    public bool isSleepDef;
    public bool isSleepDone;
    public bool isSleepStart;
    public float SleepPointFloat;

    //���ô˺���ʱ�������δ��ʼ�ж�����ʼ�ж�
    public void SleepFloatPlus(float SleepPoint)
    {
        if (!isInMistyTerrain && !isStateInvincible && !isSleepDef && !isInElectricTerrain && !isSafeguard)
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

    //ֻ�ɱ���һ�������ӳٵ��ã�����ⶳ�ĺ���
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

    //===========================================================================˯�ߵĺ���=====================================================================================


    //һ�����������Ƿ���ң�һ��������ҵĳ̶�

    public bool isConfusionDef;
    public bool isConfusionDone;
    public bool isConfusionStart;
    public float ConfusionPointFloat;
    //���ô˺���ʱ�������δ��ʼ���ң���ʼ����
    public void ConfusionFloatPlus(float ConfusionPoint)
    {

        if (!isInMistyTerrain && !isStateInvincible && !isConfusionDef && !isConfusionDone && !isSafeguard)
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


    //ֻ�ɱ���һ�������ӳٵ��ã����������ҵĺ���
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



    //===========================================================================���ҵĺ���=====================================================================================






    //===========================================================================����ʱ����Ŀǰ״̬�ĺ���=====================================================================================

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
        CopyTarget.playerUIState.InstanceObjWhenEvlo(playerUIState);

    }

    //===========================================================================����ʱ����Ŀǰ״̬�ĺ���=====================================================================================


    //***************************************************************************���Լ��ĺ���*********************************************************************************




    //***************************************************************************��ȫ��ĺ���*********************************************************************************

    /// <summary>
    /// ���б����ε�����ֵ�ı�ʱ��ʹ�ô˷�������������ʱѡ����������������ظ���Ϊ0��
    /// �˷������Զ�������ת��Ϊʵ�ʵ��˺�ֵ��������ñ����ε�����ֵ����ָ���ġ�ֵ����ѡ������ΪType.TypeEnum.IgnoreType
    /// �����統AtkPower=10�����Բ�����IgnoreTypeʱ���Ա�������ɡ���������10�ġ�һ���˺������Ե���IgnoreTypeʱ���Ա�������ɡ�10�㡱�˺���
    /// </summary>
    /// <param name="Attacker">������������</param>
    /// <param name="Attacked">��������</param>
    /// <param name="AtkPower">�˴��˺�����������</param>
    /// <param name="SpAPower">�˴��˺����ع�����</param>
    /// <param name="HpUpValue">�˴θı䲻���˺������ǻظ�</param>
    /// <param name="SkillType">�˴��˺�������</param>

    public static void PokemonHpChange(GameObject Attacker , GameObject Attacked , float AtkPower , float SpAPower ,int HpUpValue , Type.TypeEnum SkillType)
    {
        //����������
        int AttackerATK = 1;
        int AttackerSpA = 1;
        int AttackerLevel = 1;
        float EmptyTypeAlpha = 1;

        //�ͳ����йص��˺��ӳ�
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
                EmptyTypeAlpha = ((int)SkillType == PlayerAttacker.PlayerType01 ? 1.5f : 1) * ((int)SkillType == PlayerAttacker.PlayerType02 ? 1.5f : 1) * (PlayerAttacker.PlayerTeraTypeJOR == 0 ? ((int)SkillType == PlayerAttacker.PlayerTeraType ? 1.5f : 1) : ((int)SkillType == PlayerAttacker.PlayerTeraTypeJOR ? 1.5f : 1));
            }
            if (FollowBabyAttacker != null) { AttackerATK = FollowBabyAttacker.BabyAtk(); AttackerSpA = FollowBabyAttacker.BabySpA(); AttackerLevel = FollowBabyAttacker.BabyLevel(); }
        }


        //������������
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
                    (int)SkillType);
                    
                    //if (SpAPower == 0){ Debug.Log((AtkPower * (Attacker == null ? 1 : AttackerATK) * EmptyTypeAlpha /* WeatherAlpha */ * (Attacker == null ? 1 : (2 * AttackerLevel + 10))) + " + " + (250 * EmptyAttacked.DefAbilityPoint * WeatherDefAlpha + 2)); }
                    //else if (AtkPower == 0) {  Debug.Log((SpAPower * (Attacker == null ? 1 : AttackerSpA) * EmptyTypeAlpha /* WeatherAlpha */ * (Attacker == null ? 1 : (2 * AttackerLevel + 10))) + " + " + (250 * EmptyAttacked.SpdAbilityPoint * WeatherDefAlpha + 2)); }

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
                         ((AtkPower == 0) ? 0 : ( Mathf.Clamp((-AtkPower * TerrainAlpha * (Attacker == null ? 1 : AttackerATK) * (Attacker == null ? 1 : (2 * AttackerLevel + 10))) / ((int)SkillType != 19 ? 250 : 1), -10000, -1) )),
                         ((SpAPower == 0) ? 0 : ( Mathf.Clamp((-SpAPower * TerrainAlpha * (Attacker == null ? 1 : AttackerSpA) * (Attacker == null ? 1 : (2 * AttackerLevel + 10))) / ((int)SkillType != 19 ? 250 : 1), -10000, -1) )),
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
                     ((AtkPower == 0) ? 0 : Mathf.Clamp( (-AtkPower * TerrainAlpha * (Attacker == null ? 1 : AttackerATK) * (Attacker == null ? 1 : (2 * AttackerLevel + 10))) / (250) , -10000 , -1 ) ),
                     ((SpAPower == 0) ? 0 : Mathf.Clamp( (-SpAPower * TerrainAlpha * (Attacker == null ? 1 : AttackerSpA) * (Attacker == null ? 1 : (2 * AttackerLevel + 10))) / (250) , -10000 , -1 ) ),
                    (int)SkillType);
            }
        }
    }

    //***************************************************************************��ȫ��ĺ���*********************************************************************************


}