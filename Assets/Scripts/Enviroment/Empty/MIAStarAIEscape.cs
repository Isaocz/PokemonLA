using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MIAStarAIEscape : MonoBehaviour
{
    public float StaticTimer;
    Vector3 LastPosition;

    public Vector3 targetPosition;
    private Seeker seeker;
    private BoxCollider2D controller;
    public Path path;
    public float speed;
    public float nextWaypointDistance;
    private int currentWaypoint = 0;
    public bool reachedEndOfPath;
    public bool isCanNotMove;
    int RePathFind;
    Empty ParentEmpty;


    public void Start()
    {
        isCanNotMove = true;
        ParentEmpty = GetComponent<Empty>();
        LastPosition = transform.position;
    }

    public void Escape(Vector3 EscapePoint )
    {
        seeker = GetComponent<Seeker>();
        targetPosition = EscapePoint;
        seeker.StartPath(transform.position, targetPosition, OnPathComplete);
        isCanNotMove = false;
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
        speed = 7*ParentEmpty.speed;
        if (!isCanNotMove && !ParentEmpty.isBorn && !ParentEmpty.isDie && !ParentEmpty.isHit && !ParentEmpty.isSilence && !ParentEmpty.isEmptyFrozenDone)
        {
            //Debug.Log(1);
            RePathFind++;
            if (path == null)
            {
                return;
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
            Vector3 velocity = dir * speed;
            transform.position += velocity * Time.deltaTime;
        }
        if ((transform.position-targetPosition).magnitude <= 0.1f)
        {
            isCanNotMove = true;
            ParentEmpty.GetComponent<Mareep>().EscapOver();
        }
        

        


    }
    private void FixedUpdate()
    {
        if (!isCanNotMove && Mathf.Abs((LastPosition - transform.position).magnitude) <= 0.01f)
        {
            StaticTimer++;
            if(StaticTimer >= 15)
            {
                StaticTimer = 0;
                isCanNotMove = true;
                ParentEmpty.GetComponent<Mareep>().EscapOver();
            }
        }
        LastPosition = transform.position;
    }
}
