using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseSpaceItem : MonoBehaviour
{

    public static bool UseSpaceItemConditions(PlayerControler player)
    {
        if (player.spaceItem.GetComponent<SpaceItem>().ItemNum == 66)
        {
            if (player.Hp <= 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return true;
        }
    }


    // Start is called before the first frame update
    public static void UsedSpaceItem(PlayerControler player)
    {
        SpaceItem spaceItem = player.spaceItem.GetComponent<SpaceItem>();
        int ItemNum = spaceItem.ItemNum;
        switch (ItemNum)
        {
            //串洞绳
            case 0:
                EscapeRope(player);
                break;


            //驱虫烟雾
            case 1:
                Repel(player);
                break;


            //文柚果
            case 2:
                SitrusBerry(player);
                break;


            //太晶碎片
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


            //能力强化
            case 21:
                player.playerData.AtkBounsJustOneRoom += 1 ;
                player.playerData.AtkHardWorkJustOneRoom += player.AtkAbilityPoint * 0.2f;
                player.ReFreshAbllityPoint();
                break;
            case 22:
                player.playerData.DefBounsJustOneRoom += 1;
                player.playerData.DefHardWorkJustOneRoom += player.DefAbilityPoint * 0.2f;
                player.ReFreshAbllityPoint();             
                break;
            case 23:
                player.playerData.SpABounsJustOneRoom += 1;
                player.playerData.SpAHardWorkJustOneRoom += player.SpAAbilityPoint * 0.2f;
                player.ReFreshAbllityPoint();
                break;
            case 24:
                player.playerData.SpDBounsJustOneRoom += 1;
                player.playerData.SpDHardWorkJustOneRoom += player.SpdAbilityPoint * 0.2f;
                player.ReFreshAbllityPoint();
                break;
            case 25:
                player.playerData.SpeBounsJustOneRoom += 1;
                player.playerData.SpeHardWorkJustOneRoom += player.SpeedAbilityPoint * 0.2f;
                player.ReFreshAbllityPoint();
                break;
            case 26:
                player.playerData.LuckBounsJustOneRoom += 1;
                player.playerData.LuckHardWorkJustOneRoom += player.LuckPoint * 0.2f;
                player.ReFreshAbllityPoint();
                break;


            //属性抵抗树果
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

            //异常状态解除药
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
                player.PlayerFrozenRemove();
                break;
            case 50:
                player.PlayerFrozenRemove();
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



            //神奇黏糕
            case 57:
                switch (Random.Range(0,7))
                {
                    case 0:
                        UIGetANewItem.UI.JustSaySth("是体力粘糕的味道!", "体力的奋斗力上升了!");
                        player.ChangeHPW(new Vector2Int(1,12));
                        break;
                    case 1:
                        UIGetANewItem.UI.JustSaySth("是肌力粘糕的味道!", "攻击的奋斗力上升了!");
                        player.ChangeHPW(new Vector2Int(2, 12));
                        break;
                    case 2:
                        UIGetANewItem.UI.JustSaySth("是抵抗粘糕的味道!", "防御的奋斗力上升了!");
                        player.ChangeHPW(new Vector2Int(3, 12));
                        break;
                    case 3:
                        UIGetANewItem.UI.JustSaySth("是智力粘糕的味道!", "特攻的奋斗力上升了!");
                        player.ChangeHPW(new Vector2Int(4, 12));
                        break;
                    case 4:
                        UIGetANewItem.UI.JustSaySth("是精神粘糕的味道!", "特防的奋斗力上升了!");
                        player.ChangeHPW(new Vector2Int(5, 12));
                        break;
                    case 5:
                        UIGetANewItem.UI.JustSaySth("是瞬发粘糕的味道!", "攻击速度的奋斗力上升了!");
                        player.ChangeHPW(new Vector2Int(6, 12));
                        break;
                    case 6:
                        UIGetANewItem.UI.JustSaySth("是净空粘糕的味道!", "所有属性的奋斗力都下降了!");
                        player.ChangeHPW(new Vector2Int(1, -6));
                        player.ChangeHPW(new Vector2Int(2, -6));
                        player.ChangeHPW(new Vector2Int(3, -6));
                        player.ChangeHPW(new Vector2Int(4, -6));
                        player.ChangeHPW(new Vector2Int(5, -6));
                        player.ChangeHPW(new Vector2Int(6, -6));
                        break;
                }
                break;

            //彗星碎片
            case 58:
                Instantiate(player.spaceItem.GetComponent<CometShard>().Start, player.transform.position, Quaternion.identity).GetComponent<RandomStarMoney>().isLunch = true;
                Instantiate(player.spaceItem.GetComponent<CometShard>().Start, player.transform.position, Quaternion.identity).GetComponent<RandomStarMoney>().isLunch = true;
                Instantiate(player.spaceItem.GetComponent<CometShard>().Start, player.transform.position, Quaternion.identity).GetComponent<RandomStarMoney>().isLunch = true;
                Instantiate(player.spaceItem.GetComponent<CometShard>().Start, player.transform.position, Quaternion.identity).GetComponent<RandomStarMoney>().isLunch = true;
                Instantiate(player.spaceItem.GetComponent<CometShard>().Start, player.transform.position, Quaternion.identity).GetComponent<RandomStarMoney>().isLunch = true;
                if (Random.Range(0.0f,1.0f) > 0.6f)
                {
                    Instantiate(player.spaceItem.GetComponent<CometShard>().Start, player.transform.position, Quaternion.identity).GetComponent<RandomStarMoney>().isLunch = true;
                    if (Random.Range(0.0f, 1.0f) > 0.6f)
                    {
                        Instantiate(player.spaceItem.GetComponent<CometShard>().Start, player.transform.position, Quaternion.identity).GetComponent<RandomStarMoney>().isLunch = true;
                    }
                }
                break;

            //潮湿岩石
            case 59:
                Weather.GlobalWeather.ChangeWeatherRain(15,false);
                break;

            //沙沙岩石
            case 60:
                Weather.GlobalWeather.ChangeWeatherSandStorm(15, false);
                break;

            //炽热岩石
            case 61:
                Weather.GlobalWeather.ChangeWeatherSunshine(15, false);
                break;

            //冰冷岩石
            case 62:
                Weather.GlobalWeather.ChangeWeatherHail(15, false);
                break;

            //头领凭证
            case 63:
                Instantiate(player.spaceItem.GetComponent<LeadersCrest>().Atrack, player.transform.position, Quaternion.identity, player.transform);
                break;

            //皮皮玩偶
            case 64:
                Substitute Obj = Instantiate(player.spaceItem.GetComponent<PokeDoll>().substitute, player.transform.position, Quaternion.Euler(Vector3.zero));
                Obj.SetSubstitute(player.maxHp / 2, player);
                break;

            //怪兽笛
            case 65:
                player.SleepRemove();
                Room NowRoom =  MapCreater.StaticMap.RRoom[player.NowRoom];
                if (NowRoom.RoomTag == 0)
                {
                    for (int i = 0; i < NowRoom.transform.GetChild(3).childCount; i++ )
                    {
                        Empty e = NowRoom.transform.GetChild(3).GetChild(i).GetComponent<Empty>();
                        if (e != null)
                        {
                            e.EmptySleepRemove();
                        }
                    }
                }
                else if(NowRoom.RoomTag == 1)
                {
                    Blissey b = NowRoom.transform.GetChild(7).GetComponent<Blissey>();
                    if (b != null)
                    {
                        b.BlisseyAwake();
                    }
                }
                break;

            //复活草
            case 66:
                if (player.Hp <= 1) {
                    Pokemon.PokemonHpChange(null, player.gameObject, 0, 0, player.maxHp, Type.TypeEnum.IgnoreType);
                    switch (Random.Range(0, 6))
                    {
                        case 0:
                            UIGetANewItem.UI.JustSaySth("好苦!", "体力的奋斗力被苦到下降了");
                            player.ChangeHPW(new Vector2Int(1, -20));
                            break;
                        case 1:
                            UIGetANewItem.UI.JustSaySth("好苦!", "攻击的奋斗力被苦到下降了");
                            player.ChangeHPW(new Vector2Int(2, -20));
                            break;
                        case 2:
                            UIGetANewItem.UI.JustSaySth("好苦!", "防御的奋斗力被苦到下降了");
                            player.ChangeHPW(new Vector2Int(3, -20));
                            break;
                        case 3:
                            UIGetANewItem.UI.JustSaySth("好苦!", "特攻的奋斗力被苦到下降了");
                            player.ChangeHPW(new Vector2Int(4, -20));
                            break;
                        case 4:
                            UIGetANewItem.UI.JustSaySth("好苦!", "特防的奋斗力被苦到下降了");
                            player.ChangeHPW(new Vector2Int(5, -20));
                            break;
                        case 5:
                            UIGetANewItem.UI.JustSaySth("好苦!", "攻击速度的奋斗力被苦到下降了");
                            player.ChangeHPW(new Vector2Int(6, -20));
                            break;
                    }    
                }
                break;

            //电气种子
            case 67:
                Instantiate(PassiveItemGameObjList.ObjList.List[27], player.transform.position, Quaternion.identity);
                break;

            //青草种子
            case 68:
                Instantiate(PassiveItemGameObjList.ObjList.List[28], player.transform.position, Quaternion.identity);
                break;

            //迷雾种子
            case 69:
                Instantiate(PassiveItemGameObjList.ObjList.List[29], player.transform.position, Quaternion.identity);
                break;

            //精神种子
            case 70:
                Instantiate(PassiveItemGameObjList.ObjList.List[30], player.transform.position, Quaternion.identity);
                break;

        }

        player.spaceItem = null;
        player.SpaceItemImage.color = new Color(0, 0, 0, 0);
        player.SpaceItemImage.sprite = null;
        if (player.playerData.IsPassiveGetList[28] && spaceItem.ItemTypeTag != null)
        {
            foreach (int i in spaceItem.ItemTypeTag)
            {
                if (i == 1) { 
                    Pokemon.PokemonHpChange(null, player.gameObject, 0, 0, Mathf.Clamp(player.maxHp / 16, 1, 10), Type.TypeEnum.IgnoreType);
                }
            }
        }
        if (player.playerData.IsPassiveGetList[100] && spaceItem.ItemTypeTag != null)
        {
            foreach (int i in spaceItem.ItemTypeTag)
            {
                if (i == 1) { player.ChangeHPW(new Vector2Int(Random.Range(1, 7), 2)); }
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
                    Pokemon.PokemonHpChange(null , empty.GetComponent<Empty>().gameObject , 100 , 0 , 0 , Type.TypeEnum.IgnoreType);
                }
            }
        }
    }

    static void SitrusBerry(PlayerControler player)
    {
        Pokemon.PokemonHpChange(null, player.gameObject, 0, 0, player.maxHp / 3, Type.TypeEnum.IgnoreType);
    }

    static void TeraShard(PlayerControler player, int TeraShardType)
    {
        player.TeraTypeJORChange(TeraShardType - 2);
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
        if (player.gameObject.layer != LayerMask.NameToLayer("PlayerFly"))
        {
            player.gameObject.layer = LayerMask.NameToLayer("PlayerFly");
            player.transform.position += new Vector3(0, 0.5f, 0);
            player.transform.GetChild(3).position = player.transform.GetChild(3).position + Vector3.up * 0.5f;
            player.PlayerLocalPosition = player.transform.GetChild(3).localPosition;
            player.ComeInANewRoomEvent += RemoveFly;
        }
    }
    public static void RemoveFly(PlayerControler player)
    {
        if (player.gameObject.layer == LayerMask.NameToLayer("PlayerFly")) {
            player.gameObject.layer = LayerMask.NameToLayer("Player");
            player.transform.position -= new Vector3(0, 0.5f, 0);
            player.transform.GetChild(3).position = player.transform.GetChild(3).position - Vector3.up * 0.5f;
            player.PlayerLocalPosition = player.transform.GetChild(3).localPosition;
            player.ComeInANewRoomEvent -= RemoveFly;
        }
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
