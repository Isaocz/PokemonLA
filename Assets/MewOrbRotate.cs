using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MewOrbRotate : MonoBehaviour
{
    public Transform mew;
    public float rotationSpeed = 50f;

    public GameObject orbPrefab; // Orb的预制体
    public int orbCount = 30; // Orb的数量
    public float radius = 20f; // 生成范围的半径
    public float moveTime = 2f; // 移动时间
    private float currentRadius; // 当前的半径
    private List<GameObject> orbs = new List<GameObject>(); // 存储生成的Orb对象
    private Vector3[] targetPositions; // Orb的目标位置

    public void ActivatePhase3Effect()
    {
        // 生成Orb并将其保存到列表中
        for (int i = 0; i < orbCount; i++)
        {
            GameObject orb = Instantiate(orbPrefab, mew.position , Quaternion.identity);
            orb.transform.parent = transform;
            orbs.Add(orb);
        }
        // 启动协程，控制Orb的移动
        StartCoroutine(MoveOrbs());
    }
    public void SetRadius(float newRadius)
    {
        // 更新半径和目标位置
        currentRadius = newRadius;
        CalculateTargetPositions();
    }

    private void CalculateTargetPositions()
    {
        float angleStep = 360f / orbCount;
        float angle = 0f;
        targetPositions = new Vector3[orbCount];
        for (int i = 0; i < orbCount; i++)
        {
            float radian = angle * Mathf.Deg2Rad;
            targetPositions[i] = mew.position + new Vector3(Mathf.Cos(radian), Mathf.Sin(radian), 0f) * currentRadius + new Vector3(0f, 0.5f, 0f);
            angle += angleStep;
        }
    }
    private IEnumerator MoveOrbs()
    {
        CalculateTargetPositions();
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

    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(mew.position + new Vector3(0f, 0.5f, 0f), Vector3.forward, rotationSpeed * Time.deltaTime);
    }
}
