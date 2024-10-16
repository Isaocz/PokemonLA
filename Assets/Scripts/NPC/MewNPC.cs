using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MewNPC : GameNPC
{

    MewSelectSkillPanel SelecrSkillPanel;
    bool isWillFly;
    public bool isTrigger;

    // Start is called before the first frame update
    void Start()
    {
        NPCStart();
        isTrigger = false;
        SelecrSkillPanel = gameObject.transform.GetChild(4).GetChild(1).gameObject.GetComponent<MewSelectSkillPanel>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isWillFly && !isTrigger) {
            if (TalkPanel.gameObject.activeSelf == false)
            {

                NPCUpdate();
            }
            if (isInTrriger && ZButton.Z.IsZButtonDown)
            {
                SelecrSkillPanel.player = playerControler.GetComponent<PlayerControler>();
            }
        }
    }

    public void Beybey()
    {
        isWillFly = true;
        GetComponent<KonamiCode>().isWillFly = true;
        SelecrSkillPanel.gameObject.SetActive(false);
        TalkPanel.gameObject.SetActive(false);
        ZBottonObj.gameObject.SetActive(false);
        animator.SetTrigger("Byebye");
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!isWillFly && other.tag == ("Player"))
        {
            NPCOnTriggerStay2D(other);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!isWillFly && other.tag == ("Player"))
        {
            Debug.Log(other.name);
            NPCOnTriggerExit2D(other);
            if (TalkPanel.isTalkPuse) { TalkPanel.isTalkPuse = false; }
            if (other.tag == ("Player") && other.GetComponent<PlayerControler>() != null)
            {
                SelecrSkillPanel.gameObject.SetActive(false);
            }
        }
    }

    public override void CloseButton()
    {
        base.CloseButton();
        if (TalkPanel.isTalkPuse) { TalkPanel.isTalkPuse = false; }
        SelecrSkillPanel.gameObject.SetActive(false);
    }
}
