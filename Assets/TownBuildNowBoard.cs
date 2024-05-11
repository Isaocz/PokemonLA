using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TownBuildNowBoard : MonoBehaviour
{
    GameObject ZBottonOBJ;
    Image TalkPanel;
    TownPlayer player;
    bool isInTrriger;

    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        ZBottonOBJ = gameObject.transform.GetChild(1).gameObject;
        TalkPanel = gameObject.transform.GetChild(0).GetChild(0).gameObject.GetComponent<Image>();
        animator.SetInteger("State" , Random.Range(0,4) );
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == ("Player"))
        {
            if (other.GetComponent<TownPlayer>() != null)
            {
                player = other.GetComponent<TownPlayer>();
                ZBottonOBJ.SetActive(true);
                isInTrriger = true;
            }
        }
    }

    // Update is called once per frame
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == ("Player") && isInTrriger && other.GetComponent<TownPlayer>() != null)
        {
            ZBottonOBJ.SetActive(false);
            isInTrriger = false;
            if (TalkPanel != null)
            {
                player.isInZ = false;
                TalkPanel.gameObject.SetActive(false);
            }
        }
    }


    void Update()
    {
        if (isInTrriger && ZButton.Z.IsZButtonDown)
        {
            if (TalkPanel.IsActive())
            {
                player.isInZ = false;
                TalkPanel.gameObject.SetActive(false);
            }
            else
            {
                player.isInZ = true;
                TalkPanel.gameObject.SetActive(true);
            }
        }
    }
}
