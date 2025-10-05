using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VanillishPodesSnow : Projectile
{
    public VanillishPodesSnowCollision Collision;


    private void Awake()
    {
        AwakeProjectile();
        Timer.Start(this, 6.0f, () => { DestoryParent(); });
    }

    private void Start()
    {
        Collision.empty = empty;
        Collision.SpDmage = SpDmage;
    }

    void DestoryParent()
    {
        _mTool.RemoveAllPSChild(gameObject);
        Destroy(gameObject);
    }
}
