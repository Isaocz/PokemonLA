using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerButterflyManger : MonoBehaviour
{

    float Timer;
    List<FairyButterfly> TurnBFList = new List<FairyButterfly> { };
    int BFNum;
    public PlayerControler player;
    public FairyButterfly BF;



    void Start()
    {
        player = transform.parent.parent.GetComponent<PlayerControler>();
    }

    // Update is called once per frame
    void Update()
    {
        Timer += 60 * Time.deltaTime;
        if (Timer > 360) { Timer -= 360;}
        BFNum = 0;
        TurnBFList.Clear();
        for (int i = 0; i < transform.childCount; i++)
        {
            FairyButterfly BF = transform.GetChild(i).GetComponent<FairyButterfly>();
            if (BF != null ) {
                if (!BF.isAttack) {
                    BFNum++;
                    TurnBFList.Add(BF);
                }
                else
                {
                    BF.transform.parent = player.transform.parent;
                }
            }
        }
        float BetweenRotation1 = 360.0f / (float)BFNum;

        




        for (int i = 0; i < BFNum; i++)
        {
            
            float z = _mTool.Angle_360Y(TurnBFList[(i == (BFNum - 1) ? 0 : i + 1)].transform.localPosition, Vector3.right) - _mTool.Angle_360Y(TurnBFList[i].transform.localPosition, Vector3.right);
            float MagnitudeOffset = 1;
            if (TurnBFList[i].transform.localPosition.magnitude > 1) { MagnitudeOffset = 0.85f; }
            else if (TurnBFList[i].transform.localPosition.magnitude < 1) { MagnitudeOffset = 1.15f; }
            if (TurnBFList[i].transform.localPosition.magnitude - 1 < 0.2) { MagnitudeOffset = 1; }

            if (MagnitudeOffset == 1)
            {
                TurnBFList[i].transform.localPosition = Quaternion.AngleAxis(Time.deltaTime * 60 * ((z > BetweenRotation1) ? 1.3f : 1) * ((z < BetweenRotation1) ? 0.7f : 1), Vector3.forward) * TurnBFList[i].transform.localPosition.normalized;
            }
            else
            {
                TurnBFList[i].transform.localPosition = Quaternion.AngleAxis(Time.deltaTime * 60 * ((z > BetweenRotation1) ? 1.3f : 1) * ((z < BetweenRotation1) ? 0.7f : 1), Vector3.forward) * TurnBFList[i].transform.localPosition * MagnitudeOffset;
            }


        }

    }


    public void BornABF(FairyButterfly.ButterflyType t)
    {
        Instantiate(BF , transform.position + (Quaternion.AngleAxis( (transform.childCount == 0)? 0 : ( 360/(transform.childCount+1) ) , Vector3.forward) * Vector3.right ) * 0.3f , Quaternion.identity , transform).BFType = t;
        if (player.isInSuperMistyTerrain)
        {
            Instantiate(BF, transform.position + Vector3.right * 0.3f, Quaternion.identity, transform).BFType = t;
        }
    }


}
