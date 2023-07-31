using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillShopStrengthenPPUPButton : MonoBehaviour
{
    GameObject SoldOut;
    Button ThisButton;
    ParticleSystem BuyPS;


    private void Awake()
    {
        ThisButton = GetComponent<Button>();
        BuyPS = transform.parent.GetChild(4).GetComponent<ParticleSystem>();
        SoldOut = transform.parent.GetChild(6).gameObject;
    }


    public void SkillShopBlockPressed()
    {
        ThisButton.enabled = false;
        BuyPS.gameObject.SetActive(true);
        SoldOut.SetActive(true);
        ThisButton.GetComponent<Image>().color = new Color(0.7f, 0.7f, 0.7f, 1);
        ThisButton.transform.GetChild(1).GetComponent<Image>().color = new Color(0.7f, 0.7f, 0.7f, 1);
    }
}
