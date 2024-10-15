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

    private void Awake()
    {
        SetSwitch(SwitchIndex);
    }

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

    public void RemoveAllSwitch()
    {
        switchIndex = 0;
        SwitchCount = new string[] { };
    }

    public void AddSwitch(string s)
    {
        List<string> _list = new List<string>(SwitchCount);
        _list.Add(s);
        SwitchCount = _list.ToArray();
        switchIndex = 0;
        SetSwitch(switchIndex);
    }

}
