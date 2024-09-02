using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipTurn : Skill
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

            BoxCollider2D boxc = player.GetComponent<BoxCollider2D>();
            CollidorOffset = boxc.offset.y; CollidorRadiusH = (boxc.size.x/2) + boxc.edgeRadius; CollidorRadiusV = (boxc.size.y / 2) + boxc.edgeRadius;

            RaycastHit2D SearchED = new RaycastHit2D();
            RaycastHit2D SearchEU = new RaycastHit2D();
            RaycastHit2D SearchER = new RaycastHit2D();
            RaycastHit2D SearchEL = new RaycastHit2D();
            if (TurnD == Vector2.down) { SearchED = Physics2D.Raycast(new Vector2(player.transform.position.x, player.transform.position.y + CollidorOffset), Vector2.down, (CollidorRadiusV + 3f * player.speed * Time.deltaTime), LayerMask.GetMask("Enviroment", "Room")); }
            if (TurnD == Vector2.up) { SearchEU = Physics2D.Raycast(new Vector2(player.transform.position.x, player.transform.position.y + CollidorOffset), Vector2.up, (CollidorRadiusV + 3f * player.speed * Time.deltaTime), LayerMask.GetMask("Enviroment", "Room")); }
            if (TurnD == Vector2.right) { SearchER = Physics2D.Raycast(new Vector2(player.transform.position.x, player.transform.position.y + CollidorOffset), Vector2.right, (CollidorRadiusH + 3f * player.speed * Time.deltaTime), LayerMask.GetMask("Enviroment", "Room")); }
            if (TurnD == Vector2.left) { SearchEL = Physics2D.Raycast(new Vector2(player.transform.position.x, player.transform.position.y + CollidorOffset), Vector2.left,  (CollidorRadiusH + 3f * player.speed * Time.deltaTime), LayerMask.GetMask("Enviroment", "Room")); }

            if ((SearchED.collider != null && (SearchED.transform.tag == "Enviroment" || SearchED.transform.tag == "Room"))
                || (SearchEU.collider != null && (SearchEU.transform.tag == "Enviroment" || SearchEU.transform.tag == "Room"))
                || (SearchER.collider != null && (SearchER.transform.tag == "Enviroment" || SearchER.transform.tag == "Room"))
                || (SearchEL.collider != null && (SearchEL.transform.tag == "Enviroment" || SearchEL.transform.tag == "Room"))) { }
            else
            {
                Vector2 position = PlayerRigidbody2D.position;
                position.x = Mathf.Clamp(position.x + TurnD.x * 2.5f * player.speed * Time.deltaTime, player.NowRoom.x * 30 + MapCreater.StaticMap.RRoom[player.NowRoom].RoomSize[2], player.NowRoom.x * 30 + MapCreater.StaticMap.RRoom[player.NowRoom].RoomSize[3]);
                position.y = Mathf.Clamp(position.y + TurnD.y * 2.5f * player.speed * Time.deltaTime, player.NowRoom.y * 24 + MapCreater.StaticMap.RRoom[player.NowRoom].RoomSize[1], player.NowRoom.y * 24 + MapCreater.StaticMap.RRoom[player.NowRoom].RoomSize[0]);
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
            if (SkillFrom == 2 && Random.Range(0.0f, 1.0f) + ((float)player.LuckPoint / 10.0f) >= 0.5f) { target.SpeedChange(); target.SpeedRemove01(3.0f * target.OtherStateResistance); }
            HitAndKo(target);
            if (animator != null) { animator.SetTrigger("Hit"); }
            GameObject ps1 = transform.GetChild(0).gameObject;
            GameObject ps2 = transform.GetChild(1).gameObject;
            transform.DetachChildren();
            ps1.SetActive(true);ps1.transform.rotation = Quaternion.Euler(0, 0, 0);ps1.transform.position = transform.position;ps1.transform.localScale = new Vector3(1, 1, 1);
            ps2.SetActive(true);ps2.transform.rotation = Quaternion.Euler(0, 0, 0);ps2.transform.position = transform.position;ps2.transform.localScale = new Vector3(1, 1, 1);
        }

    }

    private void OnDestroy()
    {
        player.isInvincibleAlways = false;
    }
}
