using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleScenceDestoryPlayer : MonoBehaviour
{

    public Text d;

    // Update is called once per frame
    void Update()
    {
        /*
        //d.text = SystemInfo.operatingSystemFamily.ToString();
        if (FindObjectOfType<PlayerControler>() != null)
        {
            Debug.Log(FindObjectOfType<PlayerControler>());
            Destroy(FindObjectOfType<PlayerControler>().gameObject);
            Debug.Log(FindObjectOfType<PlayerControler>());
        }
        */
    }

    private void OnEnable()
    {
        //d.text = SystemInfo.operatingSystemFamily.ToString();
        if (FindObjectOfType<PlayerControler>() != null)
        {
            Debug.Log(FindObjectOfType<PlayerControler>());
            Destroy(FindObjectOfType<PlayerControler>().gameObject);
            Debug.Log(FindObjectOfType<PlayerControler>());
        }
    }
}
