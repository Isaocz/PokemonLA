using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MewHpLeftUI : MonoBehaviour
{
    private Text healthText;
    private Empty ParentEmpty;
    void Start()
    {
        ParentEmpty = transform.parent.parent.GetComponent<Empty>();
        healthText = GetComponent<Text>();
    }

    void Update()
    {
        if (ParentEmpty != null)
        {
            float healthPercentage = (float)ParentEmpty.EmptyHp / ParentEmpty.maxHP * 100f;
            healthText.text = healthPercentage.ToString("F1") + "%";
        }
    }
}
