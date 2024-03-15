using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Skill : MonoBehaviour
{

    //���ܵ����
    public int SkillIndex;
    //������Ҷ���
    public PlayerControler player;
    //����ü���Ϊ���汦���ͷţ�ʹ�ñ���
    public Baby baby;
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
    protected bool isHitDone = false;


    //����2����������ʾ���ܵ���ȴʱ�䣬�Լ����ܿ��Ի��˵��˵ľ���
    public float KOPoint;
    public float ColdDown;

    //�������һ���ȼ��ı���
    public int CTLevel;
    public int CTDamage;
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
        ȭ��,�����Ͳ�����,��ĩ��,��͵���,���鹥����,������,����,�и���,������,������,��ס��,ѹ����,�����,����,������,������,β����,���и���Ч��������Ч���ļ���,
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

    
    
    //���ڶ�˹����Ĺ�����
    protected struct EmptyList
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
    protected List<EmptyList> TargetList = new List<EmptyList> { };


    void ResetPlayer()
    {
        if(player == null && baby == null)
        {
            player = GameObject.FindObjectOfType<PlayerControler>();
        }
    }

    

    //���ɼ��ܵı�����Ч
    protected void GetCTEffect(Empty target)
    {
        //��ȡ����������Ч
        GameObject CTEffect = PublicEffect.StaticPublicEffectList.ReturnAPublicEffect(0);
        //ʵ����
        Instantiate(CTEffect, target.transform.position + Vector3.right*Random.Range(-0.5f,0.5f) + Vector3.up * Random.Range(0.0f, 0.8f), Quaternion.identity , target.transform).SetActive(true);
    }



    private void Awake()
    {
        Invoke("SkillStart", 0.05f);
    }

    void SkillStart()
    {
        Debug.Log(111);
        if (player.isInSuperPsychicTerrain)
        {
            bool isTraceSkill = false;
            for (int i = 0; i < SkillTag.Length; i++)
            {
                if (SkillTag[i] == SkillTagEnum.���и���Ч��������Ч���ļ���) { isTraceSkill = true; }
            }
            if (isTraceSkill)
            {
                TraceEffect TE = null;
                foreach (Transform t in transform)
                {
                    if (t.GetComponent<TraceEffect>())
                    {
                        TE = t.GetComponent<TraceEffect>();
                        break;
                    }
                }
                if (TE == null) { TE = GetComponent<TraceEffect>(); }
                if (TE != null)
                {
                    TE.distance += 1.3f;
                }
            }
        }
        if (SkillType == 15 && player.PlayerAbility == PlayerControler.PlayerAbilityList.ѩ��)
        {
            player.TriggerSnowCloak();
        }
        if (player.PlayerAbility == PlayerControler.PlayerAbilityList.�ٶ�)
        {
            bool isTouch = false;
            foreach (Skill.SkillTagEnum i in SkillTag){  if (i == Skill.SkillTagEnum.�Ӵ���) { isTouch = true; break; } }
            if (isTouch) { player.TriggerOblivious(); }
        }


        //�����༼��
        if (_mTool.ContainsSkillTag(SkillTag, SkillTagEnum.������)){ 

            //����065 �ֶ���˷�
            if (player.playerData.IsPassiveGetList[65])
            {
                Instantiate(PassiveItemGameObjList.ObjList.List[19], player.transform.position, Quaternion.identity, player.transform);
            }

            //����073 ����߸�
            if (player.playerData.IsPassiveGetList[73])
            {
                TorchSongFire f01 = Instantiate(PassiveItemGameObjList.ObjList.List[21], player.transform.position + (Vector3)(Vector2.up * player.SkillOffsetforBodySize[0]) + (Vector3)player.look * 0.5f, Quaternion.identity).GetComponent<TorchSongFire>();
                TorchSongFire f02 = Instantiate(PassiveItemGameObjList.ObjList.List[21], player.transform.position + (Vector3)(Vector2.up * player.SkillOffsetforBodySize[0]) + (Vector3)player.look * 0.5f, Quaternion.identity).GetComponent<TorchSongFire>();
                TorchSongFire f03 = Instantiate(PassiveItemGameObjList.ObjList.List[21], player.transform.position + (Vector3)(Vector2.up * player.SkillOffsetforBodySize[0]) + (Vector3)player.look * 0.5f, Quaternion.identity).GetComponent<TorchSongFire>();
                f01.LaunchNotForce((Vector3)player.look.normalized, 3.5f);
                f02.LaunchNotForce((Quaternion.AngleAxis(30, Vector3.forward) * (Vector3)player.look).normalized, 3.5f);
                f03.LaunchNotForce((Quaternion.AngleAxis(-30, Vector3.forward) * (Vector3)player.look).normalized, 3.5f);
                f01.player = player;
                f02.player = player;
                f03.player = player;
            }

            //����117 ˬ������
            if (player.playerData.IsPassiveGetList[117])
            {
                player.playerData.PassiveItemThroatSpray();
            }
        }

        //�����༼��
        if (_mTool.ContainsSkillTag(SkillTag, SkillTagEnum.�Ӵ���))
        {
            //����138 ����֮צ
            if (player.playerData.IsPassiveGetList[138])
            {
                CTDamage++;CTLevel++;
            }
        }

        if (SkillType == (int)Type.TypeEnum.Water )
        {
            //����074 ��ˮ����
            if (player.playerData.IsPassiveGetList[74])
            {
                player.MinusSkillCDTime(1 , 0.1f , false);
                player.MinusSkillCDTime(2 , 0.1f , false);
                player.MinusSkillCDTime(3 , 0.1f , false);
                player.MinusSkillCDTime(4 , 0.1f , false);
            }
        }

        //����078 ������
        if (player.playerData.IsPassiveGetList[78])
        {
            if (Random.Range(0.0f, 1.0f) + (float)player.LuckPoint / 30 >= 0.85f)
            {
                ScalchopPro s01 = Instantiate(PassiveItemGameObjList.ObjList.List[23], player.transform.position + (Vector3)(Vector2.up * player.SkillOffsetforBodySize[0]) + (Vector3)player.look * 0.5f, Quaternion.identity).GetComponent<ScalchopPro>();
                s01.player = player;
                s01.LaunchNotForce(player.look , 9.0f);
                s01.transform.rotation = Quaternion.Euler( 0 , 0 , _mTool.Angle_360Y(player.look , Vector2.right) );
            }
        }

        //����091 ��ˮ���｣
        if (player.playerData.IsPassiveGetList[91])
        {
            ScalchopPro s01 = Instantiate(PassiveItemGameObjList.ObjList.List[24], player.transform.position + (Vector3)(Vector2.up * player.SkillOffsetforBodySize[0]) + (Vector3)player.look * 0.5f, Quaternion.identity).GetComponent<ScalchopPro>();
            s01.player = player;
            s01.LaunchNotForce(-player.look, 9.0f);
        }

        //����093 ħ�ü���
        if (player.playerData.IsPassiveGetList[93])
        {
            player.playerData.MasqueradeChangeType((Type.TypeEnum)SkillType);
        }

        //����096 �侲ͷ��
        if (SkillType == 15 && player.playerData.IsPassiveGetList[96])
        {
            player.playerData.PassiveItemClamMind();
        }

        //����103 ���ܲ�
        if (SkillType == 15 && player.playerData.IsPassiveGetList[103])
        {
            Damage *= 1.35f;SpDamage *= 1.35f;
        }

        //����104 ���ܲ�
        if (SkillType == 8 && player.playerData.IsPassiveGetList[104])
        {
            Damage *= 1.35f; SpDamage *= 1.35f;

        }
        //����106 ��֮����
        if (SkillType == 17 && player.playerData.IsPassiveGetList[106])
        {
            Damage *= 1.35f; SpDamage *= 1.35f;
        }

        //����107 ˮ֮����
        if (SkillType == 11 && player.playerData.IsPassiveGetList[107])
        {
            Damage *= 1.35f; SpDamage *= 1.35f;
        }

        //����112 ���˴�
        if (player.playerData.IsPassiveGetList[112])
        {
            CTDamage++;
        }

        //����137 ���㾵
        if (player.playerData.IsPassiveGetList[137])
        {
            CTLevel++;
        }

        //����114 ������
        if (player.playerData.IsPassiveGetList[114])
        {
            if (SkillIndex == player.playerData.MetronomeSkillIndex) { player.playerData.MetronomeCount++; }
            else { player.playerData.MetronomeCount = 0; player.playerData.MetronomeSkillIndex = SkillIndex; }
            Damage *= Mathf.Clamp(Mathf.Pow(1.2f, player.playerData.MetronomeCount) , 1.0f , 2.0f);
            SpDamage *= Mathf.Clamp(Mathf.Pow(1.2f, player.playerData.MetronomeCount), 1.0f, 2.0f);
        }

        //����12:����
        if (player.PlayerAbility == PlayerControler.PlayerAbilityList.���� && SkillType == (int)Type.TypeEnum.Water && player.Hp < player.maxHp / 3.0f) { 
            Damage *= 1.5f; 
            SpDamage *= 1.5f; 
        }
    }

    //���������м��ܵ�Update������������ʱ��ľ�ʱ������ʧ
    public void StartExistenceTimer()
    {
        ResetPlayer();
        ExistenceTime -= Time.deltaTime;
        //if (!isStartMDone) { SkillStart(); }

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


    /// <summary>
    /// �Ե���target����˺��ͻ���
    /// </summary>
    /// <param name="target"></param>
    public virtual void HitAndKo(Empty target)
    {
        BeforeHitEvent(target);

        EmptyList TCEell = new EmptyList(target, false, 0.0f);
        int ListIndex = 0;

        if (isMultipleDamage)
        {
            bool isTargetExitInList = false;
            if (TargetList.Count == 0) { TargetList.Add(new EmptyList(target, false, 0.0f)); }
            for (int i = 0; i < TargetList.Count; i++)
            {
                if (TargetList[i].Target == target) { isTargetExitInList = true; TCEell = TargetList[i]; ListIndex = i ; /* Debug.Log("xxx" + TargetList[i].isMultipleDamageColdDown); */ break;   }
            }
            if (!isTargetExitInList)
            {
                TargetList.Add(TCEell);
            }
        }




        if (!isHitDone || (isMultipleDamage && !TCEell.isMultipleDamageColdDown)) {

            int BeforeHP = target.EmptyHp;
            if (Damage == 0)
            {
                float WeatherAlpha = ((Weather.GlobalWeather.isRain && SkillType == 11) ? (Weather.GlobalWeather.isRainPlus ? 1.8f : 1.3f) : 1) * ((Weather.GlobalWeather.isRain && SkillType == 10) ? 0.5f : 1) * ((Weather.GlobalWeather.isSunny && SkillType == 11) ? 0.5f : 1) * ((Weather.GlobalWeather.isSunny && SkillType == 10) ? (Weather.GlobalWeather.isSunnyPlus ? 1.8f : 1.3f) : 1);
                if (player != null) {
                    int EmptyBeforeHP = target.EmptyHp;
                    if (Random.Range(0.0f, 1.0f) >= 0.04f * Mathf.Pow(2, CTLevel) + 0.01f * player.LuckPoint)
                    {
                        Pokemon.PokemonHpChange(player.gameObject, target.gameObject, 0, SpDamage * (player.playerData.IsPassiveGetList[55] ? BulletGraze.instance.DamageImprovement : 1) * (player.playerData.IsPassiveGetList[58] ? 1.5f : 1f), 0, (Type.TypeEnum)SkillType);
                    }
                    else
                    {
                        Pokemon.PokemonHpChange(player.gameObject, target.gameObject, 0, SpDamage * 1.5f * (Mathf.Pow(1.2f, CTDamage)) * (player.playerData.IsPassiveGetList[55] ? BulletGraze.instance.DamageImprovement : 1 * (player.playerData.IsPassiveGetList[58] ? 1.5f : 1f)), 0, (Type.TypeEnum)SkillType, true);
                        GetCTEffect(target);
                    }
                    if (target.Abillity == Empty.EmptyAbillity.RoughSkin && _mTool.ContainsSkillTag(SkillTag, SkillTagEnum.�Ӵ���)) { Pokemon.PokemonHpChange(null, player.gameObject, Mathf.Clamp((EmptyBeforeHP - target.EmptyHp) / 4, 1, 10000), 0, 0, Type.TypeEnum.IgnoreType); }
                }
                else if (baby != null)
                {
                    Pokemon.PokemonHpChange(baby.gameObject, target.gameObject, 0, SpDamage * 1.5f, 0, (Type.TypeEnum)SkillType);
                    Debug.Log(baby);
                }

            }
            else if(SpDamage == 0)
            {
                float WeatherAlpha = ((Weather.GlobalWeather.isRain && SkillType == 11) ? (Weather.GlobalWeather.isRainPlus ? 1.8f : 1.3f) : 1) * ((Weather.GlobalWeather.isRain && SkillType == 10) ? 0.5f : 1) * ((Weather.GlobalWeather.isSunny && SkillType == 11) ? 0.5f : 1) * ((Weather.GlobalWeather.isSunny && SkillType == 10) ? (Weather.GlobalWeather.isSunnyPlus ? 1.8f : 1.3f) : 1);

                if (player != null)
                {
                    int EmptyBeforeHP = target.EmptyHp;
                    if (Random.Range(0.0f, 1.0f) >= 0.04f * Mathf.Pow(2, CTLevel) + 0.01f * player.LuckPoint)
                    {

                        Pokemon.PokemonHpChange(player.gameObject, target.gameObject, Damage * (player.playerData.IsPassiveGetList[55] ? BulletGraze.instance.DamageImprovement : 1) * (player.playerData.IsPassiveGetList[58] ? 1.5f : 1f), 0, 0, (Type.TypeEnum)SkillType);
                        //Debug.Log(player);//target.EmptyHpChange((Damage * WeatherAlpha * (SkillType == player.PlayerType01 ? 1.5f : 1) * (SkillType == player.PlayerType02 ? 1.5f : 1) * (player.PlayerTeraTypeJOR == 0 ? (SkillType == player.PlayerTeraType ? 1.5f : 1) : (SkillType == player.PlayerTeraTypeJOR ? 1.5f : 1)) * (2 * player.Level + 10) * player.AtkAbilityPoint) / (250 * target.DefAbilityPoint * ((Weather.GlobalWeather.isSandstorm ? ((target.EmptyType01 == Type.TypeEnum.Rock || target.EmptyType02 == Type.TypeEnum.Rock) ? 1.5f : 1) : 1))) + 2, 0, SkillType);

                    }
                    else
                    {

                        Pokemon.PokemonHpChange(player.gameObject, target.gameObject, Damage * 1.5f * (Mathf.Pow(1.2f, CTDamage) * (player.playerData.IsPassiveGetList[55] ? BulletGraze.instance.DamageImprovement : 1) * (player.playerData.IsPassiveGetList[58] ? 1.5f : 1f)), 0, 0, (Type.TypeEnum)SkillType, true);
                        GetCTEffect(target);
                        //Debug.Log(player);//target.EmptyHpChange((Damage * WeatherAlpha * (SkillType == player.PlayerType01 ? 1.5f : 1) * (SkillType == player.PlayerType02 ? 1.5f : 1) * (player.PlayerTeraTypeJOR == 0 ? (SkillType == player.PlayerTeraType ? 1.5f : 1) : (SkillType == player.PlayerTeraTypeJOR ? 1.5f : 1)) * 1.5f * (2 * player.Level + 10) * player.AtkAbilityPoint) / (250 * target.DefAbilityPoint * ((Weather.GlobalWeather.isSandstorm ? (( target.EmptyType01 == Type.TypeEnum.Rock || target.EmptyType02 == Type.TypeEnum.Rock) ? 1.5f : 1 ) : 1)) ) + 2, 0, SkillType);

                    }
                    if (target.Abillity == Empty.EmptyAbillity.RoughSkin && _mTool.ContainsSkillTag(SkillTag, SkillTagEnum.�Ӵ���)) { Pokemon.PokemonHpChange(null, player.gameObject, Mathf.Clamp((EmptyBeforeHP - target.EmptyHp) / 4 , 1 , 10000), 0, 0, Type.TypeEnum.IgnoreType); }
                }
                else if (baby != null)
                {
                    Pokemon.PokemonHpChange(baby.gameObject, target.gameObject, Damage, 0, 0, (Type.TypeEnum)SkillType);
                    Debug.Log(baby);
                }
            }
            target.EmptyKnockOut(KOPoint);
            isHitDone = true;
            if (isMultipleDamage) {
                TCEell.isMultipleDamageColdDown = true;
                TargetList[ListIndex] = TCEell;
            }
            HitEvent(target);


            //����136 ������
            if (player.playerData.IsPassiveGetList[136])
            {
                Drain(BeforeHP , target.EmptyHp , 0.1f);
            }

            //��ʽ378 ����+
            {
                if (player.Skill01 != null && player.Skill01.SkillIndex == 378 ) { player.MinusSkillCDTime(1,KOPoint,true); }
                if (player.Skill02 != null && player.Skill02.SkillIndex == 378 ) { player.MinusSkillCDTime(2,KOPoint,true); }
                if (player.Skill03 != null && player.Skill03.SkillIndex == 378 ) { player.MinusSkillCDTime(3,KOPoint,true); }
                if (player.Skill04 != null && player.Skill04.SkillIndex == 378 ) { player.MinusSkillCDTime(4,KOPoint,true); }
            }

        }

    }


    /// <summary>
    /// ����˺�ʱ�������¼�
    /// </summary>
    /// <param name="target"></param>
    public void HitEvent(Empty target)
    {
        if (player != null)
        {
            //�Ƿ��ǽӴ��༼��
            bool isTouch = false;
            if (SkillTag != null)
            {
                foreach (Skill.SkillTagEnum i in SkillTag)
                {
                    if (i == Skill.SkillTagEnum.�Ӵ���) { isTouch = true; break; }
                }
            }

            //�Ƿ������༼��
            bool isBite = false;
            if (SkillTag != null)
            {
                foreach (Skill.SkillTagEnum i in SkillTag)
                {
                    if (i == Skill.SkillTagEnum.����) { isBite = true; break; }
                }
            }

            //�Ƿ���צ�༼��
            bool isClaw = false;
            if (SkillTag != null)
            {
                foreach (Skill.SkillTagEnum i in SkillTag)
                {
                    if (i == Skill.SkillTagEnum.צ��) { isClaw = true; break; }
                }
            }



            //�Ӵ����ܴ������¼�
            if (isTouch) {

                //����026 ����
                if (player.playerData.IsPassiveGetList[26])
                {
                    target.EmptyToxicDone(1, 30, 0.4f + (float)player.LuckPoint / 10);
                }

                //����059 �����
                if (player.playerData.IsPassiveGetList[59] && Random.Range(0.0f, 1.0f) + ((float)player.LuckPoint / 30) > 0.8f)
                {
                    Instantiate(PassiveItemGameObjList.ObjList.List[18] , transform.position , Quaternion.identity).GetComponent<RandomStarMoney>().isLunch = true;
                }

                //���� ����֮��
                if (player.PlayerAbility == PlayerControler.PlayerAbilityList.����֮�� && Random.Range(0.0f, 1.0f) + ((float)player.LuckPoint / 30) > 0.9f)
                {
                    target.EmptyInfatuation(15, 0.5f);
                }

            }

            //�����ܴ������¼�
            if (isBite)
            {
                //����63 ����֮��
                if (player.playerData.IsPassiveGetList[63] && Random.Range(0.0f, 1.0f) + (float)player.LuckPoint / 10 > 0.4f)
                {
                    target.Fear(3.0f, 1);
                }
            }

            //צ���ܴ������¼�
            if (isClaw)
            {
                //����63 ����֮��
                if (player.playerData.IsPassiveGetList[63] && Random.Range(0.0f, 1.0f) + (float)player.LuckPoint / 10 > 0.4f)
                {
                    target.Fear(3.0f, 1);
                }
            }

            //���м��ܴ������¼�
            {
                //����025 ����֤֮
                if (player.playerData.IsPassiveGetList[25] && Random.Range(0.0f, 1.0f) + (float)player.LuckPoint / 30 > 0.8f)
                {
                    target.Fear(3.0f, 1);
                }

                //����097 ������
                if (player.playerData.IsPassiveGetList[97])
                {
                    target.EmptyToxicDone(1, 30, 0.2f + (float)player.LuckPoint / 30);
                }

            }


        }
    }


    /// <summary>
    /// ����˺�֮ǰ�������¼�
    /// </summary>
    /// <param name="target"></param>
    public void BeforeHitEvent(Empty target)
    {
        //����101 ��ʿ�����
        if (player.playerData.IsPassiveGetList[101] && target.isBlindDone)
        {
            Damage *= 1.5f;
            SpDamage *= 1.5f;
        }
    }


    //=================================��ι����Ĵ����ж�===========================================
   /// <summary>
   /// ����ʯ��������׶�ȴ���Ϊ2-5�εĹ�����ʹ�ôη����������
   /// </summary>
   /// <returns></returns>
    protected int Count2_5()
    {
        float p = Random.Range(0.0f, 1.0f)+((float)player.LuckPoint/30);
        //����119 ��������
        if (player.playerData.IsPassiveGetList[119]) {
            if (p >= 0 && p <= 0.4f)
            {
                return 4;
            }
            else
            {
                return 5;
            }
        }
        else
        {
            if (p >= 0 && p <= 0.35f)
            {
                return 2;
            }
            else if (p >= 0.35f && p <= 0.70f)
            {
                return 3;
            }
            else if (p >= 0.70f && p <= 0.85f)
            {
                return 4;
            }
            else
            {
                return 5;
            }
        }
    }

    protected int Count1_3()
    {
        float p = Random.Range(0.0f, 1.0f) + ((float)player.LuckPoint / 30);

        //����119 ��������
        if (player.playerData.IsPassiveGetList[119])
        {
            return 3;
        }
        else
        {
            if (p >= 0 && p <= 0.65f)
            {
                return 1;
            }
            else if (p >= 0.65f && p <= 0.90f)
            {
                return 2;
            }
            else
            {
                return 3;
            }
        }
    }

    //=================================��ι����Ĵ����ж�===========================================





    //==========================�йؼ��ܵ������ж�=================================

    public bool useSkillConditions(PlayerControler player)
    {
        //Ϊ�λ����ߴ���
        if( SkillIndex == 53 || SkillIndex == 54 || SkillIndex == 55 || SkillIndex == 56)
        {
            return SleepCanUseSkill(player);
        }
        //ΪͶ��
        else if (SkillIndex == 303)
        {

            if (player.spaceItem == null) { return false; }
            else { return true; }
        }
        //ΪͶ����ͨ
        else if (SkillIndex == 304)
        {
            if (Mathf.Ceil(12 + player.maxHp * 0.06f) > player.Hp - 1) { return false; }
            else { return true; }
        }
        //Ϊ��ػ�����ؾ�ͨ
        else if (SkillIndex == 317 || SkillIndex == 318)
        {
            if (((player.Skill01 == null) || (player.Skill01 != null && player.Skill01.SkillIndex != 317 && player.Skill01.SkillIndex != 318 && player.isSkill01CD) || (player.Skill01 != null && ( player.Skill01.SkillIndex == 317 || player.Skill01.SkillIndex == 318 ))) &&
                ((player.Skill02 == null) || (player.Skill02 != null && player.Skill02.SkillIndex != 317 && player.Skill02.SkillIndex != 318 && player.isSkill02CD) || (player.Skill02 != null && (player.Skill02.SkillIndex == 317 || player.Skill02.SkillIndex == 318))) &&
                ((player.Skill03 == null) || (player.Skill03 != null && player.Skill03.SkillIndex != 317 && player.Skill03.SkillIndex != 318 && player.isSkill03CD) || (player.Skill03 != null && (player.Skill03.SkillIndex == 317 || player.Skill03.SkillIndex == 318))) &&
                ((player.Skill04 == null) || (player.Skill04 != null && player.Skill04.SkillIndex != 317 && player.Skill04.SkillIndex != 318 && player.isSkill04CD) || (player.Skill04 != null && (player.Skill04.SkillIndex == 317 || player.Skill04.SkillIndex == 318))) 
                )
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        //Ϊ��������
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



    //==================================�ٷֱ���Ѫ========================================
    /// <summary>
    /// ��Ѫ�ٷֱȵĲ���
    /// </summary>
    public float DrainBounsPer;
    public void Drain(int TargetHpBefore , int TargetHpAfter , float DrainPer)
    {
        if (TargetHpAfter < TargetHpBefore)
        {
            Pokemon.PokemonHpChange(null, player.gameObject, 0, 0, Mathf.Clamp((int)((float)(TargetHpBefore - TargetHpAfter) * (DrainPer + DrainBounsPer)), 1, 100), Type.TypeEnum.IgnoreType);
        }
    }

    //==============================================================================


}
