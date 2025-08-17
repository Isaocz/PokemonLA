using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HaunterShadowClaw : MonoBehaviour
{
    /// <summary>
    /// ĸ�����˹ͨ
    /// </summary>
    public Haunter ParentHaunter;


    // Start is called before the first frame update
    void Start()
    {
        //���ð�Ӱצ����Ч��Ϊ��
        transform.GetChild(8).rotation = Quaternion.identity;
        //���ð�Ӱצ��ײ���ĸ����Ϊ��
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
