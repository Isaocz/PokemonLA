using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerSwap : Skill
{
    int AtkSpASub;

    // Start is called before the first frame update
    void Start()
    {

        AtkSpASub = Mathf.Abs(player.DefAbilityPoint - player.SpdAbilityPoint);

        int S;
        S = player.playerData.AtkBounsAlways; player.playerData.AtkBounsAlways = player.playerData.SpABounsAlways; player.playerData.SpABounsAlways = S;
        S = player.playerData.AtkBounsJustOneRoom; player.playerData.AtkBounsJustOneRoom = player.playerData.SpABounsJustOneRoom; player.playerData.SpABounsJustOneRoom = S;

        float Sf;
        Sf = player.playerData.AtkHardWorkAlways; player.playerData.AtkHardWorkAlways = player.playerData.SpAHardWorkAlways; player.playerData.SpAHardWorkAlways = Sf;
        Sf = player.playerData.AtkHardWorkJustOneRoom; player.playerData.AtkHardWorkJustOneRoom = player.playerData.SpAHardWorkJustOneRoom; player.playerData.SpAHardWorkJustOneRoom = Sf;
        player.ReFreshAbllityPoint();
        transform.position += Vector3.up * 0.6f;



    }

    private void OnDestroy()
    {
        if (SkillFrom == 2)
        {
            float RecoverPer = Mathf.Clamp((float)AtkSpASub / 1000.0f, 0.0f, 0.8f);
            player.MinusSkillCDTime(this, RecoverPer, false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        StartExistenceTimer();
    }
}
