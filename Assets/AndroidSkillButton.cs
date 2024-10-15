using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AndroidSkillButton : MonoBehaviour
{
    public static AndroidSkillButton androidSkillButton;

    bool isF;
    Canvas ParentCanvas;
    PlayerControler Player;

    RectTransform SkillButtonTransform;

    Vector3[][] ButtonLayout = new Vector3[][]
    {
        new Vector3[]{ new Vector3(-104, 241.6836f, 0) , new Vector3(-200.9233f, 232, 0), new Vector3(-292.4765f, 181.8218f, 0) , new Vector3(-353.2186f, 93.78985f, 0) , new Vector3(-157.7875f, 71.78178f, 0)  },
        new Vector3[]{ new Vector3(-60, 330, 0) , new Vector3(-60, 235, 0), new Vector3(-60, 140, 0) , new Vector3(-60, 45 , 0) , new Vector3(-217, 87.1f, 0)  },
        new Vector3[]{ new Vector3(-330, 45,0) , new Vector3(-230, 45, 0), new Vector3(-130, 45, 0) , new Vector3(-30, 45, 0) , new Vector3(-85.3f, 165, 0)  },
        new Vector3[]{ new Vector3(-30, 45,0) , new Vector3(-130, 45, 0), new Vector3(-230, 45, 0) , new Vector3(-330, 45, 0) , new Vector3(-85.3f, 165, 0)  },
        new Vector3[]{ new Vector3(-178, 200,0) , new Vector3(-253, 125, 0), new Vector3(-103, 125, 0) , new Vector3(-178, 50, 0) , new Vector3(-178, 326, 0)  }
    };

    private void Awake()
    {
        androidSkillButton = this;
        isF = true;
        ParentCanvas = transform.parent.GetComponent<Canvas>();
        DontDestroyOnLoad(ParentCanvas.gameObject);
        if (SystemInfo.operatingSystemFamily != OperatingSystemFamily.Other)
        {
            ParentCanvas.gameObject.SetActive(false);
        }
        else
        {
            ParentCanvas.gameObject.SetActive(true);
        }

        Player = GameObject.FindObjectOfType<PlayerControler>();
        SkillButtonTransform = transform.GetChild(0).GetChild(1).GetComponent<RectTransform>();
    }


    private void Start()
    {
        SetXOffset(PlayerPrefs.GetFloat("SkillButtonXOffset"));
        SetYOffset(PlayerPrefs.GetFloat("SkillButtonYOffset"));
        SetScale(PlayerPrefs.GetFloat("SkillButtonScale"));
        SetButtonPos(PlayerPrefs.GetInt("SkillButtonLayout"));
    }

    private void FixedUpdate()
    {
        
        if (SystemInfo.operatingSystemFamily == OperatingSystemFamily.Other) {
            if (ParentCanvas.worldCamera == null)
            {
                ParentCanvas.worldCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
            }
            if (MapCreater.StaticMap == null && TownMap.townMap == null) { if (isF) { isF = false; transform.GetChild(0).gameObject.SetActive(false); } }
            else { if (!isF) { isF = true; transform.GetChild(0).gameObject.SetActive(true); } }

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
        ZButton.Z.IsZButtonDown = true;
        if (SystemInfo.operatingSystemFamily == OperatingSystemFamily.Other)
        {

        }
    }

    public void SetXOffset(float x)
    {
        SkillButtonTransform.anchoredPosition = new Vector2((x * 500) - 100, SkillButtonTransform.anchoredPosition.y);
    }

    public void SetYOffset(float y)
    {
        SkillButtonTransform.anchoredPosition = new Vector2(SkillButtonTransform.anchoredPosition.x, y * 400);
    }

    public void SetScale( float s )
    {
        SkillButtonTransform.localScale = new Vector3(1 + s , 1 + s, 1);
    }


    public void SetButtonPos(int Index)
    {
        for (int i = 0; i < 5; i++ )
        {
            SkillButtonTransform.transform.GetChild(i).GetComponent<RectTransform>().anchoredPosition = ButtonLayout[Index][i];
        }
    }

}
