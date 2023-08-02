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
    public Image ToxicObj;
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

    public Image ConfusionImage;
    Image ConfusionObj;
    bool IsConfusion;

    public Image InfatuationImage;
    Image InfatuationObj;
    bool IsInfatuation;

    public Image AtkUpImage;
    Image AtkUpObj;
    bool IsAtkUp;

    public Image AtkDownImage;
    Image AtkDownObj;
    bool IsAtkDown;

    public Image DefUpImage;
    Image DefUpObj;
    bool IsDefUp;

    public Image DefDownImage;
    Image DefDownObj;
    bool IsDefDown;

    public Image SpAUpImage;
    Image SpAUpObj;
    bool IsSpAUp;

    public Image SpADownImage;
    Image SpADownObj;
    bool IsSpADown;

    public Image SpDUpImage;
    Image SpDUpObj;
    bool IsSpDUp;

    public Image SpDDownImage;
    Image SpDDownObj;
    bool IsSpDDown;

    public Image ColdImage;
    Image ColdObj;
    bool IsCold;

    public Image CurseImage;
    Image CurseObj;
    bool IsCurse;


    public void InstanceObjWhenEvlo(PlayerUIState OtherState)
    {
        IsSpeedDown = OtherState.IsSpeedDown;
        IsBlinding = OtherState.IsBlinding;
        IsFrozen = OtherState.IsFrozen;
        IsToxic = OtherState.IsToxic;
        IsParalysis = OtherState.IsParalysis;
        IsBurn = OtherState.IsBurn;
        IsSleep = OtherState.IsSleep;
        IsFear = OtherState.IsFear;
        IsEndure = OtherState.IsEndure;
        IsConfusion = OtherState.IsConfusion;

        IsInfatuation = OtherState.IsInfatuation;
        IsAtkUp = OtherState.IsAtkUp;
        IsAtkDown = OtherState.IsAtkDown;
        IsDefUp = OtherState.IsDefUp;
        IsDefDown = OtherState.IsDefDown;
        IsSpAUp = OtherState.IsSpAUp;
        IsSpADown = OtherState.IsSpADown;
        IsSpDUp = OtherState.IsSpDUp;
        IsSpDDown = OtherState.IsSpDDown;
        IsCold = OtherState.IsCold;
        IsCurse = OtherState.IsCurse;
        for (int i = 0; i < transform.childCount; i++) {

            Debug.Log(transform.childCount);
            Sprite s = transform.GetChild(i).GetComponent<Image>().sprite;



            if (IsSpeedDown && s == SpeedDownImage.sprite) { SpeedDownObj = transform.GetChild(i).GetComponent<Image>(); }
            if (IsBlinding && s == BlindingImage.sprite) { BlindingObj = transform.GetChild(i).GetComponent<Image>(); }
            if (IsFrozen && s == FrozenImage.sprite) { FrozenObj = transform.GetChild(i).GetComponent<Image>(); }
            if (IsToxic && s == ToxicImage.sprite) { 
                ToxicObj = transform.GetChild(i).GetComponent<Image>();
                Debug.Log(ToxicObj);
            }
            if (IsParalysis && s == ParalysisImage.sprite) { ParalysisObj = transform.GetChild(i).GetComponent<Image>(); }
            if (IsBurn && s == BurnImage.sprite) { BurnObj = transform.GetChild(i).GetComponent<Image>(); }
            if (IsSleep && s == SleepImage.sprite) { SleepObj = transform.GetChild(i).GetComponent<Image>(); }
            if (IsFear && s == FearImage.sprite) { FearObj = transform.GetChild(i).GetComponent<Image>(); }
            if (IsEndure && s == EndureImage.sprite) { EndureObj = transform.GetChild(i).GetComponent<Image>(); }
            if (IsConfusion && s == ConfusionImage.sprite) { ConfusionObj = transform.GetChild(i).GetComponent<Image>(); }

            if (IsInfatuation && s == InfatuationImage.sprite) { InfatuationObj = transform.GetChild(i).GetComponent<Image>(); }
            if (IsAtkUp && s == AtkUpImage.sprite) { AtkUpObj = transform.GetChild(i).GetComponent<Image>(); }
            if (IsAtkDown && s == AtkDownImage.sprite) { AtkDownObj = transform.GetChild(i).GetComponent<Image>(); }
            if (IsDefUp && s == DefUpImage.sprite) { DefUpObj = transform.GetChild(i).GetComponent<Image>(); }
            if (IsDefDown && s == DefDownImage.sprite) { DefDownObj = transform.GetChild(i).GetComponent<Image>(); }
            if (IsSpAUp && s == SpAUpImage.sprite) { SpAUpObj = transform.GetChild(i).GetComponent<Image>(); }
            if (IsSpADown && s == SpADownImage.sprite) { SpADownObj = transform.GetChild(i).GetComponent<Image>(); }
            if (IsSpDUp && s == SpDUpImage.sprite) { SpDUpObj = transform.GetChild(i).GetComponent<Image>(); }
            if (IsSpDDown && s == SpDDownImage.sprite) { SpDDownObj = transform.GetChild(i).GetComponent<Image>(); }
            if (IsCold && s == ColdImage.sprite) { ColdObj = transform.GetChild(i).GetComponent<Image>(); }
            if (IsCurse && s == CurseImage.sprite) { CurseObj = transform.GetChild(i).GetComponent<Image>(); }

        }
    }

    /// <summary>
    /// 0¼õËÙ 1ÖÂÃ¤ 2±ù¶³ 3ÖÐ¶¾ 4Âé±Ô 5ÉÕÉË 6Ë¯Ãß 7¿Ö¾å 8ÆøÊÆÍ·´÷ 9»ìÂÒ 10×ÅÃÔ 11¹¥»÷ÉÏÉý 12¹¥»÷ÏÂ½µ 13·ÀÓùÉÏÉý 14·ÀÓùÏÂ½µ 15ÌØ¹¥ÉÏÉý 16ÌØ¹¥ÏÂ½µ 17ÌØ·ÀÉÏÉý 18ÌØ·ÀÏÂ½µ 19º®Àä 20:×çÖä
    /// </summary>
    /// <param name="StateNum"></param>
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
                    Debug.Log(BlindingObj.rectTransform.rect.height);
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
            case 9:
                if (!IsConfusion)
                {
                    ConfusionObj = Instantiate(ConfusionImage, transform.position, Quaternion.identity, transform);
                    IsConfusion = true;
                }
                break;
            case 10:
                if (!IsInfatuation)
                {
                    InfatuationObj = Instantiate(InfatuationImage, transform.position, Quaternion.identity, transform);
                    IsInfatuation = true;
                    Debug.Log(InfatuationObj.rectTransform.rect.height);
                }
                break;
            case 11:
                if (!IsAtkUp)
                {
                    AtkUpObj = Instantiate(AtkUpImage, transform.position, Quaternion.identity, transform);
                    IsAtkUp = true;
                }
                break;
            case 12:
                if (!IsAtkDown)
                {
                    AtkDownObj = Instantiate(AtkDownImage, transform.position, Quaternion.identity, transform);
                    IsAtkDown = true;
                }
                break;
            case 13:
                if (!IsDefUp)
                {
                    DefUpObj = Instantiate(DefUpImage, transform.position, Quaternion.identity, transform);
                    IsDefUp = true;
                }
                break;
            case 14:
                if (!IsDefDown)
                {
                    DefDownObj = Instantiate(DefDownImage, transform.position, Quaternion.identity, transform);
                    IsDefDown = true;
                }
                break;
            case 15:
                if (!IsSpAUp)
                {
                    SpAUpObj = Instantiate(SpAUpImage, transform.position, Quaternion.identity, transform);
                    IsSpAUp = true;
                }
                break;
            case 16:
                if (!IsSpADown)
                {
                    SpADownObj = Instantiate(SpADownImage, transform.position, Quaternion.identity, transform);
                    IsSpADown = true;
                }
                break;
            case 17:
                if (!IsSpDUp)
                {
                    SpDUpObj = Instantiate(SpDUpImage, transform.position, Quaternion.identity, transform);
                    IsSpDUp = true;
                }
                break;
            case 18:
                if (!IsSpDDown)
                {
                    SpDDownObj = Instantiate(SpDDownImage, transform.position, Quaternion.identity, transform);
                    IsSpDDown = true;
                }
                break;
            case 19:
                if (!IsCold)
                {
                    ColdObj = Instantiate(ColdImage, transform.position, Quaternion.identity, transform);
                    IsCold = true;
                }
                break;
            case 20:
                if (!IsCurse)
                {
                    CurseObj = Instantiate(CurseImage, transform.position, Quaternion.identity, transform);
                    IsCurse = true;
                }
                break;
        }
    }

    /// <summary>
    ///  0¼õËÙ 1ÖÂÃ¤ 2±ù¶³ 3ÖÐ¶¾ 4Âé±Ô 5ÉÕÉË 6Ë¯Ãß 7¿Ö¾å 8ÆøÊÆÍ·´÷ 9»ìÂÒ 10×ÅÃÔ 11¹¥»÷ÉÏÉý 12¹¥»÷ÏÂ½µ 13·ÀÓùÉÏÉý 14·ÀÓùÏÂ½µ 15ÌØ¹¥ÉÏÉý 16ÌØ¹¥ÏÂ½µ 17ÌØ·ÀÉÏÉý 18ÌØ·ÀÏÂ½µ
    /// </summary>
    /// <param name="StateNum"></param>
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
            case 9:
                if (IsConfusion)
                {
                    Destroy(ConfusionObj.gameObject);
                    IsConfusion = false;
                }
                break;
            case 10:
                if (IsInfatuation)
                {
                    Destroy(InfatuationObj.gameObject);
                    IsInfatuation = false;
                }
                break;
            case 11:
                if (IsAtkUp)
                {
                    Destroy(AtkUpObj.gameObject);
                    IsAtkUp = false;
                }
                break;
            case 12:
                if (IsAtkDown)
                {
                    Destroy(AtkDownObj.gameObject);
                    IsAtkDown = false;
                }
                break;
            case 13:
                if (IsDefUp)
                {
                    Destroy(DefUpObj.gameObject);
                    IsDefUp = false;
                }
                break;
            case 14:
                if (IsDefDown)
                {
                    Destroy(DefDownObj.gameObject);
                    IsDefDown = false;
                }
                break;
            case 15:
                if (IsSpAUp)
                {
                    Destroy(SpAUpObj.gameObject);
                    IsSpAUp = false;
                }
                break;
            case 16:
                if (IsSpADown)
                {
                    Destroy(SpADownObj.gameObject);
                    IsSpADown = false;
                }
                break;
            case 17:
                if (IsSpDUp)
                {
                    Destroy(SpDUpObj.gameObject);
                    IsSpDUp = false;
                }
                break;
            case 18:
                if (IsSpDDown)
                {
                    Destroy(SpDDownObj.gameObject);
                    IsSpDDown = false;
                }
                break;
            case 19:
                if (IsCold)
                {
                    Destroy(ColdObj.gameObject);
                    IsCold = false;
                }
                break;
            case 20:
                if (IsCurse)
                {
                    Destroy(CurseObj.gameObject);
                    IsCurse = false;
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
                    //Debug.Log(OrangenalSize);
                    //Debug.Log(FrozenObj.transform.GetChild(0).GetComponent<Image>().rectTransform.rect.height);
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
            case 9:
                if (IsConfusion)
                {
                    float OrangenalSize = ConfusionObj.rectTransform.rect.height;
                    ConfusionObj.transform.GetChild(0).GetComponent<Image>().rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, (1 - Per) * OrangenalSize);
                }
                break;
            case 10:
                if (IsInfatuation)
                {
                    float OrangenalSize = InfatuationObj.rectTransform.rect.height;
                    InfatuationObj.transform.GetChild(0).GetComponent<Image>().rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, (1 - Per) * OrangenalSize);
                }
                break;
            case 20:
                if (IsCurse)
                {
                    float OrangenalSize = CurseObj.rectTransform.rect.height;
                    CurseObj.transform.GetChild(0).GetComponent<Image>().rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, (1 - Per) * OrangenalSize);
                }
                break;
        }
    }

}
