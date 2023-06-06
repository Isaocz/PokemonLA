using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{

    //������Ҷ���
    public PlayerControler player;
    //�������ܴ��ڵ�ʱ��
    public float ExistenceTime;
    //������������ֵ
    public float Damage;
    //�����ع�����ֵ
    public float SpDamage;
    //�����Ƿ�Ϊ�����ɱ仯����
    public bool IsDamageChangeable;
    //��������������
    public Animator animator;
    //�䵯�༼�ܵ�������
    public float MaxRange;

    //�������ܵ�����
    public int SkillType;
    //���ܵ�Ӣ����������������������
    public string SkillName;
    public string SkillChineseName;
    public string SkillDiscribe;

    //һ������ֵ��ʾ�����Ƿ��ѷ��������ڷǶ���˺�
    bool isHitDone = false;


    //����2����������ʾ���ܵ���ȴʱ�䣬�Լ����ܿ��Ի��˵��˵ľ���
    public float KOPoint;
    public float ColdDown;

    //���ܵ�Tag
    public int[] SkillTag;
    //Tag1:�Ӵ��� Tag2:�ǽӴ��� Tag3:צ�� Tag4:���� Tag5:������

    //��ʾ��������ʱ�Ƿ��������������Է�����ΪFales�������������Եķ�����ΪFalse���������λ�ã�����������buff�༼�ܣ�
    public bool isNotDirection;

    //��ʾ�����Ƿ����������ƶ� ֻ����isNotDirection == true�ļ�����Ч
    public bool isNotMoveWithPlayer;

    //��ʾ�����Ƿ��Ƕ�˹���
    public bool isMultipleDamage;

    //��ʾ���������Ƿ���Ҫ̧�֣�����λ���༼����Ҫ��������������һ�̿�ʼλ�ƣ����䵯����ܻ���һ��̧��ǰҡ
    public bool isImmediately;


    //���������м��ܵ�Update������������ʱ��ľ�ʱ������ʧ
    public void StartExistenceTimer()
    {
        ExistenceTime -= Time.deltaTime;
        if (ExistenceTime <= 0)
        {
            DestroySelf();
        }
    }

    //�ݻټ��ܵĺ�������Ϊ��ʱ���ڶ����е��ã����Զ�������
    public void DestroySelf()
    {
        Destroy(gameObject);
    }

    //�Ե���target����˺��ͻ���
    public void HitAndKo(Empty target)
    {
        
        if (isMultipleDamage || !isHitDone) {
            if(Damage == 0)
            {
                if(Random.Range(0.0f , 1.0f ) >= 0.04f + 0.01f * player.LuckPoint)
                {
                    target.EmptyHpChange(0, (SpDamage * (SkillType == player.PlayerType01 ? 1.5f : 1) * (SkillType == player.PlayerType02 ? 1.5f : 1) * (player.PlayerTeraTypeJOR == 0 ? (SkillType == player.PlayerTeraType ? 1.5f : 1) : (SkillType == player.PlayerTeraTypeJOR ? 1.5f : 1)) * (2 * player.Level + 10) * player.SpAAbilityPoint) / (250 * target.SpdAbilityPoint) + 2, SkillType);
                }
                else
                {
                    target.EmptyHpChange(0, (SpDamage * (SkillType == player.PlayerType01 ? 1.5f : 1) * (SkillType == player.PlayerType02 ? 1.5f : 1) * (player.PlayerTeraTypeJOR == 0 ? (SkillType == player.PlayerTeraType ? 1.5f : 1) : (SkillType == player.PlayerTeraTypeJOR ? 1.5f : 1)) * 1.5f * (2 * player.Level + 10) * player.SpAAbilityPoint) / (250 * target.SpdAbilityPoint) + 2, SkillType);
                }
                

            }
            else if(SpDamage == 0)
            {
                if (Random.Range(0.0f, 1.0f) >= 0.04f + 0.01f * player.LuckPoint)
                {
                    target.EmptyHpChange((Damage * (SkillType == player.PlayerType01 ? 1.5f : 1) * (SkillType == player.PlayerType02 ? 1.5f : 1) * (player.PlayerTeraTypeJOR == 0 ? (SkillType == player.PlayerTeraType ? 1.5f : 1) : (SkillType == player.PlayerTeraTypeJOR ? 1.5f : 1)) * (2 * player.Level + 10) * player.AtkAbilityPoint) / (250 * target.DefAbilityPoint) + 2, 0, SkillType);
                }
                else
                {
                    target.EmptyHpChange((Damage * (SkillType == player.PlayerType01 ? 1.5f : 1) * (SkillType == player.PlayerType02 ? 1.5f : 1) * (player.PlayerTeraTypeJOR == 0 ? (SkillType == player.PlayerTeraType ? 1.5f : 1) : (SkillType == player.PlayerTeraTypeJOR ? 1.5f : 1)) * 1.5f * (2 * player.Level + 10) * player.AtkAbilityPoint) / (250 * target.DefAbilityPoint) + 2, 0, SkillType);
                }
                
            }
            target.EmptyKnockOut(KOPoint);
            isHitDone = true;
            if (player.playerData.IsPassiveGetList[26] && Random.Range(0.0f, 1.0f) + (float)player.LuckPoint / 30 > 0.6f)
            {
                if (SkillTag != null)
                {
                    foreach (int i in SkillTag)
                    {
                        if (i == 1) { target.EmptyToxicDone(1); }
                    }
                }
            }
            if (player.playerData.IsPassiveGetList[25] && Random.Range(0.0f, 1.0f) + (float)player.LuckPoint / 30 > 0.8f)
            {
                target.Fear(3.0f, 1);
            }
        }

    }

}
