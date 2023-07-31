using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlusSubRockBlast : Skill
{
    int Count;

    RockBlastRock Rock1;
    RockBlastRock Rock2;
    RockBlastRock Rock3;

    Vector2 direction1;
    Vector2 direction2;
    Vector2 direction3;

    Vector3 StartPosition1;
    Vector3 StartPosition2;
    Vector3 StartPosition3;

    // Start is called before the first frame update
    void Start()
    {
        Rock1 = transform.GetChild(0).GetComponent<RockBlastRock>();
        Rock2 = transform.GetChild(1).GetComponent<RockBlastRock>();
        Rock3 = transform.GetChild(2).GetComponent<RockBlastRock>();


        Count = Random.Range(1, 4);


        switch (Count)
        {
            case 1:
                direction1 = (transform.rotation * Vector2.right).normalized;
                StartPosition1 = transform.position;
                Rock1.player = player;
                Rock1.StartPostion = StartPosition1;
                Rock1.direction = direction1;
                Rock1.gameObject.SetActive(true);
                break;
            case 2:
                direction1 = (transform.rotation * Vector2.right).normalized;
                direction2 = (transform.rotation * Vector2.right).normalized;
                StartPosition1 = new Vector3(transform.position.x - direction1.y * 0.2f, transform.position.y + direction1.x * 0.2f, transform.position.z);
                StartPosition2 = new Vector3(transform.position.x + direction1.y * 0.2f, transform.position.y - direction1.x * 0.2f, transform.position.z);
                Rock1.player = player;
                Rock1.StartPostion = StartPosition1;
                Rock1.direction = direction1;
                Rock1.gameObject.SetActive(true);
                Rock2.player = player;
                Rock2.StartPostion = StartPosition2;
                Rock2.direction = direction2;
                Rock2.gameObject.SetActive(true);
                break;
            case 3:
                direction1 = (transform.rotation * Vector2.right).normalized;
                direction2 = Quaternion.AngleAxis(5, Vector3.forward) * direction1;
                direction3 = Quaternion.AngleAxis(-5, Vector3.forward) * direction1;
                StartPosition1 = new Vector3(transform.position.x, transform.position.y, transform.position.z);
                StartPosition2 = new Vector3(transform.position.x - direction1.y * 0.18f, transform.position.y + direction1.x * 0.18f, transform.position.z);
                StartPosition3 = new Vector3(transform.position.x + direction1.y * 0.18f, transform.position.y - direction1.x * 0.18f, transform.position.z);
                Rock1.player = player;
                Rock1.StartPostion = StartPosition1;
                Rock1.direction = direction1;
                Rock1.gameObject.SetActive(true);
                Rock2.player = player;
                Rock2.StartPostion = StartPosition2;
                Rock2.direction = direction2;
                Rock2.gameObject.SetActive(true);
                Rock3.player = player;
                Rock3.StartPostion = StartPosition3;
                Rock3.direction = direction3;
                Rock3.gameObject.SetActive(true);
                break;
        }

        Rock1.SkillFrom = SkillFrom;
        Rock2.SkillFrom = SkillFrom;
        Rock3.SkillFrom = SkillFrom;
    }

    private void Update()
    {
        if (transform.childCount == 3 - Count || transform.childCount==0)
        {
            Destroy(gameObject);
        }
    }

}
