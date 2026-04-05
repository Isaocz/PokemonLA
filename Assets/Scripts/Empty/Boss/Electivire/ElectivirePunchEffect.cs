using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectivirePunchEffect : MonoBehaviour
{

    /// <summary>
    /// 拳击特效
    /// </summary>
    public GameObject PSEffect;

    /// <summary>
    /// 拳头Sprite
    /// </summary>
    public SpriteRenderer PunchSprite;

    /// <summary>
    /// 动画机
    /// </summary>
    public Animator animator;

    /// <summary>
    /// 是否是座拳
    /// </summary>
    public bool isLPunch;

    

    // Start is called before the first frame update
    public void Start()
    {
        //左右拳
        PunchSprite.flipX = !isLPunch;
        //Debug.Log("Start");
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    public void PunchOver()
    {
        PSEffect.SetActive(true);
        animator.SetTrigger("Over");
    }
}
