using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnixBodyShadow : SubEmptyBody
{
    bool isDSpriteChange;
    GameObject Shadow;
    float Y;
    public CircleCollider2D BodyCollider2D;
    SpriteRenderer BodySprite;
    Onix ParentOnix;

    public bool isTopHead;
    Vector3 LastPosition;
    public Sprite BSprite;
    public Sprite SSprite;

    public bool isCanInsideWall;
    public OnixDigAnimatorPause DigEffect;

    bool isPositionMove;
    float MoveFloatTimer;


    private void Awake()
    {
        BodySprite = transform.GetChild(1).GetComponent<SpriteRenderer>();
        BodyCollider2D = GetComponent<CircleCollider2D>();
    }

    // Start is called before the first frame update
    void Start()
    {

        ParentOnix = ParentEmpty.GetComponent<Onix>();
        SubEmptyBodyStart();
        Shadow = transform.GetChild(0).gameObject;
        Y = Shadow.transform.localPosition.y;
        LastPosition = transform.position;


        /*
        if (AnchorsSubBody != null)
        {
            float Dis = AnchorsSubBody.GetComponent<CircleCollider2D>().radius + GetComponent<CircleCollider2D>().radius + 0.1f;
            AnchorsDisMax = Dis + 0.1f;
            AnchorsDisMin = Dis - 0.1f;
        }
        */
    }

    // Update is called once per frame
    void Update()
    {
        SubEmptyBodyUpdate();
        if (Shadow.transform.rotation.eulerAngles.z != 0 || Shadow.transform.position != transform.position + Vector3.up * Y)
        {
            Shadow.transform.rotation = Quaternion.Euler(Vector3.zero);
            Shadow.transform.position = transform.position + Vector3.up * Y;

        }

        if (BodySprite.transform.rotation.eulerAngles.z != 0)
        {
            BodySprite.transform.rotation = Quaternion.Euler(Vector3.zero);
        }


    }


    private void FixedUpdate()
    {
        SubEmptyBodyFixedUpdate();
        if (!ParentEmpty.isDie && !ParentEmpty.isBorn) {

            if (!ParentOnix.isEmptyFrozenDone && !ParentOnix.isSleepDone && !ParentOnix.isCanNotMoveWhenParalysis && !ParentOnix.isSilence) {
                if (!isTopHead)
                {
                    if (!isDSpriteChange) {
                        Vector2 D = (transform.position - LastPosition).normalized;
                        Vector2Int dir = new Vector2Int(1, 1);
                        if (D.x >= 0) { dir.x = 1; } else { dir.x = -1; }
                        if (D.y >= 0) { dir.y = 1; } else { dir.y = -1; }
                        if (dir == new Vector2Int(1, 1))
                        {
                            BodySprite.sprite = BSprite; BodySprite.flipX = false;
                        }
                        else if (dir == new Vector2Int(-1, 1))
                        {
                            BodySprite.sprite = BSprite; BodySprite.flipX = true;
                        }
                        else if (dir == new Vector2Int(1, -1))
                        {
                            BodySprite.sprite = SSprite; BodySprite.flipX = true;
                        }
                        else
                        {
                            BodySprite.sprite = SSprite; BodySprite.flipX = false;
                        }
                        LastPosition = transform.position;
                    }
                }
                else
                {
                    /*
                    if (ParentEmpty.animator.GetFloat("LookY") == 1) 
                    {
                        BodySprite.transform.position = transform.position - 0.15f*Vector3.down;
                        if (ParentEmpty.animator.GetFloat("LookX") == 1)
                        {
                            BodySprite.transform.position = transform.position + 0.5f * Vector3.right;
                        }
                        else { BodySprite.transform.position = transform.position - 0.5f * Vector3.right; }
                    }
                    else { BodySprite.transform.position = transform.position;  }
                    */
                }
                if (ParentOnix.NowState == Onix.State.BodyPress)
                {
                    transform.position = new Vector3(Mathf.Clamp(transform.position.x, ParentOnix.transform.parent.position.x - 12.5f, ParentOnix.transform.parent.position.x + 12.5f),
                        Mathf.Clamp(transform.position.y, ParentOnix.transform.parent.position.y - 7.2f, ParentOnix.transform.parent.position.y + 7.2f), 0);

                    if (isPositionMove)
                    {

                        MoveFloatTimer += Time.deltaTime;
                        if (MoveFloatTimer <= 0.1667f) { }
                        else if (MoveFloatTimer > 0.1667f && MoveFloatTimer <= 0.5f) { BodySprite.transform.position += Vector3.up * 20.0f * Time.deltaTime; }
                        else if (MoveFloatTimer > 0.5 && MoveFloatTimer <= 0.8333f) { BodySprite.transform.position -= Vector3.up * 22.0f * Time.deltaTime; }
                        else if (MoveFloatTimer > 0.8333f && MoveFloatTimer <= 3.5f) { BodySprite.transform.position = transform.position + Vector3.down * 0.45f; }
                        else if (MoveFloatTimer > 9.0f) { ParentOnix.OutBodyPressState(); }
                    }
                }
            }
        }
    }


    public void JumpStart() { MoveFloatTimer = 0; isPositionMove = true; BodySprite.transform.localPosition = Vector3.zero; isDSpriteChange = true;rigidbody2D.bodyType = RigidbodyType2D.Kinematic; }
    public void JumpOver() { MoveFloatTimer = 0; isPositionMove = false; BodySprite.transform.localPosition = Vector3.zero ; isDSpriteChange = false; rigidbody2D.bodyType = RigidbodyType2D.Dynamic; }



    //忽略碰撞体和墙的碰撞关系
    public void IgnoreBodyAndWallXollision() { }
    //恢复碰撞体和墙的碰撞关系
    public void NotIgnoreBodyAndWallXollision() { }


    public void ResetMaskMode()
    {
        BodySprite.maskInteraction = SpriteMaskInteraction.None;
        Shadow.GetComponent<SpriteRenderer>().maskInteraction = SpriteMaskInteraction.None;
    }


    private void OnCollisionEnter2D(Collision2D other)
    {
        if (isCanInsideWall) 
        {
            if (other.transform.tag == ("Room"))
            {
                Physics2D.IgnoreCollision( BodyCollider2D , other.collider , true);
                if (BodySprite.maskInteraction == SpriteMaskInteraction.None) {
                    
                    if (isTopHead) {
                        ParentOnix.CameraShake(3.5f, 3.0f, true);
                        ParentOnix.isCanHitAnimation = true; ;
                        ParentOnix.FallARockTomb();
                        ParentOnix.BornStoneEdge();
                        ParentOnix.ReserSubBosy();
                        Vector2 c = (other.contacts[0].point - (Vector2)transform.position).normalized ;
                        int angle = 0;
                        if (Mathf.Abs(c.x) >= Mathf.Abs(c.y)) { if (c.x > 0) { angle = 90; } else { angle = -90; } }
                        else { if (c.y > 0) { angle = 180; } else { angle = 0; } }
                        if (angle == 0) { ParentOnix.OnixDigEffect = Instantiate(DigEffect, transform.position + Vector3.up * 1.2f * -(ParentOnix.Director.y) + Vector3.left * ((ParentOnix.Director.x >= 0) ? 2.3f : 1.9f) * -(ParentOnix.Director.x), Quaternion.Euler(0, 0, angle)); }
                        else if (angle == 90) { ParentOnix.OnixDigEffect = Instantiate(DigEffect, transform.position + Vector3.up * ((ParentOnix.Director.y >= 0) ? -2.0f : -1.3f) * -(ParentOnix.Director.y) + Vector3.left * -1.0f * -(ParentOnix.Director.x) , Quaternion.Euler(0, 0, angle)); }
                        else if (angle == -90) { ParentOnix.OnixDigEffect = Instantiate(DigEffect, transform.position + Vector3.up * ((ParentOnix.Director.y >= 0) ? -2.0f : -2.0f) * -(ParentOnix.Director.y) + Vector3.left * -1.0f * -(ParentOnix.Director.x) , Quaternion.Euler(0, 0, angle)); }
                        else if (angle == 180) { ParentOnix.OnixDigEffect = Instantiate(DigEffect, transform.position + Vector3.up * 0.8f * -(ParentOnix.Director.y) + Vector3.left * ((ParentOnix.Director.x >= 0) ? 2.3f : 1.9f) * -(ParentOnix.Director.x), Quaternion.Euler(0, 0, angle)); ParentOnix.OnixDigEffect.GetComponent<SpriteRenderer>().flipX = true; ParentOnix.OnixDigEffect.transform.GetChild(0).GetComponent<SpriteRenderer>().flipX = true; }
                        ParentOnix.OnixDigEffect.GetComponent<Animator>().SetTrigger("Start");
                        ParentOnix.OnixDigEffect.empty = ParentOnix;
                        foreach (OnixBodyShadow b in ParentOnix.SubEmptyBodyList)
                        {
                            b.BodySprite.maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
                            b.Shadow.GetComponent<SpriteRenderer>().maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
                        }
                    }
                }
                if (!ParentOnix.IgnoreColliderList.Contains(other.collider)) { ParentEmpty.GetComponent<Onix>().IgnoreColliderList.Add(other.collider); }
            }
        }
        if (other.transform.tag == ("Player"))
        {
            if (isTopHead && ParentOnix.isIronHead) 
            {
                PlayerControler p = other.gameObject.GetComponent<PlayerControler>();
                Pokemon.PokemonHpChange(ParentOnix.gameObject, other.gameObject, 80, 0, 0, Type.TypeEnum.Steel);
                if (p != null)
                {
                    Debug.Log(p);
                    p.KnockOutPoint = 5;
                    p.KnockOutDirection = (p.transform.position - transform.position).normalized;
                }
            }
            else if ( ParentOnix.isBodyPressDamage)
            {
                PlayerControler playerControler = other.gameObject.GetComponent<PlayerControler>();
                Pokemon.PokemonHpChange(ParentOnix.gameObject, other.gameObject, 80, 0, 0, Type.TypeEnum.Fighting, Pokemon.SpecialAttackTypes.BodyPress);
                if (playerControler != null)
                {
                    playerControler.KnockOutPoint = 9f;
                    playerControler.KnockOutDirection = (playerControler.transform.position - transform.position).normalized;
                }
            }
            else
            {
                ParentEmpty.EmptyTouchHit(other.gameObject);
            }
        }
        if (other.transform.tag == ("Enviroment") || other.gameObject.GetComponent<BossDoorBillboard>()) { Physics2D.IgnoreCollision(BodyCollider2D, other.collider, true); }
    }



}
