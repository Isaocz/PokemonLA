using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoredPowerEffect : Skill
{
    private float moveSpeed;
    private float timer;
    StoredPower ParentSP;
    int D;
    Vector3 targetPos;
    bool isCanNotMove;
    TraceEffect te;

    // Start is called before the first frame update
    void Start()
    {
        ParentSP = transform.parent.GetComponent<StoredPower>();
        player = ParentSP.player;
        SpDamage = ParentSP.SpDamage;
        D = (int)transform.parent.rotation.eulerAngles.z;
        timer = 0f;
        targetPos =  ((transform.parent.position + Quaternion.AngleAxis(D, Vector3.forward) * Vector3.right * 20 ) - transform.position).normalized;
        if (SkillFrom == 2) { te = GetComponent<TraceEffect>(); }
    }

    // Update is called once per frame
    void Update()
    {
        if (SkillFrom == 2) { te.moveSpeed = moveSpeed * 0.6f; }
        if (!isCanNotMove) {
            timer += 3 * Time.deltaTime;
            moveSpeed = Mathf.Exp(timer);
            transform.position += ((te.isTEDone)?0: moveSpeed) * targetPos * Time.deltaTime;
            if (timer >= 4f)
            {
                isCanNotMove = true;
            }
        }
        else
        {
            moveSpeed = 0;
            StartExistenceTimer();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Empty"))
        {

            Empty target = collision.GetComponent<Empty>();
            if (target != null)
            {
                SpDamage = ParentSP.SpDamage;
                HitAndKo(target);
                isCanNotMove = true;
                GetComponent<Collider2D>().enabled = false;
            }
        }
        if ((collision.CompareTag("Enviroment") || collision.CompareTag("Room")))
        {
            isCanNotMove = true;
            GetComponent<Collider2D>().enabled = false;
        }
    }
}
