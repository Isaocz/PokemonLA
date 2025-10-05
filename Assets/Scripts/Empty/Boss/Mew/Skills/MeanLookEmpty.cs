using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class MeanLookEmpty : Projectile
{
    public float radius;
    private PlayerControler player;
    public GameObject meanlookAni;
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerControler>();
        Destroy(gameObject, 3.5f);
        if (meanlookAni)
        {
            GameObject meanlookani = Instantiate(meanlookAni, player.transform.position + new Vector3(0, 1.5f), quaternion.identity);
            meanlookani.GetComponent<MeanLookEyes>().target = player;
        }
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector2.Distance(player.transform.position, transform.position);
        if (distance > radius)
        {
            // �����Ҿ����ɫĿ��ľ������Բ�뾶��������ƶ��غ�ɫĿ��ķ�Χ��
            Vector3 direction = (player.transform.position - transform.position).normalized;
            player.transform.position = transform.position + direction * radius;
        }
    }


}
