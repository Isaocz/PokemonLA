using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISkillButton : MonoBehaviour
{
    public static UISkillButton Instance;
    public bool isEscEnable;
    Button SkillButton;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        SkillButton = gameObject.GetComponent<Button>();
        isEscEnable = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isEscEnable && Input.GetKeyDown(InitializePlayerSetting.GlobalPlayerSetting.GetKeybind("OpenMenu"))) { SkillButton.onClick.Invoke(); }
    }
}
