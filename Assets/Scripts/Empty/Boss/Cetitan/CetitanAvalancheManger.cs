using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CetitanAvalancheManger : MonoBehaviour
{


    public CetitanAvalanche a1;
    public CetitanAvalanche a2;
    public CetitanAvalanche a3;
    public CetitanAvalanche a4;


    void Start()
    {

        Timer.Start(this, 20, () => { Destroy(this.gameObject); });
    }


    public void SetAvalanche(Cetitan parente)
    {
        if (a1 != null) { a1.ParentCetitan = parente; }
        if (a2 != null) { a2.ParentCetitan = parente; }
        if (a3 != null) { a3.ParentCetitan = parente; }
        if (a4 != null) { a4.ParentCetitan = parente; }
    }
}
