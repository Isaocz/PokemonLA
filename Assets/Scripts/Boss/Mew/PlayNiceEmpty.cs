using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayNiceEmpty : MonoBehaviour
{
    public float skillDuration = 2.5f;
    public Mew mew;
    PlayerControler player;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerControler>();
        Invoke("ApplySkillEffects", skillDuration);
    }

    void ApplySkillEffects()
    {
        if (mew.AtkAbilityPoint < 10)
            mew.AtkAbilityPoint += 1;
        if (mew.SpAAbilityPoint < 10)
            mew.SpAAbilityPoint += 1;
        if (mew.DefAbilityPoint < 10)
            mew.DefAbilityPoint += 1;
        if (mew.SpdAbilityPoint < 10)
            mew.SpdAbilityPoint += 1;
        if (mew.SpeedAbilityPoint < 10)
            mew.SpeedAbilityPoint += 1;

        if (player.playerData.AtkBounsJustOneRoom > -10)
            player.playerData.AtkBounsJustOneRoom -= 1;
        if (player.playerData.SpABounsJustOneRoom > -10)
            player.playerData.SpABounsJustOneRoom -= 1;
        if (player.playerData.DefBounsJustOneRoom > -10)
            player.playerData.DefBounsJustOneRoom -= 1;
        if (player.playerData.SpDBounsJustOneRoom > -10)
            player.playerData.SpDBounsJustOneRoom -= 1;
    }
}
