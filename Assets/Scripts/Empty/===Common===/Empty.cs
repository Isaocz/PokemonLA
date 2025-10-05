using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Linq;

public class EnumMultiAttribute : PropertyAttribute { }
public class Empty : Pokemon
{


    //���������ʱ��ɵĻ���ֵ����4��������һ������������ɵ��˺���һ���������ֵ��һ����ʾ�ƶ��ľ���,һ����ʾ�ƶ��ٶȣ�һ����ʾ��ʼѪ��
    public float Knock = 5f;
    //���˵ĵ�ǰѪ�������Ѫ��
    [Tooltip("��ǰHP")]
    public int EmptyHp;
    public int maxHP;
    [Tooltip("��ǰ����")]
    public int EmptyShield;
    //���˵ĵȼ�
    [Tooltip("�ȼ�")]
    public int Emptylevel;
    [Tooltip("�����Ƿ������ڳ��������У�������ڳ��������ж�")]
    public bool isBorn;
    [Tooltip("ui��Ѫ��")]
    public EmptyHpBar uIHealth;

    //�����������α�������ʾ���˵���������
    [Header("����")]
    [EnumMultiAttribute]
    public PokemonType.TypeEnum EmptyType01;
    //public int EmptyType01;
    public PokemonType.TypeEnum EmptyType02;

    [Header("����ֵ")]
    //���������������ݣ���ʾ��ɫ����������ֵ,�Լ����ǰ����ֵ,MaxLevel��ʾ�õ��˿��Ե������ߵȼ�
    public int HpEmptyPoint;
    public int AtkEmptyPoint;
    public int DefEmptyPoint;
    public int SpAEmptyPoint;
    public int SpdEmptyPoint;
    public int SpeedEmptyPoint;
    public int MaxLevel;


    public enum EmptyAbillity
    {
        None,
        RoughSkin,//�ֲ�Ƥ��
        Levitate,//Ư��
        IceBody,//����֮�����Ӵ��������
    }
    public EmptyAbillity Abillity;

    //����ֵ
    public float AtkAbilityPoint { get { return AtkAbility; } set { AtkAbility = value; } }
    float AtkAbility;
    public float SpAAbilityPoint { get { return SpAAbility; } set { SpAAbility = value; } }
    float SpAAbility;
    public float DefAbilityPoint { get { return DefAbility; } set { DefAbility = value; } }
    float DefAbility;
    public float SpdAbilityPoint { get { return SpDAbility; } set { SpDAbility = value; } }
    float SpDAbility;
    public float SpeedAbilityPoint { get { return SpeedAbility; } set { SpeedAbility = value; } }
    float SpeedAbility;

    public bool isCanHitAnimation { get { return iscanHitAnimation; } set { iscanHitAnimation = value; } }bool iscanHitAnimation;


    /// <summary>
    /// ���˶���ĳһ���ԵĿ��ԣ��������Ե�Index��ο�Type.cs
    /// </summary>
    public int[] TypeDef = new int[20] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };



    //���˴ݻ�ʱ��ʱ��
    public delegate void EmptyEvent();
    public EmptyEvent DestoryEvent;

    //����2������ֵ����ʾĿ������Ƿ�����,��ʾĿ���Ƿ񱻹���
    public bool isDie = false;
    public bool isHit = false;
    //�޵�״̬
    private bool isInvincible = false;
    public bool Invincible
    {
        get { return isInvincible; }
        set { isInvincible = value; }
    }
    //һ�����˼�ʱ����һ��������ֵ
    float KOTimer = 0;
    float KOPoint;


    //����һ����������ʾ��Ŀ�걻�������õľ��飬Exp����BaseExp�������
    public int Exp;
    public int BaseExp;
    /// <summary>
    /// ��ά�����ֱ�Ϊ���Ŭ��ֵ������͵���(��ֵ�ο�PlayerControler.ChangeHPW())
    /// </summary>
    public Vector2Int HWP;


    /// <summary>
    /// �����ܵ��˺��������������緵��ֵΪ3.0f �� ��õ���ÿ������ܵ��������ֵ����֮1���˺���
    /// </summary>
    /// <returns></returns>
    int MaxDmagePer()
    {
        if (EmptyBossLevel == emptyBossLevel.Boss) { return 6; }
        if (EmptyBossLevel == emptyBossLevel.MiniBoss) { return 3; }
        return 1;
    }


    //��ȡ��Ҷ���
    public PlayerControler player;

    //����һ��������� һ�����������߱���(�����������Ѿ�ת������Pokeomn)
    public new Rigidbody2D rigidbody2D;


    //��ʾ�쳣״̬�ĸ��ֱ���
    /// <summary>
    /// �����Ƿ������Ĭ״̬
    /// </summary>
    public bool isSilence = false;



    /// <summary>
    /// ����boss�ȼ� ��Ϊ��ͨ���� Сboss ��boss ����boss
    /// </summary>
    public enum emptyBossLevel
    {
        NormalEmpty,
        MiniBoss,
        Boss,
        EndBoss
    }
    public emptyBossLevel EmptyBossLevel;


    /// <summary>
    /// ���˱����������ĵ���
    /// </summary>
    public GameObject DropItem;

    /// <summary>
    /// �����ܵ����˺���ʾ
    /// </summary>
    public GameObject FloatingDmg;

    /*
    //��ʾ�����Ƿ��ڱ�������ͨ������
    public bool isMistPlus;
    */

    /// <summary>
    /// ��ʾ�����Ƿ���뱻��������״̬
    /// </summary>
    public bool isSubsititue
    {
        get { return issubstitute; }
        set { issubstitute = value; }
    }
    bool issubstitute;
    /// <summary>
    /// ��ʾ���˱���������ʱ��Ŀ������
    /// </summary>
    public GameObject SubsititueTarget
    {
        get { return subsititueTarget; }
        set { subsititueTarget = value; }
    }
    GameObject subsititueTarget;

    [Tooltip("�Ǳ�ϵ�����Ƿ�ֿ�����")]
    public bool isHailDef;
    [Tooltip("�ǵ��Ҹ�ϵ�����Ƿ�ֿ�ɳ��")]
    public bool isSandStormDef;


    public bool isShadow;


    public Room ParentPokemonRoom
    {
        get { return parentRoom; }
        set { parentRoom = value; }
    }
    Room parentRoom;


    //�����������ж������ʱ����������Ķ������ �� �����ߵĶ����ڣ� �� ��ÿһ�����岿�ַ����List��
    public List<SubEmptyBody> SubEmptyBodyList
    {
        get { return subEmptyBodyList; }
        set { subEmptyBodyList = value; }
    }
    List<SubEmptyBody> subEmptyBodyList = new List<SubEmptyBody> { };


    /// <summary>
    /// �������ж���ڵĵ���ʹ�ã�Ϊ�˷�ֹ����ڱ���ο�Ѫ���ڶ�ʱ�����õ����޵С�
    /// </summary>
    public bool isSubBodyEmptyInvincible
    {
        get { return issubBodyEmptyInvincible; }
        set { issubBodyEmptyInvincible = value; }
    }
    bool issubBodyEmptyInvincible;


    /// <summary>
    /// �����Ƿ���е���
    /// </summary>
    public bool IsHaveDropItem { get { return isHaveDropItem; } set { isHaveDropItem = value; } }
    bool isHaveDropItem;


    //Boss�ķ����͹����ӳɡ�
    protected float BossDefBouns = 1.5f;
    protected float BossAtkBouns = 0.7f;



    /// <summary>
    /// ��������ʱ�Ƿ���Դ�������Ч������������ʱ������η���򲻿ɴ�����
    /// </summary>
    public bool IsDeadrattle
    {
        get { return (!isEmptyFrozenDone) && (!isFearDone) && (!isBlindDone); }
    }



    /// <summary>
    /// �����Ƿ��˺���
    /// </summary>
    public bool isHurt { get { return ishurt; } set { ishurt = value; } }
    bool ishurt;

































































    //=============================��ʼ����������================================

    //���������δ���ı���ٶ�
    float FirstSpeed;
    public void ResetSpeed()
    {
        speed = FirstSpeed;
    }

    /// <summary>
    /// ---start�е���---��������ҵȼ���̬�趨���˵ȼ���
    /// </summary>
    /// <param name="PlayerLevel"></param>
    /// <param name="MaxLevel"></param>
    /// <returns></returns>
    protected int SetLevel(int PlayerLevel,int MaxLevel)
    {
        StartCoroutine(SetInvincible(1.2f));
        if (transform.parent.parent.GetComponent<Room>() != null) { ParentPokemonRoom = transform.parent.parent.GetComponent<Room>(); }
        FirstSpeed = speed;
        int OutPut;
        if (!(EmptyBossLevel == Empty.emptyBossLevel.Boss || EmptyBossLevel == Empty.emptyBossLevel.EndBoss || EmptyBossLevel == Empty.emptyBossLevel.MiniBoss))
        {
            OutPut = Mathf.Clamp( PlayerLevel + (player.playerData.IsPassiveGetList[29] ? 5 : 0) + (Random.Range(-2, 2)  ) , 1  , 100 );
            if (FloorNum.GlobalFloorNum != null)
            {
                OutPut = Mathf.Clamp(OutPut + FloorNum.GlobalFloorNum.EmptyLevelAlpha[FloorNum.GlobalFloorNum.FloorNumber], 1, 100);
                if (FloorNum.GlobalFloorNum.FloorNumber == 0)
                {
                    if (Mathf.Abs(transform.parent.position.x / 30) + Mathf.Abs(transform.parent.position.y / 24) < 3)
                    {
                        OutPut = Random.Range(1, 3);
                    }
                }
            }
            if (OutPut > MaxLevel) { OutPut = MaxLevel; }
        }
        else
        {
            OutPut = Mathf.Clamp(player.Level + (player.playerData.IsPassiveGetList[29] ? 5 : 0) + (Random.Range(-2, 2) ), (player.playerData.IsPassiveGetList[29] ? 10 : 5), 100);
        }
        return OutPut;
    }

    /// <summary>
    /// ȷ�������Ƿ��е���
    /// </summary>
    void SetHaveDropItem()
    {
        if (!(EmptyBossLevel == Empty.emptyBossLevel.Boss || EmptyBossLevel == Empty.emptyBossLevel.EndBoss || EmptyBossLevel == Empty.emptyBossLevel.MiniBoss))
        {
            //Я��̽����������Я������
            if (!player.playerData.IsPassiveGetList[61]) {
                isHaveDropItem = Random.Range(0.0f, 1.0f) + (float)player.LuckPoint / 100 > 0.93f;
            }
            else
            {
                isHaveDropItem = Random.Range(0.0f, 1.0f) + (float)player.LuckPoint / 30 > 0.85f;
                if (isHaveDropItem) { playerUIState.StatePlus(13); }
            }
        }
        else
        {
            isHaveDropItem = true;
        }
    }

    /// <summary>
    /// ---start�е���---����������ֵ��ʼ������Ѫ��
    /// </summary>
    /// <param name="level"></param>
    protected void EmptyHpForLevel(int level)
    {
        float BossBonus = 1.7f;
        float MiniBossBonus = 1.35f;
        if (FloorNum.GlobalFloorNum != null)
        {
            BossBonus = FloorNum.GlobalFloorNum.FloorBossHPBonus[FloorNum.GlobalFloorNum.FloorNumber];
        }
        EmptyHp = (int)((level + 10 + (int)(((float)level * HpEmptyPoint * 2) / 100.0f)) * ((EmptyBossLevel == Empty.emptyBossLevel.Boss || EmptyBossLevel == Empty.emptyBossLevel.EndBoss) ? BossBonus : 1) * ((EmptyBossLevel == Empty.emptyBossLevel.MiniBoss) ? MiniBossBonus : 1));
        maxHP = EmptyHp;
        SetHaveDropItem();
    }

    /// <summary>
    /// ---start�е���---����������ֵ��ʼ��������������ֵ
    /// </summary>
    /// <param name="level"></param>
    /// <param name="Ability"></param>
    /// <returns></returns>
    public int AbilityForLevel(int level, int Ability)
    {
        return (Ability * 2 * level) / 100 + 5;
    }



    /// <summary>
    /// ��ʼ�¼�����¼�
    /// </summary>
    public virtual void StartOverEvent()
    {
        GetShield((int)(maxHP / 3.0f));
    }

    //=============================��ʼ����������================================










    //====================����Ѫ���ı�======================

    /// <summary>
    /// �����Ƿ񱻵��������
    /// </summary>
    public bool IsBeFalseSwipe { get { return isBeFalseSwipe; } set { isBeFalseSwipe = value; } }
    bool isBeFalseSwipe;

    /// <summary>
    ///     //����һ���������ı���˶����Ѫ��
    /// </summary>
    /// <param name="Dmage">�﹥�˺�</param>
    /// <param name="SpDmage">�ع��˺�</param>
    /// <param name="SkillType">�˺����ԣ����ֲο�Type.cs��</param>
    public void EmptyHpChange(float  Dmage , float SpDmage , int SkillType, bool Crit = false)
    {
        if (isShadow && Dmage + SpDmage >= 0)
        {
            animator.SetTrigger("ShadowOver");
        }
        else
        {
            //�޵�
            if (isInvincible)
            {
                return;
            }

            PokemonType.TypeEnum enumVaue = (PokemonType.TypeEnum)SkillType;

            //Debug.Log(name);
            //Debug.Log(player);
            //Debug.Log(player.playerData);
            //Debug.Log(player.playerData.IsPassiveGetList[118]);

            Dmage = Dmage * (player.playerData.IsPassiveGetList[118] ? 1 : (((Weather.GlobalWeather.isRain && enumVaue == PokemonType.TypeEnum.Water) ? (Weather.GlobalWeather.isRainPlus ? 1.8f : 1.3f) : 1)
                * ((Weather.GlobalWeather.isRain && enumVaue == PokemonType.TypeEnum.Fire) ? 0.5f : 1)
                * ((Weather.GlobalWeather.isSunny && enumVaue == PokemonType.TypeEnum.Water) ? 0.5f : 1)
                * ((Weather.GlobalWeather.isSunny && enumVaue == PokemonType.TypeEnum.Fire) ? (Weather.GlobalWeather.isSunnyPlus ? 1.8f : 1.3f) : 1)));
            SpDmage = SpDmage * (player.playerData.IsPassiveGetList[118] ? 1 : (((Weather.GlobalWeather.isRain && enumVaue == PokemonType.TypeEnum.Water) ? (Weather.GlobalWeather.isRainPlus ? 1.8f : 1.3f) : 1)
                * ((Weather.GlobalWeather.isRain && enumVaue == PokemonType.TypeEnum.Fire) ? 0.5f : 1)
                * ((Weather.GlobalWeather.isSunny && enumVaue == PokemonType.TypeEnum.Water) ? 0.5f : 1)
                * ((Weather.GlobalWeather.isSunny && enumVaue == PokemonType.TypeEnum.Fire) ? (Weather.GlobalWeather.isSunnyPlus ? 1.8f : 1.3f) : 1)));

            float typeDef = (TypeDef[SkillType] < 0 ? (Mathf.Pow(1.2f, -TypeDef[SkillType])) : 1) * (TypeDef[SkillType] > 0 ? (Mathf.Pow(0.8f, TypeDef[SkillType])) : 1);

            //����Ѫ���ı�ǰ��Ѫ��
            int BeforeHP = EmptyHp;
            //����Ѫ���ı�ǰ�Ļ���
            int BeforeShield = EmptyShield;

            //�˺�
            if (Dmage + SpDmage >= 0)
            {
                int allDmg = 0;
                //�����˺�
                if (SkillType != 19)
                {
                    allDmg = Mathf.Clamp((int)((Dmage + SpDmage) * typeDef * (PokemonType.TYPE[SkillType][(int)EmptyType01]) * PokemonType.TYPE[SkillType][(int)EmptyType02]), 1, ((EmptyBossLevel == Empty.emptyBossLevel.Boss || EmptyBossLevel == Empty.emptyBossLevel.EndBoss || EmptyBossLevel == Empty.emptyBossLevel.MiniBoss) ? (maxHP / MaxDmagePer()) : 100000));
                    //���Ϊ����״̬
                    if (allDmg > 0 && !ishurt) { ishurt = true; }
                    //�ǳ��ܳ���
                    if (!isInPsychicTerrain) 
                    {
                        EmptyBeingHurt(allDmg);
                    }
                    //���ܳ������ߵ��˺�
                    else
                    {
                        if(Mathf.Abs((int)allDmg) > (int)(maxHP / 16))
                        {
                            EmptyBeingHurt(allDmg);
                        }
                    }
                }
                //
                else
                {
                    allDmg = Mathf.Clamp((int)((Dmage + SpDmage) * typeDef), 1, ((EmptyBossLevel == Empty.emptyBossLevel.Boss || EmptyBossLevel == Empty.emptyBossLevel.EndBoss || EmptyBossLevel == Empty.emptyBossLevel.MiniBoss) ? (maxHP / MaxDmagePer()) : 100000));
                    //���Ϊ����״̬
                    if (allDmg > 0 && !ishurt) { ishurt = true; }
                    //�ǳ��ܳ���
                    if (!isInPsychicTerrain) 
                    {
                        EmptyBeingHurt(allDmg);
                    }
                    //���ܳ������ߵ��˺�
                    else
                    {
                        if (Mathf.Abs((int)allDmg) > (int)(maxHP / 16))
                        {
                            EmptyBeingHurt(allDmg);
                        }
                    }
                }
                //��ʾ�˺�����
                if (InitializePlayerSetting.GlobalPlayerSetting.isShowDamage && GetComponent<SubEmptyBody>() == null && FloatingDmg)
                {
                    GameObject fd = Instantiate(FloatingDmg, transform.position + Vector3.right * Random.Range(-0.8f, 0.8f), Quaternion.identity) as GameObject;
                    if (Dmage > SpDmage)
                    {
                        fd.transform.GetComponent<damageShow>().SetText(allDmg, Crit, false, false);
                    }
                    else
                    {
                        fd.transform.GetComponent<damageShow>().SetText(allDmg, Crit, false, true);
                    }
                }
                EmptySleepRemove();

            }
            //��Ѫ
            else
            {
                EmptyHp = Mathf.Clamp(EmptyHp - (int)(Dmage + SpDmage), (IsBeFalseSwipe ? 1 : 0), maxHP);
                if (InitializePlayerSetting.GlobalPlayerSetting.isShowDamage && FloatingDmg)
                {
                    int allRecover = -(int)(Dmage + SpDmage);
                    GameObject fd = Instantiate(FloatingDmg, transform.position + Vector3.right * Random.Range(-0.8f, 0.8f), Quaternion.identity);
                    fd.transform.GetComponent<damageShow>().SetText(allRecover, Crit, true, false);
                }
            }

            //Debug.Log(Mathf.Clamp((int)((Dmage + SpDmage) * typeDef * (Type.TYPE[SkillType][(int)EmptyType01]) * Type.TYPE[SkillType][(int)EmptyType02]), 1, 100000) + " + " + "Dmage:" + (int)(Dmage + SpDmage));
            Debug.Log(
                "Dmage=" + Dmage + "  " 
                + "SpDmage=" + SpDmage + "  "
                + "typeDef=" + typeDef + "  "
                + "Type.TYPE[SkillType][(int)EmptyType01]=" + PokemonType.TYPE[SkillType][(int)EmptyType01] + "  "
                + "Type.TYPE[SkillType][(int)EmptyType02]=" + PokemonType.TYPE[SkillType][(int)EmptyType02] + "  ");
            //�˺�����UI
            if ((int)Dmage + (int)SpDmage > 0)
            {
                //Ѫ�������ı�����ö��� ���ܸı䲻����
                if (BeforeHP != EmptyHp) {
                    if (!isCanHitAnimation && animator != null) { animator.SetTrigger("Hit"); }
                }
                //Debug.Log((float)EmptyHp / (float)maxHP + "=" + (float)EmptyHp + "/" + (float)maxHP);
                uIHealth.Per = (float)EmptyHp / (float)maxHP;
                uIHealth.ChangeHpDown();
                uIHealth.ShieldPer = (float)EmptyShield / (float)maxHP;
                uIHealth.ChangeShieldDown();
            }
            //��Ѫ����UI
            else
            {
                uIHealth.Per = (float)EmptyHp / (float)maxHP;
                uIHealth.ChangeHpUp();
            }
            //������
            if (IsBeFalseSwipe)
            {
                IsBeFalseSwipe = false;
            }
        }
    }





    void EmptyBeingHurt(int allDmg)
    {
        //�л���ʱ�۳�����
        if (EmptyShield > 0)
        {
            int BeforeShield = EmptyShield;
            EmptyShield = Mathf.Clamp(EmptyShield - allDmg, 0, BeforeShield);
            if (EmptyShield <= 0 && GetComponent<SubEmptyBody>() == null)
            {
                if (EmptyBossLevel == emptyBossLevel.Boss || EmptyBossLevel == emptyBossLevel.EndBoss || EmptyBossLevel == emptyBossLevel.MiniBoss) {  }
                else
                {
                    GameObject ShieldBreakEffect = PublicEffect.StaticPublicEffectList.ReturnAPublicEffect(3);
                    GameObject shieldBreakEffect = Instantiate(ShieldBreakEffect, transform.position + Vector3.up * 0.5f, Quaternion.identity);
                    shieldBreakEffect.SetActive(true);
                }
                //���ܱ�־����
                //uIHealth.BreakShieldMark();
                ShieldBreakEvent();
            }
        }
        //�޻���ʱ�۳�����ֵ
        else
        {
            EmptyHp = Mathf.Clamp(EmptyHp - allDmg, (IsBeFalseSwipe ? 1 : 0), maxHP);
        }
    }

    public void GetShield(int point)
    {
        //��ǰ����Ϊ0 �һ�������ֵ����0 ���ui���ܱ�־
        //if (EmptyShield <= 0 && point > 0 )
        //{
        //    uIHealth.GetShieldMark();
        //}

        point = Mathf.Clamp(point, 0, maxHP);
        EmptyShield += point;
        uIHealth.ShieldPer = (float)EmptyShield / (float)maxHP;
        //Debug.Log(uIHealth.ShieldPer);
        uIHealth.ChangeShieldUp();
        if (subEmptyBodyList.Count != 0)
        {
            for (int i = 0; i < subEmptyBodyList.Count; i++)
            {
                subEmptyBodyList[i].EmptyShield += point;
                subEmptyBodyList[i].NowShield = subEmptyBodyList[i].EmptyShield;
            }
        }
    } 

    /// <summary>
    /// �״�����ʱ�������¼�
    /// </summary>
    public void FirstHurtEvent()
    {
        ishurt = true;
        if (subEmptyBodyList.Count != 0)
        {
            for (int i = 0; i < subEmptyBodyList.Count; i++)
            {
                subEmptyBodyList[i].isHurt = true;
            }
        }
    }


    /// <summary>
    /// �ƶ��¼�
    /// </summary>
    public virtual void ShieldBreakEvent()
    {
        //��boss�ƶ�ʱ��ä
        if (EmptyBossLevel != emptyBossLevel.Boss && EmptyBossLevel != emptyBossLevel.EndBoss)
        {
            Blind(1.5f , 10.0f);
        }
    }



    //====================����Ѫ���ı�======================









    //========================���˱��������=========================
    /// <summary>
    /// ����һ�����������õ��˱����˵ľ���
    /// </summary>
    /// <param name="KnockOutPoint"></param>
    public void EmptyKnockOut(float KnockOutPoint)
    {
        //�л���ʱ������
        if (EmptyShield <= 0) {
            isHit = true;
            KOPoint = KnockOutPoint;
        }
    }

    /// <summary>
    /// ---Update�е���---�������˱�����ʱ�ƶ�����
    /// </summary>
    protected void EmptyBeKnock()
    {
        if (!isDie && isHit)
        {
            if (rigidbody2D != null) {
                KOTimer += Time.deltaTime;
                Vector2 position = rigidbody2D.position;
                Vector2 KODirection = new Vector2(player.transform.position.x - transform.position.x, player.transform.position.y - transform.position.y);
                KODirection.Normalize();
                position.x = position.x - KOPoint * KODirection.x * Time.deltaTime;
                position.y = position.y - KOPoint * KODirection.y * Time.deltaTime;
                rigidbody2D.position = position;
            }
        }
        if (KOTimer >= 0.15f)
        {
            isHit = false;
            KOTimer = 0;
        }
    }

    /// <summary>
    /// ---Update�е���---�������˱�����ʱ�ƶ�����
    /// </summary>
    protected void EmptyBeKnockByTransform()
    {
        if (!isDie && isHit)
        {
            KOTimer += Time.deltaTime;
            Vector2 position = transform.position;
            Vector2 KODirection = new Vector2(player.transform.position.x - transform.position.x, player.transform.position.y - transform.position.y);
            KODirection.Normalize();
            position.x = position.x - KOPoint * KODirection.x * Time.deltaTime / (Mathf.Pow(rigidbody2D.mass, 0.5f));
            position.y = position.y - KOPoint * KODirection.y * Time.deltaTime / (Mathf.Pow(rigidbody2D.mass, 0.5f));
            transform.position = position;
        }
        if (KOTimer >= 0.15f)
        {
            isHit = false;
            KOTimer = 0;
        }
    }
    //========================���˱��������=========================








    //========================������ײ�˺�========================

    /// <summary>
    /// ---OnColliderEnter2D�е���---�����������ɴ����˺�
    /// </summary>
    /// <param name="player"></param>
    public void EmptyTouchHit(GameObject playerObj)
    {
        if (playerObj.layer != 23) {
            //���������������ң�ʹ��ҿ۳�һ��Ѫ��
            PlayerControler playerControler = playerObj.gameObject.GetComponent<PlayerControler>();
            PokemonHpChange(this.gameObject, playerObj.gameObject, 10, 0, 0, PokemonType.TypeEnum.No);
            if (playerControler != null)
            {
                //playerControler.ChangeHp(-(10 * AtkAbilityPoint * (2 * Emptylevel + 10) / 250 ) ,0, 0);
                playerControler.KnockOutPoint = Knock;
                playerControler.KnockOutDirection = (playerControler.transform.position - transform.position).normalized;
                if (playerControler.playerData.IsPassiveGetList[115])
                {
                    PokemonHpChange(playerControler.gameObject, this.gameObject, 10, 0, 0, PokemonType.TypeEnum.No);
                }
            }
        }
        
    }




    protected bool isInfatuationDmageDone;
    float InfatuationDmageCDTimer;
    /// <summary>
    /// �����˱��Ȼ�ʱ�����÷���
    /// </summary>
    public void UpdateInfatuationDmageCDTimer()
    {
        if (isInfatuationDmageDone)
        {
            InfatuationDmageCDTimer += Time.deltaTime;
            if(InfatuationDmageCDTimer >= 0.8)
            {
                InfatuationDmageCDTimer = 0; isInfatuationDmageDone = false;
            }
        }
    }

    public void InfatuationEmptyTouchHit(GameObject TargetEmpty)
    {
        if (!isInfatuationDmageDone) {
            //���������������ң�ʹ��ҿ۳�һ��Ѫ��
            Empty e = TargetEmpty.gameObject.GetComponent<Empty>();
            if (e != null)
            {
                PokemonHpChange(this.gameObject, e.gameObject, 10, 0, 0, PokemonType.TypeEnum.No);
                e.EmptyKnockOut(Knock);
                isInfatuationDmageDone = true;
            }
        }
    }

    //========================������ײ�˺�========================










    //=============================�����¼�===========================

    /// <summary>
    /// ---Update�е���---��Ѫ��С�ڵ���0ʱ��һ�þ����Ŭ�������˽�����������
    /// </summary>
    public void EmptyDie()
    {
        //ÿ֡���һ�Σ���Ŀ��Ѫ��С��0ʱ����Ŀ��
        if (EmptyHp <= 0)
        {
            //isDeadrattle = (!isEmptyFrozenDone) && (!isFearDone) && (!isBlindDone);
            FrozenRemove();
            Destroy(rigidbody2D);
            RemoveChild();
            if (!isDie)
            {
                if (GetComponent<Collider2D>()) { GetComponent<Collider2D>().enabled = false; }
                player.ChangeEx((int)(Exp * ((EmptyBossLevel == Empty.emptyBossLevel.Boss || EmptyBossLevel == Empty.emptyBossLevel.EndBoss ) ? 1.8f : 1.3f)));
                player.ChangeHPW(HWP);

                //��AP
                if (FloorNum.GlobalFloorNum != null && ScoreCounter.Instance != null)
                {
                    ScoreCounter.Instance.EmptyBounsAP += APBounsPoint.EmptyBouns(this , FloorNum.GlobalFloorNum.FloorNumber);
                }

                if (player.playerData.IsPassiveGetList[134] && (EmptyType01 == PokemonType.TypeEnum.Dark || EmptyType02 == PokemonType.TypeEnum.Dark) ) { player.ChangeHPW(HWP); }
                Room r = transform.parent.parent.GetComponent<Room>();
                if (r == null) { r = ParentPokemonRoom; }
                r.isClear -= 1;

                if (DestoryEvent != null) { DestoryEvent(); }
                if (player.playerData.IsPassiveGetList[89] && player.playerData.AttackWeightCount < 6)
                {
                    player.playerData.AttackWeightCount++;
                    player.playerData.AtkBounsJustOneRoom++;
                    player.ReFreshAbllityPoint();
                }
                if (player.playerData.IsPassiveGetList[90] && player.playerData.SpAtkSpecsCount < 6)
                {
                    player.playerData.SpAtkSpecsCount++;
                    player.playerData.SpABounsJustOneRoom++;
                    player.ReFreshAbllityPoint();
                }
                isDie = true;
                //�����¼�
                DieEvent();
            }
            
            animator.SetTrigger("Die");
        }
    }
    /// <summary>
    /// ���٣�������������������Ϻ�ͨ����������
    /// </summary>
    public void EmptyDestroy()
    {
        if(DropItem != null)
        {
            EmptyDrop();
        }
        Destroy(gameObject);
    }
    

    /// <summary>
    /// �ӳ����ٵ���
    /// </summary>
    /// <param name="time"></param>
    public void EmptyDelayDestroy(float time)
    {
        if (DropItem != null)
        {
            EmptyDrop();
        }
        Destroy(gameObject  ,time);
    }
    
    public void EmptyDrop()
    {
        if (IsHaveDropItem) {
            
            if ((EmptyBossLevel == Empty.emptyBossLevel.Boss || EmptyBossLevel == Empty.emptyBossLevel.EndBoss))
            {
                Vector2 DropPosition = new Vector2(Mathf.Clamp(transform.position.x, parentRoom.transform.position.x - 12.0f, parentRoom.transform.position.x + 12.0f), Mathf.Clamp(transform.position.y, parentRoom.transform.position.y - 7.0f, parentRoom.transform.position.y + 7.0f));
                if (!player.playerData.IsPassiveGetList[134]) {
                    Instantiate(DropItem, DropPosition, Quaternion.identity, parentRoom.transform).GetComponent<RandomSkillItem>().isLunch = true;
                    Instantiate(DropItem, DropPosition, Quaternion.identity, parentRoom.transform).GetComponent<RandomSkillItem>().isLunch = true;
                    Instantiate(DropItem, DropPosition, Quaternion.identity, parentRoom.transform).GetComponent<RandomSkillItem>().isLunch = true;
                }
            }
            else
            {

                Vector2 DropPosition = new Vector2(Mathf.Clamp(transform.position.x, parentRoom.transform.position.x - 12.0f, parentRoom.transform.position.x + 12.0f), Mathf.Clamp(transform.position.y, parentRoom.transform.position.y - 7.0f, parentRoom.transform.position.y + 7.0f));
                if (!player.playerData.IsPassiveGetList[134]) {
                    Instantiate(DropItem, DropPosition, Quaternion.identity, parentRoom.transform);
                }

            }
            IsHaveDropItem = false;
            playerUIState.StateDestory(13);
            DropItem = null;
        }
    }

    /// <summary>
    /// ����ʱ�����¼�
    /// </summary>
    public virtual void DieEvent()
    {
        if (ChildrenList.Count != 0)
        {
            ParentDie();
        }
        if (ParentEmptyByChild != null)
        {
            ParentEmptyByChild.ChildDie(this);
        }
        if (PartnerEmpty != null)
        {
            DieAsPartner();
        }
    }



    //=============================�����¼�===========================




















    //===============================����ʱ������������=================================

    /// <summary>
    /// ����ͨ�����߼�ⷶΧ��Ŀ��ĵ��ˣ�������ʱʹ�ô˷���Ѱ�ҵ���
    /// </summary>
    /// <param name="Range">�õ��˵���Ұ��Χ</param>
    public Empty InfatuationForRangeRayCastEmpty(float Range)
    {
        //����ĵ���Ŀ��
        Empty OutPutEmpty = null;
        //�����������������1��Ҳ���Ƿ������г��õ�������ĵ��ˣ�ʱ����������
        if (transform.parent.childCount >= 1)
        {
            float D = 1000;
            //������ǰ���������е���
            foreach (Transform e in transform.parent)
            {
                //���e�����Լ�,���߼�⣬����ɹ����e
                if(e.gameObject != this.gameObject)
                {
                    RaycastHit2D SearchTarget = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + 0.5f), new Vector2(e.transform.position.x - transform.position.x, e.transform.position.y - transform.position.y), Range , LayerMask.GetMask("Empty", "Enviroment", "Room", "EmptyFly"));
                    if (SearchTarget.collider != null && SearchTarget.collider.gameObject != gameObject && ( SearchTarget.transform.tag == "Empty" || SearchTarget.transform.tag == "EmptyFly"))
                    {
                        if((transform.position - e.transform.position).magnitude < D)
                        {
                            OutPutEmpty = e.GetComponent<Empty>();
                            D = (transform.position - e.transform.position).magnitude;
                        }
                    }

                }
            }
        }
        return OutPutEmpty;
    }




    /// <summary>
    /// ���ڲ�ͨ�����߼��ĵ��ˣ��繢������������ҵķ��е��ˣ���������ʱʹ�ô˷������������Լ���������ĵ���
    /// </summary>
    /// <returns></returns>
    public Empty InfatuationForDistanceEmpty()
    {
        //����ĵ���Ŀ��
        Empty OutPutEmpty = null;
        List<Empty> checkList = _mTool.GetAllFromTransform<Empty>(ParentPokemonRoom.EmptyFile());
        //_mTool.DebugLogList<Empty>(checkList);
        //�����������������1��Ҳ���Ƿ������г��õ�������ĵ��ˣ�ʱ����������
        if (checkList.Count >= 1)
        {
            float D = 1000;
            //������ǰ���������е���
            foreach (Empty e in checkList)
            {
                //���e�����Լ�,������룬���С�ڵ�ǰ�������e
                if (e.gameObject != this.gameObject)
                {                
                    if ((transform.position - e.transform.position).magnitude < D)     
                    {
                        OutPutEmpty = e;
                        D = (transform.position - e.transform.position).magnitude; 
                    }
                }
            }
        }
        return OutPutEmpty;
    }

    //===============================����ʱ������������=================================







    //=========================��ʱ�������ʱ��=====================

    public void UpdateEmptyChangeHP()
    {
        if (rigidbody2D != null) {
            if (rigidbody2D.bodyType != RigidbodyType2D.Static) { rigidbody2D.velocity = Vector2.zero; }
        }
        if (isShake) { ShakeUpdate(); }
        if (isToxicDone) { EmptyToxic(); }
        if (isBurnDone) { EmptyBurn(); }
        if (isParalysisDone) { EmptyParalysisJudge(); } else { isCanNotMoveWhenParalysis = false; }
        if (Weather.GlobalWeather.isHail) { EmptyHail(); }
        if (Weather.GlobalWeather.isSandstorm) { EmptySandStorm(); }
        if (isEmptyCurseDone) { EmptyCurseDmage(); }
        if (isInMistyTerrain) { EmptyGrassyTerrainHeal(); }
    }


    //========================= ����¼�========================

    /// <summary>
    /// ���˵���Լ�ʱ����ÿ��ʱ0.5s����һ���ж������ͨ���ж�����˿����ƶ� ��ͨ���������ƶ�ֱ���ж�ͨ��
    /// </summary>
    protected float EmptyParalysisJudgeTimer;
    public bool isCanNotMoveWhenParalysis;
    /// <summary>
    /// ���˵���Լ�ʱ����ÿ��ʱ0.5s����һ���ж���
    /// </summary>
    void EmptyParalysisJudge()
    {
        EmptyParalysisJudgeTimer += Time.deltaTime;
        if (EmptyParalysisJudgeTimer >= 0.5)
        {
            if (Random.Range(0.0f, 1.0f) > 0.65f) { isCanNotMoveWhenParalysis = true; }
            else { isCanNotMoveWhenParalysis = false; }
            EmptyParalysisJudgeTimer = 0;
        }

    }
    //=========================����¼�========================


    //=========================�ж��¼�========================

    /// <summary>
    /// ���˵��ж���ʱ����ÿ��ʱ������һ�ζ�
    /// </summary>
    protected float EmptyToxicTimer;
    /// <summary>
    /// �����ж�ʱ����˵�Ѫ
    /// </summary>
    void EmptyToxic()
    {
        EmptyToxicTimer += Time.deltaTime;
        if(EmptyToxicTimer >= 2)
        {
            EmptyToxicTimer += Time.deltaTime;
            PokemonHpChange(null, this.gameObject, Mathf.Clamp((((float)maxHP) / 16) * ToxicResistance, 1, (EmptyBossLevel == Empty.emptyBossLevel.Boss || EmptyBossLevel == Empty.emptyBossLevel.EndBoss || EmptyBossLevel == Empty.emptyBossLevel.MiniBoss) ? 8 : 10), 0, 0, PokemonType.TypeEnum.IgnoreType);
            EmptyToxicTimer = 0;
        }

    }
    //=========================�ж��¼�========================

    //=========================�ж��¼�========================

    /// <summary>
    /// ���˵����˼�ʱ����ÿ��ʱ��������һ��
    /// </summary>
    protected float EmptyBurnTimer;
    /// <summary>
    /// ��������ʱ����˵�Ѫ
    /// </summary>
    void EmptyBurn()
    {
        EmptyBurnTimer += Time.deltaTime;
        if (EmptyBurnTimer >= 2)
        {
            EmptyBurnTimer += Time.deltaTime;
            PokemonHpChange(null , this.gameObject , Mathf.Clamp((((float)maxHP) / 16) * BurnResistance, 1, (EmptyBossLevel == Empty.emptyBossLevel.Boss || EmptyBossLevel == Empty.emptyBossLevel.EndBoss || EmptyBossLevel == Empty.emptyBossLevel.MiniBoss) ? 8 : 10), 0 , 0 , PokemonType.TypeEnum.IgnoreType);
            //EmptyHpChange(Mathf.Clamp((((float)maxHP) / 16) * BurnResistance, 1, isBoos ? 8 : 10), 0, 19);
            EmptyBurnTimer = 0;
        }

    }
    //=========================�ж��¼�========================

    //=========================�����˺��¼�========================

    /// <summary>
    /// ���˵ı�����ʱ����ÿ��ʱ������һ�ζ�
    /// </summary>
    protected float EmptyHailTimer;
    /// <summary>
    /// ���ݱ���ʱ����˵�Ѫ
    /// </summary>
    void EmptyHail()
    {
        if (EmptyType01 != PokemonType.TypeEnum.Ice && EmptyType02 != PokemonType.TypeEnum.Ice && !isHailDef)
        {
            EmptyHailTimer += Time.deltaTime;
            if (EmptyHailTimer >= 2)
            {
                EmptyHailTimer += Time.deltaTime;
                if (Weather.GlobalWeather.isHailPlus)
                {
                    PokemonHpChange(null, this.gameObject, Mathf.Clamp((((float)maxHP) / 16) * OtherStateResistance, 1, (EmptyBossLevel == Empty.emptyBossLevel.Boss || EmptyBossLevel == Empty.emptyBossLevel.EndBoss || EmptyBossLevel == Empty.emptyBossLevel.MiniBoss) ? 8 : 10), 0, 0, PokemonType.TypeEnum.IgnoreType);
                    EmptyHailTimer = 0;
                }
                else
                {
                    PokemonHpChange(null, this.gameObject, Mathf.Clamp((((float)maxHP) / 16) * OtherStateResistance, 1, (EmptyBossLevel == Empty.emptyBossLevel.Boss || EmptyBossLevel == Empty.emptyBossLevel.EndBoss || EmptyBossLevel == Empty.emptyBossLevel.MiniBoss) ? 8 : 10), 0, 0, PokemonType.TypeEnum.IgnoreType);
                    EmptyHailTimer = 0;
                }
            }
        }
    }
    //=========================�����˺��¼�========================

    //=========================ɳ���˺��¼�========================

    /// <summary>
    /// ���˵�ɳ����ʱ����ÿ��ʱ������һ�ζ�
    /// </summary>
    protected float EmptySandStormTimer;
    /// <summary>
    /// ����ɳ��ʱ����˵�Ѫ
    /// </summary>
    void EmptySandStorm()
    {
        if (EmptyType01 != PokemonType.TypeEnum.Ground && EmptyType01 != PokemonType.TypeEnum.Rock && EmptyType01 != PokemonType.TypeEnum.Steel && EmptyType02 != PokemonType.TypeEnum.Ground && EmptyType02 != PokemonType.TypeEnum.Rock && EmptyType02 != PokemonType.TypeEnum.Steel && !isSandStormDef)
        {
            EmptySandStormTimer += Time.deltaTime;
            if (EmptySandStormTimer >= 2)
            {
                EmptySandStormTimer += Time.deltaTime;
                if (Weather.GlobalWeather.isSandstormPlus)
                {
                    PokemonHpChange(null, this.gameObject, Mathf.Clamp((((float)maxHP) / 8) * OtherStateResistance, 1, (EmptyBossLevel == Empty.emptyBossLevel.Boss || EmptyBossLevel == Empty.emptyBossLevel.EndBoss || EmptyBossLevel == Empty.emptyBossLevel.MiniBoss) ? 16 : 20), 0, 0, PokemonType.TypeEnum.IgnoreType);
                    EmptySandStormTimer = 0;
                }
                else
                {
                    PokemonHpChange(null, this.gameObject, Mathf.Clamp((((float)maxHP) / 16) * OtherStateResistance, 1, (EmptyBossLevel == Empty.emptyBossLevel.Boss || EmptyBossLevel == Empty.emptyBossLevel.EndBoss || EmptyBossLevel == Empty.emptyBossLevel.MiniBoss) ? 8 : 10), 0, 0, PokemonType.TypeEnum.IgnoreType);
                    EmptySandStormTimer = 0;
                }
            }
        }
    }
    //=========================ɳ���˺��¼�========================

    //=========================�����¼�========================

    /// <summary>
    /// ���˵������ʱ����ÿ��ʱ5s����һ��
    /// </summary>
    protected float EmptyCurseTimer;
    /// <summary>
    /// ��������ʱ����˵�Ѫ
    /// </summary>
    void EmptyCurseDmage()
    {
        if (EmptyCurseTimer == 0)
        {
            PokemonHpChange(null, this.gameObject, Mathf.Clamp(( ((EmptyBossLevel == Empty.emptyBossLevel.Boss || EmptyBossLevel == Empty.emptyBossLevel.EndBoss || EmptyBossLevel == Empty.emptyBossLevel.MiniBoss) ? ((float)EmptyHp) : ((float)maxHP)) / 4), 1, 10000), 0, 0, PokemonType.TypeEnum.IgnoreType);
        }
        EmptyCurseTimer += Time.deltaTime;
        if (EmptyCurseTimer >= 5)
        {
            EmptyCurseTimer = 0;
        }

    }
    //=========================�����¼�========================


    //=========================��ݳ��ػ�Ѫ�¼�========================
    /// <summary>
    /// ���˵���ݳ��ػ�Ѫ��ʱ����ÿ��ʱ5s��ݳ��ػ�Ѫһ��
    /// </summary>
    protected float EmptyGrassyTerrainTimer;
    /// <summary>
    /// ������ݳ���ʱ����˻�Ѫ
    /// </summary>
    void EmptyGrassyTerrainHeal()
    {
        if (EmptyGrassyTerrainTimer == 0)
        {
            PokemonHpChange(null, this.gameObject, 0, 0, (int)Mathf.Clamp(( (float)maxHP / 16), 1, 10), PokemonType.TypeEnum.IgnoreType);
        }
        EmptyGrassyTerrainTimer += Time.deltaTime;
        if (EmptyGrassyTerrainTimer >= 5)
        {
            EmptyGrassyTerrainTimer = 0;
        }

    }
    //=========================��ݳ��ػ�Ѫ�¼�========================





    // �ڰ뾶��Χ��Ѱ�ҹ���Ŀ��
    public GameObject FindAtkTarget(float radius)
    {
        GameObject target = null;
        Empty nearlyEmptyObj = InfatuationForRangeRayCastEmpty(radius);
        if (!isEmptyInfatuationDone || transform.parent.childCount <= 1 || nearlyEmptyObj == null)
        {
            if (isSubsititue && SubsititueTarget != null && Vector3.Distance(transform.position, SubsititueTarget.transform.position) <= radius)
            {
                target = SubsititueTarget;
            }
            else if(Vector3.Distance(transform.position, player.transform.position) <= radius)
            {
                target = player.gameObject;
            }
        }
        else if (nearlyEmptyObj)
        {
            target = nearlyEmptyObj.gameObject;
        }
        return target;
    }

    /// <summary>
    /// ---Update��FixedUpdate�е���---������һ������������ҽ�������Ϊ֮ǰ����Ҷ������٣�������Ҫ���»�ȡ���
    /// </summary>
    public void ResetPlayer()
    {
        if (player == null)
        {
            player = GameObject.FindObjectOfType<PlayerControler>();
        }
    }


    /// <summary>
    /// ����������ʱ���ѵ��˵���������Ч���Ӷ���ĸ��ӹ�ϵ�Ƴ�����ֹ����Ч�����ŵ���һ��ͻȻ��ʧ
    /// </summary>
    void RemoveChild()
    {
        foreach (Transform child in transform)
        {
            if(child.GetComponent<ParticleSystem>() != null)
            {
                child.transform.parent = transform.parent.parent;
                ParticleSystem _ps = child.GetComponent<ParticleSystem>();
                var main = _ps.main;
                _ps.Stop(true,ParticleSystemStopBehavior.StopEmitting);
                main.loop = false;
            }
        }
    }
    

    /// <summary>
    /// �����ڼ����˴��ͺ��⴫�͵ĵ���û���ϰ���
    /// </summary>
    /// <returns></returns>
    public bool isThisPointEmpty( Vector3 P )
    {
        RaycastHit2D SearchEmpty01 = Physics2D.Raycast(new Vector2(P.x, P.y + 0.25f), Vector2.left + Vector2.up, 0.6f, LayerMask.GetMask("Enviroment", "Water"));
        RaycastHit2D SearchEmpty02 = Physics2D.Raycast(new Vector2(P.x, P.y + 0.25f), Vector2.left + Vector2.down, 0.6f, LayerMask.GetMask("Enviroment", "Water"));
        RaycastHit2D SearchEmpty03 = Physics2D.Raycast(new Vector2(P.x, P.y + 0.25f), Vector2.right + Vector2.up, 0.6f, LayerMask.GetMask("Enviroment", "Water"));
        RaycastHit2D SearchEmpty04 = Physics2D.Raycast(new Vector2(P.x, P.y + 0.25f), Vector2.right + Vector2.down, 0.6f, LayerMask.GetMask("Enviroment", "Water"));
        RaycastHit2D SearchEmpty05 = Physics2D.Raycast(new Vector2(P.x, P.y + 0.25f), Vector2.left, 0.6f, LayerMask.GetMask("Enviroment" , "Water"));
        RaycastHit2D SearchEmpty06 = Physics2D.Raycast(new Vector2(P.x, P.y + 0.25f), Vector2.down, 0.6f, LayerMask.GetMask("Enviroment", "Water"));
        RaycastHit2D SearchEmpty07 = Physics2D.Raycast(new Vector2(P.x, P.y + 0.25f), Vector2.right, 0.6f, LayerMask.GetMask("Enviroment", "Water"));
        RaycastHit2D SearchEmpty08 = Physics2D.Raycast(new Vector2(P.x, P.y + 0.25f), Vector2.down, 0.6f, LayerMask.GetMask("Enviroment", "Water"));
        return !SearchEmpty01 && !SearchEmpty02 && !SearchEmpty03 && !SearchEmpty04 && !SearchEmpty05 && !SearchEmpty06 && !SearchEmpty07 && !SearchEmpty08;
    }

    /// <summary>
    /// ������ȷ�������Ƿ��ڵ�ǰ����ķ�Χ��
    /// </summary>
    /// <param name="P"></param>
    /// <returns></returns>
    public bool isThisPointInRoom(Vector3 P)
    {
        if(Mathf.Abs(P.x) <= 12.2f && Mathf.Abs(P.y) <= 7.1f)
        {
            return true;
        }
        else
        {
            return false;
        }
        
    }







    //===================================�����幹��ĵ��ˣ������� �� �����ߵȣ�ʹ�õĺ���===========================================


    public void UpdataMulitBodyEmptyState()
    {


        bool isOnixSubsititue = false;
        bool isOnixInGrassyTerrain = false;
        bool isOnixInPsychicTerrain = false;
        bool isOnixInElectricTerrain = false;
        bool isOnixInMistyTerrain = false;
        bool isOnixSuperInGrassyTerrain = false;
        bool isOnixSuperInPsychicTerrain = false;
        bool isOnixSuperInElectricTerrain = false;
        bool isOnixSuperInMistyTerrain = false;
        bool isOnixSpeedChange = false;


        foreach (SubEmptyBody b in SubEmptyBodyList)
        {
            if (b.isSubsititue) { isOnixSubsititue = true; SubsititueTarget = b.SubsititueTarget; }
            if (b.isInGrassyTerrain) { isOnixInGrassyTerrain = true; }
            if (b.isInPsychicTerrain) { isOnixInPsychicTerrain = true; }
            if (b.isInElectricTerrain) { isOnixInElectricTerrain = true; }
            if (b.isInMistyTerrain) { isOnixInMistyTerrain = true; }
            if (b.isInSuperGrassyTerrain) { isOnixSuperInGrassyTerrain = true; }
            if (b.isInSuperPsychicTerrain) { isOnixSuperInPsychicTerrain = true; }
            if (b.isInSuperElectricTerrain) { isOnixSuperInElectricTerrain = true; }
            if (b.isInSuperMistyTerrain) { isOnixSuperInMistyTerrain = true; }
            if (b.isSpeedChange) { isOnixSpeedChange = true; }
        }

        if (isOnixSubsititue) { isSubsititue = true; } else { isSubsititue = false; SubsititueTarget = null; }
        if (isOnixInGrassyTerrain) { isInGrassyTerrain = true; } else { isInGrassyTerrain = false; }
        if (isOnixInPsychicTerrain) { isInPsychicTerrain = true; } else { isInPsychicTerrain = false; }
        if (isOnixInElectricTerrain) { isInElectricTerrain = true; } else { isInElectricTerrain = false; }
        if (isOnixInMistyTerrain) { isInMistyTerrain = true; } else { isInMistyTerrain = false; }
        if (isOnixSuperInGrassyTerrain) { isInSuperGrassyTerrain = true; } else { isInSuperGrassyTerrain = false; }
        if (isOnixSuperInPsychicTerrain) { isInSuperPsychicTerrain = true; } else { isInSuperPsychicTerrain = false; }
        if (isOnixSuperInElectricTerrain) { isInSuperElectricTerrain = true; } else { isInSuperElectricTerrain = false; }
        if (isOnixSuperInMistyTerrain) { isInSuperMistyTerrain = true; } else { isInSuperMistyTerrain = false; }
        if (isOnixSpeedChange) { SpeedChange(); } else { SpeedRemove01(0); }

    }




    /// <summary>
    /// �����������Ϊ���޵�״̬
    /// </summary>
    public void ResetSubBodyInvincible()
    {
        if (SubEmptyBodyList.Count != 0)
        {
            for (int i = 0; i < SubEmptyBodyList.Count; i++)
            {
                SubEmptyBodyList[i].isSubBodyEmptyInvincible = false;
                this.Invincible = false;
            }
        }
    }

    IEnumerator SetInvincible(float interval)
    {
        while (true)
        {
            yield return new WaitForSeconds(interval);
            // �����¼�
            isSubBodyEmptyInvincible = false;
            ResetSubBodyInvincible();
        }
    }

    //===================================�����幹��ĵ��ˣ������� �� �����ߵȣ�ʹ�õĺ���===========================================













    //=================��ͷҡ��======================

    void ShakeUpdate()
    {
        if (isShake)
        {
            ShakeTime -= Time.deltaTime;
            CSTimer += Time.deltaTime;
            if (CSTimer >= 0.05f) { CSTimer = 0; ShakePower = -ShakePower; }
            if (isHorizontalShake)
            {
                ParentPokemonRoom.transform.GetChild(0).position += Time.deltaTime * ShakePower * Vector3.right;
                ParentPokemonRoom.transform.GetChild(1).position += Time.deltaTime * ShakePower * Vector3.right;
                ParentPokemonRoom.transform.GetChild(2).position += Time.deltaTime * ShakePower * Vector3.right;
                ParentPokemonRoom.transform.GetChild(6).position += Time.deltaTime * ShakePower * Vector3.right;
            }
            else
            {
                ParentPokemonRoom.transform.GetChild(0).position += Time.deltaTime * ShakePower * Vector3.up;
                ParentPokemonRoom.transform.GetChild(1).position += Time.deltaTime * ShakePower * Vector3.up;
                ParentPokemonRoom.transform.GetChild(2).position += Time.deltaTime * ShakePower * Vector3.up;
                ParentPokemonRoom.transform.GetChild(6).position += Time.deltaTime * ShakePower * Vector3.up;
            }
            if (ShakeTime <= 0) { isShake = false; ShakeTime = 0; isHorizontalShake = false; ShakePower = 0; CSTimer = 0; 
                ParentPokemonRoom.transform.GetChild(0).localPosition = Vector3.zero;
                ParentPokemonRoom.transform.GetChild(1).localPosition = Vector3.zero;
                ParentPokemonRoom.transform.GetChild(2).localPosition = Vector3.zero;
                ParentPokemonRoom.transform.GetChild(6).localPosition = Vector3.zero;
            }
        }
        else if(ParentPokemonRoom.transform.GetChild(0).localPosition != Vector3.zero)
        {
            ParentPokemonRoom.transform.GetChild(0).localPosition = Vector3.zero;
            ParentPokemonRoom.transform.GetChild(1).localPosition = Vector3.zero;
            ParentPokemonRoom.transform.GetChild(2).localPosition = Vector3.zero;
            ParentPokemonRoom.transform.GetChild(6).localPosition = Vector3.zero;
        }
    }


    bool isShake;
    float ShakeTime;
    float CSTimer;
    bool isHorizontalShake;
    float ShakePower;
    Vector3 PRoomNowPosition;

    public void CameraShake(float time, float Power, bool isHorizontal)
    {
        if (!isShake) { isShake = true; ShakeTime = time; isHorizontalShake = isHorizontal; ShakePower = Power;  }
    }

    //=================��ͷҡ��======================






















    //=================�й�ʹ�û������������蹥�����ĵ���======================
    /// <summary>
    /// ��ʾ�����Ƿ���һֻ��ʹ�á��������������ͷ��������������ĵ���
    /// </summary>
    public bool isUseEchoedVoice;
    /// <summary>
    /// ʹ�û���
    /// </summary>
    public virtual void UseEchoedVoice( int echoedVoiceLevel )
    {

    }

    /// <summary>
    /// �ж���ǰ�����Ƿ���Ի���
    /// </summary>
    public virtual bool isEchoedVoiceisReady()
    {
        return false;
    }
    //=================�й�ʹ�û������������蹥�����ĵ���======================





















    //=================�йز�Ӱ����======================

    /// <summary>
    /// ���˵Ĳ�Ӱʵ��
    /// </summary>
    public NormalEmptyShadow emptyShadow;
    /// <summary>
    /// ���ɲ�Ӱ�õ�Э��
    /// </summary>
    protected Coroutine ShadowCoroutine;
    /// <summary>
    /// �Ƿ����ɲ�Ӱ
    /// </summary>
    protected bool isShadowMove = false;


    /// <summary>
    /// ��ʼ��ӰЯ��
    /// </summary>
    /// <param name="Interval">���ɲ�Ӱ�ļ��</param>
    /// <param name="disappearingSpeed">��Ӱ����ʧ�ٶ�</param>
    /// <param name="color">��Ӱ����ɫ</param>
    protected void StartShadowCoroutine(float Interval, float disappearingSpeed, Color color)
    {
        //Debug.Log("StartSHadow");
        isShadowMove = true; // ��ʼ���
        ShadowCoroutine = StartCoroutine(StartShadow(Interval , disappearingSpeed , color)); // ����Э��
    }


    /// <summary>
    /// ֹͣ��ӰЭ��
    /// </summary>
    protected void StopShadowCoroutine()
    {
        //Debug.Log("StopSHadow");
        isShadowMove = false; // ����ֹͣ���
        if (ShadowCoroutine != null)
        {
            StopCoroutine(ShadowCoroutine); // ͨ������ֹͣЭ��
            ShadowCoroutine = null; // ��������Ա����ظ�����
        }
    }


    /// <summary>
    /// ÿ��һ��ʱ������һ����Ӱ
    /// </summary>
    /// <param name="Interval">���ɲ�Ӱ�ļ��</param>
    /// <param name="disappearingSpeed">��Ӱ����ʧ�ٶ�</param>
    /// <param name="color">��Ӱ����ɫ</param>
    /// <returns></returns>
    IEnumerator StartShadow( float Interval , float disappearingSpeed, Color color)
    {
        while (isShadowMove)
        {
            InstantiateShadow(disappearingSpeed , color);
            yield return new WaitForSeconds(Interval); // �ȴ����ʱ��
        }
    }

    /// <summary>
    /// ����һ����Ӱ
    /// </summary>
    /// <param name="disappearingSpeed">��Ӱ����ʧ�ٶ�</param>
    /// <param name="color">��Ӱ����ɫ</param>
    void InstantiateShadow(float disappearingSpeed , Color color)
    {
        if (!isDie && !isBorn && !isSleepDone && !isEmptyFrozenDone && !isSilence && !isCanNotMoveWhenParalysis && !isFearDone)
        {
            Instantiate(emptyShadow, transform.position, Quaternion.identity).SetNormalEmptyShadow(disappearingSpeed, GetSkinRenderers(), color);
        }  
    }

    //=================�йز�Ӱ����======================



    //���ɼ��ܵı�����Ч
    public void GetCTEffect(Transform target)
    {
        //��ȡ����������Ч
        GameObject CTEffect = PublicEffect.StaticPublicEffectList.ReturnAPublicEffect(0);
        //ʵ����
        Instantiate(CTEffect, target.transform.position + Vector3.right * Random.Range(-0.5f, 0.5f) + Vector3.up * Random.Range(0.0f, 0.8f), Quaternion.identity, target.transform).SetActive(true);
    }




    /// <summary>
    /// �ռ������Ӽ��͸����� Collider2D�������ظ�
    /// </summary>
    public void CollectAllColliders(Empty current, HashSet<Empty> visited, List<Collider2D> output)
    {
        if (current == null || visited.Contains(current)) return;

        visited.Add(current);

        Collider2D col = current.GetComponent<Collider2D>();
        if (col != null && !output.Contains(col))
        {
            output.Add(col);
        }

        // �����Ӽ�
        if (current.ChildrenList != null)
        {
            foreach (Empty child in current.ChildrenList)
            {
                CollectAllColliders(child, visited, output);
            }
        }

        // ��������
        if (current.ParentEmptyByChild != null)
        {
            CollectAllColliders(current.ParentEmptyByChild, visited, output);
        }
    }






























    //�����������������������������������������й��ӵ��˶����������������������������������������������

    //=================================��Ϊ������====================================

    /// <summary>
    /// �ӵ��˶���ļ�Transform
    /// </summary>
    public Transform ChildHome;

    /// <summary>
    /// �ӵ��˶����б�
    /// </summary>
    public List<Empty> ChildrenList;

    /// <summary>
    /// �ӵ��˶�����ȥ
    /// </summary>
    public virtual void ChildDie(Empty child)
    {
        for (int i = 0; i < ChildrenList.Count; i++)
        {
            if (ChildrenList[i] != null && child.gameObject == ChildrenList[i].gameObject) { 
                ChildrenList[i].ChildLeaveHome();
                //ChildrenList[i] = null;  
            }
        }
    }

    /// <summary>
    /// ���������Լ����ӵ��˶���������֮�����ײ
    /// </summary>
    public virtual void IgnoreCollisionParentChild()
    {
        List<Collider2D> CList = new List<Collider2D>();
        HashSet<Empty> visited = new HashSet<Empty>();
        CollectAllColliders(this, visited, CList);
        Debug.Log(string.Join(",", CList));
        //CList.Add(this.GetComponent<Collider2D>());
        foreach (Empty child in ChildrenList)
        {
            if (child.gameObject != null) { CList.Add(child.transform.GetComponent<Collider2D>()); }
        }
        for (int i = 0; i < (CList.Count - 1); i++)
        {
            for (int j = i + 1; j < CList.Count; j++)
            {
                Physics2D.IgnoreCollision(CList[i], CList[j], true);
            }
        }
    }



    /// <summary>
    /// ����ĳ���ӵ��˶���������ӵ��˶����Լ��Լ�����ײ
    /// </summary>
    public virtual void IgnoreOneChildCollision(Empty child)
    {
        //Debug.Log("NAME" + child.name);
        List<Collider2D> CList = new List<Collider2D>();
        HashSet<Empty> visited = new HashSet<Empty>();
        CollectAllColliders(this, visited, CList);
        //Physics2D.IgnoreCollision(GetComponent<Collider2D>(), child.GetComponent<Collider2D>(), true);
        List<string> t = new List<string> { };
        foreach (Collider2D c in CList)
        {
            t.Add(c.name);
            if (c != null && c.gameObject != child.gameObject)
            {
                //Debug.Log(c.name +"+"+ child.name);
                Physics2D.IgnoreCollision(c, child.GetComponent<Collider2D>(), true);
            }
        }
        if (child.ChildrenList.Count != 0) {
            for (int i = 0; i < child.ChildrenList.Count; i++)
            {
                child.IgnoreOneChildCollision(child.ChildrenList[i]);
            }
        }
        //Debug.Log(string.Join("," , t));
    }

    /// <summary>
    /// �ָ�ĳ���ӵ��˶���������ӵ��˶����Լ��Լ�����ײ
    /// </summary>
    public virtual void ResetOneChildCollision(Empty child)
    {
        List<Collider2D> CList = new List<Collider2D>();
        HashSet<Empty> visited = new HashSet<Empty>();
        CollectAllColliders(this, visited, CList);
        Debug.Log(string.Join(",", CList));
        //Physics2D.IgnoreCollision(GetComponent<Collider2D>(), child.GetComponent<Collider2D>(), false);
        foreach (Collider2D c in CList)
        {
            if (c != null && c.gameObject != child.gameObject)
            {
                Physics2D.IgnoreCollision(c, child.GetComponent<Collider2D>(), false);
            }
        }
        if (child.ChildrenList.Count != 0)
        {
            for (int i = 0; i < child.ChildrenList.Count; i++)
            {
                child.ResetOneChildCollision(child.ChildrenList[i]);
            }
        }
    }

    /// <summary>
    /// ���ػ��ŵ��ӵ��˶������ ���Ҹ���ChildrenList
    /// </summary>
    /// <returns></returns>
    public virtual List<Empty> GetAliveChildren()
    {
        List<Empty> output = new List<Empty> { };
        for (int i = 0; i < ChildrenList.Count; i++)
        {
            if (ChildrenList[i] != null && !ChildrenList[i].isDie) { output.Add(ChildrenList[i]); }
        }
        ChildrenList.Clear();
        ChildrenList = output;
        return output;
    }

    /// <summary>
    /// ����ʱ����ӵ��˶���ĸ����� ���ͷ��ӵ���
    /// </summary>
    public virtual void ParentDie()
    {
        GetAliveChildren();
        List<Empty> l = new List<Empty> { };
        for (int i = 0; i < ChildrenList.Count; i++)
        {
            l.Add(ChildrenList[i]);
        }

        for (int i = 0; i < l.Count; i++)
        {
            if (l[i] != null && !l[i].isDie)
            {
                if (l[i].transform.parent == ChildHome)
                {
                    //Debug.Log(ParentEmptyByChild);
                    l[i].ChildLeaveHome();
                    l[i].transform.parent = l[i].ParentPokemonRoom.EmptyFile();
                }
                if (l[i].ParentEmptyByChild != null) { l[i].ParentEmptyByChild = null; }
            }
        }
    }





    //=================================��Ϊ�Ӷ���====================================

    /// <summary>
    /// �Լ���Ϊ�Ӷ���ʱ�ĸ�����
    /// </summary>
    public Empty ParentEmptyByChild;

    /// <summary>
    /// �Ӷ����뿪�� ����ĸ��
    /// </summary>
    public virtual void ChildLeaveHome()
    {
        Debug.Log(ParentEmptyByChild);
        if (ParentEmptyByChild != null && ParentEmptyByChild.ChildrenList.Contains(this))
        {
            ParentEmptyByChild.ChildrenList.Remove(this);
        }
        //�趨������ͼ�
        if (ParentEmptyByChild != null)
        {
            transform.parent = ParentPokemonRoom.EmptyFile();
            ParentEmptyByChild = null;
        }
    }

    /// <summary>
    /// �Ӷ���ص��Ҽ� ����ĸ��
    /// </summary>
    public virtual void ChildBackHome(Empty parent)
    {
        //�趨������ͼ�
        ParentEmptyByChild = parent;
        transform.parent = ParentEmptyByChild.ChildHome;
        if (ParentEmptyByChild != null && !ParentEmptyByChild.ChildrenList.Contains(this))
        {
            ParentEmptyByChild.ChildrenList.Add(this);
        }
    }

    /// <summary>
    /// ���ݾ���Ѱ�����Լ�����ĸ�����
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public virtual T SearchParentByDistence<T>() where T : Empty
    {
        List<T> OutputList = new List<T> { };
        List<T> TList = _mTool.GetAllFromTransform<T>(ParentPokemonRoom.EmptyFile());
        //���������Ϊ��
        if (ParentEmptyByChild == null)
        {
            for (int i = 0; i < TList.Count; i++)
            {
                T t = TList[i];
                if (t.transform != this.transform)
                {
                    T ct = t.GetComponent<T>();
                    if (ct != null && ct.isActiveAndEnabled && !ct.isBorn && !ct.isDie)
                    {
                        OutputList.Add(ct);
                    }
                }
            }
        }


        if (OutputList.Count == 0)
        {
            return default;
        }
        else
        {
            float MinDistence = 100.0f;
            T output = null;
            for (int i = 0; i < OutputList.Count; i++)
            {
                if (Vector2.Distance((Vector2)(OutputList[i].transform.position), (Vector2)(transform.position)) < MinDistence)
                {
                    MinDistence = Vector2.Distance((Vector2)(OutputList[i].transform.position), (Vector2)(transform.position));
                    output = OutputList[i];
                }
            }
            return output;
        }
    }



    //�����������������������������������������й��Ӷ����������������������������������������������





















    //�����������������������������������������й���Я����������������������������������������������


    //���
    public Empty PartnerEmpty
    {
        get { return partnerEmpty; }
        set { partnerEmpty = value; }
    }
    public Empty partnerEmpty ;




    //�Լ��ڻ���ϵ�еĵ�λ
    public enum PositionInPartnershipEnum
    {
        BigBrother, //���
        LittleBoy,  //С��
        NoPartner,  //û�л�飬�򲻴��ڻ��״̬
    }
    public PositionInPartnershipEnum PositionInPartnership
    {
        get { return positionInPartnership; }
        set { positionInPartnership = value; }
    }
    public PositionInPartnershipEnum positionInPartnership = PositionInPartnershipEnum.NoPartner;



    /// <summary>
    /// �ڵ�ǰ�����ڸ��ݾ����������
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public T SearchPartnerInRoomByDistence<T>() where T : Empty
    {
        //Debug.Log(this.name);
        List<T> OutputList = new List<T> { };
        foreach (Transform t in transform.parent)
        {
            if (t != this.transform) {
                T ct = t.GetComponent<T>();
                if (ct != null && ct.isActiveAndEnabled && !ct.isBorn && !ct.isDie && !ct.isSleepDone && !ct.isFearDone && !ct.isEmptyFrozenDone)
                {
                    //Debug.Log(ct.name + "+" + ct.isActiveAndEnabled + "+" + (ct != null && ct.isActiveAndEnabled && !ct.isBorn && !ct.isDie));
                    OutputList.Add(ct);
                }
            }
        }

        if (OutputList.Count == 0)
        {
            return default;
        }
        else
        {
            float MinDistence = 100.0f;
            T output = null;
            for (int i = 0; i < OutputList.Count; i++)
            {
                if (Vector2.Distance((Vector2)(OutputList[i].transform.position), (Vector2)(transform.position)) < MinDistence)
                {
                    MinDistence = Vector2.Distance((Vector2)(OutputList[i].transform.position), (Vector2)(transform.position));
                    output = OutputList[i];
                }
            }
            partnerEmpty = output;
            JudgePositionInPartnership();
            return output;
        }
    }



    /// <summary>
    /// ͨ���Ƚ�ȷ������ϵ�еĵ�λ
    /// </summary>
    public void JudgePositionInPartnership()
    {
        //û�л��������޻��״̬
        if (partnerEmpty == null) { 
            this.positionInPartnership = PositionInPartnershipEnum.NoPartner;
            return;
        }
        else
        {
            if (this.GetInstanceID() > partnerEmpty.GetInstanceID())
            {
                this.positionInPartnership = PositionInPartnershipEnum.BigBrother;
                partnerEmpty.positionInPartnership = PositionInPartnershipEnum.LittleBoy;
            }
            else
            {
                this.positionInPartnership = PositionInPartnershipEnum.BigBrother;
                partnerEmpty.positionInPartnership = PositionInPartnershipEnum.LittleBoy;
            }
        }
    }




    //�Լ���Ϊ�������ʱ
    public void DieAsPartner()
    {
        RemovePartnership();
    }


    /// <summary>
    /// �������ϵ
    /// </summary>
    public void RemovePartnership()
    {
        if (partnerEmpty != null) {
            partnerEmpty.positionInPartnership = PositionInPartnershipEnum.NoPartner;
            partnerEmpty.partnerEmpty = null;
        }

    }




    /**List
         //���
    public List<Empty> PartnerEmpty
    {
        get { return partnerEmpty; }
        set { partnerEmpty = value; }
    }
    List<Empty> partnerEmpty = new List<Empty> { };




    //�Լ��ڻ���ϵ�еĵ�λ
    public enum PositionInPartnershipEnum
    {
        BigBrother, //���
        LittleBoy,  //С��
        NoPartner,  //û�л�飬�򲻴��ڻ��״̬
    }
    public PositionInPartnershipEnum PositionInPartnership
    {
        get { return positionInPartnership; }
        set { positionInPartnership = value; }
    }
    PositionInPartnershipEnum positionInPartnership = PositionInPartnershipEnum.LittleBoy;


    /// <summary>
    /// �ڵ�ǰ�����ڸ����������
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public PositionInPartnershipEnum SearchPartnerInRoom<T>() where T : Empty
    {
        foreach (Transform t in transform.parent)
        {
            if (t != this.transform) {
                T ct = t.GetComponent<T>();
                Debug.Log(ct.name + "+" + ct.isActiveAndEnabled + "+" + (ct != null && ct.isActiveAndEnabled && !ct.isBorn && !ct.isDie));
                if (ct != null && ct.isActiveAndEnabled && !ct.isBorn && !ct.isDie)
                {
                    partnerEmpty.Add(ct);
                }
            }
        }
        JudgePositionInPartnership();
        Debug.Log(this.positionInPartnership);
        return this.positionInPartnership;
    }


    /// <summary>
    /// ͨ���Ƚ�ȷ������ϵ�еĵ�λ
    /// </summary>
    public void JudgePositionInPartnership()
    {
        //��ʼ����λ
        ResetPositionInPartnership();
        //�ж���λ
        Empty big = this;
        int MaxId = this.GetInstanceID();
        //û�л��������޻��״̬
        if (partnerEmpty.Count == 0) { 
            this.positionInPartnership = PositionInPartnershipEnum.NoPartner;
            return;
        }
        for (int i = 0; i < partnerEmpty.Count; i++)
        {
            if (MaxId < partnerEmpty[i].GetInstanceID())
            {
                big = partnerEmpty[i];
                MaxId = partnerEmpty[i].GetInstanceID();
            }
        }
        big.positionInPartnership = PositionInPartnershipEnum.BigBrother;
    }



    /// <summary>
    /// ��ʼ����λ
    /// </summary>
    public void ResetPositionInPartnership()
    {
        this.positionInPartnership = PositionInPartnershipEnum.LittleBoy;
        for (int i = 0; i < partnerEmpty.Count; i++)
        {
            partnerEmpty[i].positionInPartnership = PositionInPartnershipEnum.LittleBoy;
        }
    }


    //�Լ���Ϊ�������ʱ
    public void DieAsPartner()
    {
        RemovePartnership();
    }


    /// <summary>
    /// �������ϵ
    /// </summary>
    public void RemovePartnership()
    {
        for (int i = 0; i < partnerEmpty.Count; i++)
        {
            //�����е��˻���б��еĻ���б����Ƴ��Լ�
            partnerEmpty[i].partnerEmpty.Remove(this);
            //���е������õ�λ
            partnerEmpty[i].ResetPositionInPartnership();
        }
        partnerEmpty.Clear();
    }
    **/


    //�����������������������������������������й���Я����������������������������������������������

}
