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

    public bool[] IsPassiveGetList = new bool[500];

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
                //Ì«¾§´óÎ¸Íõ
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
                //°²¸§Ö®Áå
                break;

            case 18:
                //Á¦Á¿¸ºÖØ
                MoveSpwBounsAlways--;
                MoveSpeHardWorkAlways -= 0.5f;
                break;

            case 19:
                //Á¦Á¿»¤Íó
                MoveSpwBounsAlways--;
                MoveSpeHardWorkAlways -= 0.5f;
                break;

            case 20:
                //Á¦Á¿Ñü´ø
                MoveSpwBounsAlways--;
                MoveSpeHardWorkAlways -= 0.5f;
                break;

            case 21:
                //Á¦Á¿¾µ
                MoveSpwBounsAlways--;
                MoveSpeHardWorkAlways -= 0.5f;
                break;

            case 22:
                //Á¦Á¿Êø´ø
                MoveSpwBounsAlways--;
                MoveSpeHardWorkAlways -= 0.5f;
                break;

            case 23:
                //Á¦Á¿»¤õ×
                MoveSpwBounsAlways--;
                MoveSpeHardWorkAlways -= 0.5f;
                break;

            case 24:
                //±¦¿ÉÃÎ²¡¶¾
                player.ToxicFloatPlus(1);
                break;

            case 25:
                //ÍõÕßÖ®Ö¤
                break;

            case 26:
                //¶¾ÊÖ
                break;

            case 27:
                //½à¾»Ö®ÑÎ
                player.isToxicDef = true;
                player.isParalysisDef = true;
                player.isBurnDef = true;
                player.isSleepDef = true;
                player.isFrozenDef = true;
                MoveSpwBounsAlways -= 3;
                MoveSpeHardWorkAlways = 0;
                break;

            case 28:
                //Ê÷¹ûÅëâ¿Ö¸ÄÏ
                break;

            case 29:
                //ÂèÂèÀë¿ªÁË
                break;

            case 30:
                //Ä¾Ä¾èÉ±¦±¦
                Instantiate(aGameObjectByPassiveItem(0), player.transform.position, Quaternion.identity, player.FollowBaby.transform);
                break;
            case 31:
                //Ð¡¶¼±¦±¦
                Instantiate(aGameObjectByPassiveItem(1), player.transform.position, Quaternion.identity, player.FollowBaby.transform);
                break;
            case 32:
                //Ë÷²ÆÁé±¦±¦
                Instantiate(aGameObjectByPassiveItem(2), player.transform.position, Quaternion.identity, player.FollowBaby.transform);
                break;
            case 33:
                //¶¡±¦±¦
                Instantiate(aGameObjectByPassiveItem(3), player.transform.position, Quaternion.identity, player.FollowBaby.transform);
                break;


            //====================±¡ºÉ===========================

            case 34:
                //Atk+Def-±¡ºÉ
                player.RemoveNature(player.NatureIndex);
                player.NatureIndex = 1;
                player.InstanceNature(player.NatureIndex);
                player.ReFreshAbllityPoint();
                break;
            case 35:
                //Atk+SpA-±¡ºÉ
                player.RemoveNature(player.NatureIndex);
                player.NatureIndex = 2;
                player.InstanceNature(player.NatureIndex);
                player.ReFreshAbllityPoint();
                break;
            case 36:
                //Atk+SpD-±¡ºÉ
                player.RemoveNature(player.NatureIndex);
                player.NatureIndex = 3;
                player.InstanceNature(player.NatureIndex);
                player.ReFreshAbllityPoint();
                break;
            case 37:
                //Atk+Spe-±¡ºÉ
                player.RemoveNature(player.NatureIndex);
                player.NatureIndex = 4;
                player.InstanceNature(player.NatureIndex);
                player.ReFreshAbllityPoint();
                break;
            case 38:
                //Def+Atk-±¡ºÉ
                player.RemoveNature(player.NatureIndex);
                player.NatureIndex = 5;
                player.InstanceNature(player.NatureIndex);
                player.ReFreshAbllityPoint();
                break;
            case 39:
                //Def+SpA-±¡ºÉ
                player.RemoveNature(player.NatureIndex);
                player.NatureIndex = 7;
                player.InstanceNature(player.NatureIndex);
                player.ReFreshAbllityPoint();
                break;
            case 40:
                //Def+SpD-±¡ºÉ
                player.RemoveNature(player.NatureIndex);
                player.NatureIndex = 8;
                player.InstanceNature(player.NatureIndex);
                player.ReFreshAbllityPoint();
                break;
            case 41:
                //Def+Spe-±¡ºÉ
                player.RemoveNature(player.NatureIndex);
                player.NatureIndex = 9;
                player.InstanceNature(player.NatureIndex);
                player.ReFreshAbllityPoint();
                break;
            case 42:
                //SpA+Atk-±¡ºÉ
                player.RemoveNature(player.NatureIndex);
                player.NatureIndex = 10;
                player.InstanceNature(player.NatureIndex);
                player.ReFreshAbllityPoint();
                break;
            case 43:
                //SpA+Def-±¡ºÉ
                player.RemoveNature(player.NatureIndex);
                player.NatureIndex = 11;
                player.InstanceNature(player.NatureIndex);
                player.ReFreshAbllityPoint();
                break;
            case 44:
                //SpA+SpD-±¡ºÉ
                player.RemoveNature(player.NatureIndex);
                player.NatureIndex = 13;
                player.InstanceNature(player.NatureIndex);
                player.ReFreshAbllityPoint();
                break;
            case 45:
                //SpA+Spe-±¡ºÉ
                player.RemoveNature(player.NatureIndex);
                player.NatureIndex = 14;
                player.InstanceNature(player.NatureIndex);
                player.ReFreshAbllityPoint();
                break;
            case 46:
                //SpD+Atk-±¡ºÉ
                player.RemoveNature(player.NatureIndex);
                player.NatureIndex = 15;
                player.InstanceNature(player.NatureIndex);
                player.ReFreshAbllityPoint();
                break;
            case 47:
                //SpD+Def-±¡ºÉ
                player.RemoveNature(player.NatureIndex);
                player.NatureIndex = 16;
                player.InstanceNature(player.NatureIndex);
                player.ReFreshAbllityPoint();
                break;
            case 48:
                //SpD+SpA-±¡ºÉ
                player.RemoveNature(player.NatureIndex);
                player.NatureIndex = 17;
                player.InstanceNature(player.NatureIndex);
                player.ReFreshAbllityPoint();
                break;
            case 49:
                //SpD+Spe-±¡ºÉ
                player.RemoveNature(player.NatureIndex);
                player.NatureIndex = 19;
                player.InstanceNature(player.NatureIndex);
                player.ReFreshAbllityPoint();
                break;
            case 50:
                //Spe+Atk-±¡ºÉ
                player.RemoveNature(player.NatureIndex);
                player.NatureIndex = 20;
                player.InstanceNature(player.NatureIndex);
                player.ReFreshAbllityPoint();
                break;
            case 51:
                //Spe+Def-±¡ºÉ
                player.RemoveNature(player.NatureIndex);
                player.NatureIndex = 21;
                player.InstanceNature(player.NatureIndex);
                player.ReFreshAbllityPoint();
                break;
            case 52:
                //Spe+SpA-±¡ºÉ
                player.RemoveNature(player.NatureIndex);
                player.NatureIndex = 22;
                player.InstanceNature(player.NatureIndex);
                player.ReFreshAbllityPoint();
                break;
            case 53:
                //Spe+SpD-±¡ºÉ
                player.RemoveNature(player.NatureIndex);
                player.NatureIndex = 23;
                player.InstanceNature(player.NatureIndex);
                player.ReFreshAbllityPoint();
                break;
            case 54:
                //Normal±¡ºÉ
                player.RemoveNature(player.NatureIndex);
                player.NatureIndex = 24;
                player.InstanceNature(player.NatureIndex);
                player.ReFreshAbllityPoint();
                break;



            //====================ÃÎ»ÃµÀ¾ß===========================
            case 55:
                //ÃÎ»ÃµÀ¾ß£ºÉÁË¸³çÐÄ£¨²Áµ¯£©
                Instantiate(aGameObjectByPassiveItem(4), player.transform.position + new Vector3(0f, 0.5f, 0f), Quaternion.identity, player.NotFollowBaby.transform);
                break;
            case 56:
                //ÃÎ»ÃµÀ¾ß£ºÐý×ª¹âÇò
                Instantiate(aGameObjectByPassiveItem(5), player.transform.position + new Vector3(0f, 0.5f, 0f), Quaternion.identity, player.NotFollowBaby.transform);
                break;
            case 57:
                //ÃÎ»ÃµÀ¾ß£º½û²Ý
                break;
            case 58:
                //ÃÎ»ÃµÀ¾ß£º¹ýÔØ
                break;


            //====================µÀ¾ß===========================
            case 59:
                //½ð¼ÙÑÀ
                break;

            case 60:
                //×ÔÐÐ³µ
                MoveSpwBounsAlways += 1;
                break;

            case 61:
                //Ì½±¦Æ÷
                break;

            case 62:
                //´®ÁªµçÂ·
                break;

            case 63:
                //ÈñÀûÖ®ÑÀ
                break;

            case 64:
                //Æ¤¿¨Çð·¢µç»ú
                break;

            case 65:
                //ÅÖ¶¡Âó¿Ë·ç
                break;

            case 66:
                //±¦¿ÉÃÎÕ½Æå
                SpeBounsAlways += 3;
                break;

            case 67:
                //³¬·Ê»¯
                MoveSpwBounsAlways -= 1;
                HPBounsAlways += 2;
                player.ReFreshAbllityPoint();
                player.ChangeHp(player.maxHp - player.Hp, 0, 0);
                UIHealthBar.Instance.MaxHpText.text = player.maxHp.ToString();
                break;

            case 68:
                //¾ø¶ÔË¯ÃßÕíÍ·
                player.ComeInANewRoomEvent += ComatosePillow;
                ComatosePillow(player);
                break;

            case 69:
                //°×Ë÷ÂÞÑÇ±¦±¦
                Instantiate(aGameObjectByPassiveItem(6), player.transform.position, Quaternion.identity, player.FollowBaby.transform);
                break;

            case 70:
                //Ð¡ÏÉÄÌ±¦±¦
                Instantiate(aGameObjectByPassiveItem(7), player.transform.position, Quaternion.identity, player.FollowBaby.transform);
                break;

            case 71:
                //²¨¿Ë±È±¦±¦
                Instantiate(aGameObjectByPassiveItem(8), player.transform.position, Quaternion.identity, player.FollowBaby.transform);
                break;

            case 72:
                //¸´»î»¯Ê¯
                break;

            case 73:
                //ÉÁÑæ¸ß¸è
                break;

            case 74:
                //ÈóË®·¢ÐÍ
                break;


            case 75:
                //Ç§±äÍò»¨
                Instantiate(aGameObjectByPassiveItem(22), player.transform.position, Quaternion.identity, player.FollowBaby.transform);
                break;

            case 76:
                //¾ÑÉä¶µÃ±
                player.ComeInANewRoomEvent += DecidueyeHood;
                DecidueyeHood(player);
                break;

            case 77:
                //¼ÄÉúÄ¢¹½
                player.ComeInANewRoomEvent += ParasiticSpores;
                ParasiticSpores(player);
                break;

            case 79:
                //·ÉÔÆ±ù¼¤ÁÜ
                MoveSpwBounsAlways += 1;
                break;

            case 80:
                //·ßÅ­ÂøÍ·
                AtkBounsAlways += 1;
                break;

            case 81:
                //É­Ö®Ñò¸þ
                SpABounsAlways += 1;
                break;

            case 82:
                //¸ªÑ×ÏÉ±´
                DefBounsAlways += 1;
                break;

            case 83:
                //Éî»ÒÃ×¹û
                SpDBounsAlways += 1;
                break;

            case 84:
                //ÄÑ³Ô±¦·Ò
                LuckBounsAlways -= 1;
                LuckHardWorkAlways -= 5.0f;
                break;

            case 85:
                //ßèßèÏÊÄÌ
                HPBounsAlways += 1;
                player.ReFreshAbllityPoint();
                player.ChangeHp(player.maxHp - player.Hp, 0, 0);
                UIHealthBar.Instance.MaxHpText.text = player.maxHp.ToString();
                break;

            case 86:
                //´óÂíÈøÀ­´ï
                SpeBounsAlways += 1;
                break;

            case 124:
                //¾§¹âÑ¿±¦±¦
                Instantiate(aGameObjectByPassiveItem(9), player.transform.position, Quaternion.identity, player.FollowBaby.transform);
                break;

            case 125:
                //Ð¡¶àÁú±¦±¦
                Instantiate(aGameObjectByPassiveItem(10), player.transform.position, Quaternion.identity, player.FollowBaby.transform);
                break;

            case 126:
                //Ô¹Ó°ÍÞÍÞ±¦±¦
                Instantiate(aGameObjectByPassiveItem(11), player.transform.position, Quaternion.identity, player.FollowBaby.transform);
                break;

            case 127:
                //Ð¡ÔÆÈ¸±¦±¦
                Instantiate(aGameObjectByPassiveItem(12), player.transform.position, Quaternion.identity, player.FollowBaby.transform);
                break;

            case 128:
                //ÂêÉ¯ÄÇ±¦±¦
                Instantiate(aGameObjectByPassiveItem(13), player.transform.position, Quaternion.identity, player.FollowBaby.transform);
                break;

            case 129:
                //¹´»êÑÛ±¦±¦
                Instantiate(aGameObjectByPassiveItem(14), player.transform.position, Quaternion.identity, player.FollowBaby.transform);
                break;

            case 130:
                //é»¹ûÇò±¦±¦
                Instantiate(aGameObjectByPassiveItem(15), player.transform.position, Quaternion.identity, player.FollowBaby.transform);
                break;

            case 131:
                //ë¦×Ó²Ý±¦±¦
                Instantiate(aGameObjectByPassiveItem(16), player.transform.position, Quaternion.identity, player.FollowBaby.transform);
                break;

            case 132:
                //Ð¡´Å¹Ö±¦±¦
                Instantiate(aGameObjectByPassiveItem(17), player.transform.position, Quaternion.identity, player.FollowBaby.transform);
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

    //===================================================¶ñÐÔÊÂ¼þ=================================================

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


    //===================================================¶ñÐÔÊÂ¼þ=================================================




    //===================================================Ê£·¹=================================================

    void Leftover(PlayerControler player)
    {
        if (Random.Range(0.0f , 1.0f)+((float)(player.LuckPoint)/30) >= 0.4f) {
            player.ChangeHp((int)Mathf.Clamp(player.maxHp / 10, 1, 10), 0, 0);
        }
    }

    //===================================================Ê£·¹=================================================




    //===================================================³äµçµç³Ø=================================================

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

    //===================================================³äµçµç³Ø=================================================







    //===================================================¾ø¶ÔË¯ÃßÕíÍ·=================================================

    void ComatosePillow(PlayerControler playerInput)
    {
        playerInput.SleepFloatPlus(1);
    }
    //===================================================¾ø¶ÔË¯ÃßÕíÍ·=================================================



    //===================================================¾ÑÉä¶µÃ±=================================================
    void DecidueyeHood(PlayerControler playerInput)
    {
        GameObject EmptyFile = MapCreater.StaticMap.RRoom[player.NowRoom].transform.GetChild(3).gameObject;
        for (int i = 0; i < EmptyFile.transform.childCount; i ++)
        {
            Empty e = EmptyFile.transform.GetChild(i).GetComponent<Empty>();
            if (e != null)
            {
                e.Blind(5.0f, 5.0f);
            }
        }
    }
    //===================================================¾ÑÉä¶µÃ±=================================================





    //===================================================¼ÄÉúÄ¢¹½=================================================
    void ParasiticSpores(PlayerControler playerInput)
    {
        if (playerInput.Hp >= player.maxHp/2)
        {
            Pokemon.PokemonHpChange(null , this.gameObject , playerInput.maxHp / 8 , 0 , 0 , Type.TypeEnum.IgnoreType);
        }
        else
        {
            Pokemon.PokemonHpChange(null, this.gameObject, 0, 0, playerInput.maxHp / 8, Type.TypeEnum.IgnoreType);
        }
    }
    //===================================================¼ÄÉúÄ¢¹½=================================================





    //===================================================µ±±»¶¯µÀ¾ßÐèÒªÉú³ÉÊµÌåÊ±=================================================


    GameObject aGameObjectByPassiveItem(int i)
    {
        GameObject OutPutObj = PassiveItemGameObjList.ObjList.List[i];
        return OutPutObj;
    }





    //===================================================µ±±»¶¯µÀ¾ßÐèÒªÉú³ÉÊµÌåÊ±=================================================
}

