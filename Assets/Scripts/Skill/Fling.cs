using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fling : Skill
{
    public GameObject TackleBlast;
    SpaceItem FlingItem;
    SpriteRenderer S;
    Vector2 direction;
    Vector3 StartPostion;
    bool isCanNotMove;

    public HealthUpCCg DropPosion;

    // Start is called before the first frame update
    void Start()
    {
        if (SkillFrom != 2 )  { if (player.spaceItem == null) { Destroy(gameObject); } }
        animator = GetComponent<Animator>();
        direction = (transform.rotation * Vector2.right).normalized;
        StartPostion = transform.position;
        transform.rotation = Quaternion.Euler(0, 0, 0);
        if (SkillFrom != 2 || (SkillFrom == 2 && player.spaceItem != null)) {
            FlingItem = player.spaceItem.GetComponent<SpaceItem>();
            Damage = FlingItem.FlingDamage;
            S = transform.GetChild(0).GetComponent<SpriteRenderer>();
            S.sprite = player.spaceItem.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite;
            float SpriteOffset = (float)(S.sprite.pivot.y) / (float)(S.sprite.rect.height);
            transform.GetChild(0).localPosition = new Vector3(0, -SpriteOffset, 0);
            player.spaceItem = null;
            player.SpaceItemImage.color = new Color(0, 0, 0, 0);
            player.SpaceItemImage.sprite = null;
        }
        if (SkillFrom == 2 && FlingItem == null)
        {
            Damage = 40;
            Pokemon.PokemonHpChange(null,player.gameObject, Mathf.Clamp( Mathf.Ceil(12 +  player.maxHp * 0.06f) , 1 , player.Hp - 1  ), 0 , 0 , PokemonType.TypeEnum.IgnoreType);
            player.KnockOutDirection = Vector2.right; player.KnockOutPoint = 0;
        }
    }


    private void Update()
    {
        StartExistenceTimer();
    }

    private void FixedUpdate()
    {
        if (!isCanNotMove)
        {
            Vector3 postion = transform.position;
            postion.x += direction.x * 9f * Time.deltaTime;
            postion.y += direction.y * 9f * Time.deltaTime;
            transform.position = postion;
            transform.rotation = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z + 360 * Time.deltaTime);
            if ((StartPostion - transform.position).magnitude > MaxRange)
            {
                ItemDrop();

            }
        }
    }

    void ItemDrop()
    {
        if (!isCanNotMove)
        {
            transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);
            transform.GetComponent<Collider2D>().enabled = false;
            isCanNotMove = true;
            if (FlingItem != null) {
                Instantiate(FlingItem, transform.position, Quaternion.identity).isLunch = true;
            }
            else
            {
                Instantiate(DropPosion, transform.position, Quaternion.identity).isLunch = true;
            }
        }
    }

    void ItemBreak()
    {
        if (!isCanNotMove)
        {
            transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);
            transform.GetComponent<Collider2D>().enabled = false;
            isCanNotMove = true;
            Instantiate(TackleBlast, transform.position, Quaternion.identity);
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.isTrigger == false)
        {
            if (other.tag == "Empty")
            {
                Empty target = other.GetComponent<Empty>();
                if (target != null)
                {
                    HitAndKo(target);
                }

                if (Random.Range(0.0f, 1.0f) + ((float)player.LuckPoint / 30) > 0.4f)
                {
                    ItemDrop();
                }
                else { ItemBreak(); }

            }
            else if (other.tag == "Room" || other.tag == "Enviroment")
            {
                ItemDrop();
            }
        }
    }
}
