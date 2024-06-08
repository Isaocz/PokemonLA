using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownChair : MonoBehaviour
{
    // Start is called before the first frame update
    public Animator animator;

    /// <summary>
    /// 坐在椅子上对象的状态
    /// </summary>
    public enum ChairState
    {
        JumpUp,  //目标跳上椅子
        JumpDown,//目标跳下椅子
        None,   //没有目标坐在椅子上
        SetDown, //目标坐在椅子上

    }

    public ChairState State;

    /// <summary>
    /// 坐在椅子上的目标
    /// </summary>
    public TownPlayer ChairTarget;
    GameObject ChairTargetSprite;
    Vector2 JumpVector;

    /// <summary>
    /// 目标坐在椅子上的位置
    /// </summary>
    public Vector3 TargetPosition;
    /// <summary>
    /// 目标坐在椅子上的朝向
    /// </summary>
    public Vector2 ChairV;

    /// <summary>
    /// 跳下椅子时的位置
    /// </summary>
    public Vector3 JumpDownPosition;
    /// <summary>
    /// 跳下椅子时的朝向
    /// </summary>
    public Vector2 JumpDownV;

    /// <summary>
    /// 目标跳上椅子时跳跃的最高高度
    /// </summary>
    float HighestYPosition;

    float Timer;

    public void ChairsStart()
    {
        animator = GetComponent<Animator>();
        State = ChairState.None;
    }


    public bool isNotChangeSortLayer;


    public void ChairsJumpUp()
    {
        if (ChairTarget != null) {
            if (State == ChairState.JumpUp)
            {
                Timer += Time.deltaTime;
                if (Timer >= 0.0f && Timer < 0.05f)
                {
                    ChairTargetSprite.transform.localScale = new Vector3(Mathf.Clamp(ChairTargetSprite.transform.localScale.x + Time.deltaTime * 4, 1.0f, 1.2f), Mathf.Clamp(ChairTargetSprite.transform.localScale.y - Time.deltaTime * 4f, 0.8f, 1.0f), ChairTargetSprite.transform.localScale.z);
                }
                if (Timer >= 0.05f && Timer < 0.1f)
                {
                    ChairTargetSprite.transform.localScale = new Vector3(Mathf.Clamp(ChairTargetSprite.transform.localScale.x - Time.deltaTime * 8, 0.8f, 1.2f), Mathf.Clamp(ChairTargetSprite.transform.localScale.y + Time.deltaTime * 8f, 0.8f, 1.2f), ChairTargetSprite.transform.localScale.z);
                }
                if (Timer >= 0.1f && Timer < 0.3f)
                {
                    ChairTargetSprite.transform.localScale = new Vector3(Mathf.Clamp(ChairTargetSprite.transform.localScale.x + Time.deltaTime * 1f, 0.8f, 1.0f), Mathf.Clamp(ChairTargetSprite.transform.localScale.y - Time.deltaTime * 1f, 1.0f, 1.2f), ChairTargetSprite.transform.localScale.z);
                    ChairTargetSprite.transform.localPosition = new Vector3(ChairTargetSprite.transform.localPosition.x, Mathf.Clamp(ChairTargetSprite.transform.localPosition.y + Time.deltaTime * 8, 0.0f, 1.6f), ChairTargetSprite.transform.localPosition.z);
                    ChairTarget.transform.position = new Vector3(ChairTarget.transform.position.x + Time.deltaTime * JumpVector.x * 2.5f, ChairTarget.transform.position.y + Time.deltaTime * JumpVector.y * 2.5f, 0);
                }
                if (Timer >= 0.3f && Timer < 0.5f)
                {
                    ChairTargetSprite.transform.localPosition = new Vector3(ChairTargetSprite.transform.localPosition.x, Mathf.Clamp(ChairTargetSprite.transform.localPosition.y - Time.deltaTime * 8, 0.0f, 1.6f), ChairTargetSprite.transform.localPosition.z);
                    ChairTarget.transform.position = new Vector3(ChairTarget.transform.position.x + Time.deltaTime * JumpVector.x * 2.5f, ChairTarget.transform.position.y + Time.deltaTime * JumpVector.y * 2.5f, 0);
                    if (Timer >= 0.4f && Timer < 0.45f)
                    {
                        ChairTargetSprite.transform.localScale = new Vector3(Mathf.Clamp(ChairTargetSprite.transform.localScale.x + Time.deltaTime * 4, 1.0f, 1.2f), Mathf.Clamp(ChairTargetSprite.transform.localScale.y - Time.deltaTime * 4f, 0.8f, 1.0f), ChairTargetSprite.transform.localScale.z);
                    }
                    if (Timer >= 0.45f && Timer < 0.5f)
                    {
                        ChairTargetSprite.transform.localScale = new Vector3(Mathf.Clamp(ChairTargetSprite.transform.localScale.x - Time.deltaTime * 4, 0.8f, 1.0f), Mathf.Clamp(ChairTargetSprite.transform.localScale.y + Time.deltaTime * 4f, 0.8f, 1.0f), ChairTargetSprite.transform.localScale.z);
                    }
                }
                if (Timer >= 0.5f)
                {

                    State = ChairState.SetDown;
                    Timer = 0;
                    ChairTarget.transform.GetChild(2).localScale = ChairTarget.PlayerLocalScal;
                    ChairTarget.transform.GetChild(2).localPosition = ChairTarget.PlayerLocalPosition;
                }
            }
        }
    }

    public void ChairsSetDown()
    {
        if (ChairTarget != null)
        {
            if (State == ChairState.SetDown)
            {
                if (ChairTarget.MoveSpeed.magnitude != 0)
                {
                    State = ChairState.JumpDown;
                    JumpVector = (transform.position + JumpDownPosition) - ChairTarget.transform.position;
                    ChairTarget.GetComponent<Animator>().SetFloat("LookX", JumpDownV.x);
                    ChairTarget.GetComponent<Animator>().SetFloat("LookY", JumpDownV.y);
                }
            }
        }
    }

    public void ChairsJumpDown()
    {
        if (ChairTarget != null)
        {
            if (State == ChairState.JumpDown)
            {
                Timer += Time.deltaTime;
                if (Timer >= 0.0f && Timer < 0.05f)
                {
                    ChairTargetSprite.transform.localScale = new Vector3(Mathf.Clamp(ChairTargetSprite.transform.localScale.x + Time.deltaTime * 4, 1.0f, 1.2f), Mathf.Clamp(ChairTargetSprite.transform.localScale.y - Time.deltaTime * 4f, 0.8f, 1.0f), ChairTargetSprite.transform.localScale.z);
                }
                if (Timer >= 0.05f && Timer < 0.1f)
                {
                    ChairTargetSprite.transform.localScale = new Vector3(Mathf.Clamp(ChairTargetSprite.transform.localScale.x - Time.deltaTime * 8, 0.8f, 1.2f), Mathf.Clamp(ChairTargetSprite.transform.localScale.y + Time.deltaTime * 8f, 0.8f, 1.2f), ChairTargetSprite.transform.localScale.z);
                }
                if (Timer >= 0.1f && Timer < 0.3f)
                {
                    ChairTargetSprite.transform.localScale = new Vector3(Mathf.Clamp(ChairTargetSprite.transform.localScale.x + Time.deltaTime * 1f, 0.8f, 1.0f), Mathf.Clamp(ChairTargetSprite.transform.localScale.y - Time.deltaTime * 1f, 1.0f, 1.2f), ChairTargetSprite.transform.localScale.z);
                    ChairTargetSprite.transform.localPosition = new Vector3(ChairTargetSprite.transform.localPosition.x, Mathf.Clamp(ChairTargetSprite.transform.localPosition.y + Time.deltaTime * 8, 0.0f, 1.6f), ChairTargetSprite.transform.localPosition.z);
                    ChairTarget.transform.position = new Vector3(ChairTarget.transform.position.x + Time.deltaTime * JumpVector.x * 2.5f, ChairTarget.transform.position.y + Time.deltaTime * JumpVector.y * 2.5f, 0);
                }
                if (Timer >= 0.3f && Timer < 0.5f)
                {
                    ChairTargetSprite.transform.localPosition = new Vector3(ChairTargetSprite.transform.localPosition.x, Mathf.Clamp(ChairTargetSprite.transform.localPosition.y - Time.deltaTime * 8, 0.0f, 1.6f), ChairTargetSprite.transform.localPosition.z);
                    ChairTarget.transform.position = new Vector3(ChairTarget.transform.position.x + Time.deltaTime * JumpVector.x * 2.5f, ChairTarget.transform.position.y + Time.deltaTime * JumpVector.y * 2.5f, 0);
                    if (Timer >= 0.4f && Timer < 0.45f)
                    {
                        ChairTargetSprite.transform.localScale = new Vector3(Mathf.Clamp(ChairTargetSprite.transform.localScale.x + Time.deltaTime * 4, 1.0f, 1.2f), Mathf.Clamp(ChairTargetSprite.transform.localScale.y - Time.deltaTime * 4f, 0.8f, 1.0f), ChairTargetSprite.transform.localScale.z);
                    }
                    if (Timer >= 0.45f && Timer < 0.5f)
                    {
                        ChairTargetSprite.transform.localScale = new Vector3(Mathf.Clamp(ChairTargetSprite.transform.localScale.x - Time.deltaTime * 4, 0.8f, 1.0f), Mathf.Clamp(ChairTargetSprite.transform.localScale.y + Time.deltaTime * 4f, 0.8f, 1.0f), ChairTargetSprite.transform.localScale.z);
                    }
                }
                if (Timer >= 0.5f)
                {
                    ChairTarget.isCanNotMove = false;
                    ChairTarget.isCanNotTurnDirection = false;
                    if (!isNotChangeSortLayer)
                    {
                        ChairTargetSprite.transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder -= 1;
                    }
                    State = ChairState.None;
                    Timer = 0;
                    ChairTarget.transform.GetChild(2).localScale = ChairTarget.PlayerLocalScal;
                    ChairTarget.transform.GetChild(2).localPosition = ChairTarget.PlayerLocalPosition;
                    ChairTarget = null;
                }
            }

        }
    }




    private void OnTriggerEnter2D(Collider2D other)
    {
        if (State == ChairState.None && this.enabled)
        {
            if (other.gameObject.tag == "Player")
            {

                if (!other.gameObject.GetComponent<TownPlayer>().isCanNotMove) {
                    ChairTarget = other.gameObject.GetComponent<TownPlayer>();
                    //HighestYPosition = Mathf.Max((transform.position.y + TargetPosition.y), ChairTarget.transform.position.y) + Mathf.Abs((transform.position.y + TargetPosition.y) - ChairTarget.transform.position.y) / 2;
                    ChairTarget.isCanNotMove = true;
                    ChairTarget.isCanNotTurnDirection = true;
                    ChairTarget.transform.GetChild(2).localScale = ChairTarget.PlayerLocalScal;
                    ChairTarget.transform.GetChild(2).localPosition = ChairTarget.PlayerLocalPosition;
                    ChairTarget.GetComponent<Animator>().SetFloat("Speed", 0.0f);
                    ChairTarget.GetComponent<Animator>().SetFloat("LookX", ChairV.x);
                    ChairTarget.GetComponent<Animator>().SetFloat("LookY", ChairV.y);
                    ChairTargetSprite = ChairTarget.transform.GetChild(2).gameObject;
                    //Debug.Log(ChairTargetSprite.transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder);
                    if (!isNotChangeSortLayer) { ChairTargetSprite.transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder += 1; }
                    JumpVector = ((transform.position + TargetPosition) - ChairTarget.transform.position);
                    State = ChairState.JumpUp;
                }
            }
        }
    }

}
