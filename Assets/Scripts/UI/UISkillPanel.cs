using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISkillPanel : MonoBehaviour
{


    // Update is called once per frame
    void Update()
    {
        Time.timeScale = 0;
    }

    public void SetPanelActiv1eFalse()
    {
        Debug.Log("xxx");
        Time.timeScale = 1;
        gameObject.SetActive(false);
    }
}
