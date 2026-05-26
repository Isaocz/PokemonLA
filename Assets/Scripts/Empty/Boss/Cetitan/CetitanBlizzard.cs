using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CetitanBlizzard : MonoBehaviour
{




    //==============================音效枚举===================================

    /// <summary>
    /// 音效种类枚举
    /// </summary>
    public enum CetitanBlizzardSE
    {
        Blizzard,
    }

    /// <summary>
    /// 一次性音效播放器生成
    /// </summary>
    public EnemyAudioPlayer audioPlayer;
    /// <summary>
    /// 循环音效音效播放器生成
    /// </summary>
    public LoopingSEAudioPlayer loopPlayer;

    //==============================音效枚举===================================






    /// <summary>
    /// 父浩大鲸
    /// </summary>
    public Cetitan ParentCetitan;


    /// <summary>
    /// 伤害（特攻）
    /// </summary>
    public int BlizzardSpDmage = 100;
    /// <summary>
    /// 击退值
    /// </summary>
    public float BlizzardKOPoint = 7.5f;
    /// <summary>
    /// 冰冻值
    /// </summary>
    public float BlizzardFrozenPoint = 0.5f;



    [SerializeField, Range(0, 2)]
    private int cetitanBlizzardLevel;
    /// <summary>
    /// 浩大鲸暴雪等级
    /// </summary>
    public int CetitanBlizzardLevel
    {
        get { return cetitanBlizzardLevel; }
        set { cetitanBlizzardLevel = Mathf.Clamp(value, 0, 2); }
    }


    /// <summary>
    /// 暴风雪伤害范围半径（依次为 0,1,2）
    /// </summary>
    static List<float> DamageRadius = new List<float> { 10.8f, 8.9f, 7.0f };
    /// <summary>
    /// 伤害半径
    /// </summary>
    private float currentDamageRadius = 18.0f;
    /// <summary>
    /// 是否可造成伤害
    /// </summary>
    public bool isCanDmage = true;



    /// <summary>
    /// 技能指示圈在不同等级时的缩放大小（依次为0，1，2）
    /// </summary>
    static List<float> Scale_SkillRange = new List<float> { 6.2f , 5.1f , 4.0f };
    /// <summary>
    /// 技能指示圈
    /// </summary>
    public GameObject SkillRange;
    /// <summary>
    /// 指示圈变化时间
    /// </summary>
    public static float BlizzardScaleDuration = 2.0f; 




    /// <summary>
    /// 粒子效果在不同等级时的内径比率（依次为0，1，2）
    /// </summary>
    static List<float> RadiusThickness_PS = new List<float> { 0.3f, 0.41f, 0.5f };

    /// <summary>
    /// 粒子蓝雾在三级暴雪时的发射量
    /// </summary>
    static float PSMistBlue_RateOTime_L3 = 300;

    /// <summary>
    /// 粒子蓝雾
    /// </summary>
    public ParticleSystem PSMistBlue;


    /// <summary>
    /// 粒子白雾在三级暴雪时的发射量
    /// </summary>
    static float PSMistWrite_RateOTime_L3 = 60;

    /// <summary>
    /// 粒子白雾
    /// </summary>
    public ParticleSystem PSMistWrite;


    /// <summary>
    /// 粒子拖尾雪球在三级暴雪时的发射量
    /// </summary>
    static float PSSnowTrail_RateOTime_L3 = 180;

    /// <summary>
    /// 粒子拖尾雪球
    /// </summary>
    public ParticleSystem PSSnowTrail;


    /// <summary>
    /// 粒子慢速雪花在三级暴雪时的发射量
    /// </summary>
    static float PSFrozenSlow_RateOTime_L3 = 75;

    /// <summary>
    /// 粒子慢速雪花
    /// </summary>
    public ParticleSystem PSFrozenSlow;


    /// <summary>
    /// 粒子快速雪花在三级暴雪时的发射量
    /// </summary>
    static float PSFrozenFast_RateOTime_L3 = 45;

    /// <summary>
    /// 粒子快速雪花
    /// </summary>
    public ParticleSystem PSFrozenFast;


    /// <summary>
    /// 粒子快速雪球在三级暴雪时的发射量
    /// </summary>
    static float PSSnowFast_RateOTime_L3 = 500;

    /// <summary>
    /// 粒子快速雪球
    /// </summary>
    public ParticleSystem PSSnowFast;








    private void Start()
    {
        //SetBlizzardLevel(2);
        //Timer.Start(this , 10.0f , ()=> { BlizzardOver(); });
        var clip = audioPlayer.sfxTable.GetClip(CetitanBlizzardSE.Blizzard.ToString());
        loopPlayer.PlayLoop(clip, 1f);
    }





    /// <summary>
    /// 伤害
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (ParentCetitan != null && isCanDmage && collision.gameObject.tag == "Player")
        {
            //判断距离
            float dist = Vector2.Distance(collision.gameObject.transform.position, transform.position);

            if (dist > currentDamageRadius)
            {
                PlayerControler player = collision.gameObject.GetComponent<PlayerControler>();
                Pokemon.PokemonHpChange(ParentCetitan.gameObject, collision.gameObject.gameObject, 0, BlizzardSpDmage, 0, PokemonType.TypeEnum.Ice);
                if (player != null)
                {
                    player.KnockOutPoint = BlizzardKOPoint;
                    player.KnockOutDirection = (this.transform.position - player.transform.position).normalized;
                    player.PlayerFrozenFloatPlus(BlizzardFrozenPoint, 2.0f);
                }
            }


        }
    }



    //================================暴风雪结束==================================


    public void BlizzardOver()
    {
        SkillRange.GetComponent<SkillRangeCircleManual>().SkillCircleOver();
        _mTool.RemoveAllPSChild(this.gameObject);
        isCanDmage = false;
        //var m1 = PSMistBlue.main;m1.simulationSpeed = 2.0f;
        //var m2 = PSMistWrite.main;m2.simulationSpeed = 2.0f;
        //var m3 = PSSnowTrail.main;m3.simulationSpeed = 2.0f;
        //var m4 = PSFrozenSlow.main;m4.simulationSpeed = 2.0f;
        //var m5 = PSFrozenFast.main;m5.simulationSpeed = 2.0f;
        //var m6 = PSSnowFast.main;m6.simulationSpeed = 2.0f;
        loopPlayer.StopLoop(5.0f);
    }


    //================================暴风雪结束==================================











    //================================等级设置==================================

    /// <summary>
    /// 设置暴风雪等级
    /// </summary>
    /// <param name="l"></param>
    public void SetBlizzardLevel(int l)
    {
        isCanDmage = true;
        //设置等级
        CetitanBlizzardLevel = l;
        //设置技能指示圈
        SmoothScaleSkillRange(Scale_SkillRange[CetitanBlizzardLevel]);

        //变化伤害半径
        SmoothDamageRadius(DamageRadius[CetitanBlizzardLevel]);


        //设置粒子效果
        SetParticle(PSMistBlue, CetitanBlizzardLevel, PSMistBlue_RateOTime_L3);
        SetParticle(PSMistWrite, CetitanBlizzardLevel, PSMistWrite_RateOTime_L3);
        SetParticle(PSSnowTrail, CetitanBlizzardLevel, PSSnowTrail_RateOTime_L3);
        SetParticle(PSFrozenSlow, CetitanBlizzardLevel, PSFrozenSlow_RateOTime_L3);
        SetParticle(PSFrozenFast, CetitanBlizzardLevel, PSFrozenFast_RateOTime_L3);
        SetParticle(PSSnowFast, CetitanBlizzardLevel, PSSnowFast_RateOTime_L3);

    }

    /// <summary>
    /// 设置粒子参数
    /// </summary>
    /// <param name="ps"></param>
    /// <param name="level"></param>
    /// <param name="baseRate"></param>
    private void SetParticle(ParticleSystem ps, int level, float baseRate)
    {
        if (ps == null) return;

        var s = ps.shape;
        s.radiusThickness = RadiusThickness_PS[level];

        var e = ps.emission;
        e.rateOverTime = (RadiusThickness_PS[level] / RadiusThickness_PS[2]) * baseRate;
    }


    //================================等级设置==================================















    //================================技能指示圈随时间变化==================================
    private Coroutine scaleRoutine;


    private void SmoothScaleSkillRange(float targetScale)
    {
        if (scaleRoutine != null)
            StopCoroutine(scaleRoutine);

        scaleRoutine = StartCoroutine(ScaleRoutine(targetScale));
    }

    private IEnumerator ScaleRoutine(float target)
    {
        float time = 0f;
        Vector3 start = SkillRange.transform.localScale;
        Vector3 end = new Vector3(target, target, 1);

        while (time < BlizzardScaleDuration)
        {
            float t = time / BlizzardScaleDuration;
            t = Mathf.SmoothStep(0, 1, t); // 更柔和的插值
            SkillRange.transform.localScale = Vector3.Lerp(start, end, t);

            time += Time.deltaTime;
            yield return null;
        }

        SkillRange.transform.localScale = end;
    }

    //================================技能指示圈随时间变化==================================















    //================================伤害半径设置==================================


    private Coroutine damageRoutine;

    private void SmoothDamageRadius(float target)
    {
        if (damageRoutine != null)
            StopCoroutine(damageRoutine);

        damageRoutine = StartCoroutine(DamageRadiusRoutine(target));
    }

    private IEnumerator DamageRadiusRoutine(float target)
    {
        float time = 0f;
        float start = currentDamageRadius;
        float end = target;

        while (time < BlizzardScaleDuration)
        {
            float t = time / BlizzardScaleDuration;
            t = Mathf.SmoothStep(0, 1, t);

            currentDamageRadius = Mathf.Lerp(start, end, t);

            time += Time.deltaTime;
            yield return null;
        }

        currentDamageRadius = end;
    }

    //================================伤害半径设置==================================











}
