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

    private void Awake()
    {
        InPokemonData = this;
        
    }


    private void OnEnable()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControler>();
        Invoke("RestorePokemonDataPanel", 0.0001f);
    }

    public void RestorePokemonDataPanel()
    {
        HeadUI = transform.GetChild(0).gameObject;
        TypeMarkUI = transform.GetChild(1).gameObject;
        MoneyAndExpUI = transform.GetChild(2).gameObject;
        StateUI = transform.GetChild(3).gameObject;
        StrengthDataUI = transform.GetChild(4).gameObject;
        TypeDefUI = transform.GetChild(5).gameObject;
        NatureText = transform.GetChild(6).gameObject.GetComponent<Text>(); ;

        GetHead();
        GetTypeMark();
        GetMoneyAndExp();
        GetState();
        GetPlayerStrengthData();
        GetTypeDefUI();
        GetNature();

    }

    void GetHead()
    {
        HeadUI.GetComponent<Image>().sprite = player.GetComponent<PlayerControler>().PlayerHead;
        HeadUI.transform.GetChild(0).GetComponent<Text>().text = player.GetComponent<PlayerControler>().PlayerNameChinese;
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
            case 0:  NatureText.text = "�Ը��ڷ�";   NatureDescribe.FirstText = "�ܲ�����Ը�";   NatureDescribe.DescribeText = "����ֵ�����ܵ��Ը�Ӱ��";     break;
            case 1:  NatureText.text = "�Ը��¼�į"; NatureDescribe.FirstText = "���������׳ɳ���"; NatureDescribe.DescribeText = "���Ƿ��������׳ɳ�";         break;
            case 2:  NatureText.text = "�Ը񣺹�ִ";   NatureDescribe.FirstText = "���������׳ɳ���"; NatureDescribe.DescribeText = "�����ع������׳ɳ�";         break;
            case 3:  NatureText.text = "�Ը���Ƥ";   NatureDescribe.FirstText = "���������׳ɳ���"; NatureDescribe.DescribeText = "�����ط������׳ɳ�";         break;
            case 4:  NatureText.text = "�Ը��¸�";   NatureDescribe.FirstText = "���������׳ɳ���"; NatureDescribe.DescribeText = "���ǹ����ٶȲ����׳ɳ�";     break;

            case 5:  NatureText.text = "�Ը񣺴�"; NatureDescribe.FirstText = "���������׳ɳ���"; NatureDescribe.DescribeText = "���ǹ��������׳ɳ�"; break;
            case 6:  NatureText.text = "�Ը�̹��"; NatureDescribe.FirstText = "�ܲ�����Ը�";   NatureDescribe.DescribeText = "����ֵ�����ܵ��Ը�Ӱ��"; break;
            case 7:  NatureText.text = "�Ը�����"; NatureDescribe.FirstText = "���������׳ɳ���"; NatureDescribe.DescribeText = "�����ع������׳ɳ�"; break;
            case 8:  NatureText.text = "�Ը�����"; NatureDescribe.FirstText = "���������׳ɳ���"; NatureDescribe.DescribeText = "�����ط������׳ɳ�"; break;
            case 9:  NatureText.text = "�Ը�����"; NatureDescribe.FirstText = "���������׳ɳ���"; NatureDescribe.DescribeText = "���ǹ����ٶȲ����׳ɳ�"; break;

            case 10: NatureText.text = "�Ը�����";   NatureDescribe.FirstText = "�ع������׳ɳ���"; NatureDescribe.DescribeText = "���ǹ��������׳ɳ�"; break;
            case 11: NatureText.text = "�Ը�������"; NatureDescribe.FirstText = "�ع������׳ɳ���"; NatureDescribe.DescribeText = "���Ƿ��������׳ɳ�"; break;
            case 12: NatureText.text = "�Ը񣺺���";   NatureDescribe.FirstText = "�ܲ�����Ը�";   NatureDescribe.DescribeText = "����ֵ�����ܵ��Ը�Ӱ��"; break;
            case 13: NatureText.text = "�Ը���";   NatureDescribe.FirstText = "�ع������׳ɳ���"; NatureDescribe.DescribeText = "�����ط������׳ɳ�"; break;
            case 14: NatureText.text = "�Ը��侲";   NatureDescribe.FirstText = "�ع������׳ɳ���"; NatureDescribe.DescribeText = "���ǹ����ٶȲ����׳ɳ�"; break;

            case 15: NatureText.text = "�Ը��º�"; NatureDescribe.FirstText = "�ط������׳ɳ���"; NatureDescribe.DescribeText = "���ǹ��������׳ɳ�"; break;
            case 16: NatureText.text = "�Ը���˳"; NatureDescribe.FirstText = "�ط������׳ɳ���"; NatureDescribe.DescribeText = "���Ƿ��������׳ɳ�"; break;
            case 17: NatureText.text = "�Ը�����"; NatureDescribe.FirstText = "�ط������׳ɳ���"; NatureDescribe.DescribeText = "�����ع������׳ɳ�"; break;
            case 18: NatureText.text = "�Ը񣺸���"; NatureDescribe.FirstText = "�ܲ�����Ը�";   NatureDescribe.DescribeText = "����ֵ�����ܵ��Ը�Ӱ��"; break;
            case 19: NatureText.text = "�Ը��Դ�"; NatureDescribe.FirstText = "�ط������׳ɳ���"; NatureDescribe.DescribeText = "���ǹ����ٶȲ����׳ɳ�"; break;

            case 20: NatureText.text = "�Ը񣺵�С"; NatureDescribe.FirstText = "�����ٶȸ����׳ɳ���"; NatureDescribe.DescribeText = "���ǹ��������׳ɳ�"; break;
            case 21: NatureText.text = "�Ը񣺼���"; NatureDescribe.FirstText = "�����ٶȸ����׳ɳ���"; NatureDescribe.DescribeText = "���Ƿ��������׳ɳ�"; break;
            case 22: NatureText.text = "�Ը�ˬ��"; NatureDescribe.FirstText = "�����ٶȸ����׳ɳ���"; NatureDescribe.DescribeText = "�����ع������׳ɳ�"; break;
            case 23: NatureText.text = "�Ը�����"; NatureDescribe.FirstText = "�����ٶȸ����׳ɳ���"; NatureDescribe.DescribeText = "�����ط������׳ɳ�"; break;
            case 24: NatureText.text = "�Ը�����"; NatureDescribe.FirstText = "�ܲ�����Ը�";       NatureDescribe.DescribeText = "����ֵ�����ܵ��Ը�Ӱ��"; break;

        }
    }
}
