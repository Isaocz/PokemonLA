using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalEmptyCloneBody : MonoBehaviour
{

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
    /// 销毁自己
    /// </summary>
    public void DestorySelf()
    {
        Destroy(this.gameObject);
    }

    /// <summary>
    /// 初始化分身
    /// </summary>
    /// <param name="ParentE"></param>
    public void SetCloneBody(Empty ParentE)
    {
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
    }
}
