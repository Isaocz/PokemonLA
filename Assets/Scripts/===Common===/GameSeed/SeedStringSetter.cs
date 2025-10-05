using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SeedStringSetter : MonoBehaviour
{
    public Text t;

    private void Start()
    {
        if (InitializePlayerSetting.GlobalPlayerSetting != null) 
        {
            t.text = InitializePlayerSetting.GlobalPlayerSetting.SeedString;
        }
        else
        {
            t.text = "æ¯√‹√∞œ’";
        }
    }
}
