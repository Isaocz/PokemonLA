using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeavileHelpingSon : MonoBehaviour
{

    /// <summary>
    /// 使用帮助的母玛狃拉
    /// </summary>
    public Weavile ParentWeavile;

    /// <summary>
    /// 被帮助的玛狃拉
    /// </summary>
    public Weavile HelpedWeavile;

    /// <summary>
    /// 帮助母件
    /// </summary>
    public WeavileHelpingParent HelpingParent;

    /// <summary>
    /// 拍手效果
    /// </summary>
    public HelpingHandEffect effect; 



    /// <summary>
    /// 是否已经帮助
    /// </summary>
    bool isHelped = false;
    /// <summary>
    /// 帮助时间
    /// </summary>
    float HelpingTime = 0.0f;






    //帮助开始
    public void HelpingStart(Weavile w, float t)
    {
        HelpedWeavile = w;
        HelpingTime = t;

        // Effect开始就脱离父物体
        if (effect != null) {
            effect.transform.parent = transform.parent;
        }
        if (!isHelped)
        {
            HelpedWeavile.AtkChange(2,0);
            HelpedWeavile.SpAChange(2,0);
            isHelped = true;
        }
        Destroy(gameObject, HelpingTime);
    }



    //帮助结束
    public void HelpingOver()
    {
        if (isHelped)
        {
            HelpedWeavile.AtkChange(-2, 0);
            HelpedWeavile.SpAChange(-2, 0);
            isHelped = false;
            if (effect != null)
            {
                effect.EffectOver();
                effect.transform.parent = null;
            }
        }
    }


    private void OnDestroy()
    {
        HelpingOver();
    }


}
