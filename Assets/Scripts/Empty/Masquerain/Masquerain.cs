using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Masquerain : Empty
{
    public enum FACE_TO
    {
        FL,
        FR,
        BL,
        BR,
    }
    
    private enum AI_STATE
    {
        IDLE,
        PATROL,
        CHASE,
        ATK,
    }


    private AI_STATE aiState;
    private float preBodyScale;
    
    public FACE_TO faceTo;
    //[Label("观察半径")]
    public float foundRadius = 8;
    public Transform body;
    
    void Start()
    {
        speed = 0f;
        player = GameObject.FindObjectOfType<PlayerControler>();
        Emptylevel = SetLevel(player.Level, 30);
        EmptyHpForLevel(Emptylevel);
        AtkAbilityPoint = AbilityForLevel(Emptylevel, AtkEmptyPoint);
        SpAAbilityPoint = AbilityForLevel(Emptylevel, SpAEmptyPoint);
        DefAbilityPoint = AbilityForLevel(Emptylevel, DefEmptyPoint);
        SpdAbilityPoint = AbilityForLevel(Emptylevel, SpdEmptyPoint);
        SpeedAbilityPoint = AbilityForLevel(Emptylevel, SpeedEmptyPoint);
        Exp = BaseExp * Emptylevel / 7;
        animator = GetComponent<Animator>();
        rigidbody2D = GetComponent<Rigidbody2D>();

        preBodyScale = body.localScale.x;

        // Timer.Start(this, 5, () =>
        // {
        //     animator.SetTrigger("Hit");
        // });
        //
        // Timer.Start(this, 8, () =>
        // {
        //     animator.SetTrigger("Die");
        // });

        // ChangeFaceTo(FACE_TO.BL);
    }

    void Update()
    {
        ResetPlayer();
        if (isEmptyInfatuationDone) { UpdateInfatuationDmageCDTimer(); }
        if (!isBorn)
        {
            EmptyDie();
            StateMaterialChange();
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

    public void ChangeFaceTo(FACE_TO ft)
    {
        faceTo = ft;
        Vector3 scale = body.localScale;
        if (ft == FACE_TO.FL || ft == FACE_TO.FR)
        {
            animator.SetBool("bFaceFront", true);
            scale.x = (ft == FACE_TO.FL ? 1 : -1) * preBodyScale;
        }else if (ft == FACE_TO.BL || ft == FACE_TO.BR)
        {
            animator.SetBool("bFaceFront", false);
            scale.x = (ft == FACE_TO.BR ? 1 : -1) * preBodyScale;
        }
        body.localScale = scale;
    }
}
