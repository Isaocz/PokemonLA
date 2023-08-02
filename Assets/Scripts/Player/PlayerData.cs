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

    public List<int> GetPassiveItemList = new List<int>();


    public void CopyData(PlayerData OtherData)
    {
        passiveList = OtherData.passiveList;

        HPHardWorkAlways = OtherData.HPHardWorkAlways;
        AtkHardWorkAlways = OtherData.AtkHardWorkAlways;
        DefHardWorkAlways = OtherData.DefHardWorkAlways;
        SpAHardWorkAlways = OtherData.SpAHardWorkAlways;
        SpDHardWorkAlways = OtherData.SpDHardWorkAlways;
        SpeHardWorkAlways = OtherData.SpeHardWorkAlways;
        MoveSpeHardWorkAlways = OtherData.MoveSpeHardWorkAlways;
        LuckHardWorkAlways = OtherData.LuckHardWorkAlways;

        HPHardWorkJustOneRoom = OtherData.HPHardWorkJustOneRoom;
        AtkHardWorkJustOneRoom = OtherData.AtkHardWorkJustOneRoom;
        DefHardWorkJustOneRoom = OtherData.DefHardWorkJustOneRoom;
        SpAHardWorkJustOneRoom = OtherData.SpAHardWorkJustOneRoom;
        SpDHardWorkJustOneRoom = OtherData.SpDHardWorkJustOneRoom;
        SpeHardWorkJustOneRoom = OtherData.SpeHardWorkJustOneRoom;
        MoveSpeHardWorkJustOneRoom = OtherData.MoveSpeHardWorkJustOneRoom;
        LuckHardWorkJustOneRoom = OtherData.LuckHardWorkJustOneRoom;

        HPBounsAlways = OtherData.HPBounsAlways;
        AtkBounsAlways = OtherData.AtkBounsAlways;
        DefBounsAlways = OtherData.DefBounsAlways;
        SpABounsAlways = OtherData.SpABounsAlways;
        SpDBounsAlways = OtherData.SpDBounsAlways;
        SpeBounsAlways = OtherData.SpeBounsAlways;
        MoveSpwBounsAlways = OtherData.MoveSpwBounsAlways;
        LuckBounsAlways = OtherData.LuckBounsAlways;

        HPBounsJustOneRoom = OtherData.HPBounsJustOneRoom;
        AtkBounsJustOneRoom = OtherData.AtkBounsJustOneRoom;
        DefBounsJustOneRoom = OtherData.DefBounsJustOneRoom;
        SpABounsJustOneRoom = OtherData.SpABounsJustOneRoom;
        SpDBounsJustOneRoom = OtherData.SpDBounsJustOneRoom;
        SpeBounsJustOneRoom = OtherData.SpeBounsJustOneRoom;
        MoveSpeBounsJustOneRoom = OtherData.MoveSpeBounsJustOneRoom;
        LuckBounsJustOneRoom = OtherData.LuckBounsJustOneRoom;

        TypeDefAlways = OtherData.TypeDefAlways;
        TypeDefJustOneRoom = OtherData.TypeDefJustOneRoom;
        IsPassiveGetList = OtherData.IsPassiveGetList;
        GetPassiveItemList = OtherData.GetPassiveItemList;


    }


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

            case 30:
                //木木枭宝宝
                Instantiate(aGameObjectByPassiveItem(0), player.transform.position, Quaternion.identity, player.FollowBaby.transform);
                break;
            case 31:
                //小都宝宝
                Instantiate(aGameObjectByPassiveItem(1), player.transform.position, Quaternion.identity, player.FollowBaby.transform);
                break;
            case 32:
                //索财灵宝宝
                Instantiate(aGameObjectByPassiveItem(2), player.transform.position, Quaternion.identity, player.FollowBaby.transform);
                break;
            case 33:
                //丁宝宝
                Instantiate(aGameObjectByPassiveItem(3), player.transform.position, Quaternion.identity, player.FollowBaby.transform);
                break;


            //====================薄荷===========================

            case 34:
                //Atk+Def-薄荷
                player.RemoveNature(player.NatureIndex);
                player.NatureIndex = 1;
                player.InstanceNature(player.NatureIndex);
                player.ReFreshAbllityPoint();
                break;
            case 35:
                //Atk+SpA-薄荷
                player.RemoveNature(player.NatureIndex);
                player.NatureIndex = 2;
                player.InstanceNature(player.NatureIndex);
                player.ReFreshAbllityPoint();
                break;
            case 36:
                //Atk+SpD-薄荷
                player.RemoveNature(player.NatureIndex);
                player.NatureIndex = 3;
                player.InstanceNature(player.NatureIndex);
                player.ReFreshAbllityPoint();
                break;
            case 37:
                //Atk+Spe-薄荷
                player.RemoveNature(player.NatureIndex);
                player.NatureIndex = 4;
                player.InstanceNature(player.NatureIndex);
                player.ReFreshAbllityPoint();
                break;
            case 38:
                //Def+Atk-薄荷
                player.RemoveNature(player.NatureIndex);
                player.NatureIndex = 5;
                player.InstanceNature(player.NatureIndex);
                player.ReFreshAbllityPoint();
                break;
            case 39:
                //Def+SpA-薄荷
                player.RemoveNature(player.NatureIndex);
                player.NatureIndex = 7;
                player.InstanceNature(player.NatureIndex);
                player.ReFreshAbllityPoint();
                break;
            case 40:
                //Def+SpD-薄荷
                player.RemoveNature(player.NatureIndex);
                player.NatureIndex = 8;
                player.InstanceNature(player.NatureIndex);
                player.ReFreshAbllityPoint();
                break;
            case 41:
                //Def+Spe-薄荷
                player.RemoveNature(player.NatureIndex);
                player.NatureIndex = 9;
                player.InstanceNature(player.NatureIndex);
                player.ReFreshAbllityPoint();
                break;
            case 42:
                //SpA+Atk-薄荷
                player.RemoveNature(player.NatureIndex);
                player.NatureIndex = 10;
                player.InstanceNature(player.NatureIndex);
                player.ReFreshAbllityPoint();
                break;
            case 43:
                //SpA+Def-薄荷
                player.RemoveNature(player.NatureIndex);
                player.NatureIndex = 11;
                player.InstanceNature(player.NatureIndex);
                player.ReFreshAbllityPoint();
                break;
            case 44:
                //SpA+SpD-薄荷
                player.RemoveNature(player.NatureIndex);
                player.NatureIndex = 13;
                player.InstanceNature(player.NatureIndex);
                player.ReFreshAbllityPoint();
                break;
            case 45:
                //SpA+Spe-薄荷
                player.RemoveNature(player.NatureIndex);
                player.NatureIndex = 14;
                player.InstanceNature(player.NatureIndex);
                player.ReFreshAbllityPoint();
                break;
            case 46:
                //SpD+Atk-薄荷
                player.RemoveNature(player.NatureIndex);
                player.NatureIndex = 15;
                player.InstanceNature(player.NatureIndex);
                player.ReFreshAbllityPoint();
                break;
            case 47:
                //SpD+Def-薄荷
                player.RemoveNature(player.NatureIndex);
                player.NatureIndex = 16;
                player.InstanceNature(player.NatureIndex);
                player.ReFreshAbllityPoint();
                break;
            case 48:
                //SpD+SpA-薄荷
                player.RemoveNature(player.NatureIndex);
                player.NatureIndex = 17;
                player.InstanceNature(player.NatureIndex);
                player.ReFreshAbllityPoint();
                break;
            case 49:
                //SpD+Spe-薄荷
                player.RemoveNature(player.NatureIndex);
                player.NatureIndex = 19;
                player.InstanceNature(player.NatureIndex);
                player.ReFreshAbllityPoint();
                break;
            case 50:
                //Spe+Atk-薄荷
                player.RemoveNature(player.NatureIndex);
                player.NatureIndex = 20;
                player.InstanceNature(player.NatureIndex);
                player.ReFreshAbllityPoint();
                break;
            case 51:
                //Spe+Def-薄荷
                player.RemoveNature(player.NatureIndex);
                player.NatureIndex = 21;
                player.InstanceNature(player.NatureIndex);
                player.ReFreshAbllityPoint();
                break;
            case 52:
                //Spe+SpA-薄荷
                player.RemoveNature(player.NatureIndex);
                player.NatureIndex = 22;
                player.InstanceNature(player.NatureIndex);
                player.ReFreshAbllityPoint();
                break;
            case 53:
                //Spe+SpD-薄荷
                player.RemoveNature(player.NatureIndex);
                player.NatureIndex = 23;
                player.InstanceNature(player.NatureIndex);
                player.ReFreshAbllityPoint();
                break;
            case 54:
                //Normal薄荷
                player.RemoveNature(player.NatureIndex);
                player.NatureIndex = 24;
                player.InstanceNature(player.NatureIndex);
                player.ReFreshAbllityPoint();
                break;



                //====================薄荷===========================


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




    //===================================================当被动道具需要生成实体时=================================================


    public GameObject[] GameObjectList;
    GameObject aGameObjectByPassiveItem(int i)
    {
        GameObject OutPutObj = GameObjectList[i];
        return OutPutObj;
    }





    //===================================================当被动道具需要生成实体时=================================================
}

