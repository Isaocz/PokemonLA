using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCUICatchCamera : MonoBehaviour
{
    // Start is called before the first frame update
    private void Awake()
    {
        gameObject.GetComponent<Canvas>().worldCamera =  GameObject.Find("Main Camera").GetComponent<Camera>();
    }
}
