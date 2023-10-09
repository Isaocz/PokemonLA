using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RazorLeaf : Skill
{

    Vector2 direction;
    Vector3 StartPostion;
    bool isCanNotMove;

    public Skill SubRazorLeaf;

    public bool isSub;



    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        direction = (transform.rotation * Vector2.right).normalized;
        StartPostion = transform.position;
        transform.rotation = Quaternion.Euler(0, 0, 0);
        transform.GetChild(0).rotation = Quaternion.Euler(0, 0, 0);
        transform.GetChild(0).position = StartPostion;
        transform.GetChild(0).parent = transform.parent;
    }


    private void FixedUpdate()
    {
        if (!isCanNotMove)
        {
            Vector3 postion = transform.position;
            postion.x += direction.x * 6f * (isSub ? 0.6f : 1) * Time.deltaTime;
            postion.y += direction.y * 6f * (isSub ? 0.6f : 1) * Time.deltaTime;
            transform.position = postion;
            if ((StartPostion - transform.position).magnitude > MaxRange)
            {
                LeafBreak();
            }
        }
    }

    void LeafBreak()
    {
        if (!isCanNotMove)
        {
            transform.GetChild(1).gameObject.SetActive(true);
            transform.GetChild(1).parent = transform.parent;
            transform.GetComponent<Collider2D>().enabled = false;
            Destroy(gameObject);
        }
    }



    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Empty")
        {
            Empty target = other.GetComponent<Empty>();
            HitAndKo(target);
            LeafBreak();
        }
        else if ((other.tag == "Room" || other.tag == "Enviroment") && !other.isTrigger)
        {
            LeafBreak();
        }
        if (SkillFrom == 2 && ( other.GetComponent<GressPlayerINOUT>() != null || other.GetComponent<NormalGress>() != null))
        {
            Instantiate(SubRazorLeaf , other.transform.position + new Vector3(0.7f , 0.7f , 0) ,Quaternion.Euler(0,0,45)).player = player;
            Instantiate(SubRazorLeaf , other.transform.position + new Vector3(0.7f ,-0.7f, 0), Quaternion.Euler(0,0, 315)).player = player;
            Instantiate(SubRazorLeaf , other.transform.position + new Vector3(-0.7f , 0.7f, 0), Quaternion.Euler(0,0,135)).player = player;
            Instantiate(SubRazorLeaf , other.transform.position + new Vector3(-0.7f ,-0.7f, 0), Quaternion.Euler(0,0, 225)).player = player;
        }
    }
}
