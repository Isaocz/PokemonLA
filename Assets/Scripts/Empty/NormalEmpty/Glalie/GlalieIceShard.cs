using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlalieIceShard : Projectile
{
    bool isDestory;
    public float MoveSpeed;

    bool isCanNotMove;

    /// <summary>
    /// 冰鬼护死亡后爆发冰砾的等级 等级越高速度越快 距离越远
    /// </summary>
    public int GlalieIceShardLevel;
    //冰砾速度和最远距离
    float IceshardSpeed;
    float IceshardMaxDis;
    int IceshardDmage;

    public GlalieIceShard childIceshard;


    private void Awake()
    {
        AwakeProjectile();
        Timer.Start(this, 3.0f, () => { if (!isDestory) { isDestory = true; } });
    }

    /// <summary>
    /// 设置新发射的冰砾
    /// </summary>
    public void SetNewIceShard(int level , Empty e)
    {
        empty = e;
        GlalieIceShardLevel = level;
        switch (GlalieIceShardLevel)
        {
            case 0:
                IceshardSpeed = 5.0f;
                IceshardMaxDis = 5.0f;
                IceshardDmage = 30;
                break;
            case 1:
                IceshardSpeed = 12.0f;
                IceshardMaxDis = 100.0f;
                IceshardDmage = 50;
                break;
            case 2:
                IceshardSpeed = 14.0f;
                IceshardMaxDis = 3.0f;
                IceshardDmage = 70;
                break;
            case 3:
                IceshardSpeed = 12.0f;
                IceshardMaxDis = 4.0f;
                IceshardDmage = 80;
                break;
        }
        Dmage = IceshardDmage;
        //Debug.Log(transform.eulerAngles.z);
        LaunchNotForce(new Vector2(Mathf.Cos(transform.eulerAngles.z * Mathf.Deg2Rad), Mathf.Sin(transform.eulerAngles.z * Mathf.Deg2Rad)), IceshardSpeed);
    }



    private void Update()
    {
        //this.transform.localScale += new Vector3(Time.deltaTime * 2, 0, 0);
        if (!isCanNotMove)
        {
            DestoryByRange(IceshardMaxDis);
            if (isDestory)
            {
                IceBreak(0 , Vector2.zero);
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
            IceBreak(0, Vector2.zero);
        }
    }

    /// <summary>
    /// 冰砾裂开
    /// </summary>
    /// <param name="BreakReason">裂开原因 0距离到达极限 1撞到敌人 2撞到墙壁</param>
    void IceBreak(int BreakReason , Vector2 BreakV)
    {
        if (empty.IsDeadrattle) {
            if (GlalieIceShardLevel < 2)
            {
                switch (BreakReason)
                {
                    //距离到达极限 再次分为三个
                    case 0:
                        float z = transform.eulerAngles.z;
                        GlalieIceShard i1 = Instantiate(childIceshard, transform.position, Quaternion.Euler(0, 0, z + 30 * 1), transform.parent);
                        i1.SetNewIceShard(GlalieIceShardLevel + 1, empty);
                        GlalieIceShard i2 = Instantiate(childIceshard, transform.position, Quaternion.Euler(0, 0, z + 30 * 2), transform.parent);
                        i2.SetNewIceShard(GlalieIceShardLevel + 1, empty);
                        GlalieIceShard i3 = Instantiate(childIceshard, transform.position, Quaternion.Euler(0, 0, z + 30 * 3), transform.parent);
                        i3.SetNewIceShard(GlalieIceShardLevel + 1, empty);
                        GlalieIceShard i4 = Instantiate(childIceshard, transform.position, Quaternion.Euler(0, 0, z + 30 * 4), transform.parent);
                        i4.SetNewIceShard(GlalieIceShardLevel + 1, empty);
                        GlalieIceShard i5 = Instantiate(childIceshard, transform.position, Quaternion.Euler(0, 0, z + 30 * 8), transform.parent);
                        i5.SetNewIceShard(GlalieIceShardLevel + 1, empty);
                        GlalieIceShard i6 = Instantiate(childIceshard, transform.position, Quaternion.Euler(0, 0, z + 30 * 9), transform.parent);
                        i6.SetNewIceShard(GlalieIceShardLevel + 1, empty);
                        GlalieIceShard i7 = Instantiate(childIceshard, transform.position, Quaternion.Euler(0, 0, z + 30 * 10), transform.parent);
                        i7.SetNewIceShard(GlalieIceShardLevel + 1, empty);
                        GlalieIceShard i8 = Instantiate(childIceshard, transform.position, Quaternion.Euler(0, 0, z + 30 * 11), transform.parent);
                        i8.SetNewIceShard(GlalieIceShardLevel + 1, empty);
                        GlalieIceShard i9 = Instantiate(childIceshard, transform.position, Quaternion.Euler(0, 0, z + 30 * 12), transform.parent);
                        i9.SetNewIceShard(GlalieIceShardLevel + 1, empty);
                        break;

                    //撞到墙，按照墙再分为两个
                    case 2:
                        float v = _mTool.Angle_360Y(BreakV, Vector2.right);
                        GlalieIceShard i10 = Instantiate(childIceshard, transform.position, Quaternion.Euler(0, 0, v + 120), transform.parent);
                        i10.SetNewIceShard(GlalieIceShardLevel + 1, empty);
                        GlalieIceShard i11 = Instantiate(childIceshard, transform.position, Quaternion.Euler(0, 0, v - 120), transform.parent);
                        i11.SetNewIceShard(GlalieIceShardLevel + 1, empty);
                        break;
                }
            }
            if (GlalieIceShardLevel == 2)
            {

            }
        }

        isCanNotMove = true;
        GetComponent<Animator>().SetTrigger("Break");
        if (transform.childCount > 0)
        {
            var Emission1 = transform.GetChild(0).GetComponent<ParticleSystem>().emission;
            var Main1 = transform.GetChild(0).GetComponent<ParticleSystem>().main;
            Emission1.enabled = false;
            Main1.loop = false;
        }
        if (transform.childCount > 1)
        {
            var Emission2 = transform.GetChild(1).GetComponent<ParticleSystem>().emission;
            var Main2 = transform.GetChild(1).GetComponent<ParticleSystem>().main;
            Emission2.enabled = false;
            Main2.loop = false;
        }

        transform.DetachChildren();

        
    }

    
    private void OnTriggerEnter2D(Collider2D other)
    {

        if ( other.tag == ("Room") || other.tag == ("Player") || (empty.isEmptyInfatuationDone && other.gameObject != empty.gameObject && other.tag == ("Empty")))
        {

            if(other.tag == ("Room"))
            {
                
                // 获取碰撞箱中心点
                Vector3 centerPoint = transform.position;
                Vector3 v = (centerPoint - transform.parent.position).normalized;
                Debug.Log(_mTool.Angle_360Y(v, Vector3.right));
                RaycastHit2D u = Physics2D.Raycast(transform.position , Vector2.up , LayerMask.GetMask("Room"));
                RaycastHit2D d = Physics2D.Raycast(transform.position , Vector2.down , LayerMask.GetMask("Room"));
                RaycastHit2D l = Physics2D.Raycast(transform.position , Vector2.left , LayerMask.GetMask("Room"));
                RaycastHit2D r = Physics2D.Raycast(transform.position , Vector2.right , LayerMask.GetMask("Room"));
                float minL = Mathf.Min(u.distance, d.distance, l.distance, r.distance);
                if (minL == u.distance)      {  IceBreak(2, Vector2.up); }
                else if (minL == d.distance) {  IceBreak(2, Vector2.down); }
                else if (minL == l.distance) {  IceBreak(2, Vector2.left); }
                else if (minL == r.distance) {  IceBreak(2, Vector2.right); }
                else { Debug.Log("error"); IceBreak(2, Vector2.zero); }
            }
            else
            {
                IceBreak(0, Vector2.zero);
            }
            
            isDestory = true;
            Destroy(rigidbody2D);

            float WeatherAlpha = ((Weather.GlobalWeather.isRain) ? (Weather.GlobalWeather.isRainPlus ? 1.8f : 1.3f) : 1) * ((Weather.GlobalWeather.isSunny) ? 0.5f : 1);


            if (other.tag == ("Player") && !empty.isEmptyInfatuationDone)
            {
                PlayerControler playerControler = other.GetComponent<PlayerControler>();
                Pokemon.PokemonHpChange(empty.gameObject, other.gameObject, Dmage, 0, 0, PokemonType.TypeEnum.Ice);
                if (playerControler != null)
                {
                    playerControler.KnockOutPoint = 1;
                    playerControler.KnockOutDirection = (playerControler.transform.position - transform.position).normalized;
                    playerControler.PlayerFrozenFloatPlus(0.3f, 1.2f);
                }
            }
            if (other.tag == ("Empty") && empty.isEmptyInfatuationDone)
            {
                Empty e = other.GetComponent<Empty>();
                Pokemon.PokemonHpChange(empty.gameObject, e.gameObject, Dmage, 0, 0, PokemonType.TypeEnum.Ice);
                e.Frozen(5.0f , 0.3f , 1);
            }
            
        }
    }
    
}
