using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkUp : Skill
{

    GameObject PlayerSpriteParent;
    Vector3 StartScale;

    // Start is called before the first frame update
    void Start()
    {
        if (SkillFrom != 2) {
            player.playerData.AtkBounsJustOneRoom += 1;
            player.playerData.SpABounsJustOneRoom += 1;
        }
        else
        {
            if (player.playerData.AtkBounsAlways + player.playerData.AtkBounsJustOneRoom < 0) { player.playerData.AtkBounsJustOneRoom = 1 - player.playerData.AtkBounsAlways; }
            else { player.playerData.AtkBounsJustOneRoom += 1; }
            if (player.playerData.SpABounsAlways + player.playerData.SpABounsJustOneRoom < 0) { player.playerData.SpABounsJustOneRoom = 1 - player.playerData.SpABounsAlways; }
            else { player.playerData.SpABounsJustOneRoom += 1; }
        }
        player.ReFreshAbllityPoint();

        PlayerSpriteParent = player.transform.GetChild(3).gameObject;
        StartScale = PlayerSpriteParent.transform.localScale;
    }

    void Update()
    {
        StartExistenceTimer();
        ResetPlayer();

        if (ExistenceTime > 1.3f)
        {
            PlayerSpriteParent.transform.localScale = new Vector3(Mathf.Clamp(PlayerSpriteParent.transform.localScale.x + Time.deltaTime * 2, 1.0f, 1.6f), Mathf.Clamp(PlayerSpriteParent.transform.localScale.y + Time.deltaTime * 2, 1.0f, 1.6f), PlayerSpriteParent.transform.localScale.z);
        }
        else if (ExistenceTime <= 0.9f && ExistenceTime > 0.6f)
        {
            PlayerSpriteParent.transform.localScale = new Vector3(Mathf.Clamp(PlayerSpriteParent.transform.localScale.x - Time.deltaTime * 2, 1.0f, 1.6f), Mathf.Clamp(PlayerSpriteParent.transform.localScale.y - Time.deltaTime * 2, 1.0f, 1.6f), PlayerSpriteParent.transform.localScale.z);
        }
    }


    void ResetPlayer()
    {
        if ((player == null && baby == null) || PlayerSpriteParent == null)
        {
            player = GameObject.FindObjectOfType<PlayerControler>();
            PlayerSpriteParent = player.transform.GetChild(3).gameObject;
            StartScale = PlayerSpriteParent.transform.localScale;
        }
    }


    private void OnDestroy()
    {
        ResetPlayer();
        PlayerSpriteParent.transform.localScale = player.PlayerLocalScal;
    }

}
