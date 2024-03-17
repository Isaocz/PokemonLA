using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeakTreeCut : MonoBehaviour
{

    GameObject ZButtonObj;
    Animator animator;
    bool isBeCut;

    public string NoStoneString01;
    public string NoStoneString02;

    PlayerControler p;

    // Start is called before the first frame update
    void Start()
    {
        ZButtonObj = transform.GetChild(3).gameObject;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if(other.tag == "Player" && other.GetComponent<PlayerControler>() != null)
        {

            ZButtonObj.SetActive(true);
            if (ZButton.Z.IsZButtonDown && !isBeCut)
            {
                p = other.GetComponent<PlayerControler>();
                if (p != null && !p.isInZ) {
                    p.isInZ = true;
                    if (p.Stone > 0)
                    {
                        animator.SetTrigger("Cut");
                        p.ChangeStone(-1);
                        isBeCut = true;
                    } else if (p.Stone <= 0)
                    {
                        UIGetANewItem.UI.JustSaySth(NoStoneString01, NoStoneString02);
                        p.isInZ = false;
                    }
                }
            }
        }
    }


    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player" && other.GetComponent<PlayerControler>() != null)
        {
            ZButtonObj.SetActive(false);
        }
    }

    public void DestoryTree()
    {
        Destroy(gameObject);
        p.isInZ = false;
    }

    public void SetZFalse()
    {
        ZButtonObj.gameObject.SetActive(false);
    }
}
