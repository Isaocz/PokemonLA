using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnixTailSubEmpty : SubEmptyBody
{

    bool isOnixDie;

    // Start is called before the first frame update
    void Start()
    {
        SubEmptyBodyStart();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        SubEmptyBodyUpdate();
        if (ParentEmpty.isDie && !isOnixDie) { isOnixDie = true; animator.SetTrigger("Die"); }
    }


    private void FixedUpdate()
    {
        SubEmptyBodyFixedUpdate();
    }


    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.tag == ("Player"))
        {
            ParentEmpty.EmptyTouchHit(other.gameObject);
        }
    }


    public void OnixTailDestroy()
    {
        if ( ParentEmpty.isSubBodyEmptyInvincible ) { ParentEmpty.isSubBodyEmptyInvincible = false; }
        if ( ParentEmpty.isFrozenDef) { ParentEmpty.isFrozenDef = false; }
        if ( ParentEmpty.isBurnDef) { ParentEmpty.isBurnDef = false; }
        if ( ParentEmpty.isSleepDef) { ParentEmpty.isSleepDef = false; }
        if ( ParentEmpty.isParalysisDef) { ParentEmpty.isParalysisDef = false; }
        if ( ParentEmpty.isToxicDef) { ParentEmpty.isToxicDef = false; }
        if ( ParentEmpty.EmptyCurseDef) { ParentEmpty.EmptyCurseDef = false; }
        if ( ParentEmpty.isConfusionDef) { ParentEmpty.isConfusionDef = false; }
        if ( ParentEmpty.EmptyInfatuationDef) { ParentEmpty.EmptyInfatuationDef = false; }
        if ( ParentEmpty.isBlindDef) { ParentEmpty.isBlindDef = false; }
        if ( ParentEmpty.isFearDef) { ParentEmpty.isFearDef = false; }
        if ( ParentEmpty.isAtkChangeDef) { ParentEmpty.isAtkChangeDef = false; }
        if ( ParentEmpty.isDefChangeDef) { ParentEmpty.isDefChangeDef = false; }
        if ( ParentEmpty.isSpAChangeDef) { ParentEmpty.isSpAChangeDef = false; }
        if ( ParentEmpty.isSpDChangeDef) { ParentEmpty.isSpDChangeDef = false; }
        if ( ParentEmpty.isColdDef) { ParentEmpty.isColdDef = false; }


        Destroy(gameObject);
    }

}
