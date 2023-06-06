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

    public void CreatNewGrid(Vector3 RoomVector)
    {
        GridGraph NowRoomGraph =  astarPath.data.AddGraph(typeof(GridGraph)) as GridGraph;
        NowRoomGraph.is2D = true;
        NowRoomGraph.collision.use2D = true;
        NowRoomGraph.collision.mask = LayerMask.GetMask("Water", "Enviroment", "Room","SpikeCollidor");
        NowRoomGraph.collision.diameter = 0.9f;
        NowRoomGraph.center = RoomVector;
        NowRoomGraph.SetDimensions(30, 24, 1f);
        NowRoomGraph.Scan();
    }
}
