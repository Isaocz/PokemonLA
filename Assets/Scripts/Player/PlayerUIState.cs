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

    public AbllityChangeUI AtkChangeImage;
    AbllityChangeUI AtkChangeObj;
    int AtkChangeLevel;

    public AbllityChangeUI DefChangeImage;
    AbllityChangeUI DefChangeObj;
    int DefChangeLevel;

    public AbllityChangeUI SpAChangeImage;
    AbllityChangeUI SpAChangeObj;
    int SpAChangeLevel;

    public AbllityChangeUI SpDChangeImage;
    AbllityChangeUI SpDChangeObj;
    int SpDChangeLevel;

    public AbllityChangeUI HpChangeImage;
    AbllityChangeUI HpChangeObj;
    int HpChangeLevel;

    public AbllityChangeUI SpeChangeImage;
    AbllityChangeUI SpeChangeObj;
    int SpeChangeLevel;

    public AbllityChangeUI MoveSpeChangeImage;
    AbllityChangeUI MoveSpeChangeObj;
    int MoveSpeChangeLevel;

    public AbllityChangeUI LuckChangeImage;
    AbllityChangeUI LuckChangeObj;
    int LuckChangeLevel;


    public Image ColdImage;
    Image ColdObj;
    bool IsCold;

    public Image CurseImage;
    Image CurseObj;
    bool IsCurse;

    public Image HaveItemImage;
    Image HaveItemObj;
    bool IsHaveItem;


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
        AtkChangeLevel = OtherState.AtkChangeLevel;
        DefChangeLevel = OtherState.DefChangeLevel;
        SpAChangeLevel = OtherState.SpAChangeLevel;
        SpDChangeLevel = OtherState.SpDChangeLevel;

        HpChangeLevel = OtherState.HpChangeLevel;
        SpeChangeLevel = OtherState.SpeChangeLevel;
        MoveSpeChangeLevel = OtherState.MoveSpeChangeLevel;
        LuckChangeLevel = OtherState.LuckChangeLevel;

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
            if (AtkChangeLevel != 0 && s == AtkChangeImage.AbllityChangeImage.sprite) { AtkChangeObj = transform.GetChild(i).GetComponent<AbllityChangeUI>(); }
            if (DefChangeLevel != 0 && s == DefChangeImage.AbllityChangeImage.sprite) { DefChangeObj = transform.GetChild(i).GetComponent<AbllityChangeUI>(); }
            if (SpAChangeLevel != 0 && s == SpAChangeImage.AbllityChangeImage.sprite) { SpAChangeObj = transform.GetChild(i).GetComponent<AbllityChangeUI>(); }
            if (SpDChangeLevel != 0 && s == SpDChangeImage.AbllityChangeImage.sprite) { SpDChangeObj = transform.GetChild(i).GetComponent<AbllityChangeUI>(); }
            if (HpChangeLevel != 0 && s == HpChangeImage.AbllityChangeImage.sprite) { HpChangeObj = transform.GetChild(i).GetComponent<AbllityChangeUI>(); }
            if (SpeChangeLevel != 0 && s == SpeChangeImage.AbllityChangeImage.sprite) { SpeChangeObj = transform.GetChild(i).GetComponent<AbllityChangeUI>(); }
            if (MoveSpeChangeLevel != 0 && s == MoveSpeChangeImage.AbllityChangeImage.sprite) { MoveSpeChangeObj = transform.GetChild(i).GetComponent<AbllityChangeUI>(); }
            if (LuckChangeLevel != 0 && s == LuckChangeImage.AbllityChangeImage.sprite) { LuckChangeObj = transform.GetChild(i).GetComponent<AbllityChangeUI>(); }



            if (IsCold && s == ColdImage.sprite) { ColdObj = transform.GetChild(i).GetComponent<Image>(); }
            if (IsCurse && s == CurseImage.sprite) { CurseObj = transform.GetChild(i).GetComponent<Image>(); }

        }
        SortAbllityChangeMark();
    }

    /// <summary>
    /// 0减速 1致盲 2冰冻 3中毒 4麻痹 5烧伤 6睡眠 7恐惧 8气势头戴 9混乱 10着迷 11寒冷 12:诅咒 13:带有持有物
    /// </summary>
    /// <param name="StateNum"></param>
    // Start is called before the first frame update
    public void StatePlus(int StateNum )
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
                if (!IsCold)
                {
                    ColdObj = Instantiate(ColdImage, transform.position, Quaternion.identity, transform);
                    IsCold = true;
                }
                break;
            case 12:
                if (!IsCurse)
                {
                    CurseObj = Instantiate(CurseImage, transform.position, Quaternion.identity, transform);
                    IsCurse = true;
                }
                break;
            case 13:
                if (!IsHaveItem)
                {
                    HaveItemObj = Instantiate(HaveItemImage, transform.position, Quaternion.identity, transform);
                    IsHaveItem = true;
                }
                break;
        }
        SortAbllityChangeMark();
    }

    /// <summary>
    /// 0攻击改变 1防御改变 2特攻改变 3特防改变 4HP改变 5攻速改变 6移速改变 7幸运改变
    /// </summary>
    /// <param name="AbllityIndex"></param>
    /// <param name="ChangeLevel"></param>
    public void AbllityChange(int AbllityIndex , int ChangeLevel)
    {
        switch (AbllityIndex){
            case 0:
                if (ChangeLevel == 0) 
                {
                    if (AtkChangeObj != null) {
                        Destroy(AtkChangeObj.gameObject);
                    }
                    AtkChangeLevel = 0;
                }
                else
                {
                    if (/*AtkChangeLevel == 0 &&  */AtkChangeObj == null) { AtkChangeObj = Instantiate(AtkChangeImage, transform.position, Quaternion.identity, transform); }
                    AtkChangeLevel = ChangeLevel;
                    AtkChangeObj.AbllityLevel = AtkChangeLevel;
                    AtkChangeObj.ChangeAblityLevel();
                }
                break;
            case 1:
                if (ChangeLevel == 0)
                {
                    if (DefChangeObj != null)
                    {
                        Destroy(DefChangeObj.gameObject);
                    }
                    DefChangeLevel = 0;
                }
                else
                {
                    if (/*DefChangeLevel == 0 && */ DefChangeObj == null) {  DefChangeObj = Instantiate(DefChangeImage, transform.position, Quaternion.identity, transform); }
                    DefChangeLevel = ChangeLevel;
                    DefChangeObj.AbllityLevel = DefChangeLevel;
                    DefChangeObj.ChangeAblityLevel();
                }
                break;
            case 2:
                if (ChangeLevel == 0)
                {
                    if (SpAChangeObj != null)
                    {
                        Destroy(SpAChangeObj.gameObject);
                    }
                    SpAChangeLevel = 0;
                }
                else
                {
                    if (/* SpAChangeLevel == 0 && */ SpAChangeObj == null) { SpAChangeObj = Instantiate(SpAChangeImage, transform.position, Quaternion.identity, transform); }
                    SpAChangeLevel = ChangeLevel;
                    SpAChangeObj.AbllityLevel = SpAChangeLevel;
                    SpAChangeObj.ChangeAblityLevel();
                }
                break;
            case 3:
                if (ChangeLevel == 0)
                {
                    if (SpDChangeObj != null)
                    {
                        Destroy(SpDChangeObj.gameObject);
                    }
                    SpDChangeLevel = 0;
                }
                else
                {
                    if (/*SpDChangeLevel == 0 &&  */SpDChangeObj == null) { SpDChangeObj = Instantiate(SpDChangeImage, transform.position, Quaternion.identity, transform); }
                    SpDChangeLevel = ChangeLevel;
                    SpDChangeObj.AbllityLevel = SpDChangeLevel;
                    SpDChangeObj.ChangeAblityLevel();
                }
                break;
            case 4:
                if (ChangeLevel == 0)
                {
                    if (HpChangeObj != null)
                    {
                        Destroy(HpChangeObj.gameObject);
                    }
                    HpChangeLevel = 0;
                }
                else
                {
                    if (/*SpDChangeLevel == 0 &&  */HpChangeObj == null) { HpChangeObj = Instantiate(HpChangeImage, transform.position, Quaternion.identity, transform); }
                    HpChangeLevel = ChangeLevel;
                    HpChangeObj.AbllityLevel = HpChangeLevel;
                    HpChangeObj.ChangeAblityLevel();
                }
                break;
            case 5:
                if (ChangeLevel == 0)
                {
                    if (SpeChangeObj != null)
                    {
                        Destroy(SpeChangeObj.gameObject);
                    }
                    SpeChangeLevel = 0;
                }
                else
                {
                    if (/*SpDChangeLevel == 0 &&  */SpeChangeObj == null) { SpeChangeObj = Instantiate(SpeChangeImage, transform.position, Quaternion.identity, transform); }
                    SpeChangeLevel = ChangeLevel;
                    SpeChangeObj.AbllityLevel = SpeChangeLevel;
                    SpeChangeObj.ChangeAblityLevel();
                }
                break;
            case 6:
                if (ChangeLevel == 0)
                {
                    if (MoveSpeChangeObj != null)
                    {
                        Destroy(MoveSpeChangeObj.gameObject);
                    }
                    MoveSpeChangeLevel = 0;
                }
                else
                {
                    if (/*SpDChangeLevel == 0 &&  */MoveSpeChangeObj == null) { MoveSpeChangeObj = Instantiate(MoveSpeChangeImage, transform.position, Quaternion.identity, transform); }
                    MoveSpeChangeLevel = ChangeLevel;
                    MoveSpeChangeObj.AbllityLevel = MoveSpeChangeLevel;
                    MoveSpeChangeObj.ChangeAblityLevel();
                }
                break;
            case 7:
                if (ChangeLevel == 0)
                {
                    if (LuckChangeObj != null)
                    {
                        Destroy(LuckChangeObj.gameObject);
                    }
                    LuckChangeLevel = 0;
                }
                else
                {
                    if (/*SpDChangeLevel == 0 &&  */LuckChangeObj == null) { LuckChangeObj = Instantiate(LuckChangeImage, transform.position, Quaternion.identity, transform); }
                    LuckChangeLevel = ChangeLevel;
                    LuckChangeObj.AbllityLevel = LuckChangeLevel;
                    LuckChangeObj.ChangeAblityLevel();
                }
                break;
        }
        SortAbllityChangeMark();
    }

    /// <summary>
    /// 移除所有能力变化
    /// </summary>
    public void RemoveAllAbllityChangeMark()
    {
        Debug.Log("Xxx");
        if (AtkChangeObj != null) { Destroy(AtkChangeObj.gameObject); AtkChangeLevel = 0; }
        if (DefChangeObj != null) { Destroy(DefChangeObj.gameObject); DefChangeLevel = 0; }
        if (SpAChangeObj != null) { Destroy(SpAChangeObj.gameObject); SpAChangeLevel = 0; Debug.Log("Xxx"); }
        if (SpDChangeObj != null) { Destroy(SpDChangeObj.gameObject); SpDChangeLevel = 0; }
        if (SpeChangeObj != null) { Destroy(SpeChangeObj.gameObject); SpeChangeLevel = 0; }
        if (MoveSpeChangeObj != null) { Destroy(MoveSpeChangeObj.gameObject); MoveSpeChangeLevel = 0; }
        if (HpChangeObj != null) { Destroy(HpChangeObj.gameObject); HpChangeLevel = 0; }
        if (LuckChangeObj != null) { Destroy(LuckChangeObj.gameObject); LuckChangeLevel = 0; }
    }

    /// <summary>
    /// 排序能力变化标签
    /// </summary>
    public void SortAbllityChangeMark()
    {
        if (LuckChangeObj != null) { LuckChangeObj.transform.SetAsFirstSibling(); }
        if (MoveSpeChangeObj != null) { MoveSpeChangeObj.transform.SetAsFirstSibling(); }
        if (SpeChangeObj != null) { SpeChangeObj.transform.SetAsFirstSibling(); }
        if (SpDChangeObj != null) { SpDChangeObj.transform.SetAsFirstSibling(); }
        if (SpAChangeObj != null) { SpAChangeObj.transform.SetAsFirstSibling(); }
        if (DefChangeObj != null) { DefChangeObj.transform.SetAsFirstSibling(); }
        if (AtkChangeObj != null) { AtkChangeObj.transform.SetAsFirstSibling(); }
        if (HpChangeObj != null) { HpChangeObj.transform.SetAsFirstSibling(); }
    }


    /// <summary>
    ///  0减速 1致盲 2冰冻 3中毒 4麻痹 5烧伤 6睡眠 7恐惧 8气势头戴 9混乱 10着迷  11寒冷 12:诅咒 13:带有持有物
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
                if (IsCold)
                {
                    Destroy(ColdObj.gameObject);
                    IsCold = false;
                }
                break;
            case 12:
                if (IsCurse)
                {
                    Destroy(CurseObj.gameObject);
                    IsCurse = false;
                }
                break;
            case 13:
                if (IsHaveItem)
                {
                    Destroy(HaveItemObj.gameObject);
                    IsHaveItem = false;
                }
                break;

        }
        SortAbllityChangeMark();
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
            case 12:
                if (IsCurse)
                {
                    float OrangenalSize = CurseObj.rectTransform.rect.height;
                    CurseObj.transform.GetChild(0).GetComponent<Image>().rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, (1 - Per) * OrangenalSize);
                }
                break;
        }
        SortAbllityChangeMark();
    }

}
