using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weather : MonoBehaviour
{
    public static Weather GlobalWeather;

    public enum WeatherEnum
    {
        No,
        Rain,
        Sunny,
        Hail,
        SandStorm
    }

    public PlayerControler player;
    bool isPlayerDefUP;
    bool isPlayerSpDUP;

    SpriteRenderer WeatherMaskColor;
    ParticleSystem RainPS;
    ParticleSystem SunShinePS;
    ParticleSystem HailPS1;
    ParticleSystem HailPS2;
    ParticleSystem SandstormPS;

    bool isNormalColorStartChange;
    bool isRainColorStartChange;
    bool isSunColorStartChange;
    bool isHailColorStartChange;
    bool isSandstormColorStartChange;

    public bool isNormal;
    public bool isRain;
    public bool isRainPlus;
    public bool isSunny;
    public bool isSunnyPlus;
    public bool isHail;
    public bool isHailPlus;
    public bool isSandstorm;
    public bool isSandstormPlus;

    Color NormalColor;
    Color RainColor;
    Color SunColor;
    Color HailColor;
    Color SandStormColor;

    public float WeatherTimer;


    private void Awake()
    {
        GlobalWeather = this;

        WeatherMaskColor = transform.GetChild(0).GetComponent<SpriteRenderer>();
        RainPS = transform.GetChild(1).GetComponent<ParticleSystem>();
        SunShinePS = transform.GetChild(2).GetComponent<ParticleSystem>();
        HailPS1 = transform.GetChild(3).GetChild(0).GetComponent<ParticleSystem>();
        HailPS2 = transform.GetChild(3).GetChild(1).GetComponent<ParticleSystem>();
        SandstormPS = transform.GetChild(4).GetComponent<ParticleSystem>();

        NormalColor = new Color(1, 1, 1, 0);
        RainColor = new Color(0.8246584f, 0.7688679f, 1, 0.1490196f);
        SunColor = new Color(1, 0.9974519f, 0.8632076f, 0.1490196f);
        HailColor = new Color(0.6084906f, 0.9791504f, 1, 0.2f);
        SandStormColor = new Color(0.6226415f, 0.6002588f, 0.2261481f, 0.28f);

        player = FindObjectOfType<PlayerControler>();
    }

    bool[] WeatherBool = new bool[4] { false, false, false, false};

    void InPC()
    {
        if (player.NowRoom == MapCreater.StaticMap.PCRoomPoint || player.NowRoom == MapCreater.StaticMap.StoreRoomPoint) {
            transform.GetChild(0).gameObject.SetActive(false);
            for (int i = 1; i < 5; i++)
            {
                WeatherBool[i - 1] = transform.GetChild(i).gameObject.activeInHierarchy;
                if (i == 3) { transform.GetChild(i).GetChild(0).gameObject.SetActive(false); transform.GetChild(i).GetChild(1).gameObject.SetActive(false); }
                else { transform.GetChild(i).gameObject.SetActive(false); }

            }
        }
    }

    private void Update()
    {
        CheckPlayer();

        if (player != null) {
            //进入商店PC隐藏天气效果；
            if (player.NowRoom == MapCreater.StaticMap.PCRoomPoint || player.NowRoom == MapCreater.StaticMap.StoreRoomPoint)
            {
                if (transform.GetChild(0).gameObject.activeInHierarchy)
                {
                    InPC();
                }
            }
            else {
                if (!transform.GetChild(0).gameObject.activeInHierarchy)
                {
                    transform.GetChild(0).gameObject.SetActive(true);
                    for (int i = 1; i < 5; i++)
                    {
                        if (WeatherBool[i - 1])
                        {
                            if (i == 3) { transform.GetChild(i).GetChild(0).gameObject.SetActive(true); transform.GetChild(i).GetChild(1).gameObject.SetActive(true); }
                            else { transform.GetChild(i).gameObject.SetActive(true); }
                        }
                    }

                    if (!isSunny && !isSunnyPlus) { SunShinePS.gameObject.SetActive(false); }
                    if (!isRain && !isRainPlus) { RainPS.Stop(); }
                    if (!isHail && !isHailPlus) { HailPS1.Stop(); HailPS2.Stop(); }
                    if (!isSandstorm && !isSandstormPlus) { SandstormPS.Stop(); }
                    if (isNormal) { RemoveAllWeather(); }
                }
            }

            if (!isNormal && WeatherTimer > 0)
            {
                WeatherTimer -= Time.deltaTime;
                if (WeatherTimer <= 0) { WeatherTimer = 0; ChangeWeatherNormal(); }
            }

            //=================================================改变天气时的滤镜颜色变化===============================================

            if (isRainColorStartChange)
            {
                WeatherMaskColor.color = new Color(
                    Mathf.Clamp(WeatherMaskColor.color.r + ((WeatherMaskColor.color.r > RainColor.r) ? -1 : 1) * 0.5f * Time.deltaTime, ((WeatherMaskColor.color.r > RainColor.r) ? RainColor.r : WeatherMaskColor.color.r), ((WeatherMaskColor.color.r > RainColor.r) ? WeatherMaskColor.color.r : RainColor.r)),
                    Mathf.Clamp(WeatherMaskColor.color.g + ((WeatherMaskColor.color.g > RainColor.g) ? -1 : 1) * 0.5f * Time.deltaTime, ((WeatherMaskColor.color.g > RainColor.g) ? RainColor.g : WeatherMaskColor.color.g), ((WeatherMaskColor.color.g > RainColor.g) ? WeatherMaskColor.color.g : RainColor.g)),
                    Mathf.Clamp(WeatherMaskColor.color.b + ((WeatherMaskColor.color.b > RainColor.b) ? -1 : 1) * 0.5f * Time.deltaTime, ((WeatherMaskColor.color.b > RainColor.b) ? RainColor.b : WeatherMaskColor.color.b), ((WeatherMaskColor.color.b > RainColor.b) ? WeatherMaskColor.color.b : RainColor.b)),
                    Mathf.Clamp(WeatherMaskColor.color.a + ((WeatherMaskColor.color.a > RainColor.a) ? -1 : 1) * 0.5f * Time.deltaTime, ((WeatherMaskColor.color.a > RainColor.a) ? RainColor.a : WeatherMaskColor.color.a), ((WeatherMaskColor.color.a > RainColor.a) ? WeatherMaskColor.color.a : RainColor.a)));
                if (Mathf.Abs(WeatherMaskColor.color.r - RainColor.r) <= 0.01f && Mathf.Abs(WeatherMaskColor.color.g - RainColor.g) <= 0.01f && Mathf.Abs(WeatherMaskColor.color.b - RainColor.b) <= 0.01f && Mathf.Abs(WeatherMaskColor.color.a - RainColor.a) <= 0.01f)
                {
                    isRainColorStartChange = false;
                }
            }

            if (isSunColorStartChange)
            {
                WeatherMaskColor.color = new Color(
                    Mathf.Clamp(WeatherMaskColor.color.r + ((WeatherMaskColor.color.r > SunColor.r) ? -1 : 1) * 0.5f * Time.deltaTime, ((WeatherMaskColor.color.r > SunColor.r) ? SunColor.r : WeatherMaskColor.color.r), ((WeatherMaskColor.color.r > SunColor.r) ? WeatherMaskColor.color.r : SunColor.r)),
                    Mathf.Clamp(WeatherMaskColor.color.g + ((WeatherMaskColor.color.g > SunColor.g) ? -1 : 1) * 0.5f * Time.deltaTime, ((WeatherMaskColor.color.g > SunColor.g) ? SunColor.g : WeatherMaskColor.color.g), ((WeatherMaskColor.color.g > SunColor.g) ? WeatherMaskColor.color.g : SunColor.g)),
                    Mathf.Clamp(WeatherMaskColor.color.b + ((WeatherMaskColor.color.b > SunColor.b) ? -1 : 1) * 0.5f * Time.deltaTime, ((WeatherMaskColor.color.b > SunColor.b) ? SunColor.b : WeatherMaskColor.color.b), ((WeatherMaskColor.color.b > SunColor.b) ? WeatherMaskColor.color.b : SunColor.b)),
                    Mathf.Clamp(WeatherMaskColor.color.a + ((WeatherMaskColor.color.a > SunColor.a) ? -1 : 1) * 0.5f * Time.deltaTime, ((WeatherMaskColor.color.a > SunColor.a) ? SunColor.a : WeatherMaskColor.color.a), ((WeatherMaskColor.color.a > SunColor.a) ? WeatherMaskColor.color.a : SunColor.a)));
                if (Mathf.Abs(WeatherMaskColor.color.r - SunColor.r) <= 0.01f && Mathf.Abs(WeatherMaskColor.color.g - SunColor.g) <= 0.01f && Mathf.Abs(WeatherMaskColor.color.b - SunColor.b) <= 0.01f && Mathf.Abs(WeatherMaskColor.color.a - SunColor.a) <= 0.01f)
                {
                    isSunColorStartChange = false;
                }
            }

            if (isHailColorStartChange)
            {
                WeatherMaskColor.color = new Color(
                    Mathf.Clamp(WeatherMaskColor.color.r + ((WeatherMaskColor.color.r > HailColor.r) ? -1 : 1) * 0.5f * Time.deltaTime, ((WeatherMaskColor.color.r > HailColor.r) ? HailColor.r : WeatherMaskColor.color.r), ((WeatherMaskColor.color.r > HailColor.r) ? WeatherMaskColor.color.r : HailColor.r)),
                    Mathf.Clamp(WeatherMaskColor.color.g + ((WeatherMaskColor.color.g > HailColor.g) ? -1 : 1) * 0.5f * Time.deltaTime, ((WeatherMaskColor.color.g > HailColor.g) ? HailColor.g : WeatherMaskColor.color.g), ((WeatherMaskColor.color.g > HailColor.g) ? WeatherMaskColor.color.g : HailColor.g)),
                    Mathf.Clamp(WeatherMaskColor.color.b + ((WeatherMaskColor.color.b > HailColor.b) ? -1 : 1) * 0.5f * Time.deltaTime, ((WeatherMaskColor.color.b > HailColor.b) ? HailColor.b : WeatherMaskColor.color.b), ((WeatherMaskColor.color.b > HailColor.b) ? WeatherMaskColor.color.b : HailColor.b)),
                    Mathf.Clamp(WeatherMaskColor.color.a + ((WeatherMaskColor.color.a > HailColor.a) ? -1 : 1) * 0.5f * Time.deltaTime, ((WeatherMaskColor.color.a > HailColor.a) ? HailColor.a : WeatherMaskColor.color.a), ((WeatherMaskColor.color.a > HailColor.a) ? WeatherMaskColor.color.a : HailColor.a)));
                if (Mathf.Abs(WeatherMaskColor.color.r - HailColor.r) <= 0.01f && Mathf.Abs(WeatherMaskColor.color.g - HailColor.g) <= 0.01f && Mathf.Abs(WeatherMaskColor.color.b - HailColor.b) <= 0.01f && Mathf.Abs(WeatherMaskColor.color.a - HailColor.a) <= 0.01f)
                {
                    isHailColorStartChange = false;
                }
            }

            if (isSandstormColorStartChange)
            {
                WeatherMaskColor.color = new Color(
                    Mathf.Clamp(WeatherMaskColor.color.r + ((WeatherMaskColor.color.r > SandStormColor.r) ? -1 : 1) * 0.5f * Time.deltaTime, ((WeatherMaskColor.color.r > SandStormColor.r) ? SandStormColor.r : WeatherMaskColor.color.r), ((WeatherMaskColor.color.r > SandStormColor.r) ? WeatherMaskColor.color.r : SandStormColor.r)),
                    Mathf.Clamp(WeatherMaskColor.color.g + ((WeatherMaskColor.color.g > SandStormColor.g) ? -1 : 1) * 0.5f * Time.deltaTime, ((WeatherMaskColor.color.g > SandStormColor.g) ? SandStormColor.g : WeatherMaskColor.color.g), ((WeatherMaskColor.color.g > SandStormColor.g) ? WeatherMaskColor.color.g : SandStormColor.g)),
                    Mathf.Clamp(WeatherMaskColor.color.b + ((WeatherMaskColor.color.b > SandStormColor.b) ? -1 : 1) * 0.5f * Time.deltaTime, ((WeatherMaskColor.color.b > SandStormColor.b) ? SandStormColor.b : WeatherMaskColor.color.b), ((WeatherMaskColor.color.b > SandStormColor.b) ? WeatherMaskColor.color.b : SandStormColor.b)),
                    Mathf.Clamp(WeatherMaskColor.color.a + ((WeatherMaskColor.color.a > SandStormColor.a) ? -1 : 1) * 0.5f * Time.deltaTime, ((WeatherMaskColor.color.a > SandStormColor.a) ? SandStormColor.a : WeatherMaskColor.color.a), ((WeatherMaskColor.color.a > SandStormColor.a) ? WeatherMaskColor.color.a : SandStormColor.a)));
                if (Mathf.Abs(WeatherMaskColor.color.r - SandStormColor.r) <= 0.01f && Mathf.Abs(WeatherMaskColor.color.g - SandStormColor.g) <= 0.01f && Mathf.Abs(WeatherMaskColor.color.b - SandStormColor.b) <= 0.01f && Mathf.Abs(WeatherMaskColor.color.a - SandStormColor.a) <= 0.01f)
                {
                    isSandstormColorStartChange = false;
                }
            }

            if (isNormalColorStartChange)
            {
                WeatherMaskColor.color = new Color(
                    Mathf.Clamp(WeatherMaskColor.color.r + ((WeatherMaskColor.color.r > NormalColor.r) ? -1 : 1) * 0.5f * Time.deltaTime, ((WeatherMaskColor.color.r > NormalColor.r) ? NormalColor.r : WeatherMaskColor.color.r), ((WeatherMaskColor.color.r > NormalColor.r) ? WeatherMaskColor.color.r : NormalColor.r)),
                    Mathf.Clamp(WeatherMaskColor.color.g + ((WeatherMaskColor.color.g > NormalColor.g) ? -1 : 1) * 0.5f * Time.deltaTime, ((WeatherMaskColor.color.g > NormalColor.g) ? NormalColor.g : WeatherMaskColor.color.g), ((WeatherMaskColor.color.g > NormalColor.g) ? WeatherMaskColor.color.g : NormalColor.g)),
                    Mathf.Clamp(WeatherMaskColor.color.b + ((WeatherMaskColor.color.b > NormalColor.b) ? -1 : 1) * 0.5f * Time.deltaTime, ((WeatherMaskColor.color.b > NormalColor.b) ? NormalColor.b : WeatherMaskColor.color.b), ((WeatherMaskColor.color.b > NormalColor.b) ? WeatherMaskColor.color.b : NormalColor.b)),
                    Mathf.Clamp(WeatherMaskColor.color.a + ((WeatherMaskColor.color.a > NormalColor.a) ? -1 : 1) * 0.5f * Time.deltaTime, ((WeatherMaskColor.color.a > NormalColor.a) ? NormalColor.a : WeatherMaskColor.color.a), ((WeatherMaskColor.color.a > NormalColor.a) ? WeatherMaskColor.color.a : NormalColor.a)));
                if (Mathf.Abs(WeatherMaskColor.color.r - NormalColor.r) <= 0.01f && Mathf.Abs(WeatherMaskColor.color.g - NormalColor.g) <= 0.01f && Mathf.Abs(WeatherMaskColor.color.b - NormalColor.b) <= 0.01f && Mathf.Abs(WeatherMaskColor.color.a - NormalColor.a) <= 0.01f)
                {
                    isNormalColorStartChange = false;
                }
            }

            //=================================================改变天气时的滤镜颜色变化===============================================


            //=================================================改变天气时的粒子变化===============================================

            if (RainPS.gameObject.activeInHierarchy && !RainPS.IsAlive()) { RainPS.gameObject.SetActive(false); }
            if (SunShinePS.gameObject.activeInHierarchy && !SunShinePS.IsAlive()) { SunShinePS.gameObject.SetActive(false); }
            if (HailPS1.gameObject.activeInHierarchy && !HailPS1.IsAlive()) { HailPS1.gameObject.SetActive(false); }
            if (HailPS2.gameObject.activeInHierarchy && !HailPS2.IsAlive()) { HailPS2.gameObject.SetActive(false); }
            if (SandstormPS.gameObject.activeInHierarchy && !SandstormPS.IsAlive()) { SandstormPS.gameObject.SetActive(false); }

            //=================================================改变天气时的粒子变化===============================================
        }
    }

    public void ChangeWeatherRain(float Time , bool isPlus)
    {
        if (!isRain || !isRainPlus) {
            RemoveAllWeather();
            isRain = true;
            if (isPlus) { isRainPlus = true; }
            isRainColorStartChange = true;
            RainPS.gameObject.SetActive(true);
            RainPS.Play();
            WeatherTimer = Time;
            ChangeWeatherSkillType();
        }
        InPC();
    }

    public void ChangeWeatherSunshine(float Time, bool isPlus)
    {
        if (!isSunny || !isSunnyPlus)
        {
            RemoveAllWeather();
            isSunny = true;
            if (isPlus) { isSunnyPlus = true; }
            isSunColorStartChange = true;
            SunShinePS.gameObject.SetActive(true);
            SunShinePS.Play();
            WeatherTimer = Time;
            ChangeWeatherSkillType();
        }
        InPC();
    }



    public void ChangeWeatherHail(float Time, bool isPlus)
    {
        if (!isHail || !isHailPlus)
        {
            RemoveAllWeather();
            isHail = true;
            if (isPlus) { isHailPlus = true; }
            isHailColorStartChange = true;
            HailPS1.gameObject.SetActive(true);
            HailPS2.gameObject.SetActive(true);
            HailPS1.Play();
            HailPS2.Play();
            if (!isPlayerDefUP && (player.PlayerType01 == 15 || player.PlayerType02 == 15 || player.PlayerTeraType == 15 || player.PlayerTeraTypeJOR == 15))
            {
                player.playerData.DefBounsAlways += 1;
                player.ReFreshAbllityPoint();
                isPlayerDefUP = true;
            }
            WeatherTimer = Time;
            ChangeWeatherSkillType();
        }
        InPC();
    }

    public void ChangeWeatherSandStorm(float Time, bool isPlus)
    {
        if (!isSandstorm || !isSandstormPlus)
        {
            RemoveAllWeather();
            isSandstorm = true;
            if (isPlus) { isSandstormPlus = true; }
            isSandstormColorStartChange = true;
            SandstormPS.gameObject.SetActive(true);
            SandstormPS.Play();
            WeatherTimer = Time;
            if (!isPlayerSpDUP && (player.PlayerType01 == 6 || player.PlayerType02 == 6 || player.PlayerTeraType == 6 || player.PlayerTeraTypeJOR == 6))
            {
                player.playerData.SpDBounsAlways += 1;
                player.ReFreshAbllityPoint();
                isPlayerSpDUP = true;
            }
            ChangeWeatherSkillType();
        }
        InPC();
    }

    void CheckPlayer()
    {
        if(player == null && FloorNum.GlobalFloorNum != null && FloorNum.GlobalFloorNum.FloorNumber >= 0)
        {
            player = FindObjectOfType<PlayerControler>();
        }
    }

    public void ChangeWeatherNormal()
    {
        RemoveAllWeather();
        isNormalColorStartChange = true;
        isNormal = true;
        ChangeWeatherSkillType();
        InPC();
    }

    void RemoveAllWeather()
    {
        CheckPlayer();
        WeatherTimer = 0;
        isRainColorStartChange = false;
        isSunColorStartChange = false;
        isHailColorStartChange = false;
        isSandstormColorStartChange = false;
        isNormal = false;
        isRain = false;
        isRainPlus = false;
        isSunny = false;
        isSunnyPlus = false;
        isHail = false;
        isHailPlus = false;
        if (isPlayerDefUP) { player.playerData.DefBounsAlways -= 1; player.ReFreshAbllityPoint(); isPlayerDefUP = false; }
        if (isPlayerSpDUP) { player.playerData.SpDBounsAlways -= 1; player.ReFreshAbllityPoint(); isPlayerSpDUP = false; }
        isSandstorm = false;
        isSandstormPlus = false;
        RainPS.Stop();
        SunShinePS.gameObject.SetActive(false);
        HailPS1.Stop();
        HailPS2.Stop();
        SandstormPS.Stop();
        ChangeWeatherSkillType();
    }




    //改变随天气改变属性的技能的属性
    void ChangeWeatherSkillType()
    {
        if (player == null) { CheckPlayer(); }
        //Debug.Log(player.Skill04.SkillIndex);
        if (player.Skill01 != null &&( player.Skill01.SkillIndex == 309 || player.Skill01.SkillIndex == 310))
        {
            if (isNormal) { player.Skill01.SkillType = 1; player.Skill01.SpDamage = 50; player.skillBar01.GetSkill(player.Skill01); }
            else if (!player.playerData.IsPassiveGetList[118] && (isSunny || isSunnyPlus)) { player.Skill01.SkillType = 10; player.Skill01.SpDamage = 100; player.skillBar01.GetSkill(player.Skill01); }
            else if (!player.playerData.IsPassiveGetList[118] && (isRain || isRainPlus)) { player.Skill01.SkillType = 11; player.Skill01.SpDamage = 100; player.skillBar01.GetSkill(player.Skill01); }
            else if (isHail || isHailPlus) { player.Skill01.SkillType = 15; player.Skill01.SpDamage = 100; player.skillBar01.GetSkill(player.Skill01); }
            else if (isSandstorm || isSandstormPlus) { player.Skill01.SkillType = 6; player.Skill01.SpDamage = 100; player.skillBar01.GetSkill(player.Skill01); }
        }
        else if (player.Skill02 != null &&( player.Skill02.SkillIndex == 309 || player.Skill02.SkillIndex == 310))
        {
            if (isNormal) { player.Skill02.SkillType = 1; player.Skill02.SpDamage = 50; player.skillBar02.GetSkill(player.Skill02); }
            else if (!player.playerData.IsPassiveGetList[118] && (isSunny || isSunnyPlus)) { player.Skill02.SkillType = 10; player.Skill02.SpDamage = 100; player.skillBar02.GetSkill(player.Skill02); }
            else if (!player.playerData.IsPassiveGetList[118] && (isRain || isRainPlus)) { player.Skill02.SkillType = 11; player.Skill02.SpDamage = 100; player.skillBar02.GetSkill(player.Skill02); }
            else if (isHail || isHailPlus) { player.Skill02.SkillType = 15; player.Skill02.SpDamage = 100; player.skillBar02.GetSkill(player.Skill02); }
            else if (isSandstorm || isSandstormPlus) { player.Skill02.SkillType = 6; player.Skill02.SpDamage = 100; player.skillBar02.GetSkill(player.Skill02); }
        }
        else if (player.Skill03 != null &&( player.Skill03.SkillIndex == 309 || player.Skill03.SkillIndex == 310))
        {
            if (isNormal) { player.Skill03.SkillType = 1; player.Skill03.SpDamage = 50; player.skillBar03.GetSkill(player.Skill03); }
            else if (!player.playerData.IsPassiveGetList[118] && (isSunny || isSunnyPlus)) { player.Skill03.SkillType = 10; player.Skill03.SpDamage = 100; player.skillBar03.GetSkill(player.Skill03); }
            else if (!player.playerData.IsPassiveGetList[118] && (isRain || isRainPlus)) { player.Skill03.SkillType = 11; player.Skill03.SpDamage = 100; player.skillBar03.GetSkill(player.Skill03); }
            else if (isHail || isHailPlus) { player.Skill03.SkillType = 15; player.Skill03.SpDamage = 100; player.skillBar03.GetSkill(player.Skill03); }
            else if (isSandstorm || isSandstormPlus) { player.Skill03.SkillType = 6; player.Skill03.SpDamage = 100; player.skillBar03.GetSkill(player.Skill03); }
        }
        else if (player.Skill04 != null &&( player.Skill04.SkillIndex == 309 || player.Skill04.SkillIndex == 310))
        {
            if (isNormal) { player.Skill04.SkillType = 1; player.Skill04.SpDamage = 50; player.skillBar04.GetSkill(player.Skill04); }
            else if (!player.playerData.IsPassiveGetList[118] && (isSunny || isSunnyPlus)) { player.Skill04.SkillType = 10; player.Skill04.SpDamage = 100; player.skillBar04.GetSkill(player.Skill04); }
            else if (!player.playerData.IsPassiveGetList[118] && (isRain || isRainPlus)) { player.Skill04.SkillType = 11; player.Skill04.SpDamage = 100; player.skillBar04.GetSkill(player.Skill04); }
            else if (isHail || isHailPlus) { player.Skill04.SkillType = 15; player.Skill04.SpDamage = 100; player.skillBar04.GetSkill(player.Skill04); }
            else if (isSandstorm || isSandstormPlus) { player.Skill04.SkillType = 6; player.Skill04.SpDamage = 100; player.skillBar04.GetSkill(player.Skill04); }
        }

    }

}
