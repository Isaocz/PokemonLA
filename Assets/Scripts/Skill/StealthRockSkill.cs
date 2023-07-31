using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StealthRockSkill : Skill
{

    public StealthRockPrefabs stealthRockPrefabs;
    // Start is called before the first frame update
    void Start()
    {
        Instantiate(stealthRockPrefabs, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
