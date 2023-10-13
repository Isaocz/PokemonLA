using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIKeyBoardChange : MonoBehaviour
{
    public void Skill1KeyBoard()
    {
        UIKeyBoard.instance.OpenKeybindUI("Skill1");
    }
    public void Skill2KeyBoard()
    {
        UIKeyBoard.instance.OpenKeybindUI("Skill2");
    }
    public void Skill3KeyBoard()
    {
        UIKeyBoard.instance.OpenKeybindUI("Skill3");
    }
    public void Skill4KeyBoard()
    {
        UIKeyBoard.instance.OpenKeybindUI("Skill4");
    }

}
