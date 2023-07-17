using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavySlam : Skill
{
    //30:+20 70:+40 120:60 200:80 300:100 400:120 500:135 600:+150
    
    GameObject BSColloder;
    GameObject PlayerSpriteParent;
    Vector3 StartScale;
    Vector3 StartPosition;

    // Start is called before the first frame update
    void Start()
    {
        SetPower();
        PlayerSpriteParent = player.transform.GetChild(3).gameObject;
        StartScale = PlayerSpriteParent.transform.localScale;
        StartPosition = PlayerSpriteParent.transform.localPosition;
        BSColloder = transform.GetChild(2).gameObject;
    }

    void SetPower()
    {
        if(player.PlayerBodySize == 0) { Damage = 60; }
        else if(player.PlayerBodySize == 1) { Damage = 80; }
        else if(player.PlayerBodySize == 2) { Damage = 100; }
        if(SkillFrom == 2)
        {
            if ( player.DefAbilityPoint >= 30 && player.DefAbilityPoint <= 70) { Damage += 20; }
            else if ( player.DefAbilityPoint >= 70 && player.DefAbilityPoint < 120) { Damage += 40; }
            else if ( player.DefAbilityPoint >= 120 && player.DefAbilityPoint < 200) { Damage += 60; }
            else if ( player.DefAbilityPoint >= 200 && player.DefAbilityPoint < 300) { Damage += 80; }
            else if ( player.DefAbilityPoint >= 300 && player.DefAbilityPoint < 400) { Damage += 100; }
            else if ( player.DefAbilityPoint >= 400 && player.DefAbilityPoint < 500) { Damage += 120; }
            else if ( player.DefAbilityPoint >= 500 && player.DefAbilityPoint < 600) { Damage += 135; }
            else if ( player.DefAbilityPoint >= 600) { Damage += 150; }
        }
    }

    void ResetPlayer()
    {
        if ((player == null && baby == null) || PlayerSpriteParent == null)
        {
            player = GameObject.FindObjectOfType<PlayerControler>();
            PlayerSpriteParent = player.transform.GetChild(3).gameObject;
            StartScale = PlayerSpriteParent.transform.localScale;
            StartPosition = PlayerSpriteParent.transform.localPosition;
        }
    }

    // Update is called once per frame
    void Update()
    {
        StartExistenceTimer();
        ResetPlayer();

        if (ExistenceTime > 1.4f)
        {
            PlayerSpriteParent.transform.localScale = new Vector3(PlayerSpriteParent.transform.localScale.x, Mathf.Clamp(PlayerSpriteParent.transform.localScale.y - Time.deltaTime * 2, 0.8f, 1), PlayerSpriteParent.transform.localScale.z);
        }
        else if (ExistenceTime <= 1.4f && ExistenceTime > 1f)
        {
            PlayerSpriteParent.transform.localScale = new Vector3(Mathf.Clamp(PlayerSpriteParent.transform.localScale.x - Time.deltaTime * 0.5f, 0.8f, 1.0f), Mathf.Clamp(PlayerSpriteParent.transform.localScale.y + Time.deltaTime * 1f, 0.8f, 1.2f), PlayerSpriteParent.transform.localScale.z);
        }
        else if (ExistenceTime <= 1f && ExistenceTime > 0.8f)
        {
            player.isInvincibleAlways = true;
            PlayerSpriteParent.transform.localScale = new Vector3(Mathf.Clamp(PlayerSpriteParent.transform.localScale.x + Time.deltaTime * 1f, 0.8f, 1.0f), Mathf.Clamp(PlayerSpriteParent.transform.localScale.y - Time.deltaTime * 1f, 1.0f, 1.2f), PlayerSpriteParent.transform.localScale.z);
            PlayerSpriteParent.transform.localPosition = new Vector3(PlayerSpriteParent.transform.localPosition.x, Mathf.Clamp(PlayerSpriteParent.transform.localPosition.y + Time.deltaTime * 8, 0.0f, 1.6f), PlayerSpriteParent.transform.localPosition.z);
        }
        else if (ExistenceTime <= 0.8f && ExistenceTime > 0.65f)
        {
            PlayerSpriteParent.transform.localPosition = new Vector3(PlayerSpriteParent.transform.localPosition.x, Mathf.Clamp(PlayerSpriteParent.transform.localPosition.y - Time.deltaTime * 8, 0.4f, 1.6f), PlayerSpriteParent.transform.localPosition.z);
        }
        else if (ExistenceTime <= 0.65f && ExistenceTime > 0.6f)
        {
            player.isCanNotMove = true;
            transform.GetChild(0).gameObject.SetActive(true);
            BSColloder.SetActive(true);
            PlayerSpriteParent.transform.localPosition = new Vector3(PlayerSpriteParent.transform.localPosition.x, Mathf.Clamp(PlayerSpriteParent.transform.localPosition.y - Time.deltaTime * 8, 0.0f, 0.4f), PlayerSpriteParent.transform.localPosition.z);
            PlayerSpriteParent.transform.localScale = new Vector3(Mathf.Clamp(PlayerSpriteParent.transform.localScale.x + Time.deltaTime * 4f, 1.0f, 1.2f), Mathf.Clamp(PlayerSpriteParent.transform.localScale.y - Time.deltaTime * 8f, 0.6f, 1.0f), PlayerSpriteParent.transform.localScale.z);
        }
        else if (ExistenceTime <= 0.6f && ExistenceTime > 0.4f)
        {
            player.isInvincibleAlways = false;
            PlayerSpriteParent.transform.localScale = new Vector3(Mathf.Clamp(PlayerSpriteParent.transform.localScale.x - Time.deltaTime * 1f, 1.0f, 1.2f), Mathf.Clamp(PlayerSpriteParent.transform.localScale.y + Time.deltaTime * 2f, 0.6f, 1.0f), PlayerSpriteParent.transform.localScale.z);
        }
        else if (ExistenceTime <= 0.2f)
        {
            player.isCanNotMove = false;
        }
    }

    private void OnDestroy()
    {
        ResetPlayer();
        PlayerSpriteParent.transform.localScale = new Vector3(1, 1, 1);
        PlayerSpriteParent.transform.localPosition = Vector3.zero;
        player.isCanNotMove = false;
        player.isInvincibleAlways = false;
    }
}
