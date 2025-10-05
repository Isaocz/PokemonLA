using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Yawn : Skill
{
    Vector2 direction;
    Vector3 StartPostion;
    bool isCanNotMove;
    float MoveSpeed = 10.0f;

    bool isSleepDone;


    float PlayerScaleTimer;
    GameObject PlayerSpriteParent;
    Vector3 StartScale;

    bool isDestroy;

    // Start is called before the first frame update
    void Start()
    {
        direction = (transform.rotation * Vector2.right).normalized;
        StartPostion = transform.position;
        transform.rotation = Quaternion.Euler(0, 0, 0);
        PlayerSpriteParent = player.transform.GetChild(3).gameObject;
        StartScale = PlayerSpriteParent.transform.localScale;
    }

    private void Update()
    {
        ResetPlayer();
        if (PlayerScaleTimer < 0.15f)
        {
            PlayerScaleTimer += Time.deltaTime;
            PlayerSpriteParent.transform.localScale = new Vector3(Mathf.Clamp(PlayerSpriteParent.transform.localScale.x - Time.deltaTime * 2 , 0.7f, 1.0f), Mathf.Clamp(PlayerSpriteParent.transform.localScale.y + Time.deltaTime * 4, 1.0f, 1.6f), PlayerSpriteParent.transform.localScale.z);
        }
        else if (PlayerScaleTimer >= 0.15f && PlayerScaleTimer < 0.5f) { PlayerScaleTimer += Time.deltaTime; }
        else if (PlayerScaleTimer >= 0.5f && PlayerScaleTimer < 0.65f)
        {
            PlayerScaleTimer += Time.deltaTime;
            PlayerSpriteParent.transform.localScale = new Vector3(Mathf.Clamp(PlayerSpriteParent.transform.localScale.x + Time.deltaTime * 2, 0.7f, 1.0f), Mathf.Clamp(PlayerSpriteParent.transform.localScale.y - Time.deltaTime * 4, 1.0f, 1.6f), PlayerSpriteParent.transform.localScale.z);
        }
        else if (PlayerScaleTimer >= 0.65f) { PlayerSpriteParent.transform.localScale = player.PlayerLocalScal; }
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


    private void FixedUpdate()
    {
        ResetPlayer();
        if (!isCanNotMove)
        {
            MoveSpeed = Mathf.Clamp(MoveSpeed - 4 * Time.deltaTime, 0, 100);
            Vector3 postion = transform.position;
            postion.x += direction.x * MoveSpeed * Time.deltaTime;
            postion.y += direction.y * MoveSpeed * Time.deltaTime;
            transform.position = postion;
            if ((StartPostion - transform.position).magnitude > MaxRange)
            {
                DestroyYawm();
            }
        }
        if (isDestroy)
        {
            DestroyYawm();
        }
    }

    void YawmBreak()
    {
        if (!isCanNotMove)
        {
            transform.GetComponent<Collider2D>().enabled = false;
            isCanNotMove = true;
            isDestroy = true;
        }

    }


    void DestroyYawm()
    {
        transform.GetComponent<SpriteRenderer>().color -= new Color(0, 0, 0, Time.deltaTime);
        if (transform.GetComponent<SpriteRenderer>().color.a < 0.05f)
        {
            Destroy(gameObject);
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.isTrigger == false)
        {
            if (other.tag == "Empty")
            {
                Empty target = other.GetComponent<Empty>();
                if (target != null)
                {
                    if (SkillFrom != 2)
                    {
                        if (!isSleepDone) {
                            isSleepDone = true;
                            target.EmptySleepDone(1, 5.0f, 1);
                            YawmBreak();
                        }
                    }
                    else
                    {
                        target.EmptySleepDone(1, 5.0f, 1);
                    }
                }
            }
        }
    }
}
