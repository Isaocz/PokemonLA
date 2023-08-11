using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ScaleShotEmpty : Projectile
{
    private float initialMoveSpeed = 5f; // ��ʼ�ƶ��ٶ�
    private float slowMoveSpeed = 1f; // ��������ƶ��ٶ�
    private float fastMoveSpeed = 7f; // �ٴμ��ٺ���ƶ��ٶ�

    private float timer; // ��ʱ��
    private bool isSpeedIncreased; // �Ƿ��Ѿ����ٹ�

    // Start is called before the first frame update
    void Start()
    {
        timer = 0f;
        isSpeedIncreased = false;
        Destroy(gameObject, 10f);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isSpeedIncreased)
        {
            // �ȿ�
            transform.Translate(Vector3.up * initialMoveSpeed * Time.deltaTime);
        }
        else
        {
            // ��
            transform.Translate(Vector3.up * slowMoveSpeed * Time.deltaTime);
        }

        timer += Time.deltaTime;

        if (timer >= 1f && !isSpeedIncreased)
        {
            // ��2������
            isSpeedIncreased = true;
            timer = 1f;
        }

        if (timer >= 2f && isSpeedIncreased)
        {
            // �ٹ�2����
            slowMoveSpeed = fastMoveSpeed;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerControler playerControler = collision.GetComponent<PlayerControler>();
            Pokemon.PokemonHpChange(empty.gameObject, playerControler.gameObject, 0, 80, 0, Type.TypeEnum.Dragon);
        }
    }
}
