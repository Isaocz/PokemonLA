using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastformShadow : Empty
{
    public Empty ParentEmpty;
    public BlizzardCastform Blizzard;
    Vector2 Director;

    float Timer;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rigidbody2D = GetComponent<Rigidbody2D>();
    }


    // Update is called once per frame
    void Update()
    {
        if (!isDie && !isBorn)
        {
            Timer += Time.deltaTime;
            UpdateEmptyChangeHP();
            StateMaterialChange();
            if ((ParentEmpty.isSleepDone || ParentEmpty.isFearDone || Timer > 13))
            {
                animator.SetTrigger("ShadowOver");
            }
        }
    }


    public void DestrouSelf()
    {
        if (Blizzard != null) { Blizzard.BlizzardStop(); }
        Destroy(gameObject);
    }

}
