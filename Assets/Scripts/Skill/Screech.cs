using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Screech : Skill
{
    List<Empty> AlreadySPDDown = new List<Empty> { };
    int dCount;


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
                    e.DefChange(-2, SkillFrom == 2 ? 16.0f : 10f);
                    dCount++;
                }
            }
        }
    }

    private void OnDestroy()
    {
        if (dCount > 0)
        {
            player.playerData.SpDBounsJustOneRoom++;
        }
        if (dCount > 1)
        {
            player.playerData.DefBounsJustOneRoom++;
        }
        player.ReFreshAbllityPoint();
    }
}
