using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class Psyshock : Skill
{
    private LineRenderer lineRenderer;
    private bool isSummon;
    bool isPSHitDone;
    private Vector3 summonPoint;

    public GameObject PsyshockSE;

    void Start()
    {
        isSummon = false;
        lineRenderer = GetComponentInChildren<LineRenderer>();
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        StartExistenceTimer();

        if(SkillFrom == 2)
        {//追踪效果//
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 10f, LayerMask.GetMask("Empty", "EmptyFly"));
            float nearestDistance = Mathf.Infinity;
            Transform nearestEnemy = null;
            Vector3 hitPosition = Vector3.zero;
            List<Transform> Targets = new List<Transform>();

            foreach (Collider2D collider in colliders)
            {
                Vector2 direction = (collider.transform.position - transform.position).normalized;
                float angle = Vector2.Angle(transform.right, direction);
                if (angle < 30f) // 扇形范围为60度
                {
                    float distance = Vector3.Distance(transform.position, collider.transform.position);
                    RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, distance, LayerMask.GetMask("Enviroment", "Room"));
                    if (!hit)
                    {
                        Targets.Add(collider.transform);//把扇形范围内的敌人放进Targets里
                    }
                }
            }

            if (Targets.Count > 0) // 如果存在合适的敌人
            {
                foreach (Transform target in Targets)
                {
                    float distance = Vector3.Distance(transform.position, target.transform.position);
                    if (distance < nearestDistance)
                    {
                        nearestDistance = distance;
                        nearestEnemy = target;
                    }
                }
                if (nearestEnemy != null)
                {
                    Empty target = nearestEnemy.GetComponent<Empty>();
                    HitAndKo(target);
                    isPSHitDone = true;
                    lineRenderer.SetPosition(0, transform.position);
                    lineRenderer.SetPosition(1, target.transform.position);
                    summonPoint = target.transform.position;
                }
            }
            else
            {//扇形范围内没有合适的敌怪，选择扇形范围内最近的敌怪，并将开始点和结束点分别定义
                foreach (Collider2D collider in colliders)
                {
                    Vector2 direction = (collider.transform.position - transform.position).normalized;
                    float angle = Vector2.Angle(transform.right, direction);
                    if (angle < 30f)
                    {
                        float distance = Vector3.Distance(transform.position, collider.transform.position);
                        if (distance < nearestDistance)
                        {
                            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, distance, LayerMask.GetMask("Enviroment", "Room"));
                            
                            nearestDistance = distance;
                            nearestEnemy = collider.transform;
                            hitPosition = hit.point;
                        }
                    }
                }
                if (nearestEnemy != null)
                {//扇形范围内有敌怪（被阻碍），则对应障碍物
                    lineRenderer.SetPosition(0, transform.position);
                    lineRenderer.SetPosition(1, hitPosition);
                    summonPoint = hitPosition;
                }
                else
                {//扇形范围内没有敌怪，但是直线距离可能有障碍物
                    RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, 10f, LayerMask.GetMask("Enviroment", "Room"));
                    if (hit)
                    {
                        lineRenderer.SetPosition(0, transform.position);
                        lineRenderer.SetPosition(1, hit.point);
                        summonPoint = hit.point;
                    }
                    else
                    {
                        lineRenderer.SetPosition(0, transform.position);
                        lineRenderer.SetPosition(1, transform.position + transform.right * 10);
                        summonPoint = transform.position + transform.right * 10;
                    }
                }
            }
            
        }
        if (SkillFrom != 2)
        {
            Skill0();
        }

        if (!isSummon)
        {
            summonPsyshockSE();
            isSummon = true;
        }

    }

    void Skill0()
    {//激光效果
        RaycastHit2D hitinfo = Physics2D.Raycast(transform.position, transform.right, 10, LayerMask.GetMask("Empty", "EmptyFly", "EmptyJump", "Enviroment", "Room"));
        Vector2 EndPoint = hitinfo.point;
        //如果击中敌方宝可梦，则造成伤害
        if (hitinfo)
        {
            if (hitinfo.collider != null && hitinfo.collider.gameObject.tag == "Empty")
            {
                Empty target = hitinfo.collider.GetComponent<Empty>();
                HitAndKo(target);
                isPSHitDone = true;
            }
            //如果有击中对象，将起始点和终点分别对应
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, EndPoint);
            summonPoint = hitinfo.point;
        }
        else
        {
            //否则正常显示
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, transform.position + transform.right * 10);
            summonPoint = transform.position + transform.right * 10;
        }
    }

    void summonPsyshockSE()
    {
        if (isPSHitDone) {
            GameObject psyshockSE = Instantiate(PsyshockSE, summonPoint, Quaternion.identity);
            psyshockSE.GetComponent<PsyshockSE>().player = player;
            psyshockSE.GetComponent<PsyshockSE>().SpDamage = SpDamage / 2;
        }
    }




    public void PsychockHitAndKo(Empty target)
    {
        BeforeHitEvent(target);
        isPSHitDone = true;
        if (!isHitDone)
        {

            int BeforeHP = target.EmptyHp;
            isHitDone = true;
            float WeatherAlpha = ((Weather.GlobalWeather.isRain && SkillType == 11) ? (Weather.GlobalWeather.isRainPlus ? 1.8f : 1.3f) : 1) * ((Weather.GlobalWeather.isRain && SkillType == 10) ? 0.5f : 1) * ((Weather.GlobalWeather.isSunny && SkillType == 11) ? 0.5f : 1) * ((Weather.GlobalWeather.isSunny && SkillType == 10) ? (Weather.GlobalWeather.isSunnyPlus ? 1.8f : 1.3f) : 1);

            if (player != null)
            {
                if (Random.Range(0.0f, 1.0f) >= 0.04f * Mathf.Pow(2, CTLevel) + 0.01f * player.LuckPoint)
                {
                    target.EmptyHpChange((SpDamage * (SkillType == player.PlayerType01 ? 1.5f : 1) * (SkillType == player.PlayerType02 ? 1.5f : 1) * (player.PlayerTeraTypeJOR == 0 ? (SkillType == player.PlayerTeraType ? 1.5f : 1) : (SkillType == player.PlayerTeraTypeJOR ? 1.5f : 1)) * (2 * player.Level + 10) * player.SpAAbilityPoint) / (250 * target.DefAbilityPoint * ((Weather.GlobalWeather.isSandstorm ? ((target.EmptyType01 == PokemonType.TypeEnum.Rock || target.EmptyType02 == PokemonType.TypeEnum.Rock) ? 1.5f : 1) : 1))) + 2, 0, SkillType);
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
            HitEvent(target);

            //道具136 贝壳铃
            if (player.playerData.IsPassiveGetList[136])
            {
                Drain(BeforeHP, target.EmptyHp, 0.1f);
            }
        }

    }












}



