using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolarBeamCastform : Projectile
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

    }

    // Update is called once per frame
    void Update()
    {
        if ((empty.isSleepDone || empty.isFearDone || empty.isEmptyFrozenDone) && LeserTimer < 3.45f)
        {
            LeserTimer = 3.45f;
        }
        LeserTimer += Time.deltaTime;
        if (LeserTimer < 0.5f)
        {
            lineRenderer.startWidth += LeserWidth * (Time.deltaTime / 0.5f);
            lineRenderer.endWidth += LeserWidth * (Time.deltaTime / 0.5f);
        }
        else if (LeserTimer > 3.5f)
        {
            lineRenderer.startWidth = Mathf.Clamp( lineRenderer.startWidth - LeserWidth * (Time.deltaTime / 0.5f) , 0 , 10 );
            lineRenderer.endWidth = Mathf.Clamp(lineRenderer.endWidth - LeserWidth * (Time.deltaTime / 0.5f), 0, 10);
            if (LeserTimer > 4.1f)
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
        RaycastHit2D hitinfo = Physics2D.Raycast(transform.position, (Quaternion.AngleAxis(transform.rotation.eulerAngles.z, Vector3.forward) * Vector3.right).normalized, 20, LayerMask.GetMask("Player", "PlayerFly", "PlayerJump", "Enviroment", "Room"));
        RaycastHit2D hitinfoTop = Physics2D.Raycast(transform.position + t * 0.4f, (Quaternion.AngleAxis(transform.rotation.eulerAngles.z, Vector3.forward) * Vector3.right).normalized, 20, LayerMask.GetMask("Player", "PlayerFly", "PlayerJump", "Enviroment", "Room"));
        RaycastHit2D hitinfoBottom = Physics2D.Raycast(transform.position + b * 0.4f, (Quaternion.AngleAxis(transform.rotation.eulerAngles.z, Vector3.forward) * Vector3.right).normalized, 20, LayerMask.GetMask("Player", "PlayerFly", "PlayerJump", "Enviroment", "Room"));
        Vector2 EndPoint = hitinfo.point;
        if ((hitinfo) || (hitinfoTop) || (hitinfoBottom))
        {
            Debug.Log(hitinfo.point + "+" + hitinfoTop.point + "+" + hitinfoBottom.point);
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
            if (EndRay.collider != null && EndRay.collider.gameObject.tag == "Player")
            {
                PlayerControler p = EndRay.collider.GetComponent<PlayerControler>();
                if (empty != null) { Pokemon.PokemonHpChange(empty.gameObject, EndRay.collider.gameObject, 0, SpDmage, 0, Type.TypeEnum.Grass); }
                else {
                    Pokemon.PokemonHpChange(null, EndRay.collider.gameObject, 0, SpDmage, 0, Type.TypeEnum.Grass); 
                }
                if (p != null)
                {
                    p.KnockOutPoint = 5f;
                    p.KnockOutDirection = (p.transform.position - transform.position).normalized;
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
        RingPSShape.scale = new Vector3(1 , 1 , (int)Length*5 );
        RingPSShape.position = new Vector3(0, 0, Length/2 - 1);
        var RingPSEmission = RingPS.emission;
        RingPSEmission.rateOverTime = (int)Length * 5;

    }
}
