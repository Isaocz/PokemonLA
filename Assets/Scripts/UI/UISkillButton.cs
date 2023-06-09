using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISkillButton : MonoBehaviour
{

    Button SkillButton;

    // Start is called before the first frame update
    void Start()
    {
        SkillButton = gameObject.GetComponent<Button>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) { SkillButton.onClick.Invoke(); }
    }
}
