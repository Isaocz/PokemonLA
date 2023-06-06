using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TeraBlast : Skill
{
    private LineRenderer lineRenderer;
    private void Awake()
    {
        lineRenderer = GetComponentInChildren<LineRenderer>();
    }
    // Start is called before the first frame update
    void Start()
    {
        //�����ҵĹ��������ع�������ʽ��Ϊ������ʽ
        if (player.AtkAbilityPoint > player.SpAAbilityPoint)
        {
            Damage = 80;
            SpDamage = 0;
        }
        //����ڱ�������̫������������ʽ���Ա�Ϊ̫������
        if (0 != player.PlayerTeraTypeJOR)
        {
            SkillType = player.PlayerTeraTypeJOR;
        }
        //�������߳��ֵ�λ�ã���ֹ�����ڳ�����
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, transform.position);

    }

    // Update is called once per frame
    void Update()
    {
        rayPosition();
        StartExistenceTimer();
    }

    private void rayPosition()
    {
        //������߻��еĶ���
        RaycastHit2D hitinfo = Physics2D.Raycast(transform.position ,transform.right);
        if (hitinfo)
        {
            //������ез������Σ�������˺�
            if (hitinfo.collider.gameObject.tag == "Empty")
            {
                Empty target = hitinfo.collider.GetComponent<Empty>();
                HitAndKo(target);
            }
            //����л��ж��󣬽���ʼ����յ�ֱ��Ӧ
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, hitinfo.point);
        }
        else
        {
            //����������ʾ
            lineRenderer.SetPosition(0, transform.position) ;
            lineRenderer.SetPosition(1, transform.position + transform.right * 8);
        }
    }
}
