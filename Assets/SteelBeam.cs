using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteelBeam : Skill
{
    private LineRenderer lineRenderer;
    //��ȡ����ĳ�ʼ���ȣ�
    float LeserWidth;

    //��ȡ��ʼ��Ч
    GameObject StartVFX;
    //��ȡĩβ��Ч
    GameObject EndVFX;

    //����
    bool DirectionRight;
    bool RingPSSpeedDown;

    bool isSpDDown;

    private void Awake()
    {
        lineRenderer = GetComponentInChildren<LineRenderer>();
    }
    // Start is called before the first frame update
    void Start()
    {

        int DCount = Mathf.Clamp( player.playerData.DefBounsAlways + player.playerData.DefBounsJustOneRoom + player.playerData.SpDBounsAlways + player.playerData.SpDBounsJustOneRoom , 0 , 4);
        if (DCount < 4 && DCount > 0)
        {
            player.playerData.DefBounsJustOneRoom -= player.playerData.DefBounsAlways + player.playerData.DefBounsJustOneRoom;
            player.playerData.SpDBounsJustOneRoom -= player.playerData.SpDBounsAlways + player.playerData.SpDBounsJustOneRoom;
        }
        else if (DCount == 4)
        {
            if      (player.playerData.DefBounsAlways + player.playerData.DefBounsJustOneRoom == 1) { player.playerData.DefBounsJustOneRoom -= 1; player.playerData.SpDBounsJustOneRoom -= 3; }
            else if (player.playerData.SpDBounsAlways + player.playerData.SpDBounsJustOneRoom == 1) { player.playerData.DefBounsJustOneRoom -= 3; player.playerData.SpDBounsJustOneRoom -= 1; }
            else                                                                                    { player.playerData.DefBounsJustOneRoom -= 2; player.playerData.SpDBounsJustOneRoom -= 2; }
        }

        player.ReFreshAbllityPoint();

        if (DCount < 4) {
            Pokemon.PokemonHpChange(null, player.gameObject, (player.Hp * (4 - DCount)) / 12.0f, 0, 0, Type.TypeEnum.IgnoreType);
        }
        player.KnockOutPoint = 0;
        player.KnockOutDirection = Vector2.zero;



        if (transform.rotation.eulerAngles == new Vector3(0, 0, 0) || transform.rotation.eulerAngles == new Vector3(0, 0, 180)) { DirectionRight = true; }
        else if (transform.rotation.eulerAngles == new Vector3(0, 0, 90) || transform.rotation.eulerAngles == new Vector3(0, 0, 270)) { DirectionRight = false; }

        //�������߳��ֵ�λ�ã���ֹ�����ڳ�����
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, transform.position);

        //��ʼ���������
        LeserWidth = lineRenderer.endWidth;
        lineRenderer.startWidth = 0;
        lineRenderer.endWidth = 0;

        //��ȡ���
        EndVFX = transform.GetChild(2).gameObject;
        StartVFX = transform.GetChild(1).gameObject;


    }

    // Update is called once per frame
    void Update()
    {
        if (ExistenceTime > 1.5f)
        {
            if (SkillFrom == 2)
            {
                if (ExistenceTime > 3.75)
                {
                    lineRenderer.startWidth += LeserWidth * (Time.deltaTime / 0.15f);
                    lineRenderer.endWidth += LeserWidth * (Time.deltaTime / 0.15f);
                }
                else if (ExistenceTime < 1.75)
                {
                    lineRenderer.startWidth -= LeserWidth * (Time.deltaTime / 0.15f);
                    lineRenderer.endWidth -= LeserWidth * (Time.deltaTime / 0.15f);
                }
            }
            else
            {
                if (ExistenceTime > 3.85)
                {
                    lineRenderer.startWidth += LeserWidth * (Time.deltaTime / 0.15f);
                    lineRenderer.endWidth += LeserWidth * (Time.deltaTime / 0.15f);
                }
                else if (ExistenceTime < 1.7f)
                {
                    lineRenderer.startWidth -= LeserWidth * (Time.deltaTime / 0.15f);
                    lineRenderer.endWidth -= LeserWidth * (Time.deltaTime / 0.15f);
                }
            }

            rayPosition();

            if (!RingPSSpeedDown)
            {
                RingPSSpeedDown = true;
                float LeserLength = (lineRenderer.GetPosition(0) - lineRenderer.GetPosition(1)).magnitude;
                var RingPSVofL = StartVFX.transform.GetChild(0).GetComponent<ParticleSystem>().velocityOverLifetime;
                RingPSVofL.z = 13 * (LeserLength / 8);
                RingPSVofL.orbitalZ = 39 * (LeserLength / 8);

            }
        }
        else
        {
            lineRenderer.gameObject.SetActive(false);
            EndVFX.SetActive(false);
            StartVFX.SetActive(false);
        }
        StartExistenceTimer();
    }



    private void rayPosition()
    {



        Physics2D.queriesHitTriggers = false;
        //������߻��еĶ���
        RaycastHit2D hitinfo = Physics2D.Raycast(transform.position, transform.right, MaxRange, LayerMask.GetMask("Empty", "EmptyFly", "Enviroment", "Room"));
        RaycastHit2D hitinfoTop = Physics2D.Raycast(transform.position + Vector3.up * 0.4f, transform.right, MaxRange, LayerMask.GetMask("Empty", "EmptyFly", "Enviroment", "Room"));
        RaycastHit2D hitinfoBottom = Physics2D.Raycast(transform.position - Vector3.up * 0.4f, transform.right, MaxRange, LayerMask.GetMask("Empty", "EmptyFly", "Enviroment", "Room"));

        Vector2 EndPoint = hitinfo.point;
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


            //������ез������Σ�������˺�
            if (EndRay.collider != null && EndRay.collider.gameObject.tag == "Empty")
            {
                Empty target = EndRay.collider.GetComponent<Empty>();
                
                HitAndKo(target);
            }
            if (DirectionRight) { EndPoint.y = transform.position.y; }
            else { EndPoint.x = transform.position.x; }
            //����л��ж��󣬽���ʼ����յ�ֱ��Ӧ
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, EndPoint);

        }
        else
        {
            //����������ʾ
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, transform.position + transform.right * MaxRange);
            EndPoint = transform.position + transform.right * MaxRange;
        }
        EndVFX.transform.position = EndPoint;



    }
}