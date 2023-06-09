using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPDPDateBouns : MonoBehaviour
{
    public Image PlusBouns;
    public Image MinusBouns;


    // Start is called before the first frame update
    void Start()
    {
        GetBounsMark(0);
    }

    public void GetBounsMark( int Bouns )
    {
        int i;
        for (i = 0; i < transform.childCount; i++)
        {
            GameObject child = transform.GetChild(i).gameObject;
            Destroy(child);
        }
        if (Bouns > 0) {
            for (i=0;i<Bouns;i++)
            {
                Instantiate(PlusBouns , Vector3.zero , Quaternion.identity , gameObject.transform);
            }
        }else if ( Bouns < 0)
        {
            for (i = 0; i > Bouns; i--)
            {
                Instantiate(MinusBouns, Vector3.zero, Quaternion.identity, gameObject.transform);
            }
        }
    }

}
