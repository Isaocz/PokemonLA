using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using TMPro;

public class TownPlayer : MonoBehaviour
{


    //=======================================��ɫ������===================================
    /// <summary>
    /// ��ɫ������
    /// </summary>
    public string PlayerNameChinese;
    /// <summary>
    /// ��ɫͷ��ͼ��
    /// </summary>
    public Sprite PlayerHead;
    /// <summary>
    /// ��ɫ���� 0С���� 1������ 2������
    /// </summary>
    public int PlayerBodySize;
    /// <summary>
    /// ���ݽ�ɫ���ʹ�С�������ͷŵ�λ�ã�SkillOffsetforBodySize[0]Ϊ���ĵ�y��ƫ������SkillOffsetforBodySize[1]Ϊx��ƫ������SkillOffsetforBodySize[2]y��ƫ������
    /// </summary>
    public float[] SkillOffsetforBodySize;
    /// <summary>
    /// ����һ��2D����������Ի�ý�ɫ�ĸ������
    /// </summary>
    new Rigidbody2D rigidbody2D;
    Animator animator;
    /// <summary>
    /// ����������������ȡ�����������Ϣ
    /// </summary>
    public float PlayerMoveHorizontal { get { return horizontal; } }
    private float horizontal;
    public float PlayerMoveVertical { get { return vertical; } }
    private float vertical;
    /// <summary>
    /// ����һ��2D�����������Դ洢����Ķ�ά����
    /// </summary>
    Vector2 position;


    //����һ����ά������ʾ������������ұ�,��һ����ʾλ����,
    public Vector2 look = new Vector2(0, -1);
    Vector2 move;
    Vector2 Direction;

    //�����ͱ�����ʾ����Ƿ񴥷�Z���� �������������������ٱ�����
    public bool isInZ
    {
        get { return isinz; }
        set { isinz = value; }
    }
    public bool isinz = false;

    public float Weight;


    //���������������ݣ���ʾ��ɫ����������ֵ,�Լ����ǰ����ֵ
    public int HpPlayerPoint;
    public int AtkPlayerPoint;
    public int SpAPlayerPoint;
    public int DefPlayerPoint;
    public int SpdPlayerPoint;
    public int SpeedPlayerPoint;
    public int MoveSpePlayerPoint;
    public int LuckPlayerPoint;


    //������ҵ���������
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
    /// ��ɫͼƬ�����λ�� ������Ծ֮���ָ�Ϊ��ֵ
    /// </summary>
    public Vector3 PlayerLocalPosition
    {
        get { return playerLocalPosition; }
        set { playerLocalPosition = value; }
    }
    Vector3 playerLocalPosition = Vector3.zero;


    /// <summary>
    /// ��ɫͼƬ��������� ������Ծ֮���ָ�Ϊ��ֵ
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

    //=================================��ʼ��=====================================


    //��ʼ����ҵı�Ҫ����
    /// <summary>
    /// ��ʼ��
    /// </summary>
    protected void Instance()
    {


        //���Сɽ��ĸ�������Ͷ������
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        look = Vector2.down;
        ThisPlayer = GetComponent<PlayerControler>();



        playerLocalPosition = transform.GetChild(2).localPosition;
        playerLocalScal = transform.GetChild(2).localScale;
    }


    //=================================��ʼ��=====================================


    public void SkillNow() { }


    // Update is called once per frame
    protected void UpdatePlayer()
    {
        //ÿ֡��ȡһ��ʮ�ּ��İ�����Ϣ
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
        //2D����position���ڸ������������,֮����position��xy�������ʮ�ּ�x�ٶ�x��λʱ�䣬����ø����λ�õ���position
        if ( !isTP && !isCanNotMove)
        {
            position = rigidbody2D.position;
            position.x = position.x + horizontal * speed * Time.deltaTime;
            position.y = position.y + vertical * speed * Time.deltaTime;
            rigidbody2D.position = position;



            //λ�Ʊ���Ϊʮ�ּ�����ֵ
            move = new Vector2(horizontal, vertical);

            //��������λ��ʱ���Ըı䶯���������λ�ƶ�������ı�
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
    /// ���뿪ʼtp
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
    /// ��ʼ����
    /// </summary>
    public void TPStart()
    {
        TPMask.In.TPStart = true;
        TPMask.In.BlackTime = 1.15f;
    }

    /// <summary>
    /// �ƶ�
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
    /// ��������
    /// </summary>
    public void TPEnd()
    {
        isTP = false;
        isTPMove = false;
    }


    //====================================================TP====================================================




}

