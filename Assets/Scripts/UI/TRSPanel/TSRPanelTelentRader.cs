using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TSRPanelTelentRader : MonoBehaviour
{

    /// <summary>
    /// 说明框右
    /// </summary>
    public UIDescribe uIDescribeR;
    /// <summary>
    /// 说明框左
    /// </summary>
    public UIDescribe uIDescribeL;
    /// <summary>
    /// 天赋值的说明
    /// </summary>
    public Text TalentInfo;
    /// <summary>
    /// 天赋值雷达的多边形
    /// </summary>
    public UIPolygon TalentRaderUiPolygon;


    /// <summary>
    /// 性格的文本
    /// </summary>
    public Text NatureText;
    /// <summary>
    /// 性格的增加项的图像
    /// </summary>
    public Image NaturePlus;
    /// <summary>
    /// 性格的减少项的图像
    /// </summary>
    public Image NatureMinus;
    /// <summary>
    /// 指示性格影响能力的图像（0无 1攻击 2防御 3特攻 4特防 5攻速 6随机）；
    /// </summary>
    public Sprite[] NatureAbillitySprite;

    public void SetTalentRaderPanel(RoleInfo role)
    {
        TalentInfo.GetComponent<UICallDescribe>().DescribeUI = uIDescribeR;
        SetRader(role);
        SetNature();

    }


    /// <summary>
    /// 设置天赋值雷达
    /// </summary>
    /// <param name="role"></param>
    void SetRader(RoleInfo role)
    {
        //特防 移速 幸运 攻速 防御 攻击 生命 特攻
        Debug.Log((float)role.SpDTalent);
        Debug.Log((((float)role.SpDTalent + 1.0f) / 32.0f));
        float[] data = { 
            (((float)role.SpDTalent + 1.0f) / 32.0f ), (((float)role.MoveSpeTalent + 1.0f)  / 32.0f), (((float)role.LuckTalent  + 1.0f) / 32.0f), (((float)role.SpeTalent + 1.0f)  / 32.0f),
            (((float)role.DefTalent + 1.0f)  / 32.0f), (((float)role.AtkTalent + 1.0f)  / 32.0f),     (((float)role.HPTalent + 1.0f)  / 32.0f),   (((float)role.SpATalent  + 1.0f) / 32.0f), (((float)role.SpDTalent + 1.0f) / 32.0f ) };
        TalentRaderUiPolygon.VerticesDistances = data;
        TalentRaderUiPolygon.SetVerticesDirty();
    }

    /// <summary>
    /// 设置初始性格界面
    /// </summary>
     void SetNature()
    {
        if (StartPanelPlayerData.PlayerData != null) {
            NatureText.text = "随机";
            Vector2Int Nature = StartPanelPlayerData.PlayerData.PlayerNature;
            if (Nature.x == 0 || ( Nature.x == Nature.y && Nature.x != 6)) { NaturePlus.gameObject.SetActive(false); }
            else  { NaturePlus.gameObject.SetActive(true); NaturePlus.sprite = NatureAbillitySprite[Nature.x]; }
            if (Nature.y == 0 || ( Nature.x == Nature.y && Nature.x != 6)) { NatureMinus.gameObject.SetActive(false); }
            else { NatureMinus.gameObject.SetActive(true); NatureMinus.sprite = NatureAbillitySprite[Nature.y]; }


            if (Nature.x == 6 || Nature.y == 6)
            {
                NatureText.text = "随机";
                switch (Nature.x)
                {
                    case 1: NatureText.text += "(攻击更容易成长)"; break;
                    case 2: NatureText.text += "(防御更容易成长)"; break;
                    case 3: NatureText.text += "(特攻更容易成长)"; break;
                    case 4: NatureText.text += "(特防更容易成长)"; break;
                    case 5: NatureText.text += "(攻速更容易成长)"; break;
                }
                switch (Nature.y)
                {
                    case 1: NatureText.text += "(攻击更不易成长)"; break;
                    case 2: NatureText.text += "(防御更不易成长)"; break;
                    case 3: NatureText.text += "(特攻更不易成长)"; break;
                    case 4: NatureText.text += "(特防更不易成长)"; break;
                    case 5: NatureText.text += "(攻速更不易成长)"; break;
                }
            }
            else if (Nature.x == 1 && Nature.y == 1) { NatureText.text = "勤奋"; }
            else if (Nature.x == 1 && Nature.y == 2) { NatureText.text = "怕寂寞"; }
            else if (Nature.x == 1 && Nature.y == 3) { NatureText.text = "固执"; }
            else if (Nature.x == 1 && Nature.y == 4) { NatureText.text = "顽皮"; }
            else if (Nature.x == 1 && Nature.y == 5) { NatureText.text = "勇敢"; }

            else if (Nature.x == 2 && Nature.y == 1) { NatureText.text = "大胆"; }
            else if (Nature.x == 2 && Nature.y == 2) { NatureText.text = "坦率"; }
            else if (Nature.x == 2 && Nature.y == 3) { NatureText.text = "淘气"; }
            else if (Nature.x == 2 && Nature.y == 4) { NatureText.text = "乐天"; }
            else if (Nature.x == 2 && Nature.y == 5) { NatureText.text = "悠闲"; }

            else if (Nature.x == 3 && Nature.y == 1) { NatureText.text = "内敛"; }
            else if (Nature.x == 3 && Nature.y == 2) { NatureText.text = "慢吞吞"; }
            else if (Nature.x == 3 && Nature.y == 3) { NatureText.text = "害羞"; }
            else if (Nature.x == 3 && Nature.y == 4) { NatureText.text = "马虎"; }
            else if (Nature.x == 3 && Nature.y == 5) { NatureText.text = "冷静"; }

            else if (Nature.x == 4 && Nature.y == 1) { NatureText.text = "温和"; }
            else if (Nature.x == 4 && Nature.y == 2) { NatureText.text = "温顺"; }
            else if (Nature.x == 4 && Nature.y == 3) { NatureText.text = "慎重"; }
            else if (Nature.x == 4 && Nature.y == 4) { NatureText.text = "浮躁"; }
            else if (Nature.x == 4 && Nature.y == 5) { NatureText.text = "自大"; }

            else if (Nature.x == 5 && Nature.y == 1) { NatureText.text = "胆小"; }
            else if (Nature.x == 5 && Nature.y == 2) { NatureText.text = "急躁"; }
            else if (Nature.x == 5 && Nature.y == 3) { NatureText.text = "爽朗"; }
            else if (Nature.x == 5 && Nature.y == 4) { NatureText.text = "天真"; }
            else if (Nature.x == 5 && Nature.y == 5) { NatureText.text = "认真"; }


        }
    }



}
