using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeakTreeCut : MonoBehaviour
{

    GameObject ZButton;
    Animator animator;
    bool isBeCut;

    // Start is called before the first frame update
    void Start()
    {
        ZButton = transform.GetChild(3).gameObject;
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
            ZButton.SetActive(true);
            if (Input.GetKey(KeyCode.Z))
            {
                PlayerControler p = other.GetComponent<PlayerControler>();
                if (p != null) {
                    if (p.Stone > 0 && !isBeCut)
                    {
                        animator.SetTrigger("Cut");
                        p.ChangeStone(-1);
                        isBeCut = true;
                    } else if (p.Stone <= 0)
                    {
                        UIGetANewItem.UI.JustSaySth("һ�ź�ϸС����", "����з����Ĺ��߾Ϳ��Կ�����");
                    }
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player" && other.GetComponent<PlayerControler>() != null)
        {
            ZButton.SetActive(false);
        }
    }

    public void DestoryTree()
    {
        Destroy(gameObject);
    }
}
