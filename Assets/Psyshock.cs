using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEditor.PackageManager;
using UnityEngine;

public class Psyshock : Skill
{
    private LineRenderer lineRenderer;
    private bool isSummon;
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
                if (nearestEnemy != null)//甚至扇形范围内没有敌怪
                {
                    lineRenderer.SetPosition(0, transform.position);
                    lineRenderer.SetPosition(1, hitPosition);
                    summonPoint = hitPosition;
                }
                else
                {
                    lineRenderer.SetPosition(0, transform.position);
                    lineRenderer.SetPosition(1, transform.position + transform.right * 10);
                    summonPoint = transform.position + transform.right * 10;
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
        RaycastHit2D hitinfo = Physics2D.Raycast(transform.position, transform.right, 10, LayerMask.GetMask("Empty", "EmptyFly", "Enviroment", "Room"));
        Vector2 EndPoint = hitinfo.point;
        //如果击中敌方宝可梦，则造成伤害
        if (hitinfo)
        {
            if (hitinfo.collider != null && hitinfo.collider.gameObject.tag == "Empty")
            {
                Empty target = hitinfo.collider.GetComponent<Empty>();
                HitAndKo(target);
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
        GameObject psyshockSE = Instantiate(PsyshockSE, summonPoint, Quaternion.identity);
        psyshockSE.GetComponent<PsyshockSE>().player = player;
    }
}
