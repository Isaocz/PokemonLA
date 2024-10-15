using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PsyshockSE : Skill
{
    public float boomTime;
    void Start()
    {
        Destroy(gameObject, 1.2f);
        Invoke("HitEnemy", boomTime);
    }

    void HitEnemy()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 1.3f);
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Empty"))
            {
                Empty enemy = collider.GetComponent<Empty>();
                if (enemy != null)
                {
                    HitAndKo(enemy);
                }
            }
        }
    }

    public void PsychockHitAndKo(Empty target)
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
                if (TargetList[i].Target == target) { isTargetExitInList = true; TCEell = TargetList[i]; ListIndex = i; /* Debug.Log("xxx" + TargetList[i].isMultipleDamageColdDown); */ break; }
            }
            if (!isTargetExitInList)
            {
                TargetList.Add(TCEell);
            }
        }




        if (isMultipleDamage && !TCEell.isMultipleDamageColdDown)
        {
            int BeforeHP = target.EmptyHp;

            float WeatherAlpha = ((Weather.GlobalWeather.isRain && SkillType == 11) ? (Weather.GlobalWeather.isRainPlus ? 1.8f : 1.3f) : 1) * ((Weather.GlobalWeather.isRain && SkillType == 10) ? 0.5f : 1) * ((Weather.GlobalWeather.isSunny && SkillType == 11) ? 0.5f : 1) * ((Weather.GlobalWeather.isSunny && SkillType == 10) ? (Weather.GlobalWeather.isSunnyPlus ? 1.8f : 1.3f) : 1);

            if (player != null)
            {
                if (Random.Range(0.0f, 1.0f) >= 0.04f * Mathf.Pow(2, CTLevel) + 0.01f * player.LuckPoint)
                {
                    target.EmptyHpChange(
                        (SpDamage * (SkillType == player.PlayerType01 ? 1.5f : 1) * (SkillType == player.PlayerType02 ? 1.5f : 1) * (player.PlayerTeraTypeJOR == 0 ? (SkillType == player.PlayerTeraType ? 1.5f : 1) : (SkillType == player.PlayerTeraTypeJOR ? 1.5f : 1)) * (2 * player.Level + 10) * player.SpAAbilityPoint) / (250 * target.DefAbilityPoint * ((Weather.GlobalWeather.isSandstorm ? ((target.EmptyType01 == PokemonType.TypeEnum.Rock || target.EmptyType02 == PokemonType.TypeEnum.Rock) ? 1.5f : 1) : 1))) + 2, 
                        0,
                        SkillType);
                }
                else
                {
                    target.EmptyHpChange((SpDamage * (SkillType == player.PlayerType01 ? 1.5f : 1) * (SkillType == player.PlayerType02 ? 1.5f : 1) * (player.PlayerTeraTypeJOR == 0 ? (SkillType == player.PlayerTeraType ? 1.5f : 1) : (SkillType == player.PlayerTeraTypeJOR ? 1.5f : 1)) * 1.5f * (2 * player.Level + 10) * player.SpAAbilityPoint) / (250 * target.DefAbilityPoint * ((Weather.GlobalWeather.isSandstorm ? ((target.EmptyType01 == PokemonType.TypeEnum.Rock || target.EmptyType02 == PokemonType.TypeEnum.Rock) ? 1.5f : 1) : 1))) + 2, 0, SkillType);
                    GetCTEffect(target);
                }
            }
            else if (baby != null)
            {
                Pokemon.PokemonHpChange(baby.gameObject, target.gameObject, Damage, 0, 0, (PokemonType.TypeEnum)SkillType);
                Debug.Log(baby);
            }
            target.EmptyKnockOut(KOPoint);

            if (isMultipleDamage)
            {
                TCEell.isMultipleDamageColdDown = true;
                TargetList[ListIndex] = TCEell;
            }
            HitEvent(target);

            //µ¿æﬂ136 ±¥ø«¡Â
            if (player.playerData.IsPassiveGetList[136])
            {
                Drain(BeforeHP, target.EmptyHp, 0.1f);
            }
        }

    }





}
