using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KirliaDGSix : MonoBehaviour
{

    public Kirlia ParentKirlia;

    bool isUp;

    // Start is called before the first frame update
    void Start()
    {
        if (Random.Range(0.0f , 1.0f) > 0.5f)
        {
            isUp = true;
        }
        else
        {
            isUp = false;
        }
        Invoke("SetOtherActive", 1f);
        Invoke("DestorySelf", 12.5f);

        transform.GetChild(0).GetComponent<KirliaDGOne>().empty = ParentKirlia;
        transform.GetChild(1).GetComponent<KirliaDGOne>().empty = ParentKirlia;
        transform.GetChild(2).GetComponent<KirliaDGOne>().empty = ParentKirlia;
        transform.GetChild(3).GetComponent<KirliaDGOne>().empty = ParentKirlia;
        transform.GetChild(4).GetComponent<KirliaDGOne>().empty = ParentKirlia;
        transform.GetChild(5).GetComponent<KirliaDGOne>().empty = ParentKirlia;

        if (ParentKirlia.isEmptyConfusionDone)
        {
            transform.GetChild(0).position = transform.position + new Vector3( 5.19615f , -3 , 0);
            transform.GetChild(1).position = transform.position + new Vector3( -5.19615f , -3 , 0);
            transform.GetChild(2).position = transform.position + new Vector3( 0 , 6 , 0);
            transform.GetChild(3).position = transform.position + new Vector3( 5.19615f , 3 , 0);
            transform.GetChild(4).position = transform.position + new Vector3( -5.19615f , 3 , 0);
            transform.GetChild(5).position = transform.position + new Vector3( 0 , -6 , 0);

        }

        if (isUp)
        {
            transform.GetChild(0).gameObject.SetActive(true);
            transform.GetChild(1).gameObject.SetActive(true);
            transform.GetChild(2).gameObject.SetActive(true);
        }
        else
        {
            transform.GetChild(3).gameObject.SetActive(true);
            transform.GetChild(4).gameObject.SetActive(true);
            transform.GetChild(5).gameObject.SetActive(true);
        }


    }

    void SetOtherActive()
    {
        if (isUp)
        {
            transform.GetChild(3).gameObject.SetActive(true);
            transform.GetChild(4).gameObject.SetActive(true);
            transform.GetChild(5).gameObject.SetActive(true);
        }
        else
        {
            transform.GetChild(0).gameObject.SetActive(true);
            transform.GetChild(1).gameObject.SetActive(true);
            transform.GetChild(2).gameObject.SetActive(true);
        }
    }

    void DestorySelf()
    {
        Destroy(gameObject);
    }

}
