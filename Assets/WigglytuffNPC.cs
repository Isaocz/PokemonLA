using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WigglytuffNPC : NPC
{

    

    // Start is called before the first frame update
    void Start()
    {
        NPCStart();
    }

    // Update is called once per frame
    void Update()
    {
            if (TalkPanel.gameObject.activeSelf == false)
            {
                NPCUpdate();
            }
    }



    private void OnTriggerStay2D(Collider2D other)
    {
            NPCOnTriggerStay2D(other);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
            
        NPCOnTriggerExit2D(other);
        if (other.tag == ("Player") && other.GetComponent<PlayerControler>() != null)
        {
            if (TalkPanel.isTalkPuse)
            {
                TalkPanel.isTalkPuse = false;
            }
            animator.SetTrigger("TalkOver");
            animator.ResetTrigger("Happy");
            animator.ResetTrigger("Sad");
            animator.ResetTrigger("Jump");
        }
    }
}
