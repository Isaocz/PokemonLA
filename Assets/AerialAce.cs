using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AerialAce : Skill
{

    Vector3 StartPosition;
    Rigidbody2D PlayerRigidbody;
    bool isReturnStart;
    bool isReturnOver;
    bool isNeedReturn;
    float MoveSpeed;
    Vector3 dir;
    Vector3Int PlayerNowRoom;
    bool isFly;
    SpriteRenderer BirdSprite;


    // Start is called before the first frame update
    void Start()
    {
        player.playerData.MoveSpwBounsAlways += 2;
        PlayerRigidbody = player.GetComponent<Rigidbody2D>();
        StartPosition = player.transform.position;
        player.ReFreshAbllityPoint();
        PlayerNowRoom = player.NowRoom;
        BirdSprite = transform.GetChild(0).GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        StartExistenceTimer();
        if (ExistenceTime > 0.5f && ExistenceTime <= 2.0f)
        {
            if (PlayerNowRoom != player.NowRoom) {
                ExistenceTime = 0.05f;
                player.playerData.MoveSpwBounsAlways -= 2;
                player.ReFreshAbllityPoint();
            }
        }
        else if (ExistenceTime > 0.2f && ExistenceTime <= 0.5f)
        {

            if (!isReturnStart)
            {
                isReturnStart = true;
                player.playerData.MoveSpwBounsAlways -= 2;
                player.ReFreshAbllityPoint();
                if (((StartPosition - player.transform.position).magnitude) <= 1.0f)
                {
                    isNeedReturn = false;
                    ExistenceTime = 0.05f;
                    player.ReFreshAbllityPoint();
                }
                else { isNeedReturn = true; }
                if (isNeedReturn) {
                    player.isCanNotMove = true;
                    player.isInvincibleAlways = true;
                    dir = (StartPosition - player.transform.position).normalized;
                    MoveSpeed = ((StartPosition - player.transform.position).magnitude) / (player.speed * 0.3f);
                    if (player.gameObject.layer != LayerMask.NameToLayer("PlayerFly"))
                    {
                        isFly = true;
                        player.gameObject.layer = LayerMask.NameToLayer("PlayerFly");
                        player.transform.position += new Vector3(0, 0.5f, 0);
                        player.transform.GetChild(3).position = player.transform.GetChild(3).position + Vector3.up * 0.5f;
                        player.PlayerLocalPosition = player.transform.GetChild(3).localPosition;

                    }
                    transform.GetChild(0).gameObject.SetActive(true);
                    transform.GetChild(1).gameObject.SetActive(true);
                    transform.rotation = Quaternion.Euler(0, 0, _mTool.Angle_360Y(dir, Vector2.right));
                    transform.GetChild(1).transform.localRotation = Quaternion.Euler(0, 0, -transform.rotation.eulerAngles.z);
                    transform.GetComponent<Collider2D>().enabled = true;
                }
            }
            if (isNeedReturn)
            {

                transform.GetChild(0).transform.localScale += new Vector3(1, 1.25f, 0) * Time.deltaTime;
                player.look = _mTool.MainVector2(dir);
                player.animator.SetFloat("LookX", player.look.x);
                player.animator.SetFloat("LookY", player.look.y);
                Vector2 position = PlayerRigidbody.position;
                if (player.NowRoom != new Vector3Int(100, 100, 0))
                {
                    position.x = Mathf.Clamp(position.x + dir.x * MoveSpeed * player.speed * Time.deltaTime, player.NowRoom.x * 30 - 12, player.NowRoom.x * 30 + 12);
                    position.y = Mathf.Clamp(position.y + dir.y * MoveSpeed * player.speed * Time.deltaTime, player.NowRoom.y * 24 - 7.3f, player.NowRoom.y * 24 + 7.3f);
                }
                else
                {
                    position.x = Mathf.Clamp(position.x + dir.x * MoveSpeed * player.speed * Time.deltaTime, player.NowRoom.x * 30 - 12, player.NowRoom.x * 30 + 41.5f);
                    position.y = Mathf.Clamp(position.y + dir.y * MoveSpeed * player.speed * Time.deltaTime, player.NowRoom.y * 24 - 7.3f, player.NowRoom.y * 24 + 33.5f);
                }
                PlayerRigidbody.position = position;
            }
        }
        else if (ExistenceTime > 0.0f && ExistenceTime <= 0.2f)
        {
            if (!isReturnOver)
            {
                isReturnOver = true;
                player.isCanNotMove = false;
                player.isInvincibleAlways = false;
                if(isFly){
                    if (player.gameObject.layer == LayerMask.NameToLayer("PlayerFly"))
                    {
                        player.gameObject.layer = LayerMask.NameToLayer("Player");
                        player.transform.position -= new Vector3(0, 0.5f, 0);
                        player.transform.GetChild(3).position = player.transform.GetChild(3).position - Vector3.up * 0.5f;
                        player.PlayerLocalPosition = player.transform.GetChild(3).localPosition;
                    }
                }
            }

            transform.GetChild(0).transform.localScale += new Vector3(2, 2.5f, 0) * Time.deltaTime;
            BirdSprite.color -= new Color(0, 0, 0, 5) * Time.deltaTime;
        }

        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Empty")
        {
            Empty target = other.GetComponent<Empty>();
            if (target != null)
            {
                HitAndKo(target);
            }

        }
    }

}
