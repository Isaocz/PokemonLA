using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeavileSubstituteOBJ : BreakableEnviroment
{

    public Weavile ParentWeavile;

    public Room ParentRoom;


    //生成是否完成
    public bool isBornOver = false;

    /// <summary>
    /// 限位组件
    /// </summary>
    public WeavileFlingSubstituteMoveLocker ChileMoveLocker;


    /// <summary>
    /// 破碎的时间
    /// </summary>
    public float BreakTime;




    /// <summary>
    /// 随时间破裂
    /// </summary>
    /// <returns></returns>
    IEnumerator BeHitbByTime()
    {
        while (ColliderCount < MaxHP)
        {
            if (!isBornOver)
            {
                BeHit(1);
            }

            yield return new WaitForSeconds(2.0f);
        }
    }


    private void Awake()
    {
        isBornOver = true;
    }


    private void Start()
    {
        StartCoroutine(BeHitbByTime());
    }


    /// <summary>
    ///  替身被破坏
    /// </summary>
    public override void Break()
    {
        if (gameObject != null && !isBreak)
        {
            isBreak = true;
            GetComponent<Animator>().SetTrigger("Die");
            ChileMoveLocker.CollidorOver();


        }
    }



    public override int GetHitHitPoint(int Dmage, PokemonType.TypeEnum SkillType)
    {
        float DmageAlpha = Mathf.Clamp((float)(Dmage) / (float)50, 1.0f, 10.0f);
        int TypeAlpha = 1;
        int d = (int)(DmageAlpha * TypeAlpha * (float)HitHitPoint);
        Debug.Log(d + "+" + Dmage + "+" + (float)(Dmage) / (float)50 + "+" + DmageAlpha + "+" + TypeAlpha);
        return d;
    }

}
