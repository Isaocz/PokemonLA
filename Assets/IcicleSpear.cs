using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IcicleSpear : Skill
{
    int Count;

    IcicleSpearIce Ice1;
    IcicleSpearIce Ice2;
    IcicleSpearIce Ice3;
    IcicleSpearIce Ice4;
    IcicleSpearIce Ice5;

    Vector2 direction1;
    Vector2 direction2;
    Vector2 direction3;
    Vector2 direction4;
    Vector2 direction5;

    Vector3 StartPosition1;
    Vector3 StartPosition2;
    Vector3 StartPosition3;
    Vector3 StartPosition4;
    Vector3 StartPosition5;

    // Start is called before the first frame update
    void Start()
    {
        Ice1 = transform.GetChild(0).GetComponent<IcicleSpearIce>();
        Ice2 = transform.GetChild(1).GetComponent<IcicleSpearIce>();
        Ice3 = transform.GetChild(2).GetComponent<IcicleSpearIce>();
        Ice4 = transform.GetChild(3).GetComponent<IcicleSpearIce>();
        Ice5 = transform.GetChild(4).GetComponent<IcicleSpearIce>();

        Count = Count2_5();
        switch (Count)
        {
            case 2:
                direction1 = (transform.rotation * Vector2.right).normalized;
                direction2 = (transform.rotation * Vector2.right).normalized;
                StartPosition1 = new Vector3(transform.position.x - direction1.y * 0.2f, transform.position.y + direction1.x * 0.2f, transform.position.z);
                StartPosition2 = new Vector3(transform.position.x + direction1.y * 0.2f, transform.position.y - direction1.x * 0.2f, transform.position.z);
                Ice1.player = player;
                Ice1.StartPostion = StartPosition1;
                Ice1.direction = direction1;
                Ice1.gameObject.SetActive(true);
                Ice2.player = player;
                Ice2.StartPostion = StartPosition2;
                Ice2.direction = direction2;
                Ice2.gameObject.SetActive(true);
                break;
            case 3:
                direction1 = (transform.rotation * Vector2.right).normalized;
                direction2 = Quaternion.AngleAxis(5, Vector3.forward) * direction1;
                direction3 = Quaternion.AngleAxis(-5, Vector3.forward) * direction1;
                Ice2.transform.localRotation = Quaternion.AngleAxis(5, Vector3.forward) * Ice2.transform.localRotation;
                Ice3.transform.localRotation = Quaternion.AngleAxis(-5, Vector3.forward) * Ice3.transform.localRotation;

                StartPosition1 = new Vector3(transform.position.x, transform.position.y, transform.position.z);
                StartPosition2 = new Vector3(transform.position.x - direction1.y * 0.18f, transform.position.y + direction1.x * 0.18f, transform.position.z);
                StartPosition3 = new Vector3(transform.position.x + direction1.y * 0.18f, transform.position.y - direction1.x * 0.18f, transform.position.z);
                Ice1.player = player;
                Ice1.StartPostion = StartPosition1;
                Ice1.direction = direction1;
                Ice1.gameObject.SetActive(true);
                Ice2.player = player;
                Ice2.StartPostion = StartPosition2;
                Ice2.direction = direction2;
                Ice2.gameObject.SetActive(true);
                Ice3.player = player;
                Ice3.StartPostion = StartPosition3;
                Ice3.direction = direction3;
                Ice3.gameObject.SetActive(true);
                break;
            case 4:
                direction1 = (transform.rotation * Vector2.right).normalized;
                direction2 = Quaternion.AngleAxis(-1.67f, Vector3.forward) * direction1;
                direction3 = Quaternion.AngleAxis(5, Vector3.forward) * direction1;
                direction4 = Quaternion.AngleAxis(-5, Vector3.forward) * direction1;
                direction1 = Quaternion.AngleAxis(1.67f, Vector3.forward) * direction1;
                Ice1.transform.localRotation = Quaternion.AngleAxis(1.67f, Vector3.forward) * Ice1.transform.localRotation;
                Ice2.transform.localRotation = Quaternion.AngleAxis(-1.67f, Vector3.forward) * Ice2.transform.localRotation;
                Ice3.transform.localRotation = Quaternion.AngleAxis(5, Vector3.forward) * Ice3.transform.localRotation;
                Ice4.transform.localRotation = Quaternion.AngleAxis(-5, Vector3.forward) * Ice4.transform.localRotation;

                StartPosition1 = new Vector3(transform.position.x - direction1.y * 0.1f, transform.position.y + direction1.x * 0.1f, transform.position.z);
                StartPosition2 = new Vector3(transform.position.x + direction1.y * 0.1f, transform.position.y - direction1.x * 0.1f, transform.position.z);
                StartPosition3 = new Vector3(transform.position.x - direction1.y * 0.3f, transform.position.y + direction1.x * 0.3f, transform.position.z);
                StartPosition4 = new Vector3(transform.position.x + direction1.y * 0.3f, transform.position.y - direction1.x * 0.3f, transform.position.z);
                Ice1.player = player;
                Ice1.StartPostion = StartPosition1;
                Ice1.direction = direction1;
                Ice1.gameObject.SetActive(true);
                Ice2.player = player;
                Ice2.StartPostion = StartPosition2;
                Ice2.direction = direction2;
                Ice2.gameObject.SetActive(true);
                Ice3.player = player;
                Ice3.StartPostion = StartPosition3;
                Ice3.direction = direction3;
                Ice3.gameObject.SetActive(true);
                Ice4.player = player;
                Ice4.StartPostion = StartPosition4;
                Ice4.direction = direction4;
                Ice4.gameObject.SetActive(true);
                break;
            case 5:
                direction1 = (transform.rotation * Vector2.right).normalized;
                direction2 = Quaternion.AngleAxis(4, Vector3.forward) * direction1;
                direction3 = Quaternion.AngleAxis(-4, Vector3.forward) * direction1;
                direction4 = Quaternion.AngleAxis(6, Vector3.forward) * direction1;
                direction5 = Quaternion.AngleAxis(-6, Vector3.forward) * direction1;
                Ice2.transform.localRotation = Quaternion.AngleAxis(4, Vector3.forward) * Ice2.transform.localRotation;
                Ice3.transform.localRotation = Quaternion.AngleAxis(-4, Vector3.forward) * Ice3.transform.localRotation;
                Ice4.transform.localRotation = Quaternion.AngleAxis(6, Vector3.forward) * Ice4.transform.localRotation;
                Ice4.transform.localRotation = Quaternion.AngleAxis(-6, Vector3.forward) * Ice4.transform.localRotation;

                StartPosition1 = new Vector3(transform.position.x, transform.position.y, transform.position.z);
                StartPosition2 = new Vector3(transform.position.x - direction1.y * 0.1f, transform.position.y + direction1.x * 0.1f, transform.position.z);
                StartPosition3 = new Vector3(transform.position.x + direction1.y * 0.1f, transform.position.y - direction1.x * 0.1f, transform.position.z);
                StartPosition4 = new Vector3(transform.position.x - direction1.y * 0.3f, transform.position.y + direction1.x * 0.3f, transform.position.z);
                StartPosition5 = new Vector3(transform.position.x + direction1.y * 0.3f, transform.position.y - direction1.x * 0.3f, transform.position.z);
                Ice1.player = player;
                Ice1.StartPostion = StartPosition1;
                Ice1.direction = direction1;
                Ice1.gameObject.SetActive(true);
                Ice2.player = player;
                Ice2.StartPostion = StartPosition2;
                Ice2.direction = direction2;
                Ice2.gameObject.SetActive(true);
                Ice3.player = player;
                Ice3.StartPostion = StartPosition3;
                Ice3.direction = direction3;
                Ice3.gameObject.SetActive(true);
                Ice4.player = player;
                Ice4.StartPostion = StartPosition4;
                Ice4.direction = direction4;
                Ice4.gameObject.SetActive(true);
                Ice5.player = player;
                Ice5.StartPostion = StartPosition5;
                Ice5.direction = direction5;
                Ice5.gameObject.SetActive(true);
                break;
        }

        Ice1.SkillFrom = SkillFrom;
        Ice2.SkillFrom = SkillFrom;
        Ice3.SkillFrom = SkillFrom;
        Ice4.SkillFrom = SkillFrom;
        Ice5.SkillFrom = SkillFrom;
    }

    private void Update()
    {
        if (transform.childCount == 5 - Count || transform.childCount == 0)
        {
            Destroy(gameObject);
        }
    }
}
