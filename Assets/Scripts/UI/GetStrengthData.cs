using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetStrengthData : MonoBehaviour
{
    public void GetStrengthPoint(int PlayerPoint , float HardWorking ,int Bouns , float AblityPoint )
    {
        transform.GetChild(0).GetComponent<Text>().text = PlayerPoint.ToString();
        transform.GetChild(1).GetComponent<Text>().text = string.Format("{0:0.##}", HardWorking);
        transform.GetChild(2).GetComponent<UIPDPDateBouns>().GetBounsMark(Bouns);
        transform.GetChild(3).GetComponent<Text>().text = ((int)AblityPoint).ToString();
    }
}
