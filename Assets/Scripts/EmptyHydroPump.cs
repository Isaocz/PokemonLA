using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyHydroPump : MonoBehaviour
{
    public Empty ParentEmpty;
    public float Rotation;

    float RayTimer;
     
    private LineRenderer lineRenderer;
    //��ȡ����ĳ�ʼ��ȣ�
    float LeserStartWidth;
    float LeserEndWidth;
    float MaxLength;

    //��ȡ��ʼ��Ч
    GameObject StartVFX;
    //��ȡĩβ��Ч
    GameObject EndVFX;
    ParticleSystem RingPS;
    ParticleSystem RayPS;

    bool isPSStop;




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
        LeserStartWidth = lineRenderer.startWidth;
        LeserEndWidth = lineRenderer.endWidth;
        //lineRenderer.startWidth = 0;
        //lineRenderer.endWidth = 0;

        //��ȡ���
        EndVFX = transform.GetChild(2).gameObject;
        StartVFX = transform.GetChild(1).gameObject;
        StartVFX.transform.position = transform.position + Vector3.up;
        MaxLength = 1;

        RingPS = transform.GetChild(1).GetChild(1).GetComponent<ParticleSystem>();
        RayPS = transform.GetChild(1).GetChild(3).GetComponent<ParticleSystem>();

    }

    // Update is called once per frame
    void Update()
    {
        if ((ParentEmpty.isSleepDone || ParentEmpty.isFearDone || ParentEmpty.isDie) && RayTimer < 9.95f)
        {
            RayTimer = 9.95f;
        }
        RayTimer += Time.deltaTime;
        StartVFX.transform.position = transform.position ;
        rayPosition();
        if (RayTimer >= 10)
        {
            lineRenderer.startWidth -= Time.deltaTime* LeserStartWidth/0.8f;
            lineRenderer.endWidth -= Time.deltaTime * LeserEndWidth / 0.8f;
            StartVFX.transform.localScale -= Time.deltaTime * new Vector3(1,1,1) / 0.8f;
            EndVFX.transform.localScale -= Time.deltaTime * new Vector3(1,1,1) / 0.8f;
            if (RayTimer >= 10.9) { Destroy(gameObject); }
        }
    }



    private void rayPosition()
    {
        MaxLength = Mathf.Clamp( MaxLength + 10 * Time.deltaTime , 0 ,20 );
        Physics2D.queriesHitTriggers = false;
        //������߻��еĶ���
        Vector3 t = (Quaternion.AngleAxis(transform.rotation.eulerAngles.z + 90, Vector3.forward) * Vector3.right).normalized;
        Vector3 b = (Quaternion.AngleAxis(transform.rotation.eulerAngles.z - 90, Vector3.forward) * Vector3.right).normalized;
        RaycastHit2D hitinfo = Physics2D.Raycast(transform.position, (Quaternion.AngleAxis(transform.rotation.eulerAngles.z, Vector3.forward) * Vector3.right).normalized, MaxLength, LayerMask.GetMask("Player", "PlayerFly", "PlayerJump", "Enviroment", "Room"));
        RaycastHit2D hitinfoTop = Physics2D.Raycast(transform.position + t * (lineRenderer.startWidth/2), (Quaternion.AngleAxis(transform.rotation.eulerAngles.z, Vector3.forward) * Vector3.right).normalized, MaxLength, LayerMask.GetMask("Player", "PlayerFly", "PlayerJump", "Enviroment", "Room"));
        RaycastHit2D hitinfoBottom = Physics2D.Raycast(transform.position + b * (lineRenderer.startWidth/2), (Quaternion.AngleAxis(transform.rotation.eulerAngles.z, Vector3.forward) * Vector3.right).normalized, MaxLength, LayerMask.GetMask("Player", "PlayerFly", "PlayerJump", "Enviroment", "Room"));
        Vector2 EndPoint = hitinfo.point;
        if ((hitinfo) || (hitinfoTop) || (hitinfoBottom))
        {
            //Debug.Log(hitinfo.point + "+" + hitinfoTop.point + "+" + hitinfoBottom.point);
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
            if (hitinfo.point != Vector2.zero && hitinfoTop.point != Vector2.zero && hitinfoBottom.point != Vector2.zero) {
                if ((hitinfoTop.point - hitinfo.point).magnitude >= 0.5f && (hitinfoTop.point - (Vector2)transform.position).magnitude < (hitinfo.point - (Vector2)transform.position).magnitude + 0.5f)
                {
                    EndRay = hitinfoTop; EndPoint = hitinfoTop.point - (Vector2)t * (lineRenderer.startWidth / 2.0f);
                }
                if ((hitinfoBottom.point - hitinfo.point).magnitude >= 0.5f && (hitinfoBottom.point - (Vector2)transform.position).magnitude < (hitinfo.point - (Vector2)transform.position).magnitude + 0.5f)
                {
                    EndRay = hitinfoBottom; EndPoint = hitinfoBottom.point - (Vector2)b * (lineRenderer.startWidth / 2.0f);

                }
            }

            if (EndPoint == Vector2.zero) { EndPoint = hitinfo.point; }


            //������ез������Σ�������˺�
            if (EndRay.collider != null && EndRay.collider.gameObject.tag == "Player")
            {
                PlayerControler p = EndRay.collider.GetComponent<PlayerControler>();
                if (ParentEmpty != null) { Pokemon.PokemonHpChange(ParentEmpty.gameObject, EndRay.collider.gameObject, 0, 100, 0, Type.TypeEnum.Water); }
                else { Pokemon.PokemonHpChange(null, EndRay.collider.gameObject, 0, 80, 0, Type.TypeEnum.Water); }
                if (p != null)
                {
                    p.KnockOutPoint = 10f;
                    p.KnockOutDirection = (p.transform.position - transform.parent.position).normalized;
                }
            }
            //����л��ж��󣬽���ʼ����յ�ֱ��Ӧ
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, EndPoint);

        }
        else
        {
            //����������ʾ
            lineRenderer.SetPosition(0, transform.position );
            lineRenderer.SetPosition(1, transform.position  + (Quaternion.AngleAxis(transform.rotation.eulerAngles.z, Vector3.forward) * Vector3.right).normalized * MaxLength);
            EndPoint = lineRenderer.GetPosition(1);
        }
        EndVFX.transform.position = EndPoint;
        float Length = (EndVFX.transform.position - transform.position).magnitude;

        var RingPSShape = RingPS.shape;
        RingPSShape.radius = Length * 0.45f;
        RingPSShape.position = new Vector3(Length * 0.45f + 0.1f, 0, 0);
        var RingPSEmission = RingPS.emission;
        RingPSEmission.rateOverTime = (int)Length * 7.5f;

        var RayPSShape = RayPS.shape;
        RayPSShape.radius = Length/2;
        RayPSShape.position = new Vector3(Length/2,0,0);
        var RayPSEmission = RayPS.emission;
        RayPSEmission.rateOverTime = (int)Length * 75;



    }
}
