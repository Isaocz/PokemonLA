using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class TeraBlastEmpty : Projectile
{
    public float laserDuration = 1.0f;
    public Color initialColor = Color.yellow;
    public Color finalColor = Color.red;
    float angle;//接收传入的角度
    public float rotationSpeed = 120f;
    public Vector3 startpoint;
    public Vector3 endpoint;
    Mew mew;

    Vector3 direction;
    bool isSafe = true;

    private LineRenderer lineRenderer;
    private int shootingTime;

    //获取起始特效
    GameObject StartVFX;
    //获取末尾特效
    GameObject EndVFX;

    private void Awake()
    {
        mew = FindObjectOfType<Mew>();
        lineRenderer = GetComponent<LineRenderer>();
    }
    private void Start()
    {
        shootingTime = 1;
        // 获取从起始点指向终点的标准向量
        direction = (endpoint - startpoint).normalized;
        isSafe = true;

        lineRenderer.startColor = initialColor;
        lineRenderer.endColor = initialColor;
        //获取组件
        EndVFX = transform.Find("EndVFX").gameObject;
        StartVFX = transform.Find("StartVFX").gameObject;
        //限制射线出现的位置，防止出现在出生点
        lineRenderer.SetPosition(0, startpoint);
        lineRenderer.SetPosition(1, endpoint);
        //修改激光颜色
        Invoke("ChangeColor", laserDuration);
    }
    void Update()
    {
        angle += Time.deltaTime * rotationSpeed;
        endpoint = startpoint + new Vector3(40f * Mathf.Cos(Mathf.Deg2Rad * angle), 40f * Mathf.Sin(Mathf.Deg2Rad * angle), 0f);
        direction = (endpoint - startpoint).normalized;
        //修改射线的位置
        lineRenderer.SetPosition(0, startpoint);
        lineRenderer.SetPosition(1, endpoint);
        rayPosition();
    }
    private void ChangeColor()
    {
        //变色
        lineRenderer.startColor = finalColor;
        lineRenderer.endColor= finalColor;
        isSafe = false;
        if (mew.currentPhase == 1||shootingTime==3)
        Invoke("Delete", 2f);
        else
        {
            StartCoroutine(Continue());
            IEnumerator Continue()
            {
                yield return new WaitForSeconds(2f);
                if (shootingTime <= 3)
                {
                    shootingTime++;
                    ContinueShooting();
                }
            }
        }
    }
    private void Delete()
    {
        Destroy(gameObject);
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
            EndPoint = hitinfo.point;
            RaycastHit2D EndRay = hitinfo;
            if ((hitinfoTop.point - hitinfo.point).magnitude >= 0.5f)
            {
                EndRay = hitinfoTop; EndPoint = hitinfoTop.point;
            }
            if ((hitinfoBottom.point - hitinfo.point).magnitude >= 0.5f)
            {
                EndRay = hitinfoBottom; EndPoint = hitinfoBottom.point;

            }
            if (EndPoint == Vector2.zero) { EndPoint = hitinfo.point; }


            //如果击中玩家，则对玩家造成伤害
            if (EndRay.collider != null && EndRay.collider.gameObject.tag == "Player" && isSafe == false)
            {
                PlayerControler playerControler = EndRay.collider.GetComponent<PlayerControler>();
                Pokemon.PokemonHpChange(empty.gameObject, playerControler.gameObject, 0, SpDmage, 0, Type.TypeEnum.IgnoreType);
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
    public void SetColors(Color StartColor, Color EndColor)
    {
        //传入颜色
        initialColor = StartColor;
        finalColor = EndColor;
    }

    void ContinueShooting()
    {
        lineRenderer.startColor = initialColor;
        lineRenderer.endColor = initialColor;
        isSafe = true;
        Invoke("ChangeColor", laserDuration);
    }
}