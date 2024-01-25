using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillGrassyTerrain : Skill
{
    public GrassyTerrain GT;
    // Start is called before the first frame update
    void Start()
    {
        if (SkillFrom == 2)
        {
            Instantiate(GT, transform.position, Quaternion.identity).isSuperMode = true;
        }
        else
        {
            Instantiate(GT, transform.position, Quaternion.identity).isSuperMode = false;
        }
        
        Destroy(gameObject);
    }
}
