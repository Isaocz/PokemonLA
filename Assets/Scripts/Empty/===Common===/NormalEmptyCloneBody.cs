using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敌人的分身幻影
/// </summary>
public class NormalEmptyCloneBody : MonoBehaviour
{
    [Tooltip("pokemon主体的渲染组件集")]
    public List<SpriteRenderer> skinRenderers;

    /// <summary>
    /// 是否开始消失
    /// </summary>
    public bool IsDespear
    {
        get { return isDespear; }
        set { isDespear = value; }
    }
    public bool isDespear = false;


    public float DispearTime;

    /// <summary>
    /// UI画布母体
    /// </summary>
    public Transform UICanvas;

    /// <summary>
    /// Sprite
    /// </summary>
    public SpriteRenderer Sprite;

    /// <summary>
    /// 影子
    /// </summary>
    public SpriteRenderer Shadow;

    /// <summary>
    /// 影子1
    /// </summary>
    public SpriteRenderer Shadow1;


    /// <summary>
    /// 动画机
    /// </summary>
    public Animator animator;


    /// <summary>
    /// 母体敌人
    /// </summary>
    public Empty ParentEmpty
    {
        get { return parentEmpty; }
        set { parentEmpty = value; }
    }
    Empty parentEmpty;

    /// <summary>
    /// 销毁自己
    /// </summary>
    public void DestorySelf()
    {
        //移除幻影至房间列表
        parentEmpty.ParentPokemonRoom.AddEmptyCloneList(this);

        Destroy(this.gameObject);
    }

    /// <summary>
    /// 初始化分身
    /// </summary>
    /// <param name="ParentE"></param>
    public void SetCloneBody(Empty ParentE)
    {
        parentEmpty = ParentE;
        Shadow.transform.localPosition = ParentE.transform.GetChild(0).localPosition;
        Shadow.transform.localRotation = ParentE.transform.GetChild(0).localRotation;
        Shadow.transform.localScale = ParentE.transform.GetChild(0).localScale;
        Shadow.color = ParentE.transform.GetChild(0).GetComponent<SpriteRenderer>().color;

        Shadow1.transform.localPosition = ParentE.transform.GetChild(1).localPosition;
        Shadow1.transform.localRotation = ParentE.transform.GetChild(1).localRotation;
        Shadow1.transform.localScale = ParentE.transform.GetChild(1).localScale;
        Shadow1.color = ParentE.transform.GetChild(1).GetComponent<SpriteRenderer>().color;

        UICanvas.localPosition = ParentE.transform.GetChild(2).localPosition;
        PlayerUIState state = Instantiate(ParentE.playerUIState, UICanvas.position + ParentE.playerUIState.transform.localPosition , Quaternion.identity , UICanvas);
        EmptyHpBar hpbar = Instantiate(ParentE.uIHealth, UICanvas.position + ParentE.uIHealth.transform.localPosition, Quaternion.identity , UICanvas);
        ParentE.playerUIState.ChildrenUIStateList.Add(state);
        ParentE.uIHealth.ChildrenHpBarList.Add(hpbar);

        SpriteRenderer ParentSprite = ParentE.transform.GetChild(3).GetChild(0).transform.GetComponent<SpriteRenderer>();
        Sprite.sprite = ParentSprite.sprite;
        Sprite.material = ParentSprite.material;

        //添加幻影至房间列表
        parentEmpty.ParentPokemonRoom.AddEmptyCloneList(this);
    }



    //获取 pokemon 主体的渲染组件
    public List<SpriteRenderer> GetSkinRenderers()
    {
        if (skinRenderers.Count > 0)
        {
            return skinRenderers;
        }
        List<SpriteRenderer> srs = new List<SpriteRenderer>();
        if (gameObject.GetComponent<SpriteRenderer>() != null)
        {
            srs.Add(gameObject.GetComponent<SpriteRenderer>());
            return srs;
        }

        Transform child3 = gameObject.transform.GetChild(3);
        if (child3.GetComponent<SpriteRenderer>() != null)
        {
            srs.Add(child3.GetComponent<SpriteRenderer>());
            return srs;
        }
        Transform child30 = child3.GetChild(0);
        srs.Add(child30.GetComponent<SpriteRenderer>());
        return srs;
    }



    //=================有关残影生成======================

    /// <summary>
    /// 敌人的残影实体
    /// </summary>
    public NormalEmptyShadow emptyShadow;
    /// <summary>
    /// 生成残影用的协程
    /// </summary>
    public Coroutine ShadowCoroutine;
    /// <summary>
    /// 是否生成残影
    /// </summary>
    public bool isShadowMove = false;


    /// <summary>
    /// 开始残影携程
    /// </summary>
    /// <param name="Interval">生成残影的间隔</param>
    /// <param name="disappearingSpeed">残影的消失速度</param>
    /// <param name="color">残影的颜色</param>
    public void StartShadowCoroutine(float Interval, float disappearingSpeed, Color color)
    {
        //Debug.Log("StartSHadow");
        isShadowMove = true; // 开始冲刺
        ShadowCoroutine = StartCoroutine(StartShadow(Interval, disappearingSpeed, color)); // 启动协程
    }


    /// <summary>
    /// 停止残影协程
    /// </summary>
    public void StopShadowCoroutine()
    {
        //Debug.Log("StopSHadow");
        isShadowMove = false; // 设置停止冲刺
        if (ShadowCoroutine != null)
        {
            StopCoroutine(ShadowCoroutine); // 通过引用停止协程
            ShadowCoroutine = null; // 清空引用以避免重复问题
        }
    }


    /// <summary>
    /// 每隔一段时间生成一个残影
    /// </summary>
    /// <param name="Interval">生成残影的间隔</param>
    /// <param name="disappearingSpeed">残影的消失速度</param>
    /// <param name="color">残影的颜色</param>
    /// <returns></returns>
    IEnumerator StartShadow(float Interval, float disappearingSpeed, Color color)
    {
        while (isShadowMove)
        {
            InstantiateShadow(disappearingSpeed, color);
            yield return new WaitForSeconds(Interval); // 等待间隔时间
        }
    }

    /// <summary>
    /// 生成一个残影
    /// </summary>
    /// <param name="disappearingSpeed">残影的消失速度</param>
    /// <param name="color">残影的颜色</param>
    void InstantiateShadow(float disappearingSpeed, Color color)
    {
        Instantiate(emptyShadow, transform.position, Quaternion.identity).SetNormalEmptyShadow(disappearingSpeed, GetSkinRenderers(), color);
    }

    //=================有关残影生成======================
}
