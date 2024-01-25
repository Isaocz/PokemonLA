using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TropKick : GrassSkill
{
    // Start is called before the first frame update
    void Start()
    {
        if (SkillFrom == 2)
        {
            BornAGrass(player.transform.position);
            BornAGrass(player.transform.position + Vector3.up);
            BornAGrass(player.transform.position + Vector3.down);
            BornAGrass(player.transform.position + Vector3.left);
            BornAGrass(player.transform.position + Vector3.right);
            if (player.isInSuperGrassyTerrain)
            {
                BornAGrass(player.transform.position + Vector3.up + Vector3.right);
                BornAGrass(player.transform.position + Vector3.down + Vector3.left);
            }
        } 
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
            Empty e = other.GetComponent<Empty>();
            if (e != null)
            {
                if (SkillFrom == 2 && (player.InGressCount.Count != 0 || AlreadyBornBlockList.Contains(new Vector3((int)player.transform.position.x, (int)player.transform.position.y, (int)player.transform.position.z)))) { CTLevel++; }
                HitAndKo(e);
                e.AtkChange(-1, 15.0f);
            }
        }
    }
}
