using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoadManger : MonoBehaviour
{

    public static SceneLoadManger sceneLoadManger;

    private void Awake()
    {
        sceneLoadManger = this;
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
