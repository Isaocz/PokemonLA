using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Uturn : Skill
{

    Rigidbody2D PlayerRigidbody2D;
    Vector2 TurnD;

    void Start()
    {
        animator = GetComponent<Animator>();
        PlayerRigidbody2D = player.GetComponent<Rigidbody2D>();
        TurnD = -player.look;
    }


    //每帧减少飞弹的存在时间，当存在时间等于0时销毁飞弹
    // Update is called once per frame
    void Update()
    {
        StartExistenceTimer();
        if (ExistenceTime <= 0.25f)
        {
            player.isInvincibleAlways = true;
            if (player.look != TurnD) { player.look = TurnD; }
            float CollidorOffset = 0;
            float CollidorRadiusH = 0;
            float CollidorRadiusV = 0;
            switch (player.PlayerBodySize)
            {
                case 0:
                    CollidorOffset = 0.4023046f; CollidorRadiusH = 0.6039822f; CollidorRadiusV = 0.2549849f;
                    break;
                case 1:
                    CollidorOffset = 0.5f; CollidorRadiusH = 0.7f; CollidorRadiusV = 0.7f;
                    break;
                case 2:
                    CollidorOffset = 0.7f; CollidorRadiusH = 1.3f; CollidorRadiusV = 1.1f;
                    break;
            }
            RaycastHit2D SearchED = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + CollidorOffset), Vector2.down, CollidorRadiusH + TurnD.x * 3f * player.speed * Time.deltaTime, LayerMask.GetMask("Enviroment", "Room"));
            RaycastHit2D SearchEU = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + CollidorOffset), Vector2.up, CollidorRadiusH + TurnD.x * 3f * player.speed * Time.deltaTime, LayerMask.GetMask("Enviroment", "Room"));
            RaycastHit2D SearchER = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + CollidorOffset), Vector2.right, CollidorRadiusV + TurnD.x * 3f * player.speed * Time.deltaTime, LayerMask.GetMask("Enviroment", "Room"));
            RaycastHit2D SearchEL = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + CollidorOffset), Vector2.left, CollidorRadiusV + TurnD.x * 3f * player.speed * Time.deltaTime, LayerMask.GetMask("Enviroment", "Room"));

            if ((SearchED.collider != null && (SearchED.transform.tag == "Enviroment" || SearchED.transform.tag == "Room"))
                || (SearchEU.collider != null && (SearchEU.transform.tag == "Enviroment" || SearchEU.transform.tag == "Room"))
                || (SearchER.collider != null && (SearchER.transform.tag == "Enviroment" || SearchER.transform.tag == "Room"))
                || (SearchEL.collider != null && (SearchEL.transform.tag == "Enviroment" || SearchEL.transform.tag == "Room"))) { }
            else
            {
                Vector2 position = PlayerRigidbody2D.position;
                if (player.NowRoom != new Vector3Int(100, 100, 0))
                {
                    position.x = Mathf.Clamp(position.x + TurnD.x * 2.5f * player.speed * Time.deltaTime, player.NowRoom.x * 30 - 12, player.NowRoom.x * 30 + 12);
                    position.y = Mathf.Clamp(position.y + TurnD.y * 2.5f * player.speed * Time.deltaTime, player.NowRoom.y * 24 - 7.3f, player.NowRoom.y * 24 + 7.3f);
                }
                else
                {
                    position.x = Mathf.Clamp(position.x + TurnD.x * 2.5f * player.speed * Time.deltaTime, player.NowRoom.x * 30 - 12, player.NowRoom.x * 30 + 41.5f);
                    position.y = Mathf.Clamp(position.y + TurnD.y * 2.5f * player.speed * Time.deltaTime, player.NowRoom.y * 24 - 7.3f, player.NowRoom.y * 24 + 33.5f);
                }
                PlayerRigidbody2D.position = position;
            }
        }
    }


    //当飞弹与目标碰撞时，如果目标时敌人，获取敌人的血量，并使敌人扣血
    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.tag == "Empty")
        {
            Empty target = other.GetComponent<Empty>();
            int hp = target.EmptyHp;
            HitAndKo(target);
            if (SkillFrom == 2) { Drain(hp, target.EmptyHp, 0.25f); }
            if (animator != null) { animator.SetTrigger("Hit"); }
        }

    }

    private void OnDestroy()
    {
        player.isInvincibleAlways = false;
    }

}
