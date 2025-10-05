using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomFile : MonoBehaviour
{

    /*
    public Dictionary<Room, float> RoomDict;


    public struct RoomPrefab
    {
        public Room room;
        public float roomweight;
    }
    public RoomPrefab[] RoomPrefabs;

    public void ShowDic()
    {
        // ×ÖµäÄÚÈÝ
        RoomDict = new Dictionary<Room, float>();
        for (int i = 0; i < RoomPrefabs.Length; i++)
        {
            // FruitType t = (FruitType)Enum.Parse(typeof(FruitType), sweetPrefabs[i].type, false);
            Room r = RoomPrefabs[i].room;
            if (!RoomDict.ContainsKey(r))
            {
                RoomDict.Add(r, RoomPrefabs[i].prefab);
            }
            else
            {
                Debug.LogError("key" + "ÓÐÖØ¸´");
            }
        }
        Debug.Log(RoomDict.Count);


    }
    */

    public List<Room> RoomList;
}
