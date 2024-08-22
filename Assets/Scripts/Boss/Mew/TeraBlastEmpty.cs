using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class TeraBlastEmpty : Projectile
{
    public float laserDuration = 0.5f;
    float angle;//接收传入的角度
    public Vector3 startpoint;
    public Vector3 endpoint;
    Mew mew;

    Vector3 direction;
    private LineRenderer lineRenderer;
    private float timer;

    //获取起始特效
    GameObject StartVFX;
    GameObject EndVFX;

    private void Awake()
    {
        mew = FindObjectOfType<Mew>();
        lineRenderer = GetComponent<LineRenderer>();
    }
    private void Start()
    {
        direction = (endpoint - startpoint).normalized;
        EndVFX = transform.GetChild(1).gameObject;
        StartVFX = transform.GetChild(0).gameObject;
        transform.rotation = Quaternion.Euler(0, 0, angle);
        //限制射线出现的位置，防止出现在出生点
        lineRenderer.SetPosition(0, startpoint);
        lineRenderer.SetPosition(1, endpoint);

        lineRenderer.enabled = false;
        EndVFX.SetActive(false);
        StartVFX.SetActive(true);
    }
    void Update()
    {
        //计时器工作
        timer += Time.deltaTime;
        if(timer < 1f)
        {
            lineRenderer.enabled = false;
            EndVFX.SetActive(false);
            StartVFX.SetActive(true);
        }
        else if(timer >= 1f && timer < 1.4f)
        {
            lineRenderer.enabled = true;
            EndVFX.SetActive(true);

            endpoint = startpoint + new Vector3(40f * Mathf.Cos(Mathf.Deg2Rad * angle), 40f * Mathf.Sin(Mathf.Deg2Rad * angle), 0f);
            direction = (endpoint - startpoint).normalized;
            //修改射线的位置
            lineRenderer.SetPosition(0, startpoint);
            lineRenderer.SetPosition(1, endpoint);
            rayPosition();
        }
        else if(timer >= 1.4f)
        {
            Destroy(gameObject);
        }
    }

    private void rayPosition()
    {
        //检测射线击中的对象
        RaycastHit2D hitinfo = Physics2D.Raycast(transform.position, direction, 40f, LayerMask.GetMask("Player", "Enviroment", "Room"));
        RaycastHit2D hitinfoTop = Physics2D.Raycast(transform.position + Vector3.up * 0.3f, direction, 40f, LayerMask.GetMask("Player", "Enviroment", "Room"));
        RaycastHit2D hitinfoBottom = Physics2D.Raycast(transform.position - Vector3.up * 0.3f, direction, 40f, LayerMask.GetMask("Player", "Enviroment", "Room"));
        Vector2 EndPoint = hitinfo.point;
        if (hitinfo.collider == null && hitinfoTop.collider == null && hitinfoBottom.collider == null)
        {
            EndPoint = Vector2.zero; // 设置默认值
        }
        if ((hitinfo) || (hitinfoTop) || (hitinfoBottom))
        {
            endpoint = hitinfo.point;
            RaycastHit2D EndRay = hitinfo;
            if ((hitinfoTop.point - hitinfo.point).magnitude >= 0.5f)
            {
                EndRay = hitinfoTop; endpoint = hitinfoTop.point;
            }
            if ((hitinfoBottom.point - hitinfo.point).magnitude >= 0.5f)
            {
                EndRay = hitinfoBottom; endpoint = hitinfoBottom.point;

            }
            if (EndPoint == Vector2.zero) { endpoint = hitinfo.point; }


            //如果击中玩家，则对玩家造成伤害
            if (EndRay.collider != null && EndRay.collider.gameObject.tag == "Player")
            {
                PlayerControler playerControler = EndRay.collider.GetComponent<PlayerControler>();
                Pokemon.PokemonHpChange(empty.gameObject, EndRay.collider.gameObject, 0, SpDmage, 0, Type.TypeEnum.Normal);
                if (playerControler != null)
                {
                    playerControler.KnockOutPoint = 2.5f;
                    playerControler.KnockOutDirection = (playerControler.transform.position - transform.position).normalized;
                }
            }
            //如果有击中对象，将起始点和终点分别对应
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, EndPoint);
        }
        else
        {
            //否则正常显示
            lineRenderer.SetPosition(0, startpoint);
            lineRenderer.SetPosition(1, endpoint);
        }
        EndVFX.transform.position = endpoint;
    }
    public void SetEndpoints(Vector3 Startpoint, Vector3 Endpoint, float Angle)
    {
        //接收传入点和角度
        startpoint = Startpoint;
        endpoint = Endpoint;
        angle = Angle;
    }

}