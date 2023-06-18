using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillShopBlockBuyButton : MonoBehaviour
{
    GameObject ImageParent;
    GameObject SoldOut;
    Button ThisButton;
    ParticleSystem BuyPS;
    int SkillPrice;

    private void Awake()
    {
        ThisButton = GetComponent<Button>();
        BuyPS = transform.parent.GetChild(2).GetComponent<ParticleSystem>();
        SoldOut = transform.parent.GetChild(3).gameObject;
        ImageParent = transform.GetChild(1).gameObject;
    }


    public void GetSkillPrice(int Price)
    {
        SkillPrice = Price;
        for (; ImageParent.transform.childCount <= Price-1;)
        {
            Instantiate(ImageParent.transform.GetChild(0), Vector3.zero, Quaternion.identity, ImageParent.transform);
        }
    }

    public void SkillShopBlockPressed()
    {
        ThisButton.enabled = false;
        BuyPS.gameObject.SetActive(true);
        SoldOut.SetActive(true);
        ThisButton.GetComponent<Image>().color = new Color(0.7f, 0.7f, 0.7f ,1) ;
        for(int i = 0; i < ThisButton.transform.GetChild(1).childCount; i++)
        {
            ThisButton.transform.GetChild(1).GetChild(i).GetComponent<Image>().color = new Color(0.7f, 0.7f, 0.7f, 1);
        }
    }
}
