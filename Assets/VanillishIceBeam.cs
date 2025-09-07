using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VanillishIceBeam : MonoBehaviour
{
    public Vanillish ParentVanillish;
    public float Rotation;

    float RayTimer;

    private LineRenderer lineRenderer;

    //�����в����Ӽ���
    private LineRenderer SonlineRenderer;

    //��ȡ����ĳ�ʼ��ȣ�
    float LeserWidth;

    //��ȡ��ʼ��Ч
    GameObject StartVFX;
    //��ȡĩβ��Ч
    GameObject EndVFX;
    bool isPSStop;

    //����
    bool RingPSSpeedDown;

    float InfatuationDmageCDTimer;


    private void Awake()
    {
        lineRenderer = transform.GetChild(0).GetComponent<LineRenderer>();
        SonlineRenderer = lineRenderer.transform.GetChild(0).GetComponent<LineRenderer>();
    }
    // Start is called before the first frame update
    void Start()
    {

        //�������߳��ֵ�λ�ã���ֹ�����ڳ�����
        lineRenderer.SetPosition(0, transform.position + Vector3.up);
        lineRenderer.SetPosition(1, transform.position + Vector3.up);

        //��ʼ��������
        LeserWidth = lineRenderer.endWidth;
        lineRenderer.startWidth = 0;
        lineRenderer.endWidth = 0;

        //��ȡ���
        EndVFX = transform.GetChild(2).gameObject;
        StartVFX = transform.GetChild(1).gameObject;
        StartVFX.transform.position = lineRenderer.GetPosition(0);

        SetSonBeam();
    }

    // Update is called once per frame
    void Update()
    {
        if (InfatuationDmageCDTimer > 0)
        {
            InfatuationDmageCDTimer -= Time.deltaTime;
        }
        if (!ParentVanillish.isEmptyFrozenDone && !ParentVanillish.isSleepDone && !ParentVanillish.isCanNotMoveWhenParalysis && !ParentVanillish.isSilence)
        {
            RayTimer += Time.deltaTime;
            if (!isPSStop)
            {
                if (RayTimer < 0.5)
                {
                    lineRenderer.startWidth += LeserWidth * (Time.deltaTime / 0.5f);
                    lineRenderer.endWidth += LeserWidth * (Time.deltaTime / 0.5f);
                }
                transform.rotation = Quaternion.AngleAxis(20.0f * Time.deltaTime, Vector3.forward) * transform.rotation;
                StartVFX.transform.position = lineRenderer.GetPosition(0);
                rayPosition();
            }
            else
            {
                lineRenderer.startWidth -= LeserWidth * (Time.deltaTime / 0.5f);
                lineRenderer.endWidth -= LeserWidth * (Time.deltaTime / 0.5f);
                if (lineRenderer.startWidth <= 0 && lineRenderer.endWidth <= 0)
                {
                    Destroy(transform.gameObject);
                }
            }
            SetSonBeam();
        }
        else
        {
            if (!isPSStop)
            {
                rayPosition();
            }
        }
    }


    public void StopBeam()
    {
        transform.GetChild(0).gameObject.SetActive(false);
        isPSStop = true;
        StartVFX.transform.GetChild(0).GetComponent<ParticleSystem>().Stop();
        StartVFX.transform.GetChild(1).GetComponent<ParticleSystem>().Stop();
        StartVFX.transform.GetChild(2).GetComponent<ParticleSystem>().Stop();
        StartVFX.transform.GetChild(3).GetComponent<ParticleSystem>().Stop();
        StartVFX.transform.GetChild(4).GetComponent<ParticleSystem>().Stop();
        EndVFX.transform.GetChild(0).GetComponent<ParticleSystem>().Stop();
        EndVFX.transform.GetChild(1).GetComponent<ParticleSystem>().Stop();
        EndVFX.transform.GetChild(2).GetComponent<ParticleSystem>().Stop();
        _mTool.RemoveAllPSChild(transform.gameObject);
    }

    void SetSonBeam()
    {
        if (SonlineRenderer != null)
        {
            SonlineRenderer.startWidth = lineRenderer.startWidth * 0.22f;
            SonlineRenderer.endWidth = lineRenderer.endWidth * 0.22f;
            SonlineRenderer.SetPosition(0, lineRenderer.GetPosition(0));
            SonlineRenderer.SetPosition(1, lineRenderer.GetPosition(1));
        }
    }


    private void rayPosition()
    {
        Physics2D.queriesHitTriggers = false;
        //������߻��еĶ���
        Vector3 t = (Quaternion.AngleAxis(transform.rotation.eulerAngles.z + 90, Vector3.forward) * Vector3.right).normalized;
        Vector3 b = (Quaternion.AngleAxis(transform.rotation.eulerAngles.z - 90, Vector3.forward) * Vector3.right).normalized;
        RaycastHit2D hitinfo = Physics2D.Raycast(transform.position + Vector3.up, (Quaternion.AngleAxis(transform.rotation.eulerAngles.z, Vector3.forward) * Vector3.right).normalized, 20, LayerMask.GetMask("Player", "PlayerFly", "PlayerJump", "Enviroment", "Room"));
        RaycastHit2D hitinfoTop = Physics2D.Raycast(transform.position + t * (lineRenderer.startWidth / 6.0f) + Vector3.up, (Quaternion.AngleAxis(transform.rotation.eulerAngles.z, Vector3.forward) * Vector3.right).normalized, 20f, LayerMask.GetMask("Player", "PlayerFly", "PlayerJump", "Enviroment", "Room"));
        RaycastHit2D hitinfoBottom = Physics2D.Raycast(transform.position + b * (lineRenderer.startWidth / 6.0f) + Vector3.up, (Quaternion.AngleAxis(transform.rotation.eulerAngles.z, Vector3.forward) * Vector3.right).normalized, 20f, LayerMask.GetMask("Player", "PlayerFly", "PlayerJump", "Enviroment", "Room"));
        if (ParentVanillish.isEmptyInfatuationDone)
        {
            hitinfo = Physics2D.Raycast(transform.position + Vector3.up, (Quaternion.AngleAxis(transform.rotation.eulerAngles.z, Vector3.forward) * Vector3.right).normalized, 20, LayerMask.GetMask("Empty", "EmptyFly", "EmptyJump", "Enviroment", "Room"));
            hitinfoTop = Physics2D.Raycast(transform.position + t * (lineRenderer.startWidth / 6.0f) + Vector3.up, (Quaternion.AngleAxis(transform.rotation.eulerAngles.z, Vector3.forward) * Vector3.right).normalized, 20f, LayerMask.GetMask("Empty", "EmptyFly", "EmptyJump", "Enviroment", "Room"));
            hitinfoBottom = Physics2D.Raycast(transform.position + b * (lineRenderer.startWidth / 6.0f) + Vector3.up, (Quaternion.AngleAxis(transform.rotation.eulerAngles.z, Vector3.forward) * Vector3.right).normalized, 20f, LayerMask.GetMask("Empty", "EmptyFly", "EmptyJump", "Enviroment", "Room"));
        }



        Vector2 EndPoint = hitinfo.point;
        if ((hitinfo) || (hitinfoTop) || (hitinfoBottom))
        {
            EndPoint = hitinfo.point;
            RaycastHit2D EndRay = hitinfo;

            if ((hitinfoTop.point - hitinfo.point).magnitude >= 0.5f)
            {
                EndRay = hitinfoTop; EndPoint = hitinfoTop.point - (Vector2)t * (lineRenderer.startWidth / 6.0f);
            }
            if ((hitinfoBottom.point - hitinfo.point).magnitude >= 0.5f)
            {
                EndRay = hitinfoBottom; EndPoint = hitinfoBottom.point - (Vector2)b * (lineRenderer.startWidth / 6.0f);

            }

            if (EndPoint == Vector2.zero) { EndPoint = hitinfo.point; }


            //������ез������Σ�������˺�
            if (!ParentVanillish.isEmptyInfatuationDone)
            {
                if (EndRay.collider != null && EndRay.collider.gameObject.tag == "Player")
                {
                    PlayerControler p = EndRay.collider.GetComponent<PlayerControler>();
                    Pokemon.PokemonHpChange(ParentVanillish.gameObject, EndRay.collider.gameObject, 0, 120, 0, PokemonType.TypeEnum.Ice);
                    if (p != null)
                    {
                        p.KnockOutPoint = 7f;
                        p.KnockOutDirection = (p.transform.position - transform.position).normalized;
                    }
                }
            }
            else
            {
                if (EndRay.collider.gameObject != ParentVanillish.gameObject && InfatuationDmageCDTimer <= 0)
                {
                    if (EndRay.collider != null && EndRay.collider.gameObject.tag == "Empty")
                    {
                        Empty e = EndRay.collider.GetComponent<Empty>();
                        Pokemon.PokemonHpChange(ParentVanillish.gameObject, EndRay.collider.gameObject, 0, 120, 0, PokemonType.TypeEnum.Ice);
                        InfatuationDmageCDTimer = 1.2f;
                    }
                }
            }
            //����л��ж��󣬽���ʼ����յ�ֱ��Ӧ
            lineRenderer.SetPosition(0, transform.position + Vector3.up + ((Vector3)EndPoint - transform.position).normalized * 1.4f);
            lineRenderer.SetPosition(1, EndPoint);

        }
        else
        {
            //����������ʾ
            lineRenderer.SetPosition(0, transform.position + Vector3.up + (Quaternion.AngleAxis(transform.rotation.eulerAngles.z, Vector3.forward) * Vector3.right).normalized * 1.4f);
            lineRenderer.SetPosition(1, transform.position + Vector3.up + (Quaternion.AngleAxis(transform.rotation.eulerAngles.z, Vector3.forward) * Vector3.right).normalized * 20);
            EndPoint = transform.position + Vector3.up + (Quaternion.AngleAxis(transform.rotation.eulerAngles.z, Vector3.forward) * Vector3.right).normalized * 1.2f + (Quaternion.AngleAxis(transform.rotation.eulerAngles.z, Vector3.forward) * Vector3.right).normalized * 20;
        }
        EndVFX.transform.position = EndPoint;

        ParentVanillish.SetDirector(_mTool.TiltMainVector2((EndPoint - (Vector2)transform.position).normalized));



    }
}
