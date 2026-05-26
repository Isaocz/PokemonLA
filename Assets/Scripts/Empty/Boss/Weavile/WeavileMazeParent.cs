using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 玛狃拉迷宫组件
/// </summary>
public class WeavileMazeParent : WeavileSupportParent
{
    /// <summary>
    /// 迷宫制造器
    /// </summary>
    public WeavileMazeCreator MazeCreator;
    /// <summary>
    /// 父玛狃拉
    /// </summary>
    public Weavile ParentWeavile;


    /// <summary>
    /// 墙壁列表
    /// </summary>
    public List<WeavileMazeWall> WallList = new List<WeavileMazeWall> { };




    /// <summary>
    /// 动画预制件
    /// </summary>
    public Animator animator;

    /// 帮助时间
    /// </summary>
    float HelpingTime = 0.0f;

    /// <summary>
    /// 开始帮助
    /// </summary>
    public void HelpingStart(float time)
    {
        HelpingTime = time;

        if (animator != null) { animator.gameObject.transform.parent = this.transform.parent; }
        if (MazeCreator != null && ParentWeavile != null)
        {
            int gridW = Mathf.Abs((int)((ParentWeavile.ParentPokemonRoom.RoomSize[3] - ParentWeavile.ParentPokemonRoom.RoomSize[2]) / MazeCreator.CellSize));
            int gridH = Mathf.Abs((int)((ParentWeavile.ParentPokemonRoom.RoomSize[0] - ParentWeavile.ParentPokemonRoom.RoomSize[1]) / MazeCreator.CellSize));

            WeavileMazeGenerator maze = new WeavileMazeGenerator(gridW, gridH);

            //生成虚拟迷宫
            Vector3 StartP = new Vector3(-((float)maze.Width * MazeCreator.CellSize / 2.0f) + (MazeCreator.CellSize / 2.0f), (-(float)maze.Height * MazeCreator.CellSize / 2.0f) + (MazeCreator.CellSize / 2.0f) + 0.07f, 0);
            int startX = (int)((ParentWeavile.TARGET_POSITION.x - StartP.x - ParentWeavile.ParentPokemonRoom.transform.position.x) / 2.0f);
            int startY = (int)((ParentWeavile.TARGET_POSITION.y - StartP.y - ParentWeavile.ParentPokemonRoom.transform.position.y) / 2.0f);
            Debug.Log(new Vector2Int(startX, startY));
            maze.Generate(startX, startY);

            //把终点保护起来
            int endX = (int)((ParentWeavile.transform.position.x - StartP.x - ParentWeavile.ParentPokemonRoom.transform.position.x) / 2.0f);
            int endY = (int)((ParentWeavile.transform.position.y - StartP.y - ParentWeavile.ParentPokemonRoom.transform.position.y) / 2.0f);
            maze.ProtectEndCell(maze , endX, endY);

            //提取墙壁数据
            List<WeavileMazeGenerator.MazeWall> walls = maze.ExtractWalls(maze, MazeCreator.CellSize);

            //生成真实墙壁
            MazeCreator.StartRender(walls , new Vector2Int(startX , startY) , maze, ParentWeavile.ParentPokemonRoom.transform , this);
        }
        Destroy(gameObject, HelpingTime);
    }

    /// <summary>
    /// 结束帮助
    /// </summary>
    public override void SupportOver()
    {
        if (animator != null)
        {
            Debug.Log("Over");
            animator.SetTrigger("Over");
        }
        foreach (WeavileMazeWall wall in WallList)
        {
            if (!wall.isBreak)
            {
                wall.BeHit(wall.MaxHP);
            }
        }
    }


    private void OnDestroy()
    {
        SupportOver();
    }


}
