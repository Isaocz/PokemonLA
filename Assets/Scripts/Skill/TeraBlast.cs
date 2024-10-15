using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TeraBlast : Skill
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


    private void Awake()
    {
        lineRenderer = GetComponentInChildren<LineRenderer>();
    }
    // Start is called before the first frame update
    void Start()
    {
        //如果玩家的攻击大于特攻，则招式变为攻击招式
        if (player.AtkAbilityPoint > player.SpAAbilityPoint)
        {
            Damage = SkillFrom==2 ? 90 : 80;
            SpDamage = 0;
        }
        //如果在本房间内太晶化过，则招式属性变为太晶属性
        if (0 != player.PlayerTeraTypeJOR)
        {
            SkillType = player.PlayerTeraTypeJOR;
        }

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
        //组件颜色变化
        Debug.Log(lineRenderer.GetPosition(0));
        Debug.Log(lineRenderer.GetPosition(1));
        SetRayColor();


    }

    // Update is called once per frame
    void Update()
    {
        if (SkillFrom == 2)
        {
            if (ExistenceTime > 1.35)
            {
                lineRenderer.startWidth += LeserWidth * (Time.deltaTime / 0.15f);
                lineRenderer.endWidth += LeserWidth * (Time.deltaTime / 0.15f);
            }
            else if (ExistenceTime < 0.15)
            {
                lineRenderer.startWidth -= LeserWidth * (Time.deltaTime / 0.15f);
                lineRenderer.endWidth -= LeserWidth * (Time.deltaTime / 0.15f);
            }
        }
        else
        {
            if (ExistenceTime > 0.85)
            {
                lineRenderer.startWidth += LeserWidth * (Time.deltaTime / 0.15f);
                lineRenderer.endWidth += LeserWidth * (Time.deltaTime / 0.15f);
            }
            else if (ExistenceTime < 0.15)
            {
                lineRenderer.startWidth -= LeserWidth * (Time.deltaTime / 0.15f);
                lineRenderer.endWidth -= LeserWidth * (Time.deltaTime / 0.15f);
            }
        }

        rayPosition();
        StartExistenceTimer();
        if (!RingPSSpeedDown)
        {
            RingPSSpeedDown = true;
            float LeserLength = (lineRenderer.GetPosition(0) - lineRenderer.GetPosition(1)).magnitude;
            //var RingPSVofL = StartVFX.transform.GetChild(0).GetComponent<ParticleSystem>().velocityOverLifetime;
            //RingPSVofL.z = 13*(LeserLength/8);
            //RingPSVofL.orbitalZ = 39*(LeserLength/8);

        }
    }


    void SetRayColor() 
    {
        if (SkillType != 1)
        {
            Color LaserColor = PokemonType.TypeColor[SkillType];
            LaserColor.a = 1;
            Color LaserColor2 = PokemonType.TypeColor2[SkillType];
            LaserColor2.a = 1;
            //Color LaserColor3 = new Color(Type.TypeColor[SkillType].x - 0.1f, 2 * Type.TypeColor[SkillType].x - 0.1f, 2 * Type.TypeColor[SkillType].z - 0.1f, 1);
            lineRenderer.startColor = PokemonType.TypeColor[SkillType];
            lineRenderer.endColor = LaserColor2;

            var mainS0 = StartVFX.transform.GetChild(0).GetComponent<ParticleSystem>().main;
            mainS0.startColor = LaserColor;
            var mainS1 = StartVFX.transform.GetChild(1).GetComponent<ParticleSystem>().main;
            mainS1.startColor = LaserColor;
            var mainS2 = StartVFX.transform.GetChild(2).GetComponent<ParticleSystem>().main;
            mainS2.startColor = LaserColor;
            var mainS3 = StartVFX.transform.GetChild(3).GetComponent<ParticleSystem>().main;
            mainS3.startColor = LaserColor;

            var mainE0 = EndVFX.transform.GetChild(0).GetComponent<ParticleSystem>().main;
            mainE0.startColor = LaserColor2;
            var mainE1 = EndVFX.transform.GetChild(1).GetComponent<ParticleSystem>().main;
            mainE1.startColor = LaserColor2;
        }

    }


    private void rayPosition()
    {
        Physics2D.queriesHitTriggers = false;
        //检测射线击中的对象
        RaycastHit2D hitinfo = Physics2D.Raycast(transform.position ,transform.right , 8 , LayerMask.GetMask("Empty" , "EmptyFly" , "Enviroment" , "Room") );
        RaycastHit2D hitinfoTop = Physics2D.Raycast(transform.position + Vector3.up * 0.3f,transform.right , 8f , LayerMask.GetMask("Empty" , "EmptyFly" , "Enviroment" , "Room") );
        RaycastHit2D hitinfoBottom = Physics2D.Raycast(transform.position - Vector3.up * 0.3f, transform.right , 8f , LayerMask.GetMask("Empty" , "EmptyFly" , "Enviroment" , "Room"));

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
            if (EndRay.collider != null && EndRay.collider.gameObject.tag == "Empty" )
            {
                Empty target = EndRay.collider.GetComponent<Empty>();
                HitAndKo(target);
            }
            if (DirectionRight) { EndPoint.y = transform.position.y; }
            else{ EndPoint.x = transform.position.x; }
            //如果有击中对象，将起始点和终点分别对应
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, EndPoint);

        }
        else
        {
            //否则正常显示
            lineRenderer.SetPosition(0, transform.position) ;
            lineRenderer.SetPosition(1, transform.position + transform.right * 8);
            EndPoint = transform.position + transform.right * 8;
        }
        EndVFX.transform.position = EndPoint;



    }
}
