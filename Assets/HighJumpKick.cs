using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighJumpKick : Skill
{
    public bool isHJKHitDone;
    Rigidbody2D PlayerRigidbody2D;
    Vector2 Direction;
    float SpeedBouns;

    public SubHighJumpKick sub;

    GameObject BSColloder;
    GameObject PlayerSpriteParent;
    Vector3 StartScale;
    Vector3 StartPosition;

    // Start is called before the first frame update
    void Start()
    {
        SpeedBouns = 3;
        PlayerRigidbody2D = player.GetComponent<Rigidbody2D>();
        Direction = Quaternion.AngleAxis(transform.rotation.eulerAngles.z , Vector3.forward) * Vector2.right;
        transform.rotation = Quaternion.Euler(0, 0, 0);
        Debug.Log(Direction);
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
            player.isCanNotMove = true;
            BSColloder.SetActive(true);
            PlayerMove(SpeedBouns);
            PlayerSpriteParent.transform.localPosition = new Vector3(PlayerSpriteParent.transform.localPosition.x, Mathf.Clamp(PlayerSpriteParent.transform.localPosition.y - Time.deltaTime * 8, 0.4f, 1.6f), PlayerSpriteParent.transform.localPosition.z);
        }
        else if (ExistenceTime <= 0.65f && ExistenceTime > 0.6f)
        {

            PlayerMove(SpeedBouns);
            transform.GetChild(0).gameObject.SetActive(true);
            PlayerSpriteParent.transform.localPosition = new Vector3(PlayerSpriteParent.transform.localPosition.x, Mathf.Clamp(PlayerSpriteParent.transform.localPosition.y - Time.deltaTime * 8, 0.0f, 0.4f), PlayerSpriteParent.transform.localPosition.z);
            PlayerSpriteParent.transform.localScale = new Vector3(Mathf.Clamp(PlayerSpriteParent.transform.localScale.x + Time.deltaTime * 4f, 1.0f, 1.2f), Mathf.Clamp(PlayerSpriteParent.transform.localScale.y - Time.deltaTime * 8f, 0.6f, 1.0f), PlayerSpriteParent.transform.localScale.z);
        }
        else if (ExistenceTime <= 0.6f && ExistenceTime > 0.4f)
        {
            SpeedBouns -= Time.deltaTime * 5;
            PlayerMove(SpeedBouns);
            PlayerSpriteParent.transform.localScale = new Vector3(Mathf.Clamp(PlayerSpriteParent.transform.localScale.x - Time.deltaTime * 1f, 1.0f, 1.2f), Mathf.Clamp(PlayerSpriteParent.transform.localScale.y + Time.deltaTime * 2f, 0.6f, 1.0f), PlayerSpriteParent.transform.localScale.z);
        }
        else if (ExistenceTime <= 0.4f && ExistenceTime > 0.2f)
        {
            SpeedBouns -= Time.deltaTime * 10;
            PlayerMove(SpeedBouns);
        }
        else if (ExistenceTime <= 0.2f)
        {
            PlayerMove(SpeedBouns);
            player.isInvincibleAlways = false;
            player.isCanNotMove = false;
        }
    }


    void PlayerMove(float MoveSpeed)
    {
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
        RaycastHit2D SearchED = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + CollidorOffset), Vector2.down, CollidorRadiusH + Direction.x * (MoveSpeed + 1) * player.speed * Time.deltaTime, LayerMask.GetMask("Enviroment", "Room"));
        RaycastHit2D SearchEU = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + CollidorOffset), Vector2.up, CollidorRadiusH + Direction.x * (MoveSpeed + 1) * player.speed * Time.deltaTime, LayerMask.GetMask("Enviroment", "Room"));
        RaycastHit2D SearchER = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + CollidorOffset), Vector2.right, CollidorRadiusV + Direction.x * (MoveSpeed + 1) * player.speed * Time.deltaTime, LayerMask.GetMask("Enviroment", "Room"));
        RaycastHit2D SearchEL = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + CollidorOffset), Vector2.left, CollidorRadiusV + Direction.x * (MoveSpeed + 1) * player.speed * Time.deltaTime, LayerMask.GetMask("Enviroment", "Room"));

        if ((SearchED.collider != null && (SearchED.transform.tag == "Enviroment" || SearchED.transform.tag == "Room"))
            || (SearchEU.collider != null && (SearchEU.transform.tag == "Enviroment" || SearchEU.transform.tag == "Room"))
            || (SearchER.collider != null && (SearchER.transform.tag == "Enviroment" || SearchER.transform.tag == "Room"))
            || (SearchEL.collider != null && (SearchEL.transform.tag == "Enviroment" || SearchEL.transform.tag == "Room"))) { }
        else
        {
            Vector2 position = PlayerRigidbody2D.position;
            if (player.NowRoom != new Vector3Int(100, 100, 0))
            {
                position.x = Mathf.Clamp(position.x + Direction.x * MoveSpeed * player.speed * Time.deltaTime, player.NowRoom.x * 30 - 12, player.NowRoom.x * 30 + 12);
                position.y = Mathf.Clamp(position.y + Direction.y * MoveSpeed * player.speed * Time.deltaTime, player.NowRoom.y * 24 - 7.3f, player.NowRoom.y * 24 + 7.3f);
            }
            else
            {
                position.x = Mathf.Clamp(position.x + Direction.x * MoveSpeed * player.speed * Time.deltaTime, player.NowRoom.x * 30 - 12, player.NowRoom.x * 30 + 41.5f);
                position.y = Mathf.Clamp(position.y + Direction.y * MoveSpeed * player.speed * Time.deltaTime, player.NowRoom.y * 24 - 7.3f, player.NowRoom.y * 24 + 33.5f);
            }
            PlayerRigidbody2D.position = position;
        }
    }


    private void OnDestroy()
    {
        ResetPlayer();
        PlayerSpriteParent.transform.localScale = new Vector3(1, 1, 1);
        PlayerSpriteParent.transform.localPosition = Vector3.zero;
        player.isCanNotMove = false;
        player.isInvincibleAlways = false;

        if (SkillFrom == 2)
        {
            if (!isHJKHitDone) { Pokemon.PokemonHpChange(null, player.gameObject, player.maxHp / 3, 0, 0, Type.TypeEnum.IgnoreType); }
            else { player.AddASubSkill(sub); }
        }
        else
        {
            if (!isHJKHitDone) { Pokemon.PokemonHpChange(null, player.gameObject, player.maxHp / 6, 0, 0, Type.TypeEnum.IgnoreType); }
        }
    }
}
