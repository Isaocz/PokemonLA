using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TailWhip : Skill
{
    List<Empty> AtkDownTargetList = new List<Empty> { };
    GameObject PlayerSpriteParent;
    Vector3 StartScale;
    Quaternion StartRotation;
    Vector3 StartPosition;

    private void Start()
    {
        PlayerSpriteParent = player.transform.GetChild(3).gameObject;
        StartScale = PlayerSpriteParent.transform.localScale;
        StartPosition = PlayerSpriteParent.transform.localPosition;
        StartRotation = PlayerSpriteParent.transform.localRotation;
        player.isCanNotMove = true;
        player.animator.SetFloat("Speed", 0);
        player.look = new Vector2(0, -1);
        player.animator.SetFloat("LookX", 0);
        player.animator.SetFloat("LookY", -1);
    }

    // Update is called once per frame
    void Update()
    {
        StartExistenceTimer();
        ResetPlayer();

        if (ExistenceTime > 1.0f)
        {
            PlayerSpriteParent.transform.localScale = new Vector3(Mathf.Clamp(PlayerSpriteParent.transform.localScale.x + Time.deltaTime * 1f, 1.0f, 1.2f), Mathf.Clamp(PlayerSpriteParent.transform.localScale.y - Time.deltaTime * 1f, 0.8f, 1.0f), PlayerSpriteParent.transform.localScale.z);
            PlayerSpriteParent.transform.localPosition = new Vector3(Mathf.Clamp(PlayerSpriteParent.transform.localPosition.x - Time.deltaTime * 0.5f , -0.1f, 0.0f), PlayerSpriteParent.transform.localPosition.y, PlayerSpriteParent.transform.localPosition.z);
            PlayerSpriteParent.transform.localRotation = Quaternion.Euler(0,0, PlayerSpriteParent.transform.localRotation.eulerAngles.z + 50* Time.deltaTime);
        }
        else if (ExistenceTime <= 1.0f && ExistenceTime > 0.8f)
        {
            PlayerSpriteParent.transform.localScale = new Vector3(Mathf.Clamp(PlayerSpriteParent.transform.localScale.x - Time.deltaTime * 2f, 0.8f, 1.2f), Mathf.Clamp(PlayerSpriteParent.transform.localScale.y + Time.deltaTime * 2f, 0.8f, 1.2f), PlayerSpriteParent.transform.localScale.z);
            PlayerSpriteParent.transform.localPosition = new Vector3(Mathf.Clamp(PlayerSpriteParent.transform.localPosition.x + Time.deltaTime * 0.5f, -0.1f, 0.0f), Mathf.Clamp(PlayerSpriteParent.transform.localPosition.y + Time.deltaTime * 0.8f, 0.0f, 0.16f), PlayerSpriteParent.transform.localPosition.z);
            PlayerSpriteParent.transform.localRotation = Quaternion.Euler(0, 0, PlayerSpriteParent.transform.localRotation.eulerAngles.z - 50 * Time.deltaTime);
        }
        else if (ExistenceTime <= 0.8f && ExistenceTime > 0.6f)
        {
            PlayerSpriteParent.transform.localScale = new Vector3(Mathf.Clamp(PlayerSpriteParent.transform.localScale.x + Time.deltaTime * 2f, 0.8f, 1.2f), Mathf.Clamp(PlayerSpriteParent.transform.localScale.y - Time.deltaTime * 2f, 0.8f, 1.2f), PlayerSpriteParent.transform.localScale.z);
            PlayerSpriteParent.transform.localPosition = new Vector3(Mathf.Clamp(PlayerSpriteParent.transform.localPosition.x + Time.deltaTime * 0.5f, 0.0f, 0.1f), Mathf.Clamp(PlayerSpriteParent.transform.localPosition.y - Time.deltaTime * 0.4f, 0.0f, 0.16f), PlayerSpriteParent.transform.localPosition.z);
            PlayerSpriteParent.transform.localRotation = Quaternion.Euler(0, 0, PlayerSpriteParent.transform.localRotation.eulerAngles.z - 50 * Time.deltaTime);
        }
        else if (ExistenceTime <= 0.6f && ExistenceTime > 0.4f)
        {
            PlayerSpriteParent.transform.localScale = new Vector3(Mathf.Clamp(PlayerSpriteParent.transform.localScale.x - Time.deltaTime * 2f, 0.8f, 1.2f), Mathf.Clamp(PlayerSpriteParent.transform.localScale.y + Time.deltaTime * 2f, 0.8f, 1.2f), PlayerSpriteParent.transform.localScale.z);
            PlayerSpriteParent.transform.localPosition = new Vector3(Mathf.Clamp(PlayerSpriteParent.transform.localPosition.x - Time.deltaTime * 0.5f, 0.0f, 0.1f), Mathf.Clamp(PlayerSpriteParent.transform.localPosition.y + Time.deltaTime * 0.8f, 0.0f, 0.16f), PlayerSpriteParent.transform.localPosition.z);
            PlayerSpriteParent.transform.localRotation = Quaternion.Euler(0, 0, PlayerSpriteParent.transform.localRotation.eulerAngles.z + 50 * Time.deltaTime);
        }
        else if (ExistenceTime <= 0.4f && ExistenceTime > 0.2f)
        {
            PlayerSpriteParent.transform.localScale = new Vector3(Mathf.Clamp(PlayerSpriteParent.transform.localScale.x + Time.deltaTime * 2f, 0.8f, 1.2f), Mathf.Clamp(PlayerSpriteParent.transform.localScale.y - Time.deltaTime * 2f, 0.8f, 1.2f), PlayerSpriteParent.transform.localScale.z);
            PlayerSpriteParent.transform.localPosition = new Vector3(Mathf.Clamp(PlayerSpriteParent.transform.localPosition.x - Time.deltaTime * 0.5f, -0.1f, 0.0f), Mathf.Clamp(PlayerSpriteParent.transform.localPosition.y - Time.deltaTime * 0.8f, 0.0f, 0.16f), PlayerSpriteParent.transform.localPosition.z);
            PlayerSpriteParent.transform.localRotation = Quaternion.Euler(0, 0, PlayerSpriteParent.transform.localRotation.eulerAngles.z + 50 * Time.deltaTime);
        }
        else if (ExistenceTime <= 0.2f)
        {
            PlayerSpriteParent.transform.localScale = new Vector3(Mathf.Clamp(PlayerSpriteParent.transform.localScale.x - Time.deltaTime * 1f, 1.0f, 1.2f), Mathf.Clamp(PlayerSpriteParent.transform.localScale.y + Time.deltaTime * 1f, 0.8f, 1.0f), PlayerSpriteParent.transform.localScale.z);
            PlayerSpriteParent.transform.localPosition = new Vector3(Mathf.Clamp(PlayerSpriteParent.transform.localPosition.x + Time.deltaTime * 0.5f, 0.0f, 0.1f), PlayerSpriteParent.transform.localPosition.y, PlayerSpriteParent.transform.localPosition.z);
            PlayerSpriteParent.transform.localRotation = Quaternion.Euler(0, 0, PlayerSpriteParent.transform.localRotation.eulerAngles.z - 50 * Time.deltaTime);
        }
        if (ExistenceTime <= 0.05f)
        {
            player.isCanNotMove = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Empty" && other.GetComponent<Empty>() != null)
        {
            Empty target = other.GetComponent<Empty>();
            if (!AtkDownTargetList.Contains(target))
            {
                AtkDownTargetList.Add(target);
                target.DefChange(-1, 7.5f);
                if (SkillFrom == 2) { target.EmptyInfatuation(2.0f,1); }
            }
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
            StartRotation = PlayerSpriteParent.transform.localRotation;
        }
    }


    private void OnDestroy()
    {
        ResetPlayer();
        PlayerSpriteParent.transform.localScale = player.PlayerLocalScal;
        PlayerSpriteParent.transform.localPosition = player.PlayerLocalPosition;
        PlayerSpriteParent.transform.localRotation = Quaternion.Euler(0,0,0);
        player.isCanNotMove = false;
    }



}
