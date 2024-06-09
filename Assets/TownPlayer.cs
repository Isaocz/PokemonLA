using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using TMPro;

public class TownPlayer : MonoBehaviour
{


    //=======================================角色的数据===================================
    /// <summary>
    /// 角色中文名
    /// </summary>
    public string PlayerNameChinese;
    /// <summary>
    /// 角色头像图标
    /// </summary>
    public Sprite PlayerHead;
    /// <summary>
    /// 角色体型 0小体型 1中体型 2大体形
    /// </summary>
    public int PlayerBodySize;
    /// <summary>
    /// 根据角色体型大小，技能释放的位置，SkillOffsetforBodySize[0]为中心点y轴偏移量，SkillOffsetforBodySize[1]为x轴偏移量，SkillOffsetforBodySize[2]y轴偏移量，
    /// </summary>
    public float[] SkillOffsetforBodySize;
    /// <summary>
    /// 声明一个2D刚体组件，以获得角色的刚体组件
    /// </summary>
    new Rigidbody2D rigidbody2D;
    Animator animator;
    /// <summary>
    /// 声明两个变量，获取方向键按键信息
    /// </summary>
    public float PlayerMoveHorizontal { get { return horizontal; } }
    private float horizontal;
    public float PlayerMoveVertical { get { return vertical; } }
    private float vertical;
    /// <summary>
    /// 声明一个2D向量变量，以存储刚体的二维坐标
    /// </summary>
    Vector2 position;


    //声明一个二维向量表示朝向最初朝向右边,另一个表示位移量,
    public Vector2 look = new Vector2(0, -1);
    Vector2 move;
    Vector2 Direction;

    //布尔型变量表示玩家是否触发Z互动 触发后其他互动不可再被触发
    public bool isInZ
    {
        get { return isinz; }
        set { isinz = value; }
    }
    public bool isinz = false;

    public float Weight;


    //声明六个整形数据，表示角色的六项种族值,以及六项当前能力值
    public int HpPlayerPoint;
    public int AtkPlayerPoint;
    public int SpAPlayerPoint;
    public int DefPlayerPoint;
    public int SpdPlayerPoint;
    public int SpeedPlayerPoint;
    public int MoveSpePlayerPoint;
    public int LuckPlayerPoint;


    //声明玩家的两个属性
    public int PlayerType01;
    public int PlayerType02;
    public int PlayerTeraType;
    public int PlayerTeraTypeJOR;

    public float speed = 6.0f;






    public bool isTP;
    public bool isTPMove;


    PlayerControler ThisPlayer;


    public bool isCanNotMove;
    public bool isCameraStop;
    public bool isCanNotTurnDirection;
    public bool isInvincibleAlways;





    /// <summary>
    /// 角色图片的相对位置 ，在跳跃之后会恢复为此值
    /// </summary>
    public Vector3 PlayerLocalPosition
    {
        get { return playerLocalPosition; }
        set { playerLocalPosition = value; }
    }
    Vector3 playerLocalPosition = Vector3.zero;


    /// <summary>
    /// 角色图片的相对缩放 ，在跳跃之后会恢复为此值
    /// </summary>
    public Vector3 PlayerLocalScal
    {
        get { return playerLocalScal; }
        set { playerLocalScal = value; }
    }
    Vector3 playerLocalScal = new Vector3(1.0f, 1.0f, 1.0f);

    public Vector2 MoveSpeed
    {
        get { return moveSpeed; }
        set { moveSpeed = value; }
    }
    Vector2 moveSpeed;

    //=================================初始化=====================================


    //初始化玩家的必要函数
    /// <summary>
    /// 初始化
    /// </summary>
    protected void Instance()
    {


        //获得小山猪的刚体组件和动画组件
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        look = Vector2.down;
        ThisPlayer = GetComponent<PlayerControler>();



        playerLocalPosition = transform.GetChild(2).localPosition;
        playerLocalScal = transform.GetChild(2).localScale;
    }


    //=================================初始化=====================================


    public void SkillNow() { }


    // Update is called once per frame
    protected void UpdatePlayer()
    {
        //每帧获取一次十字键的按键信息
        {
            moveSpeed = Vector2.zero;
            if (SystemInfo.operatingSystemFamily == OperatingSystemFamily.Other)
            {
                if (MoveStick.joystick != null && MoveStick.joystick.Horizontal != 0 || MoveStick.joystick.Vertical != 0)
                {
                    Vector2 StickVector = new Vector2(MoveStick.joystick.Horizontal, MoveStick.joystick.Vertical).normalized;
                    float a = _mTool.Angle_360Y(StickVector, Vector2.right);
                    if (a > 22.5f && a <= 67.5f) { moveSpeed = new Vector2(1f, 1f); }
                    else if (a > 67.5f && a <= 112.5f) { moveSpeed = new Vector2(0f, 1f); }
                    else if (a > 112.5f && a <= 157.5f) { moveSpeed = new Vector2(-1f, 1f); }
                    else if (a > 157.5f && a <= 202.5f) { moveSpeed = new Vector2(-1f, 0f); }
                    else if (a > 202.5f && a <= 247.5f) { moveSpeed = new Vector2(-1f, -1f); }
                    else if (a > 247.5f && a <= 292.5f) { moveSpeed = new Vector2(0f, -1f); }
                    else if (a > 292.5f && a <= 337.5f) { moveSpeed = new Vector2(1f, -1f); }
                    else { moveSpeed = new Vector2(1f, 0f); }
                }
            }

            if (Input.GetKey(InitializePlayerSetting.GlobalPlayerSetting.GetKeybind("Left")))
            {
                moveSpeed.x = -1f;
            }
            else if (Input.GetKey(InitializePlayerSetting.GlobalPlayerSetting.GetKeybind("Right")))
            {
                moveSpeed.x = 1f;
            }

            if (Input.GetKey(InitializePlayerSetting.GlobalPlayerSetting.GetKeybind("Up")))
            {
                moveSpeed.y = 1f;
            }
            else if (Input.GetKey(InitializePlayerSetting.GlobalPlayerSetting.GetKeybind("Down")))
            {
                moveSpeed.y = -1f;
            }

            if (moveSpeed != Vector2.zero)
            {
                moveSpeed = moveSpeed.normalized;
            }
            horizontal = moveSpeed.x;
            vertical = moveSpeed.y;

        }
    }







    protected void FixedUpdatePlayer()
    {
        //2D向量position等于刚体组件的坐标,之后让position的xy坐标加上十字键x速度x单位时间，最后让刚体的位置等于position
        if ( !isTP && !isCanNotMove)
        {
            position = rigidbody2D.position;
            position.x = position.x + horizontal * speed * Time.deltaTime;
            position.y = position.y + vertical * speed * Time.deltaTime;
            rigidbody2D.position = position;



            //位移变量为十字键操纵值
            move = new Vector2(horizontal, vertical);

            //仅当发生位移时可以改变动画，如果不位移动画不会改变
            if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
            {
                move = new Vector2(horizontal, vertical);
                if (move.x > move.y && -move.x > move.y)
                {
                    look.Set(0, -1);
                }
                else if (move.x > move.y && -move.x <= move.y)
                {
                    look.Set(1, 0);
                }
                else if (move.x <= move.y && -move.x > move.y)
                {
                    look.Set(-1, 0);
                }
                else if (move.x <= move.y && -move.x <= move.y)
                {
                    look.Set(0, 1);
                }
            }

            if (!isCanNotTurnDirection)
            {
                animator.SetFloat("LookX", look.x);
                animator.SetFloat("LookY", look.y);
            }
            animator.SetFloat("Speed", move.magnitude);
        }
    }

































    //====================================================TP====================================================

    Vector3Int TpVector3;
    /// <summary>
    /// 输入开始tp
    /// </summary>
    /// <param name="TPVector3"></param>
    public void TP(Vector3Int TPVector3)
    {
        TpVector3 = TPVector3;
        animator.SetTrigger("TP");
        isTP = true;
        isTPMove = true;
    }

    /// <summary>
    /// 开始黑屏
    /// </summary>
    public void TPStart()
    {
        TPMask.In.TPStart = true;
        TPMask.In.BlackTime = 1.15f;
    }

    /// <summary>
    /// 移动
    /// </summary>
    public void TPDoit()
    {
        gameObject.transform.position = new Vector3(30.0f * TpVector3.x, 24.0f * TpVector3.y - 2.0f, 0);
        GameObject Maincamera = GameObject.FindWithTag("MainCamera");
        Maincamera.transform.position = new Vector3(30.0f * TpVector3.x, 24.0f * TpVector3.y + 0.7f, -10);
        //UiMiniMap.Instance.MiniMapMove(new Vector3(NowRoom.x - TpVector3.x, NowRoom.y - TpVector3.y, 0));
        //MapCreater.StaticMap.RRoom[NowRoom].GetAllItem();
        //NowRoom = TpVector3;
        //InANewRoom = true;
        UiMiniMap.Instance.SeeMapOver();
    }

    /// <summary>
    /// 结束黑屏
    /// </summary>
    public void TPEnd()
    {
        isTP = false;
        isTPMove = false;
    }


    //====================================================TP====================================================




}

