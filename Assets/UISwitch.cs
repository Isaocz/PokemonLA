using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISwitch : MonoBehaviour
{

    public string[] SwitchCount;
    public Text text;

    public int SwitchIndex { get { return switchIndex; } set { switchIndex = value; } }
    int switchIndex;

    public virtual void RightSwitch()
    {
        switchIndex++;
        if (switchIndex > SwitchCount.Length-1) { switchIndex = 0; }
        SetSwitch(switchIndex);
    }

    public virtual void LeftSwitch()
    {
        switchIndex--;
        if (switchIndex < 0 ) { switchIndex = SwitchCount.Length-1; }
        SetSwitch(switchIndex);
    }

    public virtual void SetSwitch(int Index)
    {
        switchIndex = Index % SwitchCount.Length;
        text.text = SwitchCount[switchIndex];
    }
}
