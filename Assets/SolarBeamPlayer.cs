using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolarBeamPlayer : GrassSkill
{
    float LeserTimer;
    private LineRenderer lineRenderer;
    //��ȡ����ĳ�ʼ��ȣ�
    float LeserWidth;

    //��ȡ��ʼ��Ч
    GameObject StartVFX;
    //��ȡĩβ��Ч
    GameObject EndVFX;



    ParticleSystem RingPS;


    bool isEndureOver;
    bool isSPDamageDown;




    private void Awake()
    {
        lineRenderer = GetComponentInChildren<LineRenderer>();
    }
    // Start is called before the first frame update
    void Start()
    {

        //�������߳��ֵ�λ�ã���ֹ�����ڳ�����
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, transform.position);

        //��ʼ��������
        LeserWidth = lineRenderer.endWidth;
        lineRenderer.startWidth = 0;
        lineRenderer.endWidth = 0;

        //��ȡ���
        EndVFX = transform.GetChild(2).gameObject;
        StartVFX = transform.GetChild(1).gameObject;

        RingPS = transform.GetChild(1).GetChild(0).GetComponent<ParticleSystem>();

        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(1).gameObject.SetActive(false);
        transform.GetChild(2).gameObject.SetActive(false);
        player.isCanNotTurnDirection = true;
        player.SpeedChange();

        if (Weather.GlobalWeather.isSunny || Weather.GlobalWeather.isSunnyPlus)
        {
            LeserTimer = 1.45f;
            transform.GetChild(3).gameObject.SetActive(false);
        }

    }

    // Update is called once per frame
    void Update()
    {
        StartExistenceTimer();
        LeserTimer += Time.deltaTime;
        if (SkillFrom == 2 && LeserTimer < 1.5f)
        {
            for (int i = 0; i < player.InGressCount.Count; i++)
            {
                NormalGress n = player.InGressCount[i].GetComponent<NormalGress>();
                GressPlayerINOUT g = player.InGressCount[i].GetComponent<GressPlayerINOUT>();
                if (player.InGressCount[i].gameObject.tag == "Grass")
                {
                    if (n != null && !n.isDie) { n.GrassDie(); SpDamage += 15;                    }
                    if (g != null && !g.isDie) { g.GrassDie(); SpDamage += 15;                    }
                }
            }
        }

        if (LeserTimer >= 1.5f && LeserTimer < 1.85f)
        {
            if (!isEndureOver) {
                transform.GetChild(0).gameObject.SetActive(true);
                transform.GetChild(1).gameObject.SetActive(true);
                transform.GetChild(2).gameObject.SetActive(true);
                isEndureOver = true;
                player.isCanNotTurnDirection = false;
                player.SpeedRemove01(0);
            }
            lineRenderer.startWidth += LeserWidth * (Time.deltaTime / 0.35f) ;
            lineRenderer.endWidth += LeserWidth * (Time.deltaTime / 0.35f) ;
        }
        else if (LeserTimer > 3.15f)
        {
            lineRenderer.startWidth = Mathf.Clamp(lineRenderer.startWidth - LeserWidth * (Time.deltaTime / 0.35f), 0, 10)  ;
            lineRenderer.endWidth = Mathf.Clamp(lineRenderer.endWidth - LeserWidth * (Time.deltaTime / 0.35f), 0, 10) ;
            if (LeserTimer > 3.55f)
            {
                Destroy(gameObject);
            }
        }
        rayPosition();

    }



    private void rayPosition()
    {
        Physics2D.queriesHitTriggers = false;
        //������߻��еĶ���
        Vector3 t = (Quaternion.AngleAxis(transform.rotation.eulerAngles.z + 90, Vector3.forward) * Vector3.right).normalized;
        Vector3 b = (Quaternion.AngleAxis(transform.rotation.eulerAngles.z - 90, Vector3.forward) * Vector3.right).normalized;
        RaycastHit2D hitinfo = Physics2D.Raycast(transform.position, (Quaternion.AngleAxis(transform.rotation.eulerAngles.z, Vector3.forward) * Vector3.right).normalized, 20, LayerMask.GetMask("Empty", "EmptyFly", "EmptyJump", "Enviroment", "Room"));
        RaycastHit2D hitinfoTop = Physics2D.Raycast(transform.position + t * 0.4f, (Quaternion.AngleAxis(transform.rotation.eulerAngles.z, Vector3.forward) * Vector3.right).normalized, 20, LayerMask.GetMask("Empty", "EmptyFly", "EmptyJump", "Enviroment", "Room"));
        RaycastHit2D hitinfoBottom = Physics2D.Raycast(transform.position + b * 0.4f, (Quaternion.AngleAxis(transform.rotation.eulerAngles.z, Vector3.forward) * Vector3.right).normalized, 20, LayerMask.GetMask("Empty", "EmptyFly", "EmptyJump", "Enviroment", "Room"));
        Vector2 EndPoint = hitinfo.point;
        if ((hitinfo) || (hitinfoTop) || (hitinfoBottom))
        {
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
                    EndRay = hitinfoTop; EndPoint = hitinfoTop.point - (Vector2)t * 0.4f;
                }
                if ((hitinfoBottom.point - hitinfo.point).magnitude >= 0.5f && (hitinfoBottom.point - (Vector2)transform.position).magnitude < (hitinfo.point - (Vector2)transform.position).magnitude + 0.5f)
                {
                    EndRay = hitinfoBottom; EndPoint = hitinfoBottom.point - (Vector2)b * 0.4f;

                }
            }

            if (EndPoint == Vector2.zero) { EndPoint = hitinfo.point; }

            //������ез������Σ�������˺�
            if (EndRay.collider != null && EndRay.collider.gameObject.tag == "Empty")
            {
                Empty e = EndRay.collider.GetComponent<Empty>();
                if (e != null) {

                    if (Weather.GlobalWeather.isRain || Weather.GlobalWeather.isRainPlus || Weather.GlobalWeather.isHail || Weather.GlobalWeather.isHailPlus || Weather.GlobalWeather.isSandstorm || Weather.GlobalWeather.isSandstormPlus)
                    {
                        if (!isSPDamageDown) {
                            isSPDamageDown = true;
                            SpDamage /= 2;
                        }
                    }
                    HitAndKo(e);
                }
            }
            //����л��ж��󣬽���ʼ����յ�ֱ��Ӧ
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, EndPoint);

        }
        else
        {
            //����������ʾ
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, transform.position + (Quaternion.AngleAxis(transform.rotation.eulerAngles.z, Vector3.forward) * Vector3.right).normalized * 20);
            EndPoint = transform.position + transform.right * 20;
        }
        EndVFX.transform.position = EndPoint;
        float Length = (EndVFX.transform.position - transform.position).magnitude;

        var RingPSShape = RingPS.shape;
        RingPSShape.scale = new Vector3(1, 1, (int)Length * 5);
        RingPSShape.position = new Vector3(0, 0, Length / 2 - 1);
        var RingPSEmission = RingPS.emission;
        RingPSEmission.rateOverTime = (int)Length * 5;

    }
}
