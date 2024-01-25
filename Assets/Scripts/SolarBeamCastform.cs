using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolarBeamCastform : Projectile
{
    float LeserTimer;
    private LineRenderer lineRenderer;
    //获取激光的初始宽度；
    float LeserWidth;

    //获取起始特效
    GameObject StartVFX;
    //获取末尾特效
    GameObject EndVFX;



    ParticleSystem RingPS;




    private void Awake()
    {
        lineRenderer = GetComponentInChildren<LineRenderer>();
    }
    // Start is called before the first frame update
    void Start()
    {

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
        //检测射线击中的对象
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

            //如果击中敌方宝可梦，则造成伤害
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
            //如果有击中对象，将起始点和终点分别对应
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, EndPoint);

        }
        else
        {
            //否则正常显示
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
