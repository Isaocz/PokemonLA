using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CetitanIceBeam : MonoBehaviour
{





    //==============================音效枚举===================================

    /// <summary>
    /// 音效种类枚举
    /// </summary>
    public enum IceBeamSE
    {
        IceBeam,
        IceBeamBlast,
    }

    /// <summary>
    /// 一次性音效播放器生成
    /// </summary>
    public EnemyAudioPlayer audioPlayer;
    /// <summary>
    /// 循环音效音效播放器生成
    /// </summary>
    public LoopingSEAudioPlayer loopPlayer;

    //==============================音效枚举===================================







    public Cetitan ParentCetitan;

    /// <summary>
    /// 初始发射位置增加量
    /// </summary>
    public float StartLocationPosition;

    /// <summary>
    /// 是否顺时针旋转
    /// </summary>
    public bool isTurnClockwise;





    /// <summary>
    /// 伤害（特攻）
    /// </summary>
    public int IceBeamSpDmage = 90;
    /// <summary>
    /// 击退值
    /// </summary>
    public float IceBeamKOPoint = 7.5f;
    /// <summary>
    /// 冰冻值
    /// </summary>
    public float IceBeamFrozenPoint = 0.5f;





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

    /// <summary>
    /// 激光旋转速度
    /// </summary>
    static float rotationSpeed
    {
        get { return (Cetitan.ANGLE_ICEBEAM_START - Cetitan.ANGLE_ICEBEAM_OVER) / Cetitan.TIME_LAUNCH_ICEBEAM_LAUNCH; }
    }




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

        var clip = audioPlayer.sfxTable.GetClip(IceBeamSE.IceBeam.ToString());
        loopPlayer.PlayLoop(clip, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        if (ParentCetitan != null )
        {
            RayTimer += Time.deltaTime;
            if (!isPSStop)
            {
                if (RayTimer < 0.5)
                {
                    lineRenderer.startWidth += LeserWidth * (Time.deltaTime / 0.5f);
                    lineRenderer.endWidth += LeserWidth * (Time.deltaTime / 0.5f);
                }
                //发射角速度
                if (ParentCetitan.IsMove_Launch_Angry_IceBeam) {
                    //Debug.Log(RayTimer + "+" + rotationSpeed);
                    transform.rotation = Quaternion.AngleAxis((isTurnClockwise ? 1 : -1) * rotationSpeed * Time.deltaTime, Vector3.forward) * transform.rotation;

                } StartVFX.transform.position = lineRenderer.GetPosition(0);
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
                StopBeam();
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
        //ParentVaniluex.NowLunchIceBeam = null;
        //transform.GetChild(0).gameObject.SetActive(false);
        isPSStop = true;
        //StartVFX.transform.GetChild(0).GetComponent<ParticleSystem>().Stop();
        //StartVFX.transform.GetChild(1).GetComponent<ParticleSystem>().Stop();
        //StartVFX.transform.GetChild(2).GetComponent<ParticleSystem>().Stop();
        //StartVFX.transform.GetChild(3).GetComponent<ParticleSystem>().Stop();
        //StartVFX.transform.GetChild(4).GetComponent<ParticleSystem>().Stop();
        //EndVFX.transform.GetChild(0).GetComponent<ParticleSystem>().Stop();
        //EndVFX.transform.GetChild(1).GetComponent<ParticleSystem>().Stop();
        //EndVFX.transform.GetChild(2).GetComponent<ParticleSystem>().Stop();
        ParentCetitan.Lunch_IcicleCrash_Line(StartVFX.transform.position, EndVFX.transform.position, 0.01f, 1.0f);
        _mTool.RemoveAllPSChild(transform.gameObject);
        ParentCetitan = null;
        //Destroy(this.gameObject);
        loopPlayer.StopLoop(1f);
        audioPlayer.Play(IceBeamSE.IceBeamBlast, transform.position);
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
        RaycastHit2D hitinfo = Physics2D.Raycast(transform.position + Vector3.up, (Quaternion.AngleAxis(transform.rotation.eulerAngles.z, Vector3.forward) * Vector3.right).normalized, 100.0f, LayerMask.GetMask("Player", "PlayerFly", "PlayerJump", "Enviroment", "Room"));
        RaycastHit2D hitinfoTop = Physics2D.Raycast(transform.position + t * (lineRenderer.startWidth / 12.0f) + Vector3.up, (Quaternion.AngleAxis(transform.rotation.eulerAngles.z, Vector3.forward) * Vector3.right).normalized, 100.0f, LayerMask.GetMask("Player", "PlayerFly", "PlayerJump", "Enviroment", "Room"));
        RaycastHit2D hitinfoBottom = Physics2D.Raycast(transform.position + b * (lineRenderer.startWidth / 12.0f) + Vector3.up, (Quaternion.AngleAxis(transform.rotation.eulerAngles.z, Vector3.forward) * Vector3.right).normalized, 100.0f, LayerMask.GetMask("Player", "PlayerFly", "PlayerJump", "Enviroment", "Room"));

        Debug.DrawLine(transform.position + Vector3.up , hitinfo.point , Color.red);
        Debug.DrawLine(transform.position + t * (lineRenderer.startWidth / 100.0f) + Vector3.up, hitinfoTop.point , Color.red);
        Debug.DrawLine(transform.position + b * (lineRenderer.startWidth / 100.0f) + Vector3.up, hitinfoBottom.point , Color.red);



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


            //如果击中玩家宝可梦，则造成伤害
            if (EndRay.collider != null && EndRay.collider.gameObject.tag == "Player")
            {
                PlayerControler p = EndRay.collider.GetComponent<PlayerControler>();
                Pokemon.PokemonHpChange(ParentCetitan.gameObject, EndRay.collider.gameObject, 0, IceBeamSpDmage, 0, PokemonType.TypeEnum.Ice);
                if (p != null)
                {
                    p.KnockOutPoint = IceBeamKOPoint;
                    p.KnockOutDirection = (p.transform.position - transform.position).normalized;
                    p.PlayerFrozenFloatPlus(IceBeamFrozenPoint, 2.0f);
                }
            }



            if (EndRay.collider != null && EndRay.collider.gameObject.tag == "Enviroment")
            {
                IcicleCrashOBJ icobj = EndRay.collider.transform.GetComponent<IcicleCrashOBJ>();
                if (icobj != null)
                {
                    icobj.IceBreak();
                }
            }

            //Debug.Log(transform.position);
            //Debug.Log(transform.position + Vector3.up + ((Vector3)EndPoint - transform.position).normalized * StartLocationPosition);
            //如果有击中对象，将起始点和终点分别对应
            lineRenderer.SetPosition(0, transform.position + Vector3.up + ((Vector3)EndPoint - transform.position).normalized * StartLocationPosition);
            lineRenderer.SetPosition(1, EndPoint);

        }
        else
        {
            //Debug.Log(transform.position);
            //否则正常显示
            lineRenderer.SetPosition(0, transform.position + Vector3.up + (Quaternion.AngleAxis(transform.rotation.eulerAngles.z, Vector3.forward) * Vector3.right).normalized * StartLocationPosition);
            lineRenderer.SetPosition(1, transform.position + Vector3.up + (Quaternion.AngleAxis(transform.rotation.eulerAngles.z, Vector3.forward) * Vector3.right).normalized * 100.0f);
            EndPoint = transform.position + Vector3.up + (Quaternion.AngleAxis(transform.rotation.eulerAngles.z, Vector3.forward) * Vector3.right).normalized * 1.2f + (Quaternion.AngleAxis(transform.rotation.eulerAngles.z, Vector3.forward) * Vector3.right).normalized * 100.0f;
        }
        EndVFX.transform.position = EndPoint;





    }
}
