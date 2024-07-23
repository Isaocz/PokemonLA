using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PsychicLunatoneManger : MonoBehaviour
{
    public Empty ParentEmpty;
    PsychicLunatone PL01;
    PsychicLunatone PL02;
    PsychicLunatone PL03;
    PsychicLunatone PL04;
    PsychicLunatone PL05;
    PsychicLunatone PL06;
    PsychicLunatone PL07;
    PsychicLunatone PL08;


    // Start is called before the first frame update
    void Start()
    {
        int StartCount = Random.Range(0,8);

        PL01 = transform.GetChild((0 + StartCount) % 8).GetComponent<PsychicLunatone>();
        PL02 = transform.GetChild((1 + StartCount) % 8).GetComponent<PsychicLunatone>();
        PL03 = transform.GetChild((2 + StartCount) % 8).GetComponent<PsychicLunatone>();
        PL04 = transform.GetChild((3 + StartCount) % 8).GetComponent<PsychicLunatone>();
        PL05 = transform.GetChild((4 + StartCount) % 8).GetComponent<PsychicLunatone>();
        PL06 = transform.GetChild((5 + StartCount) % 8).GetComponent<PsychicLunatone>();
        PL07 = transform.GetChild((6 + StartCount) % 8).GetComponent<PsychicLunatone>();
        PL08 = transform.GetChild((7 + StartCount) % 8).GetComponent<PsychicLunatone>();
        PL01.empty = ParentEmpty;
        PL02.empty = ParentEmpty;
        PL03.empty = ParentEmpty;
        PL04.empty = ParentEmpty;
        PL05.empty = ParentEmpty;
        PL06.empty = ParentEmpty;
        PL07.empty = ParentEmpty;
        PL08.empty = ParentEmpty;
        PL01.gameObject.SetActive(true);
        if (ParentEmpty.isEmptyConfusionDone)
        {

            Timer.Start(this, 0.6f, () => { PL02.gameObject.SetActive(true); });
            Timer.Start(this, 1.2f, () => { PL03.gameObject.SetActive(true); });
            Timer.Start(this, 1.8f, () => { PL04.gameObject.SetActive(true); });
            Timer.Start(this, 2.4f, () => { PL05.gameObject.SetActive(true); });
            Timer.Start(this, 3.0f, () => { PL06.gameObject.SetActive(true); });
            Timer.Start(this, 3.6f, () => { PL07.gameObject.SetActive(true); });
        }
        else
        {

            Timer.Start(this, 0.32f, () => { PL02.gameObject.SetActive(true); });
            Timer.Start(this, 0.64f, () => { PL03.gameObject.SetActive(true); });
            Timer.Start(this, 0.96f, () => { PL04.gameObject.SetActive(true); });
            Timer.Start(this, 1.28f, () => { PL05.gameObject.SetActive(true); });
            Timer.Start(this, 1.60f, () => { PL06.gameObject.SetActive(true); });
            Timer.Start(this, 1.92f, () => { PL07.gameObject.SetActive(true); });
        }

        Timer.Start(this, 7.0f, () => { Destroy(gameObject); });

    }

}
