using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public PassiveList passiveList;

    public float HPHardWorkAlways;
    public float AtkHardWorkAlways;
    public float DefHardWorkAlways;
    public float SpAHardWorkAlways;
    public float SpDHardWorkAlways;
    public float SpeHardWorkAlways;
    public float MoveSpeHardWorkAlways;
    public float LuckHardWorkAlways;

    public float HPHardWorkJustOneRoom;
    public float AtkHardWorkJustOneRoom;
    public float DefHardWorkJustOneRoom;
    public float SpAHardWorkJustOneRoom;
    public float SpDHardWorkJustOneRoom;
    public float SpeHardWorkJustOneRoom;
    public float MoveSpeHardWorkJustOneRoom;
    public float LuckHardWorkJustOneRoom;

    public int HPBounsAlways;
    public int AtkBounsAlways;
    public int DefBounsAlways;
    public int SpABounsAlways;
    public int SpDBounsAlways;
    public int SpeBounsAlways;
    public int MoveSpwBounsAlways;
    public int LuckBounsAlways;

    public int HPBounsJustOneRoom;
    public int AtkBounsJustOneRoom;
    public int DefBounsJustOneRoom;
    public int SpABounsJustOneRoom;
    public int SpDBounsJustOneRoom;
    public int SpeBounsJustOneRoom;
    public int MoveSpeBounsJustOneRoom;
    public int LuckBounsJustOneRoom;

    public int[] TypeDefAlways = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
    public int[] TypeDefJustOneRoom = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

    public bool[] IsPassiveGetList = new bool[100];

    PlayerControler player;

    public bool isEndure;
    public bool isMist;


    private void Start()
    {
        player = GetComponent<PlayerControler>();
    }

    public void RestoreJORSata()
    {
        HPHardWorkJustOneRoom = 0;
        AtkHardWorkJustOneRoom = 0;
        DefHardWorkJustOneRoom = 0;
        SpAHardWorkJustOneRoom = 0;
        SpDHardWorkJustOneRoom = 0;
        SpeHardWorkJustOneRoom = 0;
        MoveSpeHardWorkJustOneRoom = 0;
        LuckHardWorkJustOneRoom = 0;

        HPBounsJustOneRoom = 0;
        AtkBounsJustOneRoom = 0;
        DefBounsJustOneRoom = 0;
        SpABounsJustOneRoom = 0;
        SpDBounsJustOneRoom = 0;
        SpeBounsJustOneRoom = 0;
        MoveSpeBounsJustOneRoom = 0;
        LuckBounsJustOneRoom = 0;

        TypeDefJustOneRoom = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
    }

    public List<int> GetPassiveItemList = new List<int>();




    public void GetPassiveItem(PassiveItem GetPassiveItem)
    {
        int ItemIndex = GetPassiveItem.PassiveItemIndex;
        switch (ItemIndex)
        {
            case 0:
                HPBounsAlways += 1;
                player.ReFreshAbllityPoint();
                player.ChangeHp(player.maxHp - player.Hp, 0, 0);
                UIHealthBar.Instance.MaxHpText.text = player.maxHp.ToString();
                break;
            case 1:
                UiMiniMap.Instance.SeeMap();
                break;
            case 2:
                TypeDefAlways[1]++;
                TypeDefAlways[10]++;
                TypeDefAlways[11]++;
                TypeDefAlways[12]++;
                TypeDefAlways[13]++;
                TypeDefAlways[14]++;
                TypeDefAlways[15]++;
                TypeDefAlways[17]++;
                TypeDefAlways[18]++;

                TypeDefAlways[2]--;
                TypeDefAlways[3]--;
                TypeDefAlways[4]--;
                TypeDefAlways[5]--;
                TypeDefAlways[6]--;
                TypeDefAlways[7]--;
                TypeDefAlways[8]--;
                TypeDefAlways[9]--;
                TypeDefAlways[16]--;
                break;

            case 3:
                if (HPHardWorkAlways >= 0) { HPHardWorkAlways = 0; }
                if (AtkHardWorkAlways >= 0) { AtkHardWorkAlways = 0; }
                if (DefHardWorkAlways >= 0) { DefHardWorkAlways = 0; }
                if (SpAHardWorkAlways >= 0) { SpAHardWorkAlways = 0; }
                if (SpDHardWorkAlways >= 0) { SpDHardWorkAlways = 0; }
                if (SpeHardWorkAlways >= 0) { SpeHardWorkAlways = 0; }
                if (LuckHardWorkAlways >= 0) { LuckHardWorkAlways = 0; }
                if (MoveSpeHardWorkAlways >= 0) { MoveSpeHardWorkAlways = 0; }
                HPBounsAlways += 1;
                AtkBounsAlways += 1;
                DefBounsAlways += 1;
                SpABounsAlways += 1;
                SpDBounsAlways += 1;
                SpeBounsAlways += 1;
                MoveSpwBounsAlways += 1;
                LuckBounsAlways += 1;
                player.ReFreshAbllityPoint();
                break;

            case 4:
                AtkBounsAlways += 1;
                break;

            case 5:
                SpABounsAlways += 1;
                break;

            case 6:
                LuckBounsAlways += 1;
                break;

            case 7:
                SpeBounsAlways += 1;
                break;

            case 8:
                MoveSpwBounsAlways += 1;
                break;

            case 9:

                HPBounsAlways += 1;  
                AtkBounsAlways += 1;
                DefBounsAlways += 1;
                SpABounsAlways += 1;
                SpDBounsAlways += 1;
                SpeBounsAlways += 1;
                MoveSpwBounsAlways += 1;
                LuckBounsAlways += 1;
                player.ComeInANewRoomEvent += ViciousEvent;
                player.ReFreshAbllityPoint();
                ViciousEvent(player);
                break;

            case 10:

                break;

            case 11:
                player.ChangeMoney(30);
                break;

            case 12:

                break;

            case 13:

                break;

            case 14:
                player.ClearThisRoomEvent += Leftover;
                break;

            case 15:
                //太晶大胃王
                break;

            case 16:
                if (player.isParalysisDone) 
                {
                    AtkBounsAlways++; SpABounsAlways++;
                }
                player.ParalysisDoneHappendEvent += CellBattery01;
                player.ParalysisRemoveHappendEvent += CellBattery02;
                break;

            case 17:
                //安抚之铃
                break;

            case 18:
                //力量负重
                MoveSpwBounsAlways--;
                MoveSpeHardWorkAlways -= 0.5f;
                break;

            case 19:
                //力量护腕
                MoveSpwBounsAlways--;
                MoveSpeHardWorkAlways -= 0.5f;
                break;

            case 20:
                //力量腰带
                MoveSpwBounsAlways--;
                MoveSpeHardWorkAlways -= 0.5f;
                break;

            case 21:
                //力量镜
                MoveSpwBounsAlways--;
                MoveSpeHardWorkAlways -= 0.5f;
                break;

            case 22:
                //力量束带
                MoveSpwBounsAlways--;
                MoveSpeHardWorkAlways -= 0.5f;
                break;

            case 23:
                //力量护踝
                MoveSpwBounsAlways--;
                MoveSpeHardWorkAlways -= 0.5f;
                break;

            case 24:
                //宝可梦病毒
                player.ToxicFloatPlus(1);
                break;

            case 25:
                //王者之证
                break;

            case 26:
                //毒手
                break;

            case 27:
                //洁净之盐
                player.isToxicDef = true;
                player.isParalysisDef = true;
                player.isBurnDef = true;
                player.isSleepDef = true;
                MoveSpwBounsAlways -= 3;
                MoveSpeHardWorkAlways = 0;
                break;

            case 28:
                //树果烹饪指南
                break;

            case 29:
                //妈妈离开了
                break;

        }

        player.ReFreshAbllityPoint();
        GetPassiveItemList.Add(ItemIndex);
        IsPassiveGetList[ItemIndex] = true;

        if (IsPassiveGetList[28] && GetPassiveItem.ItemTypeTag != null)
        {
            foreach (int i in GetPassiveItem.ItemTypeTag)
            {
                if (i == 1) { player.ChangeHp(Mathf.Clamp(player.maxHp / 16, 1, 10), 0, 19); }
            }
        }
    }

    //===================================================恶性事件=================================================

    int ViciousEventIndex;
    void ViciousEvent(PlayerControler playerInput)
    {
        if(ViciousEventIndex != 0)
        {
            switch (ViciousEventIndex)
            {
                case 1:
                    playerInput.ToxicRemove();
                    break;
                case 2:
                    playerInput.ParalysisRemove();
                    break;
                case 3:
                    playerInput.BurnRemove();
                    break;
                case 4:
                    playerInput.SleepRemove();
                    break;
            }
        }
        ViciousEventIndex = Random.Range(1,5);
        switch (ViciousEventIndex)
        {
            case 1:
                playerInput.ToxicFloatPlus(1);
                break;
            case 2:
                playerInput.ParalysisFloatPlus(1);
                break;
            case 3:
                playerInput.BurnFloatPlus(1);
                break;
            case 4:
                playerInput.SleepFloatPlus(1);
                break;
        }
    }


    //===================================================恶性事件=================================================




    //===================================================剩饭=================================================

    void Leftover(PlayerControler player)
    {
        if (Random.Range(0.0f , 1.0f)+((float)(player.LuckPoint)/30) >= 0.4f) {
            player.ChangeHp((int)Mathf.Clamp(player.maxHp / 10, 1, 10), 0, 0);
        }
    }

    //===================================================剩饭=================================================




    //===================================================充电电池=================================================

    void CellBattery01( PlayerControler player )
    {
        AtkBounsAlways++;
        SpABounsAlways++;
    }

    void CellBattery02(PlayerControler player)
    {
        AtkBounsAlways--;
        SpABounsAlways--;
    }

    //===================================================充电电池=================================================
}
