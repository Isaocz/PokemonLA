using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpingHandEffect : MonoBehaviour
{
    /// <summary>
    /// 效果的持续时间
    /// </summary>
    public float Effect;


    public Animator animator;

    public ParticleSystem PS;



    private void Start()
    {
        Timer.Start(this , Effect , ()=> { EffectOver(); });
    }

    /// <summary>
    /// 效果结束
    /// </summary>
    public void EffectOver()
    {

        if (animator != null) {
            animator.SetTrigger("Over");
        }
        _mTool.RemoveAllPSChild(transform.gameObject);
    }
}
