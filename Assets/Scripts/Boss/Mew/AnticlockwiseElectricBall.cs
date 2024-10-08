using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnticlockwiseElectricBall : Projectile
{
    public Transform mew;
    public float rotationSpeed = 20f;

    public GameObject orbPrefab; // Orb的预制体
    public int orbCount = 4; // Orb的数量
    public float radius = 6f; // 生成范围的半径
    public float moveTime = 0.5f; // 移动时间
    private List<GameObject> orbs = new List<GameObject>(); // 存储生成的Orb对象

    public void ElectricBallAnticlockwiseEffect()
    {
        // 生成Orb并将其保存到列表中
        for (int i = 0; i < orbCount; i++)
        {
            GameObject orb = Instantiate(orbPrefab, mew.position, Quaternion.identity);
            orb.transform.parent = transform;
            orbs.Add(orb);
        }
        // 启动协程，控制Orb的移动
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
