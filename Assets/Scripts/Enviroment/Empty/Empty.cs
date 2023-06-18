using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Empty : Pokemon
{
    //���������ʱ��ɵĻ���ֵ����4��������һ������������ɵ��˺���һ���������ֵ��һ����ʾ�ƶ��ľ���,һ����ʾ�ƶ��ٶȣ�һ����ʾ��ʼѪ��
    public float Knock;
    //���˵ĵ�ǰѪ�������Ѫ��
    public int EmptyHp;
    protected int maxHP;
    //���˵ĵȼ�
    public int Emptylevel;
    //�����Ƿ������ڳ��������У�������ڳ��������ж�
    public bool isBorn;
    //���˵�Ѫ��ui
    public EmptyHpBar uIHealth;

    //�����������α�������ʾ���˵���������
    public int EmptyType01;
    public int EmptyType02;

    //���������������ݣ���ʾ��ɫ����������ֵ,�Լ����ǰ����ֵ
    public int HpEmptyPoint;
    public int AtkEmptyPoint;
    public int SpAEmptyPoint;
    public int DefEmptyPoint;
    public int SpdEmptyPoint;
    public int SpeedEmptyPoint;

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

    //����2������ֵ����ʾĿ������Ƿ�����,��ʾĿ���Ƿ񱻹���,һ�����˼�ʱ����һ��������ֵ
    public bool isDie = false;
    public bool isHit = false;
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

    //��ʾ�����Ƿ��ڱ�������ͨ������
    public bool isMistPlus;






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
        float typeDef = (TypeDef[SkillType] < 0 ? (Mathf.Pow(1.2f, -TypeDef[SkillType])) : 1) * (TypeDef[SkillType] > 0 ? (Mathf.Pow(0.8f, TypeDef[SkillType])) : 1);
            if (Dmage + SpDmage >= 0)
            {
                if (SkillType != 19)
                {
                    EmptyHp -= (int)((Dmage + SpDmage) * (Type.TYPE[SkillType][EmptyType01]) * (Type.TYPE[SkillType][EmptyType02]) * typeDef);
                }
                else
                {
                    EmptyHp -= (int)((Dmage + SpDmage) * typeDef);
                }
            }
            else
            {
                EmptyHp = Mathf.Clamp(EmptyHp - (int)(Dmage + SpDmage), 0, maxHP);
            }

            Debug.Log(((Dmage + SpDmage) * (Type.TYPE[SkillType][EmptyType01]) * (Type.TYPE[SkillType][EmptyType02])) * typeDef);
            if ((int)Dmage + (int)SpDmage > 0)
            {
                animator.SetTrigger("Hit");
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
        if (playerControler != null)
        {
            playerControler.ChangeHp(-(10 * AtkAbilityPoint * (2 * Emptylevel + 10) / 250 ) ,0, 0);
            playerControler.KnockOutPoint = Knock;
            playerControler.KnockOutDirection = (playerControler.transform.position - transform.position).normalized;
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
            Instantiate(DropItem ,transform.position , Quaternion.identity , transform.parent);
        }
        Destroy(gameObject);
    }

    //=============================�����¼�===========================







    //=========================�ж��¼�========================

    /// <summary>
    /// ���˵��ж���ʱ����ÿ��ʱ������һ�ζ�
    /// </summary>
    protected float EmptyToxicTimer;
    /// <summary>
    /// �����ж�ʱ����˵�Ѫ
    /// </summary>
    public void EmptyToxic()
    {
        EmptyToxicTimer += Time.deltaTime;
        if(EmptyToxicTimer >= 2)
        {
            EmptyToxicTimer += Time.deltaTime;
            EmptyHpChange( Mathf.Clamp((((float)maxHP)/16)*OtherStateResistance ,1,10) , 0, 19);
            EmptyToxicTimer = 0;
        }

    }
    //=========================�ж��¼�========================








    /// <summary>
    /// ---Update��FixedUpdate�е���---������һ������������ҽ�������Ϊ֮ǰ����Ҷ������٣�������Ҫ���»�ȡ���
    /// </summary>
    public void ResetPlayer()
    {
        if (player == null)
        {
            player = GameObject.FindWithTag("Player").GetComponent<PlayerControler>();
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
