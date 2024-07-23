using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class QuickAttack : Skill
{
    Rigidbody2D PlayerRigibody;
    Vector2 Direction;
    Sprite playerTexture;
    float timer;
    public GameObject PlayerShadow;
    bool MoveStop;

    public float MoveSpeed;

    // Start is called before the first frame update
    void Start()
    {
        PlayerRigibody = player.GetComponent<Rigidbody2D>();
        Direction = transform.rotation * Vector2.right;
        player.isInvincibleAlways = true;
        playerTexture = player.transform.GetChild(3).GetChild(0).GetComponent<SpriteRenderer>().sprite;

        float Size = math.min(player.GetComponent<BoxCollider2D>().size.x, player.GetComponent<BoxCollider2D>().size.y);
        GetComponent<CircleCollider2D>().offset = Direction * Size;
        GetComponent<CircleCollider2D>().radius = Size;
    }

    // Update is called once per frame
    void Update()
    {


        StartExistenceTimer();
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            playerTexture = player.transform.GetChild(3).GetChild(0).GetComponent<SpriteRenderer>().sprite;
            timer = 0.04f;
            GameObject playershadow = Instantiate(PlayerShadow, player.transform.position, quaternion.identity);
            playershadow.GetComponent<SpriteRenderer>().sprite = playerTexture;
            Destroy(playershadow, 0.3f);
        }


    }

    private void FixedUpdate()
    {
        if (!MoveStop)
        {
            Vector2 postion = PlayerRigibody.position;
            postion.x += Direction.x * MoveSpeed * player.speed * Time.deltaTime;
            postion.y += Direction.y * MoveSpeed * player.speed * Time.deltaTime;
            PlayerRigibody.position = postion;


        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Empty")
        {
            Empty target = other.GetComponent<Empty>();
            HitAndKo(target);
        }
        if (other.tag == "Enviroment" || other.tag == "Room" || other.gameObject.tag == "Water")
        {
            MoveStop = true;
            ExistenceTime = 0.01f;
        }
    }

    private void OnDestroy()
    {
        player.isInvincibleAlways = false;
    }

    /*

    public float dashAmount;
    public float dashSpeed;
    public GameObject PlayerShadow;
    Sprite playerTexture;
    Rigidbody2D PlayerRigibody;
    Vector2 Direction;
    float timer = 0f;
    List<Empty> enemylist = new List<Empty>();

    private bool isDashing = false;
    // Start is called before the first frame update
    void Start()
    {
        //获取冲刺方向
        PlayerRigibody = player.GetComponent<Rigidbody2D>();
        Direction = player.look;

        //获取玩家图片
        playerTexture = player.transform.GetChild(3).GetChild(0).GetComponent<SpriteRenderer>().sprite;

        Dash();
    }

    void Dash()
    {
        Vector3 dashPosition = new Vector3(Mathf.Clamp((player.transform.position + (Vector3)Direction * dashAmount).x, player.NowRoom.x * 30 + MapCreater.StaticMap.RRoom[player.NowRoom].RoomSize[2], player.NowRoom.x * 30 + MapCreater.StaticMap.RRoom[player.NowRoom].RoomSize[3]), Mathf.Clamp((player.transform.position + (Vector3)Direction * dashAmount).y, player.NowRoom.y * 24 + MapCreater.StaticMap.RRoom[player.NowRoom].RoomSize[1], player.NowRoom.y * 24 + MapCreater.StaticMap.RRoom[player.NowRoom].RoomSize[0]), 0);

        //Vector3 dashPosition = new Vector3( Mathf.Clamp((player.transform.position + (Vector3)Direction * dashAmount).x, player.NowRoom.x*30                        - 12.7f                           , player.NowRoom.x * 30                         + 12.7f)                        , Mathf.Clamp((player.transform.position + (Vector3)Direction * dashAmount).y, player.NowRoom.y * 24                          - 9.0f,                          player.NowRoom.y * 24              + 9.0f), 0);
        Collider2D[] colliders = Physics2D.OverlapCircleAll(dashPosition, 1f);
        foreach (Collider2D collider in colliders)
        {//这里是检测水面，如果落点不是水面、墙、障碍，则检测位移前方是否有墙。如果落点是，则检测墙和水，这样可以让玩家位移过水面但是位移不过墙，同时不会传送进水面

            if (collider.CompareTag("Room") || collider.CompareTag("Enviroment") || collider.CompareTag("Water"))
            {
                //向三个检测 防止射线穿过碰撞物的微小缝隙
                RaycastHit2D raycastHit2D = Physics2D.Raycast(transform.position, Direction, dashAmount, LayerMask.GetMask("Room", "Enviroment", "Water"));
                RaycastHit2D raycastHit2D02 = Physics2D.Raycast(transform.position, Quaternion.AngleAxis(10,Vector3.forward) * Direction, dashAmount, LayerMask.GetMask("Room", "Enviroment", "Water"));
                RaycastHit2D raycastHit2D03 = Physics2D.Raycast(transform.position, Quaternion.AngleAxis(-10, Vector3.forward) * Direction, dashAmount, LayerMask.GetMask("Room", "Enviroment", "Water"));
                if (raycastHit2D.collider )
                {
                    dashPosition = raycastHit2D.point;
                }
                else if (raycastHit2D02.collider)
                {
                    dashPosition = raycastHit2D02.point;
                }
                else if (raycastHit2D03.collider)
                {
                    dashPosition = raycastHit2D03.point;
                }
                Debug.Log(transform.position + "+" + dashPosition);
                break;
            }
            else
            {
                RaycastHit2D raycastHit2D = Physics2D.Raycast(transform.position, Direction, dashAmount, LayerMask.GetMask("Room", "Enviroment"));
                RaycastHit2D raycastHit2D02 = Physics2D.Raycast(transform.position, Quaternion.AngleAxis(10, Vector3.forward) * Direction, dashAmount, LayerMask.GetMask("Room", "Enviroment"));
                RaycastHit2D raycastHit2D03 = Physics2D.Raycast(transform.position, Quaternion.AngleAxis(-10, Vector3.forward) * Direction, dashAmount, LayerMask.GetMask("Room", "Enviroment"));
                if (raycastHit2D.collider)
                {
                    dashPosition = raycastHit2D.point;
                }
                else if (raycastHit2D02.collider)
                {
                    dashPosition = raycastHit2D02.point;
                }
                else if (raycastHit2D03.collider)
                {
                    dashPosition = raycastHit2D03.point;
                }
                Debug.Log(transform.position + "+" + dashPosition);
            }
        }
        if (colliders.Length == 0)
        {
            RaycastHit2D raycastHit2D = Physics2D.Raycast(transform.position, Direction, dashAmount, LayerMask.GetMask("Room", "Enviroment"));
            RaycastHit2D raycastHit2D02 = Physics2D.Raycast(transform.position, Quaternion.AngleAxis(10, Vector3.forward) * Direction, dashAmount, LayerMask.GetMask("Room", "Enviroment"));
            RaycastHit2D raycastHit2D03 = Physics2D.Raycast(transform.position, Quaternion.AngleAxis(-10, Vector3.forward) * Direction, dashAmount, LayerMask.GetMask("Room", "Enviroment"));
            if (raycastHit2D.collider)
            {
                dashPosition = raycastHit2D.point;
            }
            else if (raycastHit2D02.collider)
            {
                dashPosition = raycastHit2D02.point;
            }
            else if (raycastHit2D03.collider)
            {
                dashPosition = raycastHit2D03.point;
            }
        }
        StartCoroutine(DashMovement(dashPosition));
}
    void Update()
    {
        StartExistenceTimer();
        if (isDashing)
        {
            //设置玩家阴影
            timer -= Time.deltaTime;
            if (timer <= 0f)
            {
                timer = 0.02f;
                GameObject playershadow = Instantiate(PlayerShadow, player.transform.position, quaternion.identity);
                playershadow.GetComponent<SpriteRenderer>().sprite = playerTexture;
                Destroy(playershadow, 0.3f);
            }

            player.isInvincible = true;

            //对路径上的敌人造成伤害
            Collider2D[] colliders = Physics2D.OverlapCircleAll(player.transform.position, 1f);
            foreach (Collider2D collider in colliders)
            {
                if (collider.CompareTag("Empty"))
                {
                    Empty enemy = collider.GetComponent<Empty>();
                    if(enemy != null && !enemylist.Contains(enemy))
                    {
                        enemylist.Add(enemy);
                        HitAndKo(enemy);
                    }
                }
            }
        }
    }

    IEnumerator DashMovement(Vector3 targetPosition)
    {
        isDashing = true;
        float startTime = Time.time;
        float distance = Vector3.Distance(transform.position, targetPosition);
        float journeyLength = distance / dashSpeed;

        while (Time.time < startTime + journeyLength)
        {

            float distanceCovered = (Time.time - startTime) * dashSpeed;
            float fractionOfJourney = distanceCovered / distance;
          
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
            RaycastHit2D SearchED = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + CollidorOffset), Vector2.down, CollidorRadiusH + fractionOfJourney, LayerMask.GetMask("Enviroment", "Room", "Water"));
            RaycastHit2D SearchEU = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + CollidorOffset), Vector2.up, CollidorRadiusH + fractionOfJourney, LayerMask.GetMask("Enviroment", "Room", "Water"));
            RaycastHit2D SearchER = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + CollidorOffset), Vector2.right, CollidorRadiusV + fractionOfJourney, LayerMask.GetMask("Enviroment", "Room", "Water"));
            RaycastHit2D SearchEL = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + CollidorOffset), Vector2.left, CollidorRadiusV + fractionOfJourney, LayerMask.GetMask("Enviroment", "Room", "Water"));


            if ((SearchED.collider != null && (SearchED.transform.tag == "Enviroment" || SearchED.transform.tag == "Room" || SearchED.transform.tag == "Water"))
                || (SearchEU.collider != null && (SearchEU.transform.tag == "Enviroment" || SearchEU.transform.tag == "Room" || SearchEU.transform.tag == "Water"))
                || (SearchER.collider != null && (SearchER.transform.tag == "Enviroment" || SearchER.transform.tag == "Room" || SearchER.transform.tag == "Water"))
                || (SearchEL.collider != null && (SearchEL.transform.tag == "Enviroment" || SearchEL.transform.tag == "Room" || SearchEL.transform.tag == "Water"))) { }
            else
            {
                PlayerRigibody.position = Vector3.Lerp(transform.position, targetPosition, fractionOfJourney);
            }
            yield return null;
        }

        // 结束高速移动
        isDashing = false;
        player.isInvincible = false;
    }

    */
}
