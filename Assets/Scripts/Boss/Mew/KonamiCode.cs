using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KonamiCode : MonoBehaviour
{
    private KeyCode[] konamiCode = { KeyCode.UpArrow, KeyCode.UpArrow, KeyCode.DownArrow, KeyCode.DownArrow, KeyCode.LeftArrow, KeyCode.RightArrow, KeyCode.LeftArrow, KeyCode.RightArrow, KeyCode.B, KeyCode.A, KeyCode.B, KeyCode.A };
    private int currentIndex = 0;
    public GameObject MewBossPrefab;
    public GameObject MewRoom;

    void Update()
    {
        if (Input.anyKeyDown)
        {
            if (Input.GetKeyDown(konamiCode[currentIndex]))
            {
                currentIndex++;
                if (currentIndex >= konamiCode.Length)
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
        GameObject mewPrefab = Resources.Load<GameObject>("Mew"); // ����MewԤ�Ƽ�
        if (mewPrefab != null)
        {
            GameObject MewBoss = Instantiate(mewPrefab, transform.position, Quaternion.identity);
            MewRoom.transform.Find("Empty").gameObject.SetActive(true);
            MewBoss.transform.SetParent(MewRoom.transform.Find("Empty"));
            Destroy(gameObject); // ���ٵ�ǰ��MewNPC����
        }
        else
        {
            Debug.LogError("Failed to load Mew!");
        }
    }
}

