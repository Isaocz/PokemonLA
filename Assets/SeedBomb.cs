using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedBomb : GrassSkill
{
    Vector2 direction;
    Vector3 StartPostion;
    bool isCanNotMove;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        direction = (transform.rotation * Vector2.right).normalized;
        StartPostion = transform.position;
        transform.rotation = Quaternion.Euler(0, 0, 0);
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
            postion.x += direction.x * 6f * Time.deltaTime;
            postion.y += direction.y * 6f * Time.deltaTime;
            transform.position = postion;
            transform.rotation = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z + 360 * Time.deltaTime);
            if ((StartPostion - transform.position).magnitude > MaxRange)
            {
                SeedBreak();

            }
        }
    }

    void SeedBreak()
    {
        if (!isCanNotMove)
        {
            transform.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);
            transform.GetChild(0).gameObject.SetActive(true);
            transform.GetChild(0).localScale = new Vector3(0.4f, 0.4f, 0.4f);
            transform.GetChild(0).rotation = Quaternion.Euler(0, 0, 0);
            transform.DetachChildren();
            transform.GetComponent<Collider2D>().enabled = false;
            isCanNotMove = true;

            if (SkillFrom == 2) {
                BornAGrass(transform.position);
                BornAGrass(transform.position + Vector3.left);
                BornAGrass(transform.position + Vector3.right);
                BornAGrass(transform.position + Vector3.down);
                BornAGrass(transform.position + Vector3.up);
                if (player.isInSuperGrassyTerrain)
                {
                    BornAGrass(player.transform.position + Vector3.up + Vector3.right);
                    BornAGrass(player.transform.position + Vector3.down + Vector3.left);
                }
            }
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.isTrigger == false) {
            if (other.tag == "Empty")
            {
                Empty target = other.GetComponent<Empty>();
                target.EmptyKnockOut(KOPoint);
                transform.GetChild(1).gameObject.SetActive(true);
                transform.GetChild(1).localScale = new Vector3(0.4f, 0.4f, 0.4f);
                transform.GetChild(1).rotation = Quaternion.Euler(0, 0, 0);
                transform.GetChild(1).GetComponent<SeedBombTriggers>().ParentSB = this;
                var m = transform.GetChild(0).GetComponent<ParticleSystem>().main;
                m.startDelay = 0.4f;
                SeedBreak();
            }
            else if (other.tag == "Room" || other.tag == "Enviroment")
            {
                SeedBreak();
            }
        }
    }
}
