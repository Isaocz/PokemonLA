using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnixBodyPressCollider : Projectile
{
    void Start()
    {
        Invoke("DetachSelf", 0.7f);
        Invoke("DestroySelf", 3f);
    }



    void DetachSelf()
    {
        transform.parent = null;
    }

    void DestroySelf()
    {
        Destroy(gameObject);
    }
}
