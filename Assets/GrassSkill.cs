using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassSkill : Skill
{
    public SkillBornGress Grass;
    List<Vector3> GrassBornOffsetV =
        new List<Vector3> {
            new Vector3(0,0,0),
            new Vector3(1,0,0),new Vector3(0,1,0),new Vector3(-1,0,0),new Vector3(0,-1,0),
            new Vector3(1,1,0),new Vector3(-1,1,0),new Vector3(-1,-1,0),new Vector3(1,-1,0),
            new Vector3(2,0,0),new Vector3(0,2,0),new Vector3(-2,0,0),new Vector3(0,-2,0),
        };

    protected List<Vector3> AlreadyBornBlockList = new List<Vector3> { };

    public bool isThisPointEmpty(Vector3 P)
    {

        Physics2D.queriesHitTriggers = true;
        Physics2D.queriesStartInColliders = true;
        RaycastHit2D SearchEmpty01 = Physics2D.Raycast(new Vector2(P.x, P.y), Vector2.left + Vector2.up , 0.4f ,LayerMask.GetMask("Enviroment", "Water", "Grass" , "Room" , "SpikeCollidor"));
        RaycastHit2D SearchEmpty02 = Physics2D.Raycast(new Vector2(P.x, P.y), Vector2.left + Vector2.down, 0.4f, LayerMask.GetMask("Enviroment", "Water", "Grass", "Room", "SpikeCollidor"));
        RaycastHit2D SearchEmpty03 = Physics2D.Raycast(new Vector2(P.x, P.y), Vector2.right + Vector2.up, 0.4f, LayerMask.GetMask("Enviroment", "Water", "Grass", "Room", "SpikeCollidor"));
        RaycastHit2D SearchEmpty04 = Physics2D.Raycast(new Vector2(P.x, P.y), Vector2.right + Vector2.down, 0.4f, LayerMask.GetMask("Enviroment", "Water", "Grass", "Room", "SpikeCollidor"));
        RaycastHit2D SearchEmpty05 = Physics2D.Raycast(new Vector2(P.x, P.y), Vector2.left, 0.4f, LayerMask.GetMask("Enviroment", "Water", "Grass", "Room", "SpikeCollidor"));
        RaycastHit2D SearchEmpty06 = Physics2D.Raycast(new Vector2(P.x, P.y), Vector2.down, 0.4f, LayerMask.GetMask("Enviroment", "Water", "Grass", "Room", "SpikeCollidor"));
        RaycastHit2D SearchEmpty07 = Physics2D.Raycast(new Vector2(P.x, P.y), Vector2.right, 0.4f, LayerMask.GetMask("Enviroment", "Water", "Grass", "Room", "SpikeCollidor"));
        RaycastHit2D SearchEmpty08 = Physics2D.Raycast(new Vector2(P.x, P.y), Vector2.down, 0.4f, LayerMask.GetMask("Enviroment", "Water", "Grass", "Room", "SpikeCollidor"));
        Debug.Log((!SearchEmpty01 && !SearchEmpty02 && !SearchEmpty03 && !SearchEmpty04 && !SearchEmpty05 && !SearchEmpty06 && !SearchEmpty07 && !SearchEmpty08) + "+" + P);
        Physics2D.queriesHitTriggers = false;
        Physics2D.queriesStartInColliders = false;
        return !SearchEmpty01 && !SearchEmpty02 && !SearchEmpty03 && !SearchEmpty04 && !SearchEmpty05 && !SearchEmpty06 && !SearchEmpty07 && !SearchEmpty08;
        
    }

    public void BornAGrass(Vector3 BornPosion)
    {
        if (Grass != null) {
            BornPosion = new Vector3((int)BornPosion.x, (int)BornPosion.y, (int)BornPosion.z);
            for (int i = 0; i < GrassBornOffsetV.Count; i++)
            {
                Vector3 CheckPosition = BornPosion + GrassBornOffsetV[i];
                if (isThisPointEmpty(CheckPosition) && !AlreadyBornBlockList.Contains(CheckPosition))
                {
                    Instantiate(Grass, CheckPosition, Quaternion.identity, MapCreater.StaticMap.RRoom[player.NowRoom].transform.GetChild(2));
                    AlreadyBornBlockList.Add(CheckPosition);
                    break;
                }
            }
        }
    }

}
