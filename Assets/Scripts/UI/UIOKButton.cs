using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIOKButton : MonoBehaviour
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
        if (ZButton.Z.IsZButtonDown) { SkillButton.onClick.Invoke(); }
    }
}
