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
        if(other.tag == "Player")
        {
            ZButton.SetActive(true);
            if (Input.GetKey(KeyCode.Z))
            {
                if(other.GetComponent<PlayerControler>().Stone > 0 && !isBeCut)
                {
                    animator.SetTrigger("Cut");
                    other.GetComponent<PlayerControler>().ChangeStone(-1);
                    isBeCut = true;
                }else if (other.GetComponent<PlayerControler>().Stone <= 0)
                {
                    UIGetANewItem.UI.JustSaySth("一颗很细小的树","如果有锋利的工具就可以砍倒它");
                }
                
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            ZButton.SetActive(false);
        }
    }

    public void DestoryTree()
    {
        Destroy(gameObject);
    }
}
