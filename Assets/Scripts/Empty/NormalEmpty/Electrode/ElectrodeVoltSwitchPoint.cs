using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectrodeVoltSwitchPoint : MonoBehaviour
{
    /// <summary>
    /// 闪电
    /// </summary>
    public LightningBoltEffect LightningBolt;

    /// <summary>
    /// 技能指示圈
    /// </summary>
    public SkillRangeCircleManual Rangecircle;

    /// <summary>
    /// 技能圆环PS
    /// </summary>
    public ParticleSystem CirclePS;

    /// <summary>
    /// 技能中心PS
    /// </summary>
    public ParticleSystem CenterPS;


    /// <summary>
    /// 是否被摧毁
    /// </summary>
    bool isBeDestroy = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// 移除本体
    /// </summary>
    public void RemoveVoltSwitchPoint()
    {
        LightningBolt.gameObject.SetActive( false);
        var m1 = CenterPS.main;
        m1.loop = false;
        var m2 = CirclePS.main;
        m2.loop = false;
        m2.simulationSpeed = 4.0f;
        Rangecircle.SkillCircleOver();
        isBeDestroy = false;
        //Destroy(this.gameObject , 3.0f);
    }
}
