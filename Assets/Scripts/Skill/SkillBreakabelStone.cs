using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillBreakabelStone : MonoBehaviour
{
    public bool isBreak;
    public GameObject SealthRock;
    int ColliderCount;

    private void Start()
    {
        Invoke("RockBreak" , 30);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (!isBreak && other.gameObject.tag != "Enviroment" && other.gameObject.tag != "Room" && other.gameObject.tag != "Item" && other.gameObject.tag != "Spike")
        {
            ColliderCount++;
            if (other.gameObject.tag == "Player" && other.transform.GetComponent<PlayerControler>() && other.transform.GetComponent<PlayerControler>().PlayerBodySize == 2) { ColliderCount = 7; }
            if(ColliderCount >= Random.Range(1, 6) || GetComponent<LittleTreeBeKickDown>().isBeKickDown)
            {
                RockBreak();
            }
            
        }
    }

    

    public void RockBreak()
    {
        if (gameObject != null && !isBreak)
        {
            isBreak = true;
            Instantiate(SealthRock, transform.position, Quaternion.identity);
            GetComponent<Animator>().SetTrigger("Break");
        }
    }

    

    public void DestorySelf()
    {
        Destroy(gameObject);
    }
}
