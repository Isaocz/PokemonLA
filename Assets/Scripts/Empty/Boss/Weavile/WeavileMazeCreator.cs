using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 玛狃拉迷宫制造器
/// </summary>
public class WeavileMazeCreator : MonoBehaviour
{
    static float InstantiateInterval = 0.003f;


    //水平墙壁
    public GameObject WallHorizontalPrefab;
    //左侧墙壁
    public GameObject WallLeftPrefab;
    //右侧墙壁
    public GameObject WallRightPrefab;
    //单元格长宽
    public float CellSize = 2f;






    /// <summary>
    /// 【已弃用】瞬间一起生成迷宫
    /// </summary>
    /// <param name="maze"></param>
    /// <param name="Room"></param>
    public void Render(WeavileMazeGenerator maze , Transform Room)
    {
        for (int x = 0; x < maze.Width; x++)
        {
            for (int y = 0; y < maze.Height; y++)
            {
                Vector3 cellPos = new Vector3(x * CellSize, y * CellSize, 0);

                MazeCell cell = maze.Cells[x, y];

                Vector3 StartP = new Vector3(-((float)maze.Width * CellSize / 2.0f) + (CellSize / 2.0f), (-(float)maze.Height * CellSize / 2.0f) + (CellSize / 2.0f) + 0.07f, 0);

                if (cell.WallUp)
                    Instantiate(WallHorizontalPrefab, cellPos + new Vector3(0, CellSize / 2, 0) + Room.transform.position + StartP, Quaternion.identity);

                if (cell.WallDown)
                    Instantiate(WallHorizontalPrefab, cellPos + new Vector3(0, -CellSize / 2, 0) + Room.transform.position + StartP, Quaternion.identity);

                if (cell.WallLeft)
                    Instantiate(WallLeftPrefab, cellPos + new Vector3(-CellSize / 2, 0, 0) + Room.transform.position + StartP, Quaternion.identity);

                if (cell.WallRight)
                    Instantiate(WallRightPrefab, cellPos + new Vector3(CellSize / 2, 0, 0) + Room.transform.position + StartP, Quaternion.identity);
            }
        }
    }


    /// <summary>
    /// 延迟生成墙体
    /// </summary>
    public void StartRender( List<WeavileMazeGenerator.MazeWall> walls , Vector2Int start, WeavileMazeGenerator maze, Transform Room , WeavileMazeParent mazeSupporParent)
    {
        StartCoroutine(RenderWallsWithDelay(
                walls,
                WallHorizontalPrefab,
                WallLeftPrefab,
                WallRightPrefab,
                InstantiateInterval,
                start,
                maze,
                Room,
                CellSize,
                mazeSupporParent
            ));
    }


    /// <summary>
    /// 延迟生成墙体
    /// </summary>
    /// <param name="walls"></param>
    /// <param name="wallPrefab"></param>
    /// <param name="delay"></param>
    /// <param name="startCell"></param>
    /// <param name="cellSize"></param>
    /// <returns></returns>
    public IEnumerator RenderWallsWithDelay(
    List<WeavileMazeGenerator.MazeWall> walls,
    GameObject horizontalPrefab,
    GameObject leftPrefab,
    GameObject rightPrefab,
    float delay,
    Vector2Int startCell,
    WeavileMazeGenerator maze ,
    Transform Room,
    float cellSize,
    WeavileMazeParent mazeSupporParent)
    {
        // 计算 BFS 深度
        Dictionary<WeavileMazeGenerator.MazeWall, int> depth = new Dictionary<WeavileMazeGenerator.MazeWall, int>();

        foreach (var w in walls)
        {
            int d = Mathf.Abs((int)(w.WorldPos.x / cellSize) - startCell.x)
                  + Mathf.Abs((int)(w.WorldPos.y / cellSize) - startCell.y);

            depth[w] = d;
        }

        // 按深度排序
        walls.Sort((a, b) => depth[a].CompareTo(depth[b]));

        // 逐个生成
        foreach (var w in walls)
        {
            GameObject prefab =
                w.Type == WeavileMazeGenerator.WallType.Horizontal ? horizontalPrefab :
                w.Type == WeavileMazeGenerator.WallType.Left ? leftPrefab :
                                                rightPrefab;

            Vector3 StartP = new Vector3(-((float)maze.Width * CellSize / 2.0f) + (CellSize / 2.0f), (-(float)maze.Height * CellSize / 2.0f) + (CellSize / 2.0f) + 0.07f, 0);

            
            GameObject wallobj = Instantiate(prefab, w.WorldPos + Room.transform.position + StartP, Quaternion.identity);

            //为墙体添加父支援 为父支援墙体列表添加墙体
            WeavileMazeWall wall = wallobj.GetComponent<WeavileMazeWall>();
            if (wall != null)
            {
                mazeSupporParent.WallList.Add(wall);
                wall.MazeSupportParent = mazeSupporParent;
            }
            

            yield return new WaitForSeconds(delay);
        }
    }


}