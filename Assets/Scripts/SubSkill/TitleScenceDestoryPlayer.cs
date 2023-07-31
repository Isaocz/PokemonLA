using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScenceDestoryPlayer : MonoBehaviour
{


    // Update is called once per frame
    void Update()
    {
        if (FindObjectOfType<PlayerControler>() != null)
        {
            Debug.Log(FindObjectOfType<PlayerControler>());
            Destroy(FindObjectOfType<PlayerControler>().gameObject);
            Debug.Log(FindObjectOfType<PlayerControler>());
        }
    }
}
