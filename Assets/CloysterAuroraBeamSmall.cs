using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloysterAuroraBeamSmall : MonoBehaviour
{
    public Cloyster ParentCloyster;
    public float Rotation;

    float RayTimer;

    private LineRenderer lineRenderer;
    //获取激光的初始宽度；
    float LeserWidth;

    //获取起始特效
    GameObject StartVFX;
    //获取末尾特效
    GameObject EndVFX;

    //方向
    bool RingPSSpeedDown;
    float InfatuationDmageCDTimer;


    private void Awake()
    {
        lineRenderer = GetComponentInChildren<LineRenderer>();
    }
    // Start is called before the first frame update
    void Start()
    {

        //限制射线出现的位置，防止出现在出生点
        lineRenderer.SetPosition(0, transform.position + Vector3.up + (Quaternion.AngleAxis(transform.rotation.eulerAngles.z, Vector3.forward) * Vector3.right).normalized * 0.7f);
        lineRenderer.SetPosition(1, transform.position + Vector3.up + (Quaternion.AngleAxis(transform.rotation.eulerAngles.z, Vector3.forward) * Vector3.right).normalized * 0.7f);

        //初始化激光宽度
        LeserWidth = lineRenderer.endWidth;
        lineRenderer.startWidth = 0;
        lineRenderer.endWidth = 0;

        //获取组件
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
        //检测射线击中的对象
        RaycastHit2D hitinfo = Physics2D.Raycast(transform.position + Vector3.up + (Quaternion.AngleAxis(transform.rotation.eulerAngles.z, Vector3.forward) * Vector3.right).normalized * 0.7f, ( Quaternion.AngleAxis(transform.rotation.eulerAngles.z , Vector3.forward)*Vector3.right ).normalized , 20, LayerMask.GetMask("Player", "PlayerFly", "PlayerJump", "Enviroment", "Room"));
        if (ParentCloyster.isEmptyInfatuationDone) { hitinfo = Physics2D.Raycast(transform.position + Vector3.up + (Quaternion.AngleAxis(transform.rotation.eulerAngles.z, Vector3.forward) * Vector3.right).normalized * 0.7f, (Quaternion.AngleAxis(transform.rotation.eulerAngles.z, Vector3.forward) * Vector3.right).normalized, 20, LayerMask.GetMask("Empty", "EmptyFly", "EmptyJump", "Enviroment", "Room")); }
        
        Vector2 EndPoint = hitinfo.point;
        if ((hitinfo))
        {
            EndPoint = hitinfo.point;
            RaycastHit2D EndRay = hitinfo;
            if (EndPoint == Vector2.zero) { EndPoint = hitinfo.point; }
            //如果击中敌方宝可梦，则造成伤害
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
            //如果有击中对象，将起始点和终点分别对应
            lineRenderer.SetPosition(0, transform.position + Vector3.up + (Quaternion.AngleAxis(transform.rotation.eulerAngles.z, Vector3.forward) * Vector3.right).normalized * 0.7f);
            lineRenderer.SetPosition(1, EndPoint);

        }
        else
        {
            //否则正常显示
            lineRenderer.SetPosition(0, transform.position + Vector3.up + (Quaternion.AngleAxis(transform.rotation.eulerAngles.z, Vector3.forward) * Vector3.right).normalized * 0.7f);
            lineRenderer.SetPosition(1, transform.position + Vector3.up + (Quaternion.AngleAxis(transform.rotation.eulerAngles.z, Vector3.forward) * Vector3.right).normalized * 0.7f + (Quaternion.AngleAxis(transform.rotation.eulerAngles.z, Vector3.forward) * Vector3.right).normalized * 20);
            EndPoint = transform.position + Vector3.up + (Quaternion.AngleAxis(transform.rotation.eulerAngles.z, Vector3.forward) * Vector3.right).normalized * 1.2f + (Quaternion.AngleAxis(transform.rotation.eulerAngles.z, Vector3.forward) * Vector3.right).normalized * 20;
        }
        EndVFX.transform.position = EndPoint;



    }
}
