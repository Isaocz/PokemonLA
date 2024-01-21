using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyBall : GrassSkill
{
    public float initialSpeed; 
    private float moveSpeed;
    // Start is called before the first frame update

    Vector2 direction;
    Vector3 StartPostion;
    bool isCanNotMove;
    GameObject OverPS1;



    // Start is called before the first frame update
    void Start()
    {
        moveSpeed = initialSpeed;
        animator = GetComponent<Animator>();
        direction = (transform.rotation * Vector2.right).normalized;
        StartPostion = transform.position;
        transform.rotation = Quaternion.Euler(0, 0, 0);
        OverPS1 = transform.GetChild(3).gameObject;
    }


    private void FixedUpdate()
    {
        if (!isCanNotMove)
        {
            Vector3 postion = transform.position;
            postion.x += direction.x * moveSpeed * Time.deltaTime;
            postion.y += direction.y * moveSpeed * Time.deltaTime;
            transform.position = postion;
            if ((StartPostion - transform.position).magnitude > MaxRange)
            {
                BallBreak();

            }
        }
    }


    void BallBreak()
    {
        if (!isCanNotMove)
        {
            transform.GetComponent<Collider2D>().enabled = false;
            animator.SetTrigger("Over");
            OverPS1.gameObject.SetActive(true);
            OverPS1.transform.parent = transform.parent;
            isCanNotMove = true;
        }
    }

    void BornGrass()
    {
        BornAGrass(transform.position);
        BornAGrass(transform.position + Vector3.right);
        BornAGrass(transform.position + Vector3.left);
        BornAGrass(transform.position + Vector3.down);
        BornAGrass(transform.position + Vector3.up);
        if ((!player.playerData.IsPassiveGetList[118] && ( Weather.GlobalWeather.isSunny || Weather.GlobalWeather.isSunnyPlus )) || player.isInGrassyTerrain)
        {
            BornAGrass(transform.position + Vector3.right + Vector3.down);
            BornAGrass(transform.position + Vector3.left + Vector3.up);
            BornAGrass(transform.position + Vector3.down + Vector3.left);
            BornAGrass(transform.position + Vector3.up + Vector3.right);

            if (player.isInSuperGrassyTerrain)
            {
                BornAGrass(transform.position + 2 * Vector3.right);
                BornAGrass(transform.position + 2 * Vector3.left);
                BornAGrass(transform.position + 2 * Vector3.down);
                BornAGrass(transform.position + 2 * Vector3.up);
            }
        }
    }



    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.isTrigger == false)
        {
            if (other.tag == "Empty")
            {
                Empty target = other.GetComponent<Empty>();
                HitAndKo(target);
                BallBreak();
                if (SkillFrom == 2) {  BornGrass(); }
            }
            else if (other.tag == "Room" || other.tag == "Enviroment")
            {
                BallBreak();
                if (SkillFrom == 2) { BornGrass(); }
            }
        }
    }
}
