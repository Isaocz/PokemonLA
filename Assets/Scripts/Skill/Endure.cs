using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Endure : Skill
{
    PlayerControler ParentPlayer;

    // Start is called before the first frame update
    void Start()
    {
        ParentPlayer = gameObject.transform.parent.GetComponent<PlayerControler>();
        if (ParentPlayer != null)
        {
            ParentPlayer.EndureStart();
            if (SkillFrom == 2)
            {
                player.playerData.DefBounsAlways++;
                player.playerData.SpDBounsAlways++;
            }
        }
    }

    void Update()
    {
        StartExistenceTimer();
    }

    private void OnDestroy()
    {
        ParentPlayer.EndureOver();
        if (SkillFrom == 2)
        {
            player.playerData.DefBounsAlways--;
            player.playerData.SpDBounsAlways--;
        }
    }
}
