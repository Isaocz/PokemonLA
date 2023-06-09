using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiePanel : MonoBehaviour
{


    public void Die(string Name)
    {
        Text text = transform.GetChild(0).GetComponent<Text>();
        text.text = Name + "µÄÑÛÇ°Ò»Æ¬ºÚ°µ...";
    }
}
