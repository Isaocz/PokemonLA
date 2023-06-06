using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GressPlayerINOUT : MonoBehaviour
{

    Animator animator;
    public GameObject RareCandy;
    public GameObject CCG;
    public GameObject StartDust;
    bool isVisit;
    PlayerControler player;

    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        
    }

    void LunchNewItem(GameObject LunchGameObject)
    {
        GameObject NewItem = Instantiate(LunchGameObject, gameObject.transform.position+Vector3.down*0.4f, Quaternion.identity,transform);
        NewItem.GetComponent<IteamPickUp>().isLunch = true;

    }

    void CreatNewItem()
    {
        if (!isVisit)
        {
            isVisit = true;
            if (Random.Range(0.000f, 1.000f) < (0.1f+(((float)player.LuckPoint))/30) )
            {
                float JudgeF = Random.Range(0.0f, 1.0f);
                if (JudgeF <= 1.0f && JudgeF > 0.8f) { LunchNewItem(RareCandy); }
                else if (JudgeF <= 0.8f && JudgeF > 0.6f) { LunchNewItem(StartDust); }
                else if (JudgeF <= 0.6f && JudgeF >= 0.4f) { LunchNewItem(CCG); }
                
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.tag == "Player") {
            animator.SetTrigger("PlayerIn");
            player = other.GetComponent<PlayerControler>();
            CreatNewItem();
        }
        
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.transform.tag == "Player")
        {
            animator.SetTrigger("PlayerOut");
        }
    }
}
