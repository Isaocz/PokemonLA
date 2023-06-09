using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIState : MonoBehaviour
{
    public Image SpeedDownImage;
    Image SpeedDownObj;
    bool IsSpeedDown = false;

    public Image BlindingImage;
    Image BlindingObj;
    bool IsBlinding = false;

    public Image FrozenImage;
    Image FrozenObj;
    bool IsFrozen = false;

    public Image ToxicImage;
    Image ToxicObj;
    bool IsToxic = false;

    public Image ParalysisImage;
    Image ParalysisObj;
    bool IsParalysis = false;


    public Image BurnImage;
    Image BurnObj;
    bool IsBurn = false;

    public Image SleepImage;
    Image SleepObj;
    bool IsSleep = false;

    public Image FearImage;
    Image FearObj;
    bool IsFear = false;

    public Image EndureImage;
    Image EndureObj;
    bool IsEndure = false;


    // Start is called before the first frame update
    public void StatePlus(int StateNum)
    {
        switch (StateNum) {
            case 0:
                if (!IsSpeedDown) {
                    SpeedDownObj = Instantiate(SpeedDownImage, transform.position , Quaternion.identity, transform);
                    IsSpeedDown = true;
                }
                break;
            case 1:
                if (!IsBlinding)
                {
                    BlindingObj = Instantiate(BlindingImage, transform.position , Quaternion.identity, transform);
                    IsBlinding = true;
                }
                break;
            case 2:
                if (!IsFrozen)
                {
                    FrozenObj = Instantiate(FrozenImage, transform.position , Quaternion.identity, transform);
                    IsFrozen = true;
                }
                break;
            case 3:
                if (!IsToxic)
                {
                    ToxicObj = Instantiate(ToxicImage, transform.position , Quaternion.identity, transform);
                    IsToxic = true;
                }
                break;
            case 4:
                if (!IsParalysis)
                {
                    ParalysisObj = Instantiate(ParalysisImage, transform.position , Quaternion.identity, transform);
                    IsParalysis = true;
                }
                break;
            case 5:
                if (!IsBurn)
                {
                    BurnObj = Instantiate(BurnImage, transform.position , Quaternion.identity, transform);
                    IsBurn = true;
                }
                break;
            case 6:
                if (!IsSleep)
                {
                    SleepObj = Instantiate(SleepImage, transform.position , Quaternion.identity, transform);
                    IsSleep = true;
                }
                break;
            case 7:
                if (!IsFear)
                {
                    FearObj = Instantiate(FearImage, transform.position, Quaternion.identity, transform);
                    IsFear = true;
                }
                break;
            case 8:
                if (!IsEndure)
                {
                    EndureObj = Instantiate(EndureImage, transform.position, Quaternion.identity, transform);
                    IsEndure = true;
                }
                break;
        }
    }

    public void StateDestory(int StateNum)
    {
        switch (StateNum)
        {
            case 0:
                if (IsSpeedDown)
                {
                    Destroy(SpeedDownObj.gameObject);
                    IsSpeedDown = false;
                }
                break;
            case 1:
                if (IsBlinding)
                {
                    Destroy(BlindingObj.gameObject);
                    IsBlinding = false;
                }
                break;
            case 2:
                if (IsFrozen)
                {
                    Destroy(FrozenObj.gameObject);
                    IsFrozen = false;
                }
                break;
            case 3:
                if (IsToxic)
                {
                    Destroy(ToxicObj.gameObject);
                    IsToxic = false;
                }
                break;
            case 4:
                if (IsParalysis)
                {
                    Destroy(ParalysisObj.gameObject);
                    IsParalysis = false;
                }
                break;
            case 5:
                if (IsBurn)
                {
                    Destroy(BurnObj.gameObject);
                    IsBurn = false;
                }
                break;
            case 6:
                if (IsSleep)
                {
                    Destroy(SleepObj.gameObject);
                    IsSleep = false;
                }
                break;
            case 7:
                if (IsFear)
                {
                    Destroy(FearObj.gameObject);
                    IsFear = false;
                }
                break;
            case 8:
                if (IsEndure)
                {
                    Destroy(EndureObj.gameObject);
                    IsEndure = false;
                }
                break;
        }
    }

    public void StateSlowUP(int StateNum, float Per)
    {
        switch (StateNum)
        {
            case 1:
                if (IsBlinding)
                {
                    float OrangenalSize = BlindingObj.rectTransform.rect.height;
                    BlindingObj.transform.GetChild(0).GetComponent<Image>().rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, (1 - Per) * OrangenalSize);
                }
                break;
            case 2:
                if (IsFrozen)
                {
                    float OrangenalSize = FrozenObj.rectTransform.rect.height;
                    FrozenObj.transform.GetChild(0).GetComponent<Image>().rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, (1 - Per) * OrangenalSize);
                    Debug.Log(OrangenalSize);
                    Debug.Log(FrozenObj.transform.GetChild(0).GetComponent<Image>().rectTransform.rect.height);
                }
                break;
            case 3:
                if (IsToxic)
                {
                    float OrangenalSize = ToxicObj.rectTransform.rect.height;
                    ToxicObj.transform.GetChild(0).GetComponent<Image>().rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, (1 - Per) * OrangenalSize);
                }
                break;
            case 4:
                if (IsParalysis)
                {
                    float OrangenalSize = ParalysisObj.rectTransform.rect.height;
                    ParalysisObj.transform.GetChild(0).GetComponent<Image>().rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, (1 - Per) * OrangenalSize);
                }
                break;
            case 5:
                if (IsBurn)
                {
                    float OrangenalSize = BurnObj.rectTransform.rect.height;
                    BurnObj.transform.GetChild(0).GetComponent<Image>().rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, (1 - Per) * OrangenalSize);
                }
                break;
            case 6:
                if (IsSleep)
                {
                    float OrangenalSize = SleepObj.rectTransform.rect.height;
                    SleepObj.transform.GetChild(0).GetComponent<Image>().rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, (1 - Per) * OrangenalSize);
                }
                break;
            case 7:
                if (IsFear)
                {
                    float OrangenalSize = FearObj.rectTransform.rect.height;
                    FearObj.transform.GetChild(0).GetComponent<Image>().rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, (1 - Per) * OrangenalSize);
                }
                break;
        }
    }

}
