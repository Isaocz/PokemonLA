using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartPanelBackGround : MonoBehaviour
{

    Canvas c;
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
        DontDestroyOnLoad(this);
        c = transform.GetChild(0).GetComponent<Canvas>();

    }
    private void Update()
    {
        if (c.worldCamera == null) { c.worldCamera = FindObjectsOfType<Camera>()[0]; }
        StartPanelBackGround[] Splist = FindObjectsOfType<StartPanelBackGround>();
        if (Splist.Length > 1)
        {
            for (int i = 0; i < Splist.Length; i++)
            {
                if (Splist[i].gameObject != this.gameObject) { Destroy(Splist[i].gameObject); }
            }
        }
        if (SceneManager.GetActiveScene().buildIndex != 0 && SceneManager.GetActiveScene().buildIndex != 1 && SceneManager.GetActiveScene().buildIndex != 2 && SceneManager.GetActiveScene().buildIndex != 3)
        {
            Destroy(gameObject);
        }
    }
}
