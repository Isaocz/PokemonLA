using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssuranceNew : Skill
{
    public GameObject TackleBlast;

    // Start is called before the first frame update
    void Start()
    {
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        StartExistenceTimer();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Empty")
        {
            Empty target = other.GetComponent<Empty>();
            if ((float)target.EmptyHp <= (float)target.maxHP / 2.0f) { Damage *= 2; }
            Instantiate(TackleBlast, target.transform.position, Quaternion.identity);
            HitAndKo(target);
        }
    }

}
