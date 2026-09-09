using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Substitute : MonoBehaviour
{
    public PlayerControler ParentPlayer;
    public int MaxHP;
    public bool isDieBoom;
    public Skill DieBoom;
    [Tooltip("ui：血条")]
    public SubstituteHpBar uIHealth;
    int NowHp;
    bool isSubstituteInvincible;
    float InvincileTimer = 0.8f;
    Animator animator;


    //当前宝可梦处于青草场地中
    public bool isInGrassyTerrain
    {
        get { return (GrassyTerrainCount > 0); }
    }
    //bool isinGrassyTerrain = false;
    public int GrassyTerrainCount
    {
        get { return grassyTerrainCount; }
        set { grassyTerrainCount = value; }
    }
    int grassyTerrainCount = 0;



    //当前宝可梦处于精神场地中Psychic Terrain
    public bool isInPsychicTerrain
    {
        get { return (PsychicTerrainCount > 0); }
    }
    //bool isinPsychicTerrain = false;
    public int PsychicTerrainCount
    {
        get { return psychicTerrainCount; }
        set { psychicTerrainCount = value; }
    }
    int psychicTerrainCount = 0;



    //当前宝可梦处于电气场地中Electric Terrain
    public bool isInElectricTerrain
    {
        get { return (ElectricTerrainCount > 0); }
    }
    //bool isinElectricTerrain = false;
    public int ElectricTerrainCount
    {
        get { return electricTerrainCount; }
        set { electricTerrainCount = value; }
    }
    int electricTerrainCount = 0;



    //当前宝可梦处于薄雾场地中Misty Terrain
    public bool isInMistyTerrain
    {
        get { return (MistyTerrainCount > 0); }
    }
    //bool isinMistyTerrain = false;
    public int MistyTerrainCount
    {
        get { return mistyTerrainCount; }
        set { mistyTerrainCount = value; }
    }
    int mistyTerrainCount = 0;



    //当前宝可梦处超级于青草场地中
    public bool isInSuperGrassyTerrain
    {
        get { return (SuperGrassyTerrainCount > 0); }
    }
    bool isinSuperGrassyTerrain = false;
    public int SuperGrassyTerrainCount
    {
        get { return superGrassyTerrainCount; }
        set { superGrassyTerrainCount = value; }
    }
    int superGrassyTerrainCount = 0;



    //当前宝可梦处于超级精神场地中Psychic Terrain
    public bool isInSuperPsychicTerrain
    {
        get { return (SuperPsychicTerrainCount > 0); }
    }
    bool isinSuperPsychicTerrain = false;
    public int SuperPsychicTerrainCount
    {
        get { return superPsychicTerrainCount; }
        set { superPsychicTerrainCount = value; }
    }
    int superPsychicTerrainCount = 0;



    //当前宝可梦处于超级电气场地中Electric Terrain
    public bool isInSuperElectricTerrain
    {
        get { return (SuperElectricTerrainCount > 0); }
    }
    bool isinSuperElectricTerrain = false;
    public int SuperElectricTerrainCount
    {
        get { return superElectricTerrainCount; }
        set { superElectricTerrainCount = value; }
    }
    int superElectricTerrainCount = 0;



    //当前宝可梦处于超级薄雾场地中Misty Terrain
    public bool isInSuperMistyTerrain
    {
        get { return (SuperMistyTerrainCount > 0); }
    }
    bool isinSuperMistyTerrain = false;
    public int SuperMistyTerrainCount
    {
        get { return superMistyTerrainCount; }
        set { superMistyTerrainCount = value; }
    }
    int superMistyTerrainCount = 0;




    public void SetSubstitute(int maxHP,PlayerControler player)
    {
        MaxHP = maxHP;
        NowHp = maxHP;
        ParentPlayer = player;
        animator = GetComponent<Animator>();
        animator.SetFloat("LookX" , (Random.Range(0.0f,1.0f)>0.5f?-1:1));
        animator.SetFloat("LookY" , (Random.Range(0.0f,1.0f)>0.5f?-1:1));
    }

    public virtual void SubStituteChangeHp(float ChangePoint, float ChangePointSp, int SkillType)
    {
        if (NowHp > 0) {
            if (ChangePoint < 0 || ChangePointSp < 0)
            {
                
                PokemonType.TypeEnum enumVaue = (PokemonType.TypeEnum)SkillType;
                ChangePoint = ChangePoint * (((Weather.GlobalWeather.isRain && enumVaue == PokemonType.TypeEnum.Water) ? (Weather.GlobalWeather.isRainPlus ? 1.8f : 1.3f) : 1)
                    * ((Weather.GlobalWeather.isRain && enumVaue == PokemonType.TypeEnum.Fire) ? 0.5f : 1)
                    * ((Weather.GlobalWeather.isSunny && enumVaue == PokemonType.TypeEnum.Water) ? 0.5f : 1)
                    * ((Weather.GlobalWeather.isSunny && enumVaue == PokemonType.TypeEnum.Fire) ? (Weather.GlobalWeather.isSunnyPlus ? 1.8f : 1.3f) : 1));
                ChangePointSp = ChangePointSp * ((Weather.GlobalWeather.isRain && enumVaue == PokemonType.TypeEnum.Water) ? (Weather.GlobalWeather.isRainPlus ? 1.8f : 1.3f) : 1)
                    * ((Weather.GlobalWeather.isRain && enumVaue == PokemonType.TypeEnum.Fire) ? 0.5f : 1)
                    * ((Weather.GlobalWeather.isSunny && enumVaue == PokemonType.TypeEnum.Water) ? 0.5f : 1)
                    * ((Weather.GlobalWeather.isSunny && enumVaue == PokemonType.TypeEnum.Fire) ? (Weather.GlobalWeather.isSunnyPlus ? 1.8f : 1.3f) : 1);
                ChangePoint = ChangePoint * (ParentPlayer.isReflect ? 0.75f : 1);
                ChangePointSp = ChangePointSp * (ParentPlayer.isLightScreen ? 0.75f : 1);
                //如果无敌结束，不无敌的话变为不无敌状态，无敌时间计时器时间设置为无敌时间
                if (isSubstituteInvincible || ParentPlayer.isInvincibleAlways)
                {
                    return;
                }
                else
                {
                    //Debug.Log("Before" + "+" + NowHp);

                    if ((int)SkillType != 19)
                    {
                        if (!isInPsychicTerrain) {
                            NowHp = Mathf.Clamp(NowHp + (int)((ChangePoint / ParentPlayer.DefAbilityPoint + ChangePointSp / ParentPlayer.SpdAbilityPoint - 2) * (PokemonType.TYPE[(int)SkillType][ParentPlayer.PlayerType01] * PokemonType.TYPE[(int)SkillType][ParentPlayer.PlayerType02] * (ParentPlayer.PlayerTeraTypeJOR == 0 ? PokemonType.TYPE[(int)SkillType][ParentPlayer.PlayerTeraType] : PokemonType.TYPE[(int)SkillType][ParentPlayer.PlayerTeraTypeJOR])) * ((ParentPlayer.playerData.TypeDefAlways[(int)SkillType] + ParentPlayer.playerData.TypeDefJustOneRoom[(int)SkillType]) > 0 ? Mathf.Pow(1.2f, (ParentPlayer.playerData.TypeDefAlways[(int)SkillType] + ParentPlayer.playerData.TypeDefJustOneRoom[(int)SkillType])) : Mathf.Pow(0.8f, (ParentPlayer.playerData.TypeDefAlways[(int)SkillType] + ParentPlayer.playerData.TypeDefJustOneRoom[(int)SkillType])))), (NowHp > 1) ? (ParentPlayer.playerData.isEndure ? 1 : 0) : 0, MaxHP);
                        }
                        else
                        {
                            if ((int)((ChangePoint / ParentPlayer.DefAbilityPoint + ChangePointSp / ParentPlayer.SpdAbilityPoint - 2) * (PokemonType.TYPE[(int)SkillType][ParentPlayer.PlayerType01] * PokemonType.TYPE[(int)SkillType][ParentPlayer.PlayerType02] * (ParentPlayer.PlayerTeraTypeJOR == 0 ? PokemonType.TYPE[(int)SkillType][ParentPlayer.PlayerTeraType] : PokemonType.TYPE[(int)SkillType][ParentPlayer.PlayerTeraTypeJOR])) * ((ParentPlayer.playerData.TypeDefAlways[(int)SkillType] + ParentPlayer.playerData.TypeDefJustOneRoom[(int)SkillType]) > 0 ? Mathf.Pow(1.2f, (ParentPlayer.playerData.TypeDefAlways[(int)SkillType] + ParentPlayer.playerData.TypeDefJustOneRoom[(int)SkillType])) : Mathf.Pow(0.8f, (ParentPlayer.playerData.TypeDefAlways[(int)SkillType] + ParentPlayer.playerData.TypeDefJustOneRoom[(int)SkillType])))) > (int)(MaxHP / 10))
                            {
                                NowHp = Mathf.Clamp(NowHp + (int)((ChangePoint / ParentPlayer.DefAbilityPoint + ChangePointSp / ParentPlayer.SpdAbilityPoint - 2) * (PokemonType.TYPE[(int)SkillType][ParentPlayer.PlayerType01] * PokemonType.TYPE[(int)SkillType][ParentPlayer.PlayerType02] * (ParentPlayer.PlayerTeraTypeJOR == 0 ? PokemonType.TYPE[(int)SkillType][ParentPlayer.PlayerTeraType] : PokemonType.TYPE[(int)SkillType][ParentPlayer.PlayerTeraTypeJOR])) * ((ParentPlayer.playerData.TypeDefAlways[(int)SkillType] + ParentPlayer.playerData.TypeDefJustOneRoom[(int)SkillType]) > 0 ? Mathf.Pow(1.2f, (ParentPlayer.playerData.TypeDefAlways[(int)SkillType] + ParentPlayer.playerData.TypeDefJustOneRoom[(int)SkillType])) : Mathf.Pow(0.8f, (ParentPlayer.playerData.TypeDefAlways[(int)SkillType] + ParentPlayer.playerData.TypeDefJustOneRoom[(int)SkillType])))), (NowHp > 1) ? (ParentPlayer.playerData.isEndure ? 1 : 0) : 0, MaxHP);
                            }
                        }
                    }
                    else
                    {
                        
                        if (!isInPsychicTerrain)
                        {
                            NowHp = Mathf.Clamp(NowHp + Mathf.Clamp((int)ChangePoint, -100000, -1), (NowHp > 1) ? (ParentPlayer.playerData.isEndure ? 1 : 0) : 0, MaxHP);
                        }
                        else
                        {
                            if ((int)Mathf.Clamp((int)ChangePoint, -100000, -1) > (int)(MaxHP / 10))
                            {
                                NowHp = Mathf.Clamp(NowHp + Mathf.Clamp((int)ChangePoint, -100000, -1), (NowHp > 1) ? (ParentPlayer.playerData.isEndure ? 1 : 0) : 0, MaxHP);
                            }
                        }
                    }
                    uIHealth.Per = (float)NowHp / (float)MaxHP;
                    uIHealth.ChangeHpDown();
                    isSubstituteInvincible = true;

                    //Debug.Log("After" + "+" + NowHp);

                    if (NowHp <= 0) {
                        animator.SetTrigger("Die");
                        if (isDieBoom)
                        {
                            Instantiate(DieBoom, transform.position, Quaternion.identity).player = ParentPlayer;
                        }
                    }
                    //血量上升时对血条UI输出当前血量，并调用血条上升的函数

                    //输出被击打的动画管理器参数
                    animator.SetTrigger("Hit");
                }
            }
        }
    }

    private void Update()
    {
        if (isSubstituteInvincible)
        {
            InvincileTimer -= Time.deltaTime;
            /*
            //在无敌时间计时器运行的前0。15秒内被击退
            if (InvincileTimer > 0.8f - 0.15f)
            {

                Vector2 position = rigidbody2D.position;
                position.x = Mathf.Clamp(position.x + koDirection.x * 2.2f * konckout * Time.deltaTime, NowRoom.x * 30 - 12, NowRoom.x * 30 + 12);
                position.y = Mathf.Clamp(position.y + koDirection.y * 2.2f * konckout * Time.deltaTime, NowRoom.y * 24 - 7.3f, NowRoom.y * 24 + 7.3f);
                rigidbody2D.position = position;
            }
            */
            if (InvincileTimer <= 0)
            {
                isSubstituteInvincible = false;
                InvincileTimer = 0.8f;
            }
        }
    }

    // Start is called before the first frame update
    public void DestorySelf()
    {
        Destroy(gameObject);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if(other.tag == "Empty")
        {
            Empty e = other.GetComponent<Empty>();
            if(e != null && !e.isSubsititue)
            {
                e.isSubsititue = true;
                e.SubsititueTarget = this.gameObject;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Empty")
        {
            Empty e = other.GetComponent<Empty>();
            if (e != null && e.isSubsititue)
            {
                e.isSubsititue = false;
                e.SubsititueTarget = null;
                Snorlax snorlax = e.GetComponent<Snorlax>();
                if (snorlax != null && snorlax.isSlam)
                {
                    e.isSubsititue = true;
                    e.SubsititueTarget = this.gameObject;
                }
            }
        }
    }
}
