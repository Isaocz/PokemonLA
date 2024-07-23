using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KonamiCode : MonoBehaviour
{
    private KeyCode[] triggerKeys = { KeyCode.Q, KeyCode.W, KeyCode.E, KeyCode.R };
    private bool[] keyPressStatus = new bool[4];
    private bool IsTrigger = false;
    public bool isWillFly;
    public float triggerRadius = 5f;
    public GameObject MewBossPrefab;
    public GameObject StartEffect;
    public GameObject Barrier;

    private void Start()
    {
        isWillFly = false;
    }
    void Update()
    {
        if (WithinTriggerRadius() && !AllKeysPressed() && !isWillFly)
        {
            if (Input.GetKeyDown(InitializePlayerSetting.GlobalPlayerSetting.GetKeybind("Skill1")))
            {
                keyPressStatus[0] = true;
                Debug.Log("����1�ͷ�");
            }
            if (Input.GetKeyDown(InitializePlayerSetting.GlobalPlayerSetting.GetKeybind("Skill2")))
            {
                keyPressStatus[1] = true;
                Debug.Log("����2�ͷ�");
            }
            if (Input.GetKeyDown(InitializePlayerSetting.GlobalPlayerSetting.GetKeybind("Skill3")))
            {
                keyPressStatus[2] = true;
                Debug.Log("����3�ͷ�");
            }
            if (Input.GetKeyDown(InitializePlayerSetting.GlobalPlayerSetting.GetKeybind("Skill4")))
            {
                keyPressStatus[3] = true;
                Debug.Log("����4�ͷ�");
            }
        }
        if (AllKeysPressed() && IsTrigger == false)
        {
            IsTrigger = true;
            TriggerEffect();
        }
    }

    bool WithinTriggerRadius()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, triggerRadius);
        foreach (Collider2D collider in colliders)
        {
            PlayerControler player = collider.GetComponent<PlayerControler>();
            if (player != null)
            {
                //Debug.Log("�������");
                return true;
            }
        }
        return false;
    }

    bool AllKeysPressed()
    {
        foreach (bool status in keyPressStatus)
        {
            if (!status)
            {
                return false;
            }
        }
        return true;
    }

    void TriggerEffect()
    {
        GetComponent<MewNPC>().isTrigger = true;
        Instantiate(StartEffect, transform.position, Quaternion.identity);
        Timer.Start(this, 5f, () =>
        {
            GameObject MewBoss = Instantiate(MewBossPrefab, transform.position, Quaternion.identity, transform.parent);
            Destroy(gameObject); // ���ٵ�ǰ��MewNPC����
        });

        //��������
        Vector3[] barrierPositions = new Vector3[84];
        Vector3 roomPosition = transform.parent.position;
        for(int i = 0; i < 27; i++)
        {
            float t = i - 13f;
            barrierPositions[i] = roomPosition + new Vector3(t, 7.5f, 0f);
            barrierPositions[i + 27] = roomPosition + new Vector3(t, -8.5f, 0f);
        }
        for(int i = 54; i < 69; i++)
        {
            float t = i - 61.5f;
            barrierPositions[i] = roomPosition + new Vector3(-13f, t, 0f);
            barrierPositions[i + 15] = roomPosition + new Vector3(13f, t, 0f);
        }
        for (int i = 0; i < barrierPositions.Length; i++)
        {

            GameObject barriers = Instantiate(Barrier, transform.parent.position + barrierPositions[i], Quaternion.identity);
        }
    }
}

