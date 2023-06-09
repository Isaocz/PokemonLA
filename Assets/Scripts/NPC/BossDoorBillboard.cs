using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossDoorBillboard : MonoBehaviour
{

    GameObject ZBotton;
    Image TalkPanel;
    PlayerControler playerControler;
    bool isInTrriger;

    // Start is called before the first frame update
    void Start()
    {
        ZBotton = gameObject.transform.GetChild(1).gameObject;
        TalkPanel = gameObject.transform.GetChild(0).GetChild(0).gameObject.GetComponent<Image>();
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == ("Player"))
        {
            playerControler = other.GetComponent<PlayerControler>();
            ZBotton.SetActive(true);
            isInTrriger = true;
        }
    }

    // Update is called once per frame
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == ("Player") && isInTrriger)
        {
            ZBotton.SetActive(false);
            isInTrriger = false;
            if (TalkPanel != null)
            {
                TalkPanel.gameObject.SetActive(false);
            }
        }
    }


    void Update()
    {
        if (isInTrriger && Input.GetKeyDown(KeyCode.Z))
        {
            if (TalkPanel.IsActive())
            {
                TalkPanel.gameObject.SetActive(false);
            }
            else
            {
                TalkPanel.gameObject.SetActive(true);
            }
        }
    }
}
