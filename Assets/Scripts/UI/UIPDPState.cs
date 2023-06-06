using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPDPState : MonoBehaviour
{
    public Image Toxic;
    public Image Paralysis;
    public Image Burn;
    public Image Sleep;
    public Image Frozen;

    // Start is called before the first frame update
    public void GetPokemonState(PlayerControler player)
    {
        if (!player.isToxicStart && !player.isParalysisStart && !player.isBurnStart && !player.isSleepStart)
        {
            GetComponent<Image>().color = new Color(0.74f, 1, 0.88f, 0);
        }
        else
        {
            GetComponent<Image>().color = new Color(0.74f, 1, 0.88f, 0.5f);
        }

        if(player.isToxicStart )
        {
            float OrangeSize = Toxic.rectTransform.rect.height;;
            Toxic.transform.GetChild(0).GetComponent<Image>().rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, (1 - player.ToxicPointFloat) * OrangeSize);
            Toxic.gameObject.SetActive(true);
            Toxic.GetComponent<UICallDescribe>().FirstText = "中毒：" + (player.ToxicPointFloat * 100).ToString() + "%";
            Toxic.GetComponent<UICallDescribe>().DescribeText = "当到达100%时进入中毒状态，每次进入房间时会收到伤害，同时特攻会大幅降低";
            Toxic.GetComponent<UICallDescribe>().TwoMode = true;
        }
        else
        {
            Toxic.gameObject.SetActive(false);
        }

        if (player.isParalysisStart)
        {
            float OrangeSize = Paralysis.rectTransform.rect.height; ;
            Paralysis.transform.GetChild(0).GetComponent<Image>().rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, (1 - player.ParalysisPointFloat) * OrangeSize);
            Paralysis.gameObject.SetActive(true);
            Paralysis.GetComponent<UICallDescribe>().FirstText = "麻痹：" + (player.ParalysisPointFloat * 100).ToString() + "%";
            Paralysis.GetComponent<UICallDescribe>().DescribeText = "当到达100%时进入麻痹状态，移动速度和攻击速度会大幅降低";
            Paralysis.GetComponent<UICallDescribe>().TwoMode = true;

        }
        else
        {
            Paralysis.gameObject.SetActive(false);
        }

        if (player.isBurnStart)
        {
            float OrangeSize = Burn.rectTransform.rect.height; ;
            Burn.transform.GetChild(0).GetComponent<Image>().rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, (1 - player.BurnPointFloat) * OrangeSize);
            Burn.gameObject.SetActive(true);
            Burn.GetComponent<UICallDescribe>().FirstText = "烧伤：" + (player.BurnPointFloat * 100).ToString() + "%";
            Burn.GetComponent<UICallDescribe>().DescribeText = "当到达100%时进入烧伤状态，每次进入房间时会收到伤害，同时攻击会大幅降低";
            Burn.GetComponent<UICallDescribe>().TwoMode = true;

        }
        else
        {
            Burn.gameObject.SetActive(false);
        }

        if (player.isSleepStart)
        {
            float OrangeSize = Sleep.rectTransform.rect.height; ;
            Sleep.transform.GetChild(0).GetComponent<Image>().rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, (1 - player.SleepPointFloat) * OrangeSize);
            Sleep.gameObject.SetActive(true);
            Sleep.GetComponent<UICallDescribe>().FirstText = "睡眠：" + (player.SleepPointFloat * 100).ToString() + "%";
            Sleep.GetComponent<UICallDescribe>().DescribeText = "当到达100%时进入睡眠状态，防御和特防会大幅降低，移动速度会巨幅降低，同时会变得无法使用招式，当受到攻击时会解除睡眠状态";
            Sleep.GetComponent<UICallDescribe>().TwoMode = true;

        }
        else
        {
            Sleep.gameObject.SetActive(false);
        }


    }
}
