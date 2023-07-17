using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StealthRockSkillPlus : Skill
{
    public StealthRockPrefabs stealthRockPrefabs;
    List<StealthRockPrefabs> SRList = new List<StealthRockPrefabs> { };

    // Start is called before the first frame update
    void Start()
    {
        Instantiate(stealthRockPrefabs, transform.position, Quaternion.identity);
        foreach(Transform child in player.transform)
        {
            if (child.GetComponent<StealthRockSkillPlus>() != null && child.transform != this.transform) { Destroy(gameObject); }
        }
    }

    private void Update()
    {
        StartExistenceTimer();
        if(SRList.Count != 0)
        {
            for (int i = 0; i < SRList.Count; i++)
            {
                SRList[i].transform.position += (transform.position - SRList[i].transform.position).normalized * Time.deltaTime;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Spike")
        {
            StealthRockPrefabs s = other.GetComponent<StealthRockPrefabs>();
            if(s != null && !SRList.Contains(s))
            {
                SRList.Add(s);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Spike")
        {
            StealthRockPrefabs s = other.GetComponent<StealthRockPrefabs>();
            if (s != null && SRList.Contains(s))
            {
                SRList.Remove(s);
            }
        }
    }
}
