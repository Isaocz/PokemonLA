using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IcicleSpearIce : Skill
{
    public Vector3 StartPostion;
    public Vector2 direction;
    bool isCanNotMove;
    IcicleSpear ParentRB;
    SpriteRenderer spriteRenderer;

    private void Start()
    {
        ParentRB = transform.parent.GetComponent<IcicleSpear>();
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
                transform.GetChild(0).gameObject.GetComponent<ParticleSystem>().Stop();
                if (transform.childCount > 1)
                {
                    transform.GetChild(1).gameObject.SetActive(true);
                }
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
                if(SkillFrom == 2)
                {
                    target.Frozen(7.5f, 1, 0.05f + ((float)player.LuckPoint / 30));
                }
            }
            isCanNotMove = true;
            transform.GetChild(0).gameObject.GetComponent<ParticleSystem>().Stop();
            if (transform.childCount > 1) { transform.GetChild(1).gameObject.SetActive(true); }
            spriteRenderer.color = new Color(0, 0, 0, 0);
        }
        else if (other.tag == "Room" || other.tag == "Enviroment")
        {

            isCanNotMove = true;
            transform.GetChild(0).gameObject.GetComponent<ParticleSystem>().Stop();
            if (transform.childCount > 1)
            {
                transform.GetChild(1).gameObject.SetActive(true);
            }
            spriteRenderer.color = new Color(0, 0, 0, 0);
        }
    }
}
