using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HaunterShadowClaw : MonoBehaviour
{
    /// <summary>
    /// 母对象鬼斯通
    /// </summary>
    public Haunter ParentHaunter;


    // Start is called before the first frame update
    void Start()
    {
        //设置暗影爪粒子效果为正
        transform.GetChild(8).rotation = Quaternion.identity;
        //设置暗影爪碰撞箱的母对象为此
        transform.GetChild(6).GetComponent<HaunterSHadowClawTrigger>().ParentShadowClaw = this;
        transform.GetChild(7).GetComponent<HaunterSHadowClawTrigger>().ParentShadowClaw = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void DestorySelf()
    {
        foreach (Transform t in transform.GetChild(8))
        {
            t.parent = null;
        }
        Destroy(transform.gameObject);
    }




}
