using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownParkHourse : TownChair
{

    public GameObject ZButtonObj;
    bool isInShake;

    private void Start()
    {
        ChairsStart();
        ZButtonObj = transform.GetChild(2).gameObject;
        animator = transform.GetComponent<Animator>();
    }

    public void ShakeOver() { isInShake = false; }

    private void Update()
    {
        ChairsJumpUp();
        if (!isInShake) { ChairsSetDown(); }
        ChairsJumpDown();



        if (ChairTarget != null) {
            if (State == ChairState.SetDown)
            {
                if (!ChairTarget.transform.parent) { ChairTarget.transform.parent = transform.GetChild(3); }

                if (!ChairTarget.isInZ) {
                    ZButtonObj.SetActive(true);
                    ChairTarget.isInZ = true;
                }
                if (isInShake) { ZButtonObj.SetActive(false); }
                else { ZButtonObj.SetActive(true); }
            }
            else
            {
                if (ChairTarget.transform.parent) { ChairTarget.transform.parent = null; }

                if (ZButtonObj.activeInHierarchy) {
                    ChairTarget.isInZ = false;
                    ZButtonObj.SetActive(false);
                }
            }

            if (!isInShake && ZButtonObj.activeInHierarchy && ZButton.Z.IsZButtonDown)
            {
                isInShake = true;
                animator.SetTrigger("Shake");
            }
        }
    }
}
