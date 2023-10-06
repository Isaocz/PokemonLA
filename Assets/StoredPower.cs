using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using Unity.VisualScripting;
using UnityEngine;

public class StoredPower : Skill
{
    int sum;
    public float skillRadius;
    public float summonRadius;
    public GameObject storedPower;
    public GameObject storedPowerAdvanced;

    Collider2D[] colliders;
    List<Empty> emptyList;
    // Start is called before the first frame update
    void Start()
    {
        sum = player.playerData.AtkBounsAlways + player.playerData.AtkBounsJustOneRoom + player.playerData.SpABounsAlways + player.playerData.SpABounsJustOneRoom + player.playerData.DefBounsAlways + player.playerData.DefBounsJustOneRoom + player.playerData.SpDBounsAlways + player.playerData.SpDBounsJustOneRoom + player.playerData.SpeBounsAlways + player.playerData.SpeBounsJustOneRoom + 1;
        colliders = Physics2D.OverlapCircleAll(transform.position, skillRadius);
        emptyList = new List<Empty>();
        foreach (Collider2D collider in colliders)//将在范围内的敌怪放进List
        {
            if (collider.CompareTag("Empty"))
            {
                Empty empty = collider.GetComponent<Empty>();
                if (empty != null)
                {
                    emptyList.Add(empty);
                }
            }
        }
        StartCoroutine(ShootSP());
    }
    private void Update()
    {
        StartExistenceTimer();
    }

    IEnumerator ShootSP()
    {
        float angleStep = 360f / sum;

        for (int i = 0; i< sum; i++)
        {
            float angle = i * angleStep;
            Vector3 spawnPos = transform.position + Quaternion.Euler(0f, 0f, angle) * Vector2.up * summonRadius;
            GameObject storedpower;
            if (SkillFrom == 0)
            {
                storedpower = Instantiate(storedPower, spawnPos, Quaternion.identity);
            }
            else
            {
                storedpower = Instantiate(storedPowerAdvanced, spawnPos, Quaternion.identity);
            }
            if (emptyList.Count > 0)
            {//对List内的敌怪进行目标随机选定，瞄准并造成伤害
                int randomIndex = Random.Range(0, emptyList.Count);
                Empty randomEnemy = emptyList[randomIndex];
                storedpower.GetComponent<StoredPowerEffect>().target = randomEnemy;
                storedpower.GetComponent<StoredPowerEffect>().player = player;
            }
            else
            {
                Destroy(storedpower);
            }
            yield return new WaitForSeconds(0.03f);
        }
    }
}
