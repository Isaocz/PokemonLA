using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeTears : Skill
{

    List<Empty> AlreadySPDDown = new List<Empty> {};
    int PlayerHPbefore;

    // Start is called before the first frame update
    void Start()
    {
        PlayerHPbefore = player.Hp;
    }

    // Update is called once per frame
    void Update()
    {
        StartExistenceTimer();
        if (SkillFrom == 2) {
            if (PlayerHPbefore > player.Hp)
            {
                PlayerHPbefore = player.Hp;
                foreach (Empty e in AlreadySPDDown)
                {
                    e.SpDChange(-1, 8);
                }
            }
        }

    }


    private void OnTriggerEnter2D(Collider2D other)
    {

        if (!other.isTrigger)
        {
            if (other.tag == "Empty")
            {
                Empty e = other.GetComponent<Empty>();
                if ((e != null) && !AlreadySPDDown.Contains(e))
                {
                    AlreadySPDDown.Add(e);
                    e.SpDChange(-2, 10f);
                }
            }
        }
    }
}
