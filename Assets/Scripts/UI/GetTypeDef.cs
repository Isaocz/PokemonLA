using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetTypeDef : MonoBehaviour
{
    // Start is called before the first frame update
    public void GetTypeDefData( int[] TypeDefData )
    {
        int i;
        for (i=0; i < 18; i++)
        {
            gameObject.transform.GetChild(i).GetChild(1).GetComponent<Text>().text = TypeDefData[i].ToString() + "¼¶";
        }
    }
}
