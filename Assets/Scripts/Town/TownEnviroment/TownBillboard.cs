using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TownBillboard : MonoBehaviour
{
    GameObject ZBottonOBJ;
    Image TalkPanel;
    TownPlayer player;
    bool isInTrriger;

    public Animator animator;

    // Start is called before the first frame update
    public void BillboardStart()
    {
        ZBottonOBJ = gameObject.transform.GetChild(1).gameObject;
        //Debug.Log(gameObject.transform.GetChild(1).gameObject);
        TalkPanel = gameObject.transform.GetChild(0).GetChild(0).gameObject.GetComponent<Image>();
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

    private void Start()
    {
        BillboardStart();
    }

    private void Update()
    {
        BillboardUpdate();
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


    public void BillboardUpdate()
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
