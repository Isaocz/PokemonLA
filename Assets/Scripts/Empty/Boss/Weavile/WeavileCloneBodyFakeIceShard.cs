using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeavileCloneBodyFakeIceShard : Projectile
{
    bool isDestory;
    public float MoveSpeed;

    bool isCanNotMove;


    //冰砾速度和最远距离
    float IceshardSpeed;
    public float IceshardMaxDis = 20.0f;
    int IceshardDmage;



    private void Awake()
    {
        AwakeProjectile();
        Timer.Start(this, 3.0f, () => { if (!isDestory) { isDestory = true; } });
    }




    private void Update()
    {
        //this.transform.localScale += new Vector3(Time.deltaTime * 2, 0, 0);
        if (!isCanNotMove)
        {
            DestoryByRange(IceshardMaxDis);
            if (isDestory)
            {
                IceBreak();
            }
            else
            {
                MoveNotForce();
            }
        }


    }


    public override void DestoryByRange(float ProjectileRange)
    {
        if ((transform.position - BornPosition).magnitude >= ProjectileRange)
        {
            IceBreak();
        }
    }

    /// <summary>
    /// 冰砾裂开
    /// </summary>
    /// <param name="BreakReason">裂开原因 0距离到达极限 1撞到敌人 2撞到墙壁</param>
    public void IceBreak()
    {

        isCanNotMove = true;
        GetComponent<Animator>().SetTrigger("Break");


        _mTool.RemoveAllPSChild(transform.gameObject);


    }


    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.tag == ("Room") || other.tag == ("Player"))
        {

            IceBreak();

            isDestory = true;
            Destroy(rigidbody2D);



        }

    }
}