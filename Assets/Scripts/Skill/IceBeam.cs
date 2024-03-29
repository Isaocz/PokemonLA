using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceBeam : Skill
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
    bool RingPSSpeedDown;

    bool isAnotherBeamBorn;

    public List<GameObject> AnotherBeamList = new List<GameObject> { };
    List<Empty> FrozenList = new List<Empty> { };


    private void Awake()
    {
        lineRenderer = GetComponentInChildren<LineRenderer>();
    }
    // Start is called before the first frame update
    void Start()
    {
        ExistenceTime = 1.5f;
        if (transform.rotation.eulerAngles == new Vector3(0, 0, 0) || transform.rotation.eulerAngles == new Vector3(0, 0, 180)) { DirectionRight = true; }
        else if (transform.rotation.eulerAngles == new Vector3(0, 0, 90) || transform.rotation.eulerAngles == new Vector3(0, 0, 270)) { DirectionRight = false; }

        //限制射线出现的位置，防止出现在出生点
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, transform.position);

        //初始化激光宽度
        LeserWidth = 1.3f;
        lineRenderer.startWidth = 0;
        lineRenderer.endWidth = 0;

        //获取组件
        EndVFX = transform.GetChild(2).gameObject;
        StartVFX = transform.GetChild(1).gameObject;



    }

    // Update is called once per frame
    void Update()
    {
        if (ExistenceTime > 1.35 || lineRenderer.startWidth < LeserWidth)
        {
            lineRenderer.startWidth += LeserWidth * (Time.deltaTime / 0.15f);
            lineRenderer.endWidth += LeserWidth * (Time.deltaTime / 0.15f);
        }
        else if (ExistenceTime < 0.15 || lineRenderer.startWidth > LeserWidth)
        {
            lineRenderer.startWidth -= LeserWidth * (Time.deltaTime / 0.15f);
            lineRenderer.endWidth -= LeserWidth * (Time.deltaTime / 0.15f);
        }
        rayPosition();
        StartExistenceTimer();
        if (!RingPSSpeedDown)
        {
            RingPSSpeedDown = true;
            float LeserLength = (lineRenderer.GetPosition(0) - lineRenderer.GetPosition(1)).magnitude;
            //var RingPSVofL = StartVFX.transform.GetChild(0).GetComponent<ParticleSystem>().velocityOverLifetime;
            //RingPSVofL.z = 13*(LeserLength/8);
            //RingPSVofL.orbitalZ = 39*(LeserLength/8);

        }
    }



    private void rayPosition()
    {



        Physics2D.queriesHitTriggers = false;
        //检测射线击中的对象
        RaycastHit2D hitinfo = Physics2D.Raycast(transform.position, transform.right, 8, LayerMask.GetMask("Empty", "EmptyFly", "Enviroment", "Room"));
        RaycastHit2D hitinfoTop = Physics2D.Raycast(transform.position + Vector3.up * 0.3f, transform.right, 8f, LayerMask.GetMask("Empty", "EmptyFly", "Enviroment", "Room"));
        RaycastHit2D hitinfoBottom = Physics2D.Raycast(transform.position - Vector3.up * 0.3f, transform.right, 8f, LayerMask.GetMask("Empty", "EmptyFly", "Enviroment", "Room"));

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
            if (EndRay.collider != null && ( EndRay.collider.gameObject.tag == "Empty" || EndRay.collider.gameObject.tag == "Enviroment"))
            {
                Empty target = EndRay.collider.GetComponent<Empty>();
                IcicleCrashOBJ Ice = EndRay.collider.GetComponent<IcicleCrashOBJ>();
                if (target != null)
                {
                    HitAndKo(target);
                    if (!FrozenList.Contains(target))
                    {
                        target.Frozen(7.5f, 1, 0.1f + ((float)player.LuckPoint / 30));
                    }
                    FrozenList.Add(target);
                    if (SkillFrom == 2)
                    {
                        if (!isAnotherBeamBorn && !AnotherBeamList.Contains(target.gameObject) && target.isEmptyFrozenDone)
                        {
                            AnotherBeamList.Add(target.gameObject);
                            isAnotherBeamBorn = true;
                            IceBeam Beam1 = Instantiate(this, EndPoint, Quaternion.AngleAxis(90, Vector3.forward) * transform.rotation);
                            IceBeam Beam2 = Instantiate(this, EndPoint, Quaternion.AngleAxis(-90, Vector3.forward) * transform.rotation);
                        }
                    }
                }
                if (Ice != null)
                {
                    if (SkillFrom == 2)
                    {
                        
                        if (!isAnotherBeamBorn && !AnotherBeamList.Contains(Ice.gameObject))
                        {
                            Ice.ColliderCount++;
                            if (Ice.ColliderCount >= Random.Range(1, 6))
                            {
                                Ice.IceBreak();
                            }
                            Vector2 Offset1 = Quaternion.AngleAxis(90, Vector3.forward) * ((EndPoint - (Vector2)(transform.position)).normalized);
                            Vector2 Offset2 = ((EndPoint - (Vector2)(transform.position)).normalized);
                            AnotherBeamList.Add(Ice.gameObject);
                            isAnotherBeamBorn = true;
                            IceBeam Beam1 = Instantiate(this, EndPoint + Offset1* 0.5f - Offset2 * 0.2f, Quaternion.AngleAxis(90, Vector3.forward) * transform.rotation);
                            IceBeam Beam2 = Instantiate(this, EndPoint - Offset1* 0.5f - Offset2 * 0.2f, Quaternion.AngleAxis(-90, Vector3.forward) * transform.rotation);
                        }
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
            lineRenderer.SetPosition(1, transform.position + transform.right * 8);
            EndPoint = transform.position + transform.right * 8;
        }
        EndVFX.transform.position = EndPoint;



    }

    
}
