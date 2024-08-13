using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Psybeam : Skill
{
    private LineRenderer lineRenderer;
    //获取激光的初始宽度；
    float LeserWidth;

    //获取起始特效
    GameObject StartVFX;
    //获取末尾特效
    GameObject EndVFX;

    //方向
    bool DirectionRight;


    private void Awake()
    {
        lineRenderer = GetComponentInChildren<LineRenderer>();
    }
    // Start is called before the first frame update
    void Start()
    {



        if (transform.rotation.eulerAngles == new Vector3(0, 0, 0) || transform.rotation.eulerAngles == new Vector3(0, 0, 180)) { DirectionRight = true; }
        else if (transform.rotation.eulerAngles == new Vector3(0, 0, 90) || transform.rotation.eulerAngles == new Vector3(0, 0, 270)) { DirectionRight = false; }

        //限制射线出现的位置，防止出现在出生点
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, transform.position);

        //初始化激光宽度
        LeserWidth = lineRenderer.endWidth;
        lineRenderer.startWidth = 0;
        lineRenderer.endWidth = 0;

        //获取组件
        EndVFX = transform.GetChild(2).gameObject;
        StartVFX = transform.GetChild(1).gameObject;


    }

    // Update is called once per frame
    void Update()
    {
        if (ExistenceTime > 0.0f)
        {
            if (ExistenceTime > 0.85)
            {
                lineRenderer.startWidth += LeserWidth * (Time.deltaTime / 0.15f);
                lineRenderer.endWidth += LeserWidth * (Time.deltaTime / 0.15f);
            }
            else if (ExistenceTime < 0.2f)
            {
                lineRenderer.startWidth -= LeserWidth * (Time.deltaTime / 0.15f);
                lineRenderer.endWidth -= LeserWidth * (Time.deltaTime / 0.15f);
            }

            rayPosition();

        }
        else
        {
            lineRenderer.gameObject.SetActive(false);
            EndVFX.SetActive(false);
            StartVFX.SetActive(false);
        }
        StartExistenceTimer();
    }



    private void rayPosition()
    {



        Physics2D.queriesHitTriggers = false;
        //检测射线击中的对象
        RaycastHit2D hitinfo = Physics2D.Raycast(transform.position, transform.right, MaxRange, LayerMask.GetMask("Empty", "EmptyFly", "Enviroment", "Room"));
        RaycastHit2D hitinfoTop = Physics2D.Raycast(transform.position + Vector3.up * 0.4f, transform.right, MaxRange, LayerMask.GetMask("Empty", "EmptyFly", "Enviroment", "Room"));
        RaycastHit2D hitinfoBottom = Physics2D.Raycast(transform.position - Vector3.up * 0.4f, transform.right, MaxRange, LayerMask.GetMask("Empty", "EmptyFly", "Enviroment", "Room"));

        Vector2 EndPoint = hitinfo.point;
        if ((hitinfo) || (hitinfoTop) || (hitinfoBottom))
        {
            EndPoint = hitinfo.point;
            RaycastHit2D EndRay = hitinfo;
            if ((hitinfoTop.point - hitinfo.point).magnitude >= 0.5f)
            {
                EndRay = hitinfoTop; EndPoint = hitinfoTop.point;
            }
            if ((hitinfoBottom.point - hitinfo.point).magnitude >= 0.5f)
            {
                EndRay = hitinfoBottom; EndPoint = hitinfoBottom.point;

            }
            if (EndPoint == Vector2.zero) { EndPoint = hitinfo.point; }


            //如果击中敌方宝可梦，则造成伤害
            if (EndRay.collider != null && EndRay.collider.gameObject.tag == "Empty")
            {
                Empty target = EndRay.collider.GetComponent<Empty>();
                int BeforeHp = target.EmptyHp;
                HitAndKo(target);
                //如果成功造成伤害计算特效
                if (BeforeHp - target.EmptyHp > 0) {
                    if (SkillFrom == 2)
                    {
                        if (target.isEmptyConfusionDone)
                        {
                            if (player.Skill01 != null && player.Skill01.SkillIndex != SkillIndex && player.Skill01.SkillType == 14)
                            {
                                player.MinusSkillCDTime(1, 0.5f, false);
                            }
                            if (player.Skill02 != null && player.Skill02.SkillIndex != SkillIndex && player.Skill02.SkillType == 14)
                            {
                                player.MinusSkillCDTime(2, 0.5f, false);
                            }
                            if (player.Skill03 != null && player.Skill03.SkillIndex != SkillIndex && player.Skill03.SkillType == 14)
                            {
                                player.MinusSkillCDTime(3, 0.5f, false);
                            }
                            if (player.Skill04 != null && player.Skill04.SkillIndex != SkillIndex && player.Skill04.SkillType == 14)
                            {
                                player.MinusSkillCDTime(4, 0.5f, false);
                            }
                        }
                    }
                    Debug.Log("XXX");
                    if (Random.Range(0f, 1f) + (float)player.LuckPoint / 30 > 0.9f)
                    {
                        target.EmptyConfusion(10.0f, 1.0f);
                    }
                }
            }
            if (DirectionRight) { EndPoint.y = transform.position.y; }
            else { EndPoint.x = transform.position.x; }
            //如果有击中对象，将起始点和终点分别对应
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, EndPoint);

        }
        else
        {
            //否则正常显示
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, transform.position + transform.right * MaxRange);
            EndPoint = transform.position + transform.right * MaxRange;
        }
        EndVFX.transform.position = EndPoint;



    }
}
