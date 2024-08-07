using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubAvalanche : SubSkill
{

    int StartPlayerHP;
    bool isPlayerBeHit;

    // Start is called before the first frame update
    void Start()
    {
        player.RemoveASubSkill(subskill);
        StartPlayerHP = player.Hp;
        
        if (MainSkill.SkillFrom == 2)
        {
            GameObject NowRoomEmptyFile = MapCreater.StaticMap.RRoom[player.NowRoom].transform.GetChild(3).gameObject;
            for (int i = 0; i < NowRoomEmptyFile.transform.childCount; i++)
            {
                Empty e = NowRoomEmptyFile.transform.GetChild(i).GetComponent<Empty>();
                if (e != null && e.isEmptyFrozenDone)
                {
                    e.FrozenRemove();
                    Vector3 Director = Vector3.down;
                    int AvaRotation = 0;
                    switch (Random.Range(0, 4))
                    {
                        case 0: Director = Vector3.down; AvaRotation = 270; break;
                        case 1: Director = Vector3.up; AvaRotation = 90; break;
                        case 2: Director = Vector3.left; AvaRotation = 180; break;
                        case 3: Director = Vector3.right; AvaRotation = 0; break;
                    }
                    Instantiate(gameObject, e.transform.position + Director, Quaternion.Euler(0, 0, AvaRotation));
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        StartExistenceTimer();
        if (!isPlayerBeHit && StartPlayerHP > player.Hp)
        {
            isPlayerBeHit = true;
            player.AddASubSkill(subskill);
        }
    }


}
