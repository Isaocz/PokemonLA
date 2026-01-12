using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;

[CustomEditor(typeof(PathManager))]
public class PathManagerEditor : Editor
{
    private PathManager pathManager;
    private bool showPathPoints = true;
    private bool showControlPoints = true;
    private Vector2 pathPointsScrollPosition;
    private Vector2 controlPointsScrollPosition;
    private string newPointID = "";
    private PathManager.Location newPointLocation = PathManager.Location.inTown;
    private bool newPointIsDoor = false;
    private string newPointPairedDoorID = "";

    // 控制点相关字段
    private string newControlID = "";
    private string newControlStartPointID = "";
    private string newControlEndPointID = "";
    private bool newControlUseAsHandle = true;

    // 排序选项
    private enum SortOrder { None, Ascending, Descending }
    private SortOrder pathPointsSortOrder = SortOrder.None;
    private SortOrder controlPointsSortOrder = SortOrder.None;

    void OnEnable()
    {
        pathManager = (PathManager)target;
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        // 路径点部分
        EditorGUILayout.LabelField("路径点管理", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        // 添加新路径点
        EditorGUILayout.LabelField("添加新路径点", EditorStyles.boldLabel);
        newPointID = EditorGUILayout.TextField("路径点ID", newPointID);
        newPointLocation = (PathManager.Location)EditorGUILayout.EnumPopup("所在位置", newPointLocation);
        newPointIsDoor = EditorGUILayout.Toggle("是否为门", newPointIsDoor);

        if (newPointIsDoor)
        {
            newPointPairedDoorID = EditorGUILayout.TextField("配对门ID", newPointPairedDoorID);
        }

        if (GUILayout.Button("添加路径点") && !string.IsNullOrEmpty(newPointID))
        {
            AddNewPathPoint();
        }

        EditorGUILayout.Space();

        // 显示现有路径点
        EditorGUILayout.BeginHorizontal();
        showPathPoints = EditorGUILayout.Foldout(showPathPoints, "路径点列表 (" + pathManager.PathPoints.Count + ")");

        // 添加排序按钮
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("排序", GUILayout.Width(50)))
        {
            if (pathPointsSortOrder == SortOrder.None || pathPointsSortOrder == SortOrder.Descending)
            {
                SortPathPoints(true);
                pathPointsSortOrder = SortOrder.Ascending;
            }
            else
            {
                SortPathPoints(false);
                pathPointsSortOrder = SortOrder.Descending;
            }
        }
        EditorGUILayout.EndHorizontal();

        if (showPathPoints)
        {
            // 使用滚动视图显示列表
            pathPointsScrollPosition = EditorGUILayout.BeginScrollView(pathPointsScrollPosition, GUILayout.Height(200));

            for (int i = 0; i < pathManager.PathPoints.Count; i++)
            {
                DrawPathPointItem(pathManager.PathPoints[i], i);
            }

            EditorGUILayout.EndScrollView();
        }

        EditorGUILayout.Space();

        // 控制点部分
        EditorGUILayout.LabelField("控制点管理", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        // 添加新控制点
        EditorGUILayout.LabelField("添加新控制点", EditorStyles.boldLabel);
        newControlID = EditorGUILayout.TextField("控制点ID", newControlID);
        newControlStartPointID = EditorGUILayout.TextField("起始路径点ID", newControlStartPointID);
        newControlEndPointID = EditorGUILayout.TextField("结束路径点ID", newControlEndPointID);
        newControlUseAsHandle = EditorGUILayout.Toggle("用作控制柄", newControlUseAsHandle);

        if (GUILayout.Button("添加控制点") && !string.IsNullOrEmpty(newControlID))
        {
            AddNewControlPoint();
        }

        EditorGUILayout.Space();

        // 显示现有控制点
        EditorGUILayout.BeginHorizontal();
        showControlPoints = EditorGUILayout.Foldout(showControlPoints, "控制点列表 (" + pathManager.ControlPoints.Count + ")");

        // 添加排序按钮
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("排序", GUILayout.Width(50)))
        {
            if (controlPointsSortOrder == SortOrder.None || controlPointsSortOrder == SortOrder.Descending)
            {
                SortControlPoints(true);
                controlPointsSortOrder = SortOrder.Ascending;
            }
            else
            {
                SortControlPoints(false);
                controlPointsSortOrder = SortOrder.Descending;
            }
        }
        EditorGUILayout.EndHorizontal();

        if (showControlPoints)
        {
            // 使用滚动视图显示列表
            controlPointsScrollPosition = EditorGUILayout.BeginScrollView(controlPointsScrollPosition, GUILayout.Height(200));

            for (int i = 0; i < pathManager.ControlPoints.Count; i++)
            {
                DrawControlPointItem(pathManager.ControlPoints[i], i);
            }

            EditorGUILayout.EndScrollView();
        }

        // 工具部分
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("工具", EditorStyles.boldLabel);

        if (GUILayout.Button("从选择对象创建路径点"))
        {
            CreateFromSelection();
        }

        if (GUILayout.Button("清理空连接"))
        {
            CleanEmptyConnections();
        }

        if (GUILayout.Button("验证路径点"))
        {
            ValidatePathPoints();
        }

        serializedObject.ApplyModifiedProperties();
    }

    // 排序路径点的方法
    private void SortPathPoints(bool ascending)
    {
        if (ascending)
        {
            pathManager.PathPoints = pathManager.PathPoints
                .OrderBy(p => p.PathID, new NaturalSortComparer())
                .ToList();
        }
        else
        {
            pathManager.PathPoints = pathManager.PathPoints
                .OrderByDescending(p => p.PathID, new NaturalSortComparer())
                .ToList();
        }
        EditorUtility.SetDirty(pathManager);
    }

    // 排序控制点的方法
    private void SortControlPoints(bool ascending)
    {
        if (ascending)
        {
            pathManager.ControlPoints = pathManager.ControlPoints
                .OrderBy(c => c.ControlID, new NaturalSortComparer())
                .ToList();
        }
        else
        {
            pathManager.ControlPoints = pathManager.ControlPoints
                .OrderByDescending(c => c.ControlID, new NaturalSortComparer())
                .ToList();
        }
        EditorUtility.SetDirty(pathManager);
    }

    // 自然排序比较器（处理数字和字母混合的情况）
    private class NaturalSortComparer : IComparer<string>
    {
        public int Compare(string x, string y)
        {
            if (x == null && y == null) return 0;
            if (x == null) return -1;
            if (y == null) return 1;

            int i = 0, j = 0;
            while (i < x.Length && j < y.Length)
            {
                if (char.IsDigit(x[i]) && char.IsDigit(y[j]))
                {
                    // 比较数字部分
                    string num1 = "", num2 = "";
                    while (i < x.Length && char.IsDigit(x[i])) num1 += x[i++];
                    while (j < y.Length && char.IsDigit(y[j])) num2 += y[j++];

                    int compare = int.Parse(num1).CompareTo(int.Parse(num2));
                    if (compare != 0) return compare;
                }
                else
                {
                    // 比较字母部分
                    int compare = x[i].ToString().ToLower().CompareTo(y[j].ToString().ToLower());
                    if (compare != 0) return compare;
                    i++;
                    j++;
                }
            }

            return x.Length.CompareTo(y.Length);
        }
    }

    void OnSceneGUI()
    {
        // 绘制路径点
        foreach (var point in pathManager.PathPoints)
        {
            Handles.color = point.IsDoor ? Color.blue : Color.green;
            float handleSize = point.IsDoor ? 0.5f : 0.3f;

            // 绘制路径点
            if (Handles.Button(point.Position, Quaternion.identity, handleSize, handleSize, Handles.SphereHandleCap))
            {
                Selection.activeGameObject = pathManager.gameObject;
                GUIUtility.keyboardControl = 0;
            }

            // 绘制连接线
            Handles.color = Color.white;
            foreach (string connectedID in point.ConnectPoint)
            {
                var connectedPoint = pathManager.PathPoints.Find(p => p.PathID == connectedID);
                if (connectedPoint != null)
                {
                    Handles.DrawLine(point.Position, connectedPoint.Position);

                    // 绘制方向箭头
                    Vector3 direction = (connectedPoint.Position - point.Position).normalized;
                    float distance = Vector3.Distance(point.Position, connectedPoint.Position);
                    Vector3 midPoint = point.Position + direction * (distance * 0.5f);
                    Handles.ArrowHandleCap(0, midPoint, Quaternion.LookRotation(direction), 1f, EventType.Repaint);
                }
            }

            // 绘制门的虚线连接
            if (point.IsDoor && !string.IsNullOrEmpty(point.PairedDoorID))
            {
                var pairedPoint = pathManager.PathPoints.Find(p => p.PathID == point.PairedDoorID);
                if (pairedPoint != null)
                {
                    Handles.color = Color.cyan;
                    Handles.DrawDottedLine(point.Position, pairedPoint.Position, 5f);
                }
            }

            // 显示路径点ID
            Handles.Label(point.Position + Vector3.up * 0.5f, point.PathID);
        }

        // 绘制控制点和贝塞尔曲线
        foreach (var controlPoint in pathManager.ControlPoints)
        {
            // 检查控制点是否有有效的起始和结束路径点
            var startPoint = pathManager.PathPoints.Find(p => p.PathID == controlPoint.StartPathPointID);
            var endPoint = pathManager.PathPoints.Find(p => p.PathID == controlPoint.EndPathPointID);

            if (startPoint != null && endPoint != null)
            {
                // 绘制控制点（红色）
                Handles.color = Color.red;
                if (Handles.Button(controlPoint.Position, Quaternion.identity, 0.3f, 0.3f, Handles.SphereHandleCap))
                {
                    Selection.activeGameObject = pathManager.gameObject;
                    GUIUtility.keyboardControl = 0;
                }

                // 绘制控制点到起始点和结束点的连线（黄色）
                Handles.color = Color.yellow;
                Handles.DrawLine(controlPoint.Position, startPoint.Position);
                Handles.DrawLine(controlPoint.Position, endPoint.Position);

                // 绘制贝塞尔曲线（品红色）
                Handles.color = Color.magenta;
                Vector3 prevPoint = startPoint.Position;

                // 使用二次贝塞尔曲线
                for (float t = 0; t <= 1; t += 0.05f)
                {
                    Vector3 currentPoint = CalculateQuadraticBezierPoint(
                        t,
                        startPoint.Position,
                        controlPoint.Position,
                        endPoint.Position);

                    Handles.DrawLine(prevPoint, currentPoint);
                    prevPoint = currentPoint;
                }

                // 显示控制点ID
                Handles.Label(controlPoint.Position + Vector3.up * 0.5f, controlPoint.ControlID);
            }
        }

        // 添加路径点的快捷键
        if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.P)
        {
            Ray worldRay = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
            if (Physics.Raycast(worldRay, out RaycastHit hit))
            {
                if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.P)
                {
                    AddPathPointAtPosition(hit.point);
                    Event.current.Use();
                }
            }
        }
    }

    private void DrawPathPointItem(PathManager.PathPoint point, int index)
    {
        EditorGUILayout.BeginVertical("box");

        EditorGUILayout.BeginHorizontal();
        point.PathID = EditorGUILayout.TextField("ID", point.PathID);
        if (GUILayout.Button("聚焦", GUILayout.Width(50)))
        {
            SceneView.lastActiveSceneView.pivot = point.Position;
            SceneView.lastActiveSceneView.Repaint();
        }
        if (GUILayout.Button("删除", GUILayout.Width(50)))
        {
            RemovePathPoint(point);
            return;
        }
        EditorGUILayout.EndHorizontal();

        point.Position = EditorGUILayout.Vector3Field("位置", point.Position);
        point.location = (PathManager.Location)EditorGUILayout.EnumPopup("所在位置", point.location);
        point.IsDoor = EditorGUILayout.Toggle("是否为门", point.IsDoor);

        if (point.IsDoor)
        {
            point.PairedDoorID = EditorGUILayout.TextField("配对门ID", point.PairedDoorID);
        }

        // 显示连接点
        EditorGUILayout.LabelField("连接点");
        for (int i = 0; i < point.ConnectPoint.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();
            point.ConnectPoint[i] = EditorGUILayout.TextField(point.ConnectPoint[i]);
            if (GUILayout.Button("删除", GUILayout.Width(50)))
            {
                point.ConnectPoint.RemoveAt(i);
                break;
            }
            EditorGUILayout.EndHorizontal();
        }

        // 添加新连接
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("添加连接"))
        {
            point.ConnectPoint.Add("");
        }

        // 快速连接按钮
        if (GUILayout.Button("快速连接", GUILayout.Width(80)))
        {
            ShowQuickConnectMenu(point);
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.EndVertical();
    }

    private void DrawControlPointItem(PathManager.ControlPoint controlPoint, int index)
    {
        EditorGUILayout.BeginVertical("box");

        EditorGUILayout.BeginHorizontal();
        controlPoint.ControlID = EditorGUILayout.TextField("ID", controlPoint.ControlID);
        if (GUILayout.Button("聚焦", GUILayout.Width(50)))
        {
            SceneView.lastActiveSceneView.pivot = controlPoint.Position;
            SceneView.lastActiveSceneView.Repaint();
        }
        if (GUILayout.Button("删除", GUILayout.Width(50)))
        {
            RemoveControlPoint(controlPoint);
            return;
        }
        EditorGUILayout.EndHorizontal();

        controlPoint.Position = EditorGUILayout.Vector3Field("位置", controlPoint.Position);
        controlPoint.StartPathPointID = EditorGUILayout.TextField("起始路径点ID", controlPoint.StartPathPointID);
        controlPoint.EndPathPointID = EditorGUILayout.TextField("结束路径点ID", controlPoint.EndPathPointID);
        controlPoint.UseAsHandle = EditorGUILayout.Toggle("作为控制柄", controlPoint.UseAsHandle);

        EditorGUILayout.EndVertical();
    }

    private void AddNewPathPoint()
    {
        PathManager.PathPoint newPoint = new PathManager.PathPoint
        {
            PathID = newPointID,
            Position = pathManager.transform.position,
            location = newPointLocation,
            IsDoor = newPointIsDoor,
            PairedDoorID = newPointIsDoor ? newPointPairedDoorID : ""
        };

        pathManager.PathPoints.Add(newPoint);
        newPointID = "";
        newPointPairedDoorID = "";
        EditorUtility.SetDirty(pathManager);
    }

    private void AddNewControlPoint()
    {
        PathManager.ControlPoint newControlPoint = new PathManager.ControlPoint
        {
            ControlID = newControlID,
            Position = pathManager.transform.position,
            StartPathPointID = newControlStartPointID,
            EndPathPointID = newControlEndPointID,
            UseAsHandle = newControlUseAsHandle
        };

        pathManager.ControlPoints.Add(newControlPoint);
        newControlID = "";
        newControlStartPointID = "";
        newControlEndPointID = "";
        EditorUtility.SetDirty(pathManager);
    }

    private void AddPathPointAtPosition(Vector3 position)
    {
        string newID = "Point_" + (pathManager.PathPoints.Count + 1);

        PathManager.PathPoint newPoint = new PathManager.PathPoint
        {
            PathID = newID,
            Position = position,
            location = newPointLocation,
            IsDoor = false
        };

        pathManager.PathPoints.Add(newPoint);
        EditorUtility.SetDirty(pathManager);
        Repaint();
    }

    private void RemovePathPoint(PathManager.PathPoint point)
    {
        // 删除所有对该点的引用
        foreach (var p in pathManager.PathPoints)
        {
            p.ConnectPoint.Remove(point.PathID);

            if (p.PairedDoorID == point.PathID)
            {
                p.PairedDoorID = "";
            }
        }

        // 删除所有使用该路径点的控制点
        pathManager.ControlPoints.RemoveAll(cp =>
            cp.StartPathPointID == point.PathID || cp.EndPathPointID == point.PathID);

        pathManager.PathPoints.Remove(point);
        EditorUtility.SetDirty(pathManager);
    }

    private void RemoveControlPoint(PathManager.ControlPoint controlPoint)
    {
        pathManager.ControlPoints.Remove(controlPoint);
        EditorUtility.SetDirty(pathManager);
    }

    private void CreateFromSelection()
    {
        GameObject[] selectedObjects = Selection.gameObjects;

        foreach (GameObject obj in selectedObjects)
        {
            string newID = obj.name;

            // 检查是否已存在相同ID的点
            if (pathManager.PathPoints.Any(p => p.PathID == newID))
            {
                Debug.LogWarning($"已存在ID为 {newID} 的路径点，跳过");
                continue;
            }

            PathManager.PathPoint newPoint = new PathManager.PathPoint
            {
                PathID = newID,
                Position = obj.transform.position,
                location = PathManager.Location.inTown,
                IsDoor = false
            };

            pathManager.PathPoints.Add(newPoint);
        }

        EditorUtility.SetDirty(pathManager);
    }

    private void CleanEmptyConnections()
    {
        foreach (var point in pathManager.PathPoints)
        {
            point.ConnectPoint.RemoveAll(string.IsNullOrEmpty);
        }

        EditorUtility.SetDirty(pathManager);
        Debug.Log("已清理空连接");
    }

    private void ValidatePathPoints()
    {
        // 检查重复ID
        var duplicateIDs = pathManager.PathPoints
            .GroupBy(p => p.PathID)
            .Where(g => g.Count() > 1)
            .Select(g => g.Key);

        if (duplicateIDs.Any())
        {
            Debug.LogError($"发现重复的路径点ID: {string.Join(", ", duplicateIDs)}");
        }

        // 检查控制点重复ID
        var duplicateControlIDs = pathManager.ControlPoints
            .GroupBy(cp => cp.ControlID)
            .Where(g => g.Count() > 1)
            .Select(g => g.Key);

        if (duplicateControlIDs.Any())
        {
            Debug.LogError($"发现重复的控制点ID: {string.Join(", ", duplicateControlIDs)}");
        }

        // 检查无效连接
        foreach (var point in pathManager.PathPoints)
        {
            foreach (string connectedID in point.ConnectPoint)
            {
                if (!pathManager.PathPoints.Any(p => p.PathID == connectedID))
                {
                    Debug.LogWarning($"路径点 {point.PathID} 连接到不存在的点: {connectedID}");
                }
            }

            // 检查门配对
            if (point.IsDoor && !string.IsNullOrEmpty(point.PairedDoorID))
            {
                var pairedPoint = pathManager.PathPoints.Find(p => p.PathID == point.PairedDoorID);
                if (pairedPoint == null)
                {
                    Debug.LogWarning($"路径点 {point.PathID} 配对的门不存在: {point.PairedDoorID}");
                }
                else if (!pairedPoint.IsDoor)
                {
                    Debug.LogWarning($"路径点 {point.PathID} 配对的对象不是门: {point.PairedDoorID}");
                }
            }
        }

        // 检查控制点的有效性
        foreach (var controlPoint in pathManager.ControlPoints)
        {
            var startPoint = pathManager.PathPoints.Find(p => p.PathID == controlPoint.StartPathPointID);
            var endPoint = pathManager.PathPoints.Find(p => p.PathID == controlPoint.EndPathPointID);

            if (startPoint == null)
            {
                Debug.LogWarning($"控制点 {controlPoint.ControlID} 的起始路径点不存在: {controlPoint.StartPathPointID}");
            }

            if (endPoint == null)
            {
                Debug.LogWarning($"控制点 {controlPoint.ControlID} 的结束路径点不存在: {controlPoint.EndPathPointID}");
            }
        }

        Debug.Log("路径点验证完成");
    }

    private void ShowQuickConnectMenu(PathManager.PathPoint point)
    {
        GenericMenu menu = new GenericMenu();

        foreach (var otherPoint in pathManager.PathPoints)
        {
            if (otherPoint != point && !point.ConnectPoint.Contains(otherPoint.PathID))
            {
                menu.AddItem(new GUIContent(otherPoint.PathID), false, () =>
                {
                    point.ConnectPoint.Add(otherPoint.PathID);
                    EditorUtility.SetDirty(pathManager);
                });
            }
        }

        menu.ShowAsContext();
    }

    // 计算二次贝塞尔曲线点
    private Vector3 CalculateQuadraticBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;

        Vector3 p = uu * p0;
        p += 2 * u * t * p1;
        p += tt * p2;

        return p;
    }
}