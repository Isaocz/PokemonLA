using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubDoubleHit : SubSkill
{
    // Start is called before the first frame update
    void Start()
    {
        player.RemoveASubSkill(subskill);
    }

    // Update is called once per frame
    void Update()
    {

        StartExistenceTimer();
    }

    public GameObject TackleBlast;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Empty")
        {
            Empty target = other.GetComponent<Empty>();
            Instantiate(TackleBlast, other.transform.position, Quaternion.identity);
            HitAndKo(other.gameObject);
        }
    }
}
