using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerGem : Skill
{
    private LineRenderer lineRenderer;
    //��ȡ����ĳ�ʼ��ȣ�
    float LeserWidth;


    //��ȡ��ʼ��Ч
    GameObject StartVFX;
    //��ȡĩβ��Ч
    GameObject EndVFX;


    public int StoneAngle;

    //������ʯ�����ķ����
    Vector3 CenterPoint;
    //�����Ŀ�ꡣ
    Vector3 LaserTarget;
    bool isStartOver;
    
    public bool isParentStone = true;
    GameObject Stone;

    int StoneCount = 2;

    public PowerGem LaserPowerGem;
    Vector3 InstantiatePosition;

    private void Awake()
    {
        lineRenderer = GetComponentInChildren<LineRenderer>();
    }
    // Start is called before the first frame update
    void Start()
    {
        if (isParentStone) { InstantiatePosition = transform.position; transform.position = player.transform.position + Vector3.up * player.SkillOffsetforBodySize[0]; }
        Invoke("StartLate" , 0.1f);

    }

    void StartLate()
    {

        if (isParentStone)
        {


            for (int i = 0; i < StoneCount + 1;i++)
            {
                PowerGem p = Instantiate(LaserPowerGem, InstantiatePosition, transform.rotation, transform.parent);
                p.StoneAngle = (360 / (StoneCount + 1 ))*i; p.isParentStone = false;
                p.player = player;
                p.SpDamage = SpDamage;
                p.Damage = Damage;
                p.CTLevel = CTLevel;
                p.CTDamage = CTDamage;
                p.MaxRange = MaxRange;
                p.KOPoint = KOPoint;
            }
        }



       
        if (!isParentStone) {
            transform.position += Quaternion.AngleAxis(transform.rotation.eulerAngles.z, Vector3.forward) * Vector3.right * 1.6f;
            CenterPoint = transform.position;
            LaserTarget = CenterPoint + Quaternion.AngleAxis(transform.rotation.eulerAngles.z, Vector3.forward) * Vector3.right * MaxRange;

            transform.position = player.transform.position + Quaternion.AngleAxis(StoneAngle, Vector3.forward) * (transform.position - (player.transform.position + Vector3.up * player.SkillOffsetforBodySize[0])) + Vector3.up * player.SkillOffsetforBodySize[0];

            //�������߳��ֵ�λ�ã���ֹ�����ڳ�����
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, transform.position);

            //��ʼ��������
            LeserWidth = lineRenderer.endWidth;
            lineRenderer.startWidth = 0;
            lineRenderer.endWidth = 0;

            //��ȡ���
            EndVFX = transform.GetChild(2).gameObject;
            StartVFX = transform.GetChild(1).gameObject;
            Stone = transform.GetChild(1).GetChild(3).gameObject;
            Stone.transform.rotation = Quaternion.Euler(0, 0, _mTool.Angle_360((LaserTarget - transform.position).normalized, Vector2.up));

            rayPosition();
        }
        isStartOver = true;
    }

    // Update is called once per frame
    void Update()
    {

        if (isStartOver && !isParentStone) {
            CenterPoint = ( player.transform.position + Vector3.up * player.SkillOffsetforBodySize[0]) + Quaternion.AngleAxis(transform.rotation.eulerAngles.z, Vector3.forward) * Vector3.right * 1.6f;
            LaserTarget = CenterPoint + Quaternion.AngleAxis(transform.rotation.eulerAngles.z, Vector3.forward) * Vector3.right * MaxRange;
            if (ExistenceTime > 0.3f)
            {
                if (ExistenceTime > 0.85)
                {
                    lineRenderer.startWidth += LeserWidth * (Time.deltaTime / 0.15f);
                    lineRenderer.endWidth += LeserWidth * (Time.deltaTime / 0.15f);
                }
                else if (ExistenceTime < 0.5f)
                {
                    lineRenderer.startWidth -= LeserWidth * (Time.deltaTime / 0.15f);
                    lineRenderer.endWidth -= LeserWidth * (Time.deltaTime / 0.15f);
                }

                rayPosition();

            }
            else if(lineRenderer.gameObject.activeSelf)
            {
                Animator a = Stone.GetComponent<Animator>();
                Debug.Log(a);
                a.SetBool("Break", true);
                lineRenderer.gameObject.SetActive(false);
                EndVFX.SetActive(false);
            }

        }
        StartExistenceTimer();
    }



    private void rayPosition()
    {


        if (!isParentStone) {


            Physics2D.queriesHitTriggers = false;
            //������߻��еĶ���
            RaycastHit2D hitinfo = Physics2D.Raycast(transform.position, (LaserTarget - transform.position).normalized, (LaserTarget - transform.position).magnitude, LayerMask.GetMask("Empty", "EmptyFly", "Enviroment", "Room"));


            Vector2 EndPoint = hitinfo.point;
            if ((hitinfo))
            {
                EndPoint = hitinfo.point;
                RaycastHit2D EndRay = hitinfo;

                if (EndPoint == Vector2.zero) { EndPoint = hitinfo.point; }


                //������ез������Σ�������˺�
                if (EndRay.collider != null && EndRay.collider.gameObject.tag == "Empty")
                {
                    Empty target = EndRay.collider.GetComponent<Empty>();

                    HitAndKo(target);

                }
                //����л��ж��󣬽���ʼ����յ�ֱ��Ӧ
                lineRenderer.SetPosition(0, transform.position);
                lineRenderer.SetPosition(1, EndPoint);

            }
            else
            {
                //����������ʾ
                lineRenderer.SetPosition(0, transform.position);
                lineRenderer.SetPosition(1, LaserTarget);
                EndPoint = LaserTarget;
            }
            EndVFX.transform.position = EndPoint;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Spike" && isParentStone && SkillFrom == 2)
        {
            StealthRockPrefabs stealthRock = other.GetComponent<StealthRockPrefabs>();
            if (stealthRock != null)
            {
                stealthRock.Exittime = 0;
                StoneCount ++;
            }
        }
    }

}
