using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _PathFinder : MonoBehaviour
{
    public static _PathFinder StaticPathFinder;
    AstarPath astarPath;

    // Start is called before the first frame update
    private void Awake()
    {
        StaticPathFinder = this;
        astarPath = GetComponent<AstarPath>();
        
    }

    public GridGraph CreatNewGrid(Vector3 RoomVector)
    {
        GridGraph NowRoomGraph =  astarPath.data.AddGraph(typeof(GridGraph)) as GridGraph;
        NowRoomGraph.is2D = true;
        NowRoomGraph.collision.use2D = true;
        NowRoomGraph.collision.mask = LayerMask.GetMask("Water", "Enviroment", "Room","SpikeCollidor");
        NowRoomGraph.collision.diameter = 0.9f;
        NowRoomGraph.center = RoomVector;
        NowRoomGraph.SetDimensions(30, 24, 1f);
        NowRoomGraph.Scan();
        return NowRoomGraph;
    }

    public void UpdateGraph(GridGraph g , Vector3 RoomVector)
    {
        /*
        g.is2D = true;
        g.collision.use2D = true;
        g.collision.mask = LayerMask.GetMask("Water", "Enviroment", "Room", "SpikeCollidor");
        g.collision.diameter = 0.9f;
        g.SetDimensions(30, 24, 1f);
        g.Scan();
                {
            graphIndex = g.graphIndex,
            modifyWalkability = true,
            setWalkable = false,
            modifyPenalty = true,
            penaltyDelta = 1000
        };
        */

        // 定义更新范围
        var bounds = new Bounds(RoomVector, new Vector3(30, 24, 1f));

        // 创建更新对象，仅作用于 g
        var guo = new GraphUpdateObject(bounds);

        // 提交到路径线程
        AstarPath.active.UpdateGraphs(guo);
    }


}
