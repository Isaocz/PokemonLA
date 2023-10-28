using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatonPass : Skill
{
    // Start is called before the first frame update
    void Start()
    {
        player.isBatonPass = true;
    }


    private void OnDestroy()
    {
        player.isBatonPass = false;
    }
}
