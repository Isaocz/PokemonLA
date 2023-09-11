using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class MasquerainPathFinder : MonoBehaviour
{
    
    public Vector3 targetPosition;
    public Vector3 faceDir = new Vector3(-1,-1,0);
    private Rigidbody2D rigi;
    private Seeker seeker;
    public Path path;
    private float speed;
    public float nextWaypointDistance = 0.8f;
    private int currentWaypoint = 0;
    public bool reachedEndOfPath;
    Empty ParentEmpty;
    private bool walking = false;
    public bool Walking
    {
        get => walking;
    }

    private Action onFinish;
    private List<Vector2> historyPos = new List<Vector2>();
    public void Start()
    {
        ParentEmpty = GetComponent<Empty>();
        rigi = GetComponent<Rigidbody2D>();
        seeker = GetComponent<Seeker>();
        speed = ParentEmpty.speed;
        
        // 辅助判断是否撞墙，防止卡在一个地方
        Timer.Start(this, 0.2f, true, () =>
        {
            if (walking && !ParentEmpty.isBorn && !ParentEmpty.isDie && !ParentEmpty.isHit && 
                !ParentEmpty.isSilence && !ParentEmpty.isEmptyFrozenDone  && !ParentEmpty.isSleepDone && 
                !ParentEmpty.isCanNotMoveWhenParalysis)
            {
                Vector2 rigiPos = rigi.position;
                if (historyPos.Count >= 5)
                {
                    historyPos.RemoveAt(0);
                }
                historyPos.Add(rigiPos);

                if (historyPos.Count >= 5)
                {
                    float disX = 0, disY = 0;
                    for (int i = 1; i < historyPos.Count; i++)
                    {
                        disX += Math.Abs(historyPos[i].x - historyPos[i - 1].x);
                        disY += Math.Abs(historyPos[i].y - historyPos[i - 1].y);
                    }
        
                    if (disX < 0.0001 && disY < 0.0001)
                    {
                        walking = false;
                        onFinish();
                    }
                }
            }
        });
    }
    public void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
        walking = true;
    }
    // public void Update()
    // {
    // }
    
    private void FixedUpdate()
    {
        speed = ParentEmpty.speed;
        if (walking && !ParentEmpty.isBorn && !ParentEmpty.isDie && !ParentEmpty.isHit && !ParentEmpty.isSilence && !ParentEmpty.isEmptyFrozenDone)
        {
            reachedEndOfPath = false;
            while (true)
            {
                float distanceToWaypoint = Vector3.Distance(transform.position, path.vectorPath[currentWaypoint]);
                if (distanceToWaypoint < nextWaypointDistance)
                {
                    if (currentWaypoint + 1 < path.vectorPath.Count)
                    {
                        currentWaypoint++;
                    }
                    else
                    {
                        reachedEndOfPath = true;
                        break;
                    }
                }
                else
                {
                    break;
                }
            }

            bool move = false;
            if (!reachedEndOfPath)
            {
                faceDir = (path.vectorPath[currentWaypoint] - transform.position).normalized;
                // if (ParentEmpty.isEmptyConfusionDone) { dir = (dir + new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1), 0)).normalized; }
                Vector3 velocity = faceDir * speed;
                // Vector2 rigiPos = rigi.position;
                if ((new Vector3(rigi.position.x, rigi.position.y) - targetPosition).magnitude >= 0.5f)
                {
                    rigi.MovePosition(rigi.position + new Vector2(velocity.x, velocity.y) * Time.fixedDeltaTime);
                    move = true;
                }
            }

            if (!move)
            {
                // 到了目的地，不再需要移动
                walking = false;
                onFinish();
            }
        }
    }

    public void SetTargetPos(Vector3 target, Action cb = null)
    {
        targetPosition = target;
        if (cb == null)
        {
            cb = ()=>{};
        }
        onFinish = cb;
        seeker.StartPath(rigi.position, targetPosition, OnPathComplete);
    }

    public void Stop()
    {
        walking = false;
    }
}
