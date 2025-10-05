using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCBook : GameNPC
{
    void Start()
    {
        NPCStart();
    }

    // Update is called once per frame
    void Update()
    {
        NPCUpdate();
        PlayerisinTrigger();
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
