using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleKick : Skill
{
    // Start is called before the first frame update
    CircleCollider2D DKCollider;
    bool isCTLUp;

    private void Start()
    {
        DKCollider = transform.GetComponent<CircleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        StartExistenceTimer();
        if (ExistenceTime <= 3.2f && ExistenceTime >= 3.1f)
        {
            DKCollider.radius = Mathf.Clamp(DKCollider.radius + Time.deltaTime* 5f, 0 , 0.5f);
        }
        else if (ExistenceTime <= 2.3f && ExistenceTime >= 2.2f)
        {
            DKCollider.radius = Mathf.Clamp(DKCollider.radius - Time.deltaTime * 5f, 0, 0.5f);
        }
        else if (ExistenceTime <= 2.0f && ExistenceTime >= 1.8f && DKCollider.enabled)
        {
            DKCollider.enabled = false;
        }
        else if (ExistenceTime <= 1.8f && ExistenceTime >= 1.7f)
        {
            if (!DKCollider.enabled) { DKCollider.enabled = true; isHitDone = false; }
            transform.GetChild(1).gameObject.SetActive(true);
            DKCollider.radius = Mathf.Clamp(DKCollider.radius + Time.deltaTime * 5f, 0, 0.5f);
        }
        else if (ExistenceTime <= 0.9f && ExistenceTime >= 0.8f)
        {
            DKCollider.radius = Mathf.Clamp(DKCollider.radius - Time.deltaTime * 5f, 0, 0.5f);
        }
        else if (ExistenceTime <= 0.8f)
        {
            DKCollider.enabled = false;
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Empty")
        {
            Empty e = other.GetComponent<Empty>();
            if (e != null)
            {
                HitAndKo(e);
                if (SkillFrom == 2 && !isCTLUp) { CTLevel++; isCTLUp = true; }
            }
        }
    }

}
