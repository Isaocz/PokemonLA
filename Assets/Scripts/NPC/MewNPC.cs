using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MewNPC : NPC
{

    MewSelectSkillPanel SelecrSkillPanel;
    bool isWillFly;

    // Start is called before the first frame update
    void Start()
    {
        NPCStart();
        SelecrSkillPanel = gameObject.transform.GetChild(4).GetChild(1).gameObject.GetComponent<MewSelectSkillPanel>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isWillFly) {
            if (TalkPanel.gameObject.activeSelf == false)
            {

                NPCUpdate();
            }
            if (isInTrriger && ZButton.Z.IsZButtonDown)
            {
                SelecrSkillPanel.player = playerControler;
            }
        }
    }

    public void Beybey()
    {
        isWillFly = true;
        SelecrSkillPanel.gameObject.SetActive(false);
        TalkPanel.gameObject.SetActive(false);
        ZBotton.gameObject.SetActive(false);
        animator.SetTrigger("Byebye");
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!isWillFly)
        {
            NPCOnTriggerStay2D(other);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!isWillFly)
        {
            NPCOnTriggerExit2D(other);
            if (TalkPanel.isTalkPuse) { TalkPanel.isTalkPuse = false; }
            if (other.tag == ("Player") && other.GetComponent<PlayerControler>() != null)
            {
                SelecrSkillPanel.gameObject.SetActive(false);
            }
        }
    }
}
