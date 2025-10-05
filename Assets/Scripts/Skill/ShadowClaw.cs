using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowClaw : Skill
{
    bool isFly;
    bool isClawOver;

    float MoveShadowTimer;
    public KrickeyunrMoveShadow MoveShadow;
    SpriteRenderer PlayerSprite;

    // Start is called before the first frame update
    void Start()
    {
        animator = transform.GetComponent<Animator>();
        PlayerSprite = player.transform.GetChild(3).GetChild(0).GetComponent<SpriteRenderer>();
        if (transform.rotation.eulerAngles.z == 180) { transform.localScale = new Vector3(-1.3f ,1.3f ,1); }
        transform.rotation = new Quaternion(0, 0, 0, 0);
        GameObject h = transform.GetChild(3).gameObject;
        h.transform.parent = null;
        h.transform.localScale = new Vector3(1,1,1);
        GameObject h1 = transform.GetChild(3).gameObject;
        h1.transform.parent = null;
        h1.transform.localScale = new Vector3(1, 1, 1);

        if (SkillFrom == 2)
        {
            player.playerData.MoveSpwBounsAlways += 2;
            player.ReFreshAbllityPoint();
            if (!isFly && player.gameObject.layer != LayerMask.NameToLayer("PlayerFly"))
            {
                isFly = true;
                player.gameObject.layer = LayerMask.NameToLayer("PlayerFly");
            }
        }
    }

    void Update()
    {
        StartExistenceTimer();
        if (ExistenceTime <= 0.65f && !isClawOver)
        {
            isClawOver = true;
            animator.enabled = false;
            transform.GetComponent<Collider2D>().enabled = false;
        }

        if (SkillFrom == 2) {
            MoveShadowTimer += Time.deltaTime;
            if (MoveShadowTimer >= 0.08f) {
                MoveShadowTimer = 0;
                Instantiate(MoveShadow, player.transform.position, Quaternion.identity).sprite01.sprite = PlayerSprite.sprite;
            }
        }
    }


    //当飞弹与目标碰撞时，如果目标时敌人，获取敌人的血量，并使敌人扣血
    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.tag == "Empty")
        {
            Empty target = other.GetComponent<Empty>();
            if (target != null)
            {
                HitAndKo(target);
            }
        }
    }

    private void OnDestroy()
    {
        if (SkillFrom == 2)
        {
            player.playerData.MoveSpwBounsAlways -= 2;
            player.ReFreshAbllityPoint();
            if (isFly && player.gameObject.layer == LayerMask.NameToLayer("PlayerFly"))
            {
                isFly = false;
                player.gameObject.layer = LayerMask.NameToLayer("Player");
            }
        }
    }

}
