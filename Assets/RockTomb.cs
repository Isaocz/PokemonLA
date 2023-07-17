using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockTomb : Skill
{
    GameObject Rock2;
    GameObject Rock3;
    GameObject Rock4;
    GameObject Rock5;

    // Start is called before the first frame update
    void Start()
    {
        Rock2 = transform.GetChild(1).gameObject;
        Rock3 = transform.GetChild(2).gameObject;
        Rock4 = transform.GetChild(3).gameObject;
        Rock5 = transform.GetChild(4).gameObject;
        Vector2 d = (transform.rotation * Vector2.right).normalized;

        transform.rotation = Quaternion.Euler(Vector3.zero);
        Vector2Int direction = new Vector2Int((int)d.x, (int)d.y);
        if(direction == Vector2Int.up)
        {
            Rock2.transform.position += Vector3.up + Vector3.left;
            Rock3.transform.position += Vector3.up + Vector3.right;
            Rock4.transform.position += 2*Vector3.up + 2*Vector3.left;
            Rock5.transform.position += 2*Vector3.up + 2*Vector3.right;
        }
        else if (direction == Vector2Int.down)
        {
            Rock2.transform.position += Vector3.down + Vector3.left;
            Rock3.transform.position += Vector3.down + Vector3.right;
            Rock4.transform.position += 2*Vector3.down + 2 * Vector3.left;
            Rock5.transform.position += 2 * Vector3.down + 2 * Vector3.right;
        }
        else if (direction == Vector2Int.left)
        {
            Rock2.transform.position += Vector3.left + Vector3.up;
            Rock3.transform.position += Vector3.left + Vector3.down;
            Rock4.transform.position += 2 * Vector3.left + 2 * Vector3.up;
            Rock5.transform.position += 2 * Vector3.left + 2 * Vector3.down;
        }
        else if (direction == Vector2Int.right)
        {
            Rock2.transform.position += Vector3.right + Vector3.up;
            Rock3.transform.position += Vector3.right + Vector3.down;
            Rock4.transform.position += 2 * Vector3.right + 2 * Vector3.up;
            Rock5.transform.position += 2 * Vector3.right + 2 * Vector3.down;
        }

        if (SkillFrom == 2)
        {
            Rock4.SetActive(true);
            Rock5.SetActive(true);
        }

    }

    // Update is called once per frame
    void Update()
    {
        StartExistenceTimer();
    }
}
