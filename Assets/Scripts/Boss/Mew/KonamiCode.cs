using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KonamiCode : MonoBehaviour
{
    private KeyCode[] konamiCode = { KeyCode.UpArrow, KeyCode.UpArrow, KeyCode.DownArrow, KeyCode.DownArrow, KeyCode.LeftArrow, KeyCode.RightArrow, KeyCode.LeftArrow, KeyCode.RightArrow, KeyCode.B, KeyCode.A, KeyCode.B, KeyCode.A };
    private int currentIndex = 0;
    public GameObject MewBossPrefab;
    public GameObject StartEffect;

    void Update()
    {
        if (Input.anyKeyDown)
        {
            if (Input.GetKeyDown(konamiCode[currentIndex]))
            {
                currentIndex++;
                if (currentIndex >= konamiCode.Length && Mew.MewBossKilled == false)
                {
                    // ����Ч��
                    TriggerEffect();
                    currentIndex = 0; // ��������
                }
            }
            else
            {
                currentIndex = 0; // ��������
            }
        }
    }

    void TriggerEffect()
    {
        // �������д����Ч���Ĵ���
        Debug.Log("Konami Code triggered!");
        Instantiate(StartEffect, transform.position, Quaternion.identity);
        Timer.Start(this, 5f, () =>
        {
            GameObject MewBoss = Instantiate(MewBossPrefab, transform.position, Quaternion.identity, transform.parent);
            Destroy(gameObject); // ���ٵ�ǰ��MewNPC����
        });
    }
}

