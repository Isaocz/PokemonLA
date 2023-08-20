using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloysterAuroraBeamSmall : MonoBehaviour
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
        lineRenderer.SetPosition(0, transform.position + Vector3.up + (Quaternion.AngleAxis(transform.rotation.eulerAngles.z, Vector3.forward) * Vector3.right).normalized * 0.7f);
        lineRenderer.SetPosition(1, transform.position + Vector3.up + (Quaternion.AngleAxis(transform.rotation.eulerAngles.z, Vector3.forward) * Vector3.right).normalized * 0.7f);

        //��ʼ��������
        LeserWidth = lineRenderer.endWidth;
        lineRenderer.startWidth = 0;
        lineRenderer.endWidth = 0;

        //��ȡ���
        EndVFX = transform.GetChild(2).gameObject;
        StartVFX = transform.GetChild(1).gameObject;
        StartVFX.transform.position = transform.position + Vector3.up + (Quaternion.AngleAxis(transform.rotation.eulerAngles.z, Vector3.forward) * Vector3.right).normalized * 0.7f;


    }

    // Update is called once per frame
    void Update()
    {
        if (InfatuationDmageCDTimer > 0)
        {
            InfatuationDmageCDTimer -= Time.deltaTime;
        }
        if (!ParentCloyster.isEmptyFrozenDone && !ParentCloyster.isSleepDone && !ParentCloyster.isCanNotMoveWhenParalysis && !ParentCloyster.isSilence)
        {
            RayTimer += Time.deltaTime;
            if (RayTimer < 0.3)
            {
                lineRenderer.startWidth += LeserWidth * (Time.deltaTime / 0.3f);
                lineRenderer.endWidth += LeserWidth * (Time.deltaTime / 0.3f);
            }
            else
            {
                float d1 = _mTool.Angle_360Y((ParentCloyster.TargetPosition - (Vector2)ParentCloyster.transform.position), Vector3.right) + (ParentCloyster.isEmptyConfusionDone ? Random.Range(-30, 30) : 0);
                float d2 = _mTool.Angle_360Y((Quaternion.AngleAxis(transform.rotation.eulerAngles.z, Vector3.forward) * Vector2.right).normalized, Vector3.right);
                if (d1 > 180) { d1 = d1 - 360; }
                if (d2 > 180) { d2 = d2 - 360; }
                float d = d1 - d2;
                if (d > 180) { d = d - 360; }
                else if (d < -180) { d = 360 + d; }
                if (d < 60 && d > 0) { d = 60; }
                if (d > -60 && d < 0) { d = -60; }
                transform.rotation = Quaternion.AngleAxis(d * Time.deltaTime, Vector3.forward) * transform.rotation;
                StartVFX.transform.position = transform.position + Vector3.up + (Quaternion.AngleAxis(transform.rotation.eulerAngles.z, Vector3.forward) * Vector3.right).normalized * 0.7f;
            }
            rayPosition();
        }
        else
        {
            rayPosition();
        }
    }



    private void rayPosition()
    {
        Physics2D.queriesHitTriggers = false;
        //������߻��еĶ���
        RaycastHit2D hitinfo = Physics2D.Raycast(transform.position + Vector3.up + (Quaternion.AngleAxis(transform.rotation.eulerAngles.z, Vector3.forward) * Vector3.right).normalized * 0.7f, ( Quaternion.AngleAxis(transform.rotation.eulerAngles.z , Vector3.forward)*Vector3.right ).normalized , 20, LayerMask.GetMask("Player", "PlayerFly", "PlayerJump", "Enviroment", "Room"));
        if (ParentCloyster.isEmptyInfatuationDone) { hitinfo = Physics2D.Raycast(transform.position + Vector3.up + (Quaternion.AngleAxis(transform.rotation.eulerAngles.z, Vector3.forward) * Vector3.right).normalized * 0.7f, (Quaternion.AngleAxis(transform.rotation.eulerAngles.z, Vector3.forward) * Vector3.right).normalized, 20, LayerMask.GetMask("Empty", "EmptyFly", "EmptyJump", "Enviroment", "Room")); }
        
        Vector2 EndPoint = hitinfo.point;
        if ((hitinfo))
        {
            EndPoint = hitinfo.point;
            RaycastHit2D EndRay = hitinfo;
            if (EndPoint == Vector2.zero) { EndPoint = hitinfo.point; }
            //������ез������Σ�������˺�
            if (!ParentCloyster.isEmptyInfatuationDone) 
            {
                if (EndRay.collider != null && EndRay.collider.gameObject.tag == "Player")
                {
                    PlayerControler p = EndRay.collider.GetComponent<PlayerControler>();
                    Pokemon.PokemonHpChange(ParentCloyster.gameObject, EndRay.collider.gameObject, 0, 1, 0, Type.TypeEnum.IgnoreType);
                }
            }
            else
            {
                if (EndRay.collider.gameObject != ParentCloyster.gameObject && InfatuationDmageCDTimer <= 0)
                {
                    if (EndRay.collider != null && EndRay.collider.gameObject.tag == "Empty")
                    {
                        Empty e = EndRay.collider.GetComponent<Empty>();
                        Pokemon.PokemonHpChange(ParentCloyster.gameObject, EndRay.collider.gameObject, 0, 1, 0, Type.TypeEnum.IgnoreType);
                        InfatuationDmageCDTimer = 1.2f;
                    }
                }
            }
            //EndPoint = transform.position + (Quaternion.AngleAxis(transform.rotation.eulerAngles.z, Vector3.forward) * Vector3.right).normalized * 8;
            //����л��ж��󣬽���ʼ����յ�ֱ��Ӧ
            lineRenderer.SetPosition(0, transform.position + Vector3.up + (Quaternion.AngleAxis(transform.rotation.eulerAngles.z, Vector3.forward) * Vector3.right).normalized * 0.7f);
            lineRenderer.SetPosition(1, EndPoint);

        }
        else
        {
            //����������ʾ
            lineRenderer.SetPosition(0, transform.position + Vector3.up + (Quaternion.AngleAxis(transform.rotation.eulerAngles.z, Vector3.forward) * Vector3.right).normalized * 0.7f);
            lineRenderer.SetPosition(1, transform.position + Vector3.up + (Quaternion.AngleAxis(transform.rotation.eulerAngles.z, Vector3.forward) * Vector3.right).normalized * 0.7f + (Quaternion.AngleAxis(transform.rotation.eulerAngles.z, Vector3.forward) * Vector3.right).normalized * 20);
            EndPoint = transform.position + Vector3.up + (Quaternion.AngleAxis(transform.rotation.eulerAngles.z, Vector3.forward) * Vector3.right).normalized * 1.2f + (Quaternion.AngleAxis(transform.rotation.eulerAngles.z, Vector3.forward) * Vector3.right).normalized * 20;
        }
        EndVFX.transform.position = EndPoint;



    }
}
