using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseSpaceItem : MonoBehaviour
{
    // Start is called before the first frame update
    public static void UsedSpaceItem(PlayerControler player)
    {
        SpaceItem spaceItem = player.spaceItem.GetComponent<SpaceItem>();
        int ItemNum = spaceItem.ItemNum;
        switch (ItemNum)
        {
            //´®¶´Éþ
            case 0:
                EscapeRope(player);
                break;


            //Çý³æÑÌÎí
            case 1:
                Repel(player);
                break;


            //ÎÄèÖ¹û
            case 2:
                SitrusBerry(player);
                break;


            //Ì«¾§ËéÆ¬
            case 3:
                Debug.Log(ItemNum);
                TeraShard(player, ItemNum);
                if (player.playerData.IsPassiveGetList[15]) { player.ChangeHPW(new Vector2Int(Random.Range(1, 7), 4)); }
                break;
            case 4:
                TeraShard(player, ItemNum);
                if (player.playerData.IsPassiveGetList[15]) { player.ChangeHPW(new Vector2Int(Random.Range(1, 7), 4)); }
                break;
            case 5:
                TeraShard(player, ItemNum);
                if (player.playerData.IsPassiveGetList[15]) { player.ChangeHPW(new Vector2Int(Random.Range(1, 7), 4)); }
                break;
            case 6:
                TeraShard(player, ItemNum);
                if (player.playerData.IsPassiveGetList[15]) { player.ChangeHPW(new Vector2Int(Random.Range(1, 7), 4)); }
                break;
            case 7:
                TeraShard(player, ItemNum);
                if (player.playerData.IsPassiveGetList[15]) { player.ChangeHPW(new Vector2Int(Random.Range(1, 7), 4)); }
                break;

            case 8:
                TeraShard(player, ItemNum);
                if (player.playerData.IsPassiveGetList[15]) { player.ChangeHPW(new Vector2Int(Random.Range(1, 7), 4)); }
                break;
            case 9:
                Debug.Log(ItemNum);
                TeraShard(player, ItemNum);
                if (player.playerData.IsPassiveGetList[15]) { player.ChangeHPW(new Vector2Int(Random.Range(1, 7), 4)); }
                break;
            case 10:
                TeraShard(player, ItemNum);
                if (player.playerData.IsPassiveGetList[15]) { player.ChangeHPW(new Vector2Int(Random.Range(1, 7), 4)); }
                break;
            case 11:
                TeraShard(player, ItemNum);
                if (player.playerData.IsPassiveGetList[15]) { player.ChangeHPW(new Vector2Int(Random.Range(1, 7), 4)); }
                break;
            case 12:
                TeraShard(player, ItemNum);
                if (player.playerData.IsPassiveGetList[15]) { player.ChangeHPW(new Vector2Int(Random.Range(1, 7), 4)); }
                break;

            case 13:
                TeraShard(player, ItemNum);
                if (player.playerData.IsPassiveGetList[15]) { player.ChangeHPW(new Vector2Int(Random.Range(1, 7), 4)); }
                break;
            case 14:
                TeraShard(player, ItemNum);
                if (player.playerData.IsPassiveGetList[15]) { player.ChangeHPW(new Vector2Int(Random.Range(1, 7), 4)); }
                break;
            case 15:
                TeraShard(player, ItemNum);
                if (player.playerData.IsPassiveGetList[15]) { player.ChangeHPW(new Vector2Int(Random.Range(1, 7), 4)); }
                break;
            case 16:
                TeraShard(player, ItemNum);
                if (player.playerData.IsPassiveGetList[15]) { player.ChangeHPW(new Vector2Int(Random.Range(1, 7), 4)); }
                break;
            case 17:
                TeraShard(player, ItemNum);
                if (player.playerData.IsPassiveGetList[15]) { player.ChangeHPW(new Vector2Int(Random.Range(1, 7), 4)); }
                break;

            case 18:
                TeraShard(player, ItemNum);
                if (player.playerData.IsPassiveGetList[15]) { player.ChangeHPW(new Vector2Int(Random.Range(1, 7), 4)); }
                break;
            case 19:
                TeraShard(player, ItemNum);
                if (player.playerData.IsPassiveGetList[15]) { player.ChangeHPW(new Vector2Int(Random.Range(1, 7), 4)); }
                break;
            case 20:
                TeraShard(player, ItemNum);
                if (player.playerData.IsPassiveGetList[15]) { player.ChangeHPW(new Vector2Int(Random.Range(1, 7), 4)); }
                break;


            //ÄÜÁ¦Ç¿»¯
            case 21:
                player.playerData.AtkBounsJustOneRoom = 1 ;
                player.playerData.AtkHardWorkJustOneRoom += player.AtkAbilityPoint * 0.2f;
                player.ReFreshAbllityPoint();
                break;
            case 22:
                player.playerData.DefBounsJustOneRoom = 1;
                player.playerData.DefHardWorkJustOneRoom += player.DefAbilityPoint * 0.2f;
                player.ReFreshAbllityPoint();             
                break;
            case 23:
                player.playerData.SpABounsJustOneRoom = 1;
                player.playerData.SpAHardWorkJustOneRoom += player.SpAAbilityPoint * 0.2f;
                player.ReFreshAbllityPoint();
                break;
            case 24:
                player.playerData.SpDBounsJustOneRoom = 1;
                player.playerData.SpDHardWorkJustOneRoom += player.SpdAbilityPoint * 0.2f;
                player.ReFreshAbllityPoint();
                break;
            case 25:
                player.playerData.SpeBounsJustOneRoom = 1;
                player.playerData.SpeHardWorkJustOneRoom += player.SpeedAbilityPoint * 0.2f;
                player.ReFreshAbllityPoint();
                break;
            case 26:
                player.playerData.LuckBounsJustOneRoom = 1;
                player.playerData.LuckHardWorkJustOneRoom += player.LuckPoint * 0.2f;
                player.ReFreshAbllityPoint();
                break;


            //ÊôÐÔµÖ¿¹Ê÷¹û
            case 27:
                BerryTypeDef(player, ItemNum);
                break;
            case 28:
                BerryTypeDef(player, ItemNum);
                break;
            case 29:
                BerryTypeDef(player, ItemNum);
                break;
            case 30:
                BerryTypeDef(player, ItemNum);
                break;
            case 31:
                BerryTypeDef(player, ItemNum);
                break;

            case 32:
                BerryTypeDef(player, ItemNum);
                break;
            case 33:
                BerryTypeDef(player, ItemNum);
                break;
            case 34:
                BerryTypeDef(player, ItemNum);
                break;
            case 35:
                BerryTypeDef(player, ItemNum);
                break;
            case 36:
                BerryTypeDef(player, ItemNum);
                break;

            case 37:
                BerryTypeDef(player, ItemNum);
                break;
            case 38:
                BerryTypeDef(player, ItemNum);
                break;
            case 39:
                BerryTypeDef(player, ItemNum);
                break;
            case 40:
                BerryTypeDef(player, ItemNum);
                break;
            case 41:
                BerryTypeDef(player, ItemNum);
                break;

            case 42:
                BerryTypeDef(player, ItemNum);
                break;
            case 43:
                BerryTypeDef(player, ItemNum);
                break;
            case 44:
                BerryTypeDef(player, ItemNum);
                break;

            //Òì³£×´Ì¬½â³ýÒ©
            case 45:
                player.ToxicRemove();
                break;
            case 46:
                player.ParalysisRemove();
                break;
            case 47:
                player.BurnRemove();
                break;
            case 48:
                player.SleepRemove();
                break;
            case 49:
                
                break;
            case 50:
                player.ToxicRemove();
                player.ParalysisRemove();
                player.BurnRemove();
                player.SleepRemove();
                break;



            case 51:
                Fly(player);
                break;

            case 52:
                player.TP(MapCreater.StaticMap.PCRoomPoint);
                break;

            case 53:
                player.RefreshSkillCD();
                break;

            case 54:
                ScatterBomb(player);
                break;

            case 55:
                player.TP(MapCreater.StaticMap.StoreRoomPoint);
                break;

            case 56:
                Instantiate(player.spaceItem.GetComponent<SmokeBomb>().Smoke, player.transform.position, Quaternion.identity, player.transform);
                break;




        }

        player.spaceItem = null;
        player.SpaceItemImage.color = new Color(0, 0, 0, 0);
        player.SpaceItemImage.sprite = null;
        if (player.playerData.IsPassiveGetList[28] && spaceItem.ItemTypeTag != null)
        {
            foreach (int i in spaceItem.ItemTypeTag)
            {
                if (i == 1) { player.ChangeHp(Mathf.Clamp(player.maxHp / 16, 1, 10), 0, 19); }
            }
        }
    }

    static void EscapeRope(PlayerControler player)
    {

        player.TP(new Vector3Int(0, 0, 0));
        
        
    }

    static void Repel(PlayerControler player)
    {
        Vector3Int NowRoom = player.NowRoom;
        if (MapCreater.StaticMap.RRoom.ContainsKey(NowRoom))
        {
            Room RepelRoom = MapCreater.StaticMap.RRoom[NowRoom];
            GameObject EmptyList = RepelRoom.transform.GetChild(3).gameObject;
            foreach (Transform empty in EmptyList.transform)
            {
                if (empty.GetComponent<Empty>() != null)
                {
                    empty.GetComponent<Empty>().EmptyHpChange(100, 0, 0);
                }
            }
        }
    }

    static void SitrusBerry(PlayerControler player)
    {
        player.ChangeHp(player.maxHp/3,0,0);
    }

    static void TeraShard(PlayerControler player, int TeraShardType)
    {
        player.PlayerTeraTypeJOR = TeraShardType - 2;
    }

    static void BerryTypeDef(PlayerControler player, int BerryNum)
    {
        int BerryType = BerryNum - 26;
        int TypeDefPoint = player.playerData.TypeDefJustOneRoom[BerryType] + player.playerData.TypeDefAlways[BerryType];
        if (Type.TYPE[BerryType][player.PlayerType01] == 1.2f)
        {
            TypeDefPoint--;
        }
        else if (Type.TYPE[BerryType][player.PlayerType01] == 0.8f)
        {
            TypeDefPoint++;
        }
        else if (Type.TYPE[BerryType][player.PlayerType01] == 0.64f)
        {
            TypeDefPoint++; TypeDefPoint++;
        }
        if (Type.TYPE[BerryType][player.PlayerType02] == 1.2f)
        {
            TypeDefPoint--;
        }
        else if (Type.TYPE[BerryType][player.PlayerType02] == 0.8f)
        {
            TypeDefPoint++;
        }
        else if (Type.TYPE[BerryType][player.PlayerType02] == 0.64f)
        {
            TypeDefPoint++; TypeDefPoint++;
        }


        if (player.PlayerTeraTypeJOR == 0)
        {
            if (Type.TYPE[BerryType][player.PlayerTeraType] == 1.2f)
            {
                TypeDefPoint--;
            }
            else if (Type.TYPE[BerryType][player.PlayerTeraType] == 0.8f)
            {
                TypeDefPoint++;
            }
            else if (Type.TYPE[BerryType][player.PlayerTeraType] == 0.64f)
            {
                TypeDefPoint++; TypeDefPoint++;
            }
        }
        else
        {
            if (Type.TYPE[BerryType][player.PlayerTeraTypeJOR] == 1.2f)
            {
                TypeDefPoint--;
            }
            else if (Type.TYPE[BerryType][player.PlayerTeraTypeJOR] == 0.8f)
            {
                TypeDefPoint++;
            }
            else if (Type.TYPE[BerryType][player.PlayerTeraTypeJOR] == 0.64f)
            {
                TypeDefPoint++; TypeDefPoint++;
            }
        }
        if(BerryType == 1)
        {
            if (TypeDefPoint < 1)
            {
                player.playerData.TypeDefJustOneRoom[BerryType] -= TypeDefPoint-1;
            }
        }
        else
        {
            if (TypeDefPoint < 0)
            {
                player.playerData.TypeDefJustOneRoom[BerryType] -= TypeDefPoint;
            }
        }
    }

    public static void Fly(PlayerControler player)
    {
        player.gameObject.layer = LayerMask.NameToLayer("PlayerFly");
        player.transform.position += new Vector3(0,0.5f,0);
        player.transform.GetChild(0).gameObject.SetActive(false);
        player.transform.GetChild(1).gameObject.SetActive(false);
        player.transform.GetChild(3).gameObject.SetActive(true);
        player.transform.GetChild(4).gameObject.SetActive(true);
        player.ComeInANewRoomEvent += RemoveFly; 
    }
    public static void RemoveFly(PlayerControler player)
    {
        player.gameObject.layer = LayerMask.NameToLayer("Player");
        player.transform.position -= new Vector3(0, 0.5f, 0);
        player.transform.GetChild(0).gameObject.SetActive(true);
        player.transform.GetChild(1).gameObject.SetActive(true);
        player.transform.GetChild(3).gameObject.SetActive(false);
        player.transform.GetChild(4).gameObject.SetActive(false);
        player.ComeInANewRoomEvent -= RemoveFly;
    }

    public static void ScatterBomb(PlayerControler player)
    {
        Vector3Int NowRoom = player.NowRoom;
        if (MapCreater.StaticMap.RRoom.ContainsKey(NowRoom))
        {
            Room ScatterRoom = MapCreater.StaticMap.RRoom[NowRoom];
            GameObject EmptyList = ScatterRoom.transform.GetChild(3).gameObject;
            foreach (Transform empty in EmptyList.transform)
            {
                if (empty.GetComponent<Empty>() != null)
                {
                    empty.GetComponent<Empty>().Fear(25.0f*empty.GetComponent<Empty>().OtherStateResistance , 5);
                }
            }
        }
    }
}
