using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StompingTantrum : Skill
{
    public bool isSTHitDone;
    public SubStompingTantrum SubST;

    public GameObject STMud;

    GameObject BSColloder;
    GameObject PlayerSpriteParent;
    Vector3 StartScale;
    Vector3 StartPosition;

    // Start is called before the first frame update
    void Start()
    {
        PlayerSpriteParent = player.transform.GetChild(3).gameObject;
        StartScale = PlayerSpriteParent.transform.localScale;
        StartPosition = PlayerSpriteParent.transform.localPosition;
        BSColloder = transform.GetChild(2).gameObject;
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
        if (!isSTHitDone) { player.AddASubSkill(SubST); }
        ResetPlayer();
        PlayerSpriteParent.transform.localScale = player.PlayerLocalScal;
        PlayerSpriteParent.transform.localPosition = player.PlayerLocalPosition;
        player.isCanNotMove = false;
        player.isInvincibleAlways = false;
    }
}
