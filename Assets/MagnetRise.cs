using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetRise : Skill
{

    bool isFly;


    // Start is called before the first frame update
    void Start()
    {
        if (SkillFrom == 2) { player.isMagnetRisePlus = true; }
        player.playerData.TypeDefJustOneRoom[5]++; 
        player.playerData.TypeDefJustOneRoom[13]++; 
        if (player.gameObject.layer != LayerMask.NameToLayer("PlayerFly"))
        {
            player.gameObject.layer = LayerMask.NameToLayer("PlayerFly");
            player.transform.position += new Vector3(0, 0.5f, 0);
            player.transform.GetChild(3).position = player.transform.GetChild(3).position + Vector3.up * 0.5f;
            player.PlayerLocalPosition = player.transform.GetChild(3).localPosition;
            isFly = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        StartExistenceTimer();
    }

    private void OnDestroy()
    {
        if (SkillFrom == 2) { player.isMagnetRisePlus = false; }
        player.playerData.TypeDefJustOneRoom[5]--;
        player.playerData.TypeDefJustOneRoom[13]--;
        if (isFly && player.gameObject.layer == LayerMask.NameToLayer("PlayerFly"))
        {
            player.gameObject.layer = LayerMask.NameToLayer("Player");
            player.transform.position -= new Vector3(0, 0.5f, 0);
            player.transform.GetChild(3).position = player.transform.GetChild(3).position - Vector3.up * 0.5f;
            player.PlayerLocalPosition = player.transform.GetChild(3).localPosition;
        }
    }



}
