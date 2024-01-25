using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmackDown : Skill
{
    Vector2 direction;
    Vector3 StartPostion;
    bool isCanNotMove;

    Empty SmackDownTarget;

    Vector3 StartScale;
    Vector3 StartPosition;

    public GameObject PS;
    bool isPSBorn;
    public GameObject SealthRock;

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
        if (SmackDownTarget != null && isCanNotMove)
        {
            if (ExistenceTime < 1.5f && ExistenceTime >= 1.2f)
            {
                SmackDownTarget.transform.GetChild(3).position = new Vector3(SmackDownTarget.transform.GetChild(3).position.x,  SmackDownTarget.transform.GetChild(3).position.y + Time.deltaTime * SmackDownTarget.transform.GetChild(0).localPosition.y / 0.36f, SmackDownTarget.transform.GetChild(3).position.z);
                SmackDownTarget.transform.GetChild(3).localScale = new Vector3(Mathf.Clamp( SmackDownTarget.transform.GetChild(3).localScale.x + Time.deltaTime * 0.5f , 1 , 1.15f), Mathf.Clamp( SmackDownTarget.transform.GetChild(3).localScale.y - Time.deltaTime * 2.5f, 0.25f, 1), SmackDownTarget.transform.GetChild(3).localScale.z);
            }
            if (ExistenceTime < 1.2f && ExistenceTime >= 1f)
            {
                if (!isPSBorn)
                {
                    isPSBorn = true;
                    Instantiate(PS, SmackDownTarget.transform.position, Quaternion.identity);
                }
            }
            if (ExistenceTime < 1f && ExistenceTime >= 0.4f)
            {
                SmackDownTarget.transform.GetChild(3).position = new Vector3(SmackDownTarget.transform.GetChild(3).position.x, SmackDownTarget.transform.GetChild(3).position.y - Time.deltaTime * SmackDownTarget.transform.GetChild(0).localPosition.y / 0.72f, SmackDownTarget.transform.GetChild(3).position.z);
                SmackDownTarget.transform.GetChild(3).localScale = new Vector3(Mathf.Clamp(SmackDownTarget.transform.GetChild(3).localScale.x - Time.deltaTime * 0.25f, 1, 1.15f), Mathf.Clamp(SmackDownTarget.transform.GetChild(3).localScale.y + Time.deltaTime * 1.25f, 0.25f, 1), SmackDownTarget.transform.GetChild(3).localScale.z);
            }
            if (ExistenceTime < 0.4f && ExistenceTime >= 0.3f)
            {
                SmackDownTarget.transform.GetChild(3).localScale = new Vector3(Mathf.Clamp(SmackDownTarget.transform.GetChild(3).localScale.x - Time.deltaTime * 2, 0.8f, 1f), Mathf.Clamp(SmackDownTarget.transform.GetChild(3).localScale.y + Time.deltaTime * 2 , 1, 1.2f), SmackDownTarget.transform.GetChild(3).localScale.z);
            }
            if (ExistenceTime < 0.3f && ExistenceTime >= 0.2f)
            {
                SmackDownTarget.transform.GetChild(3).localScale = new Vector3(Mathf.Clamp(SmackDownTarget.transform.GetChild(3).localScale.x + Time.deltaTime * 2, 0.8f, 1f), Mathf.Clamp(SmackDownTarget.transform.GetChild(3).localScale.y - Time.deltaTime * 2, 1, 1.2f), SmackDownTarget.transform.GetChild(3).localScale.z);
            }
            if (ExistenceTime < 0.2f)
            {
                SmackDownTarget.transform.GetChild(3).localPosition = StartPosition;
                SmackDownTarget.transform.GetChild(3).localScale = StartScale;
            }
        }
    }

    private void FixedUpdate()
    {
        if (!isCanNotMove)
        {
            Vector3 postion = transform.position;
            postion.x += direction.x * 6f * Time.deltaTime;
            postion.y += direction.y * 6f * Time.deltaTime;
            transform.position = postion;
            transform.rotation = Quaternion.Euler(0,0, transform.rotation.eulerAngles.z + 360*Time.deltaTime);
            if ((StartPostion - transform.position).magnitude > MaxRange)
            {
                StoneBreak();
                
            }
        }
    }

    void StoneBreak()
    {
        if (!isCanNotMove) {
            ExistenceTime = 1.5f;
            transform.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);
            transform.GetChild(0).gameObject.SetActive(true);
            transform.GetChild(0).localScale = new Vector3(0.4f, 0.4f, 0.4f);
            transform.DetachChildren();
            transform.GetComponent<Collider2D>().enabled = false;
            isCanNotMove = true;
        }
    }

    private void OnDestroy()
    {
        if (SmackDownTarget != null)
        {
            SmackDownTarget.transform.GetChild(3).localPosition = StartPosition;
            SmackDownTarget.transform.GetChild(3).localScale = StartScale;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Empty")
        {
            Empty target = other.GetComponent<Empty>();
            SmackDownTarget = target;
            StartPosition = target.transform.GetChild(3).localPosition;
            StartScale = target.transform.GetChild(3).localScale;
            if (SmackDownTarget.gameObject.layer == 16)
            {
                SmackDownTarget.gameObject.layer = 9;
            }
            HitAndKo(target);
            StoneBreak();
            if (SkillFrom == 2) { Instantiate(SealthRock, transform.position, Quaternion.identity); }
        }
        else if (other.tag == "Room" || other.tag == "Enviroment")
        {
            StoneBreak();
        }
    }
}
