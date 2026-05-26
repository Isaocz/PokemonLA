using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeavileUseProtectSon : MonoBehaviour
{

    /// <summary>
    /// 使用保护的母玛狃拉
    /// </summary>
    public Weavile ParentWeavile;

    /// <summary>
    /// 被保护的玛狃拉
    /// </summary>
    public Weavile HelpedWeavile;

    /// <summary>
    /// 被保护的玛狃拉替身
    /// </summary>
    public WeavileSubstituteOBJ HelpedWeavileSubstitute;

    /// <summary>
    /// 保护母件
    /// </summary>
    public WeavileUseProtectParent ProtectParent;

    /// <summary>
    /// 保护效果
    /// </summary>
    public ProtectEffect effect;



    /// <summary>
    /// 是否已经帮助
    /// </summary>
    bool isProtected = false;
    /// <summary>
    /// 帮助时间
    /// </summary>
    float ProtectTime = 0.0f;






    //保护开始
    public void HelpingStart(Weavile w, float t)
    {
        HelpedWeavile = w;
        ProtectTime = t;

        // Effect开始就脱离父物体
        if (effect != null)
        {
            effect.transform.parent = transform.parent;
            effect.Effect = ProtectTime - 0.1f;
        }
        if (!isProtected)
        {
            //保护
            HelpedWeavile.InvincibleProtect = true;
            isProtected = true;
        }
        Destroy(gameObject, ProtectTime);
    }



    //保护开始 保护替身
    public void HelpingStart(WeavileSubstituteOBJ ws, float t)
    {
        HelpedWeavileSubstitute = ws;
        ProtectTime = t;

        // Effect开始就脱离父物体
        if (effect != null)
        {
            effect.transform.parent = transform.parent;
        }
        if (!isProtected)
        {
            //保护
            HelpedWeavileSubstitute.InvincibleProtect = true;
            isProtected = true;
        }
        Destroy(gameObject, ProtectTime);
    }



    //帮助结束
    public void HelpingOver()
    {
        if (isProtected)
        {
            //保护
            if (HelpedWeavile != null) { HelpedWeavile.InvincibleProtect = false; }
            if (HelpedWeavileSubstitute != null) { HelpedWeavileSubstitute.InvincibleProtect = false; }
            isProtected = false;
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
