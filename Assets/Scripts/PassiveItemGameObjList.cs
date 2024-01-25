using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveItemGameObjList : MonoBehaviour
{
    public static PassiveItemGameObjList ObjList;
    public GameObject[] List;

    private void Awake()
    {
        ObjList = this;
    }

}
