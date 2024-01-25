using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zubat : Empty
{
    Vector2 Director;
    Vector2 TargetPosition;
    Vector2 LastPosition;



    float SonicTimer;
    Vector2 SonicDirector;
    bool isSonic;
    public ZubatSupersonic Sonic;





    // Start is called before the first frame update
    void Start()
    {
        EmptyType01 = Type.TypeEnum.Poison;
        EmptyType02 = Type.TypeEnum.Flying;
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
        LastPosition = transform.position;
    }


    // Update is called once per frame
    void Update()
    {
        ResetPlayer();
        if (!isBorn)
        {
            EmptyDie();
            UpdateEmptyChangeHP();
            StateMaterialChange();
        }
    }

    private void FixedUpdate()
    {

        ResetPlayer();
        if (isEmptyInfatuationDone) { UpdateInfatuationDmageCDTimer(); }
        if (!isBorn && !isDie)
        {
            EmptyBeKnock();
            if (!isEmptyFrozenDone && !isSleepDone && !isCanNotMoveWhenParalysis && !isSilence)
            {
                if (!isEmptyInfatuationDone || transform.parent.childCount <= 1 || InfatuationForDistanceEmpty() == null)
                {
                    TargetPosition = player.transform.position;
                    if (isSubsititue && SubsititueTarget != null) { TargetPosition = SubsititueTarget.transform.position; }
                }
                else { TargetPosition = InfatuationForDistanceEmpty().transform.position;Debug.Log(TargetPosition); }

                if (!isFearDone)
                {
                    Director = (TargetPosition - (Vector2)transform.position).normalized;
                    Director = (Quaternion.AngleAxis(isEmptyConfusionDone ? 0 : 30, Vector3.forward) * Director).normalized;
                    animator.SetFloat("LookX", (Director.x >= 0 ? 1 : -1));
                    animator.SetFloat("LookY", (Director.y >= 0 ? 1 : -1));
                    if (!isSonic) {
                        if ((TargetPosition - (Vector2)transform.position).magnitude >= (isEmptyInfatuationDone? 4 : 3.2f))
                        {
                            rigidbody2D.position = new Vector2(Mathf.Clamp(rigidbody2D.position.x + (float)Director.x * Time.deltaTime * speed, -15f + transform.parent.position.x, 15f + transform.parent.position.x), Mathf.Clamp(rigidbody2D.position.y + (float)Director.y * Time.deltaTime * speed, -10f + transform.parent.position.y, 10f + transform.parent.position.y));
                        }
                        else
                        {
                            RaycastHit2D SearchPlayer = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + 0.5f), new Vector2(TargetPosition.x - transform.position.x, TargetPosition.y - transform.position.y), 7f, LayerMask.GetMask("Player", "Enviroment", "Room", "PlayerFly"));
                            if (isEmptyInfatuationDone) { SearchPlayer = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + 0.5f), new Vector2(TargetPosition.x - transform.position.x, TargetPosition.y - transform.position.y), 7f, LayerMask.GetMask("Empty", "Enviroment", "Room", "EmptyFly")); }
                            if (SearchPlayer.collider != null && ((!isEmptyInfatuationDone && SearchPlayer.transform.tag == "Player") || (isEmptyInfatuationDone && SearchPlayer.transform.tag == "Empty")))
                            {
                                SonicDirector = (TargetPosition - (Vector2)transform.position).normalized;
                                animator.SetTrigger("Sonic");
                                isSonic = true;
                            }
                        }
                    }
                    else
                    {
                        SonicTimer += Time.deltaTime;
                        if (SonicTimer > 4.0f)
                        {
                            SonicTimer = 0;
                            isSonic = false;
                        }
                    }
                }
                else
                {
                    Director = (TargetPosition - (Vector2)transform.position).normalized;
                    Director = (Quaternion.AngleAxis(isEmptyConfusionDone ? 180 : 120, Vector3.forward) * Director).normalized;
                    animator.SetFloat("LookX", (Director.x >= 0 ? 1 : -1));
                    animator.SetFloat("LookY", (Director.y >= 0 ? 1 : -1));
                    if ((TargetPosition - (Vector2)transform.position).magnitude >= 2.5f)
                    {
                        rigidbody2D.position = new Vector2(Mathf.Clamp(rigidbody2D.position.x + (float)Director.x * Time.deltaTime * speed, -15f + transform.parent.position.x, 15f + transform.parent.position.x), Mathf.Clamp(rigidbody2D.position.y + (float)Director.y * Time.deltaTime * speed, -10f + transform.parent.position.y, 10f + transform.parent.position.y));
                    }
                    if (SonicTimer > 4.0f)
                    {
                        SonicTimer = 0;
                        isSonic = false;
                    }
                }
                



            }
            animator.SetFloat("Speed", ((Vector2)transform.position - LastPosition).magnitude);
            LastPosition = transform.position;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (!isEmptyInfatuationDone && other.transform.tag == ("Player"))
        {
            EmptyTouchHit(other.gameObject);

        }
        if (isEmptyInfatuationDone && other.transform.tag == ("Empty"))
        {
            InfatuationEmptyTouchHit(other.gameObject);
        }
    }

    public void SuperSonic()
    {
        if (!isDie)
        {
            Vector2 p = new Vector2((TargetPosition.x - transform.position.x), (TargetPosition.y - transform.position.y)).normalized;
            if (isEmptyConfusionDone)
            {
                p += new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)); p = p.normalized;
            }
            ZubatSupersonic s = Instantiate(Sonic, rigidbody2D.position + new Vector2(-0.1192138f , 0.904114f) + p * 0.7f, Quaternion.identity );
            s.transform.rotation = Quaternion.Euler(0, 0, (TargetPosition.y - transform.position.y <= 0 ? -1 : 1) * Vector2.Angle(p, new Vector2(1, 0)));
            s.ParentZubat = this;
            s.transform.GetChild(0).GetComponent<ZubatSupersonic>().ParentZubat = this;
            isSonic = true;
        }
    }



}
