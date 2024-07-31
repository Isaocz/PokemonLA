using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossDoorBillboard : MonoBehaviour
{

    GameObject ZBottonObj;
    Image TalkPanel;
    PlayerControler playerControler;
    bool isInTrriger;

    // Start is called before the first frame update
    void Start()
    {
        ZBottonObj = gameObject.transform.GetChild(1).gameObject;
        TalkPanel = gameObject.transform.GetChild(0).GetChild(0).gameObject.GetComponent<Image>();
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == ("Player"))
        {
            if (other.GetComponent<PlayerControler>() != null)
            {
                playerControler = other.GetComponent<PlayerControler>();
                ZBottonObj.SetActive(true);
                isInTrriger = true;
            }
        }
    }

    // Update is called once per frame
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == ("Player") && isInTrriger && other.GetComponent<PlayerControler>() != null)
        {
            ZBottonObj.SetActive(false);
            isInTrriger = false;
            if (TalkPanel != null)
            {
                playerControler.CanNotUseSpaceItem = false;
                TalkPanel.gameObject.SetActive(false);
            }
        }
    }


    void Update()
    {
        if ((isInTrriger || ZBottonObj.activeInHierarchy) && (transform.position - playerControler.transform.position).magnitude >= 6.0f)
        {
            ZBottonObj.SetActive(false);
            isInTrriger = false;
            playerControler.CanNotUseSpaceItem = false;
            TalkPanel.gameObject.SetActive(false);
        }
        if (isInTrriger && ZButton.Z.IsZButtonDown)
        {
            if (TalkPanel.IsActive())
            {
                playerControler.CanNotUseSpaceItem = false;
                TalkPanel.gameObject.SetActive(false);
            }
            else
            {
                playerControler.CanNotUseSpaceItem = true;
                TalkPanel.gameObject.SetActive(true);
            }
        }
    }

    public void CloseButton()
    {
        ZBottonObj.SetActive(false);
        isInTrriger = false;
        playerControler.CanNotUseSpaceItem = false;
        TalkPanel.gameObject.SetActive(false);
    }
}
