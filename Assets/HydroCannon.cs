using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HydroCannon : Skill
{
    public float Rotation;

    float RayTimer;

    private LineRenderer lineRenderer;
    //获取激光的初始宽度；
    float LeserStartWidth;
    float LeserEndWidth;
    float MaxLength;

    //获取起始特效
    GameObject StartVFX;
    //获取末尾特效
    GameObject EndVFX;
    ParticleSystem RingPS;
    ParticleSystem RayPS;


    bool isPSStop;
    bool isCTUP;




    private void Awake()
    {
        lineRenderer = GetComponentInChildren<LineRenderer>();
    }
    // Start is called before the first frame update
    void Start()
    {

        //限制射线出现的位置，防止出现在出生点
        lineRenderer.SetPosition(0, transform.position + Vector3.up);
        lineRenderer.SetPosition(1, transform.position + Vector3.up);

        //初始化激光宽度
        LeserStartWidth = lineRenderer.startWidth;
        LeserEndWidth = lineRenderer.endWidth;
        //lineRenderer.startWidth = 0;
        //lineRenderer.endWidth = 0;

        //获取组件
        EndVFX = transform.GetChild(2).gameObject;
        EndVFX.transform.position = transform.position;
        StartVFX = transform.GetChild(1).gameObject;
        StartVFX.transform.position = transform.position;
        MaxLength = 1;

        RingPS = transform.GetChild(1).GetChild(1).GetComponent<ParticleSystem>();
        RayPS = transform.GetChild(1).GetChild(3).GetComponent<ParticleSystem>();

        player.isCanNotMove = true;
        player.animator.SetFloat("Speed" , 0);
    }

    // Update is called once per frame
    void Update()
    {

            StartExistenceTimer();
            RayTimer += Time.deltaTime;
        if (RayTimer <= 3.9f)
        {
            StartVFX.transform.position = transform.position;
            rayPosition();
            if (RayTimer >= 3.0f)
            {
                lineRenderer.startWidth -= Time.deltaTime * LeserStartWidth / 1.4f;
                lineRenderer.endWidth -= Time.deltaTime * LeserEndWidth / 1.4f;
                StartVFX.transform.localScale -= Time.deltaTime * new Vector3(1, 1, 1) / 1.4f;
                EndVFX.transform.localScale -= Time.deltaTime * new Vector3(1, 1, 1) / 1.4f;
                if (RayTimer >= 3.6f && !isPSStop)
                {
                    isPSStop = true;
                    var e1 = transform.GetChild(1).GetChild(1).GetComponent<ParticleSystem>().emission;
                    e1.enabled = false;
                    var e2 = transform.GetChild(1).GetChild(2).GetComponent<ParticleSystem>().emission;
                    e2.enabled = false;
                    var e3 = transform.GetChild(1).GetChild(3).GetComponent<ParticleSystem>().emission;
                    e3.enabled = false;
                    var e4 = transform.GetChild(1).GetChild(4).GetComponent<ParticleSystem>().emission;
                    e4.enabled = false;
                    var e5 = transform.GetChild(2).GetChild(1).GetComponent<ParticleSystem>().emission;
                    e5.enabled = false;
                }
                if (RayTimer >= 3.8f) {
                    transform.GetChild(0).gameObject.SetActive(false);
                    transform.GetChild(1).gameObject.SetActive(false);
                    transform.GetChild(2).gameObject.SetActive(false);
                }
            }
        }
    }



    private void rayPosition()
    {
        MaxLength = Mathf.Clamp(MaxLength + 10.0f * Time.deltaTime, 0,10 );
        Physics2D.queriesHitTriggers = false;
        //检测射线击中的对象
        Vector3 t = (Quaternion.AngleAxis(transform.rotation.eulerAngles.z + 90, Vector3.forward) * Vector3.right).normalized;
        Vector3 b = (Quaternion.AngleAxis(transform.rotation.eulerAngles.z - 90, Vector3.forward) * Vector3.right).normalized;
        RaycastHit2D hitinfo = Physics2D.Raycast(transform.position, (Quaternion.AngleAxis(transform.rotation.eulerAngles.z, Vector3.forward) * Vector3.right).normalized, MaxLength, LayerMask.GetMask("Empty", "EmptyFly", "EmptyJump", "Enviroment", "Room"));
        RaycastHit2D hitinfoTop = Physics2D.Raycast(transform.position + t * (lineRenderer.startWidth / 2), (Quaternion.AngleAxis(transform.rotation.eulerAngles.z, Vector3.forward) * Vector3.right).normalized, MaxLength, LayerMask.GetMask("Empty", "EmptyFly", "EmptyJump", "Enviroment", "Room"));
        RaycastHit2D hitinfoBottom = Physics2D.Raycast(transform.position + b * (lineRenderer.startWidth / 2), (Quaternion.AngleAxis(transform.rotation.eulerAngles.z, Vector3.forward) * Vector3.right).normalized, MaxLength, LayerMask.GetMask("Empty", "EmptyFly", "EmptyJump", "Enviroment", "Room"));
        Vector2 EndPoint = hitinfo.point;
        if ((hitinfo) || (hitinfoTop) || (hitinfoBottom))
        {
            //Debug.Log(hitinfo.point + "+" + hitinfoTop.point + "+" + hitinfoBottom.point);
            RaycastHit2D EndRay = hitinfo;
            if (hitinfo.point != Vector2.zero)
            {
                EndPoint = hitinfo.point;
                EndRay = hitinfo;
            }
            else if (hitinfoTop.point != Vector2.zero)
            {
                EndPoint = hitinfoTop.point;
                EndRay = hitinfoTop;
            }
            else if (hitinfoBottom.point != Vector2.zero)
            {
                EndPoint = hitinfoBottom.point;
                EndRay = hitinfoBottom;
            }
            if (hitinfo.point != Vector2.zero && hitinfoTop.point != Vector2.zero && hitinfoBottom.point != Vector2.zero)
            {
                if ((hitinfoTop.point - hitinfo.point).magnitude >= 0.5f && (hitinfoTop.point - (Vector2)transform.position).magnitude < (hitinfo.point - (Vector2)transform.position).magnitude + 0.5f)
                {
                    EndRay = hitinfoTop; EndPoint = hitinfoTop.point - (Vector2)t * (lineRenderer.startWidth / 2.0f);
                }
                if ((hitinfoBottom.point - hitinfo.point).magnitude >= 0.5f && (hitinfoBottom.point - (Vector2)transform.position).magnitude < (hitinfo.point - (Vector2)transform.position).magnitude + 0.5f)
                {
                    EndRay = hitinfoBottom; EndPoint = hitinfoBottom.point - (Vector2)b * (lineRenderer.startWidth / 2.0f);

                }
            }

            if (EndPoint == Vector2.zero) { EndPoint = hitinfo.point; }


            //如果击中敌方宝可梦，则造成伤害
            if (EndRay.collider != null && EndRay.collider.gameObject.tag == "Empty")
            {
                Empty e = EndRay.collider.GetComponent<Empty>();
                if (e != null)
                {
                    if (SkillFrom == 2 && !isCTUP)
                    {
                        isCTUP = true;
                        CTLevel++;
                    }
                    HitAndKo(e);
                }
            }
            //如果有击中对象，将起始点和终点分别对应
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, EndPoint);

        }
        else
        {
            //否则正常显示
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, transform.position + (Quaternion.AngleAxis(transform.rotation.eulerAngles.z, Vector3.forward) * Vector3.right).normalized * MaxLength);
            EndPoint = lineRenderer.GetPosition(1);
        }
        EndVFX.transform.position = EndPoint;
        float Length = (EndVFX.transform.position - transform.position).magnitude;

        var RingPSShape = RingPS.shape;
        RingPSShape.radius = Length * 0.45f;
        RingPSShape.position = new Vector3(Length * 0.45f + 0.1f, 0, 0);
        var RingPSEmission = RingPS.emission;
        RingPSEmission.rateOverTime = (int)Length * 7.5f;

        var RayPSShape = RayPS.shape;
        RayPSShape.radius = Length / 2;
        RayPSShape.position = new Vector3(Length / 2, 0, 0);
        var RayPSEmission = RayPS.emission;
        RayPSEmission.rateOverTime = (int)Length * 75;



    }

    private void OnDestroy()
    {
        player.isCanNotMove = false;
    }
}
