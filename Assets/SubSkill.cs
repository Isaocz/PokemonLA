using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubSkill : MonoBehaviour
{

    //������Ҷ���
    public PlayerControler player;
    //�������ܴ��ڵ�ʱ��
    public float ExistenceTime;
    //������������ֵ
    public float Damage;
    //�����ع�����ֵ
    public float SpDamage;
    //��������������
    public Animator animator;
    //�䵯�༼�ܵ�������
    public float MaxRange;

    //�������ܵ�����
    public int SkillType;

    //һ������ֵ��ʾ�����Ƿ��ѷ��������ڷǶ���˺�
    bool isHitDone = false;


    //����2����������ʾ���ܵ���ȴʱ�䣬�Լ����ܿ��Ի��˵��˵ľ���
    public float KOPoint;
    public float ColdDown;

    //�������һ���ȼ��ı���
    public int CTLevel;

    public Skill MainSkill;
    public SubSkill subskill;





    //���ܵ�Tag
    public Skill.SkillTagEnum[] SkillTag;
    //Tag1:�Ӵ��� Tag2:�ǽӴ��� Tag3:צ�� Tag4:���� Tag5:������

    //��ʾ��������ʱ�Ƿ��������������Է�����ΪFales�������������Եķ�����Ϊtrue���������λ�ã�����������buff�༼�ܣ�
    public bool isNotDirection;
    //����һ���з���ļ��ܣ�isNotDirection == False��������ʱ������ҵľ����ж�Զ��
    public float DirctionDistance;

    //��ʾ�����Ƿ����������ƶ� ֻ����isNotDirection == true�ļ�����Ч
    public bool isNotMoveWithPlayer;

    //��ʾ�����Ƿ��Ƕ�˹���
    public bool isMultipleDamage;
    //����һ����ι������ܣ���ʾ���֮�����ȴʱ��
    public float MultipleDamageCDTime;

    //��ʾ���������Ƿ���Ҫ̧�֣�����λ���༼����Ҫ��������������һ�̿�ʼλ�ƣ����䵯����ܻ���һ��̧��ǰҡ
    public bool isImmediately;


    //���ڶ�˹����Ĺ�����
    struct EmptyList
    {


        public EmptyList(Empty target, bool v1, float v2) : this()
        {
            Target = target;
            isMultipleDamageColdDown = v1;
            MultipleDamageColdDownTimer = v2;
        }

        public Empty Target;
        public bool isMultipleDamageColdDown { get; set; }
        public float MultipleDamageColdDownTimer { get; set; }

    }
    List<EmptyList> TargetList = new List<EmptyList> { };







    //���������м��ܵ�Update������������ʱ��ľ�ʱ������ʧ
    public void StartExistenceTimer()
    {
        ExistenceTime -= Time.deltaTime;

        //��ι�����ʼ��ȴ֮��ʼ��ʱ
        if (isMultipleDamage)
        {
            RestoreTargetListCD();
        }

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


    void RestoreTargetListCD()
    {
        for (int i = 0; i < TargetList.Count; i++)
        {
            EmptyList CDCell = TargetList[i];
            if (CDCell.isMultipleDamageColdDown)
            {
                CDCell.MultipleDamageColdDownTimer += Time.deltaTime;
                if (CDCell.MultipleDamageColdDownTimer >= MultipleDamageCDTime) { CDCell.MultipleDamageColdDownTimer = 0; CDCell.isMultipleDamageColdDown = false; }
            }
            TargetList[i] = CDCell;
        }
    }



    //�Ե���target����˺��ͻ���
    public void HitAndKo(Empty target)
    {
        EmptyList TCEell = new EmptyList(target, false, 0.0f);
        int ListIndex = 0;

        if (isMultipleDamage)
        {
            bool isTargetExitInList = false;
            if (TargetList.Count == 0) { TargetList.Add(new EmptyList(target, false, 0.0f)); }
            for (int i = 0; i < TargetList.Count; i++)
            {
                if (TargetList[i].Target == target) { isTargetExitInList = true; TCEell = TargetList[i]; ListIndex = i; Debug.Log("xxx" + TargetList[i].isMultipleDamageColdDown); break; }
            }
            if (!isTargetExitInList)
            {
                TargetList.Add(TCEell);
            }
        }




        if (!isHitDone || (isMultipleDamage && !TCEell.isMultipleDamageColdDown))
        {
            if (Damage == 0)
            {
                if (Random.Range(0.0f, 1.0f) >= 0.04f + 0.01f * player.LuckPoint)
                {
                    Pokemon.PokemonHpChange(player.gameObject, target.gameObject, 0, SpDamage, 0, (Type.TypeEnum)SkillType);
                    //target.EmptyHpChange(0, (SpDamage * (SkillType == player.PlayerType01 ? 1.5f : 1) * (SkillType == player.PlayerType02 ? 1.5f : 1) * (player.PlayerTeraTypeJOR == 0 ? (SkillType == player.PlayerTeraType ? 1.5f : 1) : (SkillType == player.PlayerTeraTypeJOR ? 1.5f : 1)) * (2 * player.Level + 10) * player.SpAAbilityPoint) / (250 * target.SpdAbilityPoint) + 2, SkillType);
                }
                else
                {
                    Pokemon.PokemonHpChange(player.gameObject, target.gameObject, 0, SpDamage*1.5f, 0, (Type.TypeEnum)SkillType);
                    //target.EmptyHpChange(0, (SpDamage * (SkillType == player.PlayerType01 ? 1.5f : 1) * (SkillType == player.PlayerType02 ? 1.5f : 1) * (player.PlayerTeraTypeJOR == 0 ? (SkillType == player.PlayerTeraType ? 1.5f : 1) : (SkillType == player.PlayerTeraTypeJOR ? 1.5f : 1)) * 1.5f * (2 * player.Level + 10) * player.SpAAbilityPoint) / (250 * target.SpdAbilityPoint) + 2, SkillType);
                }


            }
            else if (SpDamage == 0)
            {
                if (Random.Range(0.0f, 1.0f) >= 0.04f * Mathf.Pow(2, CTLevel) + 0.01f * player.LuckPoint)
                {
                    Pokemon.PokemonHpChange(player.gameObject, target.gameObject, Damage, 0, 0, (Type.TypeEnum)SkillType);
                    //target.EmptyHpChange((Damage * (SkillType == player.PlayerType01 ? 1.5f : 1) * (SkillType == player.PlayerType02 ? 1.5f : 1) * (player.PlayerTeraTypeJOR == 0 ? (SkillType == player.PlayerTeraType ? 1.5f : 1) : (SkillType == player.PlayerTeraTypeJOR ? 1.5f : 1)) * (2 * player.Level + 10) * player.AtkAbilityPoint) / (250 * target.DefAbilityPoint) + 2, 0, SkillType);
                }
                else
                {
                    Pokemon.PokemonHpChange(player.gameObject, target.gameObject, Damage*1.5f, 0, 0, (Type.TypeEnum)SkillType);
                    //target.EmptyHpChange((Damage * (SkillType == player.PlayerType01 ? 1.5f : 1) * (SkillType == player.PlayerType02 ? 1.5f : 1) * (player.PlayerTeraTypeJOR == 0 ? (SkillType == player.PlayerTeraType ? 1.5f : 1) : (SkillType == player.PlayerTeraTypeJOR ? 1.5f : 1)) * 1.5f * (2 * player.Level + 10) * player.AtkAbilityPoint) / (250 * target.DefAbilityPoint) + 2, 0, SkillType);
                }

            }
            target.EmptyKnockOut(KOPoint);
            isHitDone = true;
            if (isMultipleDamage)
            {
                TCEell.isMultipleDamageColdDown = true;
                TargetList[ListIndex] = TCEell;
            }
            if (player.playerData.IsPassiveGetList[26] && Random.Range(0.0f, 1.0f) + (float)player.LuckPoint / 30 > 0.6f)
            {
                if (SkillTag != null)
                {
                    foreach (Skill.SkillTagEnum i in SkillTag)
                    {
                        if (i == Skill.SkillTagEnum.�Ӵ���) { target.EmptyToxicDone(1 , 30); }
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
