using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtectEffect : MonoBehaviour
{
    /// <summary>
    /// 效果的持续时间
    /// </summary>
    public float Effect;


    float EffectTimer = 0.0f;



    private void Start()
    {
        EffectTimer = 0.0f;
    }

    private void Update()
    {
        if (EffectTimer > 0.0f)
        {
            EffectTimer += Time.deltaTime;
        }
        if (EffectTimer >= Effect)
        {
            EffectTimer = -1;
            EffectOver();
        }
        
    }

    /// <summary>
    /// 效果结束
    /// </summary>
    public void EffectOver()
    {
        _mTool.RemoveAllPSChild(transform.gameObject);
        Destroy(gameObject);
    }
}
