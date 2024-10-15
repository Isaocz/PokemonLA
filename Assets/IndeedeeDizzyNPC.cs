using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ÃÔÂ·µÄ°®¹ÜÊÌ
/// </summary>
public class IndeedeeDizzyNPC : TownNPC
{


    void Start()
    {

        NPCStart();
        if (SaveLoader.saveLoader != null && !SaveLoader.saveLoader.saveData.TownNPCDialogState.isStateWithIndeedee01)
        {
            gameObject.SetActive(true);
            if (!SaveLoader.saveLoader.saveData.TownNPCDialogState.isStateWithIndeedee03)
            {
                animator.SetBool("Dizzy", true);
            }
            else { animator.SetBool("Dizzy", false); }

        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        PlayerisinTrigger();

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
    }

    private void OnDestroy()
    {
        if (SaveLoader.saveLoader != null && !SaveLoader.saveLoader.saveData.TownNPCDialogState.isStateWithIndeedee01 && SaveLoader.saveLoader.saveData.TownNPCDialogState.isStateWithIndeedee03)
        {
            SaveLoader.saveLoader.saveData.TownNPCDialogState.isStateWithIndeedee01 = true;
        }
    }

}
