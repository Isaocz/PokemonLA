using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClawEffect : MonoBehaviour
{

    public Animator animator;

    public Transform ClawParent1;
    public SpriteRenderer ClawSprite1;
    public ParticleSystem TrailPS1Claw1;
    public ParticleSystem TrailPS2Claw1;
    public ParticleSystem DustPSClaw1;

    public Transform ClawParent2;
    public SpriteRenderer ClawSprite2;
    public ParticleSystem TrailPS1Claw2;
    public ParticleSystem TrailPS2Claw2;
    public ParticleSystem DustPSClaw2;

    public Transform ClawParent3;
    public SpriteRenderer ClawSprite3;
    public ParticleSystem TrailPS1Claw3;
    public ParticleSystem TrailPS2Claw3;
    public ParticleSystem DustPSClaw3;




    public void ClawEffectOver()
    {
        animator.SetTrigger("Over");
        _mTool.RemoveAllPSChild(transform.gameObject);
    }

}
