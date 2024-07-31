using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardSwap : Skill
{

    int DefSpDSub;

    // Start is called before the first frame update
    void Start()
    {

        DefSpDSub = Mathf.Abs(player.DefAbilityPoint - player.SpdAbilityPoint);

        int S;
        S = player.playerData.DefBounsAlways;      player.playerData.DefBounsAlways = player.playerData.SpDBounsAlways;           player.playerData.SpDBounsAlways = S;
        S = player.playerData.DefBounsJustOneRoom; player.playerData.DefBounsJustOneRoom = player.playerData.SpDBounsJustOneRoom; player.playerData.SpDBounsJustOneRoom = S;

        float Sf;
        Sf = player.playerData.DefHardWorkAlways;      player.playerData.DefHardWorkAlways = player.playerData.SpDHardWorkAlways;           player.playerData.SpDHardWorkAlways = Sf;
        Sf = player.playerData.DefHardWorkJustOneRoom; player.playerData.DefHardWorkJustOneRoom = player.playerData.SpDHardWorkJustOneRoom; player.playerData.SpDHardWorkJustOneRoom = Sf;
        player.ReFreshAbllityPoint();
        transform.position += Vector3.up * 0.6f;

        

    }

    private void OnDestroy()
    {
        if (SkillFrom == 2)
        {
            float RecoverPer = Mathf.Clamp( (float)DefSpDSub / 1000.0f , 0.0f , 0.8f );
            player.MinusSkillCDTime(this , RecoverPer , false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        StartExistenceTimer();
    }
}
