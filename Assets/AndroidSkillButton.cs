using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AndroidSkillButton : MonoBehaviour
{
    bool isF;
    Canvas ParentCanvas;
    PlayerControler Player;

    private void Awake()
    {
        isF = true;
        ParentCanvas = transform.parent.GetComponent<Canvas>();
        DontDestroyOnLoad(ParentCanvas.gameObject);
        if (SystemInfo.operatingSystemFamily != OperatingSystemFamily.Other)
        {
            //ParentCanvas.gameObject.SetActive(false);
        }

        Player = GameObject.FindObjectOfType<PlayerControler>();
    }

    private void FixedUpdate()
    {
        if (ParentCanvas.worldCamera == null)
        {
            ParentCanvas.worldCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        }
        if (MapCreater.StaticMap == null) { if (isF) { isF = false; transform.GetChild(0).gameObject.SetActive(false); } }
        else { if (!isF) { isF = true; transform.GetChild(0).gameObject.SetActive(true); } }

        if (SystemInfo.operatingSystemFamily == OperatingSystemFamily.Other) {
            
        }
    }

    // Start is called before the first frame update
    public void Skill01KeyDown()
    {

        if (Player == null) { Player = GameObject.FindObjectOfType<PlayerControler>(); }


        if (SystemInfo.operatingSystemFamily == OperatingSystemFamily.Other)
        {
            if (Player != null)
            {
                Player.IsSkill01ButtonDown = true;
            }
        }
    }

    public void Skill02KeyDown()
    {
        if (Player == null) { Player = GameObject.FindObjectOfType<PlayerControler>(); }

           
        if (SystemInfo.operatingSystemFamily == OperatingSystemFamily.Other)
        {
            if (Player != null)
            {
                Player.IsSkill02ButtonDown = true;
            }
        }
    }

    public void Skill03KeyDown()
    {
        if (Player == null) { Player = GameObject.FindObjectOfType<PlayerControler>(); }

           
        if (SystemInfo.operatingSystemFamily == OperatingSystemFamily.Other)
        {
            if (Player != null)
            {
                Player.IsSkill03ButtonDown = true;
            }
        }
    }

    public void Skill04KeyDown()
    {
        if (Player == null) { Player = GameObject.FindObjectOfType<PlayerControler>(); }

            
        if (SystemInfo.operatingSystemFamily == OperatingSystemFamily.Other)
        {
            if (Player != null)
            {
                Player.IsSkill04ButtonDown = true;
            }
        }
    }

    public void ZButtonDown()
    {
        if (SystemInfo.operatingSystemFamily == OperatingSystemFamily.Other)
        {
            ZButton.Z.IsZButtonDown = true;
        }
    }
}
