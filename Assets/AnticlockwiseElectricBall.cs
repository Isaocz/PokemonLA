using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnticlockwiseElectricBall : Projectile
{
    public Transform mew;
    public float rotationSpeed = 20f;

    public GameObject orbPrefab; // Orb��Ԥ����
    public int orbCount = 4; // Orb������
    public float radius = 6f; // ���ɷ�Χ�İ뾶
    public float moveTime = 0.5f; // �ƶ�ʱ��
    private List<GameObject> orbs = new List<GameObject>(); // �洢���ɵ�Orb����

    public void ElectricBallAnticlockwiseEffect()
    {
        // ����Orb�����䱣�浽�б���
        for (int i = 0; i < orbCount; i++)
        {
            GameObject orb = Instantiate(orbPrefab, mew.position, Quaternion.identity);
            orb.transform.parent = transform;
            orbs.Add(orb);
        }
        // ����Э�̣�����Orb���ƶ�
        StartCoroutine(MoveElectricBalls());
    }
    private IEnumerator MoveElectricBalls()
    {
        float angleStep = 360f / orbCount;
        float angle = 0f;
        Vector3[] targetPositions = new Vector3[orbCount];
        for (int i = 0; i < orbCount; i++)
        {
            float radian = angle * Mathf.Deg2Rad;
            targetPositions[i] = mew.position + new Vector3(Mathf.Cos(radian), Mathf.Sin(radian), 0f) * radius + new Vector3(0f, 0.5f, 0f);
            angle += angleStep;
        }
        float timer = 0f;
        while (timer <= moveTime)
        {
            float progress = timer / moveTime;

            for (int i = 0; i < orbCount; i++)
            {
                orbs[i].transform.position = Vector3.Lerp(mew.position + new Vector3(0f, 0.5f, 0f), targetPositions[i], progress);
            }

            timer += Time.deltaTime;

            yield return null;
        }

        for (int i = 0; i < orbCount; i++)
        {
            orbs[i].transform.position = targetPositions[i];
        }
    }
    void Update()
    {
        transform.RotateAround(mew.position + new Vector3(0f, 0.5f, 0f), Vector3.forward, rotationSpeed * Time.deltaTime * -1);
    }
}
