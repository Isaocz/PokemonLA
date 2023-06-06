using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class poochyena : Empty
{

    public bool isEmptyU = true;
    public bool isEmptyD = true;
    public bool isEmptyL = true;
    public bool isEmptyR = true;
    bool isTurn;
    float TurnTimer = 2.0f;
    public bool isBiteAnimation;
    public bool isBite;
    float BiteTimer = 5.0f;
    Vector2 position;
    Vector2Int direction;
    Vector2 move;

    public GameObject BiteAnimation;


    // Start is called before the first frame update
    void Start()
    {
        
        isEmptyD = isEmptyL = isEmptyR = isEmptyU = true;
        switch (Random.Range(1, 5))
        {
            case 1:
                direction = new Vector2Int(1, 0);
                break;
            case 2:
                direction = new Vector2Int(-1, 0);
                break;
            case 3:
                direction = new Vector2Int(0, 1);
                break;
            case 4:
                direction = new Vector2Int(0, -1);
                break;
        }

        player = GameObject.FindWithTag("Player").GetComponent<PlayerControler>();
        Emptylevel = SetLevel(player.Level,30);
        EmptyType01 = 17;
        EmptyType02 = 0;
        
        EmptyHpForLevel(Emptylevel);
        AtkAbilityPoint = AbilityForLevel(Emptylevel, AtkEmptyPoint);
        SpAAbilityPoint = AbilityForLevel(Emptylevel, SpAEmptyPoint);
        DefAbilityPoint = AbilityForLevel(Emptylevel, DefEmptyPoint);
        SpdAbilityPoint = AbilityForLevel(Emptylevel, SpdEmptyPoint);
        SpeedAbilityPoint = AbilityForLevel(Emptylevel, SpeedEmptyPoint);
        Exp = BaseExp * Emptylevel / 7;

        //获取刚体目标 动画管理者目标 
        animator = GetComponent<Animator>();
        rigidbody2D = GetComponent<Rigidbody2D>();



    }

    // Update is called once per frame
    void Update()
    {
        if (!isBorn)
        {
            EmptyDie();
            StateMaterialChange();
            if (direction.x == 1) { animator.SetFloat("LookX", 1); }
            else if (direction.x == -1) { animator.SetFloat("LookX", 0); }

            if (!isTurn && !isBite && !isBite && !isSilence && !isBiteAnimation)
            {
                CheckTurn();
            }
            if (isTurn && !isSilence)
            {
                TurnTimer -= Time.deltaTime;
            }
            if (TurnTimer <= 0.0f)
            {
                isTurn = false;
                TurnTimer = 2.0f;
            }

            if (!isBite && !isSilence && !isBiteAnimation)
            {
                CheckPlayer();
            }
            if (isBite && !isSilence)
            {
                BiteTimer -= Time.deltaTime;
            }
            if (BiteTimer <= 0.0f)
            {
                isBite = false;
                BiteTimer = 5.0f;
            }
            if (isToxicDone) { EmptyToxic(); }
        }
        InvincibleUpdate();
    }

    private void FixedUpdate()
    {
        if (!isBorn){
            Vector2 NowPosition = position;
            if (isHit && isBiteAnimation)
            {
                isBiteAnimation = false;
            }
            if (!isDie && !isHit && !isSilence && !isBite && !isBiteAnimation)
            {
                //获取当前刚体坐标，当当前x坐标和初始x坐标距离小于设定好的移动距离时，缓慢移动 当大于时，重置初始位置，方向反转
                position = rigidbody2D.position;
                position.x = position.x + speed * direction.x * Time.deltaTime;
                position.y = position.y + speed * direction.y * Time.deltaTime;
                rigidbody2D.position = position;

            }
            if (!isDie && !isHit && !isSilence && isBite)
            {
                //咬
                position = rigidbody2D.position;
                position.x = position.x + 3 * speed * direction.x * Time.deltaTime;
                position.y = position.y + 3 * speed * direction.y * Time.deltaTime;
                rigidbody2D.position = position;

            }

            move = new Vector2(position.x - NowPosition.x, position.y - NowPosition.y);
            animator.SetFloat("Speed", move.magnitude);

            EmptyBeKnock();
        }
    }

    public void StartBite()
    {
        isBiteAnimation = false;
        isBite = true;
    }

    void CheckPlayer()
    {
        RaycastHit2D hitS = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + 0.5f), direction, 8f, LayerMask.GetMask("Player", "PlayerFly" , "Enviroment"));
        RaycastHit2D hitU = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + 0.5f), Vector2.up, 3f, LayerMask.GetMask("Player", "PlayerFly", "Enviroment"));
        RaycastHit2D hitD = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + 0.5f), Vector2.down, 3f, LayerMask.GetMask("Player", "PlayerFly", "Enviroment"));
        RaycastHit2D hitR = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + 0.5f), Vector2.right, 3f, LayerMask.GetMask("Player", "PlayerFly", "Enviroment"));
        RaycastHit2D hitL = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + 0.5f), Vector2.left, 3f, LayerMask.GetMask("Player", "PlayerFly", "Enviroment"));
        if (!isFearDone)
        {
            if (hitS.collider != null) {
                if (hitS.collider.gameObject.layer == 8 || hitS.collider.gameObject.layer == 17) {
                    isBiteAnimation = true; animator.SetTrigger("Bite");
                }
            }
            if (hitU.collider != null)
            {
                if (hitU.collider.gameObject.layer == 8 || hitU.collider.gameObject.layer == 17)
                {
                    isBiteAnimation = true; animator.SetTrigger("Bite"); direction = new Vector2Int(0, 1);
                }
            }

            if (hitD.collider != null ) {
                if (hitD.collider.gameObject.layer == 8 || hitD.collider.gameObject.layer == 17)
                {
                    isBiteAnimation = true; animator.SetTrigger("Bite"); direction = new Vector2Int(0, -1); 
                }
            }
            if (hitR.collider != null ) {
                if (hitR.collider.gameObject.layer == 8 || hitR.collider.gameObject.layer == 17) {
                    isBiteAnimation = true; animator.SetTrigger("Bite"); direction = new Vector2Int(1, 0);
                } 
            }
            if (hitL.collider != null) {
                if (hitL.collider.gameObject.layer == 8 || hitL.collider.gameObject.layer == 17) {       
                    isBiteAnimation = true; animator.SetTrigger("Bite"); direction = new Vector2Int(-1, 0);
                }
            }
        }
        else
        {
            if (hitS.collider != null) { isBite = true; direction = new Vector2Int(-direction.x, -direction.y); }
            if (hitU.collider != null) { isBite = true; direction = new Vector2Int(0, -1); }
            if (hitD.collider != null) { isBite = true; direction = new Vector2Int(0, 1); }
            if (hitR.collider != null) { isBite = true; direction = new Vector2Int(-1, 0); }
            if (hitL.collider != null) { isBite = true; direction = new Vector2Int(1, 0); }
        }
    }

    void CheckTurn()
    {
        if (direction.x == 0 && (isEmptyU && isEmptyD) && Random.Range(0.0f, 1.0f)<=0.3f)
        {
            switch (Random.Range(1,3))
            {
                case 1:
                    if (isEmptyL == true) { direction = new Vector2Int(-1, 0); isTurn = true; }
                    else if (isEmptyR == true) { direction = new Vector2Int(1, 0); isTurn = true; }
                    break;
                case 2:
                    if (isEmptyR == true) { direction = new Vector2Int(1, 0); isTurn = true; }
                    else if (isEmptyL == true) { direction = new Vector2Int(-1, 0); isTurn = true; }
                    break;
            }
        }
        else if (direction.x == 0 && !isEmptyU)
        {
            switch (Random.Range(1, 4))
            {

                case 1:
                    if      (isEmptyL == true) { direction = new Vector2Int(-1, 0); isTurn = true; }
                    else if (isEmptyR == true) { direction = new Vector2Int(1, 0); isTurn = true; }
                    else if (isEmptyD == true) { direction = new Vector2Int(0, -1); isTurn = true; }
                    break;
                case 2:
                    if (isEmptyR == true) { direction = new Vector2Int(1, 0); isTurn = true; }
                    else if (isEmptyD == true) { direction = new Vector2Int(0, -1); isTurn = true; }
                    else if (isEmptyL == true) { direction = new Vector2Int(-1, 0); isTurn = true; }
                    break;
                case 3:
                    if (isEmptyD == true) { direction = new Vector2Int(0, -1); isTurn = true; }
                    else if (isEmptyL == true) { direction = new Vector2Int(-1, 0); isTurn = true; }
                    else if (isEmptyR == true) { direction = new Vector2Int(1, 0); isTurn = true; }
                    break;
            }
        }
        else if (direction.x == 0 && !isEmptyD)
        {
            switch (Random.Range(1, 4))
            {
                case 1:
                    if (isEmptyL == true) { direction = new Vector2Int(-1, 0); isTurn = true; }
                    else if (isEmptyR == true) { direction = new Vector2Int(1, 0); isTurn = true; }
                    else if (isEmptyU == true) { direction = new Vector2Int(0, 1); isTurn = true; }
                    break;
                case 2:
                    if (isEmptyR == true) { direction = new Vector2Int(1, 0); isTurn = true; }
                    else if (isEmptyU == true) { direction = new Vector2Int(0, 1); isTurn = true; }
                    else if (isEmptyL == true) { direction = new Vector2Int(-1, 0); isTurn = true; }
                    break;
                case 3:
                    if (isEmptyU == true) { direction = new Vector2Int(0, 1); isTurn = true; }
                    else if (isEmptyL == true) { direction = new Vector2Int(-1, 0); isTurn = true; }
                    else if (isEmptyR == true) { direction = new Vector2Int(1, 0); isTurn = true; }
                    break;
            }
        }
        else if (direction.y == 0 && (isEmptyL && isEmptyR) && Random.Range(0.0f, 1.0f) <= 0.3f)
        {

            switch (Random.Range(1, 4))
            {
                case 1:
                    if (isEmptyD == true) { direction = new Vector2Int(0, -1); isTurn = true; }
                    else if (isEmptyU == true) { direction = new Vector2Int(0, 1); isTurn = true; }
                    break;
                case 2:
                    if (isEmptyU == true) { direction = new Vector2Int(0, 1); isTurn = true; }
                    else if (isEmptyD == true) { direction = new Vector2Int(0, -1); isTurn = true; }
                    break;
            }
        }
        else if(direction.y == 0 && !isEmptyL)
        {
            switch (Random.Range(1, 4))
            {
                case 1:
                    if (isEmptyD == true) { direction = new Vector2Int(0, -1); isTurn = true; }
                    else if (isEmptyR == true) { direction = new Vector2Int(1, 0); isTurn = true; }
                    else if (isEmptyU == true) { direction = new Vector2Int(0, 1); isTurn = true; }
                    break;
                case 2:
                    if (isEmptyR == true) { direction = new Vector2Int(1, 0); isTurn = true; }
                    else if (isEmptyU == true) { direction = new Vector2Int(0, 1); isTurn = true; }
                    else if (isEmptyD == true) { direction = new Vector2Int(0, -1); isTurn = true; }
                    break;
                case 3:
                    if (isEmptyU == true) { direction = new Vector2Int(0, 1); isTurn = true; }
                    else if (isEmptyD == true) { direction = new Vector2Int(0, -1); isTurn = true; }
                    else if (isEmptyR == true) { direction = new Vector2Int(1, 0); isTurn = true; }
                    break;
            }
        }
        else if (direction.y == 0 && !isEmptyR)
        {
            switch (Random.Range(1, 4))
            {
                case 1:
                    if (isEmptyD == true) { direction = new Vector2Int(0, -1); isTurn = true; }
                    else if (isEmptyL == true) { direction = new Vector2Int(-1, 0); isTurn = true; }
                    else if (isEmptyU == true) { direction = new Vector2Int(0, 1); isTurn = true; }
                    break;
                case 2:
                    if (isEmptyL == true) { direction = new Vector2Int(-1, 0); isTurn = true; }
                    else if (isEmptyU == true) { direction = new Vector2Int(0, 1); isTurn = true; }
                    else if (isEmptyD == true) { direction = new Vector2Int(0, -1); isTurn = true; }
                    break;
                case 3:
                    if (isEmptyU == true) { direction = new Vector2Int(0, 1); isTurn = true; }
                    else if (isEmptyD == true) { direction = new Vector2Int(0, -1); isTurn = true; }
                    else if (isEmptyL == true) { direction = new Vector2Int(-1, 0); isTurn = true; }
                    break;
            }
        }

    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.transform.tag == ("Player"))
        {
            if (!isBite)
            {
                EmptyTouchHit(other.gameObject);
            }
            else
            {
                PlayerControler playerControler = other.gameObject.GetComponent<PlayerControler>();
                if (playerControler != null)
                {
                    if (!player.isInvincible) { Instantiate(BiteAnimation, new Vector3(other.contacts[0].point.x, other.contacts[0].point.y, 0), Quaternion.identity, other.transform); }
                    playerControler.ChangeHp(-(75 * AtkAbilityPoint * (2 * Emptylevel + 10) / 250), 0, 17);
                    playerControler.KnockOutPoint = Knock;
                    playerControler.KnockOutDirection = (playerControler.transform.position - transform.position).normalized;

                }
            }
            
        }
    }
}
