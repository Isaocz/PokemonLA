using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 玛狃拉迷宫生成器
/// </summary>
public class WeavileMazeGenerator
{


    public int Width;   // 单元格宽度（例如 12）
    public int Height;  // 单元格高度（例如 7）

    public MazeCell[,] Cells;

    private System.Random rand = new System.Random();



    //墙体
    public struct MazeWall
    {
        public Vector3 WorldPos;
        public WallType Type; // Horizontal / Left / Right
    }
    //墙体种类
    public enum WallType
    {
        Horizontal,
        Left,
        Right
    }



    //初始化生成器
    public WeavileMazeGenerator(int width, int height)
    {
        Width = width;
        Height = height;

        Cells = new MazeCell[Width, Height];
        for (int x = 0; x < Width; x++)
            for (int y = 0; y < Height; y++)
                Cells[x, y] = new MazeCell();
    }

    //输入起始点 开始深度搜索
    // -------------------------
    // 入口：混合迷宫生成
    // -------------------------
    public void Generate(int startX, int startY)
    {
        if (!IsInside(startX, startY)) {
            Debug.Log("Error:玛狃拉迷宫生成错误 起始点不在房间内");
            startX = 0;
            startY = 0;
        }
        // 1. DFS 生成主路径
        DFS(startX, startY);

        // 2. Prim 扩展支路
        PrimExpansion();
    }

    //深度搜索
    private void DFS(int x, int y)
    {

        Cells[x, y].Visited = true;

        //搜索上下左右
        List<Vector2Int> dirs = new List<Vector2Int>
        {
            new Vector2Int(1, 0),   // 右
            new Vector2Int(-1, 0),  // 左
            new Vector2Int(0, 1),   // 上
            new Vector2Int(0, -1),  // 下
        };

        //随机排序搜索方向
        Shuffle(dirs);

        //尝试探索重排序后所有方向
        foreach (var d in dirs)
        {
            int nx = x + d.x;
            int ny = y + d.y;

            //尝试探索的单元格在房间内且未被探索 移除墙壁并继续探索
            if (IsInside(nx, ny) && !Cells[nx, ny].Visited)
            {
                RemoveWallBetween(x, y, nx, ny);
                DFS(nx, ny);
            }
        }
    }

    // -------------------------
    // Prim 扩展支路（增加复杂度）
    // -------------------------
    private void PrimExpansion()
    {
        List<Vector2Int> frontier = new List<Vector2Int>();

        // 初始化 frontier：所有已访问单元格的邻居
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                if (Cells[x, y].Visited)
                {
                    AddFrontier(x, y, frontier);
                }
            }
        }

        while (frontier.Count > 0)
        {
            // 随机挑一个 frontier
            int idx = rand.Next(frontier.Count);
            Vector2Int f = frontier[idx];
            frontier.RemoveAt(idx);

            if (Cells[f.x, f.y].Visited)
                continue;

            // 找到一个已访问的邻居
            List<Vector2Int> visitedNeighbors = GetVisitedNeighbors(f.x, f.y);

            if (visitedNeighbors.Count > 0)
            {
                Vector2Int chosen = visitedNeighbors[rand.Next(visitedNeighbors.Count)];

                RemoveWallBetween(f.x, f.y, chosen.x, chosen.y);
                Cells[f.x, f.y].Visited = true;

                AddFrontier(f.x, f.y, frontier);
            }
        }
    }

    private void AddFrontier(int x, int y, List<Vector2Int> frontier)
    {
        List<Vector2Int> dirs = new List<Vector2Int>
        {
            new Vector2Int(1, 0),
            new Vector2Int(-1, 0),
            new Vector2Int(0, 1),
            new Vector2Int(0, -1),
        };

        foreach (var d in dirs)
        {
            int nx = x + d.x;
            int ny = y + d.y;

            if (IsInside(nx, ny) && !Cells[nx, ny].Visited)
            {
                Vector2Int pos = new Vector2Int(nx, ny);
                if (!frontier.Contains(pos))
                    frontier.Add(pos);
            }
        }
    }

    private List<Vector2Int> GetVisitedNeighbors(int x, int y)
    {
        List<Vector2Int> list = new List<Vector2Int>();

        List<Vector2Int> dirs = new List<Vector2Int>
        {
            new Vector2Int(1, 0),
            new Vector2Int(-1, 0),
            new Vector2Int(0, 1),
            new Vector2Int(0, -1),
        };

        foreach (var d in dirs)
        {
            int nx = x + d.x;
            int ny = y + d.y;

            if (IsInside(nx, ny) && Cells[nx, ny].Visited)
                list.Add(new Vector2Int(nx, ny));
        }

        return list;
    }




    //单元格是否在房间内
    private bool IsInside(int x, int y)
    {
        return x >= 0 && x < Width && y >= 0 && y < Height;
    }

    //移除两个单元格之间的墙
    private void RemoveWallBetween(int x, int y, int nx, int ny)
    {
        if (nx == x + 1) // 右
        {
            Cells[x, y].WallRight = false;
            Cells[nx, ny].WallLeft = false;
        }
        else if (nx == x - 1) // 左
        {
            Cells[x, y].WallLeft = false;
            Cells[nx, ny].WallRight = false;
        }
        else if (ny == y + 1) // 上
        {
            Cells[x, y].WallUp = false;
            Cells[nx, ny].WallDown = false;
        }
        else if (ny == y - 1) // 下
        {
            Cells[x, y].WallDown = false;
            Cells[nx, ny].WallUp = false;
        }
    }

    //随机排序搜索方向
    private void Shuffle(List<Vector2Int> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int r = rand.Next(i, list.Count);
            (list[i], list[r]) = (list[r], list[i]);
        }
    }




    /// <summary>
    /// 从迷宫数据提取墙体数据
    /// </summary>
    /// <param name="maze"></param>
    /// <param name="cellSize"></param>
    /// <returns></returns>
    public List<MazeWall> ExtractWalls(WeavileMazeGenerator maze, float cellSize)
    {
        List<MazeWall> walls = new List<MazeWall>();

        int half = maze.Width / 2;

        for (int x = 0; x < maze.Width; x++)
        {
            for (int y = 0; y < maze.Height; y++)
            {
                MazeCell c = maze.Cells[x, y];
                Vector3 basePos = new Vector3(x * cellSize, y * cellSize, 0);

                bool isLeftSide = x < half;

                // -------------------------
                // 上墙（横向）
                // -------------------------
                if (c.WallUp)
                {
                    walls.Add(new MazeWall
                    {
                        WorldPos = basePos + new Vector3(0, cellSize / 2, 0),
                        Type = WallType.Horizontal
                    });
                }

                // -------------------------
                // 右墙（纵向，但按 x 决定左/右墙）
                // -------------------------
                if (c.WallRight)
                {
                    walls.Add(new MazeWall
                    {
                        WorldPos = basePos + new Vector3(cellSize / 2, 0, 0),
                        Type = isLeftSide ? WallType.Left : WallType.Right
                    });
                }

                // -------------------------
                // 下墙（只在 y==0）
                // -------------------------
                if (y == 0 && c.WallDown)
                {
                    walls.Add(new MazeWall
                    {
                        WorldPos = basePos + new Vector3(0, -cellSize / 2, 0),
                        Type = WallType.Horizontal
                    });
                }

                // -------------------------
                // 左墙（只在 x==0）
                // -------------------------
                if (x == 0 && c.WallLeft)
                {
                    walls.Add(new MazeWall
                    {
                        WorldPos = basePos + new Vector3(-cellSize / 2, 0, 0),
                        Type = isLeftSide ? WallType.Left : WallType.Right
                    });
                }
            }
        }

        return walls;
    }




    /// <summary>
    /// 保护终点单元格
    /// </summary>
    /// <param name="maze"></param>
    /// <param name="endX"></param>
    /// <param name="endY"></param>
    public void ProtectEndCell(WeavileMazeGenerator maze, int endX, int endY)
    {

        if (!IsInside(endX, endY))
        {
            Debug.Log("Error:玛狃拉迷宫生成错误 终点点不在房间内");
        }


        MazeCell end = maze.Cells[endX, endY];

        // 恢复终点自身的墙
        end.WallUp = true;
        end.WallDown = true;
        end.WallLeft = true;
        end.WallRight = true;

        // 同步恢复邻居的墙
        // 上
        if (endY + 1 < maze.Height)
            maze.Cells[endX, endY + 1].WallDown = true;

        // 下
        if (endY - 1 >= 0)
            maze.Cells[endX, endY - 1].WallUp = true;

        // 左
        if (endX - 1 >= 0)
            maze.Cells[endX - 1, endY].WallRight = true;

        // 右
        if (endX + 1 < maze.Width)
            maze.Cells[endX + 1, endY].WallLeft = true;
    }

}




