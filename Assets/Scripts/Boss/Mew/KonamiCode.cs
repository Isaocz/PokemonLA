using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KonamiCode : MonoBehaviour
{
    private KeyCode[] konamiCode = { KeyCode.UpArrow, KeyCode.UpArrow, KeyCode.DownArrow, KeyCode.DownArrow, KeyCode.LeftArrow, KeyCode.RightArrow, KeyCode.LeftArrow, KeyCode.RightArrow, KeyCode.B, KeyCode.A, KeyCode.B, KeyCode.A };
    private int currentIndex = 0;
    public GameObject MewBossPrefab;
    public GameObject StartEffect;
    public GameObject Barrier;

    void Update()
    {
        if (Input.anyKeyDown)
        {
            if (Input.GetKeyDown(konamiCode[currentIndex]))
            {
                currentIndex++;
                if (currentIndex >= konamiCode.Length && Mew.MewBossKilled == false)
                {
                    // 触发效果
                    TriggerEffect();
                    currentIndex = 0; // 重置索引
                }
            }
            else
            {
                currentIndex = 0; // 重置索引
            }
        }
    }

    void TriggerEffect()
    {
        // 在这里编写触发效果的代码
        Debug.Log("Konami Code triggered!");
        Instantiate(StartEffect, transform.position, Quaternion.identity);
        Timer.Start(this, 5f, () =>
        {
            GameObject MewBoss = Instantiate(MewBossPrefab, transform.position, Quaternion.identity, transform.parent);
            Destroy(gameObject); // 销毁当前的MewNPC对象
        });

        //创建屏障
        Vector3[] barrierPositions = new Vector3[]
        {
            new Vector3(-2f, 7.5f, 0f),
            new Vector3(-1f, 7.5f, 0f),
            new Vector3(0f, 7.5f, 0f),
            new Vector3(1f, 7.5f, 0f),
            new Vector3(2f, 7.5f, 0f),
            new Vector3(-2f, -8.5f, 0f),
            new Vector3(-1f, -8.5f, 0f),
            new Vector3(0f, -8.5f, 0f),
            new Vector3(1f, -8.5f, 0f),
            new Vector3(2f, -8.5f, 0f),
            new Vector3(13f, 0.5f, 0f),
            new Vector3(13f, -0.5f, 0f),
            new Vector3(13f, -1.5f, 0f),
            new Vector3(13f, -2.5f, 0f),
            new Vector3(-13f, 0.5f, 0f),
            new Vector3(-13f, -0.5f, 0f),
            new Vector3(-13f, -1.5f, 0f),
            new Vector3(-13f, -2.5f, 0f)
        };
        for (int i = 0; i < barrierPositions.Length; i++)
        {

            GameObject barriers = Instantiate(Barrier, transform.parent.position + barrierPositions[i], Quaternion.identity);
        }
    }
}

