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
        {//׷��Ч��//
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 10f, LayerMask.GetMask("Empty", "EmptyFly"));
            float nearestDistance = Mathf.Infinity;
            Transform nearestEnemy = null;
            foreach (Collider2D collider in colliders)
            {
                Vector2 direction = (collider.transform.position - transform.position).normalized;
                float angle = Vector2.Angle(transform.right, direction);
                if (angle < 30f) // ���η�ΧΪ60��
                {
                    float distance = Vector3.Distance(transform.position, collider.transform.position);
                    if (distance < nearestDistance)
                    {
                        nearestDistance = distance;
                        nearestEnemy = collider.transform;
                    }
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
            else
            {
                Skill0();
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
    {//����Ч��
        RaycastHit2D hitinfo = Physics2D.Raycast(transform.position, transform.right, 8, LayerMask.GetMask("Empty", "EmptyFly", "Enviroment", "Room"));
        Vector2 EndPoint = hitinfo.point;
        //������ез������Σ�������˺�
        if (hitinfo)
        {
            if (hitinfo.collider != null && hitinfo.collider.gameObject.tag == "Empty")
            {
                Empty target = hitinfo.collider.GetComponent<Empty>();
                HitAndKo(target);
            }
            //����л��ж��󣬽���ʼ����յ�ֱ��Ӧ
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, EndPoint);
            summonPoint = hitinfo.point;
        }
        else
        {
            //����������ʾ
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, transform.position + transform.right * 8);
            summonPoint = transform.position + transform.right * 8;
        }
    }

    void summonPsyshockSE()
    {
        GameObject psyshockSE = Instantiate(PsyshockSE, summonPoint, Quaternion.identity);
        psyshockSE.GetComponent<PsyshockSE>().player = player;
    }
}
