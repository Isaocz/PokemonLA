using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bulldoze : Skill
{
    public GameObject SoftMud;
    bool isSoftMudBorn;
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
            PlayerSpriteParent.transform.localScale -= new Vector3(0, Time.deltaTime * 2, 0);
        }
        else if (ExistenceTime <= 1.4f && ExistenceTime > 1f)
        {
            PlayerSpriteParent.transform.localScale += new Vector3(-Time.deltaTime * 0.5f, Time.deltaTime * 1f, 0);
        }
        else if (ExistenceTime <= 1f && ExistenceTime > 0.8f)
        {
            player.isInvincibleAlways = true;
            PlayerSpriteParent.transform.localScale += new Vector3(Time.deltaTime * 1f, -Time.deltaTime * 1f, 0);
            PlayerSpriteParent.transform.localPosition += new Vector3(0, 8f * Time.deltaTime, 0);
        }
        else if (ExistenceTime <= 0.8f && ExistenceTime > 0.65f)
        {
            PlayerSpriteParent.transform.localPosition -= new Vector3(0, 8f * Time.deltaTime, 0);
        }
        else if (ExistenceTime <= 0.65f && ExistenceTime > 0.6f)
        {
            if (SkillFrom == 2 && !isSoftMudBorn)
            {
                isSoftMudBorn = true;
                Instantiate(SoftMud, transform.position + Vector3.up * 0.2f, Quaternion.identity);
            }
            player.isCanNotMove = true;
            transform.GetChild(0).gameObject.SetActive(true);
            BSColloder.SetActive(true);
            PlayerSpriteParent.transform.localPosition -= new Vector3(0, 8f * Time.deltaTime, 0);
            PlayerSpriteParent.transform.localScale += new Vector3(Time.deltaTime * 4f, -Time.deltaTime * 8f, 0);
        }
        else if (ExistenceTime <= 0.6f && ExistenceTime > 0.4f)
        {
            player.isInvincibleAlways = false;
            PlayerSpriteParent.transform.localScale -= new Vector3(Time.deltaTime * 1f, -Time.deltaTime * 2f, 0);
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
