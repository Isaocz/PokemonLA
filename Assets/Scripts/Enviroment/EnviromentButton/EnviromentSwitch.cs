using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnviromentSwitch : MonoBehaviour
{
    //按钮切换时动画帧间隔
    public static float pading = 0.05f;

    /// <summary>
    /// 按钮是否被摁下
    /// </summary>
    public bool isON = false;

    /// <summary>
    /// 按钮是否正在被改变状态 被摁下则不可再被改变状态
    /// </summary>
    protected enum changeState
    {
        idle,
        change,
    };
    protected changeState ChangeState = changeState.idle;



    //开关的三状态
    public Sprite Switch1;
    public Sprite Switch2;
    public Sprite Switch3;

    //底板的二状态
    public Sprite Bottom1;
    public Sprite Bottom2;

    /// <summary>
    /// 开关的Sprite渲染器
    /// </summary>
    protected SpriteRenderer SwitchRenderer;
    /// <summary>
    /// 底板的Sprite渲染器
    /// </summary>
    protected SpriteRenderer BottomRenderer;
    /// <summary>
    /// 音效播放器
    /// </summary>
    protected AudioSource SE;



    /// <summary>
    /// 开关摁下
    /// </summary>
    protected IEnumerator SwitchON()
    {
        isON = true;
        SE.Play();
        ChangeState = changeState.change;
        SwitchONEvent();
        SetSwitch02();
        yield return new WaitForSeconds(pading);
        ChangeState = changeState.idle;
        SetSwitch03();
    }


    /// <summary>
    /// 开关不摁下
    /// </summary>
    protected IEnumerator SwitchOFF()
    {
        isON = false;
        SE.Play();
        ChangeState = changeState.change;
        SwitchOFFEvent();
        SetSwitch02();
        yield return new WaitForSeconds(pading);
        ChangeState = changeState.idle;
        SetSwitch01();
    }


    /// <summary>
    /// 设置开关为1状态（完全弹起）
    /// </summary>
    void SetSwitch01()
    {
        SwitchRenderer.sprite = Switch1;
        BottomRenderer.sprite = Bottom1;
        SwitchRenderer.sortingOrder = 10;
    }

    /// <summary>
    /// 设置开关为2状态（中间状态）
    /// </summary>
    void SetSwitch02()
    {
        SwitchRenderer.sprite = Switch2;
        BottomRenderer.sprite = Bottom1;
        SwitchRenderer.sortingOrder = 10;
    }

    /// <summary>
    /// 设置开关为3状态（完全摁下）
    /// </summary>
    void SetSwitch03()
    {
        SwitchRenderer.sprite = Switch3;
        BottomRenderer.sprite = Bottom2;
        SwitchRenderer.sortingOrder = -1;
    }


    private void Start()
    {
        SwitchRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        BottomRenderer = transform.GetChild(1).GetComponent<SpriteRenderer>();
        SE = transform.GetComponent<AudioSource>();
    }


    /// <summary>
    /// 按钮按下时的事件
    /// </summary>
    public virtual void SwitchONEvent()
    {

    }

    /// <summary>
    /// 按钮弹起时的事件
    /// </summary>
    public virtual void SwitchOFFEvent()
    {

    }

}
