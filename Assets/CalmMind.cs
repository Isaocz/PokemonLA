using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalmMind : Skill
{
    // Start is called before the first frame update
    void Start()
    {
        if (player.playerData.SpABounsJustOneRoom < 8)
        {
            player.playerData.SpABounsJustOneRoom += 1;
        }
        if (player.playerData.SpDBounsJustOneRoom < 8)
        {
            player.playerData.SpDBounsJustOneRoom += 1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        StartExistenceTimer();
    }
}
