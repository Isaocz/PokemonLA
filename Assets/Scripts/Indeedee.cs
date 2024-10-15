using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Indeedee : Empty
{

    Vector2 Director;
    List<Empty> ProtectList = new List<Empty> { };
    float AnimationTimer;
    GameObject ProtectAnimation;
    int ThisRoomIndeedeeCount;


    // Start is called before the first frame update
    void Start()
    {
        EmptyType01 = PokemonType.TypeEnum.Fairy;
        EmptyType02 = PokemonType.TypeEnum.Normal;
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

        ProtectAnimation = PublicEffect.StaticPublicEffectList.ReturnAPublicEffect(1);
        Invoke("ProtectOtherEmpty" , 0.01f);
    }


    void ProtectOtherEmpty()
    {
        int ChildCount = 0;
        for(int i = 0; i < transform.parent.childCount; i++)
        {
            Empty ChildEmpty = transform.parent.GetChild(i).GetComponent<Empty>();
            if (ChildEmpty != null ) {
                ChildCount++;
                if (ChildEmpty.gameObject != gameObject && ChildEmpty.GetComponent<Indeedee>() == null)
                {
                    ProtectList.Add(ChildEmpty);
                    Instantiate(ProtectAnimation, ChildEmpty.transform.position, Quaternion.identity, ChildEmpty.transform).SetActive(true);
                    ChildEmpty.Invincible = true;
                }
            }
        }
        if (ThisRoomIndeedeeCount == 0)
        {
            ThisRoomIndeedeeCount = ChildCount - ProtectList.Count;
        }
    }

    void RemoveProtect()
    {
        bool isAnotherIndedee = false;
        for (int i = 0; i < transform.parent.childCount; i++)
        {
            if (transform.parent.GetChild(i).gameObject != gameObject) {
                isAnotherIndedee = transform.parent.GetChild(i).GetComponent<Indeedee>();
                if (isAnotherIndedee) { break; }
            }
        }

        if (!isAnotherIndedee) {
            for (int i = 0; i < ProtectList.Count; i++)
            {
                ProtectList[i].Invincible = false;
            }
            ProtectList.Clear();
        }
    }

    private void OnDestroy()
    {
        RemoveProtect();
    }

    // Update is called once per frame
    void Update()
    {
        ResetPlayer();
        if (!isBorn)
        {
            EmptyDie();
            UpdateEmptyChangeHP();
            if(transform.parent.childCount > ProtectList.Count + ThisRoomIndeedeeCount)
            {
                ProtectOtherEmpty();
            }
            AnimationTimer += Time.deltaTime;
            if (AnimationTimer >= 3.5f)
            {
                for (int i = 0; i < ProtectList.Count; i++)
                {
                    SpriteRenderer S = ProtectList[i].GetSkinRenderers()[0].GetComponent<SpriteRenderer>();
                    if (S.color.a >= 0.1f) {
                        Instantiate(ProtectAnimation, ProtectList[i].transform.position, Quaternion.identity, ProtectList[i].transform).SetActive(true);
                    }
                }
                AnimationTimer = 0;
            }
        }
    }

    private void FixedUpdate()
    {

        ResetPlayer();
        if (isEmptyInfatuationDone) { UpdateInfatuationDmageCDTimer(); }
        if (!isBorn)
        {
            EmptyBeKnock();
            StateMaterialChange();

        }
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

