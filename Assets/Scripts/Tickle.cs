using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tickle : Skill
{

    List<Empty> AlreadySPDDown = new List<Empty> { };

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        StartExistenceTimer();
       

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
                    e.AtkChange(-1, 10f);
                    e.DefChange(-1, 10f);
                    if(SkillFrom == 2)
                    {
                        switch (Random.Range(0, 2)) {
                            case 0: e.AtkChange(-1, 10f);break;
                            case 1: e.DefChange(-1, 10f);break;
                        }

                    }
                }
            }
        }
    }
}
