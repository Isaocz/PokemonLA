using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Acupressure : Skill
{

    int PlusIndex1;
    int PlusIndex2;

    // Start is called before the first frame update
    void Start()
    {

        PlusIndex1 = Random.Range(0, 5);
        ImproveAb(PlusIndex1);

        if (SkillFrom == 2)
        {
            PlusIndex2 = Random.Range(0, 5);
            while (PlusIndex2 == PlusIndex1)
            {
                PlusIndex2 = Random.Range(0, 5);
            }
            ImproveAb(PlusIndex2);
        }

        

        player.ReFreshAbllityPoint();

    }

    void ImproveAb(int Index)
    {
        switch (Index)
        {
            case 0:
                player.playerData.AtkBounsJustOneRoom += 2;
                break;
            case 1:
                player.playerData.DefBounsJustOneRoom += 2;
                break;
            case 2:
                player.playerData.SpABounsJustOneRoom += 2;
                break;
            case 3:
                player.playerData.SpDBounsJustOneRoom += 2;
                break;
            case 4:
                player.playerData.MoveSpeBounsJustOneRoom += 2;
                break;
        }
    }


    // Update is called once per frame
    void Update()
    {
        StartExistenceTimer();
    }
}
