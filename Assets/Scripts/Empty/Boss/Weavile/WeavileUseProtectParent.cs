using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeavileUseProtectParent : WeavileSupportParent
{

    /// <summary>
    /// 子件预制件
    /// </summary>
    public WeavileUseProtectSon ProtectSonPrefab;

    /// <summary>
    /// 子件列表
    /// </summary>
    public List<WeavileUseProtectSon> SonList = new List<WeavileUseProtectSon>();

    /// <summary>
    /// 动画预制件
    /// </summary>
    public Animator animator;

    /// 保护时间
    /// </summary>
    float ProtectTime = 0.0f;

    /// <summary>
    /// 开始保护
    /// </summary>
    public void HelpingStart(List<Weavile> wList, float time)
    {
        ProtectTime = time;

        if (animator != null) { animator.gameObject.transform.parent = this.transform.parent; }

        for (int i = 0; i < wList.Count; i++)
        {
            WeavileUseProtectSon son = Instantiate(ProtectSonPrefab, wList[i].transform.position, Quaternion.identity, wList[i].transform);
            son.HelpingStart(wList[i], ProtectTime);
            SonList.Add(son);
        }
        Destroy(gameObject, ProtectTime);
    }

    /// <summary>
    /// 开始保护 保护替身
    /// </summary>
    public void HelpingStart(List<WeavileSubstituteOBJ> wList, float time)
    {
        ProtectTime = time;

        if (animator != null) { animator.gameObject.transform.parent = this.transform.parent; }

        for (int i = 0; i < wList.Count; i++)
        {
            WeavileUseProtectSon son = Instantiate(ProtectSonPrefab, wList[i].transform.position, Quaternion.identity, wList[i].transform);
            son.HelpingStart(wList[i], ProtectTime);
            SonList.Add(son);
        }
        Destroy(gameObject, ProtectTime);
    }


    /// <summary>
    /// 结束保护
    /// </summary>
    public override void SupportOver()
    {
        if (animator != null)
        {
            Debug.Log("Over");
            animator.SetTrigger("Over");
        }

        for (int i = 0; i < SonList.Count; i++)
        {
            if (SonList[i] != null)
            {
                SonList[i].HelpingOver();
            }
        }
    }


    private void OnDestroy()
    {
        SupportOver();
    }

}
