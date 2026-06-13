using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MisdreavusSing : MonoBehaviour
{
    /// <summary>
    /// 父梦妖
    /// </summary>
    public Misdreavus ParentMisdreavus;

    /// <summary>
    /// 技能碰撞圈
    /// </summary>
    public SkillColliderRangeChangeByTimeManual skillCollider;

    /// <summary>
    /// 技能指示圈
    /// </summary>
    public SkillRangeCircleManual skillRange;

    float IntervalTimer = 0.0f;
    static float SingInterval = 1.0f;
    bool SleepFlg = false;
    bool SleepDoneFlg = false;
    



    /// <summary>
    /// 结束唱歌
    /// </summary>
    public void SingOver()
    {
        skillCollider.SkillCircleOver();
        skillRange.SkillCircleOver();
        _mTool.RemoveAllPSChild(this.gameObject);
        Destroy(gameObject, 1.0f);
    }





    private void Update()
    {
        
        if (IntervalTimer >= SingInterval)
        {
            SleepFlg = true;
        }
        else
        {
            IntervalTimer += Time.deltaTime;
        }
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (SleepFlg && ParentMisdreavus != null) {
            if (!ParentMisdreavus.isEmptyInfatuationDone && collision.tag == "Player")
            {
                PlayerControler p = collision.GetComponent<PlayerControler>();
                if (p != null)
                {
                    p.SleepFloatPlus(0.1f);
                    SleepDoneFlg = true;
                }
            }
            if (ParentMisdreavus.isEmptyInfatuationDone && collision.tag == "Empty")
            {
                Empty e = collision.GetComponent<Empty>();
                if (e != null && e.gameObject.GetInstanceID() != ParentMisdreavus.gameObject.GetInstanceID())
                {
                    e.EmptySleepDone(0.1f , 5.0f , 1.0f);
                    SleepDoneFlg = true;
                }
            }
        }
    }

    private void LateUpdate()
    {
        if (SleepDoneFlg)
        {
            SleepDoneFlg = false;
            SleepFlg = false;
            IntervalTimer = 0.0f;
        }

    }

}
