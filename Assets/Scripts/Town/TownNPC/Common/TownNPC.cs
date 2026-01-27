using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using static PathManager;

public class TownNPC : NPC<PlayerPokemon>
{
    [HideInInspector] public bool isTeleporting;
    public enum NPCState
    {
        NotJoined,  //未加入小镇
        NotinTown,  //加入小镇但不在小镇
        inTown
    }
    public NPCState npcState;

    public enum NPCAction
    {
        Idle,       //站着不动
        Walk        //走路中
    }
    public NPCAction npcAction;

    /// <summary>
    /// 表示NPC所处位置
    /// </summary>
    public enum NPCLocation
    {
        inMilkBar, //在酒吧中
        inTown,    //在镇上
        inWoodenHouse,    //在铁骨建筑公司木屋内
        inSkillMaker,    //在图图技能艺术廊内
        inDayCareF1,    //在破壳宝育园一层
        inDayCareF2,    //在破壳宝育园二层
        inItemShop,    //在道具商店
        inBossClub,    //在头目俱乐部
        inPoliceStation,    //在冒险家俱乐部
        inRockClub,    //在滚石俱乐部
    }
    public NPCLocation location;

    /// <summary>
    /// npc时间表节点
    /// </summary>
    [System.Serializable]
    public struct NPCTimeTable
    {
        public Vector3 EndPosition;
        public int StartTime;
        public int EndTime;
    }

    private struct PathSegment
    {
        public Vector3 StartPoint;
        public Vector3 EndPoint;
        public ControlPoint ControlPoint;
        public bool IsCurve;
    }

    public NPCState InitialState;//npc初始状态
    public NPCLocation InitialLocation; //npc初始房间位置
    public Vector3 InitialTransform; //npc具体初始位置
    public List<NPCTimeTable> TimeTable = new List<NPCTimeTable>();//npc时间表

    /// <summary>
    /// NPC朝向
    /// </summary>
    public Vector2 Director;
    protected Rigidbody2D rb;
    public Vector3 targetPosition;
    public float turnSmoothSpeed = 2f;

    public float moveSpeed; //npc移动速度

    private bool hasActiveSchedule = false;
    private NPCTimeTable currentSchedule;

    private List<PathSegment> currentPathSegments;
    private int currentSegmentIndex;
    private float currentSegmentProgress;
    private bool isFollowingPath = false;

    [Header("避障设置")]
    public LayerMask obstacleLayers = LayerMask.GetMask("NPC", "Player");
    public float obstacleCheckDistance = 1.5f;
    public float obstacleCheckRadius = 0.3f;
    public float avoidanceDistance = 2f;

    private bool isAvoidingObstacle = false;
    private Vector3 avoidanceTarget;
    private Vector3 originalTargetBeforeAvoidance;
    private float originalSpeedBeforeAvoidance;
    private Coroutine avoidanceCoroutine;


    /// <summary>
    /// 初始化
    /// </summary>
    protected virtual void Initialize()
    {
        location = InitialLocation;
        npcState = InitialState;
        transform.position = InitialTransform;
        rb = GetComponent<Rigidbody2D>();
    }
    /// <summary>
    /// 设置NPC过门后的位置，同时也是重设位置
    /// </summary>
    /// <returns>重设位置</returns>
    public Vector3 InstanceNPCPosition()
    {
        Vector3 OutPut = Vector3.zero;
        switch (location)
        {
            case NPCLocation.inTown:
                break;
            case NPCLocation.inMilkBar:
                OutPut = new Vector3(1016.27f, 1.98f, 0);
                break;
            case NPCLocation.inWoodenHouse:
                OutPut = new Vector3(202.45f, -1.51f, 0);
                break;
            case NPCLocation.inSkillMaker:
                OutPut = new Vector3(400.0f, -0.14f, 0);
                break;
            case NPCLocation.inDayCareF1:
                OutPut = new Vector3(606.96f, -5.41f, 0);
                break;
            case NPCLocation.inDayCareF2:
                OutPut = new Vector3(794.278f, 3.53f, 0);
                break;
            case NPCLocation.inItemShop:
                OutPut = new Vector3(200.0f, 201.6f, 0);
                break;
            case NPCLocation.inBossClub:
                OutPut = new Vector3(387.04f, 197.85f, 0);
                break;
            case NPCLocation.inPoliceStation:
                OutPut = new Vector3(605.67f, 197.34f, 0);
                break;
            case NPCLocation.inRockClub:
                OutPut = new Vector3(789.88f, 200.14f, 0);
                break;
        }
        return OutPut;
    }


    //■■■■■■■■■■■■■■■■■■■■移动相关■■■■■■■■■■■■■■■■■■■■■■

    /// <summary>
    /// 设置朝向和动画机方向
    /// </summary>
    public void SetDirector(Vector2 director)
    {
        Director = director;
        animator.SetFloat("LookX", director.x);
        animator.SetFloat("LookY", director.y);
    }

    Vector3 LastPosition;//计算当前速度,朝向时，采用的上一时间单位的位置坐标,通过携程执行

    /// <summary>
    /// 设置动画机速度
    /// </summary>
    /// <returns></returns>
    public IEnumerator CheckLook()
    {
        while (true)
        {
            //一般状态时更改速度和朝向
            if (npcAction == NPCAction.Walk)
            {
                //根据当前位置和上一次FixedUpdate调用时的位置差计算速度
                animator.SetFloat("Speed", Mathf.Abs((transform.position - LastPosition).magnitude));
                //根据当前位置和上一次FixedUpdate调用时的位置差计算朝向 并传给动画组件
                //SetDirector(_mTool.MainVector2((transform.position - LastPosition)));
                //重置位置
                LastPosition = transform.position;
            }
            else
            {
                animator.SetFloat("Speed", 0);
            }
            yield return new WaitForSeconds(0.1f);
        }

    }

    protected virtual void FixedUpdate()
    {
        if (isTeleporting) return;
        CheckSchedule();

        if (isFollowingPath && !isAvoidingObstacle)
        {
            if (CheckForObstacles())
            {
                // 开始避障
                StartAvoidance();
            }
            else
            {
                FollowPath();
            }
                
        }
    }

    private void CheckSchedule()
    {
        int currentMinutes = TownGlobalTimer.instance.GetMinutes();
        bool foundActiveSchedule = false;

        foreach (var schedule in TimeTable)
        {
            if (currentMinutes >= schedule.StartTime && currentMinutes < schedule.EndTime)
            {
                foundActiveSchedule = true;

                // 如果这是新的时间表或目标位置已更改
                if (!hasActiveSchedule || currentSchedule.EndPosition != schedule.EndPosition)
                {
                    hasActiveSchedule = true;
                    currentSchedule = schedule;
                    targetPosition = schedule.EndPosition;
                    MoveToPosition();
                }
                break;
            }
        }

        // 如果没有活跃的时间表，停止移动
        if (!foundActiveSchedule && hasActiveSchedule)
        {
            hasActiveSchedule = false;
            StopMovement();
        }
    }

    private void MoveToPosition()
    {
        StopAvoidance();
        // 如果已经在目标位置，不需要移动
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            StopMovement();
            return;
        }

        // 构建路径段列表，包含起点（当前位置）和终点（目标位置）
        currentPathSegments = new List<PathSegment>();

        // 添加从当前位置到第一个路径点的直线段
        PathPoint firstPathPoint = PathManager.instance.GetNearestPoint(transform.position);
        if (firstPathPoint == null)
        {
            Debug.LogError("无法找到最近的路径点!");
            StopMovement();
            return;
        }

        // 第一段：从当前位置到第一个路径点（直线）
        PathSegment firstSegment = new PathSegment
        {
            StartPoint = transform.position,
            EndPoint = firstPathPoint.Position,
            ControlPoint = null,
            IsCurve = false
        };
        currentPathSegments.Add(firstSegment);

        // 获取路径点ID列表
        PathPoint endPathPoint = PathManager.instance.GetNearestPoint(targetPosition);
        if (endPathPoint == null)
        {
            Debug.LogError("无法找到目标位置附近的路径点!");
            StopMovement();
            return;
        }

        List<string> pathIDs = PathManager.instance.FindPathIDs(firstPathPoint.PathID, endPathPoint.PathID);
        Debug.Log("PathPoint首位ID：" + firstPathPoint.PathID + " PathPoint末位ID" + endPathPoint.PathID);
        if (pathIDs == null || pathIDs.Count == 0)
        {
            Debug.LogError("无法找到路径！请检查路径点连线是否正确设置！");
            Debug.Log("PathPoint首位ID：" + firstPathPoint.PathID + " PathPoint末位ID" +  endPathPoint.PathID);
            StopMovement();
            return;
        }

        // 构建中间路径段
        for (int i = 0; i < pathIDs.Count - 1; i++)
        {
            string currentID = pathIDs[i];
            string nextID = pathIDs[i + 1];

            PathPoint currentPoint = PathManager.instance.GetPathPoint(currentID);
            PathPoint nextPoint = PathManager.instance.GetPathPoint(nextID);

            // 检查这两点之间是否有控制点
            ControlPoint controlPoint = PathManager.instance.GetControlPoint(currentID, nextID);

            PathSegment segment = new PathSegment
            {
                StartPoint = currentPoint.Position,
                EndPoint = nextPoint.Position,
                ControlPoint = controlPoint,
                IsCurve = controlPoint != null
            };

            currentPathSegments.Add(segment);
        }

        // 添加从最后一个路径点到目标位置的直线段
        PathSegment lastSegment = new PathSegment
        {
            StartPoint = endPathPoint.Position,
            EndPoint = targetPosition,
            ControlPoint = null,
            IsCurve = false
        };
        currentPathSegments.Add(lastSegment);

        // 检查列表设置是否正确
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("路径段列表内容:");

        for (int i = 0; i < currentPathSegments.Count; i++)
        {
            var segment = currentPathSegments[i];
            sb.AppendLine($"[{i}]: 起点={segment.StartPoint}, 终点={segment.EndPoint}, 控制点={segment.ControlPoint?.Position}, 是否曲线={segment.IsCurve}");
        }

        Debug.Log(sb.ToString());

        currentSegmentIndex = 0;
        currentSegmentProgress = 0f;
        isFollowingPath = true;
        npcAction = NPCAction.Walk;
    }


    private void FollowPath()
    {
        if (currentPathSegments == null || currentSegmentIndex >= currentPathSegments.Count)
        {
            StopMovement();
            return;
        }

        PathSegment currentSegment = currentPathSegments[currentSegmentIndex];
        Vector2 targetPosition;


        if (currentSegment.IsCurve)
        {
            // 使用二次贝塞尔曲线计算位置
            float t = currentSegmentProgress;
            targetPosition = CalculateBezierPoint(
                t,
                currentSegment.StartPoint,
                currentSegment.ControlPoint.Position,
                currentSegment.EndPoint
            );

            // 计算切线方向用于朝向
            Vector2 tangent = CalculateBezierTangent(
                t,
                currentSegment.StartPoint,
                currentSegment.ControlPoint.Position,
                currentSegment.EndPoint
            ).normalized;

            SetDirector(tangent);
        }
        else
        {
            // 直线移动
            targetPosition = currentSegment.EndPoint;

            // 计算直线方向
            Vector2 direction = (currentSegment.EndPoint - currentSegment.StartPoint).normalized;
            SetDirector(direction);
        }

        // 移动到目标位置
        Vector2 moveDirection = (targetPosition - (Vector2)transform.position).normalized;
        Debug.Log("目标位置：" + targetPosition + " 自身位置：" + transform.position + " 移动方向："+ moveDirection);
        if (rb != null)
        {
            rb.velocity = moveDirection * moveSpeed;
        }


        if (currentSegment.IsCurve)
        {
            float segmentLength = EstimateBezierLength(currentSegment.StartPoint, currentSegment.ControlPoint.Position, currentSegment.EndPoint);
            if(segmentLength > 0.1f)
            {
                float progressIncrement = (moveSpeed * Time.fixedDeltaTime) / segmentLength;
                currentSegmentProgress = Mathf.Clamp01(currentSegmentProgress + progressIncrement);
            }
            else
            {
                currentSegmentProgress = 1;
            }
        }
        else
        {
            float segmentLength = Vector2.Distance(currentSegment.StartPoint, currentSegment.EndPoint);
            if (segmentLength > 0.1f)
            {
                float progressIncrement = 1 - (Vector2.Distance(transform.position, currentSegment.EndPoint) / segmentLength);
                currentSegmentProgress = Mathf.Clamp01(progressIncrement);

            }
            else
            {
                currentSegmentProgress = 1;
            }
            
        }
        Debug.Log($"当前路径段: {currentSegmentIndex}/{currentPathSegments.Count}, 进度: {currentSegmentProgress}");
        // 检查是否完成当前段
        if (currentSegmentProgress >= 1f || Vector2.Distance(transform.position, currentSegment.EndPoint) < 0.1f)
        {
            currentSegmentIndex++;
            currentSegmentProgress = 0f;

            // 检查是否到达最终目标
            if (currentSegmentIndex >= currentPathSegments.Count)
            {
                StopMovement();
            }
        }
    }

    private Vector3 CalculateBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;

        Vector3 p = uu * p0;
        p += 2 * u * t * p1;
        p += tt * p2;

        return p;
    }

    private Vector3 CalculateBezierTangent(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        return 2 * (1 - t) * (p1 - p0) + 2 * t * (p2 - p1);
    }

    private float EstimateBezierLength(Vector3 p0, Vector3 p1, Vector3 p2, int segments = 10)
    {
        float length = 0;
        Vector3 prevPoint = p0;

        for (int i = 1; i <= segments; i++)
        {
            float t = i / (float)segments;
            Vector3 point = CalculateBezierPoint(t, p0, p1, p2);
            length += Vector3.Distance(prevPoint, point);
            prevPoint = point;
        }

        return length;
    }


    public void RecalculatePath()
    {
        // 先完全停止当前移动
        StopMovement();

        // 重置所有路径相关变量
        currentPathSegments = null;
        currentSegmentIndex = 0;
        currentSegmentProgress = 0f;
        isFollowingPath = false;

        // 等待一帧确保状态完全重置
        StartCoroutine(DelayedRecalculate());
    }

    private IEnumerator DelayedRecalculate()
    {
        // 等待一帧确保所有状态已重置
        yield return null;

        if (hasActiveSchedule)
        {
            // 重新计算路径
            MoveToPosition();
        }
    }

    void StopMovement()
    {
        npcAction = NPCAction.Idle;
        isFollowingPath = false;
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
        }
    }

    //■■■■■■■■■■■■■■■■■■■■移动相关■■■■■■■■■■■■■■■■■■■■■■



    //■■■■■■■■■■■■■■■■■■■■避障相关■■■■■■■■■■■■■■■■■■■■

    private bool CheckForObstacles()
    {
        if (!isFollowingPath || currentPathSegments == null || currentSegmentIndex >= currentPathSegments.Count)
            return false;

        Vector2 moveDirection = GetCurrentMoveDirection();
        if (moveDirection.magnitude < 0.1f) return false;

        // 动态检测距离：速度越快，看得越远
        float dynamicCheckDist = obstacleCheckDistance + moveSpeed * 0.3f;

        RaycastHit2D hit = Physics2D.CircleCast(
            transform.position,
            obstacleCheckRadius,
            moveDirection,
            dynamicCheckDist,
            obstacleLayers
        );

        if (hit.collider != null && hit.collider.gameObject != gameObject)
        {
            Debug.Log($"碰到障碍物: {hit.collider.gameObject.name}, 距离: {hit.distance}");
            return true;
        }

        return false;
    }

    private Vector2 GetCurrentMoveDirection()
    {
        if (currentSegmentIndex < currentPathSegments.Count)
        {
            PathSegment segment = currentPathSegments[currentSegmentIndex];
            if (segment.IsCurve && segment.ControlPoint != null)
            {
                return CalculateBezierTangent(currentSegmentProgress,
                    segment.StartPoint, segment.ControlPoint.Position, segment.EndPoint).normalized;
            }
            else
            {
                return (segment.EndPoint - (Vector3)transform.position).normalized;
            }
        }
        return Director;
    }

    private void StartAvoidance()
    {
        if (isAvoidingObstacle) return;

        Debug.Log("开始避障");

        isAvoidingObstacle = true;
        originalSpeedBeforeAvoidance = moveSpeed;
        originalTargetBeforeAvoidance = targetPosition;

        avoidanceTarget = FindAvoidancePoint();

        if (avoidanceCoroutine != null)
            StopCoroutine(avoidanceCoroutine);

        avoidanceCoroutine = StartCoroutine(AvoidObstacle());
    }

    private Vector3 FindAvoidancePoint()
    {
        Vector2 moveDirection = GetCurrentMoveDirection();
        if (moveDirection.magnitude < 0.1f) moveDirection = Director;

        float[] angles = { -90, -60, -30, 0, 30, 60, 90 };
        List<Vector3> validPoints = new List<Vector3>();

        foreach (float angle in angles)
        {
            Quaternion rot = Quaternion.Euler(0, 0, angle);
            Vector2 testDir = rot * moveDirection;
            Vector3 testPoint = transform.position + (Vector3)testDir * avoidanceDistance;

            if (!Physics2D.OverlapCircle(testPoint, obstacleCheckRadius, obstacleLayers))
            {
                validPoints.Add(testPoint);
            }
        }

        if (validPoints.Count > 0)
        {
            Vector3 best = validPoints[0];
            float bestScore = -2f;
            foreach (var pt in validPoints)
            {
                Vector2 toTarget = ((Vector2)originalTargetBeforeAvoidance - (Vector2)pt).normalized;
                float score = Vector2.Dot(toTarget, moveDirection);
                if (score > bestScore)
                {
                    bestScore = score;
                    best = pt;
                }
            }
            Debug.Log($"找到最优避障点: {best}");
            return best;
        }

        Vector3 fallback = transform.position - (Vector3)moveDirection * avoidanceDistance * 0.3f;
        Debug.Log($"全部方向受阻，使用后退点: {fallback}");
        return fallback;
    }

    private IEnumerator AvoidObstacle()
    {
        moveSpeed = originalSpeedBeforeAvoidance * 0.7f;

        while (Vector3.Distance(transform.position, avoidanceTarget) > 0.2f && isAvoidingObstacle)
        {
            Vector2 direction = (avoidanceTarget - transform.position).normalized;
            rb.velocity = direction * moveSpeed;
            SetDirector(direction);
            yield return new WaitForFixedUpdate();
        }

        // 直接恢复，不等待
        isAvoidingObstacle = false;
        moveSpeed = originalSpeedBeforeAvoidance;
        Debug.Log("避障完成，尝试恢复原路径");

        ResumePathAfterAvoidance();
    }

    private void ResumePathAfterAvoidance()
    {
        if (currentPathSegments != null && currentSegmentIndex < currentPathSegments.Count)
        {
            PathSegment currentSeg = currentPathSegments[currentSegmentIndex];
            float distToPath = Vector2.Distance(transform.position, GetClosestPointOnSegment(currentSeg));
            if (distToPath < 1.5f)
            {
                isFollowingPath = true;
                npcAction = NPCAction.Walk;
                return;
            }
        }
        RecalculatePath();
    }

    private Vector2 GetClosestPointOnSegment(PathSegment seg)
    {
        if (!seg.IsCurve)
        {
            Vector2 a = seg.StartPoint;
            Vector2 b = seg.EndPoint;
            Vector2 p = transform.position;
            Vector2 ab = b - a;
            float t = Vector2.Dot(p - a, ab) / Vector2.Dot(ab, ab);
            t = Mathf.Clamp01(t);
            return a + t * ab;
        }
        else
        {
            return seg.StartPoint;
        }
    }

    public void StopAvoidance()
    {
        if (isAvoidingObstacle)
        {
            isAvoidingObstacle = false;
            moveSpeed = originalSpeedBeforeAvoidance;

            if (avoidanceCoroutine != null)
            {
                StopCoroutine(avoidanceCoroutine);
                avoidanceCoroutine = null;
            }

            RecalculatePath();
        }
    }

    //■■■■■■■■■■■■■■■■■■■■避障相关■■■■■■■■■■■■■■■■■■■■
}
