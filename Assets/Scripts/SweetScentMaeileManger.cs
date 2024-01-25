using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SweetScentMaeileManger : MonoBehaviour
{

    SweetScentMawile SS01;
    SweetScentMawile SS02;
    SweetScentMawile SS03;
    SweetScentMawile SS04;
    SweetScentMawile SS05;
    SweetScentMawile SS06;
    SweetScentMawile SS07;
    SweetScentMawile SS08;
    public Empty ParentEmpty;
    float Speed;

    // Start is called before the first frame update
    void Start()
    {
        Timer.Start(this , 6.0f , ()=> { if (gameObject != null) { Destroy(gameObject); } });
        SS01 = transform.GetChild(0).GetComponent<SweetScentMawile>();
        SS02 = transform.GetChild(1).GetComponent<SweetScentMawile>();
        SS03 = transform.GetChild(2).GetComponent<SweetScentMawile>();
        SS04 = transform.GetChild(3).GetComponent<SweetScentMawile>();
        SS05 = transform.GetChild(4).GetComponent<SweetScentMawile>();
        SS06 = transform.GetChild(5).GetComponent<SweetScentMawile>();
        SS07 = transform.GetChild(6).GetComponent<SweetScentMawile>();
        SS08 = transform.GetChild(7).GetComponent<SweetScentMawile>();
        SS01.ParentEmpty = ParentEmpty;
        SS02.ParentEmpty = ParentEmpty;
        SS03.ParentEmpty = ParentEmpty;
        SS04.ParentEmpty = ParentEmpty;
        SS05.ParentEmpty = ParentEmpty;
        SS06.ParentEmpty = ParentEmpty;
        SS07.ParentEmpty = ParentEmpty;
        SS08.ParentEmpty = ParentEmpty;
        Speed = 8.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Speed >= 0.0f)
        {
            Speed -= 8.0f * Time.deltaTime;
            SS01.transform.position += Vector3.right * Speed * Time.deltaTime;
            SS02.transform.position += (new Vector3(1, 1, 0).normalized) * Speed * Time.deltaTime;
            SS03.transform.position += Vector3.up * Speed * Time.deltaTime;
            SS04.transform.position += (new Vector3(-1, 1, 0).normalized) * Speed * Time.deltaTime;
            SS05.transform.position += Vector3.left * Speed * Time.deltaTime;
            SS06.transform.position += (new Vector3(-1, -1, 0).normalized) * Speed * Time.deltaTime;
            SS07.transform.position += Vector3.down * Speed * Time.deltaTime;
            SS08.transform.position += (new Vector3(1, -1, 0).normalized) * Speed * Time.deltaTime;
        }
    }
}
