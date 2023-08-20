using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloysterAuroraBeamManger : MonoBehaviour
{
    public Cloyster ParentCloyster;

    public CloysterAuroraBeamSmall SBeam01;
    public CloysterAuroraBeamSmall SBeam02;
    public CloysterAuroraBeamSmall SBeam03;
    public CloysterAuroraBeamSmall SBeam04;
    public CloysterAuroraBeamSmall SBeam05;
    public CloysterAuroraBeamSmall SBeam06;
    public CloysterAuroraBeamSmall SBeam07;

    public CloysterAuroraBeamLarge LBeam;
    bool isLargeBeam;

    // Start is called before the first frame update
    void Start()
    {
        SBeam01 = transform.GetChild(0).GetComponent<CloysterAuroraBeamSmall>();
        SBeam02 = transform.GetChild(1).GetComponent<CloysterAuroraBeamSmall>();
        SBeam03 = transform.GetChild(2).GetComponent<CloysterAuroraBeamSmall>();
        SBeam04 = transform.GetChild(3).GetComponent<CloysterAuroraBeamSmall>();
        SBeam05 = transform.GetChild(4).GetComponent<CloysterAuroraBeamSmall>();
        SBeam06 = transform.GetChild(5).GetComponent<CloysterAuroraBeamSmall>();
        SBeam07 = transform.GetChild(6).GetComponent<CloysterAuroraBeamSmall>();
        LBeam = transform.GetChild(7).GetComponent<CloysterAuroraBeamLarge>();

        SBeam01.ParentCloyster = ParentCloyster;
        SBeam02.ParentCloyster = ParentCloyster;
        SBeam03.ParentCloyster = ParentCloyster;
        SBeam04.ParentCloyster = ParentCloyster;
        SBeam05.ParentCloyster = ParentCloyster;
        SBeam06.ParentCloyster = ParentCloyster;
        SBeam07.ParentCloyster = ParentCloyster;
        LBeam.ParentCloyster = ParentCloyster;

        SBeam01.Rotation = 90.0f - 360.0f / 7.0f;
        SBeam02.Rotation = 90.0f;
        SBeam03.Rotation = 90.0f + 360.0f / 7.0f;
        SBeam04.Rotation = 90.0f + (360.0f / 7.0f) * 2.0f;
        SBeam05.Rotation = 90.0f + (360.0f / 7.0f) * 3.0f;
        SBeam06.Rotation = 90.0f + (360.0f / 7.0f) * 4.0f;
        SBeam07.Rotation = 90.0f + (360.0f / 7.0f) * 5.0f;
        LBeam.Rotation = 0;

    }

    // Update is called once per frame
    void Update()
    {
        if (ParentCloyster.isFearDone)
        {
            Destroy(gameObject);
            ParentCloyster.BeamClose();
        }
        if (!ParentCloyster.isEmptyFrozenDone && !ParentCloyster.isSleepDone && !ParentCloyster.isCanNotMoveWhenParalysis && !ParentCloyster.isSilence)
        {
            if (Mathf.Abs(SBeam01.transform.rotation.eulerAngles.z - SBeam02.transform.rotation.eulerAngles.z) < 1
            && Mathf.Abs(SBeam01.transform.rotation.eulerAngles.z - SBeam03.transform.rotation.eulerAngles.z) < 1
            && Mathf.Abs(SBeam01.transform.rotation.eulerAngles.z - SBeam04.transform.rotation.eulerAngles.z) < 1
            && Mathf.Abs(SBeam01.transform.rotation.eulerAngles.z - SBeam05.transform.rotation.eulerAngles.z) < 1
            && Mathf.Abs(SBeam01.transform.rotation.eulerAngles.z - SBeam06.transform.rotation.eulerAngles.z) < 1
            && Mathf.Abs(SBeam01.transform.rotation.eulerAngles.z - SBeam07.transform.rotation.eulerAngles.z) < 1
            && !isLargeBeam)
            {
                isLargeBeam = true;
                LBeam.gameObject.SetActive(true);
                LBeam.transform.rotation = Quaternion.Euler(new Vector3(0, 0, SBeam01.transform.rotation.eulerAngles.z));
                SBeam01.gameObject.SetActive(false);
                SBeam02.gameObject.SetActive(false);
                SBeam03.gameObject.SetActive(false);
                SBeam04.gameObject.SetActive(false);
                SBeam05.gameObject.SetActive(false);
                SBeam06.gameObject.SetActive(false);
                SBeam07.gameObject.SetActive(false);
            }
        }
    }
}
