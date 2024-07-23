using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeanLookIni : Skill
{
    public GameObject meanlook;
    public GameObject meanlookAnimation;
    List<Empty> enemies = new List<Empty>();

    void Update()
    {
        StartExistenceTimer();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Empty"))
        {
            Empty enemy = collision.GetComponent<Empty>();
            if (enemy && !enemies.Contains(enemy) && !collision.GetComponent<SubEmptyBody>())
            {
                enemies.Add(enemy);
                GameObject meanLook = Instantiate(meanlook, enemy.transform.position, Quaternion.identity);
                meanLook.GetComponent<MeanLook>().target = enemy;
                GameObject meanlookani = Instantiate(meanlookAnimation, enemy.transform.position + new Vector3(0, 1.5f), Quaternion.identity);
                meanlookani.GetComponent<MeanLookEyes>().target = enemy;
            }
        }
    }
}
