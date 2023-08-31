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
    Vector2 ConfusionDirection;
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

        player = GameObject.FindObjectOfType<PlayerControler>();
        Emptylevel = SetLevel(player.Level, MaxLevel);
        EmptyType01 = Type.TypeEnum.Dark;
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
        ResetPlayer();
        if (!isBorn)
        {
            EmptyDie();
            StateMaterialChange();
            if (isEmptyInfatuationDone) { UpdateInfatuationDmageCDTimer(); }
            if (!isSleepDone && !isCanNotMoveWhenParalysis)
            {
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
            }
            UpdateEmptyChangeHP();
        }
    }

    private void FixedUpdate()
    {
        ResetPlayer();
        if (!isBorn){
            if (!isSleepDone && !isCanNotMoveWhenParalysis) { 
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
                    if (!isEmptyConfusionDone)
                    {
                        position.x = position.x + 3 * speed * direction.x * Time.deltaTime;
                        position.y = position.y + 3 * speed * direction.y * Time.deltaTime;
                    }
                    else
                    {
                        position.x = position.x + 3 * speed * ConfusionDirection.x * Time.deltaTime;
                        position.y = position.y + 3 * speed * ConfusionDirection.y * Time.deltaTime;
                    }
                    rigidbody2D.position = position;
                }
                move = new Vector2(position.x - NowPosition.x, position.y - NowPosition.y);
                animator.SetFloat("Speed", move.magnitude);
            }



            EmptyBeKnock();
        }
    }

    public void StartBite()
    {
        isBiteAnimation = false;
        isBite = true;
        ConfusionDirection = (direction + new Vector2(Random.Range(-0.25f, 0.25f), Random.Range(-0.25f, 0.25f))).normalized;
    }

    void CheckPlayer()
    {
        RaycastHit2D hitS;
        RaycastHit2D hitU;
        RaycastHit2D hitD;
        RaycastHit2D hitR;
        RaycastHit2D hitL;
        if (!isEmptyInfatuationDone || transform.parent.childCount <= 1)
        {
            hitS = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + 0.5f), direction, 8f, LayerMask.GetMask("Player", "PlayerFly", "Enviroment"));
            hitU = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + 0.5f), Vector2.up, 3f, LayerMask.GetMask("Player", "PlayerFly", "Enviroment"));
            hitD = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + 0.5f), Vector2.down, 3f, LayerMask.GetMask("Player", "PlayerFly", "Enviroment"));
            hitR = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + 0.5f), Vector2.right, 3f, LayerMask.GetMask("Player", "PlayerFly", "Enviroment"));
            hitL = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + 0.5f), Vector2.left, 3f, LayerMask.GetMask("Player", "PlayerFly", "Enviroment"));
        }
        else
        {
            hitS = Physics2D.Raycast( (new Vector2(transform.position.x, transform.position.y + 0.5f)) , direction, 8f, LayerMask.GetMask("Empty", "EmptyFly", "Enviroment") );
            hitU = Physics2D.Raycast(new Vector2(transform.position.x , transform.position.y + 0.5f), Vector2.up, 3f, LayerMask.GetMask("Empty", "EmptyFly", "Enviroment"));
            hitD = Physics2D.Raycast(new Vector2(transform.position.x , transform.position.y + 0.5f), Vector2.down, 3f, LayerMask.GetMask("Empty", "EmptyFly", "Enviroment"));
            hitR = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + 0.5f), Vector2.right, 3f, LayerMask.GetMask("Empty", "EmptyFly", "Enviroment"));
            hitL = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + 0.5f), Vector2.left, 3f, LayerMask.GetMask("Empty", "EmptyFly", "Enviroment"));
        }
        if (!isFearDone)
        {
            if (hitS.collider != null) {
                if (!isEmptyInfatuationDone || transform.parent.childCount <= 1)
                {
                    if (hitS.collider.gameObject.layer == 8 || hitS.collider.gameObject.layer == 17)
                    {
                        isBiteAnimation = true; animator.SetTrigger("Bite");
                    }
                }
                else
                {
                    if (hitS.collider.gameObject != gameObject && ( hitS.collider.gameObject.layer == 9 || hitS.collider.gameObject.layer == 16))
                    {
                        isBiteAnimation = true; animator.SetTrigger("Bite");
                    }
                }
            }
            if (hitU.collider != null)
            {
                if (!isEmptyInfatuationDone || transform.parent.childCount <= 1)
                {
                    if (hitU.collider.gameObject.layer == 8 || hitU.collider.gameObject.layer == 17)
                    {
                        isBiteAnimation = true; animator.SetTrigger("Bite"); direction = new Vector2Int(0, 1);
                    }
                }
                else
                {
                    if (hitU.collider.gameObject != gameObject && (hitU.collider.gameObject.layer == 9 || hitU.collider.gameObject.layer == 16))
                    {
                        isBiteAnimation = true; animator.SetTrigger("Bite"); direction = new Vector2Int(0, 1);
                    }
                }
            }

            if (hitD.collider != null ) {
                if (!isEmptyInfatuationDone || transform.parent.childCount <= 1)
                {
                    if (hitD.collider.gameObject.layer == 8 || hitD.collider.gameObject.layer == 17)
                    {
                        isBiteAnimation = true; animator.SetTrigger("Bite"); direction = new Vector2Int(0, -1);
                    }
                }
                else
                {
                    if (hitD.collider.gameObject != gameObject && (hitD.collider.gameObject.layer == 9 || hitD.collider.gameObject.layer == 16))
                    {
                        isBiteAnimation = true; animator.SetTrigger("Bite"); direction = new Vector2Int(0, -1);
                    }
                }
            }
            if (hitR.collider != null ) {
                if (!isEmptyInfatuationDone || transform.parent.childCount <= 1)
                {
                    if (hitR.collider.gameObject.layer == 8 || hitR.collider.gameObject.layer == 17)
                    {
                        isBiteAnimation = true; animator.SetTrigger("Bite"); direction = new Vector2Int(1, 0);
                    }
                }
                else
                {
                    if (hitR.collider.gameObject != gameObject && (hitR.collider.gameObject.layer == 9 || hitR.collider.gameObject.layer == 16))
                    {
                        isBiteAnimation = true; animator.SetTrigger("Bite"); direction = new Vector2Int(1, 0);
                    }
                }
            }
            if (hitL.collider != null) {
                if (!isEmptyInfatuationDone || transform.parent.childCount <= 1)
                {
                    if (hitL.collider.gameObject.layer == 8 || hitL.collider.gameObject.layer == 17)
                    {
                        isBiteAnimation = true; animator.SetTrigger("Bite"); direction = new Vector2Int(-1, 0);
                    }
                }
                else
                {
                    if (hitL.collider.gameObject != gameObject && (hitL.collider.gameObject.layer == 9 || hitL.collider.gameObject.layer == 16))
                    {
                        isBiteAnimation = true; animator.SetTrigger("Bite"); direction = new Vector2Int(-1, 0);
                    }
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
        if (other.transform.tag == ("Player") || (isEmptyInfatuationDone && other.gameObject.tag == ("Empty")))
        {
            if (!isBite)
            {

                if (isEmptyInfatuationDone && other.transform.tag == ("Empty"))
                {
                    if (other.gameObject.GetComponent<Empty>() != null) {
                        InfatuationEmptyTouchHit(other.gameObject);
                    }
                }
                else if (!isEmptyInfatuationDone && other.transform.tag == ("Player"))
                {
                    if (other.gameObject.GetComponent<PlayerControler>() != null)
                    {
                        EmptyTouchHit(other.gameObject);
                    }
                }
            }
            else
            {

                if (isEmptyInfatuationDone && other.transform.tag == ("Empty"))
                {
                    Empty e = other.gameObject.GetComponent<Empty>();
                    if (e != null)
                    {
                        if (!isInfatuationDmageDone)
                        {
                            Instantiate(BiteAnimation, new Vector3(other.contacts[0].point.x, other.contacts[0].point.y, 0), Quaternion.identity, other.transform);

                            Pokemon.PokemonHpChange(this.gameObject , e.gameObject , 60 , 0 , 0 , Type.TypeEnum.Dark );
                            //e.EmptyHpChange((60 * AtkAbilityPoint * (2 * Emptylevel + 10) / (250 * e.SpdAbilityPoint * ((Weather.GlobalWeather.isHail ? ((e.EmptyType01 == Type.TypeEnum.Ice || e.EmptyType02 == Type.TypeEnum.Ice) ? 1.5f : 1) : 1))) + 2), 0, 17);
                            isInfatuationDmageDone = true;
                        }
                    }
                }
                else if(!isEmptyInfatuationDone && other.transform.tag == ("Player"))
                {
                    PlayerControler playerControler = other.gameObject.GetComponent<PlayerControler>();
                    
                    if (playerControler != null)
                    {
                        if (!player.isInvincible) { Instantiate(BiteAnimation, new Vector3(other.contacts[0].point.x, other.contacts[0].point.y, 0), Quaternion.identity, other.transform); }
                        playerControler.KnockOutPoint = Knock;
                        playerControler.KnockOutDirection = (playerControler.transform.position - transform.position).normalized;
                    }
                    Pokemon.PokemonHpChange(this.gameObject, other.gameObject, 75, 0, 0, Type.TypeEnum.Dark);
                }
            }
            
        }
    }
}
