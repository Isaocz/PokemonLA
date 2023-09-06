using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KonamiCode : MonoBehaviour
{
    private KeyCode[] konamiCode = { KeyCode.UpArrow, KeyCode.UpArrow, KeyCode.DownArrow, KeyCode.DownArrow, KeyCode.LeftArrow, KeyCode.RightArrow, KeyCode.LeftArrow, KeyCode.RightArrow, KeyCode.B, KeyCode.A, KeyCode.B, KeyCode.A };
    private int currentIndex = 0;
    public GameObject MewBossPrefab;

    void Update()
    {
        if (Input.anyKeyDown)
        {
            if (Input.GetKeyDown(konamiCode[currentIndex]))
            {
                currentIndex++;
                if (currentIndex >= konamiCode.Length)
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
        GameObject MewBoss = Instantiate(MewBossPrefab, transform.position, Quaternion.identity, transform.parent);
        Destroy(gameObject); // 销毁当前的MewNPC对象
    }
}

