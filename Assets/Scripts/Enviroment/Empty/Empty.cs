using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Empty : Pokemon
{
    //����4��������һ������������ɵ��˺���һ���������ֵ��һ����ʾ�ƶ��ľ���,һ����ʾ�ƶ��ٶȣ�һ����ʾ��ʼѪ��
    public float Knock;
    public int EmptyHp;
    protected int maxHP;
    public int Emptylevel;
    public bool isBorn;
    public EmptyHpBar uIHealth;

    
    //���������������ݣ���ʾ��ɫ����������ֵ,�Լ����ǰ����ֵ
    public int HpEmptyPoint;
    public int AtkEmptyPoint;
    public int SpAEmptyPoint;
    public int DefEmptyPoint;
    public int SpdEmptyPoint;
    public int SpeedEmptyPoint;

    //�����������α�������ʾ���˵���������
    public int EmptyType01;
    public int EmptyType02;

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

    //����һ�������ͱ��������޵�ʱ�䣬һ�������ͱ�����Ϊ�޵�ʱ���ʱ����һ�������ͱ����ж��Ƿ��޵�
    public float TimeInvincible;
    float InvincileTimer = 0.0f;
    public bool isInvincible = false;

    public delegate void EmptyEvent();
    public EmptyEvent DestoryEvent;

    //����2������ֵ����ʾĿ������Ƿ�����,��ʾĿ���Ƿ񱻹���,һ�����˼�ʱ����һ��������ֵ
    public bool isDie = false;
    public bool isHit = false;
    float KOTimer = 0;
    float KOPoint;

    //����һ����������ʾ��Ŀ�걻�������õľ���
    public int Exp;
    public int BaseExp;
    public Vector2Int HWP;



    //��ȡ��Ҷ���
    public PlayerControler player;

    //����һ��������� һ�����������߱���
    public new Rigidbody2D rigidbody2D;


    //��ʾ�쳣״̬�ĸ��ֱ���
    //��Ĭ�õ��˵ı���
    public bool isSilence = false;

    public bool isBoos;

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

    protected void EmptyHpForLevel(int level)
    {
        EmptyHp = (int)((level + 10 + (int)(((float)level * HpEmptyPoint * 2) / 100.0f))*(isBoos?1.7f:1));
        maxHP = EmptyHp;
    }

    protected void InvincibleUpdate()
    {
        if (isInvincible)
        {
            InvincileTimer -= Time.deltaTime;
            if (InvincileTimer <= 0)
            {
                isInvincible = false;
            }
        }
    }

    protected int AbilityForLevel(int level, int Ability)
    {
        return (Ability * 2 * level) / 100 + 5;
    }




    //����һ���������ı���˶����Ѫ��
    public void EmptyHpChange(float  Dmage , float SpDmage , int SkillType)
    {
        if (!isInvincible) {
            if (Dmage + SpDmage >= 0)
            {
                if (SkillType != 19)
                {
                    EmptyHp -= (int)((Dmage + SpDmage) * (Type.TYPE[SkillType][EmptyType01]) * Type.TYPE[SkillType][EmptyType02]);
                }
                else
                {
                    EmptyHp -= (int)(Dmage + SpDmage);
                }
            }
            else
            {
                EmptyHp = Mathf.Clamp(EmptyHp - (int)(Dmage + SpDmage), 0, maxHP);
            }

            Debug.Log((int)((Dmage + SpDmage) * (Type.TYPE[SkillType][EmptyType01]) * (Type.TYPE[SkillType][EmptyType02])));
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
            isInvincible = true;
            InvincileTimer = TimeInvincible;
        }
    }


    //����һ����������ʾ���˶��󱻻���
    public void EmptyKnockOut(float KnockOutPoint)
    {
        isHit = true;
        KOPoint = KnockOutPoint;
    }
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

    protected float EmptyToxicTimer;
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

    //���ô˺���ʱ����Ŀ����˶�����Ҫ���ڶ���
    public void EmptyDestroy()
    {

        Destroy(gameObject);
    }


}
