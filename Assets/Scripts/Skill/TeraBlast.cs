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
        //如果玩家的攻击大于特攻，则招式变为攻击招式
        if (player.AtkAbilityPoint > player.SpAAbilityPoint)
        {
            Damage = 80;
            SpDamage = 0;
        }
        //如果在本房间内太晶化过，则招式属性变为太晶属性
        if (0 != player.PlayerTeraTypeJOR)
        {
            SkillType = player.PlayerTeraTypeJOR;
        }
        //限制射线出现的位置，防止出现在出生点
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
        //检测射线击中的对象
        RaycastHit2D hitinfo = Physics2D.Raycast(transform.position ,transform.right);
        if (hitinfo)
        {
            //如果击中敌方宝可梦，则造成伤害
            if (hitinfo.collider.gameObject.tag == "Empty")
            {
                Empty target = hitinfo.collider.GetComponent<Empty>();
                HitAndKo(target);
            }
            //如果有击中对象，将起始点和终点分别对应
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, hitinfo.point);
        }
        else
        {
            //否则正常显示
            lineRenderer.SetPosition(0, transform.position) ;
            lineRenderer.SetPosition(1, transform.position + transform.right * 8);
        }
    }
}
