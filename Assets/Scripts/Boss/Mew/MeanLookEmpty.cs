using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeanLookEmpty : Projectile
{
    public List<Transform> flamesList; // �������Ӷ�����б�
    public float rotationSpeed; // ��ת�ٶ�
    public float radius;
    private PlayerControler player;
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerControler>();
        Destroy(gameObject, 3.5f);
    }

    // Update is called once per frame
    void Update()
    {
        float rotationAngle = rotationSpeed * Time.deltaTime;
        float distance = Vector2.Distance(player.transform.position, transform.position);
        if (distance > radius)
        {
            // �����Ҿ����ɫĿ��ľ������Բ�뾶��������ƶ��غ�ɫĿ��ķ�Χ��
            Vector3 direction = (player.transform.position - transform.position).normalized;
            player.transform.position = transform.position + direction * radius;
        }

        // �����������Ӷ����б���ÿ�����������ת
        foreach (Transform flame in flamesList)
        {
            flame.RotateAround(transform.position, transform.forward, rotationAngle);
        }
    }


}
