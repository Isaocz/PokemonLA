using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敌人小队
/// </summary>
public class EmptyTeamManger : MonoBehaviour
{

    //小队管理器状态机
    enum MangerState
    {
        StartDelay,     //启动延迟状态，该状态不进行任何操作，结束后进入Normal
        Normal,         //一般状态，此状态每次FixedUpdata中检查一次当前的排头是否可以执行小队操作，如果可以则执行并进入CheckInterval状态
        CheckInterval,  //操作间隔状态，此状态结束后检查下一个排头  
        Ban,            //小队被禁止
    }
    MangerState NowState;


    /// <summary>
    /// 敌人小队的queue
    /// </summary>
    public Queue<Empty> TeamQueue = new Queue<Empty> { };
    
    

    /// <summary>
    /// 小队行动的开始延迟
    /// </summary>
    public float StartDelay;
    /// <summary>
    /// 小队行动在不同成员之间执行时的间隔
    /// </summary>
    public float TeamInterval;
    /// <summary>
    /// 小队操作是否循环执行（即操作完的敌人会加入队尾重新排序）
    /// </summary>
    public bool isLoop;

    /// <summary>
    /// 连续检查计时器
    /// </summary>
    float CheckFaildTimer;
    /// <summary>
    /// 连续检查不符合条件则跳过时 连续检查的时间
    /// </summary>
    public float TIMECheckFaild;

    /// <summary>
    /// 上回操作的敌人
    /// </summary>
    Empty LastActEmpty;



    /// <summary>
    /// 在小队管理器Start时调用
    /// </summary>
    public void EmptyTeamStart()
    {
        Debug.Log("start");
        NowState = MangerState.StartDelay;
        GetTeamQueue();
        if (TeamQueue.Count != 0)//如果队列为空，不启动管理器
        {
            StartCoroutine(StartDelayOver(StartDelay));
        }
        
        
    }


    /// <summary>
    /// 在小队管理器Update时调用
    /// </summary>
    public void EmptyTeamUpdate()
    {
        //Debug.Log("update");
    }

    /// <summary>
    /// 在小队管理器FixedUpdate时调用
    /// </summary>
    public virtual void EmptyTeamFixedUpdate()
    {
        //Debug.Log("fixedupdate");
        if (TeamQueue.Count > 1) {
            int c = CheckQueueHead();

            //如果连续检查 但排头不符合条件 增加计时器
            if (c == 1)
            {
                CheckFaildTimer += Time.deltaTime;
            }
            //反之清零
            else { CheckFaildTimer = 0.0f; }

            //如果连续检查失败 则跳过排头
            if (CheckFaildTimer >= TIMECheckFaild)
            {
                if (TeamQueue.Count > 1)
                {
                    Empty JumpEmpty = TeamQueue.Dequeue();
                    TeamQueue.Enqueue(JumpEmpty);
                }
            }
        }
    }



    //=========================所有小队共通============================

    /// <summary>
    /// 获取所有符合条件的敌人,将其组成队列
    /// </summary>
    protected void GetTeamQueue()
    {
        //遍历每个敌人，检查其是否可以被加入队列
        for (int i = 0; i < transform.childCount; i++)
        {
            Empty e = transform.GetChild(i).GetComponent<Empty>();
            if (e && EmptyTeamEnqueueCondition(e) && !TeamQueue.Contains(e))
            {
                TeamQueue.Enqueue(e);
            }
        }
    }

    /// <summary>
    /// 打印当前小队队列
    /// </summary>
    protected void PrintTeamQueue()
    {
        foreach (Empty e in TeamQueue)
        {
            Debug.Log(e.gameObject.name);
        }
    }

    /// <summary>
    /// 结束开始延迟状态
    /// </summary>
    IEnumerator StartDelayOver(float delay)
    {
        yield return new WaitForSeconds(delay);
        NowState = MangerState.Normal;
    }

    /// <summary>
    /// 根据时间间隔，检查当前的排头是否可使用小队行动
    /// </summary>
    IEnumerator Interval(float teamInterval)
    {
        yield return new WaitForSeconds(teamInterval);
        NowState = MangerState.Normal;
    }

    /// <summary>
    /// 检查队列排头 0:检查未进行 1;检查进行但不符合条件 ;检查符合条件 开始操作
    /// </summary>
    int CheckQueueHead()
    {
        if (NowState == MangerState.Normal && TeamQueue.Count > 1)
        {
            //排空
            //Debug.Log(TeamQueue.Count);
            //Debug.Log((string.Join(", ", TeamQueue)));
            Empty e = null;
            while (TeamQueue.Count > 0 && TeamQueue.TryPeek(out e) && e == null)
            {
                TeamQueue.Dequeue();
            }
            if (TeamQueue.Count == 0) { return 0; }



            //如果排头符合启动小队操作的条件
            if (EmptyTeamActCondition(TeamQueue.Peek()))
            {
                //行动
                Empty ActEmpty = TeamQueue.Dequeue();
                TeamEmptyAct(ActEmpty);
                if (isLoop && ActEmpty.gameObject != null) { TeamQueue.Enqueue(ActEmpty); }
                //设置冷却状态
                NowState = MangerState.CheckInterval;
                StartCoroutine(Interval(TeamInterval));
                return 2;
            }
            else { return 1; }
        }
        return 0;
    }

    //=========================所有小队共通============================




    //=========================根据不同小队不同（可被复写）============================

    /// <summary>
    /// 该敌人小队的加入条件 判断每个敌人是否可以被加入小队
    /// </summary>
    /// <returns></returns>
    protected virtual bool EmptyTeamEnqueueCondition(Empty empty)
    {
        return (empty.isActiveAndEnabled);
    }

    /// <summary>
    /// 该敌人小队触发小队操作的条件 判断每个敌人在作为排头被检查时是否可以触发小队操作
    /// </summary>
    /// <returns></returns>
    protected virtual bool EmptyTeamActCondition(Empty empty)
    {
        //如果小队当前操作者 和上次操作者相同 则不符合条件
        return (empty != LastActEmpty);
    }

    /// <summary>
    /// 敌人小队的行动
    /// </summary>
    /// <param name="empty"></param>
    protected virtual void TeamEmptyAct(Empty empty)
    {
        LastActEmpty = empty;
    }

    //=========================根据不同小队不同（可被复写）============================

}