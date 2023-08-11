using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirSlashMew : Projectile
{
    public float airSlashSpeed = 5f;
    public int numSplitAirSlashes = 6;
    public GameObject SplitAirSlashPrefab;
    float angle;

    private Transform target; //Ŀ��
    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindWithTag("Player").transform;
        //���㳯��
        Vector3 direction = target.position - transform.position;
        angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle - 90f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.up * airSlashSpeed * Time.deltaTime);
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        //��������ϰ������ң����ٻ���Ƭ
        if (collision.CompareTag("Enviroment") || collision.CompareTag("Room") || collision.CompareTag("Player"))
        {
            if (collision.CompareTag("Player"))
            {
                PlayerControler playerControler = collision.GetComponent<PlayerControler>();
                Pokemon.PokemonHpChange(empty.gameObject, playerControler.gameObject, 0, 120, 0, Type.TypeEnum.Flying);
            }
            SplitAirSlashes(collision.transform.position);
            Destroy(gameObject);
        }
    }
    void SplitAirSlashes(Vector3 collisionPosition)
    {
        for (int i = 0; i < numSplitAirSlashes; i++)
        {
            //����Ƕ�
            float splitangle =angle + i * (360f / numSplitAirSlashes);
            //�ٻ���Ƭ
            GameObject splitAirSlash = Instantiate(SplitAirSlashPrefab, transform.position, Quaternion.Euler(0f, 0f, splitangle));
        }
    }
}
