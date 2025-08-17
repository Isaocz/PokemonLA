using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class objPoolTestObj : MonoBehaviour
{
    public float moveSpeed;
    public float dyingTime;
    public bool OBM;     //��֤�����Ƿ���ԴObjectPool
    private Vector3 randomDirection;
    private Coroutine timerCoroutine;
    private void OnEnable()
    {
        do
        {
            randomDirection = new Vector3(
                Random.Range(-1f, 1f),
                Random.Range(-1f, 1f)
            );
        } while (randomDirection.sqrMagnitude < 0.001f);
        randomDirection.Normalize();

        timerCoroutine = StartCoroutine(TimerCoroutine());
    }
    private void Update()
    {
        transform.Translate(randomDirection * moveSpeed * Time.deltaTime);
    }
    private IEnumerator TimerCoroutine()
    {
        yield return new WaitForSeconds(dyingTime);
        if (OBM)
        {
            ObjectPoolManager.ReturnObjectToPool(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnDisable()
    {
        // ȡ����ʱ����ֹ�ص��ظ�ִ��
        if (timerCoroutine != null)
        {
            StopCoroutine(timerCoroutine);
            timerCoroutine = null;
        }
    }
}
