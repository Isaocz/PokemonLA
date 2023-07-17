using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneEdge : Skill
{



    // Start is called before the first frame update
    void Start()
    {
        transform.GetChild(0).position += (transform.rotation * Vector2.right).normalized;
    }

    // Update is called once per frame
    void Update()
    {
        StartExistenceTimer();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Spike")
        {
            StealthRockPrefabs stealthRock = other.GetComponent<StealthRockPrefabs>();
            if(stealthRock != null)
            {
                stealthRock.Exittime = 0;
                Damage += 5;
            }
        }
    }
}
