using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloysterAuroraBeamLarge : MonoBehaviour
{
    public Cloyster ParentCloyster;
    public float Rotation;

    float RayTimer;

    private LineRenderer lineRenderer;
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
        lineRenderer = GetComponentInChildren<LineRenderer>();
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
        StartVFX.transform.position = transform.position + Vector3.up;


    }

    // Update is called once per frame
    void Update()
    {
        if (InfatuationDmageCDTimer > 0)
        {
            InfatuationDmageCDTimer -= Time.deltaTime;
        }
        if (!ParentCloyster.isEmptyFrozenDone && !ParentCloyster.isSleepDone && !ParentCloyster.isCanNotMoveWhenParalysis && !ParentCloyster.isSilence) {
            RayTimer += Time.deltaTime;
            if (RayTimer < 2.5f)
            {
                if (RayTimer < 0.3)
                {
                    lineRenderer.startWidth += LeserWidth * (Time.deltaTime / 0.3f);
                    lineRenderer.endWidth += LeserWidth * (Time.deltaTime / 0.3f);
                }
                if (RayTimer > 2.0f && RayTimer < 2.5f)
                {
                    lineRenderer.startWidth -= LeserWidth * (Time.deltaTime / 0.5f);
                    lineRenderer.endWidth -= LeserWidth * (Time.deltaTime / 0.5f);
                }
                float d1 = _mTool.Angle_360Y((ParentCloyster.TargetPosition - (Vector2)ParentCloyster.transform.position), Vector3.right) + (ParentCloyster.isEmptyConfusionDone?Random.Range(-30,30):0);
                float d2 = _mTool.Angle_360Y((Quaternion.AngleAxis(transform.rotation.eulerAngles.z, Vector3.forward) * Vector2.right).normalized, Vector3.right);
                if (d1 > 180) { d1 = d1 - 360; }
                if (d2 > 180) { d2 = d2 - 360; }
                float d = d1 - d2;
                if (d > 180) { d = d - 360; }
                else if (d < -180) { d = 360 + d; }
                if (d < 60 && d > 0) { d = 60; }
                if (d > -60 && d < 0) { d = -60; }
                transform.rotation = Quaternion.AngleAxis(d * Time.deltaTime, Vector3.forward) * transform.rotation;
                StartVFX.transform.position = transform.position + Vector3.up;
                rayPosition();
            }
            else if (RayTimer >= 2.45f && RayTimer < 2.7f)
            {
                if (!isPSStop)
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
                }

            }
            else if (RayTimer >= 2.52f && RayTimer < 5f)
            {
                transform.GetChild(1).gameObject.SetActive(false);
                transform.GetChild(2).gameObject.SetActive(false);
            }
            else
            {
                ParentCloyster.isBeam = false;
                ParentCloyster.BeamClose();
                Destroy(transform.parent.gameObject);
            }
        }
        else
        {
            if (RayTimer < 2.5f)
            {
                rayPosition();
            }
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
        if (ParentCloyster.isEmptyInfatuationDone) {
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
            if (!ParentCloyster.isEmptyInfatuationDone) {
                if (EndRay.collider != null && EndRay.collider.gameObject.tag == "Player")
                {
                    PlayerControler p = EndRay.collider.GetComponent<PlayerControler>();
                    Pokemon.PokemonHpChange(ParentCloyster.gameObject, EndRay.collider.gameObject, 0, 80, 0, Type.TypeEnum.Ice);
                    if (p != null)
                    {
                        p.KnockOutPoint = 7f;
                        p.KnockOutDirection = (p.transform.position - transform.position).normalized;
                    }
                }
            }
            else
            {
                if (EndRay.collider.gameObject != ParentCloyster.gameObject && InfatuationDmageCDTimer <= 0) {
                    if (EndRay.collider != null && EndRay.collider.gameObject.tag == "Empty")
                    {
                        Empty e = EndRay.collider.GetComponent<Empty>();
                        Pokemon.PokemonHpChange(ParentCloyster.gameObject, EndRay.collider.gameObject, 0, 80, 0, Type.TypeEnum.Ice);
                        InfatuationDmageCDTimer = 1.2f;
                    }
                }
            }
            //����л��ж��󣬽���ʼ����յ�ֱ��Ӧ
            lineRenderer.SetPosition(0, transform.position + Vector3.up);
            lineRenderer.SetPosition(1, EndPoint);

        }
        else
        {
            //����������ʾ
            lineRenderer.SetPosition(0, transform.position + Vector3.up);
            lineRenderer.SetPosition(1, transform.position + Vector3.up + (Quaternion.AngleAxis(transform.rotation.eulerAngles.z, Vector3.forward) * Vector3.right).normalized * 20);
            EndPoint = transform.position + Vector3.up + (Quaternion.AngleAxis(transform.rotation.eulerAngles.z, Vector3.forward) * Vector3.right).normalized * 1.2f + (Quaternion.AngleAxis(transform.rotation.eulerAngles.z, Vector3.forward) * Vector3.right).normalized * 20;
        }
        EndVFX.transform.position = EndPoint;



    }
}
