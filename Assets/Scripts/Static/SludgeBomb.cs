using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SludgeBomb : Skill
{
    public float moveSpeed;
    // Start is called before the first frame update

    Vector2 direction;
    Vector3 StartPostion;
    bool isCanNotMove;

    SpriteRenderer BallSprite;




    // Start is called before the first frame update
    void Start()
    {
        direction = (transform.rotation * Vector2.right).normalized;
        StartPostion = transform.position;
        transform.rotation = Quaternion.Euler(0, 0, 0);
        BallSprite = transform.GetChild(0).GetComponent<SpriteRenderer>();
    }


    private void FixedUpdate()
    {
        StartExistenceTimer();
        if (!isCanNotMove)
        {
            BallSprite.transform.localPosition = new Vector3(0, -4 * ExistenceTime * ExistenceTime + 36 * ExistenceTime - 80, 0);
            Vector3 postion = transform.position;
            postion.x += direction.x * moveSpeed * Time.deltaTime;
            postion.y += direction.y * moveSpeed * Time.deltaTime;
            transform.position = postion;
            if ((StartPostion - transform.position).magnitude > MaxRange)
            {
                BallBreak();
            }
        }
        else
        {
            BallSprite.color -= new Color(0, 0, 0, 5 * Time.deltaTime);
            BallSprite.transform.localScale += new Vector3(5 * Time.deltaTime, 5 * Time.deltaTime, 5 * Time.deltaTime);
        }
    }


    void BallBreak()
    {
        if (!isCanNotMove)
        {
            transform.GetChild(1).gameObject.SetActive(true);
            transform.GetChild(2).gameObject.SetActive(true);
            BallSprite.transform.GetChild(0).parent = null;
            transform.GetChild(1).parent = null;
            transform.GetChild(1).parent = null;
            BallSprite.transform.localPosition = new Vector3(0,0,0);

            transform.GetComponent<Collider2D>().enabled = false;
            isCanNotMove = true;
            ExistenceTime = 0.2f;
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
                    if (SkillFrom == 2 && target.isToxicDone) { SpDamage += 30; }
                    HitAndKo(target);
                    target.EmptyToxicDone(1,30, 0.3f + (float)player.LuckPoint / 10);
                }
                BallBreak();
            }
            else if (other.tag == "Room" || other.tag == "Enviroment")
            {
                BallBreak();
            }
        }
    }
}
