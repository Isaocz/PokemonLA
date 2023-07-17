using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoadManger : MonoBehaviour
{

    public static SceneLoadManger sceneLoadManger;
    public PlayerControler XSZ;

    private void Awake()
    {
        sceneLoadManger = this;
        if (FindObjectOfType<MapCreater>() != null && FindObjectOfType<PlayerControler>() == null) { 
            PlayerControler p = Instantiate(XSZ, Vector3.zero, Quaternion.identity);
            UIPanelGwtNewSkill.StaticUIGNS.SetPlayer(p);
            Debug.Log(p);
        }
        if (FindObjectOfType<PlayerControler>() != null) { PlayerControler player = FindObjectOfType<PlayerControler>(); player.transform.position = Vector3.zero; player.NowRoom = Vector3Int.zero; }
    }

    private void Start()
    {

    }

    public void LoadGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadSceneAsync(2);
    }

    public void ReturnTitle()
    {
        SceneManager.LoadSceneAsync(1);
    }

    public void QuitGame()
    {
        //UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();
    }
}
