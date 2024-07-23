using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCcomputer : MonoBehaviour
{

    GameObject ZBottonObj;
    bool isInTrriger;
    public PlayerControler player;
    public PCcomputerTalkPanle TalkPanel;

    // Start is called before the first frame update
    void Start()
    {
        ZBottonObj = gameObject.transform.GetChild(1).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (isInTrriger && player != null && (transform.position - player.transform.position).magnitude >= 6.0f)
        {
            ZBottonObj.SetActive(false);
            isInTrriger = false;
            TalkPanel.PlayerExit();
            player.CanNotUseSpaceItem = false;
        }
        if (isInTrriger && ZButton.Z.IsZButtonDown)
        {
            TalkPanel.gameObject.SetActive(true);
            player.CanNotUseSpaceItem = true;
        }
        if (!isInTrriger)
        {
            TalkPanel.PlayerExit();
        }
    }

    public void CloseButton()
    {
        ZBottonObj.SetActive(false);
        isInTrriger = false;
        if (player != null) { player.CanNotUseSpaceItem = false; }
    }


    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == ("Player") && other.GetComponent<PlayerControler>() != null)
        {
            player = other.GetComponent<PlayerControler>();
            ZBottonObj.SetActive(true);
            isInTrriger = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == ("Player") && isInTrriger && other.GetComponent<PlayerControler>() != null)
        {
            ZBottonObj.SetActive(false);
            isInTrriger = false;
            player.CanNotUseSpaceItem = false;
        }
    }
}
