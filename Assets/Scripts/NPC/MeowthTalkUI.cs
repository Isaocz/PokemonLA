using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MeowthTalkUI : MonoBehaviour
{
    Text TalkInformation;
    GameObject ShopPanel;
    GameObject ShadowPanel;
    Meowth ParentMeowth;
    public int TalkIndex;
    string[] TalkTextList;

    // Start is called before the first frame update
    void Start()
    {
        ShadowPanel = transform.parent.GetChild(0).gameObject;
        ParentMeowth = transform.parent.parent.GetComponent<Meowth>();
        TalkInformation = transform.GetChild(0).GetComponent<Text>();
        ShopPanel = transform.parent.GetChild(2).gameObject;
        TalkTextList = new string[]{ "��ӭ�����Ѻ��̵�����\n��è�������" ,"��������Ҫʲô��?","��������!\nлл�ݹ�����" };
        TalkIndex = 0;
        TalkInformation.text = TalkTextList[TalkIndex];
    }
    public void PlayerExit()
    {
        if (TalkInformation != null)
        {
            TalkIndex = 0;
            TalkInformation.text = TalkTextList[0];
            gameObject.SetActive(false);
        }
    }

    void TalkContinue()
    {
        if (TalkIndex == 1){
            ShadowPanel.SetActive(true);
            ShopPanel.SetActive(true);
            gameObject.SetActive(false);
        }else if (TalkIndex == 2)
        {
            TalkIndex = 0; TalkInformation.text = TalkTextList[0];
            gameObject.SetActive(false);
            ParentMeowth.GoodBye();
        }
        TalkIndex += 1;
        if(TalkIndex >= 3) { TalkIndex = 0; TalkInformation.text = TalkTextList[0]; }
        TalkInformation.text = TalkTextList[TalkIndex];
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            TalkContinue();
        }
    }
}
