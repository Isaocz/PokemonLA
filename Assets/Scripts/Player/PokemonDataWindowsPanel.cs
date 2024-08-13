using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PokemonDataWindowsPanel : MonoBehaviour
{
    public static PokemonDataWindowsPanel InPokemonData;
    public Pokemon player;
    GameObject HeadUI;
    GameObject TypeMarkUI;
    GameObject MoneyAndExpUI;
    GameObject StateUI;
    GameObject StrengthDataUI;
    GameObject TypeDefUI;
    Text NatureText;
    Text AbilityText;

    private void Awake()
    {
        InPokemonData = this;
        
    }


    private void OnEnable()
    {
        Invoke("RestorePokemonDataPanel", 0.0001f);
        PlayerControler p = GameObject.FindObjectOfType<PlayerControler>();
        if (p != null)
        {
            if (p.playerData == null) { p.playerData = p.transform.GetComponent<PlayerData>(); }
            player = GameObject.FindObjectOfType<PlayerControler>();
            HeadUI = transform.GetChild(0).gameObject;
            TypeMarkUI = transform.GetChild(1).gameObject;
            MoneyAndExpUI = transform.GetChild(2).gameObject;
            StateUI = transform.GetChild(3).gameObject;
            StrengthDataUI = transform.GetChild(4).gameObject;
            TypeDefUI = transform.GetChild(5).gameObject;
            NatureText = transform.GetChild(6).gameObject.GetComponent<Text>();
            AbilityText = transform.GetChild(7).gameObject.GetComponent<Text>();

            GetHead();
            GetTypeMark();
            GetMoneyAndExp();
            GetState();
            GetPlayerStrengthData();
            GetTypeDefUI();
            GetNature();
            GetAbility();
        }
    }

    public void RestorePokemonDataPanel()
    {
        player = GameObject.FindObjectOfType<PlayerControler>();
        HeadUI = transform.GetChild(0).gameObject;
        TypeMarkUI = transform.GetChild(1).gameObject;
        MoneyAndExpUI = transform.GetChild(2).gameObject;
        StateUI = transform.GetChild(3).gameObject;
        StrengthDataUI = transform.GetChild(4).gameObject;
        TypeDefUI = transform.GetChild(5).gameObject;
        NatureText = transform.GetChild(6).gameObject.GetComponent<Text>();
        AbilityText = transform.GetChild(7).gameObject.GetComponent<Text>();

        GetHead();
        GetTypeMark();
        GetMoneyAndExp();
        GetState();
        GetPlayerStrengthData();
        GetTypeDefUI();
        GetNature();
        GetAbility();
    }

    void GetHead()
    {
        
        PlayerControler playerControler = player.GetComponent<PlayerControler>();
        HeadUI.GetComponent<Image>().sprite = playerControler.PlayerHead;
        HeadUI.transform.GetChild(0).GetComponent<Text>().text = playerControler.PlayerNameChinese;
        string PokemonD1 = "";
        string PokemonD2;
        switch (playerControler.PlayerBodySize)
        {
            case 0:
                PokemonD1 = "小体型的宝可梦，可以进入一些狭小的道路，但是对于一些巨大的路障没有办法";
                break;
            case 1:
                PokemonD1 = "中体型的宝可梦，能勉强进入一些狭小的道路，但是对于一些巨大的路障没有办法"; 
                break;
            case 2:
                PokemonD1 = "大体型的宝可梦，完全无法进入狭小的道路，但是可以轻而易举的摧毁一些障碍物";
                break;
        }
        PokemonD2 = "这只宝可梦好像每升" + playerControler.GetSkillLevel +  "级就会有一次灵感爆发！";
        UICallDescribe s1 = HeadUI.transform.GetChild(0).GetComponent<UICallDescribe>();
        UICallDescribe s2 = HeadUI.transform.GetComponent<UICallDescribe>();
        s1.FirstText = PokemonD1;s1.DescribeText = PokemonD2;s1.TwoMode = true;
        s2.FirstText = PokemonD1;s2.DescribeText = PokemonD2;s2.TwoMode = true;
    }

    void GetTypeMark()
    {
        TypeMarkUI.transform.GetChild(0).GetComponent<UIPokemonDataPanelTypemark>().GetChildTypeMark(player.GetComponent<PlayerControler>().PlayerType01);
        TypeMarkUI.transform.GetChild(1).GetComponent<UIPokemonDataPanelTypemark>().GetChildTypeMark(player.GetComponent<PlayerControler>().PlayerType02);
        TypeMarkUI.transform.GetChild(2).GetComponent<UIPokemonDataPanelTypemark>().GetChildTypeMark(player.GetComponent<PlayerControler>().PlayerTeraTypeJOR == 0 ? player.GetComponent<PlayerControler>().PlayerTeraType : player.GetComponent<PlayerControler>().PlayerTeraTypeJOR) ;
    }

    void GetMoneyAndExp()
    {
        MoneyAndExpUI.GetComponent<UIPDPMoneyAndExp>().GetMoneyAndExp(player.GetComponent<PlayerControler>());
    }

    void GetState()
    {
        StateUI.GetComponent<UIPDPState>().GetPokemonState(player.GetComponent<PlayerControler>());
    }

    void GetPlayerStrengthData()
    {
        PlayerControler playerControler = player.GetComponent<PlayerControler>();

        StrengthDataUI.transform.GetChild(1).GetComponent<GetStrengthData>().GetStrengthPoint(playerControler.HpPlayerPoint, (playerControler.playerData.HPHardWorkAlways + playerControler.playerData.HPHardWorkJustOneRoom), (playerControler.playerData.HPBounsAlways + playerControler.playerData.HPBounsJustOneRoom), playerControler.maxHp);
        StrengthDataUI.transform.GetChild(2).GetComponent<GetStrengthData>().GetStrengthPoint(playerControler.AtkPlayerPoint, (playerControler.playerData.AtkHardWorkAlways + playerControler.playerData.AtkHardWorkJustOneRoom), (playerControler.playerData.AtkBounsAlways + playerControler.playerData.AtkBounsJustOneRoom), playerControler.AtkAbilityPoint);
        StrengthDataUI.transform.GetChild(3).GetComponent<GetStrengthData>().GetStrengthPoint(playerControler.DefPlayerPoint, (playerControler.playerData.DefHardWorkAlways + playerControler.playerData.DefHardWorkJustOneRoom), (playerControler.playerData.DefBounsAlways + playerControler.playerData.DefBounsJustOneRoom), playerControler.DefAbilityPoint);
        StrengthDataUI.transform.GetChild(4).GetComponent<GetStrengthData>().GetStrengthPoint(playerControler.SpAPlayerPoint, (playerControler.playerData.SpAHardWorkAlways + playerControler.playerData.SpAHardWorkJustOneRoom), (playerControler.playerData.SpABounsAlways + playerControler.playerData.SpABounsJustOneRoom), playerControler.SpAAbilityPoint);
        StrengthDataUI.transform.GetChild(5).GetComponent<GetStrengthData>().GetStrengthPoint(playerControler.SpdPlayerPoint, (playerControler.playerData.SpDHardWorkAlways + playerControler.playerData.SpDHardWorkJustOneRoom), (playerControler.playerData.SpDBounsAlways + playerControler.playerData.SpDBounsJustOneRoom), playerControler.SpdAbilityPoint);
        StrengthDataUI.transform.GetChild(6).GetComponent<GetStrengthData>().GetStrengthPoint(playerControler.SpeedPlayerPoint, (playerControler.playerData.SpeHardWorkAlways + playerControler.playerData.SpeHardWorkJustOneRoom), (playerControler.playerData.SpeBounsAlways + playerControler.playerData.SpeBounsJustOneRoom), playerControler.SpeedAbilityPoint);
        StrengthDataUI.transform.GetChild(7).GetComponent<GetStrengthData>().GetStrengthPoint(playerControler.MoveSpePlayerPoint, (playerControler.playerData.MoveSpeHardWorkAlways + playerControler.playerData.MoveSpeHardWorkJustOneRoom), (playerControler.playerData.MoveSpwBounsAlways + playerControler.playerData.MoveSpeBounsJustOneRoom), playerControler.speed);
        StrengthDataUI.transform.GetChild(8).GetComponent<GetStrengthData>().GetStrengthPoint(playerControler.LuckPlayerPoint, (playerControler.playerData.LuckHardWorkAlways + playerControler.playerData.LuckHardWorkJustOneRoom), (playerControler.playerData.LuckBounsAlways + playerControler.playerData.LuckBounsJustOneRoom), playerControler.LuckPoint);
    }

    void GetTypeDefUI()
    {
        PlayerControler playerControler = player.GetComponent<PlayerControler>();
        int[] Input = new int[] { playerControler.playerData.TypeDefAlways[1] + playerControler.playerData.TypeDefJustOneRoom[1],
                                  playerControler.playerData.TypeDefAlways[2] + playerControler.playerData.TypeDefJustOneRoom[2] , playerControler.playerData.TypeDefAlways[3] + playerControler.playerData.TypeDefJustOneRoom[3],
                                  playerControler.playerData.TypeDefAlways[4] + playerControler.playerData.TypeDefJustOneRoom[4] , playerControler.playerData.TypeDefAlways[5] + playerControler.playerData.TypeDefJustOneRoom[5],
                                  playerControler.playerData.TypeDefAlways[6] + playerControler.playerData.TypeDefJustOneRoom[6] , playerControler.playerData.TypeDefAlways[7] + playerControler.playerData.TypeDefJustOneRoom[7],
                                  playerControler.playerData.TypeDefAlways[8] + playerControler.playerData.TypeDefJustOneRoom[8] , playerControler.playerData.TypeDefAlways[9] + playerControler.playerData.TypeDefJustOneRoom[9],
                                  playerControler.playerData.TypeDefAlways[10] + playerControler.playerData.TypeDefJustOneRoom[10] , playerControler.playerData.TypeDefAlways[11] + playerControler.playerData.TypeDefJustOneRoom[11],
                                  playerControler.playerData.TypeDefAlways[12] + playerControler.playerData.TypeDefJustOneRoom[12] , playerControler.playerData.TypeDefAlways[13] + playerControler.playerData.TypeDefJustOneRoom[13],
                                  playerControler.playerData.TypeDefAlways[14] + playerControler.playerData.TypeDefJustOneRoom[14] , playerControler.playerData.TypeDefAlways[15] + playerControler.playerData.TypeDefJustOneRoom[15],
                                  playerControler.playerData.TypeDefAlways[16] + playerControler.playerData.TypeDefJustOneRoom[16] , playerControler.playerData.TypeDefAlways[17] + playerControler.playerData.TypeDefJustOneRoom[17],
                                  playerControler.playerData.TypeDefAlways[18] + playerControler.playerData.TypeDefJustOneRoom[18] };
        for (int i = 1; i<19; i++)
        {
            if(Type.TYPE[i][playerControler.PlayerType01] == 1.2f)
            {
                Input[i - 1]--;
            }else if (Type.TYPE[i][playerControler.PlayerType01] == 0.8f)
            {
                Input[i - 1]++;
            }
            else if (Type.TYPE[i][playerControler.PlayerType01] == 0.64f)
            {
                Input[i - 1]++; Input[i - 1]++;
            }
            if (Type.TYPE[i][playerControler.PlayerType02] == 1.2f)
            {
                Input[i - 1]--;
            }
            else if (Type.TYPE[i][playerControler.PlayerType02] == 0.8f)
            {
                Input[i - 1]++;
            }
            else if (Type.TYPE[i][playerControler.PlayerType02] == 0.64f)
            {
                Input[i - 1]++; Input[i - 1]++;
            }


            if (playerControler.PlayerTeraTypeJOR == 0)
            {
                if (Type.TYPE[i][playerControler.PlayerTeraType] == 1.2f)
                {
                    Input[i - 1]--;
                }
                else if (Type.TYPE[i][playerControler.PlayerTeraType] == 0.8f)
                {
                    Input[i - 1]++;
                }
                else if (Type.TYPE[i][playerControler.PlayerTeraType] == 0.64f)
                {
                    Input[i - 1]++; Input[i - 1]++;
                }
            }
            else
            {
                if (Type.TYPE[i][playerControler.PlayerTeraTypeJOR] == 1.2f)
                {
                    Input[i - 1]--;
                }
                else if (Type.TYPE[i][playerControler.PlayerTeraTypeJOR] == 0.8f)
                {
                    Input[i - 1]++;
                }
                else if (Type.TYPE[i][playerControler.PlayerTeraTypeJOR] == 0.64f)
                {
                    Input[i - 1]++; Input[i - 1]++;
                }
            }
        }

        TypeDefUI.transform.GetChild(1).GetComponent<GetTypeDef>().GetTypeDefData(Input);
    }

    void GetNature()
    {
        UICallDescribe NatureDescribe = NatureText.GetComponent<UICallDescribe>();
        NatureDescribe.TwoMode = true;
        PlayerControler playerControler = player.GetComponent<PlayerControler>();
        switch (playerControler.NatureIndex)
        {
            case 0:  NatureText.text = "性格：勤奋";   NatureDescribe.FirstText = "很不错的性格！";   NatureDescribe.DescribeText = "能力值不会受到性格影响";     break;
            case 1:  NatureText.text = "性格：怕寂寞"; NatureDescribe.FirstText = "攻击更容易成长！"; NatureDescribe.DescribeText = "但是防御不容易成长";         break;
            case 2:  NatureText.text = "性格：固执";   NatureDescribe.FirstText = "攻击更容易成长！"; NatureDescribe.DescribeText = "但是特攻不容易成长";         break;
            case 3:  NatureText.text = "性格：顽皮";   NatureDescribe.FirstText = "攻击更容易成长！"; NatureDescribe.DescribeText = "但是特防不容易成长";         break;
            case 4:  NatureText.text = "性格：勇敢";   NatureDescribe.FirstText = "攻击更容易成长！"; NatureDescribe.DescribeText = "但是攻击速度不容易成长";     break;

            case 5:  NatureText.text = "性格：大胆"; NatureDescribe.FirstText = "防御更容易成长！"; NatureDescribe.DescribeText = "但是攻击不容易成长"; break;
            case 6:  NatureText.text = "性格：坦率"; NatureDescribe.FirstText = "很不错的性格！";   NatureDescribe.DescribeText = "能力值不会受到性格影响"; break;
            case 7:  NatureText.text = "性格：淘气"; NatureDescribe.FirstText = "防御更容易成长！"; NatureDescribe.DescribeText = "但是特攻不容易成长"; break;
            case 8:  NatureText.text = "性格：乐天"; NatureDescribe.FirstText = "防御更容易成长！"; NatureDescribe.DescribeText = "但是特防不容易成长"; break;
            case 9:  NatureText.text = "性格：悠闲"; NatureDescribe.FirstText = "防御更容易成长！"; NatureDescribe.DescribeText = "但是攻击速度不容易成长"; break;

            case 10: NatureText.text = "性格：内敛";   NatureDescribe.FirstText = "特攻更容易成长！"; NatureDescribe.DescribeText = "但是攻击不容易成长"; break;
            case 11: NatureText.text = "性格：慢吞吞"; NatureDescribe.FirstText = "特攻更容易成长！"; NatureDescribe.DescribeText = "但是防御不容易成长"; break;
            case 12: NatureText.text = "性格：害羞";   NatureDescribe.FirstText = "很不错的性格！";   NatureDescribe.DescribeText = "能力值不会受到性格影响"; break;
            case 13: NatureText.text = "性格：马虎";   NatureDescribe.FirstText = "特攻更容易成长！"; NatureDescribe.DescribeText = "但是特防不容易成长"; break;
            case 14: NatureText.text = "性格：冷静";   NatureDescribe.FirstText = "特攻更容易成长！"; NatureDescribe.DescribeText = "但是攻击速度不容易成长"; break;

            case 15: NatureText.text = "性格：温和"; NatureDescribe.FirstText = "特防更容易成长！"; NatureDescribe.DescribeText = "但是攻击不容易成长"; break;
            case 16: NatureText.text = "性格：温顺"; NatureDescribe.FirstText = "特防更容易成长！"; NatureDescribe.DescribeText = "但是防御不容易成长"; break;
            case 17: NatureText.text = "性格：慎重"; NatureDescribe.FirstText = "特防更容易成长！"; NatureDescribe.DescribeText = "但是特攻不容易成长"; break;
            case 18: NatureText.text = "性格：浮躁"; NatureDescribe.FirstText = "很不错的性格！";   NatureDescribe.DescribeText = "能力值不会受到性格影响"; break;
            case 19: NatureText.text = "性格：自大"; NatureDescribe.FirstText = "特防更容易成长！"; NatureDescribe.DescribeText = "但是攻击速度不容易成长"; break;

            case 20: NatureText.text = "性格：胆小"; NatureDescribe.FirstText = "攻击速度更容易成长！"; NatureDescribe.DescribeText = "但是攻击不容易成长"; break;
            case 21: NatureText.text = "性格：急躁"; NatureDescribe.FirstText = "攻击速度更容易成长！"; NatureDescribe.DescribeText = "但是防御不容易成长"; break;
            case 22: NatureText.text = "性格：爽朗"; NatureDescribe.FirstText = "攻击速度更容易成长！"; NatureDescribe.DescribeText = "但是特攻不容易成长"; break;
            case 23: NatureText.text = "性格：天真"; NatureDescribe.FirstText = "攻击速度更容易成长！"; NatureDescribe.DescribeText = "但是特防不容易成长"; break;
            case 24: NatureText.text = "性格：认真"; NatureDescribe.FirstText = "很不错的性格！";       NatureDescribe.DescribeText = "能力值不会受到性格影响"; break;

        }
    }


    void GetAbility()
    {
        UICallDescribe AbilityDescribe = AbilityText.GetComponent<UICallDescribe>();
        PlayerControler playerControler = player.GetComponent<PlayerControler>();
        if ((int)playerControler.PlayerAbility == 0) { AbilityText.gameObject.SetActive(false); }
        else { AbilityText.gameObject.SetActive(true); }
        switch ((int)playerControler.PlayerAbility)
        {

            case 1: AbilityText.text = "特性：迟钝"; AbilityDescribe.TwoMode = true; AbilityDescribe.FirstText = "当异常状态的进度被连续累积时，两次累积之间的时间间隔巨幅变长。"; AbilityDescribe.DescribeText = "并且当使用了接触类招式后，一段时间异常状态的进度不会累积。"; break;
            case 2: AbilityText.text = "特性：雪隐";  AbilityDescribe.DescribeText = "使用了冰属性技能后，一段时间内大幅提升移动速度。"; break;
            case 3: AbilityText.text = "特性：厚脂肪";  AbilityDescribe.DescribeText = "111"; break;
            case 4: AbilityText.text = "特性：叶子防守";  AbilityDescribe.DescribeText = "晴天或者处于草丛中时异常状态的进度不会累积。"; break;
            case 5: AbilityText.text = "特性：甜幕";  AbilityDescribe.DescribeText = "111"; break;
            case 6: AbilityText.text = "特性：女王的威严";  AbilityDescribe.DescribeText = "向对手施加威慑力，还未受伤的敌人无法对你施加伤害！"; break;
            case 7: AbilityText.text = "特性：逃跑";  AbilityDescribe.DescribeText = "移动速度变得更快"; break;
            case 8: AbilityText.text = "特性：适应力";  AbilityDescribe.DescribeText = "与自身同属性的招式威力会变得更高。"; break;
            case 9: AbilityText.text = "特性：危险预知";  AbilityDescribe.DescribeText = "111"; break;
            case 10: AbilityText.text = "特性：迷人之躯";  AbilityDescribe.DescribeText = "当接触类招式命中后，有概率累计目标的着迷进度。"; break;
            case 11: AbilityText.text = "特性：妖精皮肤";  AbilityDescribe.DescribeText = "111"; break;
            case 12: AbilityText.text = "特性：激流";  AbilityDescribe.DescribeText = "HP减少的时候，水属性的招式威力会提高。"; break;
            case 13: AbilityText.text = "特性：好胜";  AbilityDescribe.DescribeText = "111"; break;
            case 14: AbilityText.text = "特性：恒净之躯"; AbilityDescribe.DescribeText = "能力降低时，能力的降低值大幅变少。"; break;
            case 15: AbilityText.text = "特性：轻金属"; AbilityDescribe.DescribeText = "111"; break;
            case 16: AbilityText.text = "特性：同步"; AbilityDescribe.DescribeText = "当处于麻痹，烧伤，中毒状态时，攻击敌人有概率把异常状态传染给敌人。"; break;
            case 17: AbilityText.text = "特性：精神力"; AbilityDescribe.DescribeText = "111"; break;
            case 18: AbilityText.text = "特性：魔法镜"; AbilityDescribe.DescribeText = "111"; break;



        }
    }
}
