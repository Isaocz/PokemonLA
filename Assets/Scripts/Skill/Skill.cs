using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{

    //���ܵ����
    public int SkillIndex;
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
    //��ͨ���ܵ�����
    public string PlusSkillDiscribe;
    //�ü��ܵľ�ͨ����
    public Skill PlusSkill;
    //�����Ѿ�ͨ�ļ��ܣ������˾�ͨǰ�İ汾��
    public Skill MinusSkill;

    //һ������ֵ��ʾ�����Ƿ��ѷ��������ڷǶ���˺�
    bool isHitDone = false;


    //����2����������ʾ���ܵ���ȴʱ�䣬�Լ����ܿ��Ի��˵��˵ľ���
    public float KOPoint;
    public float ColdDown;

    //�������һ���ȼ��ı���
    public int CTLevel;
    //��ʾ�ü����Ƿ񾭹�PPUP
    public bool isPPUP
    {
        //Debug.Log(isppp);
        get { return isppp; }
        set { isppp = value; }
    }
    bool isppp ;

    
    //���ܵ�Ʒ�ʵȼ�
    public int SkillQualityLevel;
    //��ʾ���ܵ���Դ 0��ʾ��ѧϰ��� 1��ʾ�ɼ���ѧϰ����� 2��ʾ��ͨ����
    public int SkillFrom;



    //���ܵ�Tag
    public Skill.SkillTagEnum[] SkillTag;
    //Tag1:�Ӵ��� Tag2:�ǽӴ��� Tag3:צ�� Tag4:���� Tag5:������
    public enum SkillTagEnum
    {
        �Ӵ���,�ǽӴ���,צ��,����,������,�������ʹ����,�ָ�HP��,��ȡHP��,����ʹ����������,���������˺���,��ը��,
        ȭ��,�����Ͳ�����,��ĩ��,��͵���,���鹥����,������,����,�и���,������,������,��ס��,
    }

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

    //���ܵ�ʹ������



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
            if (CDCell.isMultipleDamageColdDown) { 
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
                if (TargetList[i].Target == target) { isTargetExitInList = true; TCEell = TargetList[i]; ListIndex = i ;  Debug.Log("xxx" + TargetList[i].isMultipleDamageColdDown);  break;   }
            }
            if (!isTargetExitInList)
            {
                TargetList.Add(TCEell);
            }
        }




        if (!isHitDone || (isMultipleDamage && !TCEell.isMultipleDamageColdDown)) {
            if (Damage == 0)
            {
                float WeatherAlpha = ((Weather.GlobalWeather.isRain && SkillType == 11) ? (Weather.GlobalWeather.isRainPlus ? 1.8f : 1.3f) : 1) * ((Weather.GlobalWeather.isRain && SkillType == 10) ? 0.5f : 1) * ((Weather.GlobalWeather.isSunny && SkillType == 11) ? 0.5f : 1) * ((Weather.GlobalWeather.isSunny && SkillType == 10) ? (Weather.GlobalWeather.isSunnyPlus ? 1.8f : 1.3f) : 1);
                if(Random.Range(0.0f , 1.0f ) >= 0.04f + 0.01f * player.LuckPoint)
                {
                    target.EmptyHpChange(0, (SpDamage * WeatherAlpha * (SkillType == player.PlayerType01 ? 1.5f : 1) * (SkillType == player.PlayerType02 ? 1.5f : 1) * (player.PlayerTeraTypeJOR == 0 ? (SkillType == player.PlayerTeraType ? 1.5f : 1) : (SkillType == player.PlayerTeraTypeJOR ? 1.5f : 1)) * (2 * player.Level + 10) * player.SpAAbilityPoint) / (250 * target.SpdAbilityPoint * ((Weather.GlobalWeather.isHail ? ((target.EmptyType01 == Type.TypeEnum.Ice || target.EmptyType02 == Type.TypeEnum.Ice) ? 1.5f : 1) : 1))) + 2, SkillType);
                }
                else
                {
                    target.EmptyHpChange(0, (SpDamage * WeatherAlpha * (SkillType == player.PlayerType01 ? 1.5f : 1) * (SkillType == player.PlayerType02 ? 1.5f : 1) * (player.PlayerTeraTypeJOR == 0 ? (SkillType == player.PlayerTeraType ? 1.5f : 1) : (SkillType == player.PlayerTeraTypeJOR ? 1.5f : 1)) * 1.5f * (2 * player.Level + 10) * player.SpAAbilityPoint) / (250 * target.SpdAbilityPoint * ((Weather.GlobalWeather.isHail ? ((target.EmptyType01 == Type.TypeEnum.Ice || target.EmptyType02 == Type.TypeEnum.Ice) ? 1.5f : 1) : 1))) + 2, SkillType);
                }
                

            }
            else if(SpDamage == 0)
            {
                float WeatherAlpha = ((Weather.GlobalWeather.isRain && SkillType == 11) ? (Weather.GlobalWeather.isRainPlus ? 1.8f : 1.3f) : 1) * ((Weather.GlobalWeather.isRain && SkillType == 10) ? 0.5f : 1) * ((Weather.GlobalWeather.isSunny && SkillType == 11) ? 0.5f : 1) * ((Weather.GlobalWeather.isSunny && SkillType == 10) ? (Weather.GlobalWeather.isSunnyPlus ? 1.8f : 1.3f) : 1);

                if (Random.Range(0.0f, 1.0f) >= 0.04f * Mathf.Pow(2,CTLevel) + 0.01f * player.LuckPoint)
                {
                    target.EmptyHpChange((Damage * WeatherAlpha * (SkillType == player.PlayerType01 ? 1.5f : 1) * (SkillType == player.PlayerType02 ? 1.5f : 1) * (player.PlayerTeraTypeJOR == 0 ? (SkillType == player.PlayerTeraType ? 1.5f : 1) : (SkillType == player.PlayerTeraTypeJOR ? 1.5f : 1)) * (2 * player.Level + 10) * player.AtkAbilityPoint) / (250 * target.DefAbilityPoint * ((Weather.GlobalWeather.isSandstorm ? ((target.EmptyType01 == Type.TypeEnum.Rock || target.EmptyType02 == Type.TypeEnum.Rock) ? 1.5f : 1) : 1))) + 2, 0, SkillType);
                }
                else
                {
                    target.EmptyHpChange((Damage * WeatherAlpha * (SkillType == player.PlayerType01 ? 1.5f : 1) * (SkillType == player.PlayerType02 ? 1.5f : 1) * (player.PlayerTeraTypeJOR == 0 ? (SkillType == player.PlayerTeraType ? 1.5f : 1) : (SkillType == player.PlayerTeraTypeJOR ? 1.5f : 1)) * 1.5f * (2 * player.Level + 10) * player.AtkAbilityPoint) / (250 * target.DefAbilityPoint * ((Weather.GlobalWeather.isSandstorm ? (( target.EmptyType01 == Type.TypeEnum.Rock || target.EmptyType02 == Type.TypeEnum.Rock) ? 1.5f : 1 ) : 1)) ) + 2, 0, SkillType);
                }
                
            }
            target.EmptyKnockOut(KOPoint);
            isHitDone = true;
            if (isMultipleDamage) {
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




    //==========================�йؼ��ܵ������ж�=================================

    public bool useSkillConditions(PlayerControler player)
    {
        if( SkillIndex == 53 || SkillIndex == 54 || SkillIndex == 55 || SkillIndex == 56)
        {
            return SleepCanUseSkill(player);
        }
        else
        {
            return NormalSkill(player);
        }
    }


    protected static bool NormalSkill(PlayerControler player)
    {
        if (player.isSleepDone) { return false; }
        else { return true; }
    }

    protected static bool SleepCanUseSkill(PlayerControler player)
    {
        if (player.isSleepDone) { return true; }
        else { return false; }
    }

    //==========================�йؼ��ܵ������ж�=================================


}
