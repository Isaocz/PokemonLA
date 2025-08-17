using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SudokuButton : EnviromentButton
{

    public Sudoku66Manager parentManager;

    public override void SwitchONEvent()
    {
        base.SwitchONEvent();
        parentManager.CheckInputAnwser();
    }
}
