using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteelBeam : Skill
{
    private LineRenderer lineRenderer;
    //获取激光的初始宽度；
    float LeserWidth;

    //获取起始特效
    GameObject StartVFX;
    //获取末尾特效
    GameObject EndVFX;

    //方向
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
        if (SkillFrom == 2) {
            int DCount = Mathf.Clamp(player.playerData.DefBounsAlways + player.playerData.DefBounsJustOneRoom + player.playerData.SpDBounsAlways + player.playerData.SpDBounsJustOneRoom, 0, 4);
            if (DCount < 4 && DCount > 0)
            {
                player.playerData.DefBounsJustOneRoom -= player.playerData.DefBounsAlways + player.playerData.DefBounsJustOneRoom;
                player.playerData.SpDBounsJustOneRoom -= player.playerData.SpDBounsAlways + player.playerData.SpDBounsJustOneRoom;
            }
            else if (DCount == 4)
            {
                if (player.playerData.DefBounsAlways + player.playerData.DefBounsJustOneRoom == 1) { player.playerData.DefBounsJustOneRoom -= 1; player.playerData.SpDBounsJustOneRoom -= 3; }
                else if (player.playerData.SpDBounsAlways + player.playerData.SpDBounsJustOneRoom == 1) { player.playerData.DefBounsJustOneRoom -= 3; player.playerData.SpDBounsJustOneRoom -= 1; }
                else { player.playerData.DefBounsJustOneRoom -= 2; player.playerData.SpDBounsJustOneRoom -= 2; }
            }

            player.ReFreshAbllityPoint();

            if (DCount < 4) {
                Pokemon.PokemonHpChange(null, player.gameObject, (player.Hp * (4 - DCount)) / 12.0f, 0, 0, PokemonType.TypeEnum.IgnoreType);
            }
        }
        else
        {
            Pokemon.PokemonHpChange(null, player.gameObject, (player.Hp * (4)) / 12.0f, 0, 0, PokemonType.TypeEnum.IgnoreType);
        }
        player.KnockOutPoint = 0;
        player.KnockOutDirection = Vector2.zero;


        if (transform.rotation.eulerAngles == new Vector3(0, 0, 0) || transform.rotation.eulerAngles == new Vector3(0, 0, 180)) { DirectionRight = true; }
        else if (transform.rotation.eulerAngles == new Vector3(0, 0, 90) || transform.rotation.eulerAngles == new Vector3(0, 0, 270)) { DirectionRight = false; }

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
        //检测射线击中的对象
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


            //如果击中敌方宝可梦，则造成伤害
            if (EndRay.collider != null && EndRay.collider.gameObject.tag == "Empty")
            {
                Empty target = EndRay.collider.GetComponent<Empty>();
                
                HitAndKo(target);
            }
            if (DirectionRight) { EndPoint.y = transform.position.y; }
            else { EndPoint.x = transform.position.x; }
            //如果有击中对象，将起始点和终点分别对应
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, EndPoint);

        }
        else
        {
            //否则正常显示
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, transform.position + transform.right * MaxRange);
            EndPoint = transform.position + transform.right * MaxRange;
        }
        EndVFX.transform.position = EndPoint;



    }
}
