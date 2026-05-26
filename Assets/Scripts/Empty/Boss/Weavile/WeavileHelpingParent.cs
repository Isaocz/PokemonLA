using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeavileHelpingParent : WeavileSupportParent
{

    /// <summary>
    /// 子件预制件
    /// </summary>
    public WeavileHelpingSon HelpingSonPrefab;

    /// <summary>
    /// 子件列表
    /// </summary>
    public List<WeavileHelpingSon> SonList = new List<WeavileHelpingSon>();

    /// <summary>
    /// 动画预制件
    /// </summary>
    public Animator animator;

    /// 帮助时间
    /// </summary>
    float HelpingTime = 0.0f;

    /// <summary>
    /// 开始帮助
    /// </summary>
    public void HelpingStart(List<Weavile> wList , float time)
    {
        HelpingTime = time;

        if (animator != null) { animator.gameObject.transform.parent = this.transform.parent; }
        
        for (int i = 0; i < wList.Count; i++)
        {
            WeavileHelpingSon son = Instantiate(HelpingSonPrefab, wList[i].transform.position , Quaternion.identity , wList[i].transform );
            son.HelpingStart(wList[i] , HelpingTime);
            SonList.Add(son);
        }
        Destroy(gameObject, HelpingTime);
    }

    /// <summary>
    /// 结束帮助
    /// </summary>
    public override void SupportOver()
    {
        if (animator != null) {
            Debug.Log("Over");
            animator.SetTrigger("Over"); }

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
