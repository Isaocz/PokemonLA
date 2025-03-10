using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gulpin : GameNPC
{
    // Start is called before the first frame update
    void Start()
    {
        NPCStart();
    }

    // Update is called once per frame
    void Update()
    {
        PlayerisinTrigger();

        if (TalkPanel.gameObject.activeSelf == false && TalkPanel.transform.parent.GetChild(1).gameObject.activeSelf == false && TalkPanel.transform.parent.GetChild(2).gameObject.activeSelf == false)
        {

            NPCUpdate();
        }
    }

    public void Beybey()
    {
        animator.SetTrigger("Bey");
        playerControler.GetComponent<PlayerControler>().CanNotUseSpaceItem = false;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        NPCOnTriggerStay2D(other);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        NPCOnTriggerExit2D(other);
    }
}
