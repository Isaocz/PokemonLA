using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeavileHealParent : WeavileSupportParent
{
    static float TIME_CD_HEAL = 1.0f;


    public Weavile ParentWeavile;


    /// <summary>
    /// 动画(球体)预制件
    /// </summary>
    public Animator animator;

    /// <summary>
    /// 圆环发射父预制件
    /// </summary>
    public WeavileHealEffectRingParent ringParent;

    /// <summary>
    /// 指示圈预制件
    /// </summary>
    public SkillRangeCircleManual SkillRangeCircle;



    //是否开始回复玛狃拉
    bool isStart_Heal = false;


    float HealTimerCD = 0.0f;

    /// 帮助时间
    /// </summary>
    float HelpingTime = 0.0f;




    public float HealRadius = 2.5f;


    //子特效
    public GameObject ChildEffectPS;


    private void Update()
    {
        if (isStart_Heal) {
            HealTimerCD -= Time.deltaTime;
            //每次cd检测一次
            if (HealTimerCD <= 0.0f)
            {
                HealTimerCD = TIME_CD_HEAL;

                Collider2D[] cList = Physics2D.OverlapCircleAll( transform.position , HealRadius, LayerMask.GetMask("Empty"));
                if (cList.Length != 0)
                {
                    foreach (Collider2D c in cList)
                    {
                        if (c.gameObject.tag == "Empty")
                        {
                            //Debug.Log("trigger2"+ c.gameObject.name);
                            Weavile w = c.GetComponent<Weavile>();
                            if (w != null && w.gameObject.GetInstanceID() != ParentWeavile.gameObject.GetInstanceID())
                            {
                                //Debug.Log("name"+"+"+ w.name);
                                int HealP = Mathf.Clamp((int)((float)w.maxHP / 60.0f), 1, 1000);
                                Pokemon.PokemonHpChange(null , w.gameObject , 0 , 0 , HealP , PokemonType.TypeEnum.IgnoreType);
                                Instantiate(ChildEffectPS , w.transform.position , Quaternion.identity , w.transform);
                            }
                        }
                    }
                }
            }
        }
    }


    /// <summary>
    /// 开始帮助
    /// </summary>
    public void HelpingStart(float time)
    {
        HelpingTime = time;
        isStart_Heal = true;
        HealTimerCD = 0.0f;
        if (animator != null) { 
            animator.gameObject.transform.parent = this.transform.parent;
            ringParent.gameObject.transform.parent = this.transform.parent;
            SkillRangeCircle.gameObject.transform.parent = this.transform.parent;
        }

        Timer.Start(this, HelpingTime, ()=>{ SupportOver(); });
       
    }


    /// <summary>
    /// 结束帮助
    /// </summary>
    public override void SupportOver()
    {
        if (isStart_Heal)
        {
            isStart_Heal = false;
            if (animator != null)
            {
                animator.SetTrigger("Over");
                animator.transform.parent = null;
            }
            if (ringParent != null)
            {
                ringParent.isOver = false;
                ringParent.transform.parent = null;
            }
            if (SkillRangeCircle != null)
            {
                SkillRangeCircle.SkillCircleOver();
                SkillRangeCircle.transform.parent = null;
            }
            Destroy(gameObject);
        }
    }


    private void OnDestroy()
    {
        SupportOver();
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        //Debug.Log("trigger1" +"+"+ collision.gameObject.name + "+" + isHealDo + "+" + ParentWeavile + "+" + collision.gameObject.tag);
        //if (isHealDo && ParentWeavile != null)
        //{
        //    isHealDo = false;
        //    HealTimerCD = TIME_CD_HEAL;
        //    if (collision.gameObject.tag == "Empty" )
        //    {
        //        Debug.Log("trigger2"+ collision.gameObject.name);
        //        Weavile w = collision.GetComponent<Weavile>();
        //        if (w != null && w.gameObject.GetInstanceID() != ParentWeavile.gameObject.GetInstanceID())
        //        {
        //            Debug.Log("name"+"+"+ w.name);
        //        }
        //    }
        //}
    }

}
