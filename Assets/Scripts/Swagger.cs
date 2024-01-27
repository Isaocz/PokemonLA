using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swagger : Skill
{
    GameObject PlayerSpriteParent;
    Vector3 StartScale;

    // Start is called before the first frame update
    void Start()
    {
        PlayerSpriteParent = player.transform.GetChild(3).gameObject;
        StartScale = PlayerSpriteParent.transform.localScale;
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

    // Update is called once per frame
    void Update()
    {
        StartExistenceTimer();
        ResetPlayer();

        if (ExistenceTime > 0.8f)
        {
            PlayerSpriteParent.transform.localScale = new Vector3(Mathf.Clamp(PlayerSpriteParent.transform.localScale.x + Time.deltaTime * 2, 1.0f, 1.6f), Mathf.Clamp(PlayerSpriteParent.transform.localScale.y + Time.deltaTime * 2, 1.0f, 1.6f), PlayerSpriteParent.transform.localScale.z);
        }
        else if (ExistenceTime <= 0.6f && ExistenceTime > 0.3f)
        {
            PlayerSpriteParent.transform.localScale = new Vector3(Mathf.Clamp(PlayerSpriteParent.transform.localScale.x - Time.deltaTime * 2, 1.0f, 1.6f), Mathf.Clamp(PlayerSpriteParent.transform.localScale.y - Time.deltaTime * 2, 1.0f, 1.6f), PlayerSpriteParent.transform.localScale.z);
        }
    }

    private void OnDestroy()
    {
        ResetPlayer();
        PlayerSpriteParent.transform.localScale = player.PlayerLocalScal; ;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Empty")
        {
            Empty e = other.GetComponent<Empty>();
            if (e != null)
            {
                e.EmptyConfusion(30,5);
                GameObject AngryEffect = PublicEffect.StaticPublicEffectList.ReturnAPublicEffect(2);
                Instantiate(AngryEffect, e.transform.position + Vector3.up * 1.5f, Quaternion.identity).SetActive(true);
                if (SkillFrom == 2) { e.EmptyInfatuation(2.5f, 1); }
                e.AtkChange(2, 10.0f);
            }
        }

    }

}
