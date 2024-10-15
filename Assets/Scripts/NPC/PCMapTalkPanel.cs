using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PCMapTalkPanel : MonoBehaviour
{
    Button Buy;
    Button DontBuy;
    TextMeshProUGUI TalkInformation;
    public int TalkIndex;
    string[] TalkTextList;
    bool isSeeMap;
    bool CanUpdate;
    PCMap ParentPcMap;

    public Sprite YesHL;
    public Sprite NoHL;

    public int Price;

    public void ZButtonDown()
    {
        ZButton.Z.IsZButtonDown = true;
    }

    private void Awake()
    {
        TalkInformation = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        TalkTextList = new string[] { "�ܱߵ����ĵ�ͼ����Ҫ���ѹۿ���\nһ��"+ Price+"��Ǯ��Ҫ����", "��ʡһ��ɡ�����", "��Ǯ�ˣ�\n�úü�ס�ɣ��뿪����Ϳ�������", "�ǲ����ˣ�\n�ٿ�һ�ΰ�","�㻨Ǯ�����ˣ�" };
        TalkIndex = 0;
        TalkInformation.text = TalkTextList[TalkIndex];
        ParentPcMap = transform.parent.parent.GetComponent<PCMap>();
    }
    private void OnEnable()
    {
        Buy = transform.GetChild(2).GetComponent<Button>();
        DontBuy = transform.GetChild(3).GetComponent<Button>();
    }

    public void PlayerExit()
    {
        if (TalkInformation != null)
        {
            if (!isSeeMap)
            {
                TalkIndex = 0;
                TalkInformation.text = TalkTextList[0];
                Buy.gameObject.SetActive(true);
                DontBuy.gameObject.SetActive(true);
                gameObject.SetActive(false);
            }
            else
            {
                TalkIndex = -1;
                TalkInformation.text = TalkTextList[3];
                gameObject.SetActive(false);
            }
            ParentPcMap.player.CanNotUseSpaceItem = false;
        }

    }

    public void BuyMap()
    {
        if (ParentPcMap.player.Money >= Price)
        {
            TalkIndex = 2;
            TalkInformation.text = TalkTextList[2];
            isSeeMap = true;
            UiMiniMap.Instance.SeeMapJustOneRoom();
            ParentPcMap.player.ChangeMoney(-Price);
            Buy.gameObject.SetActive(false);
            DontBuy.gameObject.SetActive(false);
        }
        else
        {
            TalkIndex = 4;
            TalkInformation.text = TalkTextList[4];
            Buy.gameObject.SetActive(false);
            DontBuy.gameObject.SetActive(false);
        }
    }

    public void NotBuyMap()
    {
        TalkIndex = 1;
        TalkInformation.text = TalkTextList[1];
        Buy.gameObject.SetActive(false);
        DontBuy.gameObject.SetActive(false);
        
    }


}
