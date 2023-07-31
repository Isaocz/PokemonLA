using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockBlastRock : Skill
{
    public Vector3 StartPostion;
    public Vector2 direction;
    bool isCanNotMove;
    RockBlast ParentRB;
    SpriteRenderer spriteRenderer;

    public PlusSubRockBlast SubRockBlast;

    private void Start()
    {
        ParentRB = transform.parent.GetComponent<RockBlast>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        transform.position = StartPostion;
    }


    private void FixedUpdate()
    {
        if (!isCanNotMove)
        {
            Vector3 postion = transform.position;
            postion.x += direction.x * 7f * Time.deltaTime;
            postion.y += direction.y * 7f * Time.deltaTime;
            transform.position = postion;
            if ((StartPostion - transform.position).magnitude > MaxRange)
            {
                isCanNotMove = true;
                transform.GetChild(0).gameObject.SetActive(true);
                transform.GetChild(1).gameObject.SetActive(true);
                spriteRenderer.color = new Color(0, 0, 0, 0);
            }
        }
    }

    private void Update()
    {
        if (transform.childCount == 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Empty")
        {
            Empty target = other.GetComponent<Empty>();
            if (target != null)
            {
                //Debug.Log(player);
                HitAndKo(target);
            }
            isCanNotMove = true;
            transform.GetChild(0).gameObject.SetActive(true);
            transform.GetChild(1).gameObject.SetActive(true);
            spriteRenderer.color = new Color(0, 0, 0, 0);
        }
        else if (other.tag == "Room" || other.tag == "Enviroment")
        {
            if (SkillFrom == 2)
            {
                SkillBreakabelStone stone = other.GetComponent<SkillBreakabelStone>();
                if (stone != null && !stone.isBreak)
                {
                    stone.RockBreak();
                    Instantiate(SubRockBlast , transform.position+Vector3.right , Quaternion.Euler(0,0,0));
                    Instantiate(SubRockBlast , transform.position+Vector3.up , Quaternion.Euler(0,0,90));
                    Instantiate(SubRockBlast , transform.position+Vector3.left , Quaternion.Euler(0,0,180));
                    Instantiate(SubRockBlast , transform.position+Vector3.down , Quaternion.Euler(0,0,270));
                }
            }

            isCanNotMove = true;
            transform.GetChild(0).gameObject.SetActive(true);
            transform.GetChild(1).gameObject.SetActive(true);
            spriteRenderer.color = new Color(0, 0, 0, 0);
        }
    }
}
