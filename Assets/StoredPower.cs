using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using Unity.VisualScripting;
using UnityEngine;

public class StoredPower : Skill
{
    int sum;
    public GameObject storedPower;
    public GameObject storedPowerAdvanced;
    // Start is called before the first frame update
    void Start()
    {
        sum = Mathf.Clamp( player.playerData.AtkBounsAlways + player.playerData.AtkBounsJustOneRoom + player.playerData.SpABounsAlways + player.playerData.SpABounsJustOneRoom + player.playerData.DefBounsAlways + player.playerData.DefBounsJustOneRoom + player.playerData.SpDBounsAlways + player.playerData.SpDBounsJustOneRoom + player.playerData.SpeBounsAlways + player.playerData.SpeBounsJustOneRoom + 1 , 1, 100);
        ShootSP();
    }
    private void Update()
    {
        StartExistenceTimer();
    }

    void ShootSP()
    {
        float angleStep = 360f / sum;

        for (int i = 0; i< sum; i++)
        {
            float angle = i * angleStep + transform.rotation.eulerAngles.z - 90;
            Vector3 spawnPos = transform.position + Quaternion.Euler(0f, 0f, angle) * Vector2.up * 1.0f;
            GameObject storedpower;
            if (SkillFrom != 2)
            {
                storedpower = Instantiate(storedPower, spawnPos, transform.rotation,transform);
            }
            else
            {
                storedpower = Instantiate(storedPowerAdvanced, spawnPos, transform.rotation, transform);
            }
        }
    }
}
