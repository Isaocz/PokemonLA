using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuryAttack : Skill
{
    FuryAttackChild f01;
    FuryAttackChild f02;
    FuryAttackChild f03;
    FuryAttackChild f04;
    FuryAttackChild f05;

    // Start is called before the first frame update
    void Start()
    {
        f01 = transform.GetChild(0).GetComponent < FuryAttackChild>();
        f02 = transform.GetChild(1).GetComponent < FuryAttackChild>();
        f03 = transform.GetChild(2).GetComponent < FuryAttackChild>();
        f04 = transform.GetChild(3).GetComponent < FuryAttackChild>();
        f05 = transform.GetChild(4).GetComponent < FuryAttackChild>();
        int count = Count2_5();

        float d = Mathf.Max(player.SkillOffsetforBodySize[1], player.SkillOffsetforBodySize[2]);
        float r = transform.rotation.eulerAngles.z;
        transform.rotation = Quaternion.Euler(0, 0, 0);

        f01.transform.rotation = Quaternion.Euler(0,0,r);
        f01.transform.localPosition = (Quaternion.AngleAxis(f01.transform.rotation.eulerAngles.z, Vector3.forward)) * Vector2.right * (d + 0.75f);
        f01.gameObject.SetActive(true);

        int r02 = Random.Range(0,360);
        f02.transform.rotation = Quaternion.Euler(0, 0, r02);
        f02.transform.localPosition = (Quaternion.AngleAxis(r02, Vector3.forward)) * Vector2.right * (d + 0.75f);

        int r03 = Random.Range(0, 360);
        f03.transform.rotation = Quaternion.Euler(0, 0, r03);
        f03.transform.localPosition = (Quaternion.AngleAxis(r03, Vector3.forward)) * Vector2.right * (d + 0.75f);

        int r04 = Random.Range(0, 360);
        f04.transform.rotation = Quaternion.Euler(0, 0, r04);
        f04.transform.localPosition = (Quaternion.AngleAxis(r04, Vector3.forward)) * Vector2.right * (d + 0.75f);

        int r05 = Random.Range(0, 360);
        f05.transform.rotation = Quaternion.Euler(0, 0, r05);
        f05.transform.localPosition = (Quaternion.AngleAxis(r05, Vector3.forward)) * Vector2.right * (d + 0.75f);

        switch (count)
        {
            case 2:
                f02.gameObject.SetActive(true);
                break;
            case 3:
                f02.gameObject.SetActive(true);
                f03.gameObject.SetActive(true);
                break;
            case 4:
                f02.gameObject.SetActive(true);
                f03.gameObject.SetActive(true);
                f04.gameObject.SetActive(true);
                break;
            case 5:
                f02.gameObject.SetActive(true);
                f03.gameObject.SetActive(true);
                f04.gameObject.SetActive(true);
                f05.gameObject.SetActive(true);
                break;
        }

    }

    // Update is called once per frame
    void Update()
    {
        StartExistenceTimer();
    }
}
