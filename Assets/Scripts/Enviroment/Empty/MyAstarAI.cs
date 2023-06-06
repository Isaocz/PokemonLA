using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyAstarAI : MonoBehaviour
{
    public float StaticTimer;
    public Transform targetPosition;
    private Seeker seeker;
    private BoxCollider2D controller;
    public Path path;
    public float speed = 2;
    public float nextWaypointDistance = 3;
    private int currentWaypoint = 0;
    public bool reachedEndOfPath;
    public bool isCanNotMove;
    int RePathFind;
    Empty ParentEmpty;
    Vector3 RunTargetPosition;
    Vector3 LastPosition;
    bool isEscape;
    public void Start()
    {
        ParentEmpty = GetComponent<Empty>();
        targetPosition =  ParentEmpty.player.transform;
        RunTargetPosition = targetPosition.position;
        seeker = GetComponent<Seeker>();
        controller = GetComponent<BoxCollider2D>();
        seeker.StartPath(transform.position, RunTargetPosition, OnPathComplete);
        LastPosition = transform.position;
    }
    public void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }
    public void Update()
    {
        speed = ParentEmpty.speed;
        if (!ParentEmpty.isBorn && !ParentEmpty.isDie && !ParentEmpty.isHit && !ParentEmpty.isSilence && !ParentEmpty.isEmptyFrozenDone && !isCanNotMove) {
            RePathFind++;
            if (!ParentEmpty.isFearDone) {
                if (RePathFind >= 40)
                {
                    RunTargetPosition = targetPosition.position;
                    seeker.StartPath(transform.position, RunTargetPosition, OnPathComplete);
                    RePathFind = 0;
                }
                if (path == null)
                {
                    return;
                }
            }else if (ParentEmpty.isFearDone && !isEscape)
            {
                if ((transform.position - targetPosition.position).magnitude <= 10.5f) {
                    RunTargetPosition = ((transform.position - targetPosition.position).normalized) * (10.5f) + targetPosition.position;
                    while ((RunTargetPosition.x <= ParentEmpty.transform.parent.position.x - 11 || RunTargetPosition.x >= ParentEmpty.transform.parent.position.x + 11) || (RunTargetPosition.y <= ParentEmpty.transform.parent.position.y - 6 || RunTargetPosition.y >= ParentEmpty.transform.parent.position.y + 6))
                    {
                        RunTargetPosition = Quaternion.AngleAxis(Random.Range(-100, 100), Vector3.forward) * (5 * Vector3.up) + targetPosition.position;
                    }
                }
                else
                {
                    RunTargetPosition = transform.position;
                }
                seeker.StartPath(transform.position, RunTargetPosition, OnPathComplete);
                //RePathFind = 0;
                isEscape = true;
            }
            reachedEndOfPath = false;
            float distanceToWaypoint;
            while (true)
            {
                distanceToWaypoint = Vector3.Distance(transform.position, path.vectorPath[currentWaypoint]);
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

            Vector3 dir = (path.vectorPath[currentWaypoint] - transform.position).normalized;
            Vector3 velocity = dir * (ParentEmpty.isFearDone?speed*2:speed);
            if ((transform.position - RunTargetPosition).magnitude >= 0.5f) {
                transform.position += velocity * Time.deltaTime;
            }
            else
            {
                isEscape = false;
            }
        }
    }

    private void FixedUpdate()
    {
        if (Mathf.Abs((LastPosition - transform.position).magnitude) <= 0.01f)
        {
            StaticTimer++;
            if (StaticTimer >= 15)
            {
                StaticTimer = 0;
                isEscape = false;
            }
        }
        LastPosition = transform.position;
    }
}
