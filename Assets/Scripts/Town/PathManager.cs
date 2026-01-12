using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PathManager : MonoBehaviour
{
    public static PathManager instance;
    /// <summary>
    /// 表示路径点所处位置
    /// </summary>
    public enum Location
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

    /// <summary>
    /// 路径点
    /// </summary>
    [System.Serializable]
    public class PathPoint
    {
        public string PathID;
        public Vector3 Position;
        public Location location;
        public List<string> ConnectPoint = new List<string>();
        public bool IsDoor;             //判断是否是门
        public string PairedDoorID;     //匹配的门的点ID
        public bool IsHidden = false;   //是否隐藏
    }

    /// <summary>
    /// 控制点 - 用于NPC曲线运动的贝塞尔曲线控制点
    /// </summary>
    [System.Serializable]
    public class ControlPoint
    {
        public string ControlID;
        public Vector3 Position;
        public string StartPathPointID; // 曲线起始路径点ID
        public string EndPathPointID;   // 曲线结束路径点ID
        public bool UseAsHandle = true; // 是否作为贝塞尔曲线的控制柄
    }

    /// <summary>
    /// 路径点列表
    /// </summary>
    public List<PathPoint> PathPoints = new List<PathPoint>();

    public List<ControlPoint> ControlPoints = new List<ControlPoint>();


    private Dictionary<string, PathPoint> pathPointDict = new Dictionary<string, PathPoint>();
    private Dictionary<string, ControlPoint> controlPointDict = new Dictionary<string, ControlPoint>();
    private Dictionary<string, string> doorPairs = new Dictionary<string, string>();

    void Awake()
    {
        instance = this;

        // 构建路径点字典以便快速查找
        foreach (var point in PathPoints)
        {
            pathPointDict[point.PathID] = point;

            // 构建过门点配对
            if (point.IsDoor && !string.IsNullOrEmpty(point.PairedDoorID))
            {
                doorPairs[point.PathID] = point.PairedDoorID;
                // 确保配对是双向的
                if (!doorPairs.ContainsKey(point.PairedDoorID))
                {
                    doorPairs[point.PairedDoorID] = point.PathID;
                }
            }
        }

        foreach (var controlPoint in ControlPoints)
        {
            controlPointDict[controlPoint.ControlID] = controlPoint;
        }

    }
    /// <summary>
    /// 获取路径点
    /// </summary>
    /// <param name="pointID">路径点ID</param>
    /// <returns>路径点结构</returns>
    public PathPoint GetPathPoint(string pointID)
    {
        return pathPointDict.ContainsKey(pointID) ? pathPointDict[pointID] : null;
    }

    /// <summary>
    /// 获取距离指定位置最近的路径点
    /// </summary>
    /// <param name="position">指定位置</param>
    /// <returns>最近的路径点</returns>
    public PathPoint GetNearestPoint(Vector3 position, bool includeHidden = false)
    {
        PathPoint nearestPoint = null;
        float minDistance = float.MaxValue;
        foreach (var point in PathPoints)
        {
            if (!includeHidden && point.IsHidden) continue;
            float distance = Vector3.Distance(position, point.Position);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearestPoint = point;
            }
        }
        return nearestPoint;
    }

    public PathPoint GetVisiblePathPoint(string pointID)
    {
        if (pathPointDict.TryGetValue(pointID, out var point))
        {
            //获取非隐藏路径点
            if (!point.IsHidden) return point;
        }
        return null;
    }

    public List<Vector3> FindPath(string startPointID, string endPointID)
    {
        if (!pathPointDict.ContainsKey(startPointID) || !pathPointDict.ContainsKey(endPointID))
            return null;
        var startPoint = GetVisiblePathPoint(startPointID);
        var endPoint = GetVisiblePathPoint(endPointID);
        if (startPoint == null || endPoint == null) return null;

        // A*算法实现
        Dictionary<string, string> cameFrom = new Dictionary<string, string>();
        Dictionary<string, float> gScore = new Dictionary<string, float>();
        Dictionary<string, float> fScore = new Dictionary<string, float>();

        HashSet<string> openSet = new HashSet<string>();
        HashSet<string> closedSet = new HashSet<string>();

        // 初始化起点
        gScore[startPointID] = 0;
        fScore[startPointID] = HeuristicCost(startPointID, endPointID);
        openSet.Add(startPointID);

        while (openSet.Count > 0)
        {
            // 找到fScore最小的节点
            string current = openSet.OrderBy(node => fScore.ContainsKey(node) ? fScore[node] : float.MaxValue).First();

            if (current == endPointID)
            {
                // 找到路径，重构路径
                return ReconstructPath(cameFrom, endPointID);
            }

            openSet.Remove(current);
            closedSet.Add(current);

            foreach (string neighbor in pathPointDict[current].ConnectPoint)
            {
                if (closedSet.Contains(neighbor))
                    continue;

                // 计算移动代价 - 如果是过门点之间的移动，代价为0
                float moveCost = 0;
                if (pathPointDict[current].IsDoor && pathPointDict[neighbor].IsDoor &&
                    doorPairs.ContainsKey(current) && doorPairs[current] == neighbor)
                {
                    moveCost = 0; 
                }
                else
                {
                    moveCost = Vector3.Distance(
                        pathPointDict[current].Position,
                        pathPointDict[neighbor].Position);
                }
                float tentativeGScore = gScore[current] + moveCost;

                if (!openSet.Contains(neighbor))
                    openSet.Add(neighbor);
                else if (tentativeGScore >= gScore[neighbor])
                    continue;

                cameFrom[neighbor] = current;
                gScore[neighbor] = tentativeGScore;
                fScore[neighbor] = gScore[neighbor] + HeuristicCost(neighbor, endPointID);
            }
        }

        // 没有找到路径
        return null;
    }


    public List<string> FindPathIDs(string startPointID, string endPointID)
    {
        if (!pathPointDict.ContainsKey(startPointID) || !pathPointDict.ContainsKey(endPointID))
            return null;

        var startPoint = GetVisiblePathPoint(startPointID);
        var endPoint = GetVisiblePathPoint(endPointID);
        if (startPoint == null || endPoint == null) return null;

        // A*算法实现（与FindPath类似，但返回ID列表而不是位置列表）
        Dictionary<string, string> cameFrom = new Dictionary<string, string>();
        Dictionary<string, float> gScore = new Dictionary<string, float>();
        Dictionary<string, float> fScore = new Dictionary<string, float>();

        HashSet<string> openSet = new HashSet<string>();
        HashSet<string> closedSet = new HashSet<string>();

        // 初始化起点
        gScore[startPointID] = 0;
        fScore[startPointID] = HeuristicCost(startPointID, endPointID);
        openSet.Add(startPointID);

        while (openSet.Count > 0)
        {
            // 找到fScore最小的节点
            string current = openSet.OrderBy(node => fScore.ContainsKey(node) ? fScore[node] : float.MaxValue).First();

            if (current == endPointID)
            {
                // 找到路径，重构路径
                return ReconstructPathIDs(cameFrom, endPointID);
            }

            openSet.Remove(current);
            closedSet.Add(current);

            foreach (string neighbor in pathPointDict[current].ConnectPoint)
            {
                if (closedSet.Contains(neighbor))
                    continue;

                // 计算移动代价
                float moveCost = 0;
                if (pathPointDict[current].IsDoor && pathPointDict[neighbor].IsDoor &&
                    doorPairs.ContainsKey(current) && doorPairs[current] == neighbor)
                {
                    moveCost = 0;
                }
                else
                {
                    moveCost = Vector3.Distance(
                        pathPointDict[current].Position,
                        pathPointDict[neighbor].Position);
                }
                float tentativeGScore = gScore[current] + moveCost;

                if (!openSet.Contains(neighbor))
                    openSet.Add(neighbor);
                else if (tentativeGScore >= gScore[neighbor])
                    continue;

                cameFrom[neighbor] = current;
                gScore[neighbor] = tentativeGScore;
                fScore[neighbor] = gScore[neighbor] + HeuristicCost(neighbor, endPointID);
            }
        }

        // 没有找到路径
        return null;
    }


    // 计算启发式成本（使用欧几里得距离）
    private float HeuristicCost(string fromPointID, string toPointID)
    {
        return Vector3.Distance(pathPointDict[fromPointID].Position, pathPointDict[toPointID].Position);
    }

    // 根据A*算法结果重构路径
    private List<Vector3> ReconstructPath(Dictionary<string, string> cameFrom, string endPointID)
    {
        List<Vector3> path = new List<Vector3>();
        string current = endPointID;

        // 反向追踪
        while (current != null)
        {
            path.Insert(0, pathPointDict[current].Position);
            current = cameFrom.ContainsKey(current) ? cameFrom[current] : null;
        }

        return path;
    }

    // 根据A*算法结果重构路径ID列表
    private List<string> ReconstructPathIDs(Dictionary<string, string> cameFrom, string endPointID)
    {
        List<string> path = new List<string>();
        string current = endPointID;

        // 反向追踪
        while (current != null)
        {
            path.Insert(0, current);
            current = cameFrom.ContainsKey(current) ? cameFrom[current] : null;
        }

        return path;
    }

    // 获取指定位置中的所有路径点
    public List<PathPoint> GetPointsInLocation(Location location)
    {
        List<PathPoint> points = new List<PathPoint>();
        foreach (var point in PathPoints)
        {
            if (point.location == location)
            {
                points.Add(point);
            }
        }
        return points;
    }

    public ControlPoint GetControlPoint(string startPointID, string endPointID)
    {
        var startPoint = GetVisiblePathPoint(startPointID);
        var endPoint = GetVisiblePathPoint(endPointID);
        if (startPoint == null || endPoint == null) return null;

        foreach (var controlPoint in ControlPoints)
        {
            if ((controlPoint.StartPathPointID == startPointID && controlPoint.EndPathPointID == endPointID) ||
                (controlPoint.StartPathPointID == endPointID && controlPoint.EndPathPointID == startPointID))
            {
                return controlPoint;
            }
        }
        return null;
    }


    // 计算二次贝塞尔曲线点
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

    // 计算三次贝塞尔曲线点
    private Vector3 CalculateBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        float uuu = uu * u;
        float ttt = tt * t;

        Vector3 p = uuu * p0;
        p += 3 * uu * t * p1;
        p += 3 * u * tt * p2;
        p += ttt * p3;

        return p;
    }

    void OnDrawGizmos()
    {
        // 在编辑模式下也初始化字典，以便Gizmos能正确绘制
        Dictionary<string, PathPoint> editModePathDict = new Dictionary<string, PathPoint>();
        foreach (var point in PathPoints)
        {
            editModePathDict[point.PathID] = point;
        }

        Dictionary<string, ControlPoint> editModeControlDict = new Dictionary<string, ControlPoint>();
        foreach (var controlPoint in ControlPoints)
        {
            editModeControlDict[controlPoint.ControlID] = controlPoint;
        }

        // 绘制路径点
        foreach (var point in PathPoints)
        {
            // 过门点用不同颜色标记
            if (point.IsDoor)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawSphere(point.Position, 0.3f);

                // 绘制过门点之间的连接
                if (!string.IsNullOrEmpty(point.PairedDoorID) && editModePathDict.ContainsKey(point.PairedDoorID))
                {
                    PathPoint pairedPoint = editModePathDict[point.PairedDoorID];
                    if (pairedPoint != null)
                    {
                        Gizmos.color = Color.cyan;
                        Gizmos.DrawLine(point.Position, pairedPoint.Position);
                    }
                }
            }
            else
            {
                Gizmos.color = Color.green;
                Gizmos.DrawSphere(point.Position, 0.2f);
            }

            // 绘制普通连接线
            Gizmos.color = Color.white;
            foreach (string connectedID in point.ConnectPoint)
            {
                if (editModePathDict.ContainsKey(connectedID))
                {
                    PathPoint connectedPoint = editModePathDict[connectedID];
                    if (connectedPoint != null && !point.IsDoor && !connectedPoint.IsDoor)
                    {
                        Gizmos.DrawLine(point.Position, connectedPoint.Position);
                    }
                }
            }
        }

        // 绘制控制点和贝塞尔曲线
        foreach (var controlPoint in ControlPoints)
        {
            // 检查控制点是否有有效的起始和结束路径点
            if (editModePathDict.ContainsKey(controlPoint.StartPathPointID) &&
                editModePathDict.ContainsKey(controlPoint.EndPathPointID))
            {
                PathPoint startPoint = editModePathDict[controlPoint.StartPathPointID];
                PathPoint endPoint = editModePathDict[controlPoint.EndPathPointID];

                // 绘制控制点（红色）
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(controlPoint.Position, 0.15f);

                // 绘制控制点到起始点和结束点的连线（黄色虚线）
                Gizmos.color = Color.yellow;
                Gizmos.DrawLine(controlPoint.Position, startPoint.Position);
                Gizmos.DrawLine(controlPoint.Position, endPoint.Position);

                // 绘制贝塞尔曲线（品红色）
                Gizmos.color = Color.magenta;
                Vector3 prevPoint = startPoint.Position;

                // 使用二次贝塞尔曲线
                for (float t = 0; t <= 1; t += 0.05f)
                {
                    Vector3 currentPoint = CalculateBezierPoint(
                        t,
                        startPoint.Position,
                        controlPoint.Position,
                        endPoint.Position);

                    Gizmos.DrawLine(prevPoint, currentPoint);
                    prevPoint = currentPoint;
                }

                // 绘制标签
#if UNITY_EDITOR
                UnityEditor.Handles.Label(controlPoint.Position, controlPoint.ControlID);
                UnityEditor.Handles.Label(startPoint.Position, "Start: " + controlPoint.StartPathPointID);
                UnityEditor.Handles.Label(endPoint.Position, "End: " + controlPoint.EndPathPointID);
#endif
            }
        }
    }
}