using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveLoader : MonoBehaviour
{


    public static SaveLoader saveLoader;

    public SaveData saveData = null;
    private void Awake()
    {
        saveLoader = this;
        DontDestroyOnLoad(gameObject);
    }
}
