using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fissure : Skill
{
    GameObject Trail;
    Vector3 Direction;
    bool Up;

    int TrailMoveTimer;

    

    // Start is called before the first frame update
    void Start()
    {
        Trail = transform.GetChild(0).gameObject;
        Direction = (new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f) , 0)).normalized;
    }

    private void Update()
    {
        StartExistenceTimer();
    }

    private void FixedUpdate()
    {
        if (ExistenceTime > 1) {
            TrailMoveTimer += 1;
            Room NowPlayerRoom = MapCreater.StaticMap.RRoom[player.NowRoom];
            Vector3 NowPlayerRoomPosition = NowPlayerRoom.transform.position;
            Trail.transform.position = new Vector3(Mathf.Clamp((Trail.transform.position + Direction * Time.deltaTime * 5).x , NowPlayerRoomPosition.x + NowPlayerRoom.RoomSize[2] , NowPlayerRoomPosition.x + NowPlayerRoom.RoomSize[3]) ,
                                                   Mathf.Clamp((Trail.transform.position + Direction * Time.deltaTime * 5).y , NowPlayerRoomPosition.y + NowPlayerRoom.RoomSize[1] , NowPlayerRoomPosition.y + NowPlayerRoom.RoomSize[0]), 0); 
            if ((TrailMoveTimer % 10) == 0)
            {
                if (Up)
                {
                    Direction = Quaternion.Euler(0, 0, (Random.Range(-90, 30))) * Direction;
                    Up = false;
                }
                else
                {
                    Direction = Quaternion.Euler(0, 0, (Random.Range(30, 90))) * Direction;
                    Up = true;
                }
            }
        }
    }
}
