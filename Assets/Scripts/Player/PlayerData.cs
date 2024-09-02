using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public PassiveList passiveList;

    public Type.TypeEnum Type01Always;
    public Type.TypeEnum Type02Always;

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

        ViciousEventIndex = OtherData.ViciousEventIndex;
        AttackWeightCount = OtherData.AttackWeightCount;
        SpAtkSpecsCount = OtherData.SpAtkSpecsCount;
        ClamMindUpDone = OtherData.ClamMindUpDone;
        ThroatSprayUpDone = OtherData.ThroatSprayUpDone;
        ResurrectionFossilDone = OtherData.ResurrectionFossilDone;
        IsMasqueradeChangeTypeDone = OtherData.IsMasqueradeChangeTypeDone;
        IsKingsShieldDone = OtherData.IsKingsShieldDone;
        MetronomeSkillIndex = OtherData.MetronomeSkillIndex;
        MetronomeCount = OtherData.MetronomeCount;


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






    private void Update()
    {
        if (player == null) { player = GetComponent<PlayerControler>(); }
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
                if (HPHardWorkAlways >= 0) { HPHardWorkAlways = 0; player.HWPShow(-HPHardWorkAlways, 0); }
                if (AtkHardWorkAlways >= 0) { AtkHardWorkAlways = 0; player.HWPShow(-AtkHardWorkAlways, 1); }
                if (DefHardWorkAlways >= 0) { DefHardWorkAlways = 0; player.HWPShow(-DefHardWorkAlways, 2); }
                if (SpAHardWorkAlways >= 0) { SpAHardWorkAlways = 0; player.HWPShow(-SpAHardWorkAlways, 3); }
                if (SpDHardWorkAlways >= 0) { SpDHardWorkAlways = 0; player.HWPShow(-SpDHardWorkAlways, 4); }
                if (SpeHardWorkAlways >= 0) { SpeHardWorkAlways = 0; player.HWPShow(-SpeHardWorkAlways, 5); }
                if (LuckHardWorkAlways >= 0) { LuckHardWorkAlways = 0; player.HWPShow(-LuckHardWorkAlways, 7); }
                if (MoveSpeHardWorkAlways >= 0) { MoveSpeHardWorkAlways = 0; player.HWPShow(-MoveSpeHardWorkAlways, 6); }
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
                LuckHardWorkAlways += 2;
                player.HWPShow(2.0f, 7);
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
                //̫����θ��
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
                //����֮��
                break;

            case 18:
                //��������
                MoveSpwBounsAlways--;
                MoveSpeHardWorkAlways -= 0.5f;
                break;

            case 19:
                //��������
                MoveSpwBounsAlways--;
                MoveSpeHardWorkAlways -= 0.5f;
                break;

            case 20:
                //��������
                MoveSpwBounsAlways--;
                MoveSpeHardWorkAlways -= 0.5f;
                break;

            case 21:
                //������
                MoveSpwBounsAlways--;
                MoveSpeHardWorkAlways -= 0.5f;
                break;

            case 22:
                //��������
                MoveSpwBounsAlways--;
                MoveSpeHardWorkAlways -= 0.5f;
                break;

            case 23:
                //��������
                MoveSpwBounsAlways--;
                MoveSpeHardWorkAlways -= 0.5f;
                break;

            case 24:
                //�����β���
                player.ToxicFloatPlus(1);
                break;

            case 25:
                //����֤֮
                break;

            case 26:
                //����
                break;

            case 27:
                //�ྻ֮��
                player.isToxicDef = true;
                player.isParalysisDef = true;
                player.isBurnDef = true;
                player.isSleepDef = true;
                player.isFrozenDef = true;
                MoveSpwBounsAlways -= 3;
                MoveSpeHardWorkAlways = 0;
                break;

            case 28:
                //�������ָ��
                break;

            case 29:
                //�����뿪��
                break;

            case 30:
                //ľľ�ɱ���
                Instantiate(aGameObjectByPassiveItem(0), player.transform.position, Quaternion.identity, player.FollowBaby.transform);
                break;
            case 31:
                //С������
                Instantiate(aGameObjectByPassiveItem(1), player.transform.position, Quaternion.identity, player.FollowBaby.transform);
                break;
            case 32:
                //�����鱦��
                Instantiate(aGameObjectByPassiveItem(2), player.transform.position, Quaternion.identity, player.FollowBaby.transform);
                break;
            case 33:
                //������
                Instantiate(aGameObjectByPassiveItem(3), player.transform.position, Quaternion.identity, player.FollowBaby.transform);
                break;


            //====================����===========================

            case 34:
                //Atk+Def-����
                player.RemoveNature(player.NatureIndex);
                player.NatureIndex = 1;
                player.InstanceNature(player.NatureIndex);
                player.ReFreshAbllityPoint();
                break;
            case 35:
                //Atk+SpA-����
                player.RemoveNature(player.NatureIndex);
                player.NatureIndex = 2;
                player.InstanceNature(player.NatureIndex);
                player.ReFreshAbllityPoint();
                break;
            case 36:
                //Atk+SpD-����
                player.RemoveNature(player.NatureIndex);
                player.NatureIndex = 3;
                player.InstanceNature(player.NatureIndex);
                player.ReFreshAbllityPoint();
                break;
            case 37:
                //Atk+Spe-����
                player.RemoveNature(player.NatureIndex);
                player.NatureIndex = 4;
                player.InstanceNature(player.NatureIndex);
                player.ReFreshAbllityPoint();
                break;
            case 38:
                //Def+Atk-����
                player.RemoveNature(player.NatureIndex);
                player.NatureIndex = 5;
                player.InstanceNature(player.NatureIndex);
                player.ReFreshAbllityPoint();
                break;
            case 39:
                //Def+SpA-����
                player.RemoveNature(player.NatureIndex);
                player.NatureIndex = 7;
                player.InstanceNature(player.NatureIndex);
                player.ReFreshAbllityPoint();
                break;
            case 40:
                //Def+SpD-����
                player.RemoveNature(player.NatureIndex);
                player.NatureIndex = 8;
                player.InstanceNature(player.NatureIndex);
                player.ReFreshAbllityPoint();
                break;
            case 41:
                //Def+Spe-����
                player.RemoveNature(player.NatureIndex);
                player.NatureIndex = 9;
                player.InstanceNature(player.NatureIndex);
                player.ReFreshAbllityPoint();
                break;
            case 42:
                //SpA+Atk-����
                player.RemoveNature(player.NatureIndex);
                player.NatureIndex = 10;
                player.InstanceNature(player.NatureIndex);
                player.ReFreshAbllityPoint();
                break;
            case 43:
                //SpA+Def-����
                player.RemoveNature(player.NatureIndex);
                player.NatureIndex = 11;
                player.InstanceNature(player.NatureIndex);
                player.ReFreshAbllityPoint();
                break;
            case 44:
                //SpA+SpD-����
                player.RemoveNature(player.NatureIndex);
                player.NatureIndex = 13;
                player.InstanceNature(player.NatureIndex);
                player.ReFreshAbllityPoint();
                break;
            case 45:
                //SpA+Spe-����
                player.RemoveNature(player.NatureIndex);
                player.NatureIndex = 14;
                player.InstanceNature(player.NatureIndex);
                player.ReFreshAbllityPoint();
                break;
            case 46:
                //SpD+Atk-����
                player.RemoveNature(player.NatureIndex);
                player.NatureIndex = 15;
                player.InstanceNature(player.NatureIndex);
                player.ReFreshAbllityPoint();
                break;
            case 47:
                //SpD+Def-����
                player.RemoveNature(player.NatureIndex);
                player.NatureIndex = 16;
                player.InstanceNature(player.NatureIndex);
                player.ReFreshAbllityPoint();
                break;
            case 48:
                //SpD+SpA-����
                player.RemoveNature(player.NatureIndex);
                player.NatureIndex = 17;
                player.InstanceNature(player.NatureIndex);
                player.ReFreshAbllityPoint();
                break;
            case 49:
                //SpD+Spe-����
                player.RemoveNature(player.NatureIndex);
                player.NatureIndex = 19;
                player.InstanceNature(player.NatureIndex);
                player.ReFreshAbllityPoint();
                break;
            case 50:
                //Spe+Atk-����
                player.RemoveNature(player.NatureIndex);
                player.NatureIndex = 20;
                player.InstanceNature(player.NatureIndex);
                player.ReFreshAbllityPoint();
                break;
            case 51:
                //Spe+Def-����
                player.RemoveNature(player.NatureIndex);
                player.NatureIndex = 21;
                player.InstanceNature(player.NatureIndex);
                player.ReFreshAbllityPoint();
                break;
            case 52:
                //Spe+SpA-����
                player.RemoveNature(player.NatureIndex);
                player.NatureIndex = 22;
                player.InstanceNature(player.NatureIndex);
                player.ReFreshAbllityPoint();
                break;
            case 53:
                //Spe+SpD-����
                player.RemoveNature(player.NatureIndex);
                player.NatureIndex = 23;
                player.InstanceNature(player.NatureIndex);
                player.ReFreshAbllityPoint();
                break;
            case 54:
                //Normal����
                player.RemoveNature(player.NatureIndex);
                player.NatureIndex = 24;
                player.InstanceNature(player.NatureIndex);
                player.ReFreshAbllityPoint();
                break;



            //====================�λõ���===========================
            case 55:
                //�λõ��ߣ���˸���ģ�������
                Instantiate(aGameObjectByPassiveItem(4), player.transform.position + new Vector3(0f, 0.5f, 0f), Quaternion.identity, player.NotFollowBaby.transform);
                break;
            case 56:
                //�λõ��ߣ���ת����
                Instantiate(aGameObjectByPassiveItem(5), player.transform.position + new Vector3(0f, 0.5f, 0f), Quaternion.identity, player.NotFollowBaby.transform);
                break;
            case 57:
                //�λõ��ߣ�����
                break;
            case 58:
                //�λõ��ߣ�����
                break;


            //====================����===========================
            case 59:
                //�����
                break;

            case 60:
                //���г�
                MoveSpwBounsAlways += 1;
                break;

            case 61:
                //̽����
                break;

            case 62:
                //������·
                break;

            case 63:
                //����֮��
                break;

            case 64:
                //Ƥ���𷢵��
                break;

            case 65:
                //�ֶ���˷�
                break;

            case 66:
                //������ս��
                SpeBounsAlways += 3;
                break;

            case 67:
                //���ʻ�
                MoveSpwBounsAlways -= 1;
                HPBounsAlways += 2;
                player.ReFreshAbllityPoint();
                player.ChangeHp(player.maxHp - player.Hp, 0, 0);
                UIHealthBar.Instance.MaxHpText.text = player.maxHp.ToString();
                break;

            case 68:
                //����˯����ͷ
                player.isToxicDef = true;
                player.isParalysisDef = true;
                player.isBurnDef = true;
                player.isFrozenDef = true;
                player.ComeInANewRoomEvent += ComatosePillow;
                ComatosePillow(player);
                break;

            case 69:
                //�������Ǳ���
                Instantiate(aGameObjectByPassiveItem(6), player.transform.position, Quaternion.identity, player.FollowBaby.transform);
                break;

            case 70:
                //С���̱���
                Instantiate(aGameObjectByPassiveItem(7), player.transform.position, Quaternion.identity, player.FollowBaby.transform);
                break;

            case 71:
                //���˱ȱ���
                Instantiate(aGameObjectByPassiveItem(8), player.transform.position, Quaternion.identity, player.FollowBaby.transform);
                break;

            case 72:
                //���ʯ
                break;

            case 73:
                //����߸�
                break;

            case 74:
                //��ˮ����
                break;


            case 75:
                //ǧ����
                Instantiate(aGameObjectByPassiveItem(22), player.transform.position, Quaternion.identity, player.FollowBaby.transform);
                break;

            case 76:
                //���䶵ñ
                player.ComeInANewRoomEvent += DecidueyeHood;
                DecidueyeHood(player);
                break;

            case 77:
                //����Ģ��
                player.ComeInANewRoomEvent += ParasiticSpores;
                ParasiticSpores(player);
                break;

            case 78:
                //������
                break;

            case 79:
                //���Ʊ�����
                MoveSpwBounsAlways += 1;
                break;

            case 80:
                //��ŭ��ͷ
                AtkBounsAlways += 1;
                break;

            case 81:
                //ɭ֮���
                SpABounsAlways += 1;
                break;

            case 82:
                //�����ɱ�
                DefBounsAlways += 1;
                break;

            case 83:
                //����׹�
                SpDBounsAlways += 1;
                break;

            case 84:
                //�ѳԱ���
                LuckBounsAlways -= 1;
                LuckHardWorkAlways -= 5.0f;
                player.HWPShow(-5.0f, 7);
                break;

            case 85:
                //��������
                HPBounsAlways += 1;
                player.ReFreshAbllityPoint();
                player.ChangeHp(player.maxHp - player.Hp, 0, 0);
                UIHealthBar.Instance.MaxHpText.text = player.maxHp.ToString();
                break;

            case 86:
                //����������
                SpeBounsAlways += 1;
                break;

            case 87:
                //�ϴ���۾�
                break;

            case 88:
                //�ڰ�˹����
                AeosEnergy(player);
                break;

            case 89:
                //�͹�����
                player.ComeInANewRoomEvent += ResetAttackWeightCount;
                ResetAttackWeightCount(player);
                break;

            case 90:
                //�����۾�
                player.ComeInANewRoomEvent += ResetSpAtkSpecsCount;
                ResetSpAtkSpecsCount(player);
                break;



            case 91:
                //��ˮ���｣
                break;

            case 92:
                //���߶���
                player.ComeInANewRoomEvent += ResetKingsShieldDone;
                ResetKingsShieldDone(player);
                break;

            case 93:
                //ħ�ü���
                player.ComeInANewRoomEvent += ResetMasqueradeChangeType;
                ResetMasqueradeChangeType(player);
                break;

            case 94:
                //���б��Ľ�
                Instantiate(aGameObjectByPassiveItem(31), player.transform.position, Quaternion.identity, player.NotFollowBaby.transform);
                break;

            case 95:
                //���б��Ķ�
                Instantiate(aGameObjectByPassiveItem(32), player.transform.position, Quaternion.identity, player.NotFollowBaby.transform);
                break;

            case 96:
                //96 �侲ͷ��
                player.ComeInANewRoomEvent += ResetClamMind;
                ResetClamMind(player);
                break;

            case 97:
                //97 ������
                break;

            case 98:
                //98 �ֻ�Ь
                MoveSpwBounsAlways += 2;
                break;

            case 99:
                //99 ���
                break;

            case 100:
                //100 ���ɷ�����װ
                break;

            case 101:
                //101 ��ʿ������
                break;

            case 102:
                //102��־ʿ��ǩ��
                LuckBounsAlways += 1;
                LuckHardWorkAlways += 2;
                player.HWPShow(2.0f, 7);
                break;

            case 103:
                //103 ���ܲ�
                TypeDefAlways[15]++;
                break;

            case 104:
                //104 ���ܲ�
                TypeDefAlways[8]++;
                break;

            case 105:
                //105 ̫����
                if (player.PlayerType02 != (int)Type.TypeEnum.No) {
                    switch (Random.Range(0, 2))
                    {
                        case 0:
                            player.TeraTypeChange(player.PlayerType01);
                            break;
                        case 1:
                            player.TeraTypeChange(player.PlayerType02);
                            break;
                    }
                }
                else
                {
                    player.TeraTypeChange(player.PlayerType01);
                }
                break;

            case 106:
                //106 ��֮����
                TypeDefAlways[17]++;
                break;

            case 107:
                //107 ˮ֮����
                TypeDefAlways[11]++;
                break;

            case 108:
                //108 �̲����
                player.TeraTypeChange((int)Type.TypeEnum.Grass);
                break;

            case 109:
                //109 ˮ�����
                player.TeraTypeChange((int)Type.TypeEnum.Water);
                break;

            case 110:
                //110 �������
                player.TeraTypeChange((int)Type.TypeEnum.Fire);
                break;

            case 111:
                //111 ��ʯ���
                player.TeraTypeChange((int)Type.TypeEnum.Rock);
                break;

            case 112:
                //112 ���˴�
                break;

            case 113:
                //113 ��֮β
                DefBounsAlways += 1;
                SpDBounsAlways += 1;
                SpeBounsAlways -= 2;
                break;

            case 114:
                //114 ������
                break;

            case 115:
                //115 ͹͹ͷ��
                break;

            case 116:
                //116 ������֮��
                break;

            case 117:
                //117 ˬ������
                player.ComeInANewRoomEvent += ResetThroatSpray;
                ResetThroatSpray(player);
                break;

            case 118:
                //118 ����ɡ
                break;

            case 119:
                //119 ��������
                break;

            case 120:
                //120 ��̽����
                break;

            case 121:
                //121 ��֥��
                EnigmaBerry();
                break;

            case 122:
                //122 ������Ŀ��
                break;

            case 123:
                //123 ������ֲ��
                player.ClearThisRoomEvent += BerryPotDropBerry;
                BerryPotDropBerry(player);
                break;

            case 124:
                //����ѿ����
                Instantiate(aGameObjectByPassiveItem(9), player.transform.position, Quaternion.identity, player.FollowBaby.transform);
                break;

            case 125:
                //С��������
                Instantiate(aGameObjectByPassiveItem(10), player.transform.position, Quaternion.identity, player.FollowBaby.transform);
                break;

            case 126:
                //ԹӰ���ޱ���
                Instantiate(aGameObjectByPassiveItem(11), player.transform.position, Quaternion.identity, player.FollowBaby.transform);
                break;

            case 127:
                //С��ȸ����
                Instantiate(aGameObjectByPassiveItem(12), player.transform.position, Quaternion.identity, player.FollowBaby.transform);
                break;

            case 128:
                //��ɯ�Ǳ���
                Instantiate(aGameObjectByPassiveItem(13), player.transform.position, Quaternion.identity, player.FollowBaby.transform);
                break;

            case 129:
                //�����۱���
                Instantiate(aGameObjectByPassiveItem(14), player.transform.position, Quaternion.identity, player.FollowBaby.transform);
                break;

            case 130:
                //黹��򱦱�
                Instantiate(aGameObjectByPassiveItem(15), player.transform.position, Quaternion.identity, player.FollowBaby.transform);
                break;

            case 131:
                //��Ӳݱ���
                Instantiate(aGameObjectByPassiveItem(16), player.transform.position, Quaternion.identity, player.FollowBaby.transform);
                break;

            case 132:
                //С�Źֱ���
                Instantiate(aGameObjectByPassiveItem(17), player.transform.position, Quaternion.identity, player.FollowBaby.transform);
                break;

            case 133:
                //����133 �Խ𻬰�
                MoveSpwBounsAlways += 2;
                break;

            case 134:
                //����134 �����̾�ִ��
                break;

            case 135:
                //����135 �徻׹��
                break;

            case 136:
                //����136 ������ 
                break;

            case 137:
                //����137 ���㾵
                break;

            case 138:
                //����138 ����֮צ
                break;

        }

        player.ReFreshAbllityPoint();
        GetPassiveItemList.Add(ItemIndex);
        IsPassiveGetList[ItemIndex] = true;

        //����28 �������ָ��
        if (IsPassiveGetList[28] && GetPassiveItem.ItemTypeTag != null)
        {
            foreach (int i in GetPassiveItem.ItemTypeTag)
            {
                if (i == 1) { player.ChangeHp(Mathf.Clamp(player.maxHp / 16, 1, 10), 0, 19); }
            }
        }
        //����100 ���ɷ�����װ
        if (IsPassiveGetList[100] && GetPassiveItem.ItemTypeTag != null)
        {
            foreach (int i in GetPassiveItem.ItemTypeTag)
            {
                if (i == 1) { player.ChangeHPW(new Vector2Int(Random.Range(1, 7), 3)); }
            }
        }
    }

    //===================================================�����¼�=================================================

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
                case 5:
                    playerInput.PlayerFrozenRemove();
                    break;
            }
        }
        ViciousEventIndex = Random.Range(1,6);
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
            case 5:
                playerInput.PlayerFrozenFloatPlus(1.0f , 1.5f);
                break;
        }
    }


    //===================================================�����¼�=================================================




    //===================================================ʣ��=================================================

    void Leftover(PlayerControler player)
    {
        if (Random.Range(0.0f , 1.0f)+((float)(player.LuckPoint)/30) >= 0.4f) {
            Pokemon.PokemonHpChange(null, player.gameObject, 0, 0, (int)Mathf.Clamp(player.maxHp / 10, 1, 10), Type.TypeEnum.IgnoreType);
            //player.ChangeHp((int)Mathf.Clamp(player.maxHp / 10, 1, 10), 0, 0);
        }
    }

    //===================================================ʣ��=================================================




    //===================================================�����=================================================

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

    //===================================================�����=================================================







    //===================================================����˯����ͷ=================================================

    void ComatosePillow(PlayerControler playerInput)
    {
        float x = Random.Range(0.0f, 1.0f);
        Debug.Log(x);
        if (x > 0.5f) {
            playerInput.SleepFloatPlus(1);
        }
    }
    //===================================================����˯����ͷ=================================================



    //===================================================���䶵ñ=================================================
    void DecidueyeHood(PlayerControler playerInput)
    {

        GameObject EmptyFile = null;
        if (MapCreater.StaticMap.RRoom.ContainsKey(playerInput.NowRoom) && MapCreater.StaticMap.RRoom[playerInput.NowRoom].transform.childCount > 3) {
            EmptyFile = MapCreater.StaticMap.RRoom[playerInput.NowRoom].transform.GetChild(3).gameObject;
            //Debug.Log("decidue" + "+" + playerInput.gameObject.name  + "+" + EmptyFile.gameObject.name + "+" + EmptyFile.transform.gameObject.name + "+" + EmptyFile.transform.childCount);
            if (EmptyFile && EmptyFile.transform.childCount > 0) {
                
                for (int i = 0; i < EmptyFile.transform.childCount; i++)
                {
                    Empty e = EmptyFile.transform.GetChild(i).GetComponent<Empty>();
                    if (e != null)
                    {
                        e.Blind(5.0f, 5.0f);
                    }
                }
            }
        }

    }
    //===================================================���䶵ñ=================================================





    //===================================================����Ģ��=================================================
    void ParasiticSpores(PlayerControler playerInput)
    {
        if (playerInput.Hp >= playerInput.maxHp/2)
        {
            Pokemon.PokemonHpChange(null , playerInput.gameObject , playerInput.maxHp / 8 , 0 , 0 , Type.TypeEnum.IgnoreType);
        }
        else
        {
            Pokemon.PokemonHpChange(null, playerInput.gameObject, 0, 0, playerInput.maxHp / 8, Type.TypeEnum.IgnoreType);
        }
    }
    //===================================================����Ģ��=================================================












    //===================================================�ڰ�˹����=================================================
    void AeosEnergy(PlayerControler playerInput)
    {
        List<int> Index = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8 };

        for (int x=0; x < 100; x++)
        {
            int r = Random.Range(0, 8);
            int e = Index[r];
            Index[r] = Index[7];
            Index[7] = e;

        }

        for (int i = 0; i < 8; i++) 
        {
            AeosEnergyChangeAbility(playerInput , Index[i] , i%2 == 0 );
        }
    }

    void AeosEnergyChangeAbility(PlayerControler playerInput , int i , bool UpOrDown)
    {
        switch (i)
        {
            case 1:
                playerInput.playerData.HPBounsAlways += ((UpOrDown)? 1 : -1);
                playerInput.ReFreshAbllityPoint();
                playerInput.ChangeHp(playerInput.maxHp - playerInput.Hp, 0, 0);
                UIHealthBar.Instance.MaxHpText.text = playerInput.maxHp.ToString();
                break;
            case 2:
                playerInput.playerData.AtkBounsAlways += ((UpOrDown) ? 1 : -1);
                break;
            case 3:
                playerInput.playerData.DefBounsAlways += ((UpOrDown) ? 1 : -1);
                break;
            case 4:
                playerInput.playerData.SpABounsAlways += ((UpOrDown) ? 1 : -1);
                break;
            case 5:
                playerInput.playerData.SpDBounsAlways += ((UpOrDown) ? 1 : -1);
                break;
            case 6:
                playerInput.playerData.SpeBounsAlways += ((UpOrDown) ? 1 : -1);
                break;
            case 7:
                playerInput.playerData.MoveSpwBounsAlways += ((UpOrDown) ? 1 : -1);
                break;
            case 8:
                playerInput.playerData.LuckBounsAlways += ((UpOrDown) ? 1 : -1);
                break;
        }
    }
    //===================================================�ڰ�˹����=================================================











    //===================================================�͹�����=================================================

    public int AttackWeightCount { get { return attackWeightCount; } set { attackWeightCount = value; } }
    int attackWeightCount;
    void ResetAttackWeightCount(PlayerControler playerInput)
    {
        playerInput.playerData.AttackWeightCount = 0;
    }

    //===================================================�͹�����=================================================









    //===================================================�����۾�=================================================

    public int SpAtkSpecsCount { get { return spAtkSpecsCount; } set { spAtkSpecsCount = value; } }
    int spAtkSpecsCount;
    void ResetSpAtkSpecsCount(PlayerControler playerInput)
    {
        playerInput.playerData.SpAtkSpecsCount = 0;
    }

    //===================================================�����۾�=================================================






    //===================================================�侲ͷ��=================================================
    public bool ClamMindUpDone { get { return clamMindUpDone; } set { clamMindUpDone = value; } }
    bool clamMindUpDone;
    public void PassiveItemClamMind()
    {
        if (!ClamMindUpDone)
        {
            ClamMindUpDone = true;
            player.playerData.SpABounsJustOneRoom += 2;
            player.ReFreshAbllityPoint();
        }
    }

    void ResetClamMind(PlayerControler playerInput)
    {
        playerInput.playerData.ClamMindUpDone = false;
    }

    //===================================================�侲ͷ��=================================================





    //===================================================ˬ������=================================================
    public bool ThroatSprayUpDone { get { return throatSprayUpDone; } set { throatSprayUpDone = value; } }
    bool throatSprayUpDone;
    public void PassiveItemThroatSpray()
    {
        if (!ThroatSprayUpDone)
        {
            ThroatSprayUpDone = true;
            player.playerData.SpABounsJustOneRoom += 2;
            player.ReFreshAbllityPoint();
        }
    }

    void ResetThroatSpray(PlayerControler playerInput)
    {
        playerInput.playerData.ThroatSprayUpDone = false;
    }

    //===================================================ˬ������=================================================






    //===================================================������ֲ��=================================================
    void BerryPotDropBerry(PlayerControler playerInput)
    {
        if (Random.Range(0.0f , 1.0f) + (float)playerInput.LuckPoint / 30 > 0.65f)
        {
            if (Random.Range(0.0f, 1.0f) > 0.6f)
            {
                Instantiate( PassiveItemGameObjList.ObjList.List[25] , transform.position , Quaternion.identity ).GetComponent<HealthUpCCg>().isLunch = true;
            }
            else
            {
                Instantiate(PassiveItemGameObjList.ObjList.List[26], transform.position, Quaternion.identity).GetComponent<RandomBerryTypeDef>().isLunch = true;
            }
        }
    }

    //===================================================������ֲ��==================================================





    //===================================================ħ�ü���==================================================
    public bool IsMasqueradeChangeTypeDone { get { return isMasqueradeChangeTypeDone; } set { isMasqueradeChangeTypeDone = value; } }
    bool isMasqueradeChangeTypeDone;

    void ResetMasqueradeChangeType(PlayerControler playerInput)
    {
        if (isMasqueradeChangeTypeDone)
        {
            isMasqueradeChangeTypeDone = false;
            playerInput.PlayerType01 = (int)playerInput.playerData.Type01Always;
            playerInput.PlayerType02 = (int)playerInput.playerData.Type02Always;
        }
    }

    public void MasqueradeChangeType(Type.TypeEnum t)
    {
        if (!isMasqueradeChangeTypeDone)
        {
            isMasqueradeChangeTypeDone = true;
            player.PlayerType01 = (int)t;
            player.PlayerType02 = 0;
        }
    }




    //===================================================ħ�ü���==================================================




    //===================================================���ʯ==================================================
    public bool ResurrectionFossilDone { get { return resurrectionFossilDone; } set { resurrectionFossilDone = value; } }
    bool resurrectionFossilDone;
    //===================================================���ʯ==================================================












    //=====================================================���߶���=========================================================
    public bool IsKingsShieldDone { get { return isKingsShieldDone; } set { isKingsShieldDone = value; } }
    bool isKingsShieldDone;

    void ResetKingsShieldDone(PlayerControler playerInput)
    {
        playerInput.playerData.IsKingsShieldDone = true;
        playerInput.playerData.DefBounsJustOneRoom += 2;
        playerInput.playerData.SpDBounsJustOneRoom += 2;
        playerInput.ReFreshAbllityPoint();
    }

    public void KingsShieldDone()
    {
        if (IsKingsShieldDone)
        {
            IsKingsShieldDone = false;
            DefBounsJustOneRoom -= 2;
            SpDBounsJustOneRoom -= 2;
            player.ReFreshAbllityPoint();
        }
    }

    //=====================================================���߶���=========================================================






    //=====================================================������=========================================================

    public int MetronomeCount { get { return metronomeCount; } set { metronomeCount = value; } }
    int metronomeCount;

    public int MetronomeSkillIndex { get { return metronomeSkillIndex; } set { metronomeSkillIndex = value; } }
    int metronomeSkillIndex;



    //=====================================================������=========================================================
















    //=====================================================��֥��=========================================================
    void EnigmaBerry()
    {
        List<int> NewTypeDefList = new List<int> 
        { Random.Range(-1,2) , Random.Range(-1, 2), Random.Range(-1, 2), Random.Range(-1, 2), Random.Range(-1, 2), Random.Range(-1, 2),
        Random.Range(-1,2) , Random.Range(-1, 2), Random.Range(-1, 2), Random.Range(-1, 2), Random.Range(-1, 2), Random.Range(-1, 2),
        Random.Range(-1,2) , Random.Range(-1, 2), Random.Range(-1, 2), Random.Range(-1, 2), Random.Range(-1, 2), Random.Range(-1, 2)};
        int TypeSum = 0;

        Debug.Log(NewTypeDefList[0].ToString() + NewTypeDefList[1].ToString() + NewTypeDefList[2].ToString() + NewTypeDefList[3].ToString() + NewTypeDefList[4].ToString() + NewTypeDefList[5].ToString() +
            NewTypeDefList[6].ToString() + NewTypeDefList[7].ToString() + NewTypeDefList[8].ToString() + NewTypeDefList[9].ToString() + NewTypeDefList[10].ToString() + NewTypeDefList[11].ToString() +
            NewTypeDefList[12].ToString() + NewTypeDefList[13].ToString() + NewTypeDefList[14].ToString() + NewTypeDefList[15].ToString() + NewTypeDefList[16].ToString() + NewTypeDefList[17].ToString() );

        for (int i = 1; i < 19; i++)
        {

            TypeSum += TypeDefAlways[i] + NewTypeDefList[i-1];
            //Debug.Log(TypeSum.ToString() + NewTypeDefList[i - 1].ToString());
        }

        Debug.Log(TypeSum);

        if (TypeSum != 0)
        {
            for (int j = 0; j < Mathf.Abs(TypeSum); j++)
            {
                NewTypeDefList[Random.Range(0, 18)] += ((TypeSum > 0)? -1 : +1);
            }
        }

        Debug.Log(NewTypeDefList[0].ToString() + NewTypeDefList[1].ToString() + NewTypeDefList[2].ToString() + NewTypeDefList[3].ToString() + NewTypeDefList[4].ToString() + NewTypeDefList[5].ToString() +
    NewTypeDefList[6].ToString() + NewTypeDefList[7].ToString() + NewTypeDefList[8].ToString() + NewTypeDefList[9].ToString() + NewTypeDefList[10].ToString() + NewTypeDefList[11].ToString() +
    NewTypeDefList[12].ToString() + NewTypeDefList[13].ToString() + NewTypeDefList[14].ToString() + NewTypeDefList[15].ToString() + NewTypeDefList[16].ToString() + NewTypeDefList[17].ToString());

        for (int i = 1; i < 19; i++)
        {
            TypeDefAlways[i] = NewTypeDefList[i - 1];
        }
    }
    //=====================================================��֥��=========================================================







    //===================================================������������Ҫ����ʵ��ʱ=================================================


    GameObject aGameObjectByPassiveItem(int i)
    {
        GameObject OutPutObj = PassiveItemGameObjList.ObjList.List[i];
        return OutPutObj;
    }





    //===================================================������������Ҫ����ʵ��ʱ=================================================
}

