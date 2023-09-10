using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class MagicalLeafEmpty : Projectile
{
    public float speed = 4.5f; // ħ��Ҷ�ӵ��ٶ�
    public float lifetime = 6f; // ħ��Ҷ�Ӵ��ڵ�ʱ��

    private float timer; // ��ʱ��

    private Transform target; // �����Ŀ��

    public void SetTarget(GameObject Target)
    {
        target = Target.transform;
    }
    private void Start()
    {
        timer = lifetime; // ��ʼ����ʱ��
    }

    private void Update()
    {
        if (target != null)
        {
            // ����Ŀ���λ�ó���Ŀ���ƶ�
            transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
            // ���³���
            Vector3 direction = target.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }

        timer -= Time.deltaTime; // ���¼�ʱ��

        if (timer <= 0f)
        {
            Destroy(gameObject); // �ڼ�ʱ����������ħ��Ҷ�Ӷ���
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == ("Player"))
        {
            // ���������˺�
            PlayerControler playerControler = collision.GetComponent<PlayerControler>();
            Pokemon.PokemonHpChange(empty.gameObject, collision.gameObject, 0, SpDmage, 0, Type.TypeEnum.Grass);
            if (playerControler != null)
            {
                //playerControler.ChangeHp(0, -(SpDmage * empty.SpAAbilityPoint * WeatherAlpha * (2 * empty.Emptylevel + 10) / 250), 11);
                playerControler.KnockOutPoint = 2.5f;
                playerControler.KnockOutDirection = (playerControler.transform.position - transform.position).normalized;
            }
            Destroy(gameObject); // ����ײ������ħ��Ҷ�Ӷ���
        }
    }
}
