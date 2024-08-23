using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class TeraBlastEmpty : Projectile
{
    public float laserDuration = 0.5f;
    float angle;//���մ���ĽǶ�
    public Vector3 startpoint;
    public Vector3 endpoint;
    Mew mew;

    Vector3 direction;
    private LineRenderer lineRenderer;
    private float timer;

    //��ȡ��ʼ��Ч
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
        //�������߳��ֵ�λ�ã���ֹ�����ڳ�����
        lineRenderer.SetPosition(0, startpoint);
        lineRenderer.SetPosition(1, endpoint);

        lineRenderer.enabled = false;
        EndVFX.SetActive(false);
        StartVFX.SetActive(true);
    }
    void Update()
    {
        //��ʱ������
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
            //�޸����ߵ�λ��
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
        //������߻��еĶ���
        RaycastHit2D hitinfo = Physics2D.Raycast(transform.position, direction, 40f, LayerMask.GetMask("Player", "Enviroment", "Room"));
        RaycastHit2D hitinfoTop = Physics2D.Raycast(transform.position + Vector3.up * 0.3f, direction, 40f, LayerMask.GetMask("Player", "Enviroment", "Room"));
        RaycastHit2D hitinfoBottom = Physics2D.Raycast(transform.position - Vector3.up * 0.3f, direction, 40f, LayerMask.GetMask("Player", "Enviroment", "Room"));
        Vector2 EndPoint = hitinfo.point;
        if (hitinfo.collider == null && hitinfoTop.collider == null && hitinfoBottom.collider == null)
        {
            EndPoint = Vector2.zero; // ����Ĭ��ֵ
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


            //���������ң�����������˺�
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
            //����л��ж��󣬽���ʼ����յ�ֱ��Ӧ
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, EndPoint);
        }
        else
        {
            //����������ʾ
            lineRenderer.SetPosition(0, startpoint);
            lineRenderer.SetPosition(1, endpoint);
        }
        EndVFX.transform.position = endpoint;
    }
    public void SetEndpoints(Vector3 Startpoint, Vector3 Endpoint, float Angle)
    {
        //���մ����ͽǶ�
        startpoint = Startpoint;
        endpoint = Endpoint;
        angle = Angle;
    }

}