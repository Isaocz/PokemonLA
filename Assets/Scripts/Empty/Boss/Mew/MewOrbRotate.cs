using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MewOrbRotate : MonoBehaviour
{
    public Transform mew;
    public float rotationSpeed = 50f;

    public GameObject orbPrefab; // Orb��Ԥ����
    public float moveTime = 2f; // �ƶ�ʱ��
    private List<GameObject> orbs = new List<GameObject>(); // �洢���ɵ�Orb����
    private List<GameObject> Secondorbs = new List<GameObject>(); // �洢���ɵ�Orb����

    public IEnumerator ActivatePhase3Effect(int stage ,int counts, float radius)
    {
        // ����Orb�����䱣�浽�б���
        for (int i = 0; i < counts; i++)
        {
            GameObject orb = Instantiate(orbPrefab, mew.position, Quaternion.identity);
            orb.transform.parent = transform;
            if (stage == 1) orbs.Add(orb);
            else Secondorbs.Add(orb);
        }
        float angleStep = 360f / counts;
        float angle = 0f;
        Vector3[] targetPositions = new Vector3[counts];
        for (int i = 0; i < counts; i++)
        {
            float radian = angle * Mathf.Deg2Rad;
            targetPositions[i] = mew.position + new Vector3(Mathf.Cos(radian), Mathf.Sin(radian), 0f) * radius + new Vector3(0f, 0.5f, 0f);
            angle += angleStep;
        }
        float timer = 0f;
        while (timer <= moveTime)
        {
            float progress = timer / moveTime;

            for (int i = 0; i < counts; i++)
            {
                if (stage == 1) orbs[i].transform.position = Vector3.Lerp(mew.position + new Vector3(0f, 0.5f, 0f), targetPositions[i], progress);
                else Secondorbs[i].transform.position = Vector3.Lerp(mew.position + new Vector3(0f, 0.5f, 0f), targetPositions[i], progress);
            }

            timer += Time.deltaTime;

            yield return null;
        }

        for (int i = 0; i < counts; i++)
        {
            if (stage == 1) orbs[i].transform.position = targetPositions[i];
            else Secondorbs[i].transform.position = targetPositions[i];
        }
    }

    void Update()
    {
        transform.RotateAround(mew.position + new Vector3(0f, 0.5f, 0f), Vector3.forward, rotationSpeed * Time.deltaTime);
    }
}
