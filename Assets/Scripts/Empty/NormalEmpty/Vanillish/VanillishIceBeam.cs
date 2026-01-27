using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VanillishIceBeam : MonoBehaviour
{
    public Vanillish ParentVanillish;
    public float Rotation;

    float RayTimer;

    private LineRenderer lineRenderer;

    //激光中部的子激光
    private LineRenderer SonlineRenderer;

    //获取激光的初始宽度；
    float LeserWidth;

    //获取起始特效
    GameObject StartVFX;
    //获取末尾特效
    GameObject EndVFX;
    bool isPSStop;

    //方向
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

        //限制射线出现的位置，防止出现在出生点
        lineRenderer.SetPosition(0, transform.position + Vector3.up);
        lineRenderer.SetPosition(1, transform.position + Vector3.up);

        //初始化激光宽度
        LeserWidth = lineRenderer.endWidth;
        lineRenderer.startWidth = 0;
        lineRenderer.endWidth = 0;

        //获取组件
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
        if (ParentVanillish != null && !ParentVanillish.isEmptyFrozenDone && !ParentVanillish.isSleepDone && !ParentVanillish.isCanNotMoveWhenParalysis && !ParentVanillish.isSilence)
        {
            RayTimer += Time.deltaTime;
            if (!isPSStop)
            {
                if (RayTimer < 0.5)
                {
                    lineRenderer.startWidth += LeserWidth * (Time.deltaTime / 0.5f);
                    lineRenderer.endWidth += LeserWidth * (Time.deltaTime / 0.5f);
                }
                /**
                if (ParentVanillish.havePartnerState == Vanillish.HavePartnerState.Father && ParentVanillish.ParentEmptyByChild != null)
                {
                    float rotation = _mTool.Angle_360Y( (Vector3)(ParentVanillish.transform.position - ParentVanillish.ParentEmptyByChild.transform.position).normalized , Vector3.right);
                    transform.rotation = Quaternion.AngleAxis(rotation, Vector3.forward);
                }
                else
                {
                    
                }
                **/
                //发射角度
                float rotationSpeed = 0;
                //Debug.Log(ParentVanillish.havePartnerState);
                //无伙伴状态
                if (ParentVanillish.havePartnerState == Vanillish.HavePartnerState.No)
                {
                    float d1 = _mTool.Angle_360Y((ParentVanillish.TARGET_POSITION - (Vector2)ParentVanillish.transform.position), Vector3.right);
                    float d2 = _mTool.Angle_360Y((Quaternion.AngleAxis(transform.rotation.eulerAngles.z, Vector3.forward) * Vector2.right).normalized, Vector3.right);
                    if (d1 > 180) { d1 = d1 - 360; }
                    if (d2 > 180) { d2 = d2 - 360; }
                    rotationSpeed = d1 - d2;
                    if (rotationSpeed > 180) { rotationSpeed = rotationSpeed - 360; }
                    else if (rotationSpeed < -180) { rotationSpeed = 360 + rotationSpeed; }
                    if (rotationSpeed < 0) { rotationSpeed = -Vanillish.BEAM_ROTATION_SPEED_SINGLE; }
                    else { rotationSpeed = Vanillish.BEAM_ROTATION_SPEED_SINGLE; }
                }
                //伙伴状态
                else if (ParentVanillish.havePartnerState == Vanillish.HavePartnerState.Partner)
                {
                    rotationSpeed = Vanillish.BEAM_ROTATION_SPEED_PARTNER;
                    if (ParentVanillish.positionInPartnership == Empty.PositionInPartnershipEnum.BigBrother) { rotationSpeed *= (ParentVanillish.isReverseBeam ? -1.0f : 2.0f); }
                    else { rotationSpeed *= (ParentVanillish.isReverseBeam ? 2.0f : -1.0f); }
                }
                else if (ParentVanillish.havePartnerState == Vanillish.HavePartnerState.Father && ParentVanillish.ParentEmptyByChild != null)
                {
                    float TargetRotation = _mTool.Angle_360Y((Vector3)(ParentVanillish.transform.position - ParentVanillish.ParentEmptyByChild.transform.position).normalized, Vector3.right);
                    float NowRotation = transform.rotation.eulerAngles.z;
                    float delta = (TargetRotation - NowRotation + 360) % 360;
                    if (delta > 5) { rotationSpeed = 100; }
                    else if (delta < -5) { rotationSpeed = -100; }
                    else { rotationSpeed = 0; }
                    //Debug.Log(delta);
                }
                rotationSpeed *= (ParentVanillish.isEmptyConfusionDone ? 0.4f : 1.0f) * ((Weather.GlobalWeather.isHail || Weather.GlobalWeather.isHailPlus) ? 2.0f : 1.0f);
                transform.rotation = Quaternion.AngleAxis(rotationSpeed * Time.deltaTime, Vector3.forward) * transform.rotation;
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

        }
        else
        {
            if (!isPSStop)
            {
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
        }
        SetSonBeam();
    }


    public void StopBeam()
    {
        //ParentVanillish.NowLunchIceBeam = null;
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
        ParentVanillish = null;
        Destroy(this.gameObject);
    }

    void SetSonBeam()
    {
        if (SonlineRenderer != null)
        {
            SonlineRenderer.startWidth = lineRenderer.startWidth * 0.18f;
            SonlineRenderer.endWidth = lineRenderer.endWidth * 0.18f;
            SonlineRenderer.SetPosition(0, lineRenderer.GetPosition(0));
            SonlineRenderer.SetPosition(1, lineRenderer.GetPosition(1));
        }
    }


    private void rayPosition()
    {
        Physics2D.queriesHitTriggers = false;
        //检测射线击中的对象
        Vector3 t = (Quaternion.AngleAxis(transform.rotation.eulerAngles.z + 90, Vector3.forward) * Vector3.right).normalized;
        Vector3 b = (Quaternion.AngleAxis(transform.rotation.eulerAngles.z - 90, Vector3.forward) * Vector3.right).normalized;
        RaycastHit2D hitinfo = Physics2D.Raycast(transform.position + Vector3.up, (Quaternion.AngleAxis(transform.rotation.eulerAngles.z, Vector3.forward) * Vector3.right).normalized, 20, LayerMask.GetMask("Player", "PlayerFly", "PlayerJump", "Enviroment", "Room"));
        RaycastHit2D hitinfoTop = Physics2D.Raycast(transform.position + t * (lineRenderer.startWidth / 6.0f) + Vector3.up, (Quaternion.AngleAxis(transform.rotation.eulerAngles.z, Vector3.forward) * Vector3.right).normalized, 20f, LayerMask.GetMask("Player", "PlayerFly", "PlayerJump", "Enviroment", "Room"));
        RaycastHit2D hitinfoBottom = Physics2D.Raycast(transform.position + b * (lineRenderer.startWidth / 6.0f) + Vector3.up, (Quaternion.AngleAxis(transform.rotation.eulerAngles.z, Vector3.forward) * Vector3.right).normalized, 20f, LayerMask.GetMask("Player", "PlayerFly", "PlayerJump", "Enviroment", "Room"));
        if (ParentVanillish.isEmptyInfatuationDone)
        {
            int mask = LayerMask.GetMask("Empty", "EmptyFly", "EmptyJump", "Enviroment", "Room");
            List<Collider2D> ignore = new List<Collider2D> { ParentVanillish.GetComponent<Collider2D>() };
            hitinfo = _mTool.RaycastIgnoreCollider(transform.position + Vector3.up, (Quaternion.AngleAxis(transform.rotation.eulerAngles.z, Vector3.forward) * Vector3.right).normalized, 20f, mask , ignore);
            hitinfoTop = _mTool.RaycastIgnoreCollider(transform.position + t * (lineRenderer.startWidth / 6.0f) + Vector3.up, (Quaternion.AngleAxis(transform.rotation.eulerAngles.z, Vector3.forward) * Vector3.right).normalized, 20f, mask, ignore);
            hitinfoBottom = _mTool.RaycastIgnoreCollider(transform.position + b * (lineRenderer.startWidth / 6.0f) + Vector3.up, (Quaternion.AngleAxis(transform.rotation.eulerAngles.z, Vector3.forward) * Vector3.right).normalized, 20f, mask, ignore);
            //Debug.DrawRay(transform.position + Vector3.up, (Quaternion.AngleAxis(transform.rotation.eulerAngles.z, Vector3.forward) * Vector3.right).normalized * hitinfo.distance, Color.red);
            //Debug.DrawRay(transform.position + t * (lineRenderer.startWidth / 6.0f) + Vector3.up, (Quaternion.AngleAxis(transform.rotation.eulerAngles.z, Vector3.forward) * Vector3.right).normalized * hitinfoTop.distance, Color.red);
            //Debug.DrawRay(transform.position + b * (lineRenderer.startWidth / 6.0f) + Vector3.up, (Quaternion.AngleAxis(transform.rotation.eulerAngles.z, Vector3.forward) * Vector3.right).normalized * hitinfoBottom.distance, Color.red);
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
            //Debug.Log(EndPoint);
            //Debug.Log(EndRay.collider.gameObject.name);

            //如果击中敌方宝可梦，则造成伤害
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
                        p.PlayerFrozenFloatPlus(0.3f, 1.2f);
                    }
                }
            }
            else
            {
                if (EndRay.collider != null && EndRay.collider.gameObject != ParentVanillish.gameObject && InfatuationDmageCDTimer <= 0)
                {
                    if (EndRay.collider != null && EndRay.collider.gameObject.tag == "Empty")
                    {
                        Empty e = EndRay.collider.GetComponent<Empty>();
                        Pokemon.PokemonHpChange(ParentVanillish.gameObject, EndRay.collider.gameObject, 0, 120, 0, PokemonType.TypeEnum.Ice);
                        InfatuationDmageCDTimer = 1.2f;
                    }
                }
            }
            //如果有击中对象，将起始点和终点分别对应
            lineRenderer.SetPosition(0, transform.position + Vector3.up + ((Vector3)EndPoint - transform.position).normalized * 1.4f);
            lineRenderer.SetPosition(1, EndPoint);

        }
        else
        {
            //否则正常显示
            lineRenderer.SetPosition(0, transform.position + Vector3.up + (Quaternion.AngleAxis(transform.rotation.eulerAngles.z, Vector3.forward) * Vector3.right).normalized * 1.4f);
            lineRenderer.SetPosition(1, transform.position + Vector3.up + (Quaternion.AngleAxis(transform.rotation.eulerAngles.z, Vector3.forward) * Vector3.right).normalized * 20);
            EndPoint = transform.position + Vector3.up + (Quaternion.AngleAxis(transform.rotation.eulerAngles.z, Vector3.forward) * Vector3.right).normalized * 1.2f + (Quaternion.AngleAxis(transform.rotation.eulerAngles.z, Vector3.forward) * Vector3.right).normalized * 20;
        }
        EndVFX.transform.position = EndPoint;

        ParentVanillish.SetDirector(_mTool.TiltMainVector2((EndPoint - (Vector2)transform.position).normalized));



    }
}
