using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnumMultiAttribute : PropertyAttribute { }
public class Empty : Pokemon
{


    //���������ʱ��ɵĻ���ֵ����4��������һ������������ɵ��˺���һ���������ֵ��һ����ʾ�ƶ��ľ���,һ����ʾ�ƶ��ٶȣ�һ����ʾ��ʼѪ��
    public float Knock = 5f;
    //���˵ĵ�ǰѪ�������Ѫ��
    [Tooltip("��ǰHP")]
    public int EmptyHp;
    public int maxHP;
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
    public Type.TypeEnum EmptyType01;
    //public int EmptyType01;
    public Type.TypeEnum EmptyType02;

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
    /// �����Ƿ���boss
    /// </summary>
    public bool isBoos;


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

    public bool isHailDef;
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
        get { return isDeadrattle; }
        set { isDeadrattle = value; }
    }
    bool isDeadrattle;


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
        if (transform.parent.parent.GetComponent<Room>() != null) { ParentPokemonRoom = transform.parent.parent.GetComponent<Room>(); }
        FirstSpeed = speed;
        int OutPut;
        if (!isBoos)
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
        if (!isBoos)
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
        if (FloorNum.GlobalFloorNum != null)
        {
            BossBonus = FloorNum.GlobalFloorNum.FloorBossHPBonus[FloorNum.GlobalFloorNum.FloorNumber];
        }
        EmptyHp = (int)((level + 10 + (int)(((float)level * HpEmptyPoint * 2) / 100.0f))*(isBoos? BossBonus : 1));
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
            if (isInvincible)
            {
                return;
            }

            Type.TypeEnum enumVaue = (Type.TypeEnum)SkillType;

            Debug.Log(name);
            Debug.Log(player);
            Debug.Log(player.playerData);
            Debug.Log(player.playerData.IsPassiveGetList[118]);

            Dmage = Dmage * (player.playerData.IsPassiveGetList[118] ? 1 : (((Weather.GlobalWeather.isRain && enumVaue == Type.TypeEnum.Water) ? (Weather.GlobalWeather.isRainPlus ? 1.8f : 1.3f) : 1)
                * ((Weather.GlobalWeather.isRain && enumVaue == Type.TypeEnum.Fire) ? 0.5f : 1)
                * ((Weather.GlobalWeather.isSunny && enumVaue == Type.TypeEnum.Water) ? 0.5f : 1)
                * ((Weather.GlobalWeather.isSunny && enumVaue == Type.TypeEnum.Fire) ? (Weather.GlobalWeather.isSunnyPlus ? 1.8f : 1.3f) : 1)));
            SpDmage = SpDmage * (player.playerData.IsPassiveGetList[118] ? 1 : (((Weather.GlobalWeather.isRain && enumVaue == Type.TypeEnum.Water) ? (Weather.GlobalWeather.isRainPlus ? 1.8f : 1.3f) : 1)
                * ((Weather.GlobalWeather.isRain && enumVaue == Type.TypeEnum.Fire) ? 0.5f : 1)
                * ((Weather.GlobalWeather.isSunny && enumVaue == Type.TypeEnum.Water) ? 0.5f : 1)
                * ((Weather.GlobalWeather.isSunny && enumVaue == Type.TypeEnum.Fire) ? (Weather.GlobalWeather.isSunnyPlus ? 1.8f : 1.3f) : 1)));

            float typeDef = (TypeDef[SkillType] < 0 ? (Mathf.Pow(1.2f, -TypeDef[SkillType])) : 1) * (TypeDef[SkillType] > 0 ? (Mathf.Pow(0.8f, TypeDef[SkillType])) : 1);
            if (Dmage + SpDmage >= 0)
            {
                int allDmg = 0;
                if (SkillType != 19)
                {
                    if (!isInPsychicTerrain) 
                    { 
                        Debug.Log(IsBeFalseSwipe);
                        allDmg = Mathf.Clamp((int)((Dmage + SpDmage) * typeDef * (Type.TYPE[SkillType][(int)EmptyType01]) * Type.TYPE[SkillType][(int)EmptyType02]), 1, 100000);
                        EmptyHp = Mathf.Clamp(EmptyHp - Mathf.Clamp((int)((Dmage + SpDmage) * typeDef * (Type.TYPE[SkillType][(int)EmptyType01]) * Type.TYPE[SkillType][(int)EmptyType02]), 1, 100000), (IsBeFalseSwipe ? 1 : 0), maxHP); 
                    }
                    else
                    {
                        if(Mathf.Abs((int)Mathf.Clamp((int)((Dmage + SpDmage) * typeDef * (Type.TYPE[SkillType][(int)EmptyType01]) * Type.TYPE[SkillType][(int)EmptyType02]), 1, (isBoos ? (maxHP / 6) : 100000))) > (int)(maxHP / 16))
                        {
                            allDmg = Mathf.Clamp((int)((Dmage + SpDmage) * typeDef * (Type.TYPE[SkillType][(int)EmptyType01]) * Type.TYPE[SkillType][(int)EmptyType02]), 1, (isBoos ? (maxHP / 6) : 100000));
                            EmptyHp = Mathf.Clamp(EmptyHp - Mathf.Clamp((int)((Dmage + SpDmage) * typeDef * (Type.TYPE[SkillType][(int)EmptyType01]) * Type.TYPE[SkillType][(int)EmptyType02]), 1, (isBoos ? (maxHP / 6) : 100000)), (IsBeFalseSwipe ? 1 : 0), maxHP);
                        }
                    }
                }
                else
                {
                    if (!isInPsychicTerrain) 
                    {
                        allDmg = Mathf.Clamp((int)((Dmage + SpDmage) * typeDef), 1, (isBoos ? (maxHP / 6) : 100000));
                        EmptyHp = Mathf.Clamp(EmptyHp - Mathf.Clamp((int)((Dmage + SpDmage) * typeDef), 1, (isBoos ? (maxHP / 6) : 100000)), (IsBeFalseSwipe ? 1 : 0), maxHP); 
                    }
                    else
                    {
                        if (Mathf.Abs((int)Mathf.Clamp((int)((Dmage + SpDmage) * typeDef), 1, (isBoos ? (maxHP / 6) : 100000))) > (int)(maxHP / 16))
                        {
                            allDmg = Mathf.Clamp((int)((Dmage + SpDmage) * typeDef), 1, (isBoos ? (maxHP / 6) : 100000));
                            EmptyHp = Mathf.Clamp(EmptyHp - Mathf.Clamp((int)((Dmage + SpDmage) * typeDef), 1, (isBoos ? (maxHP / 6) : 100000)), (IsBeFalseSwipe ? 1 : 0), maxHP);
                        }
                    }
                }
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
                + "Type.TYPE[SkillType][(int)EmptyType01]=" + Type.TYPE[SkillType][(int)EmptyType01] + "  "
                + "Type.TYPE[SkillType][(int)EmptyType02]=" + Type.TYPE[SkillType][(int)EmptyType02] + "  ");
            if ((int)Dmage + (int)SpDmage > 0)
            {
                if (!isCanHitAnimation && animator != null) { animator.SetTrigger("Hit"); }
                //Debug.Log((float)EmptyHp / (float)maxHP + "=" + (float)EmptyHp + "/" + (float)maxHP);
                uIHealth.Per = (float)EmptyHp / (float)maxHP;
                uIHealth.ChangeHpDown();
            }
            else
            {
                uIHealth.Per = (float)EmptyHp / (float)maxHP;
                uIHealth.ChangeHpUp();
            }

            if (IsBeFalseSwipe)
            {
                IsBeFalseSwipe = false;
            }
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
        isHit = true;
        KOPoint = KnockOutPoint;
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
    //========================���˱��������=========================








    //========================������ײ�˺�========================

    /// <summary>
    /// ---OnColliderEnter2D�е���---�����������ɴ����˺�
    /// </summary>
    /// <param name="player"></param>
    public void EmptyTouchHit(GameObject playerObj)
    {
        Debug.Log(playerObj.layer);
        if (playerObj.layer != 23) {
            //���������������ң�ʹ��ҿ۳�һ��Ѫ��
            PlayerControler playerControler = playerObj.gameObject.GetComponent<PlayerControler>();
            PokemonHpChange(this.gameObject, playerObj.gameObject, 10, 0, 0, Type.TypeEnum.No);
            if (playerControler != null)
            {
                //playerControler.ChangeHp(-(10 * AtkAbilityPoint * (2 * Emptylevel + 10) / 250 ) ,0, 0);
                playerControler.KnockOutPoint = Knock;
                playerControler.KnockOutDirection = (playerControler.transform.position - transform.position).normalized;
                if (playerControler.playerData.IsPassiveGetList[115])
                {
                    PokemonHpChange(playerControler.gameObject, this.gameObject, 10, 0, 0, Type.TypeEnum.No);
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
                PokemonHpChange(this.gameObject, e.gameObject, 10, 0, 0, Type.TypeEnum.No);
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
            isDeadrattle = (!isEmptyFrozenDone) && (!isFearDone) && (!isBlindDone);
            FrozenRemove();
            Destroy(rigidbody2D);
            RemoveChild();
            if (!isDie)
            {
                if (GetComponent<Collider2D>()) { GetComponent<Collider2D>().enabled = false; }
                player.ChangeEx((int)(Exp * (isBoos ? 1.8f : 1.3f)));
                player.ChangeHPW(HWP);
                if (player.playerData.IsPassiveGetList[134] && (EmptyType01 == Type.TypeEnum.Dark || EmptyType02 == Type.TypeEnum.Dark) ) { player.ChangeHPW(HWP); }
                transform.parent.parent.GetComponent<Room>().isClear -= 1;
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
            }
            isDie = true;
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
    
    public void EmptyDrop()
    {
        if (IsHaveDropItem) {
            if (isBoos)
            {
                Vector2 DropPosition = new Vector2(Mathf.Clamp(transform.position.x, transform.parent.position.x - 12.0f, transform.parent.position.x + 12.0f), Mathf.Clamp(transform.position.y, transform.parent.position.y - 7.0f, transform.parent.position.y + 7.0f));
                if (!player.playerData.IsPassiveGetList[134]) {
                    Instantiate(DropItem, DropPosition, Quaternion.identity, transform.parent).GetComponent<RandomSkillItem>().isLunch = true;
                    Instantiate(DropItem, DropPosition, Quaternion.identity, transform.parent).GetComponent<RandomSkillItem>().isLunch = true;
                    Instantiate(DropItem, DropPosition, Quaternion.identity, transform.parent).GetComponent<RandomSkillItem>().isLunch = true;
                }
            }
            else
            {

                Vector2 DropPosition = new Vector2(Mathf.Clamp(transform.position.x, transform.parent.position.x - 12.0f, transform.parent.position.x + 12.0f), Mathf.Clamp(transform.position.y, transform.parent.position.y - 7.0f, transform.parent.position.y + 7.0f));
                if (!player.playerData.IsPassiveGetList[134]) {
                    Instantiate(DropItem, DropPosition, Quaternion.identity, transform.parent);
                }

            }
            IsHaveDropItem = false;
            playerUIState.StateDestory(13);
            DropItem = null;
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
        //�����������������1��Ҳ���Ƿ������г��õ�������ĵ��ˣ�ʱ����������
        if (transform.parent.childCount >= 1)
        {
            float D = 1000;
            //������ǰ���������е���
            foreach (Transform e in transform.parent)
            {
                //���e�����Լ�,������룬���С�ڵ�ǰ�������e
                if (e.gameObject != this.gameObject)
                {                
                    if ((transform.position - e.transform.position).magnitude < D)     
                    {
                        OutPutEmpty = e.GetComponent<Empty>();   
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
            PokemonHpChange(null, this.gameObject, Mathf.Clamp((((float)maxHP) / 16) * ToxicResistance, 1, isBoos ? 8 : 10), 0, 0, Type.TypeEnum.IgnoreType);
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
            PokemonHpChange(null , this.gameObject , Mathf.Clamp((((float)maxHP) / 16) * BurnResistance, 1, isBoos ? 8 : 10), 0 , 0 , Type.TypeEnum.IgnoreType);
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
        if (EmptyType01 != Type.TypeEnum.Ice && EmptyType02 != Type.TypeEnum.Ice && !isHailDef)
        {
            EmptyHailTimer += Time.deltaTime;
            if (EmptyHailTimer >= 2)
            {
                EmptyHailTimer += Time.deltaTime;
                if (Weather.GlobalWeather.isHailPlus)
                {
                    PokemonHpChange(null, this.gameObject, Mathf.Clamp((((float)maxHP) / 16) * OtherStateResistance, 1, isBoos ? 8 : 10), 0, 0, Type.TypeEnum.IgnoreType);
                    EmptyHailTimer = 0;
                }
                else
                {
                    PokemonHpChange(null, this.gameObject, Mathf.Clamp((((float)maxHP) / 16) * OtherStateResistance, 1, isBoos ? 8 : 10), 0, 0, Type.TypeEnum.IgnoreType);
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
        if (EmptyType01 != Type.TypeEnum.Ground && EmptyType01 != Type.TypeEnum.Rock && EmptyType01 != Type.TypeEnum.Steel && EmptyType02 != Type.TypeEnum.Ground && EmptyType02 != Type.TypeEnum.Rock && EmptyType02 != Type.TypeEnum.Steel && !isSandStormDef)
        {
            EmptySandStormTimer += Time.deltaTime;
            if (EmptySandStormTimer >= 2)
            {
                EmptySandStormTimer += Time.deltaTime;
                if (Weather.GlobalWeather.isSandstormPlus)
                {
                    PokemonHpChange(null, this.gameObject, Mathf.Clamp((((float)maxHP) / 8) * OtherStateResistance, 1, isBoos ? 16 : 20), 0, 0, Type.TypeEnum.IgnoreType);
                    EmptySandStormTimer = 0;
                }
                else
                {
                    PokemonHpChange(null, this.gameObject, Mathf.Clamp((((float)maxHP) / 16) * OtherStateResistance, 1, isBoos ? 8 : 10), 0, 0, Type.TypeEnum.IgnoreType);
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
            PokemonHpChange(null, this.gameObject, Mathf.Clamp(( (isBoos? ((float)EmptyHp) : ((float)maxHP)) / 4), 1, 10000), 0, 0, Type.TypeEnum.IgnoreType);
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
            PokemonHpChange(null, this.gameObject, 0, 0, (int)Mathf.Clamp(( (float)maxHP / 16), 1, 10), Type.TypeEnum.IgnoreType);
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

    //===================================�����幹��ĵ��ˣ������� �� �����ߵȣ�ʹ�õĺ���===========================================















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

    //=================��ͷҡ��======================
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



}
