using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillArrow : MonoBehaviour
{
    /// <summary>
    /// 箭头角度
    /// </summary>
    public float ArrowAngle;

    /// <summary>
    /// 箭头身体长度（不算头部）
    /// </summary>
    public float ArrowLength;

    /// <summary>
    /// 箭头宽度
    /// </summary>
    public float ArrowScale;

    /// <summary>
    /// 箭头颜色
    /// </summary>
    public Color ArrowColor;



    /// <summary>
    /// 箭头头部
    /// </summary>
    public SpriteRenderer ArrowHead;

    /// <summary>
    /// 箭头身体
    /// </summary>
    public SpriteRenderer ArrowBody;

    SpriteRenderer ArrowLineHead;

    SpriteRenderer ArrowLineBody;



    /// <summary>
    /// 箭头是否跟随一个目标点移动
    /// </summary>
    public bool IsTargetMode 
    {
        get { return isTargetMode; }
        set { isTargetMode = value; }
    }
    bool isTargetMode = true;


    /// <summary>
    /// 目标点
    /// </summary>
    public Vector2 TargetPoint
    {
        get { return targetPoint; }
        set { targetPoint = value; }
    }
    Vector2 targetPoint;


    /// <summary>
    /// 目标物体
    /// </summary>
    public Transform targetTransform;


    /// <summary>
    /// 是否结束
    /// </summary>
    public bool IsOver
    {
        get { return isOver; }
        set { isOver = value; }
    }
    bool isOver;


    /// <summary>
    /// 淡入时间
    /// </summary>
    public float FadeInTime;

    /// <summary>
    /// 淡出时间
    /// </summary>
    public float FadeOutTime ;

    /// <summary>
    /// 持续时间，时间为负时无限制时间(淡入结束后开始计时)
    /// </summary>
    public float Duration ;

    //箭头计时器
    float ArrowTimer;



    private void Start()
    {
        SetArrow();
        //SetTarget(new Vector2(5.0f, 5.0f));
        ArrowLineHead = ArrowHead.transform.GetChild(0).GetComponent<SpriteRenderer>();
        ArrowLineBody = ArrowBody.transform.GetChild(0).GetComponent<SpriteRenderer>();
    }


    private void Update()
    {
        //加时间
        ArrowTimer += Time.deltaTime;
        if (!isOver) {
            if (ArrowTimer <= FadeInTime)
            {
                ArrowHead.color = new Color(ArrowHead.color.r, ArrowHead.color.g, ArrowHead.color.b, ArrowTimer / FadeInTime);
                ArrowBody.color = new Color(ArrowBody.color.r, ArrowBody.color.g, ArrowBody.color.b, ArrowTimer / FadeInTime);
                ArrowLineHead.color = new Color(ArrowLineHead.color.r, ArrowLineHead.color.g, ArrowLineHead.color.b, ArrowTimer / FadeInTime);
                ArrowLineBody.color = new Color(ArrowLineBody.color.r, ArrowLineBody.color.g, ArrowLineBody.color.b, ArrowTimer / FadeInTime); 
            }
            //持续时间模式
            if (Duration > 0.0f)
            {
                if (ArrowTimer >= Duration + FadeInTime)
                {
                    ArrowOver();
                }
            }
        }
        else
        {
            if (ArrowTimer <= FadeOutTime)
            {
                ArrowHead.color = new Color(ArrowHead.color.r, ArrowHead.color.g, ArrowHead.color.b, (FadeOutTime - ArrowTimer) / FadeOutTime);
                ArrowBody.color = new Color(ArrowBody.color.r, ArrowBody.color.g, ArrowBody.color.b, (FadeOutTime - ArrowTimer) / FadeOutTime);
                ArrowLineHead.color = new Color(ArrowLineHead.color.r, ArrowLineHead.color.g, ArrowLineHead.color.b, (FadeOutTime - ArrowTimer) / FadeOutTime);
                ArrowLineBody.color = new Color(ArrowLineBody.color.r, ArrowLineBody.color.g, ArrowLineBody.color.b, (FadeOutTime - ArrowTimer) / FadeOutTime); 
            }
        }




        if (isTargetMode && targetTransform != null)
        {
            SetTarget(targetTransform.position);
        }
    }


    /// <summary>
    /// 设置箭头
    /// </summary>
    public void SetArrow()
    {
        //设置颜色
        var hc = ArrowHead.color;
        hc.r = ArrowColor.r;
        hc.g = ArrowColor.g;
        hc.b = ArrowColor.b;
        var bc = ArrowBody.color;
        bc.r = ArrowColor.r;
        bc.g = ArrowColor.g;
        bc.b = ArrowColor.b;


        //设置宽度
        ArrowHead.transform.parent.localScale = new Vector3(ArrowScale, ArrowScale,1.0f);

        //设置角度
        ArrowHead.transform.parent.localRotation = Quaternion.Euler(0,0, ArrowAngle-90.0f);

        //设置长度
        float l = ArrowLength / (ArrowScale * 2);
        ArrowBody.transform.localScale = new Vector3(1, l, 1);
        ArrowBody.transform.localPosition = new Vector3(0, l, 0);
        ArrowHead.transform.localPosition = new Vector3(0, 2 * l + 0.5f, 0);
    }


    /// <summary>
    /// 设置目标点
    /// </summary>
    public void SetTarget(Vector2 point)
    {
        targetPoint = point;
        ArrowLength = Mathf.Clamp( Vector2.Distance(transform.position , point) - ArrowScale, 0.0f, 10000.0f);
        ArrowAngle = _mTool.Angle_360Y(point - (Vector2)transform.position , Vector2.right);
        SetArrow();
    }

    public void ArrowOver()
    {
        isOver = true;
        ArrowTimer = 0.0f;
    }

    public void FollowATarget(Transform t)
    {
        IsTargetMode = true;
        targetTransform = t;
    }

}
