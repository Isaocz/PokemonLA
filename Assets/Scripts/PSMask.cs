using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PSMask : MonoBehaviour
{
    public GameObject LearnPanel;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (LearnPanel.activeInHierarchy)
        {
            transform.GetChild(0).gameObject.SetActive(false);
        }
        else
        {
            transform.GetChild(0).gameObject.SetActive(true);
        }
    }
}
