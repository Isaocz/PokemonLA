using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnixStoneEdgeManger : MonoBehaviour
{
    public Empty empty;

    OnixStoneEdge edge01;
    OnixStoneEdge edge02;
    OnixStoneEdge edge03;
    OnixStoneEdge edge04;
    OnixStoneEdge edge05;
    OnixStoneEdge edge06;
    OnixStoneEdge edge07;
    OnixStoneEdge edge08;

    OnixStoneEdge edge09;
    OnixStoneEdge edge10;
    OnixStoneEdge edge11;
    OnixStoneEdge edge12;
    OnixStoneEdge edge13;
    OnixStoneEdge edge14;
    OnixStoneEdge edge15;
    OnixStoneEdge edge16;

    // Start is called before the first frame update
    void Start()
    {
        edge01 = transform.GetChild(0).GetComponent<OnixStoneEdge>();
        edge02 = transform.GetChild(1).GetComponent<OnixStoneEdge>();
        edge03 = transform.GetChild(2).GetComponent<OnixStoneEdge>();
        edge04 = transform.GetChild(3).GetComponent<OnixStoneEdge>();
        edge05 = transform.GetChild(4).GetComponent<OnixStoneEdge>();
        edge06 = transform.GetChild(5).GetComponent<OnixStoneEdge>();
        edge07 = transform.GetChild(6).GetComponent<OnixStoneEdge>();
        edge08 = transform.GetChild(7).GetComponent<OnixStoneEdge>();
        edge09 = transform.GetChild(8).GetComponent<OnixStoneEdge>();
        edge10 = transform.GetChild(9).GetComponent<OnixStoneEdge>();
        edge11 = transform.GetChild(10).GetComponent<OnixStoneEdge>();
        edge12 = transform.GetChild(11).GetComponent<OnixStoneEdge>();
        edge13 = transform.GetChild(12).GetComponent<OnixStoneEdge>();
        edge14 = transform.GetChild(13).GetComponent<OnixStoneEdge>();
        edge15 = transform.GetChild(14).GetComponent<OnixStoneEdge>();
        edge16 = transform.GetChild(15).GetComponent<OnixStoneEdge>();

        edge01.empty = empty; edge01.LaunchNotForce( new Vector2 ( 0 , 1 ) , 12.5f);
        edge02.empty = empty; edge02.LaunchNotForce( new Vector2 ( 0 , -1 ) , 12.5f);
        edge03.empty = empty; edge03.LaunchNotForce( new Vector2 ( 1f , 0 ) , 12.5f);
        edge04.empty = empty; edge04.LaunchNotForce( new Vector2 ( -1 , 0 ) , 12.5f);
        edge05.empty = empty; edge05.LaunchNotForce( new Vector2 ( 1.0f , 1.0f ).normalized , 6.5f);
        edge06.empty = empty; edge06.LaunchNotForce( new Vector2 ( 1.0f , -1.0f ).normalized , 6.5f);
        edge07.empty = empty; edge07.LaunchNotForce( new Vector2 ( -1.0f , 1.0f ).normalized , 6.5f);
        edge08.empty = empty; edge08.LaunchNotForce( new Vector2 ( -1.0f , -1.0f ).normalized , 6.5f);

        if (!empty.isEmptyConfusionDone) {
            edge09.empty = empty; edge09.LaunchNotForce(new Vector2(-0.5f, 0.8660254f).normalized, 12.5f);
            edge10.empty = empty; edge10.LaunchNotForce(new Vector2(-0.8660254f, 0.5f).normalized, 12.5f);
            edge11.empty = empty; edge11.LaunchNotForce(new Vector2(-0.8660254f, -0.5f).normalized, 12.5f);
            edge12.empty = empty; edge12.LaunchNotForce(new Vector2(-0.5f, -0.8660254f).normalized, 12.5f);
            edge13.empty = empty; edge13.LaunchNotForce(new Vector2(0.5f, -0.8660254f).normalized, 12.5f);
            edge14.empty = empty; edge14.LaunchNotForce(new Vector2(0.8660254f, -0.5f).normalized, 12.5f);
            edge15.empty = empty; edge15.LaunchNotForce(new Vector2(0.8660254f, 0.5f).normalized, 12.5f);
            edge16.empty = empty; edge16.LaunchNotForce(new Vector2(0.5f, 0.8660254f).normalized, 12.5f);

        }
        else
        {
            edge09.gameObject.SetActive(false);
            edge10.gameObject.SetActive(false);
            edge11.gameObject.SetActive(false);
            edge12.gameObject.SetActive(false);
            edge13.gameObject.SetActive(false);
            edge14.gameObject.SetActive(false);
            edge15.gameObject.SetActive(false);
            edge16.gameObject.SetActive(false);
        }
        Timer.Start( this , 5.0f , ()=> { Destroy(gameObject); } );

    }

}
