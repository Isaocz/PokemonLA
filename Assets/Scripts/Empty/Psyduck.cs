using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Psyduck : Empty
{

    bool isAttak;
    float AttackTimer=0;
    //float direction;
    Vector2 TargetPosition;
    public Projectile WaterGun;
    public Projectile Confusion;

    // Start is called before the first frame update
    void Start()
    {
        speed = 0f;
        EmptyType01 = Type.TypeEnum.Water;
        EmptyType02 = 0;
        player = GameObject.FindObjectOfType<PlayerControler>();
        Emptylevel = SetLevel(player.Level, MaxLevel);
        EmptyHpForLevel(Emptylevel);
        AtkAbilityPoint = AbilityForLevel(Emptylevel, AtkEmptyPoint);
        SpAAbilityPoint = AbilityForLevel(Emptylevel, SpAEmptyPoint);
        DefAbilityPoint = AbilityForLevel(Emptylevel, DefEmptyPoint);
        SpdAbilityPoint = AbilityForLevel(Emptylevel, SpdEmptyPoint);
        SpeedAbilityPoint = AbilityForLevel(Emptylevel, SpeedEmptyPoint);
        Exp = BaseExp * Emptylevel / 7;


        //获取刚体目标 动画管理者目标 并让刚体的初始x坐标带入FirstX中
        animator = GetComponent<Animator>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        
    }

    // Update is called once per frame
    void Update()
    {
        ResetPlayer();
        if (isEmptyInfatuationDone) { UpdateInfatuationDmageCDTimer(); }
        if (!isBorn)
        {
            EmptyDie();
            StateMaterialChange();
            if (!isSleepDone && !isCanNotMoveWhenParalysis) {
                if (!isDie && !isSilence && !isAttak && !isFearDone)
                {
                    Attak();
                }
                if (isAttak && !isSilence && !isFearDone)
                {
                    AttackTimer += Time.deltaTime;
                }
                if (AttackTimer >= 3.0f)
                {
                    isAttak = false;
                    AttackTimer = 0;
                }
            }
            if (!isEmptyInfatuationDone || transform.parent.childCount <= 1 || InfatuationForRangeRayCastEmpty(8) == null) { 
                TargetPosition = player.transform.position;
                if (isSubsititue && SubsititueTarget != null) { TargetPosition = SubsititueTarget.transform.position; }
            }
            else { TargetPosition = InfatuationForRangeRayCastEmpty(8).transform.position; }
            UpdateEmptyChangeHP();
        }

    }

    private void FixedUpdate()
    {
        ResetPlayer();
        if (!isBorn)
        {
            EmptyBeKnock();
        }
    }

    public void Lunch()
    {
        if (!isDie) {
            Projectile ProjectileObject;
            Projectile Shot;
            if (Random.Range(0.0f, 1.0f) <= 0.5f)
            {
                Shot = WaterGun;
            }
            else
            {
                Shot = Confusion;
            }
            Vector2 p = new Vector2((TargetPosition.x - transform.position.x), (TargetPosition.y - transform.position.y)).normalized;
            if (isEmptyConfusionDone) { Debug.Log("xxx"); p += new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)); p = p.normalized; }

            ProjectileObject = Instantiate(Shot, rigidbody2D.position + new Vector2(0, 0.375f) + p*0.375f , Quaternion.Euler(0, 0, (TargetPosition.x - transform.position.x >= 0 ? -1 : 1) * Vector2.Angle(TargetPosition - (Vector2)transform.position, new Vector2(0, 1))));
            ProjectileObject.transform.rotation = Quaternion.Euler(0, 0, (TargetPosition.x - transform.position.x >= 0 ? -1 : 1) * Vector2.Angle(p, new Vector2(0, 1)));
            ProjectileObject.Launch(p * 6, 55);
            ProjectileObject.empty = gameObject.GetComponent<Empty>();
        }
    }

    void Attak()
    {
        if (!isEmptyInfatuationDone || transform.parent.childCount <= 1 || InfatuationForRangeRayCastEmpty(8) == null)
        {
            RaycastHit2D SearchPlayer = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + 0.5f), new Vector2(TargetPosition.x - transform.position.x, TargetPosition.y - transform.position.y), 8f, LayerMask.GetMask("Player", "Enviroment", "Room", "PlayerFly"));
            if (SearchPlayer.collider != null && SearchPlayer.transform.tag == "Player")
            {
                Turn();
                animator.SetTrigger("Attak");
                isAttak = true;
            }
        }
        else
        {
            Turn();
            animator.SetTrigger("Attak");
            isAttak = true;
        }
    }

    void Turn()
    {
        if(TargetPosition.x - transform.position.x >= 0) { animator.SetFloat("LookX", 1);/*direction = 1;*/ }
        else{ animator.SetFloat("LookX", 0);/*direction = -1;*/ }
    }


    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.tag == ("Player"))
        {
            EmptyTouchHit(other.gameObject);
        }
        if (isEmptyInfatuationDone && other.transform.tag == ("Empty"))
        {
            InfatuationEmptyTouchHit(other.gameObject);
        }
    }
}
