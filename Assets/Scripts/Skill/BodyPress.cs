using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyPress : Skill
{
    GameObject BSColloder;
    GameObject PlayerSpriteParent;
    Vector3 StartScale;
    Vector3 StartPosition;

    // Start is called before the first frame update
    void Start()
    {
        PlayerSpriteParent = player.transform.GetChild(3).gameObject;
        StartScale = PlayerSpriteParent.transform.localScale;
        StartPosition = PlayerSpriteParent.transform.localPosition;
        BSColloder = transform.GetChild(2).gameObject;
    }

    void ResetPlayer()
    {
        if ((player == null && baby == null) || PlayerSpriteParent == null)
        {
            player = GameObject.FindObjectOfType<PlayerControler>();
            PlayerSpriteParent = player.transform.GetChild(3).gameObject;
            StartScale = PlayerSpriteParent.transform.localScale;
            StartPosition = PlayerSpriteParent.transform.localPosition;
        }
    }

    // Update is called once per frame
    void Update()
    {
        StartExistenceTimer();
        ResetPlayer();

        if (ExistenceTime > 1.4f)
        {
            PlayerSpriteParent.transform.localScale = new Vector3(PlayerSpriteParent.transform.localScale.x , Mathf.Clamp(PlayerSpriteParent.transform.localScale.y - Time.deltaTime * 2  , 0.8f , 1), PlayerSpriteParent.transform.localScale.z);
        }
        else if (ExistenceTime <= 1.4f && ExistenceTime > 1f)
        {
            PlayerSpriteParent.transform.localScale = new Vector3(Mathf.Clamp(PlayerSpriteParent.transform.localScale.x - Time.deltaTime * 0.5f , 0.8f , 1.0f ), Mathf.Clamp(PlayerSpriteParent.transform.localScale.y + Time.deltaTime * 1f, 0.8f, 1.2f), PlayerSpriteParent.transform.localScale.z);
        }
        else if (ExistenceTime <= 1f && ExistenceTime > 0.8f)
        {
            player.isInvincibleAlways = true;
            PlayerSpriteParent.transform.localScale = new Vector3(Mathf.Clamp(PlayerSpriteParent.transform.localScale.x + Time.deltaTime * 1f, 0.8f, 1.0f), Mathf.Clamp(PlayerSpriteParent.transform.localScale.y - Time.deltaTime * 1f, 1.0f, 1.2f), PlayerSpriteParent.transform.localScale.z);
            PlayerSpriteParent.transform.localPosition = new Vector3(PlayerSpriteParent.transform.localPosition.x, Mathf.Clamp(PlayerSpriteParent.transform.localPosition.y + Time.deltaTime * 8, 0.0f, 1.6f), PlayerSpriteParent.transform.localPosition.z);
        }
        else if (ExistenceTime <= 0.8f && ExistenceTime > 0.65f)
        {
            PlayerSpriteParent.transform.localPosition = new Vector3(PlayerSpriteParent.transform.localPosition.x, Mathf.Clamp(PlayerSpriteParent.transform.localPosition.y - Time.deltaTime * 8, 0.4f, 1.6f), PlayerSpriteParent.transform.localPosition.z);
        }
        else if (ExistenceTime <= 0.65f && ExistenceTime > 0.6f)
        {
            player.isCanNotMove = true;
            transform.GetChild(0).gameObject.SetActive(true);
            BSColloder.SetActive(true);
            PlayerSpriteParent.transform.localPosition = new Vector3(PlayerSpriteParent.transform.localPosition.x, Mathf.Clamp(PlayerSpriteParent.transform.localPosition.y - Time.deltaTime * 8, 0.0f, 0.4f), PlayerSpriteParent.transform.localPosition.z);
            PlayerSpriteParent.transform.localScale = new Vector3(Mathf.Clamp(PlayerSpriteParent.transform.localScale.x + Time.deltaTime * 4f, 1.0f, 1.2f), Mathf.Clamp(PlayerSpriteParent.transform.localScale.y - Time.deltaTime * 8f, 0.6f, 1.0f), PlayerSpriteParent.transform.localScale.z);
        }
        else if (ExistenceTime <= 0.6f && ExistenceTime > 0.4f)
        {
            player.isInvincibleAlways = false;
            PlayerSpriteParent.transform.localScale = new Vector3(Mathf.Clamp(PlayerSpriteParent.transform.localScale.x - Time.deltaTime * 1f, 1.0f, 1.2f), Mathf.Clamp(PlayerSpriteParent.transform.localScale.y + Time.deltaTime * 2f, 0.6f, 1.0f), PlayerSpriteParent.transform.localScale.z);
        }
        else if (ExistenceTime <= 0.2f)
        {
            player.isCanNotMove = false;
        }
    }


    private void OnDestroy()
    {
        ResetPlayer();
        PlayerSpriteParent.transform.localScale = new Vector3(1, 1, 1);
        PlayerSpriteParent.transform.localPosition = Vector3.zero;
        player.isCanNotMove = false;
        player.isInvincibleAlways = false;
    }




    public void BodyPressHitAndKo(Empty target)
    {
        EmptyList TCEell = new EmptyList(target, false, 0.0f);
        int ListIndex = 0;

        if (isMultipleDamage)
        {
            bool isTargetExitInList = false;
            if (TargetList.Count == 0) { TargetList.Add(new EmptyList(target, false, 0.0f)); }
            for (int i = 0; i < TargetList.Count; i++)
            {
                if (TargetList[i].Target == target) { isTargetExitInList = true; TCEell = TargetList[i]; ListIndex = i; /* Debug.Log("xxx" + TargetList[i].isMultipleDamageColdDown); */ break; }
            }
            if (!isTargetExitInList)
            {
                TargetList.Add(TCEell);
            }
        }




        if (isMultipleDamage && !TCEell.isMultipleDamageColdDown)
        {
            if (SpDamage == 0)
            {
                float WeatherAlpha = ((Weather.GlobalWeather.isRain && SkillType == 11) ? (Weather.GlobalWeather.isRainPlus ? 1.8f : 1.3f) : 1) * ((Weather.GlobalWeather.isRain && SkillType == 10) ? 0.5f : 1) * ((Weather.GlobalWeather.isSunny && SkillType == 11) ? 0.5f : 1) * ((Weather.GlobalWeather.isSunny && SkillType == 10) ? (Weather.GlobalWeather.isSunnyPlus ? 1.8f : 1.3f) : 1);

                if (player != null)
                {
                    if (Random.Range(0.0f, 1.0f) >= 0.04f * Mathf.Pow(2, CTLevel) + 0.01f * player.LuckPoint)
                    {
                        target.EmptyHpChange((Damage * (SkillType == player.PlayerType01 ? 1.5f : 1) * (SkillType == player.PlayerType02 ? 1.5f : 1) * (player.PlayerTeraTypeJOR == 0 ? (SkillType == player.PlayerTeraType ? 1.5f : 1) : (SkillType == player.PlayerTeraTypeJOR ? 1.5f : 1)) * (2 * player.Level + 10) * player.DefAbilityPoint) / (250 * target.DefAbilityPoint * ((Weather.GlobalWeather.isSandstorm ? ((target.EmptyType01 == Type.TypeEnum.Rock || target.EmptyType02 == Type.TypeEnum.Rock) ? 1.5f : 1) : 1))) + 2, 0, SkillType);

                    }
                    else
                    {
                        target.EmptyHpChange((Damage * (SkillType == player.PlayerType01 ? 1.5f : 1) * (SkillType == player.PlayerType02 ? 1.5f : 1) * (player.PlayerTeraTypeJOR == 0 ? (SkillType == player.PlayerTeraType ? 1.5f : 1) : (SkillType == player.PlayerTeraTypeJOR ? 1.5f : 1)) * 1.5f * (2 * player.Level + 10) * player.DefAbilityPoint) / (250 * target.DefAbilityPoint * ((Weather.GlobalWeather.isSandstorm ? ((target.EmptyType01 == Type.TypeEnum.Rock || target.EmptyType02 == Type.TypeEnum.Rock) ? 1.5f : 1) : 1))) + 2, 0, SkillType);
                        GetCTEffect(target);
                        if (SkillFrom == 2 && player.playerData.DefBounsJustOneRoom <= 8) { player.playerData.DefBounsJustOneRoom++; player.ReFreshAbllityPoint(); }
                    }
                }
                else if (baby != null)
                {
                    Pokemon.PokemonHpChange(baby.gameObject, target.gameObject, Damage, 0, 0, (Type.TypeEnum)SkillType);
                    Debug.Log(baby);
                }
            }
            target.EmptyKnockOut(KOPoint);

            if (isMultipleDamage)
            {
                TCEell.isMultipleDamageColdDown = true;
                TargetList[ListIndex] = TCEell;
            }
            HitEvent(target);
        }

    }


}
