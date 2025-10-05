using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnviromentSwitch : MonoBehaviour
{
    //��ť�л�ʱ����֡���
    public static float pading = 0.05f;

    /// <summary>
    /// ��ť�Ƿ�����
    /// </summary>
    public bool isON = false;

    /// <summary>
    /// ��ť�Ƿ����ڱ��ı�״̬ �������򲻿��ٱ��ı�״̬
    /// </summary>
    protected enum changeState
    {
        idle,
        change,
    };
    protected changeState ChangeState = changeState.idle;



    //���ص���״̬
    public Sprite Switch1;
    public Sprite Switch2;
    public Sprite Switch3;

    //�װ�Ķ�״̬
    public Sprite Bottom1;
    public Sprite Bottom2;

    /// <summary>
    /// ���ص�Sprite��Ⱦ��
    /// </summary>
    protected SpriteRenderer SwitchRenderer;
    /// <summary>
    /// �װ��Sprite��Ⱦ��
    /// </summary>
    protected SpriteRenderer BottomRenderer;
    /// <summary>
    /// ��Ч������
    /// </summary>
    protected AudioSource SE;



    /// <summary>
    /// ��������
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
    /// ���ز�����
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
    /// ���ÿ���Ϊ1״̬����ȫ����
    /// </summary>
    void SetSwitch01()
    {
        SwitchRenderer.sprite = Switch1;
        BottomRenderer.sprite = Bottom1;
        SwitchRenderer.sortingOrder = 10;
    }

    /// <summary>
    /// ���ÿ���Ϊ2״̬���м�״̬��
    /// </summary>
    void SetSwitch02()
    {
        SwitchRenderer.sprite = Switch2;
        BottomRenderer.sprite = Bottom1;
        SwitchRenderer.sortingOrder = 10;
    }

    /// <summary>
    /// ���ÿ���Ϊ3״̬����ȫ���£�
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
    /// ��ť����ʱ���¼�
    /// </summary>
    public virtual void SwitchONEvent()
    {

    }

    /// <summary>
    /// ��ť����ʱ���¼�
    /// </summary>
    public virtual void SwitchOFFEvent()
    {

    }

}
