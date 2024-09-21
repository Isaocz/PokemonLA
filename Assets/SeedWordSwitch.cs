using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedWordSwitch : UISwitch
{
    public int SeedWordOrder;
    public TRSPanelSeedPanel ParentSeedPanel;


    private void Start()
    {
        SetSwitch(SwitchIndex);
    }

    public override void SetSwitch(int Index)
    {
        base.SetSwitch(Index);
        ParentSeedPanel.SetSeedByOrder(SeedWordOrder , SwitchIndex);
    }
    


}
