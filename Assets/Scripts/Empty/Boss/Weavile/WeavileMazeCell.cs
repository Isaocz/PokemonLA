using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 玛狃拉迷宫单元格
/// </summary>
public class MazeCell
{
    public bool Visited = false;

    public bool WallUp = true;      // 横向墙
    public bool WallDown = true;    // 横向墙
    public bool WallLeft = true;    // 左墙
    public bool WallRight = true;   // 右墙


}
