using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeavileCloneBodyFakeDarkPulseSmall : MonoBehaviour
{
    public SkillRangeCircle rangeCircle;

    public Animator animator;


    /// <summary>
    /// Ïú»ÙÊ±¼ä
    /// </summary>
    public float DestoryTime;



    private void Start()
    {
        Timer.Start(this, DestoryTime, () => { Destroy(gameObject); });
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            rangeCircle.OverCircle();
            animator.SetTrigger("Over");
        }
    }
}
