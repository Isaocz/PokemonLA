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
        Destroy(gameObject);
    }

}
