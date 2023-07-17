using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnumMultiAttribute : PropertyAttribute { }
public class Empty : Pokemon
{
    //���������ʱ��ɵĻ���ֵ����4��������һ������������ɵ��˺���һ���������ֵ��һ����ʾ�ƶ��ľ���,һ����ʾ�ƶ��ٶȣ�һ����ʾ��ʼѪ��
    public float Knock;
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
    //���������������ݣ���ʾ��ɫ����������ֵ,�Լ����ǰ����ֵ
    public int HpEmptyPoint;
    public int AtkEmptyPoint;
    public int SpAEmptyPoint;
    public int DefEmptyPoint;
    public int SpdEmptyPoint;
    public int SpeedEmptyPoint;

    //����ֵ
    public int AtkAbilityPoint { get { return AtkAbility; } set { AtkAbility = value; } }
    int AtkAbility;
    public int SpAAbilityPoint { get { return SpAAbility; } set { SpAAbility = value; } }
    int SpAAbility;
    public int DefAbilityPoint { get { return DefAbility; } set { DefAbility = value; } }
    int DefAbility;
    public int SpdAbilityPoint { get { return SpDAbility; } set { SpDAbility = value; } }
    int SpDAbility;
    public int SpeedAbilityPoint { get { return SpeedAbility; } set { SpeedAbility = value; } }
    int SpeedAbility;



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





    //=============================��ʼ����������================================

    /// <summary>
    /// ---start�е���---��������ҵȼ���̬�趨���˵ȼ���
    /// </summary>
    /// <param name="PlayerLevel"></param>
    /// <param name="MaxLevel"></param>
    /// <returns></returns>
    protected int SetLevel(int PlayerLevel,int MaxLevel)
    {
        int OutPut;
        if (!isBoos)
        {
            if (PlayerLevel <= 10)
            {
                OutPut = (player.playerData.IsPassiveGetList[29] ? (Random.Range(11, 13)) : (Random.Range(4, 8)));
            }
            else
            {
                OutPut = PlayerLevel - (player.playerData.IsPassiveGetList[29] ? (Random.Range(-5, -2)) : (Random.Range(2, 6)));
                if (OutPut > MaxLevel) { OutPut = MaxLevel; }
            }
        }
        else
        {
            OutPut = Mathf.Clamp(player.Level+ (player.playerData.IsPassiveGetList[29] ? (Random.Range(-5,0)):(Random.Range(-2, 3))), (player.playerData.IsPassiveGetList[29] ? 22 : 15), (player.playerData.IsPassiveGetList[29] ? 60 : 50));
        }
        return OutPut;
    }

    /// <summary>
    /// ---start�е���---����������ֵ��ʼ������Ѫ��
    /// </summary>
    /// <param name="level"></param>
    protected void EmptyHpForLevel(int level)
    {
        EmptyHp = (int)((level + 10 + (int)(((float)level * HpEmptyPoint * 2) / 100.0f))*(isBoos?1.7f:1));
        maxHP = EmptyHp;
    }

    /// <summary>
    /// ---start�е���---����������ֵ��ʼ��������������ֵ
    /// </summary>
    /// <param name="level"></param>
    /// <param name="Ability"></param>
    /// <returns></returns>
    protected int AbilityForLevel(int level, int Ability)
    {
        return (Ability * 2 * level) / 100 + 5;
    }

    //=============================��ʼ����������================================










    //====================����Ѫ���ı�======================
    /// <summary>
    ///     //����һ���������ı���˶����Ѫ��
    /// </summary>
    /// <param name="Dmage">�﹥�˺�</param>
    /// <param name="SpDmage">�ع��˺�</param>
    /// <param name="SkillType">�˺����ԣ����ֲο�Type.cs��</param>
    public void EmptyHpChange(float  Dmage , float SpDmage , int SkillType)
    {
        if (isInvincible)
        {
            return;
        }

        Type.TypeEnum enumVaue = (Type.TypeEnum)SkillType;
        Dmage = Dmage * ((Weather.GlobalWeather.isRain && enumVaue == Type.TypeEnum.Water) ? (Weather.GlobalWeather.isRainPlus ? 1.8f : 1.3f) : 1)
            * ((Weather.GlobalWeather.isRain && enumVaue == Type.TypeEnum.Fire) ? 0.5f : 1)
            * ((Weather.GlobalWeather.isSunny && enumVaue == Type.TypeEnum.Water) ? 0.5f : 1)
            * ((Weather.GlobalWeather.isSunny && enumVaue == Type.TypeEnum.Fire) ? (Weather.GlobalWeather.isSunnyPlus ? 1.8f : 1.3f) : 1);
        SpDmage = SpDmage * ((Weather.GlobalWeather.isRain && enumVaue == Type.TypeEnum.Water) ? (Weather.GlobalWeather.isRainPlus ? 1.8f : 1.3f) : 1)
            * ((Weather.GlobalWeather.isRain && enumVaue == Type.TypeEnum.Fire) ? 0.5f : 1)
            * ((Weather.GlobalWeather.isSunny && enumVaue == Type.TypeEnum.Water) ? 0.5f : 1)
            * ((Weather.GlobalWeather.isSunny && enumVaue == Type.TypeEnum.Fire) ? (Weather.GlobalWeather.isSunnyPlus ? 1.8f : 1.3f) : 1);

        float typeDef = (TypeDef[SkillType] < 0 ? (Mathf.Pow(1.2f, -TypeDef[SkillType])) : 1) * (TypeDef[SkillType] > 0 ? (Mathf.Pow(0.8f, TypeDef[SkillType])) : 1);
            if (Dmage + SpDmage >= 0)
            {
                if (SkillType != 19)
                {
                    EmptyHp -= Mathf.Clamp((int)((Dmage + SpDmage) * typeDef * (Type.TYPE[SkillType][(int)EmptyType01]) * Type.TYPE[SkillType][(int)EmptyType02]), 1, 100000);
                }
                else
                {
                    EmptyHp -= Mathf.Clamp( (int)((Dmage + SpDmage) * typeDef) , 1 , 100000 );
                }
            EmptySleepRemove();

            }
            else
            {
                EmptyHp = Mathf.Clamp(EmptyHp - (int)(Dmage + SpDmage), 0, maxHP);
            }

            Debug.Log(Mathf.Clamp((int)((Dmage + SpDmage) * typeDef * (Type.TYPE[SkillType][(int)EmptyType01]) * Type.TYPE[SkillType][(int)EmptyType02]), 1, 100000));
            if ((int)Dmage + (int)SpDmage > 0)
            {
                animator.SetTrigger("Hit");
                //Debug.Log((float)EmptyHp / (float)maxHP + "=" + (float)EmptyHp + "/" + (float)maxHP);
                uIHealth.Per = (float)EmptyHp / (float)maxHP;
                uIHealth.ChangeHpDown();
            }
            else
            {
                uIHealth.Per = (float)EmptyHp / (float)maxHP;
                uIHealth.ChangeHpUp();
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
            KOTimer += Time.deltaTime;
            Vector2 position = rigidbody2D.position;
            Vector2 KODirection = new Vector2(player.transform.position.x - transform.position.x, player.transform.position.y - transform.position.y);
            KODirection.Normalize();
            position.x = position.x - KOPoint * KODirection.x * Time.deltaTime;
            position.y = position.y - KOPoint * KODirection.y * Time.deltaTime;
            rigidbody2D.position = position;

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
    public void EmptyTouchHit(GameObject player)
    {
        //���������������ң�ʹ��ҿ۳�һ��Ѫ��
        PlayerControler playerControler = player.gameObject.GetComponent<PlayerControler>();
        PokemonHpChange(this.gameObject, player, 10, 0, 0, Type.TypeEnum.No);
        if (playerControler != null)
        {
            //playerControler.ChangeHp(-(10 * AtkAbilityPoint * (2 * Emptylevel + 10) / 250 ) ,0, 0);
            playerControler.KnockOutPoint = Knock;
            playerControler.KnockOutDirection = (playerControler.transform.position - transform.position).normalized;
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
            FrozenRemove();
            Destroy(rigidbody2D);
            RemoveChild();
            if (!isDie)
            {
                player.ChangeEx(Exp);
                player.ChangeHPW(HWP);
                transform.parent.parent.GetComponent<Room>().isClear -= 1;
                if (DestoryEvent != null) { DestoryEvent(); }
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
            if (isBoos)
            {
                Instantiate(DropItem, transform.position, Quaternion.identity, transform.parent).GetComponent<RandomSkillItem>().isLunch = true;
                Instantiate(DropItem, transform.position, Quaternion.identity, transform.parent).GetComponent<RandomSkillItem>().isLunch = true;
                Instantiate(DropItem, transform.position, Quaternion.identity, transform.parent).GetComponent<RandomSkillItem>().isLunch = true;
            }
            else
            {
                Instantiate(DropItem, transform.position, Quaternion.identity, transform.parent);
            }
        }
        Destroy(gameObject);
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
        if (isToxicDone) { EmptyToxic(); }
        if (isBurnDone) { EmptyBurn(); }
        if (isParalysisDone) { EmptyParalysisJudge(); } else { isCanNotMoveWhenParalysis = false; }
        if (Weather.GlobalWeather.isHail) { EmptyHail(); }
        if (Weather.GlobalWeather.isSandstorm) { EmptySandStorm(); }
        if (isEmptyCurseDone) { EmptyCurseDmage(); }
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
        if (EmptyType01 != Type.TypeEnum.Ice && EmptyType02 != Type.TypeEnum.Ice)
        {
            EmptyHailTimer += Time.deltaTime;
            if (EmptyHailTimer >= 2)
            {
                EmptyHailTimer += Time.deltaTime;
                if (Weather.GlobalWeather.isHailPlus)
                {
                    PokemonHpChange(null, this.gameObject, Mathf.Clamp((((float)maxHP) / 16) * OtherStateResistance, 1, isBoos ? 8 : 10), 0, 0, Type.TypeEnum.IgnoreType);
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
        if (EmptyType01 != Type.TypeEnum.Ground && EmptyType01 != Type.TypeEnum.Rock && EmptyType01 != Type.TypeEnum.Steel && EmptyType02 != Type.TypeEnum.Ground && EmptyType02 != Type.TypeEnum.Rock && EmptyType02 != Type.TypeEnum.Steel)
        {
            EmptySandStormTimer += Time.deltaTime;
            if (EmptySandStormTimer >= 2)
            {
                EmptySandStormTimer += Time.deltaTime;
                if (Weather.GlobalWeather.isSandstormPlus)
                {
                    PokemonHpChange(null, this.gameObject, Mathf.Clamp((((float)maxHP) / 8) * OtherStateResistance, 1, isBoos ? 16 : 20), 0, 0, Type.TypeEnum.IgnoreType);
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



}
