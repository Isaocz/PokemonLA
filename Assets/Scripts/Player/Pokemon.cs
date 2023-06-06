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

    public float FrozenResistance;
    public float ToxicResistance;
    public float ParalysisResistance;
    public float BurnResistance;
    public float SleepResistance;
    public float OtherStateResistance;

    public float TimeStateInvincible;
    protected float StateInvincileTimer = 0.0f;
    protected bool isStateInvincible = false;

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
        else
        {
            if (gameObject.GetComponent<SpriteRenderer>() == null) { gameObject.transform.GetChild(3).GetComponent<SpriteRenderer>().material = NormalMaterial;  }
            else { gameObject.GetComponent<SpriteRenderer>().material = NormalMaterial; }
        }
    }


    public void MarterialChangeToFrozen()
    {
        if (gameObject.GetComponent<SpriteRenderer>() == null)  {     gameObject.transform.GetChild(3).GetComponent<SpriteRenderer>().material = FrozenMaterial;  }
        else { gameObject.GetComponent<SpriteRenderer>().material = FrozenMaterial; }
        animator.speed = 0;
    }
    public void MarterialChangeToSpeedDown()
    {
        if (gameObject.GetComponent<SpriteRenderer>() == null) { gameObject.transform.GetChild(3).GetComponent<SpriteRenderer>().material = SpeedDownMaterial; }
        else { gameObject.GetComponent<SpriteRenderer>().material = SpeedDownMaterial; }
    }
    public void MarterialChangeToToxic()
    {
        if (gameObject.GetComponent<SpriteRenderer>() == null) { gameObject.transform.GetChild(3).GetComponent<SpriteRenderer>().material = ToxicMaterial; }
        else { gameObject.GetComponent<SpriteRenderer>().material = ToxicMaterial; }
    }

    public void MarterialChangeToParalysis()
    {
        if (gameObject.GetComponent<SpriteRenderer>() == null) { gameObject.transform.GetChild(3).GetComponent<SpriteRenderer>().material = ParalysisMaterial; }
        else { gameObject.GetComponent<SpriteRenderer>().material = ParalysisMaterial; }
    }

    public void MarterialChangeToBurn()
    {
        if (gameObject.GetComponent<SpriteRenderer>() == null) { gameObject.transform.GetChild(3).GetComponent<SpriteRenderer>().material = BurnMaterial; }
        else { gameObject.GetComponent<SpriteRenderer>().material = BurnMaterial; }
    }

    public void MarterialChangeToSleep()
    {
        if (gameObject.GetComponent<SpriteRenderer>() == null) { gameObject.transform.GetChild(3).GetComponent<SpriteRenderer>().material = SleepMaterial; }
        else { gameObject.GetComponent<SpriteRenderer>().material = SleepMaterial; }
        //animator.speed = 0.55f;
    }

    public void MarterialChangeToFear()
    {

        if (gameObject.GetComponent<SpriteRenderer>() == null) {  gameObject.transform.GetChild(3).GetComponent<SpriteRenderer>().material = FearMaterial; Debug.Log(gameObject.transform.GetChild(3).GetComponent<SpriteRenderer>().material); }
        else { gameObject.GetComponent<SpriteRenderer>().material = FearMaterial; }
    }

    public void StateMaterialChange()
    {
        if (gameObject.GetComponent<SpriteRenderer>() == null)
        {
            var StateMat = gameObject.transform.GetChild(3).GetComponent<SpriteRenderer>().material;
            var playerTex = gameObject.transform.GetChild(3).GetComponent<SpriteRenderer>().sprite.texture;
            StateMat.SetTexture("_PlayerTex", playerTex);
        }
        else
        {
            var StateMat = gameObject.GetComponent<SpriteRenderer>().material;
            var playerTex = GetComponent<SpriteRenderer>().sprite.texture;
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
    bool isFrozenDone = false;
    bool isFrozenStart = false;
    float EmptyFrozenPointFloat;
    float SpeedBefoerChange;

    //���ô˺���ʱ�������δ��������������״̬��Ϊ������
    public void Frozen(float FrozenTime , float FrozenPoint)
    {
        if (!isFrozenDone)
        {
            EmptyFrozenPointFloat += FrozenPoint*FrozenResistance;
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

    //ֻ�ɱ���һ�������ӳٵ��ã�����ⶳ�ĺ���
    protected void FrozenRemove()
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

    //===========================================================================�䶳�ĺ���=====================================================================================

    //===========================================================================���µĺ���=====================================================================================

    public bool isFearDone = false;
    public bool isFearStart = false;
    float EmptyFearPointFloat;

    //���ô˺���ʱ�������δ���£�״̬��Ϊ����
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
    //ֻ�ɱ���һ�������ӳٵ��ã������ٶȺ��µĺ���
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
    public void EmptyToxicDone(float ToxicPoint)
    {
        Empty EmptyObj = GetComponent<Empty>();
        if (!isToxicDef && EmptyObj.EmptyType01 != 4 && EmptyObj.EmptyType01 != 9 && EmptyObj.EmptyType02 != 4 && EmptyObj.EmptyType02 != 9 && !isToxicDone)
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
                EmptyObj.SpAAbilityPoint /= 2;
            }
        }
    }

    //===========================================================================�����ж��ĺ���=====================================================================================


    //***************************************************************************�Ե��˵ĺ���*********************************************************************************









    //***************************************************************************���Լ��ĺ���*********************************************************************************


    //===========================================================================�ı��ٶȵĺ���=====================================================================================

    //һ�����������ٶ��Ƿ񱻸ı䣬һ������ı�ı���
    bool isSpeedChange = false;

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


    //===========================================================================�ж��ĺ���=====================================================================================


    //һ�����������Ƿ��ж���һ�������ж��ĳ̶�
    public bool isToxicDef;
    public bool isToxicDone = false;
    public bool isToxicStart = false;
    public float ToxicPointFloat = 0;
    float SpAHWBeforeChange;

    //���ô˺���ʱ�������δ��ʼ�ж�����ʼ�ж�
    public void ToxicFloatPlus(float ToxicPoint)
    {
        if (!isStateInvincible && !isToxicDef)
        {
            ToxicPointFloat += ToxicPoint;
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

    //һ�����������Ƿ��ж���һ�������ж��ĳ̶�

    public bool isParalysisDone = false;
    public bool isParalysisStart = false;
    public float ParalysisPointFloat = 0;
    float MoveSpeHWBeforeChange;

    //���ô˺���ʱ�������δ��ʼ�ж�����ʼ�ж�
    public void ParalysisFloatPlus(float ParalysisPoint)
    {
        if (!isStateInvincible && !isParalysisDef)
        {
            ParalysisPointFloat += ParalysisPoint;
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
    public bool isBurnDone = false;
    public bool isBurnStart = false;
    public float BurnPointFloat = 0;
    float AtkHWBeforeChange;

    //���ô˺���ʱ�������δ��ʼ�ж�����ʼ�ж�
    public void BurnFloatPlus(float BurnPoint)
    {

        if (!isStateInvincible && !isBurnDef)
        {
            BurnPointFloat += BurnPoint;
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
    public bool isSleepDone = false;
    public bool isSleepStart = false;
    public float SleepPointFloat = 0;

    //���ô˺���ʱ�������δ��ʼ�ж�����ʼ�ж�
    public void SleepFloatPlus(float SleepPoint)
    {
        if (!isStateInvincible && !isSleepDef)
        {
            SleepPointFloat += SleepPoint;
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


    //***************************************************************************���Լ��ĺ���*********************************************************************************


    //===========================================================================���µĺ���=====================================================================================

}

